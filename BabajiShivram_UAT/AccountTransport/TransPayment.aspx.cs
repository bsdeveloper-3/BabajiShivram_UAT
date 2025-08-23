using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QueryStringEncryption;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountTransport_TransPayment : System.Web.UI.Page
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
            Response.Redirect("PendingTransPayment.aspx");
        }

        if (!IsPostBack)
        {
            string strTransPayId = Session["TransPayId"].ToString();

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

        int RequestId = 0, PaymentId = 0;
        string strInstrumentNo = "", strDocPath = "", strRemark = "";

        Decimal decPaidAmount = 0, decNetPayable = 0;

        DateTime dtInstrumentDate = DateTime.MinValue;

        PaymentId = Convert.ToInt32(hdnPaymentId.Value);
        RequestId = Convert.ToInt32(Session["TransPayId"]);

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

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

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

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);

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

        PaymentId = Convert.ToInt32(hdnPaymentId.Value);

        int PayResult = DBOperations.AddTransPayPayment(RequestId, PaymentId, strInstrumentNo, dtInstrumentDate,
            decPaidAmount, strDocPath, strRemark, LoggedInUser.glUserId);

        if (PayResult == 0)
        {
            lblError.Text = "Payment Details Updated Successfully!";
            lblError.CssClass = "success";
            // Upload Document

            string strDirPath = "ABCD";//lblJobNumber.Text.Trim().Replace("/", "");

            strDirPath = strDirPath.Replace("-", "");

            string strInvoiceFilePath = "TransportInvoice//" + strDirPath + "//";

            //if (fileUploadPaymentDocument.HasFile)
            //{
            //   // string FileName10 = UploadDocument(fileUploadPaymentDocument, strInvoiceFilePath);

            //   // int FileOutput1 = AccountExpense.AddInvoiceDocument(InvoiceId, 10, strInvoiceFilePath, FileName10, LoggedInUser.glUserId);
            //}

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
        else if (PayResult == 4)
        {
            lblError.Text = "Payment Already Completed! Please Check Payment History.";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + lblError.Text + "');", true);
        }

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

        bool IsFundTransFromAPI = true;

        int result = DBOperations.AddTransPayFromBank(PayRequestId, IsFundTransFromAPI, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Payment Moved To Bank Transfer Tab!";
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