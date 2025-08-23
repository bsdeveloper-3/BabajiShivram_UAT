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

public partial class Transport_ViewTransporterBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkExport_Consolidate);
        if (!IsPostBack)
        {
            Session["TRId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Received";

            DataFilter1.DataSource = DataSourceBillNonReceive;
            DataFilter1.DataColumns = gvNonReceiveBill.Columns;
            DataFilter1.FilterSessionID = "ViewTransporterBill.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            DataFilter2.DataSource = DataSourceBillReceive;
            DataFilter2.DataColumns = gvReceiveBill.Columns;
            DataFilter2.FilterSessionID = "ViewTransporterBill.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
        }
        if (TabPanelNormalJob.TabIndex == 0)
        {
            if (TabRequestRecd.ActiveTabIndex == 0)
            {
                lblMessage.Text = "";
                lblError_Job.Text = "";
                DataFilter1.DataSource = DataSourceBillNonReceive;
                DataFilter1.DataColumns = gvNonReceiveBill.Columns;
                DataFilter1.FilterSessionID = "ViewTransporterBill.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            }
        }
        if (TabPanelConsolidateJob.TabIndex == 1)
        {
            if (TabRequestRecd.ActiveTabIndex == 1)
            {
                lblMessage.Text = "";
                lblError_ConsolidateJob.Text = "";
                DataFilter2.DataSource = DataSourceBillReceive;
                DataFilter2.DataColumns = gvReceiveBill.Columns;
                DataFilter2.FilterSessionID = "ViewTransporterBill.aspx";
                DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            }
        }
    }

    #region NON_RECEIVED EVENTS

    protected void btnReceive_Click(object sender, EventArgs e)
    {
        int TransBillId = 0, count = 0, result = 0;
        foreach (GridViewRow row in gvNonReceiveBill.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                TransBillId = Convert.ToInt32(gvNonReceiveBill.DataKeys[row.RowIndex].Value.ToString());
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                if (chkSelect.Checked)
                {
                    result = DBOperations.AddBillReceivedDetail(TransBillId, 0, LoggedInUser.glUserId, DateTime.MinValue, DateTime.Now, 1, "", DateTime.MinValue, "", DateTime.MinValue, LoggedInUser.glUserId);
                    if (result == 0)
                    {
                        count++;
                    }
                }
            }
        }

        if (count > 0)
        {
            if (result == 0)
            {
                lblMessage.Text = "Successfully received bill.";
                lblMessage.CssClass = "success";
            }
            else
            {
                lblMessage.Text = "Error while receiving bills. Please try again later.";
                lblMessage.CssClass = "success";
            }
            gvNonReceiveBill.AllowPaging = true;
            gvNonReceiveBill.DataBind();
            gvReceiveBill.AllowPaging = true;
            gvReceiveBill.DataBind();
        }
        else
        {
            lblMessage.Text = "Please checked atleast 1 checkbox to receive bill.";
            lblMessage.CssClass = "errorMsg";
        }
    }

    #endregion

    #region RECEIVED EVENTS
    protected void gvReceiveBill_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["TRId"] = commandArgs[0].ToString();
            Session["TRConsolidateId"] = commandArgs[1].ToString();
            Session["TransBillId"] = commandArgs[2].ToString();
            Session["TransporterId"] = commandArgs[3].ToString();
            Response.Redirect("TransportBillDetail.aspx");
        }
        else if (e.CommandName.ToLower() == "updatedetail")
        {
            int TransBillId = Convert.ToInt32(e.CommandArgument.ToString());
            hdnTransBillId.Value = TransBillId.ToString();
            DataView dvGetBillDetails = DBOperations.GetTransBillDetailById(TransBillId);
            if (dvGetBillDetails != null)
            {
                lblTRRefNo.Text = dvGetBillDetails.Table.Rows[0]["TRRefNo"].ToString();
                lblJobNo.Text = dvGetBillDetails.Table.Rows[0]["JobRefNo"].ToString();
                lblTransporter.Text = dvGetBillDetails.Table.Rows[0]["Transporter"].ToString();
                lblBillNumber.Text = dvGetBillDetails.Table.Rows[0]["BillNumber"].ToString();
                lblBillSubmitDate.Text = dvGetBillDetails.Table.Rows[0]["BillSubmitDate"].ToString();
                lblBillDate.Text = dvGetBillDetails.Table.Rows[0]["BillDate"].ToString();
                lblBillAmt.Text = dvGetBillDetails.Table.Rows[0]["BillAmount"].ToString();
                lblDetentionAmt.Text = dvGetBillDetails.Table.Rows[0]["DetentionAmount"].ToString();
                lblVaraiAmt.Text = dvGetBillDetails.Table.Rows[0]["VaraiAmount"].ToString();
                lblEmptyContCharges.Text = dvGetBillDetails.Table.Rows[0]["EmptyContRcptCharges"].ToString();
                lblTotalAmt.Text = dvGetBillDetails.Table.Rows[0]["TotalAmount"].ToString();
            }

            DataSet dsGetTransBillDetail = DBOperations.GetBillReceivedDetail(TransBillId);
            if (dsGetTransBillDetail != null && dsGetTransBillDetail.Tables.Count > 0)
            {
                if (dsGetTransBillDetail.Tables[0].Rows[0]["StatusId"] != DBNull.Value)
                {
                    ddlStatus.SelectedValue = dsGetTransBillDetail.Tables[0].Rows[0]["StatusId"].ToString();
                    txtChequeNo.Text = dsGetTransBillDetail.Tables[0].Rows[0]["ChequeNo"].ToString();
                    if (dsGetTransBillDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                        txtChequeDate.Text = Convert.ToDateTime(dsGetTransBillDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");
                }
            }
            mpeReceiveBill.Show();
        }
    }

    protected void btnSaveBill_Click(object sender, EventArgs e)
    {
        DateTime dtChequeDate = DateTime.MinValue, dtReleaseDate = DateTime.MinValue;
        if (txtChequeDate.Text.Trim() != "")
            dtChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text.Trim());
        if (txtReleaseDate.Text.Trim() != "")
            dtReleaseDate = Commonfunctions.CDateTime(txtReleaseDate.Text.Trim());

        if (hdnTransBillId.Value != "" && hdnTransBillId.Value != "0")
        {
            int BillId = Convert.ToInt32(hdnTransBillId.Value);
            if (BillId > 0)
            {
                int result = DBOperations.UpdateBillReceivedDetail(BillId, Convert.ToInt32(ddlStatus.SelectedValue), txtChequeNo.Text.Trim(), dtChequeDate,
                                            txtHoldReason.Text.Trim(), dtReleaseDate, LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblError_Popup.Text = "Successfully added detail";
                    lblError_Popup.CssClass = "success";
                    mpeReceiveBill.Show();
                    gvReceiveBill.DataBind();
                }
                else
                {
                    lblError_Popup.Text = "System error. Please try again later.";
                    lblError_Popup.CssClass = "errorMsg";
                    mpeReceiveBill.Show();
                }
            }
        }
    }

    protected void imgbtnClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeReceiveBill.Hide();
    }

    #endregion

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
            DataFilter1.FilterSessionID = "ViewTransporterBill.aspx";
            DataFilter1.DataColumns = gvNonReceiveBill.Columns;
            DataFilter1.FilterDataSource();
            gvNonReceiveBill.DataBind();
            if (gvNonReceiveBill.Rows.Count == 0)
            {
                lblError_Job.Text = "No Job Found For Non-Receive Bill!";
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
            DataFilter2.FilterSessionID = "ViewTransporterBill.aspx";
            DataFilter2.DataColumns = gvReceiveBill.Columns;
            DataFilter2.FilterDataSource();
            gvReceiveBill.DataBind();
            if (gvReceiveBill.Rows.Count == 0)
            {
                lblError_ConsolidateJob.Text = "No Job Found For Receive Bill!";
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
        string strFileName = "NonReceiveBills_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvNonReceiveBill.AllowPaging = false;
        gvNonReceiveBill.AllowSorting = false;
        gvNonReceiveBill.Columns[1].Visible = false;
        gvNonReceiveBill.Caption = "Non-Received Bills On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "ViewTransporterBill.aspx";
        DataFilter1.FilterDataSource();
        gvNonReceiveBill.DataBind();
        gvNonReceiveBill.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkExport_Consolidate_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "ReceiveBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvReceiveBill.AllowPaging = false;
        gvReceiveBill.AllowSorting = false;
        gvReceiveBill.Columns[1].Visible = false;
        gvReceiveBill.Columns[2].Visible = false;
        gvReceiveBill.Columns[3].Visible = true;
        gvReceiveBill.Caption = "Receive Bills On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter2.FilterSessionID = "ViewTransporterBill.aspx";
        DataFilter2.FilterDataSource();
        gvReceiveBill.DataBind();
        gvReceiveBill.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        rfvChequeNo.Enabled = false;
        mevChequeDate.IsValidEmpty = true;
        mevReleaseDate.IsValidEmpty = true;
        rfvHoldReason.Enabled = false;
        mpeReceiveBill.Show();

        if (ddlStatus.SelectedValue != "0")
        {
            if (ddlStatus.SelectedValue == "3")         // Cheque Prepare
            {
                rfvChequeNo.Enabled = true;
                mevChequeDate.IsValidEmpty = false;
            }
            else if (ddlStatus.SelectedValue == "4")    // Payment Release
            {
                mevReleaseDate.IsValidEmpty = false;
            }
            else if (ddlStatus.SelectedValue == "5")    // Hold Bill
            {
                rfvHoldReason.Enabled = true;
            }
        }
    }

    protected void gvReceiveBill_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Priority") != DBNull.Value)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Priority")) == 1) // high
                {
                    e.Row.BackColor = System.Drawing.Color.LightSeaGreen;
                    e.Row.ToolTip = "High Priority";
                }
                else if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Priority")) == 2) // normal
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    e.Row.ToolTip = "Normal Priority";
                }
                else if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Priority")) == 3) // intense
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                    e.Row.ToolTip = "Intense Priority";
                }
            }
        }
    }

}