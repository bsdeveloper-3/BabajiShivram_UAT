using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class ContMovement_AddTableHeader : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Letter Tables";

        ScriptManager1.RegisterPostBackControl(gvHeader);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["FieldId"]) != null)
            {
                GetData(Convert.ToInt32(Session["FieldId"]));
            }
        }
    }

    protected void GetData(int FieldId)
    {
        if (FieldId > 0)
        {
            DataSet dsGetShipping = CMOperations.GetLetterFields_Lid(FieldId);
            if (dsGetShipping != null && dsGetShipping.Tables[0].Rows.Count > 0)
            {
                if (dsGetShipping.Tables[0].Rows[0]["lid"].ToString() != "")
                {
                    lblFieldName.Text = dsGetShipping.Tables[0].Rows[0]["FieldName"].ToString();
                    lblShippingName.Text = dsGetShipping.Tables[0].Rows[0]["ShippingName"].ToString();
                    lblLetterName.Text = dsGetShipping.Tables[0].Rows[0]["LetterName"].ToString();
                    gvHeader.DataBind();
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strBSColumn = "";
        int FieldId = Convert.ToInt32(Session["FieldId"]);
        if (FieldId > 0)
        {
            if (ddlBSColumn.SelectedValue != "0")
                strBSColumn = ddlBSColumn.SelectedItem.Text.Trim();
            int Result = CMOperations.AddLetterTable(FieldId, txtTableHeader.Text.Trim(), strBSColumn, Convert.ToInt32(ddlDataType.SelectedValue), loggedInUser.glUserId);
            if (Result == 0)
            {
                ResetControls();
                lblError.Text = "Successfully added table column.";
                lblError.CssClass = "success";
            }
            else if (Result == 2)
            {
                lblError.Text = "Column already exists for table!!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up table column.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("GetLetterFields.aspx");
    }

    protected void ResetControls()
    {
        txtTableHeader.Text = "";
        ddlDataType.DataBind();
        ddlBSColumn.DataBind();
        lblError.Text = "";
        gvHeader.DataBind();
    }

    protected void gvHeader_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "delete")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            int Result = CMOperations.DeleteShipperTableHeader(lid, loggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Successfully deleted header.";
                lblError.CssClass = "success";
                gvHeader.DataBind();
            }
            else if (Result == 2)
            {
                lblError.Text = "Field does not exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up field name.";
                lblError.CssClass = "errorMsg";
            }
        }
    }
}