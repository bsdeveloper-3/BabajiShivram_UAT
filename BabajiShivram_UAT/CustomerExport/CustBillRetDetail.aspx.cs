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

public partial class CustBillRetDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvBillReturnDetail);
        LoginClass loggedInClass = new LoginClass();

        Session["CustUserId"] = loggedInClass.glCustUserId;

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Return Detail";
            Session["JobId"] = null;
            if (gvBillReturnDetail.Rows.Count == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "No Job Found For Bill Return!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }
        DataFilter1.DataSource = BillReturnSqlDataSource;
        DataFilter1.DataColumns = gvBillReturnDetail.Columns;
        DataFilter1.FilterSessionID = "CustBillRetDetail.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvBillReturnDetaill_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region Data Filter


    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "CustBillRetDetail.aspx";
            DataFilter1.FilterDataSource();
            gvBillReturnDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion    

    protected void gvBillReturnDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblReturnReason = (Label)e.Row.FindControl("lblReturnReason");
            DropDownList ddlReason = (DropDownList)e.Row.FindControl("ddlBillReason");  // 
            Label lblBillReason = (Label)e.Row.FindControl("lblBillReason");

            if (lblReturnReason != null)
            {
                ddlReason.SelectedValue = lblReturnReason.Text.Trim();
                lblBillReason.Text = ddlReason.SelectedItem.Text.Trim();
            }
        }
    }
}