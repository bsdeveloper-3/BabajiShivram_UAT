using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_RequestRate : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["TRId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transportation Rate Pending";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Details Found For Rate Request!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = TruckRequestSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "RequestRate.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
        
    #region GridView Event

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            bool IsVehiclePlaced = false;

            if ((DataBinder.Eval(e.Row.DataItem, "IsVehiclePlaced")) != DBNull.Value)
            {
                IsVehiclePlaced = (bool)(DataBinder.Eval(e.Row.DataItem, "IsVehiclePlaced"));
                if (IsVehiclePlaced == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#2266bb");
                    e.Row.ToolTip = "Vehicle Placed";
                }
                else if (IsVehiclePlaced == false)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#FFFF00");
                    e.Row.ToolTip = "Pending Update";
                }
            }
        }
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strJobId = (string)e.CommandArgument;

            Session["TRId"] = strJobId;

            Response.Redirect("VehicleDetail.aspx");
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
            DataFilter1.FilterSessionID = "RequestRate.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}