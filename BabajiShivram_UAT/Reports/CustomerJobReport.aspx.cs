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
public partial class Reports_CustomerJobReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Report";
        }
    }

    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        BindGridView();
    }
    private void BindGridView()
    {
        DataSet dsReport;
        string strFieldQuery = "";
        int lastindex = 0;
        foreach (ListItem lstField in chkField.Items)
        {
            if (lstField.Selected)
            {
                strFieldQuery += lstField.Value + ",";
            }
        }
        if (strFieldQuery != "")
        {
            lastindex = strFieldQuery.LastIndexOf(",");

            strFieldQuery = strFieldQuery.Remove(lastindex);

            ReportSqlDataSource.SelectParameters["ColumnList"].DefaultValue = strFieldQuery;

            //  ReportSqlDataSource.Select();

            /*
            dsReport = ReportOperations.GetCustomerQueryField(strFieldQuery);

            if (dsReport.Tables[0].Rows.Count > 0)
            {
                gvReportField.DataSource = dsReport;
                gvReportField.DataBind();
            }
            else
            {
                lblError.Text = "No Record Found!";
                lblError.CssClass = "errorMsg";
            }
            */
        }
        else
        {
            lblError.Text = "Please Select Field For Report!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvReportField_Sorting(Object Sender, GridViewSortEventArgs e)
    {
        string sortColumn = e.SortExpression;
        e.SortDirection = SortDirection.Ascending;

        BindGridView();
    }

    protected void gvReportField_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void btnCancel_Click(Object Sender, EventArgs e)
    {
        Response.Redirect("~/Reports/CustomerJobReport.aspx");
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        ExportFunction("attachment;filename=JobReport.xls", "application/vnd.ms-excel");
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

        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvReportField.AllowPaging = false;
        gvReportField.AllowSorting = false;
        gvReportField.AutoGenerateEditButton = false;
        BindGridView();

        gvReportField.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}
