using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Reports_ABBPowerDutyReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "ABB Duty Report";

            DBOperations.FillPort(ddPortMS);

            ddPortMS.SelectedValue = "4"; //Mumbai Air Cargo
        }
    }

    #region Data Bind
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        gvReport.DataBind();
    }

    #endregion


    #region ExportData

    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "ABB_Power_Duty_Report_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvReport.AllowPaging = false;
            gvReport.AllowSorting = false;

            gvReport.DataBind();

            gvReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}