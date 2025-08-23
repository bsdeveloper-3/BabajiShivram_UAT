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

public partial class RoleMaster : System.Web.UI.Page
{
    LoginClass logobj = new LoginClass();
    private string _callbackResult = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //
            Session["AccessRoleId"] = null;

            lberror.Visible = false;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Roles And Access Rights";
            
        }
        //END_IF
    }

    protected void btnNewButton_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.Insert);
        fsMainBorder.Visible = false;
    }

    #region GridView Event

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lberror.Visible = false;
        
        if (e.CommandName.ToLower() == "roledetail")
        {
            string[] strArgument = e.CommandArgument.ToString().Split(',');

            string RoleId = strArgument[0].ToString();

            Session["AccessRoleId"] = RoleId;
            string RoleName = strArgument[1].ToString();

           Response.Redirect("RoleDetail.aspx?Name="+RoleName);
        }
        else if (e.CommandName.ToLower() == "select")
        {
            GridView1.Visible = false;
            fsMainBorder.Visible = false;
        }
    }

    #endregion

    #region FormView Event

    protected void FormView1_DataBound(Object sender, EventArgs e)
    {
        Page.Validate("Required");
    }

    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            GridView1.Visible = true;
            GridView1.SelectedIndex = -1;
            fsMainBorder.Visible = true;
        }
        else
        {
            GridView1.Visible = false;
            fsMainBorder.Visible = false;
        }
    }

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }
    
    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }
    }
        
    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
        }
    }

    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;
                
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Access Role Created Successfully.";
            lberror.CssClass = "success";

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            GridView1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {

            lberror.Text = "Access Role Name Already Exist!";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Access Role Updated Successfully.";
            lberror.CssClass = "success";

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            GridView1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime.";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "Access Role Name Already Exist!";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Access Role Deleted Successfully.";
            lberror.CssClass = "success";

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            GridView1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime.";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "Access Role Not Found!";
            lberror.CssClass = "errorMsg";
        }
    }
#endregion
}
