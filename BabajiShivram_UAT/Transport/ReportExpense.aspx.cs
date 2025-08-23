using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_ReportExpense : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkXls);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Expense Report";
    }

    protected void ddMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvSummaryMonth.DataBind();
    }

    protected void lnkXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_ Maintenance_Expense_" + ddMonth.SelectedItem.Text +"_"+ DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvSummaryMonth.AllowPaging = false;
        gvSummaryMonth.AllowSorting = false;

        gvSummaryMonth.DataSourceID = "DataSourceMonthExpense";
        gvSummaryMonth.DataBind();
        
        gvSummaryMonth.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    
}