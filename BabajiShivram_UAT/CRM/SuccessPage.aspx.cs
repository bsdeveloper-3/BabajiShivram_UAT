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
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

public partial class CRM_SuccessPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Success Page";

        if (!IsPostBack)
        {
            if (Request.QueryString["Request"] != null)
            {
                string LeadId = Decrypt(HttpUtility.UrlDecode(Request.QueryString["Request"]));
                MGMTApproval(LeadId);
            }
            else if (Request.QueryString["Bill"] != null)
            {
                lblMessage.Text = "Bill Detail Added Successfully. Truck Request Moved to Bill Received Tab.";
            }
            else if (Request.QueryString["Expense"] != null)
            {
                lblMessage.Text = "Successfully added vehicle expense.";
            }
            else if (Request.QueryString["QuoteApp"] != null)
            {
                lblMessage.Text = "Quotation has been forwarded to concerned authority for approval..!!";
            }

        }
    }

    protected void MGMTApproval(string LeadId)
    {
        lblMessage.Text = "Successfully added RFQ enquiry. Mail has been sent to management for approval.";

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
                    strCustomerEmail = "dhaval@babajishivram.com";
                    UserId = 3;
                }
                else
                {
                    strCustomerEmail = "kirti@babajishivram.com";
                    UserId = 189;
                }

                bool bSentMail = SendEmail(Convert.ToInt32(LeadId), UserId, strCustomerEmail);
                if (bSentMail == true)
                {
                    lblMessage.Text = "Successfully updated status for Quotation. Quote has been sent for management approval.";
                    lblMessage.CssClass = "success";
                    break;
                }
            }
        }
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
                    lblMessage.Text = ex.Message;
                    lblMessage.CssClass = "errorMsg";
                }

                MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/LeadApproval.aspx?a=" + EncryptedUserId + "&i=" + EncryptedLId);
                //MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/LeadApproval.aspx?a=" + UserId.ToString() + "&i=" + dsGetLead.Tables[0].Rows[0]["lid"].ToString());
                //MessageBody = MessageBody.Replace("@b", "http://live.babajishivram.com/CRM/LeadRejected.aspx?a=" + UserId.ToString() + "&i=" + dsGetLead.Tables[0].Rows[0]["lid"].ToString());
                MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
                MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
                MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
                MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
                MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
                MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
                MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

                strSubject = "Pending Lead Approval";
                strCustomerEmail = "dhaval@babajishivram.com";
                strCCEmail = "javed.shaikh@babajishivram.com"; // + " , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

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
        catch (Exception en)
        {

        }
        return cipherText;
    }
    #endregion
}