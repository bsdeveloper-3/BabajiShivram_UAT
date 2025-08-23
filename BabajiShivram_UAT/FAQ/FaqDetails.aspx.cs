using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class FAQ_UpdateFaqDetails : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "FAQ Details";

      
        if (!IsPostBack)
        {

            Session["FaqId"] = null;

            int UserId = LoggedInUser.glUserId;


        }
        DataFilter1.DataSource = FAQDetailDataSorce;
        DataFilter1.DataColumns = GridFaqDetails.Columns;
        // DataFilter1.FilterSessionID = "StockHistory.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    protected void GridFaqDetails_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;

        }
    }

    protected void DataSourceFAQDetail_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@outPut"].Value);

        if (Result == 0)
        {
        lblerror.Text = "FAQ Record  Successfully Removed!";
            lblerror.CssClass = "success";

        }
        else if (Result == 1)
        {
            lblerror.Text = "System Error! Please try after sometime";
            lblerror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblerror.Text = "FAQ Record  Not Found!";
            lblerror.CssClass = "errorMsg";
        }
    }
    protected void GridFaqDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
      
        if (e.CommandName.ToLower() == "select")
        {

            string strServiceId = (string)e.CommandArgument;
            Session["FaqId"] = strServiceId;

            Response.Redirect("FaqUpdate.aspx");
        }
    }
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
            // DataFilter1.FilterSessionID = "StockHistory.aspx";
            DataFilter1.FilterDataSource();
            GridFaqDetails.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
}