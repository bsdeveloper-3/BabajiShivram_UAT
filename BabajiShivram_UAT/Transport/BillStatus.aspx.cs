using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

public partial class Transport_BillStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["BillAmt"] = 0;
        ViewState["DetentionAmt"] = 0;
        ViewState["VaraiAmt"] = 0;
        ViewState["EmptyContReturnAmt"] = 0;
        ViewState["TollCharges"] = 0;
        ViewState["OtherCharges"] = 0;
        ViewState["TotalAmt"] = 0;

        //ScriptManager1.RegisterPostBackControl(gvTransBillDetail);
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkbtnExportExcel);
        ScriptManager1.RegisterPostBackControl(lnkbtnExportMemo);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Status Summary";
        }

        DataFilter1.DataSource = SqlDataSourceBillAmt;
        DataFilter1.DataColumns = gvTransBillDetail.Columns;
        DataFilter1.FilterSessionID = "BillStatus.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region Bill Status 
    protected void gvMemoStatusDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "priority")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DropDownList ddlPriority = (DropDownList)gvMemoStatusDetail.Rows[rowIndex].FindControl("ddlPriority");
            HiddenField hdnTransBillId = (HiddenField)gvMemoStatusDetail.Rows[rowIndex].FindControl("hdnTransBillId");
            if (ddlPriority != null && hdnTransBillId.Value != "" && hdnTransBillId.Value != "0")
            {
                int result = DBOperations.AddMemoPriority(Convert.ToInt32(hdnTransBillId.Value), Convert.ToInt32(ddlPriority.SelectedValue), LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblErrorMemo.Text = "Updated priority succesfully.";
                    lblErrorMemo.CssClass = "success";
                    gvMemoStatusDetail.DataBind();
                    mpeMemoStatus.Show();
                }
                else
                {
                    lblErrorMemo.Text = "System error while updating priority. Please try again after sometime!";
                    lblErrorMemo.CssClass = "errorMsg";
                    mpeMemoStatus.Show();
                }
            }
        }
    }

    protected void gvTransBillDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int iBillPending = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BillPending"));
            int iTotalMemos = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalMemos"));
            int iTotalCheques = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalCheques"));
            int iTotalBillHold = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalBillHold"));
            int iPaymentRelease = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PaymentRelease"));

            LinkButton lnkbtnBillPending = (LinkButton)e.Row.FindControl("lnkbtnBillPending");
            LinkButton lnkbtnMemos = (LinkButton)e.Row.FindControl("lnkbtnMemos");
            LinkButton lnkbtnCheques = (LinkButton)e.Row.FindControl("lnkbtnCheques");
            LinkButton lnkbtnHoldBill = (LinkButton)e.Row.FindControl("lnkbtnHoldBill");
            LinkButton lnkbtnRelease = (LinkButton)e.Row.FindControl("lnkbtnRelease");

            if (iBillPending == 0)
            {
                lnkbtnBillPending.Enabled = false;
            }

            if (iTotalMemos == 0)
            {
                lnkbtnMemos.Enabled = false;
            }

            if (iTotalCheques == 0)
            {
                lnkbtnCheques.Enabled = false;
            }

            if (iTotalBillHold == 0)
            {
                lnkbtnHoldBill.Enabled = false;
            }

            if (iPaymentRelease == 0)
            {
                lnkbtnRelease.Enabled = false;
            }
        }
    }

    protected void gvTransBillDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        mpeMemoStatus.Hide();
        mpePopup.Hide();
        if (e.CommandName.ToLower().Trim() == "nobill")
        {
            hdnStatusId.Value = "1";
            lblPopup_Title.Text = "Number of Bill Pending - ";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnTransporterId.Value = commandArgs[0].ToString();
            lblTransporter_OtherStatus.Text = commandArgs[1].ToString();
            if (hdnTransporterId.Value != "" && hdnTransporterId.Value != "0")
            {
                gvBillStatusDetail.DataSourceID = "DataSourceBillPending";
                gvBillStatusDetail.DataBind();
                mpePopup.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "cheque")
        {
            hdnStatusId.Value = "3";
            lblPopup_Title.Text = "Total Bill Cheques - ";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnTransporterId.Value = commandArgs[0].ToString();
            lblTransporter_OtherStatus.Text = commandArgs[1].ToString();
            if (hdnTransporterId.Value != "" && hdnTransporterId.Value != "0")
            {
                gvBillStatusDetail.DataSourceID = "DataSourceChequeStatusDetail";
                gvBillStatusDetail.DataBind();
                mpePopup.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "holdbill")
        {
            hdnStatusId.Value = "4";
            lblPopup_Title.Text = "Total Bill Hold - ";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnTransporterId.Value = commandArgs[0].ToString();
            lblTransporter_OtherStatus.Text = commandArgs[1].ToString();
            if (hdnTransporterId.Value != "" && hdnTransporterId.Value != "0")
            {
                gvBillStatusDetail.DataSourceID = "DataSourceBillHoldStatusDetail";
                gvBillStatusDetail.DataBind();
                mpePopup.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "release")
        {
            hdnStatusId.Value = "5";
            lblPopup_Title.Text = "Total Bill Release - ";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnTransporterId.Value = commandArgs[0].ToString();
            lblTransporter_OtherStatus.Text = commandArgs[1].ToString();
            if (hdnTransporterId.Value != "" && hdnTransporterId.Value != "0")
            {
                gvBillStatusDetail.DataSourceID = "DataSourceReleaseStatusDetail";
                gvBillStatusDetail.DataBind();
                mpePopup.Show();
            }
        }
        else // memo prepare
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnTransporterId.Value = commandArgs[0].ToString();
            lblTransporter_Memo.Text = commandArgs[1].ToString();
            if (hdnTransporterId.Value != "" && hdnTransporterId.Value != "0")
            {
                mpeMemoStatus.Show();
                gvMemoStatusDetail.DataBind();
            }
        }
    }

    protected void gvTransBillDetail_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "BillStatus.aspx";
            DataFilter1.DataColumns = gvTransBillDetail.Columns;
            DataFilter1.FilterDataSource();
            gvTransBillDetail.DataBind();
            if (gvTransBillDetail.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Bill Status Summary!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "BillStatusSummary_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvTransBillDetail.AllowPaging = false;
        gvTransBillDetail.AllowSorting = false;
        gvTransBillDetail.Enabled = false;
        gvTransBillDetail.Caption = "Bill Status Summary On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "Bill Status.aspx";
        DataFilter1.FilterDataSource();
        gvTransBillDetail.DataBind();
        gvTransBillDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkbtnExportExcel_Click(object sender, EventArgs e)
    {
        if (hdnStatusId.Value != "" && hdnStatusId.Value != "0")
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + "BillStatus_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            int StatusId = Convert.ToInt32(hdnStatusId.Value);
            if (StatusId == 1)         // no of bill pending
            {
                gvBillStatusDetail.Caption = "No of Bill Pending Status Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                gvBillStatusDetail.DataSourceID = "DataSourceBillPending";
            }
            else if (StatusId == 3)    // cheque
            {
                gvBillStatusDetail.Caption = "Cheque Bill Status Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                gvBillStatusDetail.DataSourceID = "DataSourceChequeStatusDetail";
            }
            else if (StatusId == 4)    // hold bill
            {
                gvBillStatusDetail.Caption = "Hold Bill Status Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                gvBillStatusDetail.DataSourceID = "DataSourceBillHoldStatusDetail";
            }
            else // release
            {
                gvBillStatusDetail.Caption = "Release Bill Status Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                gvBillStatusDetail.DataSourceID = "DataSourceReleaseStatusDetail";
            }
            gvBillStatusDetail.DataBind();
            gvBillStatusDetail.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpePopup.Hide();
        hdnTransporterId.Value = "0";
    }

    protected void lnkbtnExportMemo_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + "MemoBillStatus_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvMemoStatusDetail.AllowPaging = false;
        gvMemoStatusDetail.AllowSorting = false;
        gvMemoStatusDetail.Columns[11].Visible = false;
        gvMemoStatusDetail.Columns[12].Visible = false;
        gvMemoStatusDetail.Columns[13].Visible = false;
        gvMemoStatusDetail.Columns[14].Visible = false;
        gvMemoStatusDetail.Columns[15].Visible = false;
        gvMemoStatusDetail.Columns[16].Visible = false;
        gvMemoStatusDetail.Columns[17].Visible = true;
        gvMemoStatusDetail.Caption = "Memo Bill Status Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        gvMemoStatusDetail.DataSourceID = "DataSourceMemoStatusDetail";
        gvMemoStatusDetail.DataBind();
        gvMemoStatusDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void imgbtnCancelMemo_Click(object sender, ImageClickEventArgs e)
    {
        mpeMemoStatus.Hide();
        hdnTransporterId.Value = "0";
    }

    #endregion

    protected void gvBillStatusDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0, dcTollCharges = 0, dcOtherCharges = 0, dcTotalAmount = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "BillAmount") != DBNull.Value)
            {
                dcFreightAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BillAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "VaraiAmount") != DBNull.Value)
            {
                dcVaraiExpenses = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VaraiAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "DetentionAmount") != DBNull.Value)
            {
                dcDetentionAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DetentionAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "EmptyContRcptCharges") != DBNull.Value)
            {
                dcEmptyContRecdCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "EmptyContRcptCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TollCharges") != DBNull.Value)
            {
                dcTollCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TollCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "OtherCharges") != DBNull.Value)
            {
                dcOtherCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OtherCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            ViewState["BillAmt"] = Convert.ToDecimal(ViewState["BillAmt"]) + dcFreightAmt;
            ViewState["VaraiAmt"] = Convert.ToDecimal(ViewState["VaraiAmt"]) + dcVaraiExpenses;
            ViewState["DetentionAmt"] = Convert.ToDecimal(ViewState["DetentionAmt"]) + dcDetentionAmt;
            ViewState["EmptyContReturnAmt"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcEmptyContRecdCharges;
            ViewState["TollCharges"] = Convert.ToDecimal(ViewState["TollCharges"]) + dcTollCharges;
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["OtherCharges"]) + dcOtherCharges;
            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmount;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[4].Text = "<b>" + Convert.ToDecimal(ViewState["BillAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[5].Text = "<b>" + Convert.ToDecimal(ViewState["DetentionAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[6].Text = "<b>" + Convert.ToDecimal(ViewState["VaraiAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[7].Text = "<b>" + Convert.ToDecimal(ViewState["EmptyContReturnAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[8].Text = "<b>" + Convert.ToDecimal(ViewState["TollCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[9].Text = "<b>" + Convert.ToDecimal(ViewState["OtherCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[10].Text = "<b>" + Convert.ToDecimal(ViewState["TotalAmt"]).ToString("#,##0.00") + "</b>";

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            e.Row.BackColor = System.Drawing.Color.FromName("#cbcbdc");
        }
    }

    protected void gvMemoStatusDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlPriority = (DropDownList)e.Row.FindControl("ddlPriority");
            if (ddlPriority != null)
            {
                if (DataBinder.Eval(e.Row.DataItem, "Priority") != DBNull.Value)
                {
                    ddlPriority.SelectedValue = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Priority"));
                }
            }
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

        // calculate total amount
        decimal dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0, dcTollCharges = 0, dcOtherCharges = 0, dcTotalAmount = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "BillAmount") != DBNull.Value)
            {
                dcFreightAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BillAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "VaraiAmount") != DBNull.Value)
            {
                dcVaraiExpenses = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VaraiAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "DetentionAmount") != DBNull.Value)
            {
                dcDetentionAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DetentionAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "EmptyContRcptCharges") != DBNull.Value)
            {
                dcEmptyContRecdCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "EmptyContRcptCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TollCharges") != DBNull.Value)
            {
                dcTollCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TollCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "OtherCharges") != DBNull.Value)
            {
                dcOtherCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OtherCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            ViewState["BillAmt"] = Convert.ToDecimal(ViewState["BillAmt"]) + dcFreightAmt;
            ViewState["VaraiAmt"] = Convert.ToDecimal(ViewState["VaraiAmt"]) + dcVaraiExpenses;
            ViewState["DetentionAmt"] = Convert.ToDecimal(ViewState["DetentionAmt"]) + dcDetentionAmt;
            ViewState["EmptyContReturnAmt"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcEmptyContRecdCharges;
            ViewState["TollCharges"] = Convert.ToDecimal(ViewState["TollCharges"]) + dcTollCharges;
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["OtherCharges"]) + dcOtherCharges;
            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmount;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[4].Text = "<b>" + Convert.ToDecimal(ViewState["BillAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[5].Text = "<b>" + Convert.ToDecimal(ViewState["DetentionAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[6].Text = "<b>" + Convert.ToDecimal(ViewState["VaraiAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[7].Text = "<b>" + Convert.ToDecimal(ViewState["EmptyContReturnAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[8].Text = "<b>" + Convert.ToDecimal(ViewState["TollCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[9].Text = "<b>" + Convert.ToDecimal(ViewState["OtherCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[10].Text = "<b>" + Convert.ToDecimal(ViewState["TotalAmt"]).ToString("#,##0.00") + "</b>";

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            e.Row.BackColor = System.Drawing.Color.FromName("#cbcbdc");
        }
    }
}