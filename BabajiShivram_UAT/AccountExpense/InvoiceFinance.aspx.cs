using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BankAPI.YesBank;
public partial class AccountExpense_InvoiceFinance : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Audit - L2";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinance.aspx");
        }

        if (!IsPostBack)
        {
            GetPaymentRequest();

            GetPendingInvoicePayment();
        }
        else
        {
            // Check Invoice Status Change
            bool isStatusChanged = CheckInvoiceStatusChange();

            if (isStatusChanged == true)
            {
                lblError.Text = "Invoice details already updated! Please Check Status History";
                lblError.CssClass = "errorMsg";

                Session["Error"] = lblError.Text;

                Session["InvoiceId"] = null;

                Response.Redirect("AccountSuccess.aspx");
            }

        }
    }
    private void GetPaymentRequest()
    {
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinance.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        int StatusId = 0;
        int ExpenseTypeId = 0;
        Boolean BilledStatus = false; // Job Billing Status

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);
            hdnStatusId.Value = StatusId.ToString();

            lblLTRefNo.Text = "LT" + InvoiceID.ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            hdnJobId.Value = dsDetail.Tables[0].Rows[0]["JobId"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblContainerCount.Text = dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            lblWeight.Text = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");
            hdnInvoiceType.Value = dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString();

            hdnBillType.Value = dsDetail.Tables[0].Rows[0]["BillType"].ToString();
            hdnInvoiceMode.Value = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            BilledStatus = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsBilled"]);

            if (BilledStatus == true)
            {
                lblError.Text = "BILLED JOB!";

                lblError.CssClass = "errorMsg";
            }
            if (hdnBillType.Value == "3")
            {
                fldVendorBuySell.Visible = true;

                // Credit Vendor Job Buy/Sell Detail

                DataSet dsJobProfitDetail = AccountExpense.GetInvoiceJobProfit(InvoiceID);

                if (dsJobProfitDetail.Tables[0].Rows.Count > 0)
                {
                    txtVendorBuyValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorBuyValue"].ToString();
                    txtVendorSellValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorSellValue"].ToString();
                    txtCustomerBuyValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["CustomerBuyValue"].ToString();
                    txtCustomerSellValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["CustomerSellValue"].ToString();
                    txtJobProfitRemark.Text = dsJobProfitDetail.Tables[0].Rows[0]["Remark"].ToString();
                }
            }
            if (hdnInvoiceMode.Value == "2") // Credit Note
            {
                fldPayment.Visible = false;
                fldPaymentHistory.Visible = true;
                btnSavePayment.Visible = false;
                //RFVVendorBank.Enabled = false;
                //   ddlPaymentType.SelectedValue = "2";
                ddlPaymentType.Enabled = false;

                lblError.Text = "Credit Note ! Payment not required!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Credit Note", "alert('" + lblError.Text + "');", true);
            }
            if (hdnBillType.Value == "10") // Vendor Cheque Already Issued against Job
            {
                btnSavePayment.Visible = false;
                fldPayment.Visible = false;
                fldPaymentHistory.Visible = true;
                RFVVendorBank.Enabled = false;
                ddlPaymentType.SelectedValue = "2";
                ddlPaymentType.Enabled = false;

                fldPaymentHistory.Visible = true;
                lblError.Text = "Vendor Cheque Already Issued! Payment not required!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Blank Cheque Payment", "alert('" + lblError.Text + "');", true);
            }
            else if (hdnBillType.Value == "11") // PD Account Payment Not Reqired
            {
                btnSavePayment.Visible = false;
                fldPayment.Visible = false;
                fldPaymentHistory.Visible = true;
                RFVVendorBank.Enabled = false;
                ddlPaymentType.SelectedValue = "2";
                ddlPaymentType.Enabled = false;

                fldPaymentHistory.Visible = true;
                lblError.Text = "PD Account! Payment Not Required From Live Tracking!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "PD Account Payment", "alert('" + lblError.Text + "');", true);
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
            lblAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            lblAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            lblBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();


            if (dsDetail.Tables[0].Rows[0]["VendorId"] != DBNull.Value)
            {
                int VendorId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["VendorId"]);

                AccountExpense.FillVendorBank(ddVendorBank, VendorId);
            }

            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["OtherDeduction"] != DBNull.Value)
            {
                lblOtherDeduction.Text = dsDetail.Tables[0].Rows[0]["OtherDeduction"].ToString();
            }
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
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                lblExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["NetPayable"] != DBNull.Value)
            {
                hdnNetPayableAmount.Value = dsDetail.Tables[0].Rows[0]["NetPayable"].ToString();
                lblNetPayable.Text = dsDetail.Tables[0].Rows[0]["NetPayable"].ToString();
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

            if (dsDetail.Tables[0].Rows[0]["PaymentTypeId"] != DBNull.Value)
            {
                // ddlPaymentType.SelectedValue = dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            }

            // Vendor Detail
            string strVendorCode = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            //DataSet dsVendor = AccountExpense.GetFAVendorByCode(strVendorCode);

            //if (dsVendor.Tables[0].Rows.Count > 0)
            //{
            //    if (dsVendor.Tables[0].Rows[0]["BKNAME"] != null)
            //    {
            //        lblBankName.Text = dsVendor.Tables[0].Rows[0]["BKNAME"].ToString();
            //    }
            //    if (dsVendor.Tables[0].Rows[0]["BKACNO"] != null)
            //    {
            //        lblBankAccountNo.Text = dsVendor.Tables[0].Rows[0]["BKACNO"].ToString();
            //    }
            //    if (dsVendor.Tables[0].Rows[0]["BKIFSC"] != null)
            //    {
            //        lblBankIFSC.Text = dsVendor.Tables[0].Rows[0]["BKIFSC"].ToString();
            //    }
            //}

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
                        dsBillingParty = AccountExpense.GetFAVendorByCode(strBillingCode);
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
                        //if (dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"] != null)
                        //{
                        //    lblCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                        //}

                    }
                }
            }

            // TDS Detail

            if(dsDetail.Tables[0].Rows[0]["TransactionTypeName"] != DBNull.Value)
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

            // Proforma Invoice Detail against Final Invoice

            if (dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1") // Final)
            {
                if (dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"] != DBNull.Value)
                {
                    int ProformaInvoiceId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"]);

                    if (ProformaInvoiceId > 0)
                    {
                        // Show Proforma Invoice Detail

                        GetProformaPaymentRequest(ProformaInvoiceId);

                        // Proforma To Final  - Payment detail Not Required

                        fldPaymentHistory.Visible = false;

                    }
                }
            }

            // Performa Invoice -- Invoice Charge and RCM Not Required

            // RCM Detail
            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["RCMApplicable"]) == true)
            {
                fldRCMItem.Visible = true;
                GridViewRCM.Visible = true;
                lblRCMYes.Text = "Yes";
                lblRCMRate.Text = dsDetail.Tables[0].Rows[0]["RCMRate"].ToString();
                lblRCMTotalAmount.Text = dsDetail.Tables[0].Rows[0]["RCMTotalAmount"].ToString();
            }
        }
    }
    private void GetPendingInvoicePayment()
    {
        hdnPaymentId.Value = "0";

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        DataSet dsPaymentDetail = AccountExpense.GetInvoicePendingPayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            hdnPaymentId.Value = dsPaymentDetail.Tables[0].Rows[0]["lid"].ToString();
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
            
            // Currency
            if (dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyId"] != DBNull.Value)
            {
                ddCurrency.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyId"].ToString();
            }
            if (dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyRate"] != DBNull.Value)
            {
                txtExchangeRate.Text = dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyRate"].ToString();
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
            
            // Vendor Account Detail

            if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            {
                int VendorBankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]);

                ddVendorBank.SelectedValue = VendorBankAccountId.ToString();

                DataSet dsVendorBankDetal = AccountExpense.GetVendorBankDetail(VendorBankAccountId);

                if (dsVendorBankDetal.Tables.Count > 0)
                {
                    if (dsVendorBankDetal.Tables[0].Rows.Count > 0)
                    {
                        lblVendorBankName.Text = dsVendorBankDetal.Tables[0].Rows[0]["BankName"].ToString();
                        lblVendorBankAccountNo.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountNo"].ToString();
                        lblVendorBankAccountIFSC.Text = dsVendorBankDetal.Tables[0].Rows[0]["IFSCCode"].ToString();
                        lblVendorBankAccountName.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountName"].ToString();
                    }
                }
            }
        }
    }
    private void GetProformaPaymentRequest(int ProformaInvoiceID)
    {
        fldProfrmaInvoice.Visible = true;
        fldProformaPayment.Visible = true;

        // Show Proforma Invoice Payment History
        hdnProformaInvoiceId.Value = ProformaInvoiceID.ToString();

        DataSourceProformaPaymentHistory.SelectParameters[0].DefaultValue = ProformaInvoiceID.ToString();
        DataSourceProformaPaymentHistory.DataBind();


        DataSet dsDetail = AccountExpense.GetInvoiceDetail(ProformaInvoiceID);

        int StatusId = 0;
        int ExpenseTypeId = 0;

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);

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

            if (dsDetail.Tables[0].Rows[0]["PaidAmount"] != DBNull.Value)
            {
                lblProformaPaidAmount.Text = dsDetail.Tables[0].Rows[0]["PaidAmount"].ToString();

                // Calculate Balance For Payment

                Decimal decFinalPayment = Convert.ToDecimal(lblNetPayable.Text.Trim());
                Decimal decProformaPayment = Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["PaidAmount"]);
                Decimal decProformaTDSAmount = 0m;

                if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    decProformaTDSAmount = Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString());
                }
                Decimal decTotalPayable = (decFinalPayment - decProformaPayment - decProformaTDSAmount);

                lblNetPayable.Text = decTotalPayable.ToString();

                if (decTotalPayable > 0)
                {
                    // Allow Payment for Proforma to Final Invoice
                    fldPayment.Visible = true;
                }
                else
                {
                    // Payment Not Required
                    fldPayment.Visible = false;
                }
            }

            lblProformaRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblProformaInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();

            lblProformaInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            lblProformaTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            lblProformaTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();

            lblProformaAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            lblProformaAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            lblProformaBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblProformaBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            lblProformaBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();


            lblProformaRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            lblProformRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();

            if (dsDetail.Tables[0].Rows[0]["InvoiceAmount"] != DBNull.Value)
            {
                lblProformaTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["TaxAmount"] != DBNull.Value)
            {
                lblProformaTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["CurrencyName"] != DBNull.Value)
            {
                lblProformaInvoiceCurrency.Text = dsDetail.Tables[0].Rows[0]["CurrencyName"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                lblProformaExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblProformaGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblProformaInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            {
                lblProformaPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["dtDate"] != DBNull.Value)
            {
                lblProformaPatymentRequestDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["TDSApplicable"]) == true)
            {
                lblProformaTDSApplicable.Text = "Yes";
                lblTProformaDSRate.Text = dsDetail.Tables[0].Rows[0]["TDSRate"].ToString();

                if (dsDetail.Tables[0].Rows[0]["TDSRate"] != DBNull.Value)
                {
                    if (dsDetail.Tables[0].Rows[0]["TDSRateType"] != DBNull.Value)
                    {
                        if (dsDetail.Tables[0].Rows[0]["TDSRateType"].ToString() == "1")
                        {
                            lblProformaTDSRateType.Text = "Standard";
                        }
                        else if (dsDetail.Tables[0].Rows[0]["TDSRateType"].ToString() == "2")
                        {
                            lblProformaTDSRateType.Text = "Concessional";
                        }
                    }
                    else
                    {
                        lblProformaTDSRateType.Text = "Standard";
                    }
                }
                if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    lblProformaTDSAmount.Text = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
                }
            }
            //// Invoice Charge Detail

            //if (dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1")
            //{
            //    fldInvoiceitem.Visible = true;
            //}
            //else
            //{
            //    fldInvoiceitem.Visible = false;
            //}

            // Vendor Detail
            //if (dsDetail.Tables[0].Rows[0]["PaymentTerms"] != null)
            //{
            //    lblProformaCreditTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();
            //}

            //string strVendorCode = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            //DataSet dsVendor = AccountExpense.GetFAVendorByCode(strVendorCode);

            //if (dsVendor.Tables[0].Rows.Count > 0)
            //{
            //    if (dsVendor.Tables[0].Rows[0]["OutStandingAmount"] != null)
            //    {
            //        lblCurrentOutstanding.Text = dsVendor.Tables[0].Rows[0]["OutStandingAmount"].ToString();
            //    }
            //    if (dsVendor.Tables[0].Rows[0]["BKNAME"] != null)
            //    {
            //        lblBankName.Text = dsVendor.Tables[0].Rows[0]["BKNAME"].ToString();
            //    }
            //    if (dsVendor.Tables[0].Rows[0]["BKACNO"] != null)
            //    {
            //        lblBankAccountNo.Text = dsVendor.Tables[0].Rows[0]["BKACNO"].ToString();
            //    }
            //    if (dsVendor.Tables[0].Rows[0]["BKIFSC"] != null)
            //    {
            //        lblBankIFSC.Text = dsVendor.Tables[0].Rows[0]["BKIFSC"].ToString();
            //    }
            //}

            // Billing Party Detail

            if (dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"] != null)
            {
                DataSet dsBillingParty = null;

                string strProformaBillingGSTIN = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();

                if (strProformaBillingGSTIN != "")
                {
                    dsBillingParty = AccountExpense.GetFAVendorByGSTIN(strProformaBillingGSTIN);
                }
                else if (dsDetail.Tables[0].Rows[0]["ConsigneeCode"] != null)
                {
                    string strProformaBillingCode = dsDetail.Tables[0].Rows[0]["ConsigneeCode"].ToString();

                    if (strProformaBillingCode != "")
                    {
                        dsBillingParty = AccountExpense.GetFAVendorByCode(strProformaBillingGSTIN);
                    }
                }

                lblProformaBillingGSTN.Text = strProformaBillingGSTIN;
                lblProformaBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();

                if (dsBillingParty != null)
                {
                    if (dsBillingParty.Tables[0].Rows.Count > 0)
                    {
                        if (dsBillingParty.Tables[0].Rows[0]["girno"] != null)
                        {
                            lblProformaBillingPAN.Text = dsBillingParty.Tables[0].Rows[0]["girno"].ToString();
                        }
                        if (dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"] != null)
                        {
                            lblProformaCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                        }

                    }
                }
            }

        }

    }
    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(hdnBillType.Value) < 10)
        {
            int PayResult = UpdatePaymentRequest();

            if (PayResult == 0)
            {
                lblError.Text = "Payment Detail Added Successfully!";
                lblError.CssClass = "success";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Success", "alert('" + lblError.Text + "');", true);
            }
        }
        else
        {
            lblError.Text = "Payment Not Required!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
        }
    }

    private int UpdatePaymentRequest()
    {
        int SuccessId = 0;
        int InvoiceId = 0, PaymentTypeId = 0, BabajiBankId = 0, BankAccountId = 0;
        int CurrencyId = 0; decimal decCurrencyRate = 1;

        Boolean IsFullPayment = true;

        Boolean IsFundTransFromAPI = false;

        if (rblFundTransferFromLiveTracking.SelectedValue == "1")
        {
            IsFundTransFromAPI = true;
        }
        if (rblPayment.SelectedValue == "0")
        {
            IsFullPayment = false;
        }

        int VendorBankAccountId = 0;

        string strRemark = "";

        decimal decNetPayable = 0m;
        Decimal decPaidAmount = 0m;

        InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        Decimal.TryParse(lblNetPayable.Text.Trim(), out decNetPayable);
        Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);

        PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);
    
        CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);

        Decimal.TryParse(txtExchangeRate.Text.Trim(), out decCurrencyRate);

        if (ddBabajiBankName.SelectedIndex != -1)
        {
            BabajiBankId = Convert.ToInt32(ddBabajiBankName.SelectedValue);
        }
        if (ddBabajiBankAccount.SelectedIndex != -1)
        {
            BankAccountId = Convert.ToInt32(ddBabajiBankAccount.SelectedValue);
        }

        if (ddVendorBank.SelectedIndex != -1)
        {
            VendorBankAccountId = Convert.ToInt32(ddVendorBank.SelectedValue);
        }

        if (VendorBankAccountId == 0)
        {
            lblError.Text = "Please Select Vendor Bank Account!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Vendor Bank Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;

            return SuccessId;
        }

        if (PaymentTypeId == 0)
        {
            lblError.Text = "Please Select Payment Type!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;

            return SuccessId;
        }

        if (PaymentTypeId > 1 && PaymentTypeId < 90)
        {
            // Payment From Bank

            if (BabajiBankId == 0)
            {
                lblError.Text = "Please Select Babaji Bank Name!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 2;
            }
        }

        if (BankAccountId == 0)
        {
            lblError.Text = "Please Select Bank Account/Cash Book!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }
        strRemark = "";

        if (decNetPayable == decPaidAmount)
        {
            IsFullPayment = true;
        }

        if (decNetPayable == 0)
        {
            lblError.Text = "Invalid Net Payable Amount!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }
        else if (decPaidAmount <= 0)
        {
            lblError.Text = "Invalid Payment Amount!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }
        else if (decPaidAmount > decNetPayable)
        {
            lblError.Text = "Entered Paid Amount Exceed net Payable Amount! Please Check Paid Amount.!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }
        else if (SuccessId == 0)
        {
            /*******************************
            if (IsFundTransFromAPI == true)
            {
                if (decPaidAmount <= 200000)
                {
                    PaymentTypeId = 6;// NEFT
                }
                else
                {
                    PaymentTypeId = 4;// RTGS
                }
            }
            *******************************/

            int PayResult = AccountExpense.AddInvoicePaymentRequest2(InvoiceId, IsFullPayment, PaymentTypeId, BabajiBankId, BankAccountId,
                IsFundTransFromAPI, VendorBankAccountId, decPaidAmount, CurrencyId, decCurrencyRate, LoggedInUser.glUserId);

            if (PayResult == 0)
            {
                lblError.Text = "Payment Detail Added Successfully!";
                lblError.CssClass = "success";

                SuccessId = 0;
            }
            else if (PayResult == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 1;
            }
            else if (PayResult == 2)
            {
                lblError.Text = "Entered Paid Amount Exceed Net Payable Amount! Please Check Net Payable Amount.";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 2;
            }
        }

        return SuccessId;
    }
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPayment.SelectedValue == "1")
        {
            txtPayAmount.Text = lblNetPayable.Text;
        }
        else
        {
            txtPayAmount.Text = "";
        }
    }

    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int PaymentType = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (PaymentType == 1) // Cash
        {
            AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2);

            // Clear Bank Name

            ddBabajiBankName.Items.Clear();

            // Clear Bank Account;

        }
        else if (PaymentType == 90) // Security Deposit
        {
            AccountExpense.FillBankBookByType(ddBabajiBankAccount, 3);

            rblPayment.SelectedValue = "0";
            txtPayAmount.Text = "";

            // Clear Bank Name
            ddBabajiBankName.Items.Clear();

        }
        else if (PaymentType > 1) // Bank
        {
            // File Bank Name MS

            AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name

            // Clear Cash Book

            ddBabajiBankAccount.Items.Clear();
        }
        else
        {
            ddBabajiBankName.Items.Clear();
            ddBabajiBankAccount.Items.Clear();
        }
    }
    protected void ddBabajiBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int BabajiBankID = Convert.ToInt32(ddBabajiBankName.SelectedValue);

        AccountExpense.FillBankAccountByBankId(ddBabajiBankAccount, BabajiBankID);

    }
    protected void ddVendorBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Get Vendor Bank Detail

        int VendorBankId = Convert.ToInt32(ddVendorBank.SelectedValue);

        DataSet dsVendorBank = AccountExpense.GetVendorBankDetail(VendorBankId);

        if (dsVendorBank.Tables.Count > 0)
        {
            lblVendorBankName.Text = dsVendorBank.Tables[0].Rows[0]["BankName"].ToString();
            lblVendorBankAccountName.Text = dsVendorBank.Tables[0].Rows[0]["AccountName"].ToString();
            lblVendorBankAccountNo.Text = dsVendorBank.Tables[0].Rows[0]["AccountNo"].ToString();
            lblVendorBankAccountIFSC.Text = dsVendorBank.Tables[0].Rows[0]["IFSCCode"].ToString();
            lblVendorBankAccountType.Text = dsVendorBank.Tables[0].Rows[0]["AccountTypeName"].ToString();
            lblVendorBankRemark.Text = dsVendorBank.Tables[0].Rows[0]["Remark"].ToString();
        }

    }

    protected void ddBabajiBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
       // rblFundTransferFromLiveTracking.Enabled = false;
       // rblFundTransferFromLiveTracking.SelectedValue = "0";

        int PaymentType = 0; // Cash/ Bank
        int BankId = 0; // Yes Bank - 47 
        int BankAccountID = 0; // Yes Bank Mumbai Id - 161, FA Code - YBL000

        PaymentType = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (ddBabajiBankName.SelectedIndex != -1)
        {
            BankId = Convert.ToInt32(ddBabajiBankName.SelectedValue);
        }

        if (ddBabajiBankAccount.SelectedIndex != -1)
        {
            BankAccountID = Convert.ToInt32(ddBabajiBankAccount.SelectedValue);
        }

        if (PaymentType == 4 || PaymentType == 6)
        {
            if (BankId == 47 && BankAccountID == 161) // Yes Bank Mumbai Account
            {
                rblFundTransferFromLiveTracking.Enabled = true;
            }
            else
            {
                rblFundTransferFromLiveTracking.SelectedValue = "0";
                rblFundTransferFromLiveTracking.Enabled = false;

            }
        }
        else
        {
            rblFundTransferFromLiveTracking.SelectedValue = "0";
            rblFundTransferFromLiveTracking.Enabled = false;

        }
    }

    protected void rblFundTransferFromLiveTracking_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFundTransferFromLiveTracking.SelectedValue == "1")
        {
            string strPopupmessage = "Fund Transfer will be initiated from Live Tracking.";
            ScriptManager.RegisterStartupScript(this, GetType(), "Fund Transfer", "alert('" + strPopupmessage + "');", true);

        }
    }

    private bool CheckInvoiceStatusChange()
    {
        bool isStatusChange = true;
        int PrevStatusId = Convert.ToInt32(hdnStatusId.Value);

        if (Session["InvoiceId"] != null)
        {
            int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);

            int result = AccountExpense.CheckInvoiceStatusChange(InvoiceID, PrevStatusId);

            if (result == 0)
            {
                // Status Not changed

                isStatusChange = false;
            }
            else
            {
                // Status changed

                isStatusChange = true;
            }
        }

        return isStatusChange;
    }

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
            else if (e.CommandName.ToLower() == "view")
            {
                int DocumentId2 = Convert.ToInt32(e.CommandArgument.ToString());

                DataSet dsGetDocPath2 = AccountExpense.GetInvoiceDocument(0, DocumentId2);

                if (dsGetDocPath2.Tables.Count > 0)
                {
                    String strDocpath = "";
                    String strFilePath = dsGetDocPath2.Tables[0].Rows[0]["FilePath"].ToString();

                    String strFileName = dsGetDocPath2.Tables[0].Rows[0]["FileName"].ToString();

                    strDocpath = strFilePath + strFileName;
                    ViewDocument(strDocpath);
                }
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
    #endregion

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
}