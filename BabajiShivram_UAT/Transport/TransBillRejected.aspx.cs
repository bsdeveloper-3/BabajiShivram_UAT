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

public partial class Transport_TransBillRejected : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkJobExport);
        ScriptManager1.RegisterPostBackControl(lnkConsolidateExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Bill";

            Session["TRId"] = null;
            Session["TRConsolidateId"] = null;
            Session["TransBillId"] = null;

            DataFilter1.DataSource = DataSourceBillRejectedPending;
            DataFilter1.DataColumns = gvTransportBillApproval.Columns;
            DataFilter1.FilterSessionID = "TransBillRejected.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            DataFilter2.DataSource = DataSourceConsolidateBillRejectedPending;
            DataFilter2.DataColumns = gvConsolidateBillApproval.Columns;
            DataFilter2.FilterSessionID = "TransBillApproval.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
        }

        if (TabPanelNormalJob.TabIndex == 0)
        {
            if (TabBilling.ActiveTabIndex == 0)
            {
                lblError_Job.Text = "";
                DataFilter1.DataSource = DataSourceBillRejectedPending;
                DataFilter1.DataColumns = gvTransportBillApproval.Columns;
                DataFilter1.FilterSessionID = "TransBillRejected.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            }
        }

        if (TabPanelConsolidateJob.TabIndex == 1)
        {
            if (TabBilling.ActiveTabIndex == 1)
            {
                lblError_Consolidate.Text = "";
                DataFilter2.DataSource = DataSourceConsolidateBillRejectedPending;
                DataFilter2.DataColumns = gvConsolidateBillApproval.Columns;
                DataFilter2.FilterSessionID = "TransBillApproval.aspx";
                DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            }
        }
    }

    protected void gvTransportBillApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["TransBillId"] = commandArgs[0].ToString();
            Session["TRId"] = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                Session["TRConsolidateId"] = commandArgs[2].ToString();
            Session["TransporterId"] = commandArgs[3].ToString();
            Response.Redirect("BillRejectedDetail.aspx");
        }
    }

    protected void gvConsolidateBillApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            if (commandArgs[0].ToString() != "")
                Session["TRConsolidateId"] = commandArgs[0].ToString();
            Session["TRId"] = commandArgs[1].ToString();
            Session["TransBillId"] = commandArgs[2].ToString();
            Session["TransporterId"] = commandArgs[3].ToString();
            Response.Redirect("BillRejectedDetail.aspx");
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
            if (TabBilling.ActiveTabIndex == 0)
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
            DataFilter1.FilterSessionID = "TransBillRejected.aspx";
            DataFilter1.DataColumns = gvTransportBillApproval.Columns;
            DataFilter1.FilterDataSource();
            gvTransportBillApproval.DataBind();
            if (gvTransportBillApproval.Rows.Count == 0)
            {
                lblError_Job.Text = "No Job Found For Normal Job Detail!";
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
            DataFilter2.FilterSessionID = "TransBillRejected.aspx";
            DataFilter2.DataColumns = gvConsolidateBillApproval.Columns;
            DataFilter2.FilterDataSource();
            gvConsolidateBillApproval.DataBind();
            if (gvConsolidateBillApproval.Rows.Count == 0)
            {
                lblError_Consolidate.Text = "No Job Found For Consolidate Job Detail!";
                lblError_Consolidate.CssClass = "errorMsg";
            }
            else
            {
                lblError_Consolidate.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkJobExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + "TransportBillApproval_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvTransportBillApproval.AllowPaging = false;
        gvTransportBillApproval.AllowSorting = false;
        gvTransportBillApproval.Columns[0].Visible = false;
        gvTransportBillApproval.Columns[1].Visible = false;
        gvTransportBillApproval.Columns[2].Visible = true;

        gvTransportBillApproval.Caption = "Transport Bill Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "TransBillRejected.aspx";
        DataFilter1.FilterDataSource();
        gvTransportBillApproval.DataBind();
        gvTransportBillApproval.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkConsolidateExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + "TransportBillApproval_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvConsolidateBillApproval.AllowPaging = false;
        gvConsolidateBillApproval.AllowSorting = false;
        gvConsolidateBillApproval.Columns[0].Visible = false;
        gvConsolidateBillApproval.Columns[1].Visible = false;
        gvConsolidateBillApproval.Columns[2].Visible = true;

        gvConsolidateBillApproval.Caption = "Transport Consolidate Bill Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "TransBillRejected.aspx";
        DataFilter1.FilterDataSource();
        gvConsolidateBillApproval.DataBind();
        gvConsolidateBillApproval.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}