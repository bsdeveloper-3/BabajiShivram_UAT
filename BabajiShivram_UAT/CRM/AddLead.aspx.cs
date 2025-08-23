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
using System.IO;
using System.Text;
using AjaxControlToolkit;
using System.Collections.Generic;
using System.Security.Cryptography;


public partial class CRM_AddLead : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnEnquiryNo.Value = DBOperations.GetEnquiryRefNo();
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument2);
        ScriptManager1.RegisterPostBackControl(rptDocument2);
        ScriptManager1.RegisterPostBackControl(gvService);
        //ScriptManager1.RegisterPostBackControl(ddlRFQReceived);

        if (!IsPostBack)
        {
            dvRFQ.Visible = false;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Add Lead";
            QuotationOperations.FillBranchByUser(ddlBabajiBranch, LoggedInUser.glUserId);
            Service_InitialRow();
            DataTable dtAnnexureDoc2 = new DataTable();
            dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
            ViewState["AnnexureDoc2"] = dtAnnexureDoc2;

        }

    }
    protected void txtCompanyName_TextChanged(object sender, EventArgs e)
    {
        if (hdnCompanyId.Value != "0" && hdnCompanyId.Value != "")
        {
            lblError.Text = "";
            int CompanyId = 0;
            string CompanyName = txtCompanyName.Text.Trim();
            if (hdnCompanyId.Value != "0" && hdnCompanyId.Value != "")
                CompanyId = Convert.ToInt32(hdnCompanyId.Value);

            if (CompanyId > 0)
            {
                DataSet dsGetCompanyDetails = DBOperations.GetParticularCompany(CompanyId);
                if (dsGetCompanyDetails != null && dsGetCompanyDetails.Tables[0].Rows.Count > 0)
                {
                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                    if (dsGetCompanyDetails.Tables[0].Rows[0]["lid"] != DBNull.Value)
                    {
                        lblError.Text = "Lead already exists for company - <b><u>" + CompanyName.ToUpper().Trim() + " (owner - " + dsGetCompanyDetails.Tables[0].Rows[0]["CreatedBy"] + ") </u></b>";
                        lblError.CssClass = "errorMsg";
                        hdnCompanyId.Value = "";
                    }
                }

                DataSet dsGetCompanyDetails2 = DBOperations.GetBabajiCustomerByLid(CompanyId);
                if (dsGetCompanyDetails2 != null && dsGetCompanyDetails2.Tables[0].Rows.Count > 0)
                {
                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                    if (dsGetCompanyDetails2.Tables[0].Rows[0]["lid"] != DBNull.Value)
                    {
                        lblError.Text = "Company already exists - <b><u>" + CompanyName.ToUpper().Trim() + " (owner - " + dsGetCompanyDetails2.Tables[0].Rows[0]["CreatedBy"] + ") </u></b>. \n Please add enquiry for customer.";
                        lblError.CssClass = "errorMsg";
                        hdnCompanyId.Value = "";
                    }
                }
            }
        }
        else
        {
            btnSubmit.Visible = true;
            btnCancel.Visible = true;
            lblError.Text = "";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int strAllow=0;
        if(gvService.Rows.Count>0)
        {
            for (int c = 0; c < gvService.Rows.Count; c++)
            { 
                DropDownList ddlService = (DropDownList)gvService.Rows[c].FindControl("ddlService");
                DropDownList ddlLocation = (DropDownList)gvService.Rows[c].FindControl("ddlLocation");

                if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
                {
                    strAllow = 1;
                }
            }
        }
        if (strAllow.ToString()=="1")
        {
            int CompanyId = 0, CompanyTypeId = 0, RoleId = 0, LeadSourceId = 0, LeadStageId = 5, SectorId = 0, CategoryId = 0;// StatusId = 0;
            string ContactName = "", Designation = "", Email = "", MobileNo = "", AlternatePhone = "",
                    Description = "", SourceDesc = "", Turnover = "", EmployeeCount = "", OfficeLocation = "";
            string PaymentTerms = "", CustReference = "", EnquiryNo = "", YearsInService = "", TotalEmp = "", CompanyName = ""; //VolumeExpected = "",

            if (txtCompanyName != null && txtCompanyName.Text.Trim() != "")
            {
                ContactName = txtContactName.Text.Trim();
                Designation = txtDesignation.Text.Trim();
                Email = txtEmail.Text.Trim();
                MobileNo = txtMobileNo.Text.Trim();
                AlternatePhone = txtContactNo.Text.Trim();
                Description = txtDescription.Text.Trim();
                SourceDesc = txtSourceDescription.Text.Trim();
                Turnover = txtTurnover.Text.Trim();
                EmployeeCount = txtEmployeeCount.Text.Trim();
                OfficeLocation = txtOfficeLocation.Text.Trim();

                //PaymentTerms = txtPaymentTerms.Text.Trim();
                CustReference = txtCustRef.Text.Trim();
                YearsInService = txtYearsInService.Text.Trim();
                TotalEmp = txtEmployeeCount.Text.Trim();

                CompanyId = DBOperations.CRM_AddCompanyMS(txtCompanyName.Text.Trim(), Email, MobileNo, AlternatePhone, txtAddressLine1.Text.Trim(),
                                            txtAddressLine2.Text.Trim(), txtAddressLine3.Text.Trim(), txtWebsite.Text.Trim(), Description, ContactName, OfficeLocation, LoggedInUser.glUserId);
                if (CompanyId > 0)
                {
                    // add contact detail
                    int Contact = DBOperations.CRM_AddContactDetail(txtContactName.Text.Trim(), CompanyId, txtDesignation.Text.Trim(), Convert.ToInt16(ddlRole.SelectedValue),
                                            txtMobileNo.Text.Trim(), txtEmail.Text.Trim(), txtContactNo.Text.Trim(), "", "", LoggedInUser.glUserId);

                    if (ddlSource.SelectedValue != "0")
                        LeadSourceId = Convert.ToInt32(ddlSource.SelectedValue);
                    if (ddlRole.SelectedValue != "0")
                        RoleId = Convert.ToInt32(ddlRole.SelectedValue);
                    if (ddlCompanyType.SelectedValue != "0")
                        CompanyTypeId = Convert.ToInt32(ddlCompanyType.SelectedValue);
                    if (ddlBusinessSector.SelectedValue != "0")
                        SectorId = Convert.ToInt32(ddlBusinessSector.SelectedValue);
                    if (ddlBusinessCatg.SelectedValue != "0")
                        CategoryId = Convert.ToInt32(ddlBusinessCatg.SelectedValue);

                    //if (ddlRFQReceived.SelectedValue == "1") // yes
                    //{
                    //    LeadStageId = 4; // Enquiry - RFQ Received
                    //}

                    int result = DBOperations.CRM_AddLead(CompanyId, LeadStageId, LeadSourceId, SectorId, RoleId, CompanyTypeId, CategoryId, SourceDesc, Turnover,
                                                           EmployeeCount, ContactName, Designation, Email, MobileNo, LoggedInUser.glUserId);

                    Session["LeadId"] = result.ToString();
                    if (result > 0)
                    {

                        // Update Lead Stage
                        int LeadStage = DBOperations.CRM_UpdateLeadStatus(result, false, false, false, false, false, false, LoggedInUser.glUserId);

                        // add lead stage history
                        int result_History = DBOperations.CRM_AddLeadStageHistory(result, LeadStageId, dtClose, "", LoggedInUser.glUserId);

                        // add enquiry for normal lead 
                        //if (ddlRFQReceived.SelectedValue == "2")  //No
                        //{
                        EnquiryNo = DBOperations.GetEnquiryRefNo();
                        int result_Save = DBOperations.CRM_AddEnquiry(result, EnquiryNo, "", false, 0, PaymentTerms, CustReference, Turnover, YearsInService, TotalEmp, CompanyTypeId, "", LoggedInUser.glUserId);
                        if (result_Save > 0)
                        {
                            int EnquiryStatus = DBOperations.CRM_AddEnquiryHistory(result_Save, 0, "", LoggedInUser.glUserId);
                            // Update Lead Stage
                            LeadStage = DBOperations.CRM_UpdateLeadStatus(result, false, false, true, false, false, false, LoggedInUser.glUserId);
                        }

                        // FOR SERVICE
                        int count = 0, CompanyType = 0;
                        // validate service entered or not
                        if (gvService.Rows.Count > 0)
                        {
                            for (int c = 0; c < gvService.Rows.Count; c++)
                            {
                                DropDownList ddlService = (DropDownList)gvService.Rows[c].FindControl("ddlService");
                                DropDownList ddlLocation = (DropDownList)gvService.Rows[c].FindControl("ddlLocation");

                                if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
                                {
                                    count++;
                                }
                            }
                        }

                        if (count == 0)
                        {
                            lblError.Text = "Please enter atleast one service offered!";
                            lblError.CssClass = "errorMsg";
                        }
                        else
                        {
                            // get lead details by lid
                            DataSet dsGetLead = DBOperations.CRM_GetLeadById(result);
                            if (dsGetLead != null)
                            {
                                hdnCompanyType.Value = dsGetLead.Tables[0].Rows[0]["CompanyTypeID"].ToString();
                                hdnEmployeeCount.Value = dsGetLead.Tables[0].Rows[0]["EmployeeCount"].ToString();
                                hdnTurnover.Value = dsGetLead.Tables[0].Rows[0]["Turnover"].ToString();
                                CompanyName = dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString();
                            }

                            PaymentTerms = txtPaymentTerms.Text.Trim();
                            CustReference = txtCustRef.Text.Trim();
                            Turnover = hdnTurnover.Value.Trim();
                            YearsInService = txtYearsInService.Text.Trim();
                            TotalEmp = hdnEmployeeCount.Value.Trim();
                            CompanyType = Convert.ToInt32(hdnCompanyType.Value.Trim());

                            // add enquiry services
                            if (gvService.Rows.Count > 0)
                            {
                                for (int c = 0; c < gvService.Rows.Count; c++)
                                {
                                    DateTime dtCloseDate = DateTime.MinValue;
                                    DropDownList ddlService = (DropDownList)gvService.Rows[c].FindControl("ddlService");
                                    DropDownList ddlLocation = (DropDownList)gvService.Rows[c].FindControl("ddlLocation");
                                    TextBox txtVolumeExp = (TextBox)gvService.Rows[c].FindControl("txtVolumeExp");
                                    //TextBox txtCloseDate = (TextBox)gvService.Rows[c].FindControl("txtCloseDate");
                                    TextBox txtRequirement = (TextBox)gvService.Rows[c].FindControl("txtRequirement");

                                    //if (txtCloseDate.Text.Trim() != "")
                                    //    dtCloseDate = Commonfunctions.CDateTime(txtCloseDate.Text.Trim());

                                    if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
                                    {
                                        int result_Service = DBOperations.CRM_AddLeadService(result, Convert.ToInt32(ddlService.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue),
                                                                                txtVolumeExp.Text.Trim(), txtRequirement.Text.Trim(), dtCloseDate, LoggedInUser.glUserId);
                                        if (result_Service > 0)
                                        {
                                            int AddLeadService = DBOperations.CRM_AddEnquiry_Service(result_Service, Convert.ToInt32(ddlService.SelectedValue), LoggedInUser.glUserId);
                                        }
                                    }
                                }
                            }
                        }


                        if (LeadStageId == 4) // ad-hoc enquiry
                        {

                            // add lead stage history
                            int result_LeadStatus = DBOperations.CRM_AddLeadStageHistory(result, 6, dtClose, "", LoggedInUser.glUserId);
                            LeadStage = DBOperations.CRM_UpdateLeadStatus(result, true, false, true, false, false, false, LoggedInUser.glUserId);

                            if (result > 0)
                            {
                                string message = "Successfully added lead and service. Lead sent for management approval!";
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
                        else
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

                                    bool bSentMail = SendEmail(result, UserId, strCustomerEmail);
                                    //bool bSentMail = true;
                                    if (bSentMail == true)
                                    {
                                        if (Session["LeadId"].ToString() != "")
                                    {
                                        DateTime dtCloseDate = DateTime.MinValue;
                                        string message = "Successfully added lead and service. Lead sent for management approval!";
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
                        }
                    }
                    else if (result == 2)
                    {
                        lblError.Text = "Lead already exists!";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System error! Please try again later.";
                        lblError.CssClass = "errorMsg";
                    }
                }
                else if (CompanyId == 1)
                {
                    lblError.Text = "System error! Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Company already exists!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please enter company name!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please add atlest one service!";
            lblError.CssClass = "errorMsg";
        }
        
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtAddressLine1.Text = "";
        txtAddressLine2.Text = "";
        txtAddressLine3.Text = "";
        txtCompanyName.Text = "";
        txtContactName.Text = "";
        txtContactNo.Text = "";
        txtDescription.Text = "";
        txtDesignation.Text = "";
        txtEmail.Text = "";
        txtEmployeeCount.Text = "";
        txtMobileNo.Text = "";
        txtOfficeLocation.Text = "";
        txtSourceDescription.Text = "";
        txtTurnover.Text = "";
        //ddlService.SelectedIndex = -1;
        //ddlLocation.SelectedIndex = -1;
        //txtRequirement.Text = "";
        //txtVolumeExp.Text = "";
        //txtCloseDate.Text = "";
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
                string EncryptedUserId = HttpUtility.UrlEncode(Encrypt(UserId.ToString()));
                string EncryptedLId = HttpUtility.UrlEncode(Encrypt(dsGetLead.Tables[0].Rows[0]["lid"].ToString()));
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

                MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/LeadApproval.aspx?a=" + EncryptedUserId + "&i=" + EncryptedLId);
                //MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/NewLeadApproval.aspx?a=" + EncryptedUserId + "&i=" + EncryptedLId);
                //MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/NewLeadApproval.aspx?a=" + UserId.ToString() + "&i=" + dsGetLead.Tables[0].Rows[0]["lid"].ToString());
                //MessageBody = MessageBody.Replace("@b", "http://live.babajishivram.com/CRM/NewLeadRejected.aspx?a=" + UserId.ToString() + "&i=" + dsGetLead.Tables[0].Rows[0]["lid"].ToString());
                MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
                MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
                MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
                MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
                MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
                MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
                MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

                strSubject = "Pending Lead Approval";
                strCustomerEmail = "dhaval@babajishivram.com"; //"kivisha.jain@babajishivram.com";  
                strCCEmail = "javed.shaikh@babajishivram.com"; //+ " , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();  

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

    #region ENCRYPT/DECRYPT QUERYSTRING VARIABLES
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    private string Decrypt(string cipherText)
    {
        try
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
        catch (Exception)
        {

        }
        return cipherText;
    }
    #endregion

    protected void btnSaveDocument2_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (fuDocument2.HasFile)//fuDocument2 != null &&
            fileName = UploadFiles(fuDocument2, "");

        if (fileName != "")
        {
            DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
            if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
            {
                for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                {
                    if (dtAnnexure.Rows[i]["PkId"] != null)
                    {
                        PkId = Convert.ToInt32(dtAnnexure.Rows[i]["PkId"].ToString());
                        PkId++;
                    }
                }
            }
            if (dtAnnexure != null)
                OriginalRows = dtAnnexure.Rows.Count;              //get original rows of grid view.

            dtAnnexure.Rows.Add(PkId, fileName, fuDocument2.FileName, LoggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;     //get present rows after deleting particular row from grid view.
            ViewState["AnnexureDoc2"] = dtAnnexure;
            BindGrid();
            if (OriginalRows < AfterInsertedRows)
            {
                lblError.Text = "Document Added successfully!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please first select a file to upload...";
            lblError.CssClass = "errorMsg";
        }
    }

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FilePath);
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FilePath;

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
            return "";
    }

    protected string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }
        return builder.ToString();
    }

    protected void rptDocument2_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int OriginalRows = 0, AfterDeletedRows = 0;
            HiddenField hdnDocLid = (HiddenField)e.Item.FindControl("hdnDocLid");
            LinkButton lnkDownload = (LinkButton)e.Item.FindControl("lnkDownload");
            DataTable dt = ViewState["AnnexureDoc2"] as DataTable;
            OriginalRows = dt.Rows.Count;       // get original rows of grid view

            DataRow[] drr = dt.Select("PkId='" + hdnDocLid.Value + "' "); // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete(); // delete the row

            AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
            ViewState["AnnexureDoc2"] = dt;
            BindGrid();
            if (OriginalRows > AfterDeletedRows)
            {
                lblError.Text = "Successfully Deleted Document.";
                lblError.CssClass = "success";
                rptDocument2.DataBind();
            }
            else
            {
                lblError.Text = "Error while deleting container details. Please try again later..!!";
                lblError.CssClass = "success";
            }
        }
        if (e.CommandName.ToLower().Trim() == "downloadfile")
        {
            LinkButton DownloadPath = (LinkButton)e.Item.FindControl("lnkDownload");
            string FilePath = e.CommandArgument.ToString();
            DownloadDocument(FilePath);
        }
    }

    protected void BindGrid()
    {
        if (ViewState["AnnexureDoc2"].ToString() != "")
        {
            DataTable dtAnnexureDoc = (DataTable)ViewState["AnnexureDoc2"];
            rptDocument2.DataSource = dtAnnexureDoc;
            rptDocument2.DataBind();
        }
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        else
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;

        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception)
        {
        }
    }
    
    protected void AddEnquiry()
    {
        int count = 0,  ExistingCustomerId = 0, CompanyType = 0, LeadId = 0; //OpportunityId = 0, ApprovalTo = 0,
        bool IsOutsideEnq = false;
        string PaymentTerms = "", CustReference = "", Turnover = "", VolumeExpected = "", YearsInService = "", TotalEmp = "", CompanyName = "";
        LeadId = Convert.ToInt32(Session["LeadId"]);

        // validate service entered or not
        if (gvService.Rows.Count > 0)
        {
            for (int c = 0; c < gvService.Rows.Count; c++)
            {
                DropDownList ddlService = (DropDownList)gvService.Rows[c].FindControl("ddlService");
                DropDownList ddlLocation = (DropDownList)gvService.Rows[c].FindControl("ddlLocation");

                if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
                {
                    count++;
                }
            }
        }

        if (count == 0)
        {
            lblError.Text = "Please enter atleast one service offered!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            // get lead details by lid
            DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
            if (dsGetLead != null)
            {
                hdnCompanyType.Value = dsGetLead.Tables[0].Rows[0]["CompanyTypeID"].ToString();
                hdnEmployeeCount.Value = dsGetLead.Tables[0].Rows[0]["EmployeeCount"].ToString();
                hdnTurnover.Value = dsGetLead.Tables[0].Rows[0]["Turnover"].ToString();
                CompanyName = dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString();
            }

            PaymentTerms = txtPaymentTerms.Text.Trim();
            CustReference = txtCustRef.Text.Trim();
            Turnover = hdnTurnover.Value.Trim();
            YearsInService = txtYearsInService.Text.Trim();
            TotalEmp = hdnEmployeeCount.Value.Trim();
            CompanyType = Convert.ToInt32(hdnCompanyType.Value.Trim());

            // add enquiry
            int result_Save = DBOperations.CRM_AddEnquiry(LeadId, hdnEnquiryNo.Value.Trim(), txtNotes.Text.Trim(), IsOutsideEnq, ExistingCustomerId,
                                         PaymentTerms, CustReference, Turnover, YearsInService, TotalEmp, CompanyType, VolumeExpected, LoggedInUser.glUserId);
            if (result_Save > 0)
            {
                // Update Lead Stage
                int LeadStage = DBOperations.CRM_UpdateLeadStatus(LeadId, true, true, true, false, false, false, LoggedInUser.glUserId);

                // update enquiry history status
                int EnquiryStatus = DBOperations.CRM_AddEnquiryHistory(result_Save, 0, "", LoggedInUser.glUserId);

                // add enquiry services
                if (gvService.Rows.Count > 0)
                {
                    for (int c = 0; c < gvService.Rows.Count; c++)
                    {
                        DateTime dtCloseDate = DateTime.MinValue;
                        DropDownList ddlService = (DropDownList)gvService.Rows[c].FindControl("ddlService");
                        DropDownList ddlLocation = (DropDownList)gvService.Rows[c].FindControl("ddlLocation");
                        TextBox txtVolumeExp = (TextBox)gvService.Rows[c].FindControl("txtVolumeExp");
                        //TextBox txtCloseDate = (TextBox)gvService.Rows[c].FindControl("txtCloseDate");
                        TextBox txtRequirement = (TextBox)gvService.Rows[c].FindControl("txtRequirement");

                        //if (txtCloseDate.Text.Trim() != "")
                        //    dtCloseDate = Commonfunctions.CDateTime(txtCloseDate.Text.Trim());

                        if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
                        {
                            int result_Service = DBOperations.CRM_AddLeadService(LeadId, Convert.ToInt32(ddlService.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue),
                                                                    txtVolumeExp.Text.Trim(), txtRequirement.Text.Trim(), dtCloseDate, LoggedInUser.glUserId);
                            if (result_Service > 0)
                            {
                                int AddLeadService = DBOperations.CRM_AddEnquiry_Service(result_Service, Convert.ToInt32(ddlService.SelectedValue), LoggedInUser.glUserId);
                            }
                        }
                    }
                }

                // add rfq quote
                int SavedDraft = QuotationOperations.AddDraftQuotation(Convert.ToInt32(ddlBabajiBranch.SelectedValue), result_Save, 0, 0, CompanyName, "", "", "", "",
                                             "", false, "", "", true, true, false, txtNotes.Text.Trim(), LoggedInUser.glUserId, 0, Convert.ToInt32(hdnKAMId.Value),
                                            Convert.ToInt32(hdnSalesPersonId.Value), txtSalesPerson.Text.Trim());
                if (SavedDraft > 0)
                {
                    #region ADD DOCUMENTS DETAILS

                    if (Convert.ToString(ViewState["AnnexureDoc2"]) != "")
                    {
                        DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
                        if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
                        {
                            string DocPath = "";
                            for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                            {
                                if (dtAnnexure.Rows[i]["DocPath"] != null)
                                    DocPath = dtAnnexure.Rows[i]["DocPath"].ToString();
                                int result_Doc = QuotationOperations.AddQuotationAnnexure(SavedDraft, DocPath, LoggedInUser.glUserId);
                            }
                        }
                    }

                    #endregion

                    // add lead stage history
                    //int result_LeadStatus = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(Session["LeadId"]), 7, "", LoggedInUser.glUserId);
                    int result_LeadStatus = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(Session["LeadId"]), 7, dtClose, "", LoggedInUser.glUserId);
                    int KYCResult = QuotationOperations.UpdateQuoteStatus(SavedDraft, Convert.ToInt32(11), "",dtClose, LoggedInUser.glUserId);

                    //QuotationOperations.UpdateDraftApprovalStatus(SavedDraft, 6, LoggedInUser.glUserId);
                    string EncryptedLeadId = HttpUtility.UrlEncode(Encrypt(Convert.ToString(LeadId)));
                    Response.Redirect("SuccessPage.aspx?Request=" + EncryptedLeadId);
                }
                else
                {
                    lblError.Text = "Error while inserting record. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void btnAddTransportCharges_Click(object sender, EventArgs e)
    {
        Service_NewRow();
    }

    protected void Service_NewRow()
    {
        if (ViewState["Services"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["Services"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {
                //add new row to DataTable   
                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;
                dtCurrentTable.Rows.Add(drCurrentRow);

                //Store the current data to ViewState for future reference   
                ViewState["Services"] = dtCurrentTable;
                for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                {
                    //extract the TextBox values   
                    DropDownList ddlService = (DropDownList)gvService.Rows[i].Cells[1].FindControl("ddlService");
                    DropDownList ddlLocation = (DropDownList)gvService.Rows[i].Cells[2].FindControl("ddlLocation");
                    //TextBox txtCloseDate = (TextBox)gvService.Rows[i].Cells[3].FindControl("txtCloseDate");
                    TextBox txtVolumeExp = (TextBox)gvService.Rows[i].Cells[3].FindControl("txtVolumeExp");
                    TextBox txtRequirement = (TextBox)gvService.Rows[i].Cells[4].FindControl("txtRequirement");

                    dtCurrentTable.Rows[i]["ServiceId"] = ddlService.SelectedValue;
                    dtCurrentTable.Rows[i]["Location"] = ddlLocation.SelectedValue;
                    //dtCurrentTable.Rows[i]["CloseDate"] = txtCloseDate.Text;
                    dtCurrentTable.Rows[i]["VolumeExp"] = txtVolumeExp.Text;
                    dtCurrentTable.Rows[i]["Requirement"] = txtRequirement.Text;
                }
                //Rebind the Grid with the current data to reflect changes   
                gvService.DataSource = dtCurrentTable;
                gvService.DataBind();
            }
        }
        Service_PreviousData();
    }

    protected void Service_PreviousData()
    {
        int rowIndex = 0;
        if (ViewState["Services"] != null)
        {
            DataTable dt = (DataTable)ViewState["Services"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlService = (DropDownList)gvService.Rows[i].Cells[1].FindControl("ddlService");
                    DropDownList ddlLocation = (DropDownList)gvService.Rows[i].Cells[2].FindControl("ddlLocation");
                    //TextBox txtCloseDate = (TextBox)gvService.Rows[i].Cells[3].FindControl("txtCloseDate");
                    TextBox txtVolumeExp = (TextBox)gvService.Rows[i].Cells[3].FindControl("txtVolumeExp");
                    TextBox txtRequirement = (TextBox)gvService.Rows[i].Cells[4].FindControl("txtRequirement");

                    if (i < dt.Rows.Count - 1)
                    {
                        ddlService.DataBind();
                        ddlService.SelectedValue = dt.Rows[i]["ServiceId"].ToString();
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = dt.Rows[i]["Location"].ToString();
                        //if (dt.Rows[i]["CloseDate"].ToString() != "")
                        //    txtCloseDate.Text = Convert.ToDateTime(dt.Rows[i]["CloseDate"]).ToString("dd/MM/yyyy");
                        txtVolumeExp.Text = dt.Rows[i]["VolumeExp"].ToString();
                        txtRequirement.Text = dt.Rows[i]["Requirement"].ToString();
                    }
                    rowIndex++;
                }
            }
        }
    }

    protected void lnkDeleteRow_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["Services"] != null)
        {
            DataTable dt = (DataTable)ViewState["Services"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data and reset row number  
                    dt.Rows.Remove(dt.Rows[rowID]);
                    ResetRowID(dt);
                }
            }

            ViewState["Services"] = dt;
            gvService.DataSource = dt;
            gvService.DataBind();
        }

        Service_PreviousData();
    }

    protected void ResetRowID(DataTable dt)
    {
        int rowNumber = 1;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[0] = rowNumber;
                rowNumber++;
            }
        }
    }

    protected void gvService_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = (DataTable)ViewState["Services"];
            LinkButton lnkDeleteRow = (LinkButton)e.Row.FindControl("lnkDeleteRow");
            if (lnkDeleteRow != null)
            {
                if (dt.Rows.Count > 1)
                {
                    if (e.Row.RowIndex == dt.Rows.Count - 1)
                    {
                        lnkDeleteRow.Visible = false;
                    }
                }
                else
                {
                    lnkDeleteRow.Visible = false;
                }
            }
        }
    }

    protected void Service_InitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ServiceId", typeof(string)));
        dt.Columns.Add(new DataColumn("Location", typeof(string)));
        //dt.Columns.Add(new DataColumn("CloseDate", typeof(string)));
        dt.Columns.Add(new DataColumn("VolumeExp", typeof(string)));
        dt.Columns.Add(new DataColumn("Requirement", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["ServiceId"] = string.Empty;
        dr["Location"] = string.Empty;
        //dr["CloseDate"] = string.Empty;
        dr["VolumeExp"] = string.Empty;
        dr["Requirement"] = string.Empty;
        dt.Rows.Add(dr);

        //Store the DataTable in ViewState for future reference   
        ViewState["Services"] = dt;

        //Bind the Gridview   
        gvService.DataSource = dt;
        gvService.DataBind();
    }
}