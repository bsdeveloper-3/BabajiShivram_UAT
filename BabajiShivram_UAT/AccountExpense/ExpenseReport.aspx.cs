using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data.OleDb;
using System.Linq;
using System.Collections.Generic;


public partial class AccountExpense_ExpenseReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    static int k = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Expense Report";
        }
    }
    protected void btnDetail_Click(object sender, EventArgs e)
    {
        gridAllPayment.Visible = true;
        gridDetailReport.Visible = true;
        gridDetailReport.DataSource = null;
        gridDetailReport.DataBind();
        gridAllPayment.DataSource = null;
        gridAllPayment.DataBind();
        DateTime dtFrom = DateTime.MinValue;
        DateTime dtTo = DateTime.MinValue;

        if (chkToday.Checked == true)
        {
            string strToday = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtfrom.Text = strToday;
            txtTo.Text = strToday;
        }
        if (txtfrom.Text.Trim() != "")
        {
            dtFrom = Convert.ToDateTime(txtfrom.Text.Trim());
        }
        if (txtTo.Text.Trim() != "")
        {
            dtTo = Convert.ToDateTime(txtTo.Text.Trim());
        }

        if (dtFrom <= dtTo)
        {
            int lUserId = Convert.ToInt32(LoggedInUser.glUserId);
            DataSet dsTodayDutyDetail = new DataSet();
            DataSet dsAllPaymentDetails = new DataSet();
            int TodaysRpt = 0;

            int ExpenseType = Convert.ToInt32(ddlExpenseType.SelectedValue);
            if (chkToday.Checked == true)
            {
                TodaysRpt = 1;
            }

            if (TodaysRpt == 1 || ddlExpenseType.SelectedValue == "1")
            {                
                dsTodayDutyDetail = AccountExpense.GetTodaysDutyAmountDetails(lUserId, dtFrom, dtTo, ExpenseType);
            }
            else if (ddlExpenseType.SelectedValue != "0")
            {                
                dsAllPaymentDetails = AccountExpense.GetExpenseRPTAllPayment(lUserId, dtFrom, dtTo, ExpenseType);
            }

            if (dsTodayDutyDetail.Tables.Count > 0)
            {
                k = 0;
                DataTable dtTodayDutyDetail = new DataTable();
                dtTodayDutyDetail = dsTodayDutyDetail.Tables[0];
                ViewState["AnnexureDoc1"] = dtTodayDutyDetail;
                gridDetailReport.DataSource = dtTodayDutyDetail;
                gridDetailReport.DataBind();
                gridDetailReport.PageIndex = 0;
                gridAllPayment.Visible = false;
            }
            else if (dsAllPaymentDetails.Tables.Count > 0)
            {
                k = 1;
                DataTable dtAllPaymentDetails = new DataTable();
                dtAllPaymentDetails = dsAllPaymentDetails.Tables[0];
                ViewState["AnnexureDoc2"] = dtAllPaymentDetails;
                gridAllPayment.DataSource = dtAllPaymentDetails;
                gridAllPayment.DataBind();
                gridAllPayment.PageIndex = 0;
                gridDetailReport.Visible = false;
            }
            else
            {
                lblError.Text = "Details are not found";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {         
            gridDetailReport.Visible = false;
            lblError.Text = "From Date Is Not Greater Than To Date";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void gridDetailReport_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {       
        gridDetailReport.PageIndex = e.NewPageIndex;
        DataTable dt1 = ViewState["AnnexureDoc1"] as DataTable;     
        gridDetailReport.DataSource = dt1;
        gridDetailReport.DataBind();
    }

    protected void gridAllPayment_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridAllPayment.PageIndex = e.NewPageIndex;
        DataTable dt1 = ViewState["AnnexureDoc2"] as DataTable;        
        gridAllPayment.DataSource = dt1;
        gridAllPayment.DataBind();
    }


    protected void lnkExportReport_Click(object sender, EventArgs e)
    {
        if (k == 0)
        {
            string strFileName = "Expense Duty Details_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            DutySummaryExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
        }
        else if(k==1)
        {
            string strFileName = "Expense Details_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            PaymentSummaryExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
        }
    }

    private void DutySummaryExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gridDetailReport.AllowPaging = false;
        gridDetailReport.AllowSorting = false;
        gridDetailReport.Caption = "";
        DataTable dtsummary = ViewState["AnnexureDoc1"] as DataTable;
        gridDetailReport.DataSource = dtsummary;
        gridDetailReport.DataBind();
        //Remove Controls
        this.RemoveControls(gridDetailReport);
        gridDetailReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void PaymentSummaryExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gridAllPayment.AllowPaging = false;
        gridAllPayment.AllowSorting = false;
        gridAllPayment.Caption = "";
        DataTable dtsummary = ViewState["AnnexureDoc2"] as DataTable;
        gridAllPayment.DataSource = dtsummary;
        gridAllPayment.DataBind();
        //Remove Controls
        this.RemoveControls(gridAllPayment);
        gridAllPayment.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }


}