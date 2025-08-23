using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TransOperation
/// </summary>
public class TransOperation
{
    public TransOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static int AddCustomerBillMS(int TransReqId, int JobId, decimal TotalFreight, decimal TotalDetention, decimal TotalEmpty, decimal TotalWarai, decimal TotalBillAmount, bool IsBillToBabaji, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@TotalFreight", SqlDbType.Decimal).Value = TotalFreight;
        command.Parameters.Add("@TotalDetention", SqlDbType.Decimal).Value = TotalDetention;
        command.Parameters.Add("@TotalEmpty", SqlDbType.Decimal).Value = TotalEmpty;
        command.Parameters.Add("@TotalWarai", SqlDbType.Decimal).Value = TotalWarai;
        command.Parameters.Add("@TotalBillAmount", SqlDbType.Decimal).Value = TotalBillAmount;
        command.Parameters.Add("@IsBillToBabaji", SqlDbType.Bit).Value = IsBillToBabaji;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insCustBillMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    
    public static int AddCustomerBillDetail(int BillID, int TransReqId, int RateId, decimal FreightAmount,
        decimal DetentionAmount, decimal EmptyCharges, decimal WaraiExpense,  int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillID;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@FreightCharges", SqlDbType.Decimal).Value = FreightAmount;
        command.Parameters.Add("@DetentionCharges", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@EmptyCharges", SqlDbType.Decimal).Value = EmptyCharges;
        command.Parameters.Add("@WaraiCharges", SqlDbType.Decimal).Value = WaraiExpense;
        
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insCustBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    
    public static int AddCustomerBillDetail(int BillID, int TransReqId, int RateId, string VehicleNo, decimal FreightAmount,
        decimal DetentionAmount, decimal EmptyCharges, decimal WaraiExpense, decimal TollCharges, decimal UnionCharges,
        string FreightChallanNo, DateTime FreightChallanDate, string Dest_From, string Dest_To, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillID;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@FreightCharges", SqlDbType.Decimal).Value = FreightAmount;
        command.Parameters.Add("@DetentionCharges", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@EmptyCharges", SqlDbType.Decimal).Value = EmptyCharges;
        command.Parameters.Add("@WaraiCharges", SqlDbType.Decimal).Value = WaraiExpense;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@UnionCharges", SqlDbType.Decimal).Value = UnionCharges;
        command.Parameters.Add("@FreightChallanNo", SqlDbType.NVarChar).Value = FreightChallanNo;

        if (FreightChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FreightChallanDate", SqlDbType.Date).Value = FreightChallanDate;
        }

        command.Parameters.Add("@Dest_From", SqlDbType.NVarChar).Value = Dest_From;
        command.Parameters.Add("@Dest_To", SqlDbType.NVarChar).Value = Dest_To;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insCustBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    
    public static int AddCustomerBillPost(int TransReqId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPostDraftBill", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #region Dept Bill Submission

    public static int AddTransporterBillDetail(int TransRateID, int ChargeTypeID, decimal Charges,
        int TaxType, decimal TaxPercent, decimal TaxAmount, decimal TotalCharges, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@TransRateID", SqlDbType.Int).Value = TransRateID;
        command.Parameters.Add("@ChargeTypeID", SqlDbType.Int).Value = ChargeTypeID;
        command.Parameters.Add("@Charges", SqlDbType.Decimal).Value = Charges;
        command.Parameters.Add("@TaxType", SqlDbType.Int).Value = TaxType;
        command.Parameters.Add("@TaxPercent", SqlDbType.Decimal).Value = TaxPercent;
        command.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = TaxAmount;
        command.Parameters.Add("@TotalCharges", SqlDbType.Decimal).Value = TotalCharges;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insTransRateItem", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion
}