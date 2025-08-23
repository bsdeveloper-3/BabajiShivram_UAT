using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reports_CustomerReportView : System.Web.UI.Page
{

    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Report";

            Session["ReportId"] = null;
            Session["ReportName"] = null;
            Session["ReportCustomerId"] = null;
            Session["Lid"] = null;
        }

        DataFilter1.DataSource = ViewReportSqlDataSource;
        DataFilter1.DataColumns = gvReportMS.Columns;
        DataFilter1.FilterSessionID = "CustomerReportView.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
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
            DataFilter1.FilterSessionID = "CustomerReportView.aspx";
            DataFilter1.FilterDataSource();
            gvReportMS.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Gridview Event

    protected void gvReportMS_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvReportMS_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "generatereport")
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            Label Report = (Label)row.FindControl("lblReportName");
            string ReportName = Report.Text;

            Session["ReportId"] = e.CommandArgument.ToString();
            Session["ReportName"] = ReportName;
            Response.Redirect("~/Reports/ViewCustAdHocReport.aspx"); 
        }
        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow rowIndex = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            Label Id = (Label)rowIndex.FindControl("lblid");
            Session["Lid"] = Id.Text;
            Response.Redirect("~/Reports/Customer_Report.aspx");
        }

        if (e.CommandName.ToLower() == "deletereport")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());

            int Result = DBOperations.DeleteCustAdhocReport(lid, LoggedInUser.glCustUserId);

            if (Result == 0)
            {
                lblError.Text = "Report Deleted Successfully !";
                lblError.CssClass = "success";
                gvReportMS.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }            
        }
    }

    protected void DataSourceReport_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lblError.Text = "Report Deleted Successfully !";
            lblError.CssClass = "success";
            gvReportMS.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion
}