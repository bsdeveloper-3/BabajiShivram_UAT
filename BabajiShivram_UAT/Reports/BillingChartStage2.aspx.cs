using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

public partial class Reports_BillingChartStage2 : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Stage KPI";

            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        // Get Billing Overall KPI and Stage wise KPI
        string strReportMonth = txtReportDate.Text.Trim();

        BindReport(strReportMonth);

    }


    private void BindReport(string ReportMonth)
    {
        DataSet dsBillingKPI = BillingOperation.GetBillingKPIStage(ReportMonth);

        gvKPIReport.DataSource = dsBillingKPI;
        gvKPIReport.DataBind();

    }
    
    #region Exoprt PDF

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    #endregion
}