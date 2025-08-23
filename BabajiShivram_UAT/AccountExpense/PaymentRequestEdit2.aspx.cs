using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
public partial class AccountExpense_PaymentRequestEdit2 : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Update Payment Request";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("InvoiceRejected.aspx");
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
            Response.Redirect("InvoiceRejected.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        
        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        DataSet dsInvoiceItem = AccountExpense.GetInvoiceItem(InvoiceID);

        gvCharges.DataSource = dsInvoiceItem;

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            bool IsInterest = false;
            decimal decInterestAmount = 0m;
            int BillType = 0;

            hdnStatusId.Value = dsDetail.Tables[0].Rows[0]["lStatus"].ToString();

            txtJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();

            if (dsDetail.Tables[0].Rows[0]["ExpenseTypeId"].ToString() != "6")
            {
                ddlExpenseType.SelectedValue = dsDetail.Tables[0].Rows[0]["ExpenseTypeId"].ToString();
            }

            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");
            ddInvoiceType.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString();
            ddGSTINType.SelectedValue = dsDetail.Tables[0].Rows[0]["VendorGSTNTypeId"].ToString();
           
            int ExpenseTypeId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ExpenseTypeId"]);
            BillType = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["BillType"]);

            hdnBillType.Value = BillType.ToString();

            if (BillType == 3)
            {
                rblCreditVendor.SelectedValue = "1";
                hdnIsCreditVendor.Value = "1";

                // Disable PD Account

                rblPDAccount.Enabled = false;
            }
            else if (BillType == 11)
            {
                rblPDAccount.SelectedValue = "1";
            }

            if (ExpenseTypeId == 1 || ExpenseTypeId == 9 || ExpenseTypeId == 12 || ExpenseTypeId == 13 || ExpenseTypeId == 100)
            {
                ddlExpenseType.Enabled = false;
                ddGSTINType.Enabled = false;
                ddInvoiceType.SelectedValue = "1"; // Final Invoice
                ddInvoiceType.Enabled = false;
            }
            

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
            lblVendorType.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNType"].ToString();
            txtPaymentTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();

            //lblRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();

            txtInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            
            //lblInvoiceDate.Text = dsDetail.Tables[0].Rows[0]["InvoiceDate"].ToString();
            //txtPaymentDueDate.Text = dsDetail.Tables[0].Rows[0]["PaymentDueDate"].ToString();
            //txtPaymentRequiredDate.Text = dsDetail.Tables[0].Rows[0]["PaymentRequiredDate"].ToString();
            
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();

            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyId"] != DBNull.Value)
            {
                ddCurrency.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyId"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                txtExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentTypeId"] != DBNull.Value)
            {                
                ddlPaymentType.SelectedValue = dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["IsAdvanceReceived"] != DBNull.Value)
            {
                bool IsAdvanceReceived = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsAdvanceReceived"]);
                
                if(IsAdvanceReceived == true)
                {
                    rblAdvanceReceived.SelectedValue = "1";

                    txtAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();
                }
                else
                {
                    rblAdvanceReceived.SelectedValue = "0";
                }

            }
            
            if(dsDetail.Tables[0].Rows[0]["ExpenseTypeId"].ToString() == "1")
            {
                trDutyInterest.Visible = true;
                if (dsDetail.Tables[0].Rows[0]["IsInterest"] != DBNull.Value)
                {
                    if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsInterest"]) == true)
                    {
                        trDutyInterest.Visible = true;
                        tdtxtInterestAmnt.Visible = true;
                        rdlInterest.SelectedValue = "1";

                        txtInterestAmnt.Text = dsDetail.Tables[0].Rows[0]["InterestAmount"].ToString();

                        IsInterest = true;
                        decInterestAmount = Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["InterestAmount"].ToString());
                    }
                }
            }
            //lblAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            //lblAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            //lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            //lblBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            //lblBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();

            //lblRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            //lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();
            //lblPaidAmount.Text = dsDetail.Tables[0].Rows[0]["PaidAmount"].ToString();


            if (dsDetail.Tables[0].Rows[0]["InvoiceAmount"] != DBNull.Value)
            {
                if (IsInterest == true)
                {
                    txtTotalInvoiceValue.Text= (Convert.ToDecimal(dsDetail.Tables[0].Rows[0]["InvoiceAmount"]) - decInterestAmount).ToString();
                }
                else
                {
                    txtTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
                }
            }

            if(dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"]!= DBNull.Value)
            {
                int ProformaInvoiceId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ProformaInvoiceId"]);

                hdnProformaInvoiceId.Value = ProformaInvoiceId.ToString();

                if (ProformaInvoiceId > 0)
                {
                    // Proforma to Final Invoice Submission
                    // Can not Change Invoice Type - Final/Vendor Detail
                    ddlExpenseType.Enabled = false;
                    ddInvoiceType.Enabled = false;

                    ddGSTINType.Enabled = false;

                    txtSupplierGSTIN.Enabled = false;
                    txtPaymentTerms.Enabled = false;
                    txtVendorName.Enabled = false;
                    txtVendorPANNo.Enabled = false;
                }
            }

            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                txtInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            {
                txtPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentRequiredDate"] != DBNull.Value)
            {
                txtPaymentRequiredDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentRequiredDate"]).ToString("dd/MM/yyyy");
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

        // Immediate Payment

        if(dsDetail.Tables[0].Rows[0]["IsImmediatePayment"] != DBNull.Value)
        {
            bool IsImmediatePayment = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsImmediatePayment"]);

            if(IsImmediatePayment == false)
            {
                rblImmediatePayment.SelectedValue = "0";
                CalExtPayDueDate.Enabled = false;
                CalExtPayReqrdDate.Enabled = true;
            }
            else
            {
                rblImmediatePayment.SelectedValue = "1";
                CalExtPayDueDate.Enabled = true;
                CalExtPayReqrdDate.Enabled = true;

            }
        }

        // Get Job Detail

        GetJobDetail();

    }

    #region Wizard Event
    protected void wzRequest_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        SetVendorSearchType();
        RFVDocEmailApproval.Enabled = false;

        int IndexId = e.CurrentStepIndex;

        int TotalSteps = wzRequest.WizardSteps.Count;

        int JobId; string strJobRefNO; int ModuleID; int InvoiceType = 1; int InvoiceMode = 1; bool isRIM = false;
        int VendorGSTNType, PaymentTypeId = 0, ExpenseTypeId = 0, BillType = 0; 
        string strConsigneeGSTIN = "", strConsigneeName = "", strConsigneeCode = "", strConsigneePAN = "";

        string strVendorName, strVendorCode, strVendorGSTNo, strVendorPAN, strInvoiceNo, strRemark;

        decimal decExchangeRate = 1;

        bool IsAdvanceReceived = false; decimal AdvanceAmount = 0m;
        bool IsInterest = false; decimal InterestAmount = 0m;
        string strPaymentTerms; string strInvoiceFilePath = "";

        Decimal decInvoiceAmount = 0m, decTaxableAmount = 0m, decGSTAmount = 0m;

        DateTime dtInvoiceDate = DateTime.MinValue, dtPaymentRequiredDate = DateTime.MinValue, dtPaymentDueDate = DateTime.MinValue;

        JobId = Convert.ToInt32(hdnJobId.Value);
        ModuleID = Convert.ToInt32(hdnModuleId.Value);
        strJobRefNO = txtJobNumber.Text.Trim();

        if (rblCreditVendor.SelectedValue == "1")
        {
            BillType = 3;
        }

        InvoiceMode = Convert.ToInt32(rblInvoiceMode.SelectedValue);

        if (InvoiceMode > 1)
        {
            ddInvoiceType.SelectedValue = "1"; // Final Invoice
        }
        InvoiceType = Convert.ToInt32(ddInvoiceType.SelectedValue);

        VendorGSTNType = Convert.ToInt32(ddGSTINType.SelectedValue);

        if (txtSupplierGSTIN.Text.Trim() != "")
        {
            VendorGSTNType = 1; // Registered
        }

        // Foreign Currency
        int InvoiceCurrencyId = 46; // INR
        Decimal decInvoiceCurrencyAmt = 0m, decInvoiceCurrencyExchangeRate = 1;

        if (ddCurrency.SelectedIndex > 0 && ddGSTINType.SelectedValue == "3")
        {
            InvoiceCurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
            decimal.TryParse(txtExchangeRate.Text.Trim(), out decInvoiceCurrencyExchangeRate);
        }
        else
        {
            ddCurrency.SelectedValue = "46"; // INR
            txtExchangeRate.Text = "1";
        }

        if (decInvoiceCurrencyExchangeRate <= 0)
        {
            lblError.Text = "Invalid Currency Exchange Rate!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('" + lblError.Text + "');", true);
            e.Cancel = true;

            return;
        }

        decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

        if (IndexId == 0) // Vendor/Invoice To Party Detail
        {
            // txtCongisgneeGSTIN.Text = "";
            // txtCongisgneeName.Text = "";
            SetVendorStatus();

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
            else if (ddlExpenseType.SelectedValue == "12") // SIMS Payment
            {
                txtSupplierGSTIN.Enabled = false;
                txtVendorName.Enabled = false;
                txtVendorPANNo.Enabled = false;

                // Fix Vendor Code Dor Duty and Panelty

                txtSupplierGSTIN.Text = "";
                txtVendorName.Text = "SIMS Registration Payment";
                txtVendorPANNo.Text = "";
                hdnVendorCode.Value = "SRP001";

                // Fix BIll To Party - BoE Consingee GSTIN

                txtCongisgneeGSTIN.Text = lblConsgneeGSTIN.Text;
                txtCongisgneeName.Text = lblConsignee.Text;

                txtCongisgneeGSTIN.Enabled = false;
                txtCongisgneeName.Enabled = false;

            }
            else if (ddlExpenseType.SelectedValue == "13") // SIMS Payment
            {
                txtSupplierGSTIN.Enabled = false;
                txtVendorName.Enabled = false;
                txtVendorPANNo.Enabled = false;

                // Fix Vendor Code Dor Duty and Panelty

                txtSupplierGSTIN.Text = "";
                txtVendorName.Text = "GTI Port Registration";
                txtVendorPANNo.Text = "";
                hdnVendorCode.Value = "GPR000";

                // Fix BIll To Party - BoE Consingee GSTIN

                txtCongisgneeGSTIN.Text = lblConsgneeGSTIN.Text;
                txtCongisgneeName.Text = lblConsignee.Text;

                txtCongisgneeGSTIN.Enabled = false;
                txtCongisgneeName.Enabled = false;

            }
            //else if (VendorGSTNType == 1) // Registered
            //{
            //    txtSupplierGSTIN.Enabled = true;
            //    txtVendorName.Enabled = false;
            //    txtVendorPANNo.Enabled = false;
            //}
            //else
            //{
            //    txtSupplierGSTIN.Enabled = false;
            //    txtVendorName.Enabled = true;
            //    txtVendorPANNo.Enabled = false;
            //}

        }
        else if (IndexId == 1) // Invoice Detail
        {
            SetVendorStatus();

            if (txtVendorName.Text.Trim() == "")
            {
                lblError.Text = "Please Select Vendor Name From Search List!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Vendor Error", "alert('" + lblError.Text + "');", true);
                e.Cancel = true;
                return;

            }
            if (hdnVendorCode.Value.Trim() == "")
            {
                lblError.Text = "Vendor Details Not Found! Please Select Vendor Name/GSTIN From List";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Vendor Error", "alert('" + lblError.Text + "');", true);
                e.Cancel = true;
                return;

            }

            // Consignee/Bill To Party GSTIN should be BoE GGSTIN or Babaji GSTIN
            // If Not Match with any GSTIN Not allow to procedd

            string CheckBabajiGSTIN = "";

            bool isBabajiGST = BabajiGSTINDict.BabajiGSTIN.TryGetValue(txtCongisgneeGSTIN.Text.Trim(), out CheckBabajiGSTIN);

            if (isBabajiGST == true)
            {
                hdnIsRIM.Value = "1"; // Babaji GSTIN // Non RIM
                ddRIM.SelectedValue = "2"; // Non-RIM
            }
            else if (hdnBOEGSTIN.Value.Trim().ToUpper() == txtCongisgneeGSTIN.Text.Trim().ToUpper() && BillType == 0)
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "BOE GSTIN", "alert('" + hdnBOEGSTIN.Value + "');", true);

                hdnIsRIM.Value = "0"; // Consignee GSTIN // RIM
                ddRIM.SelectedValue = "1"; // RIM
            }
            else
            {
                //   ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party GSTIN", "alert('" + txtCongisgneeGSTIN.Text + "');", true);

                // Check with Babaji GSTIN
                                
                if (CheckBabajiGSTIN == null || CheckBabajiGSTIN == "")
                {
                    // Invalid Bill To Party GSTIN

                    if (hdnDifferentGSTNoAllowed.Value == "1" && txtCongisgneeGSTIN.Text.Trim().Length == 15 && hdnBOEGSTIN.Value.Length == 15 && BillType ==0)
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
                            if (BillType == 3 || InvoiceMode == 2)
                            {
                                lblError.Text = "Credit Vendor - Invoice To GSTIN Not Matched with Babaji GSTIN !";
                            }
                            else
                            {
                                lblError.Text = "Invoice To GSTIN Not Matched with Consignee Other GSTIN !";
                            }
                            
                            lblError.CssClass = "errorMsg";

                            ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party GSTIN", "alert('" + lblError.Text + "');", true);

                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        if (BillType == 3)
                        {
                            lblError.Text = "Invoice To GSTIN Not Matched with Babaji GSTIN !";
                        }
                        else
                        {
                            lblError.Text = "Invoice To GSTIN Not Matched with Consignee Other GSTIN !";
                        }

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
                        lblError.Text = "Bill to Party Details Not Found in Account System! Please Select From Search List.";
                        lblError.CssClass = "errorMsg";

                        ScriptManager.RegisterStartupScript(this, GetType(), "Bill To Party Error", "alert('" + lblError.Text + "');", true);

                        e.Cancel = true;

                        return;
                    }
                }
            }

            // Get Vendor Detail From FA System

            if (hdnConsigneeCode.Value.Trim() == "")
            {
                lblError.Text = "Vendor Detail Not Found! Please Select From Search List.";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Vendor Error", "alert('" + lblError.Text + "');", true);

                e.Cancel = true;
            }

            int PaymentTermsDays = 0;

            // RIM & Non-RIM
            // IF Job BoE GSTIN Match WITH Invoice To Party GSTIN - Non_RIM
            // IF Job BoE GSTIN Match WITH BABAJI GSTIN - RIM
            // ELSE - Non_RIM

            string strBoEStateCode = "";

            if (hdnBOEGSTIN.Value != "")
            {
                strBoEStateCode = hdnBOEGSTIN.Value.Substring(0, 2);
            }

            string strConsingneeStateCode = "";

            if (txtCongisgneeGSTIN.Text.Trim() != "")
            {
                strConsingneeStateCode = txtCongisgneeGSTIN.Text.Trim().Substring(0, 2);
            }

            //if (strBoEStateCode != strConsingneeStateCode)
            //{
            //    if (chkNoGSTINCustomer.Checked == false && strConsingneeStateCode != "")
            //    {
            //        lblError.Text = "BoE GSTIN and Invoice to GSTIN State are Different. Please Check Bill To GSTIN";
            //        lblError.CssClass = "errorMsg";
            //        ScriptManager.RegisterStartupScript(this, GetType(), "GSTIN Error", "alert('" + lblError.Text + "');", true);

            //        e.Cancel = true;

            //        return;
            //    }
            //}

            /*******************************************************
            if (hdnBOEGSTIN.Value == txtCongisgneeGSTIN.Text.Trim())
            {
                ddRIM.SelectedValue = "1"; // RIM

                // FOR RIM - Only Single Charge Code Allowded

                //if (rblMultiChargeCode.SelectedValue == "1")
                //{
                //    rblMultiChargeCode.SelectedValue = "0";
                //}
            }
            else if (hdnIsRIM.Value == "0")
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
                // Check if BoE GSTIN Sate matche with Party GSTIN


                ddRIM.SelectedValue = "2"; // Non-RIM
            }
            *************************************************/
            // Display Prevoid Step Value

            lblJobRefNO.Text = txtJobNumber.Text.Trim();
            lblExpenseType.Text = ddlExpenseType.SelectedItem.Text;
            lblInvoiceType.Text = ddInvoiceType.SelectedItem.Text;

            if (VendorGSTNType == 1)
            {
                lblVendorType.Text = "Registered";
            }
            else
            {
                lblVendorType.Text = ddGSTINType.SelectedItem.Text;
            }

            if (rdlInterest.SelectedValue == "1")
            {
                if (txtInterestAmnt.Text.Trim() != "")
                {
                    InterestAmount = Convert.ToDecimal(txtInterestAmnt.Text.Trim());
                }
            }

            lblInvoiceCurrency.Text = ddCurrency.SelectedItem.Text;
            lblExchangeRate.Text = txtExchangeRate.Text.Trim();

            lblEnteredTotalValue.Text = ((Convert.ToDecimal(txtTotalInvoiceValue.Text) * decExchangeRate) + InterestAmount).ToString();

            lblMultipleGSTRate.Text = rblMultiChargeCode.SelectedItem.Text;

            if (InvoiceType == 1)
            {
                if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9" || ddlExpenseType.SelectedValue == "12" || ddlExpenseType.SelectedValue == "13")
                {
                    //lblTotalTaxableValue.Text = (Convert.ToDecimal(txtTotalInvoiceValue.Text) * decExchangeRate).ToString();
                    //lblTotalValue.Text = (Convert.ToDecimal(txtTotalInvoiceValue.Text) * decExchangeRate).ToString();
                    //lblTotalGST.Text = "0";

                    lblTotalTaxableValue.Text = (Convert.ToDecimal(txtTotalInvoiceValue.Text) + InterestAmount).ToString();
                    lblTotalValue.Text = (Convert.ToDecimal(txtTotalInvoiceValue.Text) + InterestAmount).ToString();
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
                       // fldInvoiceItem.Visible = false;

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
            else if (ddlExpenseType.SelectedValue == "12" || ddlExpenseType.SelectedValue == "13") // SIMS / GTI Port Payment  - RIM
            {
                txtChargeName.Text = "Registration Charges";
                txtChargeCode.Text = "R15";

                hdnChargeName.Value = "Registration Charges";
                hdnChargeCode.Value = "R15";
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

        int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        bool bIsInputValid = ValidateInput();

        if (bIsInputValid)
        {
            int JobId; string strJobRefNO; int ModuleID; int InvoiceType = 1; bool isRIM = false;
            int VendorGSTNType, PaymentTypeId = 0, ExpenseTypeId = 0;
            string strConsigneeGSTIN = "", strConsigneeName = "", strConsigneeCode = "", strConsigneePAN = "";

            string strVendorName, strVendorCode, strVendorGSTNo, strVendorPAN, strInvoiceNo, strRemark;
            bool IsAdvanceReceived = false; bool IsImmediatePayment = false;
            decimal AdvanceAmount = 0m;
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

            decInvoiceCurrencyAmt = Convert.ToDecimal(lblEnteredTotalValue.Text.Trim());


            JobId               = Convert.ToInt32(hdnJobId.Value);
            ModuleID            = Convert.ToInt32(hdnModuleId.Value);
            strJobRefNO         = txtJobNumber.Text.Trim();
            VendorGSTNType      = Convert.ToInt32(ddGSTINType.SelectedValue);

            strConsigneeGSTIN   = txtCongisgneeGSTIN.Text.Trim();
            strConsigneeName    = txtCongisgneeName.Text.Trim();
            strConsigneeCode    = hdnConsigneeCode.Value.Trim();
            strConsigneePAN     = hdnConsigneePANNo.Value.Trim();

            strRemark           = txtRemark.Text.Trim();

            InvoiceType         = Convert.ToInt32(ddInvoiceType.SelectedValue);
            PaymentTypeId       = Convert.ToInt32(ddlPaymentType.SelectedValue);
            ExpenseTypeId       = Convert.ToInt32(ddlExpenseType.SelectedValue);

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

            if (ExpenseTypeId == 1 || ExpenseTypeId == 9) // Duty Or Panelty
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(lblTotalTaxableValue.Text.Trim());

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
            else //if (InvoiceType == 1) // Final Invoice
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
            //else
            //{
            //    if (txtTotalInvoiceValue.Text.Trim() != "")
            //    {
            //        decInvoiceAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
            //        decTaxableAmount = decInvoiceAmount;
            //    }
            //}

            if (rblImmediatePayment.SelectedValue == "0")
            {
                IsImmediatePayment = false;
            }
            else
            {
                IsImmediatePayment = true;
            }

            strVendorName = txtVendorName.Text.Trim();
            strVendorCode = hdnVendorCode.Value.Trim();
            strVendorGSTNo = txtSupplierGSTIN.Text.Trim();
            strVendorPAN = txtVendorPANNo.Text.Trim();

            strInvoiceNo = txtInvoiceNo.Text.Trim();
            strInvoiceNo = strInvoiceNo.Replace(" ", "");

            strPaymentTerms = txtPaymentTerms.Text.Trim();

            int Billtype = 0;
            int HODID = 0;

            if (rblPDAccount.SelectedValue == "1")
            {
                Billtype = 11; // PD Account
            }
            else if (fldVendorBuySell.Visible == true)
            {
                Billtype = 3; // Credit Vendor
                //HODID = Convert.ToInt32(ddHOD.SelectedValue);
            }

            int InvoiceMode = Convert.ToInt32(rblInvoiceMode.SelectedValue);

            int Result = AccountExpense.UpdateInvoiceDetail(InvoiceId,JobId, strJobRefNO, ModuleID, ExpenseTypeId,
                isRIM, InvoiceType, PaymentTypeId, VendorGSTNType, strConsigneeGSTIN, strConsigneeName, strConsigneeCode, strConsigneePAN,
                IsInterest, InterestAmount, IsAdvanceReceived, AdvanceAmount, dtPaymentRequiredDate, dtPaymentDueDate, strVendorName, strVendorCode, strVendorGSTNo,
                strVendorPAN, strInvoiceNo, dtInvoiceDate, decInvoiceAmount, decTaxableAmount, decGSTAmount, InvoiceCurrencyId,
                decInvoiceCurrencyAmt, decInvoiceCurrencyExchangeRate, strPaymentTerms, IsImmediatePayment,
                Billtype, HODID, InvoiceMode, strRemark, LoggedInUser.glUserId);

            if (Result  == 0)
            {
                lblError.Text = "Invoice Detail Updated Successfully!";
                lblError.CssClass = "success";

                // Remove Invoice Item
                AccountExpense.DeleteInvoiceItemByInvoiceID(InvoiceId, LoggedInUser.glUserId);

                // Add Invoice Item
                AddInvoiceItem(InvoiceId);

                // Invoice Updated and send for Approval
                int StatusId = Convert.ToInt32(hdnStatusId.Value);
                int NewStatusid = 100; // Send For Mgmt Approval/HOD Approval

                if (StatusId == 122)
                {
                    // Send To Account Department
                    NewStatusid = 120;
                }

                if (fldVendorBuySell.Visible == true)
                {
                    // Send To Account Department - Credit Vendor 
                
                    AddJobProfit(InvoiceId);
                }

                int result = AccountExpense.AddInvoiceStatus(InvoiceId, NewStatusid, txtRemark.Text.Trim(), LoggedInUser.glUserId);
                
                // Upload Document
                string strDirPath = strJobRefNO.Replace("/", "");

                strDirPath = strDirPath.Replace("-", "");

                strInvoiceFilePath = "VendorInvoice//" + strDirPath + "//";

                if (fuDocumentInvoice.HasFile)
                {
                    string FileName1 = UploadDocument(fuDocumentInvoice, strInvoiceFilePath);

                    int FileOutput1 = AccountExpense.AddInvoiceDocument(InvoiceId, 1, strInvoiceFilePath, FileName1, LoggedInUser.glUserId);
                }
                if (fuDocumentEmailApproval.HasFile)
                {
                    string FileName2 = UploadDocument(fuDocumentEmailApproval, strInvoiceFilePath);

                    string UploadFileName2 = strInvoiceFilePath + FileName2;

                    int FileOutput2 = AccountExpense.AddInvoiceDocument(InvoiceId, 2, strInvoiceFilePath, FileName2, LoggedInUser.glUserId);
                }
                if (fuDocumentOther.HasFile)
                {
                    string FileName3 = UploadDocument(fuDocumentOther, strInvoiceFilePath);

                    int FileOutput3 = AccountExpense.AddInvoiceDocument(InvoiceId, 3, strInvoiceFilePath, FileName3, LoggedInUser.glUserId);
                }

                Session["Success"] = lblError.Text;

                Response.Redirect("AccountSuccess.aspx");

            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "System Error", "alert('System Error! Please try after sometime!');", true);
            }
            else if (Result == 2)
            {
                lblError.Text = "Vendor Invoice Details Already Submitted!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('Invoice Details Already Exists!');", true);
            }
            else if (Result == 3)
            {
                lblError.Text = "Invoice Details Not Found!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('Invoice Details Not Found!');", true);
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
        else if (hdnChargeName.Value.Trim() != "") //(ddInvoiceType.SelectedValue == "1") // Final Invoice - One Charge Item
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
                Decimal.TryParse(lblTotalGST.Text.Trim(), out decIGSTAmount);                             
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

            //if (ddInvoiceType.SelectedValue == "1") // Final Invoice
            {
                decimal decChargeTotal = 0m;

                decimal.TryParse(lblTotalValue.Text.Trim(), out decChargeTotal);

                decimal decInvoiceTotal = 0m;
                decimal decExchangeRate = 0m;

                decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decInvoiceTotal);
                decimal.TryParse(txtExchangeRate.Text.Trim(), out decExchangeRate);

                decInvoiceTotal = decInvoiceTotal * decExchangeRate;

                decimal decDiff = decChargeTotal - decInvoiceTotal;

                if (decChargeTotal == 0 || decInvoiceTotal == 0)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Amount Details";
                    lblError.CssClass = "errorMsg";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Amount Error", "alert('" + lblError.Text + "');", true);

                    IsValid = false;
                    return IsValid;

                }
                else if (decDiff > 2 || decDiff < -2)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Amount Details";
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

                if (dsGetJobDetail.Tables[0].Rows.Count == 0)
                {
                    lblError.Text = "Job Details Not Found! Job Cancelled or Removed from System.";
                    lblError.CssClass = "errorMsg";
                    hdnJobId.Value = "0";
                    Session["InvoiceId"] = null;

                    wzRequest.Enabled = false;
                    return;
                }
                else if (dsGetJobDetail.Tables.Count > 0)
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

        if (ddlExpenseType.SelectedValue == "1")
        {
            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = false;

            ddGSTINType.SelectedValue = "2";
            ddGSTINType.Enabled = false;

            rblMultiChargeCode.SelectedValue = "0";
            rblMultiChargeCode.Enabled = false;

           // lblInvoiceNo.Text = "Challan No";
           // lblInvoiceDate.Text = "Challan Date";
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

           // lblInvoiceNo.Text = "Challan No";
           // lblInvoiceDate.Text = "Challan Date";
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

          //  lblInvoiceNo.Text = "Invoice No";
          //  lblInvoiceDate.Text = "Invoice Date";
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
    private void SetVendorStatus()
    {
        txtSupplierGSTIN.Text = "";
        txtVendorName.Text = "";
        txtVendorPANNo.Text = "";

        if (hdnVendorCode.Value != "")
        {
            DataSet dsVendor = AccountExpense.GetFAVendorByCode(hdnVendorCode.Value);

            if (dsVendor.Tables.Count > 0)
            {
                if (dsVendor.Tables[0].Rows.Count > 0)
                {
                    txtSupplierGSTIN.Text = dsVendor.Tables[0].Rows[0]["gstin"].ToString();
                    txtVendorName.Text = dsVendor.Tables[0].Rows[0]["par_name"].ToString();
                    txtVendorPANNo.Text = dsVendor.Tables[0].Rows[0]["girno"].ToString();
                    txtPaymentTerms.Text = dsVendor.Tables[0].Rows[0]["crdays"].ToString();
                }
            }

        }

        int ProformaInvoiceId = 0;

        Int32.TryParse(hdnProformaInvoiceId.Value, out ProformaInvoiceId);

        if (ProformaInvoiceId == 0)
        {
            // Allow To Change Vendor detail except Proforma To Final Invoice
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
        }

        // Job Profit Detail For Credit Vendor

        if (hdnIsCreditVendor.Value == "1")
        {
            fldVendorBuySell.Visible = true;

            // Credit Vendor Job Buy/Sell Detail

            DataSet dsJobProfitDetail = AccountExpense.GetInvoiceJobProfit(Convert.ToInt32(Session["InvoiceID"]));

            if (dsJobProfitDetail.Tables[0].Rows.Count > 0)
            {
                txtVendorBuyValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorBuyValue"].ToString();
                txtVendorSellValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["VendorSellValue"].ToString();
                txtCustomerBuyValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["CustomerBuyValue"].ToString();
                txtCustomerSellValue.Text = dsJobProfitDetail.Tables[0].Rows[0]["CustomerSellValue"].ToString();
                txtJobProfitRemark.Text = dsJobProfitDetail.Tables[0].Rows[0]["Remark"].ToString();
            }

        }
        else
        {
            fldVendorBuySell.Visible = false;
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

        //decGSTAmount = (decTaxableAmount * decGSTRate) / 100;

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
        dr["ChargeRemark"] = txtChargeRemark.Text.Trim();

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
        // txtChargeCode.Text = "";
        // txtChargeName.Text = "";

        hdnChargeHSN.Value = "";
        txtTaxableValue.Text = "";
        txtChargeRemark.Text = "";
        ddGSTRate.SelectedIndex = -1;

    }
    protected void rblImmediatePayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        int PaymentTermsDays = 0;

        CalExtPayDueDate.Enabled = true;
        CalExtPayReqrdDate.Enabled = true;
        if (txtPaymentTerms.Text.Trim() != "")
        {
            Int32.TryParse(txtPaymentTerms.Text.Trim(), out PaymentTermsDays);
        }


        if (rblImmediatePayment.SelectedValue == "0")
        {   // Vendor Credit Days

            CalExtPayDueDate.Enabled = false;
            CalExtPayReqrdDate.Enabled = false;

            txtPaymentDueDate.Text = DateTime.Now.AddDays(PaymentTermsDays).ToString("dd/MM/yyyyy");
            txtPaymentRequiredDate.Text = DateTime.Now.AddDays(PaymentTermsDays).ToString("dd/MM/yyyyy");
        }
        else
        {
            // Immediate Payment
            CalExtPayDueDate.Enabled = true;
            CalExtPayReqrdDate.Enabled = true;

            txtPaymentDueDate.Text = DateTime.Now.AddDays(PaymentTermsDays).ToString("dd/MM/yyyyy");
        }
    }
    private void SetVendorSearchType()
    {
        fldVendorBuySell.Visible = false;

        if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9" || ddlExpenseType.SelectedValue == "100")
        {
            rblPDAccount.SelectedValue = "0";
            rblCreditVendor.SelectedValue = "0";
        }

        if (rblPDAccount.SelectedValue == "1")
        {
            AutoCompleteExtenderGST.ContextKey = "10.21";
            AutoCompleteVendor.ContextKey = "10.21";

            if (rblCreditVendor.SelectedValue == "1")
            {
                rblCreditVendor.SelectedValue = "0";
            }

        }
        else
        {
            AutoCompleteExtenderGST.ContextKey = "21";
            AutoCompleteVendor.ContextKey = "21";
        }

        if (rblCreditVendor.SelectedValue == "1")
        {
            fldVendorBuySell.Visible = true;
            // rblPDAccount.SelectedValue = "0";
        }
    }
}