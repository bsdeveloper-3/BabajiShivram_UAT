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

public partial class ExpenseRuleAir : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Air - Expense Rule";
        //
        if (!IsPostBack)
        {
            if (gvAirRule.Rows.Count == 0)
            {
                lblError.Text = "No Records Found !";
                lblError.CssClass = "errorMsg";
        
            }
        }
        
    }

    protected void gvAirRule_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvAirRule_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvAirRule.EditIndex = e.NewEditIndex;
    }

    protected void gvAirRule_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string strlId = gvAirRule.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtName = (TextBox)gvAirRule.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtDescription = (TextBox)gvAirRule.Rows[e.RowIndex].FindControl("txtDescription");
            TextBox txtAmount = (TextBox)gvAirRule.Rows[e.RowIndex].FindControl("txtAmount");
            TextBox txtMaxValue = (TextBox)gvAirRule.Rows[e.RowIndex].FindControl("txtMaxValue");

            if (txtName.Text.Trim() != "" && txtDescription.Text.Trim() != "" && txtAmount.Text.Trim() != "")
            {
                SqlDataSourceAirRule.UpdateParameters["lId"].DefaultValue = strlId;
                SqlDataSourceAirRule.UpdateParameters["sName"].DefaultValue = txtName.Text.Trim();
                SqlDataSourceAirRule.UpdateParameters["Description"].DefaultValue = txtDescription.Text.Trim();
                SqlDataSourceAirRule.UpdateParameters["Charges"].DefaultValue = txtAmount.Text.Trim();

                if (txtMaxValue.Text.Trim() == "")
                    SqlDataSourceAirRule.UpdateParameters["MaxPayable"].DefaultValue = "0";
                else
                    SqlDataSourceAirRule.UpdateParameters["MaxPayable"].DefaultValue = txtMaxValue.Text.Trim();
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