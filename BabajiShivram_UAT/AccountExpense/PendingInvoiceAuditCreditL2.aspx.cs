using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class AccountExpense_PendingInvoiceAuditCreditL2 : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["InvoiceId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Audit - L2 - Credit Vendor";

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
        DataFilter1.FilterSessionID = "PendingInvoiceAuditCreditL2.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsBilled")) == true)
            {
                e.Row.Cells[5].BackColor = System.Drawing.Color.Red;
                e.Row.ToolTip = "Billed Job";
            }
            if (DataBinder.Eval(e.Row.DataItem, "lStatus") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "lStatus").ToString());
                string strStatusName = DataBinder.Eval(e.Row.DataItem, "StatusName").ToString();

                // Invoice Approval
                if (StatusId == 141) // Audit L2 ON Hold
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#E8ADAA");  //#E77471
                    e.Row.ToolTip = strStatusName;
                }

            }

        }
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        txtReason.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strInvoiceId = (string)e.CommandArgument;

            Session["InvoiceId"] = strInvoiceId;

            Response.Redirect("InvoiceFinance.aspx"); ;
        }
        else if (e.CommandName.ToLower().Trim() == "approveinvoice")
        {
            hdnOperationType.Value = "1"; // Approve Invoice For FA Posting
            lblPopupName.Text = "Complete Audit L2";

            btnApproveInvoice.Visible = true;

            btnRejectInvoice.Visible = false;
            btnHoldPayment.Visible = false;
            btnApprovePayment.Visible = false;

            hdnInvoiceId.Value = e.CommandArgument.ToString();
            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                hdnStatusId.Value = dsGetPaymentDetails.Tables[0].Rows[0]["lStatus"].ToString(); ;
                hdnBillType.Value = dsGetPaymentDetails.Tables[0].Rows[0]["BillType"].ToString();

                lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();

        }
        else if (e.CommandName.ToLower().Trim() == "rejectinvoice")
        {
            hdnOperationType.Value = "2"; // Reject Invoice Fo FA Audit Posting
            lblPopupName.Text = "Reject Invoice";

            btnRejectInvoice.Visible = true;

            btnApproveInvoice.Visible = false;
            btnHoldPayment.Visible = false;
            btnApprovePayment.Visible = false;

            hdnInvoiceId.Value = e.CommandArgument.ToString();

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());
            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                hdnStatusId.Value = dsGetPaymentDetails.Tables[0].Rows[0]["lStatus"].ToString(); ;
                lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();

        }
        else if (e.CommandName.ToLower().Trim() == "paymenthold")
        {
            hdnOperationType.Value = "3"; // Hold Payment
            lblPopupName.Text = "Hold Payment";

            btnHoldPayment.Visible = true;

            btnRejectInvoice.Visible = false;
            btnApproveInvoice.Visible = false;
            btnApprovePayment.Visible = false;

            hdnInvoiceId.Value = e.CommandArgument.ToString();

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());
            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                hdnStatusId.Value = dsGetPaymentDetails.Tables[0].Rows[0]["lStatus"].ToString(); ;
                lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();

        }
        else if (e.CommandName.ToLower().Trim() == "paymentapprove")
        {
            hdnOperationType.Value = "4"; // Approve Payment
            lblPopupName.Text = "Approve Payment";

            btnApprovePayment.Visible = true;

            btnHoldPayment.Visible = false;
            btnRejectInvoice.Visible = false;
            btnApproveInvoice.Visible = false;

            hdnInvoiceId.Value = e.CommandArgument.ToString();

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());
            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                hdnStatusId.Value = dsGetPaymentDetails.Tables[0].Rows[0]["lStatus"].ToString(); ;

                lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }

            RejectModalPopupExtender.Show();

        }
        else if (e.CommandName.ToLower() == "history")
        {
            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetJobDetails = AccountExpense.GetInvoiceJobPrevHistory(InvoiceID);

            if (dsGetJobDetails.Tables.Count > 0)
            {
                grdJobHistory.DataSource = dsGetJobDetails;
                grdJobHistory.DataBind();
            }

            ModalPopupDetailHistory.Show();
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
    }
    protected void btnApproveInvoice_OnClick(object sender, EventArgs e)
    {
        // Check Invoice Status Change
        bool isStatusChanged = CheckInvoiceStatusChange();

        if (isStatusChanged == true)
        {
            lblMessage.Text = "Invoice details already updated! Please Check Status History!";
            lblMessage.CssClass = "errorMsg";

            Session["Error"] = lblMessage.Text;

            hdnInvoiceId.Value = "";
            Session["InvoiceId"] = null;

            Response.Redirect("AccountSuccess.aspx");
        }

        if (txtReason.Text.Trim() != "" && isStatusChanged == false)
        {
            int BillType = 0;
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);

            Int32.TryParse(hdnBillType.Value.Trim(), out BillType);

            if (InvoiceID > 0 && BillType < 10)
            {
                int ApprovedJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 145, txtReason.Text.Trim(), LoggedInUser.glUserId);

                if (ApprovedJobExp == 0)
                {
                    gvDetail.DataBind();
                    lblMessage.Text = "Successfully Completed Invoice Audit L2.";
                    lblMessage.CssClass = "success";
                }
                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Invoice Audit L2 Already Completed!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "System Error!. Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
            else if (InvoiceID > 0 && BillType == 10)// Vendor Cheque Payment
            {
                int ApprovedJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 145, txtReason.Text.Trim(), LoggedInUser.glUserId);

                int PaymentJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 152, txtReason.Text.Trim(), LoggedInUser.glUserId);

                if (ApprovedJobExp == 0)
                {
                    gvDetail.DataBind();
                    lblMessage.Text = "Successfully Completed Invoice Audit L2. Vendor Cheque Payment already completed.";
                    lblMessage.CssClass = "success";
                }
                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Invoice Audit L2 Already Completed!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "System Error!. Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }
    }
    protected void btnRejectInvoice_OnClick(object sender, EventArgs e)
    {
        // Check Invoice Status Change
        bool isStatusChanged = CheckInvoiceStatusChange();

        if (isStatusChanged == true)
        {
            lblMessage.Text = "Invoice details already updated! Please Check Status History!";
            lblMessage.CssClass = "errorMsg";

            Session["Error"] = lblMessage.Text;

            hdnInvoiceId.Value = "";
            Session["InvoiceId"] = null;

            Response.Redirect("AccountSuccess.aspx");
        }

        if (txtReason.Text.Trim() != "" && isStatusChanged == false)
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID != 0)
            {
                int ApprovedJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 142, txtReason.Text.Trim(), LoggedInUser.glUserId);

                if (ApprovedJobExp == 0)
                {
                    gvDetail.DataBind();
                    lblMessage.Text = "Invoice Rejected! Sent to Audit Review.";
                    lblMessage.CssClass = "success";
                }
                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Invoice Already Rejected!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "System Error!. Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }
    }
    protected void btnHoldPayment_OnClick(object sender, EventArgs e)
    {
        // Check Invoice Status Change
        bool isStatusChanged = CheckInvoiceStatusChange();

        if (isStatusChanged == true)
        {
            lblMessage.Text = "Invoice details already updated! Please Check Status History!";
            lblMessage.CssClass = "errorMsg";

            Session["Error"] = lblMessage.Text;

            hdnInvoiceId.Value = "";
            Session["InvoiceId"] = null;

            Response.Redirect("AccountSuccess.aspx");
        }

        if (txtReason.Text.Trim() != "" && isStatusChanged == false)
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID != 0)
            {
                // 141 -- Audit L2 ON Hold
                int ApprovedJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 141, txtReason.Text.Trim(), LoggedInUser.glUserId);

                if (ApprovedJobExp == 0)
                {
                    gvDetail.DataBind();
                    lblMessage.Text = "Successfully Hold Payment!.";
                    lblMessage.CssClass = "success";
                }
                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Payment Already On Hold!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "System Error!. Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }
    }
    protected void btnApprovePayment_OnClick(object sender, EventArgs e)
    {
        // Check Invoice Status Change
        bool isStatusChanged = CheckInvoiceStatusChange();

        if (isStatusChanged == true)
        {
            lblMessage.Text = "Invoice details already updated! Please Check Status History!";
            lblMessage.CssClass = "errorMsg";

            Session["Error"] = lblMessage.Text;

            hdnInvoiceId.Value = "";
            Session["InvoiceId"] = null;

            Response.Redirect("AccountSuccess.aspx");
        }

        if (txtReason.Text.Trim() != "" && isStatusChanged == false)
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID != 0)
            {
                // 150 -- Payment Approved
                int ApprovedJobExp = AccountExpense.AddInvoiceStatus(InvoiceID, 150, txtReason.Text.Trim(), LoggedInUser.glUserId);

                if (ApprovedJobExp == 0)
                {
                    gvDetail.DataBind();
                    lblMessage.Text = "Successfully Approved Payment!.";
                    lblMessage.CssClass = "success";
                }
                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Payment Already On Approved!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "System Error!. Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";
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
    private bool CheckInvoiceStatusChange()
    {
        bool isStatusChange = true;
        int PrevStatusId = Convert.ToInt32(hdnStatusId.Value);

        if (hdnInvoiceId.Value != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);

            int result = AccountExpense.CheckInvoiceStatusChange(InvoiceID, PrevStatusId);

            if (result == 0)
            {
                // Status Not changed

                isStatusChange = false;
            }
            else
            {
                // Status changed

                isStatusChange = true;
            }
        }

        return isStatusChange;
    }
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
            DataFilter1.FilterSessionID = "PendingInvoiceAuditCreditL2.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "PendingInvoiceAuditL2_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvDetail.Columns[4].Visible = false;

        DataFilter1.FilterSessionID = "PendingInvoiceAuditCreditL2.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

}