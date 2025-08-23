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

public partial class Location : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Location Setup";

            gvLocaton.SelectedIndex = -1;
            
        }
    }

    protected void lnkCityName_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.ReadOnly);
    }
    
    protected void btnNewButton_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.Insert);
    }

    protected void gvLocaton_SelectedIndexChanged(object sender, EventArgs e)
    {
        FormView1.DataBind();
    }

    #region FormView Event

   
    protected void FormView1_DataBound(object sender, EventArgs e)
    {
      //  Page.Validate();
    }
    
    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }
    }

    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
        }
    }

    protected void DataSourceFormView_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
      int Result =   Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lberror.Visible = true;

        if(Result > 0)
        {
            lberror.Text = "City Detail Added Successfully";
            lberror.CssClass = "success";
            SDSFormView.SelectParameters[0].DefaultValue = Result.ToString();
            gvLocaton.SelectedIndex = -1;
            gvLocaton.DataBind();
        }
        else if (Result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "City Name Already Exists";
            lberror.CssClass = "errorMsg";
            
        }
    }

    protected void DataSourceFormView_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        string Result = e.Command.Parameters["@OutPut"].Value.ToString();

        lberror.Visible = true;
        if (Result == "0")
        {
            lberror.Text = "City Detail Updated Successfully";
            lberror.CssClass = "success";
            gvLocaton.DataBind();
        }
        else if (Result == "1")
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == "2")
        {
            lberror.Text = "City Name Already Exists";
            lberror.CssClass = "errorMsg";

        }
    }
    
    protected void DataSourceFormView_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        string Result = e.Command.Parameters["@OutPut"].Value.ToString();

        lberror.Visible = true;
        if (Result == "0")
        {
            lberror.Text = "City Detail Deleted Successfully";
            lberror.CssClass = "success";
            gvLocaton.DataBind();
            gvLocaton.SelectedIndex = -1;
        }
        else if (Result == "1")
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == "2")
        {
            lberror.Text = "City Not Exists";
            lberror.CssClass = "errorMsg";

        }
    }

    protected void gvLocaton_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #endregion
}
