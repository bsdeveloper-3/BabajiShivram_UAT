using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_ReportCustomerTransport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkXls);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Customer Report";
    }

    protected void lnkXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Customer_Transport_Job_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvCustTransport.AllowPaging = false;
        gvCustTransport.AllowSorting = false;

        gvCustTransport.DataSourceID = "DataSourceCustTransport";
        gvCustTransport.DataBind();

        //Remove Controls
        this.RemoveControls(gvCustTransport);

        gvCustTransport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }
}