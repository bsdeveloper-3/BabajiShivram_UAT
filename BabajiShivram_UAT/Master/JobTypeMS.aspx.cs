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

public partial class JobTypeMS : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Type Setup";
            JobType();
        }
    }

    protected void gvJobType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox LID = gvJobType.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox txtJobType = gvJobType.FooterRow.FindControl("txtJobTypefooter") as TextBox;

            if (txtJobType.Text.Trim() != "")
            {
                int result = DBOperations.AddJobType(txtJobType.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Job Type Name Added Successfully.";
                    lblresult.CssClass = "success";

                    JobType();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Job Type Name Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }
            else
            {
                lblresult.Text = " Please Enter Job Type!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvJobType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Lid = Convert.ToInt32(gvJobType.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtJobType = (TextBox)gvJobType.Rows[e.RowIndex].FindControl("txtJobType");

        if (txtJobType.Text.Trim() != "" )
        {
            int result = DBOperations.UpdateJobType(Lid, txtJobType.Text.Trim(),loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Job Type Name Updated Successfully.";

                lblresult.CssClass = "success";

                gvJobType.EditIndex = -1;

                JobType();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Job Type Name Already Exist!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Job Type!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvJobType_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvJobType.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteJobType(lid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Details Deleted Successfully.";
            lblresult.CssClass = "success";

            JobType();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void JobType()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetJobType();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvJobType.DataSource = ds;
            gvJobType.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvJobType.DataSource = ds;
            gvJobType.DataBind();
            int columncount = gvJobType.Rows[0].Cells.Count;
            gvJobType.Rows[0].Cells.Clear();
            gvJobType.Rows[0].Cells.Add(new TableCell());
            gvJobType.Rows[0].Cells[0].ColumnSpan = columncount;
            gvJobType.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvJobType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvJobType.EditIndex = e.NewEditIndex;
        JobType();
        lblresult.Text = "";

    }

    protected void gvJobType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvJobType.EditIndex = -1;

        JobType();
        lblresult.Text = "";

    }

    protected void gvJobType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvJobType.PageIndex = e.NewPageIndex;

        gvJobType.DataBind();
        JobType();
    }
}
