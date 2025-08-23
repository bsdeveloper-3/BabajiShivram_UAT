using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Reports_BillingChartUser : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing User KPI";
            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        string strReportMonth = txtReportDate.Text.Trim();

        CreateScrutinyKPI(strReportMonth);
        CreateDraftKPI(strReportMonth);
        CreateDraftCheck(strReportMonth);
        CreateFinalType(strReportMonth);
        CreateFinalCheck(strReportMonth);
        CreateBillDispatch(strReportMonth);
        CreateBillDelay(strReportMonth);
        CreateBillErrorDraftCheck(strReportMonth);
        CreateBillErrorFinalCheck(strReportMonth);
    }

    #region Chart
    private void CreateScrutinyKPI(string ReportMonth)
    {
        // Scrutiny KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserStage(ReportMonth,1);

        //*********************Sector Chart Start *******************************

        DataTable dtScrutiny = dsBillingKPI.Tables[0];

        if (dtScrutiny.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

           // int ScrutinyJob = Convert.ToInt32(dtScrutiny.Rows[0]["TotalStageJob"]);
            
            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtScrutiny.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtScrutiny.Rows.Count)];

            for (int i = 0; i < dtScrutiny.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtScrutiny.Rows[i]["BillingUser"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtScrutiny.Rows[i]["AvgPerDay"]);
            }

            ChartScrutiny.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartScrutiny.Series[0].ChartType = SeriesChartType.Column;

            ChartScrutiny.Series[0].Label = "#VALY";
            ChartScrutiny.Series[0].Name = "Scrutiny - Job Count ";
            ChartScrutiny.Series[0].MarkerSize = 10;
            ChartScrutiny.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartScrutiny.Series[0].LabelBackColor = Color.Black;
            ChartScrutiny.Series[0].LabelForeColor = Color.Wheat;

            ChartScrutiny.Series[0].LabelToolTip = "#VALY";

            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.Title = "Scrutiny Job Count";
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisX.LabelStyle.Interval = 1;
            ChartScrutiny.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateDraftKPI(string ReportMonth)
    {
        // Drafing KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserStage(ReportMonth, 2);

        //*********************Sector Chart Start *******************************

        DataTable dtDrafting = dsBillingKPI.Tables[0];

        if (dtDrafting.Rows.Count > 0)
        {
            // Billing Draft Detail

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny = new string[Convert.ToInt32(dtDrafting.Rows.Count)];
            int[] YPointScrutiny = new int[Convert.ToInt32(dtDrafting.Rows.Count)];

            for (int i = 0; i < dtDrafting.Rows.Count; i++)
            {
                XPointScrutiny[i] = dtDrafting.Rows[i]["BillingUser"].ToString();
                YPointScrutiny[i] = Convert.ToInt32(dtDrafting.Rows[i]["AvgPerDay"]);
            }

            ChartDraft.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartDraft.Series[0].ChartType = SeriesChartType.Column;

            ChartDraft.Series[0].Label = "#VALY";
            ChartDraft.Series[0].Name = "Draft - Job Count ";
            ChartDraft.Series[0].MarkerSize = 10;
            ChartDraft.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDraft.Series[0].LabelBackColor = Color.Black;
            ChartDraft.Series[0].LabelForeColor = Color.Wheat;

            ChartDraft.Series[0].LabelToolTip = "#VALY";

            ChartDraft.ChartAreas["AreaDraft"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraft.ChartAreas["AreaDraft"].AxisY.Title = "Draft Job Count";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Interval = 1;

            ChartDraft.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateDraftCheck(string ReportMonth)
    {
        // Drafing KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserStage(ReportMonth, 3);

        //*********************Sector Chart Start *******************************

        DataTable dtDraftCheck = dsBillingKPI.Tables[0];

        if (dtDraftCheck.Rows.Count > 0)
        {
            // Billing Draft Check Detail

            //storing Draft Check total rows count to loop on each Record  
            string[] XPointDraftCheck = new string[Convert.ToInt32(dtDraftCheck.Rows.Count)];
            int[] YPointDraftCheck = new int[Convert.ToInt32(dtDraftCheck.Rows.Count)];

            for (int i = 0; i < dtDraftCheck.Rows.Count; i++)
            {
                XPointDraftCheck[i] = dtDraftCheck.Rows[i]["BillingUser"].ToString();
                YPointDraftCheck[i] = Convert.ToInt32(dtDraftCheck.Rows[i]["AvgPerDay"]);
            }

            ChartDraftCheck.Series[0].Points.DataBindXY(XPointDraftCheck, YPointDraftCheck);
            ChartDraftCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartDraftCheck.Series[0].Label = "#VALY";
            ChartDraftCheck.Series[0].Name = "Draft Check- Job Count";
            ChartDraftCheck.Series[0].MarkerSize = 10;
            ChartDraftCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDraftCheck.Series[0].LabelBackColor = Color.Black;
            ChartDraftCheck.Series[0].LabelForeColor = Color.Wheat;

            ChartDraftCheck.Series[0].LabelToolTip = "#VALY";

            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.Title = "Draft Check Job Count";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisX.LabelStyle.Interval = 1;

            ChartDraftCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateFinalType(string ReportMonth)
    {
        // Drafing KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserStage(ReportMonth, 4);

        //*********************Sector Chart Start *******************************

        DataTable dtFinalType = dsBillingKPI.Tables[0];

        if (dtFinalType.Rows.Count > 0)
        {
            // Billing Final Type Check Detail

            //storing Draft Check total rows count to loop on each Record  
            string[] XPointDraftCheck = new string[Convert.ToInt32(dtFinalType.Rows.Count)];
            int[] YPointDraftCheck = new int[Convert.ToInt32(dtFinalType.Rows.Count)];

            for (int i = 0; i < dtFinalType.Rows.Count; i++)
            {
                XPointDraftCheck[i] = dtFinalType.Rows[i]["BillingUser"].ToString();
                YPointDraftCheck[i] = Convert.ToInt32(dtFinalType.Rows[i]["AvgPerDay"]);
            }

            ChartFinalType.Series[0].Points.DataBindXY(XPointDraftCheck, YPointDraftCheck);
            ChartFinalType.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalType.Series[0].Label = "#VALY";
            ChartFinalType.Series[0].Name = "Final Type - Job Count";
            ChartFinalType.Series[0].MarkerSize = 10;
            ChartFinalType.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartFinalType.Series[0].LabelBackColor = Color.Black;
            ChartFinalType.Series[0].LabelForeColor = Color.Wheat;

            ChartFinalType.Series[0].LabelToolTip = "#VALY";

            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.Title = "Final Type Job Count";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartFinalType.ChartAreas["AreaFinalType"].AxisX.LabelStyle.Interval = 1;

            ChartFinalType.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateFinalCheck(string ReportMonth)
    {
        // Final Check KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserStage(ReportMonth, 5);

        //*********************Final Check Chart Start *******************************

        DataTable dtFinalCheck = dsBillingKPI.Tables[0];

        if (dtFinalCheck.Rows.Count > 0)
        {
            // Billing Final Check Check Detail

            //storing Final Check total rows count to loop on each Record  
            string[] XPointDraftCheck = new string[Convert.ToInt32(dtFinalCheck.Rows.Count)];
            int[] YPointDraftCheck = new int[Convert.ToInt32(dtFinalCheck.Rows.Count)];

            for (int i = 0; i < dtFinalCheck.Rows.Count; i++)
            {
                XPointDraftCheck[i] = dtFinalCheck.Rows[i]["BillingUser"].ToString();
                YPointDraftCheck[i] = Convert.ToInt32(dtFinalCheck.Rows[i]["AvgPerDay"]);
            }

            ChartFinalCheck.Series[0].Points.DataBindXY(XPointDraftCheck, YPointDraftCheck);
            ChartFinalCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalCheck.Series[0].Label = "#VALY";
            ChartFinalCheck.Series[0].Name = "Final Check - Job Count";
            ChartFinalCheck.Series[0].MarkerSize = 10;
            ChartFinalCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartFinalCheck.Series[0].LabelBackColor = Color.Black;
            ChartFinalCheck.Series[0].LabelForeColor = Color.Wheat;

            ChartFinalCheck.Series[0].LabelToolTip = "#VALY";

            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.Title = "Final Check Job Count";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisX.LabelStyle.Interval = 1;

            ChartFinalCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateBillDispatch(string ReportMonth)
    {
        // Final Check KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserStage(ReportMonth, 6);

        //*********************Final Check Chart Start *******************************

        DataTable dtBillDispatch = dsBillingKPI.Tables[0];

        if (dtBillDispatch.Rows.Count > 0)
        {
            // Billing Final Check Check Detail

            //storing Final Check total rows count to loop on each Record  
            string[] XPointDraftCheck = new string[Convert.ToInt32(dtBillDispatch.Rows.Count)];
            int[] YPointDraftCheck = new int[Convert.ToInt32(dtBillDispatch.Rows.Count)];

            for (int i = 0; i < dtBillDispatch.Rows.Count; i++)
            {
                XPointDraftCheck[i] = dtBillDispatch.Rows[i]["BillingUser"].ToString();
                YPointDraftCheck[i] = Convert.ToInt32(dtBillDispatch.Rows[i]["AvgPerDay"]);
            }

            ChartDispatch.Series[0].Points.DataBindXY(XPointDraftCheck, YPointDraftCheck);
            ChartDispatch.Series[0].ChartType = SeriesChartType.Column;

            ChartDispatch.Series[0].Label = "#VALY";
            ChartDispatch.Series[0].Name = "Bill Dispatch - Job Count";
            ChartDispatch.Series[0].MarkerSize = 10;
            ChartDispatch.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDispatch.Series[0].LabelBackColor = Color.Black;
            ChartDispatch.Series[0].LabelForeColor = Color.Wheat;

            ChartDispatch.Series[0].LabelToolTip = "#VALY";

            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisY.Title = "Dispatch Job Count";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisX.LabelStyle.Interval = 1;

            ChartDispatch.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateBillDelay(string ReportMonth)
    {
        // Final Check KPI - Bill Delay by more then 3 days

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserDelay(ReportMonth);

        //*********************Final Check Chart Start *******************************

        DataTable dtBillDelay = dsBillingKPI.Tables[0];

        if (dtBillDelay.Rows.Count > 0)
        {
            // Billing Final Check Check Detail

            //storing Final Check total rows count to loop on each Record  
            string[] XPoint = new string[Convert.ToInt32(dtBillDelay.Rows.Count)];
            int[] YPoint = new int[Convert.ToInt32(dtBillDelay.Rows.Count)];

            for (int i = 0; i < dtBillDelay.Rows.Count; i++)
            {
                XPoint[i] = dtBillDelay.Rows[i]["BillingUser"].ToString();
                YPoint[i] = Convert.ToInt32(dtBillDelay.Rows[i]["TotalDelayJob"]);
            }

            ChartUserDelay.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartUserDelay.Series[0].ChartType = SeriesChartType.Column;

            ChartUserDelay.Series[0].Label = "#VALY";
            ChartUserDelay.Series[0].Name = "User Delay > 3 Days";
            ChartUserDelay.Series[0].MarkerSize = 10;
            ChartUserDelay.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartUserDelay.Series[0].LabelBackColor = Color.Black;
            ChartUserDelay.Series[0].LabelForeColor = Color.Wheat;

            ChartUserDelay.Series[0].LabelToolTip = "#VALY";

            ChartUserDelay.ChartAreas["AreaDelay"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartUserDelay.ChartAreas["AreaDelay"].AxisY.Title = "Delay Job Count";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartUserDelay.ChartAreas["AreaDelay"].AxisX.LabelStyle.Interval = 1;

            ChartUserDelay.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateBillErrorDraftCheck(string ReportMonth)
    {
        // Draft Check Errpr KPI - 

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserError(ReportMonth,3);

        //*********************Final Check Chart Start *******************************

        DataTable dtBillErrorDraft = dsBillingKPI.Tables[0];

        if (dtBillErrorDraft.Rows.Count > 0)
        {
            // Billing Draft Check Error Detail

            //storing Draft Check total rows count to loop on each Record  
            string[] XPoint = new string[Convert.ToInt32(dtBillErrorDraft.Rows.Count)];
            int[] YPoint = new int[Convert.ToInt32(dtBillErrorDraft.Rows.Count)];

            for (int i = 0; i < dtBillErrorDraft.Rows.Count; i++)
            {
                XPoint[i] = dtBillErrorDraft.Rows[i]["BillingUser"].ToString();
                YPoint[i] = Convert.ToInt32(dtBillErrorDraft.Rows[i]["TotalErrorJob"]);
            }

            ChartErrorDraftCheck.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartErrorDraftCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartErrorDraftCheck.Series[0].Label = "#VALY";
            ChartErrorDraftCheck.Series[0].Name = "Bill Error Draft";
            ChartErrorDraftCheck.Series[0].MarkerSize = 10;
            ChartErrorDraftCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartErrorDraftCheck.Series[0].LabelBackColor = Color.Black;
            ChartErrorDraftCheck.Series[0].LabelForeColor = Color.Wheat;

            ChartErrorDraftCheck.Series[0].LabelToolTip = "#VALY";

            ChartErrorDraftCheck.ChartAreas["AreaErrorDraft"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartErrorDraftCheck.ChartAreas["AreaErrorDraft"].AxisY.Title = "Draft Error Job";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartErrorDraftCheck.ChartAreas["AreaErrorDraft"].AxisX.LabelStyle.Interval = 1;

            ChartErrorDraftCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }

    private void CreateBillErrorFinalCheck(string ReportMonth)
    {
        // Draft Check Errpr KPI - 

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIUserError(ReportMonth, 3);

        //*********************Final Check Chart Start *******************************

        DataTable dtBillErrorFinalCheck = dsBillingKPI.Tables[0];

        if (dtBillErrorFinalCheck.Rows.Count > 0)
        {
            // Billing Draft Check Error Detail

            //storing Draft Check total rows count to loop on each Record  
            string[] XPoint = new string[Convert.ToInt32(dtBillErrorFinalCheck.Rows.Count)];
            int[] YPoint = new int[Convert.ToInt32(dtBillErrorFinalCheck.Rows.Count)];

            for (int i = 0; i < dtBillErrorFinalCheck.Rows.Count; i++)
            {
                XPoint[i] = dtBillErrorFinalCheck.Rows[i]["BillingUser"].ToString();
                YPoint[i] = Convert.ToInt32(dtBillErrorFinalCheck.Rows[i]["TotalErrorJob"]);
            }

            ChartErrorFinalType.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartErrorFinalType.Series[0].ChartType = SeriesChartType.Column;

            ChartErrorFinalType.Series[0].Label = "#VALY";
            ChartErrorFinalType.Series[0].Name = "Bill Error Final Type";
            ChartErrorFinalType.Series[0].MarkerSize = 10;
            ChartErrorFinalType.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartErrorFinalType.Series[0].LabelBackColor = Color.Black;
            ChartErrorFinalType.Series[0].LabelForeColor = Color.Wheat;

            ChartErrorDraftCheck.Series[0].LabelToolTip = "#VALY";

            ChartErrorFinalType.ChartAreas["AreaErrorFinalType"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartErrorFinalType.ChartAreas["AreaErrorFinalType"].AxisY.Title = "Final Type Error Job";
            //ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Font = new System.Drawing.Font("Sans Serif", 10, FontStyle.Regular);
            ChartErrorFinalType.ChartAreas["AreaErrorFinalType"].AxisX.LabelStyle.Interval = 1;

            ChartErrorFinalType.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    #endregion

    #region Exoprt PDF

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    
    #endregion
}