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
using System.IO;

public partial class Master_ExpenseRuleSea : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Sea Expense Rule Setup";
        //
        if (!IsPostBack)
        {
            if (gvSeaRule.Rows.Count == 0)
            {
                lblError.Text = "No Records Found !";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvSeaRule_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvSeaRule_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvSeaRule.EditIndex = e.NewEditIndex;
    }

    protected void gvSeaRule_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string strlId       =   gvSeaRule.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtCon20    =   (TextBox)gvSeaRule.Rows[e.RowIndex].FindControl("txtCon20");
            TextBox txtCon40    =   (TextBox)gvSeaRule.Rows[e.RowIndex].FindControl("txtCon40");
            TextBox txtCon45    =   (TextBox)gvSeaRule.Rows[e.RowIndex].FindControl("txtCon45");
            TextBox txtLCL      =   (TextBox)gvSeaRule.Rows[e.RowIndex].FindControl("txtLCL");

            if (txtCon20.Text.Trim() != "" && txtCon40.Text.Trim() != "" && txtCon45.Text.Trim() != "" && txtLCL.Text != "")
            {
                SqlDataSourceRule.UpdateParameters["lId"].DefaultValue      = strlId;
                SqlDataSourceRule.UpdateParameters["Con20"].DefaultValue    = txtCon20.Text.Trim();
                SqlDataSourceRule.UpdateParameters["Con40"].DefaultValue    = txtCon40.Text.Trim();
                SqlDataSourceRule.UpdateParameters["Con45"].DefaultValue    = txtCon45.Text.Trim();
                SqlDataSourceRule.UpdateParameters["LCL"].DefaultValue  =  txtLCL.Text.Trim();


            }
            else
            {
                e.Cancel = true;
                lblError.Text = "Please Enter Required Field!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }

    }

    #region DataSourceEvents
    protected void SqlDataSourceRule_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = false;

        if (e.Exception != null)
        {
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

            if (Result == 0)
            {
                lblError.Text = "Expense Rule Detail Updated Successfully !";
                lblError.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    #endregion
    
}