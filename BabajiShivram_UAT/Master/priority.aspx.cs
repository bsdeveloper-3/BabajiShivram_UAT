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
using System.Text;
using System.Data.SqlClient;

public partial class Master_priority : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (grvPriority.Rows.Count == 0)
            {
                lblMessage.Text = "No Data Found For Billing Priority!";
                lblMessage.CssClass = "errorMsg"; ;
            }
        }
    }

    protected void grvPriority_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string strlId = grvPriority.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtPriorityID = (TextBox)grvPriority.Rows[e.RowIndex].FindControl("txtPriorityID");

            if (strlId.Trim() != "" && txtPriorityID.Text.Trim() != "")
            {
             datasourcePriority.UpdateParameters["lId"].DefaultValue = strlId;
             datasourcePriority.UpdateParameters["lOrder"].DefaultValue = txtPriorityID.Text.Trim();
            }
            else
            {
                e.Cancel = true;
                lblMessage.Text = "Please Enter Required Field!";
                lblMessage.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.CssClass = "errorMsg";
        }

    }

    protected void grvPriority_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        grvPriority.EditIndex = e.NewEditIndex;
    }

    protected void datasourcePriority_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = false;

        if (e.Exception != null)
        {
            lblMessage.Text = e.Exception.Message;
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

            if (Result == 0)
            {
                lblMessage.Text = "Detail Updated Successfully !";
                lblMessage.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }
}