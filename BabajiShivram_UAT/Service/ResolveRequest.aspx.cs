using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_ResolveRequest : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Resolve Branch Issue";

            if (GridViewService.Rows.Count == 0)
            {
                lblError.Text = "No Branch Issue Reuqest Found!";
                lblError.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceService;
        DataFilter1.DataColumns = GridViewService.Columns;
        DataFilter1.FilterSessionID = "ResolveRequest.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int result = 0;

        foreach (GridViewRow row in GridViewService.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string RequestId = GridViewService.DataKeys[row.RowIndex].Value.ToString();

                    CheckBox chkSelect = (row.FindControl("chkSelect") as CheckBox);
                    TextBox txtApprovedRate = (row.FindControl("txtApprovedRate") as TextBox);

                    string strRate = txtApprovedRate.Text.Trim();

                    if (chkSelect.Checked)
                    {
                        try
                        {
                            if (strRate != "") // Check If Rate is not Zero
                            {
                                result = DBOperations.AddApprovareTransRate(Convert.ToInt32(RequestId), Convert.ToInt32(strRate), LoggedInUser.glUserId);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = ex.Message;
                            lblError.CssClass = "errorMsg";
                            result = -1;
                            return;
                        }
                    }//END_IF
                }//END_IF
            }//END_IF
        }//END_ForEarch

        // IF No Error in New Exbond Quantity Update Invoice Details
        if (result == 0)
        {
            lblError.Text = "Rate Approved Successfully!";
            lblError.CssClass = "success";

            GridViewService.DataBind();
        }
        else
        {
            GridViewService.DataBind();
        }
    }

    #region GridView Event

    protected void GridViewService_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = GridViewService.DataKeys[gvrow.RowIndex].Value.ToString();
        }

        if (e.CommandName.ToLower() == "cancel")
        {
            GridViewService.EditIndex = -1;
        }
    }

    protected void GridViewService_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewService.EditIndex = e.NewEditIndex;
    }

    protected void GridViewService_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int IssueId = Convert.ToInt32(GridViewService.DataKeys[e.RowIndex].Value.ToString());

        TextBox txtResolveRemark = (TextBox)GridViewService.Rows[e.RowIndex].FindControl("txtResolveRemark");


        if (txtResolveRemark.Text.Trim() != "")
        {

            int result = DBOperations.AddResolveRequest(IssueId, txtResolveRemark.Text.Trim(),LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Issue Detail Updated Successfully.";
                lblError.CssClass = "success";

                GridViewService.EditIndex = -1;
                e.Cancel = true;

            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else if (result == 2)
            {
                lblError.Text = "Issue Already Resolved!";
                lblError.CssClass = "errorMsg";
                GridViewService.EditIndex = -1;
                e.Cancel = true;
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
        }
        else
        {
            lblError.Text = "Please Enter Remark to Resolve the issue";
            lblError.CssClass = "errorMsg";
            
        }
    }

    protected void GridViewService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewService.EditIndex = -1;
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
            DataFilter1.FilterSessionID = "ResolveRequest.aspx";
            DataFilter1.FilterDataSource();
            GridViewService.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}