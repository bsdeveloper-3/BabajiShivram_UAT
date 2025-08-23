using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Reports_MISBillingIncentiveUser : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Report KPI";

            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");

            // DBOperations.FillBranch(ddBabajiBranch);
        }

        // Get Billing Overall KPI and Stage wise KPI
        string strReportMonth = txtReportDate.Text.Trim();
        int BranchID = 0;// Convert.ToInt32(ddBabajiBranch.SelectedValue);

        BindReport(strReportMonth, BranchID);

    }

    private void BindReport(string ReportMonth, int BranchId)
    {
        DataSet dsBillingKPI = BillingOperation.GetMISBillingIncentiveUser(ReportMonth, BranchId);

        gvKPIReport.DataSource = dsBillingKPI;
        gvKPIReport.DataBind();

    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Billing-User-Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvKPIReport.AllowPaging = false;
        gvKPIReport.AllowSorting = false;

        gvKPIReport.Caption = "Billing User Report " + txtReportDate.Text.Trim();

        gvKPIReport.DataBind();

        gvKPIReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    protected void lnkKPICriteria_Click(object sender, EventArgs e)
    {
        Response.Redirect("../UploadFiles/Billing_Incentive_Rule.docx");

    }
}