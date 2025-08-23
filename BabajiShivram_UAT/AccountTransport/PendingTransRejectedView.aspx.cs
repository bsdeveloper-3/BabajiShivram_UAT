using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AccountTransport_PendingTransRejectedView : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Payment Rejected";

            Session["TransPayIdTrack"]  = null;
            Session["TransPayId"]       = null;
            Session["TransReqId"]       = null;
            Session["TransporterID"]    = null;
        }

        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "PendingTransRejectedView.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        hdnInvoiceId.Value = "0";
        lblMessage.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strTransPayId = (string)e.CommandArgument;

            Session["TransPayIdTrack"] = strTransPayId;

            Response.Redirect("TransDetail.aspx"); ;
        }
        else if (e.CommandName.ToLower() == "cancel")
        {
            lblPopupName.Text = "Cancel Transporter Invoice";

            int PayRequestId = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = PayRequestId.ToString();

            DataView dvFundDetail = DBOperations.GetTransportFundRequest(PayRequestId);

            if (dvFundDetail.Table.Rows.Count > 0)
            {
               int statusID  = Convert.ToInt32(dvFundDetail.Table.Rows[0]["StatusId"]);

                if (statusID > 148 )
                {   
                    lblMessage.Text = "Invoice Cancellation Not Allowed!";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                    CancelModalPopupExtender.Hide();
                    btnCancelJob.Visible = false;

                }
                else
                {
                    btnCancelJob.Visible = true;

                    lblJobNumber.Text       =   dvFundDetail.Table.Rows[0]["JobRefNo"].ToString();
                    lblPaymentType1.Text    =   dvFundDetail.Table.Rows[0]["InvoiceTypeName"].ToString();
                    lblAmount1.Text         =   dvFundDetail.Table.Rows[0]["Amount"].ToString();
                    lblPaidTo1.Text         =   dvFundDetail.Table.Rows[0]["PaidTo"].ToString();
                    lblStatus1.Text         =   dvFundDetail.Table.Rows[0]["StatusName"].ToString();
                    lblCreatedBy1.Text      =   dvFundDetail.Table.Rows[0]["RequestBy"].ToString();

                    CancelModalPopupExtender.Show();
                }

            }

        }
    }

    protected void btnCancelJob_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        if (txtCancelReason.Text.Trim() != "")
        {
            int PayRequestId = Convert.ToInt32(hdnInvoiceId.Value);
            if (PayRequestId > 0)
            {                
                string strRemark = txtCancelReason.Text.Trim();

                Int32 StatusId = (Int32)EnumInvoiceStatus.InvoiceCancelled;

                int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblMessage.Text     = "Transporter Invoice Cancelled!";
                    lblMessage.CssClass = "success";
                    hdnInvoiceId.Value  = "0";
                    gvDetail.DataBind();
                }
                else if (result == 2)
                {
                    lblMessage.Text = "Invoice Already Cancelled!";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                    gvDetail.DataBind();
                }
                else
                {
                    lblMessage.Text = "System Error! Please try after sometime";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                }                
            }
        }
        else
        {
            lblError_CancelExp.Text = "Please Enter Reason for Cancellation!";
            lblError_CancelExp.CssClass = "errorMsg";
            txtCancelReason.Focus();
            CancelModalPopupExtender.Show();
        }
    }

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
            DataFilter1.FilterSessionID = "PendingTransRejected.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Transport_Invoice_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;
        gvDetail.Columns[1].Visible = false;
        gvDetail.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "PendingTransRejectedView.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}