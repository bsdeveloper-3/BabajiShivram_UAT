using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InvoiceTrack_InvoiceTracking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Tracking";

        DataFilter1.DataSource = SqlDataSourceTracking;
        DataFilter1.DataColumns = gvInvoiceDetail.Columns;
        DataFilter1.FilterSessionID = "InvoiceTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvInvoiceDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region Data Filter1

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
            DataFilter1.DataColumns = gvInvoiceDetail.Columns;
            DataFilter1.FilterSessionID = "InvoiceTracking.aspx";
            DataFilter1.FilterDataSource();
            gvInvoiceDetail.DataBind();
            if (gvInvoiceDetail.Rows.Count == 0)
            {
                lblError.Text = "No Detail Found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "";
            }
            
        }
        catch (Exception ex)
        {
            //DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }


    #endregion
}