using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_VehicleDriver : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Vehicle Driver Status";
    }

    #region GridView Event
    
    protected void GridViewVehicle_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = GridViewVehicle.DataKeys[gvrow.RowIndex].Value.ToString();
        }

        if (e.CommandName.ToLower() == "cancel")
        {
            GridViewVehicle.EditIndex = -1;
        }
    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int VehicleID = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        int DriverID = 0;
        string strRemark = "";

        DropDownList ddDriver = (DropDownList)GridViewVehicle.Rows[e.RowIndex].FindControl("ddDriver");
        TextBox txtRemark = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRemark");

        if (VehicleID > 0)
        {
            DriverID = Convert.ToInt32(ddDriver.SelectedValue);
            strRemark = txtRemark.Text.Trim();
        }
        else
        {
            lblError.Text = "Please Select Vehicle Number!";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
            return;
        }

        if (VehicleID > 0)
        {
            int result = DBOperations.AddVehicleDriver(VehicleID, DriverID, strRemark, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Driver Detail Added Successfully.";
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
        }//END_IF
        else
        {
            e.Cancel = true;
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please Select Vehicle No!";
        }
    }

    protected void GridViewVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewVehicle.EditIndex = -1;
    }

    #endregion
}