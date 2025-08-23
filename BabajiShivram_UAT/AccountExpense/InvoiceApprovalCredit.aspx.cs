using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;

public partial class AccountExpense_InvoiceApprovalCredit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Approval-Credit Vendor";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceApprovalCredit.aspx");
        }

        if (!IsPostBack)
        {
            btnBack.PostBackUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            GetPaymentRequest(Convert.ToInt32(Session["InvoiceId"]));
        }
    }

    private void GetPaymentRequest(int InvoiceID)
    {
        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        int StatusId = 0;
        int ExpenseTypeId = 0;
        int BillType = 0; //  10 For Bank Check issued against Job and Payment Already Completed
        Boolean BilledStatus = false; // Job Billing Status

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);
            lblLTRefNo.Text = "LT" + InvoiceID.ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            hdnJobId.Value = dsDetail.Tables[0].Rows[0]["JobId"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblWeight.Text = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            lblContainerCount.Text = dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");

            BillType = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["BillType"]);
            hdnBillType.Value = dsDetail.Tables[0].Rows[0]["BillType"].ToString();

            BilledStatus = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsBilled"]);

            if (BilledStatus == true)
            {
                lblError.Text = "BILLED JOB!";
                     
                lblError.CssClass = "errorMsg";
            }

            if (hdnBillType.Value == "3")
            {
                fldVendorBuySell.Visible = true;
                fldVendorJobMargin.Visible = true;
                fldOverAllJobMargin.Visible = true;
                //fldPayment.Visible = true;

                //GetPendingInvoicePayment();
                // Credit Vendor Job Buy/Sell Detail

                DataSet dsJobProfitDetail = AccountExpense.GetInvoiceJobProfit(InvoiceID);

                DataSet dsBilledMargin = BillingOperation.FAGetJobExpenseBilled(lblJobNumber.Text);

                if (dsJobProfitDetail.Tables[0].Rows.Count > 0)
                {
                    txtVendorBuyValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorBuyValue"].ToString();
                    txtVendorSellValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorSellValue"].ToString();
                    txtCustomerBuyValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["CustomerBuyValue"].ToString();
                    txtCustomerSellValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["CustomerSellValue"].ToString();
                    txtJobProfitRemark.Text = dsJobProfitDetail.Tables[0].Rows[0]["Remark"].ToString();

                    // Job Margin

                    lblInvoiceValue1.Text = dsJobProfitDetail.Tables[0].Rows[0]["TaxAmount"].ToString();
                    lblSellingAmount1.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorSellValue"].ToString();
                    lblMargin.Text = dsJobProfitDetail.Tables[0].Rows[0]["JobMargin"].ToString();

                    if (dsJobProfitDetail.Tables[0].Rows[0]["PercentMargin"] != DBNull.Value)
                    {
                        decimal decPercentMargin = Convert.ToDecimal(dsJobProfitDetail.Tables[0].Rows[0]["PercentMargin"]);

                        //lblPercentMargin.Text = Convert.ToDecimal(dsJobProfitDetail.Tables[0].Rows[0]["PercentMargin"]).ToString("0.##");

                        if (decPercentMargin <= 0)
                        {
                            lblPercentMargin.Text = decPercentMargin.ToString("0.##") + "% LOSS";
                            lblPercentMargin.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblPercentMargin.Text = decPercentMargin.ToString("0.##") + "%";
                            lblPercentMargin.ForeColor = System.Drawing.Color.Green;
                        }
                    }

                }
                if (dsBilledMargin.Tables[0].Rows.Count > 0)
                {
                    // Bill Margin

                    int TotalRow = dsBilledMargin.Tables[0].Rows.Count;

                    Decimal decTotalDebit = 0m, decTotalCredit = 0m;
                    Decimal decOverAllMargin = 0m, decOverAllMarginPercent = 0m;
                    // Last Row - Total Summary
                    decTotalDebit = Convert.ToDecimal(dsBilledMargin.Tables[0].Rows[TotalRow - 1]["Debit"]) + (Convert.ToDecimal(lblInvoiceValue1.Text)); ;
                    decTotalCredit = Convert.ToDecimal(dsBilledMargin.Tables[0].Rows[TotalRow - 1]["Credit"]);

                    decOverAllMargin = decTotalCredit - decTotalDebit;

                    if (decTotalDebit > 0 && decOverAllMargin > 0)
                    {
                        decOverAllMarginPercent = (decOverAllMargin * 100) / decTotalDebit;
                    }
                    else if(decTotalDebit > 0 && decTotalCredit == 0)
                    {
                        decOverAllMarginPercent = (decOverAllMargin * 100) / decTotalDebit;
                    }
                    else if (decTotalDebit == 0 && decTotalCredit == 0)
                    {
                        // DO NOTHING
                    }
                    else if (decTotalDebit > 0  && decOverAllMargin <0)
                    {
                        decOverAllMarginPercent = (decOverAllMargin * 100) / decTotalDebit;
                    }

                    if (decOverAllMargin <=0)
                    {
                        lblBillMarginPercent.Text = decOverAllMarginPercent.ToString("0.##") + "% LOSS";
                        lblBillMarginPercent.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblBillMarginPercent.Text = decOverAllMarginPercent.ToString("0.##") + "%";
                        lblBillMarginPercent.ForeColor = System.Drawing.Color.Green;
                    }

                    lblTotalCost.Text = decTotalDebit.ToString();
                    lblBilledAmount.Text = decTotalCredit.ToString();
                    lblBillMargin.Text = decOverAllMargin.ToString("0.##");
                    
                }

            }

            if (dsDetail.Tables[0].Rows[0]["ExpenseTypeId"] != DBNull.Value)
            {
                ExpenseTypeId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ExpenseTypeId"]);

                lblExpenseType.Text = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["DutyAmount"] != DBNull.Value)
            {
                lblDutyAmount.Text = dsDetail.Tables[0].Rows[0]["DutyAmount"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["IsInterest"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsInterest"]) == true)
                {
                    tdInterestAmt.Visible = true;
                    lblInterestAmount.Text = dsDetail.Tables[0].Rows[0]["InterestAmount"].ToString();
                }
            }
            if (dsDetail.Tables[0].Rows[0]["AssessableValue"] != DBNull.Value)
            {
                lblAssessableValue.Text = dsDetail.Tables[0].Rows[0]["AssessableValue"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["IGSTAmt"] != DBNull.Value)
            {
                lblIGSTAmount.Text = dsDetail.Tables[0].Rows[0]["IGSTAmt"].ToString();
            }

            lblVendorGSTIN.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNo"].ToString();
            lblPAN.Text = dsDetail.Tables[0].Rows[0]["VendorPAN"].ToString();
            lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblVendorType.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNType"].ToString();
            lblCreditTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();

            lblRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();

            lblInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            lblTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            lblTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();

            lblAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            lblAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            lblBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();


            lblRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();

            if (dsDetail.Tables[0].Rows[0]["InvoiceAmount"] != DBNull.Value)
            {
                lblTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["TaxAmount"] != DBNull.Value)
            {
                lblTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["CurrencyName"] != DBNull.Value)
            {
                lblInvoiceCurrency.Text = dsDetail.Tables[0].Rows[0]["CurrencyName"].ToString();
            }
           
            if (dsDetail.Tables[0].Rows[0]["NetPayable"] != DBNull.Value)
            {
                lblNetPayable.Text = dsDetail.Tables[0].Rows[0]["NetPayable"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            {
                lblPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["dtDate"] != DBNull.Value)
            {
                lblPatymentRequestDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

            // Invoice Charge Detail

            //if(dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1")
            //{
            //    fldInvoiceitem.Visible = true;
            //}
            //else
            //{
            //    fldInvoiceitem.Visible = false;
            //}

            // Vendor Detail
            if (dsDetail.Tables[0].Rows[0]["PaymentTerms"] != null)
            {
                lblCreditTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();
            }

            string strVendorCode = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            DataSet dsVendor = AccountExpense.GetFAVendorByCode(strVendorCode);

            if (dsVendor.Tables[0].Rows.Count > 0)
            {
                if (dsVendor.Tables[0].Rows[0]["OutStandingAmount"] != null)
                {
                    lblCurrentOutstanding.Text = dsVendor.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                }
            }

            // Billing Party Detail

            if (dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"] != null)
            {
                DataSet dsBillingParty = null;

                string strBillingGSTIN = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();

                if (strBillingGSTIN != "")
                {
                    dsBillingParty = AccountExpense.GetFAVendorByGSTIN(strBillingGSTIN);
                }
                else if (dsDetail.Tables[0].Rows[0]["ConsigneeCode"] != null)
                {
                    string strBillingCode = dsDetail.Tables[0].Rows[0]["ConsigneeCode"].ToString();

                    if (strBillingCode != "")
                    {
                        dsBillingParty = AccountExpense.GetFAVendorByCode(strBillingGSTIN);
                    }
                }

                lblBillingGSTN.Text = strBillingGSTIN;
                lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();

                if (dsBillingParty != null)
                {
                    if (dsBillingParty.Tables[0].Rows.Count > 0)
                    {
                        if (dsBillingParty.Tables[0].Rows[0]["girno"] != null)
                        {
                            lblBillingPAN.Text = dsBillingParty.Tables[0].Rows[0]["girno"].ToString();
                        }
                        if (dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"] != null)
                        {
                            lblCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                        }

                    }
                }
            }

            // TDS Detail

            if (dsDetail.Tables[0].Rows[0]["TransactionTypeName"] != DBNull.Value)
            {
                lblTransactionTypeName.Text = dsDetail.Tables[0].Rows[0]["TransactionTypeName"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["IsNoITC"] != DBNull.Value)
            {
                if (dsDetail.Tables[0].Rows[0]["IsRIM"].ToString() == "True")
                {
                    // For RIM - NoITC Not Requried
                    // NO-ITC Not Required

                    chkNoITC.Checked = false;
                    chkNoITC.Visible = false;
                }
                else
                {
                    chkNoITC.Checked = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsNoITC"]);
                }
            }

            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["TDSApplicable"]) == true)
            {
                fldTDSItem.Visible = true;
                GridViewTDS.Visible = true;
                lblTDSApplicable.Text = "Yes";
                lblTDSRate.Text = dsDetail.Tables[0].Rows[0]["TDSRate"].ToString();

                if (dsDetail.Tables[0].Rows[0]["TDSRate"] != DBNull.Value)
                {
                    if (dsDetail.Tables[0].Rows[0]["TDSRateType"] != DBNull.Value)
                    {
                        if (dsDetail.Tables[0].Rows[0]["TDSRateType"].ToString() == "1")
                        {
                            lblTDSRateType.Text = "Standard";
                        }
                        else if (dsDetail.Tables[0].Rows[0]["TDSRateType"].ToString() == "2")
                        {
                            lblTDSRateType.Text = "Concessional";

                        }
                    }
                    else
                    {
                        lblTDSRateType.Text = "Standard";
                    }
                }
                if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    lblTotalTDS.Text = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
                }
            }
        }

        ShowHideFormDetails(StatusId, ExpenseTypeId);
    }

    private void GetPendingInvoicePayment()
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        DataSet dsPaymentDetail = AccountExpense.GetInvoicePendingPayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            txtPayAmount.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

            int PaymentTypeId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"]);
            int BabajiBankID = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankId"]);
            int BankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"]);

            // Payment Mode -- Cash, Bank Account

            // Fill Bank/Cash Code
            if (PaymentTypeId == 1) // Cash
            {
                AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2); // Cash Book
            }
            if (PaymentTypeId == 90) // Security Deposit
            {
                AccountExpense.FillBankBookByType(ddBabajiBankAccount, 3); // Cash Book
            }
            else
            {
                // File Bank Name MS

                AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name

                // Fill Account No By BankId

                AccountExpense.FillBankAccountByBankId(ddBabajiBankAccount, BabajiBankID);

                ddBabajiBankName.SelectedValue = BabajiBankID.ToString();
            }

            if (dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"] != DBNull.Value)
            {
                ddBabajiBankAccount.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"].ToString();
                ddBabajiBankAccount.SelectedValue = BankAccountId.ToString();
            }

            if (dsPaymentDetail.Tables[0].Rows[0]["IsFullPayment"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsPaymentDetail.Tables[0].Rows[0]["IsFullPayment"]) == true)
                {
                    rblPayment.SelectedValue = "1";
                }
                else
                {
                    rblPayment.SelectedValue = "0";
                }
            }

            // Fund Transfer From live Tracking 

            if (dsPaymentDetail.Tables[0].Rows[0]["IsFundTransFromAPI"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsPaymentDetail.Tables[0].Rows[0]["IsFundTransFromAPI"]) == true)
                {
                    rblFundTransferFromLiveTracking.SelectedValue = "1";
                    rblFundTransferFromLiveTracking.Enabled = true;
                }
                else
                {
                    rblFundTransferFromLiveTracking.SelectedValue = "0";
                    rblFundTransferFromLiveTracking.Enabled = false;
                }
            }

        }
    }
    private void ShowHideFormDetails(int StatusId, int ExpenseTypeId)
    {
        //if (ExpenseTypeId == 1 || ExpenseTypeId == 9)
        //{
        //    fldVendor.Visible = false;
        //}
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 115, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Approved!";

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Already Approved!";

            Response.Redirect("AccountSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnHold_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 111, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice On Hold!";

            SendHoldRejectMail(InvoiceID, "Management On Hold", strRemark);

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Updated!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Already On Hold!";

            Response.Redirect("AccountSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 112, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Rejected!";

            SendHoldRejectMail(InvoiceID, "Management Reject", strRemark);

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Already Rejected!";

            Response.Redirect("AccountSuccess.aspx");

        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingInvoiceApproval.aspx");
    }
    protected void lnkBJVNo_Click(object sender, EventArgs e)
    {
        string strBJVNo = lblJobNumber.Text.Trim();

        DataSet dsBJVDetail = BillingOperation.FAGetJobExpenseBilled(strBJVNo);

        if (dsBJVDetail.Tables[0].Rows.Count > 0)
        {
            gvBJVDetail.DataSource = dsBJVDetail;
            gvBJVDetail.DataBind();
        }
        else
        {
            gvBJVDetail.DataSource = null;
            gvBJVDetail.DataBind();
        }

        ModalPopupExtender2.Show();
    }
    protected void btnCancelBJVPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }

    protected void gvInvoiceHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "StatusName").ToString().Contains("Reject"))
            {
                e.Row.BackColor = System.Drawing.Color.Coral;
                e.Row.ForeColor = System.Drawing.Color.White;

            }
        }
    }

    #region Hold Reject Email

    private bool SendHoldRejectMail(int InvoiceID, string strStatusName, string strRemark)
    {
        string MessageBody = "", strCCEmail = "", strSubject = "";

        bool bEmailSucces = false;
        StringBuilder strbuilder = new StringBuilder();

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        string strJobNumber = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
        string strCustomer = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
        string strConsignee = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();

        string strExpenseType = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

        string strVendorName = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();

        string strInvoiceNo = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();

        string strTotalInvoiceValue = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();

        string strCreatedByEmail = "amit.bakshi@BabajiShivram.com";

        if (dsDetail.Tables[0].Rows[0]["CreatedByEmail"] != DBNull.Value)
        {
            strCreatedByEmail = dsDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
        }

        string EmailContent = "";

        try
        {

            try
            {
                string strFileName = System.Web.HttpContext.Current.Server.MapPath("~") + "//EmailTemplate/EmailVendorPaymentReject.txt";

                //  string strFileName = "../EmailTemplate/EmailVendorPaymentReject.txt";

                EmailContent = File.ReadAllText(strFileName);

                //StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                //sr = File.OpenText(Server.MapPath(strFileName));
                //EmailContent = sr.ReadToEnd();
                //sr.Close();
                //sr.Dispose();
                //GC.Collect();
            }
            catch (Exception ex)
            {
                return bEmailSucces;
            }

            MessageBody = EmailContent.Replace("@JobRefNo", strJobNumber);

            MessageBody = MessageBody.Replace("@Customer", strCustomer);
            MessageBody = MessageBody.Replace("@Consignee", strConsignee);
            MessageBody = MessageBody.Replace("@ExpenseType", strExpenseType);
            MessageBody = MessageBody.Replace("@VendorName", strVendorName);
            MessageBody = MessageBody.Replace("@InvoiceNo", strInvoiceNo);
            MessageBody = MessageBody.Replace("@TotalInvoiceAmount", strTotalInvoiceValue);
            MessageBody = MessageBody.Replace("@StatusName", strStatusName);
            MessageBody = MessageBody.Replace("@Remarks", strRemark);
            MessageBody = MessageBody.Replace("@UserName", LoggedInUser.glEmpName);

            ///////////////////////////////////////////////////////////////////////////////

            try
            {
                strSubject = "Vendor Payment " + strStatusName + "/" + strJobNumber + "/ " + strInvoiceNo + "/ " + strConsignee + " / " + strExpenseType;

                string EmailBody = MessageBody;

                bEmailSucces = EMail.SendMail(LoggedInUser.glUserName, strCreatedByEmail, strSubject, EmailBody, "");
            }
            catch (System.Exception exn)
            {
                bEmailSucces = false;
                return bEmailSucces;
            }
        }
        catch (Exception enk)
        {
            return false;
        }

        return bEmailSucces;
    }

    #endregion

    #region Document Download

    protected void gvDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                DownloadDocument(strDocpath);
            }


        }
        else if (e.CommandName.ToLower() == "view")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                ViewDocument(strDocpath);
            }
        }
    }
    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
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
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    
    #region Bill Detail

    protected void gvBillDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(Convert.ToInt32(hdnJobId.Value), BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;

                DownloadDocument(strFilePath);
            }
            else
            {
                lblError.Text = "Bill Document Not Uploaded!";
                lblError.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(Convert.ToInt32(hdnJobId.Value), BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;

                ViewDocument(strFilePath);
            }
            else
            {
                lblError.Text = "Bill Document Not Uploaded!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void gvBillDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "DocId") != DBNull.Value)
            {
                int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DocId"));
                if (DocId == 0)
                {
                    LinkButton lnkViewDoc = (LinkButton)e.Row.FindControl("lnkBillView");
                    LinkButton lnkBillDownload = (LinkButton)e.Row.FindControl("lnkBillDownload");

                    if (lnkViewDoc != null)
                    {
                        lnkViewDoc.Visible = false;
                    }
                    if (lnkBillDownload != null)
                    {
                        lnkBillDownload.Visible = false;
                    }

                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Bill Document Not Uploaded";
                }
            }
        }
    }

    #endregion
    #endregion
}