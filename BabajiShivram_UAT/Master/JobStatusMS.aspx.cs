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

public partial class Master_JobStatusMS : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Status Setup";
            JobStatus();
        }
    }

    protected void gvJobStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox LID = gvJobStatus.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox txtJobStatus = gvJobStatus.FooterRow.FindControl("txtJobStatusfooter") as TextBox;

            if (txtJobStatus.Text.Trim() != "")
            {
                int result = DBOperations.AddJobStatus(txtJobStatus.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Job Type Name Added Successfully.";
                    lblresult.CssClass = "success";

                    JobStatus();

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

    protected void gvJobStatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Lid = Convert.ToInt32(gvJobStatus.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtJobStatus = (TextBox)gvJobStatus.Rows[e.RowIndex].FindControl("txtJobStatus");

        if (txtJobStatus.Text.Trim() != "")
        {
            int result = DBOperations.UpdateJobStatus(Lid, txtJobStatus.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Job Type Name Updated Successfully.";

                lblresult.CssClass = "success";

                gvJobStatus.EditIndex = -1;

                JobStatus();

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

    protected void gvJobStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvJobStatus.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteJobStatus(lid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Job Status Deleted Duccessfully.";
            lblresult.CssClass = "success";

            JobStatus();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void JobStatus()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetJobStatusMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvJobStatus.DataSource = ds;
            gvJobStatus.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvJobStatus.DataSource = ds;
            gvJobStatus.DataBind();
            int columncount = gvJobStatus.Rows[0].Cells.Count;
            gvJobStatus.Rows[0].Cells.Clear();
            gvJobStatus.Rows[0].Cells.Add(new TableCell());
            gvJobStatus.Rows[0].Cells[0].ColumnSpan = columncount;
            gvJobStatus.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvJobStatus_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvJobStatus.EditIndex = e.NewEditIndex;
        JobStatus();
        lblresult.Text = "";

    }

    protected void gvJobStatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvJobStatus.EditIndex = -1;

        JobStatus();
        lblresult.Text = "";

    }

    protected void gvJobStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvJobStatus.PageIndex = e.NewPageIndex;

        gvJobStatus.DataBind();
        JobStatus();
    }
}
