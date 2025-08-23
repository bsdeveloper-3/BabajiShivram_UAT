using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AjaxControlToolkit;
using ClosedXML.Excel;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Data.Common;
using System.Drawing;

public partial class Transport_BillHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkExport_Consolidate);
        if (!IsPostBack)
        {
            Session["TRId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transportation Request";

            DataFilter1.DataSource = DataSourceBillHistory;
            DataFilter1.DataColumns = gvBillHistory.Columns;
            DataFilter1.FilterSessionID = "BillHistory.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            DataFilter2.DataSource = DataSourceConsolidateBillHistory;
            DataFilter2.DataColumns = gvConsolidateBillHistory.Columns;
            DataFilter2.FilterSessionID = "BillHistory.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
        }
        if (TabPanelNormalJob.TabIndex == 0)
        {
            if (TabRequestRecd.ActiveTabIndex == 0)
            {
                lblError_Job.Text = "";
                DataFilter1.DataSource = DataSourceBillHistory;
                DataFilter1.DataColumns = gvBillHistory.Columns;
                DataFilter1.FilterSessionID = "BillHistory.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            }
        }
        //if (TabPanelConsolidateJob.TabIndex == 1)
        //{
        //    if (TabRequestRecd.ActiveTabIndex == 1)
        //    {
        //        lblError_ConsolidateJob.Text = "";
        //        DataFilter2.DataSource = DataSourceConsolidateBillHistory;
        //        DataFilter2.DataColumns = gvConsolidateBillHistory.Columns;
        //        DataFilter2.FilterSessionID = "BillHistory.aspx";
        //        DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);

        //    }
        //}
    }

    protected void gvBillHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            Session["TRId"] = commandArgs[0].ToString();
            Session["TransporterId"] = commandArgs[1].ToString();
            Session["lid"] = commandArgs[2].ToString();
            Response.Redirect("TransBillTracking.aspx");
        }
    }

    #region DOCUMENTS DOWNLOAD EVENTS

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
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
            if (TabRequestRecd.ActiveTabIndex == 0)
            {
                DataFilter1_OnDataBound();
            }
            else
            {
                DataFilter2_OnDataBound();
            }
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "BillHistory.aspx";
            DataFilter1.DataColumns = gvBillHistory.Columns;
            DataFilter1.FilterDataSource();
            gvBillHistory.DataBind();
            if (gvBillHistory.Rows.Count == 0)
            {
                lblError_Job.Text = "No Job Found For Bill History!";
                lblError_Job.CssClass = "errorMsg";
            }
            else
            {
                lblError_Job.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.FilterSessionID = "BillHistory.aspx";
            DataFilter2.DataColumns = gvConsolidateBillHistory.Columns;
            DataFilter2.FilterDataSource();
            gvConsolidateBillHistory.DataBind();
            if (gvConsolidateBillHistory.Rows.Count == 0)
            {
                lblError_ConsolidateJob.Text = "No Job Found For Consolidate Bill History!";
                lblError_ConsolidateJob.CssClass = "errorMsg";
            }
            else
            {
                lblError_ConsolidateJob.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "BillHistory_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvBillHistory.AllowPaging = false;
        gvBillHistory.AllowSorting = false;
        gvBillHistory.Columns[1].Visible = false;
        gvBillHistory.Columns[2].Visible = false;
        gvBillHistory.Caption = "Bill History On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "BillHistory.aspx";
        DataFilter1.FilterDataSource();
        gvBillHistory.DataBind();
        gvBillHistory.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkExport_Consolidate_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "ConsolidateBillHistory_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction_Consolidate("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ExportFunction_Consolidate(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvConsolidateBillHistory.AllowPaging = false;
        gvConsolidateBillHistory.AllowSorting = false;
        gvConsolidateBillHistory.Columns[1].Visible = false;
        gvConsolidateBillHistory.Caption = "Consolidate Bill History On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter2.FilterSessionID = "BillHistory.aspx";
        DataFilter2.FilterDataSource();
        gvConsolidateBillHistory.DataBind();
        gvConsolidateBillHistory.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    protected void gvConsolidateBillHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["TRId"] = commandArgs[0].ToString();
            Session["TRConsolidateId"] = commandArgs[1].ToString();
            Response.Redirect("ConsolidateTracking.aspx");
        }
    }
}