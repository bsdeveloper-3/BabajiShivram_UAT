using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Transport_InvoicePending : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Invoice Pending Report";
        }
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Invoice Pending On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvInvoicePending.AllowPaging = false;
        gvInvoicePending.AllowSorting = false;
        gvInvoicePending.Caption = "Invoice Pending On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        gvInvoicePending.DataBind();
        gvInvoicePending.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
}