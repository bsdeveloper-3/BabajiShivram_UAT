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

public partial class CFSMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "CFS Setup";
            GetCFSMDetails();
        }
    }

    private void GetCFSMDetails()
    {        
        DataSet dsCFSDetails = DBOperations.GetCFSDetail();
               
        if(dsCFSDetails.Tables[0].Rows.Count > 0)
        {
            gvCFSMaster.DataSource = dsCFSDetails;
            gvCFSMaster.DataBind();
        }
        else
        {
            dsCFSDetails.Tables[0].Rows.Add();
            gvCFSMaster.DataSource = dsCFSDetails;
            gvCFSMaster.DataBind();
            int columncount = gvCFSMaster.Rows[0].Cells.Count;
            gvCFSMaster.Rows[0].Cells.Clear();
            gvCFSMaster.Rows[0].Cells.Add(new TableCell());
            gvCFSMaster.Rows[0].Cells[0].ColumnSpan = columncount;
            gvCFSMaster.Rows[0].Cells[0].Text = "No Records Found!";
        }

        DropDownList ddUserFooter = gvCFSMaster.FooterRow.FindControl("ddUserFooter") as DropDownList;
        DBOperations.FillBabajiUser(ddUserFooter);
    }
           
    private void DisplayMsg(int outVal, string commandtype)
    {
        if (outVal == 0)
        {
            lblResult.Text = " CFS " + commandtype + " Successfully";
            lblResult.CssClass = "success";
            gvCFSMaster.EditIndex = -1;
            GetCFSMDetails();
        }
        else if (outVal == 1)
        {
            lblResult.Text = "System Error! Please Try After Sometime.";
            lblResult.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            if (commandtype == "Deleted")
                lblResult.Text = "CFS Name Not Found!";
            else
                lblResult.Text = "CFS Name Already Exist!";
            lblResult.CssClass = "warning";
        }
    }

    #region GridViewEvents

    protected void gvCFSMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(((Label)gvCFSMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        string sName = ((TextBox)gvCFSMaster.Rows[e.RowIndex].FindControl("txteCFSName")).Text.Trim();
        string sRemark = ((TextBox)gvCFSMaster.Rows[e.RowIndex].FindControl("txteRemark")).Text.Trim();

        DropDownList ddCFSUser = gvCFSMaster.Rows[e.RowIndex].FindControl("ddCFSUser") as DropDownList;

        int CFSUserId = Convert.ToInt32(ddCFSUser.SelectedValue);


        if (lid != 0 && !string.IsNullOrEmpty(sName))
        {
            int outVal = DBOperations.UpdateCFSMDetails(lid, CFSUserId ,sName, sRemark, LoggedInUser.glUserId);
            DisplayMsg(outVal, "CFS Details Updated SUccessfully!");
        }
        else
        {
            lblResult.Text = "Please Enter CFS Name!";
            lblResult.CssClass = "errorMsg";
        }
    }

    protected void gvCFSMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCFSMaster.EditIndex = -1;
        GetCFSMDetails();
        lblResult.Text = string.Empty;

    }

    protected void gvCFSMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int lId = Convert.ToInt32(((Label)gvCFSMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        
        if (lId != 0 && LoggedInUser.glUserId != 0)
        {
            int outVal = DBOperations.DeletFromCFSMDetails(lId, LoggedInUser.glUserId);
            
            DisplayMsg(outVal, "Deleted"); 
        }
    }

    protected void gvCFSMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCFSMaster.EditIndex = e.NewEditIndex;
        GetCFSMDetails();
        lblResult.Text = string.Empty;
    }

    protected void gvCFSMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "insert")
        {
            string sName = ((TextBox)gvCFSMaster.FooterRow.FindControl("txtCFSName")).Text.Trim();
            string sRemark = ((TextBox)gvCFSMaster.FooterRow.FindControl("txtRemark")).Text.Trim();

            DropDownList ddUserFooter = gvCFSMaster.FooterRow.FindControl("ddUserFooter") as DropDownList;

            int CFSUserId = Convert.ToInt32(ddUserFooter.SelectedValue);

            if (!string.IsNullOrEmpty(sName.Trim()))
            {
                int outVal = DBOperations.AddCFSDetail(sName, CFSUserId,LoggedInUser.glUserId, sRemark);

                DisplayMsg(outVal, "CFS Details Added Successfully!");
            }
            else
            {
                lblResult.Text = "Please Enter CFS Name!";
                lblResult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvCFSMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCFSMaster.PageIndex = e.NewPageIndex;

        gvCFSMaster.DataBind();
        GetCFSMDetails();
    }

    protected void gvCFSMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView dRowView = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlUser = (DropDownList)e.Row.FindControl("ddCFSUser");
                DBOperations.FillBabajiUser(ddlUser);

                ddlUser.SelectedValue = dRowView["CFSUserID"].ToString();
            }
        }
    }

    #endregion

}
