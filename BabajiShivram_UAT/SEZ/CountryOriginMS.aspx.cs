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

public partial class SEZ_CountryOriginMS : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Country Of Origin";
            CountryOfOrigin();
        }
    }

    protected void gvCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox CID = gvCountry.FooterRow.FindControl("txtCidfooter") as TextBox;
            TextBox txtCName = gvCountry.FooterRow.FindControl("txtCNamefooter") as TextBox;

            if (txtCName.Text.Trim() != "")
            {
                int result = SEZOperation.AddCountryOrigin(txtCName.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Country Of Origin Added Successfully.";
                    lblresult.CssClass = "success";

                    CountryOfOrigin();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Country Of Origin Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }
            else
            {
                lblresult.Text = " Please Enter Country Of Origin!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Cid = Convert.ToInt32(gvCountry.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtCName = (TextBox)gvCountry.Rows[e.RowIndex].FindControl("txtCName");

        if (txtCName.Text.Trim() != "")
        {
            int result = SEZOperation.UpdateCountryOrigin(Cid, txtCName.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Country Of Origin Updated Successfully.";

                lblresult.CssClass = "success";

                gvCountry.EditIndex = -1;

                CountryOfOrigin();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Country Of Origin Already Exist!";
                lblresult.CssClass = "errorMsg";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Country Of Origin!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int Cid = Convert.ToInt32(gvCountry.DataKeys[e.RowIndex].Values["Cid"].ToString());

        int result = SEZOperation.DeleteCountryOrigin(Cid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Country Of Origin Deleted Duccessfully.";
            lblresult.CssClass = "success";

            CountryOfOrigin();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void CountryOfOrigin()
    {
        DataSet ds = new DataSet();

        ds = SEZOperation.GetCountryOriginMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCountry.DataSource = ds;
            gvCountry.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvCountry.DataSource = ds;
            gvCountry.DataBind();
            int columncount = gvCountry.Rows[0].Cells.Count;
            gvCountry.Rows[0].Cells.Clear();
            gvCountry.Rows[0].Cells.Add(new TableCell());
            gvCountry.Rows[0].Cells[0].ColumnSpan = columncount;
            gvCountry.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvCountry_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvCountry.EditIndex = e.NewEditIndex;
        CountryOfOrigin();
        lblresult.Text = "";

    }

    protected void gvCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvCountry.EditIndex = -1;

        CountryOfOrigin();
        lblresult.Text = "";

    }

    protected void gvCountry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCountry.PageIndex = e.NewPageIndex;

        gvCountry.DataBind();
        CountryOfOrigin();
    }
}