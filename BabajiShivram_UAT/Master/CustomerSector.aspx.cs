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

public partial class CustomerSector : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Sector Setup";

            lblresult.Visible = false;

            CustomerSectorDetails();
        }
    }
    
    protected void CustomerSectorDetails()
    {
        DataSet Ds = new DataSet();
        Ds = DBOperations.GetSectorMS();

        if (Ds.Tables[0].Rows.Count > 0)
        {
            gvCustomerSector.DataSource = Ds;
            gvCustomerSector.DataBind();  
        }
        else
        {
            Ds.Tables[0].Rows.Add(Ds.Tables[0].NewRow());
            gvCustomerSector.DataSource = Ds;
            gvCustomerSector.DataBind();
            int columncount = gvCustomerSector.Rows[0].Cells.Count;
            gvCustomerSector.Rows[0].Cells.Clear();
            gvCustomerSector.Rows[0].Cells.Add(new TableCell());
            gvCustomerSector.Rows[0].Cells[0].ColumnSpan = columncount;
            gvCustomerSector.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }
    
    protected void gvCustomerSector_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCustomerSector.EditIndex = -1;
        CustomerSectorDetails();

        lblresult.Text = "";
        lblresult.Visible = false;  
    }

    protected void gvCustomerSector_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCustomerSector.PageIndex = e.NewPageIndex;
        gvCustomerSector.DataBind();
        CustomerSectorDetails(); 

    }

    protected void gvCustomerSector_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCustomerSector.EditIndex = e.NewEditIndex;
        CustomerSectorDetails();
        lblresult.Text = "";
        lblresult.Visible = false;  
    }

    protected void gvCustomerSector_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvCustomerSector.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = DBOperations.DeleteSectorMS(lid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Sector Deleted Successfully!";
            lblresult.CssClass = "success";
            CustomerSectorDetails ();
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }        
    }

    protected void gvCustomerSector_RowUpdating(object sender, GridViewUpdateEventArgs  e)
    { 

        int Lid=Convert.ToInt32(gvCustomerSector.DataKeys[e.RowIndex].Value.ToString());      
        TextBox txtCustomerSector_Namefooter=(TextBox)gvCustomerSector.Rows[e.RowIndex].FindControl("txtCustomerSector_Name");
        TextBox txtCustomerSector_Codefooter = (TextBox)gvCustomerSector.Rows[e.RowIndex].FindControl("txtCustomerSector_Code");
        TextBox txtRemarkfooter = (TextBox)gvCustomerSector.Rows[e.RowIndex].FindControl("txtRemark");

        if (txtCustomerSector_Namefooter.Text.Trim() != "" && txtCustomerSector_Codefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateSectorMS(Lid, txtCustomerSector_Namefooter.Text.Trim(), txtCustomerSector_Codefooter.Text.Trim(), txtRemarkfooter.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtCustomerSector_Namefooter.Text.Trim() + " Sector Details Updated Successfully.";
                gvCustomerSector.EditIndex = -1;
                CustomerSectorDetails();
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";

            }
            else if (result == 2)
            {
                lblresult.Text = "Sector Name Already Added!";
                lblresult.CssClass = "warning";
            }

        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }
    
    protected void gvCustomerSector_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName.ToLower() == "insert")
        {
            TextBox LID = gvCustomerSector.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox CustVNm=gvCustomerSector.FooterRow.FindControl("txtCustomerSector_Namefooter")  as TextBox ; 
            TextBox CustVCd=gvCustomerSector.FooterRow.FindControl("txtCustomerSector_Codefooter") as TextBox ;
            TextBox cRemark = gvCustomerSector.FooterRow.FindControl("txtRemarkfooter") as TextBox;

            if (CustVNm.Text.Trim() != "" && CustVNm.Text.Trim() != "")
            {
                int result = DBOperations.AddSectorMS(CustVNm.Text.Trim(), CustVCd.Text.Trim(), cRemark.Text.Trim(), LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = CustVNm.Text.Trim() + " Sector added successfully.";
                    lblresult.CssClass = "success";
                    CustomerSectorDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Sector Name Already Exist!";
                    lblresult.CssClass = "warning";
                }
            }
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }   
}
