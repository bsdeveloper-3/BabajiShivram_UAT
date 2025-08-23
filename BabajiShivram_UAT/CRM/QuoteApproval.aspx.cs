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

public partial class CRM_QuoteApproval : System.Web.UI.Page
{
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBox txb3 = (TextBox)FormViewEnquiry.FindControl("txtPaymentTerms");
            if (Request.QueryString.ToString() != null && Request.QueryString.Count > 0)
            {
                if (Convert.ToString(Request.QueryString["i"]) != "")
                {
                    hdnLeadId.Value = Convert.ToString(Decrypt(HttpUtility.UrlDecode(Request.QueryString["i"])));
                    hdnUserId.Value = Convert.ToString(Decrypt(HttpUtility.UrlDecode(Request.QueryString["a"])));
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
        btnYes.Visible = true;
        lblMessage.Text = "";
        txtApprovedBy.Text = "";
        lblGeneralMsg.Visible = true;

        DataSet dsGetLeadStatus = DBOperations.CRM_GetLeadById(Convert.ToInt32(hdnLeadId.Value));
        if (dsGetLeadStatus != null)
        {
            if (dsGetLeadStatus.Tables[0].Rows[0]["QuoteStatus"].ToString() != "")
            {
                int QuoteStatusId = Convert.ToInt32(dsGetLeadStatus.Tables[0].Rows[0]["QuoteStatus"].ToString());
                if (QuoteStatusId > 0)
                {
                    if (QuoteStatusId == 12)  // ready for KYC registration
                    {
                        lblMessage.Text = "Quote approved by " + dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        txtApprovedBy.Text = dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        btnYes.Visible = false;
                        lblGeneralMsg.Visible = false;
                        btnNo.Visible = false;
                        txtRemark.Enabled = false;
                        TextBox txb2 = FormViewEnquiry.FindControl("txtPaymentTerms") as TextBox;
                        txb2.Enabled = false;
                    }
                    else if (QuoteStatusId == 13)  // rejected for KYC
                    {
                        lblMessage.Text = "Quote rejected by " + dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        txtApprovedBy.Text = dsGetLeadStatus.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        btnYes.Visible = false;
                        lblGeneralMsg.Visible = false;
                        btnNo.Visible = false;
                        txtRemark.Enabled = false;
                        TextBox txb2 = FormViewEnquiry.FindControl("txtPaymentTerms") as TextBox;
                        txb2.Enabled = false;
                    }
                }
            }
        }
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
        {
            int EnquiryId = Convert.ToInt32(FormViewEnquiry.DataKey.Value.ToString());
            if (EnquiryId > 0)
            {
                int result_EnquiryStatus = DBOperations.ApproveEnquiry(EnquiryId, 1, Convert.ToInt32(hdnUserId.Value));

                DataSet dsGetQuoteId = DBOperations.CRM_GetQuoteByLead(Convert.ToInt32(hdnLeadId.Value));
                if (dsGetQuoteId != null && dsGetQuoteId.Tables.Count > 0)
                {
                    int QuoteId = Convert.ToInt32(dsGetQuoteId.Tables[0].Rows[0]["QuotationId"].ToString());
                    if (QuoteId > 0)
                    {
                        int result = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(12), "",dtClose, Convert.ToInt32(hdnUserId.Value));
                        if (result == 1)
                        {
                            //-----------------CHANGE BY SAYALI 03122019--------------------------------
                            TextBox txb2 = FormViewEnquiry.FindControl("txtPaymentTerms") as TextBox;
                            int Output = DBOperations.CRM_updPaymentTerms(Convert.ToInt32(hdnLeadId.Value), txb2.Text);
                            string message = "Quote successfully approved. KYC mail has been sent for the same";

                            string url = "ApprovalPendingLeads.aspx";
                            string script = "window.onload = function(){ alert('";
                            script += message;
                            script += "');";
                            script += "window.location = '";
                            script += url;
                            script += "'; }";
                           
                            
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
                            SendMailForKYC(Convert.ToInt32(hdnLeadId.Value), EnquiryId);

                            ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script.ToString());

                            Response.Redirect("ApprovalPendingLeads.aspx");

                            //FormView1.DataBind();
                            //FormViewEnquiry.DataBind();
                            //CheckApproval();
                        }
                        else
                        {
                            string script = "<script type = 'text/javascript'>alert('Error while setting quote status! Please try after sometime.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());
                        }
                    }
                }
            }
            else
            {
                string script = "<script type = 'text/javascript'>alert('Lead enquiry does not exists or not found! Please try after sometime.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());
            }
        }
        else
        {
            string script = "<script type = 'text/javascript'>alert('Lead does not exists.');</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());
        }
    }

    protected void FormViewEnquiry_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "downloadquote")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDoc(DocPath);
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            //ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;
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

    protected bool SendMailForKYC(int LeadId, int EnquiryId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", strLeadRefNo = "", strLeadDate = "", strCompanyName = "",
                strService = "", strContactName = "", strMobileNo = "", strCreatedBy = "", strCreatedDate = "", strAddress = "", strAlternatePhone = "", strEmail = "",
                strDesignation = "", strLeadCreatedByEmail = "", EmailContent = "";
        string EncryptedEnquiryId = HttpUtility.UrlEncode(Encrypt(Convert.ToString(EnquiryId)));

        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
        if (dsGetLead != null)
        {
            try
            {
                string strFileName = "../EmailTemplate/KYCRequest.txt";
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

            strSubject = "KYC Request from Babaji Shivram Clearing & Carriers Pvt. Ltd.";

            MessageBody = EmailContent.Replace("@KycLink", "http://live.babajishivram.com/KYC_New/index.aspx?p=" + EncryptedEnquiryId);
            MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
            MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
            MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
            MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
            MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
            MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
            MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

            strCustomerEmail = dsGetLead.Tables[0].Rows[0]["Email"].ToString();
            strCCEmail = dsGetLead.Tables[0].Rows[0]["LeadCreatedByEmail"].ToString() + ", " + " javed.shaikh@babajishivram.com"; //" , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

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

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
        {
            int EnquiryId = Convert.ToInt32(FormViewEnquiry.DataKey.Value.ToString());
            if (EnquiryId > 0)
            {
                int result_EnquiryStatus = DBOperations.ApproveEnquiry(EnquiryId, 0, Convert.ToInt32(hdnUserId.Value));

                DataSet dsGetQuoteId = DBOperations.CRM_GetQuoteByLead(Convert.ToInt32(hdnLeadId.Value));
                if (dsGetQuoteId != null && dsGetQuoteId.Tables.Count > 0)
                {
                    int QuoteId = Convert.ToInt32(dsGetQuoteId.Tables[0].Rows[0]["QuotationId"].ToString());
                    if (QuoteId > 0)
                    {
                        int result = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(13), txtRemark.Text.Trim(),dtClose, Convert.ToInt32(hdnUserId.Value));
                        if (result == 1)
                        {
                            //-----------------CHANGE BY SAYALI 03122019--------------------------------
                            TextBox txb2 = FormViewEnquiry.FindControl("txtPaymentTerms") as TextBox;
                            int Output = DBOperations.CRM_updPaymentTerms(Convert.ToInt32(hdnLeadId.Value), txb2.Text);
                            string message = "Quote successfully rejected.";
                            
                            string script = "window.onload = function(){ alert('";
                            //script += message;
                            script += "');";
                            script += "window.location = '";
                            //script += url;
                            script += "'; }";
                           
                            SendMail(Convert.ToInt32(hdnLeadId.Value), EnquiryId);

                            ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());

                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());

                            Response.Redirect("ApprovalPendingLeads.aspx");
                        }
                        else
                        {
                            string script = "<script type = 'text/javascript'>alert('Error while rejecting quote status! Please try after sometime.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());
                        }
                    }
                }
            }
            else
            {
                string script = "<script type = 'text/javascript'>alert('Lead enquiry does not exists or not found! Please try after sometime.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());
            }
        }
        else
        {
            string script = "<script type = 'text/javascript'>alert('Lead does not exists.');</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script.ToString());
        }
    }

    protected bool SendMail(int LeadId, int EnquiryId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", EmailContent = "";
        string EncryptedEnquiryId = HttpUtility.UrlEncode(Encrypt(Convert.ToString(EnquiryId)));

        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
        if (dsGetLead != null)
        {
            try
            {
                string strFileName = "../EmailTemplate/KYCRejectedEmail.txt";
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

            strSubject = "Quote Approval Rejected!";

            MessageBody = EmailContent.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
            MessageBody = MessageBody.Replace("@Remark", dsGetLead.Tables[0].Rows[0]["QuoteStageRemark"].ToString());

            strCustomerEmail = dsGetLead.Tables[0].Rows[0]["QuoteCreatedBy"].ToString();
            strCCEmail = "javed.shaikh@babajishivram.com" + " , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

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

}