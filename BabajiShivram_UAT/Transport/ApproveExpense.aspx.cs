using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
public partial class Transport_ApproveExpense : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Transport Expense Approval";
        //
        if (!IsPostBack)
        {
            if (gvMaintenance.Rows.Count == 0)
            {
                lblError.Text = "No Records Found !";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = SqlDataSourceExp;
        DataFilter1.DataColumns = gvMaintenance.Columns;
        DataFilter1.FilterSessionID = "ApproveExpense.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
        
    protected void gvMaintenance_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvMaintenance_RowCommand(Object sender,  GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "viewdocument")
        {
            int TransID = Convert.ToInt32(e.CommandArgument);

            BindDocumentList(TransID);
        }
    }

    protected void gvMaintenance_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        lblError.Text = "";
        gvMaintenance.EditIndex = e.NewEditIndex;
    }

    protected void gvMaintenance_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            DataKey dataKey = gvMaintenance.DataKeys[e.RowIndex];
            decimal reqAmount = 0; decimal approvedAmount = 0;

            string strMaintainId = dataKey[0].ToString();
            string strExpenseId = dataKey[1].ToString();

            Label lblAmount = (Label)gvMaintenance.Rows[e.RowIndex].FindControl("lblAmount");
            
            TextBox txtApprovedAmount = (TextBox)gvMaintenance.Rows[e.RowIndex].FindControl("txtApprovedAmount");

            reqAmount = Convert.ToDecimal(lblAmount.Text.Trim());
            
            if (txtApprovedAmount.Text.Trim() != "")
            {
                approvedAmount = Convert.ToDecimal(txtApprovedAmount.Text.Trim());
            }
            if (approvedAmount >= 1)
            {
                if (approvedAmount <= reqAmount)
                {
                    SqlDataSourceExp.UpdateParameters["MaintanceId"].DefaultValue = strMaintainId;
                    SqlDataSourceExp.UpdateParameters["ExpenseId"].DefaultValue = strExpenseId;
                    SqlDataSourceExp.UpdateParameters["ApprovedAmount"].DefaultValue = txtApprovedAmount.Text.Trim();
                }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "Please check Approved Amount!! Should be less than requested amount!";
                    lblError.CssClass = "errorMsg";  
                }
            }
            else
            {
                e.Cancel = true;
                lblError.Text = "Please Enter Approved Amount!!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            e.Cancel = true;
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Description = DataBinder.Eval(e.Row.DataItem, "ShortExpenseDesc").ToString();

            if (Description.Length < 50)
            {
                LinkButton lnkMore = (LinkButton)e.Row.FindControl("lnkMore");

                if (lnkMore != null)
                    lnkMore.Visible = false;
            }

            // Disable Edit For Approved Expense

            bool IsApproved = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsApproved"));

            if (IsApproved == true)
            {
                LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                if (lnkEdit != null)
                    lnkEdit.Visible = false;
            }
        }

         //if enter key is pressed (keycode==13) call __doPostBack on grid and with
         //1st param = gvChild.UniqueID (Gridviews UniqueID)
         //2nd param = CommandName=Update$  +  CommandArgument=RowIndex
        
        if ((e.Row.RowState == DataControlRowState.Edit) ||
            (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)))
        {
            e.Row.Attributes.Add("onkeypress", "javascript:if (event.keyCode == 13) {__doPostBack('" + gvMaintenance.UniqueID + "', 'Update$" + e.Row.RowIndex.ToString() + "'); return false; }");
        }
    }

    protected void BindDocumentList(int TransID)
    {
        SqlConnection con = CDatabase.getConnection();

        //Query to get Doc Name and Doc Path from database
        SqlCommand command = new SqlCommand("SELECT DocName,DocPath from TR_Document WHERE bDel=0 AND TransportID= " + TransID.ToString(), con);
        SqlDataAdapter da = new SqlDataAdapter(command);
        DataTable dt = new DataTable();
        da.Fill(dt);
        dlImages.DataSource = dt;
        dlImages.DataBind();
        con.Close();
                
        ModalPopupDoc.Show();
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDoc.Hide();
    }


    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }
    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "ApproveExpense.aspx";
            DataFilter1.FilterDataSource();
            gvMaintenance.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region DataSourceEvents
    
    protected void SqlDataSourceExp_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = true;

        if (e.Exception != null)
        {
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

            if (Result == 0)
            {
                lblError.Text = "Expense Detail Approved Successfully !";
                lblError.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Expense Already Approved!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    #endregion
}