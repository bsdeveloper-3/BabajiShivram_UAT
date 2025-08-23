using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Reports_DDP_DSR_KPI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "DDP KPI";

        }
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        BindFilter();
    }
    private void BindFilter()
    {
        string strFilter = "";

        DataSourceReport.FilterExpression = "";
        DateTime dtFromDate = DateTime.MinValue;
        DateTime dtToDate = DateTime.MinValue;

        if (txtFromDate.Text.Trim() != "")
        {
            dtFromDate = Commonfunctions.CDateTime(txtFromDate.Text.Trim());
        }
        if (txtToDate.Text.Trim() != "")
        {
            dtToDate = Commonfunctions.CDateTime(txtToDate.Text.Trim());
        }

        if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;

            DataSourceReport.DataBind();
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;

            DataSourceReport.DataBind();
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex == 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;

            DataSourceReport.DataBind();

        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;

            DataSourceReport.DataBind();
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;

            DataSourceReport.DataBind();
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;

        }
        else
        {
            strFilter = "";

            DataSourceReport.FilterExpression = "";

            DataSourceReport.DataBind();
            gvReport.DataBind();
        }

    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddClearedStatus.SelectedIndex = 0;

        DataSourceReport.FilterExpression = "";

        DataSourceReport.DataBind();
        gvReport.DataBind();

    }

    #region Export To Excel

    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "DDP_DSR_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExcelExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvReport.AllowPaging = false;
        gvReport.AllowSorting = false;

        //gvReport.DataSourceID = "DataSourceReport";
                
        //gvReport.DataBind();

        BindFilter();

        //Remove Controls
        this.RemoveControls(gvReport);

        gvReport.RenderControl(hw);

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

    #endregion

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType== DataControlRowType.DataRow)
        {
            bool bClearStatus = false;

            if(DataBinder.Eval(e.Row.DataItem, "ClearedStatusName") != DBNull.Value)
            {
                if (DataBinder.Eval(e.Row.DataItem, "ClearedStatusName").ToString() == "Cleared")
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                }
            }
        }
    }
}