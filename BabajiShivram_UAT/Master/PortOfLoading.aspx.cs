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

public partial class Master_PortOfLoading : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            lberror.Visible = false;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Loading Port Setup";
        }

        //
        DataFilter1.DataSource = GridViewSqlDataSource;
        DataFilter1.DataColumns = GridView1.Columns;
        DataFilter1.FilterSessionID = "PortOfLoading.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    protected void btnNewButton_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.Insert);

        GridView1.Visible = false;
        fsMainBorder.Visible = false;
    }

    #region FormView Event

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        Page.Validate("Required");
    }

    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            GridView1.Visible = true;
            GridView1.SelectedIndex = -1;
            fsMainBorder.Visible = true;
        }
        else
        {
            GridView1.Visible = false;
            fsMainBorder.Visible = false;
        }
    }

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }

    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
            lberror.Visible = true;
            lberror.Text = e.Exception.Message;
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
        }
    }

    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        string LoadingPortName = e.Command.Parameters["@LoadingPortName"].Value.ToString();

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = LoadingPortName + " Loading Port Successfully Added!";
            lberror.CssClass = "success";

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            GridView1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {

            lberror.Text = LoadingPortName + " Loading Port Already Exist.";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        string LoadingPortName = e.Command.Parameters["@LoadingPortName"].Value.ToString();

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = LoadingPortName + " Loading Port Successfully Updated!";
            lberror.CssClass = "success";

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            GridView1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = LoadingPortName + " Loading Port Already Exist.";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Loading Port Deleted Successfully !";
            lberror.CssClass = "success";

            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            GridView1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = " Loading Port Not Found.";
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FormView1.DataBind();
        FormView1.ChangeMode(FormViewMode.ReadOnly);
        GridView1.Visible = false;
        fsMainBorder.Visible = false;
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
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
            DataFilter1.FilterSessionID = "PortOfLoading.aspx";
            DataFilter1.FilterDataSource();
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}
