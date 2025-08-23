using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QueryStringEncryption;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BankAPI.Open;
using System.Security.Cryptography.X509Certificates;

public partial class AccountTransport_TransPaymentAPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int TransReqId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Transporter Payment";

        if (Session["TransPayId"] == null)
        {
            Response.Redirect("PendingTransPaymentAPI.aspx");
        }
        if (LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 3 || LoggedInUser.glUserId == 1023 || LoggedInUser.glUserId == 13478)
        {
        }
        else
        {
            Session["TransPayId"] = null;
            Response.Redirect("../Error.aspx");
        }
        if (!IsPostBack)
        {
            string strTransPayId = Session["TransPayId"].ToString();

            if (CheckCertificate() == false)
            {
                Session["TransPayId"] = null;
                Response.Redirect("../Error.aspx");
            }
            else
            {
                FundRequestDetail(Convert.ToInt32(strTransPayId));
                TruckRequestDetail(Convert.ToInt32(TransReqId));
                GetInvoicePayment();

                if (GridViewVehicle.Rows.Count == 0)
                {
                    lblError.Text = "Vehicle Rate Details Not Avalable!!";
                    lblError.CssClass = "errorMsg";
                    btnSavePayment.Visible = false;
                }
            }            
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
            txtPaymentRemark.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
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

        }
    }
    private void FundRequestDetail(int PayRequestId)
    {
        DataView dvFundDetail = DBOperations.GetTransportFundRequest(PayRequestId);

        if (dvFundDetail.Table.Rows.Count > 0)
        {
            int StatusId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["StatusId"]);

            GetPendingInvoicePayment(StatusId);

            TransReqId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["TransReqId"]);
            lblInvoiceNo.Text = dvFundDetail.Table.Rows[0]["InvoiceNo"].ToString();
            lblInvoiceType.Text = dvFundDetail.Table.Rows[0]["InvoiceTypeName"].ToString();
            lblVendorName.Text = dvFundDetail.Table.Rows[0]["PaidTo"].ToString();
            lblPaymentDueDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["PaymentRequiredDate"]).ToString("dd/MM/yyyy");
            lblPatymentRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblBillingPartyName.Text = dvFundDetail.Table.Rows[0]["BillToParty"].ToString();
            lblTotalInvoiceValue.Text = dvFundDetail.Table.Rows[0]["Amount"].ToString();
            lblDeduction.Text = dvFundDetail.Table.Rows[0]["OtherDeduction"].ToString();
            lblAdvance.Text = dvFundDetail.Table.Rows[0]["AdvanceAmt"].ToString();
            lblTDSAmount.Text = dvFundDetail.Table.Rows[0]["TDSTotalAmount"].ToString();

            lblRequestRemark.Text = dvFundDetail.Table.Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dvFundDetail.Table.Rows[0]["RequestBy"].ToString();

            if (dvFundDetail.Table.Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dvFundDetail.Table.Rows[0]["NetPayable"] != DBNull.Value)
            {
                lblNetPayable.Text = dvFundDetail.Table.Rows[0]["NetPayable"].ToString();
            }

            // TDS Detail

            if (Convert.ToBoolean(dvFundDetail.Table.Rows[0]["TDSApplicable"]) == true)
            {
                fldTDSItem.Visible = true;
                GridViewTDS.Visible = true;
                lblTDSApplicable.Text = "Yes";
                lblTDSRate.Text = dvFundDetail.Table.Rows[0]["TDSRate"].ToString();

                if (dvFundDetail.Table.Rows[0]["TDSRate"] != DBNull.Value)
                {
                    if (dvFundDetail.Table.Rows[0]["TDSRateType"] != DBNull.Value)
                    {
                        if (dvFundDetail.Table.Rows[0]["TDSRateType"].ToString() == "1")
                        {
                            lblTDSRateType.Text = "Standard";
                        }
                        else if (dvFundDetail.Table.Rows[0]["TDSRateType"].ToString() == "2")
                        {
                            lblTDSRateType.Text = "Concessional";

                        }
                    }
                    else
                    {
                        lblTDSRateType.Text = "Standard";
                    }
                }
                if (dvFundDetail.Table.Rows[0]["TDSTotalAmount"] != DBNull.Value)
                {
                    lblTotalTDS.Text = dvFundDetail.Table.Rows[0]["TDSTotalAmount"].ToString();
                }
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
                    lblBabajiAccountNo.Text = dsPaymentDetail.Tables[0].Rows[0]["AccountNo"].ToString();
                }
            }

            // Vendor Account Detail

            if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            {
                int VendorBankAccountId = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]);

                //   ddVendorBank.SelectedValue = VendorBankAccountId.ToString();

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
    private void GetPendingInvoicePayment(int StatusId)
    {
        // StatusId - 151 - Partial Payment
        int RequestId = Convert.ToInt32(Session["TransPayId"]);

        DataSet dsPaymentDetail = DBOperations.GetTransPayPendingPayment(RequestId);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
            hdnPaymentId.Value = dsPaymentDetail.Tables[0].Rows[0]["lid"].ToString();
            txtPayAmount.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();

            // lblNetPayable.Text = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            // hdnNetPayableAmount.Value = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();

            ddlPaymentType.SelectedValue = dsPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

            //  Vendor Bank Account

            if (dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"] != DBNull.Value)
            {
                //hdnVendorBankAccountId.Value = dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"].ToString();

                //GetVendorBankDetail(Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountId"]));
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

        }

    }
    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        if (GridViewVehicle.Rows.Count == 0)
        {
            lblError.Text = "Vehicle Rate Details Not Avalable!!";
            lblError.CssClass = "errorMsg";
            return;
        }

        bool IsValidTransaction = true;

        int RequestId = 0, PaymentId = 0;
        string strInstrumentNo = "", strDocPath = "", strRemark = "";
        string strPaymentRefNo = "";
        Decimal decPaidAmount = 0;

        DateTime dtInstrumentDate = DateTime.Now;

        if (Session["TransPayId"] == null)
        {
            IsValidTransaction = false;

            lblError.Text = "Session Expired! Please Login Again!";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Login Error", "alert('" + lblError.Text + "');", true);

        }
        else
        {
            Int32.TryParse(hdnPaymentId.Value, out PaymentId);
            Int32.TryParse(Session["TransPayId"].ToString(), out RequestId);

            Decimal.TryParse(txtPayAmount.Text.Trim(), out decPaidAmount);

            DataSet dsPaymentDetail = DBOperations.GetTransPayPendingPayment(RequestId);

            if (dsPaymentDetail.Tables[0].Rows.Count > 0) // Audit L2 Completed OR Payment ON Hold
            {
                Decimal CheckPayableAmount = 0, NetCheckPayableAmount = 0;

                NetCheckPayableAmount = Convert.ToDecimal(lblNetPayable.Text.ToString());

                string CheckVendorAccountNo = dsPaymentDetail.Tables[0].Rows[0]["VendorBankAccountNo"].ToString().Trim();
                int CurrentStatus = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["StatusId"]);
                Decimal.TryParse(dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString(), out CheckPayableAmount);

                if(NetCheckPayableAmount < CheckPayableAmount)
                {
                    // Net Payable Amount not Matched with Paid Amount
                    IsValidTransaction = false;
                    lblError.Text = "Please Check Check Net Payable Amount!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
                }
                else if (CurrentStatus == 145 || CurrentStatus == 149)
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
                    if (lblVendorBankAccountNo.Text.Trim() == "" || lblVendorBankAccountNo.Text.Trim() != CheckVendorAccountNo)
                    {
                        // Invalid Account NO
                        IsValidTransaction = false;
                        lblError.Text = "Please Check Transporter Bank Account No!";
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

                if (RequestId == 0 || PaymentId == 0)
                {
                    IsValidTransaction = false;

                    lblError.Text = "Session Expired! Please Login Again For Payment!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Login Error", "alert('" + lblError.Text + "');", true);

                    return;
                }

                if (lblVendorBankAccountIFSC.Text.Trim() == "")
                {
                    IsValidTransaction = false;

                    lblError.Text = "Please Enter Transporter Bank IFC Code!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }
                if (lblVendorBankAccountNo.Text.Trim() == "")
                {
                    IsValidTransaction = false;

                    lblError.Text = "Please Enter Bank Account No!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }
                if (lblVendorBankAccountName.Text.Trim() == "")
                {
                    IsValidTransaction = false;

                    lblError.Text = "Please Enter Beneficiary Name!";
                    lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

                    return;
                }

                // Check Payment Transaction Status and Get Vendor Bank Info;

                int ActiveTransaction = DBOperations.CheckTransActiveTransaction(RequestId, PaymentId);

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
                    strPaymentRefNo = LiveTransportFund(PaymentId);

                    if (strPaymentRefNo != "")
                    {                        
                        int PayResult = DBOperations.AddTransPayPayment(RequestId, PaymentId, "", dtInstrumentDate,
                            decPaidAmount, strDocPath, strRemark, LoggedInUser.glUserId);

                        if (PayResult == 0)
                        {
                            //   bool IsMailSuccess = SendPaymentConfirmationMail(InvoiceId, strPaymentRefNo, dtInstrumentDate.ToShortDateString(), 
                            //       decPaidAmount.ToString());

                            lblError.Text = "Payment Initiated Successfully! Transaction No -" + strPaymentRefNo + " <BR> UTR No will be sent on Email";
                            lblError.CssClass = "success";

                            Session["Success"] = lblError.Text;

                            Response.Redirect("TransportSuccess.aspx");
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

                Session["TransPayId"] = null;
                Session["InvoiceId"] = null;
                btnSavePayment.Visible = false;
            }
        }//END_ELSE
    }
    protected void btnHold_Click(object sender, EventArgs e)
    {
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);
        string strRemark = txtRejectRemark.Text.Trim();

        Int32 StatusId = (Int32)EnumInvoiceStatus.PaymentOnHold;

        int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Transporter Payment On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = "Transporter Payment On Hold!";

            Response.Redirect("TransportSuccess.aspx");
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
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);
        string strRemark = txtRejectRemark.Text.Trim();

        Int32 StatusId = 148; // (Int32)EnumInvoiceStatus.;

        int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Transporter Payment Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = lblError.Text;

            Response.Redirect("TransportSuccess.aspx");
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

    protected void btnPayFrom_Click(object sender, EventArgs e)
    {
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);

        bool IsFundTransFromAPI = false;

        int result = DBOperations.AddTransPayFromBank(PayRequestId, IsFundTransFromAPI, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Payment Moved To Manual Payment Tab!";
            lblError.CssClass = "success";

            Session["Success"] = lblError.Text;

            Response.Redirect("TransportSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Payment Details not Found!";
            lblError.CssClass = "errorMsg";
            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

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

    #region Fund Transfer
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

            if (Session["TransPayId"] != null)
            {
                strAInvoiceId = Session["TransPayId"].ToString();
            }
            string strMessage = strAInvoiceId + "-Transport Payment API unauthorised Access- " + LoggedInUser.glUserName + " From - " + strIPAddress;
            ErrorLog.SendMail(strMessage, ex);
            ErrorLog.LogToDatabase(Convert.ToInt32(strAInvoiceId), "TransportPaymentAPI", "API unauthorised Access", strMessage, ex, "", LoggedInUser.glUserId);
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
    private string LiveTransportFund(int PaymentId)
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

        DataSet dsPaymentRequest = DBOperations.GetTransPaymentDetail(PaymentId);

        if (dsPaymentRequest.Tables.Count > 0)
        {
            strPaymentType = ddlPaymentType.SelectedItem.Text.Trim();
            strIFSCCode = lblVendorBankAccountIFSC.Text.Trim();

            if (strIFSCCode.ToLower().Contains("yesb"))
            {
                strPaymentType = "FT";
            }
            else if (strPaymentType.ToUpper() == "RTGS" && Convert.ToDecimal(txtPayAmount.Text.Trim()) < 200000)
            {
                strPaymentType = "NEFT";
            }

            int RequestId = Convert.ToInt32(dsPaymentRequest.Tables[0].Rows[0]["RequestId"]);

            Decimal.TryParse(dsPaymentRequest.Tables[0].Rows[0]["PaidAmount"].ToString(), out decAmount);

            if (dsPaymentRequest.Tables[0].Rows[0]["RequestRefNo"] != DBNull.Value)
            {
                strReqestRefNo = dsPaymentRequest.Tables[0].Rows[0]["RequestRefNo"].ToString();
            }

            if (dsPaymentRequest.Tables[0].Rows[0]["IsPaid"] != DBNull.Value)
            {
                isPaid = Convert.ToBoolean(dsPaymentRequest.Tables[0].Rows[0]["IsPaid"]);
            }
            if (dsPaymentRequest.Tables[0].Rows[0]["StatusId"] != DBNull.Value)
            {
                InvoiceStatus = Convert.ToInt32(dsPaymentRequest.Tables[0].Rows[0]["StatusId"]);
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
        if (InvoiceStatus < 145)
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

            objData.ConsentId = "3879279";

            // Debit
            objInitiation.InstructionIdentification = strReqestRefNo;
            objInitiation.EndToEndIdentification = "";

            objInstructedAmount.Amount = txtPayAmount.Text.Trim();
            objInstructedAmount.Currency = "INR";

            objDebtorAccount.Identification = "007881300002453";
            objDebtorAccount.SecondaryIdentification = "3879279";

            // Credit

            objCreditorAccount.SchemeName = strIFSCCode;
            objCreditorAccount.Identification = lblVendorBankAccountNo.Text.Trim();

            string strAccountName = lblVendorBankAccountName.Text.Trim().Replace("(", "");
            strAccountName = strAccountName.Replace(")", "");
            strAccountName = strAccountName.Replace("&", "");
            strAccountName = strAccountName.Replace("+", " ");

            objCreditorAccount.Name = strAccountName.Trim();
            objContactInformation.EmailAddress = "somnath.kumbhar@babajishivram.com";
            objContactInformation.MobileNumber = "9224683493";

            // Payment Reference

            string strTransJobNo = lblJobNo.Text.Replace("/", "").Trim();
            strTransJobNo = strTransJobNo.Replace("-", "").Trim();

            string strTransInvoiceNo = lblInvoiceNo.Text.Replace("/", "").Trim();
            strTransInvoiceNo = strTransInvoiceNo.Replace("-", "").Trim();
            if (strTransInvoiceNo.Length <= 1)
            {
                // Minimun 2 Char Required

                strTransInvoiceNo = strTransInvoiceNo + strTransJobNo.ToString();
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

            int APITransactionResult = AccountExpense.AddTransBankPaymentAPIRequest(2, PaymentId, objInitiation.InstructionIdentification, objInstructedAmount.Amount, objInstructedAmount.Currency,
                objCreditorAccount.SchemeName, objCreditorAccount.Identification, objCreditorAccount.Name, LoggedInUser.glUserId);

            Session["PaymentTransTransactionId"] = APITransactionResult;

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

                    int ErrorUpdResult = AccountExpense.AddTransBankPaymentAPIError(APITransactionResult, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, LoggedInUser.glUserId);

                    int AccountUpdResult = AccountExpense.UpdateTransBankPaymentAPIResponse(strReqestRefNo, "", "", objError.ActionDescription, dtResponseDate, IsSuccess, StatusId, LoggedInUser.glUserId);
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

                    int AccountUpdResult = AccountExpense.UpdateTransBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, objSuccess.Data.TransactionIdentification,
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