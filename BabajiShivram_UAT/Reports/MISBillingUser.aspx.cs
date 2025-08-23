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

public partial class Reports_MISBillingUser : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    int KPIDays = 100;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (txtKPIDays.Text.Trim() != "")
        {
            KPIDays = Convert.ToInt32(txtKPIDays.Text.Trim());
        }
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing User Effeciency";
            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        string strReportMonth = txtReportDate.Text.Trim();

        CreateChartScrutinyUser(strReportMonth);
        CreateChartDraftUser(strReportMonth);
        CreateChartDraftCheckUser(strReportMonth);
        CreateChartFinalTypeUser(strReportMonth);
        CreateChartFinalCheckUser(strReportMonth);
        CreateChartBillDispatchUser(strReportMonth);

        //CreateBillDelay(strReportMonth);
        //CreateBillErrorDraftCheck(strReportMonth);
        //CreateBillErrorFinalCheck(strReportMonth);
    }

    #region Chart
    private void CreateChartScrutinyUser(string ReportMonth)
    {
        // Scrutiny KPI - Bill Completed By User - File Receive Date

        DataSet dsBillingScrutiny = BillingOperation.GetMISBillingUser(ReportMonth, 1, KPIDays,LoggedInUser.glFinYearId);

        DataTable dtScrutiny = dsBillingScrutiny.Tables[0];

        if (dtScrutiny.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dtScrutiny.Rows.Count];
            int[] YPointMember = new int[dtScrutiny.Rows.Count];
                
            for (int count = 0; count < dtScrutiny.Rows.Count; count++)
            {
                ChartScrutiny.Series[0].Points.AddXY(dtScrutiny.Rows[count]["UserName"].ToString(), dtScrutiny.Rows[count]["PassPercent"].ToString());

                ChartScrutiny.Series[0].Points[count].Label = dtScrutiny.Rows[count]["TotalJob"].ToString();
                ChartScrutiny.Series[0].Points[count].ToolTip = dtScrutiny.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + dtScrutiny.Rows[count]["KPIJob"].ToString() + "/" + dtScrutiny.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartScrutiny.Series[0].ChartType = SeriesChartType.Column;

            ChartScrutiny.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.LabelStyle.Format = "{#}%";

            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.Title = "Scrutiny KPI %";
            ChartScrutiny.ChartAreas["AreaScrutiny"].AxisX.LabelStyle.Interval = 1;
            ChartScrutiny.ChartAreas[0].AxisX.LabelStyle.Angle = -55;

            ChartScrutiny.Titles[0].Text = "Scrutiny User (KPI "+ KPIDays.ToString() +" Days)";

        }
    }   
    private void CreateChartDraftUser(string ReportMonth)
    {
        // Draft KPI - Bill Completed By User - File Sent Date To Completed Date

        DataSet dsBillingDraft = BillingOperation.GetMISBillingUser(ReportMonth, 2, KPIDays, LoggedInUser.glFinYearId);
        
        DataTable dtDraft = dsBillingDraft.Tables[0];

        if (dtDraft.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dtDraft.Rows.Count];
            int[] YPointMember = new int[dtDraft.Rows.Count];

            for (int count = 0; count < dtDraft.Rows.Count; count++)
            {
                ChartDraft.Series[0].Points.AddXY(dtDraft.Rows[count]["UserName"].ToString(), dtDraft.Rows[count]["PassPercent"].ToString());

                ChartDraft.Series[0].Points[count].Label = dtDraft.Rows[count]["TotalJob"].ToString();
                ChartDraft.Series[0].Points[count].ToolTip = dtDraft.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + dtDraft.Rows[count]["KPIJob"].ToString() + "/" + dtDraft.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartDraft.Series[0].ChartType = SeriesChartType.Column;

            ChartDraft.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartDraft.ChartAreas["AreaDraft"].AxisY.LabelStyle.Format = "{#}%";

            ChartDraft.ChartAreas["AreaDraft"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraft.ChartAreas["AreaDraft"].AxisY.Title = "Draft KPI %";
            ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Interval = 1;
            ChartDraft.ChartAreas[0].AxisX.LabelStyle.Angle = -55;

            ChartDraft.Titles[0].Text = "Draft User (KPI " + KPIDays.ToString() + " Days)";
        }
    }
    private void CreateChartDraftCheckUser(string ReportMonth)
    {
        // Draft Check KPI - Bill Completed By User - File Sent Date To Completed Date

        DataSet dsBillingDraftCheck = BillingOperation.GetMISBillingUser(ReportMonth, 3, KPIDays, LoggedInUser.glFinYearId);

        DataTable dtDraftCheck = dsBillingDraftCheck.Tables[0];

        if (dtDraftCheck.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dtDraftCheck.Rows.Count];
            int[] YPointMember = new int[dtDraftCheck.Rows.Count];

            for (int count = 0; count < dtDraftCheck.Rows.Count; count++)
            {
                ChartDraftCheck.Series[0].Points.AddXY(dtDraftCheck.Rows[count]["UserName"].ToString(), dtDraftCheck.Rows[count]["PassPercent"].ToString());

                ChartDraftCheck.Series[0].Points[count].Label = dtDraftCheck.Rows[count]["TotalJob"].ToString();
                ChartDraftCheck.Series[0].Points[count].ToolTip = dtDraftCheck.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + dtDraftCheck.Rows[count]["KPIJob"].ToString() + "/" + dtDraftCheck.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartDraftCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartDraftCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.LabelStyle.Format = "{#}%";

            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.Title = "Draft KPI %";
            ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisX.LabelStyle.Interval = 1;
            ChartDraftCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;

            ChartDraftCheck.Titles[0].Text = "Draft Check User (KPI " + KPIDays.ToString() + " Days)";
        }
    }
    private void CreateChartFinalTypeUser(string ReportMonth)
    {
        // Final Type KPI - Bill Completed By User - File Sent Date To Completed Date

        DataSet dsBillingFinalType = BillingOperation.GetMISBillingUser(ReportMonth, 4, KPIDays, LoggedInUser.glFinYearId);
                
        DataTable dtFinalType = dsBillingFinalType.Tables[0];

        if (dtFinalType.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dtFinalType.Rows.Count];
            int[] YPointMember = new int[dtFinalType.Rows.Count];

            for (int count = 0; count < dtFinalType.Rows.Count; count++)
            {
                ChartFinalType.Series[0].Points.AddXY(dtFinalType.Rows[count]["UserName"].ToString(), dtFinalType.Rows[count]["PassPercent"].ToString());

                ChartFinalType.Series[0].Points[count].Label = dtFinalType.Rows[count]["TotalJob"].ToString();
                ChartFinalType.Series[0].Points[count].ToolTip = dtFinalType.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + dtFinalType.Rows[count]["KPIJob"].ToString() + "/" + dtFinalType.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartFinalType.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalType.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.LabelStyle.Format = "{#}%";

            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalType.ChartAreas["AreaFinalType"].AxisY.Title = "Final Type KPI %";
            ChartFinalType.ChartAreas["AreaFinalType"].AxisX.LabelStyle.Interval = 1;
            ChartFinalType.ChartAreas[0].AxisX.LabelStyle.Angle = -55;

            ChartFinalType.Titles[0].Text = "Final Type User (KPI " + KPIDays.ToString() + " Days)";
        }
    }
    private void CreateChartFinalCheckUser(string ReportMonth)
    {
        // Final Check KPI - Bill Completed By User - File Sent Date To Completed Date

        DataSet dsBillingFinalCheck = BillingOperation.GetMISBillingUser(ReportMonth, 5, KPIDays, LoggedInUser.glFinYearId);
        
        DataTable dtFinalCheck = dsBillingFinalCheck.Tables[0];

        if (dtFinalCheck.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dtFinalCheck.Rows.Count];
            int[] YPointMember = new int[dtFinalCheck.Rows.Count];

            for (int count = 0; count < dtFinalCheck.Rows.Count; count++)
            {
                ChartFinalCheck.Series[0].Points.AddXY(dtFinalCheck.Rows[count]["UserName"].ToString(), dtFinalCheck.Rows[count]["PassPercent"].ToString());

                ChartFinalCheck.Series[0].Points[count].Label = dtFinalCheck.Rows[count]["TotalJob"].ToString();
                ChartFinalCheck.Series[0].Points[count].ToolTip = dtFinalCheck.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + dtFinalCheck.Rows[count]["KPIJob"].ToString() + "/" + dtFinalCheck.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartFinalCheck.Series[0].ChartType = SeriesChartType.Column;

            ChartFinalCheck.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.LabelStyle.Format = "{#}%";

            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.Title = "Final Check KPI %";
            ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisX.LabelStyle.Interval = 1;
            ChartFinalCheck.ChartAreas[0].AxisX.LabelStyle.Angle = -55;

            ChartFinalCheck.Titles[0].Text = "Final Check User (KPI " + KPIDays.ToString() + " Days)";
        }
    }
    private void CreateChartBillDispatchUser(string ReportMonth)
    {
        // Bill Dispatch KPI - Bill Completed By User - File Sent Date To Completed Date

        DataSet dsBillingDispatch = BillingOperation.GetMISBillingUser(ReportMonth, 6, KPIDays, LoggedInUser.glFinYearId);

        DataTable dtDispatch = dsBillingDispatch.Tables[0];

        if (dtDispatch.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dtDispatch.Rows.Count];
            int[] YPointMember = new int[dtDispatch.Rows.Count];

            for (int count = 0; count < dtDispatch.Rows.Count; count++)
            {
                ChartDispatch.Series[0].Points.AddXY(dtDispatch.Rows[count]["UserName"].ToString(), dtDispatch.Rows[count]["PassPercent"].ToString());

                ChartDispatch.Series[0].Points[count].Label = dtDispatch.Rows[count]["TotalJob"].ToString();
                ChartDispatch.Series[0].Points[count].ToolTip = dtDispatch.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + dtDispatch.Rows[count]["KPIJob"].ToString() + "/" + dtDispatch.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartDispatch.Series[0].ChartType = SeriesChartType.Column;

            ChartDispatch.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisY.LabelStyle.Format = "{#}%";

            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisY.Title = "Bill Dispatch KPI %";
            ChartDispatch.ChartAreas["AreaBillDispatch"].AxisX.LabelStyle.Interval = 1;
            ChartDispatch.ChartAreas[0].AxisX.LabelStyle.Angle = -55;

            ChartDispatch.Titles[0].Text = "Dispatch User (KPI " + KPIDays.ToString() + " Days)";
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