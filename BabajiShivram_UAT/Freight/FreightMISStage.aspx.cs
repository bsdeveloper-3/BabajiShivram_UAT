using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Freight_FreightMISStage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExportStage);
        //ScriptManager1.RegisterPostBackControl(lnkExportCustomer);
        
        if (!IsPostBack)
        {
            Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   = "Month Wise Status Report";
        }
        
        //GridViewHelper helper = new GridViewHelper(this.gvCustomer);
        //helper.RegisterGroup("Customer", true, true);
        //helper.ApplyGroupSort();

        //GridViewHelper helper2 = new GridViewHelper(this.gvCustomer);
        //helper2.RegisterSummary("Total", SummaryOperation.Sum);
        
        //GridViewHelper helper3 = new GridViewHelper(this.gvCustomerReport);
        //helper3.RegisterGroup("Customer", true, true);
        //helper3.RegisterSummary("Total", SummaryOperation.Sum, "Customer");
        //helper3.ApplyGroupSort();

    }

    #region ExportData

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkExportStage_Click(object sender, EventArgs e)
    {
        string strFileName = "FreightReport_Stage_" + txtCustomer.Text.Trim() + "_" + ddMode.SelectedValue.Trim() +DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportStage("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }


    protected void lnkExportCustomer_Click(object sender, EventArgs e)
    {
        string strFileName = "FreightReport_Customer_"+ddMode.SelectedValue.Trim()+ "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportCustomer("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }
        
    private void ExportStage(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvStageReport.AllowPaging = false;
        gvStageReport.AllowSorting = false;
                
        gvStageReport.DataBind();

        gvStageReport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

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