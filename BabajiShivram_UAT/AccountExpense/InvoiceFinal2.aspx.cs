using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
public partial class AccountExpense_InvoiceFinal2 : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        MskEdtValPayDueDate.MaximumValue = DateTime.Now.AddMonths(6).ToString("dd/MM/yyyy");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Final Invoice";
        
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinal.aspx");
        }

        if (!IsPostBack)
        {
            GetPaymentRequest();
        }

        CalExtInvoiceDate.EndDate = DateTime.Now;
        CalExtPayReqrdDate.StartDate = DateTime.Now;

        MskEdtValPayDueDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MskEdtValPayReqrdDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MskEdtValInvoice.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

        wzRequest.PreRender += new EventHandler(wzRequest_PreRender);
    }

    private void GetPaymentRequest()
    {
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinal.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        int StatusId = 0;
        int InvoiceType = 0; // Proforma
        bool IsFinalInvoicePending = false;

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);
            InvoiceType = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["InvoiceType"]);

            if(dsDetail.Tables[0].Rows[0]["IsFinalInvoicePending"] != DBNull.Value)
            {
                IsFinalInvoicePending = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsFinalInvoicePending"]);
            }

            // Check Proforma Invoice Status

            if (StatusId < 151 || InvoiceType > 0 || IsFinalInvoicePending == false)
            {
                // Invalid Proforma Invoice

                Session["InvoiceId"] = null;
                Response.Redirect("PendingInvoiceFinal.aspx");
            }

            txtJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            ddlExpenseType.SelectedValue = dsDetail.Tables[0].Rows[0]["ExpenseTypeId"].ToString();
            ddGSTINType.SelectedValue = dsDetail.Tables[0].Rows[0]["VendorGSTNTypeId"].ToString();

            hdnJobId.Value = dsDetail.Tables[0].Rows[0]["JobId"].ToString();
            hdnModuleId.Value = dsDetail.Tables[0].Rows[0]["ModuleId"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();

            hdnVendorCode.Value = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();
            txtSupplierGSTIN.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNo"].ToString();
            txtVendorPANNo.Text = dsDetail.Tables[0].Rows[0]["VendorPAN"].ToString();
            txtVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblVendorType.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNType"].ToString();
            txtPaymentTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();

            lblRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();
            ddlPaymentType.SelectedValue = dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            lblProformaTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();

            lblInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            lblInvoiceDate.Text = dsDetail.Tables[0].Rows[0]["InvoiceDate"].ToString();
            txtPaymentDueDate.Text = dsDetail.Tables[0].Rows[0]["PaymentDueDate"].ToString();
            txtPaymentRequiredDate.Text = dsDetail.Tables[0].Rows[0]["dtDate"].ToString(); 
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();


            lblAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            lblAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            if (dsDetail.Tables[0].Rows[0]["CurrencyName"] != DBNull.Value)
            {
                lblProformaInvoiceCurrency.Text = dsDetail.Tables[0].Rows[0]["CurrencyName"].ToString();

                ddCurrency.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyId"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                txtExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
                lblProformaExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
            }

            lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            lblBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();

            lblRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();
            lblRequestDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy");
            lblPaidAmount.Text = dsDetail.Tables[0].Rows[0]["PaidAmount"].ToString();


            //if (dsDetail.Tables[0].Rows[0]["InvoiceAmount"] != DBNull.Value)
            //{
            //    lblTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            //}

            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            {
                txtPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["dtDate"] != DBNull.Value)
            {
                txtPaymentRequiredDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

        }
        else
        {
            if (Session["InvoiceId"] == null)
            {
                Response.Redirect("PendingInvoiceFinal.aspx");
            }
        }

        // Final Invoice

        txtCongisgneeGSTIN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
        txtCongisgneeName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
        hdnConsigneeCode.Value = dsDetail.Tables[0].Rows[0]["ConsigneeCode"].ToString();
        hdnConsigneePANNo.Value = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();

        if (txtCongisgneeGSTIN.Text == "")
        {
            chkNoGSTINCustomer.Checked = true;
        }
        if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsRIM"]) == true)
        {
            ddRIM.SelectedValue = "1";
        }
        // Get Job Detail

        GetJobDetail();
    }

    #region Wizard Event

    protected void wzRequest_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        // Required for Diff GST Bill To Party
        RFVDocEmailApproval.Enabled = false;

        int IndexId = e.CurrentStepIndex;

        int TotalSteps = wzRequest.WizardSteps.Count;

        int JobId; string strJobRefNO; int ModuleID; int InvoiceType = 1; bool isRIM = false;
        int VendorGSTNType, PaymentTypeId = 0, ExpenseTypeId = 0;
        string strConsigneeGSTIN = "", strConsigneeName = "", strConsigneeCode = "", strConsigneePAN = "";

        string strVendorName, strVendorCode, strVendorGSTNo, strVendorPAN, strInvoiceNo, strRemark;

        decimal decProformaTotalInvoiceValue = 0m;
        decimal decExchangeRate = 1;

        bool IsAdvanceReceived = false; decimal AdvanceAmount = 0m;
        bool IsInterest = false; decimal InterestAmount = 0m;
        string strPaymentTerms; string strInvoiceFilePath = "";

        Decimal decInvoiceAmount = 0m, decTaxableAmount = 0m, decGSTAmount = 0m;

        DateTime dtInvoiceDate = DateTime.MinValue, dtPaymentRequiredDate = DateTime.MinValue, dtPaymentDueDate = DateTime.MinValue;

        JobId = Convert.ToInt32(hdnJobId.Value);
        ModuleID = Convert.ToInt32(hdnModuleId.Value);
        strJobRefNO = txtJobNumber.Text.Trim();
        InvoiceType = Convert.ToInt32(ddInvoiceType.SelectedValue);

        Decimal.TryParse(lblProformaTotalInvoiceValue.Text.Trim(), out decProformaTotalInvoiceValue);
        Decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decInvoiceAmount);

        // Foregin Currency
        int InvoiceCurrencyId = 46; // INR
        Decimal decInvoiceCurrencyAmt = 0m, decInvoiceCurrencyExchangeRate = 1;

        if (ddCurrency.SelectedIndex > 0)
        {
            InvoiceCurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);

            decimal.TryParse(txtExchangeRate.Text.Trim(), out decInvoiceCurrencyExchangeRate);

            if (decInvoiceCurrencyExchangeRate <= 0)
            {
                lblError.Text = "Invalid Currency Exchange Rate!";
                lblError.CssClass = "errorMsg";
                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('" + lblError.Text + "');", true);
                e.Cancel = true;
            }
        }

        decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

        VendorGSTNType = Convert.ToInt32(ddGSTINType.SelectedValue);

        if (IndexId == 0) // Vendor/Invoice To Party Detail
        {
            if(decInvoiceAmount <=0)
            {
                lblError.Text = "Please Enter Invoice Value!";
                lblError.CssClass = "errorMsg";
                ScriptManager.RegisterStartupScript(this, GetType(), "Invalid Invoice Amount", "alert('" + lblError.Text + "');", true);
                e.Cancel = true;
            }
            //else if(decInvoiceAmount <  decProformaTotalInvoiceValue)
            //{
            //    lblError.Text = "Final Invoice Value Can Not be less then Proforma Invoice Value!";
            //    lblError.CssClass = "errorMsg";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Invalid Invoice Amount", "alert('" + lblError.Text + "');", true);
            //    e.Cancel = true;
            //}

            txtCongisgneeGSTIN.Text = "";
            txtCongisgneeName.Text = "";

            txtCongisgneeGSTIN.Enabled = true;
            txtCongisgneeName.Enabled = true;

            if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9")
            {
                txtSupplierGSTIN.Enabled = false;
                txtVendorName.Enabled = false;
                txtVendorPANNo.Enabled = false;

                // Fix Vendor Code Dor Duty and Panelty

                txtSupplierGSTIN.Text = "";
                txtVendorName.Text = "Customs-ICEGATE Duty And Penalty";
                txtVendorPANNo.Text = "";
                hdnVendorCode.Value = "CDAP00";

                // Fix BIll To Party - BoE Consingee GSTIN

                txtCongisgneeGSTIN.Text = lblConsgneeGSTIN.Text;
                txtCongisgneeName.Text = lblConsignee.Text;

                txtCongisgneeGSTIN.Enabled = false;
                txtCongisgneeName.Enabled = false;

            }
            else if (VendorGSTNType == 1) // Registered
            {
                txtSupplierGSTIN.Enabled = false;
                txtVendorName.Enabled = false;
                txtVendorPANNo.Enabled = false;
            }
            else
            {
                txtSupplierGSTIN.Enabled = false;
                txtVendorName.Enabled = false;
                txtVendorPANNo.Enabled = false;
            }


        }
        else if (IndexId == 1) // Invoice Detail
        {
            int PaymentTermsDays = 0;

            if (txtSupplierGSTIN.Text.Trim() != "")
            {
                if (ddGSTINType.SelectedValue != "1")
                {
                    ddGSTINType.SelectedValue = "1"; // Registered
                }
            }


            // RIM & NonRIM
            // IF Job BoE GSTIN Match WITH Invoice To Party GSTIN - Non_RIM
            // IF Job BoE GSTIN Match WITH BABAJI GSTIN - RIM
            // ELSE - Non_RIM

            string CheckBabajiGSTIN = "";

            bool isBabajiGST = BabajiGSTINDict.BabajiGSTIN.TryGetValue(txtCongisgneeGSTIN.Text.Trim(), out CheckBabajiGSTIN);

            if (isBabajiGST == true)
            {
                hdnIsRIM.Value = "1"; // Babaji GSTIN // Non RIM
                ddRIM.SelectedValue = "2"; // Non-RIM
            }
            else if (hdnBOEGSTIN.Value == txtCongisgneeGSTIN.Text.Trim())
            {
                hdnIsRIM.Value = "0"; // Consignee GSTIN // RIM
                ddRIM.SelectedValue = "1"; // RIM
                // FOR RIM - Only Single Charge Code Allowded

                //if (rblMultiChargeCode.SelectedValue == "1")
                //{
                //    rblMultiChargeCode.SelectedValue = "0";
                //}
            }
            else
            {
                //   ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party GSTIN", "alert('" + txtCongisgneeGSTIN.Text + "');", true);

                // Check with Babaji GSTIN
                                
                if (CheckBabajiGSTIN == null || CheckBabajiGSTIN == "")
                {
                    if (hdnDifferentGSTNoAllowed.Value == "1" && txtCongisgneeGSTIN.Text.Trim().Length == 15 && hdnBOEGSTIN.Value.Length == 15)
                    {
                        // Check Bill To Party GSTIN With Customer PAN NO

                        //if(hdnCustomerPANNo.Value != "")
                        // Different GST No Allowed For Customer Other Then Bill Of Entry GSTIN

                        // Get PAN No from Bill Of Entry GSTIN

                        string strBoEPAN = ""; string strNewGSTINPAN = "";

                        strBoEPAN = hdnBOEGSTIN.Value.Substring(2, 10);
                        strNewGSTINPAN = txtCongisgneeGSTIN.Text.Trim().Substring(2, 10);

                        if (strBoEPAN.ToLower().Trim() == strNewGSTINPAN.ToLower().Trim())
                        {
                            // Valid Bill To Party GSTIN
                            hdnIsRIM.Value = "0"; // Consignee GSTIN // RIM
                            ddRIM.SelectedValue = "1"; // RIM

                            // Email Approval Copy Required For Bill To Party/Consignee Other GSTIN

                            RFVDocEmailApproval.Enabled = true;
                        }
                        else
                        {
                            lblError.Text = "Bill To GSTIN Not Matched with Consignee Other GSTIN !";
                            lblError.CssClass = "errorMsg";

                            ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party GSTIN", "alert('" + lblError.Text + "');", true);

                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        // Invalid Bill To Party GSTIN

                        lblError.Text = "Invoice To GSTIN Not Matched with BoE GSTIN or Babaji GSTIN!";
                        lblError.CssClass = "errorMsg";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party GSTIN", "alert('" + lblError.Text + "');", true);

                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    hdnIsRIM.Value = "1"; // Babaji GSTIN // Non RIM
                                          // BoE GSTIN Sate match with Invoice To GSTIN
                    ddRIM.SelectedValue = "2"; // Non-RIM
                }
            }

            /****************************
            if (hdnIsRIM.Value == "0")
            {
                ddRIM.SelectedValue = "1"; // RIM

                // FOR RIM - Only Single Charge Code Allowded

                //if (rblMultiChargeCode.SelectedValue == "1")
                //{
                //    rblMultiChargeCode.SelectedValue = "0";
                //}
            }
            else if (hdnIsRIM.Value == "1") // Babaji GSTIN
            {
                ddRIM.SelectedValue = "2"; // Non-RIM
            }
            **************************/

            // Display Prevoid Step Value

            lblJobRefNO.Text = txtJobNumber.Text.Trim();
            lblExpenseType.Text = ddlExpenseType.SelectedItem.Text;
            lblInvoiceType.Text = ddInvoiceType.SelectedItem.Text;
            lblVendorType.Text = ddGSTINType.SelectedItem.Text;

            lblInvoiceCurrency.Text = ddCurrency.SelectedItem.Text;
            lblExchangeRate.Text = txtExchangeRate.Text.Trim();

            lblEnteredTotalValue.Text = (Convert.ToDecimal(txtTotalInvoiceValue.Text) * decExchangeRate).ToString();

            lblMultipleGSTRate.Text = rblMultiChargeCode.SelectedItem.Text;


            // Get Bill To Consginee Code By GSTIN From FA System

            if (hdnConsigneeCode.Value.Trim() == "")
            {
                DataSet dsBillToParty = AccountExpense.GetFAVendorByGSTIN(txtCongisgneeGSTIN.Text.Trim());

                if (dsBillToParty.Tables.Count > 0)
                {
                    if (dsBillToParty.Tables[0].Rows.Count > 0)
                    {
                        hdnConsigneeCode.Value = dsBillToParty.Tables[0].Rows[0]["par_code"].ToString();
                        hdnConsigneePANNo.Value = dsBillToParty.Tables[0].Rows[0]["girno"].ToString();
                        txtCongisgneeName.Text = dsBillToParty.Tables[0].Rows[0]["par_name"].ToString();
                    }
                    else
                    {
                        lblError.Text = "Bill to Party Details Not Found in Account System!";
                        lblError.CssClass = "errorMsg";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party Error", "alert('" + lblError.Text + "');", true);
                    }
                }
            }

            if (InvoiceType == 1)
            {
                if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9")
                {
                    lblTotalTaxableValue.Text = txtTotalInvoiceValue.Text;
                    lblTotalValue.Text = txtTotalInvoiceValue.Text;
                    lblTotalGST.Text = "0";

                    lblTotalTaxableValue.Enabled = false;
                    lblTotalValue.Enabled = false;
                    lblTotalGST.Enabled = false;

                    fldInvoiceItem.Visible = false;
                }
                else
                {
                    // RIM - Final
                    if (ddRIM.SelectedValue == "1")
                    {
                      //  fldInvoiceItem.Visible = false;

                        if (ddGSTINType.SelectedValue == "1")
                        {
                            // Registered - Tax Applicable

                            lblTotalTaxableValue.Enabled = false;
                            lblTotalGST.Enabled = false;

                            lblTotalValue.Enabled = false;


                            ddGSTRate.Enabled = true;
                        }
                        else
                        {
                            lblTotalTaxableValue.Text = txtTotalInvoiceValue.Text.Trim();
                            lblTotalValue.Text = txtTotalInvoiceValue.Text.Trim();
                            lblTotalGST.Text = "0";

                            ddGSTRate.Enabled = false;
                            ddGSTRate.SelectedValue = "0";
                        }
                    }
                    else
                    {
                        // NonRIM - FINAL
                        fldInvoiceItem.Visible = true;

                        if (ddGSTINType.SelectedValue == "1")
                        {
                            // Registered - Tax Applicable

                            ddGSTRate.Enabled = true;
                        }
                        else
                        {
                            ddGSTRate.Enabled = false;
                            ddGSTRate.SelectedValue = "0";
                        }
                    }

                }
            }
            else
            {
                // Proforma

                fldInvoiceItem.Visible = false;

                if (VendorGSTNType == 1)
                {
                    lblTotalGST.Enabled = true;

                    lblTotalTaxableValue.Enabled = true;
                }
                else
                {
                    lblTotalGST.Enabled = false;
                    lblTotalGST.Text = "0";

                    lblTotalTaxableValue.Text = txtTotalInvoiceValue.Text;
                    lblTotalValue.Text = txtTotalInvoiceValue.Text;
                }
            }

            // Multiple GST Rate 
            Boolean IsMultipleRate = false;

            if (rblMultiChargeCode.SelectedValue == "1")
            {
                IsMultipleRate = true;
            }

            if (txtPaymentTerms.Text.Trim() != "")
            {
                Int32.TryParse(txtPaymentTerms.Text.Trim(), out PaymentTermsDays);

                txtPaymentDueDate.Text = DateTime.Now.AddDays(PaymentTermsDays).ToString("dd/MM/yyyyy");
            }
            else
            {
                txtPaymentDueDate.Text = DateTime.Now.ToString("dd/MM/yyyyy");
            }

            if (rblAdvanceReceived.SelectedValue == "1")
            {
                txtAdvanceAmount.Enabled = true;
                RFVAdvanceAmt.Enabled = true;
                REVAdvanceAmount.Enabled = true;
            }
            else
            {
                txtAdvanceAmount.Enabled = false;
                RFVAdvanceAmt.Enabled = false;
                REVAdvanceAmount.Enabled = false;
                txtAdvanceAmount.Text = "0";
            }

            // RIM-NonRIM

            if (IsMultipleRate == false)
            {
                txtChargeName.Enabled = false;
                txtChargeCode.Enabled = false;
            }

            if (ddlExpenseType.SelectedValue == "1") // Duty - RIM
            {
                if (InterestAmount == 0)
                {
                    // Duty Without Interest
                    txtChargeName.Text = "Custom Duty";
                    txtChargeCode.Text = "CDS";

                    hdnChargeName.Value = "Custom Duty";
                    hdnChargeCode.Value = "CDS";
                    hdnChargeHSN.Value = "9967";
                }
                else
                {
                    // Duty With Interest
                    txtChargeName.Text = "Custom Duty & Interest Paid";
                    txtChargeCode.Text = "CI2";

                    hdnChargeName.Value = "Custom Duty & Interest Paid";
                    hdnChargeCode.Value = "CI2";
                    hdnChargeHSN.Value = "9967";
                }

            }
            else if (ddlExpenseType.SelectedValue != "0")
            {
                GetExpenseTypeDetail();
            }
            else if (ddlExpenseType.SelectedValue == "2") // CFS - RIM
            {
                txtChargeName.Text = "CFS Charges";
                txtChargeCode.Text = "F68";

                hdnChargeName.Value = "CFS Charges";
                hdnChargeCode.Value = "F68";
                hdnChargeHSN.Value = "9967";
            }
            else if (ddlExpenseType.SelectedValue == "3") // DO - RIM
            {
                txtChargeName.Text = "Delivery Order Charges";
                txtChargeCode.Text = "D50";

                hdnChargeName.Value = "Delivery Order Charges";
                hdnChargeCode.Value = "D50";
                hdnChargeHSN.Value = "9967";
            }
            else if (ddlExpenseType.SelectedValue == "6") // Other Payment - RIM
            {
                txtChargeName.Text = "Other Charges";
                txtChargeCode.Text = "O05";

                hdnChargeName.Value = "Other Charges";
                hdnChargeCode.Value = "O05";
                hdnChargeHSN.Value = "9967";
            }
            else if (ddlExpenseType.SelectedValue == "9") // Penalty - RIM
            {
                txtChargeName.Text = "Custom Penalty";
                txtChargeCode.Text = "C43";

                hdnChargeName.Value = "Custom Penalty";
                hdnChargeCode.Value = "C43";
                hdnChargeHSN.Value = "9967";
            }
            else if (ddlExpenseType.SelectedValue == "10") // Detention - RIM
            {
                txtChargeName.Text = "Detention";
                txtChargeCode.Text = "DEN";

                hdnChargeName.Value = "Detention";
                hdnChargeCode.Value = "DEN";
                hdnChargeHSN.Value = "9967";
            }
            else if (ddlExpenseType.SelectedValue == "11") // Advance  - RIM
            {
                txtChargeName.Text = "Advance";
                txtChargeCode.Text = "AP8";

                hdnChargeName.Value = "Advance";
                hdnChargeCode.Value = "AP8";
                hdnChargeHSN.Value = "9967";
            }
            else
            {
                txtChargeName.Text = "";
                txtChargeCode.Text = "";

                hdnChargeName.Value = "";
                hdnChargeCode.Value = "";
                hdnChargeHSN.Value = "";
            }
        }
    }

    protected void wzRequest_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        //string strVendorName = "", strVendorCode = "", strGSTNo = "", strInvoiceNo = "", InvoiceFilePath = "";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinal.aspx");
        }

        int ProformaInvoiceID = Convert.ToInt32(Session["InvoiceId"]);

        int StatusIdCheck = 0;
        int InvoiceTypeCheck = 0; // Proforma
        bool IsFinalInvoicePendingCheck = false;
        string strProformaInvoiceNoCheck = "";

        DataSet dsProformaDetail = AccountExpense.GetInvoiceDetail(ProformaInvoiceID);

        // Re - Validated Profrma Invoice Detail

        if (dsProformaDetail.Tables[0].Rows.Count > 0)
        {
            StatusIdCheck           =   Convert.ToInt32(dsProformaDetail.Tables[0].Rows[0]["lStatus"]);
            InvoiceTypeCheck        =   Convert.ToInt32(dsProformaDetail.Tables[0].Rows[0]["InvoiceType"]);
            strProformaInvoiceNoCheck = dsProformaDetail.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim();

            if (dsProformaDetail.Tables[0].Rows[0]["IsFinalInvoicePending"] != DBNull.Value)
            {
                IsFinalInvoicePendingCheck = Convert.ToBoolean(dsProformaDetail.Tables[0].Rows[0]["IsFinalInvoicePending"]);
            }

            // Check Proforma Invoice Invoice No/Payment Status/Pending Final Invoice Statys/Invoice Type

            if(strProformaInvoiceNoCheck != lblInvoiceNo.Text.Trim())
            {
                // Invalid Proforma Invoice

                Session["InvoiceId"] = null;
                Response.Redirect("PendingInvoiceFinal.aspx");
            }
            else if (StatusIdCheck < 151 || InvoiceTypeCheck > 0 || IsFinalInvoicePendingCheck == false)
            {
                // Invalid Proforma Invoice

                Session["InvoiceId"] = null;
                Response.Redirect("PendingInvoiceFinal.aspx");
            }
        }

        
        bool bIsInputValid = ValidateInput();

        if (bIsInputValid)
        {
            int JobId; string strJobRefNO; int ModuleID; int BranchId; int InvoiceType = 1; bool isRIM = false;
            int VendorGSTNType, PaymentTypeId = 0, ExpenseTypeId = 0;
            string strConsigneeGSTIN = "", strConsigneeName = "", strConsigneeCode = "", strConsigneePAN = "";

            string strVendorName, strVendorCode, strVendorGSTNo, strVendorPAN, strInvoiceNo, strRemark;
            bool IsAdvanceReceived = false; decimal AdvanceAmount = 0m;
            bool IsInterest = false; decimal InterestAmount = 0m;
            string strPaymentTerms; string strInvoiceFilePath = "";

            DateTime dtInvoiceDate = DateTime.MinValue, dtPaymentRequiredDate = DateTime.MinValue, dtPaymentDueDate = DateTime.MinValue;

            Decimal decInvoiceAmount = 0m, decTaxableAmount = 0m, decGSTAmount = 0m;

            // Foregin Currency
            int InvoiceCurrencyId = 46; // INR
            Decimal decInvoiceCurrencyAmt = 0m, decInvoiceCurrencyExchangeRate = 1;

            if (ddCurrency.SelectedIndex > 0)
            {
                InvoiceCurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);

                decimal.TryParse(txtExchangeRate.Text.Trim(), out decInvoiceCurrencyExchangeRate);

                if (decInvoiceCurrencyExchangeRate == 0)
                {
                    lblError.Text = "Invalid Currency Exchange Rate!";
                    lblError.CssClass = "errorMsg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('" + lblError.Text + "');", true);
                    e.Cancel = true;
                }
            }

            decInvoiceCurrencyAmt = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
            decInvoiceCurrencyExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

            JobId = Convert.ToInt32(hdnJobId.Value);
            ModuleID = Convert.ToInt32(hdnModuleId.Value);
            BranchId = Convert.ToInt32(hdnBranchId.Value);
            strJobRefNO = txtJobNumber.Text.Trim();
            VendorGSTNType = Convert.ToInt32(ddGSTINType.SelectedValue);

            strConsigneeGSTIN = txtCongisgneeGSTIN.Text.Trim();
            strConsigneeName = txtCongisgneeName.Text.Trim();
            strConsigneeCode = hdnConsigneeCode.Value.Trim();
            strConsigneePAN = hdnConsigneePANNo.Value.Trim();

            strRemark = txtRemark.Text.Trim();

            InvoiceType = Convert.ToInt32(ddInvoiceType.SelectedValue);
            PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);
            ExpenseTypeId = Convert.ToInt32(ddlExpenseType.SelectedValue);

            if (ddRIM.SelectedValue == "1")
            {
                isRIM = true;
            }

            if (rdlInterest.SelectedValue == "1")
            {
                IsInterest = true;

                if (txtInterestAmnt.Text.Trim() != "")
                {
                    InterestAmount = Convert.ToDecimal(txtInterestAmnt.Text.Trim());
                }
            }
            if (rblAdvanceReceived.SelectedValue == "1")
            {
                IsAdvanceReceived = true;

                if (txtAdvanceAmount.Text.Trim() != "")
                {
                    AdvanceAmount = Convert.ToDecimal(txtAdvanceAmount.Text.Trim());
                }
            }

            if (txtInvoiceDate.Text != "")
            {
                dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
            }
            if (txtPaymentRequiredDate.Text != "")
            {
                dtPaymentRequiredDate = Commonfunctions.CDateTime(txtPaymentRequiredDate.Text.Trim());
            }
            if (txtPaymentDueDate.Text != "")
            {
                dtPaymentDueDate = Commonfunctions.CDateTime(txtPaymentDueDate.Text.Trim());
            }

            if (ExpenseTypeId == 1 || ExpenseTypeId == 9) // Duty Or Interest
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim()) + InterestAmount;

                    decInvoiceAmount = decTaxableAmount;
                }
                if (lblTotalTaxableValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(lblTotalTaxableValue.Text.Trim());
                }
                if (lblTotalGST.Text.Trim() != "")
                {
                    decGSTAmount = Convert.ToDecimal(lblTotalGST.Text.Trim());
                }
            }
            else if (InvoiceType == 1) // Final Invoice
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decInvoiceAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
                }
                if (lblTotalTaxableValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(lblTotalTaxableValue.Text.Trim());
                }
                if (lblTotalGST.Text.Trim() != "")
                {
                    decGSTAmount = Convert.ToDecimal(lblTotalGST.Text.Trim());
                }
            }
            else
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decInvoiceAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
                    decTaxableAmount = decInvoiceAmount;
                }
            }

            strVendorName = txtVendorName.Text.Trim();
            strVendorCode = hdnVendorCode.Value.Trim();
            strVendorGSTNo = txtSupplierGSTIN.Text.Trim();
            strVendorPAN = txtVendorPANNo.Text.Trim();

            strInvoiceNo = txtInvoiceNo.Text.Trim();
            strInvoiceNo = strInvoiceNo.Replace(" ", "");

            strPaymentTerms = txtPaymentTerms.Text.Trim();

            bool IsImmediatePayment = false;

            int RequestId = AccountExpense.AddInvoiceDetail2(JobId, ProformaInvoiceID, strJobRefNO, ModuleID, BranchId, ExpenseTypeId,
                isRIM, InvoiceType, PaymentTypeId, VendorGSTNType, strConsigneeGSTIN, strConsigneeName, strConsigneeCode, strConsigneePAN,
                IsInterest, InterestAmount, IsAdvanceReceived, AdvanceAmount, dtPaymentRequiredDate, dtPaymentDueDate, strVendorName, strVendorCode, strVendorGSTNo,
                strVendorPAN, strInvoiceNo, dtInvoiceDate, decInvoiceAmount, decTaxableAmount, decGSTAmount, InvoiceCurrencyId,
                decInvoiceCurrencyAmt, decInvoiceCurrencyExchangeRate, strPaymentTerms, IsImmediatePayment, strRemark, LoggedInUser.glUserId);

            //int RequestId2 = AccountExpense.AddFinalInvoice(ProformaInvoiceID, isRIM, strConsigneeGSTIN, strConsigneeName, strConsigneeCode,
            //    strConsigneePAN, strInvoiceNo, dtInvoiceDate, decInvoiceAmount, decTaxableAmount, decGSTAmount, strInvoiceFilePath, strRemark, LoggedInUser.glUserId);

            if (RequestId > 0)
            {
                lblError.Text = "Invoice Detail Added Successfully!";
                lblError.CssClass = "success";

                // Invoice Uploaded
                AccountExpense.AddInvoiceStatus(RequestId, 100, strRemark, LoggedInUser.glUserId);

                // Invoice Auto Appproved for Mgmt and Submitted To Accounts Dept
                AccountExpense.AddInvoiceStatus(RequestId, 120, "Auto Approved and  Submitted To Account", LoggedInUser.glUserId);


                if (InvoiceType == 1) // Final
                {
                    AddInvoiceItem(RequestId);
                }

                // Upload Document

                // Upload Document

                string strDirPath = strJobRefNO.Replace("/", "");

                strDirPath = strDirPath.Replace("-", "");

                strInvoiceFilePath = "VendorInvoice//" + strDirPath + "//";

                if (fuDocumentInvoice.HasFile)
                {
                    string FileName1 = UploadDocument(fuDocumentInvoice, strInvoiceFilePath);

                    int FileOutput1 = AccountExpense.AddInvoiceDocument(RequestId, 1, strInvoiceFilePath, FileName1, LoggedInUser.glUserId);
                }
                if (fuDocumentEmailApproval.HasFile)
                {
                    string FileName2 = UploadDocument(fuDocumentEmailApproval, strInvoiceFilePath);

                    string UploadFileName2 = strInvoiceFilePath + FileName2;

                    int FileOutput2 = AccountExpense.AddInvoiceDocument(RequestId, 2, strInvoiceFilePath, FileName2, LoggedInUser.glUserId);
                }
                if (fuDocumentOther.HasFile)
                {
                    string FileName3 = UploadDocument(fuDocumentOther, strInvoiceFilePath);

                    int FileOutput3 = AccountExpense.AddInvoiceDocument(RequestId, 3, strInvoiceFilePath, FileName3, LoggedInUser.glUserId);
                }

                Session["Success"] = lblError.Text;

                Response.Redirect("AccountSuccess.aspx");

            }
            else if (RequestId == 0)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "System Error", "alert('System Error! Please try after sometime!');", true);
            }
            else if (RequestId == -1)
            {
                lblError.Text = "Invoice Details Already Exists!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('Invoice Details Already Exists!');", true);
            }
        }//END_IF_InputValid
    }

    private void AddInvoiceItem(int InvoiceId)
    {
        int PaymentId = 0; string strChargeName = "", strChargeCode = "", strHSN = "", strRemark = "";
        decimal decTotalAmount = 0m, decTaxAmount = 0m, decIGSTRate = 0m, decCGSTRate = 0m, decSGSTRate = 0m,
        decIGSTAmount = 0m, decCGSTAmount = 0m, decSGSTAmount = 0m, decOtherDeduction = 0m;

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];

            foreach (DataRow dr in dtCharges.Rows)
            {
                strChargeCode = dr["ChargeCode"].ToString();
                strChargeName = dr["ChargeName"].ToString();
                strHSN = dr["HSN"].ToString();
                decTaxAmount = Convert.ToDecimal(dr["TaxableValue"]);
                decCGSTRate = Convert.ToDecimal(dr["CGSTRate"]);
                decSGSTRate = Convert.ToDecimal(dr["SGSTRate"]);
                decIGSTRate = Convert.ToDecimal(dr["IGSTRate"]);

                decCGSTAmount = Convert.ToDecimal(dr["CGSTAmt"]);
                decSGSTAmount = Convert.ToDecimal(dr["SGSTAmt"]);
                decIGSTAmount = Convert.ToDecimal(dr["IGSTAmt"]);

                decTotalAmount = Convert.ToDecimal(dr["ChargeTotal"]);

                strRemark = dr["ChargeRemark"].ToString();

                int result = AccountExpense.AddInvoiceItem(InvoiceId, PaymentId, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmount, decCGSTAmount, decSGSTAmount, decOtherDeduction, strRemark, LoggedInUser.glUserId);
            }
        }
        else if (ddInvoiceType.SelectedValue == "1") // Final Invoice - One Charge Item
        {
            strChargeName = hdnChargeName.Value.Trim();
            strChargeCode = hdnChargeCode.Value.Trim();
            strHSN = hdnChargeHSN.Value;

            decCGSTRate = 0;
            decSGSTRate = 0;
            decIGSTRate = 0;

            decCGSTAmount = 0;
            decSGSTAmount = 0;

            Decimal.TryParse(lblTotalTaxableValue.Text.Trim(), out decTaxAmount);

            if (lblTotalGST.Text.Trim() != "")
            {
                decIGSTAmount = Convert.ToDecimal(lblTotalGST.Text.Trim());
            }
            else
            {
                decIGSTAmount = 0;
            }

            Decimal.TryParse(lblTotalValue.Text.Trim(), out decTotalAmount);

            strRemark = txtRemark.Text.ToString();

            int result = AccountExpense.AddInvoiceItem(InvoiceId, PaymentId, strChargeName, strChargeCode,
                strHSN, decTotalAmount, decTaxAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                decIGSTAmount, decCGSTAmount, decSGSTAmount, decOtherDeduction, strRemark, LoggedInUser.glUserId);
        }

    }
    private bool ValidateInput()
    {
        bool IsValid = true;
        decimal decTotalValue = 0;

        if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9")
        {

        }
        else
        {
            if (txtVendorName.Text.Trim() == "")
            {
                lblError.Text = "Please Select Vendor Name From Search List!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Vendor Error", "alert('" + lblError.Text + "');", true);

                IsValid = false;
                return IsValid;
            }
            if (hdnVendorCode.Value.Trim() == "")
            {
                lblError.Text = "Vendor Details Not Found! Please Select Vendor Name/GSTIN From List";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Vendor Error", "alert('" + lblError.Text + "');", true);

                IsValid = false;
                return IsValid;
            }

            if (ddInvoiceType.SelectedValue == "1") // Final Invoice
            {
                decimal decChargeTotal = 0m;

                decimal.TryParse(lblTotalValue.Text.Trim(), out decChargeTotal);

                decimal decInvoiceTotal = 0m;

                decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decInvoiceTotal);

                decimal decDiff = decChargeTotal - decInvoiceTotal;

                if (decChargeTotal == 0 || decInvoiceTotal == 0)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                    lblError.CssClass = "errorMsg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

                    IsValid = false;
                    return IsValid;

                }
                else if (decDiff > 1 || decDiff < -1)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                    lblError.CssClass = "errorMsg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

                    IsValid = false;
                    return IsValid;
                }

            }

        }

        if (txtTotalInvoiceValue.Text.Trim() == "" || txtTotalInvoiceValue.Text.Trim() == "0")
        {
            lblError.Text = "Please Enter Total Value!";
            txtTotalInvoiceValue.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

            IsValid = false;
            return IsValid;
        }
        else if (decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decTotalValue) == false)
        {
            lblError.Text = "Please Enter Valid Total Value!";
            txtTotalInvoiceValue.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

            IsValid = false;
            return IsValid;
        }


        return IsValid;

    }

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

    protected void wzRequest_PreRender(object sender, EventArgs e)
    {
        Repeater SideBarList1 = wzRequest.FindControl("HeaderContainer").FindControl("SideBarList") as Repeater;
        SideBarList1.DataSource = wzRequest.WizardSteps;
        SideBarList1.DataBind();
    }
    protected string GetClassForWizardStep(object wizardStep)
    {
        WizardStep step = wizardStep as WizardStep;

        if (step == null)
        {
            return "";
        }
        int stepIndex = wzRequest.WizardSteps.IndexOf(step);

        if (stepIndex < wzRequest.ActiveStepIndex)
        {
            return "prevStep";
        }
        else if (stepIndex > wzRequest.ActiveStepIndex)
        {
            return "nextStep";
        }
        else
        {
            return "currentStep";
        }
    }

    #endregion

    #region Job Detail
    private void GetJobDetail()
    {
        if (txtJobNumber.Text.Trim() != "")
        {
            string ModuleId = hdnModuleId.Value;
            if (hdnJobId.Value != "0")
            {
                DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(hdnModuleId.Value));

                if (dsGetJobDetail.Tables.Count > 0)
                {
                    lblConsgneeGSTIN.Text = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
                    hdnBOEGSTIN.Value = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();

                    if (dsGetJobDetail.Tables[0].Rows[0]["IsDifferentGSTNoAllowed"] != DBNull.Value)
                    {
                        // 1 - Allow To Enter Different Bill To Party/GSTIN
                        if (Convert.ToBoolean(dsGetJobDetail.Tables[0].Rows[0]["IsDifferentGSTNoAllowed"]) == true)
                        {
                            hdnDifferentGSTNoAllowed.Value = "1";
                        }
                        else if (Convert.ToBoolean(dsGetJobDetail.Tables[0].Rows[0]["IsDifferentGSTNoAllowed"]) == false)
                        {
                            hdnDifferentGSTNoAllowed.Value = "0";
                        }
                    }

                    lblBENo.Text = dsGetJobDetail.Tables[0].Rows[0]["BOENo"].ToString();
                    lblBLNo.Text = dsGetJobDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
                    lblBranchName.Text = dsGetJobDetail.Tables[0].Rows[0]["BranchName"].ToString();
                    lblIECNumber.Text = dsGetJobDetail.Tables[0].Rows[0]["IECNo"].ToString();

                    //Block Fund Request for Job - Flag - IsFundAllowed 1 = Alloswd, ) -Block
                    if (dsGetJobDetail.Tables[0].Rows[0]["IsFundAllowed"] != DBNull.Value)
                    {
                        string IsAllowed = dsGetJobDetail.Tables[0].Rows[0]["IsFundAllowed"].ToString();

                        if (IsAllowed == "0")
                        {
                            lblError.Text = "<b>Fund Request Blocked For Job No - </b>" + txtJobNumber.Text;
                            lblError.CssClass = "errorMsg";

                            txtJobNumber.Text = "";
                            hdnJobId.Value = "0";

                            return;
                        }
                    }

                    else if (ModuleId == "1")
                    {
                        string strNotingStatus = dsGetJobDetail.Tables[0].Rows[0]["NotingStatus"].ToString();
                        string strJobType = dsGetJobDetail.Tables[0].Rows[0]["JobType"].ToString();

                        if (strJobType != "14" && strJobType != "101" && strJobType != "201")
                        {
                            if (strNotingStatus != "True")
                            {
                                ViewState["JobrefNO"] = txtJobNumber.Text;
                                lblError.Text = "<b>First Complete The Noting Details</b>";
                                lblError.CssClass = "errorMsg";
                                txtJobNumber.Text = ViewState["JobrefNO"].ToString();
                                return;
                            }
                        }

                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"] != DBNull.Value)
                    {
                        hdnBranchId.Value = dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString();
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"] != DBNull.Value)
                    {
                        if (Convert.ToString(dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"]) != "")
                        {
                            lblPlanningDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"]).ToString("dd/MM/yyyy");
                        }
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["DutyAmount"] != DBNull.Value)
                    {
                        lblDutyAmount.Text = dsGetJobDetail.Tables[0].Rows[0]["DutyAmount"].ToString();
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["AssessableValue"] != DBNull.Value)
                    {
                        lblAssessableValue.Text = dsGetJobDetail.Tables[0].Rows[0]["AssessableValue"].ToString();
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["IGSTAmt"] != DBNull.Value)
                    {
                        lblIGSTAmount.Text = dsGetJobDetail.Tables[0].Rows[0]["IGSTAmt"].ToString();
                    }

                    lblCustomer.Text = dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                    lblConsignee.Text = dsGetJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
                }
            }
        }
        else
        {
            hdnBranchId.Value = "0";
            lblConsignee.Text = "";
            lblCustomer.Text = "";
        }
    }
    protected void ddlExpenseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // 1 - Duty, 2 - CFS

        txtTotalInvoiceValue.Text = "";

        rblMultiChargeCode.Enabled = true;
        txtChargeName.Enabled = true;
        txtChargeCode.Enabled = true;

        ddGSTINType.Enabled = true;
        ddInvoiceType.Enabled = true;

        txtCongisgneeGSTIN.Enabled = true;
        txtCongisgneeName.Enabled = true;

        chkNoGSTINCustomer.Enabled = true;
        trDutyInterest.Visible = false;
        rdlInterest.SelectedValue = "2";
        txtInterestAmnt.Text = "0";

        // Duty/Panelty Payment -Final - Vendor Customs - GST Amount = 0

        if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9")
        {
            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = false;

            ddGSTINType.SelectedValue = "2";
            ddGSTINType.Enabled = false;

            rblMultiChargeCode.SelectedValue = "0";
            rblMultiChargeCode.Enabled = false;

            lblInvoiceNo.Text = "Challan No";
            lblInvoiceDate.Text = "Challan Date";
            lblTotalInvoiceValue.Text = "Duty Amount";

            if (ddlExpenseType.SelectedValue == "1")
            {
                txtTotalInvoiceValue.Text = lblDutyAmount.Text.Trim();
                trDutyInterest.Visible = true;
                chkNoGSTINCustomer.Enabled = false;
            }
        }
        else if (ddlExpenseType.SelectedValue == "9") // Panelty - Vendor Info Not Requuired
        {
            ddGSTINType.SelectedValue = "2";

            txtCongisgneeGSTIN.Text = lblConsgneeGSTIN.Text;
            txtCongisgneeName.Text = lblConsignee.Text;
            chkNoGSTINCustomer.Enabled = false;

            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = false;
            ddRIM.SelectedValue = "1";
            ddRIM.Enabled = false;

            lblInvoiceNo.Text = "Challan No";
            lblInvoiceDate.Text = "Challan Date";
            lblTotalInvoiceValue.Text = "Penalty Amount";
        }
        else
        {
            txtCongisgneeGSTIN.Text = "";
            txtCongisgneeName.Text = "";
            chkNoGSTINCustomer.Enabled = true;

            trDutyInterest.Visible = false;
            txtInterestAmnt.Text = "0";

            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = true;

            lblInvoiceNo.Text = "Invoice No";
            lblInvoiceDate.Text = "Invoice Date";
            lblTotalInvoiceValue.Text = "Total Invoice Value";
        }
    }
    protected void ddInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        rblMultiChargeCode.Enabled = true;

        if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9")
        {
            rblMultiChargeCode.SelectedValue = "0";
            rblMultiChargeCode.Enabled = false;
        }
        if (ddInvoiceType.SelectedValue == "2")
        {
            rblMultiChargeCode.SelectedValue = "0";
            rblMultiChargeCode.Enabled = false;
        }
    }
    protected void rdlInterest_OnSelectedIndexChanged(object sender, EventArgs e)  //radio button click
    {
        if (rdlInterest.SelectedItem.Text.ToLower() == "yes")
        {
            tdlblInterestAmnt.Visible = true;
            tdtxtInterestAmnt.Visible = true;

            CVInterestAmount.Enabled = true;
        }
        else
        {
            tdlblInterestAmnt.Visible = false;
            tdtxtInterestAmnt.Visible = false;

            CVInterestAmount.Enabled = false;

            txtInterestAmnt.Text = "0";
        }
    }
    protected void txtInterestAmnt_OnTextChanged(object sender, EventArgs e)
    {
        TotalDutyAmount();
    }
    public void TotalDutyAmount()
    {
        double DutyAmnt = 0;
        double interestAmnt = 0;
        double peneltyAmnt = 0;
        double TotalAmnt = 0;
        double Amount = 0;

        if (ddlExpenseType.SelectedValue == "1")
        {
            if (txtTotalInvoiceValue.Text.Trim() != "")
            {
                DutyAmnt = Convert.ToDouble(txtTotalInvoiceValue.Text.Trim());
            }

            if (txtInterestAmnt.Text.Trim() != "")
            {
                interestAmnt = Convert.ToDouble(txtInterestAmnt.Text.Trim());
            }


            TotalAmnt = DutyAmnt + interestAmnt + peneltyAmnt;
            lblTotalTaxableValue.Text = Convert.ToString(TotalAmnt);
            lblTotalValue.Text = Convert.ToString(TotalAmnt);
        }
    }

    #endregion

    #region Vendor Detail

    protected void ddGSTINType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSupplierGSTIN.Text = "";
        txtVendorName.Text = "";
        txtVendorPANNo.Text = "";

        if (ddGSTINType.SelectedValue == "1")
        {
            txtSupplierGSTIN.Enabled = true;
            txtVendorName.Enabled = false;
            txtVendorPANNo.Enabled = false;

            //  ddCurrency.SelectedValue = "46";
            //  txtExchangeRate.Text = "1";
            //  txtExchangeRate.Enabled = false;
        }
        else if (ddGSTINType.SelectedValue == "2")
        {
            txtSupplierGSTIN.Enabled = false;
            txtVendorName.Enabled = true;
            txtVendorPANNo.Enabled = true;

            // txtExchangeRate.Text = "1";
            // txtExchangeRate.Enabled = true;
        }
        else if (ddGSTINType.SelectedValue == "3")
        {
            txtSupplierGSTIN.Enabled = false;
            txtVendorName.Enabled = true;
            txtVendorPANNo.Enabled = false;

            //txtExchangeRate.Enabled = true;
        }

        //MultiChargeCodeVendorTypeNonRIM();
    }

    #endregion


    #region Charge Detail

    private void GetExpenseTypeDetail()
    {
        int ExpenseTypeId = Convert.ToInt32(ddlExpenseType.SelectedValue);

        DataSet dsExpenseType = AccountExpense.GetExpenseTypeById(ExpenseTypeId);

        if (dsExpenseType.Tables[0].Rows.Count > 0)
        {
            txtChargeName.Text = dsExpenseType.Tables[0].Rows[0]["ChargeName"].ToString();
            txtChargeCode.Text = dsExpenseType.Tables[0].Rows[0]["ChargeCode"].ToString(); ;

            hdnChargeName.Value = dsExpenseType.Tables[0].Rows[0]["ChargeName"].ToString();
            hdnChargeCode.Value = dsExpenseType.Tables[0].Rows[0]["ChargeCode"].ToString(); ;
            hdnChargeHSN.Value = dsExpenseType.Tables[0].Rows[0]["ChargeHSN"].ToString(); ;
        }
        else
        {
            txtChargeName.Text = "";
            txtChargeCode.Text = "";

            hdnChargeName.Value = "";
            hdnChargeCode.Value = "";
            hdnChargeHSN.Value = "";
        }
    }

    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        AddCharges();
    }
    private void AddCharges()
    {
        if (hdnChargeName.Value == "" || txtChargeName.Text.Trim() == "")
        {
            lblError.Text = "Please Select Charge Name!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Charge Error", "alert('" + lblError.Text + "');", true);
            return;
        }
        if (hdnChargeCode.Value == "" || txtChargeCode.Text.Trim() == "")
        {
            lblError.Text = "Please Select Charge Name!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Charge Error", "alert('" + lblError.Text + "');", true);
            return;
        }

        decimal decTaxableAmount = 0, decGSTRate = 0, decGSTAmount = 0, decTotalAmount = 0;
        decimal decIGSTRate = 0, decCGSTRate = 0, decSGSTRate = 0;
        decimal decIGSTAmt = 0, decCGSTAmt = 0, decSGSTAmt = 0;

        decimal decExchangeRate = 1;

        decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

        Decimal.TryParse(txtTaxableValue.Text.Trim(), out decTaxableAmount);

        if (decTaxableAmount <= 0)
        {
            lblError.Text = "Please Enter Valid Taxable Amount!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Charge Error", "alert('" + lblError.Text + "');", true);
            return;
        }

        decTaxableAmount = Convert.ToDecimal(txtTaxableValue.Text.Trim()) * decExchangeRate;

        decGSTRate = Convert.ToDecimal(ddGSTRate.SelectedValue);

        if (decGSTRate > 0)
        {
            decGSTAmount = (decTaxableAmount * decGSTRate) / 100;


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
        }

        decTotalAmount = decTaxableAmount + decGSTAmount;

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }

        if (dtCharges.Rows.Count > 0)
        {
            foreach (DataRow dr01 in dtCharges.Rows)
            {
                if (dr01["ChargeCode"].ToString() == hdnChargeCode.Value.Trim())
                {
                    lblError.Text = "Charge Name Already Added!";
                    lblError.CssClass = "errorMsg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Charge Error", "alert('" + lblError.Text + "');", true);
                    return;
                }
            }
        }

        DataRow dr = dtCharges.NewRow();

        dr["ChargeCode"] = hdnChargeCode.Value.Trim();
        dr["ChargeName"] = hdnChargeName.Value.Trim();
        dr["HSN"] = hdnChargeHSN.Value.Trim();

        dr["TaxableValue"] = txtTaxableValue.Text.Trim();
        dr["CGSTRate"] = decCGSTRate.ToString();
        dr["SGSTRate"] = decSGSTRate.ToString();
        dr["IGSTRate"] = decIGSTRate.ToString();

        dr["CGSTAmt"] = decCGSTAmt.ToString();
        dr["SGSTAmt"] = decSGSTAmt.ToString();
        dr["IGSTAmt"] = decIGSTAmt.ToString();

        dr["ChargeTotal"] = decTotalAmount.ToString();
        dr["ChargeRemark"] = "";

        //OtherDeduction - Not Required On Payment Request Page
        dr["OtherDeduction"] = "0";

        dtCharges.Rows.Add(dr);
        dtCharges.AcceptChanges();

        ViewState["vwsCharges"] = dtCharges;

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();

        // Calculate Total Charges
        CalcuateTotalCharges();

        // Reset Charge TextBox Value

        ResetCharges();
    }

    private DataTable GetChargeCodeDataTable()
    {
        DataTable dtChargeCode = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colChargeCode = new DataColumn("ChargeCode", Type.GetType("System.String"));
        DataColumn colChargeName = new DataColumn("ChargeName", Type.GetType("System.String"));
        DataColumn colHSN = new DataColumn("HSN", Type.GetType("System.String"));
        DataColumn colTaxableValue = new DataColumn("TaxableValue", Type.GetType("System.String"));
        DataColumn colOtherDeduction = new DataColumn("OtherDeduction", Type.GetType("System.String"));
        DataColumn colIGSTRate = new DataColumn("IGSTRate", Type.GetType("System.String"));
        DataColumn colIGSTAmt = new DataColumn("IGSTAmt", Type.GetType("System.String"));
        DataColumn colCGSTRate = new DataColumn("CGSTRate", Type.GetType("System.String"));
        DataColumn colCGSTAmt = new DataColumn("CGSTAmt", Type.GetType("System.String"));
        DataColumn colSGSTRate = new DataColumn("SGSTRate", Type.GetType("System.String"));
        DataColumn colSGSTAmt = new DataColumn("SGSTAmt", Type.GetType("System.String"));

        DataColumn colChargeTotal = new DataColumn("ChargeTotal", Type.GetType("System.String"));
        DataColumn colChargeRemark = new DataColumn("ChargeRemark", Type.GetType("System.String"));

        dtChargeCode.Columns.Add(colSL);
        dtChargeCode.Columns.Add(colChargeCode);
        dtChargeCode.Columns.Add(colChargeName);
        dtChargeCode.Columns.Add(colHSN);
        dtChargeCode.Columns.Add(colTaxableValue);
        dtChargeCode.Columns.Add(colOtherDeduction);

        dtChargeCode.Columns.Add(colCGSTRate);
        dtChargeCode.Columns.Add(colSGSTRate);
        dtChargeCode.Columns.Add(colIGSTRate);

        dtChargeCode.Columns.Add(colCGSTAmt);
        dtChargeCode.Columns.Add(colSGSTAmt);
        dtChargeCode.Columns.Add(colIGSTAmt);

        dtChargeCode.Columns.Add(colChargeTotal);
        dtChargeCode.Columns.Add(colChargeRemark);

        dtChargeCode.PrimaryKey = new DataColumn[] { colSL };
        dtChargeCode.AcceptChanges();

        return dtChargeCode;
    }
    private void CalcuateTotalCharges()
    {
        decimal decEnteredTotalValue = Convert.ToDecimal(lblEnteredTotalValue.Text.Trim());

        decimal decTotalTaxableValue = 0, decTotalGSTValue = 0, decTotalInvoiceValue = 0;

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];

            foreach (DataRow dr in dtCharges.Rows)
            {
                if (dr["TaxableValue"].ToString() != "")
                {
                    decTotalTaxableValue = decTotalTaxableValue + Convert.ToDecimal(dr["TaxableValue"]);
                }
                if (dr["CGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["CGSTAmt"]);
                }
                if (dr["SGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["SGSTAmt"]);
                }
                if (dr["IGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["IGSTAmt"]);
                }
                if (dr["ChargeTotal"].ToString() != "")
                {
                    decTotalInvoiceValue = decTotalInvoiceValue + Convert.ToDecimal(dr["ChargeTotal"]);
                }
            }
        }

        lblTotalTaxableValue.Text = decTotalTaxableValue.ToString();
        lblTotalGST.Text = decTotalGSTValue.ToString();
        lblTotalValue.Text = decTotalInvoiceValue.ToString();

        // Allow Add more Charges

        int ChargeRowCount = gvCharges.Rows.Count;

        if (rblMultiChargeCode.SelectedValue == "0")
        {
            // Max one Row Allowed

            if (ChargeRowCount > 0)
            {
                // Compare Total Amount with Total Entered INR
                decimal decDiff = decEnteredTotalValue - decTotalInvoiceValue;

                if (decEnteredTotalValue == 0 || decTotalInvoiceValue == 0)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                    lblError.CssClass = "errorMsg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

                }
                else if (decDiff > 5 || decDiff < -5)
                {
                    // Invalid Tax and Total Calculation
                    // Clear ViewStat
                    // Clear Calculation

                    ViewState["vwsCharges"] = null;

                    lblError.Text = "Total Invoice Amount not Match With Taxable Value + GST Amount! Please Check Taxable Value & GST Rate!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

                }
                else
                {
                    btnAddCharges.Visible = false;
                }
            }
            else
            {
                btnAddCharges.Visible = true;
            }
        }
        else
        {
            btnAddCharges.Visible = true;
        }
    }
    private void AutoCalculateTotalAmount()
    {
        decimal decTaxableValue = 0;
        decimal decTotalGST = 0;
        decimal decTotalValue = 0;

        decimal.TryParse(lblTotalTaxableValue.Text.Trim(), out decTaxableValue);

        decimal.TryParse(lblTotalGST.Text.Trim(), out decTotalGST);

        decTotalValue = decTaxableValue + decTotalGST;

        lblTotalValue.Text = decTotalValue.ToString();
    }
    protected void lblTotalTaxableValue_TextChanged(object sender, EventArgs e)
    {
        AutoCalculateTotalAmount();
    }

    protected void lblTotalGST_TextChanged(object sender, EventArgs e)
    {
        AutoCalculateTotalAmount();
    }
    protected void lnlRemove_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string strChargeId = lnk.CommandArgument.ToString();

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }

        dtCharges.Rows.Find(strChargeId).Delete();

        dtCharges.AcceptChanges();

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();

        ViewState["vwsCharges"] = dtCharges;

        // Calculate Total Charges
        CalcuateTotalCharges();
    }
    #endregion

    private void ResetCharges()
    {
        if (rblMultiChargeCode.SelectedValue == "1")// Multuple Charge
        {
            if (ViewState["vwsCharges"] != null)
            {
                txtChargeCode.Text = "";
                txtChargeName.Text = "";
                txtChargeCode.Enabled = true;
                txtChargeName.Enabled = true;

                hdnChargeCode.Value = "";
                hdnChargeName.Value = "";

                hdnChargeHSN.Value = "";
                txtTaxableValue.Text = "";
                txtChargeRemark.Text = "";

                if (ddGSTINType.SelectedValue == "2" || ddGSTINType.SelectedValue == "3")
                {
                    // Un-Register or Foreign Party
                    ddGSTRate.SelectedValue = "0"; // No GST Applicable
                }
                else
                {
                    // Register
                    ddGSTRate.SelectedIndex = -1;
                }

                DataTable dtCharges = (DataTable)ViewState["vwsCharges"];

                if (dtCharges.Rows.Count > 0)
                {
                    rblGSTType.Enabled = false;
                }
                else
                {
                    rblGSTType.Enabled = true;
                }

            }
            else
            {
                rblGSTType.Enabled = true;
            }

        }
        else
        {
            // txtChargeCode.Text = "";
            // txtChargeName.Text = "";
            hdnChargeHSN.Value = "";
            txtTaxableValue.Text = "";
            txtChargeRemark.Text = "";

            if (ddGSTINType.SelectedValue == "2" || ddGSTINType.SelectedValue == "3")
            {
                // Un-Register or Foreign Party
                ddGSTRate.SelectedValue = "0"; // No GST Applicable
            }
            else
            {
                ddGSTRate.SelectedIndex = -1;
            }
        }
    }

}