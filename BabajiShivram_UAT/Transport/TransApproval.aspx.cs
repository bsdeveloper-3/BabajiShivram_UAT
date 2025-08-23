using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_TransApproval : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Rate Approval";

            if (GridViewVehicle.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Transport Rate Approval!";
                lblError.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceVehicle;
        DataFilter1.DataColumns = GridViewVehicle.Columns;
        DataFilter1.FilterSessionID = "TransApproval.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
        
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int result = -1;

        foreach (GridViewRow row in GridViewVehicle.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string RequestId = GridViewVehicle.DataKeys[row.RowIndex].Value.ToString();

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
                        catch(Exception ex)
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

            GridViewVehicle.DataBind();
        }
        else
        {
            lblError.Text = "Please Enter Approved Rate!";
            lblError.CssClass = "errorMsg";
            GridViewVehicle.DataBind();
        }
    }
   
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
            DataFilter1.FilterSessionID = "TransApproval.aspx";
            DataFilter1.FilterDataSource();
            GridViewVehicle.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}