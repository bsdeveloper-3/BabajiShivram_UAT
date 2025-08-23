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
/// Summary description for CMOperations
/// </summary>
public class CMOperations
{
    public CMOperations()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetJobRefNo(int BranchId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_GetNextJobRefNo", command, "@OutPut");
        return Convert.ToString(SPresult);
    }

    public static DataSet GetLetterTemplatePath(int LetterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LetterId", SqlDbType.Int).Value = LetterId;
        return CDatabase.GetDataSet("CM_GetLetterTemplatePath", cmd);
    }

    public static int ProcessMovement(int JobId, bool IsProcessed, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsProcessed", SqlDbType.Bit).Value = IsProcessed;
        if (Remark != "")
            command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_updProcessMovement", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddShipperTableHeader(int ShippingFieldId, string HeaderTitle, string BSFieldName, int DataType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ShippingFieldId", SqlDbType.Int).Value = ShippingFieldId;
        command.Parameters.Add("@HeaderTitle", SqlDbType.NVarChar).Value = HeaderTitle;
        if (BSFieldName != "")
            command.Parameters.Add("@BSFieldName", SqlDbType.NVarChar).Value = BSFieldName;
        command.Parameters.Add("@DataType", SqlDbType.Int).Value = DataType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insShipperTableHeader", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteShipperTableHeader(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_delShipperTableHeader", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetMovementDetailByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("CM_GetMovementDetailByJobId", cmd);
    }

    public static DataSet GetJobDetailByLid(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("CM_GetJobDetailByLid", cmd);
    }

    public static DataSet GetJobDetail(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("CM_GetJobDetail", cmd);
    }

    ////////// Add Shipping Letters //////////////////////////////
    public static int AddShipperLetter(int ShippingId, string sName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ShippingId", SqlDbType.Int).Value = ShippingId;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insShippingLetters", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteShippingLetter(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_delShippingLetters", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    ///////// Add Letter Fields    //////////////////////////////
    public static int AddLetterField(int LetterId, string FieldName, int lType, bool IsTable, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LetterId", SqlDbType.Int).Value = LetterId;
        command.Parameters.Add("@FieldName", SqlDbType.NVarChar).Value = FieldName;
        command.Parameters.Add("@lType", SqlDbType.Int).Value = lType;
        command.Parameters.Add("@IsTable", SqlDbType.Bit).Value = IsTable;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insLetterFields", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteLetterField(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_delLetterFields", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetLetterFields_Lid(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CM_GetLetterFields_Lid", cmd);
    }

    public static DataSet GetLetterFields_ShippingId(int ShippingId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ShippingId", SqlDbType.Int).Value = ShippingId;
        return CDatabase.GetDataSet("CM_GetLetterFields_ShippingId", cmd);
    }

    public static SqlDataReader GetLetterFieldsByLetterId(int LetterId)
    {
        SqlDataReader drLetterFields;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LetterId", SqlDbType.Int).Value = LetterId;
        return drLetterFields = CDatabase.GetDataReader("CM_GetLetterFields_LetterId", cmd);
    }

    public static SqlDataReader GetLetterTablesByLetterId(int LetterId)
    {
        SqlDataReader drLetterFields;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LetterId", SqlDbType.Int).Value = LetterId;
        return drLetterFields = CDatabase.GetDataReader("CM_GetLetterTables_LetterId", cmd);
    }

    /////////  Add Table Header    //////////////////////////////
    public static int AddLetterTable(int FieldId, string HeaderTitle, string BSFieldName, int DataType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@HeaderTitle", SqlDbType.NVarChar).Value = HeaderTitle;
        command.Parameters.Add("@BSFieldName", SqlDbType.NVarChar).Value = BSFieldName;
        command.Parameters.Add("@DataType", SqlDbType.Int).Value = DataType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insLetterTables", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteLetterTable(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_delLetterTables", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static SqlDataReader GetLetterTablesHeaderByFieldId(int FieldId)
    {
        SqlDataReader drLetterFields;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        return drLetterFields = CDatabase.GetDataReader("CM_GetLetterTables_FieldId", cmd);
    }

    ////////   Documents  /////////////////////////////
    public static DataSet GetDocuments(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("CM_GetDocuments", cmd);
    }

    public static int AddDocument(int JobId, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insDocuments", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDocument(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_delDocuments", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddBackOfficeDocument(int JobId, int DocId, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocId", SqlDbType.Int).Value = DocId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insBackOfficeDocument", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteBackOfficeDocument(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_delBackOfficeDocument", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet ReportContReceivedJobs(int lUser, int FinYearId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataSet("CM_rptContReceivedJobs", cmd);
    }

    ////////   Movement Detail  ////////////////////////////

    public static int AddToBackOffice(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insMoveToBackOffice", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddToScrutiny(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insMoveToScrutiny", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddConsolidateJobDetail(int JobId, int BSJobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BSJobId", SqlDbType.Int).Value = BSJobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insConsolidateJobDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetChekListDocDetail(int JobId, int DocumentForType)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        return CDatabase.GetDataSet("CM_GetChekListDocDetail", cmd);
    }

    public static int AddMovementDetail(int JobId, DateTime EmptyContReturnDate, DateTime MovementComplete, DateTime ShippingLineDate,
                                                DateTime ConfirmedByLineDate, int NominatedCFSId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (EmptyContReturnDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EmptyContReturnDate", SqlDbType.DateTime).Value = EmptyContReturnDate;
        }
        if (MovementComplete != DateTime.MinValue)
        {
            command.Parameters.Add("@MovementComp", SqlDbType.DateTime).Value = MovementComplete;
        }
        if (ShippingLineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ShippingLineDate", SqlDbType.DateTime).Value = ShippingLineDate;
        }
        if (ConfirmedByLineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ConfirmedByLineDate", SqlDbType.DateTime).Value = ConfirmedByLineDate;
        }
        if (NominatedCFSId > 0)
        {
            command.Parameters.Add("@NominatedCFSId", SqlDbType.Int).Value = NominatedCFSId;
        }
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insMovementDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateMovementDetail(int lid, DateTime EmptyContReturnDate, DateTime MovementComplete, DateTime ShippingLineDate,
                                             DateTime ConfirmedByLineDate, int NominatedCFSId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        if (EmptyContReturnDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EmptyContReturnDate", SqlDbType.DateTime).Value = EmptyContReturnDate;
        }
        if (MovementComplete != DateTime.MinValue)
        {
            command.Parameters.Add("@MovementComp", SqlDbType.DateTime).Value = MovementComplete;
        }
        if (ShippingLineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ShippingLineDate", SqlDbType.DateTime).Value = ShippingLineDate;
        }
        if (ConfirmedByLineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ConfirmedByLineDate", SqlDbType.DateTime).Value = ConfirmedByLineDate;
        }
        if (NominatedCFSId > 0)
        {
            command.Parameters.Add("@NominatedCFSId", SqlDbType.Int).Value = NominatedCFSId;
        }
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_updMovementDetailByLid", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddJobDetail(string JobRefNo, int BranchId, int CustomerId, int DivisionId, int PlantId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insJobDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDetail(int lid, int CustomerId, int DivisionId, int PlantId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_updJobDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateReceivedJob(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_updReceivedJob", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet mailNewJobNotification(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("CM_mailNewJobNotification", cmd);
    }

    public static DataSet GetMovementCount(int FinYearId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataSet("CM_GetMovementCount", cmd);
    }

    public static DataSet GetMovementDetailByLid(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CM_GetMovementDetailByLid", cmd);
    }

    public static int AddUnprocessJobHistory(int JobId, bool IsProcessed, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsProcessed", SqlDbType.Bit).Value = IsProcessed;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insUnprocessJobHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateNominatedCFSName(int JobId, int NominatedCFSId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.NVarChar).Value = JobId;
        command.Parameters.Add("@NominatedCFSId", SqlDbType.Int).Value = NominatedCFSId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_updNominatedCFSName", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddContRecdCFSDetail(DateTime ContRecdAtCFSDate, int ContainerDetailId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ContRecdAtCFSDate", SqlDbType.Date).Value = ContRecdAtCFSDate;
        command.Parameters.Add("@ContainerDetailId", SqlDbType.Int).Value = ContainerDetailId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insContRecdCFSDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    /////////////////// Billing Operation ////////////////////////////////

    public static int ApproveRejectScrutiny(int JobId, bool IsApproved, string Remark, int reasonforPendency, string LRDCType, int lUser)
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

        SPresult = CDatabase.GetSPOutPut("CM_insPCDBillingScrutinyApproval", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DraftInvoiceJobMoveToDraftCheck(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("CM_insJobmoveDraftCheck", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int FinalTypingJobMoveToFinalCheck(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("CM_insjobmoveFinaldraftCheck", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddCFSRefundStatus(int JobId, DateTime FollowUpDate, string FollowUpRemark, bool IsActive, int StatusId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FollowUpDate", SqlDbType.DateTime).Value = FollowUpDate;
        command.Parameters.Add("@FollowUpRemark", SqlDbType.NVarChar).Value = FollowUpRemark;
        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CM_insJobCFSRefund", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
}