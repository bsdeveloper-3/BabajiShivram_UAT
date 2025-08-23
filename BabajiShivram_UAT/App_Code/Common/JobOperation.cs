using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for JobOperation
/// </summary>
public class JobOperation
{
	public JobOperation()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static int AddMiscDocument(string DocumentName, int ModuleId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@DocumentName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_insMiscDocMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateMiscDocument(int lid, string DocumentName, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@DocumentName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_updMiscDocMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int DeleteMiscDocument(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_delMiscDocMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static string GetNextMiscJobNo(int ModuleID,int BranchID,int ModeID,int TypeId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchID;
        //command.Parameters.Add("@ModeID", SqlDbType.Int).Value = ModeID;
        //command.Parameters.Add("@TypeId", SqlDbType.Int).Value = TypeId;

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("MS_GetNewJobNo", command, "@OutPut");
        return Convert.ToString(SPresult);
    }

    public static int AddMiscJobDetail(int ModuleID,int BranchID,String JobRefNo, int TransMode, int TypeID, 
        int CustomerId, int DivisionId, int PlantId, string ConsigneeGSTN,
        string JobDescription, int UserID)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchID;
        cmd.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = TypeID;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        cmd.Parameters.Add("@ConsigneeGSTN", SqlDbType.NVarChar).Value = ConsigneeGSTN;
        
        cmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = JobDescription;
        cmd.Parameters.Add("lUser", SqlDbType.Int).Value = UserID;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_insJobDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetMiscJobDetail(int MiscJobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@MiscJobId", SqlDbType.Int).Value = MiscJobId;

        return CDatabase.GetDataSet("MS_GetJobDetailByID", command);
    }

    public static void FillDocumentMS(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,DocumentName FROM MS_DocumentMS Where bDel= 0 ORDER BY DocumentName", "DocumentName", "lId");
    }
    public static void FillDocumentMS(DropDownList DropDown, int ModuleID)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,DocumentName FROM MS_DocumentMS Where ModuleId="+ModuleID +" AND bDel= 0 ORDER BY DocumentName", "DocumentName", "lId");
    }
    public static DataSet GetMiscDocumentMS()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("MS_GetDocDetail", command);
    }

    public static int AddMiscJobDoc(int JobId ,string filename, string DocPath, int DocumentId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_insJobDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetMiscDocument(int JobID, int DocId)
    {
        // Get All Job Document Set DocumentId = 0
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        command.Parameters.Add("@DocId", SqlDbType.Int).Value = DocId;

        return CDatabase.GetDataSet("MS_GetJobDocument", command);
    }
    public static DataTable Get_BillingInstructionDetail(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleId;
        return CDatabase.GetDataTable("Get_BillingInstruction", command);
    }
    public static int MS_AddBillingInstruction(int JobId, int ModuleID, string AlliedAgencyService, string AlliedAgencyRemark, string OtherService, string OtherServiceRemark,
       string Instruction, string Instruction1, string Instruction2, string Instruction3, string InstructioCopy, string InstructioCopy1, string InstructioCopy2, string InstructioCopy3,
       int UserId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;
        cmd.Parameters.Add("@AlliedAgencyService", SqlDbType.NVarChar).Value = AlliedAgencyService;
        cmd.Parameters.Add("@AlliedAgencyRemark", SqlDbType.NVarChar).Value = AlliedAgencyRemark;
        cmd.Parameters.Add("@OtherService", SqlDbType.NVarChar).Value = OtherService;
        cmd.Parameters.Add("@OtherServiceRemark", SqlDbType.NVarChar).Value = OtherServiceRemark;
        cmd.Parameters.Add("@Instruction", SqlDbType.NVarChar).Value = Instruction;
        cmd.Parameters.Add("@Instruction1", SqlDbType.NVarChar).Value = Instruction1;
        cmd.Parameters.Add("@Instruction2", SqlDbType.NVarChar).Value = Instruction2;
        cmd.Parameters.Add("@Instruction3", SqlDbType.NVarChar).Value = Instruction3;
        cmd.Parameters.Add("@InstructionCopy", SqlDbType.NVarChar).Value = InstructioCopy;
        cmd.Parameters.Add("@InstructionCopy1", SqlDbType.NVarChar).Value = InstructioCopy1;
        cmd.Parameters.Add("@InstructionCopy2", SqlDbType.NVarChar).Value = InstructioCopy2;
        cmd.Parameters.Add("@InstructionCopy3", SqlDbType.NVarChar).Value = InstructioCopy3;
        cmd.Parameters.Add("@luser", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_insBillingInstruction", cmd, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddBillingAdvice(int JobID, int ModuleID, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobID", SqlDbType.BigInt).Value = JobID;
        command.Parameters.Add("@ModuleID", SqlDbType.BigInt).Value = ModuleID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_insBillingAdvice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
}