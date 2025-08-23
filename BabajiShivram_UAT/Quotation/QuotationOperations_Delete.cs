using System;
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
/// Summary description for QuotationOperations
/// </summary>
public class QuotationOperations
{
    public QuotationOperations()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void FillBranchByUser(DropDownList DropDown, int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        CDatabase.BindControls(DropDown, "BS_GetBranchByUser", command, "BranchName", "lId");
    }

    public static int GetQuoteRefId()
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BS_GetRecentQuoteRefNo", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddQuotationChargeMS(string ChargeName, string Description, int CatgId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuoteCatg", SqlDbType.Int).Value = CatgId;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@sDescription", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuotationCharges", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuotationChargeMS(int lid, string ChargeName, string Description, int CatgId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuoteCatg", SqlDbType.Int).Value = CatgId;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@sDescription", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updQuotationCharges", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetQuotationCharges(int CatgId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CatgId", SqlDbType.Int).Value = CatgId;
        return CDatabase.GetDataSet("BS_GetParticularCharge", command);
    }

    public static int DeleteQuotationCharges(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delQuotationCharges", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static void FillQuotationMS(CheckBoxList CheckBoxList, int CatgId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CatgId", SqlDbType.Int).Value = CatgId;
        CDatabase.BindControls(CheckBoxList, "BS_GetQuotationChargeMS", command, "sName", "lid");
    }

    public static int AddChargeApplicable(int QuoteRefId, int ChargeId, string ChargesApplicable, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuoteRefId", SqlDbType.Int).Value = QuoteRefId;
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@ChargesApplicable", SqlDbType.NVarChar).Value = ChargesApplicable;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insChargesApplicable", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateChargeApplicable(int lid, int QuoteRefId, int ChargeId, string ChargesApplicable, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = QuoteRefId;
        command.Parameters.Add("@QuoteRefId", SqlDbType.Int).Value = QuoteRefId;
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@ChargesApplicable", SqlDbType.NVarChar).Value = ChargesApplicable;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updChargesApplicable", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddDraftQuotation(int BranchId, int ServiceId, int CustomerId, string CustomerName, string AddressLine1, string AddressLine2, string AttendedPerson, string Subject, Boolean Includebody,
        string PaymentTerms, string BodyContent, Boolean IsValidDraft, Boolean IsTenderQuote, Boolean IsLumpSumCode, string OtherNotes, int UserId, int TermConditionId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
        command.Parameters.Add("@AddressLine1", SqlDbType.NVarChar).Value = AddressLine1;
        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = AddressLine2;
        command.Parameters.Add("@AttendedPerson", SqlDbType.NVarChar).Value = AttendedPerson;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
        command.Parameters.Add("@IncludeBody", SqlDbType.Bit).Value = Includebody;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = PaymentTerms;
        command.Parameters.Add("@BodyContent", SqlDbType.NVarChar).Value = BodyContent;
        command.Parameters.Add("@IsValidDraft", SqlDbType.Bit).Value = IsValidDraft;
        command.Parameters.Add("@IsTenderQuote", SqlDbType.Bit).Value = IsTenderQuote;
        command.Parameters.Add("@OtherNotes", SqlDbType.NVarChar).Value = OtherNotes;
        command.Parameters.Add("@IsLumpSumCode", SqlDbType.Bit).Value = IsLumpSumCode;
        command.Parameters.Add("@TermConditionId", SqlDbType.Int).Value = TermConditionId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insDraftQuotation", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDraftQuotation(int lid, int BranchId, int ServiceId, int CustomerId, string CustomerName, string AddressLine1, string AddressLine2, string AttendedPerson, string Subject, Boolean Includebody,
        string PaymentTerms, string BodyContent, Boolean IsValidDraft, string OtherNotes, int UserId, int TermConditionId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
        command.Parameters.Add("@AddressLine1", SqlDbType.NVarChar).Value = AddressLine1;
        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = AddressLine2;
        command.Parameters.Add("@AttendedPerson", SqlDbType.NVarChar).Value = AttendedPerson;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
        command.Parameters.Add("@IncludeBody", SqlDbType.Bit).Value = Includebody;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = PaymentTerms;
        command.Parameters.Add("@BodyContent", SqlDbType.NVarChar).Value = BodyContent;
        command.Parameters.Add("@IsValidDraft", SqlDbType.Bit).Value = IsValidDraft;
        command.Parameters.Add("@OtherNotes", SqlDbType.NVarChar).Value = OtherNotes;
        command.Parameters.Add("@TermConditionId", SqlDbType.Int).Value = TermConditionId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updDraftQuotation", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDraftStatus(int lid, Boolean IsValidDraft, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@IsValidDraft", SqlDbType.Bit).Value = IsValidDraft;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updDraftStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDraftApprovalStatus(int lid, int StatusId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updDraftApprovalStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetParticularQuotation(int QuotationId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = QuotationId;
        return CDatabase.GetDataSet("BS_GetDraftQuotation", command);
    }

    public static DataSet GetAllDraftQuotations(int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataSet("BS_GetDraftQuotationLists", command);
    }

    public static DataSet GetDefQuoteTexture()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("BS_GetQuotationTexture", "command");
    }

    public static int AddChargeWsRanges(int ChargeId, string Currency, decimal MinRange, decimal MaxRange, int ApplicableOn, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;
        command.Parameters.Add("@MinRange", SqlDbType.Decimal).Value = MinRange;
        command.Parameters.Add("@MaxRange", SqlDbType.Decimal).Value = MaxRange;
        command.Parameters.Add("@ApplicableOn", SqlDbType.Int).Value = ApplicableOn;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insChargeWsRangeDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateChargeWsRanges(int lid, string Currency, decimal MinRange, decimal MaxRange, int ApplicableOn, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = Currency;
        command.Parameters.Add("@MinRange", SqlDbType.Decimal).Value = MinRange;
        command.Parameters.Add("@MaxRange", SqlDbType.Decimal).Value = MaxRange;
        command.Parameters.Add("@ApplicableOn", SqlDbType.Int).Value = ApplicableOn;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updChargeWsRangeDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteChargeWsRanges(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delChargeWsRangeDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetChargeWsRangeDetails(int ChargeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        return CDatabase.GetDataSet("BS_GetChargeWsRangeDetail", command);
    }

    public static int AddQuotationCatgMS(string CatgName, int ServiceId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = CatgName;
        command.Parameters.Add("@ServicesId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuotationCategory", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuotationCatgMS(int lid, string CatgName, int ServiceId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = CatgName;
        command.Parameters.Add("@ServicesId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updQuotationCategory", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetQuotationCatg()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("BS_GetQuotationCategory", "command");
    }

    public static int DeleteQuotationCatg(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delQuotationCategory", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetCatgAsPerCharge(int ChargeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        return CDatabase.GetDataSet("BS_GetChargesCategory", command);
    }

    public static int AddCatgAsPerCharge(int ChargeId, int QuoteCatgId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@QuoteCatgId", SqlDbType.Int).Value = QuoteCatgId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insChargesCategory", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCatgAsPerCharge(int ChargeId, int QuoteCatgId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@QuoteCatgId", SqlDbType.Int).Value = QuoteCatgId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updChargesCategory", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetMinValueForRange(int ChargeId, int ApplicableOn)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@ApplicableOn", SqlDbType.Int).Value = ApplicableOn;
        return CDatabase.GetDataSet("BS_GetMinValueForRange", command);
    }

    public static int AddQuotationAppFieldMS(string CatgName, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = CatgName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuoteApplicableFields", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuotationAppFieldMS(int lid, string CatgName, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = CatgName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuoteApplicableFields", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteQuotationAppFieldMS(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delQuoteApplicableFields", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetQuotationAppFields()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("BS_GetQuoteApplicableFields", "command");
    }

    public static int AddQuotationModeMS(string Name, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = Name;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuoteModeMS", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuotationModeMS(int lid, string Name, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = Name;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuoteModeMS", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteQuotationModeMS(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delQuoteModeMS", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetQuotationMode()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("BS_GetQuoteMode", "command");
    }

    public static int AddQuotationAnnexure(int QuotationId, string DocPath, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuotationDocDetail", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteAnnexureDocs(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delQuotationDocDetail", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetQuotationDocDetail(int QuotationId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        return CDatabase.GetDataSet("BS_GetQuotationDocDetail", command);
    }

    public static int BS_GetQuotationRatesDetails(int QuotationId, int QuoteCatgId, int ChargeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@QuotationCatgId", SqlDbType.Int).Value = QuoteCatgId;
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        return Convert.ToInt32(CDatabase.GetDataSet("BS_GetQuotationRatesDetails", command));
    }

    public static int AddQuotationRates(int QuotationId, int QuoteCategoryId, int ChargeId, int ApplicableFieldId, Boolean IsValidAmount, Boolean IsLumpSumField,
                                        double Charges, string ExtraCharges, decimal LumpSumCharges, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@QuoteCategoryId", SqlDbType.Int).Value = QuoteCategoryId;
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@Charges", SqlDbType.Decimal).Value = Charges;
        command.Parameters.Add("@ApplicableFieldId", SqlDbType.Int).Value = ApplicableFieldId;
        command.Parameters.Add("@IsValidAmount", SqlDbType.Bit).Value = IsValidAmount;
        command.Parameters.Add("@IsLumpSumField", SqlDbType.Bit).Value = IsLumpSumField;
        command.Parameters.Add("@ExtraCharges", SqlDbType.NVarChar).Value = ExtraCharges;
        command.Parameters.Add("@LumpSumCharges", SqlDbType.Decimal).Value = LumpSumCharges;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuotationRatesDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuotationRates(int lid, int QuotationId, int QuoteCategoryId, int ChargeId, int ApplicableFieldId, Boolean IsValidAmount, Boolean IsLumpSumField,
                                       double Charges, string ExtraCharges, decimal LumpSumCharges, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@QuoteCategoryId", SqlDbType.Int).Value = QuoteCategoryId;
        command.Parameters.Add("@ChargeId", SqlDbType.Int).Value = ChargeId;
        command.Parameters.Add("@Charges", SqlDbType.Decimal).Value = Charges;
        command.Parameters.Add("@ApplicableFieldId", SqlDbType.Int).Value = ApplicableFieldId;
        command.Parameters.Add("@IsValidAmount", SqlDbType.Bit).Value = IsValidAmount;
        command.Parameters.Add("@IsLumpSumField", SqlDbType.Bit).Value = IsLumpSumField;
        command.Parameters.Add("@ExtraCharges", SqlDbType.NVarChar).Value = ExtraCharges;
        command.Parameters.Add("@LumpSumCharges", SqlDbType.Decimal).Value = LumpSumCharges;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updQuotationRatesDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteQuotationRates(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delQuotationRatesDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int AddLumpsumQuotationRates(int QuotationRateId, decimal Charges, int ApplicableFieldId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationRateId", SqlDbType.Int).Value = QuotationRateId;
        command.Parameters.Add("@Charges", SqlDbType.Decimal).Value = Charges;
        command.Parameters.Add("@ApplicableFieldId", SqlDbType.Int).Value = ApplicableFieldId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insLpQuotationRates", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteLumpsumQuotationRates(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delLpQuotationRates", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int AddQuotationModes(int QuotationId, int ModeId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuotationModes", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetQuotationModes(int QuotationId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        return CDatabase.GetDataSet("BS_GetQuotationModes", command);
    }

    public static DataSet GetQuoteReportData(int QuotationId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        return CDatabase.GetDataSet("rpt_Quotation", command);
    }

    public static int AddQuotationCopy(int QuotationId, string DocPath)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updQuotationPath", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTermConditionMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("BS_GetTermConditionMS", "command");
    }

    public static DataSet GetTermConditionAsPerLid(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("BS_GetTermsConditionPdf", command);
    }

    public static int AddTermConditionMS(string Name, string DocPath, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = Name;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insTermConditionMS", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTermConditionMS(int lid, string DocPath, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updTermConditionMS", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteTermConditionMS(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delTermConditionMS", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTermConditionDetails(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@TermConditionId", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("BS_GetTermConditionDetails", command);
    }

    public static int AddTermConditionDetails(int TermConditionId, string sTermCondition, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TermConditionId", SqlDbType.Int).Value = TermConditionId;
        command.Parameters.Add("@sTermCondition", SqlDbType.NVarChar).Value = sTermCondition;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insTermConditionDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTermConditionDetails(int lid, string sTermCondition, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sTermCondition", SqlDbType.NVarChar).Value = sTermCondition;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updTermConditionDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteTermConditionDetails(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delTermConditionDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTransportationCharges(int QuotationId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        return CDatabase.GetDataSet("BS_GetTransportationCharges", command);
    }

    public static int AddTransportationCharges(int QuotationId, string Particulars, string ChargesApplicable, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@Particulars", SqlDbType.NVarChar).Value = Particulars;
        command.Parameters.Add("@ChargesApplicable", SqlDbType.NVarChar).Value = ChargesApplicable;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insTransportationCharges", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTransportationCharges(int lid, int QuotationId, string Particulars, string ChargesApplicable, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@Particulars", SqlDbType.NVarChar).Value = Particulars;
        command.Parameters.Add("@ChargesApplicable", SqlDbType.NVarChar).Value = ChargesApplicable;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updTransportationCharges", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteTransportationCharges(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_delTransportationCharges", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuoteStatus(int QuotationId, int StatusId, string Remarks, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remarks;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insQuotationStatusHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateQuotationContractDates(int QuotationId, DateTime ContractStartDt, DateTime ContractEndDt, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        if (ContractStartDt != DateTime.MinValue)
            command.Parameters.Add("@ContractStartDt", SqlDbType.Date).Value = ContractStartDt;
        if (ContractEndDt != DateTime.MinValue)
            command.Parameters.Add("@ContractEndDt", SqlDbType.Date).Value = ContractEndDt;
        command.Parameters.Add("@updUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updQuotationContractDates", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

}