using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class ExportCHA_SBPreparationDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(btnUpload);
        //ScriptManager1.RegisterPostBackControl(GridViewDocument);
        //ScriptManager1.RegisterPostBackControl(FVJobHistory);
        ScriptManager1.RegisterPostBackControl(btnAddChecklistCopy);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Detail";

            if (Session["JobId"] != null)
            {
                JobDetailMS(Convert.ToInt32(Session["JobId"]), Convert.ToString(Session["FinYearId"]));
                //DBOperations.EX_FillChekListDocDetail(ddDocument);
            }
            else
            {
                Response.Redirect("SBPreparation.aspx");
            }
        }
    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        string strReutrnUrl = ((Button)sender).CommandArgument.ToString();
        Response.Redirect(strReutrnUrl);
    }

    #region FORM VIEW EVENTS
    private void JobDetailMS(int JobId, string FinYearId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId);

        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("SBPreparation.aspx");
            Session["JobId"] = null;
        }

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            DataSet dsPerticularJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);

            if (dsPerticularJobDetail.Tables[0].Rows.Count > 0)
            {
                FVJobDetail.DataSource = dsPerticularJobDetail;
                FVJobDetail.DataBind();

                // Checklist Approval Required Check for PreAlert Plant
                bool ChecklistApproval = false;

                if (dsPerticularJobDetail.Tables[0].Rows[0]["ChecklistApproval"] != DBNull.Value)
                    ChecklistApproval = Convert.ToBoolean(dsPerticularJobDetail.Tables[0].Rows[0]["ChecklistApproval"]);

                chkClientApproval.Checked = ChecklistApproval;
                if (ChecklistApproval)
                    chkClientApproval.Text = "Yes";
                else
                    chkClientApproval.Text = "No";

                if (dsPerticularJobDetail.Tables[0].Rows[0]["TransMode"] != DBNull.Value)
                {
                    if (dsPerticularJobDetail.Tables[0].Rows[0]["TransMode"].ToString().Trim().ToLower() == "air")
                    {
                        //TabSeaContainer.Visible = false;
                    }
                    else
                    {
                        //TabSeaContainer.Visible = true;
                    }
                }
            }
        }

        //Get Job History
        //DataView dvHistory = DBOperations.EX_GetJobHistory(JobId);
        //if (dvHistory.Table.Rows.Count > 0)
        //{
        //    FVJobHistory.DataSource = dvHistory;
        //    FVJobHistory.DataBind();
        //}
    }

    //protected void btnEditJob_Click(object sender, EventArgs e)
    //{
    //    FVJobDetail.ChangeMode(FormViewMode.Edit);

    //    if (Session["JobId"] != null)
    //    {
    //        GetJobDetail(Convert.ToInt32(Session["JobId"]));
    //    }
    //}

    //private void GetJobDetail(int JobId)
    //{
    //    DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId, Convert.ToString(Session["FinYearId"]));
    //    if (dsJobDetail.Tables[0].Rows.Count > 0)
    //    {
    //        FVJobDetail.DataSource = dsJobDetail;
    //        FVJobDetail.DataBind();
    //    }
    //}

    #endregion

    #region CHECKLIST COPY UPLOAD EVENTS

    protected void btnAddChecklistCopy_OnClick(object sender, EventArgs e)
    {
        int Result = -123;

        if (chkYes.Checked)
        {
            int JobId = Convert.ToInt32(Session["JobId"]);
            int UserId = Convert.ToInt32(LoggedInUser.glUserId);

            string CheckListPath = "";
            string strFOBValue, strCIFValue, strRemark;

            bool ClientApproval = chkClientApproval.Checked;
            strRemark = txtRemark.Text.Trim();
            strFOBValue = txtFOBValue.Text.Trim();
            strCIFValue = txtCIFValue.Text.Trim();

            DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
            if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
                hdnCheckListPath.Value = dsGetJobDetail.Tables[0].Rows[0]["ChecklistDocPath"].ToString() + "\\" +
                                         dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

            // Checklist Upload
            if (fuChecklist.HasFile)
            {
                CheckListPath = UploadCheckListFiles(hdnCheckListPath.Value);
                Result = EXOperations.EX_AddChecklistDetail(JobId, ClientApproval, CheckListPath, strFOBValue, strCIFValue, strRemark, UserId);
                if (Result == 0)
                {
                    Session["JobId"] = null;
                    Response.Redirect("../Success.aspx?ExpChecklist=4002");
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error!! Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblError.Text = "Checklist already sent for customer approval!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 3)
                {
                    lblError.Text = "Checklist Not Ready For Preparation. Please Release the Hold Lock..!!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 4)
                {
                    lblError.Text = "Invoice Detail Not Found. Please Enter Atleast 1 Invoice Detail..!!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Browse Checklist Copy..!!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Confirm if Checklist is Prepared!";
            lblError.CssClass = "errorMsg";
        }
    }

    public string UploadCheckListFiles(string FilePath)
    {
        string FileName = fuChecklist.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuChecklist.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuChecklist.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
    }

    protected void btnCancelChecklist_Click(object sender, EventArgs e)
    {
        txtCIFValue.Text = "";
        txtFOBValue.Text = "";
        chkClientApproval.Checked = false;
        chkYes.Checked = false;
        txtRemark.Text = "";
    }

    #endregion

    #region INVOICE DETAILS EVENTS

    protected void btnAddInvoice_Click(object sender, EventArgs e)
    {
        int ShipmentTermsId = 0;
        DateTime dtInvoiceDate = DateTime.MinValue;
        DateTime dtLicenseDate = DateTime.MinValue;

        if (txtInvoiceDate.Text.Trim().ToString() != "")
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        if (txtLicenseDate.Text.Trim().ToString() != "")
            dtLicenseDate = Commonfunctions.CDateTime(txtLicenseDate.Text.Trim());
        if (ddlShipmentTerm.SelectedValue != "0")
            ShipmentTermsId = Convert.ToInt32(ddlShipmentTerm.SelectedValue);

        if (txtInvoiceNo.Text.Trim() != "" && dtInvoiceDate != DateTime.MinValue)
        {
            int result = EXOperations.Ex_AddInvoiceDetail(Convert.ToInt32(Session["JobId"]), 0, txtInvoiceNo.Text.Trim(), dtInvoiceDate, txtInvoiceValue.Text.Trim(), ShipmentTermsId,
                txtDBKAmount.Text.Trim(), txtLicenseNo.Text.Trim(), dtLicenseDate, txtFreightAmount.Text.Trim(), txtInsuranceAmount.Text.Trim(), LoggedInUser.glUserId,
                txtInvoiceCurrency.Text.Trim());

            if (result == 0)
            {
                lblresult.Text = "Successfully added invoice details.";
                lblresult.CssClass = "success";
                gvInvoiceDetail.DataBind();
                //gvContainer.DataBind();
                btnCancelInvoice_Click(null, EventArgs.Empty);
            }
            else
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }

        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = "Please enter mandatory fields!!";
        }

    }

    protected void btnCancelInvoice_Click(object sender, EventArgs e)
    {
        txtInvoiceNo.Text = "";
        txtInvoiceDate.Text = "";
        txtInvoiceValue.Text = "";
        ddlShipmentTerm.SelectedValue = "0";
        txtDBKAmount.Text = "";
        txtLicenseDate.Text = "";
        txtLicenseNo.Text = "";
        txtFreightAmount.Text = "";
        txtInsuranceAmount.Text = "";
    }

    protected void gvInvoiceDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int JobId = Convert.ToInt32(gvInvoiceDetail.DataKeys[e.RowIndex].Values[0].ToString());
        int Lid = Convert.ToInt32(gvInvoiceDetail.DataKeys[e.RowIndex].Values[1].ToString());
        int ShipmentTermsId = 0;
        TextBox txtInvoiceNo = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceNo") as TextBox;
        TextBox txtInvoiceDate = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceDate") as TextBox;
        TextBox txtInvoiceValue = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceValue") as TextBox;
        DropDownList ddlShipmentTerm = gvInvoiceDetail.Rows[e.RowIndex].FindControl("ddlShipmentTerm") as DropDownList;
        TextBox txtDBKAmount = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtDBKAmount") as TextBox;
        TextBox txtLicenseNo = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtLicenseNo") as TextBox;
        TextBox txtLicenseDate = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtLicenseDate") as TextBox;
        TextBox txtFreightAmount = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtFreightAmount") as TextBox;
        TextBox txtInsuranceAmount = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInsuranceAmount") as TextBox;
        TextBox txtInvoiceCurrency = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceCurrency") as TextBox;

        DateTime dtInvoiceDate = DateTime.MinValue;
        DateTime dtLicenseDate = DateTime.MinValue;
        if (txtInvoiceDate.Text.Trim().ToString() != "")
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        if (txtLicenseDate.Text.Trim().ToString() != "")
            dtLicenseDate = Commonfunctions.CDateTime(txtLicenseDate.Text.Trim());
        if (ddlShipmentTerm.SelectedValue != "0")
            ShipmentTermsId = Convert.ToInt32(ddlShipmentTerm.SelectedValue);

        if (txtInvoiceNo.Text.Trim() != "" && dtInvoiceDate != DateTime.MinValue)
        {
            int result = EXOperations.Ex_AddInvoiceDetail(JobId, Lid, txtInvoiceNo.Text.Trim(), dtInvoiceDate, txtInvoiceValue.Text.Trim(), ShipmentTermsId,
                txtDBKAmount.Text.Trim(), txtLicenseNo.Text.Trim(), dtLicenseDate, txtFreightAmount.Text.Trim(), txtInsuranceAmount.Text.Trim(), LoggedInUser.glUserId,
                txtInvoiceCurrency.Text.Trim());

            if (result == 0)
            {
                lblresult.Text = "Successfully updated invoice details.";
                lblresult.CssClass = "success";
                gvInvoiceDetail.EditIndex = -1;
                btnCancelInvoice_Click(null, EventArgs.Empty);
                //gvInvoiceDetail.DataBind();
            }
            else
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }

        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = "Please enter mandatory fields!!";
        }
        e.Cancel = true;
    }

    protected void gvInvoiceDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvInvoiceDetail.DataKeys[e.RowIndex].Values[1].ToString());
        int result = EXOperations.EX_DeleteInvoiceDetail(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            e.Cancel = true;
            lblresult.Text = "Invoice Detail Deleted Successfully!";
            lblresult.CssClass = "success";
            gvInvoiceDetail.DataBind();
            btnCancelInvoice_Click(null, EventArgs.Empty);
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void gvInvoiceDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvInvoiceDetail.EditIndex = e.NewEditIndex;
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvInvoiceDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvInvoiceDetail.EditIndex = -1;
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvInvoiceDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvInvoiceDetail.PageIndex = e.NewPageIndex;
        gvInvoiceDetail.DataBind();
    }

    #endregion

    #region DOCUMENT UPLOAD / DOWNLOAD EVENTS

    protected void GridViewDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
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

    //protected void btnUpload_Click(Object Sender, EventArgs e)
    //{
    //    lblError.Visible = true;
    //    string strFilePath = "", filePath = "";
    //    int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);
    //    int JobId = Convert.ToInt32(Session["JobId"]);

    //    DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
    //    if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
    //        strFilePath = dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

    //    if (JobId > 0)
    //    {
    //        if (DocumentId == 0)
    //        {
    //            lblError.Text = "Please Select Document Type!";
    //            lblError.CssClass = "errorMsg";
    //            return;
    //        }

    //        if (fuDocument.FileName.Trim() == "")
    //        {
    //            lblError.Text = "Please Browse The Document!";
    //            lblError.CssClass = "errorMsg";
    //            return;
    //        }

    //        filePath = UploadFiles(fuDocument, strFilePath);
    //        if (filePath != "")
    //        {
    //            int result_DocSaved = EXOperations.Ex_AddPreAlertDocs(filePath, DocumentId, JobId, LoggedInUser.glUserId);
    //            if (result_DocSaved == 0)
    //            {
    //                lblError.Text = "Document uploaded successfully!";
    //                lblError.CssClass = "success";
    //                GridViewDocument.DataBind();
    //                ddDocument.SelectedValue = "0";
    //            }
    //            else if (result_DocSaved == 2)
    //            {
    //                lblError.Text = "Document with same type already exists!!";
    //                lblError.CssClass = "errorMsg";
    //            }
    //            else
    //            {
    //                lblError.Text = "System Error. Please try after sometime!";
    //                lblError.CssClass = "errorMsg";
    //            }
    //        }
    //        else
    //        {
    //            lblError.Text = "Please Select File!";
    //            lblError.CssClass = "errorMsg";
    //        }
    //    }
    //    else
    //    {
    //        Response.Redirect("SBPreparation.aspx");
    //    }
    //}

    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        FileName = FileName.Replace(",", "");

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
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