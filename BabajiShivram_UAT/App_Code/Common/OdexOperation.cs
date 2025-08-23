using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections.Generic;
/// <summary>
/// Summary description for OdexOperation
/// </summary>
public class OdexOperation
{
    public OdexOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static int AddInvoiceRequest(int JobId, string ConsigneeName, string ConsigneeGST, string ConsigneeContactNo,
        string ConsigneeEmail, string ConsigneeAddress, string ConsigneesState, string BLNo, string invCategory, string LocationCode,
        Boolean IsFreeDays, DateTime FreeDaysValidity, int NoOfFreeDays, Boolean IsDOExt,
        Boolean IsHighSeaSales, Boolean isSeawayBL, Boolean isMblHbl, Boolean isAddChrgReq, int hblCount,
        DateTime StorageChargeDays, Boolean IsLatePymtChrg, Boolean IsHaz, Boolean IsODC, DateTime DischargeDate,
        string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@ConsigneeGST", SqlDbType.NVarChar).Value = ConsigneeGST;
        command.Parameters.Add("@ConsigneeContactNo", SqlDbType.NVarChar).Value = ConsigneeContactNo;
        command.Parameters.Add("@ConsigneeEmail", SqlDbType.NVarChar).Value = ConsigneeEmail;
        command.Parameters.Add("@ConsigneeAddress", SqlDbType.NVarChar).Value = ConsigneeAddress;
        command.Parameters.Add("@ConsigneesState", SqlDbType.NVarChar).Value = ConsigneesState;
        command.Parameters.Add("@BLNo", SqlDbType.NVarChar).Value = BLNo;
        command.Parameters.Add("@invCategory", SqlDbType.NVarChar).Value = invCategory;
        command.Parameters.Add("@LocationCode", SqlDbType.NVarChar).Value = LocationCode;
        command.Parameters.Add("@IsFreeDays", SqlDbType.Bit).Value = IsFreeDays;

        command.Parameters.Add("@NoOfFreeDays", SqlDbType.Int).Value = NoOfFreeDays;
        command.Parameters.Add("@IsDOExt", SqlDbType.Bit).Value = IsDOExt;
        command.Parameters.Add("@IsHighSeaSales", SqlDbType.Bit).Value = IsHighSeaSales;
        command.Parameters.Add("@isSeawayBL", SqlDbType.Bit).Value = isSeawayBL;
        command.Parameters.Add("@isMblHbl", SqlDbType.Bit).Value = isMblHbl;
        command.Parameters.Add("@isAddChrgReq", SqlDbType.Bit).Value = isAddChrgReq;
        command.Parameters.Add("@hblCount", SqlDbType.Int).Value = hblCount;
        command.Parameters.Add("@IsLatePymtChrg", SqlDbType.Bit).Value = IsLatePymtChrg;
        command.Parameters.Add("@IsHaz", SqlDbType.Bit).Value = IsHaz;
        command.Parameters.Add("@IsODC", SqlDbType.Bit).Value = IsODC;

        if (FreeDaysValidity != DateTime.MinValue)
        {
            command.Parameters.Add("@FreeDaysValidity", SqlDbType.DateTime).Value = FreeDaysValidity;

        }
        if (StorageChargeDays != DateTime.MinValue)
        {
            command.Parameters.Add("@StorageChargeDays", SqlDbType.DateTime).Value = StorageChargeDays;

        }
        if (DischargeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DischargeDate", SqlDbType.DateTime).Value = @DischargeDate;
        }

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insInvoiceRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDORequest(int JobId, int ModuleId, int ShippingId, string ShippingName, string ShippingCode,string BLNo, 
        string CargoType, string StuffType, string LocationCode, Boolean IsFreeDays, DateTime FreeDaysValidity, Boolean IsDOExt,
    Boolean isSeawayBL, Boolean IsOdexPayment, Boolean IsAdvanceBLSubmit, string RunnerBoyName, string RunnerBoyMobNo,
    string HSSCustomerName,string FactoryAddress, string FactoryPin, string Remark, int lStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@ShippingId", SqlDbType.Int).Value = ShippingId;
        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = ShippingName;
        command.Parameters.Add("@ShippingCode", SqlDbType.NVarChar).Value = ShippingCode;
        command.Parameters.Add("@BLNo", SqlDbType.NVarChar).Value = BLNo;
        command.Parameters.Add("@CargoType", SqlDbType.NVarChar).Value = CargoType;
        command.Parameters.Add("@StuffType", SqlDbType.NVarChar).Value = StuffType;
        command.Parameters.Add("@LocationCode", SqlDbType.NVarChar).Value = LocationCode;
        command.Parameters.Add("@IsFreeDays", SqlDbType.Bit).Value = IsFreeDays;
        command.Parameters.Add("@IsDOExt", SqlDbType.Bit).Value = IsDOExt;
    
        command.Parameters.Add("@isSeawayBL", SqlDbType.Bit).Value = isSeawayBL;
        command.Parameters.Add("@IsOdexPayment", SqlDbType.Bit).Value = IsOdexPayment;
        command.Parameters.Add("@IsAdvanceBLSubmit", SqlDbType.Bit).Value = IsAdvanceBLSubmit;
        command.Parameters.Add("@RunnerBoyName", SqlDbType.NVarChar).Value = RunnerBoyName;
        command.Parameters.Add("@RunnerBoyMobNo", SqlDbType.NVarChar).Value = RunnerBoyMobNo;
        command.Parameters.Add("@HSSCustomerName", SqlDbType.NVarChar).Value = HSSCustomerName;
        command.Parameters.Add("@FactoryAddress", SqlDbType.NVarChar).Value = FactoryAddress;
        command.Parameters.Add("@FactoryPin", SqlDbType.NVarChar).Value = FactoryPin;


        if (FreeDaysValidity != DateTime.MinValue)
        {
            command.Parameters.Add("@FreeDaysValidity", SqlDbType.DateTime).Value = FreeDaysValidity;

        }
        
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = lStatus;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insDORequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetInvoiceRequest(int RequestId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;

        return CDatabase.GetDataSet("OD_GetInvoiceRequestById", command);
    }
    public static DataSet GetInvoiceResponse(int RequestId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = @RequestId;

        return CDatabase.GetDataSet("OD_GetInvoiceResponseById", command);
    }

    public static int AddInvoiceStatus(int RequestId, string OdexRefNo, int StatusId,string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@OdexRefNo", SqlDbType.NVarChar).Value = OdexRefNo;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insInvoiceHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int AddDOStatus(int DORequestId, int StatusId, string ResponseRefNo, string DORequestStatus, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@DORequestId", SqlDbType.Int).Value = DORequestId;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value    = StatusId;
        command.Parameters.Add("@ResponseRefNo", SqlDbType.NVarChar).Value = ResponseRefNo;
        command.Parameters.Add("@DORequestStatus", SqlDbType.NVarChar).Value = DORequestStatus;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value       = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction  = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insDOHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetDORequest(int DORequestId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DORequestId", SqlDbType.Int).Value = DORequestId;

        return CDatabase.GetDataSet("OD_GetDORequestById", command);
    }
public static DataSet GetDOResponse(int DORequestId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DORequestId", SqlDbType.Int).Value = DORequestId;

        return CDatabase.GetDataSet("OD_GetInvoiceResponseById", command);
    }
    public static int AddInvoiceStatusPush(string OdexRefNo, int StatusId, string Remark, string RequestStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@OdexRefNo", SqlDbType.NVarChar).Value = OdexRefNo;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@RequestStatus", SqlDbType.NVarChar).Value = RequestStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insInvoiceHistoryPush", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddInvoiceResponse(int RequestId, string bnfCode, string locationCode, string bookingLineCode,
    string typOfCargo, string idTp, string blNo, string invNo, decimal totalPymtAmt,
    string invCategory, string invTp, DateTime invDt, string billToParty,
    string gstNo, string partialPymt, string isDORevalidation, decimal doRevalidationCharges, string isTdsDeduction,
    string jobNo, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@bnfCode", SqlDbType.NVarChar).Value = bnfCode;
        command.Parameters.Add("@locationCode", SqlDbType.NVarChar).Value = locationCode;
        command.Parameters.Add("@bookingLineCode", SqlDbType.NVarChar).Value = bookingLineCode;
        command.Parameters.Add("@typOfCargo", SqlDbType.NVarChar).Value = typOfCargo;
        command.Parameters.Add("@idTp", SqlDbType.NVarChar).Value = idTp;
        command.Parameters.Add("@blNo", SqlDbType.NVarChar).Value = blNo;
        command.Parameters.Add("@invNo", SqlDbType.NVarChar).Value = invNo;
        command.Parameters.Add("@totalPymtAmt", SqlDbType.Decimal).Value = totalPymtAmt;
        command.Parameters.Add("@invCategory", SqlDbType.NVarChar).Value = invCategory;

        command.Parameters.Add("@invTp", SqlDbType.NVarChar).Value = invTp;
        command.Parameters.Add("@invDt", SqlDbType.DateTime).Value = invDt;
        command.Parameters.Add("@billToParty", SqlDbType.NVarChar).Value = billToParty;
        command.Parameters.Add("@gstNo", SqlDbType.NVarChar).Value = gstNo;
        command.Parameters.Add("@partialPymt", SqlDbType.NVarChar).Value = partialPymt;
        command.Parameters.Add("@isDORevalidation", SqlDbType.NVarChar).Value = isDORevalidation;
        command.Parameters.Add("@doRevalidationCharges", SqlDbType.NVarChar).Value = doRevalidationCharges;

        command.Parameters.Add("@isTdsDeduction", SqlDbType.NVarChar).Value = isTdsDeduction;
        command.Parameters.Add("@jobNo", SqlDbType.NVarChar).Value = jobNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_InsInvoiceResponse", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDOResponsePush(int JobId, int ModuleID, string bnfCode, string doReqStatus, string blNo, 
        string locationCode, string remarks, string status,int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        command.Parameters.Add("@bnfCode", SqlDbType.NVarChar).Value = bnfCode;
        command.Parameters.Add("@doReqStatus", SqlDbType.NVarChar).Value = doReqStatus;
        command.Parameters.Add("@blNo", SqlDbType.NVarChar).Value = blNo;
        command.Parameters.Add("@locationCode", SqlDbType.NVarChar).Value = locationCode;
        command.Parameters.Add("@remarks", SqlDbType.NVarChar).Value = remarks;
        command.Parameters.Add("@status", SqlDbType.NVarChar).Value = status;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insRespDODetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    //public static int AddInvoiceReleaseCopy(int InvoiceId, int RequestId, string DocTitle, string DocPath, string DocName, int UserId)
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";
    //    command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
    //    command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
    //    command.Parameters.Add("@DocTitle", SqlDbType.NVarChar).Value = DocTitle;
    //    command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
    //    command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;

    //    command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
    //    command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("OD_insInvoiceDocument", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int AddInvoiceReleaseCopy(int DORespId, int DORequestId, string DocTitle, string DocPath, string DocName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@DORespId", SqlDbType.Int).Value = DORespId;
        command.Parameters.Add("@DORequestId", SqlDbType.Int).Value = DORequestId;
        command.Parameters.Add("@DocTitle", SqlDbType.NVarChar).Value = DocTitle;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("OD_insDOPushDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetInvoiceReleaseCopyById(int DocumentId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;

        return CDatabase.GetDataSet("OD_GetInvoiceDocumentById", command);
    }

    public static bool LogRequestToTextFile(string JsonReqeust, string strRequestType)
    {
        // Generate new file for each request
        bool isSuccess = false;

        try
        {
            string NewFileName = "Odex/Log/OdexRequestLog_" + DateTime.Now.ToFileTimeUtc().ToString() + ".log";
            string filePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, NewFileName);

            FileInfo ObjfileInfo = new FileInfo(filePath);

            if (ObjfileInfo.Exists)
            {
                //if (ObjfileInfo.Length > 3000000) // 3 MB  - Length in Bytes. 100,00,00 Bytes = 1.5 MB
                //{
                //ObjfileInfo.MoveTo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, NewFileName));
                //}

                NewFileName = "Odex/Log/OdexRequestLog_OldA_" + DateTime.Now.ToFileTimeUtc().ToString() + ".log";
                ObjfileInfo = new FileInfo(filePath);
            }

            using (FileStream logFile = ObjfileInfo.Create())
            {
                Byte[] text = new UTF8Encoding(true).GetBytes(JsonReqeust);
                logFile.Write(text, 0, text.Length);
                logFile.Close();
                logFile.Dispose();
                GC.Collect();
            }

        }
        catch (Exception ex)
        {
            GC.Collect();
            ErrorLog.LogToTextFile(ex.Message);
            ErrorLog.LogToDatabase(0, "Odex Log", "LogRequestToTextFile", ex.Message, ex, "", 1);
        }
        finally 
        {
            GC.Collect();
        }

        return isSuccess;
    }

    public static DataSet OD_GetInvoiceDocument(int InvoiceID, int DocumentId)
    {
        // Get All invoice Document Set DocumentId = 0
        // Get Specific Document - Provide DocumentID - AC_InvoiceDocument Table Promery Key - lid
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@OD_InvoiceId", SqlDbType.Int).Value = InvoiceID;
        command.Parameters.Add("@OD_DocId", SqlDbType.Int).Value = DocumentId;

        return CDatabase.GetDataSet("OD_GetInvoiceDocByID", command);
    }
}