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

public partial class CRM_AdminTracking : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Tracking";

            if (gvLeads.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Tracking!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "LeadTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "viewlead")
        {
            Session["LeadId"] = e.CommandArgument.ToString();
            Response.Redirect("EditLead.aspx");
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Pendingapproval_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeads.Columns[2].Visible = false;
        gvLeads.Enabled = false;
        gvLeads.Caption = "Pending Contract Approval Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "LeadTracking.aspx";
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
            DataFilter1.FilterSessionID = "LeadTracking.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Quotation
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
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", EmailContent = "";
        string EncryptedEnquiryId = HttpUtility.UrlEncode(Encrypt(Convert.ToString((EnquiryId))));

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
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
            }

            MessageBody = EmailContent.Replace("@KycLink", "http://192.168.5.43/CRMProject/KYC_New/index.aspx?p=" + EncryptedEnquiryId);
            MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
            MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
            MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
            MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
            MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
            MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
            MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

            strSubject = "KYC Request from Babaji Shivram Clearing & Carriers Pvt. Ltd.";
            //strCustomerEmail = "kivisha.jain@babajishivram.com";
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