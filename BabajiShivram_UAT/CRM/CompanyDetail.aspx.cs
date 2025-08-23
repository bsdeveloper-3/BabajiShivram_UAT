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
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public partial class CRM_CompanyDetail : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Company Detail";

            if (Session["CompanyId"] == null)
            {
                FormView1.ChangeMode(FormViewMode.Insert);
            }
            else
            {
                //FormView1.DataBind();
            }
        }
    }

    #region COMPANY FORMVIEW EVENT

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataRowView drView = (DataRowView)FormView1.DataItem;
        if (drView != null)
        {
            lblTitle.Text = drView["sName"].ToString();
        }

        TextBox txtCust = (TextBox)FormView1.FindControl("txtCustName");
        if (txtCust != null)
        {
            if (loggedInUser.glUserId == 1)
            {
                txtCust.Enabled = true;
            }
            else
            {
                txtCust.Enabled = false;

            }
        }

        // Validate Form
        Page.Validate("Required");
    }

    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        Response.Redirect("Company.aspx");
    }

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.KeepInInsertMode = true;
            e.ExceptionHandled = true;

            if (e.Exception != null)
            {
                lblError.Text = e.Exception.Message;
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.KeepInEditMode = true;
            e.ExceptionHandled = true;

            if (e.Exception != null)
            {
                lblError.Text = e.Exception.Message;
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;

            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        if (Result == 0)
        {
            lblError.Text = "Company Detail Updated Successfully";
            lblError.CssClass = "success";
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Company Name Does Not Exists.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        if (Result == 0)
        {
            lblError.Text = "Company Deleted Successfully";
            lblError.CssClass = "success";
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Company Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion

}