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

public partial class CRM_AdminEditLead : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(FormViewLead);
        ScriptManager1.RegisterPostBackControl(btnAddContact);
        ScriptManager1.RegisterPostBackControl(gvVisitReport);
        ScriptManager1.RegisterPostBackControl(gvService);
        ScriptManager1.RegisterPostBackControl(gvContractCopy);
        ScriptManager1.RegisterPostBackControl(btnEditQuote);
        ScriptManager1.RegisterPostBackControl(btnDownloadQuote);
        ScriptManager1.RegisterPostBackControl(btnAddContractCopy);
        if (!IsPostBack)
        {
            if (Session["LeadId"] == null)
            {
                Response.Redirect("Leads.aspx");
            }
            else
            {
                gvStatusHistory.DataBind();
                GetLeadDetail(Convert.ToInt32(Session["LeadId"]));
            }
        }
    }

    #region Lead Detail Update

    protected void GetLeadDetail(int LeadId)

    {
        int LeadSourceId = 0, CompanyTypeId = 0, SectorId = 0, CatgId = 0, RoleId = 0;
        bool IsRFQReceived = false;
        //FormViewEnquiry.Visible = false;
        //FormViewRfqQuote.Visible = false;

        DataSet dsGetLeadDetail = DBOperations.CRM_GetLeadById(LeadId);
        if (dsGetLeadDetail.Tables[0].Rows.Count > 0)
        {
            //FormViewLead.DataSource = dsGetLeadDetail.Tables[0];
            FormViewLead.DataBind();

            if (dsGetLeadDetail.Tables[0].Rows[0]["CompanyID"] != DBNull.Value)
            {
                hdnCompanyId.Value = dsGetLeadDetail.Tables[0].Rows[0]["CompanyID"].ToString();
            }
            if (dsGetLeadDetail.Tables[0].Rows[0]["LeadSourceID"] != DBNull.Value)
            {
                LeadSourceId = Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["LeadSourceID"]);
            }
            if (dsGetLeadDetail.Tables[0].Rows[0]["CompanyTypeID"] != DBNull.Value)
            {
                CompanyTypeId = Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["CompanyTypeID"]);
            }
            if (dsGetLeadDetail.Tables[0].Rows[0]["SectorID"] != DBNull.Value)
            {
                SectorId = Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["SectorID"]);
            }
            if (dsGetLeadDetail.Tables[0].Rows[0]["BusinessCategoryID"] != DBNull.Value)
            {
                CatgId = Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["BusinessCategoryID"]);
            }
            if (dsGetLeadDetail.Tables[0].Rows[0]["RoleId"] != DBNull.Value)
            {
                RoleId = Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["RoleId"]);
            }
            if (dsGetLeadDetail.Tables[0].Rows[0]["LeadStageId"] != DBNull.Value)
            {
                if (Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["LeadStageId"]) == 4) // RFQ Received
                {
                    IsRFQReceived = true;
                }
            }

            if (FormViewLead.CurrentMode == FormViewMode.Edit)
            {
                DropDownList ddlSource = (DropDownList)FormViewLead.FindControl("ddlSource");
                DropDownList ddlCompanyType = (DropDownList)FormViewLead.FindControl("ddlCompanyType");
                DropDownList ddlBusinessSector = (DropDownList)FormViewLead.FindControl("ddlBusinessSector");
                DropDownList ddlBusinessCatg = (DropDownList)FormViewLead.FindControl("ddlBusinessCatg");
                DropDownList ddlRole = (DropDownList)FormViewLead.FindControl("ddlRole");

                if (ddlSource != null)
                {
                    ddlSource.DataBind();
                    ddlSource.SelectedValue = LeadSourceId.ToString();
                }

                if (ddlCompanyType != null)
                {
                    ddlCompanyType.DataBind();
                    ddlCompanyType.SelectedValue = CompanyTypeId.ToString();
                }

                if (ddlBusinessSector != null)
                {
                    ddlBusinessSector.DataBind();
                    ddlBusinessSector.SelectedValue = SectorId.ToString();
                }

                if (ddlBusinessCatg != null)
                {
                    ddlBusinessCatg.DataBind();
                    ddlBusinessCatg.SelectedValue = CatgId.ToString();
                }

                if (ddlRole != null)
                {
                    ddlRole.DataBind();
                    ddlRole.SelectedValue = RoleId.ToString();
                }
            }

            DataSet dsGetQuote = DBOperations.CRM_GetQuoteByLead(LeadId);
            if (dsGetQuote != null && dsGetQuote.Tables.Count > 0)
            {
                if (dsGetQuote.Tables[0].Rows.Count > 0)
                {
                    hdnQuotationId.Value = dsGetQuote.Tables[0].Rows[0]["QuotationId"].ToString();
                    hdnQuotePath.Value = dsGetQuote.Tables[0].Rows[0]["QuotePath"].ToString();
                    if (dsGetQuote.Tables[0].Rows[0]["ContractStartDt"] != DBNull.Value)
                    {
                        txtContractStartDt.Text = Convert.ToDateTime(dsGetQuote.Tables[0].Rows[0]["ContractStartDt"]).ToString("dd/MM/yyyy");
                    }

                    if (dsGetQuote.Tables[0].Rows[0]["ContractEndDt"] != DBNull.Value)
                    {
                        txtContractEndDt.Text = Convert.ToDateTime(dsGetQuote.Tables[0].Rows[0]["ContractEndDt"]).ToString("dd/MM/yyyy");
                    }
                }
            }
        }
    }

    protected void btnEditLead_Click(object sender, EventArgs e)
    {
        FormViewLead.ChangeMode(FormViewMode.Edit);
        if (Session["LeadId"] != null)
        {
            GetLeadDetail(Convert.ToInt32(Session["LeadId"]));
        }
    }

    protected void btnDeleteLead_Click(object sender, EventArgs e)
    {

    }

    protected void btnUpdateLead_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancelLead_Click(object sender, EventArgs e)
    {
        FormViewLead.ChangeMode(FormViewMode.ReadOnly);
        if (Session["LeadId"] != null)
        {
            FormViewLead.DataBind();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Opportunity.aspx");
    }

    #endregion

    #region Lead Form-View Update

    protected void DataSourceLead_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        if (Result == 0)
        {
            lblError.Text = "Lead Detail Updated Successfully";
            lblError.CssClass = "success";
            FormViewLead.DataBind();
            gvContacts.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Company Name Does Not Exists.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DataSourceLead_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DataSourceLead_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        if (Result == 0)
        {
            lblError.Text = "Lead Deleted Successfully";
            lblError.CssClass = "success";
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Company Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormViewLead_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormViewLead_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.KeepInEditMode = true;
            e.ExceptionHandled = true;

            if (e.Exception != null)
            {
                lblError.Text = e.Exception.Message;
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void FormViewLead_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.KeepInInsertMode = true;
            e.ExceptionHandled = true;

            if (e.Exception != null)
            {
                lblError.Text = e.Exception.Message;
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void FormViewLead_DataBound(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataRowView drView = (DataRowView)FormViewLead.DataItem;
        if (drView != null)
        {
            lblTitle.Text = "Opportunity - " + drView["LeadRefNo"].ToString();
        }

        // Validate Form
        Page.Validate("Required");
    }

    protected void FormViewLead_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        DropDownList ddlSource = (DropDownList)FormViewLead.FindControl("ddlSource");
        DropDownList ddlCompanyType = (DropDownList)FormViewLead.FindControl("ddlCompanyType");
        DropDownList ddlBusinessSector = (DropDownList)FormViewLead.FindControl("ddlBusinessSector");
        DropDownList ddlBusinessCatg = (DropDownList)FormViewLead.FindControl("ddlBusinessCatg");
        DropDownList ddlRole = (DropDownList)FormViewLead.FindControl("ddlRole");
        if (ddlSource != null)
            e.NewValues["LeadSourceId"] = ddlSource.SelectedValue;
        if (ddlCompanyType != null)
            e.NewValues["CompanyTypeId"] = ddlCompanyType.SelectedValue;
        if (ddlBusinessSector != null)
            e.NewValues["SectorId"] = ddlBusinessSector.SelectedValue;
        if (ddlBusinessCatg != null)
            e.NewValues["CatgId"] = ddlBusinessCatg.SelectedValue;
        if (ddlRole != null)
            e.NewValues["RoleId"] = ddlRole.SelectedValue;
    }

    #endregion

    #region Services

    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "deleterow")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = DBOperations.CRM_DelLeadService(lid);
                if (result == 0)
                {
                    lblError.Text = "Service Deleted Successfully";
                    lblError.CssClass = "success";
                    gvService.DataBind();
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Service Does Not Exists.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }

    }

    #endregion

    #region Contacts Update

    protected void btnAddContact_Click(object sender, EventArgs e)
    {
        if (hdnCompanyId.Value != "0" && hdnCompanyId.Value != "")
        {
            int result = DBOperations.CRM_AddContactDetail(txtContactName.Text.Trim(), Convert.ToInt32(hdnCompanyId.Value), txtDesignation.Text.Trim(),
                                         Convert.ToInt16(ddlRole.SelectedValue), txtMobileNo.Text.Trim(), txtEmail.Text.Trim(), txtContactNo.Text.Trim(),
                                         txtContactAddress.Text.Trim(), txtDescription.Text.Trim(), LoggedInUser.glUserId);
            if (result > 0)
            {
                lblError.Text = "Contact added successfully!";
                lblError.CssClass = "success";
                gvContacts.DataBind();
            }
            else if (result == -2)
            {
                lblError.Text = "Contact already exists!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System error! Please try again later.";
                lblError.CssClass = "success";
            }
        }
        else
        {
            lblError.Text = "No Company found for this lead! Please check before saving contact.";
            lblError.CssClass = "errorMsg";
            return;
        }
    }

    protected void btnCancelContact_Click(object sender, EventArgs e)
    {
        gvContacts.DataBind();
        txtContactAddress.Text = "";
        txtContactName.Text = "";
        txtDescription.Text = "";
        txtDesignation.Text = "";
        txtEmail.Text = "";
        txtMobileNo.Text = "";
        txtContactNo.Text = "";
        lblError.Text = "";
    }

    #endregion

    #region Visit Report Update
    protected void btnAddVisitReport_Click(object sender, EventArgs e)
    {
        DateTime dtVisitDate = DateTime.MinValue;
        if (txtVisitDate.Text.Trim() != "")
            dtVisitDate = Commonfunctions.CDateTime(txtVisitDate.Text.Trim());

        int result = DBOperations.CRM_AddVisitReport(Convert.ToInt32(Session["LeadId"]), dtVisitDate, txtVisitRemark.Text.Trim(), LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Visit Report Added Successfully";
            lblError.CssClass = "success";
            txtVisitRemark.Text = "";
            txtVisitDate.Text = "";
            gvVisitReport.DataBind();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Visit Report Already Exists for same date!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancelVisitReport_Click(object sender, EventArgs e)
    {
        txtVisitDate.Text = "";
        txtVisitRemark.Text = "";
        lblError.Text = "";
    }

    protected void gvVisitReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "deleterow")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = DBOperations.CRM_DelVisitReport(lid, LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Visit Report Deleted Successfully";
                    lblError.CssClass = "success";
                    txtVisitRemark.Text = "";
                    txtVisitDate.Text = "";
                    gvVisitReport.DataBind();
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Visit Report Does Not Exists!";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    #endregion

    #region MOM Update
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
                    strbuilder_Attendee = strbuilder_Attendee.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactName"].ToString());
                    strAttendeeEmail = strAttendeeEmail.Append(dsGetMeetingDetail.Tables[0].Rows[0]["CompContactEmail"].ToString());
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
                        strbuilder_Attendee = strbuilder_Attendee.Append(" , ");
                        strAttendeeEmail = strAttendeeEmail.Append(" , ");
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
                strStyle = strStyle.Append(@".AgendaHdr {font-family:Calibri; font-style:normal;padding: 7px;background - color: #268CD8;color: white;font - weight: 500;text-align:center}");
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
                strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'>Participants : </b>&nbsp;" + txtParticipants.Text.Trim() + "</td></tr>");
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
                        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc;' class='AgendaBody'>" + dsGetMom_Agenda.Tables[0].Rows[a]["Description"].ToString() + "</td>");
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

    protected void gvMOMs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "showmom")
        {
            ShowPopup(Convert.ToInt32(e.CommandArgument.ToString()));
        }
    }

    #endregion

    #region Quotation/Contracts Update

    protected void btnEditQuote_Click(object sender, EventArgs e)
    {
        if (hdnQuotationId.Value != "" && hdnQuotationId.Value != "0")
        {
            Session["QuotationId"] = hdnQuotationId.Value.Trim();
            Response.Redirect("EditQuote.aspx");
        }
    }

    protected void btnDownloadQuote_Click(object sender, EventArgs e)
    {
        if (hdnQuotationId.Value != "" && hdnQuotationId.Value != "0")
        {
            string DocPath = hdnQuotePath.Value.Trim();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }
        else
        {
            lblError.Text = "Quotation does not exists!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnAddContractCopy_OnClick(object sender, EventArgs e)
    {
        string fileName = "";
        int result = 0;
        if (fuUploadContractCopy != null && fuUploadContractCopy.HasFile)
            fileName = UploadFiles(fuUploadContractCopy, "");

        if (txtContractStartDt.Text.Trim() != "")
        {
            DateTime dtContractStart = DateTime.MinValue;
            DateTime dtContractEnd = DateTime.MinValue;
            if (txtContractStartDt.Text.Trim() != "")
                dtContractStart = Commonfunctions.CDateTime(txtContractStartDt.Text.Trim());
            if (txtContractEndDt.Text.Trim() != "")
                dtContractEnd = Commonfunctions.CDateTime(txtContractEndDt.Text.Trim());

            int SaveUpdateDates = QuotationOperations.UpdateQuotationContractDates(Convert.ToInt32(hdnQuotationId.Value), dtContractStart, dtContractEnd,"", Convert.ToInt32(LoggedInUser.glUserId.ToString()));
            if (SaveUpdateDates == 2)
            {
                lblError.Text = "Document added successfully!";
                lblError.CssClass = "success";
                gvContractCopy.DataBind();
                txtContractEndDt.Text = "";
                txtContractStartDt.Text = "";
            }
            else if (SaveUpdateDates == 3)
            {
                lblError.Text = "Quotation does not exists..!!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System error. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }

        if (fileName != "")
        {
            result = QuotationOperations.AddQuotationAnnexure(Convert.ToInt32(hdnQuotationId.Value), fileName, Convert.ToInt32(LoggedInUser.glUserId.ToString()));
            if (result == 1)
            {
                DateTime dtClose = DateTime.MinValue;
                // add lead stage history
                int result_History = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(Session["LeadId"]), 13, dtClose, "", LoggedInUser.glUserId);

                //lblError.Text = "Document added successfully!";
                //lblError.CssClass = "success";
                //gvContractCopy.DataBind();
                string message = "Contract uploaded for lead successfully! Lead moved to Contract Approval tab.";
                string url = "ContractApproval.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvContractCopy_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "deletedoc")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                int result = QuotationOperations.DeleteAnnexureDocs(lid, Convert.ToInt32(LoggedInUser.glUserId));
                if (result == 1)
                {
                    lblError.Text = "Successfully deleted document!";
                    lblError.CssClass = "success";
                    gvContractCopy.DataBind();
                }
                else
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  QuotationOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "Quotation\\" + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }
        else
        {
            return "";
        }

    }

    protected string RandomString(int size)
    {

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    #endregion
}