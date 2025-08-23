using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;
public partial class Service_MumbaiExpense : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    decimal TotalAmount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        //ScriptManager1.RegisterPostBackControl(lnkSummary`Excel);

        DateTime dtStartDate = DateTime.Parse("09/01/2022");
        calStatusDateFrom.StartDate = dtStartDate;
        calStatusDateTo.StartDate = dtStartDate;

        calStatusDateFrom.EndDate = DateTime.Now;
        calStatusDateTo.EndDate = DateTime.Now;

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Labour Expense";

            txtStatusDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtStatusDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;

            }
        }
        //
        DataFilter1.DataSource = ExpenseSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "ALabourExpense.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        //txtReason.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strJobIdId = (string)e.CommandArgument;

            Session["JobIDExp"] = strJobIdId;

            // Response.Redirect("InvoiceApproval.aspx"); ;
        }
        if (e.CommandName.ToLower().Trim() == "approvejob")
        {
            hdnOperationType.Value = "1"; // Approve
            lblPopupName.Text = "FA JB Posting";

            btnApproveJob.Visible = true;
            btnRejectJob.Visible = false;
            btnHoldJob.Visible = false;

            int JobID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = JobID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetALabourExpByJobID(JobID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNO"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNO"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseDate"] != null)
                    lblExpenseDate1.Text = Convert.ToDateTime(dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseDate"]).ToString("dd/MM/yyyy");

                if (dsGetPaymentDetails.Tables[0].Rows[0]["TransMode"] != null)
                    lblMode1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["TransMode"].ToString();


                lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaidCharges"].ToString();
            }

            RejectModalPopupExtender.Show();

        }

        else if (e.CommandName.ToLower() == "hold")
        {
            hdnOperationType.Value = "3"; // Hold
            lblPopupName.Text = "Hold Expense";

            btnHoldJob.Visible = true;
            btnRejectJob.Visible = false;
            btnApproveJob.Visible = false;

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = InvoiceID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetALabourExpByJobID(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNO"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNO"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaidCharges"].ToString();
            }

            RejectModalPopupExtender.Show();

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
    protected void btnApproveJob_OnClick(object sender, EventArgs e)
    {
        // Run BJV Labour Expense Post Executable file to check if any Pending 

        int ExeResult = AccountExpense.FAPostLabourExp();

        if (ExeResult > 0)
        {
            // Error in BJV Posting
            // Do not Approve expense till the Pending Posting completed

            lblMessage.Text = "Error in FA Posting. Pending Expense Found for Posting.";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            if (lblRemark1.Text.Trim() != "")
            {
                int JobId = Convert.ToInt32(hdnInvoiceId.Value);
                string strSytemRef = "A" + JobId.ToString();
                int StatusId = 2; // Mumbai Expense Approved

                DateTime dtExpenseDate = Commonfunctions.CDateTime(lblExpenseDate1.Text.Trim());

                if (JobId > 0)
                {
                    int ApprovedJobExp = AccountExpense.ApproveLabourExpByJobID(JobId, strSytemRef, dtExpenseDate, StatusId, lblRemark1.Text.Trim(), LoggedInUser.glUserId);

                    if (ApprovedJobExp == 0)
                    {
                        // Run FA Posting Exe

                        int ExeResult0 = AccountExpense.FAPostLabourExp();

                        // Refresh Data

                        gvDetail.DataBind();
                        lblMessage.Text = "Expense Successfully Posted!.";
                        lblMessage.CssClass = "success";
                        lblRemark1.Text = "";

                        RejectModalPopupExtender.Hide();
                        ExpenseSqlDataSource.DataBind();
                    }

                    else if (ApprovedJobExp == 2)
                    {
                        lblMessage.Text = "Expense Already Approved!";
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
                lblError_RejectExp.Text = "Please enter Remark!";
                lblError_RejectExp.CssClass = "errorMsg";
                lblRemark1.Focus();
                RejectModalPopupExtender.Show();
            }
        }
    }
    protected void btnRejectJob_OnClick(object sender, EventArgs e)
    {
        if (lblRemark1.Text.Trim() != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID > 0)
            {
                //if (hdnRequestForReject.Value.ToLower().Trim() == "reject")
                {
                    int result = 2;// AccountExpense.AddInvoiceStatus(InvoiceID, 112, txtReason.Text.Trim(), LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblMessage.Text = "Invoice Rejected!";
                        lblMessage.CssClass = "success";
                        gvDetail.DataBind();

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
            lblRemark1.Focus();
            RejectModalPopupExtender.Show();
        }
    }
    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        if (lblRemark1.Text.Trim() != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);

            int result = 2;// AccountExpense.AddInvoiceStatus(InvoiceID, 111, txtReason.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Expense On Hold!";
                lblMessage.CssClass = "success";
                gvDetail.DataBind();

            }
            else if (result == 2)
            {
                lblMessage.Text = "Expense Already On Hold!";
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
            //   txtReason.Focus();
            RejectModalPopupExtender.Show();
        }
    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Charges = ((Label)e.Row.FindControl("lblCharges")).Text;
            decimal totalCharges = Convert.ToDecimal(Charges);

            TotalAmount += totalCharges;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lbl = (Label)e.Row.FindControl("lblTotal");
            lbl.Text = TotalAmount.ToString();
        }


    }
    protected void gvBJVDetail_PreRender(object sender, EventArgs e)
    {
        //if (gvBJVDetail.Rows.Count > 1)
        //{
        //    GridViewRow getRow = gvBJVDetail.Rows[gvBJVDetail.Rows.Count - 1];
        //    getRow.Cells[7].BackColor = System.Drawing.Color.Yellow;
        //    getRow.Cells[8].BackColor = System.Drawing.Color.Green;

        //    decimal decDebit = 0m;
        //    decimal decCredit = 0m;
        //    decimal decProfit = 0m;

        //    Decimal.TryParse(getRow.Cells[7].Text.Trim(), out decDebit);
        //    Decimal.TryParse(getRow.Cells[8].Text.Trim(), out decCredit);

        //    decProfit = (decCredit - decDebit);


        //    if (decProfit <= 0)
        //    {
        //        getRow.Cells[8].BackColor = System.Drawing.Color.MediumVioletRed;
        //        getRow.Cells[9].Text = "Loss: " + decProfit.ToString();
        //    }
        //    else
        //    {
        //        getRow.Cells[9].Text = "Profit: " + decProfit.ToString();
        //    }

        //}
    }
    protected void btnCancelBJVPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }
    protected void txtStatusDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtReportDate = DateTime.MinValue;

        if (txtStatusDateFrom.Text != "")
        {
            dtReportDate = Commonfunctions.CDateTime(txtStatusDateFrom.Text.Trim());
        }
    }

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
            DataFilter1.FilterSessionID = "ALabourExpense.aspx";
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
        string strFileName = "PendingExpense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvDetail.Columns[3].Visible = false;
        gvDetail.Columns[4].Visible = false;
        gvDetail.Columns[5].Visible = true;

        DataFilter1.FilterSessionID = "LabourExpense.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region Export To Excel Summary

    protected void lnkSummaryExcel_Click(object sender, EventArgs e)
    {
        if (txtStatusDateFrom.Text.Trim() != "'")
            ExportToExcelBillDetail();
        else
        {

        }

    }
    private void ExportToExcelBillDetail()
    {
        //Register Syncfusion license
        SyncfusionLicenseProvider.RegisterLicense("MDAxQDMxMzkyZTMyMmUzMEw0YkNCVGlYWER5dmVWYmUwV0RMc1dvMzlISmN0VTRLK3VRY2JoQ1VrTG89");

        DateTime dtExpenseDate = Commonfunctions.CDateTime(txtStatusDateFrom.Text);

        DataSet dsBillDetail = AccountExpense.GetLabourExpenseForExcel(dtExpenseDate);

        string strOutputFile = "A-Labour_ExpenseSummary" + txtStatusDateFrom.Text.Replace("/", "-") + ".xlsx";

        if (dsBillDetail.Tables.Count > 0)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                //Create a new workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet sheet = workbook.Worksheets[0];
                sheet.Name = "A-Labour_Expense";

                if (sheet.ListObjects.Count == 0)
                {
                    sheet.ImportDataTable(dsBillDetail.Tables[0], true, 1, 1);
                }

                //Refresh Excel table to get updated values from database
                // sheet.ListObjects[0].Refresh();

                sheet.UsedRange.AutofitColumns();

                workbook.SaveAs(strOutputFile, ExcelSaveType.SaveAsXLS, Response);
            }
        }
        else
        {
            lblMessage.Text = "Expense Detail Not Found";
            lblMessage.CssClass = "errorMsg";
        }

    }

    #endregion
    protected void btnPostAll_Click(object sender, EventArgs e)
    {
        // Run BJV Labour Expense Post Executable file to check if any Pending 

        int ExeResult = AccountExpense.FAPostLabourExp();

        if (ExeResult > 0)
        {
            // Error in BJV Posting
            // Do not Approve expense till the Pending Posting completed

            lblMessage.Text = "Error in FA Posting. Pending Expense Found for Posting.";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Int32 JobID = 0;
            int JobCount = 0;
            int StatusId = 2; // Mumbai Expense Approved
            string strSystemRefNo = "";

            //strSystemRefNo = "AC" + ddlBranch.SelectedValue+ddlMode.SelectedValue + DateTime.Now.Ticks.ToString();

            DateTime ExpenseDate = DateTime.Now;

            foreach (GridViewRow gvr1 in gvDetail.Rows)
            {
                if (gvr1.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chk1");

                    if (chk1.Checked)
                    {
                        //strJobidList = strJobidList + gvDetail.DataKeys[gvr1.RowIndex].Values[0].ToString() + ",";


                        JobID = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Values[0]);

                        //if (strSystemRefNo == "")
                        //{
                        //    strSystemRefNo = "AC" + JobID.ToString() + DateTime.Now.Minute.ToString();
                        //}

                        Label objlblExpenseDate = (Label)gvr1.FindControl("lblExpenseDate");

                        DateTime dtExpenseDate = Commonfunctions.CDateTime(objlblExpenseDate.Text.Trim());

                        int ApprovedJobExp = AccountExpense.ApproveLabourExpByJobID(JobID, strSystemRefNo, dtExpenseDate, StatusId, lblRemark1.Text.Trim(), LoggedInUser.glUserId);

                        if (ApprovedJobExp == 0)
                        {
                            JobCount = JobCount + 1;
                        }

                        else if (ApprovedJobExp == 2)
                        {
                            lblMessage.Text = "Expense Already Approved!";
                            lblMessage.CssClass = "errorMsg";
                        }
                        else
                        {
                            lblMessage.Text = "System Error!. Please try after sometime.";
                            lblMessage.CssClass = "errorMsg";
                        }
                    }

                    if (JobCount > 0)
                    {
                        lblMessage.Text = "Total " + JobCount.ToString() + " Job Expense Successfully Posted!.";
                        lblMessage.CssClass = "success";
                        ExpenseSqlDataSource.DataBind();
                    }
                    else
                    {
                        if (JobCount > 0)
                        {
                            lblMessage.Text = "Please select Job for Expense Posting";
                            lblMessage.CssClass = "errorMsg";
                        }
                    }
                }
            }

            // Run FA Posting Exe

            int ExeResult0 = AccountExpense.FAPostLabourExp();

        }
    }
}