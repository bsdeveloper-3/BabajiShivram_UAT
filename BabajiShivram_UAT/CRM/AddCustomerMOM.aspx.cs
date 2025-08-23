using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
using Ionic.Zip;
using System.Text.RegularExpressions;

public partial class CRM_AddCustomerMOM : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnAddAttendee);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Add Customer MOM";
            SetInitialRow_Agenda();
            txtMeetingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable dtAttendee = new DataTable();
            dtAttendee.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("UserId"), new DataColumn("UserName"), new DataColumn("EmailId") });
            ViewState["Attendee"] = dtAttendee;
        }
    }

    protected void btnSaveMOM_Click(object sender, EventArgs e)
    {
        //btnSendMail.Enabled = false;
        AddNewRow_Agenda();
        if (txtMeetingTitle.Text.Trim() != "")
        {
            DateTime MeetingDate = DateTime.MinValue, StartTime = DateTime.MinValue, EndTime = DateTime.MinValue;
            if (txtMeetingDate.Text.Trim() != "")
                MeetingDate = Commonfunctions.CDateTime(txtMeetingDate.Text.Trim());

            if (txtStartTime.Text.Trim() != "")
                StartTime = Convert.ToDateTime(txtStartTime.Text.Trim());

            if (txtEndTime.Text.Trim() != "")
                EndTime = Convert.ToDateTime(txtEndTime.Text.Trim());

            if (gvExistingAttendee == null || gvExistingAttendee.Rows.Count == 0)
            {
                lblError.Text = "Please enter atleast one attendee.";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (gvAgenda == null || gvAgenda.Rows.Count == 0)
            {
                lblError.Text = "Please enter atleast one agenda.";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (ddlCustomer.SelectedValue != "0")
            {
                int SaveMeeting = DBOperations.CRM_AddOpportunity_MOM(0, txtMeetingTitle.Text.Trim(), MeetingDate, StartTime, EndTime,
                                txtObservers.Text.Trim(), txtResources.Text.Trim(), txtSpecialNotes.Text.Trim(), LoggedInUser.glUserId,
                                true, Convert.ToInt32(ddlCustomer.SelectedValue));
                if (SaveMeeting != -123)
                {
                    //Add up attendee for MOM
                    if (gvExistingAttendee != null)
                    {
                        for (int i = 0; i < gvExistingAttendee.Rows.Count; i++)
                        {
                            int UserId = 0;
                            Label lblUserId = (Label)gvExistingAttendee.Rows[i].FindControl("lblUserId");
                            Label lblUserName = (Label)gvExistingAttendee.Rows[i].FindControl("lblUserName");
                            Label lblEmailId = (Label)gvExistingAttendee.Rows[i].FindControl("lblEmailId");

                            if (lblUserId.Text.Trim() != "" && lblUserId.Text.Trim() != "0")
                                UserId = Convert.ToInt32(lblUserId.Text.Trim());
                            if (lblEmailId.Text != "")
                            {
                                int SaveAttendee = DBOperations.CRM_AddMOM_Attendee(SaveMeeting, lblUserName.Text.Trim(), lblEmailId.Text.Trim(), UserId, LoggedInUser.glUserId);
                            }
                        }
                    }

                    // Add Agenda Info
                    if (gvAgenda != null && gvAgenda.Rows.Count > 0)
                    {
                        for (int i = 0; i < gvAgenda.Rows.Count; i++)
                        {
                            TextBox txtTopic = (TextBox)gvAgenda.Rows[i].FindControl("txtTopic");
                            TextBox txtDescription = (TextBox)gvAgenda.Rows[i].FindControl("txtDescription");
                            TextBox txtPersonName = (TextBox)gvAgenda.Rows[i].FindControl("txtPersonName");

                            if (txtTopic.Text != "")
                            {
                                int SaveAgenda = DBOperations.CRM_AddMOM_Agenda(SaveMeeting, txtTopic.Text.Trim(), txtDescription.Text.Trim(), txtPersonName.Text.Trim(), LoggedInUser.glUserId);
                            }
                        }
                    }

                    lblError.Text = "Successfully added Minutes of Meeting.";
                    lblError.CssClass = "success";
                    hdnMomId_Popup.Value = SaveMeeting.ToString();
                    ShowPopup(SaveMeeting);
                    //string str_Message = "<script type='text/javascript' language='javascript'>" +
                    //      "_toastr('Successfully added Minutes of Meeting.','top-full-width','success',false); </script>";
                    //ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                    //return;
                }
                else
                {
                    lblError.Text = "Error while sending mail. Please try gaian later.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please select customer to send MOM for.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Enter Meeting title.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnSendMail_OnClick(object sender, EventArgs e)
    {
        if (hdnMomId_Popup.Value != "" && hdnMomId_Popup.Value != "0")
        {
            bool bSuccess = SendEmail(Convert.ToInt32(hdnMomId_Popup.Value));
            if (bSuccess == true)
            {
                lblError.Text = "MOM successfully been added! Thank You.";
                lblError.CssClass = "success";
                txtMeetingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtMeetingTitle.Text = "";
                //txtNewAttendeeEmail.Text = "";
                //txtNewAttendeeName.Text = "";

            }
            else
            {
                lblError.Text = "Error while sending mail. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancelMOM_Click(object sender, EventArgs e)
    {
        txtMeetingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtMeetingTitle.Text = "";
        txtStartTime.Text = "";
        txtEndTime.Text = "";
        txtResources.Text = "";
        txtObservers.Text = "";
        txtSpecialNotes.Text = "";
        ViewState["Agenda"] = null;
        ViewState["Attendee"] = null;
        gvAgenda.DataBind();
        gvExistingAttendee.DataBind();
        SetInitialRow_Agenda();
    }

    protected void btnCancelPopup_OnClick(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }

    protected void ShowPopup(int MomId)
    {
        //bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilder_Attendee = new StringBuilder();
        StringBuilder strAttendeeEmail = new StringBuilder();

        if (MomId > 0)
        {
            string strMeetingTitle = "", strClient = "", strMeetingDate = "", strStartTime = "", strEndTime = "", strCustomerEmail = "",
                strCCEmail = "",  MessageBody = "", strObservers = "", strResources = "", strNotes = "", strMomCreatedBy = "", strMomCreatedByMail = ""; //strBCCEmail = "", strSubject = "",

            //Get Basic Detail - Title, date, time
            DataSet dsGetMeetingDetail = DBOperations.CRM_GetOpportunity_MOM_Lid(MomId);
            if (dsGetMeetingDetail != null && dsGetMeetingDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetMeetingDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    strMeetingTitle = dsGetMeetingDetail.Tables[0].Rows[0]["Title"].ToString();
                    strMeetingDate = Convert.ToDateTime(dsGetMeetingDetail.Tables[0].Rows[0]["Date"]).ToString("MMMM dd, yyyy");
                    if (dsGetMeetingDetail.Tables[0].Rows[0]["StartTime"] != DBNull.Value)
                        strStartTime = Convert.ToDateTime(dsGetMeetingDetail.Tables[0].Rows[0]["StartTime"]).ToString("HH: mm tt");
                    if (dsGetMeetingDetail.Tables[0].Rows[0]["EndTime"] != DBNull.Value)
                        strEndTime = Convert.ToDateTime(dsGetMeetingDetail.Tables[0].Rows[0]["EndTime"]).ToString("HH: mm tt");
                    strClient = dsGetMeetingDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    strObservers = dsGetMeetingDetail.Tables[0].Rows[0]["Observers"].ToString();
                    strResources = dsGetMeetingDetail.Tables[0].Rows[0]["Resources"].ToString();
                    strNotes = dsGetMeetingDetail.Tables[0].Rows[0]["SpecialNotes"].ToString();
                    strMomCreatedBy = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedBy"].ToString();
                    strMomCreatedByMail = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedByMail"].ToString();
                    // strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactName"].ToString());  ///COMMENT BY SAYALI ON 13-11-2019
                    // strAttendeeEmail = strAttendeeEmail.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactEmail"].ToString());  ///COMMENT BY SAYALI ON 13-11-2019
                }

                // Get Attendees Name
                DataSet dsGetMom_Attendee = DBOperations.CRM_GetMom_Attendee(MomId);
                if (dsGetMom_Attendee != null && dsGetMom_Attendee.Tables[0].Rows.Count > 0)
                {
                    if (dsGetMom_Attendee.Tables[0].Rows.Count > 1)
                    {
                        strbuilder_Attendee = strbuilder_Attendee.Append(" , ");
                        strAttendeeEmail = strAttendeeEmail.Append(" , ");
                        for (int a = 0; a < dsGetMom_Attendee.Tables[0].Rows.Count; a++)
                        {
                            if (a != dsGetMom_Attendee.Tables[0].Rows.Count - 1)
                            {
                                strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserName"].ToString() + " , ");
                                strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserEmail"].ToString() + " , ");
                            }
                            else
                            {
                                strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserName"].ToString());
                                strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserEmail"].ToString());
                            }
                        }
                    }
                    else
                    {
                        //-----------------------------ADDED BY SAYALI ON 13-11-2019------------------------
                        if (strAttendeeEmail.ToString() != "" && strAttendeeEmail.ToString() != null)
                        {
                            strAttendeeEmail = strAttendeeEmail.Append(" , ");
                        }
                        if (strbuilder_Attendee.ToString() != "" && strbuilder_Attendee.ToString() != null)
                        {
                            strbuilder_Attendee = strbuilder_Attendee.Append(" , ");
                        }
                        //------------------------------------------------------------------------------------
                        strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[0]["UserName"].ToString());
                        strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[0]["UserEmail"].ToString());
                    }
                }
            }

            // Email Format
            strCustomerEmail = lblCustomerEmail.Text.Trim(); // strAttendeeEmail.ToString();  //"jr.developer@babajishivram.com";// strAttendeeEmail.ToString();  
            strCCEmail = txtMailCC.Text.Trim(); // "dhaval@babajishivram.com , kirti@babajishivram.com";
            try
            {
                StringBuilder strStyle = new StringBuilder();
                strStyle = strStyle.Append("<html><body class='setupTab' style='font-family:Calibri; font-style:normal; bEditID: b1st1; bLabel: body;'><center>");

                // email css
                strStyle = strStyle.Append(@"<style type='text/css'> p {margin-top: 0px; margin-bottom: 0px;} font {color: black;}");
                strStyle = strStyle.Append(@".topTr {font-family:Calibri; font-style:normal;border: 2px solid darkgray;border-bottom: none;bEditID: r3st1;color: white;bLabel: main;font-size: 12pt;font-family: calibri;height: 35px;background-color: #268CD8;}");
                strStyle = strStyle.Append(@".MeetingTitle {font-family:Calibri; font-style:normal;font-size: 19px;text-align: right;font-weight: 600;color: #268CD8;} .MeetingDate {font-size: 13px;text-align: right;color: black;font-weight: 400;} ");
                strStyle = strStyle.Append(@".MOMHdr {font-family:Calibri; font-style:normal; color:black; padding:4px;} .csTitle {font-family:Calibri; font-style:normal;color: #268CD8;} .AgendaHdr {padding: 7px;background-color: #268CD8;color: white;} ");
                strStyle = strStyle.Append(@".AgendaHdr {font-family:Calibri; font-style:normal;padding: 7px;background - color: #268CD8;color: white;font - weight: 500;}");
                strStyle = strStyle.Append(@".AgendaBody {font-family:Calibri; font-style:normal;padding-left: 7px;padding-right: 7px;color: black;font - weight: 400;}");
                strStyle = strStyle.Append(@".AgendaBodySL {font-family:Calibri; font-style:normal;padding-left: 7px;padding-right: 7px;color: black;font - weight: 400;text - align: center;}");
                strStyle = strStyle.Append(@"</style>");

                // body header
                strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
                strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
                strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");
                strStyle = strStyle.Append(@"<tr><td class='MeetingTitle'>" + strMeetingTitle + "<br /><span class='MeetingDate'>" + strMeetingDate + " at " + strStartTime);
                strStyle = strStyle.Append(@"</span></td></tr><tr><td styleInsert='1' style='border-bottom:1px solid #268CD8'></td></tr>");

                // body middle portion
                strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
                strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
                strStyle = strStyle.Append(@"<p style='color: rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
                strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='99%'><colgroup><col width='50%' /><col width='50%' /></colgroup>");
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Client : </b>&nbsp;" + strClient + "</td></tr>");
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Duration : </b>&nbsp;" + strStartTime + " - " + strEndTime + "</td><td style='color: black; padding: 4px'>");
                //strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Participants : </b>&nbsp;" + txtParticipants.Text.Trim() + "</td></tr>");//comment by sayali on 13-11-2019
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Participants : </b>&nbsp;" + strbuilder_Attendee.ToString() + "</td></tr>");//Added by sayali on 13-11-2019
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Resources : </b>&nbsp;" + strResources + "</td></tr>");
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Observers : </b>&nbsp;" + strObservers + "</td></tr>");
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Special Notes : </b>&nbsp;" + strNotes + "</td></tr>");
                strStyle = strStyle.Append(@"</table></p></div></div></td></tr>");

                // agenda table
                strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
                strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
                strStyle = strStyle.Append(@"<p style='color:rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
                strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='99%'><colgroup><col width='5%' /><col width='30%' /><col width='45%' /><col width='20%' /></colgroup>");
                strStyle = strStyle.Append(@"<tr><td class='AgendaHdr'>Sr.No.</td><td class='AgendaHdr'>Agenda</td><td class='AgendaHdr'>Description</td><td class='AgendaHdr'>Presenter</td></tr>");
                DataSet dsGetMom_Agenda = DBOperations.CRM_GetMOM_Agenda(MomId);
                if (dsGetMom_Agenda != null && dsGetMom_Agenda.Tables[0].Rows.Count > 0)
                {
                    int SL = 0;
                    for (int a = 0; a < dsGetMom_Agenda.Tables[0].Rows.Count; a++)
                    {
                        SL++;
                        strStyle = strStyle.Append(@"<tr>");
                        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBodySL'>" + SL.ToString() + "</td>");
                        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody'>" + dsGetMom_Agenda.Tables[0].Rows[a]["Topic"].ToString() + "</td>");
                       // strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody'"+ "\" > " + dsGetMom_Agenda.Tables[0].Rows[a]["Description"].ToString() + "</td>");   // comment on SAYALI 12-+11-2019
                       
                        //---------------------------ADDED BY SAYALI 12-11-2019-------------------------------------------
                        string[] sep = new string[] { "\r\n" };
                        string[] result = Regex.Split(dsGetMom_Agenda.Tables[0].Rows[a]["Description"].ToString(), "\n\\s*");

                        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody' > ");
                        for (int i = 0; i < result.Length; i++)
                        {
                            string oldString = result[i].ToString();
                            //newString = newString + oldString + Environment.NewLine;
                            strStyle = strStyle.Append(oldString + "<br />");
                        }
                        strStyle = strStyle.Append(@"</td>");
                        //------------------------------------------------------------------------------------------------

                        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody'>" + dsGetMom_Agenda.Tables[0].Rows[a]["PersonName"].ToString() + "</td>");
                        strStyle = strStyle.Append(@"</tr>");
                    }
                }
                strStyle = strStyle.Append(@"</table></p></div></div></td>");

                // body footer
                strStyle = strStyle.Append(@"</table></td></tr><tr><td styleInsert='1' height='80' class='topTr' style=''>");
                strStyle = strStyle.Append(@"<p style='text-align: center; font-weight: 500;font-size: 12pt; padding: 15px;'>");
                strStyle = strStyle.Append(@"<font style='color:white; padding-top: 10px'><b> &nbsp; &nbsp; Note*: </b>This is system generated mail, please do not reply to this message via e-mail.");
                strStyle = strStyle.Append(@"</font></p></td></tr></table>");
                strStyle = strStyle.Append(@"</center></body></html>");

                MessageBody = strStyle.ToString();
                lblCustomerEmail.Text = strAttendeeEmail.ToString();
                txtParticipants.Text = strbuilder_Attendee.ToString();
                txtSubject.Text = "Minutes of Meeting - " + strMeetingTitle + " as held on " + strMeetingDate;
                txtMailCC.Text = strMomCreatedByMail;

                divPreviewEmail.InnerHtml = MessageBody.ToString();
                ModalPopupEmail.Show();
            }
            catch (Exception )
            {
            }
        }
    }

    protected bool SendEmail(int MomId)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilder_Attendee = new StringBuilder();
        StringBuilder strAttendeeEmail = new StringBuilder();

        if (MomId > 0)
        {
            string strMeetingTitle = "", strClient = "", strMeetingDate = "", strStartTime = "", strEndTime = "", strCustomerEmail = "",
                strCCEmail = "",  strSubject = "", MessageBody = "", strObservers = "", strResources = "", strNotes = "", strMomCreatedBy = "", strMomCreatedByMail = "";// strBCCEmail = "",

            //Get Basic Detail - Title, date, time
            DataSet dsGetMeetingDetail = DBOperations.CRM_GetOpportunity_MOM_Lid(MomId);
            if (dsGetMeetingDetail != null && dsGetMeetingDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetMeetingDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    strMeetingTitle = dsGetMeetingDetail.Tables[0].Rows[0]["Title"].ToString();
                    strMeetingDate = Convert.ToDateTime(dsGetMeetingDetail.Tables[0].Rows[0]["Date"]).ToString("MMMM dd, yyyy");
                    if (dsGetMeetingDetail.Tables[0].Rows[0]["StartTime"] != DBNull.Value)
                        strStartTime = Convert.ToDateTime(dsGetMeetingDetail.Tables[0].Rows[0]["StartTime"]).ToString("HH: mm tt");
                    if (dsGetMeetingDetail.Tables[0].Rows[0]["EndTime"] != DBNull.Value)
                        strEndTime = Convert.ToDateTime(dsGetMeetingDetail.Tables[0].Rows[0]["EndTime"]).ToString("HH: mm tt");
                    strClient = dsGetMeetingDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    strObservers = dsGetMeetingDetail.Tables[0].Rows[0]["Observers"].ToString();
                    strResources = dsGetMeetingDetail.Tables[0].Rows[0]["Resources"].ToString();
                    strNotes = dsGetMeetingDetail.Tables[0].Rows[0]["SpecialNotes"].ToString();
                    strMomCreatedBy = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedBy"].ToString();
                    strMomCreatedByMail = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedByMail"].ToString();
                    //strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactName"].ToString());  //----COMMENT BY SAYALI ON 13-11-2019
                    //strAttendeeEmail = strAttendeeEmail.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactEmail"].ToString());  //----COMMENT BY SAYALI ON 13-11-2019
                }

                // Get Attendees Name
                DataSet dsGetMom_Attendee = DBOperations.CRM_GetMom_Attendee(MomId);
                if (dsGetMom_Attendee != null && dsGetMom_Attendee.Tables[0].Rows.Count > 0)
                {
                    if (dsGetMom_Attendee.Tables[0].Rows.Count > 1)
                    {
                        strbuilder_Attendee = strbuilder_Attendee.Append(" , ");
                        strAttendeeEmail = strAttendeeEmail.Append(" , ");
                        for (int a = 0; a < dsGetMom_Attendee.Tables[0].Rows.Count; a++)
                        {
                            if (a != dsGetMom_Attendee.Tables[0].Rows.Count - 1)
                            {
                                strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserName"].ToString() + " , ");
                                strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserEmail"].ToString() + " , ");
                            }
                            else
                            {
                                strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserName"].ToString());
                                strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[a]["UserEmail"].ToString());
                            }
                        }
                    }
                    else
                    {
                        //-------------------------ADDED BY SAYALI ON 13-11-2019------------------------
                        if (strAttendeeEmail.ToString() != "" && strAttendeeEmail.ToString() != null)
                        {
                            strAttendeeEmail = strAttendeeEmail.Append(" , ");
                        }
                        if (strbuilder_Attendee.ToString() != "" && strbuilder_Attendee.ToString() != null)
                        {
                            strbuilder_Attendee = strbuilder_Attendee.Append(" , ");
                        }
                        //-----------------------------------------------------------------------------
                        strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[0]["UserName"].ToString());
                        strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[0]["UserEmail"].ToString());
                    }
                }
            }
            else
                return false;

            // Email Format
            strCustomerEmail = lblCustomerEmail.Text.Trim(); // strAttendeeEmail.ToString();  //"jr.developer@babajishivram.com";// strAttendeeEmail.ToString();  
            strCCEmail = txtMailCC.Text.Trim(); // "dhaval@babajishivram.com , kirti@babajishivram.com";
            strSubject = "Minutes of Meeting - " + strMeetingTitle + " as held on " + strMeetingDate;

            if (strCustomerEmail == "" || strSubject == "")
                return false;
            else
            {
                try
                {
                    StringBuilder strStyle = new StringBuilder();
                    strStyle = strStyle.Append("<html><body class='setupTab' style='font-family:Calibri; font-style:normal; bEditID: b1st1; bLabel: body;'><center>");

                    // email css
                    strStyle = strStyle.Append(@"<style type='text/css'> p {margin-top: 0px; margin-bottom: 0px;} font {color: black;}");
                    strStyle = strStyle.Append(@".topTr {font-family:Calibri; font-style:normal;border: 2px solid darkgray;border-bottom: none;bEditID: r3st1;color: white;bLabel: main;font-size: 12pt;font-family: calibri;height: 35px;background-color: #268CD8;}");
                    strStyle = strStyle.Append(@".MeetingTitle {font-family:Calibri; font-style:normal;font-size: 19px;text-align: right;font-weight: 600;color: #268CD8;} .MeetingDate {font-size: 13px;text-align: right;color: black;font-weight: 400;} ");
                    strStyle = strStyle.Append(@".MOMHdr {font-family:Calibri; font-style:normal; color:black; padding:4px;} .csTitle {font-family:Calibri; font-style:normal;color: #268CD8;} .AgendaHdr {padding: 7px;background-color: #268CD8;color: white;} ");
                    strStyle = strStyle.Append(@".AgendaHdr {font-family:Calibri; font-style:normal;padding: 7px;background - color: #268CD8;color: white;font - weight: 500;}");
                    strStyle = strStyle.Append(@".AgendaBody {font-family:Calibri; font-style:normal;padding-left: 7px;padding-right: 7px;color: black;font - weight: 400;}");
                    strStyle = strStyle.Append(@".AgendaBodySL {font-family:Calibri; font-style:normal;padding-left: 7px;padding-right: 7px;color: black;font - weight: 400;text - align: center;}");
                    strStyle = strStyle.Append(@"</style>");

                    // body header
                    strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
                    strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
                    strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");
                    strStyle = strStyle.Append(@"<tr><td class='MeetingTitle'>" + strMeetingTitle + "<br /><span class='MeetingDate'>" + strMeetingDate + " at " + strStartTime);
                    strStyle = strStyle.Append(@"</span></td></tr><tr><td styleInsert='1' style='border-bottom:1px solid #268CD8'></td></tr>");

                    // body middle portion
                    strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
                    strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
                    strStyle = strStyle.Append(@"<p style='color: rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
                    strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='99%'><colgroup><col width='50%' /><col width='50%' /></colgroup>");
                    strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Client : </b>&nbsp;" + strClient + "</td></tr>");
                    strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Duration : </b>&nbsp;" + strStartTime + " - " + strEndTime + "</td><td style='color: black; padding: 4px'>");
                    // strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Participants : </b>&nbsp;" + txtParticipants.Text.Trim() + "</td></tr>");//comment by sayali on 13-11-2019
                    strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Participants : </b>&nbsp;" + strbuilder_Attendee.ToString() + "</td></tr>"); //Added by Sayali on 13-11-2019
                    strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Resources : </b>&nbsp;" + strResources + "</td></tr>");
                    strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Observers : </b>&nbsp;" + strObservers + "</td></tr>");
                    strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Special Notes : </b>&nbsp;" + strNotes + "</td></tr>");
                    strStyle = strStyle.Append(@"</table></p></div></div></td></tr>");

                    // agenda table
                    strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
                    strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
                    strStyle = strStyle.Append(@"<p style='color:rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
                    strStyle = strStyle.Append(@"<table border='1' cellpadding='0' cellspacing='0' width='99%'><colgroup><col width='5%' /><col width='30%' /><col width='45%' /><col width='20%' /></colgroup>");
                    strStyle = strStyle.Append(@"<tr><td class='AgendaHdr'>Sr.No.</td><td class='AgendaHdr'>Agenda</td><td class='AgendaHdr'>Description</td><td class='AgendaHdr'>Presenter</td></tr>");
                    DataSet dsGetMom_Agenda = DBOperations.CRM_GetMOM_Agenda(MomId);
                    if (dsGetMom_Agenda != null && dsGetMom_Agenda.Tables[0].Rows.Count > 0)
                    {
                        int SL = 0;
                        for (int a = 0; a < dsGetMom_Agenda.Tables[0].Rows.Count; a++)
                        {
                            SL++;
                            strStyle = strStyle.Append(@"<tr>");
                            strStyle = strStyle.Append(@"<td class='AgendaBodySL'>" + SL.ToString() + "</td>");
                            strStyle = strStyle.Append(@"<td class='AgendaBody'>" + dsGetMom_Agenda.Tables[0].Rows[a]["Topic"].ToString() + "</td>");
                            // strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody'"+ "\" > " + dsGetMom_Agenda.Tables[0].Rows[a]["Description"].ToString() + "</td>");   // comment on SAYALI 12-+11-2019

                            //---------------------------ADDED BY SAYALI 12-11-2019-------------------------------------------
                            string[] sep = new string[] { "\r\n" };
                            string[] result = Regex.Split(dsGetMom_Agenda.Tables[0].Rows[a]["Description"].ToString(), "\n\\s*");
                            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody' > ");
                            for (int i = 0; i < result.Length; i++)
                            {
                                string oldString = result[i].ToString();
                                //newString = newString + oldString + Environment.NewLine;
                                strStyle = strStyle.Append(oldString + "<br />");
                            }
                            strStyle = strStyle.Append(@"</td>");
                            //------------------------------------------------------------------------------------------------

                            strStyle = strStyle.Append(@"<td class='AgendaBody'>" + dsGetMom_Agenda.Tables[0].Rows[a]["PersonName"].ToString() + "</td>");
                            strStyle = strStyle.Append(@"</tr>");
                        }
                    }
                    strStyle = strStyle.Append(@"</table></p></div></div></td>");

                    // body footer
                    strStyle = strStyle.Append(@" </table></td></tr><tr><td styleInsert='1' height='80' class='topTr' style=''>");
                    strStyle = strStyle.Append(@"<p style='text-align: left; font-weight: 500;font-size: 12pt; padding: 15px;'>");
                    strStyle = strStyle.Append(@"<font style='color:white; padding-top: 10px'><b> &nbsp; &nbsp; Note*: </b>This is system generated mail, please do not reply to this message via e-mail.");
                    strStyle = strStyle.Append(@"</font></p></td></tr></table>");
                    strStyle = strStyle.Append(@"</center></body></html>");

                    List<string> lstDocPath = new List<string>();
                    MessageBody = strStyle.ToString();
                    bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstDocPath);
                    return bEmailSuccess;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
        else
            return false;
    }

    protected void txtParticipants_TextChanged(object sender, EventArgs e)
    {
        //ShowPopup(Convert.ToInt32(hdnMomId_Popup.Value));
        ModalPopupEmail.Show();
    }

    protected void txtSubject_TextChanged(object sender, EventArgs e)
    {
        ShowPopup(Convert.ToInt32(hdnMomId_Popup.Value));
    }

    protected void txtMailCC_TextChanged(object sender, EventArgs e)
    {
        ShowPopup(Convert.ToInt32(hdnMomId_Popup.Value));
    }

    protected void gvMOMs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "showmom")
        {
            ShowPopup(Convert.ToInt32(e.CommandArgument.ToString()));
            btnSendMail.Visible = false;
        }
    }

    #region Attendee / Agenda

    protected void txtName_TextChanged(object sender, EventArgs e)
    {
        if (hdnAttendeeUserId.Value != "" && hdnAttendeeUserId.Value != "0")
        {
            DataSet dsGetUserDetail = DBOperations.GetUserByID(Convert.ToInt32(hdnAttendeeUserId.Value));
            if (dsGetUserDetail != null && dsGetUserDetail.Tables[0].Rows.Count > 0)
            {
                txtAttendeeEmail.Text = dsGetUserDetail.Tables[0].Rows[0]["sEmail"].ToString();
            }

            hdnAttendeeUserId.Value = "0";
        }
    }

    protected void btnAddAttendee_Click(object sender, EventArgs e)
    {
        int AfterInsertedRows = 0, OriginalRows = 0, PkId = 1;
        DataTable dtAttendee = (DataTable)ViewState["Attendee"];
        if (dtAttendee != null && dtAttendee.Rows.Count > 0)
        {
            for (int i = 0; i < dtAttendee.Rows.Count; i++)
            {
                if (dtAttendee.Rows[i]["PkId"] != null)
                {
                    PkId = Convert.ToInt32(dtAttendee.Rows[i]["PkId"].ToString());
                    PkId++;
                }
            }
        }

        if (dtAttendee != null)
            OriginalRows = dtAttendee.Rows.Count;               //get original rows of grid view.

        if (txtName.Text.Trim() != "") // && hdnAttendeeUserId.Value != "0")
        {
            dtAttendee.Rows.Add(PkId, Convert.ToInt32(hdnAttendeeUserId.Value), Convert.ToString(txtName.Text.Trim()), txtAttendeeEmail.Text.Trim());
            AfterInsertedRows = dtAttendee.Rows.Count;          //get present rows after deleting particular row from grid view.
            ViewState["Attendee"] = dtAttendee;
            BindAttendee();
            if (OriginalRows < AfterInsertedRows)
            {
                lblError.Text = "User Added successfully.";
                lblError.CssClass = "success";
                hdnAttendeeUserId.Value = "0";
                txtName.Text = "";
                txtAttendeeEmail.Text = "";
            }
            else
            {
                lblError.Text = "System Error. Please Try After Sometime!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvExistingAttendee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "remove")
        {
            int gvrow = Convert.ToInt32(e.CommandArgument.ToString());
            int OriginalRows = 0, AfterDeletedRows = 0;
            Label lblPkId = (Label)gvExistingAttendee.Rows[gvrow].FindControl("lblPkId");

            DataTable dtAttendee = ViewState["Attendee"] as DataTable;
            OriginalRows = dtAttendee.Rows.Count;                                   // get original rows of grid view

            DataRow[] drr = dtAttendee.Select("PkId='" + lblPkId.Text + "' ");      // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete();                                                       // delete the row

            AfterDeletedRows = dtAttendee.Rows.Count;                               // get present rows after deleting particular row from grid view
            ViewState["Attendee"] = dtAttendee;
            BindAttendee();
            if (OriginalRows > AfterDeletedRows)
            {
                lblError.CssClass = "success";
                lblError.Text = "Successfully Deleted attendee.";
            }
            else
            {
                lblError.Text = "Error while deleting container details. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void BindAttendee()
    {
        if (ViewState["Attendee"].ToString() != "")
        {
            DataTable dtAttendee = (DataTable)ViewState["Attendee"];
            gvExistingAttendee.DataSource = dtAttendee;
            gvExistingAttendee.DataBind();
        }
    }

    /////////////////////////////////////////////////////////////

    protected void btnAddAgenda_Click(object sender, EventArgs e)
    {
        AddNewRow_Agenda();
    }

    protected void SetInitialRow_Agenda()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("PkId", typeof(string)));
        dt.Columns.Add(new DataColumn("Topic", typeof(string)));
        dt.Columns.Add(new DataColumn("Description", typeof(string)));
        dt.Columns.Add(new DataColumn("PersonName", typeof(string)));

        dr = dt.NewRow();
        dr["PkId"] = 1;
        dr["Topic"] = string.Empty;
        dr["Description"] = string.Empty;
        dr["PersonName"] = string.Empty;
        dt.Rows.Add(dr);
        ViewState["Agenda"] = dt;

        gvAgenda.DataSource = dt;
        gvAgenda.DataBind();
    }

    protected void AddNewRow_Agenda()
    {
        int rowIndex = 0;
        if (ViewState["Agenda"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Agenda"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtTopic = (TextBox)gvAgenda.Rows[rowIndex].Cells[1].FindControl("txtTopic");
                    TextBox txtDescription = (TextBox)gvAgenda.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                    TextBox txtPersonName = (TextBox)gvAgenda.Rows[rowIndex].Cells[3].FindControl("txtPersonName");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["PkId"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Topic"] = txtTopic.Text;
                    dtCurrentTable.Rows[i - 1]["Description"] = txtDescription.Text;
                    dtCurrentTable.Rows[i - 1]["PersonName"] = txtPersonName.Text;

                    rowIndex++;
                }

                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["Agenda"] = dtCurrentTable;

                gvAgenda.DataSource = dtCurrentTable;
                gvAgenda.DataBind();
            }
        }

        SetPreviousData_Agenda();
    }

    protected void SetPreviousData_Agenda()
    {
        int rowIndex = 0;
        if (ViewState["Agenda"] != null)
        {
            DataTable dt = (DataTable)ViewState["Agenda"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtTopic = (TextBox)gvAgenda.Rows[rowIndex].Cells[1].FindControl("txtTopic");
                    TextBox txtDescription = (TextBox)gvAgenda.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                    TextBox txtPersonName = (TextBox)gvAgenda.Rows[rowIndex].Cells[3].FindControl("txtPersonName");

                    txtTopic.Text = dt.Rows[i]["Topic"].ToString();
                    txtDescription.Text = dt.Rows[i]["Description"].ToString();
                    txtPersonName.Text = dt.Rows[i]["PersonName"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    #endregion

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerMom.aspx");
    }


    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        ModalPopupEmail.Hide();
    }
}