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


public partial class CRM_CustomerMom : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "MOM List";

            if (gvMOMs.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For MOM!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }
        DataFilter1.DataSource = DataSourceMOMs;
        DataFilter1.DataColumns = gvMOMs.Columns;
        DataFilter1.FilterSessionID = "CustomerMom.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Others.aspx");
    }

    protected void btnNewMOM_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddCustomerMOM.aspx");
    }

    protected void gvMOMs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "showmom")
        {
            ShowPopup(Convert.ToInt32(e.CommandArgument.ToString()));
        }
    }

    //////////////////////  Mail Events  ////////////////////////////////////////////
    protected void btnCancelPopup_OnClick(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
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

    protected void ShowPopup(int MomId)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilder_Attendee = new StringBuilder();
        StringBuilder strAttendeeEmail = new StringBuilder();

        if (MomId > 0)
        {
            string strMeetingTitle = "", strClient = "", strMeetingDate = "", strStartTime = "", strEndTime = "", strCustomerEmail = "",
                strCCEmail = "", strBCCEmail = "", strSubject = "", MessageBody = "", strObservers = "", strResources = "", strNotes = "", strMomCreatedBy = "", strMomCreatedByMail = "";

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
                    // strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactName"].ToString()); ///COMMENT BY SAYALI ON 13-11-2019
                    // strAttendeeEmail = strAttendeeEmail.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactEmail"].ToString()); ///COMMENT BY SAYALI ON 13-11-2019
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
                        if(strAttendeeEmail.ToString()!="" && strAttendeeEmail.ToString()!=null)
                        {
                            strAttendeeEmail = strAttendeeEmail.Append(" , ");
                        }
                        if(strbuilder_Attendee.ToString()!="" && strbuilder_Attendee.ToString()!=null)
                        {
                            strbuilder_Attendee = strbuilder_Attendee.Append(" , ");
                        }
                        strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMom_Attendee.Tables[0].Rows[0]["UserName"].ToString());
                        strAttendeeEmail = strAttendeeEmail.Append(dsGetMom_Attendee.Tables[0].Rows[0]["UserEmail"].ToString());
                    }
                }
            }

            // Email Format
            //strCustomerEmail = "kivisha.jain@babajishivram.com"; //lblCustomerEmail.Text.Trim(); // strAttendeeEmail.ToString();  //"jr.developer@babajishivram.com";// strAttendeeEmail.ToString();  
            strCCEmail = ""; // txtMailCC.Text.Trim(); // "dhaval@babajishivram.com , kirti@babajishivram.com";
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
                    string newString = "", oldString = "";
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
                            oldString = result[i].ToString();
                            //newString = newString + oldString + Environment.NewLine;
                            strStyle = strStyle.Append( oldString + "<br />");
                        }
                        strStyle =strStyle.Append(@"</td>");
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
            catch (Exception en)
            {
            }
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "MOMListOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvMOMs.AllowPaging = false;
        gvMOMs.AllowSorting = false;
        gvMOMs.Columns[0].Visible = false;
        gvMOMs.Columns[1].Visible = false;
        gvMOMs.Enabled = false;
        gvMOMs.Caption = "Mom Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "CustomerMom.aspx";
        DataFilter1.FilterDataSource();
        gvMOMs.DataBind();
        gvMOMs.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "CustomerMom.aspx";
            DataFilter1.FilterDataSource();
            gvMOMs.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion


    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        ModalPopupEmail.Hide();
    }
}