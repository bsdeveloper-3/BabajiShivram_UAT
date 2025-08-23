using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContMovement_ContainerDetail : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCancelContainer_Click(object sender, EventArgs e)
    {
        Response.Redirect("ContReceived.aspx");
    }

    protected void lnkexport_Click(object sender, EventArgs e)
    {

    }

    protected void gvContRecdCFS_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToString().Trim().ToLower() == "addcfs")
        {
            DateTime dtContRecdAtCFSDate = DateTime.MinValue;
            int lid = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvContRecdCFS.Rows[lid];
            TextBox txtContRecdAtCFSDate = (TextBox)(row.FindControl("txtContRecdAtCFSDate"));
            HiddenField hdnlid = (HiddenField)(row.FindControl("hdnlid"));
            HiddenField hdnDispatchDate = (HiddenField)(row.FindControl("hdnDispatchDate"));

            if (hdnDispatchDate.Value != "" && hdnDispatchDate.Value != "0")
            {
                if (txtContRecdAtCFSDate.Text.Trim() != "")
                    dtContRecdAtCFSDate = Commonfunctions.CDateTime(txtContRecdAtCFSDate.Text.Trim());

                if (dtContRecdAtCFSDate != DateTime.MinValue)
                {
                    int result = CMOperations.AddContRecdCFSDetail(dtContRecdAtCFSDate, Convert.ToInt32(hdnlid.Value), loggedInUser.glUserId);
                    if (result == 0)
                    {
                        lblError.Text = "Successfully added container CFS received date.";
                        lblError.CssClass = "success";
                        gvContRecdCFS.DataBind();
                    }
                    else if (result == 2)
                    {
                        lblError.Text = "Job does not exists!";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "Error while adding up container CFS received date.";
                        lblError.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblError.Text = "Required - container received at CFS date.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please update dispatch date before updating container received at CFS date.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvContRecdCFS_PreRender(object sender, EventArgs e)
    {

    }

    protected void gvContRecdCFS_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void DataSourceContReceived_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;
        int result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);
        if (result == 0)
        {
            lblError.Text = "Successfully added container CFS received date.";
            lblError.CssClass = "success";
        }
        else if (result == 2)
        {
            lblError.Text = "Job does not exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Error while adding up container CFS received date.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DataSourceContReceived_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        System.Data.Common.DbParameterCollection CmdParams = e.Command.Parameters;
        ParameterCollection UpdParams = ((SqlDataSourceView)sender).UpdateParameters;

        Hashtable ht = new Hashtable();
        foreach (Parameter UpdParam in UpdParams)
            ht.Add(UpdParam.Name, true);

        for (int i = 0; i < CmdParams.Count; i++)
        {
            if (!ht.Contains(CmdParams[i].ParameterName.Substring(1)))
                CmdParams.Remove(CmdParams[i--]);
        }
    }

    protected void gvDeliveryDetails_PreRender(object sender, EventArgs e)
    {

    }

}