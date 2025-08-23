using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class ExportCHA_PendingExportJob : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Job Creation";
        lblMessage.Visible = false;
        //
        if (!IsPostBack)
        {
            Session["PreAlertId"] = null;
            Session["JobId"] = null;

            if (gvPreAlert.Rows.Count == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Job Not Found For Creation!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //
        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvPreAlert.Columns;
        DataFilter1.FilterSessionID = "PendingExportJob.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GridView Event
    protected void gvPreAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int32 AgingHour = 0;

            //if ((DataBinder.Eval(e.Row.DataItem, "PreAlertHour")) != DBNull.Value)
            //{
            //    AgingHour = (Int32)(DataBinder.Eval(e.Row.DataItem, "PreAlertHour"));

            //    if (AgingHour > 80)
            //    {
            //        LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");

            //        lnkCreate.Text = "Unlock Job";
            //        lnkCreate.CommandName = "UnlockJob";

            //        e.Row.BackColor = System.Drawing.Color.FromName("#2266bb");
            //        e.Row.ToolTip = "Job Locked - Delay Penalty will be applicable";
            //    }
            //}
        }
    }

    protected void gvPreAlert_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strJobId = (string)e.CommandArgument;
            Session["JobId"] = strJobId;
            Response.Redirect("JobEntry.aspx");
        }
    }

    protected void gvPreAlert_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "PendingExportJob.aspx";
            DataFilter1.FilterDataSource();
            gvPreAlert.DataBind();
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
        ExportFunction("attachment;filename=PendingJob.xls", "application/vnd.ms-excel");
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
        gvPreAlert.AllowPaging = false;
        gvPreAlert.AllowSorting = false;
        gvPreAlert.DataSourceID = "GridviewSqlDataSource";

        DataFilter1.FilterSessionID = "PendingExportJob.aspx";
        DataFilter1.FilterDataSource();

        gvPreAlert.DataBind();
        // BindGridData();
        //gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        // gvPreAlert.HeaderRow.Cells[0].Visible = false;
        gvPreAlert.Columns[1].Visible = false;

        //for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //{
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //}


        gvPreAlert.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}