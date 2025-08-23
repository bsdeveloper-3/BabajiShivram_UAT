using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class AccountExpense_InvoiceRejected : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["InvoiceId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vendor Invoice Rejected";

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
        DataFilter1.FilterSessionID = "InvoiceRejected.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        txtCancelReason.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strInvoiceId = (string)e.CommandArgument;

            Session["InvoiceId"] = strInvoiceId;

            Response.Redirect("PaymentRequestEdit2.aspx");
        }
        else if (e.CommandName.ToLower() == "cancel")
        {
            lblPopupName.Text = "Cancel Invoice";

            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = InvoiceID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                /*************************************************************
                  
                if (dsGetPaymentDetails.Tables[0].Rows[0]["ProformaInvoiceId"] != null)
                {
                    if (Convert.ToInt32(dsGetPaymentDetails.Tables[0].Rows[0]["ProformaInvoiceId"]) > 0)
                    {
                        lblMessage.Text = "Proforma To Final Invoice not allowed for Cancellation";
                        lblMessage.CssClass = "errorMsg";
                        hdnInvoiceId.Value = "0";
                        CancelModalPopupExtender.Hide();

                        return;
                    }
                }

                **********************************************************/

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

                CancelModalPopupExtender.Show();
            }

        }
    }

    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 13478 || LoggedInUser.glUserId == 12158 || LoggedInUser.glUserId == 5376)
            {
                LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");

                if (lnkCancel != null)
                {
                    lnkCancel.Visible = true;
                }
            }

        }
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
            DataFilter1.FilterSessionID = "InvoiceRejected.aspx";
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
        string strFileName = "Invoice_Rejected" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvDetail.Columns[2].Visible = true;

        gvDetail.Columns[gvDetail.Columns.Count-1].Visible = false; // Cancel Button

        DataFilter1.FilterSessionID = "InvoiceRejected.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        //-- gvJobDetail.DataSourceID = "PendingIGMSqlDataSource";
        //-- gvJobDetail.DataBind();
        // BindGridData();
        //gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //// gvJobDetail.HeaderRow.Cells[0].Visible = false;
        //for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //{
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //}

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    protected void btnCancelJob_Click(object sender, EventArgs e)
    {
        if (txtCancelReason.Text.Trim() != "")
        {
            int InvoiceID = Convert.ToInt32(hdnInvoiceId.Value);
            if (InvoiceID > 0)
            {
                {
                    int result = AccountExpense.AddInvoiceStatus(InvoiceID, 105, txtCancelReason.Text.Trim(), LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblMessage.Text = "Invoice Cancelled!";
                        lblMessage.CssClass = "success";
                        gvDetail.DataBind();
                    }
                    else if (result == 2)
                    {
                        lblMessage.Text = "Invoice Already Cancelled!";
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
            lblError_CancelExp.Text = "Please enter reason for Cancel!";
            lblError_CancelExp.CssClass = "errorMsg";
            txtCancelReason.Focus();
            CancelModalPopupExtender.Show();
        }
    }
}