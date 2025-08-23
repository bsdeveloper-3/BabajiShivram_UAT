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

public partial class CRM_CustEnquiry : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        hdnEnquiryNo.Value = DBOperations.GetEnquiryRefNo();
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(btnCancel);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument2);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Existing Customer Enquiry";
            Service_InitialRow();

            DataTable dtAnnexureDoc2 = new DataTable();
            dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
            ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int count = 0;
        if (hdnCustId.Value != "0" && hdnCustId.Value != "")
        {
            int CustomerId = 0;
            string EnquiryNo = "", PaymentTerms = "",LeadId="0";

            //DataSet dsLeadID = new DataSet(); 
            //dsLeadID = DBOperations.GetCRMLeadID(txtCustomer.Text.ToString(), LoggedInUser.glUserId); // added by sayali on05-11-2019
            //if (dsLeadID.Tables[0].Rows.Count>0)
            //{
            //    foreach (DataRow row in dsLeadID.Tables[0].Rows)
            //    {
            //        LeadId = row["lid"].ToString();
            //    }
            //}
            //else
            //{
            //    LeadId = "0";
            //}

            if (gvService.Rows.Count == 1)
            {
                lblError.Text = "Please enter atleast one service!";
                lblError.CssClass = "errorMsg";
                count = -123;
            }

            if (count == 0)
            {

                int CompanyId = 0, LeadStageId = 6, LeadSourceId = 0, SectorId = 0, RoleId = 1, CompanyTypeId = 0, CategoryId = 0;
                string SourceDesc = "", Turnover = "", EmployeeCount = "", ContactName = "", Designation = "", Email = "", MobileNo = "";
                DateTime dtClose = DateTime.MinValue;
                int result = DBOperations.CRM_AddLead(CompanyId, LeadStageId, LeadSourceId, SectorId, RoleId, CompanyTypeId, CategoryId,
                    SourceDesc, Turnover, EmployeeCount, ContactName, Designation, Email, MobileNo, LoggedInUser.glUserId);

                // Update Lead Stage
                int LeadStage = DBOperations.CRM_UpdateLeadStatus(result, true, false, false, false, false, false, LoggedInUser.glUserId);

                // add lead stage history
                int result_History = DBOperations.CRM_AddLeadStageHistory(result, LeadStageId, dtClose, "", LoggedInUser.glUserId);


                EnquiryNo = DBOperations.GetEnquiryRefNo();
                CustomerId = Convert.ToInt32(hdnCustId.Value);
                PaymentTerms = txtPaymentTerms.Text.Trim();

                int result_Save = DBOperations.CRM_AddEnquiry(Convert.ToInt32(LeadId), EnquiryNo, txtNotes.Text.Trim(), true, CustomerId, PaymentTerms, "", "", "", "", 0, "", LoggedInUser.glUserId);
                if (result_Save > 0)
                {
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
                                int result_Service = DBOperations.CRM_AddEnquiryService(Convert.ToInt32(LeadId), result_Save, Convert.ToInt32(ddlService.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue),
                                                                        "", txtRequirement.Text.Trim(), dtCloseDate, LoggedInUser.glUserId);
                            }
                        }
                    }

                    Session["EnquiryId"] = result_Save.ToString();
                    string message = "Successfully added enquiry.";
                    string url = "CustQuote.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += message;
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                }
                else if (result_Save == -123)
                {
                    lblError.Text = "Enquiry already exists with same ref no.";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while adding up enquiry! Try after sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please enter atleast one service!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please select customer!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Leads.aspx");
    }

    protected void ResetControls()
    {
        txtNotes.Text = "";
        txtCustomer.Text = "";
        hdnCustId.Value = "0";
    }

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
        catch (Exception ex)
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
        catch (Exception ex)
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


   
}