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

public partial class InTransitWarehouse : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["JobId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Trans Warehouse Delivery";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For General Warehouse!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = TransitWarehouseSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "InTransitWarehouse.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
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

    protected void gvJobDetail_RowCommad(Object sender, GridViewCommandEventArgs e)
    {

        int result = -123;
        if (e.CommandName.ToLower() == "move")
        {
            int JobId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["JobId"] = JobId.ToString();

            result = DBOperations.AddTransitWarehouse(JobId, LoggedInUser.glUserId);

            if (result == 0)
            {
                //lblMessage.Text = "Job is sucessfully moved in Under Delivery !";
                //lblMessage.CssClass = "success";

                //gvJobDetail.DataSourceID = "TransitWarehouseSqlDataSource";
                //gvJobDetail.DataBind();

                Response.Redirect("WarehouseDelivery.aspx");
            }
            else if (result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime."; ;
                lblMessage.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblMessage.Text = "Job already moved To Under Delivery!";
                lblMessage.CssClass = "errorMsg";

                gvJobDetail.DataSourceID = "TransitWarehouseSqlDataSource";
                gvJobDetail.DataBind();
            }
        }//END IF
    }

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
            DataFilter1.FilterSessionID = "InTransitWarehouse.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "JobsInGeneralWarehouse_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;

        gvJobDetail.Columns[10].Visible = false;

        gvJobDetail.Caption = "In General Warehouse Job Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "InTransitWarehouse.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        //gvJobDetail.DataSourceID = "TransitWarehouseSqlDataSource";
        //gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}
