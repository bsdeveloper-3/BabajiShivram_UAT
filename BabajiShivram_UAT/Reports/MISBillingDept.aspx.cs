using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
public partial class Reports_MISBillingDept : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Dept Report";

            DBOperations.FillBranch(ddBabajiBranch);

            ddBabajiBranch.Items[0].Text = "--All Branch--";
        }

        int BranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        int KPIDays = 0;

        if (txtKPIDays.Text.Trim() != "")
        {
            KPIDays = Convert.ToInt32(txtKPIDays.Text.Trim());
        }

        CreateChartPerformacne(KPIDays, 0, 0);

    }

    private void CreateChartPerformacne(int KPIDays, int BranchId, int CustomerId)
    {
        // Job Opening KPI - Same Day Job Creation

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMIS = BillingOperation.GetMISBillingPerformance(KPIDays, BranchId, CustomerId);

        //*********************Sector Chart Start *******************************

        DataTable dtBilling = dsMIS.Tables[0];

        // Populate Scrutiny 
        
        for (int i = 0; i < dtBilling.Rows.Count; i++)
        {
            /************* Scrutiny ********************/
            
            if (dtBilling.Rows[i]["DeptID"].ToString() == "1" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartScrutiny.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "1" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartScrutiny.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }

            /************* Draft ********************/
            
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "2" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartDraft.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "2" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartDraft.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            /************* Draft Check ********************/
          
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "3" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartDraftCheck.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "3" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartDraftCheck.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            /************* Final Type ********************/
            
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "4" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartFinalType.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "4" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartFinalType.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            /************* Final Check ********************/
           
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "5" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartFinalCheck.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "5" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartFinalCheck.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            /************* Bill Dispatch ********************/
            
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "6" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartDispatch.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
            else if (dtBilling.Rows[i]["DeptID"].ToString() == "6" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartDispatch.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["PassPercent"].ToString());
            }
        }


        // Set Y axis Data Type &  Set X axis margin
        ChartScrutiny.ChartAreas["AreaScrutiny"].AxisY.LabelStyle.Format = "{#}%";
        ChartScrutiny.ChartAreas["AreaScrutiny"].AxisX.IsMarginVisible = true;
        ChartScrutiny.ChartAreas["AreaScrutiny"].AxisX.LabelStyle.Interval = 1;
        ChartScrutiny.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;
        ChartScrutiny.Series["Series3"].IsValueShownAsLabel = true;
        
        //
        ChartDraft.ChartAreas["AreaDraft"].AxisY.LabelStyle.Format = "{#}%";
        ChartDraft.ChartAreas["AreaDraft"].AxisX.IsMarginVisible = true;
        ChartDraft.ChartAreas["AreaDraft"].AxisX.LabelStyle.Interval = 1;
        ChartDraft.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;
        ChartDraft.Series["Series3"].IsValueShownAsLabel = true;
        //
        ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisY.LabelStyle.Format = "{#}%";
        ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisX.IsMarginVisible = true;
        ChartDraftCheck.ChartAreas["AreaDraftCheck"].AxisX.LabelStyle.Interval = 1;
        ChartDraftCheck.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;
        ChartDraftCheck.Series["Series3"].IsValueShownAsLabel = true;
        //
        
        ChartFinalType.ChartAreas["AreaFinalType"].AxisY.LabelStyle.Format = "{#}%";
        ChartFinalType.ChartAreas["AreaFinalType"].AxisX.IsMarginVisible = true;
        ChartFinalType.ChartAreas["AreaFinalType"].AxisX.LabelStyle.Interval = 1;
        ChartFinalType.Series["Series3"].MarkerStyle = MarkerStyle.Diamond;
        ChartFinalType.Series["Series3"].IsValueShownAsLabel = true;
        //
        ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisY.LabelStyle.Format = "{#}%";
        ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisX.IsMarginVisible = true;
        ChartFinalCheck.ChartAreas["AreaFinalCheck"].AxisX.LabelStyle.Interval = 1;
        ChartFinalCheck.Series["Series3"].IsValueShownAsLabel = true;
        ChartFinalCheck.Series["Series3"].IsValueShownAsLabel = true;
        //
        ChartDispatch.ChartAreas["AreaDispatch"].AxisY.LabelStyle.Format = "{#}%";
        ChartDispatch.ChartAreas["AreaDispatch"].AxisX.IsMarginVisible = true;
        ChartDispatch.ChartAreas["AreaDispatch"].AxisX.LabelStyle.Interval = 1;
        ChartDispatch.Series["Series3"].IsValueShownAsLabel = true;
        ChartDispatch.Series["Series3"].IsValueShownAsLabel = true;

    }
    
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
}