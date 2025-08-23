using System;
using System.Web.UI.WebControls;

public partial class Inventory_IMSItemType : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Inventory Category";
            GrvIMSCategory.DataBind();
        }
    }
    protected void btnAddCategory_Click(object sender, EventArgs e)
    {
        string ItemCategory = txtCategory.Text.Trim();


        int Result = InventoryOperation.AddImsCategory(ItemCategory,LoggedInUser.glUserId);


         if (Result == 0)
            {
                lblerror.Text = "System Error! Please Try After Sometime.";
                lblerror.CssClass = "errorMsg";
               // txtConsigneeName.Text = String.Empty;
               // hdnConsigneeId.Value = "0";
               
            }
            else if (Result == 1)
            {
                lblerror.Text = "Category Added sucessfully.";
                lblerror.CssClass = "sucsses";
                GrvIMSCategory.DataBind();
            }
            else if (Result == 2)
            {
                lblerror.Text = "Category Already Exist!";
                lblerror.CssClass = "info";
            }
        }
        



    protected void DataSorceCategory_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lblerror.Text = "Removed!";
            lblerror.CssClass = "success";
            GrvIMSCategory.DataBind();
        }
        else if (Result == 1)
        {
            lblerror.Text = "System Error! Please try after sometime";
            lblerror.CssClass = "errorMsg";
           // GrvIMSCategory.DataBind();
        }
        else if (Result == 2)
        {
            lblerror.Text = "IMS Not Found!";
            lblerror.CssClass = "errorMsg";
        }
    }
}