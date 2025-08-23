using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.IO;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using BankAPI.Open;

public partial class AccountExpense_AccountSuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Status";

        Session["InvoiceId"] = null;
        Session["JobId"] = null;
        
        if (!IsPostBack)
        {
            if (Session["Error"] != null)
            {
                lblMessage.Text = Session["Error"].ToString();
                lblMessage.CssClass = "errorMsgImg";
                Session["Error"] = null;
            }
            else if (Session["Success"] != null)
            {
                lblMessage.Text = Session["Success"].ToString();
                Session["Success"] = null;

                //if (Session["PaymentTransactionId"] != null)
                //{
                //    // Wait For 5 Second to get the UTR No

                //   // Thread.Sleep(1000);

                //    // Check Bank Payment Current Status
                //    int PaymentId = Convert.ToInt32(Session["PaymentTransactionId"]);

                //    CheckPaymentStatus(PaymentId);

                //    // Reset PaymentID

                //    Session["PaymentTransactionId"] = null;
                //}
            }
            else
            {
                Response.Redirect("Dashboard.aspx");
            }
        }
    }

    #region Current Status check
    private void CheckPaymentStatus(int APITransactionID)
    {
        //DataSet dsPendingTransaction = AccountExpense.GetBankPendingTransactionByID(PaymentID);

        DataSet dsTransactionDetail = AccountExpense.GetAPIBankTransactionById(APITransactionID);

        if (dsTransactionDetail.Tables.Count > 0)
        {
            int CurrentStatusId = 0;
            bool isMailSent = false;
            string strUTRNo = "";
            int PaymentRequestId = Convert.ToInt32(dsTransactionDetail.Tables[0].Rows[0]["PaymentRequestId"]);

            string strRefNo = dsTransactionDetail.Tables[0].Rows[0]["ReqReferenceNo"].ToString();
            isMailSent = Convert.ToBoolean(dsTransactionDetail.Tables[0].Rows[0]["IsMailSent"]);

            LIVEStatusCheck(APITransactionID, PaymentRequestId, strRefNo, strUTRNo, isMailSent, CurrentStatusId);

            dsTransactionDetail = AccountExpense.GetAPIBankTransactionById(APITransactionID);

            if (dsTransactionDetail.Tables.Count > 0)
            {
                if (dsTransactionDetail.Tables[0].Rows.Count > 0)
                {
                    string TransactionStatus = "";
                    string UTRNo = "";
                    int StatusId = 0;
                    string strAmount = "";

                    if (dsTransactionDetail.Tables[0].Rows[0]["RespStatus"] != DBNull.Value)
                    {
                        TransactionStatus = dsTransactionDetail.Tables[0].Rows[0]["RespStatus"].ToString();
                    }
                    if (dsTransactionDetail.Tables[0].Rows[0]["UniqueReferenceNo"] != DBNull.Value)
                    {
                        UTRNo = dsTransactionDetail.Tables[0].Rows[0]["UniqueReferenceNo"].ToString();
                    }
                    if (dsTransactionDetail.Tables[0].Rows[0]["StatusId"] != DBNull.Value)
                    {
                        StatusId = Convert.ToInt32(dsTransactionDetail.Tables[0].Rows[0]["StatusId"].ToString());
                    }
                    if (dsTransactionDetail.Tables[0].Rows[0]["Amount"] != DBNull.Value)
                    {
                        strAmount = dsTransactionDetail.Tables[0].Rows[0]["Amount"].ToString();
                    }

                    if (StatusId == 100)
                    {
                        // Transaction Failed

                        lblMessage.CssClass = "errorMsgImg";
                        lblMessage.Text = lblMessage.Text + "<BR>" + "Transaction Status - " + TransactionStatus + " Amount:" + strAmount;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Payment Error", "alert('" + TransactionStatus + "');", true);

                    }
                    else
                    {
                        // Transaction Success

                        lblMessage.Text = lblMessage.Text + "<BR>" + " Transaction Status - " + TransactionStatus + " Amount:" + strAmount;
                        lblMessage.Text = lblMessage.Text + "<BR>" + "UTR No:" + UTRNo;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Transaction Status - ", "alert('" + TransactionStatus + "');", true);

                    }
                }
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

            if (objError.Code.ToLower() == "ns:e404" || objError.Code.ToLower() == "ns:e503")
            {
                // 1 ns:E503 - for transactions initiated during Yes Bank system downtime.
                // 2 ns:E404 - for transactions initiated during Yes Bank API layer is down(this is an existing error code)

                // DO Nothing - Add Error Description and Do NOt Mark As Failed Transaction -  Retry Get Status After 15 Minuts

                ErrorUpdResult = DBOperations.AddBankPaymentAPIError(APITransactionID, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, 1);
            }
            else
            {
                ErrorUpdResult = DBOperations.AddBankPaymentAPIError(APITransactionID, objError.Code, objError.Id, objError.Message, objError.ActionCode, objError.ActionDescription, 1);

                DateTime dtResponseDate = DateTime.Now;

                int AccountUpdResult = DBOperations.UpdateBankPaymentAPIResponse(strRefNo, "", "", objError.ActionDescription, dtResponseDate, IsSuccess, StatusId, 1);
            }

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
}