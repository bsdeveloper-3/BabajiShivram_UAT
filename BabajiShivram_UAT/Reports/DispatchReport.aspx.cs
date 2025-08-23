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
public partial class Reports_DispatchReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblGridMessage.Text = string.Empty;
        ViewState["BillDept"] = 0;
        ViewState["PCA"] = 0;
        divTotalCount.Visible = false;
    }
     
    protected void btnSummary_Click(object sender, EventArgs e)
    {
        divTotalCount.Visible = true;
        lnkDispatchDetail.Visible = false;
        lblGridMessage.Visible = true;
        lblGridMessage.Text = "Dispatch Summary";
        gridReport.Visible = true;
        gridDetailReport.Visible = false;
        gridDetailReport.DataSource = null;
        gridDetailReport.DataBind();

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
            DataSet dsDispatch = new DataSet();
            DataSet dsCountDispatch = new DataSet();

            if (chkToday.Checked == true)
            {
                dsDispatch = DBOperations.GetTodaysDispatchRpt(dtFrom, dtTo);
                dsCountDispatch = DBOperations.GetTodaysCountDispatchRpt(dtFrom, dtTo);
            }
            else
            {
                dsDispatch = DBOperations.GetDispatchReport(dtFrom, dtTo);
                dsCountDispatch = DBOperations.GetCountDispatchReport(dtFrom, dtTo);
            }

            if (dsCountDispatch.Tables[0].Rows.Count > 0)
            {
                lblBillDeptTot.Text = dsCountDispatch.Tables[0].Rows[0][0].ToString();
                lblPCATot.Text = dsCountDispatch.Tables[0].Rows[0][1].ToString();

                if (lblBillDeptTot.Text == "")
                {
                    lblBillDeptTot.Text = "0";
                }
                if (lblPCATot.Text == "")
                {
                    lblPCATot.Text = "0";
                }
            }
            else
            {
                lblBillDeptTot.Text = "0";
                lblPCATot.Text = "0";
            }

            if (dsDispatch.Tables[0].Rows.Count > 0)
            {
                DataTable dtDispatch = new DataTable();
                dtDispatch = dsDispatch.Tables[0];

                ViewState["AnnexureDoc2"] = dtDispatch;

                gridReport.DataSource = dtDispatch;
                gridReport.DataBind();
                gridReport.PageIndex = 0;
                lnkDispatchReport.Visible = true;
            }
            else
            {
                lblError.Text = "Dispatch Summary are not found";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lnkDispatchReport.Visible = false;
            lblGridMessage.Visible = false;
            gridReport.Visible = false;
            gridDetailReport.Visible = false;
            lblError.Text = "From Date Is Not Greater Than To Date";
            lblError.CssClass = "errorMsg";
        }

    }
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        divTotalCount.Visible = true;
        lblGridMessage.Visible = true;
        lblGridMessage.Text = "Dispatch Summary";
        gridReport.PageIndex = e.NewPageIndex;
        DataTable dt = ViewState["AnnexureDoc2"] as DataTable;
        lnkDispatchReport.Visible = true;
        gridReport.DataSource = dt;
        gridReport.DataBind();
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        divTotalCount.Visible = false;
        lnkDispatchReport.Visible = false;
        lblGridMessage.Visible = true;
        lblGridMessage.Text = "Dispatch Details";
        gridDetailReport.Visible = true;
        gridReport.Visible = false;
        gridReport.DataSource = null;
        gridReport.DataBind();

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
            DataSet dsDispatchDetail = new DataSet();

            if (chkToday.Checked == true)
            {
                dsDispatchDetail = DBOperations.GetTodaysDispatchDetailRpt(dtFrom, dtTo);
            }
            else
            {
                dsDispatchDetail = DBOperations.GetDispatchDetailReport(dtFrom, dtTo);
            }

            if (dsDispatchDetail.Tables[0].Rows.Count > 0)
            {
                DataTable dtDispatchDetail = new DataTable();
                dtDispatchDetail = dsDispatchDetail.Tables[0];

                ViewState["AnnexureDoc1"] = dtDispatchDetail;

                gridDetailReport.DataSource = dtDispatchDetail;
                gridDetailReport.DataBind();
                gridDetailReport.PageIndex = 0;
                lnkDispatchDetail.Visible = true;
            }
            else
            {
                lblError.Text = "Dispatch Details are not found";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lnkDispatchDetail.Visible = false;
            lblGridMessage.Visible = false;
            gridReport.Visible = false;
            gridDetailReport.Visible = false;
            lblError.Text = "From Date Is Not Greater Than To Date";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void gridDetailReport_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // divTotalCount.Visible = false;
        lblGridMessage.Visible = true;
        lblGridMessage.Text = "Dispatch Details";
        gridDetailReport.PageIndex = e.NewPageIndex;
        DataTable dt1 = ViewState["AnnexureDoc1"] as DataTable;
        lnkDispatchDetail.Visible = true;
        gridDetailReport.DataSource = dt1;
        gridDetailReport.DataBind();
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

    protected void lnkDispatchReport_Click(object sender, EventArgs e)
    {
        string strFileName = "Dispatch Summary_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        DispatchSummaryExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void DispatchSummaryExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gridReport.AllowPaging = false;
        gridReport.AllowSorting = false;
        gridReport.Caption = "";

        DataTable dtsummary = ViewState["AnnexureDoc2"] as DataTable;

        gridReport.DataSource = dtsummary;
        gridReport.DataBind();

        //Remove Controls
        this.RemoveControls(gridReport);

        gridReport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkDispatchDetail_Click(object sender, EventArgs e)
    {
        string strFileName = "Dispatch Detail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        DispatchDetailExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void DispatchDetailExport(string header, string contentType)
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

        DataTable dtDetail = ViewState["AnnexureDoc1"] as DataTable;

        gridDetailReport.DataSource = dtDetail;
        gridDetailReport.DataBind();

        //Remove Controls
        this.RemoveControls(gridDetailReport);

        gridDetailReport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gridReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ClosingBalance = (Label)e.Row.Cells[1].FindControl("lblBillDept");
                Label lblPCA = (Label)e.Row.Cells[1].FindControl("lblPCA");

                if (ClosingBalance.Text != "")
                {
                    ViewState["BillDept"] = Convert.ToDecimal(ViewState["BillDept"]) + Convert.ToDecimal(ClosingBalance.Text);
                    ViewState["PCA"] = Convert.ToDecimal(ViewState["PCA"]) + Convert.ToDecimal(lblPCA.Text);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "<b>Total</b>";
                e.Row.Cells[2].Text = "<b>" + ViewState["BillDept"].ToString() + "</b>";
                e.Row.Cells[3].Text = "<b>" + ViewState["PCA"].ToString() + "</b>";
            }
        }
        catch
        {
        }
    }

    protected void chkToday_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkToday.Checked == true)
        {
            
        }
    }
}