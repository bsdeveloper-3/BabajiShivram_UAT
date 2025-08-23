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
using System.Security.Cryptography;

public partial class CRM_Rejected : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Rejected Leads";

            if (gvLeads.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Rejected Leads!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "Rejected.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region Grid view Events

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Services") == DBNull.Value)  // Services
            {
                ImageButton imgbtnStatus = (ImageButton)e.Row.FindControl("imgbtnStatus");
                if (imgbtnStatus != null)
                {
                    imgbtnStatus.Visible = false;
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "LeadStageId") != DBNull.Value)  // LeadStageID
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LeadStageId")) == 5) // MGMT approval pending
                {
                    e.Row.Cells[1].Text = "";
                }
            }
        }
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "viewlead")
        {
            Session["LeadId"] = e.CommandArgument.ToString();
            Response.Redirect("EditLead.aspx");
        }
        else if (e.CommandName.ToLower().Trim() == "sendapproval")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnLeadId.Value = commandArgs[0].ToString();
            lblLeadRefNo.Text = commandArgs[1].ToString();

            if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
            {
                mpeRemark.Show();
            }
        }
    }

    private bool SendRejectionMail(int LeadId, string strRejectionRemark, int UserId, string strCustomerEmail)
    {
        string MessageBody = "", strCCEmail = "", strSubject = "", strLeadRefNo = "", strCompanyName = "",
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
                        string strFileName = "../EmailTemplate/LeadReapproval.txt";
                        StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                        sr = File.OpenText(Server.MapPath(strFileName));
                        EmailContent = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        lblError.CssClass = "errorMsg";
                    }

                    MessageBody = EmailContent.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
                    MessageBody = MessageBody.Replace("@a", "http://192.168.5.46/Crmproject/CRM/LeadApproval.aspx?a=" + UserId.ToString() + "&i=" + LeadId);
                    MessageBody = MessageBody.Replace("@b", "http://192.168.5.46/Crmproject/CRM/LeadRejected.aspx?a=" + UserId.ToString() + "&i=" + LeadId);
                    MessageBody = MessageBody.Replace("@Remark", strRejectionRemark);
                    strLeadCreatedByEmail = dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

                    strCustomerEmail = strCustomerEmail;
                    strCCEmail = strLeadCreatedByEmail.ToString();
                    strSubject = "Lead Re-Approval For " + dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString().ToUpper();

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

    #endregion

    #region Reject Popup

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeRemark.Hide();
        hdnLeadId.Value = "0";
        lbError_Popup.Text = "";
    }

    protected void btnSendApproval_Click(object sender, EventArgs e)
    {
        if (hdnLeadId.Value != "" && hdnLeadId.Value != "0")
        {
            if (txtRemark.Text.Trim() != "")
            {
                DateTime dtClose = DateTime.MinValue;
                int LeadId = Convert.ToInt32(hdnLeadId.Value);
                int result_History = DBOperations.CRM_AddLeadStageHistory(LeadId, 5, dtClose, txtRemark.Text.Trim(), loggedInUser.glUserId);

                DataSet dsGetQuoteId = DBOperations.CRM_GetQuoteByLead(LeadId);
                if (dsGetQuoteId != null && dsGetQuoteId.Tables.Count > 0)
                {
                    int QuoteId = Convert.ToInt32(dsGetQuoteId.Tables[0].Rows[0]["QuotationId"].ToString());
                    if (QuoteId > 0)
                    {
                        int KYCResult = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(12), "",dtClose, loggedInUser.glUserId);
                        //int result_QuoteHistory = DBOperations.CRM_AddLeadStageHistory(LeadId, 7, "", loggedInUser.glUserId);
                    }
                }

                string script = "<script type = 'text/javascript'>alert('Lead successfully send for re-approval! Thank You.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script.ToString());

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
                            strCustomerEmail = "kirti@babajishivram.com";
                            UserId = 189;
                        }
                        else
                        {
                            strCustomerEmail = "dhaval@babajishivram.com";
                            UserId = 3;
                        }

                        bool bSentMail = SendRejectionMail(LeadId, txtRemark.Text.Trim(), UserId, strCustomerEmail);
                        if (bSentMail == true)
                        {
                            lblError.Text = "Successfully updated lead. Lead has been sent for management approval.";
                            lblError.CssClass = "success";
                            break;
                        }
                    }
                }
            }
            else
            {
                lbError_Popup.Text = "Please enter remark!";
                lbError_Popup.CssClass = "errorMsg";
                mpeRemark.Show();
            }
        }
    }

    #endregion

    #region Export Data

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Rejectedlead_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvLeads.AllowPaging = false;
        gvLeads.AllowSorting = false;
        gvLeads.Columns[0].Visible = false;
        gvLeads.Columns[1].Visible = false;
        gvLeads.Enabled = false;
        gvLeads.Caption = "Rejected Lead Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "Rejected.aspx";
        DataFilter1.FilterDataSource();
        gvLeads.DataBind();
        gvLeads.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "Rejected.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

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