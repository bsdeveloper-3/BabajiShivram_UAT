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

public partial class CRM_LeadStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(FormViewLead);
        ScriptManager1.RegisterPostBackControl(btnSaveStatus);
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

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        DateTime dtClose = DateTime.MinValue;
        int result = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(Session["LeadId"]), Convert.ToInt32(ddlStatus.SelectedValue),
                                                           dtClose, txtRemark.Text.Trim(), LoggedInUser.glUserId);

        if (result == 0)
        {
            if (Convert.ToInt32(ddlStatus.SelectedValue) == 6) // converted
            {
                // Update Lead Stage
                int LeadStage = DBOperations.CRM_UpdateLeadStatus(Convert.ToInt32(Session["LeadId"]), true, false, false, false, false, false, LoggedInUser.glUserId);

                string message = "Lead successfully converted and forwarded to Lead Approved tab!";
                string url = "Converted.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            else if (Convert.ToInt32(ddlStatus.SelectedValue) == 5) // send for approval
            {
                string strCustomerEmail = "";
                int UserId = 0;
                List<string> lstEmailTo = new List<string>();
                lstEmailTo.Add("1");
                lstEmailTo.Add("2");

                if (lstEmailTo.Count > 0)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (lstEmailTo[k].ToString() == "1")
                        {
                            strCustomerEmail = "kirti@babajishivram.com";
                            UserId = 189;
                        }
                        else
                        {
                            strCustomerEmail = "dhaval@babajishivram.com";
                            UserId = 3;
                        }

                        bool bSentMail = SendEmail(Convert.ToInt32(Session["LeadId"]), UserId, strCustomerEmail);
                        if (bSentMail == true)
                        {
                            string message = "Lead has been sent for management approval!";
                            string url = "Leads.aspx";
                            string script = "window.onload = function(){ alert('";
                            script += message;
                            script += "');";
                            script += "window.location = '";
                            script += url;
                            script += "'; }";
                            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                            break;
                        }
                    }
                }
            }
            else
            {
                string message = "Lead status updated successfully!";
                string url = "Leads.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
            gvStatusHistory.DataBind();
        }
        else if (result == 2)
        {
            lblError.Text = "Lead Status Already Updated.";
            lblError.CssClass = "errorMsg";
            gvStatusHistory.DataBind();
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
                if (FormViewLead.CurrentMode == FormViewMode.ReadOnly)
                {
                    if (Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["LeadStageId"]) == 4) // RFQ Received
                    {
                        IsRFQReceived = true;
                    }

                    if (Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["LeadStageId"]) == 10 ||
                        Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["LeadStageId"]) == 5) // Rejected OR MGMT Approval Pending
                    {
                        ddlStatus.Items.RemoveAt(1);   // First Attempt
                        ddlStatus.Items.RemoveAt(1);   // Under Progress
                        ddlStatus.Items.RemoveAt(1);   // Converted
                        if (Convert.ToInt32(dsGetLeadDetail.Tables[0].Rows[0]["LeadStageId"]) == 5)
                            ddlStatus.Items.RemoveAt(1);   // Send lead for approval
                    }
                    else
                    {
                        ddlStatus.Items.RemoveAt(4);   // Send lead for approval
                    }
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
                    ddlSource.SelectedValue = LeadSourceId.ToString();
                }

                if (ddlCompanyType != null)
                {
                    ddlCompanyType.SelectedValue = CompanyTypeId.ToString();
                }

                if (ddlBusinessSector != null)
                {
                    ddlBusinessSector.SelectedValue = SectorId.ToString();
                }

                if (ddlBusinessCatg != null)
                {
                    ddlBusinessCatg.SelectedValue = CatgId.ToString();
                }

                if (ddlRole != null)
                {
                    ddlRole.SelectedValue = RoleId.ToString();
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
        Response.Redirect("Leads.aspx");
    }

    protected bool SendEmail(int LeadId, int UserId, string strCustomerEmail)
    {
        bool bEmailSuccess = false;
        if (LeadId > 0)
        {
            string strCCEmail = "", strSubject = "", MessageBody = "", EmailContent = "";
            DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
            if (dsGetLead != null)
            {
                try
                {
                    //string strFileName = "../EmailTemplate/CRM_Lead.html";
                    string strFileName = "../EmailTemplate/LeadApproval.txt";
                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    lblError.CssClass = "errorMsg";
                }

                MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/NewLeadApproval.aspx?a=" + UserId.ToString() + "&i=" + dsGetLead.Tables[0].Rows[0]["lid"].ToString());
                MessageBody = MessageBody.Replace("@b", "http://live.babajishivram.com/CRM/NewLeadRejected.aspx?a=" + UserId.ToString() + "&i=" + dsGetLead.Tables[0].Rows[0]["lid"].ToString());
                MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
                MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
                MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
                MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
                MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
                MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
                MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

                strSubject = "Revised Lead Approval";
                strCustomerEmail = "dhaval@babajishivram.com"; //"kivisha.jain@babajishivram.com";
                strCCEmail = "javed.shaikh@babajishivram.com" + " , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

                if (strCustomerEmail == "" || strSubject == "")
                    return false;
                else
                {
                    List<string> lstFileDoc = new List<string>();
                    DataSet dsGetRFQDocs = DBOperations.CRM_GetRFQDocuments(LeadId);
                    if (dsGetRFQDocs != null)
                    {
                        for (int i = 0; i < dsGetRFQDocs.Tables[0].Rows.Count; i++)
                        {
                            if (dsGetRFQDocs.Tables[0].Rows[i]["DocPath"].ToString() != "")
                            {
                                lstFileDoc.Add("Quotation\\" + dsGetRFQDocs.Tables[0].Rows[i]["DocPath"].ToString());
                            }
                        }
                    }

                    bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFileDoc);
                    return bEmailSuccess;
                }
            }
            else
                return false;
        }
        else
            return false;
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
            lblTitle.Text = "Lead - " + drView["LeadRefNo"].ToString();
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

    //#region Services
    //protected void ResetServiceControls()
    //{
    //    txtCloseDate.Text = "";
    //    txtVolumeExp.Text = "";
    //    txtRequirement.Text = "";
    //    ddlService.Enabled = true;
    //    ddlService.Items.Clear();
    //    ddlService.DataBind();
    //    ddlLocation.Items.Clear();
    //    ddlLocation.DataBind();
    //    btnAddService.Text = "Save";
    //    gvService.DataBind();
    //}

    //protected void btnAddService_Click(object sender, EventArgs e)
    //{
    //    if (ddlService.SelectedValue != "0")
    //    {
    //        DateTime dtCloseDate = DateTime.MinValue;
    //        if (txtCloseDate.Text.Trim() != "")
    //            dtCloseDate = Commonfunctions.CDateTime(txtCloseDate.Text.Trim());

    //        if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
    //        {
    //            if (btnAddService.Text.ToLower().Trim() == "save")
    //            {
    //                int Result = DBOperations.CRM_AddLeadService(Convert.ToInt32(Session["LeadId"]), Convert.ToInt32(ddlService.SelectedValue),
    //                        Convert.ToInt32(ddlLocation.SelectedValue), txtVolumeExp.Text.Trim(), txtRequirement.Text.Trim(), dtCloseDate, LoggedInUser.glUserId);
    //                if (Result > 0)
    //                {
    //                    lblError.Text = "Service Added Successfully";
    //                    lblError.CssClass = "success";
    //                    ResetServiceControls();
    //                }
    //                else if (Result == 1)
    //                {
    //                    lblError.Text = "System Error! Please try after sometime";
    //                    lblError.CssClass = "errorMsg";
    //                }
    //                else if (Result == 2)
    //                {
    //                    lblError.Text = "Service Already Exists.";
    //                    lblError.CssClass = "errorMsg";
    //                }
    //            }
    //            else
    //            {
    //                int Result = DBOperations.CRM_UpdLeadService(Convert.ToInt32(hdnLid.Value), Convert.ToInt32(ddlLocation.SelectedValue),
    //                                              txtVolumeExp.Text.Trim(), txtRequirement.Text.Trim(), dtCloseDate, LoggedInUser.glUserId);
    //                if (Result == 0)
    //                {
    //                    lblError.Text = "Service Updated Successfully";
    //                    lblError.CssClass = "success";
    //                    ResetServiceControls();
    //                }
    //                else if (Result == 1)
    //                {
    //                    lblError.Text = "System Error! Please try after sometime";
    //                    lblError.CssClass = "errorMsg";
    //                }
    //                else if (Result == 2)
    //                {
    //                    lblError.Text = "Service Does Not Exists.";
    //                    lblError.CssClass = "errorMsg";
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        lblError.Text = "Please select service";
    //        lblError.CssClass = "errorMsg";
    //    }
    //}

    //protected void btnCancelService_Click(object sender, EventArgs e)
    //{
    //    ResetServiceControls();
    //}

    //protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToLower().Trim() == "editrow")
    //    {
    //        int lid = Convert.ToInt32(e.CommandArgument.ToString());
    //        if (lid > 0)
    //        {
    //            DataSet dsGetDetail = DBOperations.CRM_GetServicesById(lid);
    //            if (dsGetDetail != null && dsGetDetail.Tables[0].Rows.Count > 0)
    //            {
    //                hdnLid.Value = dsGetDetail.Tables[0].Rows[0]["lid"].ToString();
    //                ddlService.SelectedValue = dsGetDetail.Tables[0].Rows[0]["ServiceId"].ToString();
    //                ddlService.Enabled = false;
    //                ddlLocation.SelectedValue = dsGetDetail.Tables[0].Rows[0]["ServiceLocationId"].ToString();
    //                txtVolumeExp.Text = dsGetDetail.Tables[0].Rows[0]["VolumeExpected"].ToString();
    //                txtRequirement.Text = dsGetDetail.Tables[0].Rows[0]["Requirement"].ToString();
    //                if (dsGetDetail.Tables[0].Rows[0]["ExpectedCloseDate"] != DBNull.Value)
    //                    txtCloseDate.Text = Convert.ToDateTime(dsGetDetail.Tables[0].Rows[0]["ExpectedCloseDate"]).ToString("dd/MM/yyyy");
    //                btnAddService.Text = "Update";
    //            }
    //        }
    //    }
    //    else if (e.CommandName.ToLower().Trim() == "deleterow")
    //    {
    //        int lid = Convert.ToInt32(e.CommandArgument.ToString());
    //        if (lid > 0)
    //        {
    //            int result = DBOperations.CRM_DelLeadService(lid);
    //            if (result == 0)
    //            {
    //                lblError.Text = "Service Deleted Successfully";
    //                lblError.CssClass = "success";
    //                ResetServiceControls();
    //            }
    //            else if (result == 1)
    //            {
    //                lblError.Text = "System Error! Please try after sometime";
    //                lblError.CssClass = "errorMsg";
    //            }
    //            else if (result == 2)
    //            {
    //                lblError.Text = "Service Does Not Exists.";
    //                lblError.CssClass = "errorMsg";
    //            }
    //        }
    //    }

    //}

    //#endregion

}