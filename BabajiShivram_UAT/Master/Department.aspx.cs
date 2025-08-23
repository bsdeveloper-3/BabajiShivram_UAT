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
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Drawing;


public partial class Department : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Department Setup";

            lblresult.Visible = false;

            departmentDetails();
        }
    }

    protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox LID = gvDepartment.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox DEPT_NAME = gvDepartment.FooterRow.FindControl("txtDepartment_Namefooter") as TextBox;
            TextBox DEPT_CODE = gvDepartment.FooterRow.FindControl("txtDepartment_Codefooter") as TextBox;
            TextBox SREMARK = gvDepartment.FooterRow.FindControl("txtRemarkfooter") as TextBox;
                      
            if (DEPT_NAME.Text.Trim() != "" && DEPT_CODE.Text.Trim() != "")
            {                
                int result = DBOperations.AddDepartment(DEPT_NAME.Text.Trim(), DEPT_CODE.Text.Trim(), SREMARK.Text.Trim(),LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = DEPT_NAME.Text.Trim() + " Department added successfully.";
                    lblresult.CssClass = "success";

                    departmentDetails();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Department Name Already Exist!";
                    lblresult.CssClass = "warning";
                }

            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvDepartment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvDepartment.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtDepartment_Namefooter = (TextBox)gvDepartment.Rows[e.RowIndex].FindControl("txtDepartment_Name");
        TextBox txtDepartment_Codefooter = (TextBox)gvDepartment.Rows[e.RowIndex].FindControl("txtDepartment_Code");
        TextBox txtRemarkfooter = (TextBox)gvDepartment.Rows[e.RowIndex].FindControl("txtRemark");
                        
        if (txtDepartment_Namefooter.Text.Trim() != "" && txtDepartment_Codefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateDepartment(Lid, txtDepartment_Namefooter.Text.Trim(), txtDepartment_Codefooter.Text.Trim(), txtRemarkfooter.Text.Trim(),LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.CssClass = "success";

                lblresult.Text = txtDepartment_Namefooter.Text.Trim() + " Department Details Updated Successfully.";

                gvDepartment.EditIndex = -1;

                departmentDetails();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Department Name Already Added!";
                lblresult.CssClass = "warning";
            }
        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }

    protected void gvDepartment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvDepartment.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteDepartmnet(lid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Department Deleted Successfully!";
            lblresult.CssClass = "success";

            departmentDetails();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void departmentDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetDepartmentDetails();
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDepartment.DataSource = ds;
            gvDepartment.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvDepartment.DataSource = ds;
            gvDepartment.DataBind();
            int columncount = gvDepartment.Rows[0].Cells.Count;
            gvDepartment.Rows[0].Cells.Clear();
            gvDepartment.Rows[0].Cells.Add(new TableCell());
            gvDepartment.Rows[0].Cells[0].ColumnSpan = columncount;
            gvDepartment.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }
    
    protected void gvDepartment_RowEditing(object sender, GridViewEditEventArgs e)
    {

        gvDepartment.EditIndex = e.NewEditIndex;
        departmentDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvDepartment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDepartment.EditIndex = -1;

        departmentDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDepartment.PageIndex = e.NewPageIndex;

        gvDepartment.DataBind();
        departmentDetails();
    }

}


