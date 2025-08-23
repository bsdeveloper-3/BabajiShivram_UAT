using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;

public partial class Freight_FreightMISMonthCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExportCustomer);
    }

    #region ExportData

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    
    protected void lnkExportCustomer_Click(object sender, EventArgs e)
    {
        string strFileName = "FreightReport_Customer_" +  DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportCustomer("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    private void ExportCustomer(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvCustomerReport.AllowPaging = false;
        gvCustomerReport.AllowSorting = false;

        gvCustomerReport.DataBind();

        gvCustomerReport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }
    #endregion
}