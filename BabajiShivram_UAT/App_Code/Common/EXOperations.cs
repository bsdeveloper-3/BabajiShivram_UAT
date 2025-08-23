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
using System.Web.Configuration;

/// <summary>
/// Summary description for EXOperations
/// </summary>
public class EXOperations
{
    public EXOperations()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static int EX_GetNewJobId()
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_GetNewJobId", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet EXGetPCDDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("EX_GetPCDDetail", command);
    }

    public static int EX_AddExportJob(int BabajiBranchId, string JobRefNo, int CustomerId, int ShipperId, string ConsigneeName, int TransMode,
          string BuyerName, string ProductDesc, int PortOfLoadingId, int PortOfDischargeId, int ConsignmentCountryId, int DestinationCountryId, int NoOfPackages, int PackageType,
          int ShippingBillType, string ForwarderName, string CustRefNo, int ContainerLoadedId, double GrossWT,
          double NetWT, int TransportBy, int Priority, int DivisionId, int PlantId, int lUser, string PickUpFrom, string Destination, int IsBabajiForwarder,
          string MAWBNo, string HAWBNo, DateTime MAWBDATE, DateTime HAWBDate, int ExportType, string Dimension,
          string PickupPersonName, DateTime PickupDate, string PickupPhoneNo, bool IsADC, bool IsHaze, string JobRemark, int PreAlertId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ShipperId", SqlDbType.Int).Value = ShipperId;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        command.Parameters.Add("@ProductDesc", SqlDbType.NVarChar).Value = ProductDesc;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@ConsignmentCountryId", SqlDbType.Int).Value = ConsignmentCountryId;
        command.Parameters.Add("@DestinationCountryId", SqlDbType.Int).Value = DestinationCountryId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@ShippingBillType", SqlDbType.Int).Value = ShippingBillType;
        command.Parameters.Add("@ForwarderName", SqlDbType.NVarChar).Value = ForwarderName;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@ContainerLoadedId", SqlDbType.Int).Value = ContainerLoadedId;
        command.Parameters.Add("@GrossWT", SqlDbType.Decimal).Value = GrossWT;
        command.Parameters.Add("@NetWT", SqlDbType.Decimal).Value = NetWT;
        command.Parameters.Add("@TransportBy", SqlDbType.Int).Value = TransportBy;
        command.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@ExportTypeId", SqlDbType.Int).Value = ExportType;
        command.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = Destination;
        command.Parameters.Add("@PickUpFrom", SqlDbType.NVarChar).Value = PickUpFrom;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        command.Parameters.Add("@IsADC", SqlDbType.NVarChar).Value = IsADC;
        command.Parameters.Add("@IsHaze", SqlDbType.NVarChar).Value = IsHaze;

        if (MAWBDATE != DateTime.MinValue)
            command.Parameters.Add("@MAWBDATE", SqlDbType.Date).Value = MAWBDATE;
        if (HAWBDate != DateTime.MinValue)
            command.Parameters.Add("@HAWBDate", SqlDbType.Date).Value = HAWBDate;
        command.Parameters.Add("@IsBabajiForwarder", SqlDbType.Bit).Value = IsBabajiForwarder;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@PickupPersonName", SqlDbType.NVarChar).Value = PickupPersonName;
        if (PickupDate != DateTime.MinValue)
            command.Parameters.Add("@PickupDate", SqlDbType.DateTime).Value = PickupDate;
        command.Parameters.Add("@PickupMobileNo", SqlDbType.NVarChar).Value = PickupPhoneNo;
        command.Parameters.Add("@JobRemark", SqlDbType.NVarChar).Value = JobRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;          // Freight Export
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = PreAlertId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int EX_AddExportNewJob(int BabajiBranchId, string JobRefNo, int CustomerId, int ShipperId, string ConsigneeName, int TransMode, string BuyerName, string ProductDesc,
          int PortOfLoadingId, int PortOfDischargeId, int ConsignmentCountryId, int DestinationCountryId, int NoOfPackages, int PackageType,
          int ShippingBillType, string ForwarderName, string CustRefNo, int ContainerLoadedId, double GrossWT,
          double NetWT, int TransportBy, int Priority, int DivisionId, int PlantId, int lUser, string PickUpFrom, string Destination, int IsBabajiForwarder,
          string MAWBNo, string HAWBNo, DateTime MAWBDATE, DateTime HAWBDate, int ExportType, string Dimension,
          string PickupPersonName, DateTime PickupDate, string PickupPhoneNo, bool IsADC, bool IsHaze, string JobRemark, int PreAlertId, int FinYear)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ShipperId", SqlDbType.Int).Value = ShipperId;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        command.Parameters.Add("@ProductDesc", SqlDbType.NVarChar).Value = ProductDesc;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@ConsignmentCountryId", SqlDbType.Int).Value = ConsignmentCountryId;
        command.Parameters.Add("@DestinationCountryId", SqlDbType.Int).Value = DestinationCountryId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@ShippingBillType", SqlDbType.Int).Value = ShippingBillType;
        command.Parameters.Add("@ForwarderName", SqlDbType.NVarChar).Value = ForwarderName;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@ContainerLoadedId", SqlDbType.Int).Value = ContainerLoadedId;
        command.Parameters.Add("@GrossWT", SqlDbType.Decimal).Value = GrossWT;
        command.Parameters.Add("@NetWT", SqlDbType.Decimal).Value = NetWT;
        command.Parameters.Add("@TransportBy", SqlDbType.Int).Value = TransportBy;
        command.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@ExportTypeId", SqlDbType.Int).Value = ExportType;
        command.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = Destination;
        command.Parameters.Add("@PickUpFrom", SqlDbType.NVarChar).Value = PickUpFrom;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        command.Parameters.Add("@IsADC", SqlDbType.NVarChar).Value = IsADC;
        command.Parameters.Add("@IsHaze", SqlDbType.NVarChar).Value = IsHaze;

        if (MAWBDATE != DateTime.MinValue)
            command.Parameters.Add("@MAWBDATE", SqlDbType.Date).Value = MAWBDATE;
        if (HAWBDate != DateTime.MinValue)
            command.Parameters.Add("@HAWBDate", SqlDbType.Date).Value = HAWBDate;
        command.Parameters.Add("@IsBabajiForwarder", SqlDbType.Bit).Value = IsBabajiForwarder;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@PickupPersonName", SqlDbType.NVarChar).Value = PickupPersonName;
        if (PickupDate != DateTime.MinValue)
            command.Parameters.Add("@PickupDate", SqlDbType.DateTime).Value = PickupDate;
        command.Parameters.Add("@PickupMobileNo", SqlDbType.NVarChar).Value = PickupPhoneNo;
        command.Parameters.Add("@JobRemark", SqlDbType.NVarChar).Value = JobRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;          // Freight Export
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = PreAlertId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insNewJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int Ex_AddPreAlertDocs(string DocPath, int DocumentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insJobDocsDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet EX_GetParticularJobDetail(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("EX_GetParticularJobDetail", cmd);
    }

    public static DataSet EX_GetJobDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("EX_GetParticularJobDetail", command);
    }
    public static DataSet EX_GetJobBasicDetail(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("EX_GetJobBasicDetail", cmd);
    }
    public static int UpdateDeliveryDetail(int JobId, string DeliveryDestination, int TransportBy, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DeliveryDestination", SqlDbType.NVarChar).Value = DeliveryDestination;
        command.Parameters.Add("@TransportBy", SqlDbType.Int).Value = TransportBy;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_updDeliveryDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static void EX_FillPCDDocument(DropDownList DropDownList)
    {
        CDatabase.BindControls(DropDownList, "SELECT lId,sName FROM BS_PCDDocumentMS WHERE lType=1 AND  bDel = 0 ORDER BY sName", "sName", "lId");
    }

    /*********************************************  INVOICE DETAILS METHODS  *********************************************************************************/
    public static DataSet EX_GetInvoiceDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("EX_GetInvoiceDetail", "command");
    }

    public static int Ex_AddInvoiceDetail(int JobId, int lid, string InvoiceNo, DateTime InvoiceDate, string InvoiceValue, int ShipmentTermsId,
                                    string DBKAmount, string LicenseNo, DateTime LicenseDate, string FreightAmount, string InsuranceAmount, int lUser, string InvoiceCurrency)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (lid != 0)
            command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceValue", SqlDbType.NVarChar).Value = InvoiceValue;
        command.Parameters.Add("@InvoiceCurrency", SqlDbType.NVarChar).Value = InvoiceCurrency;
        command.Parameters.Add("@ShipmentTermId", SqlDbType.Int).Value = ShipmentTermsId;
        command.Parameters.Add("@DBKAmount", SqlDbType.NVarChar).Value = DBKAmount;
        command.Parameters.Add("@LicenseNo", SqlDbType.NVarChar).Value = LicenseNo;
        if (LicenseDate != DateTime.MinValue)
            command.Parameters.Add("@LicenseDate", SqlDbType.Date).Value = LicenseDate;
        command.Parameters.Add("@FreightAmount", SqlDbType.NVarChar).Value = FreightAmount;
        command.Parameters.Add("@InsuranceAmount", SqlDbType.NVarChar).Value = InsuranceAmount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insInvoiceDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int EX_DeleteInvoiceDetail(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_delInvoiceDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    /*********************************************  /INVOICE DETAILS METHODS  *********************************************************************************/

    public static int AddChecklistStatusDetail(int JobId, int ChecklistStatus, int AuthorisedBy, DateTime AuthorisedDate, string AuthorRemark, int RequestedBy,
                                     string RequestRemark, DateTime RequestedDate, int IsActive)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ChecklistStatus", SqlDbType.Int).Value = ChecklistStatus;
        if (AuthorisedBy != 0)
            command.Parameters.Add("@AuthorisedBy", SqlDbType.Int).Value = AuthorisedBy;
        if (AuthorisedDate != DateTime.MinValue)
            command.Parameters.Add("@AuthorisedDate", SqlDbType.DateTime).Value = AuthorisedDate;
        if (AuthorRemark != "")
            command.Parameters.Add("@AuthorRemark", SqlDbType.NVarChar).Value = AuthorRemark;
        command.Parameters.Add("@RequestedBy", SqlDbType.Int).Value = RequestedBy;
        command.Parameters.Add("@RequestRemark", SqlDbType.NVarChar).Value = RequestRemark;
        if (RequestedDate != DateTime.MinValue)
            command.Parameters.Add("@RequestedDate", SqlDbType.DateTime).Value = RequestedDate;
        command.Parameters.Add("@IsActive", SqlDbType.Int).Value = IsActive;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insChecklistStatusDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    /*********************************************  CONTAINER DETAILS METHODS  *********************************************************************************/
    public static int EX_AddContainerDetail(int JobId, string ContainerNo, int ContainerSize, int ContainerType, int lUser, string SealNo)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@SealNo", SqlDbType.NVarChar).Value = SealNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insContainerDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int EX_DeleteContainerDetail(int lid, Int32 lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_delContainerDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    /*********************************************  /CONTAINER DETAILS METHODS  ********************************************************************************/

    public static DataView EX_GetJobHistory(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("EX_GetJobHistoryById", cmd);
    }

    public static int EX_AddChecklistDetail(int JobId, bool CustomerApproval, string CheckListPath, string FOBValue, string CIFValue, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ClientApproval", SqlDbType.Bit).Value = CustomerApproval;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        if (CheckListPath != "")
            command.Parameters.Add("@CheckListPath", SqlDbType.NVarChar).Value = CheckListPath;
        if (FOBValue != "")
            command.Parameters.Add("@FOBValue", SqlDbType.NVarChar).Value = FOBValue;
        if (CIFValue != "")
            command.Parameters.Add("@CIFValue", SqlDbType.NVarChar).Value = CIFValue;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_insChecklistDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int EX_UpdateChecklistDetail(int JobId, string CheckListPath, string FOBValue, string CIFValue, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        if (CheckListPath != "")
            command.Parameters.Add("@CheckListPath", SqlDbType.NVarChar).Value = CheckListPath;
        if (FOBValue != "")
            command.Parameters.Add("@FOBValue", SqlDbType.NVarChar).Value = FOBValue;
        if (CIFValue != "")
            command.Parameters.Add("@CIFValue", SqlDbType.NVarChar).Value = CIFValue;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_updChecklistDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int EX_AddShippingBillDetail(int JobId, string SBNo, DateTime SBDate, int MarkAppraising, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@SBNo", SqlDbType.NVarChar).Value = SBNo;
        command.Parameters.Add("@SBDate", SqlDbType.Date).Value = SBDate;
        command.Parameters.Add("@MarkAppraising", SqlDbType.Int).Value = MarkAppraising;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insSBDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int EX_AddFilingDetail(int JobId, string SBNo, DateTime SBDate, int MarkAppraising, DateTime MarkPassingDate, DateTime RegisterationDate,
                                     DateTime ExamineDate, DateTime ExamineReportDate, DateTime LEODate, DateTime CartDate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@SBNo", SqlDbType.NVarChar).Value = SBNo;
        command.Parameters.Add("@SBDate", SqlDbType.Date).Value = SBDate;
        command.Parameters.Add("@MarkAppraising", SqlDbType.Int).Value = MarkAppraising;
        if (MarkPassingDate != DateTime.MinValue)
            command.Parameters.Add("@MarkPassingDate", SqlDbType.Date).Value = MarkPassingDate;
        if (RegisterationDate != DateTime.MinValue)
            command.Parameters.Add("@RegisterationDate", SqlDbType.Date).Value = RegisterationDate;
        if (ExamineDate != DateTime.MinValue)
            command.Parameters.Add("@ExamineDate", SqlDbType.Date).Value = ExamineDate;
        if (ExamineReportDate != DateTime.MinValue)
            command.Parameters.Add("@ExamineReportDate", SqlDbType.Date).Value = ExamineReportDate;
        if (CartDate != DateTime.MinValue)
            command.Parameters.Add("@CartingDate", SqlDbType.Date).Value = CartDate;
        if (LEODate != DateTime.MinValue)
            command.Parameters.Add("@LEODate", SqlDbType.Date).Value = LEODate;
        if (Remark != "")
            command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insSBFilingDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int EX_UpdateFilingCustomDetail(int JobId, string SBNo, DateTime SBDate, int MarkAppraising, DateTime MarkPassingDate, DateTime RegisterationDate,
                                     DateTime ExamineDate, DateTime ExamineReportDate, DateTime LEODate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@SBNo", SqlDbType.NVarChar).Value = SBNo;
        command.Parameters.Add("@SBDate", SqlDbType.Date).Value = SBDate;
        command.Parameters.Add("@MarkAppraising", SqlDbType.Int).Value = MarkAppraising;
        if (MarkPassingDate != DateTime.MinValue)
            command.Parameters.Add("@MarkPassingDate", SqlDbType.Date).Value = MarkPassingDate;
        if (RegisterationDate != DateTime.MinValue)
            command.Parameters.Add("@RegisterationDate", SqlDbType.Date).Value = RegisterationDate;
        if (ExamineDate != DateTime.MinValue)
            command.Parameters.Add("@ExamineDate", SqlDbType.Date).Value = ExamineDate;
        if (ExamineReportDate != DateTime.MinValue)
            command.Parameters.Add("@ExamineReportDate", SqlDbType.Date).Value = ExamineReportDate;
        if (LEODate != DateTime.MinValue)
            command.Parameters.Add("@LEODate", SqlDbType.Date).Value = LEODate;
        if (Remark != "")
            command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_updFilingCustomDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetJobDetailForCustomProcess(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("EX_GetJobDetailForCustomProcess", command);
    }

    public static int EX_AddShippingGetInDetail(int JobId, DateTime ShippingLineDate, string ExporterCopyPath, string VGMCopyPath, DateTime FreightForwardedDate,
                                    string ForwarderPersonName, string ForwardToEmail, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ShippingLineDate", SqlDbType.Date).Value = ShippingLineDate;
        command.Parameters.Add("@ExporterCopyPath", SqlDbType.NVarChar).Value = ExporterCopyPath;
        command.Parameters.Add("@VGMCopyPath", SqlDbType.NVarChar).Value = VGMCopyPath;
        command.Parameters.Add("@FreightForwardedDate", SqlDbType.Date).Value = FreightForwardedDate;
        command.Parameters.Add("@ForwarderPersonName", SqlDbType.NVarChar).Value = ForwarderPersonName;
        command.Parameters.Add("@ForwardToEmail", SqlDbType.NVarChar).Value = ForwardToEmail;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insShipmentGetInDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    /********************************************  PCD DOCUMENT METHODS  *********************************************************************************/
    public static int EX_AddPCDToBackOffice(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insPCDToBackOffice", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetShippingDetailByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("EX_GetShippingDetailByJobId", command);
    }

    public static DataSet EX_GetPCDDocumentForWorkFlow(int JobId, int DocumentForType)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        return CDatabase.GetDataSet("EX_GetPCDDocumentForWorkFlow", command);
    }

    public static DataSet EX_GetJobDetailforPCALetter(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("EX_GetJobDetailForPCDLetter", command);
    }

    public static DataSet EX_GetPendingOperationCnt(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("EX_GetPendingExportCount", command);

        return dsPending;
    }

    public static DataSet EX_GetPendingOperationCntByCustUserId(int CustUserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("EX_GetPendingCountByCustId", command);

        return dsPending;
    }

    public static int EX_AddForm13JobDetail(int JobId, DateTime Form13Date, DateTime TransHandoverDate, DateTime ContainerGetInDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (Form13Date != DateTime.MinValue)
            command.Parameters.Add("@Form13Date", SqlDbType.Date).Value = Form13Date;
        if (TransHandoverDate != DateTime.MinValue)
            command.Parameters.Add("@TransHandoverDate", SqlDbType.Date).Value = TransHandoverDate;
        if (ContainerGetInDate != DateTime.MinValue)
            command.Parameters.Add("@ContainerGetInDate", SqlDbType.Date).Value = ContainerGetInDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insForm13JobDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int EX_UpdateForm13JobDetail(int JobId, DateTime Form13Date, DateTime TransHandoverDate, DateTime ContainerGetInDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (Form13Date != DateTime.MinValue)
            command.Parameters.Add("@Form13Date", SqlDbType.Date).Value = Form13Date;
        if (TransHandoverDate != DateTime.MinValue)
            command.Parameters.Add("@TransHandoverDate", SqlDbType.Date).Value = TransHandoverDate;
        if (ContainerGetInDate != DateTime.MinValue)
            command.Parameters.Add("@ContainerGetInDate", SqlDbType.Date).Value = ContainerGetInDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_updForm13Detail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    /*********************************************  FIELD GROUPS EVENTS  **********************************************************************************/
    public static DataSet EX_GetFieldGroup()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("EX_GetFieldGroup", cmd);
    }

    public static DataSet EX_GetConditionalFieldGroup()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("EX_GetConditionalFieldGroup", cmd);
    }

    /**********************************************  USER REPORTS  ***************************************************************************************/
    public static DataView EX_GetAdhocReport(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateFrom;
        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTo;
        if (DeliveryStatus != "")
            cmd.Parameters.Add("@DeliveryStatus", SqlDbType.Char).Value = DeliveryStatus;
        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = AdhocFilter;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataView("EX_rptAdHocReport", cmd);
    }

    public static DataTable EX_GetAdhocReportByStatus(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateFrom;
        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTo;
        if (DeliveryStatus != "")
            cmd.Parameters.Add("@DeliveryStatus", SqlDbType.Char).Value = DeliveryStatus;
        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = AdhocFilter;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataTable("EX_rptAdHocReport", cmd);
    }

    public static SqlDataReader EX_GetJobAdditionalFields(int JobId)
    {
        SqlDataReader drJobFields;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return drJobFields = CDatabase.GetDataReader("EX_GetJobFields", cmd);

    }

    public static string EX_GenerateJobRefNo()
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_GenerateJobRefNo", command, "@OutPut");
        return SPresult;
    }

    public static void EX_FillChekListDocDetail(DropDownList DropDown)
    {

        CDatabase.BindControls(DropDown, "SELECT lId,DocumentName FROM BS_ChekListDocDetail Where bDel= 0 AND DocType = 11 ORDER BY DocumentName", "DocumentName", "lId");
    }

    // public static int EX_AddJobAdditionalFields(int JobId, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();
    //    string SPresult = "";

    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
    //    command.Parameters.Add("@TermsOfDelivery", SqlDbType).Value = ;
    //    command.Parameters.Add("@Airline", SqlDbType).Value = ;
    //    command.Parameters.Add("@FLTDetails", SqlDbType).Value = ;
    //    command.Parameters.Add("@Detension", SqlDbType).Value = ;
    //    command.Parameters.Add("@Remarks", SqlDbType).Value = ;
    //    command.Parameters.Add("@ExportInvoiceRef", SqlDbType).Value = ;
    //    command.Parameters.Add("@BriefDescription", SqlDbType).Value = ;
    //    command.Parameters.Add("@NatureOfExport", SqlDbType).Value = ;
    //    command.Parameters.Add("@CargoPickUpOn", SqlDbType).Value = ;
    //    command.Parameters.Add("@CargoCartOn", SqlDbType).Value = ;
    //    command.Parameters.Add("@MrfScnNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@Currency", SqlDbType).Value = ;
    //    command.Parameters.Add("@CIFValueInINR", SqlDbType).Value = ;
    //    command.Parameters.Add("@FOBValueInINR", SqlDbType).Value = ;
    //    command.Parameters.Add("@DispatchFromFactory", SqlDbType).Value = ;
    //    command.Parameters.Add("@ArrivedAtPOCDate", SqlDbType).Value = ;
    //    command.Parameters.Add("@PODate", SqlDbType).Value = ;
    //    command.Parameters.Add("@InvoiceValueInINR", SqlDbType).Value = ;
    //    command.Parameters.Add("@ETD", SqlDbType).Value = ;
    //    command.Parameters.Add("@ETA", SqlDbType).Value = ;
    //    command.Parameters.Add("@ReExportType", SqlDbType).Value = ;
    //    command.Parameters.Add("@DeliveryNote", SqlDbType).Value = ;
    //    command.Parameters.Add("@ReExportInvoiceNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@CIFValueInFC", SqlDbType).Value = ;
    //    command.Parameters.Add("@FOBValueInFC", SqlDbType).Value = ;
    //    command.Parameters.Add("@DHLAWBNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@InvoiceValueInCIF", SqlDbType).Value = ;
    //    command.Parameters.Add("@Description", SqlDbType).Value = ;
    //    command.Parameters.Add("@SerialPartNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@PSL", SqlDbType).Value = ;
    //    command.Parameters.Add("@FCC", SqlDbType).Value = ;
    //    command.Parameters.Add("@InstructionfromPSL", SqlDbType).Value = ;
    //    command.Parameters.Add("@Contract", SqlDbType).Value = ;
    //    command.Parameters.Add("@ImportInvoiceNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@ImportBERef", SqlDbType).Value = ;
    //    command.Parameters.Add("@GLSoughtOn", SqlDbType).Value = ;
    //    command.Parameters.Add("@FemaGR", SqlDbType).Value = ;
    //    command.Parameters.Add("@CHAName", SqlDbType).Value = ;
    //    command.Parameters.Add("@DateIssued", SqlDbType).Value = ;
    //    command.Parameters.Add("@AWBLRNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@AWBDate", SqlDbType).Value = ;
    //    command.Parameters.Add("@ATD", SqlDbType).Value = ;
    //    command.Parameters.Add("@BMSNo", SqlDbType).Value = ;
    //    command.Parameters.Add("@FirstOptator", SqlDbType).Value = ;
    //    command.Parameters.Add("@SASLFileRef", SqlDbType).Value = ;
    //    command.Parameters.Add("@BusinessSegment", SqlDbType).Value = ;
    //    command.Parameters.Add("@ExpLandingDate", SqlDbType).Value = ;
    //    command.Parameters.Add("@ClearanceMode", SqlDbType).Value = ;
    //    command.Parameters.Add("@ECDutyRecvdOn", SqlDbType).Value = ;
    //    command.Parameters.Add("@CustomDutyAmtINR", SqlDbType).Value = ;
    //    command.Parameters.Add("@CustomDutyArrangeBy", SqlDbType).Value = ;
    //    command.Parameters.Add("@AgencyChargesINR", SqlDbType).Value = ;
    //    command.Parameters.Add("@TransportationINR", SqlDbType).Value = ;
    //    command.Parameters.Add("@DemurrageAmt", SqlDbType).Value = ;
    //    command.Parameters.Add("@DetentionAmt", SqlDbType).Value = ;
    //    command.Parameters.Add("@OtherCharges", SqlDbType).Value = ;
    //    command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
    //    command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("EX_insJobAdditionalFields", command, "@Output");
    //    return Convert.ToInt32(SPresult);
    //}

    public static DataSet EX_GetJobActivityDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("EX_GetJobActivityDetailById", command);
    }

    /*******************************************************  DASHBOARD EVENTS   ***********************************************************************/
    public static DataSet GetPendingStageWsJobDetails(int FinYearID, int Status, int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@FinYearID", SqlDbType.Int).Value = FinYearID;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        return CDatabase.GetDataSet("EX_GetPendingStageWsJobDetails", command);
    }

    /*******************************************************  SHIPMENT TRACKING METHODS *****************************************************************/
    public static int EX_UpdateExportJobAdmin(int JobId, int BabajiBranchId, string CustRefNo, int CustomerId, int ShipperId, int DivisionId, int PlantId, string ConsigneeName, int Mode,
       string ProductDesc, string BuyerName, int PortOfLoadingId, int PortOfDischargeId, int ConsignmentCountryId, int DestinationCountryId,
       int ExportType, int PackageType, int ShippingBillType, int TransportBy, int Priority, string PickUpFrom, string Destination,
       int NoOfPackages, int IsBabajiForwarder, string ForwarderName, int ContainerLoadedId, double GrossWT, double NetWT,
       string MAWBNo, DateTime MAWBDATE, string HAWBNo, DateTime HAWBDate, string Dimension, int lUser,
       bool IsOctroi, bool IsSForm, bool IsNForm, bool IsRoadPermit, string PickUpPersonName, DateTime PickUpDate, string PickUpMobileNo, bool IsADC, bool IsHaze, string JobRemark)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ShipperId", SqlDbType.Int).Value = ShipperId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@ProductDesc", SqlDbType.NVarChar).Value = ProductDesc;
        command.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        command.Parameters.Add("@PortOfLoading", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischarge", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@ConsignmentCountry", SqlDbType.Int).Value = ConsignmentCountryId;
        command.Parameters.Add("@DestinationCountry", SqlDbType.Int).Value = DestinationCountryId;
        command.Parameters.Add("@ExportTypeId", SqlDbType.Int).Value = ExportType;
        command.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@ShippingBillTypeId", SqlDbType.Int).Value = ShippingBillType;
        command.Parameters.Add("@TransportById", SqlDbType.Int).Value = TransportBy;
        command.Parameters.Add("@PriorityId", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@PickUpFrom", SqlDbType.NVarChar).Value = PickUpFrom;
        command.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = Destination;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@IsBabajiForwarder", SqlDbType.Bit).Value = IsBabajiForwarder;
        command.Parameters.Add("@ForwarderName", SqlDbType.NVarChar).Value = ForwarderName;
        command.Parameters.Add("@ContainerLoadedId", SqlDbType.Int).Value = ContainerLoadedId;
        command.Parameters.Add("@GrossWT", SqlDbType.Decimal).Value = GrossWT;
        command.Parameters.Add("@NetWT", SqlDbType.Decimal).Value = NetWT;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@IsADC", SqlDbType.Bit).Value = IsADC;
        command.Parameters.Add("@IsHaze", SqlDbType.Bit).Value = IsHaze;

        if (MAWBDATE != DateTime.MinValue)
            command.Parameters.Add("@MAWBDATE", SqlDbType.Date).Value = MAWBDATE;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        if (HAWBDate != DateTime.MinValue)
            command.Parameters.Add("@HAWBDate", SqlDbType.Date).Value = HAWBDate;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@IsOctroi", SqlDbType.Bit).Value = IsOctroi;
        command.Parameters.Add("@IsSForm", SqlDbType.Bit).Value = IsSForm;
        command.Parameters.Add("@IsNForm", SqlDbType.Bit).Value = IsNForm;
        command.Parameters.Add("@IsRoadPermit", SqlDbType.Bit).Value = IsRoadPermit;
        command.Parameters.Add("@updUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@PickupPersonName", SqlDbType.NVarChar).Value = PickUpPersonName;
        if (PickUpDate != DateTime.MinValue)
            command.Parameters.Add("@PickupDate", SqlDbType.DateTime).Value = PickUpDate;
        command.Parameters.Add("@PickupMobileNo", SqlDbType.NVarChar).Value = PickUpMobileNo;
        command.Parameters.Add("@JobRemark", SqlDbType.NVarChar).Value = JobRemark;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_updJobDetailAdmin", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int EX_UpdateExportJob(int JobId, int BabajiBranchId, string CustRefNo, int CustomerId, int ShipperId, int DivisionId, int PlantId, string ConsigneeName, int Mode,
       string ProductDesc, string BuyerName, int PortOfLoadingId, int PortOfDischargeId, int ConsignmentCountryId, int DestinationCountryId,
       int ExportType, int PackageType, int ShippingBillType, int TransportBy, int Priority, string PickUpFrom, string Destination,
       int NoOfPackages, int IsBabajiForwarder, string ForwarderName, int ContainerLoadedId, double GrossWT, double NetWT,
       string MAWBNo, DateTime MAWBDATE, string HAWBNo, DateTime HAWBDate, string Dimension, int lUser,
       bool IsOctroi, bool IsSForm, bool IsNForm, bool IsRoadPermit, string PickUpPersonName, DateTime PickUpDate, string PickUpMobileNo, string JobRemark)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ShipperId", SqlDbType.Int).Value = ShipperId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@ProductDesc", SqlDbType.NVarChar).Value = ProductDesc;
        command.Parameters.Add("@BuyerName", SqlDbType.NVarChar).Value = BuyerName;
        command.Parameters.Add("@PortOfLoading", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischarge", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@ConsignmentCountry", SqlDbType.Int).Value = ConsignmentCountryId;
        command.Parameters.Add("@DestinationCountry", SqlDbType.Int).Value = DestinationCountryId;
        command.Parameters.Add("@ExportTypeId", SqlDbType.Int).Value = ExportType;
        command.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@ShippingBillTypeId", SqlDbType.Int).Value = ShippingBillType;
        command.Parameters.Add("@TransportById", SqlDbType.Int).Value = TransportBy;
        command.Parameters.Add("@PriorityId", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@PickUpFrom", SqlDbType.NVarChar).Value = PickUpFrom;
        command.Parameters.Add("@Destination", SqlDbType.NVarChar).Value = Destination;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@IsBabajiForwarder", SqlDbType.Bit).Value = IsBabajiForwarder;
        command.Parameters.Add("@ForwarderName", SqlDbType.NVarChar).Value = ForwarderName;
        command.Parameters.Add("@ContainerLoadedId", SqlDbType.Int).Value = ContainerLoadedId;
        command.Parameters.Add("@GrossWT", SqlDbType.Decimal).Value = GrossWT;
        command.Parameters.Add("@NetWT", SqlDbType.Decimal).Value = NetWT;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;

        if (MAWBDATE != DateTime.MinValue)
            command.Parameters.Add("@MAWBDATE", SqlDbType.Date).Value = MAWBDATE;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        if (HAWBDate != DateTime.MinValue)
            command.Parameters.Add("@HAWBDate", SqlDbType.Date).Value = HAWBDate;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@IsOctroi", SqlDbType.Bit).Value = IsOctroi;
        command.Parameters.Add("@IsSForm", SqlDbType.Bit).Value = IsSForm;
        command.Parameters.Add("@IsNForm", SqlDbType.Bit).Value = IsNForm;
        command.Parameters.Add("@IsRoadPermit", SqlDbType.Bit).Value = IsRoadPermit;
        command.Parameters.Add("@updUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@PickupPersonName", SqlDbType.NVarChar).Value = PickUpPersonName;
        if (PickUpDate != DateTime.MinValue)
            command.Parameters.Add("@PickupDate", SqlDbType.DateTime).Value = PickUpDate;
        command.Parameters.Add("@PickupMobileNo", SqlDbType.NVarChar).Value = PickUpMobileNo;
        command.Parameters.Add("@JobRemark", SqlDbType.NVarChar).Value = JobRemark;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_updJobDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int EX_AddExamineDetail(int JobId, bool IsOctroi, bool IsSForm, bool IsNForm, bool IsRoadPermit, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsOctroi", SqlDbType.Bit).Value = IsOctroi;
        command.Parameters.Add("@IsSForm", SqlDbType.Bit).Value = IsSForm;
        command.Parameters.Add("@IsNForm", SqlDbType.Bit).Value = IsNForm;
        command.Parameters.Add("@IsRoadPermit", SqlDbType.Bit).Value = IsRoadPermit;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insExamineDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataView EX_GetJobDetailForDelivery(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("EX_GetJobDetailForDelivery", cmd);
    }

    public static void EX_FillPendingContainerDetail(DropDownList DropDown, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        CDatabase.BindControls(DropDown, "EX_GetPendingContainerDetail", command, "ContainerNo", "lId");
    }

    public static int EX_AddDeliveryDetail(int JobId, int ContainerId, int NoOfPackages, string VehicleNo, DateTime TruckReqDate,
           DateTime VehicleRcvdDate, string TransporterName, int TransporterID, string LRNo, DateTime LRDate, string DeliveryPoint, DateTime DispatchDate,
           DateTime DeliveryDate, DateTime EmptyContRetrunDate, string RoadPermitNo, DateTime RoadPermitDate, string CargoReceivedBy, string PODPath,
           string NFormNo, DateTime NFormDate, DateTime NClosingDate, string SFormNo, DateTime SFormDate, DateTime SClosingDate, string OctroiAmount,
           string OctroiReceiptNo, DateTime OctroiPaidDate, int VehicleType, string BabajiChallanNo, DateTime BabajiChallanDate,
           string ChallanPath, string DamageCopyPath, string strIsRunwayDelivery, int LabourTypeId, int TransitType, int WarehouseId,
           string DriverName, string DriverPhone, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;

        if (TruckReqDate != DateTime.MinValue)
        {
            command.Parameters.Add("@TruckReqDate", SqlDbType.DateTime).Value = TruckReqDate;
        }
        if (VehicleRcvdDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VehicleRcvdDate", SqlDbType.DateTime).Value = VehicleRcvdDate;
        }
        if (LRDate != DateTime.MinValue)
        {
            command.Parameters.Add("@LRDate", SqlDbType.DateTime).Value = LRDate;
        }
        if (DispatchDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = DispatchDate;
        }
        if (DeliveryDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = DeliveryDate;
        }
        if (EmptyContRetrunDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EmptyContRetrunDate", SqlDbType.DateTime).Value = EmptyContRetrunDate;
        }
        if (RoadPermitDate != DateTime.MinValue)
        {
            command.Parameters.Add("@RoadPermitDate", SqlDbType.DateTime).Value = RoadPermitDate;
        }

        if (strIsRunwayDelivery != "")
        {
            if (strIsRunwayDelivery.ToLower() == "yes")
                command.Parameters.Add("@IsRunwayDelivery", SqlDbType.Bit).Value = true;
            else if (strIsRunwayDelivery.ToLower() == "no")
                command.Parameters.Add("@IsRunwayDelivery", SqlDbType.Bit).Value = false;

        }

        command.Parameters.Add("@LabourTypeId", SqlDbType.Int).Value = LabourTypeId;

        command.Parameters.Add("@RoadPermitNo", SqlDbType.NVarChar).Value = RoadPermitNo;
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@DeliveryPoint", SqlDbType.NVarChar).Value = DeliveryPoint;
        command.Parameters.Add("@CargoReceivedBy", SqlDbType.NVarChar).Value = CargoReceivedBy;
        command.Parameters.Add("@PODAttachmentPath", SqlDbType.NVarChar).Value = PODPath;
        command.Parameters.Add("@NFormNo", SqlDbType.NVarChar).Value = NFormNo;

        if (NFormDate != DateTime.MinValue)
        {
            command.Parameters.Add("@NFormDate", SqlDbType.DateTime).Value = NFormDate;
        }
        if (NClosingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@NClosingDate", SqlDbType.DateTime).Value = NClosingDate;
        }

        if (SFormDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SFormDate", SqlDbType.DateTime).Value = SFormDate;
        }
        if (SClosingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SClosingDate", SqlDbType.DateTime).Value = SClosingDate;
        }
        if (OctroiPaidDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OctroiPaidDate", SqlDbType.DateTime).Value = OctroiPaidDate;
        }
        if (OctroiAmount != "")
        {
            command.Parameters.Add("@OctroiAmount", SqlDbType.NVarChar).Value = OctroiAmount;
        }
        if (BabajiChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;
        }

        command.Parameters.Add("@SFormNo", SqlDbType.NVarChar).Value = SFormNo;
        command.Parameters.Add("@OctroiReceiptNo", SqlDbType.NVarChar).Value = OctroiReceiptNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        command.Parameters.Add("@ChallanPath", SqlDbType.NVarChar).Value = ChallanPath;
        command.Parameters.Add("@DamageCopyPath", SqlDbType.NVarChar).Value = DamageCopyPath;

        command.Parameters.Add("@TransitType", SqlDbType.Int).Value = TransitType;
        command.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = WarehouseId;
        command.Parameters.Add("@DriverName", SqlDbType.NVarChar).Value = DriverName;
        command.Parameters.Add("@DriverPhone", SqlDbType.NVarChar).Value = DriverPhone;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insDeliveryDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static void EX_FillTransporter(DropDownList dropdownlist, int JobId, int CategoryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;
        CDatabase.BindControls(dropdownlist, "EX_GetCompanyByCategoryID", command, "CustName", "lid");
    }

    public static int EX_DeleteDeliveryWarehouseDetail(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_delDeliveryDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet EX_GetExpenseDetailBylId(int ExpenseId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lId", SqlDbType.Int).Value = ExpenseId;
        return CDatabase.GetDataSet("EX_GetExpensesDetailById", command);
    }

    public static string EX_GetBabajiJobRefNo(string JobRefNo)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_GetJobRefNo", command, "@OutPut");
        return SPresult;
    }

    public static DataSet EX_GetFreightOperatnLists()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("FR_GetEnqTransferUser", command);
    }

    public static DataSet EX_GetAdhocReportChildNodeTest(string ParentNode, int ReportType, int CustomerID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;
        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
        return CDatabase.GetDataSet("EX_GetReportChildNodeByParentNode", cmd);
    }
}