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

public partial class SEZ_PlaceOriginMS : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Place Of Origin";
            PlaceOfOrigin();
        }
    }

    protected void gvPlace_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox PID = gvPlace.FooterRow.FindControl("txtPidfooter") as TextBox;
            TextBox txtPName = gvPlace.FooterRow.FindControl("txtPNamefooter") as TextBox;

            if (txtPName.Text.Trim() != "")
            {
                int result = SEZOperation.AddPlaceOrigin(txtPName.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Place Of Origin Added Successfully.";
                    lblresult.CssClass = "success";

                    PlaceOfOrigin();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Place Of Origin Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }
            else
            {
                lblresult.Text = " Please Enter Place Of Origin!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvPlace_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Pid = Convert.ToInt32(gvPlace.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtPName = (TextBox)gvPlace.Rows[e.RowIndex].FindControl("txtPName");

        if (txtPName.Text.Trim() != "")
        {
            int result = SEZOperation.UpdatePlaceOrigin(Pid, txtPName.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Place Of Origin Updated Successfully.";

                lblresult.CssClass = "success";

                gvPlace.EditIndex = -1;

                PlaceOfOrigin();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Place Of Origin Already Exist!";
                lblresult.CssClass = "errorMsg";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Place Of Origin!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvPlace_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int Pid = Convert.ToInt32(gvPlace.DataKeys[e.RowIndex].Values["Pid"].ToString());

        int result = SEZOperation.DeletePlaceOrigin(Pid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Place Of Origin Deleted Duccessfully.";
            lblresult.CssClass = "success";

            PlaceOfOrigin();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void PlaceOfOrigin()
    {
        DataSet ds = new DataSet();

        ds = SEZOperation.GetPlaceOriginMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPlace.DataSource = ds;
            gvPlace.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvPlace.DataSource = ds;
            gvPlace.DataBind();
            int columncount = gvPlace.Rows[0].Cells.Count;
            gvPlace.Rows[0].Cells.Clear();
            gvPlace.Rows[0].Cells.Add(new TableCell());
            gvPlace.Rows[0].Cells[0].ColumnSpan = columncount;
            gvPlace.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvPlace_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvPlace.EditIndex = e.NewEditIndex;
        PlaceOfOrigin();
        lblresult.Text = "";

    }

    protected void gvPlace_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvPlace.EditIndex = -1;

        PlaceOfOrigin();
        lblresult.Text = "";

    }

    protected void gvPlace_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPlace.PageIndex = e.NewPageIndex;

        gvPlace.DataBind();
        PlaceOfOrigin();
    }

}