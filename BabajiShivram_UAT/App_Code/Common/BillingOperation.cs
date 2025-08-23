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
/// Summary description for BillingOperation
/// </summary>
public class BillingOperation
{
    public BillingOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Start Billing

    #region Dahsboard Billing
    public static int GetRole(int UserId, int roleid)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@RoleId", SqlDbType.Int).Value = roleid;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("GetRole", command, "@outVal");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTime(int CustwiseId, int branchwiseId, int UserwiseId, int UserId, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustWiseId", SqlDbType.Int).Value = CustwiseId;
        command.Parameters.Add("@BranchwiseId", SqlDbType.Int).Value = branchwiseId;
        command.Parameters.Add("@UserwiseId", SqlDbType.Int).Value = UserwiseId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("GetBillTime", command);

        return dsReport;
    }

    public static DataSet GetTop10ClientTime(int CustwiseId, int branchwiseId, int UserwiseId, int UserId, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustWiseId", SqlDbType.Int).Value = CustwiseId;
        command.Parameters.Add("@BranchwiseId", SqlDbType.Int).Value = branchwiseId;
        command.Parameters.Add("@UserwiseId", SqlDbType.Int).Value = UserwiseId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("GetTop10ClientTime", command);

        return dsReport;
    }

    public static DataSet GetChartClient(int UserId, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopClient", command);

        return dsReport;
    }

    public static DataSet GetChartAging1(int CustwiseId, int branchwiseId, int UserwiseId, int UserId, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustWiseId", SqlDbType.Int).Value = CustwiseId;
        command.Parameters.Add("@BranchwiseId", SqlDbType.Int).Value = branchwiseId;
        command.Parameters.Add("@UserwiseId", SqlDbType.Int).Value = UserwiseId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopAging1", command);

        return dsReport;
    }

    public static DataSet GetChartAging2(int CustwiseId, int branchwiseId, int UserwiseId, int UserId, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustWiseId", SqlDbType.Int).Value = CustwiseId;
        command.Parameters.Add("@BranchwiseId", SqlDbType.Int).Value = branchwiseId;
        command.Parameters.Add("@UserwiseId", SqlDbType.Int).Value = UserwiseId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopAging2", command);

        return dsReport;
    }

    public static DataSet GetChartAging3(int CustwiseId, int branchwiseId, int UserwiseId, int UserId, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustWiseId", SqlDbType.Int).Value = CustwiseId;
        command.Parameters.Add("@BranchwiseId", SqlDbType.Int).Value = branchwiseId;
        command.Parameters.Add("@UserwiseId", SqlDbType.Int).Value = UserwiseId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopAging3", command);

        return dsReport;
    }

    #endregion

    public static DataSet GetPendingBiilingCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("GetPendingBillingCountForUser", command);

        return dsPending;
    }
    public static int Addbranchforonlinebill(int Customerid, int BranchId, int CreatedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Customerid", SqlDbType.Int).Value = Customerid;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerBranchonlineBill", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddUrgentbill(int CustomerId, int Urgentbill, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@Urgentbill", SqlDbType.Int).Value = Urgentbill;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insCustomerUrgentbill", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddCustomerquickmaster(int CustomerId, int quickpaymaster, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@quickpaymaster", SqlDbType.Int).Value = quickpaymaster;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insCustomerQuickmaster", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerCreditDays(int CustomerId, string strcreditdays, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@CreditDays", SqlDbType.NVarChar).Value = strcreditdays;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerCreditDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int DeleteCustomerCreditDays(int CustomerId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomercreditdays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateCustomercreditdays(int CreditDaysid, int CustomerId, string CreditDays, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CreditDaysid", SqlDbType.Int).Value = CreditDaysid;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@CreditDays", SqlDbType.NVarChar).Value = CreditDays;

        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomercreditdays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerWeekDays(int CustomerId, int weeks, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@Weeks", SqlDbType.Int).Value = weeks;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerBillingWeekDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int DeleteCustomerWeekDays(int CustomerId, int weeks, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@weeks", SqlDbType.Int).Value = weeks;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomerBillingWeekDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateCustomerweekdays(int CustomerId, int weeks, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";


        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@weeks", SqlDbType.Int).Value = weeks;

        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomerBillingWeekDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerDays(int CustomerId, int Days, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@Days", SqlDbType.Int).Value = Days;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerBillingDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int DeleteCustomerDays(int CustomerId, int Days, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@Days", SqlDbType.Int).Value = Days;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomerBillingDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateCustomerdays(int CustomerId, int Days, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";


        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@Days", SqlDbType.Int).Value = Days;

        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomerBillingDays", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int insbillingrejectedcomplete(int JobId, int lUser, int Rejectid, int rejectedby, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@Rejectid", SqlDbType.Int).Value = Rejectid;
        command.Parameters.Add("@rejectedby", SqlDbType.Int).Value = rejectedby;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insRejPCD", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int manualmailnotification(int JobId, int lUser, int rejectedby, int FinYearId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@rejectedby", SqlDbType.NVarChar).Value = rejectedby;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("mailBillRejection", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectScrutiny(int JobId, bool IsApproved, string Remark, int reasonforPendency, string LRDCType, int CategoryId, int lUser, int ModuleId)  //ADDED BY 31032020
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDBillingScrutinyApproval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int Rejectmailnotification(int JobId, int lUser, int rejectedby, int FinYearId, string Mailtoid)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@rejectedby", SqlDbType.NVarChar).Value = rejectedby;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@Mailtoid", SqlDbType.NVarChar).Value = Mailtoid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("MailBillRejectionAll", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int RejectDrafting(int JobId, bool IsApproved, string Remark, int reasonforPendency, string LRDCType, int CategoryId, int lUser, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDDraftInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DraftInvoicejobmovetoDraftcheck(int JobId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insjobmovedraftcheck", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int FinalTypingjobmovetoFinalcheck(int JobId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insjobmoveFinaldraftCheck", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int ApprovebyFADrafting(int JobId, bool IsApproved, string Remark, int lUser, int reasonforPendency, string LRDCType)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDDraftInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectChecking(int JobId, bool IsApproved, string Remark, int reasonforPendency, string LRDCType, string Correct, int CategoryId, int lUser, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@Correct", SqlDbType.NVarChar).Value = Correct;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDChecking", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int ApproveRejectTyping(int JobId, bool IsApproved, string Remark, int reasonforPendency, string LRDCType, int CategoryId, int lUser, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDTyping", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApprovebyFAFinalTyping(int JobId, bool IsApproved, string Remark, int lUser, int reasonforPendency, string LRDCType)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDTyping", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectFinalChecking(int JobId, bool IsApproved, string Remark, int reasonforPendency, string Correct, string LRDCType, int CategoryId, int lUser, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@Correct", SqlDbType.NVarChar).Value = Correct;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDFinalChecking", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectDispatching(int JobId, bool IsApproved, string Remark, int reasonforPendency, string LRDCType, int CategoryId, int lUser, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDDispatching", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int updateBillingEBill(int JobId, DateTime EBillDate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (EBillDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EBillDate", SqlDbType.DateTime).Value = EBillDate;
        }

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDDispatchingEBill", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillingEBill(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetPCDDispatchingEBill", command);
    }
    public static int updatebillingpriority(int lid, int lorder, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lOrder", SqlDbType.Int).Value = lorder;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updbillingPriority", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int receivedfile(int JobId, int lUser, int ReceivedBy, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@ReceivedBy", SqlDbType.Int).Value = ReceivedBy;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;    //ADDED BY 27032020
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insReceivedfile", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet FillBJVDetails(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetBJVDetails", command);
    }
    public static DataSet GetBJVJobStatus(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;

        return CDatabase.GetDataSet("GetBJVJobStatus", command);
    }
    public static int AddBJVAuditRemark(int JobId, int ModuleId, string strRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@AuditRemark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BJV_insJobStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int colorforfollowupdetaildone(int JobId, int Rejecttypeid)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Rejecttypeid", SqlDbType.Int).Value = Rejecttypeid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("Getfollowupdonedetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet FillPCDDocumentForRecieved(int JobId, int DocumentForType)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        return CDatabase.GetDataSet("GetPCDDocumentByRecieved", command);
    }

    public static DataSet FillPCDDocumentForonlineBill(int JobId, int Uploadtype)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Uploadtype", SqlDbType.Int).Value = Uploadtype;
        return CDatabase.GetDataSet("GetPCDDocumentonlineBill", command);
    }


    public static int consolidatedfile(int JobId, int lUser, string consolidatedjobs, int ReceivedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@ConsolidatedJobId", SqlDbType.NVarChar).Value = consolidatedjobs;
        command.Parameters.Add("@ReceivedBy", SqlDbType.Int).Value = ReceivedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insConsolidatedReceivedfile", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddScanPCDDocument(int JobId, int DocumentId, string Docpath, int lUser, int @uploadby)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        //command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;        
        if (Docpath != "")
            command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = Docpath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@uploadby", SqlDbType.Int).Value = uploadby;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDScanDoc", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet FillPCDDocumentForFinalTyping(int JobId, int DocumentForType)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        return CDatabase.GetDataSet("GetPCDDocumentForFinalTyping", command);
    }

    public static int freightJob(int JobId, int lUser, string FreightjobNo, int ReceivedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FreightjobNo", SqlDbType.NVarChar).Value = FreightjobNo;
        command.Parameters.Add("@ReceivedBy", SqlDbType.Int).Value = ReceivedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insFrieghtJob", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetFreightDetail(int Jobid)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@Jobid", SqlDbType.Int).Value = Jobid;

        return CDatabase.GetDataSet("GetFriehtjobno", command);
    }
    public static int AddCoverletterPath(int Customerid, string jobid, string filename, string masterInvoiceNo, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@Customerid", SqlDbType.Int).Value = Customerid;
        command.Parameters.Add("@jobid", SqlDbType.NVarChar).Value = jobid;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@masterInvoiceNo", SqlDbType.NVarChar).Value = masterInvoiceNo;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCoverletterPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet FillPCDScanDocumentByWorkFlow(int JobId, int TypeId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = TypeId;

        return CDatabase.GetDataSet("GetPCDScanDocumentByWorkflow", command);
    }

    public static int AddBillingStatus(int JobId, int StatusID, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = StatusID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBillStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int EX_DraftInvoicejobmovetoDraftcheck(int JobId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_insjobmovedraftcheck", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int EX_FinalTypingjobmovetoFinalcheck(int JobId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_insjobmoveFinaldraftCheck", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int TR_DraftInvoicejobmovetoDraftcheck(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insjobmovedraftcheck", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int TR_FinalTypingjobmovetoFinalcheck(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insjobmoveFinaldraftCheck", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    #region Report KPI
    public static DataSet GetBillingKPI(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIOverall", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIWithReject(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIWithReject", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIClearance(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIClearance", command);

        return dsReport;
    }

    public static DataSet GetBillingKPICustomer(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPICustomer", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIStage(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIStage", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIRejection(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIReject", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIRejectReason(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIRejectReason", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIRejectionCustomer(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIRejectCust", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIUserStage(string ReportMonth, int BillingStage)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BillingStage", SqlDbType.Int).Value = BillingStage;

        dsReport = CDatabase.GetDataSet("rptBillingKPIUserStage", command);

        return dsReport;
    }
    public static DataSet GetBillingKPIUserDelay(string ReportMonth)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;

        dsReport = CDatabase.GetDataSet("rptBillingKPIUserDelay", command);

        return dsReport;
    }

    public static DataSet GetBillingKPIUserError(string ReportMonth, int StageId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@StageId", SqlDbType.Int).Value = StageId;

        dsReport = CDatabase.GetDataSet("rptBillingKPIUserError", command);

        return dsReport;
    }

    public static DataSet GetBillingKPICustomerStage(string ReportMonth, int BillingStage)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BillingStage", SqlDbType.Int).Value = BillingStage;

        dsReport = CDatabase.GetDataSet("rptBillingKPICustomerStage", command);

        return dsReport;
    }
    #endregion

    #region Report MIS

    public static DataSet GetMISImport(int FinYear, int BranchId, int CustomerId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        dsReport = CDatabase.GetDataSet("rptMISImport", command);

        return dsReport;
    }

    public static DataSet GetMISImportVolume(int FinYear, int BranchId, int CustomerId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        dsReport = CDatabase.GetDataSet("rptMISImportVolume", command);

        return dsReport;
    }

    public static DataSet GetMISBilling(int FinYear, int BranchId, int CustomerId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        dsReport = CDatabase.GetDataSet("rptMISBilling", command);

        return dsReport;
    }


    public static DataSet GetMISBillingPerformance(int KPIDays, int BranchId, int CustomerId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@KPIDays", SqlDbType.Int).Value = KPIDays;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        if (CustomerId > 0)
        {
            command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        }

        dsReport = CDatabase.GetDataSet("rptMISBillingYear", command);

        return dsReport;
    }
    public static DataSet GetMISBillingUser(string ReportMonth, int StageId, int KPIDays, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@StageId", SqlDbType.Int).Value = StageId;
        command.Parameters.Add("@KPIDays", SqlDbType.Int).Value = KPIDays;
        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        dsReport = CDatabase.GetDataSet("rptMISBillingUser", command);

        return dsReport;
    }

    public static DataSet GetMISBillingIncentive(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISBillingIncentive", command);

        return dsReport;
    }

    public static DataSet GetMISBillingIncentiveUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISBillingIncentiveUser", command);

        return dsReport;
    }

    public static DataSet GetMISBillingScrutinyA(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISBillingIncentiveUser", command);

        return dsReport;
    }

    public static DataSet GetMISJobOpen(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISJobOpening", command);

        return dsReport;
    }

    public static DataSet GetMISJobOpenUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISJobOpeningUser", command);

        return dsReport;
    }

    public static DataSet GetMISIGM(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISIGM", command);

        return dsReport;
    }

    public static DataSet GetMISIGMUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISIGMUser", command);

        return dsReport;
    }
    public static DataSet GetMISChecklistPrepare(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISChecklist", command);

        return dsReport;
    }

    public static DataSet GetMISChecklistPrepareUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISChecklistUser", command);

        return dsReport;
    }
    public static DataSet GetMISChecklistAudit(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISChecklistAudit", command);

        return dsReport;
    }

    public static DataSet GetMISChecklistAuditUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISChecklistAuditUser", command);

        return dsReport;
    }
    public static DataSet GetMISDOCollection(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISDOCollection", command);

        return dsReport;
    }

    public static DataSet GetMISNoting(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISNoting", command);

        return dsReport;
    }

    public static DataSet GetMISDOUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISDOCollectionUser", command);

        return dsReport;
    }
    public static DataSet GetMISNotingUser(string ReportMonth, int BranchId)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportMonth;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        dsReport = CDatabase.GetDataSet("rptMISNotingUser", command);

        return dsReport;
    }


    public static DataSet GetMISFreightVolume(int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("rptMISFreightVolume", command);

        return dsReport;
    }

    public static DataSet GetMISFreightPerformance()
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        dsReport = CDatabase.GetDataSet("rptMISFreightYear", command);

        return dsReport;
    }
    #endregion
    #endregion End Billing

    public static int AD_ApproveRejectScrutiny(int JobId, bool IsApproved, string Remark, int lUser, int reasonforPendency, string LRDCType)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@reasonforPendency", SqlDbType.Int).Value = reasonforPendency;
        command.Parameters.Add("@LRDCType", SqlDbType.NVarChar).Value = LRDCType;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("AD_insPCDBillingScrutinyApproval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetCustomerInstruction(string CustomerId)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;        
        dsPending = CDatabase.GetDataSet("GetCustomerInstruction", command);

        return dsPending;
    }

    public static int AddCustomerInstruction(int CustomerId, string Instruction, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@Instruction", SqlDbType.NVarChar).Value = Instruction;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insCustomerInstruction", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }


    #region BJV FA System

    public static DataSet FAGetCurrentJOb()
    {
        DataSet dsCurrentJob;

        string strCommand = " declare @PendingBill Table(ref Nvarchar(50));" +
        "INSERT INTO @PendingBill(ref)" +
        "SELECT ref from bjvdet where ay = 2018 AND type<> 'CC' AND type<> 'Y1'" +
        " Except" +
        " select ref FROM bil_det where type<> 'Y1';" +

        " SELECT Ex.ref, B.par_code, B.vessal, B.yRef,B.odate,B.FSB,B.clrDate,SUM(Convert(Decimal(18, 0), Ex.amount)) AS Amount," +
            " B.Remarks,S.par_name,S.locn_code,S.div_code,S.CCM,S.KAM,S.HOD" +
        " FROM bjvdet AS Ex" +
        " INNER JOIN @PendingBill AS P ON P.ref = Ex.ref" +
        " LEFT OUTER Join babprime.dbo.bjv AS B ON B.ref = Ex.ref" +
        " LEFT OUTER join babprime.dbo.[SAL_PART] AS S WITH(NOLOCK) On S.par_code = B.par_code" +
        " WHERE Ex.type <> 'Y1'" +
        " GROUP BY Ex.ref, B.par_code, B.vessal, B.yRef,B.odate,B.FSB,B.clrDate,B.Remarks," +
        " S.par_name,S.locn_code,S.div_code,S.CCM,S.KAM,S.HOD Having SUM(Convert(Decimal(18, 0), Ex.amount)) <> 0;";


        dsCurrentJob = CDatabase.GetFADataSet(strCommand);

        return dsCurrentJob;
    }

    public static DataSet FAGetJobStatus(string JobNo)
    {

        SqlCommand command = new SqlCommand();
        command.CommandText = "BJV_GETJobStatus";
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@BJVNO", SqlDbType.NVarChar).Value = JobNo;

        return CDatabase.GetDataSet("BJV_GETJobStatus", command);

    }

    public static DataSet FAGetJobExpense(string JobNo)
    {

        SqlCommand command = new SqlCommand();
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@BJVNO", SqlDbType.NVarChar).Value = JobNo;

        return CDatabase.GetDataSet("BJV_GetJobExpense", command);

    }

    public static DataSet FAGetJobExpenseBilled(string JobNo)
    {

        SqlCommand command = new SqlCommand();
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@BJVNO", SqlDbType.NVarChar).Value = JobNo;

        return CDatabase.GetDataSet("BJV_GetJobExpenseBilled", command);
    }
    public static DataSet FAGetJobAdjustmentBilled(string JobNo)
    {

        SqlCommand command = new SqlCommand();
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = JobNo;

        return CDatabase.GetDataSet("BJV_GetBillAdjustment", command);
    }
    public static DataSet FAGetTransJobExpenseBilled(string JobNo)
    {

        SqlCommand command = new SqlCommand();
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@BJVNO", SqlDbType.NVarChar).Value = JobNo;

        return CDatabase.GetDataSet("BJV_GetTransJobExpenseBilled", command);
    }

    public static int FACheckDraftInvoice(string JobRefNO)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNO;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BJV_ExcelDraftFinalDateByJob2", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region FREIGHT BILLING

    public static DataTable GetModuleId(string JobRefNo)
    {
        DataTable dtModule;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        dtModule = CDatabase.GetDataTable("GetModuleId", cmd);
        return dtModule;
    }

    #endregion

    #region Billing Dispatch
    public static DataSet GetBillDispatchEmail(int JobId, string JobRefNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        DataSet dsBillDispatch = CDatabase.GetDataSet("BL_GetBillDispatchEmail", command);

        return dsBillDispatch;
    }
    public static int AddBillDispatchCondition(int JobId, int BillId, int PhysicalBillStatus, int EBillStatus, int PortalStatus, int ModuleId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@PhysicalBillStatus", SqlDbType.Int).Value = PhysicalBillStatus;
        command.Parameters.Add("@EBillStatus", SqlDbType.Int).Value = EBillStatus;
        command.Parameters.Add("@PortalStatus", SqlDbType.Int).Value = PortalStatus;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BL_insPCDDispatchingClient", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillDispatchCondition(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("BL_GetPCDDispatchingClient", command);

    }

    public static DataSet GetBJVBillDetailByID(int BillId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BillId", SqlDbType.NVarChar).Value = BillId;
        return CDatabase.GetDataSet("BL_GetBJVDetailById", command);
    }
    public static DataSet GetBillDocById(int JobId, int BillId, int DocType)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;

        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;

        return CDatabase.GetDataSet("BL_GetBillDocPathById", command);
    }
    public static DataSet GetBillDocPathByJobId(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;

        return CDatabase.GetDataSet("BL_GetBillDocPathByJobId", command);
    }
    public static DataSet GetBillJobDetailForEBill(int JobId, int BillId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;

        return CDatabase.GetDataSet("BL_GetBillJobDetailForEBill", command);
    }
    public static DataSet GetBillCoverDocById(int BillCoverId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BillCoverId", SqlDbType.Int).Value = BillCoverId;

        return CDatabase.GetDataSet("BL_GetBillCoverById", command);
    }
    public static DataSet GetBillCoverDocByBillId(int BillId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;

        return CDatabase.GetDataSet("BL_GetBillCoverByBillId", command);
    }
    public static int AddBillDocPath(int JobId, int BillId, string filename, string DocPath, int DocumentId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        //BL_insBillDocPath
        SPresult = CDatabase.GetSPOutPut("BL_insBillDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int DeleteBillDocById(int BillId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BL_delBillDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int AddBillStatus(int JobId, int BillId, int StatusId, string strRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BL_updBillStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }
    public static int AddBillClientPortalDate(int JobId, int BillId, DateTime ClientPortalDate, string strClientPortalRefNo, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@ClientPortalDate", SqlDbType.DateTime).Value = ClientPortalDate;
        command.Parameters.Add("@ClientPortalRefNo", SqlDbType.NVarChar).Value = strClientPortalRefNo;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BL_insBillClientPortalDate", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }
    public static int AddBillCoverletterPath(int BillId, int JobId, int Customerid, string filename, string masterInvoiceNo, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@Jobid", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Customerid", SqlDbType.Int).Value = Customerid;

        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@masterInvoiceNo", SqlDbType.NVarChar).Value = masterInvoiceNo;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BL_insBillCoverletterPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int updateBillingEBillList(string JobIdList, DateTime EBillDate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobIdList", SqlDbType.NVarChar).Value = JobIdList;

        if (EBillDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EBillDate", SqlDbType.DateTime).Value = EBillDate;
        }

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BL_insEBillDateJobList", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddBillDispatchComplete(int JobId, bool IsApproved, string Remark, int ModuleId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BL_insPCDDispatching", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetPendingBillDispatchForExcel(int UserId, int FinYear)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.NVarChar).Value = FinYear;

        DataSet dsBillDispatch = CDatabase.GetDataSet("BL_GetPendingDispatchExcel", command);

        return dsBillDispatch;
    }
    public static int CheckDispatchCoverLetter(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BL_CheckBillCoverletter", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    #endregion
}