using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class AccountExpense_InvoicePosting : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Audit";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoicePosting.aspx");
        }

        if (!IsPostBack)
        {
            if (!Page.IsPostBack)
            {
                ddCurrency.SelectedValue = "46";

                btnCancel.PostBackUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            }

            GetPaymentRequest();

            GetInvoicePayment();

            if (gvCharges.Rows.Count == 0)
            {
                lblError.Text = "Charge Detail Missing! Please reject Invoice for Re-Submission";
                lblError.CssClass = "errorMsg";

                btnPostSubmit.Visible = false;
            }
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
        // Reset Net Payable

        lblNetPayable.Text = "0";
        txtPayAmount.Text = "0";
        hdnNetPayableAmount.Value = "0";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoicePosting.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        int StatusId = 0;
        int ExpenseTypeId = 0;
        int BillType = 0; //  10 For Bank Check issued against Job and Payment Already Completed
        int InvoiceMode = 1;

        Boolean IsRim = true;
        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        { 
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);
            hdnStatusId.Value = StatusId.ToString();
            lblLTRefNo.Text = "LT" + InvoiceID.ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblContainerCount.Text =dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            lblWeight.Text = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            hdnInvoiceType.Value = dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString();
            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");

            BillType = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["BillType"]);
            InvoiceMode = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["InvoiceMode"]);
            hdnBillType.Value = dsDetail.Tables[0].Rows[0]["BillType"].ToString();
            hdnInvoiceMode.Value = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();

            if (hdnBillType.Value == "10") // Cheque Already Issued against Job
            {
                fldPayment.Visible = false;
                fldPaymentHistory.Visible = true;
                RFVVendorBank.Enabled = false;
                ddlPaymentType.SelectedValue = "2";
                ddlPaymentType.Enabled = false;

                lblError.Text = "Vendor Cheque Already Issued! Payment not required!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Blank Cheque Payment", "alert('" + lblError.Text + "');", true);
            }
            else if (hdnBillType.Value == "11") // PD Account Payment
            {
                fldPayment.Visible = false;
                fldPaymentHistory.Visible = true;
                RFVVendorBank.Enabled = false;
                ddlPaymentType.SelectedValue = "2";
                ddlPaymentType.Enabled = false;

                lblError.Text = "PD Account! Payment Not Required From Live Tracking!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "PD Account", "alert('" + lblError.Text + "');", true);
            }
            else if (hdnInvoiceMode.Value == "2") // Credit Note
            {
                fldPayment.Visible = false;
                fldPaymentHistory.Visible = true;
                RFVVendorBank.Enabled = false;
             //   ddlPaymentType.SelectedValue = "2";
                ddlPaymentType.Enabled = false;

                lblError.Text = "Credit Note ! Payment not required!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Credit Note", "alert('" + lblError.Text + "');", true);
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

            if (hdnInvoiceType.Value == "0") // Proforma Invoice
            {
                // For Proforma Partial Payment Not Allowed
                txtPayAmount.Enabled = false;
            }

            IsRim = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsRIM"]);

            if(IsRim == true)
            {
                hdnIsRim.Value = "1"; // RIM
            }
            else
            {
                hdnIsRim.Value = "0"; // Non-RIM
            }

            if (dsDetail.Tables[0].Rows[0]["ExpenseTypeId"] != DBNull.Value)
            {
                ExpenseTypeId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ExpenseTypeId"]);

                lblExpenseType.Text = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
                hdnExpenseTypeID.Value = ExpenseTypeId.ToString();
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
            if (dsDetail.Tables[0].Rows[0]["OtherDeduction"] != DBNull.Value)
            {
                lblOtherDeduction.Text = dsDetail.Tables[0].Rows[0]["OtherDeduction"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["IECNo"] != DBNull.Value)
            {
                lblIECNo.Text = dsDetail.Tables[0].Rows[0]["IECNo"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["PortCode"] != DBNull.Value)
            {
                lblPortCode.Text = dsDetail.Tables[0].Rows[0]["PortCode"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["VendorId"] != DBNull.Value)
            {
                int VendorId = Convert.ToInt32 (dsDetail.Tables[0].Rows[0]["VendorId"]);

                AccountExpense.FillVendorBank(ddVendorBank, VendorId);
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

            lblRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();

            ddTransactionType.SelectedValue  = dsDetail.Tables[0].Rows[0]["TransactionTypeId"].ToString(); 

            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();

                if (Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString()) > 0)
                {
                    ddTransactionType.SelectedValue = "0";
                    ddTransactionType.Visible = false;
                    fldRCMItem.Visible = false;
                }
                else
                {
                    // Transaction Type - Required For Final And Non-RIM
                    ddTransactionType.Visible = true;

                    if(IsRim == true)
                    {
                        ddTransactionType.SelectedValue = "1";
                        ddTransactionType.Enabled = false;
                    }
                    else
                    {
                        // Not-Applicable Transaction Type Not required for Non-RIM
                       ListItem lstNoApplicable  = ddTransactionType.Items.FindByValue("1");
                        if(lstNoApplicable != null)
                        {
                            ddTransactionType.Items.Remove(lstNoApplicable);
                        }
                        // Non-RIM RCM Detail
                        chkNoITC.Visible = true;

                        GridViewRCM.Visible = true;
                        
                        if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["RCMApplicable"]) == true)
                        {
                            chkRCMYes.Checked = true;
                            ddRCMRate.SelectedValue = dsDetail.Tables[0].Rows[0]["RCMRate"].ToString();
                            lblRCMTotalAmount.Text = dsDetail.Tables[0].Rows[0]["RCMTotalAmount"].ToString();
                            GridViewRCM.Visible = true;
                        }
                    }
                }
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

                if (fldPayment.Visible == true)
                {                    
                    lblPaymentCurrencyName1.Text = dsDetail.Tables[0].Rows[0]["CurrencyName"].ToString();

                    ddCurrency.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyId"].ToString();
                }
            }
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                lblExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
                txtExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
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
                ddlPaymentType.SelectedValue = dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

                if (dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString() == "1") // Cash
                {
                    AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2);
                }
                else // Bank
                {
                    // File Bank Name MS

                    AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name
                }
            }

            // Vendor Detail
            string strVendorCode = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            DataSet dsVendor = AccountExpense.GetFAVendorByCode(strVendorCode);

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

            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["TDSApplicable"]) == true)
            {
                chkTDSYes.Checked = true;

                if (dsDetail.Tables[0].Rows[0]["TDSRate"] != DBNull.Value)
                {
                    if (dsDetail.Tables[0].Rows[0]["TDSRateType"] != DBNull.Value)
                    {
                        if(dsDetail.Tables[0].Rows[0]["TDSRateType"].ToString() == "1")
                        {
                            ddTDSRateType.SelectedValue = "1";
                            ddTDSRate.SelectedValue = dsDetail.Tables[0].Rows[0]["TDSRate"].ToString();

                            ddTDSRate.Visible       =   true;
                            rfvddTDSRate.Enabled    =   true;

                            txtTDSRate.Visible      =   false;
                            rfvtxtTdsRate.Enabled   =   false;
                        }
                        else if (dsDetail.Tables[0].Rows[0]["TDSRateType"].ToString() == "2")
                        {
                            ddTDSRateType.SelectedValue = "2";
                            txtTDSRate.Text         =   dsDetail.Tables[0].Rows[0]["TDSRate"].ToString();
                            
                            txtTDSRate.Visible      =   true;
                            rfvtxtTdsRate.Enabled   =   true;

                            ddTDSRate.Visible       =   false;
                            rfvddTDSRate.Enabled    =   false;
                        }
                    }
                    else
                    {
                        ddTDSRate.SelectedValue = dsDetail.Tables[0].Rows[0]["TDSRate"].ToString();
                    }
                }
                if (dsDetail.Tables[0].Rows[0]["TDSLedgerCodeId"] != DBNull.Value)
                {
                    ddTDSLedgerCode.SelectedValue = dsDetail.Tables[0].Rows[0]["TDSLedgerCodeId"].ToString();
                }
                if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    lblTotalTDS.Text = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
                }

                GridViewTDS.Visible = true;
            }
            else
            {
                chkTDSYes.Checked = false;
                lblTotalTDS.Text = "0";
                txtTDSRate.Text = "";
                ddTDSRate.SelectedValue = "0";
                ddTDSLedgerCode.SelectedIndex = -1;
                GridViewTDS.Visible = false;
            }
            // Proforma Invoice Detail against Final Invoice

            if (dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1") // Final)
            {
                if(dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"] != DBNull.Value)
                {
                    int ProformaInvoiceId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"]);

                    if(ProformaInvoiceId > 0)
                    {
                        // Show Proforma Invoice Detail
                        // Show Proforma Invoice Payment History
                        GetProformaPaymentRequest(ProformaInvoiceId);
                        // Payment detail Not Required

                        //fldPayment.Visible = false;

                    }
                }
            }

            // Proforma Invoice -- Invoice Charge and RCM Not Required

            if (dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "0") // Proforma
            {
                btnNewCharge.Visible = false;

               // fldInvoiceItem.Visible = false;

                fldRCMItem.Visible = false;
               // fldPostInvoice.Visible = false;

                fldTDSItem.Visible = true; // TDS Applicable
            }
            else if (IsRim == true) // RIM
            {
                ddTransactionType.Visible = true;
                ddTransactionType.Enabled = false;
                ddTransactionType.SelectedValue = "1"; // Not Applicable

                fldInvoiceItem.Visible = true;

                fldRCMItem.Visible = false;
                // fldPostInvoice.Visible = false;

                fldTDSItem.Visible = true; // TDS Applicable

                // NO-ITC Not Required

                chkNoITC.Checked = false;
                chkNoITC.Visible = false;
            }
            else
            {
                // Non RIM RCM & No ITC Applicable
            }

            if (ExpenseTypeId == 1 || ExpenseTypeId == 9 || ExpenseTypeId == 100 || ExpenseTypeId == 104)
            {
                // TDS Not Required

                fldTDSItem.Visible = false;
                fldRCMItem.Visible = false;
            }
        }
                
        // Hide Details for Payment Status

        ShowHideFormDetails(StatusId, ExpenseTypeId);
    }
    private void GetInvoicePayment()
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        DataSet dsPaymentDetail = AccountExpense.GetInvoicePayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            txtPayAmount.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            //txtPaymentRemark.Text = dsPaymentDetail.Tables[0].Rows[0]["Remark"].ToString();
        
            if(dsPaymentDetail.Tables[0].Rows[0]["IsFullPayment"] != DBNull.Value)
            {
                if(Convert.ToBoolean(dsPaymentDetail.Tables[0].Rows[0]["IsFullPayment"]) == true)
                {
                    rblPayment.SelectedValue = "1";
                }
                else
                {
                    rblPayment.SelectedValue = "0";
                }
            }

            // Payment Mode
            if (ddlPaymentType.SelectedValue != "0")
            {
                // Fill Bank/Cash Code
                if (ddlPaymentType.SelectedValue == "1")
                {
                    AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2); // Cash Book
                }
                else
                {
                    // File Bank Name MS

                    AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name

                    if (dsPaymentDetail.Tables[0].Rows[0]["BankId"] != DBNull.Value)
                    {
                        int BabajiBankID = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankId"]);

                        AccountExpense.FillBankAccountByBankId(ddBabajiBankAccount, BabajiBankID);
                    }
                }

                if (dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"] != DBNull.Value)
                {
                    ddBabajiBankAccount.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"].ToString();
                }
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
                Decimal decFinalTDS = 0m;
                
                Decimal.TryParse(lblTotalTDS.Text.Trim(), out decFinalTDS);

                Decimal decProformaPayment = Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["PaidAmount"]);
                Decimal decProformaTDSAmount = 0m;
               
                if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    decProformaTDSAmount = Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString());
                }

                Decimal decTotalPayable = (decFinalPayment - decProformaPayment - decProformaTDSAmount);

                lblNetPayable.Text = decTotalPayable.ToString();

                if(decTotalPayable > 2)
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
                        //if (dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"] != null)
                        //{
                        //    lblProformaCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                        //}
                    }
                }
            }

        }

    }
    private void GetVendorBankDetail(int VendorBankID)
    {
        //AccountExpense.FillBankBookByType
    }
    private void ShowHideFormDetails(int StatusId, int ExpenseTypeId)
    {
        if (StatusId < 140)
        {
            // Invoice Not Posted
            // Hide payment Details

           // fldPayment.Visible = false;
           // fldPaymentHistory.Visible = false;
        }

        //if (ExpenseTypeId == 1 || ExpenseTypeId == 9)
        //{
        //    fldVendor.Visible = false;
        //    fldInvoiceItem.Visible = false;
        //    fldTDSItem.Visible = false;
        //    fldRCMItem.Visible = false;
        //    fldPostInvoice.Visible = false;

        //    // Allow Payment

        //    fldPayment.Visible = true;
        //}

        if (StatusId >= 142) // Invoice Finance Reject
        {
            // Show All Field
        }
        else if (StatusId >= 140) // Invoice Posted
        {
            // Hide Hold/Reject Button
            // Dissable All Update except Payment Detail

            btnHold.Visible = false;
            btnReject.Visible = false;

            gvCharges.Enabled = false;
            btnNewCharge.Visible = false;
            btnAddCharges.Visible = false;
            btnAddTDS.Visible = false;
            btnAddRCM.Visible = false;

           // fldPostInvoice.Visible = false;
        }

        if (StatusId == 152) // Full Payment
        {
            fldPayment.Visible = false;
        }
    }
    protected void btnPostSubmit_Click(object sender, EventArgs e)
    {
        int ProformaInvoiceId = 0;
        int PaymentResult = 0;
        int VendorBankId = 0;
        int BillType = 0;
        int InvoiceMode = 1;

        BillType = Convert.ToInt32(hdnBillType.Value);
        InvoiceMode = Convert.ToInt32(hdnInvoiceMode.Value);

        int TransactionTypeId = Convert.ToInt32(ddTransactionType.SelectedValue);
        int TDSExemptReasonID = Convert.ToInt32(ddTDSExempt.SelectedItem.Value);

        Boolean bNoItc = chkNoITC.Checked;

        Decimal decNetPayable = 0m;

        if (lblNetPayable.Text.Trim() != "")
        {
            decNetPayable = Convert.ToDecimal(lblNetPayable.Text.Trim());
        }

        // TDS Validation

        if (hdnExpenseTypeID.Value == "1" || hdnExpenseTypeID.Value == "9" || hdnExpenseTypeID.Value == "100" || hdnExpenseTypeID.Value == "104")
        {
            // TDS not applicable for Duty and Panelty / Overtime Charges

            TDSExemptReasonID = 50; // N.A.
        }
        else if (chkTDSNo.Checked == false && chkTDSYes.Checked == false)
        {
            lblError.Text = "TDS Error: Please select TDS Yes/NO!";
            lblError.CssClass = "errorMsg";
            return;
        }
        else if (chkTDSNo.Checked && chkTDSYes.Checked)
        {
            lblError.Text = "TDS Error: Please select TDS Yes/NO!";
            lblError.CssClass = "errorMsg";
            return;
        }
        else if (chkTDSNo.Checked && ddTDSExempt.SelectedValue == "0")
        {
            lblError.Text = "Please select TDS Exempt Reason!";
            lblError.CssClass = "errorMsg";
            return;
        }

        // Transaction Type Validation

        Decimal decGSTAmount = 0m;
        Decimal decRCMAmount = 0m;

        Decimal.TryParse(lblGSTValue.Text.Trim(), out decGSTAmount);
        Decimal.TryParse(lblRCMTotalAmount.Text.Trim(), out decRCMAmount);

        if (TransactionTypeId == 0 && decGSTAmount == 0 && decRCMAmount == 0 && hdnIsRim.Value == "0" && hdnInvoiceType.Value == "1")
        {
            lblError.Text = "Transaction Type Required For No GST, Non-RIM and Without RCM Invoice!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            return;
        }

        VendorBankId = Convert.ToInt32(ddVendorBank.SelectedValue);

        if(VendorBankId == 0)
        {
            if (ddlPaymentType.SelectedValue == "4" || ddlPaymentType.SelectedValue == "6")
            {
                lblError.Text = "Please Select Vendor Bank Account for RTGS/NEFT Payment!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                return;
            }
        }

        if(txtAuditRemark.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Audit Narration!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('" + lblError.Text + "');", true);
            return;
        }

        if (hdnProformaInvoiceId.Value != "")
        {
            ProformaInvoiceId = Convert.ToInt32(hdnProformaInvoiceId.Value);
        }

        if (ProformaInvoiceId == 0 && BillType != 10 && InvoiceMode != 2)
        {
            // Payment Required Only For Proforma and Final Invoice

            // Payment Not required for Final Invoice against Proforma

            if (BillType != 10 && BillType != 11)
            {
                PaymentResult = UpdatePaymentRequest();
            }
        }
        else 
        {
            // Check For Balance Payment - Proforma to Final
            
            if (decNetPayable > 5 && ProformaInvoiceId > 0 && Convert.ToInt32(hdnBillType.Value) < 10)
            {
                if (InvoiceMode != 2)
                    PaymentResult = UpdatePaymentRequest();
            }
        }


        if (PaymentResult == 0)
        {
            // Audit Completed and Submit for Financial Approval

            int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);

            if (ProformaInvoiceId == 0 || decNetPayable > 5)
            {
                int AuditResult = AccountExpense.AddInvoiceAudit(InvoiceID, TDSExemptReasonID, TransactionTypeId, bNoItc, LoggedInUser.glUserId);

                if (hdnBillType.Value == "3")
                {
                    int result2 = AccountExpense.AddInvoiceStatus(InvoiceID, 140, txtAuditRemark.Text.Trim(), LoggedInUser.glUserId);

                    if (result2 == 0)
                    {
                        AddJobProfit(InvoiceID);

                        lblError.Text = "Invoice Sent For Management Approval!";
                        lblError.CssClass = "success";

                        Session["Success"] = lblError.Text;

                        /** Credit Vendor Mgmt Approval Email Not Required ***/
                        //SendRequestMail(InvoiceID);

                        Response.Redirect("AccountSuccess.aspx");
                    }
                }
                else
                {
                    int result2 = AccountExpense.AddInvoiceStatus(InvoiceID, 140, txtAuditRemark.Text.Trim(), LoggedInUser.glUserId);

                    if (result2 == 0)
                    {
                        lblError.Text = "Invoice Sent For Finance Approval!";
                        lblError.CssClass = "success";

                        Session["Success"] = lblError.Text;

                        Response.Redirect("AccountSuccess.aspx");
                    }
                }
            }
            else
            {
                // Proforma To Final Invoice - Audit Completed- Auto Finace Approve For FA Posting

                int AuditResult = AccountExpense.AddInvoiceAudit(InvoiceID, TDSExemptReasonID, TransactionTypeId, bNoItc, LoggedInUser.glUserId);

                // Audit Completed
                int result = AccountExpense.AddInvoiceStatus(InvoiceID, 140, txtAuditRemark.Text.Trim(), LoggedInUser.glUserId);

                // Invoice Auto Finance Approve Completed
                int result21 = AccountExpense.AddInvoiceStatus(InvoiceID, 145, "", LoggedInUser.glUserId);

                // Invoice Workflow Completed
                int result31 = AccountExpense.AddInvoiceStatus(InvoiceID, 152, "", LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Invoice Audit Completed!";
                    lblError.CssClass = "success";

                    Session["Success"] = lblError.Text;

                    Response.Redirect("AccountSuccess.aspx");
                }

            }
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRejectRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 122, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            // Remove TDS Detail - TDS Applicable = false
            AccountExpense.AddInvoiceTDS(InvoiceID, false, "", 0, 0, LoggedInUser.glUserId);

            lblError.Text = "Invoice Detail Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = lblError.Text;

            SendHoldRejectMail(InvoiceID, "Accounts Dept Reject", strRemark);

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Rejected!";
            lblError.CssClass = "errorMsg";

            Session["Success"] = lblError.Text;

            Response.Redirect("AccountSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnHold_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRejectRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 121, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = lblError.Text;

            SendHoldRejectMail(InvoiceID, "Accounts Dept On Hold", strRemark);

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already On Hold!";
            lblError.CssClass = "errorMsg";

            Session["Success"] = lblError.Text;

            Response.Redirect("AccountSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {        
        Response.Redirect(HttpContext.Current.Request.UrlReferrer.ToString());

        //Response.Redirect("PendingInvoicePosting.aspx");
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

    #region Invoice Item

    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        lblChargeError.Text = "";

        int ItemId = Convert.ToInt32(hdnItemId.Value);

        if (Session["InvoiceId"] != null)
        {
           int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

            string strChargeName = "", strChargeCode = "", strHSN = "", strRemark = "";
            decimal decTotalAmount = 0m, decTaxAmount = 0m, decIGSTRate = 0m, decCGSTRate = 0m, decSGSTRate = 0m,
            decIGSTAmount = 0m, decCGSTAmount = 0m, decSGSTAmount = 0m, decOtherDeduction = 0m;

            decimal decTaxableAmount = 0, decGSTRate = 0, decGSTAmount = 0;
            decimal decIGSTAmt = 0, decCGSTAmt = 0, decSGSTAmt = 0;

            if (txtOtherDeduction.Text.Trim() != "")
            {
                decOtherDeduction = Convert.ToDecimal(txtOtherDeduction.Text.Trim());
            }

            decTaxableAmount = Convert.ToDecimal(txtTaxableValue.Text.Trim());

            decGSTRate = Convert.ToDecimal(ddGSTRate.SelectedValue);

            // Deduct OtherDeduction from Taxable valeu and Calculate GST Amount
            decGSTAmount = ((decTaxableAmount - decOtherDeduction) * decGSTRate) / 100;

            decTotalAmount = (decTaxableAmount - decOtherDeduction) + decGSTAmount;

            if (rblGSTType.SelectedValue == "1")
            {
                decIGSTRate = decGSTRate;
                decIGSTAmt = decGSTAmount;
            }
            else
            {
                decCGSTRate = decGSTRate / 2;
                decSGSTRate = decGSTRate / 2;

                decCGSTAmt = decGSTAmount / 2;
                decSGSTAmt = decGSTAmount / 2;
            }

            strChargeName = txtChargeName.Text.Trim();
            strChargeCode = txtChargeCode.Text.Trim();
            strHSN = hdnChargeHSN.Value.Trim();
            strRemark = txtChargeRemark.Text.Trim();

            if (ItemId == 0) // Add New Charge
            {
                int resultAdd = AccountExpense.AddInvoiceItem(InvoiceId, 0, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxableAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmt, decCGSTAmt, decSGSTAmt, decOtherDeduction, strRemark, LoggedInUser.glUserId);
            
                if(resultAdd == 0)
                {
                    lblError.Text = "Invoice Detail Added!";
                    lblError.CssClass = "success";
                    gvCharges.DataBind();
                    ResetCharges();

                    ModalPopupExtender1.Hide();
                }
                else if (resultAdd == 2)
                {
                    lblChargeError.Text = "Charge Detail Already Exist!";
                    lblChargeError.CssClass = "errorMsg";

                    ModalPopupExtender1.Show();
                }
                else
                {
                    lblChargeError.Text = "System Error! Please try after sometime";
                    lblChargeError.CssClass = "errorMsg";

                    ModalPopupExtender1.Show();
                }
            }
            else if (ItemId >= 0) // Update Charge
            {
                int resultUpd = AccountExpense.UpdateInvoiceItem(ItemId, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxableAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmt, decCGSTAmt, decSGSTAmt, decOtherDeduction, strRemark, LoggedInUser.glUserId);

                if (resultUpd == 0)
                {
                    lblError.Text = "Invoice Detail Updated!";
                    lblError.CssClass = "success";
                    gvCharges.DataBind();
                    ResetCharges();

                    GetPaymentRequest();
                    ModalPopupExtender1.Hide();
                }
                 else
                {
                    lblChargeError.Text = "System Error! Please try after sometime";
                    lblChargeError.CssClass = "errorMsg";

                    ModalPopupExtender1.Show();
                }
            }
        }
        else
        {
            lblChargeError.Text = "Login Session Expired! Please Login Again.";
            lblChargeError.CssClass = "errorMsg";

            ModalPopupExtender1.Show();
        }

    }
    protected void btnNewCharge_Click(object sender, EventArgs e)
    {
        lblPopupHeader.Text = "Add New Charge Detail";
        btnAddCharges.Text = "Add Charges";
        hdnItemId.Value = "0";

        txtChargeCode.Enabled = true;
        txtChargeName.Enabled = true;
        txtTaxableValue.Enabled = true;
        rblGSTType.Enabled = true;
        ddGSTRate.Enabled = true;

        ResetCharges();
        ModalPopupExtender1.Show();
    }
    protected void lnkEditCharge_Click(object sender, EventArgs e)
    {
        txtChargeCode.Enabled = false;
        txtChargeName.Enabled = false;
        txtTaxableValue.Enabled = false;
        rblGSTType.Enabled = false;
        ddGSTRate.Enabled = false;

        txtChargeRemark.Text = "";
        lblPopupHeader.Text = "Update Charge Detail";
        btnAddCharges.Text = "Update Charges";
        
        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        DataSet dsItem = AccountExpense.GetInvoiceItemById(ChargeId);

        if(dsItem.Tables[0].Rows.Count > 0)
        { 
            hdnItemId.Value = dsItem.Tables[0].Rows[0]["lid"].ToString();
            txtChargeCode.Text = dsItem.Tables[0].Rows[0]["ChargeCode"].ToString();
            txtChargeName.Text = dsItem.Tables[0].Rows[0]["ChargeName"].ToString();
            hdnChargeHSN.Value = dsItem.Tables[0].Rows[0]["HSN"].ToString();

            txtTaxableValue.Text = dsItem.Tables[0].Rows[0]["TaxAmount"].ToString();
            txtOtherDeduction.Text = dsItem.Tables[0].Rows[0]["OtherDeduction"].ToString();
            txtChargeRemark.Text = dsItem.Tables[0].Rows[0]["Remark"].ToString();

            decimal decIGSTAmount = Convert.ToDecimal(dsItem.Tables[0].Rows[0]["IGSTAmount"]);
            decimal decIGSTRate = Convert.ToDecimal(dsItem.Tables[0].Rows[0]["IGSTRate"]);
            decimal decCGSTRate = Convert.ToDecimal(dsItem.Tables[0].Rows[0]["CGSTRate"]);

            if (decIGSTAmount > 0)
            {
                rblGSTType.SelectedValue = "1";

                if (decIGSTRate == 0)
                {
                    ddGSTRate.SelectedValue = "0";
                }
                else if (decIGSTRate == 18)
                {
                    ddGSTRate.SelectedValue = "18";
                }
                else if (decIGSTRate == 12)
                {
                    ddGSTRate.SelectedValue = "12";
                }
                else if (decIGSTRate == 5)
                {
                    ddGSTRate.SelectedValue = "5";
                }
            }
            else if(decCGSTRate > 0)
            {
                rblGSTType.SelectedValue = "0";

                ddGSTRate.SelectedValue = (decCGSTRate * 2).ToString();
            }
            else
            {
                // GST Not Applicable

                rblGSTType.SelectedValue = "1";
                ddGSTRate.SelectedValue = "0";
            }


            ModalPopupExtender1.Show();
        }

    }
    protected void lnlRemoveCharge_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        int result = AccountExpense.DeleteInvoiceItem(ChargeId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Charge Details Removed Successfully!";
            lblError.CssClass = "success";
            gvCharges.DataBind();
        }
        else 
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnClosePopup_Click(object sender, EventArgs e)
    {
        ResetCharges();
        ModalPopupExtender1.Hide();
    }
    private void ResetCharges()
    {
        txtChargeCode.Text = "";
        txtChargeName.Text = "";
        hdnChargeHSN.Value = "";
        txtTaxableValue.Text = "";
        txtChargeRemark.Text = "";
        txtOtherDeduction.Text = "";
        ddGSTRate.SelectedIndex = -1;
        hdnItemId.Value = "0";

    }

    #endregion

    #region TDS

    private DataTable GetTDSChargeCodeDataTable()
    {
        DataTable dtTDSChargeCode = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colTdsChargeCode = new DataColumn("TDSChargeCode", Type.GetType("System.String"));
        DataColumn colTdsChargeName = new DataColumn("TDSChargeName", Type.GetType("System.String"));
        DataColumn colTDSTaxableValue = new DataColumn("TDSTaxableValue", Type.GetType("System.String"));
        DataColumn colTDSRate = new DataColumn("TDSRate", Type.GetType("System.String"));
        DataColumn colTDSAmt = new DataColumn("TDSAmt", Type.GetType("System.String"));
        DataColumn colNetPayable = new DataColumn("NetPayable", Type.GetType("System.String"));
        DataColumn colTDSRemark = new DataColumn("TDSRemark", Type.GetType("System.String"));

        dtTDSChargeCode.Columns.Add(colSL);
        dtTDSChargeCode.Columns.Add(colTdsChargeCode);
        dtTDSChargeCode.Columns.Add(colTdsChargeName);
        dtTDSChargeCode.Columns.Add(colTDSTaxableValue);

        dtTDSChargeCode.Columns.Add(colTDSRate);
        dtTDSChargeCode.Columns.Add(colTDSAmt);
        dtTDSChargeCode.Columns.Add(colNetPayable);
        dtTDSChargeCode.Columns.Add(colTDSRemark);
        dtTDSChargeCode.PrimaryKey = new DataColumn[] { colSL };
        dtTDSChargeCode.AcceptChanges();

        return dtTDSChargeCode;
    }
    
    protected void lnlTdsRemove_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string strChargeId = lnk.CommandArgument.ToString();

        DataTable dtCharges = GetTDSChargeCodeDataTable();

        if (ViewState["vwsTDSCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsTDSCharges"];
        }

        dtCharges.Rows.Find(strChargeId).Delete();

        dtCharges.AcceptChanges();

        GridViewTDS.DataSource = dtCharges;
        GridViewTDS.DataBind();

        ViewState["vwsTDSCharges"] = dtCharges;
    }

    protected void btnAddTDS_Click(object sender, EventArgs e)
    {
        // Reset Net Payable

        lblNetPayable.Text = "0";
        txtPayAmount.Text = "0";

        decimal decTDSRate = 0m;

        int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        bool tdsApplicable = false;

        string strTDSLedgerCode = "";

        strTDSLedgerCode = ddTDSLedgerCode.SelectedValue;

        if(ddTDSLedgerCode.SelectedIndex == 0)
        {
            lblError.Text = "Please Select TDS Ledger!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);

            return;
        }

        int TDSRateType = Convert.ToInt32(ddTDSRateType.SelectedValue);

        if (chkTDSYes.Checked)
        {
            if (ddTDSRate.SelectedIndex != -1)
            {
                tdsApplicable = true;

                if (TDSRateType == 1) // Standard
                {
                    decTDSRate = Convert.ToDecimal(ddTDSRate.SelectedValue);
                }
                else
                {
                    decTDSRate = Convert.ToDecimal(txtTDSRate.Text.Trim());
                }

                // Calculate TDS Amount for invoice Item

                int Result = AccountExpense.AddInvoiceTDS(InvoiceId, tdsApplicable, strTDSLedgerCode, TDSRateType, decTDSRate, LoggedInUser.glUserId);

                if (hdnInvoiceType.Value == "0")
                {
                    GridViewTDS.DataBind();
                    GridViewTDS.Visible = false;
                }
                else
                {
                    GridViewTDS.Visible = true;

                    GridViewTDS.DataBind();
                }

                if (Result == 0)
                {
                    lblError.Text = "TDS Details Updated!.";
                    lblError.CssClass = "success";
                    GridViewTDS.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);

                    GetPaymentRequest();
                }
                else
                {
                    lblError.Text = "TDS Calculation Error!. Please check Amount & Rate!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);
                }
            }
            else
            {
                lblError.Text = "Please Select TDS Rate!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);
            }
        }
        else
        {
            tdsApplicable = false;

            int Result = AccountExpense.AddInvoiceTDS(InvoiceId, tdsApplicable, "", 0, 0, LoggedInUser.glUserId);

            lblError.Text = "TDS Details Removed!.";
            lblError.CssClass = "success";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);
            GetPaymentRequest();

            GridViewTDS.Visible = false;

            GridViewTDS.DataBind();
        }
    }

    protected void chkTDSYes_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTDSYes.Checked)
        {
            GridViewTDS.Visible = true;
            chkTDSNo.Checked = false;
            ddTDSExempt.SelectedIndex = 0;
        }
        else
        {
            GridViewTDS.Visible = false;
        }
    }
    protected void chkTDSNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTDSYes.Checked)
        {
            GridViewTDS.Visible = false;
            chkTDSYes.Checked = false;

        }
        else
        {
            ddTDSExempt.SelectedIndex = 0;
        }
    }
    protected void ddTDSRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddTDSRateType.SelectedValue == "1") // Standard
        {
            ddTDSRate.Visible = true;
            txtTDSRate.Visible = false;
        }
        else if (ddTDSRateType.SelectedValue == "2") // Concessional
        {
            ddTDSRate.Visible = false;
            txtTDSRate.Visible = true;
        }
    }


    #endregion

    #region TDS - EDIT

    protected void lnkEditTDS_Click(object sender, EventArgs e)
    {
        // Reset Net Payable

        lblNetPayable.Text = "0";
        txtPayAmount.Text = "0";

        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        //hdnTDSItemId.Value = ChargeId.ToString();
        DataSet dsItem = AccountExpense.GetInvoiceItemById(ChargeId);

        if (dsItem.Tables[0].Rows.Count > 0)
        {
            txtEditTDSRate.Text = "";
            ddEditTDSRate.SelectedValue = "0";

            hdnTDSItemId.Value = dsItem.Tables[0].Rows[0]["lid"].ToString();

            lblTDSTaxableValue.Text = dsItem.Tables[0].Rows[0]["NetTaxableValue"].ToString();
            ddEditTDSLedgerCode.SelectedValue = dsItem.Tables[0].Rows[0]["TDSLedgerCodeId"].ToString();

            if (dsItem.Tables[0].Rows[0]["TDSRateType"] != DBNull.Value)
            {
                if (dsItem.Tables[0].Rows[0]["TDSRateType"].ToString() == "1")
                {
                    ddEditTDSRateType.SelectedValue = "1";
                    ddEditTDSRate.SelectedValue = dsItem.Tables[0].Rows[0]["TDSRate"].ToString();

                    ddEditTDSRate.Visible = true;
                    txtEditTDSRate.Visible = false;

                    rfvddEditTDSRate.Enabled = true;
                    rfvtxtEditTDSRate.Enabled = false;
                }
                else if (dsItem.Tables[0].Rows[0]["TDSRateType"].ToString() == "2")
                {
                    ddEditTDSRateType.SelectedValue = "2";
                    txtEditTDSRate.Text = dsItem.Tables[0].Rows[0]["TDSRate"].ToString();

                    ddEditTDSRate.Visible = false;
                    txtEditTDSRate.Visible = true;

                    rfvddEditTDSRate.Enabled = false;
                    rfvtxtEditTDSRate.Enabled = true;
                }
                else
                {
                    ddEditTDSRateType.SelectedValue = "1";

                    ddEditTDSRate.Visible = true;
                    txtEditTDSRate.Visible = false;

                    rfvddEditTDSRate.Enabled = true;
                    rfvtxtEditTDSRate.Enabled = false;
                }

            }

        }

        ModalPopupTDS.Show();

    }
    protected void btnUpdateTDS_Click(object sender, EventArgs e)
    {
        decimal decTDSRate = 0m;

        int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);
        int ItemId = Convert.ToInt32(hdnTDSItemId.Value);

        bool tdsApplicable = false;

        string strTDSLedgerCode = "";

        strTDSLedgerCode = ddEditTDSLedgerCode.SelectedValue;

        if (ddEditTDSLedgerCode.SelectedIndex == 0)
        {
            lblError.Text = "Please Select TDS Ledger!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);

            return;
        }

        int TDSRateType = Convert.ToInt32(ddEditTDSRateType.SelectedValue);

        if (chkTDSYes.Checked)
        {
            if (ddEditTDSRateType.SelectedIndex != -1)
            {
                tdsApplicable = true;

                if (TDSRateType == 1) // Standard
                {
                    decTDSRate = Convert.ToDecimal(ddEditTDSRate.SelectedValue);
                }
                else
                {
                    decTDSRate = Convert.ToDecimal(txtEditTDSRate.Text.Trim());
                }

                // Calculate TDS Amount for invoice Item

                int Result = AccountExpense.UpdateInvoiceTDS(InvoiceId, ItemId, tdsApplicable, strTDSLedgerCode, TDSRateType, decTDSRate, LoggedInUser.glUserId);
                                
                if (Result == 0)
                {
                    lblError.Text = "TDS Details Updated!.";
                    lblError.CssClass = "success";

                    ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);

                    GridViewTDS.DataBind();
                    GridViewTDS.Visible = true;
                    GetPaymentRequest();

                }
                else
                {
                    lblError.Text = "TDS Calculation Error!. Please check Amount & Rate!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);

                    GridViewTDS.DataBind();
                    GridViewTDS.Visible = true;
                    GetPaymentRequest();
                }
            }
            else
            {
                lblError.Text = "Please Select TDS Rate!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);
            }
        }
        else
        {
            tdsApplicable = false;

            int Result = AccountExpense.AddInvoiceTDS(InvoiceId, tdsApplicable, "", 0, 0, LoggedInUser.glUserId);

            GetPaymentRequest();

            GridViewTDS.Visible = false;

            GridViewTDS.DataBind();
        }
    }

    protected void lnkRemoveTDS_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        int Result = AccountExpense.DeleteInvoiceTDS(ChargeId, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblError.Text = "TDS Details Removed!.";
            lblError.CssClass = "success";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);

            GetPaymentRequest();

            GridViewTDS.DataBind();

        }
        else
        {
            lblError.Text = "TDS Delete Error!!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);
        }

    }
    protected void ddEditTDSRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupTDS.Show();

        if (ddEditTDSRateType.SelectedValue == "1")
        {
            ddEditTDSRate.Visible = true;
            txtEditTDSRate.Visible = false;

            rfvddEditTDSRate.Enabled = true;
            rfvtxtEditTDSRate.Enabled = false;
        }
        else if (ddEditTDSRateType.SelectedValue == "2")
        {
            ddEditTDSRate.Visible = false;
            txtEditTDSRate.Visible = true;

            rfvddEditTDSRate.Enabled = false;
            rfvtxtEditTDSRate.Enabled = true;
        }
    }

    protected void btnCloseTDSPopup_Click(object sender, EventArgs e)
    {
        ModalPopupTDS.Hide();
    }
    #endregion
 
    #region RCM Applicable
    protected void chkRCMYes_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRCMYes.Checked)
        {
            GridViewRCM.Visible = true;
        }
        //else
        //{
        //    GridViewRCM.Visible = false;
        //}
    }
    protected void btnAddRCM_Click(object sender, EventArgs e)
    {
        decimal decRCMRate = 0m;

        int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        bool rcmApplicable = false;
        int RCMGstType = Convert.ToInt32(rbRCMGstType.SelectedValue);

        if (chkRCMYes.Checked)
        {
            if (ddRCMRate.SelectedIndex != -1)
            {
                // Calculate TDS Amount for invoice Item

                rcmApplicable = true;
                decRCMRate = Convert.ToDecimal(ddRCMRate.SelectedValue);

                int Result = AccountExpense.AddInvoiceRCM(InvoiceId, rcmApplicable, decRCMRate, RCMGstType,LoggedInUser.glUserId);

                if (Result == 0)
                {

                    lblError.Text = "RCM Details Updated!.";
                    lblError.CssClass = "success";

                    ScriptManager.RegisterStartupScript(this, GetType(), "RCM Success", "alert('" + lblError.Text + "');", true);

                    GetPaymentRequest();

                    GridViewRCM.Visible = true;

                    GridViewRCM.DataBind();
                }
                else
                {
                    lblError.Text = "RCM Calculation Error!. Please check Amount & Rate!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "RCM Error", "alert('" + lblError.Text + "');", true);

                    GetPaymentRequest();

                    GridViewRCM.Visible = true;

                    GridViewRCM.DataBind();
                }
            }
            else
            {
                lblError.Text = "Please Enter RCM Rate!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            rcmApplicable = false;
            int Result = AccountExpense.AddInvoiceRCM(InvoiceId, rcmApplicable, decRCMRate, RCMGstType,LoggedInUser.glUserId);

            GridViewRCM.Visible = false;

            GridViewRCM.DataBind();

            lblRCMTotalAmount.Text = "";
        }
    }

    #endregion

    #region RCM - EDIT
    protected void lnkEditRCM_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        DataSet dsItem = AccountExpense.GetInvoiceItemById(ChargeId);

        if (dsItem.Tables[0].Rows.Count > 0)
        {

            hdnRCMItemId.Value = dsItem.Tables[0].Rows[0]["lid"].ToString();

            lblRCMTaxableValue.Text = dsItem.Tables[0].Rows[0]["NetTaxableValue"].ToString();

            if (dsItem.Tables[0].Rows[0]["RCMRate"] != DBNull.Value)
            {
                ddEditRCMRate.SelectedValue = dsItem.Tables[0].Rows[0]["RCMRate"].ToString();
            }
            if (dsItem.Tables[0].Rows[0]["RCMIGSTAmt"] != DBNull.Value)
            {
                if(Convert.ToDecimal(dsItem.Tables[0].Rows[0]["RCMIGSTAmt"]) > 0)
                {
                    rbEditRCMGstType.SelectedValue = "1"; // IGST
                }
            }
            if (dsItem.Tables[0].Rows[0]["RCMCGSTAmt"] != DBNull.Value)
            {
                if (Convert.ToDecimal(dsItem.Tables[0].Rows[0]["RCMCGSTAmt"]) > 0)
                {
                    rbEditRCMGstType.SelectedValue = "2"; // CGST/IGST
                }
            }

        }

        ModalPopupRCM.Show();
    }
    protected void btnUpdateRCM_Click(object sender, EventArgs e)
    {
        decimal decRCMAmount = 0m;

        int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);
        int ItemId = Convert.ToInt32(hdnRCMItemId.Value);
                
        int RCMGstType = Convert.ToInt32(rbEditRCMGstType.SelectedValue);

        if (chkRCMYes.Checked)
        {
            if (ddRCMRate.SelectedIndex != -1)
            {
                // Calculate TDS Amount for invoice Item

               bool rcmApplicable = true;
               decimal decRCMRate = Convert.ToDecimal(ddEditRCMRate.SelectedValue);

                int Result = AccountExpense.UpdateInvoiceRCM(InvoiceId, ItemId, rcmApplicable, decRCMRate, RCMGstType, LoggedInUser.glUserId);

                GridViewRCM.Visible = true;

                GridViewRCM.DataBind();

                if (Result == 0)
                {
                    lblError.Text = "RCM Details Updated!.";
                    lblError.CssClass = "success";

                    ScriptManager.RegisterStartupScript(this, GetType(), "RCM Success", "alert('" + lblError.Text + "');", true);

                    GetPaymentRequest();
                }
                else
                {
                    lblError.Text = "RCM Calculation Error!. Please check Amount & Rate!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "RCM Error", "alert('" + lblError.Text + "');", true);
                }
            }
            else
            {
                lblError.Text = "Please Enter RCM Rate!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void btnCloseRCMPopup_Click(object sender, EventArgs e)
    {
        ModalPopupRCM.Hide();
    }
    #endregion

    private void AddJobProfit(int InvoiceId)
    {
        decimal decVendorBuyValue = 0m, decVendorSellValue = 0m;
        decimal decCustomerBuyValue = 0m, decCustomerSellValue = 0m;

        string strJbProfitRemark = "";

        decimal.TryParse(txtVendorBuyValue.Text.Trim(), out decVendorBuyValue);
        decimal.TryParse(txtVendorSellValue.Text.Trim(), out decVendorSellValue);
        decimal.TryParse(txtCustomerBuyValue.Text.Trim(), out decCustomerBuyValue);
        decimal.TryParse(txtCustomerSellValue.Text.Trim(), out decCustomerSellValue);

        strJbProfitRemark = txtJobProfitRemark.Text.Trim();

        AccountExpense.AddInvoiceJobProfit(InvoiceId, decVendorBuyValue, decVendorSellValue,
         decCustomerBuyValue, decCustomerSellValue, strJbProfitRemark, LoggedInUser.glUserId);
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

        int VendorBankAccountId = 0;

        string strRemark = "";

        decimal decNetPayable = 0m;
        Decimal decPaidAmount = 0m;

        InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        Decimal.TryParse(lblNetPayable.Text.Trim(), out decNetPayable);
        Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);

        PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (PaymentTypeId == 4) // RTGS
        {
            PaymentTypeId = 6; // NEFT
        }

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

        if(PaymentTypeId == 0)
        {
            lblError.Text = "Please Select Payment Type!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;

            return SuccessId;
        }

        if(PaymentTypeId > 1 && PaymentTypeId < 90)
        {
            // Payment From Bank

            if(BabajiBankId == 0)
            {
                lblError.Text = "Please Select Babaji Bank Name!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 2;
            }
        }

        if(BankAccountId == 0 )
        {
            lblError.Text = "Please Select Bank Account!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }

        strRemark = "";

        if(decNetPayable == decPaidAmount)
        {
            IsFullPayment = true;
        }

        if (decNetPayable <= 0)
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

            SuccessId  = 2; 
        }
        else if (decPaidAmount > decNetPayable)
        {
            lblError.Text = "Entered Paid Amount Exceed net Payable Amount! Please Check Paid Amount.!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('"+ lblError.Text + "');", true);

            SuccessId = 2;
        }
        else if (SuccessId == 0)
        {
            if(IsFundTransFromAPI == true)
            {
                if(decPaidAmount <=200000)
                {
                    PaymentTypeId = 6;// NEFT
                }
                else
                {
                    PaymentTypeId = 4;// RTGS
                }
            }

            int PayResult = AccountExpense.AddInvoicePaymentRequest2(InvoiceId, IsFullPayment, PaymentTypeId, BabajiBankId, BankAccountId, 
                IsFundTransFromAPI, VendorBankAccountId, decPaidAmount, CurrencyId, decCurrencyRate, LoggedInUser.glUserId);

            if (PayResult > 0)
            {
                lblError.Text = "Payment Detail Added Successfully!";
                lblError.CssClass = "success";

                SuccessId =0;
            }
            else if (PayResult == 0)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 1;
            }
            else if (PayResult == -1)
            {
                lblError.Text = "Entered Paid Amount Exceed net Payable Amount! Please Check Net Payable Amount.";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 2;
            }
        }

        return SuccessId;
    }
    protected void ddTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddTransactionType.SelectedIndex > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "RCM Required", "alert('RCM will be required for Transaction Type');", true);
        }
    }
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(rblPayment.SelectedValue == "1")
        {
            txtPayAmount.Text = lblNetPayable.Text.Trim();

            txtPayAmount.Enabled = false;
        }
        else
        {
            txtPayAmount.Enabled = true;
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
        else if(PaymentType > 1) // Bank
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

        DataSet dsVendorBank  = AccountExpense.GetVendorBankDetail(VendorBankId);

        if(dsVendorBank.Tables.Count > 0)
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

        if(ddBabajiBankName.SelectedIndex != -1)
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
        //  rblFundTransferFromLiveTracking.SelectedValue = "0";

        if (rblFundTransferFromLiveTracking.SelectedValue == "1")
        {
            string strPopupmessage = "Fund Transfer will be initiated from Live Tracking.";
            ScriptManager.RegisterStartupScript(this, GetType(), "Fund Transfer", "alert('" + strPopupmessage + "');", true);
        }
    }

    #region Hold Reject Email
    private bool SendHoldRejectMail(int InvoiceID, string strStatusName, string strRemark)
    {
        string MessageBody = "", strCCEmail = "", strSubject = "";

        bool bEmailSucces = false;
        strCCEmail = EmailConfig.GetEmailAccountCCTo();

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

                bEmailSucces = EMail.SendMailCC(LoggedInUser.glUserName, strCreatedByEmail, strCCEmail, strSubject, EmailBody, "");
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
    private bool SendRequestMail(int InvoiceID)
    {
        string MessageBody = "", strCCEmail = "", strSubject = "";
        string strMgmtEmail = "Dhaval@babajishivram.com";
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

        string strRemark = dsDetail.Tables[0].Rows[0]["Remark"].ToString();

        string EmailContent = "";

        try
        {
            try
            {
                string strFileName = System.Web.HttpContext.Current.Server.MapPath("~") + "//EmailTemplate/EmailVendorPaymentRequest.txt";

                //  string strFileName = "../EmailTemplate/EmailVendorPaymentReject.txt";

                EmailContent = File.ReadAllText(strFileName);
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
            MessageBody = MessageBody.Replace("@Remarks", strRemark);
            MessageBody = MessageBody.Replace("@UserName", LoggedInUser.glEmpName);

            ///////////////////////////////////////////////////////////////////////////////

            try
            {
                strSubject = "Credit Vendor Payment Request Amount-" + strTotalInvoiceValue + "/" + strJobNumber + "/ " + strInvoiceNo + "/ " + strConsignee + " / " + strExpenseType;

                string EmailBody = MessageBody;

                bEmailSucces = EMail.SendMailCC(LoggedInUser.glUserName, strMgmtEmail, strCCEmail, strSubject, EmailBody, "");
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
            
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    #endregion
}