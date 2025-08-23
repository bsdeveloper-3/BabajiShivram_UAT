using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
public partial class Inventory_DispatchedItem : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkIMSXls);
        calDispDate.EndDate = DateTime.Now;

        if (!IsPostBack)
        {
            InventoryOperation.FillIMSItemTypeMS(ddlCategory);
            InventoryOperation.FillItemsIms(ddlItem);
            InventoryOperation.FillBranchIMS(ddlBranch);
            InventoryOperation.FillDeptNameIms(ddldept);

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Inventory Dispatch";
        }

        DataFilter1.DataSource = DataSorceDispatchedDetails;
        DataFilter1.DataColumns = GrvDispatched.Columns;
        DataFilter1.FilterSessionID = "DispatchedItem.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void btnSubmitDispached_Click(object sender, EventArgs e)
    {
        string employeeName = txtempName.Text.Trim();

        int Quantity    = Convert.ToInt32(txtQuantity.Text.Trim());
        int Branch      = Convert.ToInt32(ddlBranch.SelectedValue);
        int Department  = Convert.ToInt32(ddldept.SelectedValue);
        int Item        = Convert.ToInt32(ddlItem.SelectedValue);

        DateTime dtDispatchDate = DateTime.Now;

        if(txtDispatchDate.Text.Trim() != "")
        {
            dtDispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
        }

        if (Quantity > 0 && Item > 0 && employeeName !="")
        {
            int Result = InventoryOperation.AddImsDispached(employeeName, Quantity, Branch, Department, Item, dtDispatchDate, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "System Error! Please Try After Sometime..";
                lblError.CssClass = "ErrorMsg";
            }
            else if (Result == 1)
            {
                lblremanihg.Text    = "";
                txtQuantity.Text    = "";
                ddlCategory.SelectedValue = "0";
                lblError.Text       = "Dispatch Item Added Successfully";
                lblError.CssClass   = "success";
                GrvDispatched.DataBind();
            }
            else if (Result == 2)
            {
                lblError.Text = "Insufficient Stock Quantity!!";
                lblError.CssClass = "info";
            }
        }
        else
        {
            lblError.Text = "Please Enter Valid Quantity!";
            lblError.CssClass = "info";
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {       
        if (ddlCategory.SelectedIndex > 0)
        { 
            int CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            string remaining = lblremanihg.Text;
            InventoryOperation.FillItemByCategoryId(ddlItem, CategoryId);
        }
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {        
        if (ddlItem.SelectedIndex > 0)
        {
            int ItemId = Convert.ToInt32(ddlItem.SelectedValue);
            DataSet dsgst = InventoryOperation.GetItemById(Convert.ToInt32(ItemId));

            lblremanihg.Text = dsgst.Tables[0].Rows[0]["BalQuntity"].ToString();
        }
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
            DataFilter1.FilterSessionID = "DispatchedItem.aspx";
            DataFilter1.FilterDataSource();
            GrvDispatched.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export To Excel

    protected void lnkIMSXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Inventory_Details_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExcelExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GrvDispatched.Visible = true;
        GrvDispatched.AllowPaging = false;
        GrvDispatched.AllowSorting = false;

        GrvDispatched.DataSourceID = "DataSorceDispatchedDetails";

        DataFilter1.FilterSessionID = "DispatchedItem.aspx";
        DataFilter1.FilterDataSource();

        GrvDispatched.DataBind();
    

        GrvDispatched.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    

    #endregion

}