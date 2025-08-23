using System;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountTransport_TransDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int TransReqId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "BSCCPL - Tracking";

        if (Session["TransPayIdTrack"] == null)
        {
            Response.Redirect("TransTracking.aspx");
        }

        if (!IsPostBack)
        {
            string strTransPayId = Session["TransPayIdTrack"].ToString();

            FundRequestDetail(Convert.ToInt32(strTransPayId));
            TruckRequestDetail(Convert.ToInt32(TransReqId));
            GetInvoicePayment(Convert.ToInt32(strTransPayId));
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
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
                        
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

            if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
        }
    }
    private void FundRequestDetail(int PayRequestId)
    {
        DataView dvFundDetail = DBOperations.GetTransportFundRequest(PayRequestId);

        if (dvFundDetail.Table.Rows.Count > 0)
        {
            TransReqId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["TransReqId"]);
            lblInvoiceNo.Text = dvFundDetail.Table.Rows[0]["InvoiceNo"].ToString();
            lblInvoiceType.Text = dvFundDetail.Table.Rows[0]["InvoiceTypeName"].ToString();
            lblVendorName.Text = dvFundDetail.Table.Rows[0]["PaidTo"].ToString();
            lblPaymentDueDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["PaymentRequiredDate"]).ToString("dd/MM/yyyy");
            lblPatymentRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblBillingPartyName.Text = dvFundDetail.Table.Rows[0]["BillToParty"].ToString();
            lblBillingGSTN.Text = dvFundDetail.Table.Rows[0]["BillingGSTIN"].ToString();
            lblTotalInvoiceValue.Text = dvFundDetail.Table.Rows[0]["Amount"].ToString();
            lblDeduction.Text = dvFundDetail.Table.Rows[0]["OtherDeduction"].ToString();
            lblAdvance.Text = dvFundDetail.Table.Rows[0]["AdvanceAmt"].ToString();
            lblPaidAmount.Text = dvFundDetail.Table.Rows[0]["PaidAmount"].ToString();
            lblJBNo.Text = dvFundDetail.Table.Rows[0]["BJVJBNumber"].ToString();

            lblRequestRemark.Text = dvFundDetail.Table.Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dvFundDetail.Table.Rows[0]["RequestBy"].ToString();

            if (dvFundDetail.Table.Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

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
    private void GetInvoicePayment(int InvoiceID)
    {
        DataSet dsPaymentDetail = DBOperations.GetTransPayPayment(InvoiceID);

        if (dsPaymentDetail.Tables[0].Rows.Count > 0)
        {
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

    #region Document Download

    protected void gvDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        int TransPayId = Convert.ToInt32(Session["TransPayIdTrack"].ToString());

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
}