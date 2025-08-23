using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

public partial class Reports_BillingChartCustomerStage : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Customer KPI";

            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        // Get Billing Overall KPI and Stage wise KPI
        string strReportMonth = txtReportDate.Text.Trim();

        CreateCustomerChart(strReportMonth);
        CreateScrutinyKPI(strReportMonth);
        CreateDraftKPI(strReportMonth);
        CreateDraftCheckKPI(strReportMonth);
        CreateFinalTypeKPI(strReportMonth);
        CreateFinalCheckKPI(strReportMonth);
        CreateDispatchKPI(strReportMonth);
    }

    #region Customer Stage Chart

    private void CreateCustomerChart(string ReportMonth)
    {
        DataSet ds = new DataSet();

        DataSet dsBarChart = BillingOperation.GetBillingKPICustomer(ReportMonth);
        DataTable dt = dsBarChart.Tables[0];

        string[] x = new string[dt.Rows.Count];
        int[] y = new int[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            x[i] = dt.Rows[i][0].ToString();
            y[i] = Convert.ToInt32(dt.Rows[i][1]);
        }

        ChartCustomer.Series[0].Points.DataBindXY(x, y);
        ChartCustomer.Series[0].ChartType = SeriesChartType.Column;

        ChartCustomer.Series[0].Label = "#VALY";
        //ChartCustomer.Series[0].Name = "Customer - Bill Days";
        ChartCustomer.Series[0].MarkerSize = 10;
        ChartCustomer.Series[0].MarkerStyle = MarkerStyle.Triangle;
        ChartCustomer.Series[0].LabelBackColor = Color.Black;
        ChartCustomer.Series[0].LabelForeColor = Color.Wheat;

        ChartCustomer.Series[0].LabelToolTip = "#VALY";

        ChartCustomer.ChartAreas["AreaCustomer"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
        ChartCustomer.ChartAreas["AreaCustomer"].AxisY.Title = "Bill Days";
        ChartCustomer.ChartAreas["AreaCustomer"].AxisX.LabelStyle.Interval = 1;
        ChartCustomer.Series[0].Label = "#VALY";
        //  ChartCustomer.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Sans Serif", 10, FontStyle.Bold);

        ChartCustomer.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
    }
    private void CreateScrutinyKPI(string ReportMonth)
    {
        // Scrutiny KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPICustomerStage(ReportMonth, 1);

        //*********************Sector Chart Start *******************************

        DataTable dtScrutiny = dsBillingKPI.Tables[0];

        if (dtScrutiny.Rows.Count > 0)
        {
            // Customer Scrutiny Detail

            string[] XPointScrutiny = new string[Convert.ToInt32(dtScrutiny.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtScrutiny.Rows.Count)];

            for (int i = 0; i < dtScrutiny.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtScrutiny.Rows[i]["CustomerName"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtScrutiny.Rows[i]["AvgBillDays"]);
            }

            ChartScrutiny.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartScrutiny.Series[0].ChartType = SeriesChartType.Column;

            ChartScrutiny.Series[0].Label = "#VALY";
            ChartScrutiny.Series[0].Name = "Scrutiny - Days";
            ChartScrutiny.Series[0].MarkerSize = 10;
            ChartScrutiny.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartScrutiny.Series[0].LabelBackColor = Color.Black;
            ChartScrutiny.Series[0].LabelForeColor = Color.Wheat;

            ChartScrutiny.Series[0].LabelToolTip = "#VALY";

            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.Title = "Scrutiny Days";
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisX.LabelStyle.Interval = 1;
            ChartScrutiny.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateDraftKPI(string ReportMonth)
    {

        // Draft KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPICustomerStage(ReportMonth, 2);

        //*********************Sector Chart Start *******************************

        DataTable dtDraft = dsBillingKPI.Tables[0];

        if (dtDraft.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

            // int ScrutinyJob = Convert.ToInt32(dtScrutiny.Rows[0]["TotalStageJob"]);

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtDraft.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtDraft.Rows.Count)];

            for (int i = 0; i < dtDraft.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtDraft.Rows[i]["CustomerName"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtDraft.Rows[i]["AvgBillDays"]);
            }

            ChartDraft.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartDraft.Series[0].ChartType = SeriesChartType.Column;

            ChartDraft.Series[0].Label = "#VALY";
            ChartDraft.Series[0].Name = "Draft - Days";
            ChartDraft.Series[0].MarkerSize = 10;
            ChartDraft.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDraft.Series[0].LabelBackColor = Color.Black;
            ChartDraft.Series[0].LabelForeColor = Color.Wheat;

            ChartDraft.Series[0].LabelToolTip = "#VALY";

            ChartDraft.ChartAreas["AreaDraft"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraft.ChartAreas["AreaDraft"].AxisY.Title = "Draft Days";
            ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Interval = 1;
            ChartDraft.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateDraftCheckKPI(string ReportMonth)
    {

        // Draft KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPICustomerStage(ReportMonth, 3);

        //*********************Sector Chart Start *******************************

        DataTable dtDraftCheck = dsBillingKPI.Tables[0];

        if (dtDraftCheck.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

            // int ScrutinyJob = Convert.ToInt32(dtScrutiny.Rows[0]["TotalStageJob"]);

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtDraftCheck.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtDraftCheck.Rows.Count)];

            for (int i = 0; i < dtDraftCheck.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtDraftCheck.Rows[i]["CustomerName"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtDraftCheck.Rows[i]["AvgBillDays"]);
            }

            ChartDraftCheck.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartDraftCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartDraftCheck.Series[0].Label = "#VALY";
            ChartDraftCheck.Series[0].Name = "Draft Check - Days";
            ChartDraftCheck.Series[0].MarkerSize = 10;
            ChartDraftCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDraftCheck.Series[0].LabelBackColor = Color.Black;
            ChartDraftCheck.Series[0].LabelForeColor = Color.Wheat;

            ChartDraftCheck.Series[0].LabelToolTip = "#VALY";

            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.Title = "Draft Check Days";
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisX.LabelStyle.Interval = 1;
            ChartDraftCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateFinalTypeKPI(string ReportMonth)
    {

        // Draft KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPICustomerStage(ReportMonth, 4);

        //*********************Sector Chart Start *******************************

        DataTable dtDraftCheck = dsBillingKPI.Tables[0];

        if (dtDraftCheck.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

            // int ScrutinyJob = Convert.ToInt32(dtScrutiny.Rows[0]["TotalStageJob"]);

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtDraftCheck.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtDraftCheck.Rows.Count)];

            for (int i = 0; i < dtDraftCheck.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtDraftCheck.Rows[i]["CustomerName"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtDraftCheck.Rows[i]["AvgBillDays"]);
            }

            ChartFinalType.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartFinalType.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalType.Series[0].Label = "#VALY";
            ChartFinalType.Series[0].Name = "Final Type - Days";
            ChartFinalType.Series[0].MarkerSize = 10;
            ChartFinalType.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartFinalType.Series[0].LabelBackColor = Color.Black;
            ChartFinalType.Series[0].LabelForeColor = Color.Wheat;

            ChartFinalType.Series[0].LabelToolTip = "#VALY";

            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.Title = "Final Type Days";
            ChartFinalType.ChartAreas["AreaFinalType"].AxisX.LabelStyle.Interval = 1;
            ChartFinalType.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateFinalCheckKPI(string ReportMonth)
    {

        // Draft KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPICustomerStage(ReportMonth, 5);

        //*********************Sector Chart Start *******************************

        DataTable dtDraftCheck = dsBillingKPI.Tables[0];

        if (dtDraftCheck.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

            // int ScrutinyJob = Convert.ToInt32(dtScrutiny.Rows[0]["TotalStageJob"]);

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtDraftCheck.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtDraftCheck.Rows.Count)];

            for (int i = 0; i < dtDraftCheck.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtDraftCheck.Rows[i]["CustomerName"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtDraftCheck.Rows[i]["AvgBillDays"]);
            }

            ChartFinalCheck.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartFinalCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalCheck.Series[0].Label = "#VALY";
            ChartFinalCheck.Series[0].Name = "Final Check - Days";
            ChartFinalCheck.Series[0].MarkerSize = 10;
            ChartFinalCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartFinalCheck.Series[0].LabelBackColor = Color.Black;
            ChartFinalCheck.Series[0].LabelForeColor = Color.Wheat;

            ChartFinalCheck.Series[0].LabelToolTip = "#VALY";

            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.Title = "Final Check Days";
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisX.LabelStyle.Interval = 1;
            ChartFinalCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateDispatchKPI(string ReportMonth)
    {

        // Draft KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPICustomerStage(ReportMonth, 6);

        //*********************Sector Chart Start *******************************

        DataTable dtDraftCheck = dsBillingKPI.Tables[0];

        if (dtDraftCheck.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

            // int ScrutinyJob = Convert.ToInt32(dtScrutiny.Rows[0]["TotalStageJob"]);

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtDraftCheck.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtDraftCheck.Rows.Count)];

            for (int i = 0; i < dtDraftCheck.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtDraftCheck.Rows[i]["CustomerName"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtDraftCheck.Rows[i]["AvgBillDays"]);
            }

            ChartDispatch.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartDispatch.Series[0].ChartType = SeriesChartType.Column;

            ChartDispatch.Series[0].Label = "#VALY";
            ChartDispatch.Series[0].Name = "Dispatch - Days";
            ChartDispatch.Series[0].MarkerSize = 10;
            ChartDispatch.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDispatch.Series[0].LabelBackColor = Color.Black;
            ChartDispatch.Series[0].LabelForeColor = Color.Wheat;

            ChartDispatch.Series[0].LabelToolTip = "#VALY";

            ChartDispatch.ChartAreas["AreaDispatch"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDispatch.ChartAreas["AreaDispatch"].AxisY.Title = "Dispatch Days";
            ChartDispatch.ChartAreas["AreaDispatch"].AxisX.LabelStyle.Interval = 1;
            ChartDispatch.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
     
    #endregion

    #region Exoprt PDF

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    #endregion

    protected void ChartCustomer_Click(object sender, ImageMapEventArgs e)
    {
        
    }
}