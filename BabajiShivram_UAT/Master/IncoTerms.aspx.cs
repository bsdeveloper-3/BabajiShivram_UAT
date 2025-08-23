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

public partial class IncoTerms : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        lblresult.Visible = false;

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Incoterms of Shipment Setup";
            IncoTermDetails();
        }
    }

    protected void gvInco_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            TextBox LID = gvInco.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox sNAME = gvInco.FooterRow.FindControl("txtsNamefooter") as TextBox;
                        
            if (sNAME.Text.Trim() != "")
            {                
                int result = DBOperations.AddIncoTerms(sNAME.Text.Trim(),LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblresult.Text = "Details added successfully";
                    lblresult.CssClass = "success";
                    IncoTermDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please try after sometime.";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Incoterm already exists!";
                    lblresult.CssClass = "warning";
                }
            }
            else
            {                
                lblresult.Text = " Please Enter Incoterm name!";
                lblresult.CssClass = "errorMsg";
            }

            

        }
    }

    protected void gvInco_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvInco.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtsNamefooter = (TextBox)gvInco.Rows[e.RowIndex].FindControl("txtsName");
                
        if (txtsNamefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateIncoTerms(Lid, txtsNamefooter.Text.Trim(),LoggedInUser.glUserId);
            if (result == 0)
            {
                lblresult.Text = "Details updated successfully";
                lblresult.CssClass = "success";
                gvInco.EditIndex = -1;
                IncoTermDetails();
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please try after sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Incoterm already exists!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Incoterm name!";
            lblresult.CssClass = "errorMsg";
        }

    }

    protected void gvInco_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvInco.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteIncoTerms(lid,LoggedInUser.glUserId);
        if (result == 0)
        {
            lblresult.CssClass = "success";
            lblresult.Text = "Details deleted successfully";
            
            IncoTermDetails();
        }
        else if(result == 1)
        {            
            lblresult.Text = "System Error! Please try after sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void IncoTermDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetIncoTermDetails();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInco.DataSource = ds;
            gvInco.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvInco.DataSource = ds;
            gvInco.DataBind();
            int columncount = gvInco.Rows[0].Cells.Count;
            gvInco.Rows[0].Cells.Clear();
            gvInco.Rows[0].Cells.Add(new TableCell());
            gvInco.Rows[0].Cells[0].ColumnSpan = columncount;
            gvInco.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }
    
    protected void gvInco_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvInco.EditIndex = e.NewEditIndex;
        IncoTermDetails();
        lblresult.Text = "";

    }

    protected void gvInco_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvInco.EditIndex = -1;

        IncoTermDetails();
        lblresult.Text = "";

    }
    protected void gvInco_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvInco.PageIndex = e.NewPageIndex;

        gvInco.DataBind();
        IncoTermDetails();
    }

}
