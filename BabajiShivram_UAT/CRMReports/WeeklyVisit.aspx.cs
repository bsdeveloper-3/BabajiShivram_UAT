using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class CRMReports_WeeklyVisit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Weekly Visit";

            txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            gvVisitReport.DataBind();

            if (gvVisitReport.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Weekly Visit!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceVisitReport;
        DataFilter1.DataColumns = gvVisitReport.Columns;
        DataFilter1.FilterSessionID = "WeeklyVisit.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "WeeklyVisitOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvVisitReport.AllowPaging = false;
        gvVisitReport.AllowSorting = false;
        gvVisitReport.Columns[0].Visible = false;
        gvVisitReport.Enabled = false;
        gvVisitReport.Caption = "Weekly Visit Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "WeeklyVisit.aspx";
        DataFilter1.FilterDataSource();
        gvVisitReport.DataBind();
        gvVisitReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

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
            DataFilter1.FilterSessionID = "WeeklyVisit.aspx";
            DataFilter1.FilterDataSource();
            gvVisitReport.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtStartDate = DateTime.MinValue;
        if (txtStartDate.Text.Trim() != "")
        {
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            DataSourceVisitReport.SelectParameters["StartDate"].DefaultValue = dtStartDate.ToString("dd/MM/yyyy");
            gvVisitReport.DataBind();
        }
        else
        {
            lblError.Text = "Select Start Date";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtEndDate = DateTime.MinValue;
        if (txtEndDate.Text.Trim() != "")
        {
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());
            DataSourceVisitReport.SelectParameters["EndDate"].DefaultValue = dtEndDate.ToString("dd/MM/yyyy");
            gvVisitReport.DataBind();
        }
        else
        {
            lblError.Text = "Select End Date";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void chkIncludeVisitDate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIncludeVisitDate.Checked == true)
        {
            DataSourceVisitReport.SelectParameters["IsVisitReport"].DefaultValue = "1";
        }
        else
        {
            DataSourceVisitReport.SelectParameters["IsVisitReport"].DefaultValue = "0";
        }
        gvVisitReport.DataBind();
    }
}