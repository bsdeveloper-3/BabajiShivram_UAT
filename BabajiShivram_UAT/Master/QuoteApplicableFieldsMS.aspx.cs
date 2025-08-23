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


public partial class QuoteApplicableFieldsMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(gvQuotationAppFields);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Quotation Applicable Fields Setup";

            lblresult.Visible = false;
            FillQuoteAppFields();
        }
    }

    protected void gvQuotationAppFields_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName == "Insert")
        {
            TextBox txtQuotationAppField = gvQuotationAppFields.FooterRow.FindControl("txtQuotationAppFieldfooter") as TextBox;
            if (txtQuotationAppField.Text.Trim() != "" && txtQuotationAppField.Text.Trim() != "")
            {
                int result = QuotationOperations.AddQuotationAppFieldMS(txtQuotationAppField.Text.Trim(), LoggedInUser.glUserId);
                if (result == 1)
                {
                    lblresult.Text = txtQuotationAppField.Text.Trim() + " Quotation Applicable Field Added Successfully..!!";
                    lblresult.CssClass = "success";
                    FillQuoteAppFields();
                }
                else if (result == 2)
                {
                    lblresult.Text = "Quotation Applicable Field Already Exist!";
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
                txtQuotationAppField.Focus();
            }
        }
    }

    protected void gvQuotationAppFields_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvQuotationAppFields.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtQuotationCatg = (TextBox)gvQuotationAppFields.Rows[e.RowIndex].FindControl("txtQuotationAppField");

        if (txtQuotationCatg.Text.Trim() != "" && txtQuotationCatg.Text.Trim() != "")
        {
            int result = QuotationOperations.UpdateQuotationAppFieldMS(Lid, txtQuotationCatg.Text.Trim(), LoggedInUser.glUserId);
            if (result == 1)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtQuotationCatg.Text.Trim() + " Quotation Applicable Field Updated Successfully..!!";
                gvQuotationAppFields.EditIndex = -1;
                FillQuoteAppFields();
            }
            else if (result == 2)
            {
                lblresult.Text = "Quotation Applicable Field Does Not Exists..!!";
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
            lblresult.Text = " Please Enter Quotation Applicable Field.";
        }
    }

    protected void gvQuotationAppFields_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvQuotationAppFields.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteQuotationAppFieldMS(lid, LoggedInUser.glUserId);
        if (result == 1)
        {
            lblresult.Text = "Quotation Applicable Field Deleted Successfully!";
            lblresult.CssClass = "success";
            FillQuoteAppFields();
        }
        else if (result == 2)
        {
            lblresult.Text = "Quotation Applicable Field Does Not Exists..!!";
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
        ds = QuotationOperations.GetQuotationAppFields();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuotationAppFields.DataSource = ds;
            gvQuotationAppFields.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvQuotationAppFields.DataSource = ds;
            gvQuotationAppFields.DataBind();
            int columncount = gvQuotationAppFields.Rows[0].Cells.Count;
            gvQuotationAppFields.Rows[0].Cells.Clear();
            gvQuotationAppFields.Rows[0].Cells.Add(new TableCell());
            gvQuotationAppFields.Rows[0].Cells[0].ColumnSpan = columncount;
            gvQuotationAppFields.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvQuotationAppFields_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvQuotationAppFields.EditIndex = e.NewEditIndex;
        FillQuoteAppFields();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuotationAppFields_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvQuotationAppFields.EditIndex = -1;
        FillQuoteAppFields();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuotationAppFields_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvQuotationAppFields.PageIndex = e.NewPageIndex;
        gvQuotationAppFields.DataBind();
        FillQuoteAppFields();
    }
}