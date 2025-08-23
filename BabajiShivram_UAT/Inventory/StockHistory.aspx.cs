using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Inventory_StockHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Inventory Inward History";
        }

        //
        DataFilter1.DataSource = DataSorceStorckHistory;
        DataFilter1.DataColumns = GrvStockHistory.Columns;
        DataFilter1.FilterSessionID = "StockHistory.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }


    #region Export To Excel

    protected void lnkIMSXls_Click(object sender, EventArgs e)
    {
        string strFileName = "InventoryDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExcelExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GrvStockHistory.Visible = true;
        GrvStockHistory.AllowPaging = false;
        GrvStockHistory.AllowSorting = false;

        DataFilter1.FilterSessionID = "StockHistory.aspx";
        DataFilter1.FilterDataSource();
        GrvStockHistory.DataBind();

        //GrvStockHistory.DataSourceID = "DataSorceStorckHistory";
        //GrvStockHistory.DataBind();
        //Remove Controls
        this.RemoveControls(GrvStockHistory);
        GrvStockHistory.RenderControl(hw);
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

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "StockHistory.aspx";
            DataFilter1.FilterDataSource();
            GrvStockHistory.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    //#region GridView Event
    //protected void gvPreAlert_RowCommand(Object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToLower() == "select")
    //    {
    //        string strJobId = (string)e.CommandArgument;
    //        Session[" "] = strJobId;
    //        Response.Redirect("JobDetailTab.aspx");
    //    }
    //}

    //protected void gvPreAlert_PreRender(object sender, EventArgs e)
    //{
    //    GridView gv = (GridView)sender;
    //    GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
    //    if (gvr != null)
    //    {
    //        gvr.Visible = true;
    //        gv.TopPagerRow.Visible = true;
    //    }
    //}

    //#endregion
}