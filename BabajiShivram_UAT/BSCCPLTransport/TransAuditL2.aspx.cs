using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class BSCCPLTransport_TransAuditL2 : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int TransReqId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Audit L2";

        if (Session["TransPayId"] == null)
        {
            Response.Redirect("PendingTransL2.aspx");
        }

        if (!IsPostBack)
        {
            btnBack.PostBackUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            string strTransPayId = Session["TransPayId"].ToString();

            FundRequestDetail(Convert.ToInt32(strTransPayId));
            TruckRequestDetail(Convert.ToInt32(TransReqId));
            GetInvoicePayment();
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
            TransReqId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["TransReqId"]);
            Session["TRId"] = TransReqId;
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

            if (dsPaymentDetail.Tables[0].Rows[0]["IsFundTransFromAPI"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsPaymentDetail.Tables[0].Rows[0]["IsFundTransFromAPI"]) == true)
                {
                    rblFundTransferFromLiveTracking.SelectedValue = "1";
                }
                else
                {
                    rblFundTransferFromLiveTracking.SelectedValue = "0";
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
                        ddBabajiBankName.SelectedValue = BabajiBankID.ToString();
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

                DataSet dsVendorBank = DBOperations.GetTransporterBankDetail(VendorBankAccountId);

                if (dsVendorBank.Tables[0].Rows.Count > 0)
                {
                    lblVendorBankName.Text = dsVendorBank.Tables[0].Rows[0]["BankName"].ToString();
                    lblVendorBankAccountName.Text = lblVendorName.Text; //dsVendorBank.Tables[0].Rows[0]["AccountName"].ToString();
                    lblVendorBankAccountNo.Text = dsVendorBank.Tables[0].Rows[0]["AccountNo"].ToString();
                    lblVendorBankAccountIFSC.Text = dsVendorBank.Tables[0].Rows[0]["IFSCCode"].ToString();
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);
        string strRemark = txtRemark.Text.Trim();

        Int32 StatusId = (Int32)EnumInvoiceStatus.L2Approved;

        int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Audit L2 Completed!";
            lblError.CssClass = "success";

            Session["Success"] = "Audit L2 Completed!";

            Response.Redirect("TransportSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Audit L2 Already Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Audit L2 Approved!";

            Response.Redirect("TransportSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        // Audit Reject

        if (txtRemark.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Reject Remark!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int RequestId = Convert.ToInt32(Session["TransPayId"]);

            Int32 StatusId = (Int32)EnumInvoiceStatus.L2Reject;

            int result2 = DBOperations.AddTransPayStatus(RequestId, StatusId, txtRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result2 == 0)
            {
                lblError.Text = "Payment Request Rejected!";
                lblError.CssClass = "success";

                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else if (result2 == 2)
            {
                lblError.Text = "Audit L2 Already Rejected!";
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

        if (txtRemark.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Hold Remark!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int RequestId = Convert.ToInt32(Session["TransPayId"]);

            Int32 StatusId = (Int32)EnumInvoiceStatus.L2OnHold;

            int result2 = DBOperations.AddTransPayStatus(RequestId, StatusId, txtRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result2 == 0)
            {
                lblError.Text = "Payment Request On Hold!";
                lblError.CssClass = "success";

                Session["Success"] = lblError.Text;

                Response.Redirect("TransportSuccess.aspx");
            }
            else if (result2 == 2)
            {
                lblError.Text = "Audit L2 Aready On Hold!";
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
            //if (BankId == 47 && BankAccountID == 161) // Yes Bank NAVBHARAT Account
            //{
            //    rblFundTransferFromLiveTracking.Enabled = true;
            //}

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
    private void GetVendorBankDetail(int VendorBankAccountId)
    {
        if (VendorBankAccountId > 0)
        {
            DataSet dsVendorBankDetal = DBOperations.GetTransporterBankDetail(VendorBankAccountId);

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