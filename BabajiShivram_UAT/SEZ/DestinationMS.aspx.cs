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

public partial class SEZ_DestinationMS : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Destination";
            Destination();
        }
    }

    protected void gvDestination_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox DID = gvDestination.FooterRow.FindControl("txtDidfooter") as TextBox;
            TextBox txtDName = gvDestination.FooterRow.FindControl("txtDNamefooter") as TextBox;

            if (txtDName.Text.Trim() != "")
            {
                int result = SEZOperation.AddDestination(txtDName.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Destination Added Successfully.";
                    lblresult.CssClass = "success";

                    Destination();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Destination Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }
            else
            {
                lblresult.Text = " Please Enter Destination!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvDestination_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Did = Convert.ToInt32(gvDestination.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtDName = (TextBox)gvDestination.Rows[e.RowIndex].FindControl("txtDName");

        if (txtDName.Text.Trim() != "")
        {
            int result = SEZOperation.UpdateDestination(Did, txtDName.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Destination Updated Successfully.";

                lblresult.CssClass = "success";

                gvDestination.EditIndex = -1;

                Destination();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Destination Already Exist!";
                lblresult.CssClass = "errorMsg";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Job Type!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvDestination_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int Did = Convert.ToInt32(gvDestination.DataKeys[e.RowIndex].Values["Did"].ToString());

        int result = SEZOperation.DeleteDestination(Did, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Destination Deleted Duccessfully.";
            lblresult.CssClass = "success";

            Destination();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void Destination()
    {
        DataSet ds = new DataSet();

        ds = SEZOperation.GetDestinationMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDestination.DataSource = ds;
            gvDestination.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvDestination.DataSource = ds;
            gvDestination.DataBind();
            int columncount = gvDestination.Rows[0].Cells.Count;
            gvDestination.Rows[0].Cells.Clear();
            gvDestination.Rows[0].Cells.Add(new TableCell());
            gvDestination.Rows[0].Cells[0].ColumnSpan = columncount;
            gvDestination.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvDestination_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvDestination.EditIndex = e.NewEditIndex;
        Destination();
        lblresult.Text = "";

    }

    protected void gvDestination_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvDestination.EditIndex = -1;

        Destination();
        lblresult.Text = "";

    }

    protected void gvDestination_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDestination.PageIndex = e.NewPageIndex;

        gvDestination.DataBind();
        Destination();
    }
}