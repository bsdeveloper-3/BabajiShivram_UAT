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
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

public partial class Master_QuoteModeMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvQuoteModeDetails);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Quotation Mode Setup";

            lblresult.Visible = false;
            FillQuoteAppFields();
        }
    }

    protected void gvQuoteModeDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName == "Insert")
        {
            TextBox txtQuotationModeFooter = gvQuoteModeDetails.FooterRow.FindControl("txtQuotationModeFooter") as TextBox;
            if (txtQuotationModeFooter.Text.Trim() != "" && txtQuotationModeFooter.Text.Trim() != "")
            {
                int result = QuotationOperations.AddQuotationModeMS(txtQuotationModeFooter.Text.Trim(), LoggedInUser.glUserId);
                if (result == 1)
                {
                    lblresult.Text = txtQuotationModeFooter.Text.Trim() + " Quotation Mode Added Successfully..!!";
                    lblresult.CssClass = "success";
                    FillQuoteAppFields();
                }
                else if (result == 2)
                {
                    lblresult.Text = "Quotation Mode Already Exist!";
                    lblresult.CssClass = "warning";
                }
                else
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }


            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please Enter Quotation Category Name.";
                txtQuotationModeFooter.Focus();
            }
        }
    }

    protected void gvQuoteModeDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvQuoteModeDetails.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtQuotationMode = (TextBox)gvQuoteModeDetails.Rows[e.RowIndex].FindControl("txtQuotationMode");

        if (txtQuotationMode.Text.Trim() != "" && txtQuotationMode.Text.Trim() != "")
        {
            int result = QuotationOperations.UpdateQuotationModeMS(Lid, txtQuotationMode.Text.Trim(), LoggedInUser.glUserId);
            if (result == 1)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtQuotationMode.Text.Trim() + " Quotation Mode Updated Successfully..!!";
                gvQuoteModeDetails.EditIndex = -1;
                FillQuoteAppFields();
            }
            else if (result == 2)
            {
                lblresult.Text = "Quotation Mode Does Not Exists..!!";
                lblresult.CssClass = "warning";
            }
            else
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }

        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please Enter Quotation Mode.";
        }
    }

    protected void gvQuoteModeDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvQuoteModeDetails.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteQuotationModeMS(lid, LoggedInUser.glUserId);
        if (result == 1)
        {
            lblresult.Text = "Quotation Mode Deleted Successfully!";
            lblresult.CssClass = "success";
            FillQuoteAppFields();
        }
        else if (result == 2)
        {
            lblresult.Text = "Quotation Mode Does Not Exists..!!";
            lblresult.CssClass = "warning";
        }
        else
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void FillQuoteAppFields()
    {
        DataSet ds = new DataSet();
        ds = QuotationOperations.GetQuotationMode();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuoteModeDetails.DataSource = ds;
            gvQuoteModeDetails.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvQuoteModeDetails.DataSource = ds;
            gvQuoteModeDetails.DataBind();
            int columncount = gvQuoteModeDetails.Rows[0].Cells.Count;
            gvQuoteModeDetails.Rows[0].Cells.Clear();
            gvQuoteModeDetails.Rows[0].Cells.Add(new TableCell());
            gvQuoteModeDetails.Rows[0].Cells[0].ColumnSpan = columncount;
            gvQuoteModeDetails.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvQuoteModeDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvQuoteModeDetails.EditIndex = e.NewEditIndex;
        FillQuoteAppFields();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuoteModeDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvQuoteModeDetails.EditIndex = -1;
        FillQuoteAppFields();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuoteModeDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvQuoteModeDetails.PageIndex = e.NewPageIndex;
        gvQuoteModeDetails.DataBind();
        FillQuoteAppFields();
    }
}