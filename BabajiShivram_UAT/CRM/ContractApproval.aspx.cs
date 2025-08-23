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

public partial class CRM_ContractApproval : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(btnApprove);
        ScriptManager1.RegisterPostBackControl(btnReject);
        ScriptManager1.RegisterPostBackControl(btnRejectContract);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Contract Approval";

            if (gvLeads.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Contract Approval!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "ContractApproval.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[14].ToolTip = "KYC registered - Today's date";

            if ((DataBinder.Eval(e.Row.DataItem, "LeadStageID")) != DBNull.Value)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LeadStageID")) == 15 ||
                    Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LeadStageID")) == 18) // rejected contract
                {
                    //e.Row.Cells[1].Text = "";
                    e.Row.BackColor = System.Drawing.Color.FromName("#ff000059");
                    e.Row.ToolTip = "Contract been rejected!";
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "RfqReceived") != DBNull.Value)
            {
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "RfqReceived")).ToLower().Trim() == "yes")
                {
                    e.Row.Cells[2].Text = "";
                }
            }
        }
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "viewlead")
        {
            Session["LeadId"] = e.CommandArgument.ToString();
            Response.Redirect("LeadDetail.aspx");
        }
        else if (e.CommandName.ToLower() == "downloadquote")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDoc(DocPath);
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
        gvLeads.Enabled = false;
        gvLeads.Caption = "Pending Contract Approval Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "ContractApproval.aspx";
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
            DataFilter1.FilterSessionID = "ContractApproval.aspx";
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

    #endregion

    #region Contract Rejection

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeContractRejection.Hide();
        txtRemark.Text = "";
        gvRejectContract.DataBind();
        ViewState["ContractReject"] = null;
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int count = 0;
        foreach (GridViewRow gvr in gvLeads.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                int LeadId = Convert.ToInt32(gvLeads.DataKeys[gvr.RowIndex].Values[0].ToString());
                int QuoteId = Convert.ToInt32(gvLeads.DataKeys[gvr.RowIndex].Values[1].ToString());

                LinkButton lnkLead = (LinkButton)gvr.FindControl("lnkLead");
                string Customer = gvr.Cells[6].Text;
                if (QuoteId > 0)
                {
                    count++;
                    AddNewRow(LeadId, lnkLead.Text.Trim(), Customer, QuoteId);
                }
            }
        }

        if (count == 0)
        {
            lblError.Text = "Please select atleast one lead to approve!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblPopup_Title.Text = "Approve Contract";
            mpeContractRejection.Show();
            pnlApproval.Visible = true;
            pnlRejection.Visible = false;
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        int count = 0;
        foreach (GridViewRow gvr in gvLeads.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                int LeadId = Convert.ToInt32(gvLeads.DataKeys[gvr.RowIndex].Values[0].ToString());
                int QuoteId = Convert.ToInt32(gvLeads.DataKeys[gvr.RowIndex].Values[1].ToString());
                LinkButton lnkLead = (LinkButton)gvr.FindControl("lnkLead");

                string Customer = gvr.Cells[6].Text;
                if (QuoteId > 0)
                {
                    count++;
                    AddNewRow(LeadId, lnkLead.Text.Trim(), Customer, QuoteId);
                }
            }
        }

        if (count == 0)
        {
            lblError.Text = "Please select atleast one lead to reject!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblPopup_Title.Text = "Reject Contract";
            mpeContractRejection.Show();
            pnlApproval.Visible = false;
            pnlRejection.Visible = true;
        }
    }

    protected void btnDeleteRow_Click(object sender, EventArgs e)
    {
        Button lb = (Button)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["ContractReject"] != null)
        {
            DataTable dt = (DataTable)ViewState["ContractReject"];
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowID]);
            }

            if (dt.Rows.Count == 0)
            {
                ViewState["ContractReject"] = null;
            }
            else
            {
                ViewState["ContractReject"] = dt;
            }
            gvRejectContract.DataSource = dt;
            gvRejectContract.DataBind();
        }
        SetPreviousData();
        mpeContractRejection.Show();
    }

    protected void AddNewRow(int LeadId, string LeadRefNo, string Customer, int QuoteId)
    {
        int rowIndex = 0;
        if (ViewState["ContractReject"] != null)
        {
            DataTable dt = (DataTable)ViewState["ContractReject"];
            DataRow dr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvRejectContract.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblLeadId = (Label)gvRejectContract.Rows[rowIndex].Cells[1].FindControl("lblLeadId");
                    Label lblLeadRefNo = (Label)gvRejectContract.Rows[rowIndex].Cells[2].FindControl("lblLeadRefNo");
                    Label lblCustomer = (Label)gvRejectContract.Rows[rowIndex].Cells[3].FindControl("lblCustomer");
                    Label hdnQuoteId = (Label)gvRejectContract.Rows[rowIndex].Cells[5].FindControl("hdnQuoteId");

                    dr = dt.NewRow();
                    dr["RowNumber"] = i + 1;
                    dr["LeadId"] = LeadId.ToString();
                    dr["LeadRefNo"] = LeadRefNo;
                    dr["Customer"] = Customer;
                    dr["QuoteId"] = QuoteId;

                    dt.Rows[i - 1]["RowNumber"] = lblRowNumber.Text;
                    dt.Rows[i - 1]["LeadId"] = lblLeadId.Text;
                    dt.Rows[i - 1]["LeadRefNo"] = lblLeadRefNo.Text;
                    dt.Rows[i - 1]["Customer"] = lblCustomer.Text;
                    dt.Rows[i - 1]["QuoteId"] = hdnQuoteId.Text;
                    rowIndex++;
                }

                dt.Rows.Add(dr);
                ViewState["ContractReject"] = dt;
                gvRejectContract.DataSource = dt;
                gvRejectContract.DataBind();
            }
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("LeadId", typeof(string)));
            dt.Columns.Add(new DataColumn("LeadRefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer", typeof(string)));
            dt.Columns.Add(new DataColumn("QuoteId", typeof(string)));

            DataRow dr = null;
            dr = dt.NewRow();

            dr["RowNumber"] = 1;
            dr["LeadId"] = LeadId.ToString();
            dr["LeadRefNo"] = LeadRefNo;
            dr["Customer"] = Customer;
            dr["QuoteId"] = QuoteId;

            dt.Rows.Add(dr);
            ViewState["ContractReject"] = dt;
            gvRejectContract.DataSource = dt;
            gvRejectContract.DataBind();

            Label lblRowNumber = (Label)gvRejectContract.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
            Label lblLeadId = (Label)gvRejectContract.Rows[rowIndex].Cells[1].FindControl("lblLeadId");
            Label lblLeadRefNo = (Label)gvRejectContract.Rows[rowIndex].Cells[2].FindControl("lblLeadRefNo");
            Label lblCustomer = (Label)gvRejectContract.Rows[rowIndex].Cells[3].FindControl("lblCustomer");
            Label hdnQuoteId = (Label)gvRejectContract.Rows[rowIndex].Cells[5].FindControl("hdnQuoteId");

            lblLeadId.Text = LeadId.ToString();
            lblLeadRefNo.Text = LeadRefNo.ToString();
            lblCustomer.Text = Customer.ToString();
            hdnQuoteId.Text = QuoteId.ToString();
        }
        SetPreviousData();
    }

    protected void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["ContractReject"] != null)
        {
            DataTable dt = (DataTable)ViewState["ContractReject"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvRejectContract.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblLeadId = (Label)gvRejectContract.Rows[rowIndex].Cells[1].FindControl("lblLeadId");
                    Label lblLeadRefNo = (Label)gvRejectContract.Rows[rowIndex].Cells[2].FindControl("lblLeadRefNo");
                    Label lblCustomer = (Label)gvRejectContract.Rows[rowIndex].Cells[3].FindControl("lblCustomer");
                    Label hdnQuoteId = (Label)gvRejectContract.Rows[rowIndex].Cells[5].FindControl("hdnQuoteId");

                    lblRowNumber.Text = dt.Rows[i]["RowNumber"].ToString();
                    lblLeadId.Text = dt.Rows[i]["LeadId"].ToString();
                    lblLeadRefNo.Text = dt.Rows[i]["LeadRefNo"].ToString();
                    lblCustomer.Text = dt.Rows[i]["Customer"].ToString();
                    hdnQuoteId.Text = dt.Rows[i]["QuoteId"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    protected void btnRejectContract_Click(object sender, EventArgs e)
    {
        int count = 0;
        if (gvRejectContract != null && gvRejectContract.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvRejectContract.Rows)
            {
                if (((Label)gvr.FindControl("lblLeadId")).Text.Trim() != "" && ((Label)gvr.FindControl("lblLeadId")).Text.Trim() != "0")
                {
                    int result = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(((Label)gvr.FindControl("lblLeadId")).Text.Trim()), 15,dtClose, txtRemark.Text.Trim(), loggedInUser.glUserId);
                    if (result == 0)
                    {
                        count++;
                    }
                    else
                    {
                        lblError.Text = "Error while rejecting contract! Please try again later.";
                        lblError.CssClass = "errorMsg";
                        break;
                    }
                }
                else
                {
                    if (((Label)gvr.FindControl("hdnQuoteId")).Text.Trim() != "")
                    {
                        int QuoteId = Convert.ToInt32(((Label)gvr.FindControl("hdnQuoteId")).Text.Trim());
                        int result = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(18), txtRemark.Text.Trim(),dtClose, loggedInUser.glUserId);
                        if (result == 1)
                        {
                            count++;
                        }
                        else
                        {
                            lblError.Text = "Error while rejecting contract! Please try again later.";
                            lblError.CssClass = "errorMsg";
                            break;
                        }
                    }
                }
            }

            if (count == 0)
            {
                lblError.Text = "Please select atleast one lead to reject!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Contract rejected!";
                lblError.CssClass = "success";
                mpeContractRejection.Hide();
                gvLeads.DataBind();
            }
        }
        else
        {
            lblError.Text = "Please select atleast one lead to reject!";
            lblError.CssClass = "errorMsg";
            mpeContractRejection.Hide();
            gvLeads.DataBind();
        }
    }

    protected void btnApproveContract_Click(object sender, EventArgs e)
    {
        int count = 0;
        if (gvRejectContract != null && gvRejectContract.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvRejectContract.Rows)
            {
                if (((Label)gvr.FindControl("lblLeadId")).Text.Trim() != "" && ((Label)gvr.FindControl("lblLeadId")).Text.Trim() != "0")
                {
                    int result = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(((Label)gvr.FindControl("lblLeadId")).Text.Trim()), 14, dtClose, "", loggedInUser.glUserId);
                    if (result == 0)
                    {
                        count++;
                    }
                    else
                    {
                        lblError.Text = "Error while approving contract! Please try again later.";
                        lblError.CssClass = "errorMsg";
                        break;
                    }
                }
                else
                {
                    if (((Label)gvr.FindControl("hdnQuoteId")).Text.Trim() != "")
                    {
                        int QuoteId = Convert.ToInt32(((Label)gvr.FindControl("hdnQuoteId")).Text.Trim());
                        int result = QuotationOperations.UpdateQuoteStatus(QuoteId, Convert.ToInt32(17), txtRemark.Text.Trim(),dtClose, loggedInUser.glUserId);
                        if (result == 1)
                        {
                            count++;
                        }
                        else
                        {
                            lblError.Text = "Error while approving contract! Please try again later.";
                            lblError.CssClass = "errorMsg";
                            break;
                        }
                    }
                }
            }

            if (count == 0)
            {
                lblError.Text = "Please select atleast one lead to approve!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Contract Approved Successfully!";
                lblError.CssClass = "success";
                mpeContractRejection.Hide();
                gvLeads.DataBind();
            }
        }
        else
        {
            lblError.Text = "Please select atleast one lead to approve!";
            lblError.CssClass = "errorMsg";
            mpeContractRejection.Hide();
            gvLeads.DataBind();
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