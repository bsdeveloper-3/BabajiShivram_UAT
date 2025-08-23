using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_RangeCriteriaMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Range Criteria Detail";


        }


        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvRange.Columns;
        DataFilter1.FilterSessionID = "RangeCriteriaMaster.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);


    }






    #region FormView Event



    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "cancel")
        {
            gvRange.SelectedIndex = -1;
            gvRange.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;
            FormViewDataSource.SelectParameters[0].DefaultValue = "-1"; ;
        }
        else
        {
            gvRange.Visible = false;
            DataFilter1.Visible = false;
            fsMainBorder.Visible = false;
        }


    }

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        // Always Show Required Field Validator Message 

        Page.Validate("Required");
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
        }
    }

    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
        }
    }

    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lberror.Visible = true;
        if (Result > 0)
        {
            lberror.Text = "Range Details Added Successfully.";
            lberror.CssClass = "success";
            FormViewDataSource.SelectParameters[0].DefaultValue = "-1"; ;
            gvRange.SelectedIndex = -1;

            gvRange.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;



        }
        else if (Result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "Range Code Already Exists. Please Use Different one!";
            lberror.CssClass = "errorMsg";

        }
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Range Details Updated Successfully !";
            lberror.CssClass = "success";

            gvRange.SelectedIndex = -1;
            gvRange.DataBind();
            gvRange.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "Range Code Already Exists.!";
            lberror.CssClass = "errorMsg";
        }

    }

    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Range Deleted Successfully !";
            lberror.CssClass = "success";

            gvRange.SelectedIndex = -1;
            gvRange.DataBind();
            gvRange.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;

            lberror.Visible = true;
            lberror.Text = e.Exception.Message;
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion



    #region GridView Event

    protected void gvRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvRange.Visible = false;
        DataFilter1.Visible = false;
        fsMainBorder.Visible = false;
        if (gvRange.SelectedIndex == -1)
        {
            FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }
        FormView1.DataBind();
    }

    protected void gvRange_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "RangeCriteriaMaster.aspx";
            DataFilter1.FilterDataSource();
            gvRange.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData


    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "BabajiRangeMasterDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvRange.AllowPaging = false;
        gvRange.AllowSorting = false;
        gvRange.Columns[1].Visible = false;
        gvRange.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "RangeCriteriaMaster.aspx";
        DataFilter1.FilterDataSource();
        gvRange.DataBind();

        gvRange.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}