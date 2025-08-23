using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Transport_VehicleDelivery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Session["JobId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle Delivery";
        }

        DataFilter1.DataSource = DataSourcePendingDelivery;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "VehicleDelivery.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //gvJobDetail.DataBind();
    }

    #region GridView Event

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            Session["JobId"] = commandArgs[0].ToString();
            Session["ModuleId"] = commandArgs[1].ToString();
            Response.Redirect("UpdateDelivery.aspx");
        }
    }

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "VehicleDelivery.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "PendingDelivery_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        JobExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void JobExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;
        gvJobDetail.Caption = "Pending Delivery Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "VehicleDelivery.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

}