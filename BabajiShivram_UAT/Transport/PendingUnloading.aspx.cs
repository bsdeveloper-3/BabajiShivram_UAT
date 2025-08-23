using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Transport_PendingUnloading : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Movement Pending Report";
        }
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Movement Pending On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvMovementPending.AllowPaging = false;
        gvMovementPending.AllowSorting = false;
        gvMovementPending.Caption = "Movement Pending On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        gvMovementPending.DataBind();
        gvMovementPending.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvMovementPending.DataBind();
    }
}