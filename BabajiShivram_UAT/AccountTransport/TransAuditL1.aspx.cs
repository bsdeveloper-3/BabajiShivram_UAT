using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AccountTransport_TransAuditL1 : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int TransReqId = 0;
    int ConsolidateId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Transport Audit L1";

        if (Session["TransPayId"] == null)
        {
            Response.Redirect("PendingTransL1.aspx");
        }

        if (!IsPostBack)
        {
            btnBack.PostBackUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            string strTransPayId = Session["TransPayId"].ToString();
            
            GetDetail();

        }
    }
    private void GetDetail()
    {
        FundRequestDetail();
        TruckRequestDetail(Convert.ToInt32(TransReqId));
        GetInvoicePayment();

        if (ConsolidateId > 0)
        {
            Session["TRSConsolidateId"] = ConsolidateId.ToString();
            // Bind Consolidate Job Details
        }
        else
        {
            fsConsolidateJobs.Visible = false;
            Session["TRSConsolidateId"] = null;
        }
    }
    private void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequestDetail(TranRequestId);

        if (dvDetail.Table.Rows.Count > 0)
        {
            lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            hdnJobId.Value = dvDetail.Table.Rows[0]["JobId"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");

            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
            lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();

            lblPickAdd.Text = dvDetail.Table.Rows[0]["PickUpAddress"].ToString();
            lblDropAdd.Text = dvDetail.Table.Rows[0]["DropAddress"].ToString();
            lblpickPincode.Text = dvDetail.Table.Rows[0]["PickupPincode"].ToString();             //added new pickup and drop pincode, city and state for Transport Management Approval
            lblpickState.Text = dvDetail.Table.Rows[0]["PickupState"].ToString();
            lblpickCity.Text = dvDetail.Table.Rows[0]["PickupCity"].ToString();
            lblDropPincode.Text = dvDetail.Table.Rows[0]["DropPincode"].ToString();
            lblDropState.Text = dvDetail.Table.Rows[0]["DropState"].ToString();
            lblDropCity.Text = dvDetail.Table.Rows[0]["DropCity"].ToString();

        }
    }
    private void FundRequestDetail()
    {
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);

        DataView dvFundDetail = DBOperations.GetTransportFundRequest(PayRequestId);

        if (dvFundDetail.Table.Rows.Count > 0)
        {
            TransReqId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["TransReqId"]);
            ConsolidateId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["ConsolidateID"]);
            lblInvoiceNo.Text = dvFundDetail.Table.Rows[0]["InvoiceNo"].ToString();
            lblInvoiceType.Text = dvFundDetail.Table.Rows[0]["InvoiceTypeName"].ToString();
            lblVendorName.Text = dvFundDetail.Table.Rows[0]["PaidTo"].ToString();
            lblPaymentDueDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["PaymentRequiredDate"]).ToString("dd/MM/yyyy");
            lblPatymentRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblBillingPartyName.Text = dvFundDetail.Table.Rows[0]["BillToParty"].ToString();
            lblTotalInvoiceValue.Text = dvFundDetail.Table.Rows[0]["Amount"].ToString();
            lblDeduction.Text = dvFundDetail.Table.Rows[0]["OtherDeduction"].ToString();
            lblAdvance.Text = dvFundDetail.Table.Rows[0]["AdvanceAmt"].ToString();
            lblAdvanceTDS.Text = dvFundDetail.Table.Rows[0]["AdvanceTDS"].ToString();
            lblTDSAmount.Text = dvFundDetail.Table.Rows[0]["TDSTotalAmount"].ToString();

            lblRequestRemark.Text = dvFundDetail.Table.Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dvFundDetail.Table.Rows[0]["RequestBy"].ToString();
            lblRequestDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["dtDate"]).ToString("dd/MM/yyyy");

            if (dvFundDetail.Table.Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }
            if (dvFundDetail.Table.Rows[0]["NetPayable"] != DBNull.Value)
            {
                hdnNetPayableAmount.Value = dvFundDetail.Table.Rows[0]["NetPayable"].ToString();
                lblNetPayable.Text = dvFundDetail.Table.Rows[0]["NetPayable"].ToString();
            }

            if (dvFundDetail.Table.Rows[0]["VendorId"] != DBNull.Value)
            {
                int VendorId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["VendorId"]);

                //ListItem lstItemName = new ListItem("HDFC","17");

                //ddVendorBank.Items.Insert(1, lstItemName);

                DBOperations.FillTransporterBank(ddVendorBank, VendorId);
            }
            if (dvFundDetail.Table.Rows[0]["PaymentTypeId"] != DBNull.Value)
            {
                ddlPaymentType.SelectedValue = dvFundDetail.Table.Rows[0]["PaymentTypeId"].ToString();

                if (dvFundDetail.Table.Rows[0]["PaymentTypeId"].ToString() == "1") // Cash
                {
                    AccountExpense.FillBankBookByType(ddBabajiBankAccount, 2);
                }
                else // Bank
                {
                    // File Bank Name MS

                    AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name
                }
            }
            if (Convert.ToBoolean(dvFundDetail.Table.Rows[0]["TDSApplicable"]) == true)
            {
                chkTDSYes.Checked = true;
                //chkTDSYes.Enabled = false;

                if (dvFundDetail.Table.Rows[0]["TDSRate"] != DBNull.Value)
                {
                    if (dvFundDetail.Table.Rows[0]["TDSRateType"] != DBNull.Value)
                    {
                        if (dvFundDetail.Table.Rows[0]["TDSRateType"].ToString() == "1")
                        {
                            ddTDSRateType.SelectedValue = "1";
                            ddTDSRate.SelectedValue = dvFundDetail.Table.Rows[0]["TDSRate"].ToString();

                            ddTDSRate.Visible = true;
                            rfvddTDSRate.Enabled = true;

                            txtTDSRate.Visible = false;
                            rfvtxtTdsRate.Enabled = false;
                        }
                        else if (dvFundDetail.Table.Rows[0]["TDSRateType"].ToString() == "2")
                        {
                            ddTDSRateType.SelectedValue = "2";
                            txtTDSRate.Text = dvFundDetail.Table.Rows[0]["TDSRate"].ToString();

                            txtTDSRate.Visible = true;
                            rfvtxtTdsRate.Enabled = true;

                            ddTDSRate.Visible = false;
                            rfvddTDSRate.Enabled = false;
                        }
                    }
                    else
                    {
                        ddTDSRate.SelectedValue = dvFundDetail.Table.Rows[0]["TDSRate"].ToString();
                    }
                }
                if (dvFundDetail.Table.Rows[0]["TDSLedgerCodeId"] != DBNull.Value)
                {
                    ddTDSLedgerCode.SelectedValue = dvFundDetail.Table.Rows[0]["TDSLedgerCodeId"].ToString();
                }
                if (dvFundDetail.Table.Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    lblTotalTDS.Text = dvFundDetail.Table.Rows[0]["TDSTotalAmount"].ToString();
                }

                GridViewTDS.Visible = true;
            }
            else
            {
                chkTDSYes.Checked = false;
                chkTDSYes.Enabled = true;
                lblTotalTDS.Text = "0";
                txtTDSRate.Text = "";
                ddTDSRate.SelectedValue = "0";
                ddTDSLedgerCode.SelectedIndex = -1;
                GridViewTDS.Visible = false;
            }
        }
    }
    private void GetInvoicePayment()
    {
        int InvoiceID = Convert.ToInt32(Session["TransPayId"]);

        DataSet dsPaymentDetail = DBOperations.GetTransPayPayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            txtPayAmount.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

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

            // Vendor Account Detail

            if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            {
                int VendorBankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]);

                ddVendorBank.SelectedValue = VendorBankAccountId.ToString();

                DataSet dsVendorBankDetal = DBOperations.GetTransporterBankDetail(VendorBankAccountId);

                if (dsVendorBankDetal.Tables.Count > 0)
                {
                    if (dsVendorBankDetal.Tables[0].Rows.Count > 0)
                    {
                        if (dsVendorBankDetal.Tables[0].Rows[0]["AccountName"] != DBNull.Value)
                        {
                            lblVendorBankAccountName.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountName"].ToString();
                        }
                        else
                        {
                            lblVendorBankAccountName.Text = dsVendorBankDetal.Tables[0].Rows[0]["TransporterName"].ToString();
                        }

                        lblVendorBankName.Text = dsVendorBankDetal.Tables[0].Rows[0]["BankName"].ToString();
                        lblVendorBankAccountNo.Text = dsVendorBankDetal.Tables[0].Rows[0]["AccountNo"].ToString();
                        lblVendorBankAccountIFSC.Text = dsVendorBankDetal.Tables[0].Rows[0]["IFSCCode"].ToString();
                    }
                }
            }
        }
    }
    protected void ddBabajiBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int BabajiBankID = Convert.ToInt32(ddBabajiBankName.SelectedValue);

        AccountExpense.FillBankAccountByBankId(ddBabajiBankAccount, BabajiBankID);
    }
    protected void ddBabajiBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        // rblFundTransferFromLiveTracking.Enabled = false;
        // rblFundTransferFromLiveTracking.SelectedValue = "0";

        int PaymentType = 0; // Cash/ Bank
        int BankId = 0; // Yes Bank - 47 
        int BankAccountID = 0; // Yes Bank Nav Bharat - 163, FA Code - YBL000

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
            if (BankId == 47 && BankAccountID == 163) // Yes Bank Mumbai Account
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
    protected void ddVendorBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Get Vendor Bank Detail

        int VendorBankId = Convert.ToInt32(ddVendorBank.SelectedValue);

        DataSet dsVendorBank = DBOperations.GetTransporterBankDetail(VendorBankId);

        if (dsVendorBank.Tables.Count > 0)
        {
            if (dsVendorBank.Tables[0].Rows[0]["AccountName"] != DBNull.Value)
            {
                lblVendorBankAccountName.Text = dsVendorBank.Tables[0].Rows[0]["AccountName"].ToString();
            }
            else
            {
                lblVendorBankAccountName.Text = dsVendorBank.Tables[0].Rows[0]["TransporterName"].ToString();
            }
            
            lblVendorBankName.Text = dsVendorBank.Tables[0].Rows[0]["BankName"].ToString();
            lblVendorBankAccountNo.Text = dsVendorBank.Tables[0].Rows[0]["AccountNo"].ToString();
            lblVendorBankAccountIFSC.Text = dsVendorBank.Tables[0].Rows[0]["IFSCCode"].ToString();
        }

    }
    protected void btnPostSubmit_Click(object sender, EventArgs e)
    {
        int PaymentResult = 0;
        int VendorBankId = 0;
        Decimal decNetPayable = 0m;

        if (lblNetPayable.Text.Trim() != "")
        {
            decNetPayable = Convert.ToDecimal(lblNetPayable.Text.Trim());
        }
                
        Decimal decGSTAmount = 0m;
        
        VendorBankId = Convert.ToInt32(ddVendorBank.SelectedValue);

        if (VendorBankId == 0)
        {
            if (ddlPaymentType.SelectedValue == "4" || ddlPaymentType.SelectedValue == "6")
            {
                lblError.Text = "Please Select Transporter Bank Account for RTGS/NEFT Payment!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                return;
            }
        }

        if (txtAuditRemark.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Audit Narration!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('" + lblError.Text + "');", true);
            return;
        }
        
        // Update Payment Detail

        PaymentResult = UpdatePaymentRequest();
        
        if (PaymentResult == 0)
        {
            // Audit Completed and Submit for Financial Approval

            int RequestId = Convert.ToInt32(Session["TransPayId"]);

            Int32 StatusId = (Int32)EnumInvoiceStatus.L1Approved;
                        
            int result2 = DBOperations.AddTransPayStatus(RequestId, StatusId, txtAuditRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result2 == 0)
            {
                lblError.Text = "Payment Request Sent For Finance Approval!";
                lblError.CssClass = "success";

                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else if (result2 == 2)
            {
                lblError.Text = "Audit L1 Aready Completed!";
                lblError.CssClass = "success";

                Session["Success"] = "Audit L1 Aready Completed!";

                Response.Redirect("TransportSuccess.aspx");
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        // Audit L1 Rejected
        if (txtAuditRemark.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Reject Remark!";
            lblError.CssClass = "errorMsg";
        }
        else
        {

            int RequestId = Convert.ToInt32(Session["TransPayId"]);

            Int32 StatusId = (Int32)EnumInvoiceStatus.L1Reject;

            int result2 = DBOperations.AddTransPayStatus(RequestId, StatusId, txtAuditRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result2 == 0)
            {
                lblError.Text = "Payment Request Rejected!";
                lblError.CssClass = "success";

                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else if (result2 == 2)
            {
                lblError.Text = "Audit L1 Aready Rejected!";
                lblError.CssClass = "success";
                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void btnHold_Click(object sender, EventArgs e)
    {
        // Audit On Hold
        if (txtAuditRemark.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Hold Remark!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int RequestId = Convert.ToInt32(Session["TransPayId"]);

            Int32 StatusId = (Int32)EnumInvoiceStatus.L1OnHold;

            int result2 = DBOperations.AddTransPayStatus(RequestId, StatusId, txtAuditRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result2 == 0)
            {
                lblError.Text = "Payment Request On Hold!";
                lblError.CssClass = "success";

                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else if (result2 == 2)
            {
                lblError.Text = "Audit L1 Aready On Hold!";
                lblError.CssClass = "success";
                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPayment.SelectedValue == "1")
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
    private int UpdatePaymentRequest()
    {
        int SuccessId = 0;
        int RequestId = 0, PaymentTypeId = 0, BabajiBankId = 0, BankAccountId = 0;
        
        Boolean IsFullPayment = true;

        Boolean IsFundTransFromAPI = false;

        if (rblFundTransferFromLiveTracking.SelectedValue == "1")
        {
            IsFundTransFromAPI = true;
        }

        int VendorBankAccountId = 0;

        decimal decNetPayable = 0m;
        Decimal decPaidAmount = 0m;

        RequestId = Convert.ToInt32(Session["TransPayId"]);

        Decimal.TryParse(lblNetPayable.Text.Trim(), out decNetPayable);
        Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);

        PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (IsFundTransFromAPI == true)
        {
            // Check Payment Type it should be only Neft Or RTGS

            if (PaymentTypeId != 4 && PaymentTypeId != 6)
            {
                lblError.Text = "Please Select Payment Type as NEFT or RTGS for !";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                SuccessId = 2;

                return SuccessId;
            }
        }

        //if (PaymentTypeId == 4)
        //{
        //    PaymentTypeId = 6;
        //}

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
            lblError.Text = "Please Select Bank Account!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

            SuccessId = 2;
        }
                
        if (decNetPayable == decPaidAmount)
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
            //if (IsFundTransFromAPI == true)
            //{
            //    if (decPaidAmount <= 200000)
            //    {
            //        PaymentTypeId = 6;// NEFT
            //    }
            //    else
            //    {
            //        PaymentTypeId = 4;// RTGS
            //    }
            //}

            int PayResult = DBOperations.AddTransPayPaymentRequest(RequestId, IsFullPayment, PaymentTypeId, BabajiBankId, 
                BankAccountId, IsFundTransFromAPI, VendorBankAccountId, decPaidAmount, LoggedInUser.glUserId);

            if (PayResult > 0)
            {
                lblError.Text = "Payment Detail Added Successfully!";
                lblError.CssClass = "success";

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
    
    #region TDS
    protected void btnAddTDS_Click(object sender, EventArgs e)
    {
        // Reset Net Payable

        lblNetPayable.Text = "0";
        txtPayAmount.Text = "0";

        decimal decTDSRate = 0m;

        int RequestId = Convert.ToInt32(Session["TransPayId"]);

        bool tdsApplicable = false;

        string strTDSLedgerCode = "";

        strTDSLedgerCode = ddTDSLedgerCode.SelectedValue;

        if (ddTDSLedgerCode.SelectedIndex == 0)
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

                int Result = DBOperations.AddTransPayTDS(RequestId, tdsApplicable, strTDSLedgerCode, TDSRateType, decTDSRate, LoggedInUser.glUserId);


                if (Result == 0)
                {
                    lblError.Text = "TDS Details Updated!.";
                    lblError.CssClass = "success";
                    GridViewTDS.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);

                    GridViewTDS.Visible = true;

                    GridViewTDS.DataBind();

                    FundRequestDetail();
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

            int Result = DBOperations.AddTransPayTDS(RequestId, tdsApplicable, "", 0, 0, LoggedInUser.glUserId);

            lblError.Text = "TDS Details Removed!.";
            lblError.CssClass = "success";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);
            FundRequestDetail();

            GridViewTDS.Visible = false;

            GridViewTDS.DataBind();
        }
    }
    protected void chkTDSYes_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTDSYes.Checked)
        {
            GridViewTDS.Visible = true;
        }
        else
        {
            GridViewTDS.Visible = false;
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
    protected void lnkRemoveTDS_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        int Result = DBOperations.DeleteTransPayTDS(ChargeId, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblError.Text = "TDS Details Removed!.";
            lblError.CssClass = "success";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Success", "alert('" + lblError.Text + "');", true);

            GetDetail();

            GridViewTDS.DataBind();
            
        }
        else
        {
            lblError.Text = "TDS Delete Error!!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "TDS Error", "alert('" + lblError.Text + "');", true);
        }
    }
    #endregion

    #region Edit Charges
    protected void lnkEditCharge_Click(object sender, EventArgs e)
    {
        lblChargeError.Text = "";
        int TransPayId = 0;
        Int32.TryParse(Session["TransPayId"].ToString(), out TransPayId);

        if (TransPayId == 0)
        {
            Response.Redirect("PendingTransL1.aspx");
        }

        txtChargeRemark.Text = "";
        lblPopupHeader.Text = "Update Deduction Detail";
        btnAddCharges.Text = "Update Deduction";
        btnAddCharges.Visible = true;

        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        DataSet dsItem = DBOperations.GetTransPayPaymentItem(TransPayId,ChargeId);

        if (dsItem.Tables[0].Rows.Count > 0)
        {
            decimal decTDSAmount = 0m;
            
            hdnItemId.Value = dsItem.Tables[0].Rows[0]["lid"].ToString();
            lblEditChargeCode.Text = dsItem.Tables[0].Rows[0]["ChargeCode"].ToString();
            lblEditChargeName.Text = dsItem.Tables[0].Rows[0]["ChargeName"].ToString();

            lblEditAmount.Text = dsItem.Tables[0].Rows[0]["Amount"].ToString();
            lblEditTDS.Text     = dsItem.Tables[0].Rows[0]["TdsAmount"].ToString();
            txtEditOtherDeduction.Text = dsItem.Tables[0].Rows[0]["OtherDeduction"].ToString();
            txtChargeRemark.Text = dsItem.Tables[0].Rows[0]["Remark"].ToString();

            Decimal.TryParse(lblEditTDS.Text.Trim(), out decTDSAmount);
            if (decTDSAmount > 0)
            {
                lblChargeError.Text = "TDS already calculated! Please First Remove TDS detail.";
                lblChargeError.CssClass = "errorMsg";
                btnAddCharges.Visible = false;
            }

            ModalPopupExtender1.Show();
        }
    }
    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        lblChargeError.Text = "";
        int TransPayId = 0;
        Int32.TryParse(Session["TransPayId"].ToString(), out TransPayId);

        int ItemId = Convert.ToInt32(hdnItemId.Value);

        if (TransPayId == 0)
        {
            Response.Redirect("PendingTransL1.aspx");
        }

        if (TransPayId > 0)
        {
            string strRemark = "";
            decimal decTDSAmount = 0m, decOtherDeduction = 0m;

            Decimal.TryParse(lblEditTDS.Text.Trim(), out decTDSAmount);
            Decimal.TryParse(txtEditOtherDeduction.Text.Trim(), out decOtherDeduction);

            strRemark = txtChargeRemark.Text.Trim();

            if(decTDSAmount > 0)
            {
                lblChargeError.Text = "TDS already calculated! Please First Remove TDS detail.";
                lblChargeError.CssClass = "errorMsg";

                ModalPopupExtender1.Show();
            }

            else if (ItemId >= 0) // Update Charge
            {
                int resultUpd = DBOperations.AddTransPayPaymentDeduction(TransPayId,ItemId, decOtherDeduction, strRemark, LoggedInUser.glUserId);

                if (resultUpd == 0)
                {
                    lblError.Text = "Deduction Detail Updated Successfully!";
                    lblError.CssClass = "success";
                    gvCharges.DataBind();
                    ResetCharges();

                    GetDetail();
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
    protected void btnClosePopup_Click(object sender, EventArgs e)
    {
        ResetCharges();
        ModalPopupExtender1.Hide();
    }
    private void ResetCharges()
    {
        lblEditChargeCode.Text = "";
        lblEditChargeName.Text = "";
        lblEditAmount.Text = "";
        txtChargeRemark.Text = "";
        txtEditOtherDeduction.Text = "";
        hdnItemId.Value = "0";
    }

    #endregion

    #region Document Download
    protected void gvDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        int TransPayId = Convert.ToInt32(Session["TransPayId"].ToString());

        if (e.CommandName.ToLower() == "download")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = DBOperations.GetTransPayDocument(TransPayId, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath;

                //strDocpath = strFilePath + strFileName;
                DownloadDocument(strDocpath);
            }


        }
        else if (e.CommandName.ToLower() == "view")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = DBOperations.GetTransPayDocument(TransPayId, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath;

                //strDocpath = strFilePath + strFileName;
                ViewDocument(strDocpath);
            }
        }
    }
    private void DownloadDocument(string DocumentPath)
    {
        lblError.Text = "";
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
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
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