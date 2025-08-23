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
using System.Security.Cryptography;


public partial class KYC_New_SuccessKYC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["KycVendorId"] != null)
            {
                if (Convert.ToString(Session["KycVendorId"]) != "")
                {
                    SendApproval(Convert.ToInt32(Session["KycVendorId"]));
                }
            }
        }
    }

    protected bool SendApproval(int VendorId)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilder_Attendee = new StringBuilder();
        StringBuilder strAttendeeEmail = new StringBuilder();
        string EncryptedVendorId = HttpUtility.UrlEncode(Encrypt(VendorId.ToString()));

        if (VendorId > 0)
        {
            string strCompanyName = "", strlType = "", strAddress1 = "", strAddress2 = "", strCity = "", strState = "", strCountry = "", strPincode = "",
                    strConstitution = "", strSector = "", strNOB = "", strWebsiteAddress = "", strCompanyEmail = "", strKYCPath = "", strCustomerEmail = "",
                    strCCEmail = "", strSubject = "", MessageBody = "", strObservers = "", strResources = "", strNotes = "", strCreatedBy = "", strCreatedByMail = "";

            //Get Basic Detail - Title, date, time
            DataSet dsGetMeetingDetail = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetMeetingDetail != null && dsGetMeetingDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetMeetingDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    strCompanyName = dsGetMeetingDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    strlType = dsGetMeetingDetail.Tables[0].Rows[0]["lTypeName"].ToString();
                    strAddress1 = dsGetMeetingDetail.Tables[0].Rows[0]["Address1"].ToString();
                    strAddress2 = dsGetMeetingDetail.Tables[0].Rows[0]["Address2"].ToString();
                    strCity = dsGetMeetingDetail.Tables[0].Rows[0]["City"].ToString();
                    strState = dsGetMeetingDetail.Tables[0].Rows[0]["StateName"].ToString();
                    strCountry = dsGetMeetingDetail.Tables[0].Rows[0]["CountryName"].ToString();
                    strPincode = dsGetMeetingDetail.Tables[0].Rows[0]["Pincode"].ToString();
                    strConstitution = dsGetMeetingDetail.Tables[0].Rows[0]["ConstitutionName"].ToString();
                    strSector = dsGetMeetingDetail.Tables[0].Rows[0]["SectorName"].ToString();
                    strNOB = dsGetMeetingDetail.Tables[0].Rows[0]["NatureofBusinessName"].ToString();
                    strWebsiteAddress = dsGetMeetingDetail.Tables[0].Rows[0]["WebsiteAdd"].ToString();
                    strCompanyEmail = dsGetMeetingDetail.Tables[0].Rows[0]["Email"].ToString();
                    strCustomerEmail = dsGetMeetingDetail.Tables[0].Rows[0]["sEmail"].ToString();
                    strCreatedBy = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedBy"].ToString();
                    strCreatedByMail = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
                }
            }
            else
                return false;

            List<string> lstFilePath = new List<string>();
            DataSet dsGetKYCDetails = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetKYCDetails != null && dsGetKYCDetails.Tables[0].Rows[0]["KYCScannedCopyPath"] != DBNull.Value)
            {
                strKYCPath = "\\KYC\\" + dsGetKYCDetails.Tables[0].Rows[0]["KYCScannedCopyPath"].ToString();
                lstFilePath.Add(strKYCPath);
            }

            // Email Format
            strCustomerEmail = "javed.shaikh@babajishivram.com";
            strCCEmail = ""; //strCreatedByMail;
            strSubject = "KYC Approval for customer " + strCompanyName.Trim().ToUpper();

            if (strCustomerEmail == "" || strSubject == "")
                return false;
            else
            {
                MessageBody = "<BR/>Dear Sir,<BR/><BR/>KYC form has been generated for company named " + strCompanyName.ToUpper().Trim() + ".<BR/>" +
                                "Please give approval for the same by <a href='http://live.babajishivram.com/KYC_New/ApproveKYC.aspx?p=" + EncryptedVendorId + "'> clicking </a> here.";
                MessageBody = MessageBody + "<BR/><BR/>" + strbuilder;
                MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + strCreatedBy + "<BR/>";

                // bEmailSuccess = EMail.SendMail(strCustomerEmail, strCustomerEmail, strSubject, MessageBody, strKYCPath);
                bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                return bEmailSuccess;
            }
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
        catch (Exception en)
        {

        }
        return cipherText;
    }
    #endregion
}