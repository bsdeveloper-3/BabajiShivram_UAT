using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using BankAPI.Open;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;

public partial class AccountExpense_InvoicePaymentAPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Payment - API";

       // MskEdtValInstrumentDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
       // CalExtInstrumentDate.EndDate = DateTime.Now;

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoicePaymentAPI.aspx");
        }
        if (LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 3 || LoggedInUser.glUserId == 1023 || LoggedInUser.glUserId == 13478)
        {
        }
        else
        {
            Session["InvoiceId"] = null;
            Response.Redirect("../Error.aspx");
        }
        if (!IsPostBack)
        {
            if (CheckCertificate() == false)
            {
                Session["InvoiceId"] = null;
                Response.Redirect("../Error.aspx");
            }
            else
            {
                GetPaymentRequest();
            }

           // GetPaymentRequest();
        }
    }
    private void GetPaymentRequest()
    {
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoicePaymentAPI.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        int StatusId = 0;
        int ExpenseTypeId = 0;

        DataSet dsDetail = AccountExpense.GetInvoiceDetailForPayment(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);
                        
            if (StatusId == 149 || StatusId == 145 || StatusId == 139)
            {
                GetPendingInvoicePayment(StatusId);

                // Check Payment Limit -  Max 20L For Neeraj

                Decimal decMaxLimit = 9000000;

                Decimal decPayAmt = 0;

                Decimal.TryParse(txtPayAmount.Text.Trim(), out decPayAmt);

                if (decPayAmt >= decMaxLimit && LoggedInUser.glUserId != 3)
                {
                    // Allow decMaxLimit 20L for Dhaval

                    if (LoggedInUser.glUserId == 13478)
                    {
                        // allow for A/C
                    }
                    else
                    {
                        btnSavePayment.Visible = false;

                        lblError.Text = "Not Authorised for Payment Above 20 Lac!";
                        lblError.CssClass = "errorMsg";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                        Session["InvoiceId"] = null;
                    }
                }
            }
            else
            {
                btnSavePayment.Visible = false;

                lblError.Text = "Session Expired! Please Login Again!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                Session["InvoiceId"] = null;
            }

            lblJobNumber.Text   = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            txtPaymentRemark.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            lblCustomer.Text    = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text   = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text        = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text        = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblContainerCount.Text = dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            lblWeight.Text      = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            hdnInvoiceType.Value = dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString();

            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");

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
                        if (dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"] != null)
                        {
                            lblCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                        }

                    }
                }
            }


            // TDS Detail

            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["TDSApplicable"]) == true)
            {
                fldTDSItem.Visible = true;
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
            }
        }
        else
        {
            lblError.Text = "Invoice Payment Already InProcess!";
            lblError.CssClass = "errorMsg";

            Session["InvoiceId"] = null;
            btnSavePayment.Visible = false;
        }
    }
    private void GetPendingInvoicePayment(int StatusId)
    {
        // StatusId - 151 - Partial Payment
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        DataSet dsPaymentDetail = AccountExpense.GetInvoicePendingPayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0 && (StatusId == 145 || StatusId == 149 || StatusId == 139)) // Audit L2 Completed OR Payment ON Hold
        {
            hdnPaymentId.Value = dsPaymentDetail.Tables[0].Rows[0]["lid"].ToString();
            txtPayAmount.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            lblNetPayable.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            hdnNetPayableAmount.Value = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();

            ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            
            CheckBankTime(Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"]));

            //ddAPITransferType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

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

            // Payment Mode
            if (ddlPaymentType.SelectedValue != "0")
            {
                // Fill Bank/Cash Code
                if (ddlPaymentType.SelectedValue == "1")
                {
                    AccountExpense.FillBankBookByType(ddBabajiBank, 2); // Cash Book
                }
                else
                {
                    AccountExpense.FillBankBookByType(ddBabajiBank, 1); // Bank Book
                }

                if (dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"] != DBNull.Value)
                {
                    ddBabajiBank.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["BankAccountId"].ToString();
                    lblBabajiAccountNo.Text = dsPaymentDetail.Tables[0].Rows[0]["AccountNo"].ToString();
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

                DataSet dsVendorBankDetal = AccountExpense.GetVendorBankDetail(VendorBankAccountId);

                if (dsVendorBankDetal.Tables.Count > 0)
                {
                    if (dsVendorBankDetal.Tables[0].Rows.Count > 0)
                    {
                        lblBeneficiaryName.Text= dsVendorBankDetal.Tables[0].Rows[0]["AccountName"].ToString();
                        lblBankName.Text = dsVendorBankDetal.Tables[0].Rows[0]["BankName"].ToString();
                        lblBankAccountNo.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountNo"].ToString();
                        lblBankIFSC.Text = dsVendorBankDetal.Tables[0].Rows[0]["IFSCCode"].ToString();

                        /****************IMPS N.A.*************
                        if (dsVendorBankDetal.Tables[0].Rows[0]["IsIMPS"] != DBNull.Value)
                        {
                            Decimal decPaidAmountA = 0;
                            Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmountA);

                            bool IsIMPS = Convert.ToBoolean(dsVendorBankDetal.Tables[0].Rows[0]["IsIMPS"]);

                            if (IsIMPS == false && decPaidAmountA < 200000)
                            {
                                btnSavePayment.Visible = false;

                                lblError.Text = "Vendor Bank Not Accepting IMPS Payment! Please Pay from Other Bank";
                                lblError.CssClass = "errorMsg";

                                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                            }
                        }

                        *****************************************/
                    }
                }
            }
        }
        else
        {
            hdnPaymentId.Value = "0";
            txtPayAmount.Text = "0";
            lblNetPayable.Text = "0";
            hdnNetPayableAmount.Value = "0";

            btnSavePayment.Visible = false;

            Session["InvoiceId"] = null;

            lblError.Text = "Invoice Payment Error!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
        }

        //else if (StatusId == 151)
        //{
        //    dsPaymentDetail = AccountExpense.GetInvoicePayment(InvoiceID);

        //    if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        //    {
        //        hdnPaymentId.Value = "0";
        //        rblPayment.Enabled = true;
        //        txtPayAmount.Enabled = true;
        //        ddlPaymentType.Enabled = true;
        //        ddBabajiBank.Enabled = true;
        //        ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

        //        if (dsPaymentDetail.Tables[0].Rows[0]["IsFullPayment"] != DBNull.Value)
        //        {
        //            if (Convert.ToBoolean(dsPaymentDetail.Tables[0].Rows[0]["IsFullPayment"]) == true)
        //            {
        //                rblPayment.SelectedValue = "1";
        //            }
        //            else
        //            {
        //                rblPayment.SelectedValue = "0";
        //            }
        //        }

        //        // Payment Mode
        //        if (ddlPaymentType.SelectedValue != "0")
        //        {
        //            // Fill Bank/Cash Code
        //            if (ddlPaymentType.SelectedValue == "1")
        //            {
        //                AccountExpense.FillBankBookByType(ddBabajiBank, 2); // Cash Book
        //            }
        //            else
        //            {
        //                AccountExpense.FillBankBookByType(ddBabajiBank, 1); // Bank Book
        //            }

        //            if (dsPaymentDetail.Tables[0].Rows[0]["BankId"] != DBNull.Value)
        //            {
        //                ddBabajiBank.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["BankId"].ToString();
        //            }
        //        }

        //        // Currency
        //        if (dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyId"] != DBNull.Value)
        //        {
        //            ddCurrency.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyId"].ToString();
        //        }
        //        else
        //        {
        //            ddCurrency.SelectedValue = "46";
        //        }
        //        if (dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyRate"] != DBNull.Value)
        //        {
        //            txtExchangeRate.Text = dsPaymentDetail.Tables[0].Rows[0]["PaymentCurrencyRate"].ToString();
        //        }
        //        else
        //        {
        //            txtExchangeRate.Text = "1";
        //        }
        //    }
        //}
    }
    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        bool IsValidTransaction = true;

        int InvoiceId = 0, PaymentId = 0;
        string strInstrumentNo = "", strDocPath = "", strRemark = "";
        string strPaymentRefNo = "";
        Decimal decPaidAmount = 0;

        DateTime dtInstrumentDate = DateTime.Now;

        if (Session["InvoiceId"] == null)
        {
            IsValidTransaction = false;

            lblError.Text = "Session Expired! Please Login Again!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Login Error", "alert('" + lblError.Text + "');", true);

        }
        else
        {
            Int32.TryParse(hdnPaymentId.Value, out PaymentId);
            Int32.TryParse(Session["InvoiceId"].ToString(), out InvoiceId);

            Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);

            DataSet dsPaymentDetail = AccountExpense.GetInvoicePendingPayment(InvoiceId);

            if (dsPaymentDetail.Tables[0].Rows.Count > 0) // Audit L2 Completed OR Payment ON Hold
            {
                Decimal CheckPayableAmount = 0;
                string CheckVendorAccountNo = dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountNo"].ToString().Trim();
                int CurrentStatus = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["lStatus"]);
                Decimal.TryParse(dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString(), out CheckPayableAmount);

                if (CurrentStatus == 145 || CurrentStatus == 149 || CurrentStatus == 139)
                {
                    // Valid Transaction

                    if (decPaidAmount != CheckPayableAmount)
                    {
                        // Payable Amount not Matched
                        IsValidTransaction = false;
                        lblError.Text = "Please Check Check Payable Amount!";
                        lblError.CssClass = "errorMsg";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    }
                    if (lblBankAccountNo.Text.Trim() == "" || lblBankAccountNo.Text.Trim() != CheckVendorAccountNo)
                    {
                        // Invalid Account NO
                        IsValidTransaction = false;
                        lblError.Text = "Please Check Vendor Bank Account No!";
                        lblError.CssClass = "errorMsg";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    }
                }
                else
                {
                    // Payment Status Changed -- Reset Payment Amount to Zero
                    IsValidTransaction = false;
                    decPaidAmount = 0;

                    lblError.Text = "Payment Status Changed! Please check Invoice History";
                    lblError.CssClass = "errorMsg";
                    btnSavePayment.Visible = false;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                }
            }
            if (IsValidTransaction)
            {

                if (decPaidAmount <= 0)
                {
                    IsValidTransaction = false;

                    lblError.Text = "Invalid Payment Amount!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }

                if (InvoiceId == 0 || PaymentId == 0)
                {
                    IsValidTransaction = false;

                    lblError.Text = "Session Expired! Please Login Again For Payment!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Login Error", "alert('" + lblError.Text + "');", true);

                    return;
                }

                if (lblBankIFSC.Text.Trim() == "")
                {
                    IsValidTransaction = false;

                    lblError.Text = "Please Enter Bank IFC Code!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }
                if (lblBankAccountNo.Text.Trim() == "")
                {
                    IsValidTransaction = false;

                    lblError.Text = "Please Enter Bank Account No!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }
                if (lblBeneficiaryName.Text.Trim() == "")
                {
                    IsValidTransaction = false;

                    lblError.Text = "Please Enter Beneficiary Name!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }

                // Check Payment Transaction Status and Get Vendor Bank Info;

                int ActiveTransaction = AccountExpense.CheckActiveTransaction(InvoiceId, PaymentId);

                if (ActiveTransaction < 0)
                {
                    IsValidTransaction = false;

                    lblError.Text = "Payment Already in Process! Please Check Status";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }

                strRemark = txtPaymentRemark.Text.Trim();

                if (IsValidTransaction == true)
                {
                    strPaymentRefNo = LiveTransferFund(PaymentId);

                    if (strPaymentRefNo != "")
                    {
                        int PayResult = AccountExpense.AddInvoicePayment(InvoiceId, PaymentId, "", dtInstrumentDate,
                            decPaidAmount, strDocPath, strRemark, LoggedInUser.glUserId);

                        if (PayResult == 0)
                        {
                            //   bool IsMailSuccess = SendPaymentConfirmationMail(InvoiceId, strPaymentRefNo, dtInstrumentDate.ToShortDateString(), 
                            //       decPaidAmount.ToString());

                            lblError.Text = "Payment Initiated Successfully! Transaction No -" + strPaymentRefNo + " <BR> UTR No will be sent on Email";
                            lblError.CssClass = "success";
                            
                            Session["Success"] = lblError.Text;

                            Response.Redirect("AccountSuccess.aspx");
                        }
                        else if (PayResult == 1)
                        {
                            lblError.Text = "System Error! Please try after sometime";
                            lblError.CssClass = "errorMsg";

                            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                        }
                        else if (PayResult == 2)
                        {
                            lblError.Text = "Entered Paid Amount Exceed Net Payable Amount! Please Check Paid Amount.";
                            lblError.CssClass = "errorMsg";

                            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                        }

                    }
                }
                // Yes Bank API Payment

            }//END_IF
            else
            {
                lblError.Text = "Invoice Payment Already InProcess!";
                lblError.CssClass = "errorMsg";

                Session["InvoiceId"] = null;
                btnSavePayment.Visible = false;
            }
        }//END_ELSE
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

        if (strRemark == "")
        {
            lblError.Text = "Please Enter Remark!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Reject Error", "alert('" + lblError.Text + "');", true);
        }
        else
        {
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
    }
    private bool CheckBankTime(int PaymentMode)
    {
        bool isValidTime = true;

        TimeSpan tsToday = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 00);

        TimeSpan tsMaxTime = new TimeSpan(17, 30, 00); // 05:30 PM - RTGS ENDs

        if (tsToday > tsMaxTime && PaymentMode == 4)
        {
            ddlPaymentType.SelectedValue = "6";

            lblError.Text = "RTGS Not Allowed After 05:30 PM Please Change Payment Mode To NEFT";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Warning", "alert('RTGS Not Allowed After 05:30 PM Please Change Payment Mode To NEFT');", true);

            isValidTime = false;
        }

        return isValidTime;
    }
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPayment.SelectedValue == "1")
        {
            txtPayAmount.Text = hdnNetPayableAmount.Value;
        }
        else
        {
            txtPayAmount.Text = "";
        }
    }

    private bool CheckCertificate()
    {
        bool isValidClient = false;

        try
        {
            var x509 = new X509Certificate2(this.Request.ClientCertificate.Certificate);
            var chain = new X509Chain(true);
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
            chain.Build(x509);

            var validThumbprints = new HashSet<string>(
                System.Configuration.ConfigurationManager.AppSettings["ClientCertificateIssuerThumbprints"]
                    .Replace(" ", "").Split(',', ';'),
                StringComparer.OrdinalIgnoreCase);

            string[] strTest = System.Configuration.ConfigurationManager.AppSettings["ClientCertificateIssuerThumbprints"]
                    .Replace(" ", "").Split(',', ';');

            string strKeys = "";
            // if the certificate is self-signed, verify itself.

            for (int i = 0; i < chain.ChainElements.Count; i++)
            {
                //strKeys = strKeys + "<BR>"+ chain.ChainElements[i].Certificate.Thumbprint;

                if (validThumbprints.Contains(chain.ChainElements[i].Certificate.Thumbprint))
                {
                    isValidClient = true;
                }

            }
        }
        catch (Exception ex)
        {
            string strAInvoiceId = "0";
            string strIPAddress = GetUserIP();

            if (Session["InvoiceId"] != null)
            {
                strAInvoiceId = Session["InvoiceId"].ToString();
            }
            string strMessage = strAInvoiceId + "-Payment API unauthorised Access- " + LoggedInUser.glUserName + " From - " + strIPAddress;
            ErrorLog.SendMail(strMessage, ex);
            ErrorLog.LogToDatabase(Convert.ToInt32(strAInvoiceId), "PaymentAPI", "API unauthorised Access", strMessage, ex, "", LoggedInUser.glUserId);
        }

        return isValidClient;
        //if (isValidClient == false)
        //{

        // //   throw new UnauthorizedAccessException("The client certificate selected is not authorized for this system. Please restart the browser and pick the valid certificate");
        //}

        //lblMessage.Text = strKeys;
        // certificate Subject would contain some identifier of the user (an ID number, SIN number or anything else unique). here it is assumed that it contains the login name and nothing else
        //if (!string.Equals("CN=" + login, x509.Subject, StringComparison.OrdinalIgnoreCase))
        //    throw new UnauthorizedAccessException("The client certificate selected is authorized for another user. Please restart the browser and pick another certificate.");
    }

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }

    #region Bank API Payment

    private string UATTransferFund(int PaymentRequestId)
    {        
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string strReqestRefNo = unixTimestamp.ToString();

            string strResponseRefNo = "";

            BankReqTransfer.Root objTransferFund = new BankReqTransfer.Root();

            BankReqTransfer.Data objData = new BankReqTransfer.Data();

            BankReqTransfer.Risk objRisk = new BankReqTransfer.Risk();

            objTransferFund.Data = objData;
            objTransferFund.Risk = objRisk;

            BankReqTransfer.Initiation objInitiation = new BankReqTransfer.Initiation();

            BankReqTransfer.DebtorAccount objDebtorAccount = new BankReqTransfer.DebtorAccount();
            BankReqTransfer.CreditorAccount objCreditorAccount = new BankReqTransfer.CreditorAccount();
            BankReqTransfer.RemittanceInformation objRemittanceInformation = new BankReqTransfer.RemittanceInformation();

            BankReqTransfer.Unstructured objUnstructured = new BankReqTransfer.Unstructured();
            BankReqTransfer.Unstructured2 objUnstructured2 = new BankReqTransfer.Unstructured2();
            BankReqTransfer.ContactInformation objContactInformation = new BankReqTransfer.ContactInformation();
            BankReqTransfer.DeliveryAddress objDeliveryAddress = new BankReqTransfer.DeliveryAddress();

            objData.Initiation = objInitiation;
            objInitiation.DebtorAccount = objDebtorAccount;
            objInitiation.CreditorAccount = objCreditorAccount;
            objInitiation.RemittanceInformation = objRemittanceInformation;

            objRisk.DeliveryAddress = objDeliveryAddress;

            objUnstructured.ContactInformation = objContactInformation;
            objCreditorAccount.Unstructured = objUnstructured;

            objRemittanceInformation.Unstructured = objUnstructured2;

            BankReqTransfer.InstructedAmount objInstructedAmount = new BankReqTransfer.InstructedAmount();

            objInitiation.InstructedAmount  = objInstructedAmount;
            objInitiation.DebtorAccount     = objDebtorAccount;
            objInitiation.CreditorAccount   = objCreditorAccount;

            objData.ConsentId = "527274";

            // Debit
            objInitiation.InstructionIdentification = strReqestRefNo;
            objInitiation.EndToEndIdentification = "";

            objInstructedAmount.Amount = txtPayAmount.Text.Trim();
            objInstructedAmount.Currency = "INR";

            objDebtorAccount.Identification = "001790600004039";
            objDebtorAccount.SecondaryIdentification = "527274";

            // Credit

            objCreditorAccount.SchemeName = "yesb0000270";
            objCreditorAccount.Identification = "026291800001191";

            string strAccountName = "ABC";// lblBeneficiaryName.Text.Trim().Replace("(", "");
            strAccountName = strAccountName.Replace(")", "");
            strAccountName = strAccountName.Replace("+", " ");

            objCreditorAccount.Name = strAccountName.Trim();
            objContactInformation.EmailAddress = "amit.bakshi@babajishivram.com";
            objContactInformation.MobileNumber = "98333708840";

        // Payment Reference

            string strTransJobNo = lblJobNumber.Text.Replace("/", "").Trim();
            strTransJobNo = strTransJobNo.Replace("-", "").Trim();

            string strTransInvoiceNo = lblInvoiceNo.Text.Replace("/", "").Trim();
            strTransInvoiceNo = strTransInvoiceNo.Replace("-", "").Trim();
            if (strTransInvoiceNo.Length == 1)
            {
                // Minimun 2 Char Required

                strTransInvoiceNo = strTransInvoiceNo + "111";
            }
            
            objRemittanceInformation.Reference =  strTransJobNo+ strTransInvoiceNo;
            objUnstructured2.CreditorReferenceInformation = strTransInvoiceNo;
            // Payment Mode

            objInitiation.ClearingSystemIdentification = ddlPaymentType.SelectedItem.Text;

            // Delivery Address

            List<string> lstAddressLine = new List<string>();

            lstAddressLine.Add("Plot No 2A");
            lstAddressLine.Add("Behind Excom House");

            objDeliveryAddress.AddressLine = lstAddressLine;

            objDeliveryAddress.StreetName = "Sakinaka";
            objDeliveryAddress.BuildingNumber = "2A";
            objDeliveryAddress.PostCode = "400072";
            objDeliveryAddress.TownName = "Mumbai";

            List<string> lstCountySubDivision = new List<string>();

            lstCountySubDivision.Add("MH");
            objDeliveryAddress.CountySubDivision = lstCountySubDivision;
            objDeliveryAddress.Country = "IN";

            ////////////////////////////////////////////////////////

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonRequest = serializer.Serialize(objTransferFund);

            BankAPIError.Root objError = new BankAPIError.Root();
            BankAPIRespTransfer.Root objSuccess = new BankAPIRespTransfer.Root();

            int APITransactionResult = AccountExpense.AddBankPaymentAPIRequest(1, PaymentRequestId, objInitiation.InstructionIdentification, objInstructedAmount.Amount, objInstructedAmount.Currency,
                objCreditorAccount.SchemeName, objCreditorAccount.Identification, objCreditorAccount.Name, LoggedInUser.glUserId);

            int IsSuccess = 0;
            if (APITransactionResult > 0)
            {
                int StatusId = -1;
                string result = BankAPIMethods.UATBankPayment(jsonRequest, ref objError, ref objSuccess);

                if (objError.Code != null)
                {
                    strReqestRefNo = "";
                    lblError.Text = objError.Message;
                    lblError.CssClass = "errorMsg";

                    IsSuccess = 0; // Transaction Failed
                    DateTime dtResponseDate = DateTime.Now;

                    StatusId = (int)EnumBankStatus.FAILED;

                    int ErrorUpdResult = AccountExpense.AddBankPaymentAPIError(APITransactionResult, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, LoggedInUser.glUserId);

                    int AccountUpdResult = AccountExpense.UpdateBankPaymentAPIResponse(strReqestRefNo, "", "", objError.ActionDescription, dtResponseDate, IsSuccess, StatusId, LoggedInUser.glUserId);

                    ErrorLog.LogToTextFile("Vendor Payment Exception:" + strReqestRefNo + " Error Desc: " + result.ToString());

                    ErrorLog.SendMail("Bank Payment Exception:" + strReqestRefNo + ", Error Desc: " + result.ToString(), null);

                }
                else if (objSuccess.Data != null)
                {
                    strResponseRefNo = objSuccess.Data.TransactionIdentification;
                    IsSuccess = -1; // Transaction Initiated

                    StatusId = (int)Enum.Parse(typeof(EnumBankStatus), objSuccess.Data.Status);
                    DateTime dtResponseDate = DateTime.Parse(objSuccess.Data.StatusUpdateDateTime.ToString());

                    if (StatusId == (int)EnumBankStatus.SettlementCompleted)
                    {
                        IsSuccess = 1;
                    }
                    else if (StatusId == (int)EnumBankStatus.FAILED)
                    {
                        IsSuccess = 0;
                    }

                    int AccountUpdResult = AccountExpense.UpdateBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, objSuccess.Data.TransactionIdentification,
                        objSuccess.Data.Initiation.EndToEndIdentification, objSuccess.Data.Status, dtResponseDate, IsSuccess, StatusId, LoggedInUser.glUserId);
                }
                else
                {
                    return strResponseRefNo;
                }
            }
            else if (APITransactionResult == 0)
            {
                strResponseRefNo = "";
                lblError.Text = "Request Reference No Already Exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                strResponseRefNo = "";
                lblError.Text = "System Erro! Payment Request Not Initiated!";
                lblError.CssClass = "errorMsg";
            }
            //   return result.requestReferenceNo.ToString();

            return strResponseRefNo;
        }

    private string LiveTransferFund(int PaymentRequestId)
    {
        string strReqestRefNo = "";
        string strResponseRefNo = "";
        string strPaymentType = "";
        string strIFSCCode = "";
        decimal decAmount = 0m;

        bool isValidRequest = true;
        bool isPaid = false;
        int InvoiceStatus = 0;
        bool IsFundTransFromAPI = false;

        DataSet dsPaymentRequest = AccountExpense.GetInvoicePaymentDetail(PaymentRequestId);
        
        if (dsPaymentRequest.Tables.Count > 0)
        {
            strPaymentType = ddlPaymentType.SelectedItem.Text.Trim();
            strIFSCCode = lblBankIFSC.Text.Trim();

            if (strIFSCCode.ToLower().Contains("yesb"))
            {
                strPaymentType = "FT";
            }
            else if (strIFSCCode.ToLower().Contains("citi") && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            {
                strPaymentType = "IMPS";
            }
            else if (strIFSCCode.ToLower().Contains("indb") && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            {
                strPaymentType = "IMPS";
            }
            else if (strIFSCCode.ToLower().Contains("dbss") && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            {
                strPaymentType = "IMPS";
            }
            else if (strPaymentType.ToUpper() == "RTGS" && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            {
                strPaymentType = "NEFT";
            }
            //else if (strPaymentType.ToUpper() == "NEFT" && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            //{
            //    strPaymentType = "IMPS";
            //}

            //else if (strPaymentType.ToUpper() == "NEFT" && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            //{
            //    strPaymentType = "IMPS";
            //}

            int Invoiceid = Convert.ToInt32(dsPaymentRequest.Tables[0].Rows[0]["InvoiceId"]);

            Decimal.TryParse(dsPaymentRequest.Tables[0].Rows[0]["PaidAmount"].ToString(), out decAmount);

            if (dsPaymentRequest.Tables[0].Rows[0]["RequestRefNo"] != DBNull.Value)
            {
                strReqestRefNo = dsPaymentRequest.Tables[0].Rows[0]["RequestRefNo"].ToString();
            }

            if (dsPaymentRequest.Tables[0].Rows[0]["IsPaid"] != DBNull.Value)
            {
                isPaid = Convert.ToBoolean(dsPaymentRequest.Tables[0].Rows[0]["IsPaid"]);
            }
            if (dsPaymentRequest.Tables[0].Rows[0]["lStatus"] != DBNull.Value)
            {
                InvoiceStatus = Convert.ToInt32(dsPaymentRequest.Tables[0].Rows[0]["lStatus"]);
            }
            if (dsPaymentRequest.Tables[0].Rows[0]["IsFundTransFromAPI"] != DBNull.Value)
            {
                IsFundTransFromAPI = Convert.ToBoolean(dsPaymentRequest.Tables[0].Rows[0]["IsFundTransFromAPI"]);
            }

            // strUTRNo = dsPaymentRequest.Tables[0].Rows[0]["InstrumentNo"].ToString();

        }
        else
        {
            isValidRequest = false;
            isPaid = true;
        }

        //if (strReqestRefNo == "")
        //{
        //    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //    strReqestRefNo = "A" + unixTimestamp.ToString();
        //}

        // Check If Valid Payment Request

        if (isPaid == true)
        {
            // Payment Already Completed
            isValidRequest = false;

            lblError.Text = "Payment Error! Payment Already Completed!";
            lblError.CssClass = "errorMsg";
        }
        if (InvoiceStatus < 145 && InvoiceStatus != 139)
        {
            // 145 - Audit L2 Not Completed/ 151 Payment Already Completed
            isValidRequest = false;

            lblError.Text = "System Error! Audit L2 Pending For Invoice!";
            lblError.CssClass = "errorMsg";
        }
        if (InvoiceStatus >= 151)
        {
            // 145 - Audit L2 Not Completed/ 151 Payment Already Completed
            isValidRequest = false;

            lblError.Text = "System Error! Payment Already Processed!";
            lblError.CssClass = "errorMsg";
        }
        if (IsFundTransFromAPI == false)
        {
            // Fund Transfer not from Live Tracking
            isValidRequest = false;
            lblError.Text = "System Erro! Payment not from Live Tracking!";
            lblError.CssClass = "errorMsg";
        }

        if (isValidRequest == true)
        {
            BankReqTransfer.Root objTransferFund = new BankReqTransfer.Root();

            BankReqTransfer.Data objData = new BankReqTransfer.Data();

            BankReqTransfer.Risk objRisk = new BankReqTransfer.Risk();

            objTransferFund.Data = objData;
            objTransferFund.Risk = objRisk;

            BankReqTransfer.Initiation objInitiation = new BankReqTransfer.Initiation();

            BankReqTransfer.DebtorAccount objDebtorAccount = new BankReqTransfer.DebtorAccount();
            BankReqTransfer.CreditorAccount objCreditorAccount = new BankReqTransfer.CreditorAccount();
            BankReqTransfer.RemittanceInformation objRemittanceInformation = new BankReqTransfer.RemittanceInformation();

            BankReqTransfer.Unstructured objUnstructured = new BankReqTransfer.Unstructured();
            BankReqTransfer.Unstructured2 objUnstructured2 = new BankReqTransfer.Unstructured2();
            BankReqTransfer.ContactInformation objContactInformation = new BankReqTransfer.ContactInformation();
            BankReqTransfer.DeliveryAddress objDeliveryAddress = new BankReqTransfer.DeliveryAddress();

            objData.Initiation = objInitiation;
            objInitiation.DebtorAccount = objDebtorAccount;
            objInitiation.CreditorAccount = objCreditorAccount;
            objInitiation.RemittanceInformation = objRemittanceInformation;

            objRisk.DeliveryAddress = objDeliveryAddress;

            objUnstructured.ContactInformation = objContactInformation;
            objCreditorAccount.Unstructured = objUnstructured;

            objRemittanceInformation.Unstructured = objUnstructured2;

            BankReqTransfer.InstructedAmount objInstructedAmount = new BankReqTransfer.InstructedAmount();

            objInitiation.InstructedAmount = objInstructedAmount;
            objInitiation.DebtorAccount = objDebtorAccount;
            objInitiation.CreditorAccount = objCreditorAccount;

            objData.ConsentId = "418585";

            // Debit
            objInitiation.InstructionIdentification = strReqestRefNo;
            objInitiation.EndToEndIdentification = "";

            objInstructedAmount.Amount = txtPayAmount.Text.Trim();
            objInstructedAmount.Currency = "INR";

            objDebtorAccount.Identification = "007881300000296";
            objDebtorAccount.SecondaryIdentification = "418585";

            // Credit

            objCreditorAccount.SchemeName = strIFSCCode;
            objCreditorAccount.Identification = lblBankAccountNo.Text.Trim();

            string strAccountName = lblBeneficiaryName.Text.Trim().Replace("(", "");
            strAccountName = strAccountName.Replace(")", "");
            strAccountName = strAccountName.Replace("&", "");
            strAccountName = strAccountName.Replace("+", " ");
            strAccountName = strAccountName.Replace(".", " ");
            strAccountName = strAccountName.Replace(",", " ");
            strAccountName = strAccountName.Replace("~", " ");
            strAccountName = strAccountName.Replace("@", " ");
            strAccountName = strAccountName.Replace("#", " ");
            strAccountName = strAccountName.Replace("%", " ");
            strAccountName = strAccountName.Replace("^", " ");
            strAccountName = strAccountName.Replace("-", " ");
            strAccountName = strAccountName.Replace("_", " ");
            strAccountName = strAccountName.Replace("+", " ");
            strAccountName = strAccountName.Replace("/", " ");
            strAccountName = strAccountName.Replace("\\", " ");
            strAccountName = strAccountName.Replace("|", " ");
            strAccountName = strAccountName.Replace("<", " ");
            strAccountName = strAccountName.Replace(">", " ");
            strAccountName = strAccountName.Replace(">", " ");

            objCreditorAccount.Name = strAccountName.Trim(); 
            objContactInformation.EmailAddress = "somnath.kumbhar@babajishivram.com";
            objContactInformation.MobileNumber = "9224683493";

            // Payment Reference

            string strTransJobNo = lblJobNumber.Text.Replace("/", "").Trim();
            strTransJobNo = strTransJobNo.Replace("-", "").Trim();

            string strTransInvoiceNo = lblInvoiceNo.Text.Replace("/", "").Trim();
            strTransInvoiceNo = strTransInvoiceNo.Replace("-", "").Trim();
            if (strTransInvoiceNo.Length == 1)
            {
                // Minimun 2 Char Required

                strTransInvoiceNo = strTransInvoiceNo + "00";
            }
            objRemittanceInformation.Reference = strTransJobNo + " " + strTransInvoiceNo;
            objUnstructured2.CreditorReferenceInformation = strTransInvoiceNo;
            // Payment Mode

            objInitiation.ClearingSystemIdentification = strPaymentType;

            // Delivery Address

            List<string> lstAddressLine = new List<string>();

            lstAddressLine.Add("Babaji Shivram, Plot No 2");
            lstAddressLine.Add("Sakivhar Road");

            objDeliveryAddress.AddressLine = lstAddressLine;

            objDeliveryAddress.StreetName = "Andheri East Behind Excom House,";
            objDeliveryAddress.BuildingNumber = "2";
            objDeliveryAddress.PostCode = "400072";
            objDeliveryAddress.TownName = "Mumbai";

            List<string> lstCountySubDivision = new List<string>();

            lstCountySubDivision.Add("MH");
            objDeliveryAddress.CountySubDivision = lstCountySubDivision;
            objDeliveryAddress.Country = "IN";

            ////////////////////////////////////////////////////////

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonRequest = serializer.Serialize(objTransferFund);

            BankAPIError.Root objError = new BankAPIError.Root();
            BankAPIRespTransfer.Root objSuccess = new BankAPIRespTransfer.Root();

            int APITransactionResult = AccountExpense.AddBankPaymentAPIRequest(1, PaymentRequestId, objInitiation.InstructionIdentification, objInstructedAmount.Amount, objInstructedAmount.Currency,
                objCreditorAccount.SchemeName, objCreditorAccount.Identification, objCreditorAccount.Name, LoggedInUser.glUserId);

            Session["PaymentTransactionId"] = APITransactionResult;

            int IsSuccess = 0;
            if (APITransactionResult > 0)
            {
                int StatusId = -1;
                string result = BankAPIMethods.LiveBankPayment(jsonRequest, ref objError, ref objSuccess);

                if (objError.Code != null)
                {
                    strReqestRefNo = "";
                    lblError.Text = objError.Message;
                    lblError.CssClass = "errorMsg";

                    IsSuccess = 0; // Transaction Failed
                    DateTime dtResponseDate = DateTime.Now;

                    StatusId = (int)EnumBankStatus.FAILED;

                    int ErrorUpdResult = AccountExpense.AddBankPaymentAPIError(APITransactionResult, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, LoggedInUser.glUserId);

                    int AccountUpdResult = AccountExpense.UpdateBankPaymentAPIResponse(strReqestRefNo, "", "", objError.ActionDescription, dtResponseDate, IsSuccess, StatusId, LoggedInUser.glUserId);
                }
                else if (objSuccess.Data != null)
                {
                    if (objSuccess.Data.TransactionIdentification == null)
                    {
                        strResponseRefNo = strReqestRefNo;
                    }
                    else
                    {
                        strResponseRefNo = objSuccess.Data.TransactionIdentification;
                    }

                    IsSuccess = -1; // Transaction Initiated

                    StatusId = (int)Enum.Parse(typeof(EnumBankStatus), objSuccess.Data.Status);
                    DateTime dtResponseDate = DateTime.Parse(objSuccess.Data.StatusUpdateDateTime.ToString());

                    if (StatusId == (int)EnumBankStatus.SettlementCompleted)
                    {
                        IsSuccess = 1;
                    }
                    else if (StatusId == (int)EnumBankStatus.FAILED)
                    {
                        IsSuccess = 0;
                    }

                    int AccountUpdResult = AccountExpense.UpdateBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, objSuccess.Data.TransactionIdentification,
                        objSuccess.Data.Initiation.EndToEndIdentification, objSuccess.Data.Status, dtResponseDate, IsSuccess, StatusId, LoggedInUser.glUserId);
                }
                else
                {
                    return strResponseRefNo;
                }
            }
            else if (APITransactionResult == 0)
            {
                strResponseRefNo = "";
                lblError.Text = "Request Reference No Already Exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                strResponseRefNo = "";
                lblError.Text = "System Erro! Payment Request Not Initiated!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            // Invoice Payment Request   

            strResponseRefNo = "";

        }
        //   return result.requestReferenceNo.ToString();

        return strResponseRefNo;
    }
    public static void UATStatusCheck(int APITransactionID, string strRefNo)
    {
        //  string strToken = GenerateToken();

        {
            BankReqStatus.Root objRoot = new BankReqStatus.Root();
            BankReqStatus.Data objData = new BankReqStatus.Data();

            objRoot.Data = objData;

            objData.ConsentId = "527274";
            objData.InstrId = strRefNo;
            objData.SecondaryIdentification = "527274";

            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var jsonPayLoad = @serializer.Serialize(objRoot);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonPayLoad = serializer.Serialize(objRoot);

            BankAPIError.Root objError = new BankAPIError.Root();
            BankAPIRespTransfer.Root objSuccess = new BankAPIRespTransfer.Root();

            string jsonResponse = "";
            int StatusId = -1;

            string result = BankAPIMethods.UATBankGetStatus(jsonPayLoad, ref objError, ref objSuccess);

            if (objError.Code != null)
            {
                int IsSuccess = 0;

                StatusId = (int)EnumBankStatus.FAILED;

                jsonResponse = objError.Message;

                int ErrorUpdResult = DBOperations.AddBankPaymentAPIError(APITransactionID, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, 1);

                DateTime dtResponseDate = DateTime.Now;

                int AccountUpdResult = DBOperations.UpdateBankPaymentAPIResponse(strRefNo, "", "", objError.ActionDescription, dtResponseDate, IsSuccess, StatusId, 1);
            }
            else if (objSuccess.Data != null)
            {
                StatusId = (int)Enum.Parse(typeof(EnumBankStatus), objSuccess.Data.Status);

                jsonResponse = objSuccess.Data.Status;

                int IsSuccess = -1; // NULL

                if (StatusId == (int)EnumBankStatus.SettlementCompleted)
                {
                    IsSuccess = 1;
                }
                else if (StatusId == (int)EnumBankStatus.FAILED)
                {
                    IsSuccess = 0;
                }

                DateTime dtResponseDate = DateTime.Parse(objSuccess.Data.StatusUpdateDateTime.ToString());

                int AccountUpdResult = DBOperations.UpdateBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, objSuccess.Data.TransactionIdentification,
                    objSuccess.Data.Initiation.EndToEndIdentification, objSuccess.Data.Status, objSuccess.Data.StatusUpdateDateTime, IsSuccess, StatusId, 1);
            }

        }


    }
    #endregion

    #region Payment Email

    private bool SendPaymentConfirmationMail(int InvoiceID, string strUTRNo, string strUTRDate, string strPaidAmount)
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

        string strTDSTotalAmount = "0";

        string strCreatedByEmail = "amit.bakshi@BabajiShivram.com";

        if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
        {
            strTDSTotalAmount = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
        }

        if (dsDetail.Tables[0].Rows[0]["CreatedByEmail"] != DBNull.Value)
        {
           // strCreatedByEmail = dsDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
        }

        string strPaymentUser       = LoggedInUser.glEmpName;
        string strTotalPaidAmount   = txtPayAmount.Text.Trim();

        string strPaymentRemark     = txtPaymentRemark.Text.Trim();

        string EmailContent = "";

        try
        {

            try
            {
                string strFileName = "../EmailTemplate/EmailVendorPaymentConfirmation.txt";

                StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                sr = File.OpenText(Server.MapPath(strFileName));
                EmailContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                GC.Collect();
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
                strSubject = " Payment Details /" +strJobNumber +"/ "+strInvoiceNo + "/ "+strConsignee+ " / "+ strExpenseType;

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

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    #endregion
}