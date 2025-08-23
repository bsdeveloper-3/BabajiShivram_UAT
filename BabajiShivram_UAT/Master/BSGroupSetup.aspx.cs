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
using System.Drawing;

public partial class BSGroupSetup : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "BS Group Setup";
            DivisionDetails();
        }
    }
    
    protected void gvDivision_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox LID = gvDivision.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox DIV_NAME = gvDivision.FooterRow.FindControl("txtDivisionNamefooter") as TextBox;
            TextBox DIV_CODE = gvDivision.FooterRow.FindControl("txtDivisionCodefooter") as TextBox;

            TextBox SREMARK = gvDivision.FooterRow.FindControl("txtsRemarksfooter") as TextBox;
            
            if (DIV_NAME.Text.Trim() != "" && DIV_CODE.Text.Trim() != "")
            {
                int result = DBOperations.AddDivision(DIV_NAME.Text.Trim(), DIV_CODE.Text.Trim(), SREMARK.Text.Trim(), LoggedInUser.glUserId);
                
                if (result == 0)
                {
                    lblresult.Text = "BS Group Added Successfully.";
                    lblresult.CssClass = "success";
                    
                    DivisionDetails();

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "BS Group Name Already Exist!";
                    lblresult.CssClass = "warning";
                }

            }
            else
            {
                lblresult.Text = " Please fill all the details!";
                lblresult.CssClass = "warning";
            }
        }
    }
    
    protected void gvDivision_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Lid = Convert.ToInt32(gvDivision.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtDivNamefooter = (TextBox)gvDivision.Rows[e.RowIndex].FindControl("txtDivisionName");
        TextBox txtDivCodefooter = (TextBox)gvDivision.Rows[e.RowIndex].FindControl("txtDivisionCode");

        TextBox txtsRemarksfooter = (TextBox)gvDivision.Rows[e.RowIndex].FindControl("txtsRemarks");

        if (txtDivNamefooter.Text.Trim() != "" && txtDivCodefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateDivision(Lid, txtDivNamefooter.Text.Trim(), txtDivCodefooter.Text.Trim(), txtsRemarksfooter.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "BS Group Details Updated Successfully.";
                
                lblresult.CssClass = "success";

                gvDivision.EditIndex = -1;

                DivisionDetails();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "BS Group Name Already Exist!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.Text = " Please fill all the details!";
            lblresult.CssClass = "warning";
        }

    }

    protected void gvDivision_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvDivision.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteDivision(lid,LoggedInUser.glUserId);
        
        if (result == 0)
        {
            lblresult.Text = "Details Deleted Successfully.";
            lblresult.CssClass = "success";

            DivisionDetails();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void DivisionDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetDivisionDetails();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDivision.DataSource = ds;
            gvDivision.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvDivision.DataSource = ds;
            gvDivision.DataBind();
            int columncount = gvDivision.Rows[0].Cells.Count;
            gvDivision.Rows[0].Cells.Clear();
            gvDivision.Rows[0].Cells.Add(new TableCell());
            gvDivision.Rows[0].Cells[0].ColumnSpan = columncount;
            gvDivision.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }
    
    protected void gvDivision_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvDivision.EditIndex = e.NewEditIndex;
        DivisionDetails();
        lblresult.Text = "";
      
    }

    protected void gvDivision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvDivision.EditIndex = -1;

        DivisionDetails();
        lblresult.Text = "";
       
    }
    
    protected void gvDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDivision.PageIndex = e.NewPageIndex;

        gvDivision.DataBind();
        DivisionDetails();
    }
}
