using System;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.IO;

public partial class KYC_New_ApproveKYC : System.Web.UI.Page
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

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int VendorId = 0;
        if (hdnVendorId.Value != "0" && hdnVendorId.Value != "")
        {
            VendorId = Convert.ToInt32(hdnVendorId.Value);
        }

        if (VendorId > 0)
        {
            int SaveKYC = KYCOperation.ApproveRejectKYC(VendorId, 1, "");
            if (SaveKYC == 0)
            {
                Session["KycVendorId"] = hdnVendorId.Value;
                Response.Redirect("SuccessApprove.aspx?op=1");
            }
            else
            {
                string script = "<script type=\"text/javascript\">alert('Error while approving KYC. Please try once again.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
            }
        }
        else
        {
            string script = "<script type=\"text/javascript\">alert('Error while approving KYC. Please try once again.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {

        int VendorId = 0;
        if (hdnVendorId.Value != "0" && hdnVendorId.Value != "")
        {
            VendorId = Convert.ToInt32(hdnVendorId.Value);
        }

        if (VendorId > 0)
        {
            int SaveKYC = KYCOperation.ApproveRejectKYC(VendorId, 0, txtRemark.Text.Trim());
            if (SaveKYC == 0)
            {
                Session["KycVendorId"] = hdnVendorId.Value;
                Response.Redirect("SuccessApprove.aspx?op=0");
            }
            else
            {
                string script = "<script type=\"text/javascript\">alert('Error while approving KYC. Please try once again.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
            }
        }
        else
        {
            string script = "<script type=\"text/javascript\">alert('Error while approving KYC. Please try once again.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
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
                    hdnKYCCopyPath.Value = dsGetVendorDetails.Tables[0].Rows[0]["KYCScannedCopyPath"].ToString();
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

    protected void imgbtnKYCCopy_Click(object sender, ImageClickEventArgs e)
    {
        if (hdnKYCCopyPath.Value != "")
        {
            DownloadDoc(hdnKYCCopyPath.Value);
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
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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