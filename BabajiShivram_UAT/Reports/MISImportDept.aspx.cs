using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
public partial class Reports_MISImportDept : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Import Dept Report";

            DBOperations.FillBranch(ddBabajiBranch);

            ddBabajiBranch.Items[0].Text = "--All Branch--";
        }

        //string strReportMonth = txtReportDate.Text.Trim();

        string strReportMonth = "01/01/2018";
        CreateChartJobOpen(strReportMonth);
        CreateChartIGM(strReportMonth);
        CreateChartChecklistPrepare(strReportMonth);
        CreateChartChecklistAudit(strReportMonth);
        CreateChartDOCollectioin(strReportMonth);
        CreateChartNoting(strReportMonth);
    }

    private void CreateChartJobOpen(string strReportMonth)
    {
        // Job Opening KPI - Same Day Job Creation

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMIS = BillingOperation.GetMISJobOpen(strReportMonth, BabajiBranchId);

        //*********************Sector Chart Start *******************************

        DataTable dtJobOpen = dsMIS.Tables[0];

        // Populate Job Openning data
        Random random = new Random();

        for (int i = 0; i < dtJobOpen.Rows.Count; i++)
        {
            if (dtJobOpen.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartJobOpen.Series["Series1"].Points.AddXY(dtJobOpen.Rows[i]["ReportMonth"].ToString(), dtJobOpen.Rows[i]["PassPercent"].ToString());
            }
            else if (dtJobOpen.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartJobOpen.Series["Series2"].Points.AddXY(dtJobOpen.Rows[i]["ReportMonth"].ToString(), dtJobOpen.Rows[i]["PassPercent"].ToString());
            }
            else if (dtJobOpen.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartJobOpen.Series["Series3"].Points.AddXY(dtJobOpen.Rows[i]["ReportMonth"].ToString(), dtJobOpen.Rows[i]["PassPercent"].ToString());
            }
        }


        // Set series Job chart type
        ChartJobOpen.Series["Series1"].ChartType = SeriesChartType.Line;
        ChartJobOpen.Series["Series2"].ChartType = SeriesChartType.Line;
        ChartJobOpen.Series["Series3"].ChartType = SeriesChartType.Line;
        ChartJobOpen.Series["Series2"].MarkerStyle = MarkerStyle.Square;
        ChartJobOpen.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;
        
        ChartJobOpen.Series["Series1"].IsValueShownAsLabel = false;
        ChartJobOpen.Series["Series2"].IsValueShownAsLabel = false;
        ChartJobOpen.Series["Series3"].IsValueShownAsLabel = true;

        // Set Y axis Data Type
        ChartJobOpen.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "{#}%";
        // Set X axis margin
        ChartJobOpen.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;
        ChartJobOpen.ChartAreas["ChartArea1"].AxisX.LabelStyle.Interval = 1;
        
    }

    private void CreateChartIGM(string strReportMonth)
    {
        // IGM Dept KPI - IGM Filed On or Before ETA

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMISIGM = BillingOperation.GetMISIGM(strReportMonth, BabajiBranchId);

        //*********************IGM Chart Start *******************************

        DataTable dtIGM = dsMISIGM.Tables[0];

        // Populate IGM Filing data
        Random random = new Random();

        for (int i = 0; i < dtIGM.Rows.Count; i++)
        {
            if (dtIGM.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartIGM.Series["Series1"].Points.AddXY(dtIGM.Rows[i]["ReportMonth"].ToString(), dtIGM.Rows[i]["PassPercent"].ToString());
            }
            else if (dtIGM.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartIGM.Series["Series2"].Points.AddXY(dtIGM.Rows[i]["ReportMonth"].ToString(), dtIGM.Rows[i]["PassPercent"].ToString());
            }
            else if (dtIGM.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartIGM.Series["Series3"].Points.AddXY(dtIGM.Rows[i]["ReportMonth"].ToString(), dtIGM.Rows[i]["PassPercent"].ToString());
            }
        }
        
        // Set series IGM chart type
        ChartIGM.Series["Series1"].ChartType = SeriesChartType.Line;
        ChartIGM.Series["Series2"].ChartType = SeriesChartType.Line;
        ChartIGM.Series["Series3"].ChartType = SeriesChartType.Line;
        ChartIGM.Series["Series2"].MarkerStyle = MarkerStyle.Square;
        ChartIGM.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;
        
        ChartIGM.Series["Series1"].IsValueShownAsLabel = false;
        ChartIGM.Series["Series2"].IsValueShownAsLabel = false;
        ChartIGM.Series["Series3"].IsValueShownAsLabel = true;

        // Set Y axis Data Type
        ChartIGM.ChartAreas["AreaIGM"].AxisY.LabelStyle.Format = "{#}%";
        // Set X axis margin
        ChartIGM.ChartAreas["AreaIGM"].AxisX.IsMarginVisible = true;
        ChartIGM.ChartAreas["AreaIGM"].AxisX.LabelStyle.Interval = 1;
    }

    private void CreateChartChecklistPrepare(string strReportMonth)
    {
        // Checklist Prepare KPI - Prepared in One Day of Job Creation Date

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMISChecklist = BillingOperation.GetMISChecklistPrepare(strReportMonth, BabajiBranchId);

        //*********************Checklist Preparation Chart Start *******************************

        DataTable dtChecklist = dsMISChecklist.Tables[0];

        // Populate Checklist Request data
        Random random = new Random();

        for (int i = 0; i < dtChecklist.Rows.Count; i++)
        {
            if (dtChecklist.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartChecklistPrepare.Series["Series1"].Points.AddXY(dtChecklist.Rows[i]["ReportMonth"].ToString(), dtChecklist.Rows[i]["PassPercent"].ToString());
            }
            else if (dtChecklist.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartChecklistPrepare.Series["Series2"].Points.AddXY(dtChecklist.Rows[i]["ReportMonth"].ToString(), dtChecklist.Rows[i]["PassPercent"].ToString());
            }
            else if (dtChecklist.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartChecklistPrepare.Series["Series3"].Points.AddXY(dtChecklist.Rows[i]["ReportMonth"].ToString(), dtChecklist.Rows[i]["PassPercent"].ToString());
            }
        }

        // Set series IGM chart type
        ChartChecklistPrepare.Series["Series1"].ChartType = SeriesChartType.Line;
        ChartChecklistPrepare.Series["Series2"].ChartType = SeriesChartType.Line;
        ChartChecklistPrepare.Series["Series3"].ChartType = SeriesChartType.Line;
        ChartChecklistPrepare.Series["Series2"].MarkerStyle = MarkerStyle.Square;
        ChartChecklistPrepare.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;

        ChartChecklistPrepare.Series["Series1"].IsValueShownAsLabel = false;
        ChartChecklistPrepare.Series["Series2"].IsValueShownAsLabel = false;
        ChartChecklistPrepare.Series["Series3"].IsValueShownAsLabel = true;

        // Set Y axis Data Type
        ChartChecklistPrepare.ChartAreas["AreaChecklistPrepare"].AxisY.LabelStyle.Format = "{#}%";
        // Set X axis margin
        ChartChecklistPrepare.ChartAreas["AreaChecklistPrepare"].AxisX.IsMarginVisible = true;
        ChartChecklistPrepare.ChartAreas["AreaChecklistPrepare"].AxisX.LabelStyle.Interval = 1;
    }

    private void CreateChartChecklistAudit(string strReportMonth)
    {
        // Checklist Audit KPI - Approval in One Day of Job Creation Date

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMISAudit = BillingOperation.GetMISChecklistAudit(strReportMonth, BabajiBranchId);

        //*********************Checklist Audit Chart Start *******************************

        DataTable dtAudit = dsMISAudit.Tables[0];

        // Populate Checklist Audit data
        Random random = new Random();

        for (int i = 0; i < dtAudit.Rows.Count; i++)
        {
            if (dtAudit.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartAudit.Series["Series1"].Points.AddXY(dtAudit.Rows[i]["ReportMonth"].ToString(), dtAudit.Rows[i]["PassPercent"].ToString());
            }
            else if (dtAudit.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartAudit.Series["Series2"].Points.AddXY(dtAudit.Rows[i]["ReportMonth"].ToString(), dtAudit.Rows[i]["PassPercent"].ToString());
            }
            else if (dtAudit.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartAudit.Series["Series3"].Points.AddXY(dtAudit.Rows[i]["ReportMonth"].ToString(), dtAudit.Rows[i]["PassPercent"].ToString());
            }
        }

        // Set series IGM chart type
        ChartAudit.Series["Series1"].ChartType = SeriesChartType.Line;
        ChartAudit.Series["Series2"].ChartType = SeriesChartType.Line;
        ChartAudit.Series["Series3"].ChartType = SeriesChartType.Line;
        ChartAudit.Series["Series2"].MarkerStyle = MarkerStyle.Square;
        ChartAudit.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;

        ChartAudit.Series["Series1"].IsValueShownAsLabel = false;
        ChartAudit.Series["Series2"].IsValueShownAsLabel = false;
        ChartAudit.Series["Series3"].IsValueShownAsLabel = true;

        // Set Y axis Data Type
        ChartAudit.ChartAreas["AreaAudit"].AxisY.LabelStyle.Format = "{#}%";
        // Set X axis margin
        ChartAudit.ChartAreas["AreaAudit"].AxisX.IsMarginVisible = true;
        ChartAudit.ChartAreas["AreaAudit"].AxisX.LabelStyle.Interval = 1;
    }

    private void CreateChartDOCollectioin(string strReportMonth)
    {
        // Checklist DO KPI - ETA To Final DO Date

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMISAudit = BillingOperation.GetMISDOCollection(strReportMonth, BabajiBranchId);

        //*********************Checklist Audit Chart Start *******************************

        DataTable dtAudit = dsMISAudit.Tables[0];

        // Populate Checklist Audit data
        Random random = new Random();

        for (int i = 0; i < dtAudit.Rows.Count; i++)
        {
            if (dtAudit.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartDO.Series["Series1"].Points.AddXY(dtAudit.Rows[i]["ReportMonth"].ToString(), dtAudit.Rows[i]["PassPercent"].ToString());
            }
            else if (dtAudit.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartDO.Series["Series2"].Points.AddXY(dtAudit.Rows[i]["ReportMonth"].ToString(), dtAudit.Rows[i]["PassPercent"].ToString());
            }
            else if (dtAudit.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartDO.Series["Series3"].Points.AddXY(dtAudit.Rows[i]["ReportMonth"].ToString(), dtAudit.Rows[i]["PassPercent"].ToString());
            }
        }

        // Set series IGM chart type
        ChartDO.Series["Series1"].ChartType = SeriesChartType.Line;
        ChartDO.Series["Series2"].ChartType = SeriesChartType.Line;
        ChartDO.Series["Series3"].ChartType = SeriesChartType.Line;
        ChartDO.Series["Series2"].MarkerStyle = MarkerStyle.Square;
        ChartDO.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;

        ChartDO.Series["Series1"].IsValueShownAsLabel = false;
        ChartDO.Series["Series2"].IsValueShownAsLabel = false;
        ChartDO.Series["Series3"].IsValueShownAsLabel = true;

        // Set Y axis Data Type
        ChartDO.ChartAreas["AreaDO"].AxisY.LabelStyle.Format = "{#}%";
        // Set X axis margin
        ChartDO.ChartAreas["AreaDO"].AxisX.IsMarginVisible = true;
        ChartDO.ChartAreas["AreaDO"].AxisX.LabelStyle.Interval = 1;
    }
    private void CreateChartNoting(string strReportMonth)
    {
        // Noting - ETA To BE Date, Exbond Job Creation To BE Date

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMISNoting = BillingOperation.GetMISNoting(strReportMonth, BabajiBranchId);

        //*********************Checklist Audit Chart Start *******************************

        DataTable dtNoting = dsMISNoting.Tables[0];

        // Populate Checklist Audit data
        Random random = new Random();

        for (int i = 0; i < dtNoting.Rows.Count; i++)
        {
            if (dtNoting.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartNoting.Series["Series1"].Points.AddXY(dtNoting.Rows[i]["ReportMonth"].ToString(), dtNoting.Rows[i]["PassPercent"].ToString());
            }
            else if (dtNoting.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartNoting.Series["Series2"].Points.AddXY(dtNoting.Rows[i]["ReportMonth"].ToString(), dtNoting.Rows[i]["PassPercent"].ToString());
            }
            else if (dtNoting.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartNoting.Series["Series3"].Points.AddXY(dtNoting.Rows[i]["ReportMonth"].ToString(), dtNoting.Rows[i]["PassPercent"].ToString());
            }
        }

        // Set series IGM chart type
        ChartNoting.Series["Series1"].ChartType = SeriesChartType.Line;
        ChartNoting.Series["Series2"].ChartType = SeriesChartType.Line;
        ChartNoting.Series["Series3"].ChartType = SeriesChartType.Line;
        ChartNoting.Series["Series2"].MarkerStyle = MarkerStyle.Square;
        ChartNoting.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;

        ChartNoting.Series["Series1"].IsValueShownAsLabel = false;
        ChartNoting.Series["Series2"].IsValueShownAsLabel = false;
        ChartNoting.Series["Series3"].IsValueShownAsLabel = true;

        // Set Y axis Data Type
        ChartNoting.ChartAreas["AreaNoting"].AxisY.LabelStyle.Format = "{#}%";
        // Set X axis margin
        ChartNoting.ChartAreas["AreaNoting"].AxisX.IsMarginVisible = true;
        ChartNoting.ChartAreas["AreaNoting"].AxisX.LabelStyle.Interval = 1;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
        
}