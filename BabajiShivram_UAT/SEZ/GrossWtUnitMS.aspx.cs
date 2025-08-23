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


public partial class SEZ_GrossWtUnitMS : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Gross Weight Unit";
            GrossWtUnit();
        }
    }

    protected void gvGrossWtUnit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox GID = gvGrossWtUnit.FooterRow.FindControl("txtGidfooter") as TextBox;
            TextBox txtUName = gvGrossWtUnit.FooterRow.FindControl("txtUNamefooter") as TextBox;

            if (txtUName.Text.Trim() != "")
            {
                int result = SEZOperation.AddGrossWtUnit(txtUName.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Gross Weight Unit Added Successfully.";
                    lblresult.CssClass = "success";

                    GrossWtUnit();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Gross Weight Unit Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }
            else
            {
                lblresult.Text = " Please Enter Gross Weight Unit!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvGrossWtUnit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Gid = Convert.ToInt32(gvGrossWtUnit.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtUnitName = (TextBox)gvGrossWtUnit.Rows[e.RowIndex].FindControl("txtUName");

        if (txtUnitName.Text.Trim() != "")
        {
            int result = SEZOperation.UpdateGrossWtUnit(Gid, txtUnitName.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Gross Weight Unit Updated Successfully.";

                lblresult.CssClass = "success";

                gvGrossWtUnit.EditIndex = -1;

                GrossWtUnit();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Gross Weight Unit Already Exist!";
                lblresult.CssClass = "errorMsg";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Gross Weight Unit!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvGrossWtUnit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int Gid = Convert.ToInt32(gvGrossWtUnit.DataKeys[e.RowIndex].Values["Gid"].ToString());

        int result = SEZOperation.DeleteGrossWtUnit(Gid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Gross Weight Unit Deleted Duccessfully.";
            lblresult.CssClass = "success";

            GrossWtUnit();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void GrossWtUnit()
    {
        DataSet ds = new DataSet();

        ds = SEZOperation.GetGrossWtUnitMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvGrossWtUnit.DataSource = ds;
            gvGrossWtUnit.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvGrossWtUnit.DataSource = ds;
            gvGrossWtUnit.DataBind();
            int columncount = gvGrossWtUnit.Rows[0].Cells.Count;
            gvGrossWtUnit.Rows[0].Cells.Clear();
            gvGrossWtUnit.Rows[0].Cells.Add(new TableCell());
            gvGrossWtUnit.Rows[0].Cells[0].ColumnSpan = columncount;
            gvGrossWtUnit.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvGrossWtUnit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvGrossWtUnit.EditIndex = e.NewEditIndex;
        GrossWtUnit();
        lblresult.Text = "";

    }

    protected void gvGrossWtUnit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvGrossWtUnit.EditIndex = -1;

        GrossWtUnit();
        lblresult.Text = "";

    }

    protected void gvGrossWtUnit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGrossWtUnit.PageIndex = e.NewPageIndex;

        gvGrossWtUnit.DataBind();
        GrossWtUnit();
    }

}