using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

public partial class Reports_BillingChartStage : System.Web.UI.Page
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

        CreateStageKPI(strReportMonth);

    }

    #region Stage Chart

    private void CreateStageKPI(string ReportMonth)
    {
        // Billing Stage Aging (Hours) - Bill Receive Date To Next Stage Receive Date

        DataSet dsBillingKPI = BillingOperation.GetBillingKPIStage(ReportMonth);

        //*********************Billing Stage Aging Chart Start *******************************

        DataTable BillingStage = dsBillingKPI.Tables[0];

        //************ Billing Overall Chart Start ****************

        if (BillingStage.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember   = new string[BillingStage.Rows.Count];
            int[] YPointMember      = new int[BillingStage.Rows.Count];

            int[] ZPointMember = new int[BillingStage.Rows.Count];

            for (int count = 0; count < BillingStage.Rows.Count; count++)
            {
                //storing Values for X axis  -- Billing Stage
                XPointMember[count] = BillingStage.Rows[count]["BillingStage"].ToString() + "\n"+ BillingStage.Rows[count]["TotalJob"];

                //storing values for Y Axis  -- Average Job Count
                YPointMember[count] = Convert.ToInt32(BillingStage.Rows[count]["AvgHour"]);

            }

            //binding chart control  
            ChartStage.Series[0].Points.DataBindXY(XPointMember, YPointMember);
            ChartStage.Series[0].ChartType = SeriesChartType.Column;
            
            ChartStage.Series[0].Label = "#VALY";
            ChartStage.Series[0].LabelForeColor = Color.Red;
            ChartStage.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartStage.Series[0].LabelForeColor = Color.Wheat;
            ChartStage.Series[0].LabelBackColor = Color.Black;
            ChartStage.Series[0].LabelToolTip = "#VALY";
            
            ChartStage.ChartAreas["AreaStage"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartStage.ChartAreas["AreaStage"].AxisY.Title = "Aging Hours";
            ChartStage.ChartAreas["AreaStage"].AxisX.LabelStyle.Interval = 1;

            //************ END - Billing Overall Chart Start ****************
            //************ START - Billing Stage Aging Chart Start **********

            CreateScrutinyKPI(BillingStage);

            CreateDraftKPI(BillingStage);

            CreateDraftCheckKPI(BillingStage);

            CreateFinalTypeKPI(BillingStage);

            CreateFinalCheckKPI(BillingStage);

            CreateDispatchKPI(BillingStage);
        }
    }      
    private void CreateScrutinyKPI(DataTable dtScrutiny)
    {        
        if (dtScrutiny.Rows.Count > 0)
        {
            // Billing Scrutiny Detail

            int ScrutinyJob             =   Convert.ToInt32(dtScrutiny.Rows[0]["TotalJob"]);
            int ScrutinyReject          =   Convert.ToInt32(dtScrutiny.Rows[0]["RejectCount"]);
            int ScrutinyOtherReject     =   Convert.ToInt32(dtScrutiny.Rows[0]["RejectOtherDept"]);
            int ScrutinyPending7Days    =   Convert.ToInt32(dtScrutiny.Rows[0]["AvgPending"]);

            //storing Scrutiny total rows count to loop on each Record  
            string[] XPointScrutiny     =   new string[3];
            int[] YPointScrutiny        =   new int[3];

            //Total Job Count
            //XPointScrutiny[0] = "Total Job";
            //YPointScrutiny[0] = ScrutinyJob;

            //Total Reject
            XPointScrutiny[0] = "Total Reject";
            YPointScrutiny[0] = ScrutinyReject;

            //Total Other Reject
            XPointScrutiny[1] = "Other Dept Reject";
            YPointScrutiny[1] = ScrutinyOtherReject;

            //Total Pending 7 Days
            XPointScrutiny[2] = "Pending 7 Days";
            YPointScrutiny[2] = ScrutinyPending7Days;

            ChartScrutiny.Series[0].Points.DataBindXY(XPointScrutiny, YPointScrutiny);
            ChartScrutiny.Series[0].ChartType = SeriesChartType.Column;

            ChartScrutiny.Series[0].Label = "#VALY";
            ChartScrutiny.Series[0].Name = "Scrutiny - Job Count " + ScrutinyJob.ToString();
            ChartScrutiny.Series[0].MarkerSize = 10;
            ChartScrutiny.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartScrutiny.Series[0].LabelBackColor = Color.Black;
            ChartScrutiny.Series[0].LabelForeColor = Color.Wheat;
            
            ChartScrutiny.Series[0].LabelToolTip = "#VALY";

            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.Title = "Scrutiny Job Count";
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisX.LabelStyle.Interval = 1;
        }
    }
    private void CreateDraftKPI(DataTable dtDraft)
    {

        if (dtDraft.Rows.Count > 0)
        {
            // Billing Draft Detail

            int dtDraftJob = Convert.ToInt32(dtDraft.Rows[1]["TotalJob"]);
            int dtDraftReject = Convert.ToInt32(dtDraft.Rows[1]["RejectCount"]);
            int dtDraftOtherReject = Convert.ToInt32(dtDraft.Rows[1]["RejectOtherDept"]);
            int dtDraftPending7Days = Convert.ToInt32(dtDraft.Rows[1]["AvgPending"]);
                        
            string[] XPoint = new string[3];
            int[] YPoint = new int[3];

            //Total Job Count
            //XPoint[0] = "Total Job";
            //YPoint[0] = DraftJob;

            //Total Reject
            XPoint[0] = "Total Reject";
            YPoint[0] = dtDraftReject;

            //Total Other Reject
            XPoint[1] = "Other Dept Reject";
            YPoint[1] = dtDraftOtherReject;

            //Total Pending 7 Days
            XPoint[2] = "Pending 7 Days";
            YPoint[2] = dtDraftPending7Days;

            ChartDraft.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartDraft.Series[0].ChartType = SeriesChartType.Column;

            ChartDraft.Series[0].Label = "#VALY";
            ChartDraft.Series[0].Name = "Draft - Job Count " + dtDraftJob.ToString();
            ChartDraft.Series[0].MarkerSize = 10;
            ChartDraft.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDraft.Series[0].LabelBackColor = Color.Black;
            ChartDraft.Series[0].LabelForeColor = Color.Wheat;
            ChartDraft.Series[0].LabelToolTip = "#VALY";

            ChartDraft.ChartAreas["AreaDraft"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraft.ChartAreas["AreaDraft"].AxisY.Title = "Draft Job Count";
            ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Interval = 1;
        }
    }
    private void CreateDraftCheckKPI(DataTable dtDraftCheck)
    {
        if (dtDraftCheck.Rows.Count > 0)
        {
            // Billing Draft Detail

            int dtDraftCheckJob = Convert.ToInt32(dtDraftCheck.Rows[2]["TotalJob"]);
            int dtDraftCheckReject = Convert.ToInt32(dtDraftCheck.Rows[2]["RejectCount"]);
            int dtDraftCheckOtherReject = Convert.ToInt32(dtDraftCheck.Rows[2]["RejectOtherDept"]);
            int dtDraftCheckPending7Days = Convert.ToInt32(dtDraftCheck.Rows[2]["AvgPending"]);
            
            string[] XPoint = new string[3];
            int[] YPoint = new int[3];

            //Total Job Count
            //XPoint[0] = "Total Job";
            //YPoint[0] = DraftJob;

            //Total Reject
            XPoint[0] = "Total Reject";
            YPoint[0] = dtDraftCheckReject;

            //Total Other Reject
            XPoint[1] = "Other Dept Reject";
            YPoint[1] = dtDraftCheckOtherReject;

            //Total Pending 7 Days
            XPoint[2] = "Pending 7 Days";
            YPoint[2] = dtDraftCheckPending7Days;

            ChartDraftCheck.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartDraftCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartDraftCheck.Series[0].Label = "#VALY";
            ChartDraftCheck.Series[0].Name = "Draft Check- Job Count " + dtDraftCheckJob.ToString();
            ChartDraftCheck.Series[0].MarkerSize = 10;
            ChartDraftCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDraftCheck.Series[0].LabelBackColor = Color.Black;
            ChartDraftCheck.Series[0].LabelForeColor = Color.Wheat;
            ChartDraftCheck.Series[0].LabelToolTip = "#VALY";

            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.Title = "Draft Job Count";
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisX.LabelStyle.Interval = 1;
        }
    }
    private void CreateFinalTypeKPI(DataTable dtFintalType)
    {
        if (dtFintalType.Rows.Count > 0)
        {
            // Billing Draft Detail

            int dtFinalTypeJob = Convert.ToInt32(dtFintalType.Rows[3]["TotalJob"]);
            int dtFinalTypeReject = Convert.ToInt32(dtFintalType.Rows[3]["RejectCount"]);
            int dtFinalTypeOtherReject = Convert.ToInt32(dtFintalType.Rows[3]["RejectOtherDept"]);
            int dtFinalTypePending7Days = Convert.ToInt32(dtFintalType.Rows[3]["AvgPending"]);
            
            string[] XPoint = new string[3];
            int[] YPoint = new int[3];

            //Total Job Count
            //XPoint[0] = "Total Job";
            //YPoint[0] = DraftJob;

            //Total Reject
            XPoint[0] = "Total Reject";
            YPoint[0] = dtFinalTypeReject;

            //Total Other Reject
            XPoint[1] = "Other Dept Reject";
            YPoint[1] = dtFinalTypeOtherReject;

            //Total Pending 7 Days
            XPoint[2] = "Pending 7 Days";
            YPoint[2] = dtFinalTypePending7Days;

            ChartFinalType.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartFinalType.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalType.Series[0].Label = "#VALY";
            ChartFinalType.Series[0].Name = "Final Type - Job Count " + dtFinalTypeJob.ToString();
            ChartFinalType.Series[0].MarkerSize = 10;
            ChartFinalType.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartFinalType.Series[0].LabelBackColor = Color.Black;
            ChartFinalType.Series[0].LabelForeColor = Color.Wheat;
            ChartFinalType.Series[0].LabelToolTip = "#VALY";

            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.Title = "Final Type Job Count";
            ChartFinalType.ChartAreas["AreaFinalType"].AxisX.LabelStyle.Interval = 1;
        }
    }
    private void CreateFinalCheckKPI(DataTable dtFintalCheck)
    {
        if (dtFintalCheck.Rows.Count > 0)
        {
            // Billing Draft Detail

            int dtFinalCheckJob = Convert.ToInt32(dtFintalCheck.Rows[4]["TotalJob"]);
            int dtFinalCheckReject = Convert.ToInt32(dtFintalCheck.Rows[4]["RejectCount"]);
            int dtFinalCheckOtherReject = Convert.ToInt32(dtFintalCheck.Rows[4]["RejectOtherDept"]);
            int dtFinalCheckPending7Days = Convert.ToInt32(dtFintalCheck.Rows[4]["AvgPending"]);
            
            string[] XPoint = new string[3];
            int[] YPoint = new int[3];

            //Total Job Count
            //XPoint[0] = "Total Job";
            //YPoint[0] = DraftJob;

            //Total Reject
            XPoint[0] = "Total Reject";
            YPoint[0] = dtFinalCheckReject;

            //Total Other Reject
            XPoint[1] = "Other Dept Reject";
            YPoint[1] = dtFinalCheckOtherReject;

            //Total Pending 7 Days
            XPoint[2] = "Pending 7 Days";
            YPoint[2] = dtFinalCheckPending7Days;

            ChartFinalCheck.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartFinalCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalCheck.Series[0].Label = "#VALY";
            ChartFinalCheck.Series[0].Name = "Final Check - Job Count " + dtFinalCheckJob.ToString();
            ChartFinalCheck.Series[0].MarkerSize = 10;
            ChartFinalCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartFinalCheck.Series[0].LabelBackColor = Color.Black;
            ChartFinalCheck.Series[0].LabelForeColor = Color.Wheat;
            ChartFinalCheck.Series[0].LabelToolTip = "#VALY";

            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.Title = "Final Check Job Count";
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisX.LabelStyle.Interval = 1;
        }
    }
    private void CreateDispatchKPI(DataTable dtDispatch)
    {
        if (dtDispatch.Rows.Count > 0)
        {
            // Billing Draft Detail

            int dtDispatchJob = Convert.ToInt32(dtDispatch.Rows[5]["TotalJob"]);
            int dtDispatchReject = Convert.ToInt32(dtDispatch.Rows[5]["RejectCount"]);
            int dtDispatchOtherReject = Convert.ToInt32(dtDispatch.Rows[5]["RejectOtherDept"]);
            int dtDispatchPending7Days = Convert.ToInt32(dtDispatch.Rows[5]["AvgPending"]);

            string[] XPoint = new string[3];
            int[] YPoint = new int[3];

            //Total Job Count
            //XPoint[0] = "Total Job";
            //YPoint[0] = DraftJob;

            //Total Reject
            XPoint[0] = "Total Reject";
            YPoint[0] = dtDispatchReject;

            //Total Other Reject
            XPoint[1] = "Other Dept Reject";
            YPoint[1] = dtDispatchOtherReject;

            //Total Pending 7 Days
            XPoint[2] = "Pending 7 Days";
            YPoint[2] = dtDispatchPending7Days;

            ChartDispatch.Series[0].Points.DataBindXY(XPoint, YPoint);
            ChartDispatch.Series[0].ChartType = SeriesChartType.Column;

            ChartDispatch.Series[0].Label = "#VALY";
            ChartDispatch.Series[0].Name = "Dispatch - Job Count " + dtDispatchJob.ToString();
            ChartDispatch.Series[0].MarkerSize = 10;
            ChartDispatch.Series[0].MarkerStyle = MarkerStyle.Triangle;
            ChartDispatch.Series[0].LabelBackColor = Color.Black;
            ChartDispatch.Series[0].LabelForeColor = Color.Wheat;
            ChartDispatch.Series[0].LabelToolTip = "#VALY";

            ChartDispatch.ChartAreas["AreaDispatch"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDispatch.ChartAreas["AreaDispatch"].AxisY.Title = "Dispatch Job Count";
            ChartDispatch.ChartAreas["AreaDispatch"].AxisX.LabelStyle.Interval = 1;
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