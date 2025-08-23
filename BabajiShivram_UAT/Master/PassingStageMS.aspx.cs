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

public partial class PassingStageMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Passing Stage Setup";

            GetPStagesMDetails();
          
        }
    }

    private void GetPStagesMDetails()
    {

        DataSet dsCFSDetails = null;
        
        dsCFSDetails = GetPStageDetails();
        
        if (dsCFSDetails == null || dsCFSDetails.Tables == null || dsCFSDetails.Tables[0].Rows.Count == 0)
        {
            dsCFSDetails.Tables[0].Rows.Add();
            gvPStageMaster.DataSource = dsCFSDetails;
            gvPStageMaster.DataBind();
        }
        else
        {
            gvPStageMaster.DataSource = dsCFSDetails;
            gvPStageMaster.DataBind();
        }

    }
    
    private DataSet GetPStageDetails()
    {
        return CDatabase.GetDataSet("GetPassingStagesMaster", "PSDetails");
    }

    private void DisplayMsg(int outVal, string commandtype)
    {
        if (outVal == 0)
        {
            lblResult.Text = " Passing Stage " + commandtype + " Successfully.";
            lblResult.CssClass = "success";
            gvPStageMaster.EditIndex = -1;
            GetPStagesMDetails();
        }
        else if (outVal == 1)
        {
            lblResult.Text = "System Error! Please Try After Sometime.";
            lblResult.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            if (commandtype == "Deleted")
                lblResult.Text = "Passing Stage Detail Not Found!";
            else
                lblResult.Text = "Passing Stage Already Exist!";
            lblResult.CssClass = "warning";
        }
    }

    #region GridViewEvents

    protected void gvPStageMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(((Label)gvPStageMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        string sName = ((TextBox)gvPStageMaster.Rows[e.RowIndex].FindControl("txteStageName")).Text;
        string sRemark = ((TextBox)gvPStageMaster.Rows[e.RowIndex].FindControl("txteRemark")).Text;
        
        if (lid != 0 && !string.IsNullOrEmpty(sName))
        {
            int outVal = DBOperations.UpdatePassignStageMaster(lid, sName, sRemark, LoggedInUser.glUserId);

            DisplayMsg(outVal, "Updated");
        }
        else
        {
            lblResult.Text = "Please Enter Passing Stage Name!";
            lblResult.CssClass = "errorMsg";
        }
    }

    protected void gvPStageMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPStageMaster.EditIndex = -1;
        GetPStagesMDetails();
        lblResult.Text = string.Empty;

    }

    protected void gvPStageMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {        
        int lId = Convert.ToInt32(((Label)gvPStageMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        
        if (lId != 0 && LoggedInUser.glUserId != 0)
        {
            int outVal = DBOperations.DeletPassignStageMaster(lId, LoggedInUser.glUserId);

            DisplayMsg(outVal, "Deleted");
        }
    }

    protected void gvPStageMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPStageMaster.EditIndex = e.NewEditIndex;
        GetPStagesMDetails();
        lblResult.Text = string.Empty;
    }

    protected void gvPStageMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Insert")
        {
            string sName = ((TextBox)gvPStageMaster.FooterRow.FindControl("txtStageName")).Text;
            string sRemark = ((TextBox)gvPStageMaster.FooterRow.FindControl("txtRemark")).Text;
            
            if (!string.IsNullOrEmpty(sName.Trim()))
            {
                int outVal = DBOperations.AddPassignStageMaster(sName, LoggedInUser.glUserId, sRemark);

                DisplayMsg(outVal, "Added");
            }
            else
            {
                lblResult.Text = "Please Enter Passing Stage Name!";
                lblResult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvPStageMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPStageMaster.PageIndex = e.NewPageIndex;

        gvPStageMaster.DataBind();
        GetPStagesMDetails();
    }
    #endregion

}
