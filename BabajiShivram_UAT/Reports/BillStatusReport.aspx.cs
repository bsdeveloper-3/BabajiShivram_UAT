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

public partial class Reports_BillStatusReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Drpbranch.DataSource = DsAllBranch;
            Drpbranch.DataValueField = "lid";
            Drpbranch.DataTextField = "BranchName";
            Drpbranch.DataBind();

            DrpCustomer.DataSource = DsCustomer;
            DrpCustomer.DataValueField = "lid";
            DrpCustomer.DataTextField = "CustName";
            DrpCustomer.DataBind();
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        gvbillReport.Visible = true;
        gvbillReport.DataSource = DsbillReports;
        gvbillReport.DataBind();

        if (gvbillReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "BillReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void Cancel_OnClick(Object sender, EventArgs e)
    {
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        Drpbranch.SelectedValue = "0";
        DrpCustomer.SelectedValue = "0";
        gvbillReport.Visible = false;

    }

    private void ExportFunction(string header, string contentType)
    {

        gvbillReport.DataSource = DsbillReports;
        gvbillReport.DataBind();

        if (gvbillReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
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
            gvbillReport.AllowPaging = false;
            gvbillReport.AllowSorting = false;

            gvbillReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}