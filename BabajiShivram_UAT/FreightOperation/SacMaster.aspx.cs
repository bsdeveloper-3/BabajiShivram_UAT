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

public partial class FreightOperation_SacMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "HSN/SAC Master";

            lblresult.Visible = false;
            GetHSNSacDetails();
        }
    }

    protected void GetHSNSacDetails()
    {
        DataSet ds = new DataSet();
        ds = DBOperations.GetHSNSacDetails();

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv = ds.Tables[0].AsDataView();
            gvSacDetail.DataSource = dv;
            gvSacDetail.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvSacDetail.DataSource = ds;
            gvSacDetail.DataBind();
            int columncount = gvSacDetail.Rows[0].Cells.Count;
            gvSacDetail.Rows[0].Cells.Clear();
            gvSacDetail.Rows[0].Cells.Add(new TableCell());
            gvSacDetail.Rows[0].Cells[0].ColumnSpan = columncount;
            gvSacDetail.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvSacDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName.ToLower() == "insert")
        {
            TextBox txtSACFooter = gvSacDetail.FooterRow.FindControl("txtSACFooter") as TextBox;
            TextBox txtCGSTFooter = gvSacDetail.FooterRow.FindControl("txtCGSTFooter") as TextBox;
            TextBox txtSGSTFooter = gvSacDetail.FooterRow.FindControl("txtSGSTFooter") as TextBox;
            TextBox txtIGSTFooter = gvSacDetail.FooterRow.FindControl("txtIGSTFooter") as TextBox;
            decimal SGST = 0, CGST = 0, IGST = 0;

            string SACCode = txtSACFooter.Text.Trim();
            //if (txtCGSTFooter.Text.Trim() != "")
            //    CGST = Convert.ToDecimal(txtCGSTFooter.Text.Trim());
            //if (txtSGSTFooter.Text.Trim() != "")
            //    SGST = Convert.ToDecimal(txtSGSTFooter.Text.Trim());
            //if (txtIGSTFooter.Text.Trim() != "")
            //    IGST = Convert.ToDecimal(txtIGSTFooter.Text.Trim());

            if (SACCode != "")
            {
                int result = DBOperations.AddHSNSacDetail(SACCode, CGST, SGST, IGST, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = SACCode + " added successfully.";
                    lblresult.CssClass = "success";
                    GetHSNSacDetails();
                }
                else if (result == -1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == -2)
                {
                    lblresult.Text = "HSN/SAC Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvSacDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        decimal SGST = 0, CGST = 0, IGST = 0;

        int lid = Convert.ToInt32(gvSacDetail.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtSAC = (TextBox)gvSacDetail.Rows[e.RowIndex].FindControl("txtSAC");
        TextBox txtCGST = (TextBox)gvSacDetail.Rows[e.RowIndex].FindControl("txtCGST");
        TextBox txtSGST = (TextBox)gvSacDetail.Rows[e.RowIndex].FindControl("txtSGST");
        TextBox txtIGST = (TextBox)gvSacDetail.Rows[e.RowIndex].FindControl("txtIGST");

        string SACCode = txtSAC.Text.Trim();
        if (txtCGST.Text.Trim() != "")
            CGST = Convert.ToDecimal(txtCGST.Text.Trim());
        if (txtSGST.Text.Trim() != "")
            SGST = Convert.ToDecimal(txtSGST.Text.Trim());
        if (txtIGST.Text.Trim() != "")
            IGST = Convert.ToDecimal(txtIGST.Text.Trim());

        if (lid != 0)
        {
            int result = DBOperations.UpdateHSNSacDetail(lid, CGST, SGST, IGST, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblresult.CssClass = "success";
                lblresult.Text = SACCode + " Updated Successfully.";
                gvSacDetail.EditIndex = -1;
                GetHSNSacDetails();
            }
            else if (result == -1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == -2)
            {
                lblresult.Text = "HSN/SAC Already Exists..!!";
                lblresult.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }

    protected void gvSacDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvSacDetail.DataKeys[e.RowIndex].Values["lId"].ToString());

        int result = DBOperations.DeleteSACDetail(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblresult.Text = "HSN/SAC Deleted Successfully!";
            lblresult.CssClass = "success";
            GetHSNSacDetails();
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void gvSacDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSacDetail.EditIndex = e.NewEditIndex;
        GetHSNSacDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvSacDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSacDetail.EditIndex = -1;
        GetHSNSacDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvSacDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSacDetail.PageIndex = e.NewPageIndex;
        gvSacDetail.DataBind();
        GetHSNSacDetails();
    }
}