using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_Vendor_ApproveReject : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vendor Pending Approval";
            BindVendorDetails();
            int KYCId = Convert.ToInt32(Session["KYCAPRID"]);
            hdnVendorKycId.Value = KYCId.ToString();
            VendorKYCOps.FillVendorKYCDOCMS(ddDocument);

        }
    }
    private void BindVendorDetails()
    {
        DataSourceDocDownload.DataBind();
        GridViewDoc.DataBind();
        DataSourceApprovalHistory.DataBind();
        gvApprovalHistory.DataBind();
    }
    protected void GetVendorDetails(int VendorKYCID)
    {
        DataView dtKYCDetail = VendorKYCOps.GetKYCDetailById(VendorKYCID);

        if (dtKYCDetail.Table.Rows.Count > 0)
        {
            hdnUploadPath.Value = dtKYCDetail.Table.Rows[0]["FileDirName"].ToString();

        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int ApprovalStatusID = (Int32)EnumVendorKYCStatus.KYC_Approved;

        int VendorKycId = Convert.ToInt32(hdnVendorKycId.Value); // Retrieve the VendorKycId

        string remark = txtremark.Text.Trim();
        string CompanyCode = txtcompanycode.Text.Trim(); //Added company code to vendor
        if (string.IsNullOrEmpty(remark))
        {
            lblError.Text = "Please Enter an Approval Remark!";
            return;
        }

        int approvalResult = VendorKYCOps.AddKYCStatus(VendorKycId, ApprovalStatusID, remark, CompanyCode, loggedInUser.glUserId); 
        
        if (approvalResult == 0)
        {
            lblError.Text = "Vendor Details approved successfully!";
            lblError.CssClass = "success";

            Session["VendorKYCSuccess"] = lblError.Text;

            Response.Redirect("ServiceSuccess.aspx");
        }
        else
        {
            lblError.Text = "Error approving vendor. Please try again.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        int RejectedStatusId = (Int32)EnumVendorKYCStatus.KYC_Reject;

        int VendorKycId = Convert.ToInt32(hdnVendorKycId.Value); // Retrieve the VendorKycId

        string remark = txtremark.Text.Trim();
        string CompanyCode = txtcompanycode.Text.Trim(); //Added company code to vendor
        if (string.IsNullOrEmpty(remark))
        {
            lblError.Text = "Please Enter an Rejection Remark!";
            return;
        }

        int rejectResult = VendorKYCOps.AddKYCStatus(VendorKycId, RejectedStatusId, remark, CompanyCode, loggedInUser.glUserId);

        if (rejectResult == 0)
        {
            lblError.Text = "Vendor Details rejected successfully!";
            lblError.CssClass = "success";

            Session["VendorKYCSuccess"] = lblError.Text;

            Response.Redirect("ServiceSuccess.aspx");
        }
        else
        {
            lblError.Text = "Error Rejection vendor. Please try again.";
            lblError.CssClass = "errorMsg";
        }
    }
  
    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("VendorKYC_Approval.aspx");
    }
    //protected void GridViewDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Download")
    //    {
    //        string docPath = e.CommandArgument.ToString();
    //        DownloadDocument(docPath);
    //    }
    //}
    protected void GridViewDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            string docPath = e.CommandArgument.ToString();

            /////////////////////////////
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = VendorKYCOps.GetVendorKYCDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                lblError.Text = "";

                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + "\\" + strFileName;
                DownloadDocument(strDocpath);
            }

        }
    }

    #region Document
    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
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
            lblError.Text = "Download Document System Error!";
        }
    }
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            string strURL = "ViewDoc.aspx?ref= " + DocumentPath;

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        int VendorKYCID = Convert.ToInt32(Session["KYCAPRID"]);
        int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);

        GetVendorDetails(VendorKYCID);

        string strFilePath = hdnUploadPath.Value;

        if (strFilePath == "")
            strFilePath = "Other";

        string strKYCFilePath = "VendorKYC//" + strFilePath + "//";

        int Result = -123;
        string strFileName = "";

        if (fuDocument.FileName.Trim() == "")
        {
            lblError.Text = "Please Browse The Document!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (DocumentId == 0)
        {
            lblError.Text = "Please Select Document Type!";
            lblError.CssClass = "errorMsg";
            return;
        }

        strFileName = UploadDocument(strKYCFilePath);

        if (strFileName != "")
        {
            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strFileName, strKYCFilePath, DocumentId, loggedInUser.glUserId);
            Result = 0;
        }
        else
        {
            Result = 1;
        }

        if (Result == 0)
        {
            lblError.Text = "Document uploaded successfully!";
            lblError.CssClass = "success";
            GridViewDoc.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "Please Select File!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Document not uploaded. Please try again!";
            lblError.CssClass = "errorMsg";
        }
    }

    private string UploadDocument(string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + FilePath);
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
        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FileName;
        }

        else
        {
            return "";
        }

    }
    public string RandomString(int size)
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