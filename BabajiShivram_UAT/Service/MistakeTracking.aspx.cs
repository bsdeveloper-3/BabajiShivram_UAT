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

public partial class MistakeTracking : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Mistake Tracking";
        if (!IsPostBack)
        {
            if (gvMistakeDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Mistake Detail Found!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        //

        DataFilter1.DataSource = MistakeDetailSqlDataSource;
        DataFilter1.DataColumns = gvMistakeDetail.Columns;
        DataFilter1.FilterSessionID = "MistakeTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvMistakeDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvMistakeDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "update")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strMistakeId = gvMistakeDetail.DataKeys[gvrow.RowIndex].Value.ToString();
                        
        }

        else if (e.CommandName.ToLower() == "cancel")
        {
            lblMessage.Text = "";
            gvMistakeDetail.EditIndex = -1;
        }
    }

    protected void gvMistakeDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvMistakeDetail.EditIndex = e.NewEditIndex;
    }

    protected void gvMistakeDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int MistakeId = Convert.ToInt32(gvMistakeDetail.DataKeys[e.RowIndex].Value.ToString());

        int Amount = 0, StatusId = 0; 
        string CustomerName = "", MistakeRemarks = "" ;
        DateTime dtMistakeDate = DateTime.Now;
        
        TextBox txtMistakeDate = (TextBox)gvMistakeDetail.Rows[e.RowIndex].FindControl("txtMisakeDate");
        TextBox txtAmount = (TextBox)gvMistakeDetail.Rows[e.RowIndex].FindControl("txtAmount");
        TextBox txtCustomer = (TextBox)gvMistakeDetail.Rows[e.RowIndex].FindControl("txtCustomer");
        TextBox txtRemarks = (TextBox)gvMistakeDetail.Rows[e.RowIndex].FindControl("txtRemarks");

        DropDownList ddStatus = (DropDownList)gvMistakeDetail.Rows[e.RowIndex].FindControl("ddStatus");
        
        CustomerName = txtCustomer.Text.Trim();
        MistakeRemarks = txtRemarks.Text.Trim();
        StatusId = Convert.ToInt32(ddStatus.SelectedValue);

        if (txtMistakeDate.Text != "")
        {
            dtMistakeDate = Commonfunctions.CDateTime(txtMistakeDate.Text.Trim());
        }
        if (txtAmount.Text.Trim() != "")
        {
            Amount = Convert.ToInt32(txtAmount.Text.Trim());
        }
        
        int result = DBOperations.UpdateMistakeLog(MistakeId, dtMistakeDate,Amount, MistakeRemarks, CustomerName, StatusId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblMessage.Text = "User Mistake Log Updated Successfully";
            lblMessage.CssClass = "success";

            gvMistakeDetail.EditIndex = -1;
            e.Cancel = true;
        }
        else if (result == 1)
        {
            lblMessage.Text = "System Error! Please try after sometime.";
            lblMessage.CssClass = "errorMsg";
            e.Cancel = true;
        }
        else if (result == 2)
        {
            lblMessage.Text = "Mistake Log Details Not Found";
            lblMessage.CssClass = "errorMsg";
            e.Cancel = true;
        }
    }

    protected void gvMistakeDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvMistakeDetail.EditIndex = -1;
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
            DataFilter1.FilterSessionID = "MistakeTracking.aspx";
            DataFilter1.FilterDataSource();
            gvMistakeDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}
