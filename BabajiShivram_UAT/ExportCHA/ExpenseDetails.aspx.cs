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
using System.IO;
using System.Text;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
public partial class ExportCHA_ExpenseDetails : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (Session["JobId"] == null)
        {
            Response.Redirect("PendingJobExpense.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Expense Detail";
            JobDetail(Convert.ToInt32(Session["JobId"]));
            Page.Validate();
        }

    }

    private void JobDetail(int JobId)
    {
        DataSet dsJobDetail = DBOperations.GetJobBasicDetail(JobId);

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            string strJonNum = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text += " " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            ViewState["JobNum"] = strJonNum;
        }
    }

    #region FormView Expense Events

    protected void fvExpense_DataBound(object sender, EventArgs e)
    {
        if (fvExpense.CurrentMode == FormViewMode.Insert)
        {
            DropDownList ddlExpenseType = (DropDownList)fvExpense.FindControl("ddlExpenseType");
            DropDownList ddPaymentType = (DropDownList)fvExpense.FindControl("ddPaymentType");

            if (ddlExpenseType != null)
                DBOperations.FillExpenseMS(ddlExpenseType);

            if (ddPaymentType != null)
                DBOperations.FillPaymentType(ddPaymentType, 3);

            MaskedEditValidator cboMEValReceiptDate = (MaskedEditValidator)fvExpense.FindControl("MEValReceiptDate");
            cboMEValReceiptDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        }
        else if (fvExpense.CurrentMode == FormViewMode.Edit)
        {
            DropDownList ddlUPDExpenseType = (DropDownList)fvExpense.FindControl("ddlUPDExpenseType");
            DropDownList ddUPDPaymentType = (DropDownList)fvExpense.FindControl("ddUPDPaymentType");
            DropDownList ddUPDReceipt = (DropDownList)fvExpense.FindControl("ddUPDReceipt");

            HiddenField hdnExpenseType = (HiddenField)fvExpense.FindControl("hdnExpenseType");
            HiddenField hdnPaymentType = (HiddenField)fvExpense.FindControl("hdnPaymentType");
            HiddenField hdnReceiptable = (HiddenField)fvExpense.FindControl("hdnReceiptable");

            if (ddlUPDExpenseType != null)
            {
                DBOperations.FillExpenseMS(ddlUPDExpenseType);
                ddlUPDExpenseType.SelectedValue = hdnExpenseType.Value;
            }

            if (ddUPDPaymentType != null)
            {
                DBOperations.FillPaymentType(ddUPDPaymentType, 3);
                ddUPDPaymentType.SelectedValue = hdnPaymentType.Value;
            }

            if (ddUPDReceipt != null)
            {
                ddUPDReceipt.SelectedValue = hdnReceiptable.Value;
            }
        }
    }

    protected void btnNewExpense_Click(object sender, EventArgs e)
    {
        fvExpense.ChangeMode(FormViewMode.Insert);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        float eAmount = 0;
        bool IsBillable = true;

        DateTime dtReceiptDate = DateTime.MinValue;
        DateTime dtChequeDate = DateTime.MinValue;

        DateTime dtMinValue = DateTime.Now.AddYears(-1);

        DropDownList ddlExpenseType = (DropDownList)fvExpense.FindControl("ddlExpenseType");
        DropDownList ddReceipt = (DropDownList)fvExpense.FindControl("ddReceipt");
        DropDownList ddPaymentType = (DropDownList)fvExpense.FindControl("ddPaymentType");
        DropDownList ddBillable = (DropDownList)fvExpense.FindControl("ddBillable");

        TextBox txtEAmount = (TextBox)fvExpense.FindControl("txtEAmount");
        TextBox txtReceiptNo = (TextBox)fvExpense.FindControl("txtReceiptNo");
        TextBox txtPaidTo = (TextBox)fvExpense.FindControl("txtPaidTo");
        TextBox txtLocation = (TextBox)fvExpense.FindControl("txtLocation");
        TextBox txtReceiptAmount = (TextBox)fvExpense.FindControl("txtReceiptAmount");
        TextBox txtChequeNo = (TextBox)fvExpense.FindControl("txtChequeNo");
        TextBox txtReceiptDate = (TextBox)fvExpense.FindControl("txtReceiptDate");
        TextBox txtChequeDate = (TextBox)fvExpense.FindControl("txtChequeDate");
        TextBox txtRemark = (TextBox)fvExpense.FindControl("txtRemark");

        int ExpenseType = Convert.ToInt32(ddlExpenseType.SelectedValue);

        if (txtEAmount.Text.Trim() != "")
        {
            eAmount = Convert.ToSingle(txtEAmount.Text.Trim());
        }

        string reciepyNo = txtReceiptNo.Text.Trim();
        string paidTo = txtPaidTo.Text.Trim();
        int ReceiptType = Convert.ToInt32(ddReceipt.SelectedValue);
        string Location = txtLocation.Text.Trim();
        int PaymentType = Convert.ToInt32(ddPaymentType.SelectedValue);
        string ReceiptAmount = txtReceiptAmount.Text.Trim();
        string ChequeNo = txtChequeNo.Text.Trim();
        string strRemark = txtRemark.Text.Trim();

        if (txtReceiptDate.Text.Trim() != "")
        {
            dtReceiptDate = Commonfunctions.CDateTime(txtReceiptDate.Text.Trim());

            if (dtReceiptDate < dtMinValue)
            {
                lblResult.Text = "Invalid Receipt Date!";
                lblResult.CssClass = "errorMsg";
                return;
            }
        }
        if (txtChequeDate.Text.Trim() != "")
        {
            dtChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text.Trim());

            if (dtChequeDate < dtMinValue)
            {
                lblResult.Text = "Invalid Cheque Date!";
                lblResult.CssClass = "errorMsg";
                return;
            }
        }

        if (ddBillable.SelectedIndex > 0)
        {
            Convert.ToBoolean(Convert.ToInt32(ddBillable.SelectedValue));
        }

        int outVal = DBOperations.AddJobExpensesDetails(JobId, ExpenseType, eAmount, reciepyNo, paidTo, ReceiptType,
            Location, PaymentType, ReceiptAmount, dtReceiptDate, ChequeNo, dtChequeDate, IsBillable, strRemark,0, LoggedInUser.glUserId);

        if (outVal == 0)
        {
            lblResult.Text = "Expense Detail Added Successfully";
            lblResult.CssClass = "success";
            ddlExpenseType.SelectedIndex = -1;
            txtEAmount.Text = string.Empty;
            ddReceipt.SelectedIndex = -1;
            txtReceiptAmount.Text = "";
            txtPaidTo.Text = "";
            ddBillable.SelectedIndex = -1;
            txtChequeNo.Text = "";
            txtChequeDate.Text = "";

            ddPaymentType.SelectedIndex = -1;

            gvExpenseDetails.DataSourceID = "DataJobExpenseDetails";
            gvExpenseDetails.DataBind();
            Page.Validate();
        }
        else if (outVal == 1)
        {
            lblResult.Text = "System Error! Please Try After Sometime.";
            lblResult.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            lblResult.Text = "Expense Detail Already Exist.";
            lblResult.CssClass = "errorMsg";
        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DateTime dtClose = DateTime.MinValue;

        int ExpenseId = Convert.ToInt32(fvExpense.SelectedValue);

        float eAmount = 0;

        DropDownList ddlExpenseType = (DropDownList)fvExpense.FindControl("ddlUPDExpenseType");
        DropDownList ddReceipt = (DropDownList)fvExpense.FindControl("ddUPDReceipt");
        DropDownList ddPaymentType = (DropDownList)fvExpense.FindControl("ddUPDPaymentType");
        DropDownList ddBillable = (DropDownList)fvExpense.FindControl("ddUPDBillable");

        TextBox txtEAmount = (TextBox)fvExpense.FindControl("txtUPDExAmount");
        TextBox txtReceiptNo = (TextBox)fvExpense.FindControl("txtUPDReceiptNo");
        TextBox txtPaidTo = (TextBox)fvExpense.FindControl("txtUPDPaidTo");
        TextBox txtLocation = (TextBox)fvExpense.FindControl("txtUPDLocation");
        TextBox txtReceiptAmount = (TextBox)fvExpense.FindControl("txtUPDReceiptAmount");
        TextBox txtChequeNo = (TextBox)fvExpense.FindControl("txtUPDChequeNo");
        TextBox txtReceiptDate = (TextBox)fvExpense.FindControl("txtUPDReceiptDate");
        TextBox txtChequeDate = (TextBox)fvExpense.FindControl("txtUPDChequeDate");
        TextBox txtUPDRemark = (TextBox)fvExpense.FindControl("txtUPDRemark");

        int ExpenseType = Convert.ToInt32(ddlExpenseType.SelectedValue);

        if (txtEAmount.Text.Trim() != "")
        {
            eAmount = Convert.ToSingle(txtEAmount.Text.Trim());
        }

        string reciepyNo = txtReceiptNo.Text.Trim();
        string paidTo = txtPaidTo.Text.Trim();
        int ReceiptType = Convert.ToInt32(ddReceipt.SelectedValue);
        string Location = txtLocation.Text.Trim();
        int PaymentType = Convert.ToInt32(ddPaymentType.SelectedValue);
        string ReceiptAmount = txtReceiptAmount.Text.Trim();
        string ChequeNo = txtChequeNo.Text.Trim();
        string strRemark = txtUPDRemark.Text.Trim();

        DateTime dtReceiptDate = DateTime.MinValue;
        DateTime dtChequeDate = DateTime.MinValue;

        if (txtReceiptDate.Text.Trim() != "")
        {
            dtReceiptDate = Commonfunctions.CDateTime(txtReceiptDate.Text.Trim());

            if (dtReceiptDate < DateTime.MinValue)
            {
                lblResult.Text = "Invalid Receipt Date!";
                lblResult.CssClass = "success";
                return;
            }
        }
        if (txtChequeDate.Text.Trim() != "")
        {
            dtChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text.Trim());

        }

        bool IsBillable = Convert.ToBoolean(Convert.ToInt32(ddBillable.SelectedValue));

        int outVal = DBOperations.UpdateJobExpensesDetails(ExpenseId, ExpenseType, eAmount, reciepyNo, paidTo, ReceiptType,
                 Location, PaymentType, ReceiptAmount, dtReceiptDate, ChequeNo, dtChequeDate, IsBillable,"",0,  LoggedInUser.glUserId);

        if (outVal == 0)
        {
            gvExpenseDetails.SelectedIndex = -1;
            fvExpense.ChangeMode(FormViewMode.ReadOnly);
            fvExpense.DataBind();

            lblResult.Text = "Expense Detail Updated Successfully";
            lblResult.CssClass = "success";

            gvExpenseDetails.DataSourceID = "DataJobExpenseDetails";
            gvExpenseDetails.DataBind();

        }
        else if (outVal == 1)
        {
            lblResult.Text = "System Error! Please Try After Sometime.";
            lblResult.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            lblResult.Text = "Expense Detail Already Exist.";
            lblResult.CssClass = "errorMsg";
        }

    }

    protected void btnNewCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PendingJobExpense.aspx");
    }

    protected void btnUpdCancel_OnClick(object sender, EventArgs e)
    {
        gvExpenseDetails.SelectedIndex = -1;
        fvExpense.ChangeMode(FormViewMode.ReadOnly);
        fvExpense.DataBind();
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("PendingJobExpense.aspx");
    }

    #endregion

    protected void gvExpenseDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            int ExpenseId = Convert.ToInt32(e.CommandArgument.ToString());
            DataSet dsExpense = EXOperations.EX_GetExpenseDetailBylId(ExpenseId);

            fvExpense.ChangeMode(FormViewMode.Edit);
            fvExpense.DataSource = dsExpense;
            fvExpense.DataBind();
        }
    }

    #region Print
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    //protected void PrintAllPages(object sender, EventArgs e)
    //{
    //    gvExpenseDetails.AllowPaging = false;
    //    gvExpenseDetails.DataBind();
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);
    //    gvExpenseDetails.RenderControl(hw);
    //    string gridHTML = sw.ToString().Replace("\"", "'")
    //        .Replace(System.Environment.NewLine, "");
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append("<script type = 'text/javascript'>");
    //    sb.Append("window.onload = new function(){");
    //    sb.Append("var printWin = window.open('', '', 'left=0");
    //    sb.Append(",top=0,width=1000,height=600,status=0');");
    //    sb.Append("printWin.document.write(\"");
    //    sb.Append(gridHTML);
    //    sb.Append("\");");
    //    sb.Append("printWin.document.close();");
    //    sb.Append("printWin.focus();");
    //    sb.Append("printWin.print();");
    //    sb.Append("printWin.close();};");
    //    sb.Append("</script>");
    //    ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
    //    gvExpenseDetails.AllowPaging = true;
    //    gvExpenseDetails.DataBind();
    //}


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvExpenseDetails.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=JobExpenseDetail-" + ViewState["JobNum"].ToString() + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                sw.Write("<br/>");
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvExpenseDetails.AllowPaging = false;
                gvExpenseDetails.Columns[1].Visible = false; // Edit Button
                gvExpenseDetails.DataBind();
                gvExpenseDetails.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 0, 0, 15, 15);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("Expense Job Number: " + ViewState["JobNum"].ToString() + "  " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")));
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                //Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblResult.Text = "There is no expense record to print!";
                lblResult.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = "Print Functionality Requires Full Trust Level On Server!";
            lblResult.CssClass = "errorMsg";
        }

    }

    #endregion
}