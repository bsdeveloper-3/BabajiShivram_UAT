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

public partial class EWayBill_ProductMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Product Master";
        }
    }

    protected void gvBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvBranch.Visible = false;
        fsMainBorder.Visible = false;
        if (gvBranch.SelectedIndex == -1)
        {
            //  FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }

        FormView1.DataBind();

    }

    protected void gvBranch_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region ForView Event

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        Page.Validate("Required");
    }

    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "cancel")
        {
            gvBranch.Visible = true;
            gvBranch.SelectedIndex = -1;
            fsMainBorder.Visible = true;
        }
        else
        {
            gvBranch.Visible = false;
            fsMainBorder.Visible = false;
        }
    }

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
           // e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }
    }

    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
        }
    }

    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lberror.Visible = true;
        if (Result > 0)
        {
            lberror.Text = "Branch Detail Added Successfully";
            lberror.CssClass = "success";
            FormViewDataSource.SelectParameters[0].DefaultValue = Result.ToString();
            gvBranch.SelectedIndex = -1;
            gvBranch.DataBind();
        }
        else if (Result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "Branch Already Exist.";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lberror.Visible = true;
        if (Result > 0)
        {
            lberror.Text = "Branch Detail Updated Successfully";
            lberror.CssClass = "success";

            gvBranch.DataBind();
        }
        else if (Result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "Branch Already Exist.";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Branch Deleted Successfully !";
            lberror.CssClass = "success";

            gvBranch.SelectedIndex = -1;
            gvBranch.DataBind();
            gvBranch.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion

    #region Auto Complete Port Event
    protected void btnAddPort_Cick(object sender, EventArgs e)
    {
        int Result = -123;
        HiddenField cboPortId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnPortId");
        TextBox cboPortName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$txtPortName");
        int PortId = int.Parse(cboPortId.Value);

        HiddenField cboBranchId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnBranchId");
        int BranchId = int.Parse(cboBranchId.Value);

        lberror.Visible = true;
        if (PortId > 0 && BranchId > 0)
        {
            Result = DBOperations.AddBranchPort(BranchId, PortId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lberror.Text = "Port Added Successfully";
                lberror.CssClass = "success";
                cboPortName.Text = String.Empty;
                cboPortId.Value = "0";

                SqlDataSource cboDataSourcePort = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourcePort");
                cboDataSourcePort.SelectParameters[0].DefaultValue = BranchId.ToString();

                GridView cboGridViewPort = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvPort");
                cboGridViewPort.DataBind();
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please Try After Sometime";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lberror.Text = "Port Already Exist!";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            // Error
            lberror.Text = "Please Select Port Name From List!";
            lberror.CssClass = "errorMsg";
        }
    }
    #endregion

    #region Auto Complete WareHouse Event
    protected void btnAddWareHouse_Cick(object sender, EventArgs e)
    {
        int Result = -123;
        HiddenField cboWarehouseId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnWareHouseId");
        TextBox cboWarehouseName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$txtWareHouseName");
        int WareHouseId = int.Parse(cboWarehouseId.Value);

        HiddenField cboBranchId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnBranchId");
        int BranchId = int.Parse(cboBranchId.Value);

        lberror.Visible = true;
        if (WareHouseId > 0 && BranchId > 0)
        {
            Result = DBOperations.AddBranchWarehouse(BranchId, WareHouseId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lberror.Text = "Warehouse Added Successfully";
                lberror.CssClass = "success";
                cboWarehouseName.Text = String.Empty;
                cboWarehouseId.Value = "0";

                SqlDataSource cboDataSourceWarehouse = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceWareHouse");
                cboDataSourceWarehouse.SelectParameters[0].DefaultValue = BranchId.ToString();

                GridView cboGridViewWareHouse = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvWareHouse");
                cboGridViewWareHouse.DataBind();
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please Try After Sometime";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lberror.Text = "Warehouse Already Exist!";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            // Error
            lberror.Text = "Please Select Warehouse Name From List!";
            lberror.CssClass = "errorMsg";
        }

    }
    #endregion

    #region Auto Complete CFS Event
    protected void btnAddCFS_Cick(object sender, EventArgs e)
    {
        int Result = -123;
        HiddenField cboCFSId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnCFSId");
        TextBox cboCFSName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$txtCFSName");
        int CFSId = int.Parse(cboCFSId.Value);

        HiddenField cboBranchId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnBranchId");
        int BranchId = int.Parse(cboBranchId.Value);

        lberror.Visible = true;
        if (CFSId > 0 && BranchId > 0)
        {
            Result = DBOperations.AddBranchCFS(BranchId, CFSId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lberror.Text = "CFS Added Successfully";
                lberror.CssClass = "success";
                cboCFSName.Text = String.Empty;
                cboCFSId.Value = "0";

                SqlDataSource cboDataSourceCFS = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceCFS");
                cboDataSourceCFS.SelectParameters[0].DefaultValue = BranchId.ToString();

                GridView cboGridViewCFS = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvCFS");
                cboGridViewCFS.DataBind();
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please Try After Sometime";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lberror.Text = "CFS Already Exist!";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            // Error
            lberror.Text = "Please Select CFS Name From List!";
            lberror.CssClass = "errorMsg";
        }

    }
    #endregion
}