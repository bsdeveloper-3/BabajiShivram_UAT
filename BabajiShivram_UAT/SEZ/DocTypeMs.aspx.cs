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

public partial class SEZ_DocTypeMs : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Document Setup";
            JobDocument();
        }
    }


    protected void gvJobDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox LID = gvJobDocument.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox txtJobDocument = gvJobDocument.FooterRow.FindControl("txtJobDocumentfooter") as TextBox;

            if (txtJobDocument.Text.Trim() != "")
            {
                int result = SEZOperation.AddJobDocument(txtJobDocument.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Job Document Type Name Added Successfully.";
                    lblresult.CssClass = "success";

                    JobDocument();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Document Type Name Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }
            else
            {
                lblresult.Text = " Please Enter Document Type!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvJobDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Lid = Convert.ToInt32(gvJobDocument.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtJobDocument = (TextBox)gvJobDocument.Rows[e.RowIndex].FindControl("txtJobDocument");

        if (txtJobDocument.Text.Trim() != "")
        {
            int result = SEZOperation.UpdateJobDocument(Lid, txtJobDocument.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Job Type Name Updated Successfully.";

                lblresult.CssClass = "success";

                gvJobDocument.EditIndex = -1;

                JobDocument();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Document Type Name Already Exist!";
                lblresult.CssClass = "errorMsg";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Document Type!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvJobDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvJobDocument.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = SEZOperation.DeleteJobDocument(lid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Job Document Deleted Duccessfully.";
            lblresult.CssClass = "success";

            JobDocument();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void JobDocument()
    {
        DataSet ds = new DataSet();

        ds = SEZOperation.GetJobDocumentMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvJobDocument.DataSource = ds;
            gvJobDocument.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvJobDocument.DataSource = ds;
            gvJobDocument.DataBind();
            int columncount = gvJobDocument.Rows[0].Cells.Count;
            gvJobDocument.Rows[0].Cells.Clear();
            gvJobDocument.Rows[0].Cells.Add(new TableCell());
            gvJobDocument.Rows[0].Cells[0].ColumnSpan = columncount;
            gvJobDocument.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvJobDocument_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvJobDocument.EditIndex = e.NewEditIndex;
        JobDocument();
        lblresult.Text = "";

    }

    protected void gvJobDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvJobDocument.EditIndex = -1;

        JobDocument();
        lblresult.Text = "";

    }

    protected void gvJobDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvJobDocument.PageIndex = e.NewPageIndex;

        gvJobDocument.DataBind();
        JobDocument();
    }

}