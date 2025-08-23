using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_MiscJobDocMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Document Master";

            lblresult.Visible = false;

            GetDocumentDetails();
        }
    }

    protected void gvchecklistDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert")
        {
            lblresult.Visible = true;

            TextBox LID = gvchecklistDocument.FooterRow.FindControl("txtlidfooter") as TextBox;

            DropDownList ddlModule = gvchecklistDocument.FooterRow.FindControl("ddlModule") as DropDownList;

            TextBox DocumentName = gvchecklistDocument.FooterRow.FindControl("txtDocumentNamefooter") as TextBox;
            TextBox SREMARK = gvchecklistDocument.FooterRow.FindControl("txtsRemarksfooter") as TextBox;

            int luserID = LoggedInUser.glUserId;

            if (ddlModule.SelectedValue != "0" && DocumentName.Text.Trim() != "")
            {

                int result = JobOperation.AddMiscDocument(DocumentName.Text.Trim(), Convert.ToInt32(ddlModule.SelectedItem.Value), SREMARK.Text.Trim(), luserID);
                
                if (result > 0)
                {
                    lblresult.CssClass = "success";
                    lblresult.Text = DocumentName.Text.Trim() + " Document details added successfully!";

                    GetDocumentDetails();
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

    protected void gvchecklistDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        int Lid = Convert.ToInt32(gvchecklistDocument.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtDocumentNamefooter = (TextBox)gvchecklistDocument.Rows[e.RowIndex].FindControl("txtDocumentName");
        TextBox txtsRemarksfooter = (TextBox)gvchecklistDocument.Rows[e.RowIndex].FindControl("txtsRemarks");

        if (txtDocumentNamefooter.Text.Trim() != "")
        {
            int result = JobOperation.UpdateMiscDocument(Lid, txtDocumentNamefooter.Text.Trim(),  txtsRemarksfooter.Text.Trim(), LoggedInUser.glUserId);
            if (result == 0)
            {
                lblresult.Text = "Document Details Updated Successfully.";
                lblresult.CssClass = "success";

                gvchecklistDocument.EditIndex = -1;
                GetDocumentDetails();
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please try after sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Document Name Already Exists!";
                lblresult.CssClass = "warning";
            }
        }
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the detail!";
        }

    }

    protected void gvchecklistDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int lid = Convert.ToInt32(gvchecklistDocument.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = JobOperation.DeleteMiscDocument(lid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Document Details Deleted Successfully.";
            lblresult.CssClass = "success";

            GetDocumentDetails();
        }
        else
        {
            lblresult.Text = "System Error! Please try after sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void GetDocumentDetails()
    {
        DataSet ds = new DataSet();

        ds = JobOperation.GetMiscDocumentMS();

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvchecklistDocument.DataSource = ds;
            gvchecklistDocument.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvchecklistDocument.DataSource = ds;
            gvchecklistDocument.DataBind();
            int columncount = gvchecklistDocument.Rows[0].Cells.Count;
            gvchecklistDocument.Rows[0].Cells.Clear();
            gvchecklistDocument.Rows[0].Cells.Add(new TableCell());
            gvchecklistDocument.Rows[0].Cells[0].ColumnSpan = columncount;
            gvchecklistDocument.Rows[0].Cells[0].Text = "No Records Found!";
        }

    }

    protected void gvchecklistDocument_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;
        gvchecklistDocument.EditIndex = e.NewEditIndex;
        GetDocumentDetails();
        lblresult.Text = "";
    }

    protected void gvchecklistDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        gvchecklistDocument.EditIndex = -1;
        GetDocumentDetails();
        lblresult.Text = "";
    }

    protected void gvchecklistDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblresult.Visible = false;
        gvchecklistDocument.PageIndex = e.NewPageIndex;

        gvchecklistDocument.DataBind();
        GetDocumentDetails();
        lblresult.Text = "";
    }

}