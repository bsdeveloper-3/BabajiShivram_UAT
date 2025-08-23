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

public partial class Expenses : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
             lblTitle.Text = "Expense Type Setup";
            
            GetExpensesMDetails();
            
        }
    }

    private void GetExpensesMDetails()
    {

        DataSet dsCFSDetails = DBOperations.GetExpenseTYpeMS();
        
        if (dsCFSDetails.Tables[0].Rows.Count > 0)
        {
            gvExpensesMaster.DataSource = dsCFSDetails;
            gvExpensesMaster.DataBind();
        }
        else
        {
            dsCFSDetails.Tables[0].Rows.Add();
            gvExpensesMaster.DataSource = dsCFSDetails;
            gvExpensesMaster.DataBind();

            int columncount = gvExpensesMaster.Rows[0].Cells.Count;
            gvExpensesMaster.Rows[0].Cells.Clear();
            gvExpensesMaster.Rows[0].Cells.Add(new TableCell());
            gvExpensesMaster.Rows[0].Cells[0].ColumnSpan = columncount;
            gvExpensesMaster.Rows[0].Cells[0].Text = "No Record Found!";
        
        }

    }
        
    private void DisplayMsg(int outVal, string commandtype)
    {
        if (outVal == 0)
        {
            lblResult.Text = " Expense Type " + commandtype + " Successfully";
            lblResult.CssClass = "success";
            gvExpensesMaster.EditIndex = -1;
            GetExpensesMDetails();
        }
        else if (outVal == 1)
        {
            lblResult.Text = "System Error! Please Try After Sometime.";
            lblResult.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            if (commandtype == "Deleted")
                lblResult.Text = "Expense Type Not Found!";
            else
                lblResult.Text = "Expense Type Already Exist!";
            lblResult.CssClass = "warning";
        }
    }

    #region GridViewEvents

    protected void gvExpensesMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(((Label)gvExpensesMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        string expenseName = ((TextBox)gvExpensesMaster.Rows[e.RowIndex].FindControl("txteExpenseMName")).Text;
        string eRemark = ((TextBox)gvExpensesMaster.Rows[e.RowIndex].FindControl("txteRemark")).Text;
        
        if (lid != 0 && !string.IsNullOrEmpty(expenseName))
        {
            int outVal = DBOperations.UpdateExpensesMDetails(lid, expenseName, eRemark, LoggedInUser.glUserId);

            DisplayMsg(outVal, "Updated");
        }
        else
        {
            lblResult.Text = "Please Enter Expense Type!";
            lblResult.CssClass = "errorMsg";
        }
    }

    protected void gvExpensesMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvExpensesMaster.EditIndex = -1;
        GetExpensesMDetails();
        lblResult.Text = string.Empty;

    }

    protected void gvExpensesMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        
        int lId = Convert.ToInt32(((Label)gvExpensesMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        
        if (lId != 0 && LoggedInUser.glUserId != 0)
        {
            int outVal = DBOperations.DeletExpensesMDetails(lId, LoggedInUser.glUserId);

            DisplayMsg(outVal, "Deleted");

        }
    }

    protected void gvExpensesMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvExpensesMaster.EditIndex = e.NewEditIndex;
        GetExpensesMDetails();
        lblResult.Text = string.Empty;
    }

    protected void gvExpensesMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert")
        {
            string expenseName = ((TextBox)gvExpensesMaster.FooterRow.FindControl("txtExpenseMName")).Text;
            string eRemark = ((TextBox)gvExpensesMaster.FooterRow.FindControl("txtRemark")).Text;
            
            if (!string.IsNullOrEmpty(expenseName.Trim()))
            {
                int outVal = DBOperations.AddExpensesMDetails(expenseName, LoggedInUser.glUserId, eRemark);

                DisplayMsg(outVal, "Added");
            }
            else
            {
                lblResult.Text = "Please Enter Expense Type!";
                lblResult.CssClass = "errorMsg";
            }
        }

    }

    protected void gvDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvExpensesMaster.PageIndex = e.NewPageIndex;

        gvExpensesMaster.DataBind();
        GetExpensesMDetails();
    }
    #endregion
}
