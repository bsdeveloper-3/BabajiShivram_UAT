using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
public partial class AccountExpense_InvoiceDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Detail";

        if (rblInvoiceMode.SelectedIndex != -1)
        {
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");
        }

        if (Session["InvoiceIdTrack"] == null)
        {
            Session["JobId"] = null;
            Response.Redirect("InvoiceTracking.aspx");
        }

        if (!IsPostBack)
        {
            Session["JobId"] = null;

            GetPaymentRequest(Convert.ToInt32(Session["InvoiceIdTrack"]));

            HtmlAnchor hrefGoBack = (HtmlAnchor)Page.Master.FindControl("hrefGoBack");
            if (hrefGoBack != null)
            {
                if (Request.UrlReferrer != null)
                {
                    int startIndex = Request.UrlReferrer.AbsolutePath.LastIndexOf("/");

                    if (startIndex > 0)
                    {
                        string strReturnURL = Request.UrlReferrer.AbsolutePath;

                        hrefGoBack.HRef = strReturnURL;
                    }
                }

                //hrefGoBack.HRef = "AccountExpense/InvoiceTracking.aspx";
            }
        }
    }
    private void GetPaymentRequest(int InvoiceID)
    {
        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        int StatusId = 0;
        int ExpenseTypeId = 0;
        int HOIDID = 0;
        
        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);

            lblLTRefNo.Text = "LT" + InvoiceID.ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            hdnJobId.Value = dsDetail.Tables[0].Rows[0]["JobId"].ToString();
            hdnModuleId.Value = dsDetail.Tables[0].Rows[0]["ModuleID"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblWeight.Text = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            lblContainerCount.Text = dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            lblJBNo.Text = dsDetail.Tables[0].Rows[0]["BJVJBNumber"].ToString();

            hdnBillType.Value = dsDetail.Tables[0].Rows[0]["BillType"].ToString();
            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");

            if (hdnBillType.Value == "3")
            {
                HOIDID = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["HODID"]);

                fldVendorBuySell.Visible = true;

                if (LoggedInUser.glUserId == 1 | LoggedInUser.glUserId == 3 || LoggedInUser.glUserId == HOIDID)
                {
                    fldVendorJobMargin.Visible = true;
                    fldOverAllJobMargin.Visible = true;
                }

                lblError.Text = "Credit Vendor Payment";

                // Credit Vendor Job Buy/Sell Detail

                DataSet dsJobProfitDetail = AccountExpense.GetInvoiceJobProfit(InvoiceID);
                DataSet dsBilledMargin = BillingOperation.FAGetJobExpenseBilled(lblJobNumber.Text);
                lblInvoiceValue1.Text = "0.0";
                if (dsJobProfitDetail.Tables[0].Rows.Count > 0)
                {
                    txtVendorBuyValue.Text      = dsJobProfitDetail.Tables[0].Rows[0]["VendorBuyValue"].ToString();
                    txtVendorSellValue.Text     = dsJobProfitDetail.Tables[0].Rows[0]["VendorSellValue"].ToString();
                    txtCustomerBuyValue.Text    = dsJobProfitDetail.Tables[0].Rows[0]["CustomerBuyValue"].ToString();
                    txtCustomerSellValue.Text   = dsJobProfitDetail.Tables[0].Rows[0]["CustomerSellValue"].ToString();
                    txtJobProfitRemark.Text     = dsJobProfitDetail.Tables[0].Rows[0]["Remark"].ToString();

                    // Job Margin

                    lblInvoiceValue1.Text   = dsJobProfitDetail.Tables[0].Rows[0]["TaxAmount"].ToString();
                    lblSellingAmount1.Text  = dsJobProfitDetail.Tables[0].Rows[0]["VendorSellValue"].ToString();
                    lblMargin.Text          = dsJobProfitDetail.Tables[0].Rows[0]["JobMargin"].ToString();

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
                    else if (decTotalDebit > 0 && decTotalCredit == 0)
                    {
                        decOverAllMarginPercent = (decOverAllMargin * 100) / decTotalDebit;
                    }
                    else if (decTotalDebit == 0 && decTotalCredit == 0)
                    {
                        // DO NOTHING
                    }
                    else if (decTotalDebit > 0 && decOverAllMargin < 0)
                    {
                        decOverAllMarginPercent = (decOverAllMargin * 100) / decTotalDebit;
                    }

                    if (decOverAllMargin <= 0)
                    {
                        lblBillMarginPercent.Text = decOverAllMarginPercent.ToString("0.##") + "% LOSS";
                        lblBillMarginPercent.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblBillMarginPercent.Text = decOverAllMarginPercent.ToString("0.##") + "%";
                        lblBillMarginPercent.ForeColor = System.Drawing.Color.Green;
                    }

                    lblTotalCost.Text = decTotalDebit.ToString("0.##");
                    lblBilledAmount.Text = decTotalCredit.ToString("0.##");
                    lblBillMargin.Text = decOverAllMargin.ToString("0.##");

                }
            }
            else if (hdnBillType.Value == "0")
            {
                lblError.Text = "Vendor Payment";
            }
            else if (hdnBillType.Value == "10")
            {
                lblError.Text = "Blank Cheque Payment";
            }
            else if (hdnBillType.Value == "11")
            {
                lblError.Text = "PD Account Payment";
            }
            
            /******************************************/


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
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                lblExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["PaidAmount"] != DBNull.Value)
            {
                lblPaidAmount.Text = dsDetail.Tables[0].Rows[0]["PaidAmount"].ToString();

                //if(lblPaidAmount.Text !="0")
                //{
                //    fldPaymentHistory.Visible = true;
                //}
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

            //if (dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1")
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
                           // lblCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
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

                        lblError.Text = "Proforma To Final Invoice";

                        // Proforma To Final  - Payment detail Not Required

                        //fldPaymentHistory.Visible = false;

                    }
                }
            }
            // Performa Invoice -- Invoice Charge and RCM Not Required

            // RCM DetailGetInvoicePayment
            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["RCMApplicable"]) == true)
            {
                fldRCMItem.Visible = true;
                GridViewRCM.Visible = true;
                lblRCMYes.Text = "Yes";
                lblRCMRate.Text = dsDetail.Tables[0].Rows[0]["RCMRate"].ToString();
                lblRCMTotalAmount.Text = dsDetail.Tables[0].Rows[0]["RCMTotalAmount"].ToString();
            }

            // Payment Detail - 
            GetInvoicePayment();
        }

    }

    private void GetInvoicePayment()
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceIdTrack"]);
        int VendorBankAccountId = 0;
        DataSet dsPaymentDetail;

        // Get Papyment Completed
        dsPaymentDetail = AccountExpense.GetInvoicePayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            // Vendor Account Detail

            if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            {
                VendorBankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]);
            }
        }
        else
        {
            dsPaymentDetail = AccountExpense.GetInvoicePendingPayment(InvoiceID);
            
            if (dsPaymentDetail.Tables[0].Rows.Count > 0)
            {
                // Vendor Account Detail

                if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
                {
                    VendorBankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]);
                }
            }
        }

        if (VendorBankAccountId> 0 )
        {
            DataSet dsVendorBankDetal = AccountExpense.GetVendorBankDetail(VendorBankAccountId);

            if (dsVendorBankDetal.Tables.Count > 0)
            {
                if (dsVendorBankDetal.Tables[0].Rows.Count > 0)
                {
                    lblVendorBankName.Text = dsVendorBankDetal.Tables[0].Rows[0]["BankName"].ToString();
                    lblVendorBankAccountName.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountName"].ToString();
                    lblVendorBankAccountNo.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountNo"].ToString();
                    lblVendorBankAccountIFSC.Text = dsVendorBankDetal.Tables[0].Rows[0]["IFSCCode"].ToString();
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
    protected void gvPaymentHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "IsSuccess") != DBNull.Value)
            {
                bool IsSuccess = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsSuccess"));

                if (IsSuccess == false) // Transaction Failed - 
                {
                    e.Row.BackColor = System.Drawing.Color.Red;  //LightSalmon;    
                }
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;  //LightSalmon;    
            }
        }
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
    
    protected void lnkJobDetail_Click(object sender, EventArgs e)
    {
        if (hdnModuleId.Value == "1")
        {
            Session["JobIdV"] = hdnJobId.Value;
            Response.Redirect("JobView.aspx");
        }
        else if (hdnModuleId.Value == "2")
        {
            Session["JobIdV"] = hdnJobId.Value;
            Response.Redirect("JobViewFr.aspx");
        }
        else if (hdnModuleId.Value == "5")
        {
            Session["JobIdV"] = hdnJobId.Value;
            Response.Redirect("JobViewEx.aspx");
        }
    }

    #region Credit Vendor
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

    protected void gvPaymentHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "sendpaymentemail")
        {
            Int32 Invoiceid = Convert.ToInt32(Session["InvoiceIdTrack"]);

            Int32 PaymentId = Convert.ToInt32(e.CommandArgument.ToString());

            int Result = AccountExpense.SendPaymentEmail(Invoiceid, PaymentId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Payment Email Sent Successfully!";
                lblError.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblError.Text = "Email Sending Error!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Payment details Not Found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    #endregion
}