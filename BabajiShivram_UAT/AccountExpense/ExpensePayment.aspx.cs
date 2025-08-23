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
using System.IO;
using System.Collections.Generic;
using System.Drawing;

public partial class AccountExpense_ExpensePayment : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Debit"] = 0;
        ViewState["Credit"] = 0;
        ScriptManager1.RegisterPostBackControl(gvExpenseDetails);
        ScriptManager1.RegisterPostBackControl(lnkAdd);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Expense Payment Details";

            lblError.Visible = false;
            GetPaymentExpenses();

            // Get Voucher No
            txtVoucherNo.Text = AccountExpense.GenerateVoucherNo();
            txtDate.Text = DateTime.Now.ToShortDateString();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["PaymentId"]) != null)
        {
            string strVoucherNo = "", strRate = "", strChequeNo = "", strBankName = "", strFavouring = "", strNarration = "";
            DateTime dtDate = DateTime.Now, dtChequeDate = DateTime.MinValue;
            int CurrencyId = 0, PaymentId = 0, VendorCode = 0, result = -123;
            double Total = 0;

            if (txtChequeDate.Text != "")
                dtChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text);
            if (ddCurrency.SelectedValue != "0")
                CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
            if (hdnVendorCodeId.Value != "0")
                VendorCode = Convert.ToInt32(hdnVendorCodeId.Value);

            PaymentId = Convert.ToInt32(Session["PaymentId"]);
            strVoucherNo = txtVoucherNo.Text.Trim();
            strRate = txtRate.Text.Trim();
            strChequeNo = txtChequeNo.Text.Trim();
            strBankName = txtBankName.Text.Trim();
            strFavouring = txtFavouring.Text.Trim();
            strNarration = txtNarration.Text.Trim();

            result = 0; // AccountExpense.AddPaymentDetails(PaymentId, strVoucherNo, VendorCode, CurrencyId, strRate, strChequeNo,
               // dtChequeDate, strBankName, strFavouring, strNarration, Total, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Successfully Added Job Payment Expense Details.";
                lblError.CssClass = "success";
            }
            else if (result == 2)
            {
                lblError.Text = "Record already exists.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while inserting record. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void GetPaymentExpenses()
    {
        DataSet ds = new DataSet();
        ds = AccountExpense.GetPaymentExpenses(Convert.ToInt32(Session["PaymentId"]));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvExpenseDetails.DataSource = ds;
            gvExpenseDetails.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvExpenseDetails.DataSource = ds;
            gvExpenseDetails.DataBind();
            //int columncount = gvExpenseDetails.Rows[0].Cells.Count;
            //gvExpenseDetails.Rows[0].Cells.Clear();
            //gvExpenseDetails.Rows[0].Cells.Add(new TableCell());
            //gvExpenseDetails.Rows[0].Cells[0].ColumnSpan = columncount;
            //gvExpenseDetails.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    #region VENDOR CODE

    protected void txtVendorCode_TextChanged(object sender, EventArgs e)
    {
        if (txtVendorCode.Text.Trim() != "")
        {
            if (hdnVendorCodeId.Value != "0")
            {
                DataSet dsGetVendorName = AccountExpense.GetVendorNameById(Convert.ToInt32(hdnVendorCodeId.Value));
                if (dsGetVendorName != null)
                {
                    if (dsGetVendorName.Tables[0].Rows[0]["sName"] != DBNull.Value)
                        lblVendorCodeName.Text = dsGetVendorName.Tables[0].Rows[0]["sName"].ToString();
                }
            }
        }
    }

    #endregion

    #region ACCOUNT CODE  

    protected void txtAcCode_footer_TextChanged(object sender, EventArgs e)
    {
        int ACCodeId = 0;
        //TextBox txtACCode = (TextBox)sender;
        //GridViewRow currentRow = (GridViewRow)txtACCode.Parent.Parent;

        // TextBox txtAcName_footer = (TextBox)currentRow.FindControl("txtAcName_footer");
        if (txtAcName_footer.Text != null)
        {
            if (hdnACCodeId.Value != "0")
                ACCodeId = Convert.ToInt32(hdnACCodeId.Value);

            DataSet dsGetACCodeName = AccountExpense.GetAccountNameByCodeId(ACCodeId);
            if (dsGetACCodeName != null)
            {
                if (dsGetACCodeName.Tables[0].Rows[0]["sName"] != DBNull.Value)
                {
                    txtAcName_footer.Text = dsGetACCodeName.Tables[0].Rows[0]["sName"].ToString();
                }
            }
        }
    }

    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        int ACCodeId = 0;
        TextBox txtACCode = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)txtACCode.Parent.Parent;

        TextBox txtAcName = (TextBox)currentRow.FindControl("txtAcName");
        if (txtAcName != null)
        {
            if (hdnACCodeId.Value != "0")
                ACCodeId = Convert.ToInt32(hdnACCodeId.Value);

            DataSet dsGetACCodeName = AccountExpense.GetAccountNameByCodeId(ACCodeId);
            if (dsGetACCodeName != null)
            {
                if (dsGetACCodeName.Tables[0].Rows[0]["sName"] != DBNull.Value)
                {
                    txtAcName.Text = dsGetACCodeName.Tables[0].Rows[0]["sName"].ToString();
                }
            }
        }
    }

    #endregion

    #region PAYMENT EXPENSES

    protected void gvExpenseDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Debit Cell
            Label lblDebit = (Label)e.Row.Cells[1].FindControl("lblDebit");
            if (lblDebit != null)
            {
                if (lblDebit.Text.Trim() != "")
                {
                    Decimal dcDebit = Convert.ToDecimal(lblDebit.Text);
                    ViewState["Debit"] = Convert.ToDecimal(ViewState["Debit"]) + dcDebit;
                }
            }

            // Credit Cell
            Label lblCredit = (Label)e.Row.Cells[1].FindControl("lblCredit");
            if (lblCredit != null)
            {
                if (lblCredit.Text.Trim() != "")
                {
                    Decimal dcCredit = Convert.ToDecimal(lblCredit.Text);
                    ViewState["Credit"] = Convert.ToDecimal(ViewState["Credit"]) + dcCredit;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.BackColor = System.Drawing.Color.FromName("#cbcbdc");
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[0].ColumnSpan = 1;
            e.Row.Cells[4].Text = "<b>" + ViewState["Debit"].ToString() + "</b>";
            e.Row.Cells[5].Text = "<b>" + ViewState["Credit"].ToString() + "</b>";
        }
    }

    protected void gvExpenseDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Visible = true;
        if (e.CommandName == "Insert")
        {
            int ACCodeId = 0, JobId = 0;
            double dcDebit = 0, dcCredit = 0, number = 0;
            if (hdnACCodeId.Value != "0")
                ACCodeId = Convert.ToInt32(hdnACCodeId.Value);
            if (hdnJobId.Value != "0")
                JobId = Convert.ToInt32(hdnJobId.Value);
            TextBox txtAcCode_footer = gvExpenseDetails.FooterRow.FindControl("txtAcCode_footer") as TextBox;
            TextBox txtAcName_footer = gvExpenseDetails.FooterRow.FindControl("txtAcName_footer") as TextBox;
            TextBox txtJobNo_footer = gvExpenseDetails.FooterRow.FindControl("txtJobNo_footer") as TextBox;
            TextBox txtDebit_footer = gvExpenseDetails.FooterRow.FindControl("txtDebit_footer") as TextBox;
            TextBox txtCredit_footer = gvExpenseDetails.FooterRow.FindControl("txtCredit_footer") as TextBox;

            if (txtDebit_footer.Text.Trim() != "")
            {
                if (Double.TryParse(txtDebit_footer.Text.Trim(), out number))
                    dcDebit = Convert.ToDouble(txtDebit_footer.Text.Trim());
            }

            if (txtCredit_footer.Text.Trim() != "")
            {
                if (Double.TryParse(txtCredit_footer.Text.Trim(), out number))
                    dcCredit = Convert.ToDouble(txtCredit_footer.Text.Trim());
            }

            if (txtAcCode_footer.Text.Trim() != "" && txtAcCode_footer.Text.Trim() != "")
            {
                int result = AccountExpense.AddPaymentExpenses(Convert.ToInt32(Session["PaymentId"]), ACCodeId, JobId, dcDebit, dcCredit, LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Payment expense added successfully.";
                    lblError.CssClass = "success";
                    GetPaymentExpenses();
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Expense Already Exist!";
                    lblError.CssClass = "warning";
                }

            }//END_IF
            else
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvExpenseDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvExpenseDetails.DataKeys[e.RowIndex].Value.ToString());
        int ACCodeId = 0, JobId = 0;
        double dcDebit = 0, dcCredit = 0, number = 0;
        if (hdnACCodeId.Value != "0")
            ACCodeId = Convert.ToInt32(hdnACCodeId.Value);
        if (hdnJobId.Value != "0")
            JobId = Convert.ToInt32(hdnJobId.Value);

        TextBox txtAcCode = gvExpenseDetails.Rows[e.RowIndex].FindControl("txtAcCode") as TextBox;
        TextBox txtAcName = gvExpenseDetails.Rows[e.RowIndex].FindControl("txtAcName") as TextBox;
        TextBox txtJobNo = gvExpenseDetails.Rows[e.RowIndex].FindControl("txtJobNo") as TextBox;
        TextBox txtDebit = gvExpenseDetails.Rows[e.RowIndex].FindControl("txtDebit") as TextBox;
        TextBox txtCredit = gvExpenseDetails.Rows[e.RowIndex].FindControl("txtCredit") as TextBox;

        if (txtDebit.Text.Trim() != "")
        {
            if (Double.TryParse(txtDebit.Text.Trim(), out number))
                dcDebit = Convert.ToDouble(txtDebit.Text.Trim());
        }

        if (txtCredit.Text.Trim() != "")
        {
            if (Double.TryParse(txtCredit.Text.Trim(), out number))
                dcCredit = Convert.ToDouble(txtCredit.Text.Trim());
        }

        if (txtAcCode.Text.Trim() != "" && txtAcCode.Text.Trim() != "")
        {
            int result = AccountExpense.UpdatePaymentExpenses(Lid, JobId, dcDebit, dcCredit, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Payment expense updated successfully.";
                lblError.CssClass = "success";
                gvExpenseDetails.EditIndex = -1;
                GetPaymentExpenses();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Expense Already Exist!";
                lblError.CssClass = "warning";
            }

        }//END_IF
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please fill all the details!";
        }
    }

    protected void gvExpenseDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblError.Visible = true;
        int lid = Convert.ToInt32(gvExpenseDetails.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = AccountExpense.DeletePaymentExpenses(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Expense Deleted Successfully!";
            lblError.CssClass = "success";
            GetPaymentExpenses();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvExpenseDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {

        gvExpenseDetails.EditIndex = e.NewEditIndex;
        GetPaymentExpenses();
        lblError.Text = "";
        lblError.Visible = false;
    }

    protected void gvExpenseDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvExpenseDetails.EditIndex = -1;

        GetPaymentExpenses();
        lblError.Text = "";
        lblError.Visible = false;
    }

    protected void gvExpenseDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvExpenseDetails.PageIndex = e.NewPageIndex;

        gvExpenseDetails.DataBind();
        GetPaymentExpenses();
    }

    protected void lnkAdd_OnClick(object sender, EventArgs e)
    {
        int ACCodeId = 0, JobId = 0;
        double dcDebit = 0, dcCredit = 0, number = 0;
        if (hdnACCodeId.Value != "0")
            ACCodeId = Convert.ToInt32(hdnACCodeId.Value);
        if (hdnJobId.Value != "0")
            JobId = Convert.ToInt32(hdnJobId.Value);
        //TextBox txtAcCode_footer = gvExpenseDetails.FooterRow.FindControl("txtAcCode_footer") as TextBox;
        //TextBox txtAcName_footer = gvExpenseDetails.FooterRow.FindControl("txtAcName_footer") as TextBox;
        //TextBox txtJobNo_footer = gvExpenseDetails.FooterRow.FindControl("txtJobNo_footer") as TextBox;
        //TextBox txtDebit_footer = gvExpenseDetails.FooterRow.FindControl("txtDebit_footer") as TextBox;
        //TextBox txtCredit_footer = gvExpenseDetails.FooterRow.FindControl("txtCredit_footer") as TextBox;

        if (txtDebit_footer.Text.Trim() != "")
        {
            if (Double.TryParse(txtDebit_footer.Text.Trim(), out number))
                dcDebit = Convert.ToDouble(txtDebit_footer.Text.Trim());
        }

        if (txtCredit_footer.Text.Trim() != "")
        {
            if (Double.TryParse(txtCredit_footer.Text.Trim(), out number))
                dcCredit = Convert.ToDouble(txtCredit_footer.Text.Trim());
        }

        if (txtAcCode_footer.Text.Trim() != "" && txtAcCode_footer.Text.Trim() != "")
        {
            int result = AccountExpense.AddPaymentExpenses(Convert.ToInt32(Session["PaymentId"]), ACCodeId, JobId, dcDebit, dcCredit, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Payment expense added successfully.";
                lblError.CssClass = "success";
                GetPaymentExpenses();
                txtAcCode_footer.Text = "";
                txtAcName_footer.Text = "";
                txtJobNo_footer.Text = "";
                txtDebit_footer.Text = "";
                txtCredit_footer.Text = "";
                hdnACCodeId.Value = "0";
                hdnJobId.Value = "0";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Expense Already Exist!";
                lblError.CssClass = "warning";
            }

        }//END_IF
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please fill all the details!";
        }
    }

    #endregion

    #region ACCOUNT NAME

    protected void txtAcName_TextChanged(object sender, EventArgs e)
    {
        int ACCodeId = 0;
        TextBox txtAcName = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)txtAcName.Parent.Parent;

        TextBox txtAcCode = (TextBox)currentRow.FindControl("txtAcCode");
        if (txtAcCode != null)
        {
            if (hdnACCodeId.Value != "0")
                ACCodeId = Convert.ToInt32(hdnACCodeId.Value);

            DataSet dsGetACCodeName = AccountExpense.GetAccountNameByCodeId(ACCodeId);
            if (dsGetACCodeName != null)
            {
                if (dsGetACCodeName.Tables[0].Rows[0]["sCode"] != DBNull.Value)
                {
                    txtAcCode.Text = dsGetACCodeName.Tables[0].Rows[0]["sCode"].ToString();
                }
            }
        }
    }

    protected void txtAcName_footer_TextChanged(object sender, EventArgs e)
    {
        int ACCodeId = 0;
        //TextBox txtACNameFooter = (TextBox)sender;
        //GridViewRow currentRow = (GridViewRow)txtACNameFooter.Parent.Parent;

        //TextBox txtAcCode_footer = (TextBox)currentRow.FindControl("txtAcCode_footer");
        if (txtAcCode_footer.Text != null)
        {
            if (hdnACCodeId.Value != "0")
                ACCodeId = Convert.ToInt32(hdnACCodeId.Value);

            DataSet dsGetACCodeName = AccountExpense.GetAccountNameByCodeId(ACCodeId);
            if (dsGetACCodeName != null)
            {
                if (dsGetACCodeName.Tables[0].Rows[0]["sCode"] != DBNull.Value)
                {
                    txtAcCode_footer.Text = dsGetACCodeName.Tables[0].Rows[0]["sCode"].ToString();
                }
            }
        }
    }

    #endregion
}