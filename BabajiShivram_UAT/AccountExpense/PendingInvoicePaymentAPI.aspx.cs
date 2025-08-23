using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;
public partial class AccountExpense_PendingInvoicePaymentAPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["InvoiceId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending API Payment";

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
        DataFilter1.FilterSessionID = "PendingInvoicePaymentAPI.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strInvoiceId = (string)e.CommandArgument;

            Session["InvoiceId"] = strInvoiceId;

            Response.Redirect("InvoicePaymentAPI.aspx"); ;
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
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsBilled")) == true)
            {
                e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
                e.Row.ToolTip = "Billed Job";
            }
            if (DataBinder.Eval(e.Row.DataItem, "lStatus") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "lStatus").ToString());
                string strStatusRemark = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();

                if (StatusId == 149) // Payment On Hold - 
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    e.Row.ToolTip = strStatusRemark;
                }
            }
        }
    }
    protected void gvBJVDetail_PreRender(object sender, EventArgs e)
    {
        lblProfitMessage.Text = "";
        if (gvBJVDetail.Rows.Count > 1)
        {
            GridViewRow getRow = gvBJVDetail.Rows[gvBJVDetail.Rows.Count - 1];
            getRow.Cells[7].Font.Bold = true;
            getRow.Cells[8].Font.Bold = true;

            decimal decDebit = 0m;
            decimal decCredit = 0m;
            decimal decProfit = 0m;

            Decimal.TryParse(getRow.Cells[7].Text.Trim(), out decDebit);
            Decimal.TryParse(getRow.Cells[8].Text.Trim(), out decCredit);

            decProfit = (decCredit - decDebit);

            if (decCredit > 0)
            {
                if (decProfit <= 0)
                {
                    // getRow.Cells[8].BackColor = System.Drawing.Color.MediumVioletRed;
                    lblProfitMessage.Text = "Loss: " + decProfit.ToString();
                    lblProfitMessage.ForeColor = System.Drawing.Color.MediumVioletRed;
                    getRow.Cells[9].Text = "Loss: " + decProfit.ToString();
                    getRow.Cells[9].BackColor = System.Drawing.Color.MediumVioletRed;
                    getRow.Cells[9].ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    lblProfitMessage.Text = "Profit: " + decProfit.ToString();
                    lblProfitMessage.ForeColor = System.Drawing.Color.Green;

                    getRow.Cells[9].Text = "Profit: " + decProfit.ToString();
                    getRow.Cells[9].BackColor = System.Drawing.Color.Green;
                    getRow.Cells[9].ForeColor = System.Drawing.Color.White;
                }
            }

        }
    }
    protected void btnCancelBJVPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
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
            DataFilter1.FilterSessionID = "PendingInvoicePaymentAPI.aspx";
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
        string strFileName = "PendingInvoicePayment" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        DataFilter1.FilterSessionID = "PendingInvoicePaymentAPI.aspx";
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
}