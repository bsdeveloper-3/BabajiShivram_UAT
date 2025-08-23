using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
public partial class Reports_MISFreightDept : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Dept Report";

            DBOperations.FillBranch(ddBabajiBranch);

            ddBabajiBranch.Items[0].Text = "--All Branch--";
        }

        int BranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        int KPIDays = 0;

        if (txtKPIDays.Text.Trim() != "")
        {
            KPIDays = Convert.ToInt32(txtKPIDays.Text.Trim());
        }

        CreateChartFreight();

    }

    private void CreateChartFreight()
    {
        // Job Opening KPI - Same Day Job Creation

        int BabajiBranchId = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        DataSet dsMIS = BillingOperation.GetMISFreightPerformance();

        //*********************Sector Chart Start *******************************

        DataTable dtBilling = dsMIS.Tables[0];

        // Populate Freight Data 

        for (int i = 0; i < dtBilling.Rows.Count; i++)
        {
            /************* Enquiry ********************/

            if (dtBilling.Rows[i]["StageID"].ToString() == "1" && dtBilling.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartEnquiry.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "1" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartEnquiry.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "1" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartEnquiry.Series["Series4"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }

            /************* Quote ********************/

            else if (dtBilling.Rows[i]["StageID"].ToString() == "2" && dtBilling.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartQuote.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "2" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartQuote.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "2" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartQuote.Series["Series4"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }

            /************* Award ********************/

            else if (dtBilling.Rows[i]["StageID"].ToString() == "3" && dtBilling.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartAward.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "3" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartAward.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "3" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartAward.Series["Series4"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }

            /************* Lost ********************/

            else if (dtBilling.Rows[i]["StageID"].ToString() == "4" && dtBilling.Rows[i]["ReportYear"].ToString() == "2016")
            {
                ChartLost.Series["Series2"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "4" && dtBilling.Rows[i]["ReportYear"].ToString() == "2017")
            {
                ChartLost.Series["Series3"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
            else if (dtBilling.Rows[i]["StageID"].ToString() == "4" && dtBilling.Rows[i]["ReportYear"].ToString() == "2018")
            {
                ChartLost.Series["Series4"].Points.AddXY(dtBilling.Rows[i]["ReportMonth"].ToString(), dtBilling.Rows[i]["EnqCount"].ToString());
            }
        }


        // Set Y axis Data Type &  Set X axis margin

        ChartEnquiry.ChartAreas["AreaEnquiry"].AxisX.IsMarginVisible = true;
        ChartEnquiry.ChartAreas["AreaEnquiry"].AxisX.LabelStyle.Interval = 1;
        ChartEnquiry.Series["Series4"].MarkerStyle = MarkerStyle.Diamond;
        ChartEnquiry.Series["Series4"].IsValueShownAsLabel = true;

        //

        ChartQuote.ChartAreas["AreaQuote"].AxisX.IsMarginVisible = true;
        ChartQuote.ChartAreas["AreaQuote"].AxisX.LabelStyle.Interval = 1;
        ChartQuote.Series["Series4"].MarkerStyle = MarkerStyle.Diamond;
        ChartQuote.Series["Series4"].IsValueShownAsLabel = true;
        //

        ChartAward.ChartAreas["AreaAward"].AxisX.IsMarginVisible = true;
        ChartAward.ChartAreas["AreaAward"].AxisX.LabelStyle.Interval = 1;
        ChartAward.Series["Series4"].MarkerStyle = MarkerStyle.Diamond;
        ChartAward.Series["Series4"].IsValueShownAsLabel = true;
        //

        ChartLost.ChartAreas["AreaLost"].AxisX.IsMarginVisible = true;
        ChartLost.ChartAreas["AreaLost"].AxisX.LabelStyle.Interval = 1;
        ChartLost.Series["Series4"].MarkerStyle = MarkerStyle.Diamond;
        ChartLost.Series["Series4"].IsValueShownAsLabel = true;
        
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
}