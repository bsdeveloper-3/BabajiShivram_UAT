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
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Drawing;


public partial class WareHouse : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle =(Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Warehouse Setup";

            lblresult.Visible = false;
            wareHouseDetails();
        }
    }
    protected void wareHouseDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetWareHouseDetails();
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWareHouse.DataSource = ds;
            gvWareHouse.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvWareHouse.DataSource = ds;
            gvWareHouse.DataBind();

            int columncount = gvWareHouse.Rows[0].Cells.Count;
            gvWareHouse.Rows[0].Cells.Clear();
            gvWareHouse.Rows[0].Cells.Add(new TableCell());
            gvWareHouse.Rows[0].Cells[0].ColumnSpan = columncount;
            gvWareHouse.Rows[0].Cells[0].Text = "No Records Found!";  
        }
    }

    protected void gvWareHouse_RowCommand(object sender,GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName.ToLower() == "insert")
        {
            TextBox txtWarehouseIdFooter = gvWareHouse.FooterRow.FindControl("txtlidFooter") as TextBox;
            TextBox txtWareHouseNameFooter = gvWareHouse.FooterRow.FindControl("txtWareHouse_NameFooter") as TextBox;
            TextBox txtWarehouseCodeFooter = gvWareHouse.FooterRow.FindControl("txtWareHouse_CodeFooter") as TextBox;

            TextBox txtWareHouseAddressFooter = gvWareHouse.FooterRow.FindControl("txtWareHouse_AddressFooter") as TextBox;
            TextBox txtWareHouseContactNameFooter = gvWareHouse.FooterRow.FindControl("txtWareHouse_ContactNameFooter") as TextBox;
            TextBox txtWareHouseContactNumberFooter = gvWareHouse.FooterRow.FindControl("txtWareHouse_ContactNumberFooter") as TextBox;
            TextBox txtWareHouseEmailIdFooter = gvWareHouse.FooterRow.FindControl("txtWareHouse_EmailIdFooter") as TextBox;

            DropDownList ddWarehouseType_Footer = gvWareHouse.FooterRow.FindControl("ddWarehouseType_Footer") as DropDownList;

            DropDownList ddWareHouse_StatusFooter = gvWareHouse.FooterRow.FindControl("ddWareHouse_StatusFooter") as DropDownList;
            
            string sName = "", sCode = "", sAddress = "", sContactName = "", sContactNumber = "", sEmail = "";
            bool bStatus = false;
            
            sName = txtWareHouseNameFooter.Text.Trim();
            sCode = txtWarehouseCodeFooter.Text.Trim();
            sAddress = txtWareHouseAddressFooter.Text.Trim();

            sContactName = txtWareHouseContactNameFooter.Text.Trim();
            sContactNumber = txtWareHouseContactNumberFooter.Text.Trim();
            sEmail = txtWareHouseEmailIdFooter.Text.Trim();

            bStatus = Convert.ToBoolean(ddWareHouse_StatusFooter.SelectedValue);

            int lType = Convert.ToInt32(ddWarehouseType_Footer.SelectedValue);

            if (sName != "" && sCode != "")
            {
                int result = DBOperations.AddWareHouse(sName,sCode,sAddress, sContactName, sContactNumber, sEmail,lType,bStatus, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = txtWarehouseIdFooter.Text.Trim() + "  Warehouse added successfully.";
                    lblresult.CssClass = "success";
                    wareHouseDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Warehouse Name Already Exist!";
                    lblresult.CssClass = "warning";
                }
            }
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvWareHouse_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvWareHouse.EditIndex = e.NewEditIndex;
        wareHouseDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvWareHouse_RowUpdating(object sender, GridViewUpdateEventArgs e )  
    {
        int lid = Convert.ToInt32(gvWareHouse.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtWareHouseName = (TextBox)gvWareHouse.Rows[e.RowIndex].FindControl("txtWareHouse_Name");
        TextBox txtWareHouseCode = (TextBox)gvWareHouse.Rows[e.RowIndex].FindControl("txtWareHouse_Code");
        DropDownList ddWarehouseType = (DropDownList)gvWareHouse.Rows[e.RowIndex].FindControl("ddWarehouseType");

        TextBox txtWareHouseAddress = gvWareHouse.Rows[e.RowIndex].FindControl("txtWareHouse_Address") as TextBox;
        TextBox txtWareHouseContactName = gvWareHouse.Rows[e.RowIndex].FindControl("txtWareHouse_ContactName") as TextBox;
        TextBox txtWareHouseContactNumber = gvWareHouse.Rows[e.RowIndex].FindControl("txtWareHouse_ContactNumber") as TextBox;
        TextBox txtWareHouseEmailId = gvWareHouse.Rows[e.RowIndex].FindControl("txtWareHouse_EmailId") as TextBox;

        DropDownList ddWareHouse_Status = gvWareHouse.Rows[e.RowIndex].FindControl("ddWareHouse_Status") as DropDownList;

        string sName = "", sCode = "", sAddress = "", sContactName = "", sContactNumber = "", sEmail = "";
        bool bStatus = false;

        sName = txtWareHouseName.Text.Trim();
        sCode = txtWareHouseCode.Text.Trim();
        sAddress = txtWareHouseAddress.Text.Trim();

        sContactName = txtWareHouseContactName.Text.Trim();
        sContactNumber = txtWareHouseContactNumber.Text.Trim();
        sEmail = txtWareHouseEmailId.Text.Trim();

        bStatus = Convert.ToBoolean(ddWareHouse_Status.SelectedValue);

        int lType = Convert.ToInt32(ddWarehouseType.SelectedValue);

        if (sName != "" && sCode != "")
        {
            int result = DBOperations.UpdateWareHouse(lid, sName, sCode, sAddress, sContactName, sContactNumber, sEmail, lType, bStatus, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.CssClass = "success";

                lblresult.Text = sName + " Warehouse Details Updated Successfully.";
                gvWareHouse.EditIndex = -1;
                wareHouseDetails();
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Warehouse Name Already Added!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }
    
    protected void gvWareHouse_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvWareHouse.PageIndex = e.NewPageIndex;
        gvWareHouse.DataBind();
        wareHouseDetails();
    }
    
    protected void gvWareHouse_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert .ToInt32(gvWareHouse.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = DBOperations.DeleteWareHouse(lid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = " Warehouse Deleted Successfully!";
            lblresult.CssClass = "success";
            wareHouseDetails();
        }
        else if (result == 1)
        {
            lblresult.Text = " System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }
    
    protected void gvWareHouse_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvWareHouse.EditIndex = -1;
        wareHouseDetails();
        lblresult.Text = "";
        lblresult.Visible = false; 
    }
}

