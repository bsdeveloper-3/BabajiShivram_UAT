using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class Quotation_QuoteDashboard : System.Web.UI.Page
{
    string str_Message = "";
    LoginClass LoggedInUser = new LoginClass();
    DateTime dtClose = DateTime.MinValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkTotalSummaryExport);
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(lnkExportSummary);
        ScriptManager1.RegisterPostBackControl(lnkExportSummaryDetail);
        ScriptManager1.RegisterPostBackControl(gvSummaryQuotationMonth);
        ScriptManager1.RegisterPostBackControl(gvQuotationDetails);
        ScriptManager1.RegisterPostBackControl(gvSummaryQuotation);
        ScriptManager1.RegisterPostBackControl(gvSummaryStatusDetail);
    }

    protected void ddPendingQuotation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQuotationSummaryByType();
        gvSummaryQuotationMonth.DataBind();
    }

    protected void BindQuotationSummaryByType()
    {
        int ReportType = Convert.ToInt32(ddReportType.SelectedValue);

        if (ReportType == 0)        // ALL
        {
            legendName.InnerText = "Quotation Summary";
            DataSourceQuotationSummary.SelectCommand = "BS_AllQuotationSummary";
            gvSummaryQuotation.DataBind();
        }
        else if (ReportType == 1)   //Quoted for Customer
        {
            legendName.InnerText = "Customer Wise Summary";
            DataSourceQuotationSummary.SelectCommand = "BS_QuotedForCustSummary";
            gvSummaryQuotation.DataBind();
        }
        else if (ReportType == 2)   // Quotation Type
        {
            legendName.InnerText = "Quotation Type Wise Summary";
            DataSourceQuotationSummary.SelectCommand = "BS_QuotedForTypeSummary";
            gvSummaryQuotation.DataBind();
        }
        else if (ReportType == 3)   // Division
        {
            legendName.InnerText = "Division Wise Summary";
            DataSourceQuotationSummary.SelectCommand = "BS_QuotedForDivisionSummary";
            gvSummaryQuotation.DataBind();
        }
    }

    protected void ddReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindQuotationSummaryByType();
        //BindQuotationSummaryDetail();
    }

    protected void btnCancelMonthPopup_Click(object sender, EventArgs e)
    {

    }

    protected void lnkExportSummary_Click(object sender, EventArgs e)
    {
        string strFileName = "Freight_" + ddPendingQuotation.SelectedItem.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportSummary("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    #region POPUP STATUS CHANGES EVENT

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue != "0")
        {
            int Save = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(Session["QuotationId"]), Convert.ToInt32(ddlStatus.SelectedValue), txtRemark.Text.Trim(),dtClose, LoggedInUser.glUserId);
            if (Save == 1)
            {
                lbError_Popup.Text = "Successfully updated status for Quotation.";
                lbError_Popup.CssClass = "success";
                gvStatusHistory.DataBind();
                txtRemark.Text = "";
                ModalPopupDocument.Show();
            }
            else
            {
                lbError_Popup.Text = "Please select status.";
                lbError_Popup.CssClass = "errorMsg";
                ModalPopupDocument.Show();
            }
        }
        else
        {
            lbError_Popup.Text = "Please select status.";
            lbError_Popup.CssClass = "errorMsg";
            ddlStatus.Focus();
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue != "0")
        {
            if (Convert.ToInt32(ddlStatus.SelectedValue) == 5)
            {
                rfvrem.Visible = false;
                ModalPopupDocument.Show();
            }
            else
            {
                rfvrem.Visible = true;
                ModalPopupDocument.Show();
            }
        }
    }

    #endregion

    #region QUOTATION SUMMARY

    protected void gvQuotationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string IsTenderQuote = rowView["QuotedFormat"].ToString();
            foreach (TableCell cell in e.Row.Cells)
            {
                if (IsTenderQuote.Trim().ToLower() != "normal")
                {
                    e.Row.Cells[13].Text = "";
                }
            }

            string IsValidDraft = rowView["IsValidDraft"].ToString();
            foreach (TableCell cell in e.Row.Cells)
            {
                if (IsValidDraft.Trim().ToLower() == "0")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(238, 170, 144, 0.87)");
                    e.Row.ToolTip = "Invalid Draft Quotation.";
                }
                else if (IsValidDraft.Trim().ToLower() == "1")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(247, 233, 134, 0.29)");
                    e.Row.ToolTip = "Valid Draft Quotation.";
                }
            }

            int Status = Convert.ToInt32(rowView["ApprovalStatusId"].ToString());
            foreach (TableCell cell in e.Row.Cells)
            {
                if (Status == 2 || Status == 1) // aproval pending or draft quotation 
                {
                    e.Row.Cells[13].Text = "";
                    e.Row.Cells[12].Enabled = false;
                }
            }

            string QuotedFormat = rowView["QuotedFormat"].ToString();
            if (QuotedFormat != "")
            {
                if (QuotedFormat.ToLower().Trim() != "normal")
                {
                    e.Row.Cells[12].Text = "";
                }
            }
        }
        ModalPopupMonthStatus.Show();
    }

    protected void gvQuotationDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "getquote")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            int StatusId = 0;

            Session["QuotationId"] = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                StatusId = Convert.ToInt32(commandArgs[1].ToString());

            if (StatusId == 1)
                Response.Redirect("PendingQuotation.aspx");
            else if (StatusId > 2)
                Response.Redirect("ApprovedQuote.aspx");
            else
                Response.Redirect("EditQuotation.aspx");
        }
        else if (e.CommandName.ToLower() == "downloadquote")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDoc(DocPath);
        }
        else if (e.CommandName.ToLower() == "getstatus")
        {
            int QuotationId = Convert.ToInt32(e.CommandArgument.ToString());
            if (QuotationId != 0)
            {
                lblErrorPopup.Text = "";
                lblErrorPopup.CssClass = "";
                Session["QuotationId"] = QuotationId.ToString();
                ModalPopupDocument.Show();
            }
        }
    }

    protected void gvQuotationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ModalPopupMonthStatus.Show();
    }

    protected void gvQuotationDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        ModalPopupMonthStatus.Show();
    }

    protected void gvSummaryQuotationMonth_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "monthdetail")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strStageId = "", strMonthId = "", strSummaryType = "";

            strStageId = commandArgs[0].ToString();
            strMonthId = commandArgs[1].ToString();
            strSummaryType = ddPendingQuotation.SelectedValue;

            DataSourceMonthDetail.SelectParameters["MonthId"].DefaultValue = strMonthId;
            DataSourceMonthDetail.SelectParameters["StatusId"].DefaultValue = strStageId;
            DataSourceMonthDetail.SelectParameters["ReportType"].DefaultValue = strSummaryType;

            gvQuotationDetails.PageIndex = 0;
            gvQuotationDetails.DataBind();

            if (gvQuotationDetails.Rows.Count > 0)
                ModalPopupMonthStatus.Show();
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            //ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
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

    #region CUSTOMER WISE SUMMARY

    protected void BindQuotationSummaryDetail()
    {
        int ReportType = Convert.ToInt32(ddReportType.SelectedValue);

        if (ReportType == 0) // All
        {
            DataSourceSummaryDetail.SelectCommand = "BS_AllQuotationSummaryDetails";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 1) // Customer Wise
        {
            DataSourceSummaryDetail.SelectCommand = "BS_GetSummaryCustomerDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 2) // Quotation Type
        {
            DataSourceSummaryDetail.SelectCommand = "BS_GetSummaryTypeDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 3) // Division
        {
            DataSourceSummaryDetail.SelectCommand = "BS_GetSummaryDivisionDetail";
            DataSourceSummaryDetail.DataBind();
        }
    }

    protected void gvSummaryQuotation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSummaryQuotation.PageIndex = e.NewPageIndex;

        // Bind Freight Summary Detail GridView for Selected Type

        BindQuotationSummaryByType();
    }

    protected void gvSummaryQuotation_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindQuotationSummaryByType();
    }

    protected void btnCancelSummaryPopup_Click(object sender, EventArgs e)
    {
        ModalPopupSummaryDetail.Hide();
    }

    protected void DataSourceSummaryDetail_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            //Show error message

            //Set the exception handled property so it doesn't bubble-up
            e.ExceptionHandled = true;
        }
    }

    protected void gvSummaryStatusDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string IsTenderQuote = rowView["QuotedFormat"].ToString();
            foreach (TableCell cell in e.Row.Cells)
            {
                if (IsTenderQuote.Trim().ToLower() != "normal")
                {
                    e.Row.Cells[13].Text = "";
                }
            }

            string IsValidDraft = rowView["IsValidDraft"].ToString();
            foreach (TableCell cell in e.Row.Cells)
            {
                if (IsValidDraft.Trim().ToLower() == "0")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(238, 170, 144, 0.87)");
                    e.Row.ToolTip = "Invalid Draft Quotation.";
                }
                else if (IsValidDraft.Trim().ToLower() == "1")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(247, 233, 134, 0.29)");
                    e.Row.ToolTip = "Valid Draft Quotation.";
                }
            }

            int Status = Convert.ToInt32(rowView["ApprovalStatusId"].ToString());
            foreach (TableCell cell in e.Row.Cells)
            {
                if (Status == 2 || Status == 1) // aproval pending or draft quotation 
                {
                    e.Row.Cells[13].Text = "";
                    e.Row.Cells[12].Enabled = false;
                }
            }

            string QuotedFormat = rowView["QuotedFormat"].ToString();
            if (QuotedFormat != "")
            {
                if (QuotedFormat.ToLower().Trim() != "normal")
                {
                    e.Row.Cells[12].Text = "";
                }
            }
        }
        ModalPopupSummaryDetail.Show();
    }

    protected void gvSummaryStatusDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "getquote")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            int StatusId = 0;

            Session["QuotationId"] = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                StatusId = Convert.ToInt32(commandArgs[1].ToString());

            if (StatusId == 1)
                Response.Redirect("PendingQuotation.aspx");
            else if (StatusId > 2)
                Response.Redirect("ApprovedQuote.aspx");
            else
                Response.Redirect("EditQuotation.aspx");
        }
        else if (e.CommandName.ToLower() == "downloadquote")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDoc(DocPath);
        }
        else if (e.CommandName.ToLower() == "getstatus")
        {
            int QuotationId = Convert.ToInt32(e.CommandArgument.ToString());
            if (QuotationId != 0)
            {
                lblErrorPopup.Text = "";
                lblErrorPopup.CssClass = "";
                Session["QuotationId"] = QuotationId.ToString();
                ModalPopupDocument.Show();
            }
        }
    }

    protected void gvSummaryStatusDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ModalPopupSummaryDetail.Show();
    }

    protected void gvSummaryStatusDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        ModalPopupSummaryDetail.Show();
    }


    #endregion

    #region POPUP FOR SUMMARY QUOTATION EVENTS

    protected void gvSummaryQuotation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "summarydetail")
        {
            ModalPopupSummaryDetail.Show();
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            string strDetailFor = "", strStageId = "", strMonthId = "", strSummaryType = "", strStageName = "";
            int ReportType = 0;

            strDetailFor = commandArgs[0].ToString();
            strStageId = commandArgs[1].ToString();
            strMonthId = ddMonth.SelectedValue;
            strSummaryType = ddPendingQuotation.SelectedValue;

            ReportType = Convert.ToInt32(ddReportType.SelectedValue);
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
            Label lblName = (Label)gvSummaryQuotation.Rows[row.RowIndex].FindControl("lnksName");

            if (strStageId == "1")
                strStageName = "Draft Quotation";
            else if (strStageId == "2")
                strStageName = "Approval Pending";
            else if (strStageId == "3")
                strStageName = "Approved";
            else if (strStageId == "4")
                strStageName = "Rejected";
            else if (strStageId == "5")
                strStageName = "Awarded";
            else if (strStageId == "6")
                strStageName = "Awaited";
            else if (strStageId == "7")
                strStageName = "Lost";
            else if (strStageId == "8")
                strStageName = "Negotiate";

            lblSummaryStatus.Text = lblName.Text + " - " + ddMonth.SelectedItem.Text + " - " + ddPendingQuotation.SelectedItem.Text + " - " + strStageName;
            DataSourceSummaryDetail.SelectParameters["lId"].DefaultValue = strDetailFor;
            DataSourceSummaryDetail.SelectParameters["MonthId"].DefaultValue = strMonthId;
            DataSourceSummaryDetail.SelectParameters["StatusId"].DefaultValue = strStageId;
            DataSourceSummaryDetail.SelectParameters["ReportType"].DefaultValue = strSummaryType;
            gvSummaryStatusDetail.PageIndex = 0;
            BindQuotationSummaryDetail();
        }
    }

    #endregion

    #region ExportData

    private void ExportSummary(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvQuotationDetails.AllowPaging = false;
        gvQuotationDetails.AllowSorting = false;

        // Bind Summary Grid
        gvQuotationDetails.DataBind();
        DataSourceMonthDetail.DataBind();
        gvQuotationDetails.Columns[13].Visible = false;

        //Remove LinkButtong Controls
        this.RemoveControls(gvQuotationDetails);
        gvQuotationDetails.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();

    }

    protected void lnkTotalSummaryExport_Click(object sender, EventArgs e)
    {
        string strReportType = ddPendingQuotation.SelectedItem.Text + "_";
        string strReportFor = ddReportType.SelectedItem.Text + "_";

        string strFileName = "FreightStatus_" + strReportType + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportTotal("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ExportTotal(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvSummaryQuotationMonth.AllowPaging = false;
        gvSummaryQuotationMonth.AllowSorting = false;

        // Bind Summary Grid
        BindQuotationSummaryByType();

        //Remove LinkButtong Controls
        this.RemoveControls(gvSummaryQuotationMonth);
        gvSummaryQuotationMonth.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();

    }

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strReportType = ddPendingQuotation.SelectedItem.Text + "_";
        string strMonth = ddMonth.SelectedItem.Text + "_";
        string strReportFor = ddReportType.SelectedItem.Text + "_";

        string strFileName = "FreightStatus_" + strReportType + strMonth + strReportFor + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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

        gvSummaryQuotation.AllowPaging = false;
        gvSummaryQuotation.AllowSorting = false;

        // Bind Summary Grid
        BindQuotationSummaryByType();

        //Remove LinkButtong Controls
        this.RemoveControls(gvSummaryQuotation);
        gvSummaryQuotation.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }

    protected void lnkExportSummaryDetail_Click(object sender, EventArgs e)
    {
        string strFileName = lblSummaryStatus.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportSummaryDetail("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ExportSummaryDetail(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvSummaryStatusDetail.AllowPaging = false;
        gvSummaryStatusDetail.AllowSorting = false;

        // Bind Summary Grid
        BindQuotationSummaryDetail();
        gvSummaryStatusDetail.Columns[13].Visible = false;
        this.RemoveControls(gvSummaryStatusDetail);
        gvSummaryStatusDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}