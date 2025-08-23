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
using System.Security.Cryptography;

public partial class CRM_AddEnquiry : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnEnquiryNo.Value = DBOperations.GetEnquiryRefNo();
        ScriptManager1.RegisterPostBackControl(btnSaveDocument2);
        ScriptManager1.RegisterPostBackControl(gvService);
        ScriptManager1.RegisterPostBackControl(rptDocument2);
        if (!IsPostBack)
        {
            if (Session["LeadId"] == null)
            {
                Response.Redirect("Leads.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Add Enquiry";
                lblError.Text = "";
                QuotationOperations.FillBranchByUser(ddlBabajiBranch, LoggedInUser.glUserId);
                Service_InitialRow();

                DataTable dtAnnexureDoc2 = new DataTable();
                dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
                ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Enquiry.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtKAM.Text = "";
        txtNotes.Text = "";
        txtSalesPerson.Text = "";
        hdnKAMId.Value = "0";
        hdnSalesPersonId.Value = "0";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        btnAddTransportCharges_Click(null, EventArgs.Empty);
        int count = 0, ExistingCustomerId = 0, CompanyType = 0, LeadId = 0; //OpportunityId = 0, ApprovalTo = 0,
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
            lblError.Visible = true;
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
                        TextBox txtCloseDate = (TextBox)gvService.Rows[c].FindControl("txtCloseDate");
                        TextBox txtRequirement = (TextBox)gvService.Rows[c].FindControl("txtRequirement");

                        if (txtCloseDate.Text.Trim() != "")
                            dtCloseDate = Commonfunctions.CDateTime(txtCloseDate.Text.Trim());

                        if (ddlService.SelectedValue != "0" && ddlLocation.SelectedValue != "0")
                        {
                            int result_Service = DBOperations.CRM_AddLeadService(LeadId, Convert.ToInt32(ddlService.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue),
                                                                    "", txtRequirement.Text.Trim(), dtCloseDate, LoggedInUser.glUserId);
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
                    DateTime dtClose = DateTime.MinValue;
                    // add lead stage history
                    //int result_LeadStatus = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(Session["LeadId"]), 7, "", LoggedInUser.glUserId);
                    int result_LeadStatus = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(Session["LeadId"]), 7,dtClose, "", LoggedInUser.glUserId);
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

    #region Documnet Upload/Download/Delete
    protected void DownloadDoc(string DocumentPath)
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

    protected void btnSaveDocument2_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (fuDocument2 != null && fuDocument2.HasFile)
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

    protected string UploadFiles(string GetFileName)
    {
        string FileName = GetFileName;
        FileName = FileName.Replace(".", "");

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FileName;

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = ".pdf";
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }
            return FileName;
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

    protected void BindGrid()
    {
        if (ViewState["AnnexureDoc2"].ToString() != "")
        {
            DataTable dtAnnexureDoc = (DataTable)ViewState["AnnexureDoc2"];
            rptDocument2.DataSource = dtAnnexureDoc;
            rptDocument2.DataBind();
        }
    }

    #endregion

    #region Services
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
        dt.Columns.Add(new DataColumn("CloseDate", typeof(string)));
        dt.Columns.Add(new DataColumn("VolumeExp", typeof(string)));
        dt.Columns.Add(new DataColumn("Requirement", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["ServiceId"] = string.Empty;
        dr["Location"] = string.Empty;
        dr["CloseDate"] = string.Empty;
        dr["VolumeExp"] = string.Empty;
        dr["Requirement"] = string.Empty;
        dt.Rows.Add(dr);

        //Store the DataTable in ViewState for future reference   
        ViewState["Services"] = dt;

        //Bind the Gridview   
        gvService.DataSource = dt;
        gvService.DataBind();
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
                    TextBox txtCloseDate = (TextBox)gvService.Rows[i].Cells[3].FindControl("txtCloseDate");
                    TextBox txtVolumeExp = (TextBox)gvService.Rows[i].Cells[4].FindControl("txtVolumeExp");
                    TextBox txtRequirement = (TextBox)gvService.Rows[i].Cells[5].FindControl("txtRequirement");

                    dtCurrentTable.Rows[i]["ServiceId"] = ddlService.SelectedValue;
                    dtCurrentTable.Rows[i]["Location"] = ddlLocation.SelectedValue;
                    dtCurrentTable.Rows[i]["CloseDate"] = txtCloseDate.Text;
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
                    TextBox txtCloseDate = (TextBox)gvService.Rows[i].Cells[3].FindControl("txtCloseDate");
                    TextBox txtVolumeExp = (TextBox)gvService.Rows[i].Cells[4].FindControl("txtVolumeExp");
                    TextBox txtRequirement = (TextBox)gvService.Rows[i].Cells[5].FindControl("txtRequirement");

                    if (i < dt.Rows.Count - 1)
                    {
                        ddlService.DataBind();
                        ddlService.SelectedValue = dt.Rows[i]["ServiceId"].ToString();
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = dt.Rows[i]["Location"].ToString();
                        if (dt.Rows[i]["CloseDate"].ToString() != "")
                            txtCloseDate.Text = Convert.ToDateTime(dt.Rows[i]["CloseDate"]).ToString("dd/MM/yyyy");
                        txtVolumeExp.Text = dt.Rows[i]["VolumeExp"].ToString();
                        txtRequirement.Text = dt.Rows[i]["Requirement"].ToString();
                    }
                    rowIndex++;
                }
            }
        }
    }

    protected void btnAddTransportCharges_Click(object sender, EventArgs e)
    {
        Service_NewRow();
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

    #endregion

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
}