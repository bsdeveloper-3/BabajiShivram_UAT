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


public partial class ChecklistDocument : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }
    private string _sortDirection;
    Image sortImage = new Image();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Document Detail";
            
            lblresult.Visible = false;
            
            checklistDocumentDetails();
        }
    }
        
    protected void gvchecklistDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert")
        {
            lblresult.Visible = true;
                        
            TextBox LID = gvchecklistDocument.FooterRow.FindControl("txtlidfooter") as TextBox;
            
            DropDownList DOC_TYPE = gvchecklistDocument.FooterRow.FindControl("ddDocTypefooter") as DropDownList;
            
            TextBox DocumentName = gvchecklistDocument.FooterRow.FindControl("txtDocumentNamefooter") as TextBox;
            TextBox SREMARK = gvchecklistDocument.FooterRow.FindControl("txtsRemarksfooter") as TextBox;
            
            int luserID = LoggedInUser.glUserId;
            
            if (DOC_TYPE.SelectedValue != "0" && DocumentName.Text.Trim() != "")
            {                
       
                int result = DBOperations.AddChecklistDocument(DocumentName.Text.Trim(), Convert.ToInt32(DOC_TYPE.SelectedItem.Value), SREMARK.Text.Trim(), luserID);
                if (result > 0)
                {
                    lblresult.CssClass = "success";
                    lblresult.Text = DocumentName.Text.Trim() + " Document details added successfully!";

                    checklistDocumentDetails();
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
        DropDownList ddldoctype = (DropDownList)gvchecklistDocument.Rows[e.RowIndex].FindControl("ddDocType");
        TextBox txtsRemarksfooter = (TextBox)gvchecklistDocument.Rows[e.RowIndex].FindControl("txtsRemarks");
        
        if (ddldoctype.SelectedValue !="0" && txtDocumentNamefooter.Text.Trim() != "")
        {
            int result = DBOperations.UpdateChecklistDocument(Lid, txtDocumentNamefooter.Text.Trim(), Convert.ToInt32(ddldoctype.SelectedItem.Value), txtsRemarksfooter.Text.Trim(),LoggedInUser.glUserId);
            if (result == 0)
            {
                lblresult.Text = "Document Details Updated Successfully.";
                lblresult.CssClass = "success";
                
                gvchecklistDocument.EditIndex = -1;
                checklistDocumentDetails();
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
        int result = DBOperations.DeleteChecklistDocument(lid,LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Document Details Deleted Successfully.";
            lblresult.CssClass = "success";
            
            checklistDocumentDetails();
        }
        else
        {
            lblresult.Text = "System Error! Please try after sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }
    
    protected void checklistDocumentDetails()
    {
        DataSet ds = new DataSet();
             
        ds = DBOperations.GetChecklistDocument();
        
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
        
        DropDownList DOC_TYPE = gvchecklistDocument.FooterRow.FindControl("ddDocTypefooter") as DropDownList;
        DBOperations.FillChekListDocType(DOC_TYPE);
        
    }
    
    protected void gvchecklistDocument_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblresult.Visible = false;   
        gvchecklistDocument.EditIndex = e.NewEditIndex;
        checklistDocumentDetails();
        lblresult.Text = "";
     }

    protected void gvchecklistDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblresult.Visible = false;
        gvchecklistDocument.EditIndex = -1;
        checklistDocumentDetails();
        lblresult.Text = "";
    }

    protected void gvchecklistDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblresult.Visible = false;
        gvchecklistDocument.PageIndex = e.NewPageIndex;

        gvchecklistDocument.DataBind();
        checklistDocumentDetails();
        lblresult.Text = "";
    }

    protected void gvchecklistDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        DataRowView dRowView = (DataRowView)e.Row.DataItem;
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
               
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddDocType");
                DBOperations.FillChekListDocType(ddlStatus);
                ddlStatus.SelectedValue = dRowView["DocTypeId"].ToString();
                
              //  ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(dRowView[2].ToString()));
              //  ddlStatus.SelectedItem.Text = dRowView[2].ToString();
              // ddlStatus.SelectedItem.Value = DBOperations.searchDocindexposition(dRowView[2].ToString()).ToString();
          
            }//END_IF

        }//END_IF
    }

    protected void gvchecklist_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection(SortDireaction);
        DataSet ds = new DataSet();

        ds = DBOperations.GetChecklistDocument();

        ds.Tables[0].DefaultView.Sort = e.SortExpression + " " + _sortDirection;
        gvchecklistDocument.DataSource = ds.Tables[0].DefaultView;
        gvchecklistDocument.DataBind();
        SortDireaction = _sortDirection;

        //Sort the data.
        
        //int columnIndex = 0;
        //foreach (DataControlFieldHeaderCell headerCell in gvchecklistDocument.HeaderRow.Cells)
        //{
        //    if (headerCell.ContainingField.SortExpression == e.SortExpression)
        //    {
        //        columnIndex = gvchecklistDocument.HeaderRow.Cells.GetCellIndex(headerCell);
        //    }
        //}

        //gvchecklistDocument.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
        
    }
    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";
          //  sortImage.ImageUrl = "Images//view_sort_ascending.png";
        }
        else
        {
            _sortDirection = "ASC";
          //  sortImage.ImageUrl = "Images//view_sort_descending.png";
        }
    } 
}
