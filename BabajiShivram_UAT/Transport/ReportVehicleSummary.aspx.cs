using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_ReportVehicleSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkXls);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle Summary Status";

            
            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }
    }
    protected void lnkXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_Month_Summary_" + txtReportDate.Text + ".xls";
        ExportToExcel("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportToExcel(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridViewVehicle.AllowPaging = false;
        GridViewVehicle.AllowSorting = false;

        GridViewVehicle.DataSourceID = "SqlDataSourceExp";
        GridViewVehicle.DataBind();
        
        GridViewVehicle.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
}