using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
public partial class Reports_BillingChartRejection : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Reject Reason KPI";
        }

        string strReportYear = ddYear.SelectedValue;

        string strReportMonth = "01/01/" + strReportYear;

        CreateRejectionKPI(strReportMonth);

        CreateReasonKPI(strReportMonth);

        CreateRejectionCustomer(strReportMonth);
        
    }

    #region Reject Chart

    private void CreateRejectionKPI(string ReportMonth)
    {
        // Billing Stage Aging (Hours) - Bill Receive Date To Next Stage Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIRejection(ReportMonth);

        //*********************Billing Stage Aging Chart Start *******************************

        DataTable BillingStage = dsBillingKPI.Tables[0];

        //************ Billing Overall Chart Start ****************

        if (BillingStage.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[BillingStage.Rows.Count];
            int[] YPointMember = new int[BillingStage.Rows.Count];

            int[] ZPointMember = new int[BillingStage.Rows.Count];

            for (int count = 0; count < BillingStage.Rows.Count; count++)
            {
                //storing Values for X axis  -- Billing Stage
                XPointMember[count] = BillingStage.Rows[count]["StageName"].ToString() + "\n" + BillingStage.Rows[count]["TotalReject"];

                //storing values for Y Axis  -- Average Job Count
                YPointMember[count] = Convert.ToInt32(BillingStage.Rows[count]["AvgPerMonth"]);
            }

            //binding chart control  
            ChartRejectMonth.Series[0].Points.DataBindXY(XPointMember, YPointMember);
            ChartRejectMonth.Series[0].ChartType = SeriesChartType.Column;

            ChartRejectMonth.Series[0].Label = "#VALY";
            ChartRejectMonth.Series[0].LabelForeColor = Color.Red;
            ChartRejectMonth.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartRejectMonth.Series[0].LabelForeColor = Color.Wheat;
            ChartRejectMonth.Series[0].LabelBackColor = Color.Black;
            ChartRejectMonth.Series[0].LabelToolTip = "#VALY";

            ChartRejectMonth.ChartAreas["AreaReject"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartRejectMonth.ChartAreas["AreaReject"].AxisY.Title = "Avg Per Month";
            ChartRejectMonth.ChartAreas["AreaReject"].AxisX.LabelStyle.Interval = 1;

            //************ END - Billing Reject per Month ****************
            //************ START - Billing Reject per Day ****************

            //storing total rows count to loop on each Record  
            string[] X1PointMember = new string[BillingStage.Rows.Count];
            int[] Y1PointMember = new int[BillingStage.Rows.Count];

            for (int count = 0; count < BillingStage.Rows.Count; count++)
            {
                //storing Values for X axis  -- Billing Stage
                X1PointMember[count] = BillingStage.Rows[count]["StageName"].ToString() + "\n" + BillingStage.Rows[count]["TotalReject"];

                //storing values for Y Axis  -- Average Job Count
                Y1PointMember[count] = Convert.ToInt32(BillingStage.Rows[count]["AvgPerDay"]);
            }

            //binding chart control  
            ChartRejectDay.Series[0].Points.DataBindXY(X1PointMember, Y1PointMember);
            ChartRejectDay.Series[0].ChartType = SeriesChartType.Column;

            ChartRejectDay.Series[0].Label = "#VALY";
            ChartRejectDay.Series[0].LabelForeColor = Color.Red;
            ChartRejectDay.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartRejectDay.Series[0].LabelForeColor = Color.Wheat;
            ChartRejectDay.Series[0].LabelBackColor = Color.Black;
            ChartRejectDay.Series[0].LabelToolTip = "#VALY";

            ChartRejectDay.ChartAreas["DayStage"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartRejectDay.ChartAreas["DayStage"].AxisY.Title = "Avg Per Day";
            ChartRejectDay.ChartAreas["DayStage"].AxisX.LabelStyle.Interval = 1;

        }
    }

    private void CreateReasonKPI(string ReportMonth)
    {
        // Billing Reject Reason Aging (Month) - 

        DataSet dsReasonKPI = BillingOperation.GetBillingKPIRejectReason(ReportMonth);

        //*********************Billing Stage Aging Chart Start *******************************

        DataTable BillingReason = dsReasonKPI.Tables[0];

        //************ Billing Overall Chart Start ****************

        if (BillingReason.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[BillingReason.Rows.Count];
            int[] YPointMember = new int[BillingReason.Rows.Count];

            int[] ZPointMember = new int[BillingReason.Rows.Count];

            for (int count = 0; count < BillingReason.Rows.Count; count++)
            {
                //storing Values for X axis  -- Billing Stage
                XPointMember[count] = BillingReason.Rows[count]["RejectReason"].ToString() + "\n" + BillingReason.Rows[count]["TotalReject"];

                //storing values for Y Axis  -- Average Job Count
                YPointMember[count] = Convert.ToInt32(BillingReason.Rows[count]["AvgPerMonth"]);
            }

            //binding chart control  
            ChartReasonMonth.Series[0].Points.DataBindXY(XPointMember, YPointMember);
            ChartReasonMonth.Series[0].ChartType = SeriesChartType.Column;

            ChartReasonMonth.Series[0].Label = "#VALY";
            ChartReasonMonth.Series[0].LabelForeColor = Color.Red;
            ChartReasonMonth.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartReasonMonth.Series[0].LabelForeColor = Color.Wheat;
            ChartReasonMonth.Series[0].LabelBackColor = Color.Black;
            ChartReasonMonth.Series[0].LabelToolTip = "#VALY";

            ChartReasonMonth.ChartAreas["AreaReason"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartReasonMonth.ChartAreas["AreaReason"].AxisY.Title = "Avg Per Month";
            ChartReasonMonth.ChartAreas["AreaReason"].AxisX.LabelStyle.Interval = 1;

            //************ END - Billing Reject Reason per Month ****************
            //************ START - Billing Reject Reason per Day ****************

            //storing total rows count to loop on each Record  
            string[] X1PointMember = new string[BillingReason.Rows.Count];
            int[] Y1PointMember = new int[BillingReason.Rows.Count];

            for (int count = 0; count < BillingReason.Rows.Count; count++)
            {
                //storing Values for X axis  -- Billing Stage
                X1PointMember[count] = BillingReason.Rows[count]["RejectReason"].ToString() + "\n" + BillingReason.Rows[count]["TotalReject"];

                //storing values for Y Axis  -- Average Job Count
                Y1PointMember[count] = Convert.ToInt32(BillingReason.Rows[count]["AvgPerDay"]);
            }

            //binding chart control  
            ChartReasonDay.Series[0].Points.DataBindXY(X1PointMember, Y1PointMember);
            ChartReasonDay.Series[0].ChartType = SeriesChartType.Column;

            ChartReasonDay.Series[0].Label = "#VALY";
            ChartReasonDay.Series[0].LabelForeColor = Color.Red;
            ChartReasonDay.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartReasonDay.Series[0].LabelForeColor = Color.Wheat;
            ChartReasonDay.Series[0].LabelBackColor = Color.Black;
            ChartReasonDay.Series[0].LabelToolTip = "#VALY";

            ChartReasonDay.ChartAreas["DayReason"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartReasonDay.ChartAreas["DayReason"].AxisY.Title = "Avg Per Day";
            ChartReasonDay.ChartAreas["DayReason"].AxisX.LabelStyle.Interval = 1;

        }
    }

    private void CreateRejectionCustomer(string ReportMonth)
    {
        // Billing Stage Aging (Hours) - Bill Receive Date To Next Stage Receive Date

        DataSet dsBillingCustomer = BillingOperation.GetBillingKPIRejectionCustomer(ReportMonth);

        //*********************Billing Stage Aging Chart Start *******************************

        DataTable BillingCustomer = dsBillingCustomer.Tables[0];

        //************ Billing Overall Chart Start ****************

        if (BillingCustomer.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[BillingCustomer.Rows.Count];
            int[] YPointMember = new int[BillingCustomer.Rows.Count];

            int[] ZPointMember = new int[BillingCustomer.Rows.Count];

            for (int count = 0; count < BillingCustomer.Rows.Count; count++)
            {
                //storing Values for X axis  -- Billing Stage
                XPointMember[count] = BillingCustomer.Rows[count]["Customer"].ToString();

                //storing values for Y Axis  -- Average Job Count
                YPointMember[count] = Convert.ToInt32(BillingCustomer.Rows[count]["AvgPerMonth"]);
            }

            //binding chart control  
            ChartCustomerMonth.Series[0].Points.DataBindXY(XPointMember, YPointMember);
            ChartCustomerMonth.Series[0].ChartType = SeriesChartType.Column;

            ChartCustomerMonth.Series[0].Label = "#VALY";
            ChartCustomerMonth.Series[0].LabelForeColor = Color.Red;
            ChartCustomerMonth.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartCustomerMonth.Series[0].LabelForeColor = Color.Wheat;
            ChartCustomerMonth.Series[0].LabelBackColor = Color.Black;
            ChartCustomerMonth.Series[0].LabelToolTip = "#VALY";

            ChartCustomerMonth.ChartAreas["AreaCustomer"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartCustomerMonth.ChartAreas["AreaCustomer"].AxisY.Title = "Avg Per Month";
            ChartCustomerMonth.ChartAreas["AreaCustomer"].AxisX.LabelStyle.Interval = 1;
            ChartCustomerMonth.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
            //************ END - Customer Reject per Month ****************

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