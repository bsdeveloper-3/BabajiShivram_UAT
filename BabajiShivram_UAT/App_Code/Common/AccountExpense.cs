using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using BSImport;

/// <summary>
/// Summary description for AccountExpense
/// </summary>
public class AccountExpense
{
    public AccountExpense()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GenerateVoucherNo()
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("AC_GenerateVoucherNo", command, "@OutPut");
        return SPresult;
    }

    public static int GetNewPaymentLid()
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();
        Count = CDatabase.GetSPCount("AC_GetNewPaymentLid", command);
        return Count;
    }

    public static DataSet GetExpenseTypeById(int ExpenseId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        return CDatabase.GetDataSet("AC_GetRequestTypeById", command);
    }

    public static DataSet GetJobdetailById(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        return CDatabase.GetDataSet("AC_GetJobdetailById", command);
    }

    public static DataSet GetJobDetailByRefNo(string strJobRefNo, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = strJobRefNo;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        return CDatabase.GetDataSet("AC_GetJobdetailByRefNo", command);
    }

    public static DataSet GetBJVVendorById(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("BJV_GetVendorMSById", command);
    }

    public static DataSet GetVendorByCode(string VendorCode)
    {
        // Get Accounts Vendor By Code // BJV_VendorMS
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        return CDatabase.GetDataSet("AC_GetVendorMSByCode", command);
    }

    public static DataSet GetVendorNameById(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("AC_GetVendorNameById", command);
    }

    public static DataSet GetPaymentExpenses(int PaymentId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        return CDatabase.GetDataSet("AC_GetPaymentExpenses", command);
    }

    public static DataSet GetAccountNameByCodeId(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("AC_GetAccountNameByCodeId", command);
    }

    public static DataSet GetPaymentRequestById(int PaymentId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        return CDatabase.GetDataSet("AC_GetPaymentRequestById", command);
    }

    public static DataSet GetJobDetailforjob(int JobId, decimal ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@jobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        return CDatabase.GetDataSet("AC_GetJobDetailsHistory", command);
    }

    public static int AddJobDetails(int JobId, string JobRefNo, int ExpenseTypeId, int PaymentTypeId, decimal Amount, string PaidTo, int BranchId, string Remark, int UserId,
                                         Boolean AdvanceReceived, decimal AdvanceAmt, int ACPNonACP, int RdDutyPenalty, string PenaltyApprMail, decimal DutyAmount,
                                         string ChallanNo, int IsInterestReq, int IsPenaltyReq,
                                         decimal dcInterestAmount, decimal dcPenaltyAmount, string strPenaltyCopy, string Total_Amnt, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@RequestTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@PaidTo", SqlDbType.NVarChar).Value = PaidTo;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AdvanceReceived", SqlDbType.Bit).Value = AdvanceReceived;
        command.Parameters.Add("@AdvanceAmt", SqlDbType.Decimal).Value = AdvanceAmt;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@ACPNonACP", SqlDbType.Int).Value = ACPNonACP;                     // N
        command.Parameters.Add("@RdDutyPenalty", SqlDbType.Int).Value = RdDutyPenalty;             // N
        command.Parameters.Add("@PenaltyApprMail", SqlDbType.NVarChar).Value = PenaltyApprMail;
        command.Parameters.Add("@DutyAmount", SqlDbType.Decimal).Value = DutyAmount;               // N
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;                // N
        command.Parameters.Add("@IsInterestReq", SqlDbType.Int).Value = IsInterestReq;             // N
        command.Parameters.Add("@IsPenaltyReq", SqlDbType.Int).Value = IsPenaltyReq;               // N
        command.Parameters.Add("@dcInterestAmount", SqlDbType.Decimal).Value = dcInterestAmount;   // N
        command.Parameters.Add("@dcPenaltyAmount", SqlDbType.Decimal).Value = dcPenaltyAmount;     // N
        command.Parameters.Add("@PenaltyMailCopy", SqlDbType.NVarChar).Value = strPenaltyCopy;     // N   
        command.Parameters.Add("@Total_Amnt", SqlDbType.Decimal).Value = Total_Amnt;               // N 
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insPaymentRequest", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int ApproveJobExpenseDetail(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updPaymentRequest", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddPaymentExpenses(int PaymentId, int ACCodeId, int JobId, double Debit, double Credit, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@ACCodeId", SqlDbType.Int).Value = ACCodeId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Debit", SqlDbType.Decimal).Value = Debit;
        command.Parameters.Add("@Credit", SqlDbType.Decimal).Value = Credit;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insPaymentExpenses", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdatePaymentExpenses(int lid, int JobId, double Debit, double Credit, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Debit", SqlDbType.Decimal).Value = Debit;
        command.Parameters.Add("@Credit", SqlDbType.Decimal).Value = Credit;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updPaymentExpenses", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeletePaymentExpenses(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_delPaymentExpenses", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddPaymentDetails(int PaymentId, string VoucherNo, int VendorId, int CurrencyId, string Rate,int JobId, string ChequeNo, DateTime ChequeDate,
        string BankName, string Remark, double Total, string RefNo, string PODDocPath, DateTime PaymentDate,
        int ACPNonACP, string TR6ChallenNo, int PaymentType, string PenaltyAppMail, double AssessableValue, double CustomDuty, double StampDuty,
        string GSTNo, double DutyAmount, double InterestAmount, double PenaltyAmount, string PenaltyCopyPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = RefNo;
        command.Parameters.Add("@PODDocPath", SqlDbType.NVarChar).Value = PODDocPath;
        command.Parameters.Add("@PaymentDate", SqlDbType.Date).Value = PaymentDate;
        command.Parameters.Add("@VoucherNo", SqlDbType.NVarChar).Value = VoucherNo;
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@CurrencyId", SqlDbType.Int).Value = CurrencyId;
        command.Parameters.Add("@Rate", SqlDbType.NVarChar).Value = Rate;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("@ChequeDate", SqlDbType.Date).Value = ChequeDate;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        //command.Parameters.Add("@Favouring", SqlDbType.NVarChar).Value = Favouring;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        if (ACPNonACP > 0)
            command.Parameters.Add("@ACPNonACP", SqlDbType.Int).Value = ACPNonACP;
        command.Parameters.Add("@TR6ChallenNo", SqlDbType.NVarChar).Value = TR6ChallenNo;
        if (PaymentType > 0)
            command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@PenaltyAppMail", SqlDbType.NVarChar).Value = PenaltyAppMail;
        command.Parameters.Add("@AssessableValue", SqlDbType.Decimal).Value = AssessableValue;
        command.Parameters.Add("@CustomDuty", SqlDbType.Decimal).Value = CustomDuty;
        command.Parameters.Add("@StampDuty", SqlDbType.Decimal).Value = StampDuty;
        command.Parameters.Add("@GSTNo", SqlDbType.NVarChar).Value = GSTNo;
        command.Parameters.Add("@DutyAmount", SqlDbType.Decimal).Value = DutyAmount;
        command.Parameters.Add("@InterestAmount", SqlDbType.Decimal).Value = InterestAmount;
        command.Parameters.Add("@PenaltyAmount", SqlDbType.Decimal).Value = PenaltyAmount;
        command.Parameters.Add("@PenaltyCopyPath", SqlDbType.NVarChar).Value = PenaltyCopyPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insPaymentDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddExpenseDocDetails(int PaymentId, string DocPath, string FileName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insExpenseDocDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetExpenseDocDetails(int PaymentId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        return CDatabase.GetDataSet("AC_GetExpenseDocDetails", command);
    }

    public static int AddPaymentStatus(int PaymentId, int StatusId, string Remark, Boolean IsActive, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insPaymentStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddExamineDetails(int JobId, DateTime PlanningDate, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PlanningDate", SqlDbType.DateTime).Value = PlanningDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insExamineDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetReportByRequestType(string RequestTypeName)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PaymentTypeName", SqlDbType.NVarChar).Value = RequestTypeName;
        return CDatabase.GetDataSet("AC_rptFundDetails", command);
    }

    public static DataSet GetDutyPaymentReport(int lUser)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("AC_rptDutyPayment", command);
    }

    public static DataSet GetStampDutyPaymentReport(int lUser)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("AC_rptStampDutyPayment", command);
    }

  
    public static DataSet GetTodaysDutyAmountDetails(int lUser, DateTime From, DateTime To, int ExpenseType)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;
        command.Parameters.Add("@ExpenseType", SqlDbType.Int).Value = ExpenseType;
        return CDatabase.GetDataSet("AC_rptGetDutyPayment", command);
    }

    public static DataSet GetExpenseRPTAllPayment(int lUser,DateTime From, DateTime To, int ExpenseType)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;
        command.Parameters.Add("@ExpenseType", SqlDbType.Int).Value = ExpenseType;

        return CDatabase.GetDataSet("AC_rptGetAllPayment", command);
    }

    public static DataSet GetJobDetailForCompPayment(int PaymentId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;

        return CDatabase.GetDataSet("AC_GetPaymentRequestById", command);
    }

    public static int UpdateJobDetailCompPayment(int JobId, int PaymentId, decimal Amount, string PaidTo, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@PaidTo", SqlDbType.NVarChar).Value = PaidTo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDetailPayment", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDetailDuty(int JobId, int PaymentId, int ACPNonACP, string ChallanNo, decimal DutyAmount, decimal IntAmount,
                decimal PenaltyAmount, int RdDutyPenalty, decimal AdvanceDetails, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@ACPNonACP", SqlDbType.Int).Value = ACPNonACP;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;
        command.Parameters.Add("@DutyAmount", SqlDbType.Decimal).Value = DutyAmount;
        command.Parameters.Add("@IntAmount", SqlDbType.Decimal).Value = IntAmount;
        command.Parameters.Add("@PenaltyAmount", SqlDbType.Decimal).Value = PenaltyAmount;
        command.Parameters.Add("@RdDutyPenalty", SqlDbType.Int).Value = RdDutyPenalty;
        command.Parameters.Add("@AdvanceDetails", SqlDbType.Decimal).Value = AdvanceDetails;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDetailDutyPayment", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetPaymentDocument(int PaymentId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        return CDatabase.GetDataSet("AC_GetExpenseDocDetails", command);
    }

    public static int DeletePaymentDocuments(int DocId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@DocId", SqlDbType.Int).Value = DocId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_delPaymentDocuments", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdIsPayProcess(int PaymentId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updIsPaymentProcess", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #region FA Expense
    public static void FillJobExpensePaidTo(DropDownList DropDown, string JobRefNo)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        CDatabase.BindControls(DropDown, "AC_FAFillPaidToByJobNo", command, "PaidToName", "PaidToName");
    }
    public static int FA_AddIssueInstrument(int JobId, string JobRefNo, string ChequeNo, DateTime ChequeDate,
        string BankName, decimal ChequeAmount, int CustomerId, decimal JobAmount, string PayTo, string PayToCode, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("@ChequeDate", SqlDbType.Date).Value = ChequeDate;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        command.Parameters.Add("@ChequeAmount", SqlDbType.Decimal).Value = ChequeAmount;

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@JobAmount", SqlDbType.Decimal).Value = JobAmount;
        command.Parameters.Add("@PayTo", SqlDbType.NVarChar).Value = PayTo;
        command.Parameters.Add("@PayToCode", SqlDbType.NVarChar).Value = PayToCode;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_FAinsChequeDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet FA_GetIssueInstrumentByID(int ChequeIssueId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ChequeIssueId", SqlDbType.Int).Value = ChequeIssueId;

        DataSet dsResult = CDatabase.GetDataSet("AC_FAGetChequeDetailById", command);

        return dsResult;
    }

    public static int FA_AddExpenseBooking(int JobId, string JobRefNo, int PayModeID, int RIMorAG, string PaidTo, string PaidToCode,
        int ChequeIssueID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@PayModeID", SqlDbType.Int).Value = PayModeID;
        command.Parameters.Add("@RIMorAG", SqlDbType.Int).Value = RIMorAG;
        command.Parameters.Add("@PaidTo", SqlDbType.NVarChar).Value = PaidTo;
        command.Parameters.Add("@PaidToCode", SqlDbType.NVarChar).Value = PaidToCode;
        command.Parameters.Add("@ChequeIssueID", SqlDbType.Int).Value = ChequeIssueID;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_FAinsExpenseBooking", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int FA_AddExpenseBookingRIM(int BookingId, string ChargeName, string ChargeCode, string GSTIN, string InvoiceNo,
        DateTime InvoiceDate, decimal InvoiceTaxableAmount, decimal InvoiceTotalAmount, int CGST, int SGST, int IGST,
        string ReceiptNumber, DateTime ReceiptDate, string VendorName, string VendorCode, string Narration, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BookingId", SqlDbType.Int).Value = BookingId;
        command.Parameters.Add("@ChargeName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@ChargeCode", SqlDbType.NVarChar).Value = ChargeCode;
        command.Parameters.Add("@GSTIN", SqlDbType.NVarChar).Value = GSTIN;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceTaxableAmount", SqlDbType.Decimal).Value = InvoiceTaxableAmount;
        command.Parameters.Add("@InvoiceTotalAmount", SqlDbType.Decimal).Value = InvoiceTotalAmount;

        command.Parameters.Add("@CGST", SqlDbType.Int).Value = CGST;
        command.Parameters.Add("@SGST", SqlDbType.Int).Value = SGST;
        command.Parameters.Add("@IGST", SqlDbType.Int).Value = IGST;

        command.Parameters.Add("@ReceiptNumber", SqlDbType.NVarChar).Value = ReceiptNumber;
        command.Parameters.Add("@ReceiptDate", SqlDbType.DateTime).Value = ReceiptDate;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@Narration", SqlDbType.NVarChar).Value = Narration;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_FAinsExpenseBookingRIM", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int FA_AddExpenseBookingAgency(int BookingId, string ChargeName, string ChargeCode, decimal AgencyAmount, bool ChargeToParty,
        int ApprovedBy, string ApprovalFilePath, decimal PartyChargeAmount, int ServiceId, string ServiceRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BookingId", SqlDbType.Int).Value = BookingId;
        command.Parameters.Add("@ChargeName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@ChargeCode", SqlDbType.NVarChar).Value = ChargeCode;
        command.Parameters.Add("@AgencyAmount", SqlDbType.Decimal).Value = AgencyAmount;
        command.Parameters.Add("@ChargeToParty", SqlDbType.Bit).Value = ChargeToParty;
        command.Parameters.Add("@ApprovedBy", SqlDbType.Int).Value = ApprovedBy;
        command.Parameters.Add("@ApprovalFilePath", SqlDbType.NVarChar).Value = ApprovalFilePath;
        command.Parameters.Add("@PartyChargeAmount", SqlDbType.Decimal).Value = PartyChargeAmount;

        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@ServiceRemark", SqlDbType.NVarChar).Value = ServiceRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_FAinsExpenseBookingAG", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int FA_PostRIMExpense(BJVRIMExpense bjvExp)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@billno", SqlDbType.Int).Value = bjvExp.BillNo;
        command.Parameters.Add("@billdate", SqlDbType.SmallDateTime).Value = bjvExp.BillDate;
        command.Parameters.Add("@par_code", SqlDbType.NVarChar).Value = bjvExp.Par_Code;
        command.Parameters.Add("@ref", SqlDbType.NVarChar).Value = bjvExp.RefNO;
        command.Parameters.Add("@narration", SqlDbType.NVarChar).Value = bjvExp.Narration;
        command.Parameters.Add("@amount", SqlDbType.Decimal).Value = bjvExp.Amount;
        command.Parameters.Add("@jvpcode", SqlDbType.NVarChar).Value = bjvExp.JvpCode;
        //command.Parameters.Add("@chargable", SqlDbType.Decimal).Value = bjvExp.Chargable;
        command.Parameters.Add("@chequeno", SqlDbType.NVarChar).Value = bjvExp.ChequeNo;
        command.Parameters.Add("@chequedt", SqlDbType.SmallDateTime).Value = bjvExp.ChequeDate;
        command.Parameters.Add("@bankname", SqlDbType.VarChar).Value = bjvExp.BankName;

        command.Parameters.Add("@CSTCODE1", SqlDbType.NVarChar).Value = bjvExp.CSTCODEOne;
        command.Parameters.Add("@CSTCODE2", SqlDbType.NVarChar).Value = bjvExp.CSTCODETwo;
        command.Parameters.Add("@CSTCODE3", SqlDbType.NVarChar).Value = bjvExp.CSTCODEThree;
        command.Parameters.Add("@CSTCODE4", SqlDbType.NVarChar).Value = bjvExp.CSTCODEFour;
        command.Parameters.Add("@vcode", SqlDbType.NVarChar).Value = bjvExp.VCode;

        command.Parameters.Add("@charge", SqlDbType.NVarChar).Value = bjvExp.ChargeCode;
        command.Parameters.Add("@paid_to", SqlDbType.NVarChar).Value = bjvExp.Paid_To;
        command.Parameters.Add("@rno", SqlDbType.NVarChar).Value = bjvExp.RNO;
        command.Parameters.Add("@date", SqlDbType.SmallDateTime).Value = bjvExp.Paid_Date;
        command.Parameters.Add("@txval", SqlDbType.Decimal).Value = bjvExp.Txval;
        command.Parameters.Add("@CGSTRT", SqlDbType.NVarChar).Value = bjvExp.CGSTRT;
        command.Parameters.Add("@CGSTAMT", SqlDbType.NVarChar).Value = bjvExp.CGSTAMT;
        command.Parameters.Add("@SGSTRT", SqlDbType.NVarChar).Value = bjvExp.SGSTRT;
        command.Parameters.Add("@SGSTAMT", SqlDbType.NVarChar).Value = bjvExp.SGSTAMT;
        command.Parameters.Add("@IGSTRT", SqlDbType.NVarChar).Value = bjvExp.IGSTRT;
        command.Parameters.Add("@IGSTAMT", SqlDbType.NVarChar).Value = bjvExp.IGSTAMT;
        command.Parameters.Add("@brdate", SqlDbType.NVarChar).Value = bjvExp.BRDate;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetFASPOutPut("BJV_insBookingExpense", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int FA_PostRimERC(int BookingId, int RIMExpenseId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BookingId", SqlDbType.Int).Value = BookingId;
        command.Parameters.Add("@RIMExpenseId", SqlDbType.Int).Value = RIMExpenseId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FA_InsERC", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetExpeneBooking(int BookingID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BookingID", SqlDbType.Int).Value = BookingID;

        return CDatabase.GetDataSet("AC_FAGetExpenseBookingByID", command);
    }

    public static DataSet GetDuplicateVendorInvoice(string strJobRefNO, string VendorCode, string InvoiceNo, DateTime InvoiceDate)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;

        return CDatabase.GetDataSet("AC_CheckDuplicateInvoice", command);
    }

    public static DataSet CheckVendorInvoicePending(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;

        return CDatabase.GetDataSet("AC_CheckInvoicePending", command);
    }

    public static DataSet CheckFinalInvoicePending(string strJobRefNO, int JobId, int ModuleId)
    {
        // Pass Either JobRefNo or JobId and ModuleId
        SqlCommand command = new SqlCommand();

        if (strJobRefNO != "")
        {
            command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        }
        else
        {
            command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
            command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        }

        return CDatabase.GetDataSet("AC_CheckFinalInvoicePending", command);
    }

    public static DataSet CheckFinalInvoicePendingProforma(int VendorId, string VendorCode, string strJobRefNO, int JobId, int ModuleId)
    {
        // Pass Either JobRefNo or JobId and ModuleId
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;

        if (strJobRefNO != "")
        {
            command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        }
        else
        {
            command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
            command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        }

        return CDatabase.GetDataSet("AC_CheckFinalInvoicePendingVendor", command);
    }

    public static int AddInvoiceDetail(int JobId, string strJobRefNO, int ModuleID, int BranchId, int ExpenseTypeId, int InvoiceMode,
    int BillType, bool isRIM, int InvoiceType, int PaymentTypeId, int VendorGSTNType, string ConsigneeGSTIN,
    string ConsigneeName, string ConsigneeCode, string ConsigneePAN, bool IsInterest, decimal InterestAmount,
    bool IsAdvanceReceived, decimal AdvanceAmount, DateTime PaymentRequiredDate, DateTime PaymentDueDate,
    string VendorName, string VendorCode, string VendorGSTNo, string VendorPAN, string InvoiceNo, DateTime InvoiceDate,
    Decimal InvoicexAmount, Decimal decTaxableAmount, Decimal decGSTAmount, int InvoiceCurrencyId, decimal decInvoiceCurrencyAmt,
    decimal decInvoiceCurrencyExchangeRate, string strPaymentTerms, bool IsImmediatePayment, string strRemark, int HODId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@BillType", SqlDbType.Int).Value = BillType;
        command.Parameters.Add("@InvoiceMode", SqlDbType.Int).Value = InvoiceMode;
        command.Parameters.Add("@IsRim", SqlDbType.Bit).Value = isRIM;
        command.Parameters.Add("@InvoiceType", SqlDbType.Int).Value = InvoiceType;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;

        command.Parameters.Add("@ConsigneeGSTIN", SqlDbType.NVarChar).Value = ConsigneeGSTIN;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@ConsigneeCode", SqlDbType.NVarChar).Value = ConsigneeCode;
        command.Parameters.Add("@ConsigneePAN", SqlDbType.NVarChar).Value = ConsigneePAN;

        command.Parameters.Add("@IsInterest", SqlDbType.Bit).Value = IsInterest;
        command.Parameters.Add("@InterestAmount", SqlDbType.Decimal).Value = InterestAmount;
        command.Parameters.Add("@IsAdvanceReceived", SqlDbType.Bit).Value = IsAdvanceReceived;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;

        if (PaymentRequiredDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentRequiredDate", SqlDbType.DateTime).Value = PaymentRequiredDate;

        if (PaymentDueDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = PaymentDueDate;

        command.Parameters.Add("@VendorGSTNType", SqlDbType.Int).Value = VendorGSTNType;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@VendorGSTNo", SqlDbType.NVarChar).Value = VendorGSTNo;
        command.Parameters.Add("@VendorPAN", SqlDbType.NVarChar).Value = VendorPAN;

        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = InvoicexAmount;
        command.Parameters.Add("@InvoiceTaxableAmount", SqlDbType.Decimal).Value = decTaxableAmount;
        command.Parameters.Add("@InvoiceGSTAmount", SqlDbType.Decimal).Value = decGSTAmount;

        command.Parameters.Add("@InvoiceCurrencyId", SqlDbType.Int).Value = InvoiceCurrencyId;
        command.Parameters.Add("@InvoiceCurrencyAmt", SqlDbType.Decimal).Value = decInvoiceCurrencyAmt;
        command.Parameters.Add("@InvoiceCurrencyExchangeRate", SqlDbType.Decimal).Value = decInvoiceCurrencyExchangeRate;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = strPaymentTerms;
        command.Parameters.Add("@IsImmediatePayment", SqlDbType.Bit).Value = IsImmediatePayment;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@HODId", SqlDbType.Int).Value = HODId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceDetail(int JobId, string strJobRefNO, int ModuleID, int BranchId, int ExpenseTypeId,
     bool isRIM, int InvoiceType, int PaymentTypeId,
     int VendorGSTNType, string ConsigneeGSTIN, string ConsigneeName, string ConsigneeCode, string ConsigneePAN,
     bool IsInterest, decimal InterestAmount, bool IsAdvanceReceived, decimal AdvanceAmount,
     DateTime PaymentRequiredDate, DateTime PaymentDueDate, string VendorName, string VendorCode, string VendorGSTNo,
     string VendorPAN, string InvoiceNo, DateTime InvoiceDate, Decimal InvoicexAmount, Decimal decTaxableAmount, Decimal decGSTAmount,
     int InvoiceCurrencyId, decimal decInvoiceCurrencyAmt, decimal decInvoiceCurrencyExchangeRate, string strPaymentTerms,
     bool IsImmediatePayment, string strRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@IsRim", SqlDbType.Bit).Value = isRIM;
        command.Parameters.Add("@InvoiceType", SqlDbType.Int).Value = InvoiceType;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;

        command.Parameters.Add("@ConsigneeGSTIN", SqlDbType.NVarChar).Value = ConsigneeGSTIN;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@ConsigneeCode", SqlDbType.NVarChar).Value = ConsigneeCode;
        command.Parameters.Add("@ConsigneePAN", SqlDbType.NVarChar).Value = ConsigneePAN;

        command.Parameters.Add("@IsInterest", SqlDbType.Bit).Value = IsInterest;
        command.Parameters.Add("@InterestAmount", SqlDbType.Decimal).Value = InterestAmount;
        command.Parameters.Add("@IsAdvanceReceived", SqlDbType.Bit).Value = IsAdvanceReceived;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;

        if (PaymentRequiredDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentRequiredDate", SqlDbType.DateTime).Value = PaymentRequiredDate;

        if (PaymentDueDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = PaymentDueDate;

        command.Parameters.Add("@VendorGSTNType", SqlDbType.Int).Value = VendorGSTNType;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@VendorGSTNo", SqlDbType.NVarChar).Value = VendorGSTNo;
        command.Parameters.Add("@VendorPAN", SqlDbType.NVarChar).Value = VendorPAN;


        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = InvoicexAmount;
        command.Parameters.Add("@InvoiceTaxableAmount", SqlDbType.Decimal).Value = decTaxableAmount;
        command.Parameters.Add("@InvoiceGSTAmount", SqlDbType.Decimal).Value = decGSTAmount;

        command.Parameters.Add("@InvoiceCurrencyId", SqlDbType.Int).Value = InvoiceCurrencyId;
        command.Parameters.Add("@InvoiceCurrencyAmt", SqlDbType.Decimal).Value = decInvoiceCurrencyAmt;
        command.Parameters.Add("@InvoiceCurrencyExchangeRate", SqlDbType.Decimal).Value = decInvoiceCurrencyExchangeRate;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = strPaymentTerms;
        command.Parameters.Add("@IsImmediatePayment", SqlDbType.Bit).Value = IsImmediatePayment;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceDetail2(int JobId, int ProformaInvoiceId, string strJobRefNO, int ModuleID, int BranchId, int ExpenseTypeId,
   bool isRIM, int InvoiceType, int PaymentTypeId, int VendorGSTNType, string ConsigneeGSTIN, string ConsigneeName, string ConsigneeCode, string ConsigneePAN,
   bool IsInterest, decimal InterestAmount, bool IsAdvanceReceived, decimal AdvanceAmount,
   DateTime PaymentRequiredDate, DateTime PaymentDueDate, string VendorName, string VendorCode, string VendorGSTNo,
   string VendorPAN, string InvoiceNo, DateTime InvoiceDate, Decimal InvoicexAmount, Decimal decTaxableAmount, Decimal decGSTAmount,
   int InvoiceCurrencyId, decimal decInvoiceCurrencyAmt, decimal decInvoiceCurrencyExchangeRate,
   string strPaymentTerms, bool IsImmediatePayment, string strRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ProformaInvoiceId", SqlDbType.Int).Value = ProformaInvoiceId;
        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@IsRim", SqlDbType.Bit).Value = isRIM;
        command.Parameters.Add("@InvoiceType", SqlDbType.Int).Value = InvoiceType;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;

        command.Parameters.Add("@ConsigneeGSTIN", SqlDbType.NVarChar).Value = ConsigneeGSTIN;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@ConsigneeCode", SqlDbType.NVarChar).Value = ConsigneeCode;
        command.Parameters.Add("@ConsigneePAN", SqlDbType.NVarChar).Value = ConsigneePAN;

        command.Parameters.Add("@IsInterest", SqlDbType.Bit).Value = IsInterest;
        command.Parameters.Add("@InterestAmount", SqlDbType.Decimal).Value = InterestAmount;
        command.Parameters.Add("@IsAdvanceReceived", SqlDbType.Bit).Value = IsAdvanceReceived;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;

        if (PaymentRequiredDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentRequiredDate", SqlDbType.DateTime).Value = PaymentRequiredDate;

        if (PaymentDueDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = PaymentDueDate;

        command.Parameters.Add("@VendorGSTNType", SqlDbType.Int).Value = VendorGSTNType;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@VendorGSTNo", SqlDbType.NVarChar).Value = VendorGSTNo;
        command.Parameters.Add("@VendorPAN", SqlDbType.NVarChar).Value = VendorPAN;

        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = InvoicexAmount;
        command.Parameters.Add("@InvoiceTaxableAmount", SqlDbType.Decimal).Value = decTaxableAmount;
        command.Parameters.Add("@InvoiceGSTAmount", SqlDbType.Decimal).Value = decGSTAmount;

        command.Parameters.Add("@InvoiceCurrencyId", SqlDbType.Decimal).Value = InvoiceCurrencyId;
        command.Parameters.Add("@InvoiceCurrencyAmt", SqlDbType.Decimal).Value = decInvoiceCurrencyAmt;
        command.Parameters.Add("@InvoiceCurrencyExchangeRate", SqlDbType.Decimal).Value = decInvoiceCurrencyExchangeRate;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = strPaymentTerms;
        command.Parameters.Add("@IsImmediatePayment", SqlDbType.Bit).Value = IsImmediatePayment;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateInvoiceDetail(int InvoiceId, int JobId, string strJobRefNO, int ModuleID, int ExpenseTypeId,
       bool isRIM, int InvoiceType, int PaymentTypeId,
       int VendorGSTNType, string ConsigneeGSTIN, string ConsigneeName, string ConsigneeCode, string ConsigneePAN,
       bool IsInterest, decimal InterestAmount, bool IsAdvanceReceived, decimal AdvanceAmount,
       DateTime PaymentRequiredDate, DateTime PaymentDueDate, string VendorName, string VendorCode, string VendorGSTNo,
       string VendorPAN, string InvoiceNo, DateTime InvoiceDate, Decimal InvoicexAmount, Decimal decTaxableAmount, Decimal decGSTAmount,
       int InvoiceCurrencyId, decimal decInvoiceCurrencyAmt, decimal decInvoiceCurrencyExchangeRate,
       string strPaymentTerms, bool IsImmediatePayment, int BillType, int HODId, int InvoiceMode, string strRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        command.Parameters.Add("@BillType", SqlDbType.Int).Value = BillType;
        command.Parameters.Add("@HODId", SqlDbType.Int).Value = BillType;
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@IsRim", SqlDbType.Bit).Value = isRIM;
        command.Parameters.Add("@InvoiceType", SqlDbType.Int).Value = InvoiceType;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;

        command.Parameters.Add("@ConsigneeGSTIN", SqlDbType.NVarChar).Value = ConsigneeGSTIN;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@ConsigneeCode", SqlDbType.NVarChar).Value = ConsigneeCode;
        command.Parameters.Add("@ConsigneePAN", SqlDbType.NVarChar).Value = ConsigneePAN;

        command.Parameters.Add("@IsInterest", SqlDbType.Bit).Value = IsInterest;
        command.Parameters.Add("@InterestAmount", SqlDbType.Decimal).Value = InterestAmount;
        command.Parameters.Add("@IsAdvanceReceived", SqlDbType.Bit).Value = IsAdvanceReceived;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;

        if (PaymentRequiredDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentRequiredDate", SqlDbType.DateTime).Value = PaymentRequiredDate;

        if (PaymentDueDate != DateTime.MinValue)
            command.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = PaymentDueDate;

        command.Parameters.Add("@VendorGSTNType", SqlDbType.Int).Value = VendorGSTNType;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@VendorGSTNo", SqlDbType.NVarChar).Value = VendorGSTNo;
        command.Parameters.Add("@VendorPAN", SqlDbType.NVarChar).Value = VendorPAN;


        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = InvoicexAmount;
        command.Parameters.Add("@InvoiceTaxableAmount", SqlDbType.Decimal).Value = decTaxableAmount;
        command.Parameters.Add("@InvoiceGSTAmount", SqlDbType.Decimal).Value = decGSTAmount;

        command.Parameters.Add("@InvoiceCurrencyId", SqlDbType.Decimal).Value = InvoiceCurrencyId;
        command.Parameters.Add("@InvoiceCurrencyAmt", SqlDbType.Decimal).Value = decInvoiceCurrencyAmt;
        command.Parameters.Add("@InvoiceCurrencyExchangeRate", SqlDbType.Decimal).Value = decInvoiceCurrencyExchangeRate;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = strPaymentTerms;
        command.Parameters.Add("@IsImmediatePayment", SqlDbType.Bit).Value = IsImmediatePayment;
        command.Parameters.Add("@InvoiceMode", SqlDbType.Int).Value = InvoiceMode;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }



    public static int AddFinalInvoice(int ProformaInvoiceId, bool isRIM, string ConsigneeGSTIN, string ConsigneeName, string ConsigneeCode,
        string ConsigneePAN, string InvoiceNo, DateTime InvoiceDate, Decimal InvoicexAmount, Decimal decTaxableAmount, Decimal decGSTAmount,
        string strFinalInvoiceFilePath, string strRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ProformaInvoiceId", SqlDbType.Int).Value = ProformaInvoiceId;
        command.Parameters.Add("@IsRim", SqlDbType.Bit).Value = isRIM;

        command.Parameters.Add("@ConsigneeGSTIN", SqlDbType.NVarChar).Value = ConsigneeGSTIN;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@ConsigneeCode", SqlDbType.NVarChar).Value = ConsigneeCode;
        command.Parameters.Add("@ConsigneePAN", SqlDbType.NVarChar).Value = ConsigneePAN;

        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = InvoicexAmount;
        command.Parameters.Add("@InvoiceTaxableAmount", SqlDbType.Decimal).Value = decTaxableAmount;
        command.Parameters.Add("@InvoiceGSTAmount", SqlDbType.Decimal).Value = decGSTAmount;

        command.Parameters.Add("@InvoiceFilePath", SqlDbType.NVarChar).Value = strFinalInvoiceFilePath;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceFinal", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceItem(int InvoiceId, int PaymentId, string ChargeName, string ChargeCode,
       string HSN, decimal Amount, decimal TaxAmount, decimal IGSTRate, decimal CGSTRate, decimal SGSTRate,
        decimal IGSTAmount, decimal CGSTAmount, decimal SGSTAmount, decimal OtherDeduction, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@ChargeName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@ChargeCode", SqlDbType.NVarChar).Value = ChargeCode;

        command.Parameters.Add("@HSN", SqlDbType.NVarChar).Value = HSN;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = TaxAmount;
        command.Parameters.Add("@IGSTRate", SqlDbType.Decimal).Value = IGSTRate;
        command.Parameters.Add("@CGSTRate", SqlDbType.Decimal).Value = CGSTRate;
        command.Parameters.Add("@SGSTRate", SqlDbType.Decimal).Value = SGSTRate;

        command.Parameters.Add("@IGSTAmount", SqlDbType.Decimal).Value = IGSTAmount;
        command.Parameters.Add("@CGSTAmount", SqlDbType.Decimal).Value = CGSTAmount;
        command.Parameters.Add("@SGSTAmount", SqlDbType.Decimal).Value = SGSTAmount;
        command.Parameters.Add("@OtherDeduction", SqlDbType.Decimal).Value = OtherDeduction;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceItem", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateInvoiceItem(int ItemId, string ChargeName, string ChargeCode,
        string HSN, decimal Amount, decimal TaxAmount, decimal IGSTRate, decimal CGSTRate, decimal SGSTRate,
        decimal IGSTAmount, decimal CGSTAmount, decimal SGSTAmount, decimal OtherDeduction, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
        command.Parameters.Add("@ChargeName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@ChargeCode", SqlDbType.NVarChar).Value = ChargeCode;

        command.Parameters.Add("@HSN", SqlDbType.NVarChar).Value = HSN;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = TaxAmount;
        command.Parameters.Add("@IGSTRate", SqlDbType.Decimal).Value = IGSTRate;
        command.Parameters.Add("@CGSTRate", SqlDbType.Decimal).Value = CGSTRate;
        command.Parameters.Add("@SGSTRate", SqlDbType.Decimal).Value = SGSTRate;

        command.Parameters.Add("@IGSTAmount", SqlDbType.Decimal).Value = IGSTAmount;
        command.Parameters.Add("@CGSTAmount", SqlDbType.Decimal).Value = CGSTAmount;
        command.Parameters.Add("@SGSTAmount", SqlDbType.Decimal).Value = SGSTAmount;
        command.Parameters.Add("@OtherDeduction", SqlDbType.Decimal).Value = OtherDeduction;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updInvoiceItem", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceDocument(int InvoiceId, int DocumentId, string FilePath, string FileName,int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = FilePath;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceJobProfit(int InvoiceId, decimal VendorBuyValue, decimal VendorSellValue,
    decimal CustomerBuyValue, decimal CustomerSellValue, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@VendorBuyValue", SqlDbType.Decimal).Value = VendorBuyValue;
        command.Parameters.Add("@VendorSellValue", SqlDbType.Decimal).Value = VendorSellValue;
        command.Parameters.Add("@CustomerBuyValue", SqlDbType.Decimal).Value = CustomerBuyValue;

        command.Parameters.Add("@CustomerSellValue", SqlDbType.Decimal).Value = CustomerSellValue;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insVendorBuyDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetInvoiceJobProfit(int InvoiceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;

        return CDatabase.GetDataSet("INV_GetVendorBuyDetail", command);
    }

    public static int DeleteInvoiceItem(int ItemId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_delInvoiceItem", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteInvoiceItemByInvoiceID(int InvoiceId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_delInvoiceItemByInvoiceId", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetInvoiceItemById(int ItemId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;

        return CDatabase.GetDataSet("AC_GetInvoiceItemById", command);
    }

public static DataSet GetInvoiceItem(int InvoiceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;

        return CDatabase.GetDataSet("AC_GetInvoiceItem", command);
    }
    public static int AddInvoiceStatus(int InvoiceId, int StatusId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@sRemark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int CheckInvoiceStatusChange(int InvoiceId, int PrevStatusId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = PrevStatusId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_CheckInvoiceStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceAudit(int InvoiceId, int TransactionType, Boolean NoITC, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@TransactionTypeId", SqlDbType.Int).Value = TransactionType;
        command.Parameters.Add("@IsNoITC", SqlDbType.Bit).Value = NoITC;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceAudit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceAudit(int InvoiceId, int TDSExemptReasonID, int TransactionType, Boolean NoITC, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@TransactionTypeId", SqlDbType.Int).Value = TransactionType;
        command.Parameters.Add("@TDSExemptReasonID", SqlDbType.Int).Value = TDSExemptReasonID;
        command.Parameters.Add("@IsNoITC", SqlDbType.Bit).Value = NoITC;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceAudit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceTDS(int InvoiceId, bool tdsApplicable, string strTDSLedgerCode, int TDSRateTpe, decimal decTDSRate, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@TDSApplicable", SqlDbType.Bit).Value = tdsApplicable;
        command.Parameters.Add("@TDSLedgerCode", SqlDbType.NVarChar).Value = strTDSLedgerCode;
        command.Parameters.Add("@TDSRateType", SqlDbType.Int).Value = TDSRateTpe;
        command.Parameters.Add("@TDSRate", SqlDbType.Decimal).Value = decTDSRate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceTDS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateInvoiceTDS(int InvoiceId, int ItemId, bool tdsApplicable, string strTDSLedgerCode, int TDSRateTpe, decimal decTDSRate, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
        command.Parameters.Add("@TDSApplicable", SqlDbType.Bit).Value = tdsApplicable;
        command.Parameters.Add("@TDSLedgerCode", SqlDbType.NVarChar).Value = strTDSLedgerCode;
        command.Parameters.Add("@TDSRateType", SqlDbType.Int).Value = TDSRateTpe;
        command.Parameters.Add("@TDSRate", SqlDbType.Decimal).Value = decTDSRate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updInvoiceTDSByID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteInvoiceTDS(int ItemId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_delInvoiceTDSByID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceRCM(int InvoiceId, bool rcmApplicable, decimal decRCMRate, int RCMGstType, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@RCMApplicable", SqlDbType.Bit).Value = rcmApplicable;
        command.Parameters.Add("@RCMRate", SqlDbType.Decimal).Value = decRCMRate;
        command.Parameters.Add("@RCMGstType", SqlDbType.Int).Value = RCMGstType;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoiceRCM", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateInvoiceRCM(int InvoiceId, int ItemId, bool rcmApplicable, decimal decRCMRate, int RCMGstType, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
        command.Parameters.Add("@RCMApplicable", SqlDbType.Bit).Value = rcmApplicable;
        command.Parameters.Add("@RCMRate", SqlDbType.Decimal).Value = decRCMRate;
        command.Parameters.Add("@RCMGstType", SqlDbType.Int).Value = RCMGstType;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updInvoiceRCMById", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoicePaymentRequest(int InvoiceId, Boolean IsFullPayment, int PaymentTypeId, int BankId,string strBankName, Decimal PaidAmount,
        int PaymentCurrencyId, decimal decPaymentCurrencyRate, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@IsFullPayment", SqlDbType.Int).Value = IsFullPayment;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;
        command.Parameters.Add("@BankId", SqlDbType.NVarChar).Value = BankId;
	command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = strBankName;
        command.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = PaidAmount;

        command.Parameters.Add("@PaymentCurrencyId", SqlDbType.Int).Value = PaymentCurrencyId;
        command.Parameters.Add("@PaymentCurrencyRate", SqlDbType.Decimal).Value = decPaymentCurrencyRate;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoicePaymentRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

public static int AddInvoicePaymentRequest2(int InvoiceId, Boolean IsFullPayment, int PaymentTypeId, int BabajiBankId, int BankAccountId, 
       bool IsFundTransFromAPI, int VendorBankAccountId, Decimal PaidAmount, int PaymentCurrencyId, decimal decPaymentCurrencyRate, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@IsFullPayment", SqlDbType.Int).Value = IsFullPayment;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;
        command.Parameters.Add("@BabajiBankId", SqlDbType.NVarChar).Value = BabajiBankId;
        command.Parameters.Add("@BankAccountId", SqlDbType.NVarChar).Value = BankAccountId;
        command.Parameters.Add("@IsFundTransFromAPI", SqlDbType.Bit).Value = IsFundTransFromAPI;
        command.Parameters.Add("@VendorBankAccountId", SqlDbType.NVarChar).Value = VendorBankAccountId;
        command.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = PaidAmount;

        command.Parameters.Add("@PaymentCurrencyId", SqlDbType.Int).Value = PaymentCurrencyId;
        command.Parameters.Add("@PaymentCurrencyRate", SqlDbType.Decimal).Value = decPaymentCurrencyRate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoicePaymentRequest2", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int AddInvoicePayment(int InvoiceId, int PaymentId, string InstrumentNo, DateTime InstrumentDate,
        Decimal PaidAmount, string DocPath, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@InstrumentNo", SqlDbType.NVarChar).Value = InstrumentNo;

        if (InstrumentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InstrumentDate", SqlDbType.DateTime).Value = InstrumentDate;
        }
        command.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = PaidAmount;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insInvoicePayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPaymentAPIResponse(int InvoiceId, int attemptNo, string reqTransferType, string requestReferenceNo,
        string statusCode, string subStatusCode, string subStatusText, string uniqueResponseNo, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PaymentRequestId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@attemptNo", SqlDbType.Int).Value = attemptNo;
        command.Parameters.Add("@reqTransferType", SqlDbType.NVarChar).Value = reqTransferType;
        command.Parameters.Add("@requestReferenceNo", SqlDbType.NVarChar).Value = requestReferenceNo;
        command.Parameters.Add("@statusCode", SqlDbType.NVarChar).Value = statusCode;

        command.Parameters.Add("@subStatusCode", SqlDbType.NVarChar).Value = subStatusCode;
        command.Parameters.Add("@subStatusText", SqlDbType.NVarChar).Value = subStatusText;
        command.Parameters.Add("@uniqueResponseNo", SqlDbType.NVarChar).Value = uniqueResponseNo;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insAPIBankTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFailedPaymentToBankTransfer(int InvoiceId, int PaymentId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updAPIBankFailedTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int CheckActiveTransaction(int InvoiceId, int PaymentId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_checkAPIActiveTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetInvoiceDocument(int InvoiceID, int DocumentId)
    {
        // Get All invoice Document Set DocumentId = 0
        // Get Specific Document - Provide DocumentID - AC_InvoiceDocument Table Promery Key - lid
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;

        return CDatabase.GetDataSet("AC_GetInvoiceDocument", command);
    }

    public static DataSet GetInvoiceJobPrevHistory(int InvoiceID)
    {
        // Get Job Payment History Except Current Job
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;

        return CDatabase.GetDataSet("AC_GetInvoiceJobPrevHistory", command);
    }

    public static DataSet GetInvoicePendingPayment(int InvoiceID)
    {
        // Get Active Pending Payment for Instrument No & Date Update
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;

        return CDatabase.GetDataSet("AC_GetPendingInvoicePayment", command);
    }
    public static DataSet GetInvoicePayment(int InvoiceID)
    {
        // Get All Payment
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;

        return CDatabase.GetDataSet("AC_GetInvoicePayment", command);
    }

    public static DataSet GetInvoicePaymentDetail(int PaymentID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = PaymentID;

        return CDatabase.GetDataSet("AC_GetInvoicePaymentById", command);
    }

    public static DataSet GetInvoiceDetail(int InvoiceID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;

        return CDatabase.GetDataSet("AC_GetInvoiceById", command);
    }
    public static DataSet GetInvoiceDetailForPayment(int InvoiceID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;

        return CDatabase.GetDataSet("AC_GetInvoiceForPaymentById", command);
    }
    public static DataSet GetFAVendorByCode(string strVendorCode)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = strVendorCode;

        return CDatabase.GetDataSet("BJV_GetVendorByCode", command);
    }

    public static DataSet GetFAVendorByGSTIN(string strVendorGSTIN)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@VendorGSTIN", SqlDbType.NVarChar).Value = strVendorGSTIN;

        return CDatabase.GetDataSet("BJV_GetVendorByCode", command);
    }

    public static void FillBankMS(DropDownList DropDown, int BankType)
    {
        // 0 -- All Bank
        // 1 -- Babaji Bank List

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BankType", SqlDbType.NVarChar).Value = BankType;

        CDatabase.BindControls(DropDown, "BJV_GetBankMS", command, "sName", "lid");
    }

    public static void FillBankAccountByBankId(DropDownList DropDown, int BankId)
    {
        // Get FA- Bank Account By Bank

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BankId", SqlDbType.Int).Value = BankId;

        CDatabase.BindControls(DropDown, "AC_GetBankBookMSByBankId", command, "DisplayName", "lid");
    }
    public static void FillBankBookByType(DropDownList DropDown, int BankType)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BankType", SqlDbType.NVarChar).Value = BankType;

        CDatabase.BindControls(DropDown, "AC_GetBankBookMS", command, "DisplayName", "lid");
    }
    public static void FillAPIBankAccount(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "AC_GetAPIBankAccount", command, "AccountNo", "AccountID");
    }
    public static DataSet GetAPIBankAccount()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("AC_GetAPIBankAccount", command);
    }
    public static void FillVendorBank(DropDownList DropDown, int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;

        CDatabase.BindControls(DropDown, "BJV_GetVendorBankDetail", command, "DisplayName", "lid");
    }

    public static DataSet GetVendorBankDetail(int VendorBankId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lID", SqlDbType.NVarChar).Value = VendorBankId;

        return CDatabase.GetDataSet("BJV_GetVendorBankDetailById", command);
    }
    public static DataSet GetAPIDetailByAccountID(int BankAccountID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BankAccountID", SqlDbType.NVarChar).Value = BankAccountID;

        return CDatabase.GetDataSet("AC_GetAPIBankByAccountId", command);
    }
    public static DataSet GetAPIBankStatementByChequeNo(int ChequeNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ChequeNO", SqlDbType.Int).Value = ChequeNo;

        return CDatabase.GetDataSet("AC_SearchBankStatement", command);
    }
    public static int SendPaymentEmail(int InvoiceId, int PaymentId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_MailPaymentDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #region Cheque Book Entry

    #region Cheque Book Entry

    public static int AddBankChequeBook(int BankId, int BranchId, int AccountId, int StartChequeNo, int EndChequeNo, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BankId", SqlDbType.Int).Value = BankId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AccountId", SqlDbType.Int).Value = AccountId;
        command.Parameters.Add("@StartChequeNo", SqlDbType.Int).Value = StartChequeNo;
        command.Parameters.Add("@EndChequeNo", SqlDbType.Int).Value = EndChequeNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insBankChequeMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddBankChequeJobNo(string JobRefNo, string ChequeNo, DateTime dtChequeDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = dtChequeDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insBankChequeJobNo", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateBankChequeInvoiceNo(int ChequeId, Decimal ChequeAmount, int InvoiceId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ChequeId", SqlDbType.Int).Value = ChequeId;
        command.Parameters.Add("@ChequeAmount", SqlDbType.Decimal).Value = ChequeAmount;
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insBankChequeInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int CancelBankCheque(int ChequeId, int ChequeNo, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ChequeId", SqlDbType.Int).Value = ChequeId;
        command.Parameters.Add("@ChequeNo", SqlDbType.Int).Value = ChequeNo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insBankChequeCancel", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetIssuedCheckDetail(int ChequeId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ChequeId", SqlDbType.Int).Value = @ChequeId;

        return CDatabase.GetDataSet("AC_GetIssuedChequebyId", command);
    }

    #endregion

    #endregion

    #region Open BANK API

    public static int AddBankPaymentAPIRequest(int BankId, int PaymentRequestId, string ReqReferenceNo, string Amount, string Currency,
    string CreditorAccountIFSC, string CreditorAccountNo, string CreditorAccountName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BankId", SqlDbType.Int).Value = BankId;
        command.Parameters.Add("@PaymentRequestId", SqlDbType.Int).Value = PaymentRequestId;
        command.Parameters.Add("@ReqReferenceNo", SqlDbType.NVarChar).Value = ReqReferenceNo;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;

        command.Parameters.Add("@CreditorAccountIFSC", SqlDbType.NVarChar).Value = CreditorAccountIFSC;
        command.Parameters.Add("@CreditorAccountNo", SqlDbType.NVarChar).Value = CreditorAccountNo;
        command.Parameters.Add("@CreditorAccountName", SqlDbType.NVarChar).Value = CreditorAccountName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insAPIBankTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransBankPaymentAPIRequest(int BankId, int PaymentId, string ReqReferenceNo, string Amount, string Currency,
    string CreditorAccountIFSC, string CreditorAccountNo, string CreditorAccountName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BankId", SqlDbType.Int).Value = BankId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@ReqReferenceNo", SqlDbType.NVarChar).Value = ReqReferenceNo;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;

        command.Parameters.Add("@CreditorAccountIFSC", SqlDbType.NVarChar).Value = CreditorAccountIFSC;
        command.Parameters.Add("@CreditorAccountNo", SqlDbType.NVarChar).Value = CreditorAccountNo;
        command.Parameters.Add("@CreditorAccountName", SqlDbType.NVarChar).Value = CreditorAccountName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insAPIBankTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateBankPaymentAPIResponse(string ReqReferenceNo, string RespReferenceNo, string UniqueReferenceNo,
        string RespStatus, DateTime RespDate, int IsSuccess, int StatusId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReqReferenceNo", SqlDbType.NVarChar).Value = ReqReferenceNo;
        command.Parameters.Add("@RespReferenceNo", SqlDbType.NVarChar).Value = RespReferenceNo;
        command.Parameters.Add("@UniqueReferenceNo", SqlDbType.NVarChar).Value = UniqueReferenceNo;

        command.Parameters.Add("@RespStatus", SqlDbType.NVarChar).Value = RespStatus;
        
        if (RespDate == DateTime.MinValue)
        {
            command.Parameters.Add("@RespDate", SqlDbType.DateTime).Value = DateTime.Now;
        }
        else
        {
            command.Parameters.Add("@RespDate", SqlDbType.DateTime).Value = RespDate;
        }
        
        if (IsSuccess == 0)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = false;
        }
        else if (IsSuccess == 1)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = true;
        }

        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updAPIBankTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTransBankPaymentAPIResponse(string ReqReferenceNo, string RespReferenceNo, string UniqueReferenceNo,
        string RespStatus, DateTime RespDate, int IsSuccess, int StatusId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReqReferenceNo", SqlDbType.NVarChar).Value = ReqReferenceNo;
        command.Parameters.Add("@RespReferenceNo", SqlDbType.NVarChar).Value = RespReferenceNo;
        command.Parameters.Add("@UniqueReferenceNo", SqlDbType.NVarChar).Value = UniqueReferenceNo;

        command.Parameters.Add("@RespStatus", SqlDbType.NVarChar).Value = RespStatus;
        command.Parameters.Add("@RespDate", SqlDbType.DateTime).Value = RespDate;

        if (IsSuccess == 0)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = false;
        }
        else if (IsSuccess == 1)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = true;
        }

        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updAPIBankTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddBankPaymentAPIError(int TransactionId, string strErrorCode, string strErrorId, string strErrorMessage,
        string strErrorActionCode, string strErrorActionDescription, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@TransactionId", SqlDbType.Int).Value = TransactionId;
        command.Parameters.Add("@ErrorCode", SqlDbType.NVarChar).Value = strErrorCode;
        command.Parameters.Add("@ErrorId", SqlDbType.NVarChar).Value = strErrorId;
        command.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar).Value = strErrorMessage;
        command.Parameters.Add("@ErrorActionCode", SqlDbType.NVarChar).Value = strErrorActionCode;

        command.Parameters.Add("@ErrorActionDescription", SqlDbType.NVarChar).Value = strErrorActionDescription;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insAPIBankTransactionError", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddTransBankPaymentAPIError(int TransactionId, string strErrorCode, string strErrorId, string strErrorMessage,
        string strErrorActionCode, string strErrorActionDescription, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@TransactionId", SqlDbType.Int).Value = TransactionId;
        command.Parameters.Add("@ErrorCode", SqlDbType.NVarChar).Value = strErrorCode;
        command.Parameters.Add("@ErrorId", SqlDbType.NVarChar).Value = strErrorId;
        command.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar).Value = strErrorMessage;
        command.Parameters.Add("@ErrorActionCode", SqlDbType.NVarChar).Value = strErrorActionCode;

        command.Parameters.Add("@ErrorActionDescription", SqlDbType.NVarChar).Value = strErrorActionDescription;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insAPIBankTransactionError", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBankPendingTransaction()
    {
        SqlCommand command = new SqlCommand();

        DataSet dsResult = CDatabase.GetDataSet("AC_GetPendingBankTransaction", command);

        return dsResult;
    }
    public static DataSet GetBankPendingTransactionByID(int PaymentID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentID;

        DataSet dsResult = CDatabase.GetDataSet("AC_GetPendingBankTransactionByID", command);

        return dsResult;
    }

    public static DataSet GetAPIBankTransactionById(int TransactionID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@TransactionID", SqlDbType.Int).Value = TransactionID;

        DataSet dsResult = CDatabase.GetDataSet("AC_GetAPIBankTransactionById", command);
             
        return dsResult;
    }
    public static DataSet GetAPIBankTransactionByRequestNo(string ReqReferenceNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReqReferenceNo", SqlDbType.NVarChar).Value = ReqReferenceNo;

        DataSet dsResult = CDatabase.GetDataSet("AC_GetAPIBankTransactionById", command);

        return dsResult;
    }
    public static int UpdateBankPaymentUTRNo(int PaymentId, string UniqueReferenceNo, DateTime RespDate)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = @PaymentId;
        command.Parameters.Add("@InstrumentNo", SqlDbType.NVarChar).Value = UniqueReferenceNo;
        
        if (RespDate == DateTime.MinValue)
        {
            command.Parameters.Add("@InstrumentDate", SqlDbType.DateTime).Value = DateTime.Now;
        }
        else
        {
            command.Parameters.Add("@InstrumentDate", SqlDbType.DateTime).Value = RespDate;
        }

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insAPIInvoicePaymentUTR", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateBankPaymentPushNotify(string ReqReferenceNo, string BankReferenceNumber, string RespStatus, DateTime RespDate, int IsSuccess, int StatusId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReqReferenceNo", SqlDbType.NVarChar).Value = ReqReferenceNo;

        command.Parameters.Add("@RespStatus", SqlDbType.NVarChar).Value = RespStatus;
        
        if (RespDate == DateTime.MinValue)
        {
            command.Parameters.Add("@RespDate", SqlDbType.DateTime).Value = DateTime.Now;
        }
        else
        {
            command.Parameters.Add("@RespDate", SqlDbType.DateTime).Value = RespDate;
        }

        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        if (BankReferenceNumber != "" && BankReferenceNumber != null)
        {
            command.Parameters.Add("@BankReferenceNumber", SqlDbType.NVarChar).Value = BankReferenceNumber;
        }

        if (IsSuccess != -1)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = IsSuccess;
        }

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updAPIBankTransactionPush", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int UpdateBankPaymentMailStatus(int PaymentId, bool isMailSent)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@MailStatus", SqlDbType.Bit).Value = isMailSent;

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insAPIInvoicePaymentEmail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion
    #endregion

    #region Labour Expense
    public static DataSet GetALabourExpByJobID(int JobID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;

        return CDatabase.GetDataSet("GetDailyALabourExpByJobID", command);
    }
    public static DataSet GetLabourExpenseForExcel(DateTime ExpenseDate)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value = ExpenseDate;

        DataSet dsBillDispatch = CDatabase.GetDataSet("GetLabourExpExcel", command);

        return dsBillDispatch;
    }
    public static int ApproveLabourExpByJobID(int JobID, string SystemRef, DateTime ExpenseDate, int StatusID, string sRemark, int UserID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        command.Parameters.Add("@SystemRef", SqlDbType.NVarChar).Value = SystemRef;
        command.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value = ExpenseDate;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = StatusID;
        command.Parameters.Add("@sRemark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        string SPresult = CDatabase.GetSPOutPut("insApproveLabourExpByJobID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int FAPostLabourExp()
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        string SPresult = CDatabase.GetSPOutPut("MIG_BJVRunAutoPostExpenseExe", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int FAPostAdditionalExp()
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        string SPresult = CDatabase.GetSPOutPut("MIG_BJVAutoExpenseAddtPost", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    #endregion

    #region Invoice Memo
    public static int AddInvoicePayMemo(int VendorId, decimal TotalMemoAmount, int TotalRequest, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@TotalRequest", SqlDbType.Int).Value = TotalRequest;
        command.Parameters.Add("@TotalMemoAmount", SqlDbType.Int).Value = TotalMemoAmount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemo", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoicePayMemo(int VendorId, decimal TotalMemoAmount, int TotalRequest, Boolean isAPIPayment, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@TotalRequest", SqlDbType.Int).Value = TotalRequest;
        command.Parameters.Add("@isAPIPayment", SqlDbType.Bit).Value = isAPIPayment;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@TotalMemoAmount", SqlDbType.Int).Value = TotalMemoAmount;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemo", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceMemoStatus(int InvoiceMemoId, int StatusId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoId", SqlDbType.Int).Value = InvoiceMemoId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insMemoStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceMemoCancel(int InvoiceMemoId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoId", SqlDbType.Int).Value = InvoiceMemoId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoCancel", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceMemoDetailRemittance(int InvoiceId, int PaymentId, int InvoiceMemoID, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoRemittance", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddInvoiceMemoDetail(int InvoiceId, int PaymentId, int InvoiceMemoID, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddInvoiceMemoAudit(int InvoiceMemoID, int AuditStatus, int InvoiceStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@AuditStatus", SqlDbType.Int).Value = AuditStatus;
        command.Parameters.Add("@InvoiceStatus", SqlDbType.Int).Value = InvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoAudit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceMemoApproval(int InvoiceMemoID, int ApprovalStatus, int InvoiceStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = ApprovalStatus;
        command.Parameters.Add("@InvoiceStatus", SqlDbType.Int).Value = InvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoAppoval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddInvoiceMemoPayment(int InvoiceMemoID, int MemoPaymentStatus, int InvoiceStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@MemoPaymentStatus", SqlDbType.Int).Value = MemoPaymentStatus;
        command.Parameters.Add("@InvoiceStatus", SqlDbType.Int).Value = InvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoPayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddInvoiceMemoCancelBatchPayment(int InvoiceMemoID, int UserId)
    {
        // Cancel Batch Payment
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insCancelMemoPayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceMemoPayBacth(int InvoiceMemoId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoId", SqlDbType.Int).Value = InvoiceMemoId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoBatchPay", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceMemoProfit(int InvoiceMemoID, string Remark, string FilePath, string FileName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@InvoiceMemoID", SqlDbType.Int).Value = InvoiceMemoID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = FilePath;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceMemoProfit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetInvoiceMemoDetail(int InvoiceMemoId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceMemoId", SqlDbType.Int).Value = InvoiceMemoId;

        return CDatabase.GetDataSet("INV_GetInvoiceMemoById", command);

    }

    public static int ValidateVendorInvoiceMemoExpense(int MemoID, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BJV_ValidateJobExpenseByMemoID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion
}