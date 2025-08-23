using System;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountTransport_TransMgmtApproval : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int TransReqId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Mgmt Approval";

        if (Session["TransPayId"] == null)
        {
            Response.Redirect("PendingTransMgmt.aspx");
        }

        if (!IsPostBack)
        {
            btnBack.PostBackUrl = HttpContext.Current.Request.UrlReferrer.ToString();
            string strTransPayId = Session["TransPayId"].ToString();

            FundRequestDetail(Convert.ToInt32(strTransPayId));
            TruckRequestDetail(Convert.ToInt32(TransReqId));

            if (GridViewVehicle.Rows.Count == 0)
            {
                lblError.Text = "Vehicle Rate Details Not Avalable!!";
                lblError.CssClass = "errorMsg";
                btnSubmit.Visible = false;
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
            TransReqId = Convert.ToInt32(dvFundDetail.Table.Rows[0]["TransReqId"]);
            Session["TRId"] = TransReqId;
            lblInvoiceNo.Text = dvFundDetail.Table.Rows[0]["InvoiceNo"].ToString();
            lblInvoiceType.Text  = dvFundDetail.Table.Rows[0]["InvoiceTypeName"].ToString();
            lblPaymentDueDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["PaymentRequiredDate"]).ToString("dd/MM/yyyy");
            lblPatymentRequestDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblBillingPartyName.Text = dvFundDetail.Table.Rows[0]["BillToParty"].ToString();
            lblBillingGSTN.Text = dvFundDetail.Table.Rows[0]["BillingGSTIN"].ToString();
            lblTotalInvoiceValue.Text = dvFundDetail.Table.Rows[0]["Amount"].ToString();
            lblDeduction.Text = dvFundDetail.Table.Rows[0]["OtherDeduction"].ToString();
            lblAdvance.Text = dvFundDetail.Table.Rows[0]["AdvanceAmt"].ToString();
            lblNetPayable.Text = dvFundDetail.Table.Rows[0]["NetPayable"].ToString();
            lblRequestRemark.Text = dvFundDetail.Table.Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dvFundDetail.Table.Rows[0]["RequestBy"].ToString();

            if (dvFundDetail.Table.Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dvFundDetail.Table.Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }
            GetVendorDetail(Convert.ToInt32(dvFundDetail.Table.Rows[0]["VendorId"]));
        }
    }
    private void GetVendorDetail(int VendorId)
    {
        if (VendorId > 0)
        {
            DataView dsVendorBankDetal = DBOperations.GetCustomerDetail(VendorId.ToString()); //AccountExpense.GetVendorBankDetail(VendorBankAccountId);

            if (dsVendorBankDetal.Table.Rows.Count > 0)
            {
                lblVendorName.Text = dsVendorBankDetal.Table.Rows[0]["CustName"].ToString();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (GridViewVehicle.Rows.Count == 0)
        {
            lblError.Text = "Vehicle Rate Details Not Avalable!!";
            lblError.CssClass = "errorMsg";
            return;
        }

        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);
        string strRemark = txtRemark.Text.Trim();

        Int32 StatusId = (Int32) EnumInvoiceStatus.MgmtApproved;

        int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Transport Payment Mgmt Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Transport Payment Mgmt Approved!";

            Response.Redirect("TransportSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Payment Already Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Payment Already Approved!";

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
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);
        string strRemark = txtRemark.Text.Trim();

        Int32 StatusId = (Int32)EnumInvoiceStatus.MgmtReject;

        int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Transport Payment Mgmt Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = "Transport Payment Mgmt Rejected!";

            Response.Redirect("TransportSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Payment Already Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = "Payment Already Rejected!";

            Response.Redirect("TransportSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnHold_Click(object sender, EventArgs e)
    {
        int PayRequestId = Convert.ToInt32(Session["TransPayId"]);
        string strRemark = txtRemark.Text.Trim();

        Int32 StatusId = (Int32)EnumInvoiceStatus.MgmtOnHold;

        int result = DBOperations.AddTransPayStatus(PayRequestId, StatusId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Transport Payment Mgmt On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = "Transport Payment Mgmt On Hold!";

            Response.Redirect("TransportSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Payment Already On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = "Payment Already On Hold!";

            Response.Redirect("TransportSuccess.aspx");
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