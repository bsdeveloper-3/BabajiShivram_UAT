using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
using System.Security.Cryptography;


public partial class KYC_New_SaveKYC : System.Web.UI.Page
{
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.ToString() != null && Request.QueryString.Count > 0)
            {
                if (Convert.ToString(Request.QueryString["p"]) != "")
                {
                    hdnVendorId.Value = Convert.ToString(Decrypt(HttpUtility.UrlDecode(Request.QueryString["p"])));
                    GetData(Convert.ToInt32(hdnVendorId.Value));
                }
            }
        }
    }

    protected void GetData(int VendorId)
    {
        if (VendorId > 0)
        {
            DataSet dsGetVendorDetails = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetVendorDetails != null && dsGetVendorDetails.Tables[0].Rows.Count > 0)
            {
                if (dsGetVendorDetails.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    #region GENERAL DETAILS
                    txtGeneral_CompanyName.Text = dsGetVendorDetails.Tables[0].Rows[0]["CompanyName"].ToString();
                    ddlGeneral_KYCType.SelectedValue = dsGetVendorDetails.Tables[0].Rows[0]["lType"].ToString();
                    txtGeneral_CorporateAddress1.Text = dsGetVendorDetails.Tables[0].Rows[0]["Address1"].ToString();
                    txtGeneral_CorporateAddress2.Text = dsGetVendorDetails.Tables[0].Rows[0]["Address2"].ToString();
                    txtGeneral_City.Text = dsGetVendorDetails.Tables[0].Rows[0]["City"].ToString();
                    ddlGeneral_State.SelectedValue = dsGetVendorDetails.Tables[0].Rows[0]["StateId"].ToString();
                    ddlCountry.SelectedValue = dsGetVendorDetails.Tables[0].Rows[0]["CountryId"].ToString();
                    txtTelephoneNo.Text = dsGetVendorDetails.Tables[0].Rows[0]["TelephoneNo"].ToString();
                    DisableControls();
                    #endregion
                }
            }
        }
    }

    protected void DisableControls()
    {
        txtGeneral_City.Enabled = false;
        txtGeneral_CompanyName.Enabled = false;
        txtGeneral_CorporateAddress1.Enabled = false;
        txtGeneral_CorporateAddress2.Enabled = false;
        ddlCountry.Enabled = false;
        ddlGeneral_KYCType.Enabled = false;
        ddlGeneral_State.Enabled = false;
        txtTelephoneNo.Enabled = false;
    }

    #region KYC PRINT COPY EVENTS

    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
        }
        else
        {
            ServerFilePath = ServerFilePath + "\\KYC\\" + FilePath;
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

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\KYC\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "\\KYC\\" + DocumentPath;
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

    #endregion

    protected void btnSaveKYC_Click(object sender, EventArgs e)
    {
        if (fuUploadKYC.HasFile)
        {
            int VendorId = 0;
            string strKYCPath = "", strFilePath = "";
            if (hdnVendorId.Value != "" && hdnVendorId.Value != "0")
            {
                VendorId = Convert.ToInt32(hdnVendorId.Value);
                DataSet dsGetKYCDetails = KYCOperation.GetVendorDetailById(VendorId);
                if (dsGetKYCDetails != null && dsGetKYCDetails.Tables[0].Rows[0]["CompanyName"] != DBNull.Value)
                {
                    strFilePath = dsGetKYCDetails.Tables[0].Rows[0]["CompanyName"].ToString().ToUpper().Substring(0, 5) + "\\";
                }
                strKYCPath = UploadFiles(fuUploadKYC, strFilePath);
                if (strKYCPath != "")
                {
                    int SaveKYC = KYCOperation.UpdateScannedKYCPath(VendorId, strKYCPath);
                    if (SaveKYC == 0)
                    {
                        Session["KycVendorId"] = hdnVendorId.Value;
                        Response.Redirect("SuccessKYC.aspx");
                    }
                    else
                    {
                        string script = "<script type=\"text/javascript\">alert('Please browse the KYC Copy.');</script>";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
                    }
                }
            }
        }
        else
        {
            string script = "<script type=\"text/javascript\">alert('Please browse the KYC Copy.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
        }
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
        catch (Exception en)
        {

        }
        return cipherText;
    }
    #endregion
}