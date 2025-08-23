using System;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;

public partial class AccountExpense_PendingInvoiceApprovalHOD : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvDocument);

        if (!IsPostBack)
        {
            Session["InvoiceId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "HOD - Invoice Approval";

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;

            }
        }
        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "PendingInvoiceApprovalHOD.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        txtReason.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strInvoiceId = (string)e.CommandArgument;

            Session["InvoiceId"] = strInvoiceId;

            Response.Redirect("InvoiceApprovalHOD.aspx"); ;
        }
        else if (e.CommandName.ToLower() == "viewbjv")
        {
            string strBJVNo = e.CommandArgument.ToString();

            DataSet dsBJVDetail = BillingOperation.FAGetJobExpenseBilled(strBJVNo);

            if (dsBJVDetail.Tables[0].Rows.Count > 0)
            {
                gvBJVDetail.DataSource = dsBJVDetail;
                gvBJVDetail.DataBind();
            }
            else
            {
                gvBJVDetail.DataSource = null;
                gvBJVDetail.DataBind();
            }

            ModalPopupExtender2.Show();
        }
        else if (e.CommandName.ToLower().Trim() == "approvejob")
        {
            hdnOperationType.Value = "1"; // Approve
            lblPopupName.Text = "Approve Invoice";

            btnApproveJob.Visible = true;
            btnRejectJob.Visible = false;
            btnHoldJob.Visible = false;

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = InvoiceID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"] != null)
                    lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"] != null)
                    lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"] != null)
                    lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"] != null)
                    lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Remark"] != null)
                    lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"] != null)
                    lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();

        }

        else if (e.CommandName.ToLower() == "hold")
        {
            hdnOperationType.Value = "3"; // Hold
            lblPopupName.Text = "Hold Invoice";

            btnHoldJob.Visible = true;
            btnRejectJob.Visible = false;
            btnApproveJob.Visible = false;

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = InvoiceID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"] != null)
                    lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"] != null)
                    lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"] != null)
                    lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"] != null)
                    lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Remark"] != null)
                    lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"] != null)
                    lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();

        }

        else if (e.CommandName.ToLower() == "reject")
        {
            hdnOperationType.Value = "2"; // Reject
            lblPopupName.Text = "Reject Invoice";

            btnRejectJob.Visible = true;

            btnApproveJob.Visible = false;
            btnHoldJob.Visible = false;

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = InvoiceID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"] != null)
                    lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"] != null)
                    lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"] != null)
                    lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"] != null)
                    lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Remark"] != null)
                    lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"] != null)
                    lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();
            ////int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());


        }
        else if (e.CommandName.ToLower() == "history")
        {
            // hdnReqForJobDetails.Value = "details";
            // lblDetailsPopName.Text = "Job Details/History";
            //btnRejectJob.Text = "Request Details/History";

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetJobDetails = AccountExpense.GetInvoiceJobPrevHistory(InvoiceID);

            if (dsGetJobDetails.Tables.Count > 0)
            {
                grdJobHistory.DataSource = dsGetJobDetails;
                grdJobHistory.DataBind();

            }

            ModalPopupDetailHistory.Show();
        }
        else if (e.CommandName.ToLower() == "downloaddoc")
        {
            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocbDetails = AccountExpense.GetInvoiceDocument(InvoiceID, 0);

            if (dsGetDocbDetails.Tables.Count > 0)
            {
                gvDocument.DataSource = dsGetDocbDetails;
                gvDocument.DataBind();
            }

            ModalPopupDetailDocument.Show();
        }
    }
    protected void btnApproveJob_OnClick(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID > 0)
            {

                int ApprovedJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 110, txtReason.Text.Trim(), LoggedInUser.glUserId);

                if (ApprovedJobExp == 0)
                {
                    gvDetail.DataBind();
                    lblMessage.Text = "Invoice Approved!. Sent For Audit L1";
                    lblMessage.CssClass = "success";
                }

                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Invoice Already Approved!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "System Error!. Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";
                }

            }
        }
        else
        {
            lblError_RejectExp.Text = "Please Enter Remark!";
            lblError_RejectExp.CssClass = "errorMsg";
            txtReason.Focus();
            RejectModalPopupExtender.Show();
        }
    }
    protected void btnRejectJob_OnClick(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID > 0)
            {
                //if (hdnRequestForReject.Value.ToLower().Trim() == "reject")
                {
                    int result = AccountExpense.AddInvoiceStatus(InvoiceID, 109, txtReason.Text.Trim(), LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblMessage.Text = "Invoice Rejected!";
                        lblMessage.CssClass = "success";
                        gvDetail.DataBind();

                        SendHoldRejectMail(InvoiceID, "HOD Reject", txtReason.Text.Trim());
                    }
                    else if (result == 2)
                    {
                        lblMessage.Text = "Invoice Already Rejected!";
                        lblMessage.CssClass = "errorMsg";
                        gvDetail.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "System Error! Please try after sometime";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
        else
        {
            lblError_RejectExp.Text = "Please enter reason for Reject!";
            lblError_RejectExp.CssClass = "errorMsg";
            txtReason.Focus();
            RejectModalPopupExtender.Show();
        }
    }
    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);

            int result = AccountExpense.AddInvoiceStatus(InvoiceID, 108, txtReason.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Invoice On Hold!";
                lblMessage.CssClass = "success";
                gvDetail.DataBind();

                SendHoldRejectMail(InvoiceID, "HOD On Hold", txtReason.Text.Trim());

            }
            else if (result == 2)
            {
                lblMessage.Text = "Invoice Already On Hold!";
                lblMessage.CssClass = "errorMsg";
                gvDetail.DataBind();
            }
            else
            {
                lblMessage.Text = "System Error! Please try after sometime";
                lblMessage.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError_RejectExp.Text = "Please enter Remark for Hold!";
            lblError_RejectExp.CssClass = "errorMsg";
            txtReason.Focus();
            RejectModalPopupExtender.Show();
        }
    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsBilled")) == true)
            {
                e.Row.Cells[6].BackColor = System.Drawing.Color.Red;
                e.Row.ToolTip = "Billed Job";
            }
            if (DataBinder.Eval(e.Row.DataItem, "lStatus") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "lStatus").ToString());
                string strStatusName = DataBinder.Eval(e.Row.DataItem, "StatusName").ToString();

                if (StatusId == 108) // Mgmt Hold - 
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    e.Row.ToolTip = strStatusName;
                }
            }
        }
    }

    protected void gvBJVDetail_PreRender(object sender, EventArgs e)
    {
        if (gvBJVDetail.Rows.Count > 1)
        {
            GridViewRow getRow = gvBJVDetail.Rows[gvBJVDetail.Rows.Count - 1];
            getRow.Cells[7].BackColor = System.Drawing.Color.Yellow;
            getRow.Cells[8].BackColor = System.Drawing.Color.Green;

            decimal decDebit = 0m;
            decimal decCredit = 0m;
            decimal decProfit = 0m;

            Decimal.TryParse(getRow.Cells[7].Text.Trim(), out decDebit);
            Decimal.TryParse(getRow.Cells[8].Text.Trim(), out decCredit);

            decProfit = (decCredit - decDebit);


            if (decProfit <= 0)
            {
                getRow.Cells[8].BackColor = System.Drawing.Color.MediumVioletRed;
                getRow.Cells[9].Text = "Loss: " + decProfit.ToString();
            }
            else
            {
                getRow.Cells[9].Text = "Profit: " + decProfit.ToString();
            }

        }
    }
    protected void btnCancelBJVPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }
    #region Hold Reject Email

    private bool SendHoldRejectMail(int InvoiceID, string strStatusName, string strRemark)
    {
        string MessageBody = "", strCCEmail = "", strSubject = "";

        bool bEmailSucces = false;
        strCCEmail = EmailConfig.GetEmailAccountCCTo();
        StringBuilder strbuilder = new StringBuilder();

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        string strJobNumber = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
        string strCustomer = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
        string strConsignee = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();

        string strExpenseType = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

        string strVendorName = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();

        string strInvoiceNo = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();

        string strTotalInvoiceValue = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();

        string strCreatedByEmail = "amit.bakshi@BabajiShivram.com";

        if (dsDetail.Tables[0].Rows[0]["CreatedByEmail"] != DBNull.Value)
        {
            strCreatedByEmail = dsDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
        }

        string EmailContent = "";

        try
        {

            try
            {
                string strFileName = System.Web.HttpContext.Current.Server.MapPath("~") + "//EmailTemplate/EmailVendorPaymentReject.txt";

                //  string strFileName = "../EmailTemplate/EmailVendorPaymentReject.txt";

                EmailContent = File.ReadAllText(strFileName);

                //StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                //sr = File.OpenText(Server.MapPath(strFileName));
                //EmailContent = sr.ReadToEnd();
                //sr.Close();
                //sr.Dispose();
                //GC.Collect();
            }
            catch (Exception ex)
            {
                return bEmailSucces;
            }

            MessageBody = EmailContent.Replace("@JobRefNo", strJobNumber);

            MessageBody = MessageBody.Replace("@Customer", strCustomer);
            MessageBody = MessageBody.Replace("@Consignee", strConsignee);
            MessageBody = MessageBody.Replace("@ExpenseType", strExpenseType);
            MessageBody = MessageBody.Replace("@VendorName", strVendorName);
            MessageBody = MessageBody.Replace("@InvoiceNo", strInvoiceNo);
            MessageBody = MessageBody.Replace("@TotalInvoiceAmount", strTotalInvoiceValue);
            MessageBody = MessageBody.Replace("@StatusName", strStatusName);
            MessageBody = MessageBody.Replace("@Remarks", strRemark);
            MessageBody = MessageBody.Replace("@UserName", LoggedInUser.glEmpName);

            ///////////////////////////////////////////////////////////////////////////////

            try
            {
                strSubject = "Vendor Payment " + strStatusName + "/" + strJobNumber + "/ " + strInvoiceNo + "/ " + strConsignee + " / " + strExpenseType;

                string EmailBody = MessageBody;

                bEmailSucces = EMail.SendMailCC(LoggedInUser.glUserName, strCreatedByEmail, strCCEmail, strSubject, EmailBody, "");
            }
            catch (System.Exception exn)
            {
                bEmailSucces = false;
                return bEmailSucces;
            }
        }
        catch (Exception enk)
        {
            return false;
        }

        return bEmailSucces;
    }


    #endregion

    #region Document Download

    protected void gvDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                DownloadDocument(strDocpath);
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                ViewDocument(strDocpath);
            }
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
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

    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);
            
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "PendingInvoiceApprovalHOD.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "HODApproval_CreditVendor" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;
        gvDetail.Columns[1].Visible = false;
        gvDetail.Columns[2].Visible = false;
        gvDetail.Columns[3].Visible = true;

        DataFilter1.FilterSessionID = "PendingInvoiceApprovalHOD.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();
                
        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}