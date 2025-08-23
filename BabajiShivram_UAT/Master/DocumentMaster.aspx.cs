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
using System.IO;

public partial class Master_DocumentMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Document Master";

            lblresult.Visible = false;

            DocumentDetails();
        }
    }

    protected void DocumentDetails()
    {
        DataSet ds = new DataSet();

        ds = DBOperations.GetDocumentMaster();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentMaster.DataSource = ds;
            gvDocumentMaster.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvDocumentMaster.DataSource = ds;
            gvDocumentMaster.DataBind();
            int columncount = gvDocumentMaster.Rows[0].Cells.Count;
            gvDocumentMaster.Rows[0].Cells.Clear();
            gvDocumentMaster.Rows[0].Cells.Add(new TableCell());
            gvDocumentMaster.Rows[0].Cells[0].ColumnSpan = columncount;
            gvDocumentMaster.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvDocumentMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDocumentMaster.PageIndex = e.NewPageIndex;
        gvDocumentMaster.DataBind();
        DocumentDetails();
    }

    protected void gvDocumentMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(gvDocumentMaster.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtDoc_Namefooter = (TextBox)gvDocumentMaster.Rows[e.RowIndex].FindControl("txtDoc_Name");
        DropDownList ddlOperationtype = (DropDownList)gvDocumentMaster.Rows[e.RowIndex].FindControl("ddFieldType");
        DropDownList ddlOperationMode = (DropDownList)gvDocumentMaster.Rows[e.RowIndex].FindControl("ddlMode");
        DropDownList ddCompFooter = (DropDownList)gvDocumentMaster.Rows[e.RowIndex].FindControl("ddComp");


        if (txtDoc_Namefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateDocument(lid, txtDoc_Namefooter.Text,ddlOperationtype.SelectedItem.Text,ddlOperationMode.SelectedItem.Text,Convert.ToInt32(ddCompFooter.SelectedValue),LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtDoc_Namefooter.Text.Trim() + " Document Details Updated Successfully.";
                gvDocumentMaster.EditIndex = -1;
                DocumentDetails();
            }
            else if (result == 1)
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " System Error! Please Try After Sometime.";
            }
            else if (result == 2)
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = "Document Name Already Added!";
            }
        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }

    protected void gvDocumentMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvDocumentMaster.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteDocument(lid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = " Document Deleted Successfully!";
            lblresult.CssClass = "success";
            DocumentDetails();
        }
        else if (result == 1)
        {
            lblresult.Text = " System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void gvDocumentMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName.ToLower() == "insert")
        {
            TextBox lid = gvDocumentMaster.FooterRow.FindControl("txtIdFooter") as TextBox;
            TextBox txt_DocName = gvDocumentMaster.FooterRow.FindControl("txtDoc_Namefooter") as TextBox;
            DropDownList ddlOperationtype = gvDocumentMaster.FooterRow.FindControl("ddFieldTypeFooter") as DropDownList;
            DropDownList ddlOperationMode = gvDocumentMaster.FooterRow.FindControl("ddModeFooter") as DropDownList;
            DropDownList ddlCompulsary = gvDocumentMaster.FooterRow.FindControl("ddCompFooter") as DropDownList;

            if (txt_DocName.Text.Trim() != "" && ddlOperationtype.SelectedItem.Text!= "--Select--" && ddlCompulsary.SelectedValue!= "--Select--")
            {
                int result = DBOperations.AddDocument(txt_DocName.Text.Trim(), ddlOperationtype.SelectedItem.Text,ddlOperationMode.SelectedItem.Text,Convert.ToInt32(ddlCompulsary.SelectedValue), LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblresult.Text = txt_DocName.Text.Trim() + "  Document added successfully.";
                    lblresult.CssClass = "success";
                    DocumentDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Document Name Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }
            }
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvDocumentMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDocumentMaster.EditIndex = e.NewEditIndex;
        DocumentDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvDocumentMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDocumentMaster.EditIndex = -1;
        DocumentDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        ExportFunction("attachment;filename=DocumentMaster.xls", "application/vnd.ms-excel");
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvDocumentMaster.AllowPaging = false;
        gvDocumentMaster.AllowSorting = false;

        gvDocumentMaster.Columns[1].Visible = false;
        gvDocumentMaster.Columns[2].Visible = true;

        gvDocumentMaster.Columns[0].Visible = false;
        //gvDocumentMaster.Columns[3].Visible = true;

        DataSet ds = new DataSet();

        ds = DBOperations.GetDocumentMaster();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentMaster.DataSource = ds;
            gvDocumentMaster.DataBind();
        }

        gvDocumentMaster.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //Allows for printing
    }
}