using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
public partial class Reports_MISImportUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Import User Performance";
            
            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");

            DBOperations.FillBranch(ddBabajiBranch);

            ddBabajiBranch.Items[0].Text = "--All Branch--";
        }

        string strReportMonth = txtReportDate.Text.Trim();
            
        CreateChartJobUser(strReportMonth);
        CreateChartIGMUser(strReportMonth);
        CreateChartChecklistUser(strReportMonth);
        CreateChartAuditUser(strReportMonth);
        CreateChartNotingUser(strReportMonth);
        CreateChartDOUser(strReportMonth);
    }       
    private void CreateChartJobUser(string ReportMonth)
    {
        // Job Opening User (Job Count - Same Day) - 
        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);
        DataSet dsUserJob = BillingOperation.GetMISJobOpenUser(ReportMonth, BabajiBranchId);

        DataTable JobUser = dsUserJob.Tables[0];

        if (JobUser.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobUser.Rows.Count];
            int[] YPointMember = new int[JobUser.Rows.Count];

            int[] ZPointMember = new int[JobUser.Rows.Count];

            for (int count = 0; count < JobUser.Rows.Count; count++)
            {
                ChartJobUser.Series[0].Points.AddXY(JobUser.Rows[count]["JobUser"].ToString(), JobUser.Rows[count]["JobPercent"].ToString());

                ChartJobUser.Series[0].Points[count].Label = JobUser.Rows[count]["TotalJob"].ToString();
                ChartJobUser.Series[0].Points[count].ToolTip = JobUser.Rows[count]["JobPercent"].ToString() + "%" +
                    "  " + JobUser.Rows[count]["KPIJob"].ToString() +"/" + JobUser.Rows[count]["TotalJob"].ToString();
                
            }

            //binding chart control  
            
            ChartJobUser.Series[0].ChartType = SeriesChartType.Column;
                        
            ChartJobUser.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartJobUser.ChartAreas["AreaUser"].AxisY.LabelStyle.Format = "{#}%";

            ChartJobUser.ChartAreas["AreaUser"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartJobUser.ChartAreas["AreaUser"].AxisY.Title = "Job Open KPI %";
            ChartJobUser.ChartAreas["AreaUser"].AxisX.LabelStyle.Interval = 1;
            ChartJobUser.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateChartIGMUser(string ReportMonth)
    {
        // Job IGM User (IGM Count - ETA) - 
        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);
        DataSet dsUserJob = BillingOperation.GetMISIGMUser(ReportMonth, BabajiBranchId);

        DataTable JobUser = dsUserJob.Tables[0];

        if (JobUser.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobUser.Rows.Count];
            int[] YPointMember = new int[JobUser.Rows.Count];

            int[] ZPointMember = new int[JobUser.Rows.Count];

            for (int count = 0; count < JobUser.Rows.Count; count++)
            {
                ChartIGMUser.Series[0].Points.AddXY(JobUser.Rows[count]["IGMUser"].ToString(), JobUser.Rows[count]["PassPercent"].ToString());

                ChartIGMUser.Series[0].Points[count].Label = JobUser.Rows[count]["TotalJob"].ToString();
                ChartIGMUser.Series[0].Points[count].ToolTip = JobUser.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + JobUser.Rows[count]["KPIJob"].ToString() + "/" + JobUser.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  
            
            ChartIGMUser.Series[0].ChartType = SeriesChartType.Column;

            ChartIGMUser.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartIGMUser.ChartAreas["AreaIGM"].AxisY.LabelStyle.Format = "{#}%";

            ChartIGMUser.ChartAreas["AreaIGM"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartIGMUser.ChartAreas["AreaIGM"].AxisY.Title = "IGM Filing KPI %";
            ChartIGMUser.ChartAreas["AreaIGM"].AxisX.LabelStyle.Interval = 1;
            ChartIGMUser.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateChartChecklistUser(string ReportMonth)
    {
        // Checklist User (Checklist Count - Within a day) - 
        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);
        DataSet dsUserJob = BillingOperation.GetMISChecklistPrepareUser(ReportMonth, BabajiBranchId);

        DataTable JobUser = dsUserJob.Tables[0];

        if (JobUser.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobUser.Rows.Count];
            int[] YPointMember = new int[JobUser.Rows.Count];

            int[] ZPointMember = new int[JobUser.Rows.Count];

            for (int count = 0; count < JobUser.Rows.Count; count++)
            {
                ChartChecklist.Series[0].Points.AddXY(JobUser.Rows[count]["ChecklistUser"].ToString(), JobUser.Rows[count]["PassPercent"].ToString());

                ChartChecklist.Series[0].Points[count].Label = JobUser.Rows[count]["TotalJob"].ToString();
                ChartChecklist.Series[0].Points[count].ToolTip = JobUser.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + JobUser.Rows[count]["KPIJob"].ToString() + "/" + JobUser.Rows[count]["TotalJob"].ToString();
            }

            //binding chart control  

            ChartChecklist.Series[0].ChartType = SeriesChartType.Column;

            ChartChecklist.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartChecklist.ChartAreas["AreaChecklist"].AxisY.LabelStyle.Format = "{#}%";

            ChartChecklist.ChartAreas["AreaChecklist"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartChecklist.ChartAreas["AreaChecklist"].AxisY.Title = "CHecklist KPI %";
            ChartChecklist.ChartAreas["AreaChecklist"].AxisX.LabelStyle.Interval = 1;
            ChartChecklist.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateChartAuditUser(string ReportMonth)
    {
        // Checklist Audit (Checklist Count - Within a day) - 
        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);
        DataSet dsUserJob = BillingOperation.GetMISChecklistAuditUser(ReportMonth, BabajiBranchId);

        DataTable JobUser = dsUserJob.Tables[0];

        if (JobUser.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobUser.Rows.Count];
            int[] YPointMember = new int[JobUser.Rows.Count];

            int[] ZPointMember = new int[JobUser.Rows.Count];

            for (int count = 0; count < JobUser.Rows.Count; count++)
            {
                ChartAudit.Series[0].Points.AddXY(JobUser.Rows[count]["AuditUser"].ToString(), JobUser.Rows[count]["PassPercent"].ToString());

                ChartAudit.Series[0].Points[count].Label = JobUser.Rows[count]["TotalJob"].ToString();
                ChartAudit.Series[0].Points[count].ToolTip = JobUser.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + JobUser.Rows[count]["KPIJob"].ToString() + "/" + JobUser.Rows[count]["TotalJob"].ToString();
                
            }

            //binding chart control  

            ChartAudit.Series[0].ChartType = SeriesChartType.Column;

            ChartAudit.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartAudit.ChartAreas["AreaAudit"].AxisY.LabelStyle.Format = "{#}%";

            ChartAudit.ChartAreas["AreaAudit"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartAudit.ChartAreas["AreaAudit"].AxisY.Title = "Audit KPI %";
            ChartAudit.ChartAreas["AreaAudit"].AxisX.LabelStyle.Interval = 1;
            ChartAudit.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateChartDOUser(string ReportMonth)
    {
        // DO User (KPI - FCL 2 Days, LCL 3 Days - ETA To Final DO Date) 

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);
        DataSet dsUserJob = BillingOperation.GetMISDOUser(ReportMonth, BabajiBranchId);

        DataTable JobUser = dsUserJob.Tables[0];

        if (JobUser.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobUser.Rows.Count];
            int[] YPointMember = new int[JobUser.Rows.Count];

            int[] ZPointMember = new int[JobUser.Rows.Count];

            for (int count = 0; count < JobUser.Rows.Count; count++)
            {
                ChartDO.Series[0].Points.AddXY(JobUser.Rows[count]["DOUser"].ToString(), JobUser.Rows[count]["PassPercent"].ToString());

                ChartDO.Series[0].Points[count].Label = JobUser.Rows[count]["TotalDO"].ToString();
                ChartDO.Series[0].Points[count].ToolTip = JobUser.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + JobUser.Rows[count]["KPIDO"].ToString() + "/" + JobUser.Rows[count]["TotalDO"].ToString();
            }

            //binding chart control  

            ChartDO.Series[0].ChartType = SeriesChartType.Column;

            ChartDO.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartDO.ChartAreas["AreaDO"].AxisY.LabelStyle.Format = "{#}%";

            ChartDO.ChartAreas["AreaDO"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartDO.ChartAreas["AreaDO"].AxisY.Title = "DO KPI %";
            ChartDO.ChartAreas["AreaDO"].AxisX.LabelStyle.Interval = 1;
            ChartDO.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    private void CreateChartNotingUser(string ReportMonth)
    {
        // Noting User (FCL – 1 day, LCL - 3 days, ETA To BE Date, Exbond – 2 Days ) 

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);
        DataSet dsUserJob = BillingOperation.GetMISNotingUser(ReportMonth, BabajiBranchId);

        DataTable JobUser = dsUserJob.Tables[0];

        if (JobUser.Rows.Count > 0)
        {
            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[JobUser.Rows.Count];
            int[] YPointMember = new int[JobUser.Rows.Count];

            int[] ZPointMember = new int[JobUser.Rows.Count];

            for (int count = 0; count < JobUser.Rows.Count; count++)
            {
                ChartNoting.Series[0].Points.AddXY(JobUser.Rows[count]["NotingUser"].ToString(), JobUser.Rows[count]["PassPercent"].ToString());

                ChartNoting.Series[0].Points[count].Label = JobUser.Rows[count]["TotalNoting"].ToString();
                ChartNoting.Series[0].Points[count].ToolTip = JobUser.Rows[count]["PassPercent"].ToString() + "%" +
                    "  " + JobUser.Rows[count]["KPINoting"].ToString() + "/" + JobUser.Rows[count]["TotalNoting"].ToString();
            }

            //binding chart control  

            ChartNoting.Series[0].ChartType = SeriesChartType.Column;

            ChartNoting.Series[0].MarkerStyle = MarkerStyle.Triangle;

            // Set Y axis Data Type
            ChartNoting.ChartAreas["AreaNoting"].AxisY.LabelStyle.Format = "{#}%";

            ChartNoting.ChartAreas["AreaNoting"].AxisY.TitleFont = new System.Drawing.Font("Sans Serif", 12, FontStyle.Bold);
            ChartNoting.ChartAreas["AreaNoting"].AxisY.Title = "Noting KPI %";
            ChartNoting.ChartAreas["AreaNoting"].AxisX.LabelStyle.Interval = 1;
            ChartNoting.ChartAreas[0].AxisX.LabelStyle.Angle = -55;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
}