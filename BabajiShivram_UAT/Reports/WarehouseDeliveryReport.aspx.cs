using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Reports_WarehouseDeliveryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnShowReport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Warehouse Delivery Report";
                        
            DBOperations.FillPort(ddPortList);
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "WarehouseReport_"+ ddTransitType.SelectedItem.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

        /************************************************************
        gvWarehouseReport.DataSource = datasrcWarehouse;
        gvWarehouseReport.DataBind();

        if (gvWarehouseReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
        ************************************************************/
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "WarehouseReport_" + ddTransitType.SelectedItem.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        gvWarehouseReport.DataSource = datasrcWarehouse;
        gvWarehouseReport.DataBind();

        if (gvWarehouseReport.Rows.Count < 1)
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
            gvWarehouseReport.AllowPaging = false;
            gvWarehouseReport.AllowSorting = false;

            gvWarehouseReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}