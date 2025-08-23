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

public partial class PackageTypeMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Package Type Setup";
            GetPackageDetails();
        }
    }

    private void GetPackageDetails()
    {
        DataSet dsPackageDetails = DBOperations.GetPackageTypeMS();

        if (dsPackageDetails.Tables[0].Rows.Count > 0)
        {
            gvPackageMaster.DataSource = dsPackageDetails;
            gvPackageMaster.DataBind();
        }
        else
        {
            dsPackageDetails.Tables[0].Rows.Add();
            gvPackageMaster.DataSource = dsPackageDetails;
            gvPackageMaster.DataBind();
            int columncount = gvPackageMaster.Rows[0].Cells.Count;
            gvPackageMaster.Rows[0].Cells.Clear();
            gvPackageMaster.Rows[0].Cells.Add(new TableCell());
            gvPackageMaster.Rows[0].Cells[0].ColumnSpan = columncount;
            gvPackageMaster.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }
        
    private void DisplayMsg(int outVal, string commandtype)
    {
        if (outVal == 0)
        {
            lblResult.Text = " Package Type " + commandtype + " Successfully";
            lblResult.CssClass = "success";
            gvPackageMaster.EditIndex = -1;
            GetPackageDetails();
        }
        else if (outVal == 1)
        {
            lblResult.Text = "System Error! Please Try After Sometime.";
            lblResult.CssClass = "errorMsg";
        }
        else if (outVal == 2)
        {
            if (commandtype == "Deleted")
                lblResult.Text = "Package Type Not Found!";
            else
                lblResult.Text = "Package Type Already Exist!";
            lblResult.CssClass = "errorMsg";
        }
    }

    #region GridViewEvents

    protected void gvPackageMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(((Label)gvPackageMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);
        string sName = ((TextBox)gvPackageMaster.Rows[e.RowIndex].FindControl("txtEdtPackageName")).Text.Trim();
        string sCode = ((TextBox)gvPackageMaster.Rows[e.RowIndex].FindControl("txtEdtPackageCode")).Text.Trim();
        
        if (lid != 0 && !string.IsNullOrEmpty(sName))
        {
            int outVal = DBOperations.UpdatePackageTypeMS(lid, sName,sCode, LoggedInUser.glUserId);

            DisplayMsg(outVal, "Updated");
        }
        else
        {
            lblResult.Text = "Please Enter Package Type!";
            lblResult.CssClass = "errorMsg";
        }
    }

    protected void gvPackageMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPackageMaster.EditIndex = -1;
        GetPackageDetails();
        lblResult.Text = string.Empty;

    }

    protected void gvPackageMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int lId = Convert.ToInt32(((Label)gvPackageMaster.Rows[e.RowIndex].FindControl("lblLid")).Text);

        if (lId != 0 && LoggedInUser.glUserId != 0)
        {
            int outVal = DBOperations.DeletePackageTypeMS(lId, LoggedInUser.glUserId);

            DisplayMsg(outVal, "Deleted");

        }
    }

    protected void gvPackageMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPackageMaster.EditIndex = e.NewEditIndex;
        GetPackageDetails();
        lblResult.Text = string.Empty;
    }

    protected void gvPackageMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "insert")
        {
            string sName = ((TextBox)gvPackageMaster.FooterRow.FindControl("txtPackageName")).Text.Trim();
            string sCode = ((TextBox)gvPackageMaster.FooterRow.FindControl("txtPackageCode")).Text.Trim();
            
            if (sName != "" && sCode != "")
            {
                int outVal = DBOperations.AddPackageTypeMS(sName,sCode, LoggedInUser.glUserId);

                DisplayMsg(outVal, "Added");
            }
            else
            {
                lblResult.Text = "Please Enter Required Details!";
                lblResult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvPackageMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPackageMaster.PageIndex = e.NewPageIndex;

        gvPackageMaster.DataBind();
        GetPackageDetails();
    }
    #endregion

}
