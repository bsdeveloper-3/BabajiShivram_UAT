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

public partial class CRM_NewLeadApproval : System.Web.UI.Page
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
                    hdnLeadId.Value = Convert.ToString(Decrypt(HttpUtility.UrlDecode(Request.QueryString["i"])));
                    hdnUserId.Value = Convert.ToString(Decrypt(HttpUtility.UrlDecode(Request.QueryString["a"])));
                    if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
                    {
                        FormView1.DataBind();
                        //FormViewEnquiry.DataBind();
                        CheckApproval();
                    }
                }
            }
        }
    }

    protected void CheckApproval()
    {
        DataSet dsGetApprovalHistory = DBOperations.CRM_GetLeadStageHistory(Convert.ToInt32(hdnLeadId.Value));
        if (dsGetApprovalHistory != null)
        {
            if (dsGetApprovalHistory.Tables.Count > 0)
            {
                if (dsGetApprovalHistory.Tables[0].Rows.Count > 0)
                {
                    txtRemark.Visible = true;
                    btnYes.Visible = true;
                    lblMessage.Text = "";
                    int StageId = Convert.ToInt32(dsGetApprovalHistory.Tables[0].Rows[0]["StageId"].ToString());
                    int IsApproved = Convert.ToInt32(dsGetApprovalHistory.Tables[0].Rows[0]["IsApproved"].ToString());
                    if (StageId > 0)
                    {
                        if (StageId == 6) // approved
                        {
                            lblMessage.Text = "Lead already been approved by " + dsGetApprovalHistory.Tables[0].Rows[0]["UpdatedBy"].ToString();
                            txtRemark.Visible = false;
                            btnYes.Visible = false;
                            btnNo.Visible = false;
                        }
                        else if (StageId == 10)
                        {
                            lblMessage.Text = "Lead already been rejected by " + dsGetApprovalHistory.Tables[0].Rows[0]["UpdatedBy"].ToString();
                            txtRemark.Visible = false;
                            btnYes.Visible = false;
                            btnNo.Visible = false;
                        }
                        else
                        {
                            if (IsApproved == 1) // approved
                            {
                                lblMessage.Text = "Lead already been approved by " + dsGetApprovalHistory.Tables[0].Rows[0]["UpdatedBy"].ToString();
                                txtRemark.Visible = false;
                                btnYes.Visible = false;
                                btnNo.Visible = false;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
        {
            //int EnquiryId = Convert.ToInt32(FormViewEnquiry.DataKey.Value.ToString());
            //if (EnquiryId > 0)
            //{
            //int result = DBOperations.CRM_AddEnquiryHistory(EnquiryId, 7, txtRemark.Text.Trim(), Convert.ToInt32(hdnUserId.Value));
            int result = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(hdnLeadId.Value), 6,dtClose, txtRemark.Text.Trim(), Convert.ToInt32(hdnUserId.Value));
            if (result == 0)
            {
                // Update Lead Stage
                int LeadStage = DBOperations.CRM_UpdateLeadStatus(Convert.ToInt32(hdnLeadId.Value), true, false, false, false, false, false, Convert.ToInt32(hdnUserId.Value));

                string script = "<script type = 'text/javascript'>alert('Lead successfully approved.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
                FormView1.DataBind();
                //FormViewEnquiry.DataBind();
                CheckApproval();
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
            //}
            //else
            //{
            //    string script = "<script type = 'text/javascript'>alert('Error while approving the enquiry. Please try again later.');</script>";
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
            //}
        }
        else
        {
            string script = "<script type = 'text/javascript'>alert('Lead does not exists.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
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

    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
        {
            int result = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(hdnLeadId.Value), 10, dtClose, txtRemark.Text.Trim(), Convert.ToInt32(hdnUserId.Value));
            if (result == 0)
            {
                string script = "<script type = 'text/javascript'>alert('Lead successfully rejected.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
                SendRejectionMail(Convert.ToInt32(hdnLeadId.Value), txtRemark.Text.Trim());
            }
            else if (result == 2)
            {
                string script = "<script type = 'text/javascript'>alert('Lead does not exists.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
            }
            else
            {
                string script = "<script type = 'text/javascript'>alert('Error while rejecting the Lead. Please try again later.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
            }
        }
        else
        {
            string script = "<script type = 'text/javascript'>alert('Lead does not exists.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());
        }
    }

    protected bool SendRejectionMail(int LeadId, string strRejectionRemark)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", strLeadRefNo = "", strCompanyName = "",
                strLeadCreatedByEmail = "", EmailContent = "";

        bool bEmailSuccess = false;
        try
        {
            if (LeadId > 0)
            {
                DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
                if (dsGetLead != null)
                {
                    try
                    {
                        //string strFileName = "../EmailTemplate/RejectedLead.html";
                        string strFileName = "../EmailTemplate/RejectedLead.txt";
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

                    MessageBody = EmailContent.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
                    MessageBody = MessageBody.Replace("@Remark", strRejectionRemark);
                    strLeadCreatedByEmail = dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

                    strCustomerEmail = strLeadCreatedByEmail.ToString(); //"kivisha.jain@babajishivram.com"; 
                    strCCEmail = "javed.shaikh@babajishivram.com";
                    strSubject = "Lead Approval Rejected For " + dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString().ToUpper();

                    if (strCustomerEmail == "" || strSubject == "")
                        return bEmailSuccess;
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
            else
                return false;
        }
        catch (Exception en)
        {
            return false;
        }
    }

}