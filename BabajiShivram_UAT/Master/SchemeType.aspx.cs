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

public partial class SchemeType : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Scheme/Licence Type Setup";

            lblresult.Visible = false;

            SchemeTypeDetails();
        }
    }

    protected void SchemeTypeDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetSchemeTypeMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSchemeType.DataSource = ds;
            gvSchemeType.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvSchemeType.DataSource = ds;
            gvSchemeType.DataBind();
            int columncount = gvSchemeType.Rows[0].Cells.Count;
            gvSchemeType.Rows[0].Cells.Clear();
            gvSchemeType.Rows[0].Cells.Add(new TableCell());
            gvSchemeType.Rows[0].Cells[0].ColumnSpan = columncount;
            gvSchemeType.Rows[0].Cells[0].Text = "No Scheme Type Detail Found!";
        }

    }

    protected void gvSchemeType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            lblresult.Visible = true;

            TextBox txtLid = gvSchemeType.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox txtSchemeType = gvSchemeType.FooterRow.FindControl("txtSchemeTypefooter") as TextBox;

            TextBox txtRemark = gvSchemeType.FooterRow.FindControl("txtsRemarksfooter") as TextBox;
            
            if (txtSchemeType.Text.Trim() != "" && txtRemark.Text.Trim() != "")
            {                
                int result = DBOperations.AddSchemeType(txtSchemeType.Text.Trim(), txtRemark.Text.Trim(),LoggedInUser.glUserId );
                
                if (result == 0)
                {
                    lblresult.Text = " Scheme/Licence Type " + txtSchemeType.Text.Trim() + " Added Successfully!";
                    lblresult.CssClass = "success";

                    SchemeTypeDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime.";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = " Scheme Type " + txtSchemeType.Text.Trim() + " Already Exists";
                    lblresult.CssClass = "warning";
                }
            }
            else
            {
                lblresult.Text = " Please fill all the details!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }
    
    protected void gvSchemeType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;

        int Lid = Convert.ToInt32(gvSchemeType.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtSchemeType = (TextBox)gvSchemeType.Rows[e.RowIndex].FindControl("txtSchemeType");

        TextBox txtsRemarksfooter = (TextBox)gvSchemeType.Rows[e.RowIndex].FindControl("txtsRemarks");
                
        if (txtSchemeType.Text.Trim() != "" && txtsRemarksfooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateSchemeType(Lid, txtSchemeType.Text.Trim(), txtsRemarksfooter.Text.Trim(),LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.Text = " Scheme type " + txtSchemeType.Text.Trim() + " Updated Successfully.";
                lblresult.CssClass = "success";
                
                gvSchemeType.EditIndex = -1;

                SchemeTypeDetails();
            }
            else if( result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if(result == 2)
            {
                lblresult.Text = " Scheme Type " + txtSchemeType.Text.Trim() + " Already Exists!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.Text = " Please fill all the details!";
            lblresult.CssClass = "errorMsg";
        }
    }
    
    protected void gvSchemeType_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int lid = Convert.ToInt32(gvSchemeType.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteSchemeType(lid,LoggedInUser.glUserId);

        lblresult.Visible = true;
        
        if (result == 0)
        {
            lblresult.Text = " Scheme Type Deleted Successfully";
            lblresult.CssClass = "success";
        
            SchemeTypeDetails();
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void gvSchemeType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSchemeType.EditIndex = e.NewEditIndex;
        SchemeTypeDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvSchemeType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSchemeType.EditIndex = -1;

        SchemeTypeDetails();
        lblresult.Visible = false;
        lblresult.Text = "";
    }
    
    protected void gvSchemeType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {       
        gvSchemeType.PageIndex = e.NewPageIndex;
        
        gvSchemeType.DataBind();
        SchemeTypeDetails();
    }
}
