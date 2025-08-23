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

public partial class VehicleMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle Type Setup";

            lblresult.Visible = false;
            VehicleMasterDetails();
        }
    }
    
    protected void VehicleMasterDetails()
    {
        DataSet ds = new DataSet();
        ds = DBOperations.GetVehicleMasterDetails();
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVehicleMaster.DataSource = ds;
            gvVehicleMaster.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvVehicleMaster.DataSource = ds;
            gvVehicleMaster.DataBind();

            int columncount = gvVehicleMaster.Rows[0].Cells.Count;
            gvVehicleMaster.Rows[0].Cells.Clear();
            gvVehicleMaster.Rows[0].Cells.Add(new TableCell());
            gvVehicleMaster.Rows[0].Cells[0].ColumnSpan = columncount;
            gvVehicleMaster.Rows[0].Cells[0].Text = "No Records Found!";

        }
    }

    protected void gvVehicleMaster_RowCommand(object sender,GridViewCommandEventArgs e )
    {
        lblresult.Visible = true;
        if (e.CommandName.ToLower() == "insert")
        { 
            TextBox lid=gvVehicleMaster.FooterRow.FindControl("txtlidfooter") as TextBox ;
            TextBox SName=gvVehicleMaster.FooterRow.FindControl("txtVehicle_Namefooter") as TextBox ;
            TextBox sRemarks = gvVehicleMaster.FooterRow.FindControl("txtVehicle_Remarkfooter") as TextBox;

            if (SName.Text.Trim() != "")
            {
                int result = DBOperations.AddVehicle(SName.Text.Trim(), sRemarks.Text.Trim(), LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblresult.Text = SName.Text.Trim() + "  Vehicle added successfully.";
                    lblresult.CssClass = "success";
                    VehicleMasterDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Vehicle Name Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }
            }
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!"; 
            }
        }
    }
    
    protected void gvVehicleMaster_RowUpdating(object sender,GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32 (gvVehicleMaster.DataKeys [e.RowIndex].Value.ToString());
        TextBox txtVehicle_Namefooter = (TextBox)gvVehicleMaster.Rows[e.RowIndex].FindControl("txtVehicle_Name");
        TextBox txtVehicle_Remarkfooter = (TextBox)gvVehicleMaster.Rows[e.RowIndex].FindControl("txtVehicle_Remark");

        if (txtVehicle_Namefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateVehicle(lid, txtVehicle_Namefooter.Text.Trim(),
                txtVehicle_Remarkfooter.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtVehicle_Namefooter.Text.Trim() + " Vehicle Details Updated Successfully.";
                gvVehicleMaster.EditIndex = -1;
                VehicleMasterDetails();
            }
            else if (result == 1)
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " System Error! Please Try After Sometime.";
            }
            else if (result == 2)
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Vehicle Name Already Added!";
            }
        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }
    
    protected void gvVehicleMaster_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        gvVehicleMaster.PageIndex = e.NewPageIndex;
        gvVehicleMaster.DataBind();
        VehicleMasterDetails(); 
    }   
    
    protected void gvVehicleMaster_RowDeleting(object sender,GridViewDeleteEventArgs e)    
    {
        lblresult.Visible = true ; 

        int lid = Convert.ToInt32(gvVehicleMaster.DataKeys[e.RowIndex].Values["lid"].ToString());
        
        int result = DBOperations.DeleteVehicle(lid,LoggedInUser.glUserId);   

        if (result == 0)
        {
            lblresult.Text=" Vehicle Deleted Successfully!";
            lblresult.CssClass ="success"; 
            VehicleMasterDetails();  
        }
        else if (result == 1)
        {
            lblresult.Text =" System Error! Please Try After Sometime.";
            lblresult.CssClass="errorMsg";  
        }
    }

    protected void gvVehicleMaster_RowEditing(object sender,GridViewEditEventArgs e)
    {
        gvVehicleMaster.EditIndex = e.NewEditIndex;
        VehicleMasterDetails();
        lblresult.Text = ""; 
        lblresult.Visible = false;
    }
    
    protected void gvVehicleMaster_RowCancelingEdit(object sender,GridViewCancelEditEventArgs e)
    {
        gvVehicleMaster.EditIndex = -1;
        VehicleMasterDetails();
        lblresult.Text = "";
        lblresult.Visible = false; 
    }
}
