using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using BankAPI.Open;

public partial class BSCCPLTransport_TransTransaction : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["TransPayId"]   = null;
            Session["TransPayIdTrack"] = null;
            Session["JobId"]        = null;
            Session["InvoiceId"]    = null;
            Session["InvoiceId"]    = null;
            Session["InvoiceIdTrack"] = null;
            Session["ChequeId"]     = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Babaji Bank Transaction";

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;

            }
        }
        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "TransTransaction.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strInvoiceId = (string)e.CommandArgument;

            Session["TransPayIdTrack"] = strInvoiceId;

            Response.Redirect("TransDetailBT.aspx");
        }
        else if (e.CommandName.ToLower() == "movetobankpayment")
        {
            //MoveToBankTransfer
            int PaymentId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.UpdateFailedTransPayToBankTransfer(0, PaymentId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Invoice Moved To Bank Transfer Tab!";
                lblMessage.CssClass = "success";
            }
            else if (result == 1)
            {
                lblMessage.Text = "Transaction Already in Processs! Please Check Current Status.";
                lblMessage.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblMessage.Text = "Failed/Reversed Transaction Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblMessage.Text = "Invoice Detail Not Found for!";
                lblMessage.CssClass = "errorMsg";
            }
            else
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "IsSuccess") != DBNull.Value)
            {
                bool IsSuccess = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsSuccess"));
                //string strStatusName = DataBinder.Eval(e.Row.DataItem, "StatusName").ToString();

                if (IsSuccess == false) // Transaction Failed - 
                {
                    e.Row.BackColor = System.Drawing.Color.Red;  //LightSalmon;    
                    
                    //  e.Row.ToolTip = strStatusName;
                    // Show Move To Bank Transfer Button For Failed Transaction
                    
                    LinkButton objlnkMove = (LinkButton)e.Row.FindControl("lnkMove");

                    objlnkMove.Visible = true;
                }
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;  //LightSalmon;    
                                                                // Check Reverse Transaction
                if (DataBinder.Eval(e.Row.DataItem, "StatusId") != DBNull.Value)
                {
                    int statusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId"));

                    if (statusId == (Int32)EnumBankStatus.SettlementReversed)
                    {
                        // Show Move To Bank Transfer Button For Failed Transaction
                        LinkButton objlnkMove = (LinkButton)e.Row.FindControl("lnkMove");

                        objlnkMove.Visible = true;
                    }
                }
            }
        }
    }

    #region Current Status check
    protected void btnCheckStatus_Click(object sender, EventArgs e)
    {
        DataSet dsPendingTransaction = AccountExpense.GetBankPendingTransaction();

        if (dsPendingTransaction.Tables.Count > 0)
        {
            foreach (DataRow dr in dsPendingTransaction.Tables[0].Rows)
            {
                int CurrentStatusId = 0;
                bool isMailSent = false;
                string strUTRNo = "";
                int APITransactionID = Convert.ToInt32(dr["lid"]);
                int PaymentRequestId = Convert.ToInt32(dr["PaymentRequestId"]);

                string strRefNo = dr["ReqReferenceNo"].ToString();

                if (dr["StatusId"] != DBNull.Value)
                {
                    CurrentStatusId = Convert.ToInt16(dr["StatusId"]);
                }
                if (dr["InstrumentNo"] != DBNull.Value)
                {
                    strUTRNo = dr["InstrumentNo"].ToString();
                }
                if (dr["IsMailSent"] != DBNull.Value)
                {
                    isMailSent = Convert.ToBoolean(dr["IsMailSent"]);
                }

               // LIVEStatusCheck(APITransactionID, PaymentRequestId, strRefNo, strUTRNo, isMailSent, CurrentStatusId);
            }
        }
    }

    public static void LIVEStatusCheck(int APITransactionID, int PaymentRequestId, string strRefNo, string strUTRNo, bool isAlreadyMailSent, int CurrentStatusId)
    {
        //  string strToken = GenerateToken();

        BankReqStatus.Root objRoot = new BankReqStatus.Root();
        BankReqStatus.Data objData = new BankReqStatus.Data();

        objRoot.Data = objData;

        objData.ConsentId = "418585";
        objData.InstrId = strRefNo;
        objData.SecondaryIdentification = "418585";


        //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //var jsonPayLoad = @serializer.Serialize(objRoot);

        var jsonPayLoad = JsonConvert.SerializeObject(objRoot);

        BankAPIError.Root objError = new BankAPIError.Root();
        BankAPIRespTransfer.Root objSuccess = new BankAPIRespTransfer.Root();

        string jsonResponse = "";
        int StatusId = -1;

        string result = BankAPIMethods.LiveBankGetStatus(jsonPayLoad, ref objError, ref objSuccess);
        int ErrorUpdResult = 0;

        if (objError.Code != null)
        {
            int IsSuccess = 0;

            StatusId = (int)EnumBankStatus.FAILED;

            jsonResponse = objError.Message;

            ErrorUpdResult = DBOperations.AddBankPaymentAPIError(APITransactionID, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, 1);

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

                if (objSuccess.Meta != null)
                {
                    ErrorUpdResult = DBOperations.AddBankPaymentAPIError(APITransactionID, objSuccess.Meta.ErrorCode, "", objSuccess.Meta.ErrorSeverity, objSuccess.Meta.ActionCode, objSuccess.Meta.ActionDescription, 1);
                }
            }

            DateTime dtResponseDate = DateTime.Parse(objSuccess.Data.StatusUpdateDateTime.ToString());
            string strTransactionIdentification = "";
            string strEndToEndIdentification = "";

            if (objSuccess.Data.TransactionIdentification != null && objSuccess.Data.TransactionIdentification != "")
            {
                strTransactionIdentification = objSuccess.Data.TransactionIdentification;
            }
            if (objSuccess.Data.Initiation.EndToEndIdentification != null && objSuccess.Data.Initiation.EndToEndIdentification != "")
            {
                strEndToEndIdentification = objSuccess.Data.Initiation.EndToEndIdentification;

                if (strEndToEndIdentification != strUTRNo)
                {
                    int UTRResult = AccountExpense.UpdateBankPaymentUTRNo(PaymentRequestId, strEndToEndIdentification, dtResponseDate);
                }

                if (isAlreadyMailSent == false && StatusId != (int)EnumBankStatus.FAILED)
                {
                    bool bMailSucces = SendPaymentConfirmationMail(PaymentRequestId);
                    AccountExpense.UpdateBankPaymentMailStatus(PaymentRequestId, bMailSucces);
                }
            }

            if (CurrentStatusId != StatusId)
            {
                // Update if change in Status;

                int AccountUpdResult = DBOperations.UpdateBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, strTransactionIdentification,
                    strEndToEndIdentification, objSuccess.Data.Status, objSuccess.Data.StatusUpdateDateTime, IsSuccess, StatusId, 1);

            }
        }

    }

    private static bool SendPaymentConfirmationMail(int PaymentId)
    {
        int Invoiceid = 0;

        bool IsMailSent = false;

        string MessageBody = "", strToEmail = "", strCCEmail = "", strSubject = "";

        bool bEmailSucces = false;
        strCCEmail = EmailConfig.GetEmailCCTo();

        string strPaymentUser = "";

        string strTotalPaidAmount = "";
        string strPaymentRemark = "";
        string strUTRNo = "";
        string strUTRDate = "";
        string EmailContent = "";

        StringBuilder strbuilder = new StringBuilder();

        DataSet dsPaymentDetail = AccountExpense.GetInvoicePaymentDetail(PaymentId);

        if (dsPaymentDetail.Tables.Count > 0)
        {
            Invoiceid = Convert.ToInt32(dsPaymentDetail.Tables[0].Rows[0]["InvoiceId"]);

            strTotalPaidAmount = dsPaymentDetail.Tables[0].Rows[0]["PaidAmount"].ToString();
            strUTRNo = dsPaymentDetail.Tables[0].Rows[0]["InstrumentNo"].ToString();
            strUTRDate = Convert.ToDateTime(dsPaymentDetail.Tables[0].Rows[0]["InstrumentDate"]).ToString("dd/MM/yyyy");

            strPaymentRemark = dsPaymentDetail.Tables[0].Rows[0]["Remark"].ToString();

            if (dsPaymentDetail.Tables[0].Rows[0]["IsMailSent"] != DBNull.Value)
            {
                IsMailSent = Convert.ToBoolean(dsPaymentDetail.Tables[0].Rows[0]["IsMailSent"]);
            }
        }

        if (IsMailSent == false)
        {
            DataSet dsDetail = AccountExpense.GetInvoiceDetail(Invoiceid);

            string strJobNumber = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            string strCustomer = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            string strConsignee = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();

            string strExpenseType = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

            string strVendorName = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();

            string strInvoiceNo = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();

            string strTotalInvoiceValue = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();

            string strTDSTotalAmount = "0";

            string strCreatedByEmail = "neeraj.mandowara@babajishivram.com";

            if (dsDetail.Tables[0].Rows[0]["TDSTotalAmount"] != DBNull.Value)
            {
                strTDSTotalAmount = dsDetail.Tables[0].Rows[0]["TDSTotalAmount"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["CreatedByEmail"] != DBNull.Value)
            {
                strCreatedByEmail = dsDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
            }

            try
            {
                try
                {
                    string strFileName = System.Web.HttpContext.Current.Server.MapPath("~") + "//EmailTemplate/EmailVendorPaymentConfirmation.txt";

                    //string strFileName = Path.Combine(Environment.CurrentDirectory, "EmailVendorPaymentConfirmation.txt");

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
                MessageBody = MessageBody.Replace("@TotalPaidAmount", strTotalPaidAmount);
                MessageBody = MessageBody.Replace("@ChequeUTRNo", strUTRNo);
                MessageBody = MessageBody.Replace("@ChequeUTRDate", strUTRDate);
                MessageBody = MessageBody.Replace("@Remarks", strPaymentRemark);
                MessageBody = MessageBody.Replace("@PaymentUserName", strPaymentUser);

                ///////////////////////////////////////////////////////////////////////////////

                try
                {
                    strSubject = "Vendor Payment Detail /" + strJobNumber + "/ " + strInvoiceNo + "/ " + strConsignee + " / " + strExpenseType;

                    string EmailBody = MessageBody;

                    bEmailSucces = EMail.SendMailCC("neeraj.mandowara@babajishivram.com ", strCreatedByEmail, "accounts@babajishivram.com", strSubject, EmailBody, "");
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
        }
        else
        {
            bEmailSucces = true;
        }
        return bEmailSucces;
    }

    #endregion
    
    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "TransTransaction.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Trans_BankTransaction_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;
        gvDetail.Columns[1].Visible = false;
        gvDetail.Columns[2].Visible = true;
        gvDetail.Columns[gvDetail.Columns.Count - 1].Visible = false;

        DataFilter1.FilterSessionID = "TransTransaction.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();
         
        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}