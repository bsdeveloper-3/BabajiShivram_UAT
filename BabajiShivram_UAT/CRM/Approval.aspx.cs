using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;

public partial class Forms_Approval : System.Web.UI.Page
{
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.ToString() != null && Request.QueryString.Count > 0)
            {
                if (Convert.ToString(Request.QueryString["i"]) != "")
                {
                    hdnLeadId.Value = Convert.ToString(Request.QueryString["i"]);
                    hdnUserId.Value = Convert.ToString(Request.QueryString["a"]);
                    if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
                    {
                        FormView1.DataBind();
                        FormViewEnquiry.DataBind();
                        CheckApproval();
                    }
                }
            }
        }
    }

    protected void CheckApproval()
    {
        txtRemark.Visible = true;
        btnYes.Visible = true;
        lblMessage.Text = "";

        DataSet dsGetQuoteId = DBOperations.CRM_GetQuoteByLead(Convert.ToInt32(hdnLeadId.Value));
        if (dsGetQuoteId != null && dsGetQuoteId.Tables.Count > 0)
        {
            int QuoteId = Convert.ToInt32(dsGetQuoteId.Tables[0].Rows[0]["StatusId"].ToString());
            if (QuoteId > 0)
            {
                int KYCResult = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(12), "",dtClose, Convert.ToInt32(hdnUserId.Value));
            }
        }

        DataSet dsGetLeadStatus = DBOperations.CRM_GetLeadById(Convert.ToInt32(hdnLeadId.Value));
        if (dsGetLeadStatus != null)
        {
            if (dsGetLeadStatus.Tables[0].Rows[0]["LeadStageId"].ToString() != "")
            {
                int LeadStageId = Convert.ToInt32(dsGetLeadStatus.Tables[0].Rows[0]["LeadStageId"].ToString());
                if (LeadStageId > 0)
                {
                    if (LeadStageId == 5) // MGTMT approval pending
                    {

                    }
                    else if (LeadStageId == 7)  // ready for quote
                    {
                        lblMessage.Text = "Lead already been approved by " + dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();

                        txtApprovedBy.Text = dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        txtRemark.Visible = false;
                        btnYes.Visible = false;
                    }
                    else if (LeadStageId == 10)  // rejected
                    {
                        lblMessage.Text = "Lead already been rejected by " + dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();

                        txtApprovedBy.Text = dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        txtRemark.Visible = false;
                        btnYes.Visible = false;
                    }
                }
            }
        }
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
        {
            DateTime dtClose = DateTime.MinValue;
            int result_History = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(hdnLeadId.Value), 6, dtClose, txtRemark.Text.Trim(), Convert.ToInt32(hdnUserId.Value));
            int EnquiryId = Convert.ToInt32(FormViewEnquiry.DataKey.Value.ToString());
            if (EnquiryId > 0)
            {
                int result = DBOperations.CRM_AddEnquiryHistory(EnquiryId, 7, txtRemark.Text.Trim(), Convert.ToInt32(hdnUserId.Value));
                if (result == 0)
                {
                    DataSet dsGetQuoteId = DBOperations.CRM_GetQuoteByLead(Convert.ToInt32(hdnLeadId.Value));
                    if (dsGetQuoteId != null && dsGetQuoteId.Tables.Count > 0)
                    {
                        int QuoteId = Convert.ToInt32(dsGetQuoteId.Tables[0].Rows[0]["QuotationId"].ToString());
                        if (QuoteId > 0)
                        {
                            int KYCResult = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(12), "",dtClose, Convert.ToInt32(hdnUserId.Value));
                            int result_QuoteHistory = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(hdnLeadId.Value), 7, dtClose, txtRemark.Text.Trim(), Convert.ToInt32(hdnUserId.Value));
                        }
                    }

                    string script = "<script type = 'text/javascript'>alert('Enquiry successfully approved. KYC mail has been sent for the same.');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
                    SendMailForKYC(Convert.ToInt32(hdnLeadId.Value));
                }
                else if (result == 2)
                {
                    string script = "<script type = 'text/javascript'>alert('Enquiry does not exists.');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
                }
                else
                {
                    string script = "<script type = 'text/javascript'>alert('Error while approving the enquiry. Please try again later.');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
                }
            }
            else
            {
                string script = "<script type = 'text/javascript'>alert('Error while approving the enquiry. Please try again later.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
            }
        }
        else
        {
            string script = "<script type = 'text/javascript'>alert('Lead does not exists.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
        }
    }

    protected void btnNo_OnClick(object sender, EventArgs e)
    {

    }

    protected bool SendMailForKYC(int LeadId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", strLeadRefNo = "", strLeadDate = "", strCompanyName = "",
                strService = "", strContactName = "", strMobileNo = "", strCreatedBy = "", strCreatedDate = "", strAddress = "", strAlternatePhone = "", strEmail = "",
                strDesignation = "", strLeadCreatedByEmail = "", EmailContent = "";
        string EncryptedEnquiryId = HttpUtility.UrlEncode(Encrypt(Convert.ToString(FormViewEnquiry.DataKey.Value.ToString())));

        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
        if (dsGetLead != null)
        {
            try
            {
                string strFileName = "../EmailTemplate/KYCRequest.txt";
                //string strFileName = "../EmailTemplate/KYCRequest.html";
                StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                sr = File.OpenText(Server.MapPath(strFileName));
                EmailContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }

            MessageBody = EmailContent.Replace("@KycLink", "http://live.babajishivram.com/KYC_New/index.aspx?p=" + EncryptedEnquiryId);
            MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
            MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
            MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
            MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
            MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
            MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
            MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

            strSubject = "KYC Request from Babaji Shivram Clearing & Carriers Pvt. Ltd.";
            strCustomerEmail = "javed.shaikh@babajishivram.com";
            strCCEmail = ""; //"kivisha.jain@babajishivram.com , javed.shaikh@babajishivram.com"; //" , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

            if (strCustomerEmail == "" || strSubject == "")
                return false;
            else
            {
                List<string> lstFileDoc = new List<string>();
                bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFileDoc);
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