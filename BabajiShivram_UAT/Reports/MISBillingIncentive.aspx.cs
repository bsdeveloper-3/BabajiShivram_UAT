using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_MISBillingIncentive : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Incentive KPI";

            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");

            DBOperations.FillBranch(ddBabajiBranch);
        }

        // Get Billing Overall KPI and Stage wise KPI
        string strReportMonth = txtReportDate.Text.Trim();
        int BranchID = Convert.ToInt32(ddBabajiBranch.SelectedValue);

        BindReport(strReportMonth, BranchID);

    }

    private void BindReport(string ReportMonth, int BranchId)
    {
        DataSet dsBillingKPI = BillingOperation.GetMISBillingIncentive(ReportMonth, BranchId);

        gvKPIReport.DataSource = dsBillingKPI;
        gvKPIReport.DataBind();

    }

    #region Exoprt PDF

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    #endregion

    protected void lnkKPICriteria_Click(object sender, EventArgs e)
    {
        Response.Redirect("../UploadFiles/Billing_Incentive_Rule.docx");
        
    }
}