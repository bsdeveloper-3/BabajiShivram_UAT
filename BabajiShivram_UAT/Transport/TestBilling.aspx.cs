using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_TestBilling : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Bill";

            if (GridViewVehicle.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Transport Bill!";
                lblError.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceVehicle;
        DataFilter1.DataColumns = GridViewVehicle.Columns;
        DataFilter1.FilterSessionID = "TransBill.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GridView Event

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // For Edit Button Click. 
        // IF First Check Required BOE is Non-RMS
        /**********************************
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == GridViewVehicle.EditIndex)
        {
            Label cboFirstCheck = (Label)e.Row.FindControl("lblFirstCheck");
            if (cboFirstCheck.Text.ToLower() == "yes")
            {
                DropDownList cboRMS = (DropDownList)e.Row.FindControl("ddRMS");
                cboRMS.SelectedValue = "2"; // Non-RMS
                cboRMS.Enabled = false;
            }
        }
        **********************************/

    }

    protected void GridViewVehicle_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string TransReqId = GridViewVehicle.DataKeys[gvrow.RowIndex].Value.ToString();

            Session["TransReqId"] = TransReqId;
            Response.Redirect("TestBillDetail.aspx");
        }

    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int RateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());

        TextBox txtReportingDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtReportingDate");
        TextBox txtLoadingDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtLoadingDate");
        TextBox txtUnLoadingDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtUnLoadingDate");
        TextBox txtContReturnDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtContReturnDate");

        DateTime dtReportingDate = DateTime.MinValue;
        DateTime dtLoadingDate = DateTime.MinValue;
        DateTime dtUnLoadingDate = DateTime.MinValue;
        DateTime dtContReturnDate = DateTime.MinValue;

        if (txtReportingDate.Text.Trim() != "")
            dtReportingDate = Commonfunctions.CDateTime(txtReportingDate.Text.Trim());

        if (txtLoadingDate.Text.Trim() != "")
            dtLoadingDate = Commonfunctions.CDateTime(txtLoadingDate.Text.Trim());

        if (txtUnLoadingDate.Text.Trim() != "")
            dtUnLoadingDate = Commonfunctions.CDateTime(txtUnLoadingDate.Text.Trim());

        if (txtContReturnDate.Text.Trim() != "")
            dtContReturnDate = Commonfunctions.CDateTime(txtContReturnDate.Text.Trim());

        int result = DBOperations.AddTransMovement(RateId, dtUnLoadingDate, dtContReturnDate, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Detail Updated Successfully.";
            lblError.CssClass = "success";

            GridViewVehicle.EditIndex = -1;
            e.Cancel = true;

        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
    }

    protected void GridViewVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewVehicle.EditIndex = -1;
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
            DataFilter1.FilterSessionID = "TransBill.aspx";
            DataFilter1.FilterDataSource();
            GridViewVehicle.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}