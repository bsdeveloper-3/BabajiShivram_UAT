using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class AccountExpense_PendingChequeInvoice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Session["ChequeId"] = null;
            Session["InvoiceId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Issued Cheque/Invoice Pending";

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;

            }
        }
        //
        DataFilter1.DataSource = ChequeSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "PendingChequeInvoice.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        txtCancelReason.Text = "";
        hdnChequeId.Value = "0";
        hdnInvoiceId.Value = "0";
        if (e.CommandName.ToLower() == "select")
        {
            // string strChequeId = (string)e.CommandArgument;

            int rowIndex = int.Parse(e.CommandArgument.ToString());

            string strChequeId = Convert.ToInt32(this.gvDetail.DataKeys[rowIndex][0]).ToString();
            String strInvoiceId = Convert.ToInt32(this.gvDetail.DataKeys[rowIndex][1]).ToString();

            if (strInvoiceId == "0")
            {
                Session["ChequeId"] = strChequeId;
                Response.Redirect("InvoiceCheque.aspx"); ;
            }
            else
            {
                Session["InvoiceId"] = strInvoiceId;
                Session["ChequeId"] = strChequeId;

                Response.Redirect("InvoiceChequeEdit.aspx"); ;

            }
        }
        else if (e.CommandName.ToLower() == "cancel")
        {            
            hdnChequeId.Value   = "0";
            hdnInvoiceId.Value  = "0";
            int rowIndex = int.Parse(e.CommandArgument.ToString());

            hdnChequeId.Value   = this.gvDetail.DataKeys[rowIndex][0].ToString();
            hdnInvoiceId.Value  = this.gvDetail.DataKeys[rowIndex][1].ToString();

            if (hdnInvoiceId.Value == "0" && hdnChequeId.Value != "0")
            {
                lblPopupNameExp.Text = "Cancel Blank Cheque";
                btnCancelJob.Text = "Cancel Cheque";

                DataSet dsChequeDetail = AccountExpense.GetIssuedCheckDetail(Convert.ToInt32(hdnChequeId.Value));

                if (dsChequeDetail.Tables.Count > 0)
                {
                    if (dsChequeDetail.Tables[0].Rows.Count > 0)
                    {
                        hdnChequeId.Value       = dsChequeDetail.Tables[0].Rows[0]["lid"].ToString();
                        lblJobNumber.Text       = dsChequeDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
                        lblConsigneeName.Text   = dsChequeDetail.Tables[0].Rows[0]["Consignee"].ToString();
                        lblChequeNo.Text        = dsChequeDetail.Tables[0].Rows[0]["ChequeNo"].ToString();
                        lblChequeDate.Text      = Convert.ToDateTime(dsChequeDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyy");
                        lblBankName.Text        = dsChequeDetail.Tables[0].Rows[0]["BankName"].ToString();
                        lblBankAccountName.Text = dsChequeDetail.Tables[0].Rows[0]["AccountName"].ToString();
                        lblCreatedBy.Text       = dsChequeDetail.Tables[0].Rows[0]["CreatedBy"].ToString();

                        lblPaidTo.Text = "";
                        lblRemark.Text = "";

                        CancelModalPopupExp.Show();
                    }
                    else
                    {
                        lblMessage.Text = "Cheque Details Not Found!";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblMessage.Text = "Cheque Details Not Found!";
                    lblMessage.CssClass = "errorMsg";
                }
            }
            else
            {
                lblPopupNameExp.Text = "Cancel Invoice";
                btnCancelJob.Text = "Cancel Invoice";

                DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(Convert.ToInt32(hdnInvoiceId.Value));
                DataSet dsChequeDetail = AccountExpense.GetIssuedCheckDetail(Convert.ToInt32(hdnChequeId.Value));

                if (dsGetPaymentDetails.Tables.Count > 0)
                {
                    if (dsGetPaymentDetails.Tables[0].Rows[0]["ProformaInvoiceId"] != null)
                    {
                        if (Convert.ToInt32(dsGetPaymentDetails.Tables[0].Rows[0]["ProformaInvoiceId"]) > 0)
                        {
                            lblMessage.Text = "Proforma To Final Invoice not allowed for Cancellation";
                            lblMessage.CssClass = "errorMsg";
                            hdnInvoiceId.Value = "0";
                            hdnChequeId.Value = "0";
                            CancelModalPopupExp.Hide();

                            return;
                        }                        
                    }

                    // Get Cheque Detail
                    //lblConsigneeName.Text = dsChequeDetail.Tables[0].Rows[0]["Consignee"].ToString();
                    lblChequeNo.Text = dsChequeDetail.Tables[0].Rows[0]["ChequeNo"].ToString();
                    lblChequeDate.Text = Convert.ToDateTime(dsChequeDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyy");
                    lblBankName.Text = dsChequeDetail.Tables[0].Rows[0]["BankName"].ToString();
                    lblBankAccountName.Text = dsChequeDetail.Tables[0].Rows[0]["AccountName"].ToString();

                    // Get Invoice Detail
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                    lblAmount.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                    lblPaidTo.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();

                    lblRemark.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                    lblCreatedBy.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();

                    CancelModalPopupExp.Show();
                }

            }

        }
    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "InvoiceId") != DBNull.Value)
            {
                int InvoiceId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "InvoiceId").ToString());
                string strRemark = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();

                if (InvoiceId > 0) // Invoice Rejected  
                {
                    e.Row.BackColor = System.Drawing.Color.Red;  //LightSalmon;    
                    e.Row.ToolTip = strRemark;

                    LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");

                    if (lnkCancel != null)
                    {
                        lnkCancel.Text = "Cancel Invoice";
                        lnkCancel.Visible = true;
                    }
                }
            }
            if (LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 13478 || LoggedInUser.glUserId == 12158 || LoggedInUser.glUserId == 14017)
            {
                LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");

                if (lnkCancel != null)
                {
                    lnkCancel.Visible = true;
                }
            }

        }
    }
    protected void btnCancelJob_Click(object sender, EventArgs e)
    {
        if (txtCancelReason.Text.Trim() != "")
        {
            int InvoiceID       = Convert.ToInt32(hdnInvoiceId.Value);
            int ChequeID        = Convert.ToInt32(hdnChequeId.Value);
            int ChequeNo        = 0;

            Int32.TryParse(lblChequeNo.Text.Trim(), out ChequeNo);

            if (InvoiceID > 0 && ChequeID>0)
            {                
                int result = AccountExpense.AddInvoiceStatus(InvoiceID, 105, txtCancelReason.Text.Trim(), LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblMessage.Text = "Invoice Cancelled!";
                    lblMessage.CssClass = "success";
                    hdnInvoiceId.Value = "0";
                    hdnChequeId.Value = "0";
                    gvDetail.DataBind();
                }
                else if (result == 2)
                {
                    lblMessage.Text = "Invoice Already Cancelled!";
                    lblMessage.CssClass = "errorMsg";
                    gvDetail.DataBind();
                }
                else
                {
                    lblMessage.Text = "System Error! Please try after sometime";
                    lblMessage.CssClass = "errorMsg";
                }                
            }
            else if(InvoiceID == 0 && ChequeID > 0)
            {                 
                int result2 = AccountExpense.CancelBankCheque(ChequeID, ChequeNo, txtCancelReason.Text.Trim(), LoggedInUser.glUserId);
                
                if (result2 == 0)
                {
                    lblMessage.Text = "Cheque Detail Cancelled!";
                    lblMessage.CssClass = "success";
                    hdnInvoiceId.Value = "0";
                    hdnChequeId.Value = "0";
                    gvDetail.DataBind();
                }
                else if (result2 == 2)
                {
                    lblMessage.Text = "Cheque details not Found! ";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                    hdnChequeId.Value = "0";
                    gvDetail.DataBind();
                }
                else if (result2 == 3)
                {
                    lblMessage.Text = "Error:Cheque Already Cleared! ";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                    hdnChequeId.Value = "0";
                    gvDetail.DataBind();
                }
                else if(result2 == 4)
                {
                    lblMessage.Text = "Error:Cheque/Invoice detail already submitted! ";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                    hdnChequeId.Value = "0";
                    gvDetail.DataBind();
                }
                else
                {
                    lblMessage.Text = "System Error! Please try after sometime ";
                    lblMessage.CssClass = "errorMsg";
                    hdnInvoiceId.Value = "0";
                    hdnChequeId.Value = "0";
                    gvDetail.DataBind();
                }

            }
        }
        else
        {
            lblError_CancelExp.Text = "Please enter reason for Cancellation!";
            lblError_CancelExp.CssClass = "errorMsg";
            txtCancelReason.Focus();
            CancelModalPopupExp.Show();
        }
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
            DataFilter1.FilterSessionID = "PendingChequeInvoice.aspx";
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
        string strFileName = "Pending_Cheque_Invoice" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvDetail.Columns[gvDetail.Columns.Count - 1].Visible = false;

        DataFilter1.FilterSessionID = "PendingChequeInvoice.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        //-- gvJobDetail.DataSourceID = "PendingIGMSqlDataSource";
        //-- gvJobDetail.DataBind();
        // BindGridData();
        //gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //// gvJobDetail.HeaderRow.Cells[0].Visible = false;
        //for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //{
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //}

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}