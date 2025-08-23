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

public partial class Service_VendorKYCDetail : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager2.RegisterPostBackControl(btnUpload);
        ScriptManager2.RegisterPostBackControl(GridViewDoc);
        
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vendor KYC Detail";
            VendorKYCOps.FillVendorKYCDOCMS(ddDocument);

            //BindVendorDetails();
            int lId = Convert.ToInt32(Session["ViewvendorKYCID"]);
            //VendorAprroveDetails(lId);

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
        //if (dtKYCDetail.Table.Rows.Count > 0)
        //{

        //    lblvendors.Text = dtKYCDetail.Table.Rows[0]["VendorName"].ToString();

        //    lblContactPerson.Text = dtKYCDetail.Table.Rows[0]["ContactPerson"].ToString();
        //    lblContactNo.Text = dtKYCDetail.Table.Rows[0]["ContactNo"].ToString();
        //    lblLegalName.Text = dtKYCDetail.Table.Rows[0]["LegalName"].ToString();
        //    lblTradeName.Text = dtKYCDetail.Table.Rows[0]["TradeName"].ToString();
        //    lblAddress.Text = dtKYCDetail.Table.Rows[0]["Address"].ToString();
        //    lblOfficeTelephone.Text = dtKYCDetail.Table.Rows[0]["OfficeTelephone"].ToString();
        //    lblEmail.Text = dtKYCDetail.Table.Rows[0]["Email"].ToString();
        //    lblCreditDays.Text = dtKYCDetail.Table.Rows[0]["CreditDays"].ToString();
        //    lblGSTN.Text = dtKYCDetail.Table.Rows[0]["GSTN"].ToString();
        //    GSTRegType.Text = dtKYCDetail.Table.Rows[0]["GSTRegTypeId"].ToString();
        //    //   lblLocation.Text = AVDetail.Table.Rows[0]["Location"].ToString();
        //    lblDivision.Text = dtKYCDetail.Table.Rows[0]["Division"].ToString();
        //    lblKAM.Text = dtKYCDetail.Table.Rows[0]["KAM"].ToString();
        //    lblHOD.Text = dtKYCDetail.Table.Rows[0]["HOD"].ToString();
        //    lblCCM.Text = dtKYCDetail.Table.Rows[0]["CCM"].ToString();
        //    lblMSME.Text = dtKYCDetail.Table.Rows[0]["MSME"].ToString();
        //    lblTDS.Text = dtKYCDetail.Table.Rows[0]["TDS"].ToString();
        //    lblBankName.Text = dtKYCDetail.Table.Rows[0]["BankName"].ToString();
        //    AcctNo.Text = dtKYCDetail.Table.Rows[0]["AccountNo"].ToString();
        //    lblIFSC.Text = dtKYCDetail.Table.Rows[0]["IFSC"].ToString();
        //    lblMICR.Text = dtKYCDetail.Table.Rows[0]["MICR"].ToString();
        //    //  lblBranchName.Text = AVDetail.Table.Rows[0]["BankBranchName"].ToString();
        //    lblBranchCode.Text = dtKYCDetail.Table.Rows[0]["BranchCode"].ToString();
        //    //   lblBankAddress.Text = AVDetail.Table.Rows[0]["BankAddress"].ToString();
        //    //   lblNineDigitCode.Text = AVDetail.Table.Rows[0]["BankNineDigitCode"].ToString();
        //    lblAccountType.Text = dtKYCDetail.Table.Rows[0]["AccountType"].ToString();
        //    // lblYesNo.Text = AVDetail.Table.Rows[0]["Yes"].ToString();
        //    lblcountry.Text = dtKYCDetail.Table.Rows[0]["Country"].ToString();
        //    lblstate.Text = dtKYCDetail.Table.Rows[0]["State"].ToString();
        //    lblcity.Text = dtKYCDetail.Table.Rows[0]["City"].ToString();
        //    lblpincode.Text = dtKYCDetail.Table.Rows[0]["Pincode"].ToString();
        //    lblpan.Text = dtKYCDetail.Table.Rows[0]["PanNo"].ToString();
        //    lblvendorcat.Text = dtKYCDetail.Table.Rows[0]["VendorCategory"].ToString();

        //}

    }

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

                strDocpath = strFilePath +"\\" + strFileName;
                DownloadDocument(strDocpath);
            }

        }
    }

    //protected void DownloadDocument(string ExeCertificatePath)
    //{
    //    string RandomString = "";
    //    string ServerPath = FileServer.GetFileServerDir();
    //    if (ServerPath == "")
    //        ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\VendorDoc\\" + RandomString + "\\" + ExeCertificatePath);
    //    else
    //        ServerPath = ServerPath + ExeCertificatePath;
    //    try
    //    {
    //        System.Web.HttpResponse response = Page.Response;
    //        FileDownload.Download(response, ServerPath, ExeCertificatePath);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

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
        int VendorKYCID = Convert.ToInt32(Session["ViewvendorKYCID"]);
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