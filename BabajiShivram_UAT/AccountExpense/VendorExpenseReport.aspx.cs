using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Data;
public partial class AccountExpense_VendorExpenseReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Vendor Payment";

        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        gvPaymentReport.DataBind();

        if (gvPaymentReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "Vendor_Payment_Report_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        gvPaymentReport.DataBind();

        if (gvPaymentReport.Rows.Count < 1)
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
            gvPaymentReport.AllowPaging = false;
            gvPaymentReport.AllowSorting = false;

            gvPaymentReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}