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

public partial class FieldMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Additional Field Setup";

            lblresult.Visible = false;

            FieldDetails();
        }
    }

    protected void FieldDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetFieldDetails();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvField.DataSource = ds;
            gvField.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvField.DataSource = ds;
            gvField.DataBind();
            int columncount = gvField.Rows[0].Cells.Count;
            gvField.Rows[0].Cells.Clear();
            gvField.Rows[0].Cells.Add(new TableCell());
            gvField.Rows[0].Cells[0].ColumnSpan = columncount;
            gvField.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvField_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox txtFieldId       = gvField.FooterRow.FindControl("txtFieldIdFooter") as TextBox;
            TextBox txtFieldName     = gvField.FooterRow.FindControl("txtField_Namefooter") as TextBox;
            DropDownList ddFieldType = gvField.FooterRow.FindControl("ddFieldTypeFooter") as DropDownList;
            DropDownList ddModule = gvField.FooterRow.FindControl("ddModuleFooter") as DropDownList;

            if (txtFieldName.Text.Trim() != "" && ddFieldType.SelectedValue != "0")
            {
                int result = DBOperations.AddFieldMaster(txtFieldName.Text.Trim(), Convert.ToInt32(ddFieldType.SelectedValue), Convert.ToInt32(ddModule.SelectedValue), LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblresult.Text = txtFieldName.Text.Trim() + " Field added successfully.";
                    lblresult.CssClass = "success";

                    FieldDetails();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Field Name Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvField_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int FieldId = Convert.ToInt32(gvField.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtField_Name = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtField_Name");
        DropDownList ddFieldType = (DropDownList)gvField.Rows[e.RowIndex].FindControl("ddFieldType");
        DropDownList ddModule = (DropDownList)gvField.Rows[e.RowIndex].FindControl("ddModule");

        if (txtField_Name.Text.Trim() != "" && ddFieldType.SelectedValue != "0")
        {
            int result = DBOperations.UpdateFieldMaster(FieldId, txtField_Name.Text.Trim(), Convert.ToInt32(ddFieldType.SelectedValue),
                                                                      Convert.ToInt32(ddModule.SelectedValue), LoggedInUser.glUserId);
            if (result == 0)
            {
                lblresult.CssClass = "success";

                lblresult.Text = txtField_Name.Text.Trim() + " Field Details Updated Successfully.";

                gvField.EditIndex = -1;

                FieldDetails();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Field Name Already Added!";
                lblresult.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }

    protected void gvField_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int FieldId = Convert.ToInt32(gvField.DataKeys[e.RowIndex].Values["FieldId"].ToString());

        int result = DBOperations.DeleteFieldMaster(FieldId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Field Deleted Successfully!";
            lblresult.CssClass = "success";

            FieldDetails();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void gvField_RowEditing(object sender, GridViewEditEventArgs e)
    {

        gvField.EditIndex = e.NewEditIndex;
        FieldDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvField_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvField.EditIndex = -1;

        FieldDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvField_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvField.PageIndex = e.NewPageIndex;

        gvField.DataBind();
        FieldDetails();
    }
}
