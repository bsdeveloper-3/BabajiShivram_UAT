using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
public partial class AccountExpense_AddtnlExpense : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        CalFromDate.EndDate = DateTime.Today;

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Additional Expenses Report";

            DBOperations.FillBranch(ddBranch);
        }
    }

    protected void btnShowReport_OnClick(object Sender, EventArgs e)
    {
        gvJobExpensesReport.DataSource = datasrcJobExpenses;
        gvJobExpensesReport.DataBind();

        if (gvJobExpensesReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "JobExpensesReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        gvJobExpensesReport.DataSource = datasrcJobExpenses;
        gvJobExpensesReport.DataBind();

        if (gvJobExpensesReport.Rows.Count < 1)
        {
            // lblMessage.Text = "No Record Found!";
            // lblMessage.CssClass = "errorMsg";
        }
        else
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvJobExpensesReport.AllowPaging = false;
            gvJobExpensesReport.AllowSorting = false;

            gvJobExpensesReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}