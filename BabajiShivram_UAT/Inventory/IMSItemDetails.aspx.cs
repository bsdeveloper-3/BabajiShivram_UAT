using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Inventory_IMSItemDetails : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Inventory Item";

            InventoryOperation.FillIMSItemTypeMS(ddlCategory);

            BindItemDetail();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
        string itemtype = txtItemType.Text.Trim();

        int result = InventoryOperation.AddItemDetail(CategoryId,itemtype,LoggedInUser.glUserId);

        if (result == 0)
        {
            lblerror.Text = "System Error! Please Try After Sometime..";
            lblerror.CssClass = "success";
            
        }
        else if (result == 1)
        {
            lblerror.Text = "Item Details Added Successfully";
            lblerror.CssClass = "error";
        }
        else if (result == 2)
        {
            lblerror.Text = "Item Details Already Exist!";
            lblerror.CssClass = "info";
        }

        BindItemDetail();
    }
    
    protected void GrvIMSItemDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GrvIMSItemDetails.EditIndex = e.NewEditIndex;
        BindItemDetail();
    }

    protected void GrvIMSItemDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ItemID = Convert.ToInt32(GrvIMSItemDetails.DataKeys[e.RowIndex].Value);
               
        TextBox txtNewQuantity = (TextBox)GrvIMSItemDetails.Rows[e.RowIndex].FindControl("txtNewQuantity");
        TextBox txtVendorName = (TextBox)GrvIMSItemDetails.Rows[e.RowIndex].FindControl("txtVendorName");
        TextBox txtBillNO = (TextBox)GrvIMSItemDetails.Rows[e.RowIndex].FindControl("txtBillNO");
        TextBox txtbilldate = (TextBox)GrvIMSItemDetails.Rows[e.RowIndex].FindControl("txtBillDate");
        TextBox txtbillAmount = (TextBox)GrvIMSItemDetails.Rows[e.RowIndex].FindControl("txtBillAmount");
       
        int NewQuantity = Convert.ToInt32(txtNewQuantity.Text.Trim());

        string VendorName = txtVendorName.Text.Trim();

        string billno = txtBillNO.Text.Trim();

        DateTime BillDate = Commonfunctions.CDateTime(txtbilldate.Text.Trim());

        string BillAmount = txtbillAmount.Text.Trim();

        if (NewQuantity != 0)
        {
            int result = InventoryOperation.UpdateIMSQuantity(ItemID, NewQuantity, VendorName, billno, BillDate, BillAmount, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblerror.Text = txtNewQuantity.Text.Trim() + " " + "Quantity Updated Successfully.";
                lblerror.CssClass = "success";

                GrvIMSItemDetails.EditIndex = -1;

                BindItemDetail();
            }
            else if (result == 1)
            {
                lblerror.Text = "System Error! Please Try After Sometime.";
                lblerror.CssClass = "errorMsg";
                e.Cancel = true;
            }
        }
        else
        {
            lblerror.Text = "Please Enter Valid Quantity!";
            lblerror.CssClass = "errorMsg";
            e.Cancel = true;
        }

    } 
      
    protected void GrvIMSItemDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {        
        GrvIMSItemDetails.EditIndex = -1;
        BindItemDetail();
    }
    
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItemDetail();
    }

    private void BindItemDetail()
    {
        DataSet dsItemDetail = InventoryOperation.GetItemByCategoryId(Convert.ToInt16(ddlCategory.SelectedValue));
        
        GrvIMSItemDetails.DataSource = dsItemDetail;
        GrvIMSItemDetails.DataBind();
    }

    protected void GrvIMSItemDetails_PreRender(object sender, EventArgs e)
    {
        BindItemDetail();
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }

        
    }
    protected void GrvIMSItemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrvIMSItemDetails.PageIndex = e.NewPageIndex;
        BindItemDetail();
    }
}