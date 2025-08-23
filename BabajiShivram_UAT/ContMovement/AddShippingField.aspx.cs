using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ContMovement_AddShippingField : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Add Letter Field";

        if (!IsPostBack)
        {
            ddlShippingMS.Focus();
        }
    }

    protected void ddlShippingMS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlShippingMS.SelectedValue != "0")
        {
            ddlLetters.DataBind();
            ddlLetters.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int lType = 1;
        bool IsTable = false;   // 0 --> Other than table, 1 --> table 
        if (rblFieldType.SelectedValue == "1")
        {
            IsTable = true;
        }

        if (ddlLetters.SelectedValue != "0")
        {
            int Result = CMOperations.AddLetterField(Convert.ToInt32(ddlLetters.SelectedValue), txtFieldName.Text.Trim(), lType, IsTable, loggedInUser.glUserId);
            if (Result > 0)
            {
                ResetControls();
                lblError.Text = "Successfully added field.";
                lblError.CssClass = "success";
            }
            else if (Result == -2)
            {
                lblError.Text = "Field already exists for letter " + ddlLetters.SelectedItem.Text.Trim().ToUpper() + "!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up field name.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Select letter to add field for.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("GetLetterFields.aspx");
    }

    protected void ResetControls()
    {
        ddlShippingMS.Items.Clear();
        ddlShippingMS.DataBind();
        ddlLetters.Items.Clear();
        ddlLetters.DataBind();
        txtFieldName.Text.Trim();
    }
}