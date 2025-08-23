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
using BSImport;
using System.Data.SqlClient;
using System.Drawing;

public partial class PCDDocument : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "PCA Document";

            lblresult.Visible = false;
            PCDDocumentDetails();
        }
    }
    protected void gvPCDDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert")
        {
            lblresult.Visible = true;

            TextBox lid = gvPCDDocument.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox PCDname = gvPCDDocument.FooterRow.FindControl("txtPCDDocumentNamefooter") as TextBox;
            TextBox remark = gvPCDDocument.FooterRow.FindControl("txtsRemarksfooter") as TextBox;
            
            if (PCDname.Text.Trim() != "")
            {
                int result = DBOperations.AddPCDDocument(PCDname.Text.Trim(), remark.Text.Trim(), LoggedInUser.glUserId);

                if (result > 0)
                {
                    lblresult.CssClass = "success";
                    lblresult.Text = PCDname.Text.Trim() + " PCDDocument details added successfully!";

                    PCDDocumentDetails();
                }
                else
                {
                    lblresult.CssClass = "errorMsg";
                    lblresult.Text = "System Error! Please Try After Sometime.";
                }
            }
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the detail!";
            }
        }
    }
    protected void gvPCDDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;

        int lid=Convert.ToInt32(gvPCDDocument.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtPCDDocumentNamefooter = (TextBox)gvPCDDocument.Rows[e.RowIndex].FindControl("txtPCDDocumentName");
        TextBox txtsRemarksfooter = (TextBox)gvPCDDocument.Rows[e.RowIndex].FindControl("txtsRemarks");
        if (txtPCDDocumentNamefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdatePCDDocument(lid, txtPCDDocumentNamefooter.Text.Trim(), txtsRemarksfooter.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblresult.Text = " PCDDocument Details Updated Successfully.";
                lblresult.CssClass = "success";

                gvPCDDocument.EditIndex = -1;
                PCDDocumentDetails();
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please try after sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "PCDDocument Name Already Exists!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the detail!";
        }
    }

    protected void gvPCDDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvPCDDocument.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = DBOperations.DeletePCDDocument(lid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = " PCDDocument Details Deleted Successfully.";
            lblresult.CssClass = "success";
            PCDDocumentDetails();
        }
        else
        {
            lblresult.Text = "System Error! Please try after sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void PCDDocumentDetails()
    {
        DataSet ds = new DataSet();
        ds = DBOperations.GetPCDDocument();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPCDDocument.DataSource = ds;
            gvPCDDocument.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvPCDDocument.DataSource = ds;
            gvPCDDocument.DataBind();
            int columncount = gvPCDDocument.Rows[0].Cells.Count;
            gvPCDDocument.Rows[0].Cells.Clear();
            gvPCDDocument.Rows[0].Cells.Add(new TableCell());
            gvPCDDocument.Rows[0].Cells[0].ColumnSpan = columncount;
            gvPCDDocument.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvPCDDocument_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvPCDDocument.EditIndex = e.NewEditIndex;
        PCDDocumentDetails();
    }

    protected void gvPCDDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvPCDDocument.EditIndex = -1;
        PCDDocumentDetails(); 
    }

    protected void gvPCDDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblresult.Visible = false;
        lblresult.Text = "";
        gvPCDDocument.PageIndex  = e.NewPageIndex;
        gvPCDDocument.DataBind();
        PCDDocumentDetails();
    }    
}
