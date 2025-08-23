using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;

public partial class AccountExpense_PendingInvoiceMemo : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["InvoicePayId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Memo Prepration";

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;

            }
        }

        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "PendingInvoiceMemo.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strInvoicePayId = (string)e.CommandArgument;

            Session["InvoicePayId"] = strInvoicePayId;

            //Response.Redirect("TransPaymentAPI.aspx"); ;
        }

    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsBilled")) == true)
            //{
            //    e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
            //    e.Row.ToolTip = "Billed Job";
            //}

            if (DataBinder.Eval(e.Row.DataItem, "lStatus") != DBNull.Value)
            {
                int lStatus = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "lStatus").ToString());
                string strStatusRemark = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();

                if (lStatus == (int)EnumInvoiceStatus.PaymentOnHold) // Payment On Hold - 
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    e.Row.ToolTip = strStatusRemark;
                    CheckBox chk1 = (CheckBox)e.Row.FindControl("chkjoball");

                    chk1.Enabled = false;
                    chk1.Checked = false;
                    chk1.Visible = false;
                }
                else if (lStatus != (int)EnumInvoiceStatus.L2Approved) // Audit L2 Complete - 
                {
                    if (lStatus != (int)EnumInvoiceStatus.MemoCancelled) // Memo Cancelled Allow to Create Memo Again - 
                    {
                        e.Row.BackColor = System.Drawing.Color.Red;
                        e.Row.ToolTip = strStatusRemark;
                        CheckBox chk1 = (CheckBox)e.Row.FindControl("chkjoball");

                        chk1.Enabled = false;
                        chk1.Checked = false;
                        chk1.Visible = false;
                    }
                }
            }
        }
    }
    protected void btnMemoGeneration_Click(object sender, EventArgs e)
    {
        int MemoID = 0;

        int TotalBatchRequest = 0;
        int TotalBatchSuccess = 0;
        decimal TotalMemoAmount = 0;

        lblMessage.Text = "Memo Generation Initiated";

        int RefTotalRequest = 0;
        int VendorId = CheckCommonVendor(ref RefTotalRequest, ref TotalMemoAmount);

        if (VendorId > 0)
        {
            MemoID = AccountExpense.AddInvoicePayMemo(VendorId, TotalMemoAmount, RefTotalRequest, LoggedInUser.glUserId);

            if (MemoID > 0)
            {
                // Add Memo Status
                string remark = "Memo Generated";
                AccountExpense.AddInvoiceMemoStatus(MemoID, (int)EnumInvoiceStatus.MemoPrepared, remark, LoggedInUser.glUserId);

                foreach (GridViewRow gvr1 in gvDetail.Rows)
                {
                    if (gvr1.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                        if (chk1.Checked)
                        {
                            TotalBatchRequest = TotalBatchRequest + 1;

                            int RequestId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Value);

                            int Output =  AccountExpense.AddInvoiceMemoDetail(RequestId, 0, MemoID, LoggedInUser.glUserId);

                            if (Output == 0)
                            {
                                TotalBatchSuccess = TotalBatchSuccess + 1;
                            }
                        }
                    }
                }//END_ForEach

                if (TotalBatchRequest == 0)
                {
                    lblMessage.Text = "Please select Invoices For Memo!";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "Memo Generated for " + TotalBatchSuccess.ToString() + " Out Of " + TotalBatchRequest.ToString() + " Invoices!";
                    lblMessage.CssClass = "success";
                }
            }
            else
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }
        else
        {
            lblMessage.Text = "Please Select Payment for One Vendor Only!";
            lblMessage.CssClass = "errorMsg";
        }
    }
    private int CheckCommonVendor(ref int RefTotalRequest, ref decimal TotalMemoAmount)
    {
        TotalMemoAmount = 0;

        int VendorId = 0;

        string strRequestIdList = "";

        foreach (GridViewRow gvr1 in gvDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    Label ObjlblToPay = (Label)gvr1.Cells[1].FindControl("lblToPay");

                    TotalMemoAmount = TotalMemoAmount + Convert.ToDecimal(ObjlblToPay.Text.Trim());

                    RefTotalRequest = RefTotalRequest + 1;

                    strRequestIdList = strRequestIdList + gvDetail.DataKeys[gvr1.RowIndex].Values[0].ToString() + ",";

                    if (VendorId == 0)
                    {
                        VendorId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Values[1].ToString());
                    }
                    else if (VendorId != Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Values[1].ToString()))
                    {
                        VendorId = 0;

                        // Different Vendor Selected For Covering Letter Generation
                        lblMessage.Text = "Please Select Payment for One Vendor Only!";
                        lblMessage.CssClass = "errorMsg";

                        // break;
                    }
                }
            }
        }//END_ForEach

        return VendorId;

    }

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
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
            DataFilter1.FilterSessionID = "PendingInvoiceMemo.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
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
        string strFileName = "Pending_For_Memo_Prepare_" + DateTime.Now.ToString("dd/MM/yyyy") + ".xls";

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

        DataFilter1.FilterSessionID = "PendingInvoiceMemo.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}