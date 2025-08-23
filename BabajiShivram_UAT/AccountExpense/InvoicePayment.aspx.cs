using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
public partial class AccountExpense_InvoicePayment : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSavePayment);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Payment";

        MskEdtValInstrumentDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        CalExtInstrumentDate.EndDate = DateTime.Now;

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoicePayment.aspx");
        }

        if (!IsPostBack)
        {
            GetPaymentRequest();
        }
    }
    private void GetPaymentRequest()
    {
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoicePayment.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        int StatusId = 0;
        int ExpenseTypeId = 0;

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);

            GetPendingInvoicePayment(StatusId);
            lblLTRefNo.Text = "LT" + InvoiceID.ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblContainerCount.Text = dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            lblWeight.Text = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            hdnInvoiceType.Value = dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString();
            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");

            lblJBNo.Text = dsDetail.Tables[0].Rows[0]["BJVJBNumber"].ToString();
            lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();

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
            if (dsDetail.Tables[0].Rows[0]["IECNo"] != DBNull.Value)
            {
                lblIECNo.Text = dsDetail.Tables[0].Rows[0]["IECNo"].ToString();
            }
            
            if (dsDetail.Tables[0].Rows[0]["PortCode"] != DBNull.Value)
            {
                lblPortCode.Text = dsDetail.Tables[0].Rows[0]["PortCode"].ToString();
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
                        
            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
            {
                lblTDSAmount.Text = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
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
                lblPaymentRequestDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentTypeId"] != DBNull.Value)
            {
               // ddlPaymentType.SelectedValue = dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
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

            // Performa Invoice -- Invoice Charge and RCM Not Required

            // RCM Detail
            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["RCMApplicable"]) == true)
            {
                fldRCMItem.Visible = true;
                lblRCMYes.Text = "Yes";
                lblRCMRate.Text = dsDetail.Tables[0].Rows[0]["RCMRate"].ToString();
                lblRCMTotalAmount.Text = dsDetail.Tables[0].Rows[0]["RCMTotalAmount"].ToString();
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

                Decimal decTotalPayable = (decFinalPayment - decProformaPayment- decProformaTDSAmount);

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

            // Proforma Invoice Detail against Final Invoice

            if (dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1") // Final)
            {
                if (dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"] != DBNull.Value)
                {
                    int ProformaInvoiceId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"]);

                    if (ProformaInvoiceId > 0)
                    {
                        // Show Proforma Invoice Detail
                        // Show Proforma Invoice Payment History
                        GetProformaPaymentRequest(ProformaInvoiceId);
                    }
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
    private void GetPendingInvoicePayment(int StatusId)
    {
        // StatusId - 151 - Partial Payment
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        DataSet dsPaymentDetail = AccountExpense.GetInvoicePendingPayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            hdnPaymentId.Value = dsPaymentDetail.Tables[0].Rows[0]["lid"].ToString();
            txtPayAmount.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
         
         // lblNetPayable.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
         // hdnNetPayableAmount.Value = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();

            ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

            if (dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString() == "5")
            {
                RFVDocument.Enabled = true;
            }

            //  Vendor Bank Account

            if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            {
                hdnVendorBankAccountId.Value = dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"].ToString();

                GetVendorBankDetail(Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]));
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

            int PaymentTypeId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"]);
            int BabajiBankID = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankId"]);
            int BankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"]);
            // Payment Mode
            // Fill Bank/Cash Code
            if (PaymentTypeId == 1) // Cash
            {
                AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2); // Cash Book
            }
            else if (PaymentTypeId == 90) // Security Deposit
            {
                AccountExpense.FillBankBookByType(ddBabajiBankAccount, 3); // Security Deposit

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                txtInstrumentNo.Text = "S"+unixTimestamp.ToString();
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

            // Vendor Account Detail

            //if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            //{
            //    int VendorBankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]);

            //    DataSet dsVendorBankDetal = AccountExpense.GetVendorBankDetail(VendorBankAccountId);

            //    if(dsVendorBankDetal.Tables.Count > 0)
            //    {
            //        if (dsVendorBankDetal.Tables[0].Rows.Count > 0)
            //        {
            //            lblBankName.Text = dsVendorBankDetal.Tables[0].Rows[0]["BankName"].ToString();
            //            lblBankAccountNo.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountNo"].ToString();
            //            lblBankIFSC.Text = dsVendorBankDetal.Tables[0].Rows[0]["IFSCCode"].ToString();
            //        }
            //    }
            //}
        }
        else if(StatusId == 151)
        {
            dsPaymentDetail = AccountExpense.GetInvoicePayment(InvoiceID);

            if (dsPaymentDetail.Tables[0].Rows.Count > 0)
            {
                hdnPaymentId.Value      =   "0"; // New Payment Request
                rblPayment.Enabled      =   true;
                txtPayAmount.Enabled    =   true;
                ddlPaymentType.Enabled  =   true;
                ddBabajiBankName.Enabled    =   true;
                ddBabajiBankAccount.Enabled = true;

                ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

                int PaymentTypeId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"]);
                int BabajiBankID = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankId"]);
                int BankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"]);

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

                // Payment Mode -- Cash, Bank Account

                // Fill Bank/Cash Code
                if (PaymentTypeId == 1) // Cash
                {
                    AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2); // Cash Book
                }
                else if (PaymentTypeId == 90) // Security Deposit
                {
                    AccountExpense.FillBankBookByType(ddBabajiBankAccount, 3); // Security Deposit Book

                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    txtInstrumentNo.Text = "S" + unixTimestamp.ToString();
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
                else
                {
                    ddCurrency.SelectedValue = "46";
                }
                if (dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyRate"] != DBNull.Value)
                {
                    txtExchangeRate.Text = dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyRate"].ToString();
                }
                else
                {
                    txtExchangeRate.Text = "1";
                }
            }
            else
            {
                hdnPaymentId.Value = "0"; // New Payment Request
                rblPayment.Enabled = true;
                txtPayAmount.Enabled = true;
                ddlPaymentType.Enabled = true;
                ddCurrency.Enabled = true;
                txtExchangeRate.Enabled = true;
                ddCurrency.SelectedValue = "46"; //INT
                txtExchangeRate.Text = "1";

                ddBabajiBankName.Enabled = true;
                ddBabajiBankAccount.Enabled = true;

            }
        }
    }
    private void GetVendorBankDetail(int VendorBankId)
    {
        // Get Vendor Bank Detail

        DataSet dsVendorBank = AccountExpense.GetVendorBankDetail(VendorBankId);

        if (dsVendorBank.Tables.Count > 0)
        {
            if (dsVendorBank.Tables[0].Rows.Count > 0)
            {
                lblVendorBankName.Text = dsVendorBank.Tables[0].Rows[0]["BankName"].ToString();
                lblVendorBankAccountName.Text = dsVendorBank.Tables[0].Rows[0]["AccountName"].ToString();
                lblVendorBankAccountNo.Text = dsVendorBank.Tables[0].Rows[0]["AccountNo"].ToString();
                lblVendorBankAccountIFSC.Text = dsVendorBank.Tables[0].Rows[0]["IFSCCode"].ToString();
                lblVendorBankAccountType.Text = dsVendorBank.Tables[0].Rows[0]["AccountTypeName"].ToString();
                lblVendorBankRemark.Text = dsVendorBank.Tables[0].Rows[0]["Remark"].ToString();
            }
        }

    }
    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        int InvoiceId = 0, PaymentId = 0;
        string strInstrumentNo = "", strDocPath = "", strRemark = "";

        Decimal decPaidAmount = 0, decNetPayable = 0;

        DateTime dtInstrumentDate = DateTime.MinValue;

        PaymentId = Convert.ToInt32(hdnPaymentId.Value);
        InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);
        Decimal.TryParse(lblNetPayable.Text.Trim(), out decNetPayable);

        strInstrumentNo = txtInstrumentNo.Text.Trim();

        if (strInstrumentNo == "")
        {
            lblError.Text = "Please Enter Instrument No!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            return;
        }

        if (txtInstrumentDate.Text != "")
        {
            dtInstrumentDate = Commonfunctions.CDateTime(txtInstrumentDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter Instrument Date!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('"+ lblError.Text + "');", true);

            return;
        }
        //if (fileUploadPaymentDocument.HasFile == false)
        //{
        //    lblError.Text = "Please Upload Payment Document!";
        //    lblError.CssClass = "errorMsg";

        //    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

        //    return;
        //}
        if (decNetPayable <= 0)
        {
            lblError.Text = "Invalid Net Payable Amount!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            return;
        }
        if (decPaidAmount <= 0)
        {
            lblError.Text = "Invalid Payment Amount!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('"+lblError.Text+"');", true);

            return;
        }

        if (decPaidAmount > decNetPayable)
        {
            lblError.Text = "Paid Amount is Greater then Net Payable Amount!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            return;
        }

        strRemark = txtPaymentRemark.Text.Trim();

        if(hdnPaymentId.Value == "0")
        {
            // Create New Payment Request
           int SuccessId  = UpdatePaymentRequest();

            if(SuccessId < 0)
            {
                // Payment Request Error
                return;
            }
        }

        PaymentId = Convert.ToInt32(hdnPaymentId.Value);

        int PayResult = AccountExpense.AddInvoicePayment(InvoiceId, PaymentId, strInstrumentNo, dtInstrumentDate, 
            decPaidAmount, strDocPath, strRemark, LoggedInUser.glUserId);

        if (PayResult == 0)
        {
            bool IsMailSuccess = SendPaymentConfirmationMail(InvoiceId, strInstrumentNo, dtInstrumentDate.ToShortDateString(), decPaidAmount.ToString());
            AccountExpense.UpdateBankPaymentMailStatus(PaymentId, IsMailSuccess);

            lblError.Text = "Payment Details Updated Successfully!";
            lblError.CssClass = "success";
            // Upload Document

            string strDirPath = lblJobNumber.Text.Trim().Replace("/", "");

            strDirPath = strDirPath.Replace("-", "");

            string strInvoiceFilePath = "VendorInvoice//" + strDirPath + "//";

            if (fileUploadPaymentDocument.HasFile)
            {
                string FileName10 = UploadDocument(fileUploadPaymentDocument, strInvoiceFilePath);

                int FileOutput1 = AccountExpense.AddInvoiceDocument(InvoiceId, 10, strInvoiceFilePath, FileName10, LoggedInUser.glUserId);
            }

            Session["Success"] = lblError.Text;

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (PayResult == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('"+ lblError.Text + "');", true);
        }
        else if (PayResult == 2)
        {
            lblError.Text = "Entered Paid Amount Exceed Net Payable Amount! Please Check Paid Amount.";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('"+ lblError.Text + "');", true);
        }
        else if (PayResult == 4)
        {
            lblError.Text = "Payment Already Completed! Please Check Payment History.";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
        }


        // Yes Bank API Payment

        /*****************************************************************
        BNRespStartTransfer PayResponse = new BNRespStartTransfer();
        PayResponse = TransferFund(decPaidAmount);

        if (PayResponse.statusCode.ToLower() == "ok")
        {
           int TransactionId = AccountExpense.AddPaymentAPIResponse(InvoiceId,Convert.ToInt32(PayResponse.attemptNo),PayResponse.reqTransferType,
               PayResponse.requestReferenceNo, PayResponse.statusCode, PayResponse.subStatusCode, PayResponse.subStatusText,
               PayResponse.uniqueResponseNo,LoggedInUser.glUserId);
        }
        else
        {
            string respError = PayResponse.statusCode;
            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('"+ respError + "');", true);
        }

        *****************************************************************/
    }
    protected void btnHold_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRejectRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 149, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice Payment On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = lblError.Text;

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Payment Already On Hold!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Hold Error", "alert('" + lblError.Text + "');", true);
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
        string strRemark = txtRejectRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 148, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice Payment Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = lblError.Text;

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Payment Already Rejected!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Reject Error", "alert('" + lblError.Text + "');", true);
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }
    private bool CheckBankTime(int PaymentMode)
    {
        bool isValidTime = true;

        TimeSpan tsToday = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 00);

        TimeSpan tsMaxTime = new TimeSpan(17, 30, 00); // 05:30 PM - RTGS ENDs

        if (tsToday > tsMaxTime && PaymentMode == 4)
        {
            lblError.Text = "RTGS Not Allowed After 05:30 PM Please Change Payment Mode To NEFT";

            isValidTime = false;
        }

        return isValidTime;
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

        decimal decNetPayable = 0m;
        Decimal decPaidAmount = 0m;

        InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        Decimal.TryParse(lblNetPayable.Text.Trim(), out decNetPayable);
        Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);

        PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);

        BankAccountId = Convert.ToInt32(ddBabajiBankAccount.SelectedValue);

        CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);

        Decimal.TryParse(txtExchangeRate.Text.Trim(), out decCurrencyRate);

        if (ddBabajiBankName.SelectedIndex != -1)
        {
            BabajiBankId = Convert.ToInt32(ddBabajiBankName.SelectedValue);
        }

        if (hdnVendorBankAccountId.Value  != "")
        {
            VendorBankAccountId = Convert.ToInt32(hdnVendorBankAccountId.Value);
        }


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
            lblError.Text = "Entered Paid Amount Exceed net Payable Amount! Please Check Paid Amount.!"+ decNetPayable.ToString();
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }
        else
        {
            int PayResult = AccountExpense.AddInvoicePaymentRequest2(InvoiceId, IsFullPayment, PaymentTypeId, BabajiBankId, BankAccountId,
                IsFundTransFromAPI, VendorBankAccountId, decPaidAmount, CurrencyId, decCurrencyRate, LoggedInUser.glUserId);

            if (PayResult > 0)
            {
                lblError.Text = "Payment Detail Added Successfully!";
                lblError.CssClass = "success";

                hdnPaymentId.Value = PayResult.ToString();

                SuccessId = 0;
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
   
    #region On_Change_Event
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
        txtInstrumentNo.Text = "";
        int PaymentType = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (PaymentType == 1) // Cash
        {
            AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2);

            // Clear Bank Name

            ddBabajiBankName.Items.Clear();

            // Clear Bank Account;

        }
        if (PaymentType == 90) // Security Deposit
        {
            AccountExpense.FillBankBookByType(ddBabajiBankAccount, 3);

            // Auto Generate Instrument No

            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            txtInstrumentNo.Text = "S"+unixTimestamp.ToString();

            // Clear Bank Name

            ddBabajiBankName.Items.Clear();

            // Clear Bank Account;

        }
        else if (PaymentType > 1) // Bank
        {
            // File Bank Name MS

            AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name

            // Clear Cash Book

            ddBabajiBankAccount.Items.Clear();
        }
    }
    protected void ddBabajiBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int BabajiBankID = Convert.ToInt32(ddBabajiBankName.SelectedValue);

        AccountExpense.FillBankAccountByBankId(ddBabajiBankAccount, BabajiBankID);
    }

    #endregion

    public string UploadDocument(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FileName;
        }
        else
        {
            FileName = "";

            return FileName;
        }
    }
    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }

        return builder.ToString();
    }
   
    #region Payment Email

    private bool SendPaymentConfirmationMail(int InvoiceID, string strUTRNo, string strUTRDate, string strPaidAmount)
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

        string strTDSTotalAmount = "0";

        string strCreatedByEmail = "amit.bakshi@BabajiShivram.com";

        if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
        {
            strTDSTotalAmount = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
        }

        if (dsDetail.Tables[0].Rows[0]["CreatedByEmail"] != DBNull.Value)
        {
            strCreatedByEmail = dsDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
        }

        string strPaymentUser = LoggedInUser.glEmpName;

        string strTotalPaidAmount = txtPayAmount.Text.Trim();

        string strPaymentRemark = txtPaymentRemark.Text.Trim();

        string EmailContent = "";

        try
        {

            try
            {
                string strFileName = "../EmailTemplate/EmailVendorPaymentConfirmation.txt";

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
            MessageBody = MessageBody.Replace("@TDSDeductedAmount", strTDSTotalAmount);
            MessageBody = MessageBody.Replace("@TotalPaidAmount", strPaidAmount);
            MessageBody = MessageBody.Replace("@ChequeUTRNo", strUTRNo);
            MessageBody = MessageBody.Replace("@ChequeUTRDate", strUTRDate);
            MessageBody = MessageBody.Replace("@Remarks", strPaymentRemark);
            MessageBody = MessageBody.Replace("@PaymentUserName", strPaymentUser);


            ///////////////////////////////////////////////////////////////////////////////

            try
            {
                strSubject = "Vendor Payment Detail /" + strJobNumber + "/ " + strInvoiceNo + "/ " + strConsignee + " / " + strExpenseType;

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


    #endregion
}