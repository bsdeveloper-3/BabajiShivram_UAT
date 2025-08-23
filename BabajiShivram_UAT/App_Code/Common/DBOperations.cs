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

public class DBOperations
{
    public DBOperations()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Stamp New Sw

    public static DataSet GetStampDutyDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("BS_GetStampDutyDetailById", command);
    }

    public static DataView GetJobDetailForStampDuty(int JobId, int userid)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

        return CDatabase.GetDataView("GetJobDetailForStampDuty", cmd);
    }

    public static int AddStampDutyDetail(int JobId, string AdministrationCharges, string Challanno, int Challanamt, DateTime challandate, string ChallanPath, string servicetax,
    string serviceamount, string swachbharattax, string swachbharatamt, string krishikalayntax, string krishikalaynamt, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@AdministrationCharges", SqlDbType.NVarChar).Value = AdministrationCharges;

        command.Parameters.Add("@Challanno", SqlDbType.NVarChar).Value = Challanno;
        command.Parameters.Add("@Challanamt", SqlDbType.Int).Value = Challanamt;


        command.Parameters.Add("@challandate", SqlDbType.DateTime).Value = challandate;

        command.Parameters.Add("@ChallanPath", SqlDbType.NVarChar).Value = ChallanPath;

        command.Parameters.Add("@servicetax", SqlDbType.NVarChar).Value = servicetax;
        command.Parameters.Add("@serviceamount", SqlDbType.NVarChar).Value = serviceamount;
        command.Parameters.Add("@swachbharattax", SqlDbType.NVarChar).Value = swachbharattax;
        command.Parameters.Add("@swachbharatamt", SqlDbType.NVarChar).Value = swachbharatamt;
        command.Parameters.Add("@krishikalayntax", SqlDbType.NVarChar).Value = krishikalayntax;
        command.Parameters.Add("@krishikalaynamt", SqlDbType.NVarChar).Value = krishikalaynamt;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insStampDutyDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int updStampDutyAmount(int JobId, decimal StampDutyAmnt)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@StampDutyAmnt", SqlDbType.Decimal).Value = StampDutyAmnt;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updStampDutyAmnt", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region BSImport Fill Details

    public static void BindStartDate(DropDownList ddDateTime, DateTime StartDate, DateTime EndDate)
    {
        DateTime dtDate;

        TimeSpan intMonth = EndDate.Subtract(StartDate);
        //   
        //   ddDateTime.Items.Add(new ListItem("Select","0" ));

        for (int i = 0; i < 12; i++)
        {
            dtDate = StartDate.AddMonths(i);

            string strStartDate = dtDate.ToString("dd MMM yyyy");

            ddDateTime.Items.Add(new ListItem(strStartDate, i.ToString()));
        }
        //END_FOR
    }

    public static void BindEndDate(DropDownList ddDateTime, DateTime StartDate, DateTime EndDate)
    {
        string strEndDate;

        for (int i = 0; i < 12; i++)
        {
            StartDate = StartDate.AddMonths(1);

            StartDate = StartDate.AddDays(-(StartDate.Day));

            strEndDate = StartDate.ToString("dd MMM yyyy");

            ddDateTime.Items.Add(new ListItem(strEndDate, i.ToString()));

            StartDate = StartDate.AddMonths(1);
        }
        //END_FOR
    }
    public static void FillEWAYBillGSTIN(DropDownList DropDown, int ModuleID)
    {
        CDatabase.BindControls(DropDown, "selecT sName,GSTIN from TR_EwayLogin where ModuleId= " + ModuleID + " AND bDel=0 Order by sName", "sName", "GSTIN");
    }
    public static void FillEWAYBillGSTIN2(DropDownList DropDown, int ModuleID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleID;

        CDatabase.BindControls(DropDown, "TR_EWayGetGSTINByModule", command, "sName", "GSTIN");
    }
    public static void FillState(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName AS StateName FROM LocDetail WHERE lTypId=3 AND bDel=0 Order BY sName", "StateName", "lId");
    }

    public static void FillStateGSTID(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT StateCode,sName AS StateName FROM LocDetail WHERE lTypId=3 AND bDel=0 Order by sName", "StateName", "StateCode");
    }

    public static DataSet GetStateGSTID()
    {
        string strQuery = "SELECT StateCode,sName AS StateName FROM LocDetail WHERE lTypId=3 AND bDel=0 Order by sName";

        return CDatabase.GetDataSet(strQuery);
    }

    private static DataTable GetFinancialYearTable()
    {
        DataTable dt = CDatabase.GetDataTable("SELECT lId, StartDate, EndDate FROM BS_FinYearMS");
        DataTable New = new DataTable();
        // return CDatabase.GetDataTable(SqlString)        
        //DataTable New = CreateSourceTable();
        if (dt.Rows.Count > 0)
        {
            //New = CreateSourceTable();            
            New.Columns.Add("lId", typeof(string));
            New.Columns.Add("FinancialYear", typeof(string));

            foreach (DataRow r in dt.Rows)
            {
                DataRow dr = New.NewRow();
                dr["lId"] = r[0].ToString();

                string[] first = r[1].ToString().Trim().Split('/');
                string[] second = r[2].ToString().Trim().Split('/');

                dr["FinancialYear"] = first[2].Substring(0, 4) + " - " + second[2].Substring(0, 4);
                New.Rows.Add(dr);
            }
        }
        return New;
    }

    public static void FillFinYear(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,(Convert(nvarchar,Datepart(yy,StartDate))+'-'+ convert(nvarchar,Datepart(yy,EndDate))) AS sName FROM BS_FinYearMS Order By lId Desc", "sName", "lId");
    }

    public static bool IsCurrentFinYear(int FinYearId)
    {
        string strQuery = "SELECT lId FROM BS_FinYearMS where lId=" + FinYearId + " AND bDel=0";
        bool result = CDatabase.IsRecordExist(strQuery);
        return result;
    }

    public static void FillJobSubStatus(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_JobSubStatusMS where bDel=0 order by lOrder", "sName", "lId");
    }

    public static void FillBabajiUser(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_UserMS WHERE lType = 1 AND bDel = 0 order by sName", "sName", "lId");
    }

    public static void FillBabajiUserByKAM(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetUserByKAM", command, "sName", "lId");
    }

    public static void FillBabajiUserByDeptID(DropDownList DropDown, int DeptID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DeptId", SqlDbType.Int).Value = DeptID;

        CDatabase.BindControls(DropDown, "GetUserByDeptId", command, "UserName", "UserId");
    }

    public static void FillBabajiUserByDeptID(ListBox CheckBoxList, int DeptID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DeptId", SqlDbType.Int).Value = DeptID;

        CDatabase.BindControls(CheckBoxList, "GetUserByDeptId", command, "UserName", "UserId");
    }

    public static void FillBabajiUserByDeptID(CheckBoxList CheckBoxList, int DeptID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DeptId", SqlDbType.Int).Value = DeptID;

        CDatabase.BindControls(CheckBoxList, "GetUserByDeptId", command, "UserName", "UserId");
    }
    public static void FillBabajiUserByDivisonID(DropDownList DropDown, int DivisionID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DivisionID", SqlDbType.Int).Value = DivisionID;

        CDatabase.BindControls(DropDown, "GetUserByDivisionId", command, "UserName", "UserId");
    }
    public static void FillBabajiHOD(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_UserMS WHERE lType = 1 AND LRoleId=2 AND bDel = 0 order by sName", "sName", "lId");
    }

    public static void FillBabajiUserCustomer(DropDownList DropDown, int UserId, bool IsReport)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@IsReport", SqlDbType.Bit).Value = IsReport;

        CDatabase.BindControls(DropDown, "GetUserCustomer", command, "CustName", "CustomerId");
    }

    public static void FillPreAlertReceived(DropDownList DropDown, int TransMode)
    {
        //GetPreAlertReceived

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("UserId", SqlDbType.Int).Value = 1;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;

        CDatabase.BindControls(DropDown, "GetPreAlertReceived", command, "PreAlertRefno", "lId");
    }

    public static void FillBranch(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,BranchName FROM BS_BranchMS where bDel=0 order by BranchName", "BranchName", "lId");
    }

    public static void FillUserBranchByPort(DropDownList DropDown, int UserId, int PortId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("PortId", SqlDbType.Int).Value = PortId;

        CDatabase.BindControls(DropDown, "GetUserBranchByPort", command, "BranchName", "BranchId");
    }

    public static void FillUserBranch(DropDownList DropDown, int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("UserId", SqlDbType.Int).Value = UserId;
        CDatabase.BindControls(DropDown, "GetUserBranch", command, "BranchName", "BranchId");
    }

    public static void FillBranch(CheckBoxList CheckBoxList)
    {
        CDatabase.BindControls(CheckBoxList, "SELECT lId,BranchName FROM BS_BranchMS where bDel=0 order by BranchName", "BranchName", "lId");
    }

    public static void FillBranchByPort(DropDownList DropDown, int PortId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        CDatabase.BindControls(DropDown, "GetBranchByPort", command, "BranchName", "lId");
    }

    public static void FillPort(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,PortName FROM BS_PortMS Where bDel = 0 ORDER BY PortName", "PortName", "lId");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DropDown"></param>
    /// <param name="Mode">Port Mode 1 For Air and 2 for Sea</param>
    public static void FillPort(DropDownList DropDown, int Mode)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,PortName FROM BS_PortMS Where lMode=" + Mode + "  AND bDel = 0 ORDER BY PortName", "PortName", "lId");
    }

    public static void FillUserPort(DropDownList DropDown, int UserId, int ModeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("Mode", SqlDbType.Int).Value = ModeId;

        CDatabase.BindControls(DropDown, "GetUserPort", command, "PortName", "PortId");
    }

    public static void FillCustomer(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetCustomerMS", command, "CustName", "lId");
    }

    public static void FillCustomerByConsignee(DropDownList DropDown, int ConsigneeId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        CDatabase.BindControls(DropDown, "GetCustomerByConsignee", command, "CustName", "lId");
    }

    /*
    public static void FillCustomer(ComboBox ComboBox)
    {
        CDatabase.BindControls(ComboBox, "SELECT lId,CustName FROM BS_CustomerMS WHERE bDel=0 order by CustName", "CustName", "lId");
    }
    */
    public static void FillConsignee(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,ConsigneeName FROM BS_ConsigneeMS WHERE bDel=0 order by ConsigneeName", "ConsigneeName", "lId");
    }

    public static void FillConsignee(DropDownList DropDown, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        CDatabase.BindControls(DropDown, "GetCustomerConsignee", command, "ConsigneeName", "ConsigneeId");
    }

    public static void FillMode(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,TransMode FROM BS_TransMS  ORDER by lId", "TransMode", "lId");
    }

    public static void FillJobType(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_JobTypeMS WHERE bDel=0 ORDER by sName ", "sName", "lId");
    }

    public static void FillContainerType(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_ContainerTypeMS where bDel=0 order by sName", "sName", "lId");
    }

    public static void FillContainerDetail(DropDownList DropDown, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        CDatabase.BindControls(DropDown, "GetContainerDetail", command, "ContainerNo", "lId");
    }

    public static void FillPendingContainerDetail(DropDownList DropDown, int JobId, int TransitType)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@TransitType", SqlDbType.Int).Value = TransitType;

        CDatabase.BindControls(DropDown, "GetPendingContainerDetail", command, "ContainerNo", "lId");
    }

    public static void FillAlertDocType(CheckBoxList CheckBoxList)
    {
        CDatabase.BindControls(CheckBoxList, "SELECT lId,DocumentName FROM BS_ChekListDocDetail WHERE bDel=0 ORDER BY DocumentName", "DocumentName", "lId");
    }

    public static void FillChekListDocDetail(DropDownList DropDown)
    {

        CDatabase.BindControls(DropDown, "SELECT lId,DocumentName FROM BS_ChekListDocDetail Where bDel= 0 and DocType not in (11) ORDER BY DocumentName", "DocumentName", "lId");
    }

    public static void FillChekListDocDetail(CheckBoxList CheckBoxList)
    {

        CDatabase.BindControls(CheckBoxList, "SELECT lId,DocumentName FROM BS_ChekListDocDetail Where bDel= 0 ORDER BY DocumentName", "DocumentName", "lId");
    }

    public static DataSet FillChekListDocDetail(int DocTypeId)
    {
        return CDatabase.GetDataSet("SELECT lId,DocumentName FROM BS_ChekListDocDetail WHERE DocType=" + DocTypeId + "  AND bDel = 0 ORDER BY DocumentName");
    }

    public static void FillChekListDocType(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,DocType FROM BS_ChekListDocTypeMS WHERE bDel = 0 ORDER BY DocType", "DocType", "lId");
    }

    public static void FillPendingDoc(DropDownList DropDown, int PreAlertId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;
        CDatabase.BindControls(DropDown, "GetPendingDoc", command, "PendingDocName", "DocId");
    }

    public static DataSet FillPendingDoc(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetPendingDoc", command);

    }

    public static DataSet GetReportFieldBylType()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetReportFieldBylType", command);

    }
    public static void FillBEType(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,BETypeName FROM BS_BETypeMS ORDER by BETypeName", "BETypeName", "lId");
    }

    public static void FillDeptName(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,DeptName FROM BS_DeptMS WHERE bDel = 0 ORDER BY DeptName", "DeptName", "lId");
    }

    public static void FillDeptGroup(DropDownList DropDown, int DeptId)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,GroupName FROM BS_DeptGroupMS bDel = 0 ORDER BY GroupName", "GroupName", "lId");
    }

    public static void FillDivision(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,DivisionName FROM BS_DivisionMS WHERE bDel=0 ORDER by DivisionName", "DivisionName", "lId");
    }

    public static void FillDivisionList(ListBox listBox)
    {
        CDatabase.BindControls(listBox, "SELECT lId,DivisionName FROM BS_DivisionMS WHERE bDel=0 ORDER by DivisionName", "DivisionName", "lId");
    }

    public static void FillCustomerFieldList(ListBox listBox, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        CDatabase.BindControls(listBox, "GetAdhocReportCustomerField", command, "FieldName", "lid");
    }

    public static void FillJobRefNum(DropDownList DropDown, Int32 FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        CDatabase.BindControls(DropDown, "GetJobNoForCopyJob", command, "JobRefNo", "lid");
    }

    public static void FillPriority(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid,sName FROM BS_PriorityMS  ORDER by sName", "sName", "lid");
    }

    public static void FillIncoTermDetails(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid,sName FROM BS_IncoTermMS  WHERE bDel = 0 ORDER by sName", "sName", "lid");
    }

    public static void FillCustomerDivision(DropDownList DropDown, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        CDatabase.BindControls(DropDown, "GetCustomerDivision", command, "DivisionName", "lid");
    }

    public static void FillCustomerPlant(DropDownList DropDown, int DivisionId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        CDatabase.BindControls(DropDown, "GetCustomerPlant", command, "PlantName", "lid");
    }
    public static void FillCustomerGSTN(DropDownList DropDown, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        CDatabase.BindControls(DropDown, "GetCustomerGSTN", command, "GSTNNo", "lid");
    }
    public static void FillCustomerUserDivision(DropDownList DropDown, int CustomerUserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;

        CDatabase.BindControls(DropDown, "GetCustomerUserDivision", command, "DivisionName", "lid");
    }

    public static void FillCustomerUserPlant(DropDownList DropDown, int CustomerUserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        //command.Parameters.Add("@DivisonId",SqlDbType.Int).Value    =   DivisionId;
        CDatabase.BindControls(DropDown, "GetCustomerUserPlant", command, "PlantName", "lid");
    }

    public static void FillCustomerUserPlant(DropDownList DropDown, int CustomerUserId, int DivisionId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        CDatabase.BindControls(DropDown, "GetCustomerUserPlant", command, "PlantName", "lid");
    }

    public static void FillExpenseMS(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetExpensesMaster", command, "ExpenseName", "lid");
    }
    public static void FillExpenseMS(DropDownList DropDown, int ExpenseTypeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;

        CDatabase.BindControls(DropDown, "GetExpensesMasterById", command, "ExpenseName", "lid");
    }
    public static void FillExpenseUser(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetExpenseUserList", command, "UserName", "UserId");
    }
    public static void FillCFS(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid ,sName FROM BS_CFSMaster WHERE bDel=0 ORDER BY sName", "sName", "lid");
    }

    public static void FillCFS(DropDownList DropDown, int BranchId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        CDatabase.BindControls(DropDown, "GetCFSByBranch", command, "CFSName", "CFSId");
    }

    public static void FillVehicleType(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid ,sName FROM BS_VehicleMaster WHERE bDel=0 ORDER BY sName", "sName", "lid");
    }

    public static void FillAdditionalField(CheckBoxList CheckBoxList)
    {
        SqlCommand command = new SqlCommand();
        CDatabase.BindControls(CheckBoxList, "GetAdditionalField", command, "FieldName", "FieldId");
    }

    //public static void FillReportField(CheckBoxList CheckBoxList, int CustomerId)
    //{
    //    SqlCommand command = new SqlCommand();
    //    command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
    //    CDatabase.BindControls(CheckBoxList, "GetReportField", command, "FieldName", "FieldId");
    //}

    /// <summary>
    /// Fill Payment Type For Specific Form:
    /// TypeId 1 For DO Payment Type,
    /// TypeId 2 For Duty Payment Type, And
    /// TypeId 3 For Expense Payment Type
    /// </summary>
    /// <param name="DropDown"></param>
    /// <param name="FunctionTypeId"></param>
    public static void FillPaymentType(DropDownList DropDown, int FunctionTypeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@FunctionTypeId", SqlDbType.Int).Value = FunctionTypeId;

        CDatabase.BindControls(DropDown, "GetPaymentType", command, "sName", "lid");
    }
    /// <summary>
    /// Fill All Payment Type
    /// </summary>
    /// <param name="DropDown"></param>
    public static void FillPaymentType(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetPaymentType", command, "sName", "lid");
    }

    public static void FillPackageType(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetPackageType", command, "sName", "lid");
    }

    public static void FillDeliveryType(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetDeliveryType", command, "sName", "lid");
    }

    public static void FillNotificationType(DropDownList DropDown, bool IsBabaji, bool IsCustomer)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@IsBabaji", SqlDbType.Bit).Value = IsBabaji;
        command.Parameters.Add("@IsCustomer", SqlDbType.Bit).Value = IsCustomer;
        CDatabase.BindControls(DropDown, "GetNotificationType", command, "sName", "lid");
    }

    public static void FillNotificationMode(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_NotificationMode Where bDel=0 ORDER by sName", "sName", "lId");
    }

    public static void FillDOStage(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_DOStageMS Where bDel=0 ORDER by lOrder", "sName", "lId");
    }

    public static void FillJobStatus(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();
        CDatabase.BindControls(DropDown, "GetJobStatusMS", command, "StatusName", "lId");
        //CDatabase.BindControls(DropDown, "SELECT lId,StatusName FROM BS_JobStatusMS Where bDel=0 ORDER by StatusName", "StatusName", "lId");
    }
    // Fill All Warehouse
    public static void FillWarehouse(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_WarehouseMS WHERE bStatus=1 and bDel=0 ORDER BY sName", "sName", "lId");
    }
    // Fill Warehouse for specific type i.e. 1. General, 2. Bonded or 3. SEZ
    public static void FillWarehouse(DropDownList DropDown, int WareouseType)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM BS_WarehouseMS WHERE lType=" + WareouseType + " AND bStatus=1 AND bDel=0 ORDER BY sName", "sName", "lId");
    }
    // Fill Warehouse for specific type and Babaji Branchi.e. 1. General, 2. Bonded or 3. SEZ 
    public static void FillWarehouse(DropDownList DropDown, int WareouseType, int BranchId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@WareHouseType", SqlDbType.Int).Value = WareouseType;

        CDatabase.BindControls(DropDown, "GetWareHouseByBranch", command, "WareHouseName", "WareHouseId");
    }

    public static void FillVehicleForNavbharat(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid, sName FROM TR_EquipmentMS WHERE bdel = 0 AND CompanyName LIKE '%BHARAT%'", "sName", "lid");
    }
    public static void FillVehicleForNAVJEEVAN(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid, sName FROM TR_EquipmentMS WHERE bdel = 0 AND CompanyName LIKE '%JEEVAN%'", "sName", "lid");
    }

    public static void FillVehicleNo(DropDownList DropDown, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        CDatabase.BindControls(DropDown, "BS_GetVehiclesForDelivery", command, "VehicleNo", "lid");
    }

    public static void FillVehicleNoForWarehouse(DropDownList DropDown, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        CDatabase.BindControls(DropDown, "BS_GetVehiclesForWarehouse", command, "VehicleNo", "lid");
    }
    #endregion

    #region BSImport Get Detail
    #region Master Data

    public static DataSet GetCities()
    {

        return CDatabase.GetDataSet("SELECT lId,sName AS CityName FROM LocDetail WHERE lTypId=4 AND bDel=0");

    }

    public static DataSet GetCities(string FilterExpression)
    {

        return CDatabase.GetDataSet("SELECT lId,sName AS CityName FROM LocDetail where lTypId=4 and bDel = 0 AND sName like '" + FilterExpression + "%'");

    }

    public static DataSet GetAwaitedDocById(int jobid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = jobid;

        return CDatabase.GetDataSet("GetAwaitedDoc", command);
    }

    public static DataSet GetJobDetail()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("GetJobDetailById", command);
    }

    public static DataSet GetJobDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetJobDetailById", command);
    }

    public static DataSet GetJobDetailByJobRefNo(string JobRefNo)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        return CDatabase.GetDataSet("GetJobDetailByJobRefNo", command);
    }

    public static DataSet GetJobExpenseDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetExpensesDetailsByJobId", command);
    }

    public static DataSet GetExpenseDetailBylId(int ExpenseId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lId", SqlDbType.Int).Value = ExpenseId;

        return CDatabase.GetDataSet("GetExpensesDetailById", command);
    }

    public static DataSet GetJobDetailByPreAlertId(int PreAlertId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;

        return CDatabase.GetDataSet("GetJobDetailByAlertId", command);
    }

    public static DataSet GetJobDetailByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetJobDetailByJobId", command);
    }

    public static DataSet GetJobActivityDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetJobActivityDetailById", command);
    }

    public static DataSet GetAdhocReport()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataSet("GetAdhocReport", cmd);
    }

    public static DataSet GetAdhocCustomerReport()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataSet("GetAdhocCustomerReport", cmd);
    }

    public static DataSet GetReportFieldNameById(int FieldId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        return CDatabase.GetDataSet("GetReportFieldNameById", cmd);
    }

    public static DataView GetUserDetail(string UserId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        return CDatabase.GetDataView("GetUserDetail", command);
    }

    public static DataView GetUserDetailByEmail(string UserEmailID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserEmail", SqlDbType.NVarChar).Value = UserEmailID;

        return CDatabase.GetDataView("GetUserDetailByEmail", command);
    }

    public static DataSet GetKAMUserByCustomerBranch(int CustomerId, int BranchId, int TransMod)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMod;

        return CDatabase.GetDataSet("GetKAMByCustomerBranch", command);
    }

    public static DataView GetCustomerUserDetail(string CustomerUserId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;

        return CDatabase.GetDataView("GetCustomerUserDetail", cmd);
    }

    public static String GetUserNameById(int UserId)
    {
        return CDatabase.ResultInString("SELECT sName AS Value FROM BS_UserMS where lid=" + UserId);
    }

    public static String GetStateNameByCode(int GSTStateCode)
    {
        return CDatabase.ResultInString("SELECT sName AS Value FROM LocDetail WHERE StateCode=" + GSTStateCode);
    }

    public static DataView GetBranchDetail(string BranchId)
    {
        return CDatabase.GetDataView("SELECT * FROM BS_BranchMS where lid=" + BranchId);
    }

    public static String GetBranchNameById(int BranchId)
    {
        return CDatabase.ResultInString("SELECT BranchName AS Value FROM BS_BranchMS where lid=" + BranchId);
    }

    public static DataView GetPortDetail(string PortId)
    {
        return CDatabase.GetDataView("SELECT * FROM BS_PortMS WHERE  lId =" + PortId);
    }

    public static String GetPortNameById(int PortId)
    {
        return CDatabase.ResultInString("SELECT PortName AS Value FROM BS_PortMS where lid=" + PortId);
    }

    public static DataView GetCustomerDetail(string CustomerId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@lId", SqlDbType.Int).Value = CustomerId;

        return CDatabase.GetDataView("GetCustomerBylid", cmd);
    }

    public static String GetCustomerNameById(string CustomerId)
    {
        return CDatabase.ResultInString("SELECT  CustName as Value FROM BS_CustomerMS WHERE lId =" + CustomerId);
    }

    public static String GetCustomerNameEmailById(string CustomerId, ref string strEmail)
    {
        return CDatabase.ResultInString("SELECT  CustName as Value, Email AS SecondValue FROM BS_CustomerMS WHERE lId =" + CustomerId, ref strEmail);
    }

    public static DataSet GetCustomerDetailByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetCustomerDetaiByJobId", cmd);
    }

    public static DataSet GetCustomerUserDetailByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetCustomerUserDetailByJobId", cmd);
    }

    public static DataSet GetCustomerUserDetailByAlertId(int AlertId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@AlertId", SqlDbType.Int).Value = AlertId;

        return CDatabase.GetDataSet("GetCustomerUserDetailByAlertId", cmd);
    }

    /// <summary>
    /// Get Customer User Email/Mobile For Notification
    /// </summary>
    /// <param name="JobId"></param>
    /// <param name="NotificationType"></param>
    /// <param name="NotificationMode">Email/SMS</param>
    /// <returns>DataSet</returns>
    public static DataSet GetCustUserEmailMobileForNotification(int JobId, int NotificationType, int NotificationMode)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        cmd.Parameters.Add("@NotificationType", SqlDbType.Int).Value = NotificationType;
        cmd.Parameters.Add("@Notificationmode", SqlDbType.Int).Value = NotificationMode;

        return CDatabase.GetDataSet("GetUserEmailForNotification", cmd);
    }
    /// <summary>
    /// Get Babaji User Email/Mobile For Job Notification
    /// </summary>
    /// <param name="JobId"></param>
    /// <param name="NotificationType"></param>
    /// <param name="NotificationMode">Email/SMS</param>
    /// <returns>DataSet</returns>
    public static DataSet GetBabajiUserEmailMobileForNotification(int JobId, int NotificationType, int NotificationMode)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        cmd.Parameters.Add("@NotificationType", SqlDbType.Int).Value = NotificationType;
        cmd.Parameters.Add("@Notificationmode", SqlDbType.Int).Value = NotificationMode;

        return CDatabase.GetDataSet("GetBSUserEmailForNotification", cmd);
    }

    public static DataSet GetUserDetailForPasscodeEmail()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("GetUserCodeForMail", cmd);
    }

    public static DataSet GetChecklistDocument()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("GetChekListDocDetail", command);
    }

    public static DataSet GetPCDDocument()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetPCDDocDetail", command);
    }

    public static DataSet GetDivisionDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetDivisionMS", "command");
    }

    public static DataSet GetDepartmentDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetDepartmentMS", "command");
    }

    public static DataSet GetWareHouseDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetWareHouseMS", "command");
    }

    public static DataSet GetWareHouseById(int WarehouseId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = WarehouseId;

        return CDatabase.GetDataSet("GetWareHousebyId", command);
    }

    public static DataSet GetVehicleMasterDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetVehicleMS", "command");
    }

    public static DataSet GetSectorMS()
    {
        SqlCommand command = new SqlCommand();
        //return CDatabase.GetDataSet("GetCustVarMS", "command");

        return CDatabase.GetDataSet("getSectorMS", "command");
    }

    public static DataSet GetSchemeTypeMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetSchemeTypeMS", command);
    }

    public static DataSet GetContainerDetail(string PreAlertId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;
        return CDatabase.GetDataSet("GetContainerDetail", command);
    }

    public static DataSet GetInvoiceDetail(string PreAlertId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;
        return CDatabase.GetDataSet("GetInvoiceDetail", command);
    }

    public static int GetInvoiceCountByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "GetJobInvoiceCount";

        return CDatabase.ResultInInteger(command);
    }

    public static int GetJobIdByJobRefNo(string JobRefNo)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        command.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("GetJobIdByJobRefNo", command, "@output");
        return Convert.ToInt32(SPresult);
    }

    public static int GetJobIDByBOENo(string strBOENo)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BOENo", SqlDbType.NVarChar).Value = strBOENo;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "GetJobIDByBOE";

        return CDatabase.ResultInInteger(command);
    }

    public static DataSet GetProductDetail(string PreAlertId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;
        return CDatabase.GetDataSet("GetProductDetail", command);
    }

    public static DataSet GetIncoTermDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetIncoTermMS", "command");
    }

    public static DataSet GetCustomerDivision(int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        return CDatabase.GetDataSet("GetCustomerDivision", command);
    }

    public static DataSet GetCustomerPlant(int DivisionId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        return CDatabase.GetDataSet("GetCustomerPlant", command);
    }

    public static DataSet GetCustomerConsignee(int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        return CDatabase.GetDataSet("GetCustomerConsignee", command);
    }

    public static DataSet GetUserEmailByDeptJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetUserEmailByDeptJobId", command);
    }

    public static DataSet GetUserEmailByDeptJobIdQueryId(int JobId, int QueryId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (QueryId != 0)
        {
            command.Parameters.Add("@QueryId", SqlDbType.Int).Value = QueryId;
        }
        return CDatabase.GetDataSet("GetUserEmailByDeptJobIdQueryId", command);
    }

    public static DataSet GetJobUserEmailByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetJobUserEmailByJobId", command);
    }

    public static DataSet GetCFSDetail()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("GetCFSDetail", command);
    }

    public static DataSet GetPackageTypeMS()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("GetPackageMaster", command);
    }

    public static DataSet GetExpenseTYpeMS()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("GetExpensesMaster", command);
    }

    public static DataSet GetFieldDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetFieldMaster", "command");
    }

    public static DataSet GetJobType()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetJobTypeMS", "command");
    }

    public static DataSet GetJobStatusMS()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("GetJobStatusMS", "command");
    }

    public static DataSet GetPCDDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetPCDDetail", command);
    }

    public static DataSet GetCustomerPCDDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetCustomerPCDDetail", command);
    }

    public static DataSet GetPassingBondDetail(int Jobid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = Jobid;

        return CDatabase.GetDataSet("GetPassingBondDetail", command);
    }

    public static DataSet GetADCDetail(int Jobid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = Jobid;

        return CDatabase.GetDataSet("GetJobADCDetail", command);
    }

    public static DataSet GetPHODetail(int Jobid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = Jobid;

        return CDatabase.GetDataSet("GetJobPHODetail", command);
    }

    public static DataSet GetADCPHODetail(int Jobid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = Jobid;

        return CDatabase.GetDataSet("GetJobADCPHODetail", command);
    }

    public static DataSet GetAdHocReportDetail(int ReportId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        return CDatabase.GetDataSet("GetReportDetail", command);

    }

    public static DataSet GetReportColumnNamebyId(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("GetReportColumnNamebyId", command);

    }

    public static DataSet GetReportConditionFieldsbyId(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("GetAdhocReportConditionFieldsById", command);

    }

    public static DataSet GetCustomerPlantAddressById(int AddressId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@AddressId", SqlDbType.Int).Value = AddressId;
        return CDatabase.GetDataSet("GetCustomerPlantAddressById", command);

    }

    public static DataSet GetJobPlantAddress(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetJobPlantAddress", command);

    }
    public static DataSet GetJobPlantAddressList(string strJobIdList)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobIDList", SqlDbType.NVarChar).Value = strJobIdList;
        return CDatabase.GetDataSet("GetJobPlantAddressForDispatch", command);

    }

    public static string GetDocumentPath(int DocumentId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
        return CDatabase.GetSPOutPut("GetDocPathById", command, "@OutPut");
    }
    #endregion

    #region Form Detail Data

    public static DataView GetCustomerRequestById(int PreAlertID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@PreAlertID", SqlDbType.Int).Value = PreAlertID;

        return CDatabase.GetDataView("GetCustomerRequestByID", cmd);
    }

    public static DataView GetPreAlertById(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetPreAlertById", cmd);
    }

    public static DataView GetJobDetailForCustomer(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForCustomer", cmd);
    }

    public static DataView GetJobDetailForIGM(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForIGM", cmd);
    }

    public static DataView GetJobDetailForChecklistPrepare(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForChecklistPrepare", cmd);
    }

    public static DataView GetJobDetailForDOCollection(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForDOCollection", cmd);
    }

    public static DataView GetJobDetailForDutyRequest(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForDutyRequest", cmd);
    }

    public static DataView GetJobDetailForExamine(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForExamine", cmd);
    }

    public static DataView GetJobHistory(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobHistoryById", cmd);
    }

    public static DataView CheckJobCreated(int PreAlertId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;

        return CDatabase.GetDataView("CheckJobCreated", cmd);
    }

    public static DataSet GetFirstCheckDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetFirstCheckDetail", command);

    }

    public static DataSet GetPassingQuery(int JobId)

    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetPassingQuery", command);

    }

    public static DataSet GetPassingQueryByQueryId(int QueryId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@QueryId", SqlDbType.Int).Value = QueryId;

        return CDatabase.GetDataSet("GetPassingQueryById", command);

    }

    public static DataView GetJobDetailForDelivery(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForDelivery", cmd);
    }

    public static DataView GetJobDetailForPCDToCustomer(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForPCDToCustomer", cmd);
    }

    public static DataView GetJobDetailForPCABillingAdvice(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForPCABillingAdvice", cmd);
    }

    public static DataView GetJobDetailForPCDToScrutiny(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForPCDToScrutiny", cmd);
    }

    public static DataView GetJobDetailForPCDBilling(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForPCDBilling", cmd);
    }

    public static DataView GetJobDetailForPCDDispatch(int JobId, Boolean DispatchType)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@PCDToCustomer", SqlDbType.Bit).Value = DispatchType;

        return CDatabase.GetDataView("GetJobDetailForPCDDispatch", cmd);
    }

    public static SqlDataReader GetJobAdditionalFields(int JobId)
    {
        SqlDataReader drJobFields;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return drJobFields = CDatabase.GetDataReader("GetJobFields", cmd);

    }
    public static SqlDataReader GetAdhocReportFieldGroup()
    {
        SqlDataReader drGroupFields;
        SqlCommand cmd = new SqlCommand();
        return drGroupFields = CDatabase.GetDataReader("GetFieldGroup", cmd);
    }
    public static SqlDataReader GetAdhocReportChildNode(string ParentNode, int ReportType, int CustomerID)
    {
        SqlDataReader drChildNode;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;
        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
        return drChildNode = CDatabase.GetDataReader("GetReportChildNodeByParentNode", cmd);

    }

    public static DataSet GetAdhocReportFieldGroupTest()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("GetFieldGroup", cmd);
    }

    public static DataSet GetAdhocReportChildNodeTest(string ParentNode, int ReportType, int CustomerID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;
        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
        return CDatabase.GetDataSet("GetReportChildNodeByParentNode", cmd);
    }

    public static DataSet GetJobBasicDetail(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetJobBasicDetail", cmd);
    }
    public static DataSet GetJobDetailByRefNoList(string JobRefNoList)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobList", SqlDbType.NVarChar).Value = JobRefNoList;

        return CDatabase.GetDataSet("GetJobDetailByRefNoList", cmd);
    }

    public static DataSet GetJobDetailByJobIDList(string JobIDList)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobIDList", SqlDbType.NVarChar).Value = JobIDList;

        return CDatabase.GetDataSet("GetJobDetailByJobIDList", cmd);
    }

    public static DataSet GetJobDetailByList(string JobIDList)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobIDList", SqlDbType.NVarChar).Value = JobIDList;

        return CDatabase.GetDataSet("GetJobDetailByList", cmd);
    }

    public static SqlDataReader GetReportConditionFields(int ReportId)
    {
        SqlDataReader drReportCondition;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;

        return drReportCondition = CDatabase.GetDataReader("GetAdhocReportConditionFields", cmd);

    }

    public static DataSet GetReporFieldsCount(int ReportId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;

        return CDatabase.GetDataSet("GetAdhocReportConditionFields", cmd);
    }

    public static DataView GetAdhocReport(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
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

        return CDatabase.GetDataView("rptAdHocReport", cmd);
    }

    public static DataView GetAdhocReportCust(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
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

        return CDatabase.GetDataView("rptAdHocReportCust", cmd);
    }

    public static DataView GetAdhocCustomerReport(int ReportId, DateTime DateFrom, DateTime DateTo, string CustomerReportFilter, int CustomerUserId, int FinYear)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateFrom;
        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTo;
        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = CustomerReportFilter;
        cmd.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataView("rptAdHocCustomerReport", cmd);
    }

    public static DataView GetPlantAddress(int PlantId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;

        return CDatabase.GetDataView("GetCustomerPlantAddress", cmd);
    }

    public static DataSet GetJobAllStatus(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetJobALLStatusById", cmd);
    }

    public static int updAdhocReportLastGeneratedBy(int ReportId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updAdhocReportGeneratedBy", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Various Pending/Completed Job Count

    public static DataSet GetPendingCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("GetPendingCountForUser", command);

        return dsPending;
    }

    public static DataSet GetPendingPCDCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("GetPendingPCDCountForUser", command);

        return dsPending;
    }

    public static DataSet GetPendingPCDDispatchCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("GetPendingPCDBillingDispatchCount", command);

        return dsPending;
    }

    public static int GetPendingPreAlertCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingPreAlertCount", command);

        return Count;
    }

    public static int GetPendingJobCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingJobCountForUser", command);

        return Count;
    }

    public static int GetPendingIGMCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingIGMCountForUser", command);

        return Count;
    }

    public static int GetPendingOtherJobCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingOtherJobCount", command);

        return Count;
    }

    public static int GetPendingDOCollectionCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingDoCountForUser", command);

        return Count;
    }

    public static int GetPendingChecklistCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingChecklistCountForUser", command);

        return Count;
    }

    public static int GetPendingChecklistCountForApproval(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingChecklistCountForApproval", command);

        return Count;
    }

    public static int GetRejectedChecklistCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetRejectedChecklistCount", command);

        return Count;
    }

    public static int GetPendingNotingCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingNotingCountForUser", command);

        return Count;
    }

    public static int GetPendingPassingCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingPassingCountForUser", command);

        return Count;
    }
    public static int GetPendingDutyCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingDutyCountForUser", command);

        return Count;
    }
    public static int GetPendingFirstCheckCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingFirstCheckCountForUser", command);

        return Count;
    }
    public static int GetPendingExamineCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingExamineCountForUser", command);

        return Count;
    }
    public static int GetPendingDeliveryCount(int UserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        Count = CDatabase.GetSPCount("GetPendingDeliveryCountForUser", command);

        return Count;
    }

    public static int GetPendingDocCount(int UserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetPendingDocCount", command);

        return Count;
    }
    public static int GetPendingCustomerDocCount(int CustId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;
        Count = CDatabase.GetSPCount("GetPendingDocCountForCustomer", command);

        return Count;
    }

    public static int GetPendingDocCountForCustomerUser(int CustUserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetPendingDocCountForCustomerUser", command);

        return Count;
    }

    public static int GetPendingCustPreAlertCount(int CustUserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetPendingCustPreAlertCount", command);

        return Count;
    }

    public static int GetChecklistApprovalCountForCustomer(int CustId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;
        Count = CDatabase.GetSPCount("GetChecklistCountForApprovalByCustomer", command);

        return Count;
    }

    public static int GetChecklistApprovalCountForCustomerUser(int CustomerUserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetChecklistCountForApprovalByCustomerUser", command);

        return Count;
    }

    public static int GetDutyStatusCustomerCount(int CustId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;
        Count = CDatabase.GetSPCount("GetPendingCountCustomerDuty", command);

        return Count;
    }

    public static int GetDutyStatusCustomerUserCount(int CustomerUserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetPendingCountCustomerUserDuty", command);

        return Count;
    }

    public static int GetPreAlertCustomerCount(int CustId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;
        Count = CDatabase.GetSPCount("GetPreAlertCustomerCount", command);

        return Count;
    }

    public static int GetPreAlertCustomerUserCount(int CustUserId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustUserId;
        Count = CDatabase.GetSPCount("GetPreAlertCustomerUserCount", command);

        return Count;
    }

    public static int GetUnderClearanceCustUserCnt(int CustUserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetUnderClearanceCustUserCnt", command);

        return Count;
    }

    public static int GetAllBillReturnDetailCount(int CustUserId, int FinYearId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        Count = CDatabase.GetSPCount("GetBillReturnDetailCount", command);

        return Count;
    }

    #endregion
    #endregion

    #region BSImport Add-Del-Mod

    #region Master Data Add-Del-Mod

    public static void AddErrorLog(int JobId, string ErrorTypeName, string ProcedureName, string ErrorMessage,
           string ErrorDescription, int lUser)
    {
        SqlCommand command = new SqlCommand();

        int rowEffected = 0;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@sTypeName", SqlDbType.NVarChar).Value = ErrorTypeName;
        command.Parameters.Add("@sError_Procedure", SqlDbType.NVarChar).Value = ProcedureName;
        command.Parameters.Add("@sError_Message", SqlDbType.NVarChar).Value = ErrorMessage;
        command.Parameters.Add("@sDescription", SqlDbType.NVarChar).Value = ErrorDescription;
        command.Parameters.Add("lUser", SqlDbType.Int).Value = lUser;

        rowEffected = CDatabase.ExecuteSP("insJobErrorStats", command);

    }

    public static int AddConsignee(string ConsigneeName, string Address, string ContactNo, string Email,
           string IECNo, string ConsigneeGroup, string IncomeTaxNum, string Code, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = ConsigneeName;
        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@IECNo", SqlDbType.NVarChar).Value = IECNo;
        command.Parameters.Add("@ConsigneeGroup", SqlDbType.NVarChar).Value = ConsigneeGroup;
        command.Parameters.Add("@IncomeTaxNum", SqlDbType.NVarChar).Value = IncomeTaxNum;
        command.Parameters.Add("@sCode", SqlDbType.NVarChar).Value = Code;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insConsigneeMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int AddBranch(string BranchName, string CityId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BranchName", SqlDbType.NVarChar).Value = BranchName;
        command.Parameters.Add("@CityId", SqlDbType.Int).Value = CityId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBranch", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddBranchPort(int BranchId, int PortId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBranchPort", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddBranchWarehouse(int BranchId, int WarehouseId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = WarehouseId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBranchWarehouse", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddUserBranch(int UserId, int BranchId, int CreatedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insUserBranch", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddUserBranchAll(int UserId, bool AllBranch, int CreatedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@AllBranch", SqlDbType.Bit).Value = AllBranch;
        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insUserBranchAll", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddUserCustomer(int UserId, int CustomerId, int CreatedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insUserCustomer", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddUserCustomerAll(int UserId, bool AllCustomer, int CreatedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@AllCustomer", SqlDbType.Bit).Value = AllCustomer;

        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insUserCustomerAll", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteUserCustomerAll(int UserId, bool AllCustomer, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@AllCustomer", SqlDbType.Bit).Value = AllCustomer;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delUserCustomerAll", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDDocument(string DocumentName, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PCDDocName", SqlDbType.NVarChar).Value = DocumentName;
        //command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDDocDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdatePCDDocument(int lid, string DocumentName, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@PCDDocName", SqlDbType.NVarChar).Value = DocumentName;
        //command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updPCDDocDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeletePCDDocument(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delPCDDocDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddChecklistDocument(string DocumentName, int DocType, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@DocumentName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insChekListDocDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateChecklistDocument(int lid, string DocumentName, int DocType, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@DocumentName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updChekListDocDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteChecklistDocument(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delChekListDocDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddWareHouse(string WareHouseName, string WareHCode, string sAddress, string sContactName, string sContactNumber,
            string sEmail, int lType, bool bStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@WareHName", SqlDbType.NVarChar).Value = WareHouseName;
        command.Parameters.Add("@WHCode", SqlDbType.NVarChar).Value = WareHCode;

        command.Parameters.Add("@sAddress", SqlDbType.NVarChar).Value = sAddress;
        command.Parameters.Add("@sContactName", SqlDbType.NVarChar).Value = sContactName;
        command.Parameters.Add("@sContactNumber", SqlDbType.NVarChar).Value = sContactNumber;
        command.Parameters.Add("@sEmailId", SqlDbType.NVarChar).Value = sEmail;

        command.Parameters.Add("@lType", SqlDbType.Int).Value = lType;
        command.Parameters.Add("@bStatus", SqlDbType.Bit).Value = bStatus;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insWareHouseMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateWareHouse(int lid, string WareHouseName, string WHCode, string sAddress, string sContactName, string sContactNumber,
            string sEmail, int lType, bool bStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@SName", SqlDbType.NVarChar).Value = WareHouseName;
        command.Parameters.Add("@WHCode", SqlDbType.NVarChar).Value = WHCode;
        command.Parameters.Add("@sAddress", SqlDbType.NVarChar).Value = sAddress;
        command.Parameters.Add("@sContactName", SqlDbType.NVarChar).Value = sContactName;
        command.Parameters.Add("@sContactNumber", SqlDbType.NVarChar).Value = sContactNumber;
        command.Parameters.Add("@sEmailId", SqlDbType.NVarChar).Value = sEmail;

        command.Parameters.Add("@lType", SqlDbType.Int).Value = lType;
        command.Parameters.Add("@bStatus", SqlDbType.Bit).Value = bStatus;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updWareHouseMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteWareHouse(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DELWareHouse", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddSectorMS(string SectorName, string SectorCode, string Remarks, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = SectorName;
        command.Parameters.Add("@sCode", SqlDbType.NVarChar).Value = SectorCode;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insSectorMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateSectorMS(int lid, string SectorName, string SectorCode, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = SectorName;
        command.Parameters.Add("@sCode", SqlDbType.NVarChar).Value = SectorCode;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updSectorMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteSectorMS(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delVarintMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddVehicle(string VehicleName, string VehicleRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Vehiclename", SqlDbType.NVarChar).Value = VehicleName;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = VehicleRemark;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insVehicleMaster", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateVehicle(int lid, string VehicleName, string VehicleRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@Lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@VehicleName", SqlDbType.NVarChar).Value = VehicleName;
        command.Parameters.Add("@VehicleRemark", SqlDbType.NVarChar).Value = VehicleRemark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updVehicle", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteVehicle(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string Spresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        Spresult = CDatabase.GetSPOutPut("DELVehicle", command, "@OutPut");

        return Convert.ToInt32(Spresult);
    }
    public static int UpdatePCDDoc(int JobId, int DocumentId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updPCDDocument", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int AddDepartment(string DepartmentName, string DeptCode, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@DeptName", SqlDbType.NVarChar).Value = DepartmentName;
        command.Parameters.Add("@DeptCode", SqlDbType.NVarChar).Value = DeptCode;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDepartment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDepartment(int lid, string DepartmentName, string DeptCode, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@DeptName", SqlDbType.NVarChar).Value = DepartmentName;
        command.Parameters.Add("@DeptCode", SqlDbType.NVarChar).Value = DeptCode;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDepartment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDepartmnet(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delDepartment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDivision(string DivisionName, string DivisionCode, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@DivisionName", SqlDbType.NVarChar).Value = DivisionName;
        command.Parameters.Add("@DivisionCode", SqlDbType.NVarChar).Value = DivisionCode;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDivision(int lid, string DivisionName, string DivisionCode, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@DivisionName", SqlDbType.NVarChar).Value = DivisionName;
        command.Parameters.Add("@DivisionCode", SqlDbType.NVarChar).Value = DivisionCode;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDivision(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddSchemeType(string SchemeType, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@SchemeType", SqlDbType.NVarChar).Value = SchemeType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insSchemeType", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateSchemeType(int lid, string SchemeType, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@SchemeType", SqlDbType.NVarChar).Value = SchemeType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updLicensesType", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteSchemeType(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delSchemeType", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobDailyActivity(int JobId, string DailyProgress, string DocumentPath, int SummaryStatus,
            Boolean VisibleToCustomer, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@DocumentPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@SummaryStatus", SqlDbType.Int).Value = SummaryStatus;
        command.Parameters.Add("@VisibleToCustomer", SqlDbType.Bit).Value = VisibleToCustomer;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDailyActivity(int ActivityId, int StatusId, string DailyProgress, Boolean VisibleToCustomer, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@VisibleToCustomer", SqlDbType.Bit).Value = VisibleToCustomer;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDailyActivityAdmin(int ActivityId, int StatusId, string DailyProgress, Boolean VisibleToCustomer, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@VisibleToCustomer", SqlDbType.Bit).Value = VisibleToCustomer;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDailyActivityAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteJobDailyActivity(int ActivityId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delJobDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerConsignee(int CustomerId, int ConsigneeId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerConsignee", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerSector(int CustomerId, int SectorId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@VariantId", SqlDbType.Int).Value = SectorId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerSector", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerDivision(int CustomerId, string strDivisionName, string strDivisionCode, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionName", SqlDbType.NVarChar).Value = strDivisionName;
        command.Parameters.Add("@DivisionCode", SqlDbType.NVarChar).Value = strDivisionCode;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCustomerDivision(int DivisionId, int CustomerId, string strDivisionName, string strDivisionCode, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionName", SqlDbType.NVarChar).Value = strDivisionName;
        command.Parameters.Add("@DivisionCode", SqlDbType.NVarChar).Value = strDivisionCode;

        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomerDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCustomerDivision(int DivisionId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;

        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomerDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerPlant(int CustomerId, int DivisonId, string strPlantName, string strPlantCode, string strGSTNNo,
        bool ChecklistApproval, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisonId;
        command.Parameters.Add("@PlantName", SqlDbType.NVarChar).Value = strPlantName;
        command.Parameters.Add("@PlantCode", SqlDbType.NVarChar).Value = strPlantCode;
        command.Parameters.Add("@GSTNNo", SqlDbType.NVarChar).Value = strGSTNNo;
        command.Parameters.Add("@ChecklistApproval", SqlDbType.NVarChar).Value = ChecklistApproval;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerPlant", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddCustomerBillDispatchCondition(int CustomerId, int PhysicalBillStatus, int EBillStatus, int PortalStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@IsDispatch", SqlDbType.Bit).Value = PhysicalBillStatus;
        command.Parameters.Add("@IsEbill", SqlDbType.Bit).Value = EBillStatus;
        command.Parameters.Add("@IsPortal", SqlDbType.Bit).Value = PortalStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insCustomerDispatch", command, "@Output");
        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetCustomerBillDispatchCondition(int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        return CDatabase.GetDataSet("BS_GetCustomerDispatch", command);

    }
    public static int AddTransportBillTo(int CustomerId, bool BillToBabaji, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@BillToBabaji", SqlDbType.Bit).Value = BillToBabaji;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insCustomerTransBillTo", command, "@Output");
        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetTransportBillTo(int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        return CDatabase.GetDataSet("BS_GetCustomerTransBillTo", command);

    }
    public static int UpdateCustomerPlant(int PlantId, int DivisionId, string strPlantName, string strPlantCode,
        string strGSTNNo, bool ChecklistApproval, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantName", SqlDbType.NVarChar).Value = strPlantName;
        command.Parameters.Add("@PlantCode", SqlDbType.NVarChar).Value = strPlantCode;
        command.Parameters.Add("@GSTNNo", SqlDbType.NVarChar).Value = strGSTNNo;
        command.Parameters.Add("@ChecklistApproval", SqlDbType.NVarChar).Value = ChecklistApproval;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomerPlant", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCustomerPlant(int PlantId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomerPlant", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddIncoTerms(string sName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insIncoTerms", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateIncoTerms(int lid, string sName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updIncoTerms", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteIncoTerms(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delIncoTerms", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerUserDivision(int CustomerUserId, int DivisionId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerUserDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCustomerUserDivision(int CustomerUserId, int DivisionId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delCustomerUserDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerUserPlant(int CustomerUserId, int PlantId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerUserPlant", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCustomerUserPlant(int CustomerUserId, int PlantId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delCustomerUserPlant", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerUserNotification(int CustomerUserId, int NotificationTypeId, int NotificationMode, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@NotificationTypeId", SqlDbType.Int).Value = NotificationTypeId;
        command.Parameters.Add("@NotificationMode", SqlDbType.Int).Value = NotificationMode;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerUserNotification", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCustomerUserNotification(int NotificationId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = NotificationId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomerUserNotification", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCFSDetail(string sName, int CFSUserId, int lUserId, string sRemark)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@CFSUserId", SqlDbType.Int).Value = CFSUserId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("insCFSMaster", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int DeletFromCFSMDetails(int lid, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("delCFSMaster", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdateCFSMDetails(int lid, int CFSUserId, string sName, string sRemark, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@CFSUserId", SqlDbType.Int).Value = CFSUserId;
        command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updCFSMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int AddPassignStageMaster(string sName, int lUserId, string sRemark)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@stageName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("insBS_PassingStageMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int DeletPassignStageMaster(int lid, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("delPassingStageMaster", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdatePassignStageMaster(int lid, string sName, string sRemark, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@stageName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updPassingStageMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int AddPackageTypeMS(string sName, string sCode, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@sCode", SqlDbType.NVarChar).Value = sCode;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("insPackageTypeMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdatePackageTypeMS(int lid, string sName, string sCode, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@sCode", SqlDbType.NVarChar).Value = sCode;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updPackageTypeMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int DeletePackageTypeMS(int lid, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("delPackageTypeMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int AddExpensesMDetails(string sName, int lUserId, string sRemark)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@expenseName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("insBS_ExpensesMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int DeletExpensesMDetails(int lid, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("delExpensesMaster", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdateExpensesMDetails(int lid, string sName, string sRemark, int lUserId)
    {
        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@expenseName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@remark", SqlDbType.NVarChar).Value = sRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updExpensesMS", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int AddCustomerNotes(int CustId, string strNotes, string strFilePath, int NoteType, DateTime StartDate, DateTime ValidDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = strNotes;
        command.Parameters.Add("@NoteType", SqlDbType.Int).Value = NoteType;
        command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = strFilePath;

        if (StartDate != DateTime.MinValue)
        {
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
        }
        if (ValidDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ValidTillDate", SqlDbType.Date).Value = ValidDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerNotes", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddTransporterNotes(int TransporterID, int ContractCustomerID, string strNotes, string strFilePath, int NoteType, DateTime StartDate, DateTime ValidDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustId", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@ContractCustomerID", SqlDbType.Int).Value = ContractCustomerID;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = strNotes;
        command.Parameters.Add("@NoteType", SqlDbType.Int).Value = NoteType;
        command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = strFilePath;

        if (StartDate != DateTime.MinValue)
        {
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
        }
        if (ValidDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ValidTillDate", SqlDbType.Date).Value = ValidDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerNotes", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCustomerNotes(int NoteId, string strNotes, DateTime StartDate, DateTime ValidDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@NoteId", SqlDbType.Int).Value = NoteId;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = strNotes;

        if (StartDate != DateTime.MinValue)
        {
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = StartDate;
        }
        if (ValidDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ValidTillDate", SqlDbType.Date).Value = ValidDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomerNotes", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobExpensesDetails(int JobId, int TypeId, float Amount, string receiptNo, string paidTo,
        int ReceiptType, string Location, int PaymentType, string ReceiptAmount, DateTime dtReceiptDate,
        string ChequeNo, DateTime dtChequeDate, bool IsBillable, string strRemark, Int32 HODID, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = TypeId;
        command.Parameters.Add("@Amount", SqlDbType.Float).Value = Amount;
        command.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar).Value = receiptNo;
        command.Parameters.Add("@PaidTo", SqlDbType.NVarChar).Value = paidTo;
        command.Parameters.Add("@ReceiptType", SqlDbType.Int).Value = ReceiptType;
        command.Parameters.Add("@Location", SqlDbType.NVarChar).Value = Location;
        command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@ReceiptAmount", SqlDbType.NVarChar).Value = ReceiptAmount;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillable;
        command.Parameters.Add("@HODID", SqlDbType.Int).Value = HODID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        if (dtReceiptDate != DateTime.MinValue)
        {
            command.Parameters.Add("@dtReceiptDate", SqlDbType.DateTime).Value = dtReceiptDate;
        }
        if (dtChequeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@dtChequeDate", SqlDbType.DateTime).Value = dtChequeDate;
        }
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("insExpenseDetail", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdateJobExpensesDetails(int ExpenseId, int TypeId, float Amount, string receiptNo, string paidTo, int ReceiptType,
       string Location, int PaymentType, string ReceiptAmount, DateTime dtReceiptDate, string ChequeNo,
       DateTime dtChequeDate, bool IsBillable, string strRemark, int HODID, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = TypeId;
        command.Parameters.Add("@Amount", SqlDbType.Float).Value = Amount;
        command.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar).Value = receiptNo;
        command.Parameters.Add("@PaidTo", SqlDbType.NVarChar).Value = paidTo;
        command.Parameters.Add("@ReceiptType", SqlDbType.Int).Value = ReceiptType;
        command.Parameters.Add("@Location", SqlDbType.NVarChar).Value = Location;
        command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@ReceiptAmount", SqlDbType.NVarChar).Value = ReceiptAmount;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("IsBillable", SqlDbType.Bit).Value = IsBillable;
        command.Parameters.Add("@HODID", SqlDbType.Int).Value = HODID;
        command.Parameters.Add("Remark", SqlDbType.NVarChar).Value = strRemark;

        if (dtReceiptDate != DateTime.MinValue)
        {
            command.Parameters.Add("@dtReceiptDate", SqlDbType.DateTime).Value = dtReceiptDate;
        }
        if (dtChequeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@dtChequeDate", SqlDbType.DateTime).Value = dtChequeDate;
        }
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updExpenseDetailUser", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdateJobExpensesMgmt(int ExpenseId, decimal decAmount, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        command.Parameters.Add("@Amount", SqlDbType.Float).Value = decAmount;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updExpenseDetailMgmt", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdateJobExpensesMgmt(int ExpenseId, int ApprovalStatusId, decimal decAmount, string ApprovalRemark, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        command.Parameters.Add("@ApprovalStatusId", SqlDbType.Int).Value = ApprovalStatusId;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = decAmount;
        command.Parameters.Add("@ApprovalRemark", SqlDbType.NVarChar).Value = ApprovalRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updExpenseDetailMgmt", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }


    public static int DelJobExpensesDetails(int JobExpenseId, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = JobExpenseId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@outVal", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("delJobExpenseDetail", command, "@outVal");

        return Convert.ToInt32(sp_result);
    }

    public static DataSet GetAdditionalExpensesReportNew(DateTime ReportDate, int PortId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("rptTestAditionalNew", command);

    }

    public static DataSet GetAdditionalExpensesReportBranch(DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranch", command);

    }
    public static DataSet GetAdditionalExpensesReportHOD(DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranchHOD", command);

    }
    public static DataSet GetAdditionalExpensesReportHOD(Int32 ExpenseTypeId, DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranchHODTemp", command);

    }
    public static DataSet GetAdditionalExpensesReportMgmt(DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranchMgmt", command);

    }
    public static DataSet GetAdditionalExpensesReportMgmt(int ExpenseID, int CategoryID, DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@ExpenseID", SqlDbType.Int).Value = ExpenseID;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranchMgmt", command);

    }
    public static DataSet GetAdditionalExpensesReportMgmt3(int ExpenseID, int ExpenseUserId, DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@ExpenseID", SqlDbType.Int).Value = ExpenseID;
        command.Parameters.Add("@ExpenseUserId", SqlDbType.Int).Value = ExpenseUserId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranchMgmt3", command);

    }
    public static DataSet GetAdditionalExpensesReportMgmt(int CategoryID, DateTime ReportDate, int BranchId, int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetPendingExpenseBranchMgmtTemp", command);

    }
    public static int ApproveAdditionalExpensesHOD(DateTime ReportDate, int BranchId, int UserId)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }

        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdditionalExpenseApprovalHOD", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int ApproveAdditionalExpensesHOD(Int32 ExpenseTypeId, DateTime ReportDate, int BranchId, int UserId, int Finyear)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@ExpenseTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Finyear", SqlDbType.Int).Value = Finyear;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdditionalExpenseApprovalHODTemp", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int ApproveAdditionalExpensesMgmt(DateTime ReportDate, int BranchId, int UserId)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }

        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdditionalExpenseApprovalMgmt", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int ApproveAdditionalExpensesMgmt_Del(int ExpenseTypeID, DateTime ReportDate, int BranchId, int UserId, int Finyear)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@ExpenseTypeID", SqlDbType.Int).Value = ExpenseTypeID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Finyear", SqlDbType.Int).Value = Finyear;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdditionalExpenseApprovalMgmt", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int ApproveAdditionalExpensesMgmt(int ExpenseTypeID, DateTime ReportDate, int BranchId, int ExpenseUserId, int UserId, int Finyear)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }
        command.Parameters.Add("@ExpenseTypeID", SqlDbType.Int).Value = ExpenseTypeID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ExpenseUserId", SqlDbType.Int).Value = ExpenseUserId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Finyear", SqlDbType.Int).Value = Finyear;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdditionalExpenseApprovalMgmt", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int ApproveAdditionalExpenses(DateTime ReportDate, int BranchId, int UserId)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        if (ReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        }

        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdditionalExpenseApproval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int MailAdditionalExpenses(DateTime ReportDate, int PortId, int UserId)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = ReportDate;
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("mailAdditionalExpenseAccount", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetAdditionalExpensesReport()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("rptTestAditional", command);

    }
    public static int AddJobExpensesStatus(int ExpenseId, int StatusId, string Remark, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("BS_insExpenseStatus", command, "@Output");

        return Convert.ToInt32(sp_result);
    }
    public static int UpdateJobExpensesAmount(int ExpenseId, decimal Amount, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updExpenseAmount", command, "@Output");

        return Convert.ToInt32(sp_result);
    }
    public static int AddHoliday(string HolidayName, DateTime HolidayDate, int BranchId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        int temp = 0;
        try
        {
            command.Parameters.Add("@HolidayName", SqlDbType.NVarChar).Value = HolidayName;
            command.Parameters.Add("@HolidayDate", SqlDbType.DateTime).Value = HolidayDate;
            command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
            command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
            command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

            temp = Convert.ToInt32(CDatabase.GetSPOutPut("insHoliday", command, "@OutPut"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return temp;
    }

    public static int AddFieldMaster(string FieldName, int FieldType, int ModuleId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FieldName", SqlDbType.NVarChar).Value = FieldName;
        command.Parameters.Add("@FieldType", SqlDbType.Int).Value = FieldType;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insFieldMaster", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFieldMaster(int FieldId, string FieldName, int FieldType, int ModuleId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@FieldName", SqlDbType.NVarChar).Value = FieldName;
        command.Parameters.Add("@FieldType", SqlDbType.Int).Value = FieldType;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updFieldMaster", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFieldMaster(int FieldId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelFieldMaster", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerAdditionalField(int CustomerId, string FieldId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@FieldId", SqlDbType.NVarChar).Value = FieldId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerAdditionalField", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddCustomerReportField(int CustomerId, string FieldId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@FieldId", SqlDbType.NVarChar).Value = FieldId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerReportField", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobType(string JobTypeName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobTypeName", SqlDbType.NVarChar).Value = JobTypeName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobType", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobType(int lid, string JobTypeName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@JobTypeName", SqlDbType.NVarChar).Value = JobTypeName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobType", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteJobType(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delJobType", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    /// <summary>
    /// Job Status Master Setup -Feb.04.2014
    /// </summary>
    /// <param name="JobStausName"></param>
    /// <param name="UserId"></param>
    /// <returns></returns>
    public static int AddJobStatus(string JobStatusName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobStatusName", SqlDbType.NVarChar).Value = JobStatusName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobStatus(int lid, string JobStatusName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@JobStatusName", SqlDbType.NVarChar).Value = JobStatusName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteJobStatus(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delJobStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPlantAddress(int PlantId, int AddressType, string ContactPerson, string Address1, string Address2,
        string City, string PinCode, string MobileNo, string Email, bool IsDefaultAddress, bool IsCommonAddress, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@AddressType", SqlDbType.Int).Value = AddressType;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = Address1;
        command.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = Address2;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@PinCode", SqlDbType.NVarChar).Value = PinCode;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@IsDefault", SqlDbType.NVarChar).Value = IsDefaultAddress;
        command.Parameters.Add("@IsCommonAddress", SqlDbType.NVarChar).Value = IsCommonAddress;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("insCustomerPlantAddress", command, "@Output");

        return Convert.ToInt32(sp_result);
    }

    public static int UpdatePlantAddress(int AddressId, string ContactPerson, string Address1, string Address2,
            string City, string PinCode, string MobileNo, string Email, bool IsDefaultAddress, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@AddressId", SqlDbType.Int).Value = AddressId;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = Address1;
        command.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = Address2;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@PinCode", SqlDbType.NVarChar).Value = PinCode;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@IsDefault", SqlDbType.NVarChar).Value = IsDefaultAddress;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("updCustomerPlantAddress", command, "@Output");

        return Convert.ToInt32(sp_result);
    }

    public static int DeletePlantAddress(int AddressId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@AddressId", SqlDbType.Int).Value = AddressId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelCustomerPlantAddress", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Job Detail Add-Del-Mod

    public static int AddCustomerRequest(int CustomerId, int ConsigneeId, int PortId, int TransMode, int DivisionId,
       int PlantId, int BabajiBranchId, int PortOfLoadingId, string CustRefNo, string CustInstruction, DateTime ETADate,
       int BOETypeID, int KAMID, string strADCode, string IECBranchCode, string BankName, string PreAlertDirPath,
       string CustUserEmail, string LoginIP, int lUser, int lUserType)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@Port", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustInstruction", SqlDbType.NVarChar).Value = CustInstruction;
        command.Parameters.Add("@BOETypeId", SqlDbType.Int).Value = BOETypeID;
        command.Parameters.Add("@KAMId", SqlDbType.Int).Value = KAMID;

        command.Parameters.Add("@ADCode", SqlDbType.NVarChar).Value = strADCode;
        command.Parameters.Add("@IECBranchCode", SqlDbType.NVarChar).Value = IECBranchCode;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        command.Parameters.Add("@PreAlertDirPath", SqlDbType.NVarChar).Value = PreAlertDirPath;
        command.Parameters.Add("@CustUserEmail", SqlDbType.NVarChar).Value = CustUserEmail;
        command.Parameters.Add("@LoginIP", SqlDbType.NVarChar).Value = LoginIP;
        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerAlert", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }


    public static int DelCustomerRequest(int RequestId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@RequestId", SqlDbType.NVarChar).Value = RequestId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delCustomerAlert", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static DataSet AddPreAlert(int CustomerId, int ConsigneeId, int PortId, int TransMode, int DivisionId, int PlantId,
        int BabajiBranchId, int PortOfLoadingId, string CustRefNo, string CustInstruction, DateTime ETADate, string Remark, int lUser, int lUserType)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@Port", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustInstruction", SqlDbType.NVarChar).Value = CustInstruction;
        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@lUserType", SqlDbType.Int).Value = lUserType;

        return CDatabase.GetDataSet("insPreAlertDoc", command);


    }
    //new
    public static int UpdatePreAlert(int AlertId, int CustomerId, int ConsigneeId, int PortId, int TransMode, int DivisionId, int PlantId,
        int BabajiBranchId, int PortOfLoadingId, string CustRefNo, string CustomerInstructions, DateTime ETADate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@AlertId", SqlDbType.Int).Value = AlertId;
        command.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@Port", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.NVarChar).Value = PortOfLoadingId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustInstruction", SqlDbType.NVarChar).Value = CustomerInstructions;
        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }
        if (Remark.Trim() != "")
        {
            command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        }
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updPreAlertDoc", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeletePreAlert(int AlertId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@AlertId", SqlDbType.NVarChar).Value = AlertId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delPreAlertDoc", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    // Delete This
    /*
    public static int AddPreAlertdocPath(string filename, int DocumentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPreAlertDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    */
    public static int AddPreAlertdocPath(string filename, string DocPath, int DocumentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPreAlertDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdatePreAlertdocPath(string filename, string DocPath, int doctype, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = doctype;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updPreAlertDocPath", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    // Delete This
    /*
    public static int UpdatePreAlertdocPath(string filename, int doctype, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        
        command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = doctype;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updPreAlertDocPath", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    */
    public static int AddJobdocPath_Deleted(string filename, string strFilePath, int doctype, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = strFilePath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = doctype;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobDocPath", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int AddPreAlertPendingDoc(string DocumentId, bool bDel, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocAwaited", SqlDbType.NVarChar).Value = DocumentId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@bDel", SqlDbType.Bit).Value = bDel;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPendingDoc", command, "@OutPut");
        return Convert.ToInt32(SPresult);
        //CDatabase.ExecuteSP("insPendingDoc", command);
    }

    public static int UpdateJobDirName(string NewDirName, string NewFilePath, int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@NewJobDirName", SqlDbType.NVarChar).Value = NewDirName;
        command.Parameters.Add("@NewFilePath", SqlDbType.NVarChar).Value = NewFilePath;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updJobDirName", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdatePendingDocRequestDate(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updPendingDocLastRequestDate", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeletePreAlertdocPath(int lid)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delUploadedDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobDetail(int JobId, int CustomerId, int DivisionId, int PlantId, int PortId, string JobRefNo, string strCustRefNo, int JobType,
        string Supplier, string ShortDesc, string GrossWT, string FFName, string ShippingName, string VesselName, string strHSSSellerName,
        string MAWBNo, string HAWBNo, DateTime MAWBDate, DateTime HAWBDate, int Priority, int IncoTerms, int DutyStatus,
        bool FirstCheckRequired, DateTime ETADate, DateTime ATADate, int BOEType, int NoOfPackages, int PackageType, DateTime OBLRcvdDate,
        int DeliveryType, int BabajiBranchId, int DOBranch, int LoadingPortId, int intFreeDays, int KAMId,
        int CustomerPreAlertID, string ADCode, string ADBankName, string IECBranchCode, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = strCustRefNo;
        command.Parameters.Add("@JobType", SqlDbType.Int).Value = JobType;
        command.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = Supplier;
        command.Parameters.Add("@ShortDesc", SqlDbType.NVarChar).Value = ShortDesc;
        command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = GrossWT;
        command.Parameters.Add("@FFName", SqlDbType.NVarChar).Value = FFName;
        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = ShippingName;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = VesselName;
        command.Parameters.Add("@HSSSellerName", SqlDbType.NVarChar).Value = strHSSSellerName;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        command.Parameters.Add("@BOEType", SqlDbType.Int).Value = BOEType;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@ADCode", SqlDbType.NVarChar).Value = ADCode;
        command.Parameters.Add("@ADBankName", SqlDbType.NVarChar).Value = ADBankName;
        command.Parameters.Add("@IECBranchCode", SqlDbType.NVarChar).Value = IECBranchCode;
        command.Parameters.Add("@FreeDays", SqlDbType.Int).Value = intFreeDays;
        command.Parameters.Add("@KAMId", SqlDbType.Int).Value = KAMId;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@IncoTerms", SqlDbType.Int).Value = IncoTerms;
        command.Parameters.Add("@DutyStatus", SqlDbType.Int).Value = DutyStatus;
        command.Parameters.Add("@FirstCheckReq", SqlDbType.Int).Value = FirstCheckRequired;
        command.Parameters.Add("@CustomerPreAlertID", SqlDbType.Int).Value = CustomerPreAlertID;

        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }
        if (ATADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = ATADate;
        }
        if (MAWBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MAWBDate", SqlDbType.DateTime).Value = MAWBDate;
        }
        if (HAWBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HAWBDate", SqlDbType.DateTime).Value = HAWBDate;
        }

        if (OBLRcvdDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OBLRcvdDate", SqlDbType.DateTime).Value = OBLRcvdDate;
        }

        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@DOBranch", SqlDbType.Int).Value = DOBranch;
        command.Parameters.Add("@LoadingPortId", SqlDbType.Int).Value = LoadingPortId;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int UpdateJobDetail(int JobId, int AlertId, int ConsigneeId, int DivisionId, int PlantId, int JobType, string strCustRefNo, int NoOfPckgs, int PackageType,
    string strGrossWT, int IncoTerms, DateTime ETADate, DateTime ATADate, DateTime OBLDate, int DutyStatus, int LoadingPortId, string Dimension, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@AlertId", SqlDbType.Int).Value = AlertId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantID", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@JobTypeId", SqlDbType.Int).Value = JobType;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPckgs;
        command.Parameters.Add("@PackagesType", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = strGrossWT;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = strCustRefNo;
        command.Parameters.Add("@IncoTerms", SqlDbType.Int).Value = IncoTerms;
        command.Parameters.Add("@DutyStatus", SqlDbType.Int).Value = DutyStatus;
        command.Parameters.Add("@LoadingPortId", SqlDbType.Int).Value = LoadingPortId;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;  // Update Dimension

        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }

        if (ATADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = ATADate;
        }
        if (OBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OBLDate", SqlDbType.DateTime).Value = OBLDate;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteJobDetail(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.NVarChar).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int CancelJobDetail(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.NVarChar).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delJobDetailCancel", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int UpdateAdcdetail(int JobID, DateTime ADCwithdrawn, DateTime AdcNocDate,
        DateTime AdcExamDate, string Remark, int lUser, int JobTypeID)
    {
        SqlCommand command = new SqlCommand();
        string Spresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        if (ADCwithdrawn != DateTime.MinValue)
        {
            command.Parameters.Add("@Adcwithdrwandate", SqlDbType.DateTime).Value = ADCwithdrawn;
        }
        if (AdcNocDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AdcNocDate", SqlDbType.DateTime).Value = AdcNocDate;
        }
        if (AdcExamDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ADCExamDate", SqlDbType.DateTime).Value = AdcExamDate;
        }
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@JobtypeId", SqlDbType.Int).Value = JobTypeID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        Spresult = CDatabase.GetSPOutPut("updAdcDetail", command, "@OutPut");
        return Convert.ToInt32(Spresult);
    }

    public static int UpdatePhodetail(int JobID, DateTime PhoSubmitDate, DateTime PHOScrutinyDate, DateTime PHOPaymentDate, DateTime PHOAppointDate, DateTime PHOWithdrawnDate,
        DateTime PHOLabTestDate, DateTime PHOReportDate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string Spresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        if (PhoSubmitDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoSubmitDate", SqlDbType.DateTime).Value = PhoSubmitDate;
        }
        if (PHOScrutinyDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoScrutinyDate", SqlDbType.DateTime).Value = PHOScrutinyDate;
        }
        if (PHOPaymentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoPaymentDate", SqlDbType.DateTime).Value = PHOPaymentDate;
        }
        if (PHOAppointDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoAppointDate", SqlDbType.DateTime).Value = PHOAppointDate;
        }
        if (PHOWithdrawnDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoWithdrawndate", SqlDbType.DateTime).Value = PHOWithdrawnDate;
        }
        if (PHOLabTestDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoLabtestdate", SqlDbType.DateTime).Value = PHOLabTestDate;
        }
        if (PHOReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoReportdate", SqlDbType.DateTime).Value = PHOWithdrawnDate;
        }
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        Spresult = CDatabase.GetSPOutPut("updPHODetail", command, "@Output");
        return Convert.ToInt32(Spresult);
    }

    public static int UpdateADCPhoHistory(int JobID, DateTime ADCwithdrawn, DateTime AdcNocDate, DateTime AdcExamDate, DateTime PhoSubmitDate, DateTime PHOScrutinyDate, DateTime PHOPaymentDate, DateTime PHOAppointDate, DateTime PHOWithdrawnDate,
       DateTime PHOLabTestDate, DateTime PHOReportDate, string Remark, int lUser)//int JobTypeID
    {
        SqlCommand command = new SqlCommand();
        string Spresult = "";
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        if (ADCwithdrawn != DateTime.MinValue)
        {
            command.Parameters.Add("@Adcwithdrwandate", SqlDbType.DateTime).Value = ADCwithdrawn;
        }
        if (AdcNocDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AdcNocDate", SqlDbType.DateTime).Value = AdcNocDate;
        }
        if (AdcExamDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ADCExamDate", SqlDbType.DateTime).Value = AdcExamDate;
        }

        if (PhoSubmitDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoSubmitDate", SqlDbType.DateTime).Value = PhoSubmitDate;
        }
        if (PHOScrutinyDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoScrutinyDate", SqlDbType.DateTime).Value = PHOScrutinyDate;
        }
        if (PHOPaymentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoPaymentDate", SqlDbType.DateTime).Value = PHOPaymentDate;
        }
        if (PHOAppointDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoAppointDate", SqlDbType.DateTime).Value = PHOAppointDate;
        }
        if (PHOWithdrawnDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoWithdrawndate", SqlDbType.DateTime).Value = PHOWithdrawnDate;
        }
        if (PHOLabTestDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoLabtestdate", SqlDbType.DateTime).Value = PHOLabTestDate;
        }
        if (PHOReportDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PhoReportdate", SqlDbType.DateTime).Value = PHOReportDate;
        }
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        //command.Parameters.Add("@JobTypeID", SqlDbType.Int).Value = JobTypeID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        Spresult = CDatabase.GetSPOutPut("updADCPHODetail", command, "@OutPut");//UpdateADCPHODetail//UpdateADCPHODetailHistory
        return Convert.ToInt32(Spresult);
    }

    public static int UpdatePassingBondDetail(int JobID, int WareHouseId, string NocNo, DateTime NocDate,
            DateTime CompletedDate, string strBondRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string Spresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobID;
        command.Parameters.Add("@WareHouseId", SqlDbType.Int).Value = WareHouseId;
        command.Parameters.Add("@NocNo", SqlDbType.NVarChar).Value = NocNo;
        command.Parameters.Add("@BondRemark", SqlDbType.NVarChar).Value = strBondRemark;

        if (NocDate != DateTime.MinValue)
        {
            command.Parameters.Add("@NocDate", SqlDbType.DateTime).Value = NocDate;
        }
        if (CompletedDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CompletedDate", SqlDbType.DateTime).Value = CompletedDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        Spresult = CDatabase.GetSPOutPut("updPassingBondDetail", command, "@OutPut");

        return Convert.ToInt32(Spresult);

    }

    public static int UpdateCustomStages(int JobId, string StageName, string Remark, string UserName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SpResult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@StageName", SqlDbType.NVarChar).Value = StageName;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SpResult = CDatabase.GetSPOutPut("UpdateCustomStages", command, "@OutPut");
        return Convert.ToInt32(SpResult);

    }

    public static int UpdateJobDetailAdmin(int JobId, int PreAlertId, string JobRefNo, int JobType, string GrossWT, int NoOfPckgs,
       int PackageType, int IncoTerms, DateTime ATADate, DateTime OBLRcvdDate, int Priority, int DutyStatus,
       string JobRemark, int CustomerId, int ConsigneeId, int TransMode, int PortId, DateTime ETADate,
       bool FirstCheckRequired, int DivisionId, int PlantId, int BabajiBranchId, int LoadingPortId, int KamId, string CustRefNo,
       string CustomerInstructions, string PreAlertRemark, string Dimension, int lUser)//string Dimension,
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = PreAlertId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@JobType", SqlDbType.Int).Value = JobType;
        command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = GrossWT;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPckgs;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@IncoTermsId", SqlDbType.Int).Value = IncoTerms;

        if (ATADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = ATADate;
        }
        if (OBLRcvdDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OBLRcvdDate", SqlDbType.DateTime).Value = OBLRcvdDate;
        }

        command.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@DutyStatus", SqlDbType.Int).Value = DutyStatus;
        command.Parameters.Add("@FirstCheckReq", SqlDbType.Int).Value = FirstCheckRequired;
        command.Parameters.Add("@JobRemark", SqlDbType.NVarChar).Value = JobRemark;
        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@Port", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;  // Update Dimension

        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantID", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        command.Parameters.Add("@LoadingPortId", SqlDbType.Int).Value = LoadingPortId;
        command.Parameters.Add("@KamId", SqlDbType.Int).Value = KamId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustInstruction", SqlDbType.NVarChar).Value = CustomerInstructions;
        command.Parameters.Add("@PreAlertRemark", SqlDbType.NVarChar).Value = PreAlertRemark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int CheckExbondValidity(string JobRefNo, int InbondJobId, int NoOfPackages, string GrossWT)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@InBondJobId", SqlDbType.Int).Value = InbondJobId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = GrossWT;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CheckExbondValid", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddExbondJobDetail(string JobRefNo, int InbondJobId, int NoOfPackages, string GrossWT,
        int BalNoOfPackages, string BalGrossWT, int Priority, int DutyStatus, bool FirstCheckRequired, int BOEType,
        int DeliveryType, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@InBondJobId", SqlDbType.Int).Value = InbondJobId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = GrossWT;
        command.Parameters.Add("@BalNoOfPackages", SqlDbType.Int).Value = BalNoOfPackages;
        command.Parameters.Add("@BalGrossWT", SqlDbType.NVarChar).Value = BalGrossWT;
        command.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@BOEType", SqlDbType.Int).Value = BOEType;
        command.Parameters.Add("@DutyStatus", SqlDbType.Int).Value = DutyStatus;
        command.Parameters.Add("@FirstCheckReq", SqlDbType.Int).Value = FirstCheckRequired;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insExbondJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int CopyJobDetail(int JobId, string JobRefNo, int lUser, int lUserType)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@lUserType", SqlDbType.Int).Value = lUserType;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CopyJobDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddNotingDetail(int JobId, string BOENo, DateTime BOEDate, bool IsFirstCheck, int RMSNonRMS, int CustomsGroupId, int lUser, decimal dutyamnt, decimal assesamnt, decimal licdebitamnt)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BOENo", SqlDbType.NVarChar).Value = BOENo;
        command.Parameters.Add("@BOEDate", SqlDbType.DateTime).Value = BOEDate;
        command.Parameters.Add("@FirstCheckRequired", SqlDbType.Bit).Value = IsFirstCheck;
        command.Parameters.Add("@RMSType", SqlDbType.Int).Value = RMSNonRMS;
        command.Parameters.Add("@CustomsGroupId", SqlDbType.NVarChar).Value = CustomsGroupId;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        command.Parameters.Add("@dutyamnt", SqlDbType.Decimal).Value = dutyamnt;
        command.Parameters.Add("@assesamnt", SqlDbType.Decimal).Value = assesamnt;
        command.Parameters.Add("@licdebitamnt", SqlDbType.Decimal).Value = licdebitamnt;

        SPresult = CDatabase.GetSPOutPut("insNotingDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateNotingDetail(int JobId, string strBOENo, DateTime BOEDate, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@BOENo", SqlDbType.NVarChar).Value = strBOENo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        if (BOEDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BOEDate", SqlDbType.DateTime).Value = BOEDate;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobNotingDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);


    }

    public static int UpdateNotingDetail(int JobId, string strBOENo, DateTime BOEDate, string strRemark, int NotingUserId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@BOENo", SqlDbType.NVarChar).Value = strBOENo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        if (BOEDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BOEDate", SqlDbType.DateTime).Value = BOEDate;
        }

        command.Parameters.Add("@NotingUserId", SqlDbType.Int).Value = NotingUserId;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobNotingDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int UpdateNotingDetailAdmin(int JobId, string BOENo, DateTime BOEDate, int RMSNonRMS, int CustomsGroupId, string NotingRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@BOENo", SqlDbType.NVarChar).Value = BOENo;

        if (BOEDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BOEDate", SqlDbType.DateTime).Value = BOEDate;
        }
        command.Parameters.Add("@RMSNonRMS", SqlDbType.Int).Value = RMSNonRMS;

        command.Parameters.Add("@CustomsGroupId", SqlDbType.NVarChar).Value = @CustomsGroupId;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = NotingRemark;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobNotingDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);


    }

    public static int SendNotingEmail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("mailBESubmission", command, "@OutPut");

        if (SPresult == "")
            SPresult = "-1";

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDetailIGM(int JobId, DateTime IGMDate, string IGMNo, DateTime IGMDate2, string IGMNo2,
        DateTime IGMDate3, string IGMNo3, string IGMItem, string IGMSubItem, string IGMSubItem2, string IGMSubItem3,
        string FFName, string Supplier, string MAWBNo, string HAWBNo, DateTime MAWBDate, DateTime HAWBDate, string ShortDesc, string ShippingName, int CFSId,
        DateTime CFSDate, string LandIGMNo, DateTime LandIGMDate, DateTime InwardDate, string VesselName, string IGMRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IGMNo", SqlDbType.NVarChar).Value = IGMNo;
        command.Parameters.Add("@IGMNo2", SqlDbType.NVarChar).Value = IGMNo2;
        command.Parameters.Add("@IGMNo3", SqlDbType.NVarChar).Value = IGMNo3;
        command.Parameters.Add("@IGMItem", SqlDbType.NVarChar).Value = IGMItem;
        command.Parameters.Add("@IGMSubItem", SqlDbType.NVarChar).Value = IGMSubItem;
        command.Parameters.Add("@IGMSubItem2", SqlDbType.NVarChar).Value = IGMSubItem2;
        command.Parameters.Add("@IGMSubItem3", SqlDbType.NVarChar).Value = IGMSubItem3;
        command.Parameters.Add("@FFName", SqlDbType.NVarChar).Value = FFName;
        command.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = Supplier;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        command.Parameters.Add("@LandIGMNo", SqlDbType.NVarChar).Value = LandIGMNo;

        if (IGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate", SqlDbType.DateTime).Value = IGMDate;
        }

        if (IGMDate2 != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate2", SqlDbType.DateTime).Value = IGMDate2;
        }

        if (IGMDate3 != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate3", SqlDbType.DateTime).Value = IGMDate3;
        }

        if (MAWBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MAWBDate", SqlDbType.DateTime).Value = MAWBDate;
        }
        if (HAWBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HAWBDate", SqlDbType.DateTime).Value = HAWBDate;
        }
        if (LandIGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@LandIGMDate", SqlDbType.DateTime).Value = LandIGMDate;
        }

        if (InwardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InwardDate", SqlDbType.DateTime).Value = InwardDate;
        }

        command.Parameters.Add("@ShortDesc", SqlDbType.NVarChar).Value = ShortDesc;
        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = ShippingName;
        command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = VesselName;
        command.Parameters.Add("@IGMRemark", SqlDbType.NVarChar).Value = IGMRemark;

        if (CFSDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CFSDate", SqlDbType.DateTime).Value = CFSDate;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDetailIGM", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateIGMDetailAdmin(int JobId, string IGMNo, DateTime IGMDate, DateTime IGMDate2,
        string IGMNo2, DateTime IGMDate3, string IGMNo3, string IGMItem, string IGMSubItem, string IGMSubItem2,
        string IGMSubItem3, DateTime CFSDate, int CFSId, int BOEType, string ShortDesc, string ShippingName,
        string Supplier, string MAWBNo, string HAWBNo, DateTime MAWBDate, DateTime HAWBDate, DateTime InwardDate,
        string FFName, string LandIGMNo, DateTime LandIGMDate, string VesselName, string IGMRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IGMNo", SqlDbType.NVarChar).Value = IGMNo;
        command.Parameters.Add("@IGMNo2", SqlDbType.NVarChar).Value = IGMNo2;
        command.Parameters.Add("@IGMNo3", SqlDbType.NVarChar).Value = IGMNo3;
        command.Parameters.Add("@IGMItem", SqlDbType.NVarChar).Value = IGMItem;
        command.Parameters.Add("@IGMSubItem", SqlDbType.NVarChar).Value = IGMSubItem;
        command.Parameters.Add("@IGMSubItem2", SqlDbType.NVarChar).Value = IGMSubItem2;
        command.Parameters.Add("@IGMSubItem3", SqlDbType.NVarChar).Value = IGMSubItem3;
        command.Parameters.Add("@LandIGMNo", SqlDbType.NVarChar).Value = LandIGMNo;

        if (IGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate", SqlDbType.DateTime).Value = IGMDate;
        }

        if (IGMDate2 != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate2", SqlDbType.DateTime).Value = IGMDate2;
        }

        if (IGMDate3 != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate3", SqlDbType.DateTime).Value = IGMDate3;
        }
        if (CFSDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CFSDate", SqlDbType.DateTime).Value = CFSDate;
        }
        if (LandIGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@LandIGMDate", SqlDbType.DateTime).Value = LandIGMDate;
        }

        command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;
        command.Parameters.Add("@BOEType", SqlDbType.Int).Value = BOEType;
        command.Parameters.Add("@ShortDesc", SqlDbType.NVarChar).Value = ShortDesc;
        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = ShippingName;
        command.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = Supplier;
        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;

        if (MAWBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MAWBDate", SqlDbType.DateTime).Value = MAWBDate;
        }
        if (HAWBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HAWBDate", SqlDbType.DateTime).Value = HAWBDate;
        }
        if (InwardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InwardDate", SqlDbType.DateTime).Value = InwardDate;
        }
        command.Parameters.Add("@FFName", SqlDbType.NVarChar).Value = FFName;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = VesselName;
        command.Parameters.Add("@IGMRemark", SqlDbType.NVarChar).Value = IGMRemark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updIGMDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobPassingDetailAdmin(int JobId, DateTime AppraisingDate, DateTime AssessmentDate, string PassingRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (AppraisingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AppraisingDate", SqlDbType.DateTime).Value = AppraisingDate;
        }

        if (AssessmentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AssessmentDate", SqlDbType.DateTime).Value = AssessmentDate;
        }

        command.Parameters.Add("@PassingRemark", SqlDbType.NVarChar).Value = PassingRemark;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobPassingDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);


    }

    public static int UpdateJobPassingDetailStage(int JobId, DateTime AppraisingDate, DateTime AssessmentDate, int PassingStageId, string PassingRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (AppraisingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AppraisingDate", SqlDbType.DateTime).Value = AppraisingDate;
        }

        if (AssessmentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AssessmentDate", SqlDbType.DateTime).Value = AssessmentDate;
        }
        command.Parameters.Add("@PassingStageId", SqlDbType.NVarChar).Value = PassingStageId;
        command.Parameters.Add("@PassingRemark", SqlDbType.NVarChar).Value = PassingRemark;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobPassingDetailStage", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int UpdateJobFirstCheckDetailAdmin(int JobId, DateTime FirstAppraisingDate,
        DateTime FirstAssessmentDate, DateTime CEInspectionDate, DateTime CargoExaminationDate,
        DateTime ForwdAppraisingDate, string FirstCheckRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (FirstAppraisingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FirstAppraisingDate", SqlDbType.DateTime).Value = FirstAppraisingDate;
        }

        if (FirstAssessmentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FirstAssessmentDate", SqlDbType.DateTime).Value = FirstAssessmentDate;
        }

        if (CEInspectionDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CEInspectionDate", SqlDbType.DateTime).Value = CEInspectionDate;
        }

        if (CargoExaminationDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CargoExaminationDate", SqlDbType.DateTime).Value = CargoExaminationDate;
        }

        if (ForwdAppraisingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ForwdAppraisingDate", SqlDbType.DateTime).Value = ForwdAppraisingDate;
        }

        command.Parameters.Add("@FirstCheckRemark", SqlDbType.NVarChar).Value = FirstCheckRemark;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updFirstCheckDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);


    }

    public static int AddJobSchemeDetail(int JobId, int SchemeTypeId, string SchemeName, string SchemeNo, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@SchemeTypeId", SqlDbType.Int).Value = SchemeTypeId;
        command.Parameters.Add("@SchemeName", SqlDbType.NVarChar).Value = SchemeName;
        command.Parameters.Add("@SchemeNo", SqlDbType.NVarChar).Value = SchemeNo;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobSchemeDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobSchemeDetail(int lid, int SchemeTypeId, string SchemeNo, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@SchemeTypeId", SqlDbType.Int).Value = SchemeTypeId;
        command.Parameters.Add("@SchemeNo", SqlDbType.NVarChar).Value = SchemeNo;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobSchemeDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobSchemeDetail(int lid, int SchemeTypeId, string SchemeNo, decimal Amount, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@SchemeTypeId", SqlDbType.Int).Value = SchemeTypeId;
        command.Parameters.Add("@SchemeNo", SqlDbType.NVarChar).Value = SchemeNo;
        command.Parameters.Add("@LicenseAmount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobSchemeDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteJobSchemeDetail(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delJobSchemeDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddIGMDetail(int JobId, string FFName, string MAWBNo, string HAWBNo, string strShippingName, string strVesselName,
    DateTime ATADate, DateTime ETADate, int NoOfPackages, int PackageType, string strGrossWT,
    string IGMNo, string IGMNo2, string IGMNo3, DateTime IGMDate, DateTime IGMDate2, DateTime IGMDate3,
    string IGMItem, string IGMSubItem, string IGMSubItem2, string IGMSubItem3, DateTime InwardDate, int CFSId, DateTime CFSDate,
    DateTime MblDate, DateTime HblDate, string strLandIGMNo, DateTime LandIGMDate, DateTime FollowUpDate, string Remark, int ShippingId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FFName", SqlDbType.NVarChar).Value = FFName;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;

        command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
        command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = strShippingName;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = strVesselName;

        command.Parameters.Add("@IGMNo2", SqlDbType.NVarChar).Value = IGMNo2;
        command.Parameters.Add("@IGMNo3", SqlDbType.NVarChar).Value = IGMNo3;
        command.Parameters.Add("@IGMSubItem", SqlDbType.NVarChar).Value = IGMSubItem;
        command.Parameters.Add("@IGMSubItem2", SqlDbType.NVarChar).Value = IGMSubItem2;
        command.Parameters.Add("@IGMSubItem3", SqlDbType.NVarChar).Value = IGMSubItem3;

        if (strGrossWT != "")
        {
            command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = strGrossWT;
        }
        if (IGMNo != "")
        {
            command.Parameters.Add("@IGMNo", SqlDbType.NVarChar).Value = IGMNo;
        }
        if (IGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate", SqlDbType.DateTime).Value = IGMDate;
        }
        if (IGMItem != "")
        {
            command.Parameters.Add("@IGMItem", SqlDbType.NVarChar).Value = IGMItem;
        }
        if (IGMDate2 != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate2", SqlDbType.DateTime).Value = IGMDate2;
        }
        if (IGMDate3 != DateTime.MinValue)
        {
            command.Parameters.Add("@IGMDate3", SqlDbType.DateTime).Value = IGMDate3;
        }

        if (ETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
        }
        if (ATADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = ATADate;
        }
        if (MblDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MblDate", SqlDbType.DateTime).Value = MblDate;
        }
        if (HblDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HblDate", SqlDbType.DateTime).Value = HblDate;
        }
        if (CFSDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CFSDate", SqlDbType.DateTime).Value = CFSDate;
        }
        if (InwardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InwardDate", SqlDbType.DateTime).Value = InwardDate;
        }

        command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@ShippingId", SqlDbType.Int).Value = ShippingId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insIGMDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    //public static int AddIGMDetail(int JobId, string FFName, string MAWBNo, string HAWBNo, string strShippingName, string strVesselName,
    //    DateTime ATADate, DateTime ETADate, int NoOfPackages, int PackageType, string strGrossWT,
    //    string IGMNo, string IGMNo2, string IGMNo3, DateTime IGMDate, DateTime IGMDate2, DateTime IGMDate3,
    //    string IGMItem, string IGMSubItem, string IGMSubItem2, string IGMSubItem3, DateTime InwardDate, int CFSId, DateTime CFSDate,
    //    DateTime MblDate, DateTime HblDate, string strLandIGMNo, DateTime LandIGMDate, DateTime FollowUpDate, string Remark, int lUser) // int ShippingId
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";

    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
    //    command.Parameters.Add("@FFName", SqlDbType.NVarChar).Value = FFName;
    //    command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
    //    command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;

    //    command.Parameters.Add("@MAWBNo", SqlDbType.NVarChar).Value = MAWBNo;
    //    command.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
    //    command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = strShippingName;
    //    command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = strVesselName;

    //    command.Parameters.Add("@IGMNo2", SqlDbType.NVarChar).Value = IGMNo2;
    //    command.Parameters.Add("@IGMNo3", SqlDbType.NVarChar).Value = IGMNo3;
    //    command.Parameters.Add("@IGMSubItem", SqlDbType.NVarChar).Value = IGMSubItem;
    //    command.Parameters.Add("@IGMSubItem2", SqlDbType.NVarChar).Value = IGMSubItem2;
    //    command.Parameters.Add("@IGMSubItem3", SqlDbType.NVarChar).Value = IGMSubItem3;
    //    command.Parameters.Add("@LandIGMNo", SqlDbType.NVarChar).Value = strLandIGMNo;

    //    if (strGrossWT != "")
    //    {
    //        command.Parameters.Add("@GrossWT", SqlDbType.NVarChar).Value = strGrossWT;
    //    }
    //    if (IGMNo != "")
    //    {
    //        command.Parameters.Add("@IGMNo", SqlDbType.NVarChar).Value = IGMNo;
    //    }
    //    if (IGMDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@IGMDate", SqlDbType.DateTime).Value = IGMDate;
    //    }
    //    if (IGMItem != "")
    //    {
    //        command.Parameters.Add("@IGMItem", SqlDbType.NVarChar).Value = IGMItem;
    //    }
    //    if (IGMDate2 != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@IGMDate2", SqlDbType.DateTime).Value = IGMDate2;
    //    }
    //    if (IGMDate3 != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@IGMDate3", SqlDbType.DateTime).Value = IGMDate3;
    //    }
    //    if (LandIGMDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@LandIGMDate", SqlDbType.DateTime).Value = LandIGMDate;
    //    }
    //    if (FollowUpDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@FollowUpDate", SqlDbType.DateTime).Value = FollowUpDate;
    //    }
    //    if (ETADate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = ETADate;
    //    }
    //    if (ATADate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = ATADate;
    //    }
    //    if (MblDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@MblDate", SqlDbType.DateTime).Value = MblDate;
    //    }
    //    if (HblDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@HblDate", SqlDbType.DateTime).Value = HblDate;
    //    }
    //    if (CFSDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@CFSDate", SqlDbType.DateTime).Value = CFSDate;
    //    }
    //    if (InwardDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@InwardDate", SqlDbType.DateTime).Value = InwardDate;
    //    }

    //    command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;
    //    command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
    //    //command.Parameters.Add("@ShippingId", SqlDbType.Int).Value = ShippingId;
    //    command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
    //    command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("insIGMDetail", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int AddFirstCheckDetail(int JobId, DateTime dtAppraisingDate, DateTime dtAssessmentDate,
        DateTime dtCEInspectionDate, DateTime dtCargoExaminationDate, DateTime dtForwdAppraising, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (dtAppraisingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FirstAppraisingDate", SqlDbType.DateTime).Value = dtAppraisingDate;
        }
        if (dtAssessmentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FirstAssessmentDate", SqlDbType.DateTime).Value = dtAssessmentDate;
        }

        if (dtCEInspectionDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CEInspectionDate", SqlDbType.DateTime).Value = dtCEInspectionDate;
        }

        if (dtCargoExaminationDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CargoExaminationDate", SqlDbType.DateTime).Value = dtCargoExaminationDate;
        }
        if (dtForwdAppraising != DateTime.MinValue)
        {
            command.Parameters.Add("@ForwdAppraisingDate", SqlDbType.DateTime).Value = dtForwdAppraising;
        }

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insFirstCheckDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDOCollectionDetail(int JobId, DateTime ConsolDoDate, DateTime FinalDODate, DateTime EmptyDate,
        int FreeDays, int PaymentType, string strEmptyYardName, string BlankChequeNo,
        DateTime BlankChequeDate, bool BondSubmitted, int SecurityDeposit, string SecurityReceiptPath, int DOStageId, string PickupPerson, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BondSubmitted", SqlDbType.Bit).Value = @BondSubmitted;
        command.Parameters.Add("@SecurityDeposit", SqlDbType.Int).Value = SecurityDeposit;
        command.Parameters.Add("@FreeDays", SqlDbType.Int).Value = FreeDays;
        command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        if (FinalDODate != DateTime.MinValue)
        {
            command.Parameters.Add("@FinalDODate", SqlDbType.DateTime).Value = FinalDODate;
        }
        if (ConsolDoDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ConsolDoDate", SqlDbType.DateTime).Value = ConsolDoDate;
        }
        if (EmptyDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EmptyDate", SqlDbType.DateTime).Value = EmptyDate;
        }
        if (BlankChequeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BlankChequeDate", SqlDbType.DateTime).Value = BlankChequeDate;
        }

        if (strEmptyYardName != "")
        {
            command.Parameters.Add("@EmptyYardName", SqlDbType.NVarChar).Value = strEmptyYardName;
        }
        if (BlankChequeNo != "")
        {
            command.Parameters.Add("@BlankChequeNo", SqlDbType.NVarChar).Value = BlankChequeNo;
        }

        if (SecurityReceiptPath != "")
        {
            command.Parameters.Add("@SecurityReceiptPath", SqlDbType.NVarChar).Value = SecurityReceiptPath;
        }

        command.Parameters.Add("@DOStageId", SqlDbType.Int).Value = DOStageId;
        command.Parameters.Add("@PickupPerson", SqlDbType.NVarChar).Value = PickupPerson;

        SPresult = CDatabase.GetSPOutPut("insDOCollectionDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDOCollection(int JobId, DateTime ConsolDoDate, DateTime FinalDODate, int PaymentType, int FreeDays, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@FreeDays", SqlDbType.Int).Value = FreeDays;
        command.Parameters.Add("@ConsolDoDate", SqlDbType.DateTime).Value = ConsolDoDate;
        command.Parameters.Add("@FinalDODate", SqlDbType.DateTime).Value = FinalDODate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDOCollection", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDOCollectionAdmin(int JobId, DateTime ConsolDoDate, DateTime FinalDODate, int FreeDays,
        DateTime EmptyValidityDate, string EmptyYardName, string BlankChequeNo,
        DateTime BlankChequeDate, bool BondSubmitted, int SecurityDeposit, string SecurityReceiptPath,
        int PaymentType, int DOBranch, string DORemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FreeDays", SqlDbType.Int).Value = FreeDays;

        if (ConsolDoDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ConsolDoDate", SqlDbType.DateTime).Value = ConsolDoDate;
        }

        if (FinalDODate != DateTime.MinValue)
        {
            command.Parameters.Add("@FinalDODate", SqlDbType.DateTime).Value = FinalDODate;
        }

        if (EmptyValidityDate != DateTime.MinValue)
        {
            command.Parameters.Add("@EmptyValidityDate", SqlDbType.DateTime).Value = EmptyValidityDate;
        }
        if (EmptyYardName != "")
        {
            command.Parameters.Add("@EmptyYardName", SqlDbType.NVarChar).Value = EmptyYardName;
        }
        if (BlankChequeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BlankChequeDate", SqlDbType.DateTime).Value = BlankChequeDate;
        }

        if (SecurityReceiptPath != "")
        {
            command.Parameters.Add("@SecurityReceiptPath", SqlDbType.NVarChar).Value = SecurityReceiptPath;
        }

        command.Parameters.Add("@BlankChequeNo", SqlDbType.NVarChar).Value = BlankChequeNo;
        command.Parameters.Add("@BondSubmitted", SqlDbType.Bit).Value = BondSubmitted;
        command.Parameters.Add("@SecurityDeposit", SqlDbType.Int).Value = SecurityDeposit;
        command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@DOBranch", SqlDbType.Int).Value = DOBranch;
        command.Parameters.Add("@DORemark", SqlDbType.NVarChar).Value = DORemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDOCollectionAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDutyDetail(int JobId, string strDutyAmount, string strIGSTAmount, string strIntrestAmt, string strFineAmount,
        string strPenaltyAmount, int DutyPayType, string strAssessableValue, string CopyOfChallan, string ChallanNo, DateTime DDChallanDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DutyAmount", SqlDbType.NVarChar).Value = strDutyAmount;

        if (strIGSTAmount != "")
            command.Parameters.Add("@IGSTAmt", SqlDbType.NVarChar).Value = strIGSTAmount;
        if (strIntrestAmt != "")
            command.Parameters.Add("@InterestAmt", SqlDbType.NVarChar).Value = strIntrestAmt;
        if (strFineAmount != "")
            command.Parameters.Add("@FineAmount", SqlDbType.NVarChar).Value = strFineAmount;
        if (strPenaltyAmount != "")
            command.Parameters.Add("@PenaltyAmount", SqlDbType.NVarChar).Value = strPenaltyAmount;

        command.Parameters.Add("@DutyPayType", SqlDbType.Int).Value = DutyPayType;
        command.Parameters.Add("@AssessableValue", SqlDbType.NVarChar).Value = strAssessableValue;

        if (CopyOfChallan != "")
        {
            command.Parameters.Add("@CopyOfChallan", SqlDbType.NVarChar).Value = CopyOfChallan;
        }
        if (ChallanNo != "")
        {
            command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;
        }
        if (DDChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DDChallanDate", SqlDbType.DateTime).Value = DDChallanDate;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updDutyDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDutyDetailAdmin(int JobId, string strDutyAmount, string strIGSTAmount, string strIntrestAmt,
        string strFinePenaltyAmount, int DutyPayType, string strAssessableValue, DateTime DutyReqDate,
         DateTime DutyPaymentDate, string ChallanNo, DateTime DDChallanDate, string CopyOfChallan,
        string DutyRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DutyAmount", SqlDbType.NVarChar).Value = strDutyAmount;
        if (strIGSTAmount != "")
            command.Parameters.Add("@IGSTAmt", SqlDbType.NVarChar).Value = strIGSTAmount;
        if (strIntrestAmt != "")
            command.Parameters.Add("@InterestAmt", SqlDbType.NVarChar).Value = strIntrestAmt;
        if (strFinePenaltyAmount != "")
            command.Parameters.Add("@FinePenalty", SqlDbType.NVarChar).Value = strFinePenaltyAmount;

        command.Parameters.Add("@DutyPayType", SqlDbType.Int).Value = DutyPayType;
        command.Parameters.Add("@AssessableValue", SqlDbType.NVarChar).Value = strAssessableValue;
        if (DutyReqDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DutyReqDate", SqlDbType.DateTime).Value = DutyReqDate;
        }

        if (DutyPaymentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DutyPaymentDate", SqlDbType.DateTime).Value = DutyPaymentDate;
        }
        if (ChallanNo != "")
        {
            command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;
        }
        if (DDChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DDChallanDate", SqlDbType.DateTime).Value = DDChallanDate;
        }
        if (CopyOfChallan != "")
        {
            command.Parameters.Add("@CopyOfChallan", SqlDbType.NVarChar).Value = CopyOfChallan;
        }

        if (DutyRemark != "")
        {
            command.Parameters.Add("@DutyRemark", SqlDbType.NVarChar).Value = DutyRemark;
        }
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updDutyDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddChecklistHold(int JobId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insChecklistHold", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddChecklistDetail(int JobId, bool CustomerApproval, string CheckListPath, string DutyAmount, string AssessableValue, string Remark, int lUser, string licDebitDuty)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ClientApproval", SqlDbType.Bit).Value = CustomerApproval;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        if (CheckListPath != "")
        {
            command.Parameters.Add("@CheckListPath", SqlDbType.NVarChar).Value = CheckListPath;
        }

        if (DutyAmount == "")
        {
            DutyAmount = "0";
        }
        command.Parameters.Add("@DutyAmount", SqlDbType.NVarChar).Value = DutyAmount;

        if (licDebitDuty == "")
        {
            licDebitDuty = "0";
        }
        command.Parameters.Add("@licDebitDuty", SqlDbType.Decimal).Value = licDebitDuty;

        if (AssessableValue == "")
        {
            AssessableValue = "0";
        }
        command.Parameters.Add("@AssessableValue", SqlDbType.Decimal).Value = AssessableValue;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insChecklistDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int ApproveRejectChecklist(int JobId, string CheckListDocPath, bool IsApproved, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@CheckListDocPath", SqlDbType.NVarChar).Value = CheckListDocPath;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insChecklistApproval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectChecklist_Customer(int JobId, bool IsApproved, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@CustUserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insChecklistApproval_Customer", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetChecklistHistoryByStatus(int JobId, int StatusId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;

        return CDatabase.GetDataSet("GetChecklistByStatusId", cmd);
    }

    public static int AddJobSubStatusDetail(int JobId, int StatusId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@SubStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        // command.Parameters.Add("@dtDate", SqlDbType.DateTime).Value = DateTime.Now;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobSubStatusDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobCurrentStatus(int JobId, string Status, string StatusHistory, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobID", SqlDbType.NVarChar).Value = JobId;
        command.Parameters.Add("@CurrentStatus", SqlDbType.NVarChar).Value = Status;
        command.Parameters.Add("@StatusHistory", SqlDbType.NVarChar).Value = StatusHistory;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobCurrentStatus", command, "@OutPut");
        // CDatabase.ExecuteSP("insPreAlertDoc", command);


        return Convert.ToInt32(SPresult);
    }

    public static int AddDutyRequest(int JobId, string DutyAmount, DateTime RequestDate, int PaymentType,
    DateTime PaymentDate, string ChallanNo, string IGSTAmt, string InterestAmt, string FineAmount,
    string PenaltyAmount, DateTime DDChallanDate, string CopyOfchallan, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        int DutyStatus = 3;//2;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@DutyAmount", SqlDbType.NVarChar).Value = DutyAmount;

        command.Parameters.Add("@RequestDate", SqlDbType.DateTime).Value = RequestDate;

        if (PaymentDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = PaymentDate;
            // DutyStatus = 4;
            DutyStatus = 5;
        }
        command.Parameters.Add("@PaymentType", SqlDbType.Int).Value = PaymentType;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;

        if (IGSTAmt.Trim() != "")
            command.Parameters.Add("@IGSTAmt", SqlDbType.NVarChar).Value = IGSTAmt;
        if (InterestAmt.Trim() != "")
            command.Parameters.Add("@InterestAmt", SqlDbType.NVarChar).Value = InterestAmt;
        if (FineAmount.Trim() != "")
            command.Parameters.Add("@FinePenalty", SqlDbType.NVarChar).Value = FineAmount;
        if (PenaltyAmount.Trim() != "")
            command.Parameters.Add("@PenaltyAmount", SqlDbType.NVarChar).Value = PenaltyAmount;

        command.Parameters.Add("@DutyStatus", SqlDbType.Int).Value = DutyStatus;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        if (DDChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DDChallanDate", SqlDbType.DateTime).Value = DDChallanDate;

        }
        if (CopyOfchallan != "")
        {
            command.Parameters.Add("@CopyOfChallan", SqlDbType.NVarChar).Value = CopyOfchallan;
        }
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDutyDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDutyAmount(int JobId, string strDutyAmount, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DutyAmount", SqlDbType.NVarChar).Value = strDutyAmount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDutyAmount", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateAssessableValue(int JobId, string AssessableValue, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@AssessableValue", SqlDbType.Decimal).Value = AssessableValue;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updAssessableValue", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    //public static int AddOutOfChargeExamineDetail(int JobId, DateTime ExamineDate, DateTime OutOfChargeDate, bool bExamineStatus, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";

    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

    //    if (ExamineDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@ExamineDate", SqlDbType.DateTime).Value = ExamineDate;
    //    }
    //    if (OutOfChargeDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@OutOfChargeDate", SqlDbType.DateTime).Value = OutOfChargeDate;
    //    }

    //    command.Parameters.Add("@ExamineStatus", SqlDbType.Bit).Value = bExamineStatus;

    //    command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
    //    command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("insOutOfChargeExamineDetail", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int AddExamineDetail(int JobId, int DeliveryTypeId, DateTime PassingDate, DateTime DeliveryPlanningDate, DateTime FrankingDate, string FrankingAmount,
        string RDAmount, string RDPercentage, DateTime TruckReqDate, string DeliveryDestination, string DeliveryAddress, bool IsOctroi, bool IsSForm, bool IsNForm,
        bool IsRoadPermit, bool TransportationByBabaji, int PlanningFund, Boolean bOCCYard, string Remark, string Dimension, bool IsReExamine, bool IsValidPlanning, int lUser,
        DateTime VehiclePlaceDate)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        // command.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@DeliveryTypeId", SqlDbType.Int).Value = DeliveryTypeId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@IsContainerReExamine", SqlDbType.Bit).Value = IsReExamine;
        command.Parameters.Add("@IsValidPlanning", SqlDbType.Bit).Value = IsValidPlanning;
        if (PassingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@AssessmentDate", SqlDbType.DateTime).Value = PassingDate;
        }

        if (DeliveryPlanningDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DeliveryPlanningDate", SqlDbType.DateTime).Value = DeliveryPlanningDate;
        }

        if (FrankingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FrankingDate", SqlDbType.DateTime).Value = FrankingDate;
        }
        if (TruckReqDate != DateTime.MinValue)
        {
            command.Parameters.Add("@TruckRequestDate", SqlDbType.DateTime).Value = TruckReqDate;
        }

        command.Parameters.Add("@FrankingAmount", SqlDbType.NVarChar).Value = FrankingAmount;
        command.Parameters.Add("@RDAmount", SqlDbType.NVarChar).Value = RDAmount;
        command.Parameters.Add("@RDPercentage", SqlDbType.NVarChar).Value = RDPercentage;

        command.Parameters.Add("@DeliveryDestination", SqlDbType.NVarChar).Value = DeliveryDestination;
        command.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar).Value = DeliveryAddress;
        command.Parameters.Add("@IsOctroi", SqlDbType.Bit).Value = IsOctroi;
        command.Parameters.Add("@IsSForm", SqlDbType.Bit).Value = IsSForm;
        command.Parameters.Add("@IsNForm", SqlDbType.Bit).Value = IsNForm;
        command.Parameters.Add("@IsRoadPermit", SqlDbType.Bit).Value = IsRoadPermit;
        command.Parameters.Add("@TransportationByBabaji", SqlDbType.Bit).Value = TransportationByBabaji;
        command.Parameters.Add("@PlanningFund", SqlDbType.Int).Value = PlanningFund;
        command.Parameters.Add("@IsOCCYard", SqlDbType.Bit).Value = bOCCYard;
        if (VehiclePlaceDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VehiclePlaceDate", SqlDbType.DateTime).Value = VehiclePlaceDate;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insExamineDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddExamineDetail(int JobId, int DeliveryTypeId, int RMSNonRMS, DateTime DeliveryPlanningDate,
    string RDAmount, string RDPercentage, DateTime TruckReqDate, string DeliveryDestination, string DeliveryAddress, bool TransportationByBabaji,
    int PlanningFund, Boolean bOCCYard, string Remark, string Dimension, bool IsReExamine, bool IsValidPlanning, int lUser,
    DateTime VehiclePlaceDate,int CFSId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        // command.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@DeliveryTypeId", SqlDbType.Int).Value = DeliveryTypeId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@IsContainerReExamine", SqlDbType.Bit).Value = IsReExamine;
        command.Parameters.Add("@IsValidPlanning", SqlDbType.Bit).Value = IsValidPlanning;
        command.Parameters.Add("@RMSNonRMS", SqlDbType.Int).Value = RMSNonRMS;

        if (DeliveryPlanningDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DeliveryPlanningDate", SqlDbType.DateTime).Value = DeliveryPlanningDate;
        }

        if (TruckReqDate != DateTime.MinValue)
        {
            command.Parameters.Add("@TruckRequestDate", SqlDbType.DateTime).Value = TruckReqDate;
        }

        command.Parameters.Add("@RDAmount", SqlDbType.NVarChar).Value = RDAmount;
        command.Parameters.Add("@RDPercentage", SqlDbType.NVarChar).Value = RDPercentage;

        command.Parameters.Add("@DeliveryDestination", SqlDbType.NVarChar).Value = DeliveryDestination;
        command.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar).Value = DeliveryAddress;
        command.Parameters.Add("@TransportationByBabaji", SqlDbType.Bit).Value = TransportationByBabaji;
        command.Parameters.Add("@PlanningFund", SqlDbType.Int).Value = PlanningFund;
        command.Parameters.Add("@IsOCCYard", SqlDbType.Bit).Value = bOCCYard;

        if (VehiclePlaceDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VehiclePlaceDate", SqlDbType.DateTime).Value = VehiclePlaceDate;
        }
        command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insExamineDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateExaminDetail(int JobId, DateTime ExamineDate, DateTime OutOfChargeDate,
        DateTime DeliveryPlanningDate, string FrankingAmount, DateTime FrankingDate, string RDAmount, string RDPercentage,
        bool IsOctroi, bool IsSForm, bool IsNForm, bool IsRoadPermit, int PlanningFund, int DeliveryTypeId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (ExamineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ExamineDate", SqlDbType.DateTime).Value = ExamineDate;
        }
        if (OutOfChargeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OutOfChargeDate", SqlDbType.DateTime).Value = OutOfChargeDate;
        }
        if (DeliveryPlanningDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DeliveryPlanningDate", SqlDbType.DateTime).Value = DeliveryPlanningDate;
        }
        if (FrankingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FrankingDate", SqlDbType.DateTime).Value = FrankingDate;
        }

        command.Parameters.Add("@FrankingAmount", SqlDbType.Decimal).Value = FrankingAmount;
        command.Parameters.Add("@RDAmount", SqlDbType.NVarChar).Value = RDAmount;
        command.Parameters.Add("@RDPercentage", SqlDbType.NVarChar).Value = RDPercentage;
        command.Parameters.Add("@IsOctroi", SqlDbType.Bit).Value = IsOctroi;
        command.Parameters.Add("@IsSForm", SqlDbType.Bit).Value = IsSForm;
        command.Parameters.Add("@IsNForm", SqlDbType.Bit).Value = IsNForm;
        command.Parameters.Add("@IsRoadPermit", SqlDbType.Bit).Value = IsRoadPermit;
        command.Parameters.Add("@PlanningFund", SqlDbType.Int).Value = PlanningFund;
        command.Parameters.Add("@DeliveryTypeId", SqlDbType.Int).Value = DeliveryTypeId;
        command.Parameters.Add("@ExamineRemark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updExamineDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateExaminDetailAdmin(int JobId, DateTime ExamineDate, DateTime OutOfChargeDate,
       DateTime DeliveryPlanningDate, string FrankingAmount, DateTime FrankingDate, string RDAmount, string RDPercentage,
        bool IsOctroi, bool IsSForm, bool IsNForm, bool IsRoadPermit, int WarehouseId, int PlanningFund, int DeliveryTypeId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = WarehouseId;

        if (ExamineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ExamineDate", SqlDbType.DateTime).Value = ExamineDate;
        }
        if (OutOfChargeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OutOfChargeDate", SqlDbType.DateTime).Value = OutOfChargeDate;
        }
        if (DeliveryPlanningDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DeliveryPlanningDate", SqlDbType.DateTime).Value = DeliveryPlanningDate;
        }
        if (FrankingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FrankingDate", SqlDbType.DateTime).Value = FrankingDate;
        }

        command.Parameters.Add("@FrankingAmount", SqlDbType.Decimal).Value = FrankingAmount;
        command.Parameters.Add("@RDAmount", SqlDbType.NVarChar).Value = RDAmount;
        command.Parameters.Add("@RDPercentage", SqlDbType.NVarChar).Value = RDPercentage;
        command.Parameters.Add("@IsOctroi", SqlDbType.Bit).Value = IsOctroi;
        command.Parameters.Add("@IsSForm", SqlDbType.Bit).Value = IsSForm;
        command.Parameters.Add("@IsNForm", SqlDbType.Bit).Value = IsNForm;
        command.Parameters.Add("@IsRoadPermit", SqlDbType.Bit).Value = IsRoadPermit;
        command.Parameters.Add("@DeliveryTypeId", SqlDbType.Int).Value = DeliveryTypeId;
        command.Parameters.Add("@PlanningFund", SqlDbType.Int).Value = PlanningFund;
        command.Parameters.Add("@ExamineRemark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updExamineDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDeliveryPlanningDate(int JobId, DateTime PlanningDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@DeliveryPlanningDate", SqlDbType.DateTime).Value = PlanningDate;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDeliveryPlanningDate", command, "@OutPut");

        return Convert.ToInt32(SPresult);


    }

    public static int AddTransitWarehouse(int JobId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobTransitHistory", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDeliveryDetail(int JobId, int ContainerId, int NoOfPackages, string VehicleNo, DateTime TruckReqDate,
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

        SPresult = CDatabase.GetSPOutPut("insDeliveryDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDeliveryWarehouse(int JobId, int ContainerId, int NoOfPackages, string VehicleNo, DateTime TruckReqDate,
    DateTime VehicleRcvdDate, string TransporterName, int TransporterID, bool TransportationByBabaji,
    string LRNo, DateTime LRDate, string DeliveryPoint, DateTime DispatchDate,
    DateTime DeliveryDate, DateTime EmptyContRetrunDate, string RoadPermitNo, DateTime RoadPermitDate, string CargoReceivedBy, string LRCopyPath,
    string NFormNo, DateTime NFormDate, DateTime NClosingDate, string SFormNo, DateTime SFormDate, DateTime SClosingDate, string OctroiAmount,
    string OctroiReceiptNo, DateTime OctroiPaidDate, int VehicleType, string BabajiChallanNo, DateTime BabajiChallanDate,
    string ChallanPath, string DamageCopyPath, int TransitType,
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

        command.Parameters.Add("@RoadPermitNo", SqlDbType.NVarChar).Value = RoadPermitNo;
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@TransportationByBabaji", SqlDbType.Bit).Value = TransportationByBabaji;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@LRCopyPath", SqlDbType.NVarChar).Value = LRCopyPath;
        command.Parameters.Add("@DeliveryPoint", SqlDbType.NVarChar).Value = DeliveryPoint;
        command.Parameters.Add("@CargoReceivedBy", SqlDbType.NVarChar).Value = CargoReceivedBy;

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
        command.Parameters.Add("@DamageImage", SqlDbType.NVarChar).Value = DamageCopyPath;

        command.Parameters.Add("@TransitType", SqlDbType.Int).Value = TransitType;

        command.Parameters.Add("@DriverName", SqlDbType.NVarChar).Value = DriverName;
        command.Parameters.Add("@DriverPhone", SqlDbType.NVarChar).Value = DriverPhone;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDeliveryWarehouse", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int AddDeliveryConsolidateMS(int ConsolidateID, string VehicleNo, DateTime DispatchDate,
        string TransporterName, int TransporterId, bool IsCommonLR, int lUser, bool bDel)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ConsolidateID", SqlDbType.Int).Value = ConsolidateID;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@IsCommonLR", SqlDbType.Bit).Value = IsCommonLR;

        if (DispatchDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = DispatchDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@bDel", SqlDbType.Bit).Value = bDel;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDeliveryConsolidateMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDeliveryConsolidateDetail(int ConsolidateId, int DeliveryId, int JobId, int NoOfPackages, string VehicleNo, DateTime TruckReqDate,
            DateTime VehicleRcvdDate, string TransporterName, int TransporterId, string LRNo, DateTime LRDate, string DeliveryPoint, DateTime DispatchDate,
            string RoadPermitNo, DateTime RoadPermitDate, string PODPath, string NFormNo, DateTime NFormDate, DateTime NClosingDate,
            string SFormNo, DateTime SFormDate, DateTime SClosingDate, string OctroiAmount, string OctroiReceiptNo, DateTime OctroiPaidDate,
            int VehicleType, string BabajiChallanNo, DateTime BabajiChallanDate, string ChallanPath, string DamageCopyPath, string strIsRunwayDelivery,
                int LabourTypeId, int TransitType, int WarehouseId, DateTime ExamineDate, DateTime OutOfChargeDate, int lUser, bool bDel)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ConsolidateId", SqlDbType.Int).Value = ConsolidateId;
        command.Parameters.Add("@DeliveryId", SqlDbType.Int).Value = DeliveryId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;

        if (ExamineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ExamineDate", SqlDbType.DateTime).Value = ExamineDate;
        }
        if (OutOfChargeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OutOfChargeDate", SqlDbType.DateTime).Value = OutOfChargeDate;
        }

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
        command.Parameters.Add("@TransporterID", SqlDbType.NVarChar).Value = TransporterId;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@DeliveryPoint", SqlDbType.NVarChar).Value = DeliveryPoint;
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
            command.Parameters.Add("@BabajiChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;
        }

        command.Parameters.Add("@SFormNo", SqlDbType.NVarChar).Value = SFormNo;
        command.Parameters.Add("@OctroiReceiptNo", SqlDbType.NVarChar).Value = OctroiReceiptNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        command.Parameters.Add("@BabajiChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        command.Parameters.Add("@ChallanPath", SqlDbType.NVarChar).Value = ChallanPath;
        command.Parameters.Add("@DamageCopyPath", SqlDbType.NVarChar).Value = DamageCopyPath;

        command.Parameters.Add("@TransitType", SqlDbType.Int).Value = TransitType;
        command.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = WarehouseId;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@bDel", SqlDbType.Bit).Value = bDel;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDeliveryConsolidate", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDeliveryDetail(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("delDeliveryDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDeliveryWarehouseDetail(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("delDeliveryWarhouseDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddContainerDetail(int JobId, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insContainerDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateContainerDetail(int lid, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("UpdContainerDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteContainerDetail(int lid, Int32 lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delContainerDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddExbondInvoice(int JobId, string InvoiceId, string InbondQuantity, string ExbondQuantity, string Balance, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.NVarChar).Value = InvoiceId;
        command.Parameters.Add("@InbondQuantity", SqlDbType.NVarChar).Value = InbondQuantity;
        command.Parameters.Add("@ExbondQuantity", SqlDbType.NVarChar).Value = ExbondQuantity;
        command.Parameters.Add("@Balance", SqlDbType.NVarChar).Value = Balance;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insExbondInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateExbondInvoice(int JobId, string InvoiceId, string ExbondQuantity, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.NVarChar).Value = InvoiceId;
        command.Parameters.Add("@ExbondQuantity", SqlDbType.NVarChar).Value = ExbondQuantity;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updExbondInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPassingQuery(int JobId, string strQuery, DateTime QueryDate, string strFilePath, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Query", SqlDbType.NVarChar).Value = strQuery;
        command.Parameters.Add("@QueryDate", SqlDbType.DateTime).Value = QueryDate;
        command.Parameters.Add("@FilePath", SqlDbType.NVarChar).Value = strFilePath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPassingQuery", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPassingBond(int JobId, int WareHouseId, string NocNumber, DateTime NocDate,
            DateTime CompletedDate, bool bBondStatus, string strBondRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@WareHouseId", SqlDbType.Int).Value = WareHouseId;
        command.Parameters.Add("@NocNumber", SqlDbType.NVarChar).Value = NocNumber;
        command.Parameters.Add("@BondRemark", SqlDbType.NVarChar).Value = strBondRemark;

        if (NocDate != DateTime.MinValue)
            command.Parameters.Add("@NocDate", SqlDbType.DateTime).Value = NocDate;
        if (CompletedDate != DateTime.MinValue)
            command.Parameters.Add("@CompletedDate", SqlDbType.DateTime).Value = CompletedDate;

        command.Parameters.Add("@BondStatus", SqlDbType.Bit).Value = bBondStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPassingBondDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    //public static int AddCustomQuerydocPath(int JobId, string strQueryfilepath, int Doctype, int UserId)
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";

    //    command.Parameters.Add("@JobId",SqlDbType.Int).Value = JobId;
    //    command.Parameters.Add("@QueryDocPath",SqlDbType.NVarChar).Value =strQueryfilepath;
    //    command.Parameters.Add("@DocType",SqlDbType.Int).Value = Doctype;
    //    command.Parameters.Add("@lUser",SqlDbType.Int).Value = UserId;

    //    command.Parameters.Add("@OutPut",SqlDbType.Int).Direction =ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("insCustomQueryDocPath", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int UpdatePassingQuery(int QueryId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@QueryId", SqlDbType.Int).Value = QueryId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updPassingQuery", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddVehicleDetail(int AlertId, int ContainerId, string VehicleNo)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PreAlertId", SqlDbType.Int).Value = AlertId;
        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insVehicleDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobAdditionalFields(int JobId, int FieldId, string FieldValue, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@FieldValue", SqlDbType.NVarChar).Value = FieldValue;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobAdditionalField", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobDeliveryDetail(int JobId, DateTime TruckRequestDate, DateTime JobDeliveryDate, string DeliveryIns,
             string DeliveryDestination, string DeliveryAddress, bool TransportationByBabaji, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (TruckRequestDate != DateTime.MinValue)
        {
            command.Parameters.Add("@TruckRequestDate", SqlDbType.DateTime).Value = TruckRequestDate;
        }
        if (JobDeliveryDate != DateTime.MinValue)
        {
            command.Parameters.Add("@JobDeliveryDate", SqlDbType.DateTime).Value = JobDeliveryDate;
        }

        command.Parameters.Add("@DeliveryIns", SqlDbType.NVarChar).Value = DeliveryIns;
        command.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar).Value = DeliveryAddress;
        command.Parameters.Add("@DeliveryDestination", SqlDbType.NVarChar).Value = DeliveryDestination;
        command.Parameters.Add("@TransportationByBabaji", SqlDbType.Bit).Value = TransportationByBabaji;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDeliveryMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddADCDetail(int JobId, DateTime ADCWithdrawnDate, DateTime AdcNocDate, DateTime ADCExamDate, bool bStatus, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@bStatus", SqlDbType.Int).Value = bStatus;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@ADCWithdrawnDate", SqlDbType.DateTime).Value = ADCWithdrawnDate;

        if (AdcNocDate != DateTime.MinValue)
            command.Parameters.Add("@AdcNocDate", SqlDbType.DateTime).Value = AdcNocDate;

        if (ADCExamDate != DateTime.MinValue)
            command.Parameters.Add("@ADCExamDate", SqlDbType.DateTime).Value = ADCExamDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobAdcDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPHODetail(int JobId, DateTime PHOSubmitDate, DateTime PHOScrutinyDate, DateTime PHOPaymentDate, DateTime PHOAppointDate, DateTime PHOWithdrawnDate,
        DateTime PHOLabTestDate, DateTime PHOReportDate, bool bStatus, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@bStatus", SqlDbType.Int).Value = bStatus;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        command.Parameters.Add("@PHOSubmitDate", SqlDbType.DateTime).Value = PHOSubmitDate;

        if (PHOScrutinyDate != DateTime.MinValue)
            command.Parameters.Add("@PHOScrutinyDate", SqlDbType.DateTime).Value = PHOScrutinyDate;
        if (PHOPaymentDate != DateTime.MinValue)
            command.Parameters.Add("@PHOPaymentDate", SqlDbType.DateTime).Value = PHOPaymentDate;

        if (PHOAppointDate != DateTime.MinValue)
            command.Parameters.Add("@PHOAppointDate", SqlDbType.DateTime).Value = PHOAppointDate;

        if (PHOWithdrawnDate != DateTime.MinValue)
            command.Parameters.Add("@PHOWithdrawnDate", SqlDbType.DateTime).Value = PHOWithdrawnDate;

        if (PHOLabTestDate != DateTime.MinValue)
            command.Parameters.Add("@PHOLabTestDate", SqlDbType.DateTime).Value = PHOLabTestDate;

        if (PHOReportDate != DateTime.MinValue)
            command.Parameters.Add("@PHOReportDate", SqlDbType.DateTime).Value = PHOReportDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobPHODetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddADCPHODetail(int JobId, DateTime ADCWithdrawnDate, DateTime AdcNocDate, DateTime ADCExamDate,
        DateTime PHOSubmitDate, DateTime PHOScrutinyDate, DateTime PHOPaymentDate, DateTime PHOAppointDate,
        DateTime PHOWithdrawnDate, DateTime PHOLabTestDate, DateTime PHOReportDate, bool bStatus, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@bStatus", SqlDbType.Int).Value = bStatus;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        //ADC Detail
        command.Parameters.Add("@ADCWithdrawnDate", SqlDbType.DateTime).Value = ADCWithdrawnDate;

        if (AdcNocDate != DateTime.MinValue)
            command.Parameters.Add("@AdcNocDate", SqlDbType.DateTime).Value = AdcNocDate;

        if (ADCExamDate != DateTime.MinValue)
            command.Parameters.Add("@ADCExamDate", SqlDbType.DateTime).Value = ADCExamDate;

        // PHO Detail

        if (PHOSubmitDate != DateTime.MinValue)
            command.Parameters.Add("@PHOSubmitDate", SqlDbType.DateTime).Value = PHOSubmitDate;

        if (PHOScrutinyDate != DateTime.MinValue)
            command.Parameters.Add("@PHOScrutinyDate", SqlDbType.DateTime).Value = PHOScrutinyDate;
        if (PHOPaymentDate != DateTime.MinValue)
            command.Parameters.Add("@PHOPaymentDate", SqlDbType.DateTime).Value = PHOPaymentDate;

        if (PHOAppointDate != DateTime.MinValue)
            command.Parameters.Add("@PHOAppointDate", SqlDbType.DateTime).Value = PHOAppointDate;

        if (PHOWithdrawnDate != DateTime.MinValue)
            command.Parameters.Add("@PHOWithdrawnDate", SqlDbType.DateTime).Value = PHOWithdrawnDate;

        if (PHOLabTestDate != DateTime.MinValue)
            command.Parameters.Add("@PHOLabTestDate", SqlDbType.DateTime).Value = PHOLabTestDate;

        if (PHOReportDate != DateTime.MinValue)
            command.Parameters.Add("@PHOReportDate", SqlDbType.DateTime).Value = PHOReportDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJOBADCPHODetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddAdhocReport(string ReportName, string ColumnListId, string strConditionColumnId, int ReporType, int CustomerId, int IsCustomer, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = strConditionColumnId;
        command.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReporType;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@IsCustomer", SqlDbType.Int).Value = IsCustomer;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insAdhocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddAdhocCustomerReport_delete(string ReportName, string ColumnListId, string strConditionColumnId, int lUser, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = strConditionColumnId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insAdhocCustomerReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateAdhocReport(int ReportId, string ReportName, string ColumnListId, int ReportType, int CustomerId, string ConditionColumnId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = ConditionColumnId;
        command.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updAdhocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int UpdateAdhocCustomerReport_delete(int ReportId, string ReportName, string ColumnListId, string ConditionColumnId, int lUser, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = ConditionColumnId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updAdhocCustomerReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int DeleteAdhocReport(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelAdHocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobStatusAdhoc(int JobId, string strStatusName, string strStatusValue, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string Spresult = "1";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (strStatusName != "" && strStatusValue != "")
        {
            command.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = strStatusName;
            command.Parameters.Add("@StatusValue", SqlDbType.Int).Value = strStatusValue;
            command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
            command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
            Spresult = CDatabase.GetSPOutPut("updJobStatusAdhoc", command, "@OutPut");
        }

        return Convert.ToInt32(Spresult);
    }

    #endregion
    #endregion

    #region Access Role

    public static void FillRole(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "GetRoleById", command, "RoleName", "RoleId");
    }

    public static void FillRoleDetail(DropDownList cboItemType, string C)
    {
        if (C == "ROLE") //'ROLE' is use to fill allTypes of Role in dropdownlist
        {
            cboItemType.DataSource = CDatabase.GetDataSet("SELECT lRoleId AS lId,sName FROM BS_RoleMS WHERE bDel = 0 order by lId");
        }
        else if (C == "COMP") //'COMP' is use to fill allTypes of Companies in dropdownlist
        {
            //  cboItemType.DataSource = GetDataSet("SELECT lId, sName FROM CompanyMS WHERE bDel = 0 order by lId");
        }
        else if (C == "MODU") //'COMP' is use to fill allTypes of Modules in dropdownlist
        {
            //  cboItemType.DataSource = GetDataSet("SELECT lModuleId AS lId, ModuleName AS sName FROM ModuleMS order by lId");
        }
        cboItemType.DataTextField = "sName";
        cboItemType.DataValueField = "lId";
        cboItemType.DataBind();
    }

    public static DataTable GetRoleMenu(int RoleId)
    {
        SqlCommand command = new SqlCommand();
        DataTable dtForMenuu;
        command.Parameters.Add("@RoleId", SqlDbType.Int).Value = RoleId;

        return dtForMenuu = CDatabase.GetDataTable("GetRoleDetail", command, "ForMenu");
    }
    #endregion

    #region Product-InvoiceDetail

    public static int AddProductInvoiceDetail(int JobId, string InvoiceNo, DateTime InvoiceDate, string TermsOfInvoice,
        string ProductDescription, Decimal Quantity, string UnitOfProduct, Decimal UnitPrice, Decimal ProductAmount, string InvoiceCurrency, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@TermsOfInvoice", SqlDbType.NVarChar).Value = TermsOfInvoice;
        command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar).Value = ProductDescription;
        if (Quantity != -1)
        {
            command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;
        }
        if (UnitOfProduct != "")
        {
            command.Parameters.Add("@UnitOfProduct", SqlDbType.NVarChar).Value = UnitOfProduct;
        }
        if (UnitPrice != -1)
        {
            command.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = UnitPrice;
        }
        if (ProductAmount != -1)
        {
            command.Parameters.Add("@ProductAmount", SqlDbType.Decimal).Value = ProductAmount;
        }
        if (InvoiceCurrency != "")
        {
            command.Parameters.Add("@InvoiceCurrency", SqlDbType.NVarChar).Value = InvoiceCurrency;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insProductInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteProductInvoiceDetail(int lid)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delProductInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetProductInvoiceDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetProductInvoiceDetail", command);
    }

    public static DataSet GetProductInvoiceByInvoiceId(int JobId, int InvoiceId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        return CDatabase.GetDataSet("GetProductInvoiceDetailByInvoiceId", command);
    }

    public static DataSet GetJobInvoiceDuty(int JobId, int InvoiceId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        return CDatabase.GetDataSet("GetJobInvoiceDuty", command);
    }
    public static DataSet SearchProductInvoiceByValue(int JobId, int InvoiceId, decimal UnitPrice, decimal Quantity, decimal ItemAmount)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = UnitPrice;
        command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;
        command.Parameters.Add("@ItemAmount", SqlDbType.Decimal).Value = ItemAmount;
        return CDatabase.GetDataSet("SrchProductInvoiceDetail", command);
    }

    public static DataSet SearchProductInvoiceByValueABB(int JobId, int InvoiceId, decimal UnitPrice, decimal Quantity, decimal ItemAmount)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = UnitPrice;
        command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;
        command.Parameters.Add("@ItemAmount", SqlDbType.Decimal).Value = ItemAmount;
        return CDatabase.GetDataSet("SrchProductInvoiceDetailABB", command);
    }

    public static DataSet GetProductInvoiceDetailHSN(int JobId, bool IsIGST)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@TaxType", SqlDbType.Bit).Value = IsIGST;

        return CDatabase.GetDataSet("GetProductInvoiceDetailHSN", command);
    }
    public static DataSet GetProductInvoiceDetailGroupHSN(int JobId, bool IsIGST)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@TaxType", SqlDbType.Bit).Value = IsIGST;

        return CDatabase.GetDataSet("GetProductInvoiceDetailGroupHSN", command);
    }

    public static DataSet GetInvoicenoProduct(int JobId, string InvoiceNo)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        return CDatabase.GetDataSet("GetDistinctInvoicenoProduct", command);
    }

    public static DataSet GetDistinctInvoiceNumber(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetDistinctInvoiceNo", command);
    }

    public static int UpdateProductInvoiceDetail(int lid, string InvoiceNo, DateTime InvoiceDate, string TermsOfInvoice,
        string ProductDescription, Decimal Quantity, string UnitOfProduct, Decimal UnitPrice, Decimal ProductAmount, string InvoiceCurrency, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@TermsOfInvoice", SqlDbType.NVarChar).Value = TermsOfInvoice;
        command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar).Value = ProductDescription;
        if (Quantity != -1)
        {
            command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;
        }
        if (UnitOfProduct != "")
        {
            command.Parameters.Add("@UnitOfProduct", SqlDbType.NVarChar).Value = UnitOfProduct;
        }
        if (UnitPrice != -1)
        {
            command.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = UnitPrice;
        }
        if (ProductAmount != -1)
        {
            command.Parameters.Add("@ProductAmount", SqlDbType.Decimal).Value = ProductAmount;
        }
        if (InvoiceCurrency != "")
        {
            command.Parameters.Add("@InvoiceCurrency", SqlDbType.NVarChar).Value = InvoiceCurrency;
        }
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updProductInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);


    }
    #endregion

    #region Dahsboard
    public static DataSet GetPortWiseJob(int JobId)
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("ds_PortwiseJob", command);

    }
    public static DataSet GetChartSector(int UserId, int ModeId, int Duration, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@Duration", SqlDbType.Int).Value = Duration;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopSector", command);

        return dsReport;
    }

    public static DataSet GetChartCustomer(int UserId, int ModeId, int Duration, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@Duration", SqlDbType.Int).Value = Duration;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopCustomer", command);

        return dsReport;
    }

    public static DataSet GetChartBranch(int UserId, int ModeId, int Duration, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@Duration", SqlDbType.Int).Value = Duration;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopBranch", command);

        return dsReport;
    }

    public static DataSet GetChartPort(int UserId, int ModeId, int Duration, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@Duration", SqlDbType.Int).Value = Duration;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopPort", command);

        return dsReport;
    }

    public static DataSet GetChartJobType(int UserId, int ModeId, int Duration, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@Duration", SqlDbType.Int).Value = Duration;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTopJobType", command);

        return dsReport;
    }

    public static DataSet GetChartTEU(int UserId, int ModeId, int Duration, int FinYear)
    {
        DataSet dsReport;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@Duration", SqlDbType.Int).Value = Duration;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        dsReport = CDatabase.GetDataSet("ds_rptTEUPort", command);

        return dsReport;
    }

    #endregion

    #region PCD Document

    public static DataSet FillPendingPCDDoc(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetPendingPCDDoc", command);
    }

    public static void FillPCDDocument(DropDownList DropDownList)
    {
        CDatabase.BindControls(DropDownList, "SELECT lId,sName FROM BS_PCDDocumentMS WHERE bDel = 0 ORDER BY sName", "sName", "lId");
    }

    public static void FillPCDDocument(CheckBoxList CheckBoxList)
    {
        CDatabase.BindControls(CheckBoxList, "SELECT lId,sName FROM BS_PCDDocumentMS Where bDel= 0 ORDER BY sName", "sName", "lId");
    }

    public static DataSet FillPCDDocument()
    {
        return CDatabase.GetDataSet("SELECT lId,sName FROM BS_PCDDocumentMS WHERE bDel = 0 ORDER BY sName");
    }

    public static DataSet FillPCDDocumentCustomerPCA(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetPCDCustomerPCADocMS", command);
    }

    public static DataSet FillPCDDocumentByWorkFlow(int JobId, int TypeId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = TypeId;

        return CDatabase.GetDataSet("GetPCDDocumentByWorkFlow", command);
    }

    public static DataSet GetJobDetailforPCAPrint(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetJobDetailForPCDPrint", command);
    }
    public static void AddPCDBackOfficeDoc(int DocumentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        CDatabase.ExecuteSP("insPCDDocPath", command);
    }

    public static void AddPCDConsolidatedJob(string JobIDList, int lUser)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobIDList", SqlDbType.NVarChar).Value = JobIDList;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        CDatabase.ExecuteSP("insPCDConsoleJobId", command);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DocumentIdList">comma "," separated Document Id</param>
    /// <param name="JobId"></param>
    /// <param name="lUser"></param>
    public static int AddPCDBackOfficeList(string DocumentIdList, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentList", SqlDbType.NVarChar).Value = DocumentIdList;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDBackOfficeDocList", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDToBackOffice(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToBackOffice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDDToCustomerList(string DocumentIdList, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentList", SqlDbType.NVarChar).Value = DocumentIdList;
        command.Parameters.Add("@PCDToCustomer", SqlDbType.Bit).Value = true;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToCustomerDocList", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDToCustomer(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        // command.Parameters.Add("@DispatchLocation", SqlDbType.NVarChar).Value = strDispatchLocation;
        // command.Parameters.Add("@HandoverDate", SqlDbType.DateTime).Value = HandoverDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToCustomer", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDToScrutiny(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToScrutiny", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectScrutiny(int JobId, bool IsApproved, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDScrutinyApproval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDToTransport(int JobId, string strPersonName, DateTime TransportDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = strPersonName;
        command.Parameters.Add("@TransportDate", SqlDbType.DateTime).Value = TransportDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToTransport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDToBilling(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToBilling", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int CheckPaymentReceiptPending(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_CheckPaymentReceiptPending", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetPCDRequiredDocument(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("CheckBillingDocUploaded", command);
    }

    //public static int AddPCDToBilling(int JobId, string strDispatchLocation, string strInvoiceNumber, string strInvoiceAmount, DateTime dtDraftInvoiceDate,
    //   DateTime dtCheckingDate, DateTime dtFinalTypingDate, DateTime dtInvoiceDate, DateTime dtGenerlisingDate, DateTime dtDispatchDate, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";
    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
    //    command.Parameters.Add("@DispatchLocation", SqlDbType.NVarChar).Value = strDispatchLocation;
    //    command.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar).Value = strInvoiceNumber;
    //    command.Parameters.Add("@InvoiceAmount", SqlDbType.NVarChar).Value = strInvoiceAmount;

    //    command.Parameters.Add("@DraftInvoiceDate", SqlDbType.DateTime).Value = dtDraftInvoiceDate;
    //    command.Parameters.Add("@CheckingDate", SqlDbType.DateTime).Value = dtCheckingDate;
    //    command.Parameters.Add("@FinalTypingDate", SqlDbType.DateTime).Value = dtFinalTypingDate;
    //    command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = dtInvoiceDate;
    //    command.Parameters.Add("@GenerlisingDate", SqlDbType.DateTime).Value = dtGenerlisingDate;
    //    command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = dtDispatchDate;

    //    command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

    //    command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("insPCDToBilling", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int AddPCDToDispatch(int JobId, string strCarryingPerson, string DocketNo, string strReceivedBy, int TypeOfDelivery,
        DateTime dtDeliveryDate, DateTime dtDispatchDate, Boolean PCDCustomer, string strFilePath, bool bStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@CarryingPerson", SqlDbType.NVarChar).Value = strCarryingPerson;
        command.Parameters.Add("@DocketNo", SqlDbType.NVarChar).Value = DocketNo;
        command.Parameters.Add("@ReceivedBy", SqlDbType.NVarChar).Value = strReceivedBy;
        command.Parameters.Add("@TypeOfDelivery", SqlDbType.Int).Value = TypeOfDelivery;
        if (dtDeliveryDate != DateTime.MinValue)
            command.Parameters.Add("@PCDDeliveryDate", SqlDbType.DateTime).Value = dtDeliveryDate;

        if (dtDispatchDate != DateTime.MinValue)
            command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = dtDispatchDate;

        if (strFilePath != "")
            command.Parameters.Add("@DispatchDocPath", SqlDbType.NVarChar).Value = strFilePath;

        command.Parameters.Add("@PCDCustomer", SqlDbType.Bit).Value = PCDCustomer;
        command.Parameters.Add("@DispatchStatus", SqlDbType.Bit).Value = bStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToDispatch", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDToDispatchConsolidated(string JobId, string strCarryingPerson, string DocketNo, string strReceivedBy, int TypeOfDelivery,
      DateTime dtDeliveryDate, DateTime dtDispatchDate, Boolean PCDCustomer, string strFilePath, bool bStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobIdList", SqlDbType.NVarChar).Value = JobId;
        command.Parameters.Add("@CarryingPerson", SqlDbType.NVarChar).Value = strCarryingPerson;
        command.Parameters.Add("@DocketNo", SqlDbType.NVarChar).Value = DocketNo;
        command.Parameters.Add("@ReceivedBy", SqlDbType.NVarChar).Value = strReceivedBy;
        command.Parameters.Add("@TypeOfDelivery", SqlDbType.Int).Value = TypeOfDelivery;
        if (dtDeliveryDate != DateTime.MinValue)
            command.Parameters.Add("@PCDDeliveryDate", SqlDbType.DateTime).Value = dtDeliveryDate;

        if (dtDispatchDate != DateTime.MinValue)
            command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = dtDispatchDate;

        if (strFilePath != "")
            command.Parameters.Add("@DispatchDocPath", SqlDbType.NVarChar).Value = strFilePath;

        command.Parameters.Add("@PCDCustomer", SqlDbType.Bit).Value = PCDCustomer;
        command.Parameters.Add("@DispatchStatus", SqlDbType.Bit).Value = bStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToDispatchConsolidated", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }



    public static DataSet GetPCDToDispatchByType(int JobId, Boolean PCDCustomer)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PCDCustomer", SqlDbType.Bit).Value = PCDCustomer;

        return CDatabase.GetDataSet("GetPCDToDispatchByType", command);
    }

    //public static void AddPCDdocPath(string DocPath, int doctype, int JobId, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();

    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
    //    command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = DocPath;
    //    command.Parameters.Add("@DocType", SqlDbType.Int).Value = doctype;
    //   // command.Parameters.Add("@PCDToCustomer", SqlDbType.Int).Value = PCDToCustomer;
    //    command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

    //    CDatabase.ExecuteSP("insPCDDocPath", command);
    //}

    public static int AddPCDPendingDoc(string DocumentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocAwaited", SqlDbType.NVarChar).Value = DocumentId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insPCDPendingDoc", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddPCDDocument(int JobId, int DocumentId, int DocumentForType, bool IsCopy, bool IsOriginal, string Docpath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        command.Parameters.Add("@IsCopy", SqlDbType.Bit).Value = IsCopy;
        command.Parameters.Add("@IsOriginal", SqlDbType.Bit).Value = IsOriginal;

        if (Docpath != "")
            command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = Docpath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDDocForWorkflow", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteUploadedPCDDoc(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delUploadedPCDDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeletePCDDocument(int JobId, int DocumentId, int DocumentForType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@PCDDocId", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delPCDDocForWorkflow", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet FillPCDDocumentForWorkFlow(int JobId, int DocumentForType)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentForType", SqlDbType.Int).Value = DocumentForType;
        return CDatabase.GetDataSet("GetPCDDocumentForWorkFlow", command);
    }

    #endregion

    #region DO Planning

    public static DataView GetJobDetailForDOPlanning(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetJobDetailForDOPlanning", cmd);
    }

    public static int AddDOPlanningDetail(int JobId, DateTime ConsolProcessDate, DateTime FinalProcessDate, DateTime OBLReceivedDate,
      int Department, int TypeOfDelivery, string Remark, int DocHandTo, DateTime DocHandoverDate,
      string ShippingName, int ShippingId, int IsDoSecurity, int SecType, string SecCheqDDNo, DateTime SecCheqDDDate, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (ConsolProcessDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ConsolProcessDate", SqlDbType.DateTime).Value = ConsolProcessDate;
        }
        if (FinalProcessDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FinalProcessDate", SqlDbType.DateTime).Value = FinalProcessDate;
        }
        if (OBLReceivedDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OBLReceivedDate", SqlDbType.DateTime).Value = OBLReceivedDate;
        }
        command.Parameters.Add("@Department", SqlDbType.Int).Value = Department;
        command.Parameters.Add("@TypeOfDelivery", SqlDbType.Int).Value = TypeOfDelivery;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@DocHandTo", SqlDbType.Int).Value = DocHandTo;
        if (DocHandoverDate != DateTime.MinValue)
        {
            command.Parameters.Add("@DocHandoverDate", SqlDbType.DateTime).Value = DocHandoverDate;
        }

        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = ShippingName;
        command.Parameters.Add("@ShippingId", SqlDbType.Int).Value = ShippingId;

        command.Parameters.Add("@IsDoSecurity", SqlDbType.Int).Value = IsDoSecurity;     //DO Security
        command.Parameters.Add("@SecType", SqlDbType.Int).Value = SecType;               //DO Security
        command.Parameters.Add("@SecCheqDDNo", SqlDbType.NVarChar).Value = SecCheqDDNo;  //DO Security
        if (SecCheqDDDate != DateTime.MinValue)                                          //DO Security
        {
            command.Parameters.Add("@SecCheqDDDate", SqlDbType.DateTime).Value = SecCheqDDDate;
        }

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDOPlanningDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDOPlanRequiredDoc(int JobId, int ReqDoc, int ReqSelect, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ReqDoc", SqlDbType.Int).Value = ReqDoc;
        command.Parameters.Add("@ReqSelect", SqlDbType.Int).Value = ReqSelect;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDOPlanRequireDoc", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDOProcessingRemark(int JobId, int ProcessRemark, int IsProcessRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ProcessRemark", SqlDbType.Int).Value = ProcessRemark;
        command.Parameters.Add("@IsProcessRemark", SqlDbType.Int).Value = IsProcessRemark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDOProcessRemark", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataView GetDOPlanRequiredDoc(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetDOPlanRequiredDoc", cmd);
    }

    public static DataView GetDOPlanProcessRemark(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetDOPlanProcessRemark", cmd);
    }

    public static DataSet GetUploadedDOPlanDocument(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetUploadedDocument", cmd);
    }


    public static void BindDOProcessRMK(CheckBoxList CheckBoxList)
    {
        CDatabase.BindControls(CheckBoxList, "SELECT lid,DOProcessRemark FROM BS_DOProcessRemarkMS AS DR  WHERE bdel=0", "DOProcessRemark", "lid");
    }

    public static void BindDORequiredDoc(CheckBoxList CheckBoxList)
    {
        CDatabase.BindControls(CheckBoxList, "SELECT lid,DORequiredDoc FROM BS_DORequiredDocMS  WHERE bDel=0", "DORequiredDoc", "lid");
    }


    #endregion

    public static SqlConnection DisconnectSQL(SqlConnection Con)
    {
        if (Con.State == ConnectionState.Open)
        {
            Con.Close();
        }

        //Con.Close();
        return Con;
    }
    public static int AddCustomGroup(int PortId, string CustomGroupId, string PersonName, string MobileNo, int lUser, Boolean Status)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@CustomGroupId", SqlDbType.NVarChar).Value = CustomGroupId;
        command.Parameters.Add("@GroupUserName", SqlDbType.NVarChar).Value = PersonName;
        command.Parameters.Add("@GroupUserMobile", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@Status", SqlDbType.Bit).Value = Status;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomGroupDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCustomeGroup(int lid, int PortId, string CustomGroupId, string PersonName, string MobileNo, int lUser, Boolean Status)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        command.Parameters.Add("@CustomGroupId", SqlDbType.NVarChar).Value = CustomGroupId;
        command.Parameters.Add("@GroupUserName", SqlDbType.NVarChar).Value = PersonName;
        command.Parameters.Add("@GroupUserMobile", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@Status", SqlDbType.Bit).Value = Status;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomGroupDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetAllotedCustomsGroupById(int GroupId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@lId", SqlDbType.Int).Value = GroupId;

        return CDatabase.GetDataSet("GetAllottedCustomGroup", cmd);
    }

    public static DataSet GetCustomsGroupUserMobile(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetCustomsGroupUserMobile", cmd);
    }

    public static DataSet GetChecklistHold(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetChecklistHold", command);
    }

    public static int UpdateChecklistHold(int HoldId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@HoldId", SqlDbType.Int).Value = HoldId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updChecklistHold", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobUnlock(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobUnlock", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobNotofication(int JobId, int NotificationMode, int NotificationType, string strSentTo, string strSentCC,
          string strSubject, string strMessage, string strStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModId", SqlDbType.Int).Value = NotificationMode;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = NotificationType;
        command.Parameters.Add("@SentTo", SqlDbType.NVarChar).Value = strSentTo;
        command.Parameters.Add("@SentCC", SqlDbType.NVarChar).Value = strSentCC;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = strSubject;
        command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = strMessage;
        command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = strStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobNotificationLog", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    
    public static int AddJobNotoficationList(string strJobIdList, int NotificationMode, int NotificationType, string strSentTo, string strSentCC,
          string strSubject, string strMessage, string strStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobIdList", SqlDbType.NVarChar).Value = strJobIdList;
        command.Parameters.Add("@ModId", SqlDbType.Int).Value = NotificationMode;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = NotificationType;
        command.Parameters.Add("@SentTo", SqlDbType.NVarChar).Value = strSentTo;
        command.Parameters.Add("@SentCC", SqlDbType.NVarChar).Value = strSentCC;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = strSubject;
        command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = strMessage;
        command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = strStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobNotificationLogList", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddUserNotification(int UserId, int NotificationTypeId, bool IsEmail, bool IsSMS, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@NotificationTypeId", SqlDbType.Int).Value = NotificationTypeId;
        command.Parameters.Add("@IsEmail", SqlDbType.Bit).Value = IsEmail;
        command.Parameters.Add("@IsSMS", SqlDbType.Bit).Value = IsSMS;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insUserNotification", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteUserNotification(int NotificationId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = NotificationId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("DelUserNotification", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateBillingInstructions(int JobID, string strInstructions, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobID", SqlDbType.BigInt).Value = JobID;
        command.Parameters.Add("@Instructions", SqlDbType.NVarChar).Value = strInstructions;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updBillingInstructions", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #region Freight

    public static string GetNewFreightEnquiryNo()
    {
        string strEnqRefNo = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        strEnqRefNo = CDatabase.GetSPOutPut("FR_GetNewEnqRefNo", command, "@OutPut");

        return strEnqRefNo;
    }

    public static int AddFreightEnquiry(string strEnqRefNumber, int FreightType, int FreightMode, string strCustRefNo, string strCustomerName,
             string strShipper, string strConsignee, int CountryId, int PortOfLoadingId, int PortOfDischargeId, int TermsId, int EnquiryValue, string strAgentName, int SalesRepId,
            int Count20, int Count40, int ContainerType, string strContainerSubType, string LCLVolume, int NoOfPackages, string strGrossWeight, string strChargeWeight, bool IsDangerous, string strRemarks, int AssignedTo, int CreatedBy)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ENQRefNo", SqlDbType.NVarChar).Value = strEnqRefNumber;
        command.Parameters.Add("@lType", SqlDbType.Int).Value = FreightType;
        command.Parameters.Add("@lMode", SqlDbType.Int).Value = FreightMode;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = strCustRefNo;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = strCustomerName;
        command.Parameters.Add("@Shipper", SqlDbType.NVarChar).Value = strShipper;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = strConsignee;
        command.Parameters.Add("@CountryId", SqlDbType.Int).Value = CountryId;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@TermsId", SqlDbType.Int).Value = TermsId;
        command.Parameters.Add("@EnquiryValue", SqlDbType.Int).Value = EnquiryValue;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;
        command.Parameters.Add("@SalesRepId", SqlDbType.NVarChar).Value = SalesRepId;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSubType", SqlDbType.NVarChar).Value = strContainerSubType;
        command.Parameters.Add("@LCLVolume", SqlDbType.NVarChar).Value = LCLVolume;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = strGrossWeight;
        command.Parameters.Add("@ChargeableWeight", SqlDbType.NVarChar).Value = strChargeWeight;
        command.Parameters.Add("@IsDangerousGood", SqlDbType.Bit).Value = IsDangerous;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = strRemarks;
        command.Parameters.Add("@AssignedTo", SqlDbType.Int).Value = AssignedTo;
        command.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insEnquiry", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int UpdateFreightEnquiry(int EnqID, int ModeId, int FreightType, string strCustRefNo, string strCustomerName,
            string strShipper, string strConsignee, int CountryId, int PortOfLoadingId, int PortOfDischargeId, int TermsId, string strAgentName, int SalesRepId,
            int Count20, int Count40, int ContainerType, string strContainerSubType, string LCLVolume, int NoOfPackages, string strGrossWeight,
            string strChargeWeight, bool IsDangerous, string strRemarks, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.NVarChar).Value = EnqID;
        command.Parameters.Add("@lMode", SqlDbType.NVarChar).Value = ModeId;
        command.Parameters.Add("@lType", SqlDbType.Int).Value = FreightType;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = strCustRefNo;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = strCustomerName;
        command.Parameters.Add("@Shipper", SqlDbType.NVarChar).Value = strShipper;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = strConsignee;
        command.Parameters.Add("@CountryId", SqlDbType.Int).Value = CountryId;
        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@TermsId", SqlDbType.Int).Value = TermsId;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;
        command.Parameters.Add("@SalesRepId", SqlDbType.NVarChar).Value = SalesRepId;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSubType", SqlDbType.NVarChar).Value = strContainerSubType;
        command.Parameters.Add("@LCLVolume", SqlDbType.NVarChar).Value = LCLVolume;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = strGrossWeight;
        command.Parameters.Add("@ChargeableWeight", SqlDbType.NVarChar).Value = strChargeWeight;
        command.Parameters.Add("@IsDangerousGood", SqlDbType.Bit).Value = IsDangerous;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = strRemarks;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_updEnquiry", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFreightStatus(int EnqId, int StatusId, DateTime StatusDate, int LostStatusID,int EnquiryValue, string Remark, int lUser)//int JobTypeID
    {
        SqlCommand command = new SqlCommand();
        string Spresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@StatudDate", SqlDbType.DateTime).Value = StatusDate;
        command.Parameters.Add("@LostStatusID", SqlDbType.Int).Value = LostStatusID;
        command.Parameters.Add("@EnquiryValue", SqlDbType.Int).Value = EnquiryValue;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        Spresult = CDatabase.GetSPOutPut("FR_updStatus", command, "@OutPut");
        return Convert.ToInt32(Spresult);
    }

    public static DataSet GetFreightDetail(int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        DataSet dsFreight = CDatabase.GetDataSet("FR_GetFreightDetail", command);

        return dsFreight;

    }

    public static int AddFreightReminder(int EnqId, int NotifyMode, int RemindUser, DateTime RemindDate, string RemindNotes, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@lMode", SqlDbType.Int).Value = NotifyMode;
        command.Parameters.Add("@RemindUser", SqlDbType.Int).Value = RemindUser;
        command.Parameters.Add("@RemindDate", SqlDbType.DateTime).Value = RemindDate;
        command.Parameters.Add("@RemindNotes", SqlDbType.NVarChar).Value = RemindNotes;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_InsReminder", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFreightReminder(int ReminderId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ReminderId", SqlDbType.Int).Value = ReminderId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_DelReminder", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightDocument(int EnqId, string DocumentName, string DocumentPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_InsDocument", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFreightDocument(int FreighDocId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@FreighDocId", SqlDbType.Int).Value = FreighDocId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_DelDocument", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static void FillFreightStatus(DropDownList DropDown, int StatusId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@StatusId", SqlDbType.Int, 4).Value = StatusId;
        CDatabase.BindControls(DropDown, "FR_GetStatusMS", command, "sName", "lId");
    }

    public static DataSet GetFreightPendingCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("FR_GetFreightCountForUser", command);

        return dsPending;
    }

    public static DataSet GetChartEnquiry(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("FR_ChartEnquiry", command);

        return dsPending;
    }

    public static DataSet GetChartMonth(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("FR_ChartMonthwise", command);

        return dsPending;
    }

    public static int AddEnquiryUser(int EnqId, string strUserList, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@UserList", SqlDbType.NVarChar).Value = strUserList;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insEnquiryUser", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }



    public static int AddFreightAgent(int EndId, int AgentID, bool isMailSent, int UserID)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EndId;
        command.Parameters.Add("@AgentID", SqlDbType.Int).Value = AgentID;
        command.Parameters.Add("@isMailSent", SqlDbType.Bit).Value = isMailSent;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserID;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insEnquiryAgent", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static void FillFreightAgentCompany(DropDownList DropdownList, int EnqID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqID", SqlDbType.Int).Value = EnqID;

        CDatabase.BindControls(DropdownList, "FR_GetEnquiryAgentComp", command, "CustName", "lid");
    }

    public static DataSet GetFreightAgent(int EndId)
    {
        DataSet dsDetail;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EndId;

        dsDetail = CDatabase.GetDataSet("FR_GetEnquiryAgent", command);
        return dsDetail;
    }

    public static DataSet GetFreightRateByPort(int TransMode, string POL, string POD, string Country, DateTime ValidityDate)
    {
        DataSet dsDetail;
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("POL", SqlDbType.NVarChar).Value = POL;
        command.Parameters.Add("POD", SqlDbType.NVarChar).Value = POD;
        command.Parameters.Add("Country", SqlDbType.NVarChar).Value = Country;
        command.Parameters.Add("ValidityDate", SqlDbType.DateTime).Value = ValidityDate;

        dsDetail = CDatabase.GetDataSet("FR_GetFreightRateByPort", command);

        return dsDetail;
    }

    public static int AddFreightSeaRate(string strCountry, string strPOL, string strPOD, string strShippingline, string strTransitDays,
        string str20GP, string str40GPHQ, string strCurrency, string strAgent, string strRemark, DateTime dtRateValidity, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = strCountry;
        command.Parameters.Add("@POL", SqlDbType.NVarChar).Value = strPOL;
        command.Parameters.Add("@POD", SqlDbType.NVarChar).Value = strPOD;
        command.Parameters.Add("@Shippingline", SqlDbType.NVarChar).Value = strShippingline;
        command.Parameters.Add("@TransitDays", SqlDbType.NVarChar).Value = strTransitDays;
        command.Parameters.Add("@20GP", SqlDbType.NVarChar).Value = str20GP;
        command.Parameters.Add("@40GPHQ", SqlDbType.NVarChar).Value = str40GPHQ;
        command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = strCurrency;
        command.Parameters.Add("@Agent", SqlDbType.NVarChar).Value = strAgent;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@RateValidityDate", SqlDbType.DateTime).Value = dtRateValidity;

        command.Parameters.Add("@lUser", SqlDbType.NVarChar).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insSeaRate", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightAirRate(string strCountry, string strPOL, string strPOD, string strMINCharge, string str45kg,
        string str100kg, string str300kg, string str500kg, string str1000kg, string strFSCCharge, string strSSCCharge,
        string strOtherCharge, string strAirline, string strCurrency, string strAgent, DateTime dtRateValidityDate, string strRemark, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = strCountry;
        command.Parameters.Add("@POL", SqlDbType.NVarChar).Value = strPOL;
        command.Parameters.Add("@POD", SqlDbType.NVarChar).Value = strPOD;
        command.Parameters.Add("@MINCharge", SqlDbType.NVarChar).Value = strMINCharge;
        command.Parameters.Add("@45kg", SqlDbType.NVarChar).Value = str45kg;
        command.Parameters.Add("@100kg", SqlDbType.NVarChar).Value = str100kg;
        command.Parameters.Add("@300kg", SqlDbType.NVarChar).Value = str300kg;
        command.Parameters.Add("@500kg", SqlDbType.NVarChar).Value = str500kg;
        command.Parameters.Add("@1000kg", SqlDbType.NVarChar).Value = str1000kg;
        command.Parameters.Add("@FSCCharge", SqlDbType.NVarChar).Value = strFSCCharge;
        command.Parameters.Add("@SSCCharge", SqlDbType.NVarChar).Value = strSSCCharge;
        command.Parameters.Add("@OtherCharge", SqlDbType.NVarChar).Value = strOtherCharge;

        command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = strCurrency;
        command.Parameters.Add("@Airline", SqlDbType.NVarChar).Value = strAirline;
        command.Parameters.Add("@Agent", SqlDbType.NVarChar).Value = strAgent;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@RateValidityDate", SqlDbType.DateTime).Value = dtRateValidityDate;

        command.Parameters.Add("@lUser", SqlDbType.NVarChar).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insAirRate", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFreightSeaRate(int FreightRateID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = FreightRateID;

        command.Parameters.Add("@lUser", SqlDbType.NVarChar).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_delSeaRate", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFreightAirRate(int AirRateID, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = AirRateID;

        command.Parameters.Add("@lUser", SqlDbType.NVarChar).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_delAirRate", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Freight Operation

    public static DataSet GetFreightOperationPendingCount(int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("FR_GetOperationCountForUser", command);

        return dsPending;
    }

    public static string GetNewBookingJobNo(string BranchId, string ModeId, string TypeId)
    {
        string strFRJobNo = "";
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ModeID", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = TypeId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        strFRJobNo = CDatabase.GetSPOutPut("FOP_GetNewJobNo", command, "@OutPut");

        return strFRJobNo;
    }

    public static int AddFreightBooking(string strBookingNo, int EnqID, int FreightMode, string strCustomer, string strConsignee, string strShipper,
        string strConsigneeAddress, int ConsigneeStateID, string strGSTN, string strShipperAddress, int PortOfLoadingId, int PortOfDischargeId, int TermsId, int BranchId,
        string strAgentName, int AgentCompId, int ContainerTypeId, string ContainerSubType, int Count20, int Count40, string LCLVolume, int NoOfPackages,
        int PackagesType, string strGrossWeight, string strChargeWeight, string InvoiceNo, DateTime InvoiceDate, string PONumber,
        string strDescription, DateTime BookingDate, string BookingDetails, int CustId, int ConsigneeId, int Division, int Plant,
        int CHABy, string CHAByName, string transportByName, int transportBy, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@BookingNo", SqlDbType.NVarChar).Value = strBookingNo;
        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@FreightMode", SqlDbType.Int).Value = FreightMode;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = strCustomer;

        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = strConsignee;
        command.Parameters.Add("@ConsigneeStateID", SqlDbType.NVarChar).Value = ConsigneeStateID;
        command.Parameters.Add("@ConsigneeAddress", SqlDbType.NVarChar).Value = strConsigneeAddress;
        command.Parameters.Add("@ConsigneeGSTN", SqlDbType.NVarChar).Value = strGSTN;

        command.Parameters.Add("@Shipper", SqlDbType.NVarChar).Value = strShipper;
        command.Parameters.Add("@ShipperAddress", SqlDbType.NVarChar).Value = strShipperAddress;

        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@TermsId", SqlDbType.Int).Value = TermsId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;
        command.Parameters.Add("@AgentCompId", SqlDbType.Int).Value = AgentCompId;

        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerTypeId;
        command.Parameters.Add("@ContainerSubType", SqlDbType.NVarChar).Value = ContainerSubType;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@LCLVolume", SqlDbType.NVarChar).Value = LCLVolume;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackagesType;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = strGrossWeight;
        command.Parameters.Add("@ChargeWeight", SqlDbType.NVarChar).Value = strChargeWeight;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;

        command.Parameters.Add("@PONumber", SqlDbType.NVarChar).Value = PONumber;
        command.Parameters.Add("@CargoDescription", SqlDbType.NVarChar).Value = strDescription;
        command.Parameters.Add("@BookingDetails", SqlDbType.NVarChar).Value = BookingDetails;

        if (InvoiceDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        }

        if (BookingDate != DateTime.MinValue)
            command.Parameters.Add("@BookingDate", SqlDbType.DateTime).Value = BookingDate;

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;
        command.Parameters.Add("@ConsignId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@Division", SqlDbType.Int).Value = Division;
        command.Parameters.Add("@Plant", SqlDbType.Int).Value = Plant;
        command.Parameters.Add("@CHABy", SqlDbType.Int).Value = CHABy;
        command.Parameters.Add("@CHAByName", SqlDbType.NVarChar).Value = CHAByName;
        command.Parameters.Add("@transportBy", SqlDbType.Int).Value = transportBy;
        command.Parameters.Add("@transportByName", SqlDbType.NVarChar).Value = transportByName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insBooking", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int UpdateFreightBooking(string strBookingNo, int EnqID, int FreightMode, string strCustomer, string strConsignee, string strShipper,
        string strConsigneeAddress, int ConsigneeStateID, string strGSTN, string strShipperAddress, int PortOfLoadingId, int PortOfDischargeId,
        int TermsId, int BranchId, string strAgentName, int AgentID, int ContainerTypeId, int Count20, int Count40, string LCLVolume, int NoOfPackages,
        int PackageType, string strGrossWeight, string strChargeWeight, string InvoiceNo, DateTime InvoiceDate, string PONumber, string strDescription,
        int Division, int Plant, int TransportionBy, int CHABy, string TransporterName, string CHAName, DateTime BookingDate, string BookingDetails, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@BookingNo", SqlDbType.NVarChar).Value = strBookingNo;
        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@FreightMode", SqlDbType.Int).Value = FreightMode;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = strConsignee;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = strConsignee;
        command.Parameters.Add("@Shipper", SqlDbType.NVarChar).Value = strShipper;

        command.Parameters.Add("@ConsigneeStateID", SqlDbType.Int).Value = ConsigneeStateID;
        command.Parameters.Add("@ConsigneeAddress", SqlDbType.NVarChar).Value = strConsigneeAddress;
        command.Parameters.Add("@ConsigneeGSTN", SqlDbType.NVarChar).Value = strGSTN;

        command.Parameters.Add("@ShipperAddress", SqlDbType.NVarChar).Value = strShipperAddress;

        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@TermsId", SqlDbType.Int).Value = TermsId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AgentID", SqlDbType.Int).Value = AgentID;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;

        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerTypeId;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@LCLVolume", SqlDbType.NVarChar).Value = LCLVolume;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackageType;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = strGrossWeight;
        command.Parameters.Add("@ChargeWeight", SqlDbType.NVarChar).Value = strChargeWeight;

        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@PONumber", SqlDbType.NVarChar).Value = PONumber;
        command.Parameters.Add("@CargoDescription", SqlDbType.NVarChar).Value = strDescription;
        command.Parameters.Add("@Division", SqlDbType.NVarChar).Value = Division;
        command.Parameters.Add("@Plant", SqlDbType.NVarChar).Value = Plant;
        command.Parameters.Add("@TransportionBy", SqlDbType.NVarChar).Value = TransportionBy;
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@CHABy", SqlDbType.NVarChar).Value = CHABy;
        command.Parameters.Add("@CHAName", SqlDbType.NVarChar).Value = CHAName;

        command.Parameters.Add("@BookingDetails", SqlDbType.NVarChar).Value = BookingDetails;

        if (InvoiceDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        }

        if (BookingDate != DateTime.MinValue)
            command.Parameters.Add("@BookingDate", SqlDbType.DateTime).Value = BookingDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updBooking", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFreightDebitNote(int EnqID, string strDebitNoteAmount, string strDebitNoteRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;

        command.Parameters.Add("@DebitNoteAmount", SqlDbType.NVarChar).Value = strDebitNoteAmount;
        command.Parameters.Add("@DebitNoteRemark", SqlDbType.NVarChar).Value = strDebitNoteRemark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updDebitNote", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddAgentPreAlert(int EnqID, string strMBLNo, string strHBLNo, string strVesselName, string strVesselNumber,
        DateTime dtMBLDate, DateTime dtHBLDate, DateTime dtETDDate, DateTime dtETADate, string FinalAgent, int FinalAgentID, bool IsCompleted, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@MBLNo", SqlDbType.NVarChar).Value = strMBLNo;
        command.Parameters.Add("@HBLNo", SqlDbType.NVarChar).Value = strHBLNo;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = strVesselName;
        command.Parameters.Add("@VesselNumber", SqlDbType.NVarChar).Value = strVesselNumber;
        command.Parameters.Add("@FinalAgent", SqlDbType.NVarChar).Value = FinalAgent;
        command.Parameters.Add("@FinalAgentID", SqlDbType.Int).Value = FinalAgentID;
        command.Parameters.Add("@IsCompleted", SqlDbType.NVarChar).Value = IsCompleted;

        if (dtMBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MBLDate", SqlDbType.DateTime).Value = dtMBLDate;
        }
        if (dtHBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HBLDate", SqlDbType.DateTime).Value = dtHBLDate;
        }
        if (dtETDDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETDDate", SqlDbType.DateTime).Value = dtETDDate;
        }
        if (dtETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = dtETADate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insAgentPreAlert", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateAgentPreAlert(int EnqID, string strMBLNo, string strHBLNo, string strVesselName, string strVesselNumber,
                 DateTime dtMBLDate, DateTime dtHBLDate, DateTime dtETDDate, DateTime dtETADate, string FinalAgent, int FinalAgentID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@MBLNo", SqlDbType.NVarChar).Value = strMBLNo;
        command.Parameters.Add("@HBLNo", SqlDbType.NVarChar).Value = strHBLNo;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = strVesselName;
        command.Parameters.Add("@VesselNumber", SqlDbType.NVarChar).Value = strVesselNumber;
        command.Parameters.Add("@FinalAgent", SqlDbType.NVarChar).Value = FinalAgent;
        command.Parameters.Add("@FinalAgentID", SqlDbType.Int).Value = FinalAgentID;

        if (dtMBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MBLDate", SqlDbType.DateTime).Value = dtMBLDate;
        }
        if (dtHBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HBLDate", SqlDbType.DateTime).Value = dtHBLDate;
        }
        if (dtETDDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETDDate", SqlDbType.DateTime).Value = dtETDDate;
        }
        if (dtETADate != DateTime.MinValue)
        {
            command.Parameters.Add("@ETADate", SqlDbType.DateTime).Value = dtETADate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updAgentPreAlert", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerPreAlert(int EnqID, DateTime ShippedOnBoardDate, DateTime PreAlertToCustDate, string CustomerEmail, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;

        if (ShippedOnBoardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ShippedOnBoardDate", SqlDbType.DateTime).Value = ShippedOnBoardDate;
        }
        if (PreAlertToCustDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PreAlertToCustDate", SqlDbType.DateTime).Value = PreAlertToCustDate;
        }

        command.Parameters.Add("@CustomerEmail", SqlDbType.NVarChar).Value = CustomerEmail;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insCustomerPreAlert", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCustomerPreAlert(int EnqID, DateTime ShippedOnBoardDate, DateTime PreAlertToCustDate, string CustomerEmail, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;

        if (ShippedOnBoardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ShippedOnBoardDate", SqlDbType.DateTime).Value = ShippedOnBoardDate;
        }
        if (PreAlertToCustDate != DateTime.MinValue)
        {
            command.Parameters.Add("@PreAlertToCustDate", SqlDbType.DateTime).Value = PreAlertToCustDate;
        }

        command.Parameters.Add("@CustomerEmail", SqlDbType.NVarChar).Value = CustomerEmail;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updCustomerPreAlert", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCustomerPreAlertEmailStatus(int EnqID, string CustEmail, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;

        command.Parameters.Add("@CustEmail", SqlDbType.NVarChar).Value = CustEmail;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updCustomerPreAlertEmail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCargoArrival(int EnqID, string strIGMNo, DateTime IGMDate, DateTime ATADate, string strItemNo, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@IGMNo", SqlDbType.NVarChar).Value = strIGMNo;
        command.Parameters.Add("@IGMDate", SqlDbType.DateTime).Value = IGMDate;
        command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = ATADate;
        command.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insCargoArrival", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCargoArrival(int EnqID, string strIGMNo, DateTime dtIGMDate, DateTime dtATADate, string strItemNo, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@IGMNo", SqlDbType.NVarChar).Value = strIGMNo;
        command.Parameters.Add("@IGMDate", SqlDbType.DateTime).Value = dtIGMDate;
        command.Parameters.Add("@ATADate", SqlDbType.DateTime).Value = dtATADate;
        command.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updCargoArrival", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCANPercentDetail(int EnqID, int InvoiceItemId, int PercentItemId, string InvoiceTotal, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@InvoiceItemId", SqlDbType.Int).Value = InvoiceItemId;
        command.Parameters.Add("@PercentItemId", SqlDbType.Int).Value = PercentItemId;
        command.Parameters.Add("@InvoiceTotal", SqlDbType.Decimal).Value = InvoiceTotal;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insCANPercentDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteCANInvoiceDetail(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_DelCANInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDeliveryOrder(int EnqID, string CHAName, int PaymentTerm, string DOIssuedTo, int PayId, string ChequeNo,
        DateTime ChequeDate, decimal DOAmount, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@CHAName", SqlDbType.NVarChar).Value = CHAName;
        command.Parameters.Add("@PaymentTerm", SqlDbType.Int).Value = PaymentTerm;
        command.Parameters.Add("@DOIssuedTo", SqlDbType.NVarChar).Value = DOIssuedTo;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PayId;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("@DOAmount", SqlDbType.Decimal).Value = DOAmount;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        if (ChequeDate != DateTime.MinValue)
            command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = ChequeDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insDeliveryOrder", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDeliveryOrder(int EnqID, string CHAName, int PaymentTerm, string DOIssuedTo, int PayId, string ChequeNo,
        DateTime ChequeDate, Decimal DOAmount, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@CHAName", SqlDbType.NVarChar).Value = CHAName;
        command.Parameters.Add("@PaymentTerm", SqlDbType.Int).Value = PaymentTerm;
        command.Parameters.Add("@DOIssuedTo", SqlDbType.NVarChar).Value = DOIssuedTo;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PayId;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        command.Parameters.Add("@DOAmount", SqlDbType.Decimal).Value = DOAmount;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        if (ChequeDate != DateTime.MinValue)
            command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = ChequeDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updDeliveryOrder", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightAdvice(int EnqID, bool IsInvoiceRcvd, bool IsSentToBilling, string Remark, int LRpending, int ModuleId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@IsInvoiceReceived", SqlDbType.Bit).Value = IsInvoiceRcvd;
        command.Parameters.Add("@IsSentToBilling", SqlDbType.Bit).Value = IsSentToBilling;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@BillLRStatus", SqlDbType.Bit).Value = LRpending;//ADDED BY 20032020
        command.Parameters.Add("@ModuleId", SqlDbType.NVarChar).Value = ModuleId;//ADDED BY 20032020

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insBillingAdvice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightBilling(int EnqID, DateTime dtFileReceivedDate, string strBillNumber, DateTime dtBillDate, decimal decBillAmount,
        string Remark, bool bBillingStatus, bool bAgentInvoiceStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@FileReceivedDate", SqlDbType.DateTime).Value = dtFileReceivedDate;
        command.Parameters.Add("@BillNumber", SqlDbType.NVarChar).Value = strBillNumber;
        command.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = decBillAmount;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        if (dtBillDate != DateTime.MinValue)
            command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = dtBillDate;

        command.Parameters.Add("@IsBillingCompleted", SqlDbType.Bit).Value = bBillingStatus;
        command.Parameters.Add("@AgentInvoiceStatus", SqlDbType.Bit).Value = bAgentInvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insBillingDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightAgentInvoice(int EnqID, DateTime dtInvoiceReceivedDate, string strJBNumber, DateTime dtJBDate, int AgentID, string strAgentName,
        string strInvoiceNo, DateTime dtInvoiceDate, decimal decInvoiceAmount,
        int InvoiceCurrencyId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@JBNumber", SqlDbType.NVarChar).Value = strJBNumber;
        command.Parameters.Add("@AgentID", SqlDbType.NVarChar).Value = AgentID;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = strInvoiceNo;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = decInvoiceAmount;
        command.Parameters.Add("@InvoiceCurrencyId", SqlDbType.Int).Value = InvoiceCurrencyId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        if (dtInvoiceReceivedDate != DateTime.MinValue)
            command.Parameters.Add("@InvoiceReceivedDate", SqlDbType.DateTime).Value = dtInvoiceReceivedDate;

        if (dtInvoiceDate != DateTime.MinValue)
            command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = dtInvoiceDate;

        if (dtJBDate != DateTime.MinValue)
            command.Parameters.Add("@JBDate", SqlDbType.DateTime).Value = dtJBDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insAgentInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFreightAgentInvoice(int lid, int EnqID, string strJBNumber, DateTime dtJBDate, string strAgentName, string strInvoiceNo, DateTime dtInvoiceDate, decimal decInvoiceAmount,
        int InvoiceCurrencyId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@EnqID", SqlDbType.Int).Value = EnqID;
        command.Parameters.Add("@JBNumber", SqlDbType.NVarChar).Value = strJBNumber;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = strInvoiceNo;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = decInvoiceAmount;
        command.Parameters.Add("@InvoiceCurrencyId", SqlDbType.Int).Value = InvoiceCurrencyId;

        if (dtInvoiceDate != DateTime.MinValue)
            command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = dtInvoiceDate;

        if (dtJBDate != DateTime.MinValue)
            command.Parameters.Add("@JBDate", SqlDbType.DateTime).Value = dtJBDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updAgentInvoice", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateBillingDetail(int EnqID, string strBillNumber, DateTime dtBillDate, decimal decBillAmount, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@BillNumber", SqlDbType.NVarChar).Value = strBillNumber;
        command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = dtBillDate;
        command.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = decBillAmount;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updBillingDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static DataSet GetBookingDetail(int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        DataSet dsFreight = CDatabase.GetDataSet("FOP_GetBookingDetail", command);

        return dsFreight;
    }

    public static DataSet GetOperationDetail(int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;

        return CDatabase.GetDataSet("FOP_GetOperationById", command);
    }

    public static DataSet GetAgenPreAlertDetail(int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        DataSet dsFreight = CDatabase.GetDataSet("FOP_GetAgentAlertDetail", command);

        return dsFreight;
    }

    public static DataSet GetCustomerPreAlertDetail(int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        DataSet dsFreight = CDatabase.GetDataSet("FOP_GetCustPreAlertDetail", command);

        return dsFreight;
    }

    public static DataSet GetInvoiceFieldDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("FOP_GetInvoiceFieldMS", "command");
    }

    public static DataSet GetInvoiceFieldById(int FieldId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;

        return CDatabase.GetDataSet("FOP_GetInvoiceFieldById", command);
    }

    public static DataSet GetInvoiceFieldValue(int FieldId, int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;

        return CDatabase.GetDataSet("FOP_GetInvoiceFieldValue", command);
    }

    public static DataSet GetCANInvoicebyID(int InvoiceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;

        return CDatabase.GetDataSet("FOP_GetCANInvoicebyID", command);
    }

    public static DataSet GetFreightDetailforCANPrint(int EnqId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        return CDatabase.GetDataSet("FOP_GetEnqForCANPrint", command);
    }

    public static DataSet GetFreightCANPrintInvoice(int EnqId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        return CDatabase.GetDataSet("FOP_GetCANPrintInvoiceDetail", command);
    }

    public static int DeleteInvoiceFieldMaster(int FieldId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_delInvoiceFieldMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightContainerMS(int EnqId, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insContainerMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFreightContainerMS(int lid, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_UpdContainerMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFreightContainerMS(int lid, Int32 lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_delContainerMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int AddFreightActivity(int EnqId, string DailyProgress, string DocumentPath, int StatusId,
                Boolean VisibleToCustomer, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@DocumentPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteFreightActivity(int ActivityId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_delDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static string GetFreightConsigneeAddress(string strConsignee)
    {
        string strAddress = "";

        if (strConsignee != "")
        {
            string strQuery = "SELECT sAddress AS Value FROM FOP_ConsigneeMS WHERE sName=@ConsigneeName";

            SqlCommand command = new SqlCommand();
            command.CommandText = strQuery;
            command.Parameters.Add("@ConsigneeName", SqlDbType.NVarChar).Value = strConsignee;

            strAddress = CDatabase.ResultInString(command);
        }

        return strAddress;
    }

    public static string GetFreightShipperAddress(string strShipper)
    {
        string strAddress = "";

        if (strShipper != "")
        {
            string strQuery = "SELECT sAddress AS Value FROM FOP_ShipperMS WHERE sName= @ShipperName";

            SqlCommand command = new SqlCommand();
            command.CommandText = strQuery;
            command.Parameters.Add("@ShipperName", SqlDbType.NVarChar).Value = strShipper;

            strAddress = CDatabase.ResultInString(command);
        }

        return strAddress;
    }

    public static int AddFreightBillingReceivedFile(int EnqId, int ReceivedBy)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@ReceivedBy", SqlDbType.Int).Value = ReceivedBy;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insBillingReceivedfile", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDebitNote(int EnqId, string DebitNoteAmount, string GSTRate, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@DebitNoteAmount", SqlDbType.Decimal).Value = DebitNoteAmount;
        command.Parameters.Add("@GSTRate", SqlDbType.Decimal).Value = GSTRate;
        command.Parameters.Add("@DebitNoteRemark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insDebitNote", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static int AddDebitNote(int EnqId, string DebitNoteAmount, string GSTRate, int AgentId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@DebitNoteAmount", SqlDbType.Decimal).Value = DebitNoteAmount;
        command.Parameters.Add("@GSTRate", SqlDbType.Decimal).Value = GSTRate;
        command.Parameters.Add("@AgentId", SqlDbType.Int).Value = AgentId;
        command.Parameters.Add("@DebitNoteRemark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insDebitNote", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Freight Report

    public static DataSet GetFreightReportFieldGroup()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("FR_GetFieldGroup", cmd);
    }

    public static DataSet GetFreightReportChildNode(string ParentNode)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;

        return CDatabase.GetDataSet("FR_GetReportChildNode", cmd);
    }

    public static int AddFreightAdhocReport(string ReportName, string ColumnListId, string strConditionColumnId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = strConditionColumnId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insAdhocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateFreightAdhocReport(int ReportId, string ReportName, string ColumnListId, string ConditionColumnId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = ConditionColumnId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_updAdhocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int UpdateFreightAdhocReportLastGeneratedBy(int ReportId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_updAdhocReportGeneratedBy", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataView GetFreightAdhocReport(int ReportId, DateTime DateFrom, DateTime DateTo, string AdhocFilter, int FinYear, int UserId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateFrom;
        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTo;
        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = AdhocFilter;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataView("FR_rptAdHocReport", cmd);
    }

    public static SqlDataReader GetFreightReportConditionFields(int ReportId)
    {
        SqlDataReader drReportCondition;
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;

        return drReportCondition = CDatabase.GetDataReader("FR_GetFreightReportConditionFields", cmd);

    }

    public static DataSet GetFreightReportFieldNameById(int FieldId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        return CDatabase.GetDataSet("FR_GetFreightReportFieldNameById", cmd);
    }

    public static DataView GetFreightReport(int ReportId, DateTime DateFrom, DateTime DateTo, string AdhocFilter, int FinYear, int UserId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = DateFrom;
        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = DateTo;
        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = AdhocFilter;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataView("FR_rptAdHocReport", cmd);
    }

    public static DataSet GeFreighttReportColumnNamebyId(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("FR_GetReportColumnNamebyId", command);

    }

    public static DataSet GetFreightReportConditionFieldsbyId(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("FR_GetAdhocReportConditionFieldsById", command);

    }

    public static DataSet GetFreightAdHocReportDetail(int ReportId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        return CDatabase.GetDataSet("FR_GetReportDetail", command);

    }

    public static DataSet GetFreightDebitNote(int EnqId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        return CDatabase.GetDataSet("FOP_GetDebitNote", command);
    }

    public static DataSet GetFreighPrintDebitNote(int EnqId, int AgentId)
    {
        SqlCommand Cmd = new SqlCommand();
        Cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        Cmd.Parameters.Add("@AgentId", SqlDbType.Int).Value = AgentId;
        return CDatabase.GetDataSet("FOP_GetDebitNotePdfAgent", Cmd);
    }

    public static int updFrightDebitRemark(int DebitId, string DebitRemark, int lUser)
    {
        SqlCommand Cmd = new SqlCommand();
        string SPresult = "";
        Cmd.Parameters.Add("@Debitid", SqlDbType.Int).Value = DebitId;
        Cmd.Parameters.Add("@DebitNoteRemark", SqlDbType.NVarChar).Value = DebitRemark;
        Cmd.Parameters.Add("@luser", SqlDbType.Int).Value = lUser;

        Cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updDebitNoteRemark", Cmd, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Service Module

    public static int AddServiceRequest(int DeptID, string EmpName, string BranchName, string IssueRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@DeptID", SqlDbType.Int).Value = DeptID;
        command.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = EmpName;
        command.Parameters.Add("@BranchName", SqlDbType.NVarChar).Value = BranchName;
        command.Parameters.Add("@RequestRemark", SqlDbType.NVarChar).Value = IssueRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SR_insServiceRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int AddResolveRequest(int IssueID, string ResolveRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@IssueID", SqlDbType.Int).Value = IssueID;
        command.Parameters.Add("@ResolveRemark", SqlDbType.NVarChar).Value = ResolveRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("SR_insResolveRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    #endregion

    #region KPI - Performance Appraisal

    public static int KPI_CheckEmpTarget(int EmpID, string EmpEmail, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EMPID", SqlDbType.Int).Value = EmpID;
        command.Parameters.Add("@EmpEmail", SqlDbType.NVarChar).Value = EmpEmail;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KPI_CheckEmpTarget", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int KPI_ADDEmpTarget(int EmpID, string EmpName, string EmpEmail, string EmpCode, int HODID, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EMPID", SqlDbType.Int).Value = EmpID;
        command.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = EmpName;
        command.Parameters.Add("@EmpEmail", SqlDbType.NVarChar).Value = EmpEmail;
        command.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = EmpCode;
        command.Parameters.Add("@HODID", SqlDbType.Int).Value = HODID;
        command.Parameters.Add("@EmpRemark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KPI_insEMPTarget", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int KPI_UpdateEmpTarget(int KPIID, string EmpName, string EmpEmail, string EmpCode, int HODID, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@KPIID", SqlDbType.Int).Value = KPIID;
        command.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = EmpName;
        command.Parameters.Add("@EmpEmail", SqlDbType.NVarChar).Value = EmpEmail;
        command.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = EmpCode;
        command.Parameters.Add("@HODID", SqlDbType.Int).Value = HODID;
        //command.Parameters.Add("@EmpRemark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KPI_updEMPTarget", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static DataSet KPI_GETEmpTarget(string EmpEmail, string EmpCode)
    {
        SqlCommand command = new SqlCommand();

        if (EmpEmail != "")
            command.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = EmpEmail;

        if (EmpCode != "")
            command.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = EmpCode;

        return CDatabase.GetDataSet("KPI_GetEmpTarget", command);

    }

    public static DataSet KPI_GETEmpTarget(int KPIID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@KPIID", SqlDbType.Int).Value = KPIID;

        return CDatabase.GetDataSet("KPI_GetEmpTargetForReview", command);

    }

    public static int KPI_ApproveEmpTarget(int KPIID, string ApproveRemark, int HODID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@KPIID", SqlDbType.Int).Value = KPIID;
        command.Parameters.Add("@ApproveRemark", SqlDbType.NVarChar).Value = ApproveRemark;
        command.Parameters.Add("@HODID", SqlDbType.NVarChar).Value = HODID;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KPI_updApproveEMPTarget", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int KPI_ADDEmpParticular(int KPIID, string Particular, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@KPIID", SqlDbType.Int).Value = KPIID;
        command.Parameters.Add("@Particular", SqlDbType.NVarChar).Value = Particular;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KPI_insEmpParticular", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int KPI_UpdateEmpParticular(int ParticularID, string ParticularName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ParticularID", SqlDbType.Int).Value = ParticularID;
        command.Parameters.Add("@ParticularName", SqlDbType.NVarChar).Value = ParticularName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KPI_updEmpParticular", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static DataSet KPI_GETEmpParticular(int EMPId, int KPIID)
    {
        SqlCommand command = new SqlCommand();

        if (EMPId > 0)
            command.Parameters.Add("@EMPId", SqlDbType.Int).Value = EMPId;

        if (KPIID > 0)
            command.Parameters.Add("@KPIID", SqlDbType.Int).Value = KPIID;


        return CDatabase.GetDataSet("KPI_GetEmpParticular", command);
    }

    #endregion

    #region Mistake Log

    public static int AddMistakeLog(int MistakeBy, DateTime dtMistakeDate, int Amount, string MistakeRemarks, string CustomerName, int StatusId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@MistakeBy", SqlDbType.Int).Value = MistakeBy;
        command.Parameters.Add("@MistakeDate", SqlDbType.DateTime).Value = dtMistakeDate;
        command.Parameters.Add("@Amount", SqlDbType.Int).Value = Amount;
        command.Parameters.Add("@MistakeRemarks", SqlDbType.NVarChar).Value = MistakeRemarks;
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insMistakeLog", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static int UpdateMistakeLog(int MistakeId, DateTime dtMistakeDate, int Amount, string MistakeRemarks, string CustomerName, int StatusId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@MistakeId", SqlDbType.Int).Value = MistakeId;
        command.Parameters.Add("@MistakeDate", SqlDbType.DateTime).Value = dtMistakeDate;
        command.Parameters.Add("@Amount", SqlDbType.Int).Value = Amount;
        command.Parameters.Add("@MistakeRemarks", SqlDbType.NVarChar).Value = MistakeRemarks;
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updMistakeLog", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    #endregion

    #region Admin/Maintenance Expense For Branch
    public static string GetNewAdminExpenseRefNo()
    {
        string strEnqRefNo = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        strEnqRefNo = CDatabase.GetSPOutPut("IMS_GetNewRefNo", command, "@OutPut");

        return strEnqRefNo;
    }

    public static void FillBranchMaintenanceCategory(ListBox listbox)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(listbox, "IMS_GetMaintenanceCategory", command, "sName", "lId");
    }
    public static void FillBranchMaintenanceCategory(DropDownList listbox)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(listbox, "IMS_GetMaintenanceCategory", command, "sName", "lId");
    }
    public static int AddMaintenanceWorkBranch(DateTime WorkDate, int BranchID, string strWorkDesc, string strCategoryList, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@WorkDate", SqlDbType.DateTime).Value = WorkDate;
        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchID;
        command.Parameters.Add("@WorkDesc", SqlDbType.NVarChar).Value = strWorkDesc;
        command.Parameters.Add("@CategoryList", SqlDbType.NVarChar).Value = strCategoryList;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("IMS_insMaintenanceBranch", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddMaintenanceWorkBranchByRefID(int MaintenanceID, int CategoryId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@MaintenanceID", SqlDbType.Int).Value = MaintenanceID;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("IMS_insMaintenanceByID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddMaintenanceDocumentBranch(int MaintenanceID, string DocumentName, string DocumentPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@MaintenanceID", SqlDbType.Int).Value = MaintenanceID;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("IMS_insMaintenanceDocument", command, "@Output");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetMaintenanceWorkBranch(int MaintenanceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("MaintenanceId", SqlDbType.Int).Value = MaintenanceId;
        return CDatabase.GetDataSet("IMS_GetMaintenanceWorkByID", command);
    }

    public static DataSet GetWorkExpenseBranch(int MaintenanceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("MaintenanceId", SqlDbType.Int).Value = MaintenanceId;
        return CDatabase.GetDataSet("IMS_GetWorkExpenseById", command);
    }
    // Get Branch Expense History For Approval and ED Voucher Print and Exclude Expense of supplied MaintenanceId
    public static DataSet GetBranchExpepsneByDate(int MaintenanceId, int BranchID, DateTime StartDate, DateTime EndDate)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@MaintenanceId", SqlDbType.NVarChar).Value = MaintenanceId;
        cmd.Parameters.Add("@BranchID", SqlDbType.NVarChar).Value = BranchID;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDate;

        return CDatabase.GetDataSet("IMS_GetBranchExpByDateRange", cmd);
    }
    #endregion

    #region Auto Expense

    public static void FillAdditionalExpenseRule(CheckBoxList CheckBoxList)
    {
        SqlCommand command = new SqlCommand();
        CDatabase.BindControls(CheckBoxList, "TST_GetExpenseRule", command, "sName", "lid");
    }

    public static DataSet GetAdditionalExpenseRuleById(int RuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@RuleId", SqlDbType.Int).Value = RuleId;

        return CDatabase.GetDataSet("TST_GetExpenseRuleById", command);
    }

    public static int AddJobAdditionalExpense(string AdditionalIdList, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@AdditionalIdList", SqlDbType.NVarChar).Value = AdditionalIdList;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TST_insJobAdditionalExpense", command, "@OutPut");
        return Convert.ToInt32(SPresult);
        //CDatabase.ExecuteSP("insPendingDoc", command);
    }
    #endregion

    #region Transport

    public static void FillTransporterList(DropDownList dropdownlist, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        CDatabase.BindControls(dropdownlist, "TR_GetTransporterPlacedList", command, "TransporterName", "TransporterID");
    }

    public static string GetNewTransportRefNo()
    {
        string strEnqRefNo = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        strEnqRefNo = CDatabase.GetSPOutPut("TR_GetNewRefNo", command, "@OutPut");

        return strEnqRefNo;
    }

    public static void FillTransportVehicle(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "TR_GetEquipmentMS", command, "sName", "lId");
    }

    public static void FillTransportVehicleByType(DropDownList DropDown, int VehicleTypeId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lType", SqlDbType.Int).Value = VehicleTypeId;
        CDatabase.BindControls(DropDown, "TR_GetEquipmentByType", command, "sName", "lId");
    }

    public static void FillMaintenanceCategory(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "TR_GetMaintenanceCategory", command, "sName", "lId");
    }
    public static void FillMaintenanceCategory(DropDownList DropDown, int CategoryTypeId) // 1- Transport, 2- Vessel
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@TypeID", SqlDbType.Int).Value = CategoryTypeId;
        CDatabase.BindControls(DropDown, "TR_GetMaintenanceCategoryByType", command, "sName", "lId");
    }

    public static void FillMaintenanceCategory(ListBox listbox, int CategoryTypeId) // 1- Transport, 2- Vessel 
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@TypeID", SqlDbType.Int).Value = CategoryTypeId;
        CDatabase.BindControls(listbox, "TR_GetMaintenanceCategoryByType", command, "sName", "lId");
    }

    public static void FillMaintenanceCategory(ListBox listbox)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(listbox, "TR_GetMaintenanceCategory", command, "sName", "lId");
    }

    public static int AddMaintenanceWork(DateTime WorkDateStrat, DateTime WorkDateEnd, string strStartTime, string strEndTime, int VehicleID, string strWorkDesc,
        string strWorkLocation, string strCategoryList, string strEmpList, bool IsMistakeByDriver, string strMistakePersonName, string strMistakeReason, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@WorkDate", SqlDbType.DateTime).Value = WorkDateStrat;
        command.Parameters.Add("@WorkDateEnd", SqlDbType.DateTime).Value = WorkDateEnd;
        command.Parameters.Add("@StartTime", SqlDbType.NVarChar).Value = strStartTime;
        command.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = strEndTime;
        command.Parameters.Add("@VehicleID", SqlDbType.Int).Value = VehicleID;
        command.Parameters.Add("@WorkDesc", SqlDbType.NVarChar).Value = strWorkDesc;
        command.Parameters.Add("@WorkLocation", SqlDbType.NVarChar).Value = strWorkLocation;
        command.Parameters.Add("@CategoryList", SqlDbType.NVarChar).Value = strCategoryList;
        command.Parameters.Add("@EmpListID", SqlDbType.NVarChar).Value = strEmpList;
        command.Parameters.Add("@IsMistake", SqlDbType.Bit).Value = IsMistakeByDriver;
        command.Parameters.Add("@MistakePersonName", SqlDbType.NVarChar).Value = strMistakePersonName;
        command.Parameters.Add("@MistakeReason", SqlDbType.NVarChar).Value = strMistakeReason;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insMaintenanceWork", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddMaintenanceVessel(DateTime WorkDate, int VesselID, string strWorkDesc, string strWorkLocation, string strCategoryList, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@WorkDate", SqlDbType.DateTime).Value = WorkDate;
        command.Parameters.Add("@VesselID", SqlDbType.Int).Value = VesselID;
        command.Parameters.Add("@WorkDesc", SqlDbType.NVarChar).Value = strWorkDesc;
        command.Parameters.Add("@WorkLocation", SqlDbType.NVarChar).Value = strWorkLocation;
        command.Parameters.Add("@CategoryList", SqlDbType.NVarChar).Value = strCategoryList;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insMaintenanceVessel", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddMaintenanceByRefID(int MaintenanceID, int VehicleID, int CategoryId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@MaintenanceID", SqlDbType.Int).Value = MaintenanceID;
        command.Parameters.Add("@VehicleID", SqlDbType.Int).Value = VehicleID;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insMaintenanceByID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetMaintenanceWork(int MaintenanceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("MaintenanceId", SqlDbType.Int).Value = MaintenanceId;
        return CDatabase.GetDataSet("TR_GetMaintenanceWorkByID", command);
    }

    public static DataSet GetWorkExpense(int MaintenanceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("MaintenanceId", SqlDbType.Int).Value = MaintenanceId;
        return CDatabase.GetDataSet("TR_GetWorkExpenseById", command);
    }

    public static int AddMaintenanceDocument(int TransId, string DocumentName, string DocumentPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@TransId", SqlDbType.Int).Value = TransId;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_InsDocument", command, "@Output");

        return Convert.ToInt32(SPresult);
    }

    public static int AddVehicleDriver(int VehicleID, int DriverID, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@VehicleID", SqlDbType.Int).Value = VehicleID;
        command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleDriver", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static string GetNewTransportNo()
    {
        string strTRRefNo = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        strTRRefNo = CDatabase.GetSPOutPut("TR_GetNewTransRefNo", command, "@OutPut");

        return strTRRefNo;
    }

    public static int AddNewTransportRequest(string strTRRefNum, int TransMode, int TransportType, int CustomerId, int DivisionId, int PlantId, string strJobNo, string strLocFrom, string strLocTo,
        int Count20, int Count40, int NoOfPackages, decimal strGrossWeight, string strRemarks, int DeliveryType, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@TRRefNo", SqlDbType.NVarChar).Value = strTRRefNum;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = TransMode;
        command.Parameters.Add("@lType", SqlDbType.Int).Value = TransportType;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@JobNo", SqlDbType.NVarChar).Value = strJobNo;
        command.Parameters.Add("@LocFrom", SqlDbType.NVarChar).Value = strLocFrom;
        command.Parameters.Add("@LocTo", SqlDbType.NVarChar).Value = strLocTo;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@NoOfPkgs", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWeight", SqlDbType.Decimal).Value = strGrossWeight;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemarks;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_NewTransportRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddJobTransportRequest(int JobId, int CustomerId, string strJobRefNum, DateTime RequestDate, int TransportType, string strLocFrom, string strLocTo,
            int Count20, int Count40, int NoOfPackages, string strGrossWeight, string strRemarks, int JobType, int DeliveryType, int ExportType, string Dimension,
            DateTime VehiclePlaceRequireDate, int VehicleRequired, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = strJobRefNum;
        command.Parameters.Add("@RequestDate", SqlDbType.DateTime).Value = RequestDate;
        command.Parameters.Add("@lType", SqlDbType.Int).Value = TransportType;
        command.Parameters.Add("@LocFrom", SqlDbType.NVarChar).Value = strLocFrom;
        command.Parameters.Add("@LocTo", SqlDbType.NVarChar).Value = strLocTo;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@NoOfPkgs", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = strGrossWeight;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemarks;
        command.Parameters.Add("@JobType", SqlDbType.Int).Value = JobType;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@ExportType", SqlDbType.Int).Value = ExportType;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@VehiclePlaceRequireDate", SqlDbType.Date).Value = VehiclePlaceRequireDate;
        command.Parameters.Add("@VehicleRequired", SqlDbType.Int).Value = VehicleRequired;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_JobTransportRequest", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddTransporterPlaced(int TransReqID, int TransporterId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRequestId", SqlDbType.NVarChar).Value = TransReqID;
        command.Parameters.Add("@TransporterID", SqlDbType.NVarChar).Value = TransporterId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insTransporterPlaced", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int RemoveTransporterPlaced(int TransReqID, int TransporterId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRequestId", SqlDbType.NVarChar).Value = TransReqID;
        command.Parameters.Add("@TransporterID", SqlDbType.NVarChar).Value = TransporterId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_delTransporterPlaced", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddVehiclePlaced(string TransportIdList, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransportIdList", SqlDbType.NVarChar).Value = TransportIdList;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updVehiclePlaced", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddVehiclePlacedDetail(int TransReqId, string VehicleNo, int VehicleType, int Packages, int Con20, int Con40,
       string TransporterName, int TransporterID, string DeliveryFrom, string DeliveryTo, DateTime DispatchDate, DateTime DeliveryDate, int UserID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRequestId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        command.Parameters.Add("@Packages", SqlDbType.Int).Value = Packages;
        command.Parameters.Add("@Con20", SqlDbType.Int).Value = Con20;
        command.Parameters.Add("@Con40", SqlDbType.Int).Value = Con40;
        command.Parameters.Add("@Transporter", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@DeliveryFrom", SqlDbType.NVarChar).Value = DeliveryFrom;
        command.Parameters.Add("@DeliveryTo", SqlDbType.NVarChar).Value = DeliveryTo;

        if (DispatchDate != DateTime.MinValue)
            command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = DispatchDate;
        if (DeliveryDate != DateTime.MinValue)
            command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = DeliveryDate;

        command.Parameters.Add("lUser", SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddTransportRate(int TransRateId, int TransRequestId, string VehicleNo, int Amount, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRateID", SqlDbType.Int).Value = TransRateId;
        command.Parameters.Add("@TransRequestId", SqlDbType.Int).Value = TransRequestId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = Amount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insTransportRate", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddApprovareTransRate(int ApproveId, int Amount, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ApproveId", SqlDbType.NVarChar).Value = ApproveId;
        command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = Amount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updApproveTransRate", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddTransMovement(int RateId, DateTime ReportingDate, DateTime dtUnLoadingDate, DateTime dtContReturnDate, bool MovementCompleted, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@RateId", SqlDbType.NVarChar).Value = RateId;
        if (ReportingDate != DateTime.MinValue)
            command.Parameters.Add("@ReportingDate", SqlDbType.DateTime).Value = ReportingDate;
        if (dtUnLoadingDate != DateTime.MinValue)
            command.Parameters.Add("@UnLoadingDate", SqlDbType.DateTime).Value = dtUnLoadingDate;
        if (dtContReturnDate != DateTime.MinValue)
            command.Parameters.Add("@ContReturnDate", SqlDbType.DateTime).Value = dtContReturnDate;
        command.Parameters.Add("@MovementCompleted", SqlDbType.Bit).Value = MovementCompleted;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insTransMovement", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddTransBillDetail(int TransReqId, int TransporterID, int TransitDays, DateTime BillSubmitDate, string BillNumber, DateTime BillDate,
        string BillAmount, string DetentionAmount, string VaraiAmount, string EmptyContRcptCharges, string TollCharges, string OtherCharges, string TotalAmount,
        string BillPersonName, bool IsValid, string Justification, bool IsConsolidated, int ConsolidateID, string DocName, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.NVarChar).Value = TransReqId;
        command.Parameters.Add("@TransporterID", SqlDbType.NVarChar).Value = TransporterID;
        command.Parameters.Add("@TransitDays", SqlDbType.NVarChar).Value = TransitDays;
        if (BillSubmitDate != DateTime.MinValue)
            command.Parameters.Add("@BillSubmitDate", SqlDbType.DateTime).Value = BillSubmitDate;
        command.Parameters.Add("@BillNumber", SqlDbType.NVarChar).Value = BillNumber;
        if (BillDate != DateTime.MinValue)
            command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = BillDate;

        if (BillAmount != "")
            command.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = BillAmount;
        if (DetentionAmount != "")
            command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        if (VaraiAmount != "")
            command.Parameters.Add("@VaraiAmount", SqlDbType.Decimal).Value = VaraiAmount;
        if (EmptyContRcptCharges != "")
            command.Parameters.Add("@EmptyContRcptCharges", SqlDbType.Decimal).Value = EmptyContRcptCharges;
        if (TollCharges != "")
            command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        if (OtherCharges != "")
            command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        if (TotalAmount != "")
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;

        command.Parameters.Add("@BillPersonName", SqlDbType.NVarChar).Value = BillPersonName;
        command.Parameters.Add("@Justification", SqlDbType.NVarChar).Value = Justification;
        command.Parameters.Add("@IsValid", SqlDbType.Bit).Value = IsValid;
        command.Parameters.Add("@IsConsolidated", SqlDbType.Bit).Value = IsConsolidated;
        command.Parameters.Add("@ConsolidateID", SqlDbType.Int).Value = ConsolidateID;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insTransBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddVehicleDailyStatus(int VehicleId, int StatusID, DateTime StatusDate, int DriverID, bool IsDriverPresent, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VehicleID", SqlDbType.NVarChar).Value = VehicleId;
        command.Parameters.Add("@StatusID", SqlDbType.NVarChar).Value = StatusID;
        command.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = StatusDate;
        command.Parameters.Add("@DriverID", SqlDbType.NVarChar).Value = DriverID;
        command.Parameters.Add("@IsDriverPresent", SqlDbType.Bit).Value = IsDriverPresent;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleDailyStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddVehicleDailyExpense(int VehicleId, DateTime ExpenseDate, Decimal decFuel, Decimal decFuel2, Decimal decFuelLiter, Decimal decFuel2Liter,
        Decimal decTollCharges, Decimal decFineWithoutCleaner, Decimal decXerox, Decimal decVaraiUnloading, Decimal decEmptyContainer, Decimal decParking,
        Decimal decGarage, Decimal decBhatta, Decimal decODCOverweight, Decimal decOtherCharges, Decimal decDamageContainer, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@VehicleID", SqlDbType.NVarChar).Value = VehicleId;
        command.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value = ExpenseDate;

        if (decFuel != 0)
            command.Parameters.Add("@FuelCharges", SqlDbType.Decimal).Value = decFuel;
        if (decFuel2 != 0)
            command.Parameters.Add("@FuelCharges2", SqlDbType.Decimal).Value = decFuel2;
        if (decFuelLiter != 0)
            command.Parameters.Add("@FuelChargesLiter", SqlDbType.Decimal).Value = decFuelLiter;
        if (decFuel2Liter != 0)
            command.Parameters.Add("@FuelCharges2Liter", SqlDbType.Decimal).Value = decFuel2Liter;
        if (decTollCharges != 0)
            command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = decTollCharges;
        if (decFineWithoutCleaner != 0)
            command.Parameters.Add("@FineCleaner", SqlDbType.Decimal).Value = decFineWithoutCleaner;
        if (decXerox != 0)
            command.Parameters.Add("@Xerox", SqlDbType.Decimal).Value = decXerox;
        if (decVaraiUnloading != 0)
            command.Parameters.Add("@VaraiUnloading", SqlDbType.Decimal).Value = decVaraiUnloading;
        if (decEmptyContainer != 0)
            command.Parameters.Add("@EmptyContainer", SqlDbType.Decimal).Value = decEmptyContainer;
        if (decParking != 0)
            command.Parameters.Add("@Parking", SqlDbType.Decimal).Value = decParking;
        if (decGarage != 0)
            command.Parameters.Add("@Garage", SqlDbType.Decimal).Value = decGarage;
        if (decBhatta != 0)
            command.Parameters.Add("@Bhatta", SqlDbType.Decimal).Value = decBhatta;
        if (decODCOverweight != 0)
            command.Parameters.Add("@ODCOverweight", SqlDbType.Decimal).Value = decODCOverweight;
        if (decOtherCharges != 0)
            command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = decOtherCharges;
        if (decDamageContainer != 0)
            command.Parameters.Add("@DamageContainer", SqlDbType.Decimal).Value = decDamageContainer;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleDailyExp", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static DataSet GetVehicleDailyStatus(string VehicleNo, DateTime StatusDate)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        cmd.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = StatusDate;

        return CDatabase.GetDataSet("TR_GetVehicleStatusByDateNo", cmd);
    }

    public static DataSet GetVehicleStatusByDate(int VehicleID, DateTime StartDate, DateTime EndDate)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@VehicleID", SqlDbType.NVarChar).Value = VehicleID;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDate;

        return CDatabase.GetDataSet("TR_GetVehicleStatusByDateRange", cmd);
    }

    // Get Vehicle Expense History For Approval Od ED Voucher Print and Exclude Expense of supplied MaintenanceId
    public static DataSet GetVehicleExpepsneByDate(int MaintenanceId, int VehicleID, DateTime StartDate, DateTime EndDate)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@MaintenanceId", SqlDbType.NVarChar).Value = MaintenanceId;
        cmd.Parameters.Add("@VehicleID", SqlDbType.NVarChar).Value = VehicleID;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDate;

        return CDatabase.GetDataSet("TR_GetVehicleExpByDateRange", cmd);
    }

    public static DataView GetTransportRequestDetail(int TransportId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@TransportId", SqlDbType.Int).Value = TransportId;

        return CDatabase.GetDataView("TR_GetTruckRequestById", cmd);
    }

    public static void FillTransporter(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid,sName FROM TR_TransportVendor WHERE bDel=0 ORDER BY sName", "sName", "lid");
    }

    public static void FillTransporter(ListBox DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid,sName FROM TR_TransportVendor WHERE bDel=0 ORDER BY sName", "sName", "lid");
    }

    public static void FillTransporterPlaced(DropDownList DropDown, int JobId, int TransReqID)
    {
        SqlCommand cmd = new SqlCommand();

        if (JobId > 0)
            cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        else if (TransReqID > 0)
            cmd.Parameters.Add("@TransRequestId", SqlDbType.Int).Value = TransReqID;

        CDatabase.BindControls(DropDown, "TR_GetTransporterPlaced", cmd, "TransporterName", "TransporterID");
    }

    public static DataView GetVehicleRateDetailByLid(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataView("TR_GetTransDeliveryDetail_lid", cmd);
    }

    public static int AddVehicleRateExpense(int lid, string VaraiAmount, string DetentionAmount, string EmptyContRcptCharges)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;
        if (VaraiAmount != "")
            command.Parameters.Add("@VaraiAmount", SqlDbType.Decimal).Value = VaraiAmount;
        if (DetentionAmount != "")
            command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        if (EmptyContRcptCharges != "")
            command.Parameters.Add("@EmptyContRcptCharges", SqlDbType.Decimal).Value = EmptyContRcptCharges;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleRateExpenses", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    /********************* 01-11-2018  ****************************************************************/

    public static int UpdateTransBillDetail(int TransReqId, int TransporterID, string BillAmount, string DetentionAmount, string VaraiAmount,
    string EmptyContRcptCharges, string TollCharges, string OtherCharges, string TotalAmount, bool IsValid, string Justification, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.NVarChar).Value = TransReqId;
        command.Parameters.Add("@TransporterID", SqlDbType.NVarChar).Value = TransporterID;
        if (BillAmount != "")
            command.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = BillAmount;
        if (DetentionAmount != "")
            command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        if (VaraiAmount != "")
            command.Parameters.Add("@VaraiAmount", SqlDbType.Decimal).Value = VaraiAmount;
        if (EmptyContRcptCharges != "")
            command.Parameters.Add("@EmptyContRcptCharges", SqlDbType.Decimal).Value = EmptyContRcptCharges;
        if (TollCharges != "")
            command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        if (OtherCharges != "")
            command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        if (TotalAmount != "")
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;
        command.Parameters.Add("@Justification", SqlDbType.NVarChar).Value = Justification;
        command.Parameters.Add("@IsValid", SqlDbType.Bit).Value = IsValid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updTransBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static string GetConsolidateRefNo(int FinYearId)
    {
        string strEnqRefNo = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
        strEnqRefNo = CDatabase.GetSPOutPut("TR_GetConsolidateRefNo", command, "@OutPut");
        return strEnqRefNo;
    }

    public static int UpdateConsolidateJob(int TransReqId, int ConsolidateID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@ConsolidateID", SqlDbType.Int).Value = ConsolidateID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updConsolidateJob", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddConsolidateRequest(string TransRefNo, int TransporterId, int FinYear, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRefNo", SqlDbType.NVarChar).Value = TransRefNo;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insConsolidateRequest", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddConsolidateJobDetail(int ConsolidateId, int TransReqId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ConsolidateId", SqlDbType.Int).Value = ConsolidateId;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insConsolidateJobDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView GetTransportJobDetailByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("TR_GetJobDetailByJobId", cmd);
    }

    public static int AddTransportRateDetail(bool IsConsolidate, int TransReqId, int VehicleId, int TransporterId, int VehicleType, string VehicleNo, string MemoAttachment, string City, decimal MarketBillingRate,
        string LRNo, string ChallanNo, decimal Rate, decimal Advance, decimal AdvanceAmount, decimal FreightAmount, decimal DetentionAmount, decimal VaraiExpense,
        decimal EmptyContRecptCharges, decimal TollCharges, decimal OtherCharges, DateTime LRDate, DateTime ChallanDate, int DispatchedCount,
        decimal ContractPrice, decimal SellingPrice, string EmailAttacment, string ContractAttachment, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@IsConsolidate", SqlDbType.Bit).Value = IsConsolidate;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        if (VehicleId > 0)
            command.Parameters.Add("@VehicleId", SqlDbType.Int).Value = VehicleId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        if (MemoAttachment != "")
            command.Parameters.Add("@MemoAttachment", SqlDbType.NVarChar).Value = MemoAttachment;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@MarketBillingRate", SqlDbType.Decimal).Value = MarketBillingRate;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;
        command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Rate;
        command.Parameters.Add("@Advance", SqlDbType.Decimal).Value = Advance;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;
        command.Parameters.Add("@FreightAmount", SqlDbType.Decimal).Value = FreightAmount;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiExpense", SqlDbType.Decimal).Value = VaraiExpense;
        command.Parameters.Add("@EmptyContRecptCharges", SqlDbType.Decimal).Value = EmptyContRecptCharges;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        if (LRDate != DateTime.MinValue)
            command.Parameters.Add("@LRDate", SqlDbType.Date).Value = LRDate;
        if (ChallanDate != DateTime.MinValue)
            command.Parameters.Add("@ChallanDate", SqlDbType.Date).Value = ChallanDate;
        command.Parameters.Add("@DispatchedCount", SqlDbType.Int).Value = DispatchedCount;
        command.Parameters.Add("@ContractPrice", SqlDbType.Decimal).Value = ContractPrice;
        command.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = SellingPrice;
        command.Parameters.Add("@EmailAttachment", SqlDbType.NVarChar).Value = EmailAttacment;
        command.Parameters.Add("@ContractAttachment", SqlDbType.NVarChar).Value = ContractAttachment;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insTransRateDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTransportRateDetail(int TransReqId, int TransporterId, int VehicleType, int VehicleId, string VehicleNo, string MemoAttachment, string City, decimal MarketBillingRate,
        string LRNo, string ChallanNo, decimal Rate, decimal Advance, decimal AdvanceAmount, decimal FreightAmount, decimal DetentionAmount, decimal VaraiExpense,
        decimal EmptyContRecptCharges, decimal TollCharges, decimal OtherCharges, DateTime LRDate, DateTime ChallanDate,
        decimal ContractPrice, decimal SellingPrice, string EmailAttacment, string ContractAttachment, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        if (VehicleId > 0)
            command.Parameters.Add("@VehicleId", SqlDbType.Int).Value = VehicleId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        if (MemoAttachment != "")
            command.Parameters.Add("@MemoAttachment", SqlDbType.NVarChar).Value = MemoAttachment;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@MarketBillingRate", SqlDbType.Decimal).Value = MarketBillingRate;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;
        command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Rate;
        command.Parameters.Add("@Advance", SqlDbType.Decimal).Value = Advance;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;
        command.Parameters.Add("@FreightAmount", SqlDbType.Decimal).Value = FreightAmount;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiExpense", SqlDbType.Decimal).Value = VaraiExpense;
        command.Parameters.Add("@EmptyContRecptCharges", SqlDbType.Decimal).Value = EmptyContRecptCharges;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        if (LRDate != DateTime.MinValue)
            command.Parameters.Add("@LRDate", SqlDbType.Date).Value = LRDate;
        if (ChallanDate != DateTime.MinValue)
            command.Parameters.Add("@ChallanDate", SqlDbType.Date).Value = ChallanDate;
        command.Parameters.Add("@ContractPrice", SqlDbType.Decimal).Value = ContractPrice;
        command.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = SellingPrice;
        command.Parameters.Add("@EmailAttachment", SqlDbType.NVarChar).Value = EmailAttacment;
        command.Parameters.Add("@ContractAttachment", SqlDbType.NVarChar).Value = ContractAttachment;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updTransRateDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTransportRateDetailForConsolidateJob(int TransReqId, int TransporterId, int VehicleType, int VehicleId, string VehicleNo, string MemoAttachment, string City, decimal MarketBillingRate,
        string LRNo, string ChallanNo, decimal Rate, decimal Advance, decimal AdvanceAmount, decimal FreightAmount, decimal DetentionAmount, decimal VaraiExpense,
        decimal EmptyContRecptCharges, decimal TollCharges, decimal OtherCharges, DateTime LRDate, DateTime ChallanDate,
        decimal ContractPrice, decimal SellingPrice, string EmailAttacment, string ContractAttachment, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        if (VehicleId > 0)
            command.Parameters.Add("@VehicleId", SqlDbType.Int).Value = VehicleId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        if (MemoAttachment != "")
            command.Parameters.Add("@MemoAttachment", SqlDbType.NVarChar).Value = MemoAttachment;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@MarketBillingRate", SqlDbType.Decimal).Value = MarketBillingRate;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = ChallanNo;
        command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Rate;
        command.Parameters.Add("@Advance", SqlDbType.Decimal).Value = Advance;
        command.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = AdvanceAmount;
        command.Parameters.Add("@FreightAmount", SqlDbType.Decimal).Value = FreightAmount;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiExpense", SqlDbType.Decimal).Value = VaraiExpense;
        command.Parameters.Add("@EmptyContRecptCharges", SqlDbType.Decimal).Value = EmptyContRecptCharges;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        if (LRDate != DateTime.MinValue)
            command.Parameters.Add("@LRDate", SqlDbType.Date).Value = LRDate;
        if (ChallanDate != DateTime.MinValue)
            command.Parameters.Add("@ChallanDate", SqlDbType.Date).Value = ChallanDate;
        command.Parameters.Add("@ContractPrice", SqlDbType.Decimal).Value = ContractPrice;
        command.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = SellingPrice;
        command.Parameters.Add("@EmailAttachment", SqlDbType.NVarChar).Value = EmailAttacment;
        command.Parameters.Add("@ContractAttachment", SqlDbType.NVarChar).Value = ContractAttachment;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updTransRateDetailForConsoleJob", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTransRateDetail(int TransReqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        return CDatabase.GetDataSet("TR_GetTransRateDetail", cmd);
    }

    public static DataSet GetTransRateDetailForRequest(int TransReqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        return CDatabase.GetDataSet("TR_GetTransRateDetailForRequest", cmd);
    }

    public static DataView GetTransRateDetailById(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataView("TR_GetTransRateDetailById", cmd);
    }

    public static int UpdateTransportFundId(string RateDetailId, int FundRequestId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@RateDetailId", SqlDbType.NVarChar).Value = RateDetailId;
        command.Parameters.Add("@FundRequestId", SqlDbType.Int).Value = FundRequestId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updTransRateFundId", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteTransportRateId(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_delTransRateDetailId", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillVehicleDetail(int TransReqID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@TransReqID", SqlDbType.Int).Value = TransReqID;
        return CDatabase.GetDataSet("TR_TestBillVehicleDetail", cmd);
    }

    public static DataSet GetTransBillDetail(int TransReqID, int TransporterID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@TransReqID", SqlDbType.Int).Value = TransReqID;
        cmd.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        return CDatabase.GetDataSet("TR_GetTransBillDetail", cmd);
    }

    public static int AddTransApproveRejectBill(int lid, int IsApproved, decimal ApprovedAmount, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@IsApproved", SqlDbType.Int).Value = IsApproved;
        if (ApprovedAmount > 0)
            command.Parameters.Add("@ApprovedAmount", SqlDbType.Decimal).Value = ApprovedAmount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insApproveRejectBill", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddTransBillApprovalHistory(int lid, int IsApproved, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransBillId", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@IsApproved", SqlDbType.Int).Value = IsApproved;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insBillApprovalHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddBillReceivedDetail(int TransBillId, int SentUser, int ReceivedBy, DateTime SentDate, DateTime ReceivedDate, int StatusId,
                                                string ChequeNo, DateTime ChequeDate, string HoldReason, DateTime ReleaseDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransBillId", SqlDbType.Int).Value = TransBillId;
        if (SentUser > 0)
            command.Parameters.Add("@SentUser", SqlDbType.Int).Value = SentUser;
        if (SentDate != DateTime.MinValue)
            command.Parameters.Add("@SentDate", SqlDbType.DateTime).Value = SentDate;
        if (ReceivedBy > 0)
            command.Parameters.Add("@ReceivedBy", SqlDbType.Int).Value = ReceivedBy;
        if (ReceivedDate != DateTime.MinValue)
            command.Parameters.Add("@ReceivedDate", SqlDbType.DateTime).Value = ReceivedDate;
        if (StatusId > 0)
            command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        if (ChequeNo != "")
            command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        if (ChequeDate != DateTime.MinValue)
            command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = ChequeDate;
        if (HoldReason != "")
            command.Parameters.Add("@HoldReason", SqlDbType.DateTime).Value = HoldReason;
        if (ReleaseDate != DateTime.MinValue)
            command.Parameters.Add("@ReleaseDate", SqlDbType.DateTime).Value = ReleaseDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insBillReceivedDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView GetTransBillDetailById(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataView("TR_GetTransBillDetailById", cmd);
    }

    public static int UpdateBillReceivedDetail(int lid, int StatusId, string ChequeNo, DateTime ChequeDate, string HoldReason, DateTime ReleaseDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
        if (ChequeDate != DateTime.MinValue)
            command.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = ChequeDate;
        if (HoldReason != "")
            command.Parameters.Add("@HoldReason", SqlDbType.NVarChar).Value = HoldReason;
        if (ReleaseDate != DateTime.MinValue)
            command.Parameters.Add("@ReleaseDate", SqlDbType.DateTime).Value = ReleaseDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updBillReceivedDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillReceivedDetail(int TransBillId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransBillId", SqlDbType.Int).Value = TransBillId;
        return CDatabase.GetDataSet("TR_GetBillReceivedDetail", cmd);
    }

    public static int AddTransporterBankDetails(int TransporterId, string BankName, string AccountNo, string IFSCCode, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        command.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = AccountNo;
        command.Parameters.Add("@IFSCCode", SqlDbType.NVarChar).Value = IFSCCode;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insTransporterBankDetails", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataView GetTransporterBankDetails(int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataView("BS_GetTransporterBankDetails", cmd);
    }

    public static int AddPackingListDocs(int TransReqId, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insPackingListDocs", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetPackingListDocs(int TransReqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        return CDatabase.GetDataSet("TR_GetPackingListDocs", cmd);
    }

    public static int UpdateRateDeliveryStatus(int RateId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;


        SPresult = CDatabase.GetSPOutPut("TR_updRateDeliveryStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTransRateDetailByTP(int TransReqId, int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        cmd.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataSet("TR_GetTransRateDetailByTP", cmd);
    }

    public static DataView GetConsolidateRequestById(int ConsolidateReqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = ConsolidateReqId;
        return CDatabase.GetDataView("TR_GetConsolidateRequestById", cmd);
    }

    public static DataView TP_GetVehicleDetail(int JobId, string VehicleNo, int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        cmd.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataView("TR_GetVehicleDetail", cmd);
    }

    public static int TR_UpdateVehicleDetail(int lid, int ConsolidateId, int JobId, string VehicleNo, string LRNo, string BabajiChallanNo, DateTime LRDate, DateTime BabajiChallanDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@ConsolidateId", SqlDbType.Int).Value = ConsolidateId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@BabajiChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        if (LRDate != DateTime.MinValue)
            command.Parameters.Add("@LRDate", SqlDbType.DateTime).Value = LRDate;
        if (BabajiChallanDate != DateTime.MinValue)
            command.Parameters.Add("@BabajiChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_updVehicleDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetConsolidateJobDetail(int TransReqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        return CDatabase.GetDataSet("TR_GetConsolidateJobDetail", cmd);
    }
    public static DataSet GetTransConsolidateJobDetail(int ConsolidateID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = ConsolidateID;
        return CDatabase.GetDataSet("TR_GetTransConsolidateJobDetail", cmd);
    }
    public static int AddExpenseConsolidateJobs(int PaymentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("AC_insConsolidateJobs", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_UpdateVehicleNo(int TransRateId, int ConsolidateID, string VehicleNo, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRateId", SqlDbType.Int).Value = TransRateId;
        command.Parameters.Add("@ConsolidateID", SqlDbType.Int).Value = ConsolidateID;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_updVehicleNo", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddDailyStatusHistory(int TransReqId, string CurrentStatus, string EmailTo, string EmailCC, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@CurrentStatus", SqlDbType.NVarChar).Value = CurrentStatus;
        command.Parameters.Add("@EmailTo", SqlDbType.NVarChar).Value = EmailTo;
        command.Parameters.Add("@EmailCC", SqlDbType.NVarChar).Value = EmailCC;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insDailyStatusHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_MailCustomerDelivery(int CustID, int JobId, string VehicleNo, string LRNo, string BabajiChallanNo, DateTime LRDate, DateTime BabajiChallanDate)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CustID", SqlDbType.Int).Value = CustID;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        command.Parameters.Add("@LRDate", SqlDbType.DateTime).Value = LRDate;
        command.Parameters.Add("@ChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_mailCustomerDelivery", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTruckRequestByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("TR_GetTruckRequestByJobId", cmd);
    }

    public static int DeleteTranportMS(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_delTranportMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteConsolidateTranportMS(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_delTransConsolidateMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_AddVehicleRateExpense(int RateId, decimal Fuel, decimal FuelLiter, decimal Fuel2, decimal Fuel2Liter, decimal TollCharges,
                    decimal FineWithoutCleaner, decimal Xerox, decimal VaraiUnloading, decimal EmptyContainerReceipt, decimal ParkingGatePass, decimal Garage,
                    decimal Bhatta, decimal OtherCharges, decimal AdditionalChargesForODCOverweight, decimal NakaPassingDamageContainer, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@Fuel", SqlDbType.Decimal).Value = Fuel;
        command.Parameters.Add("@FuelLiter", SqlDbType.Decimal).Value = FuelLiter;
        command.Parameters.Add("@Fuel2", SqlDbType.Decimal).Value = Fuel2;
        command.Parameters.Add("@Fuel2Liter", SqlDbType.Decimal).Value = Fuel2Liter;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@FineWithoutCleaner", SqlDbType.Decimal).Value = FineWithoutCleaner;
        command.Parameters.Add("@Xerox", SqlDbType.Decimal).Value = Xerox;
        command.Parameters.Add("@VaraiUnloading", SqlDbType.Decimal).Value = VaraiUnloading;
        command.Parameters.Add("@EmptyContainerReceipt", SqlDbType.Decimal).Value = EmptyContainerReceipt;
        command.Parameters.Add("@ParkingGatePass", SqlDbType.Decimal).Value = ParkingGatePass;
        command.Parameters.Add("@Garage", SqlDbType.Decimal).Value = Garage;
        command.Parameters.Add("@Bhatta", SqlDbType.Decimal).Value = Bhatta;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        command.Parameters.Add("@AdditionalChargesForODCOverweight", SqlDbType.Decimal).Value = AdditionalChargesForODCOverweight;
        command.Parameters.Add("@NakaPassingDamageContainer", SqlDbType.Decimal).Value = NakaPassingDamageContainer;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insVehicleRateExpense", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_DeleteVehicleRateExpense(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_delVehicleRateExpense", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView TR_GetVehicleRateExpenseById(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataView("TR_GetVehicleRateExpenseById", cmd);
    }

    public static int AddSellingRateDetail(int TransReqId, int RateId, decimal MarketBillingRate, decimal FreightRate, decimal DetentionAmount,
                        decimal VaraiExpense, decimal EmptyContRecptCharges, decimal TollCharges, decimal OtherCharges, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@MarketBillingRate", SqlDbType.Decimal).Value = MarketBillingRate;
        command.Parameters.Add("@FreightRate", SqlDbType.Decimal).Value = FreightRate;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiExpense", SqlDbType.Decimal).Value = VaraiExpense;
        command.Parameters.Add("@EmptyContRecptCharges", SqlDbType.Decimal).Value = EmptyContRecptCharges;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleSellingDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_AddBillingInstructions(int TransReqId, int RateId, int TransporterId, string Instruction, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@Instruction", SqlDbType.NVarChar).Value = Instruction;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insBillingInstructions", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_AddConsolidateInstructions(int TransReqId, int TransporterId, string Instruction, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        command.Parameters.Add("@Instruction", SqlDbType.NVarChar).Value = Instruction;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insConsolidateInstructions", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView TR_GetConsolidateInstructions(int TransReqId, int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        cmd.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataView("TR_GetConsolidateInstructions", cmd);
    }

    public static DataView TR_GetBillingInstructions(int TransReqId, int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        cmd.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataView("TR_GetBillingInstructions", cmd);
    }

    public static int AddMemoPriority(int TransBillId, int Priority, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransBillId", SqlDbType.Int).Value = TransBillId;
        command.Parameters.Add("@Priority", SqlDbType.Int).Value = Priority;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updMemoPriority", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddContainerDetailTR(int JobId, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insContainerDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateContainerDetailTR(int JobId, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updContainerDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteContainerDetailTR(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_delContainerDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static void TR_GetPendingContainerDetail(DropDownList DropDown, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        CDatabase.BindControls(DropDown, "TR_GetPendingContainerDetail", command, "ContainerNo", "lId");
    }

    public static DataView TR_GetJobDetailForDelivery(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("TR_GetJobDetailForDelivery", cmd);
    }

    public static void FillVehicleForDelivery(DropDownList DropDown, int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        CDatabase.BindControls(DropDown, "TR_GetVehicleForPendingDelivery", command, "VehicleNo", "lid");
    }

    public static int TR_AddDeliveryDetail(int JobId, int ContainerId, int NoOfPackages, string VehicleNo, int VehicleType,
        DateTime VehicleRcvdDate, string TransporterName, int TransporterID, string LRNo, DateTime LRDate, string DeliveryPoint, DateTime DispatchDate,
        DateTime DeliveryDate, DateTime EmptyContRetrunDate, string CargoReceivedBy, string BabajiChallanNo, DateTime BabajiChallanDate, string ChallanPath,
        string DamageCopyPath, string PODPath, string DriverName, string DriverPhone, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        if (VehicleRcvdDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VehicleRcvdDate", SqlDbType.DateTime).Value = VehicleRcvdDate;
        }
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        if (LRDate != DateTime.MinValue)
        {
            command.Parameters.Add("@LRDate", SqlDbType.DateTime).Value = LRDate;
        }
        command.Parameters.Add("@DeliveryPoint", SqlDbType.NVarChar).Value = DeliveryPoint;
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
        command.Parameters.Add("@CargoReceivedBy", SqlDbType.NVarChar).Value = CargoReceivedBy;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        if (BabajiChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;
        }
        command.Parameters.Add("@ChallanPath", SqlDbType.NVarChar).Value = ChallanPath;
        command.Parameters.Add("@DamageCopyPath", SqlDbType.NVarChar).Value = DamageCopyPath;
        command.Parameters.Add("@PODAttachmentPath", SqlDbType.NVarChar).Value = PODPath;
        command.Parameters.Add("@DriverName", SqlDbType.NVarChar).Value = DriverName;
        command.Parameters.Add("@DriverPhone", SqlDbType.NVarChar).Value = DriverPhone;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insDeliveryDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_DeleteDeliveryDetail(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_delDeliveryDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static string TR_GetNewJobRefNo(int FinYearId)
    {
        string strTRRefNo = "";
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
        strTRRefNo = CDatabase.GetSPOutPut("TR_GetNewTransportJobNo", command, "@OutPut");
        return strTRRefNo;
    }

    public static int TR_MoveToBackOffice(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insMoveToBackOffice", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_MoveToScrutiny(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insMoveToScrutiny", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet TR_GetPCDDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("TR_GetPCDDetail", command);
    }

    public static DataSet GetBillVehicleDetailByTP(int TransReqID, int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqID", SqlDbType.Int).Value = TransReqID;
        cmd.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataSet("TR_TestBillVehicleDetailByTP", cmd);
    }

    public static int TR_MoveDeliveryToWarehouse(int JobId, int WarehouseId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = WarehouseId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insMoveDeliveryToWarehouse", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_UpdateTransporterPlaced(int TransRequestId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransRequestId", SqlDbType.Int).Value = TransRequestId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updTransporterPlaced", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_updJobTransportBabaji(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updJobTransportBabaji", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Trans EWay Bill

    public static DataSet EWAYGetAuthTokan(string UserGSTIN)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserGSTIN", SqlDbType.NVarChar).Value = UserGSTIN;

        return CDatabase.GetDataSet("TR_GetEWayAuthToken", command);

    }

    public static int EWAYUpdateAuthTokan(string UserGSTIN, string TokenNo, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@UserGSTIN", SqlDbType.NVarChar).Value = UserGSTIN;
        command.Parameters.Add("@AuthTokan", SqlDbType.NVarChar).Value = TokenNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updEWayBillToken", command, "@OutPut");
        return Convert.ToInt32(SPresult);


    }

    public static int EWAYAddLog(string APIAction, string AppUserName, string ErrCode, bool IsSuccess,
        string OutcomeMsg, string TxnDateTime, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@APIAction", SqlDbType.NVarChar).Value = APIAction;
        command.Parameters.Add("@AppUserName", SqlDbType.NVarChar).Value = AppUserName;
        command.Parameters.Add("@ErrCode", SqlDbType.NVarChar).Value = ErrCode;
        command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = IsSuccess;
        command.Parameters.Add("@OutcomeMsg", SqlDbType.NVarChar).Value = OutcomeMsg;
        command.Parameters.Add("@TxnDateTime", SqlDbType.NVarChar).Value = TxnDateTime;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insEWayLog", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }
    public static void FillTransSubType(DropDownList DropDown, int TransactionType)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@TransactionType", SqlDbType.Int).Value = TransactionType;
        CDatabase.BindControls(DropDown, "TR_GetEwaySubType", command, "sName", "sValue");
    }
    public static DataSet GetEwayBillByJobID(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("TR_GetEWayByJobId", command);
    }
    public static DataSet GetTransportEwayBillByJobID(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("TR_GetEwayBillByJobId", command);
    }
    public static string GetEwayAPIGstinByBillNo(Int64 EWayBillNo)
    {
        SqlCommand command = new SqlCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "TR_GetEWayAPIGstinByBillNo";

        command.Parameters.Add("@EWayBillNo", SqlDbType.BigInt).Value = EWayBillNo;

        return CDatabase.ResultInString(command);
    }
    public static int AddEWayBill(int TransType, int TansSubType, int JobID, string RefNo, string EWayBillNo, string DocumentType, string DocumentNo,
        DateTime DocumentDate, string VehicleNo, string VehicleDate, string BillGenDate, string ValidityDate, string UserGSTIN, string strAPI_GSTIN, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@TransTypeId", SqlDbType.Int).Value = TransType;
        command.Parameters.Add("@TansSubTypeId", SqlDbType.Int).Value = TansSubType;

        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        command.Parameters.Add("@RefNo", SqlDbType.NVarChar).Value = RefNo;
        command.Parameters.Add("@EWayBillNo", SqlDbType.NVarChar).Value = EWayBillNo;
        command.Parameters.Add("@DocumentType", SqlDbType.NVarChar).Value = DocumentType;
        command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = DocumentNo;

        if (DocumentDate != DateTime.MinValue)
            command.Parameters.Add("@DocumentDate", SqlDbType.DateTime).Value = DocumentDate;

        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleDate", SqlDbType.NVarChar).Value = VehicleDate;
        command.Parameters.Add("@BillGenDate", SqlDbType.NVarChar).Value = BillGenDate;
        command.Parameters.Add("@ValidityDate", SqlDbType.NVarChar).Value = ValidityDate;
                
        command.Parameters.Add("@UserGSTIN", SqlDbType.NVarChar).Value = UserGSTIN;
        command.Parameters.Add("@API_GSTIN", SqlDbType.NVarChar).Value = strAPI_GSTIN;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insEWayBill", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int EWAYAddStatus(int EWayID, string EWayNo, int StatusId, string StatusDate, string StatusReason, string StatusRemark, int UserId)
    {
        //0 - Not Valid For Movement, 1 Active, 2 Cancel, 3 Reject
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EWayID", SqlDbType.Int).Value = EWayID;

        command.Parameters.Add("@EwayBillNo", SqlDbType.NVarChar).Value = EWayNo;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@StatusDate", SqlDbType.NVarChar).Value = StatusDate;
        command.Parameters.Add("@StatusReason", SqlDbType.NVarChar).Value = StatusReason;
        command.Parameters.Add("@StatusRemark", SqlDbType.NVarChar).Value = StatusRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insEWayStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);


    }

    public static int EWAYAddVehicle(int EWayID, string EWayNo, string VehicleNo, string VehicleDate, string ValidityDate,
        string FromPlace, int FromStateCode, string Reason, int UserId)
    {
        //0 - Not Valid For Movement, 1 Active, 2 Cancel, 3 Reject
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EWayID", SqlDbType.Int).Value = EWayID;

        command.Parameters.Add("@EwayBillNo", SqlDbType.NVarChar).Value = EWayNo;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleDate", SqlDbType.NVarChar).Value = VehicleDate;
        command.Parameters.Add("@ValidityDate", SqlDbType.NVarChar).Value = ValidityDate;
        command.Parameters.Add("@FromPlace", SqlDbType.NVarChar).Value = FromPlace;
        command.Parameters.Add("@FromStateCode", SqlDbType.Int).Value = FromStateCode;
        command.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = Reason;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insEWayVehicleLog", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int UpdateEWayBill(string strEWayBillNo, int TransType, int TansSubType, string strDocType, string strDocNo,
        DateTime dtDocDate, string FromGSTIN, string ToGSTIN, string ToPlace, int ToPin, int ToState, decimal decTotalinvoicevalue, decimal decCGST,
        decimal decSGST, decimal decIGST, decimal decCess, string strTransportorGSTIN, string strVehicleNo, string strLRDate,
        string strStatus, string strRejectStatus, string strValidityDate, int intExtendedTimes, string strBillGenDate,
        string strJsonData, string strUserGSTIN, string strAPI_GSTIN, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EWayBillNo", SqlDbType.NVarChar).Value = strEWayBillNo;
        command.Parameters.Add("@TransTypeId", SqlDbType.Int).Value = TransType;
        command.Parameters.Add("@TansSubTypeId", SqlDbType.Int).Value = TansSubType;
        command.Parameters.Add("@DocumentType", SqlDbType.NVarChar).Value = strDocType;
        command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = strDocNo;
        command.Parameters.Add("@DocumentDate", SqlDbType.DateTime).Value = dtDocDate;

        command.Parameters.Add("@FromGSTIN", SqlDbType.NVarChar).Value = FromGSTIN;
        command.Parameters.Add("@ToGSTIN", SqlDbType.NVarChar).Value = ToGSTIN;
        command.Parameters.Add("@TransportGSTIN", SqlDbType.NVarChar).Value = strTransportorGSTIN;

        command.Parameters.Add("@DeliveryPlace", SqlDbType.NVarChar).Value = ToPlace;
        command.Parameters.Add("@DeliveryPinCode", SqlDbType.Int).Value = ToPin;
        command.Parameters.Add("@DeliveryStateCode", SqlDbType.Int).Value = ToState;

        command.Parameters.Add("@TotInvValue", SqlDbType.Decimal).Value = decTotalinvoicevalue;
        command.Parameters.Add("@CGST", SqlDbType.Decimal).Value = decCGST;
        command.Parameters.Add("@SGST", SqlDbType.Decimal).Value = decSGST;
        command.Parameters.Add("@IGST", SqlDbType.Decimal).Value = decIGST;
        command.Parameters.Add("@CESS", SqlDbType.Decimal).Value = decCess;

        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = strVehicleNo;
        command.Parameters.Add("@LRDate", SqlDbType.NVarChar).Value = strLRDate;


        command.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = strStatus;
        command.Parameters.Add("@RejectStatus", SqlDbType.NVarChar).Value = strRejectStatus;
        command.Parameters.Add("@ExtendedTimes", SqlDbType.Int).Value = intExtendedTimes;
        command.Parameters.Add("@BillGenDate", SqlDbType.NVarChar).Value = strBillGenDate;
        command.Parameters.Add("@ValidityDate", SqlDbType.NVarChar).Value = strValidityDate;
        command.Parameters.Add("@JSONData", SqlDbType.NVarChar).Value = strJsonData;
        command.Parameters.Add("@UserGSTIN", SqlDbType.NVarChar).Value = strUserGSTIN;

        command.Parameters.Add("@API_GSTIN", SqlDbType.NVarChar).Value = strAPI_GSTIN;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updEWayBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddEWayBillConsole(string EWayBillNo, string BillGenDate, string UserGSTIN, string DocumentNo,
        DateTime DocumentDate, string FromGSTIN, string ToGSTIN, Decimal TotalInvoiceValue, string DeliveryPlace, int PinCode,
        int StateCode, string ValidityDate, int ExtendedTimes, string StatusName, string RejectStatus, string strAPI_GSTIN, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EWayBillNo", SqlDbType.NVarChar).Value = EWayBillNo;
        command.Parameters.Add("@BillGenDate", SqlDbType.NVarChar).Value = BillGenDate;
        command.Parameters.Add("@UserGSTIN", SqlDbType.NVarChar).Value = UserGSTIN;
        command.Parameters.Add("@FromGSTIN", SqlDbType.NVarChar).Value = FromGSTIN;
        command.Parameters.Add("@ToGSTIN", SqlDbType.NVarChar).Value = ToGSTIN;
        command.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = DocumentNo;
        command.Parameters.Add("@TotInvValue", SqlDbType.NVarChar).Value = TotalInvoiceValue;

        if (DocumentDate != DateTime.MinValue)
            command.Parameters.Add("@DocumentDate", SqlDbType.DateTime).Value = DocumentDate;

        command.Parameters.Add("@DeliveryPlace", SqlDbType.NVarChar).Value = DeliveryPlace;

        command.Parameters.Add("@PinCode", SqlDbType.Int).Value = PinCode;
        command.Parameters.Add("@StateCode", SqlDbType.Int).Value = StateCode;

        command.Parameters.Add("@ValidityDate", SqlDbType.NVarChar).Value = ValidityDate;

        command.Parameters.Add("@ExtendedTimes", SqlDbType.Int).Value = ExtendedTimes;
        command.Parameters.Add("@StatusName", SqlDbType.NVarChar).Value = StatusName;
        command.Parameters.Add("@RejectStatus", SqlDbType.NVarChar).Value = RejectStatus;

        command.Parameters.Add("@API_GSTIN", SqlDbType.NVarChar).Value = strAPI_GSTIN;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insEWayBillAuto", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }
    #endregion

    #region Credit Control
    public static DataSet GetCreditActiveCustomer()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("GetCustCreditInput", command);
    }
    public static DataSet GetCreditByCustomerName(string CustomerName)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;

        return CDatabase.GetDataSet("GetCustCreditbyName", command);
    }
    #endregion

    #region Company Category
    public static int AddCompanyCategory(int CustomerId, int CategoryID, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCompanyCategory", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetCompanyCategoryMS()
    {
        return CDatabase.GetDataSet("SELECT lId, sName FROM BS_CompanyCategoryMS WHERE bDel=0");
    }

    public static void FillCompanyByCategory(DropDownList dropdownlist, int CategoryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;

        CDatabase.BindControls(dropdownlist, "GetCompanyByCategoryID", command, "CustName", "lid");
    }
    public static void FillCompanyByCategory(ListBox listbox, int CategoryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;

        CDatabase.BindControls(listbox, "GetCompanyByCategoryID", command, "CustName", "lid");
    }

    public static void FillCompanyCategoryByCountryID(ListBox CheckBoxList, int CompanyCategoryID, int CountryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CompanyCategoryID;
        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;

        CDatabase.BindControls(CheckBoxList, "GetCompanyByCategoryCountry", command, "CustName", "lid");
    }

    public static void FillCompanyUserByCountryID(ListBox CheckBoxList, int CompanyCategoryID, int CountryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CompanyCategoryID;
        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;

        CDatabase.BindControls(CheckBoxList, "GetCompanyUserByCategoryCountry", command, "sName", "lid");
    }

    public static void FillCompanyUserByListID(ListBox CheckBoxList, string CompanyListID, int CompanyCategoryID, int CountryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CompanyListID", SqlDbType.NVarChar).Value = CompanyListID;
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CompanyCategoryID;
        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;

        CDatabase.BindControls(CheckBoxList, "GetCompanyUserByCompanyList", command, "sName", "lid");
    }
    #endregion

    #region N FORM DELIVERY DETAILS

    public static int AddPCDDocument_Nform(int JobId, int DocumentId, string FileName, string Docpath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        if (Docpath != "")
            command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = Docpath;
        if (FileName != "")
            command.Parameters.Add("@FileName", SqlDbType.Int).Value = FileName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDDocForNform", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int GetJobDetailForNForm(string JobRefNo)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("GetJobDetailForNForm", command, "@output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetDeliveryDetailsForNForm(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetNFormDeliveryDetail", cmd);
    }

    public static int UpdateDeliveryDetForNForm(int lid, DateTime NClosingDate, int UserId, int JobId, string Amount)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        if (NClosingDate != DateTime.MinValue)
            command.Parameters.Add("@NClosingDate", SqlDbType.DateTime).Value = NClosingDate;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (Amount != "")
            command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDeliveryDetailForNForm", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetDeliveryNform_Report(DateTime dtStartDate, DateTime dtEndDate)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = dtStartDate;
        cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = dtEndDate;
        return CDatabase.GetDataSet("GetBS_JobDeliveryDetailNFormReport", cmd);
    }

    public static DataSet GetUploadedNformDetail(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetNformUploadedDate", cmd);
    }

    #endregion

    #region OTHER JOBS DETAILS

    public static int GetJobIdCustRefNo_AsPerJobNo(int JobId, string Task, string CustRefNo)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Task", SqlDbType.VarChar).Value = Task;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("GetJobIdCustRefNo_AsPerJobNo", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static int AddOthersJob(int OtherJobId, string CustRefNo, int CustomerId, int ConsigneeId, int Mode, string Details, int UserID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@OtherJobID", SqlDbType.NVarChar).Value = OtherJobId;
        command.Parameters.Add("@CustRefNo", SqlDbType.NVarChar).Value = CustRefNo;
        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ConsigneeId", SqlDbType.Int).Value = ConsigneeId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Details;
        command.Parameters.Add("lUser", SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updOtherJob", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static string GetNextOtherJobNo(int BranchId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchId;
        cmd.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_GetNextOtherJobNo", cmd, "@OutPut");

        return Convert.ToString(SPresult);
    }

    public static string GetNextOtherJobNo(int BranchId, int FinYearId) //int FinYearId,
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        cmd.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_GetNextOtherJobNo", cmd, "@OutPut");

        return Convert.ToString(SPresult);
    }

    public static int AddOthersJobDetail(String JobRefNo, int CustomerId, int BabajiBranchId, int DivisionId, int PlantId, int FInYear, string Purpose, int UserID)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        cmd.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BabajiBranchId;
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        cmd.Parameters.Add("@FInYear", SqlDbType.Int).Value = FInYear;
        cmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Purpose;
        cmd.Parameters.Add("lUser", SqlDbType.Int).Value = UserID;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insOtherJobDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    #endregion

    #region BILLING TRANSPORT

    public static int AddTransportVehicleDetail(int JobId, string TransporterName, int JobDeliveryId, int Packages, int ContainerId, string VehicleNo, int VehicleType,
                                                string DeliveryFrom, string DeliveryTo, DateTime DispatchDate, DateTime DeliveryDate, int lUser, string TPFrightRate,
                                                DateTime dtReportDate, DateTime dtUnloadDate, int DetentionDays, string DetentionCharges, string VaraiCharges,
                                                string EmptyOffLoadingCharges, string TempoUnionCharges, string Total, string Remarks, DateTime dtEmptyContReturnDate,
                                                string LrCopiesDocPath, string ReceiptDocPath)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobDeliveryId", SqlDbType.Int).Value = JobDeliveryId;
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@Packages", SqlDbType.Int).Value = Packages;
        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        command.Parameters.Add("@DeliveryFrom", SqlDbType.NVarChar).Value = DeliveryFrom;
        command.Parameters.Add("@DeliveryTo", SqlDbType.NVarChar).Value = DeliveryTo;
        command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = DispatchDate;
        command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = DeliveryDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@TPFrightRate", SqlDbType.Decimal).Value = TPFrightRate;
        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = dtReportDate;
        command.Parameters.Add("@UnloadDate", SqlDbType.DateTime).Value = dtUnloadDate;
        command.Parameters.Add("@DetentionDays", SqlDbType.Int).Value = DetentionDays;
        command.Parameters.Add("@DetentionCharges", SqlDbType.Decimal).Value = DetentionCharges;
        command.Parameters.Add("@WaraiCharges", SqlDbType.Decimal).Value = VaraiCharges;
        command.Parameters.Add("@EmptyOffLoadingCharges", SqlDbType.Decimal).Value = EmptyOffLoadingCharges;
        command.Parameters.Add("@TempoUnionCharges", SqlDbType.Decimal).Value = TempoUnionCharges;
        command.Parameters.Add("@Total", SqlDbType.Decimal).Value = Total;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
        if (dtEmptyContReturnDate != DateTime.MinValue)
            command.Parameters.Add("@EmptyContReturnDate", SqlDbType.DateTime).Value = dtEmptyContReturnDate;
        command.Parameters.Add("@LrCopiesDocPath", SqlDbType.NVarChar).Value = LrCopiesDocPath;
        command.Parameters.Add("@ReceiptDocPath", SqlDbType.NVarChar).Value = ReceiptDocPath;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insTOP_VehicleDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }


    //public static int GetJobDetailByJobRefNo(string JobRefNo)
    //{
    //    SqlCommand command = new SqlCommand();
    //    string SPresult = "";
    //    command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

    //    command.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
    //    SPresult = CDatabase.GetSPOutPut("GetJobDetailByJobRefNo", command, "@output");
    //    return Convert.ToInt32(SPresult);
    //}

    public static int UpdateTransportVehicleDetail(int lid, DateTime DeliveryDate, string TPFrightRate, DateTime dtReportDate, DateTime dtUnloadDate, int DetentionDays,
                                                string DetentionCharges, string Total, string VaraiCharges, string EmptyOffLoadingCharges, string TempoUnionCharges,
                                                string Remarks, DateTime dtEmptyContReturnDate, string LrCopiesDocPath, string ReceiptDocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = DeliveryDate;
        command.Parameters.Add("@TPFrightRate", SqlDbType.Decimal).Value = TPFrightRate;
        command.Parameters.Add("@ReportDate", SqlDbType.DateTime).Value = dtReportDate;
        command.Parameters.Add("@UnloadDate", SqlDbType.DateTime).Value = dtUnloadDate;
        command.Parameters.Add("@DetentionDays", SqlDbType.Int).Value = DetentionDays;
        command.Parameters.Add("@DetentionCharges", SqlDbType.Decimal).Value = DetentionCharges;
        command.Parameters.Add("@VaraiCharges", SqlDbType.Decimal).Value = VaraiCharges;
        command.Parameters.Add("@EmptyOffLoadingCharges", SqlDbType.Decimal).Value = EmptyOffLoadingCharges;
        command.Parameters.Add("@TempoUnionCharges", SqlDbType.Decimal).Value = TempoUnionCharges;
        command.Parameters.Add("@Total", SqlDbType.Decimal).Value = Total;
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
        if (dtEmptyContReturnDate != DateTime.MinValue)
            command.Parameters.Add("@EmptyContReturnDate", SqlDbType.DateTime).Value = dtEmptyContReturnDate;
        command.Parameters.Add("@LrCopiesDocPath", SqlDbType.NVarChar).Value = LrCopiesDocPath;
        command.Parameters.Add("@ReceiptDocPath", SqlDbType.NVarChar).Value = ReceiptDocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updTOP_VehicleDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBS_JobDeliveryDetailAsPerLid(int lid, int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetBS_JobDeliveryDetailAsPerLid", cmd);
    }

    public static DataSet GetAllTOP_VehicleDetail()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataSet("GetTOP_VehicleDetail", cmd);
    }

    public static DataSet GetTOP_VehicleDetail(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("GetTOP_VehicleDetailAsPerLid", cmd);
    }

    public static int AddTransportInvoiceCopy(int JobId, string DocPath, string FileName, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DocId", SqlDbType.Int).Value = 49;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insTransportInvoiceDoc", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillInvoiceCopy(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("TR_GetBillInvoiceCopy", cmd);
    }

    public static DataSet GetTPVehicleDetailReport(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("TR_GetVehicleDetailReport", cmd);
    }

    public static DataSet GetJobWsVehicleDetReport(int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("TR_GetJobWsVehicleDetReport", cmd);
    }

    public static DataSet GetDeliveredVehicleDetail(string Transporter)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = Transporter;
        return CDatabase.GetDataSet("TR_GetDeliveredVehicleDetails", cmd);
    }

    public static DataSet GetBillDetailForApprval(string VehicleNo, int JobId, int TransporterId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterId;
        return CDatabase.GetDataSet("TR_GetBillDetailForApprval", cmd);
    }

    public static DataSet GetDetailedVehicleDetails(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("TR_GetDetailedVehicleDetails", cmd);
    }

    public static int UpdateVehicleBillFinal(int lid, string TPFrightRate, int DetentionDays, string DetentionCharges, string VaraiCharges, string EmptyOffLoadingCharges,
                                                    string TempoUnionCharges, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@FreightRate", SqlDbType.Decimal).Value = TPFrightRate;
        command.Parameters.Add("@DetentionDays", SqlDbType.Int).Value = DetentionDays;
        command.Parameters.Add("@DetentionCharges", SqlDbType.Decimal).Value = DetentionCharges;
        command.Parameters.Add("@WaraiCharges", SqlDbType.Decimal).Value = VaraiCharges;
        command.Parameters.Add("@EmptyOffLoadingCharges", SqlDbType.Decimal).Value = EmptyOffLoadingCharges;
        command.Parameters.Add("@TempoUnionCharges", SqlDbType.Decimal).Value = TempoUnionCharges;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_updVehicleBillFinal", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddConsolidateJobBill(int TransBillId, int JobId, int BillRequestId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@TransBillingId", SqlDbType.Int).Value = TransBillId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillRequestId", SqlDbType.Int).Value = BillRequestId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insTOP_BillingJob", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillRequestDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetBillRequestDetail", command);
    }

    public static DataSet GetJobDetailForTransVendor(int JobId, int DeliveryId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (DeliveryId != 0)
            command.Parameters.Add("@DeliveryId", SqlDbType.Int).Value = DeliveryId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("GetJobDetailForTransVendor", command);
    }

    public static DataSet GetUpdatedVehicles(int JobId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        return CDatabase.GetDataSet("TR_GetUpdatedVehicles", command);
    }

    public static int GetRejectedVehicle(int JobId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_GetRejectedVehicles", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    //////////////////////////////////////  BABAJI  TRANSPORTER MODULE   ///////////////////////////////////////////////////////////

    public static DataSet GetVehicleWsBillDetails()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataSet("GetVehicleWsBillDetails", cmd);
    }

    public static int UpdateJobBillStatus(int JobId, int StatusId, int RequestBy, int ApprovedBy, DateTime dtApprovedDate, string Remark, string VehicleNo)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@RequestBy", SqlDbType.Int).Value = RequestBy;
        if (ApprovedBy != 0)
            command.Parameters.Add("@ApprovedBy", SqlDbType.Int).Value = ApprovedBy;
        if (dtApprovedDate != DateTime.MinValue)
            command.Parameters.Add("@ApprovedDate", SqlDbType.DateTime).Value = dtApprovedDate;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        if (VehicleNo.ToString() != "")
            command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insBillRequestDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateRejectedVehicleStatus(int lid)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Deliverylid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_updRejectVehicle", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    /////////////////////////////////////////  NFORM DETAILS FOR TRANSPORTER  ///////////////////////////////////////////////////////////////
    public static int GetJobBranchDetail(int JobId)
    {
        int Count = 0;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        Count = CDatabase.GetSPCount("TR_GetJobBranchDetail", command);

        return Count;
    }

    public static DataSet GetNformDetail(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("TR_GetNformDetailForJob", cmd);
    }

    public static int UpdateNformByTransporter(int lid, DateTime NClosingDate, DateTime dtNformDate, int UserId, int JobId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        if (NClosingDate != DateTime.MinValue)
            command.Parameters.Add("@NClosingDate", SqlDbType.DateTime).Value = NClosingDate;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        if (dtNformDate != DateTime.MinValue)
            command.Parameters.Add("@NFormDate", SqlDbType.Date).Value = dtNformDate;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updJobDeliveryDetailForNForm", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }


    #endregion

    public static int AddBranchDailyExpense(int BranchId, int PaidAmount, DateTime PaymentDate, string strRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@BranchId", SqlDbType.NVarChar).Value = BranchId;
        command.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = PaymentDate;
        command.Parameters.Add("@PaidAmount", SqlDbType.NVarChar).Value = PaidAmount;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBranchExpense", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    #region ALIBABA ENQUIRY DETAILS

    public static int SetUserAvailability(string Task, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_SetUserLastLoginDate", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddNewEnquiry(string EnqRefNo, int ModeId, int TermsId, string Customer, string Consignee, int IEC, string Shipper,
                                    string ShipperAddress, string ShipperPinCode, int PortofLoadingId, int PortofDischargeId, int ShipmentTypeId,
                                    string Commodity, Decimal Quantity, Decimal DimensionLength, Decimal DimensionWidth, Decimal DimensionHeight,
                                    Decimal Weight, int Pkgs, int Cont20, int Cont40, int IsDgGoods, string ProductLink, Decimal HsCode,
                                    Decimal InvoiceValue, string DeliveryAddress, string DeliveryAddPincode, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqRefNo", SqlDbType.NVarChar).Value = EnqRefNo;
        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@TermsId", SqlDbType.Int).Value = TermsId;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = Customer;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = Consignee;
        command.Parameters.Add("@IEC", SqlDbType.Bit).Value = IEC;
        if (Shipper != "")
            command.Parameters.Add("@Shipper", SqlDbType.NVarChar).Value = Shipper;
        if (ShipperAddress != "")
            command.Parameters.Add("@ShipperAddress", SqlDbType.NVarChar).Value = ShipperAddress;
        if (ShipperPinCode != "")
            command.Parameters.Add("@ShipperPinCode", SqlDbType.NVarChar).Value = ShipperPinCode;
        command.Parameters.Add("@PortofLoadingId", SqlDbType.Int).Value = PortofLoadingId;
        command.Parameters.Add("@PortofDischargeId", SqlDbType.Int).Value = PortofDischargeId;
        if (ShipmentTypeId != 0)
            command.Parameters.Add("@ShipmentTypeId", SqlDbType.Int).Value = ShipmentTypeId;
        command.Parameters.Add("@Commodity", SqlDbType.NVarChar).Value = Commodity;
        if (Quantity != 0)
            command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = Quantity;
        command.Parameters.Add("@DimensionLength", SqlDbType.Decimal).Value = DimensionLength;
        command.Parameters.Add("@DimensionWidth", SqlDbType.Decimal).Value = DimensionWidth;
        command.Parameters.Add("@DimensionHeight", SqlDbType.Decimal).Value = DimensionHeight;
        command.Parameters.Add("@Weight", SqlDbType.Decimal).Value = Weight;
        command.Parameters.Add("@NoofPkg", SqlDbType.Int).Value = Pkgs;
        command.Parameters.Add("@Cont20", SqlDbType.Int).Value = Cont20;
        command.Parameters.Add("@Cont40", SqlDbType.Int).Value = Cont40;
        command.Parameters.Add("@IsDgGoods", SqlDbType.Bit).Value = IsDgGoods;
        command.Parameters.Add("@ProductLink", SqlDbType.NVarChar).Value = ProductLink;
        command.Parameters.Add("@HsCode", SqlDbType.Decimal).Value = HsCode;
        command.Parameters.Add("@InvoiceValue", SqlDbType.Decimal).Value = InvoiceValue;
        command.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar).Value = DeliveryAddress;
        command.Parameters.Add("@DeliveryAddPincode", SqlDbType.NVarChar).Value = DeliveryAddPincode;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_insAlibabaEnquiry", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static List<AlibabaFreightTracking> GetAlibabaTrackingDetails(int StatusId, string Task, int lUser)
    {
        SqlConnection con = CDatabase.getConnection();
        List<AlibabaFreightTracking> lstFreightTrackingDetails = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetAlibabaFreightTracking", con);
            if (@StatusId != 0)
                cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
            cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
            if (lUser != 0)
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstFreightTrackingDetails.Add(new AlibabaFreightTracking()
                    {
                        lid = dr["EnqId"].ToString(),
                        EnqRefNo = dr["EnqRefNo"].ToString(),
                        EnqDate = dr["EnqDate"].ToString(),
                        Mode = dr["Mode"].ToString(),
                        Terms = dr["Term"].ToString(),
                        CustomerName = dr["CustName"].ToString(),
                        Consignee = dr["Consignee"].ToString(),
                        IEC = dr["IEC"].ToString(),
                        Shipper = dr["Shipper"].ToString(),
                        ShipperAddress = dr["ShipperAddress"].ToString(),
                        ShipperAddPinCode = dr["ShipperPincode"].ToString(),
                        PortOfLoading = dr["PortofLoading"].ToString(),
                        PortOfDischarge = dr["PortofDischarged"].ToString(),
                        ShipmentType = dr["ShipmentType"].ToString(),
                        Commodity = dr["Commodity"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        DimensionLength = dr["DimensionLength"].ToString(),
                        DimensionWidth = dr["DimensionWidth"].ToString(),
                        DimensionHeight = dr["DimensionHeight"].ToString(),
                        Dimension = dr["Dimension"].ToString(),
                        Weight = dr["Weight"].ToString(),
                        NoofPkgs = dr["NoofPkg"].ToString(),
                        Cont20 = dr["Cont20"].ToString(),
                        Cont40 = dr["Cont40"].ToString(),
                        IsDgGoods = dr["IsDgGoods"].ToString(),
                        ProductLink = dr["ProductLink"].ToString(),
                        HsCode = dr["HsCode"].ToString(),
                        InvoiceValue = dr["InvoiceValue"].ToString(),
                        DeliveryAddress = dr["DeliveryAddress"].ToString(),
                        DeliveryAddPincode = dr["DeliveryAddPincode"].ToString(),
                        Status = dr["Status"].ToString(),
                        StatusDate = Convert.ToDateTime(dr["StatusDate"].ToString()),
                        lUser = dr["EnquiryUserName"].ToString(),
                        //UserName = dr["UserName"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstFreightTrackingDetails;
    }

    public static List<AlibabaFreightTracking> GetAlibabaAwardedEnquiry(string Task, int lUser)
    {
        SqlConnection con = CDatabase.getConnection(); ;
        List<AlibabaFreightTracking> lstFreightTrackingDetails = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetAlibabaAwardedEnquiry", con);
            cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
            if (lUser != 0)
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstFreightTrackingDetails.Add(new AlibabaFreightTracking()
                    {
                        lid = dr["EnqId"].ToString(),
                        EnqRefNo = dr["EnqRefNo"].ToString(),
                        EnqDate = dr["EnqDate"].ToString(),
                        Mode = dr["Mode"].ToString(),
                        Terms = dr["Term"].ToString(),
                        CustomerName = dr["CustName"].ToString(),
                        Consignee = dr["Consignee"].ToString(),
                        IEC = dr["IEC"].ToString(),
                        Shipper = dr["Shipper"].ToString(),
                        ShipperAddress = dr["ShipperAddress"].ToString(),
                        ShipperAddPinCode = dr["ShipperPincode"].ToString(),
                        PortOfLoading = dr["PortofLoading"].ToString(),
                        PortOfDischarge = dr["PortofDischarged"].ToString(),
                        ShipmentType = dr["ShipmentType"].ToString(),
                        Commodity = dr["Commodity"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        DimensionLength = dr["DimensionLength"].ToString(),
                        DimensionWidth = dr["DimensionWidth"].ToString(),
                        DimensionHeight = dr["DimensionHeight"].ToString(),
                        Dimension = dr["Dimension"].ToString(),
                        Weight = dr["Weight"].ToString(),
                        NoofPkgs = dr["NoofPkg"].ToString(),
                        Cont20 = dr["Cont20"].ToString(),
                        Cont40 = dr["Cont40"].ToString(),
                        IsDgGoods = dr["IsDgGoods"].ToString(),
                        ProductLink = dr["ProductLink"].ToString(),
                        HsCode = dr["HsCode"].ToString(),
                        InvoiceValue = dr["InvoiceValue"].ToString(),
                        DeliveryAddress = dr["DeliveryAddress"].ToString(),
                        DeliveryAddPincode = dr["DeliveryAddPincode"].ToString(),
                        Status = dr["Status"].ToString(),
                        StatusDate = Convert.ToDateTime(dr["StatusDate"].ToString()),
                        lUser = dr["EnquiryUserName"].ToString(),
                        //UserName = dr["UserName"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstFreightTrackingDetails;
    }

    public static List<AlibabaFreightTracking> GetEnqDetailAsPerEnqId(int EnqId)
    {
        SqlConnection con = CDatabase.getConnection(); ;
        List<AlibabaFreightTracking> lstFreightTrackingDetails = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetEnqDetail", con);
            cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstFreightTrackingDetails.Add(new AlibabaFreightTracking()
                    {
                        lid = dr["EnqId"].ToString(),
                        EnqRefNo = dr["EnqRefNo"].ToString(),
                        EnqDate = dr["EnqDate"].ToString(),
                        Mode = dr["Mode"].ToString(),
                        Terms = dr["Term"].ToString(),
                        CustomerName = dr["CustName"].ToString(),
                        Consignee = dr["Consignee"].ToString(),
                        IEC = dr["IEC"].ToString(),
                        Shipper = dr["Shipper"].ToString(),
                        ShipperAddress = dr["ShipperAddress"].ToString(),
                        ShipperAddPinCode = dr["ShipperPincode"].ToString(),
                        PortOfLoading = dr["PortofLoading"].ToString(),
                        PortOfDischarge = dr["PortofDischarged"].ToString(),
                        ShipmentType = dr["ShipmentType"].ToString(),
                        Commodity = dr["Commodity"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        DimensionLength = dr["DimensionLength"].ToString(),
                        DimensionWidth = dr["DimensionWidth"].ToString(),
                        DimensionHeight = dr["DimensionHeight"].ToString(),
                        Dimension = dr["Dimension"].ToString(),
                        Weight = dr["Weight"].ToString(),
                        NoofPkgs = dr["NoofPkg"].ToString(),
                        Cont20 = dr["Cont20"].ToString(),
                        Cont40 = dr["Cont40"].ToString(),
                        IsDgGoods = dr["IsDgGoods"].ToString(),
                        ProductLink = dr["ProductLink"].ToString(),
                        HsCode = dr["HsCode"].ToString(),
                        InvoiceValue = dr["InvoiceValue"].ToString(),
                        DeliveryAddress = dr["DeliveryAddress"].ToString(),
                        DeliveryAddPincode = dr["DeliveryAddPincode"].ToString(),
                        Status = dr["Status"].ToString(),
                        StatusDate = Convert.ToDateTime(dr["StatusDate"].ToString()),
                        DocDir = dr["DocDir"].ToString(),
                        lUser = dr["EnquiryUserName"].ToString(),
                        StatusId = Convert.ToInt32(dr["StatusID"].ToString()),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstFreightTrackingDetails;
    }

    public static List<AlibabaFreightTracking> GetFreightEnqDoc(int EnqId)
    {
        SqlConnection con = CDatabase.getConnection(); ;
        List<AlibabaFreightTracking> lstFreightTrackingDetails = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetUploadedDocument", con);
            cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstFreightTrackingDetails.Add(new AlibabaFreightTracking()
                    {
                        UploadedDate = Convert.ToDateTime(dr["UploadedDate"].ToString()),
                        DocId = Convert.ToInt32(dr["DocId"].ToString()),
                        DocName = dr["DocName"].ToString(),
                        lUser = dr["UserName"].ToString(),
                        DocPath = dr["DocPath"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstFreightTrackingDetails;
    }

    public static List<AlibabaFreightTracking> GetEnqStatusHistory(int EnqId)
    {
        SqlConnection con = CDatabase.getConnection();
        List<AlibabaFreightTracking> lstFreightTrackingDetails = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetAlibabaStatusHistory", con);
            cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstFreightTrackingDetails.Add(new AlibabaFreightTracking()
                    {
                        StatusDate = Convert.ToDateTime(dr["StatusDate"].ToString()),
                        Status = dr["StatusName"].ToString(),
                        Remarks = dr["Remarks"].ToString(),
                        lUser = dr["UserName"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstFreightTrackingDetails;
    }

    /*************************************************   CHATTING FUNCTIONS   **********************************************************/

    public static int SendChatMessage(string Message, int MessageTo, int StatusId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sMessage", SqlDbType.NVarChar).Value = Message;
        command.Parameters.Add("@MessageTo", SqlDbType.Int).Value = MessageTo;
        command.Parameters.Add("@StatusId", SqlDbType.Bit).Value = StatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_insEnquiryChat", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static List<FreightChat> GetEnquiryChatDates(int lUser, int MessageTo)
    {
        SqlConnection con = CDatabase.getConnection(); ;
        List<FreightChat> lstGetChatDetails = new List<FreightChat>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetEnquiryChatDates", con);
            cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
            cmd.Parameters.Add("@MessageTo", SqlDbType.Int).Value = MessageTo;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstGetChatDetails.Add(new FreightChat()
                    {
                        Date = Convert.ToDateTime(dr["ChatDate"].ToString()),
                        FormattedDate = dr["FormatedDate"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstGetChatDetails;
    }

    public static List<FreightChat> GetDayWsChatDetails(int lUser, int MessageTo, DateTime dtDate)
    {
        SqlConnection con = CDatabase.getConnection(); ;
        List<FreightChat> lstGetChatDetails = new List<FreightChat>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetDayWsChatDetails", con);
            cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
            cmd.Parameters.Add("@MessageTo", SqlDbType.Int).Value = MessageTo;
            cmd.Parameters.Add("@Date", SqlDbType.Date).Value = dtDate;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstGetChatDetails.Add(new FreightChat()
                    {
                        Date = Convert.ToDateTime(dr["MatchDate"].ToString()),
                        lid = dr["lid"].ToString(),
                        Message = dr["sMessage"].ToString(),
                        MessageTo_UserId = dr["MessageToId"].ToString(),
                        CurrentStatusId = dr["StatusId"].ToString(),
                        MessageTime = dr["MsgTime"].ToString(),
                        lUser = dr["UserId"].ToString(),
                        MessageToName = dr["MessageTo"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        PersonNaming = dr["PersonNaming"].ToString(),
                        FormattedDate = dr["FormatedDate"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstGetChatDetails;
    }

    public static List<FreightChat> GetOnlineUsers(int lUser)
    {
        SqlConnection con = CDatabase.getConnection(); ;
        List<FreightChat> lstGetOnlineUsers = new List<FreightChat>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetOnlineUsers", con);
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstGetOnlineUsers.Add(new FreightChat()
                    {
                        lUser = dr["lid"].ToString(),
                        UserLastLoginDate = dr["LastLoginDate"].ToString(),
                        UserName = dr["sName"].ToString(),
                        IsAvailable = dr["IsAvailable"].ToString(),
                        CurrentStatus = dr["LoginLog"].ToString(),
                        TotalMsgs = dr["MsgsReceived"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstGetOnlineUsers;
    }

    public static int UpdateMsgAsRead(int MessageTo, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@MsgTo", SqlDbType.Int).Value = MessageTo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_updReadMsg", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    /****************************************************  DASHBOARD EVENTS  **********************************************************/

    public static DataSet GetNoOfEnquiries(string Task, int lUser)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
        if (lUser != 0)
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("FR_GetNoOfEnquiries", cmd);
    }

    public static DataSet GetQuotedEnquiryCnt(string Task, int lUser)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
        if (lUser != 0)
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("FR_GetCntOfQuotedEnquiry", cmd);
    }

    public static DataSet GetCntOfEnquiryStatus(int Status, string Task, int lUser)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@StatusId", SqlDbType.Int).Value = Status;
        cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
        if (lUser != 0)
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("FR_GetCntOfEnquiryStatus", cmd);
    }

    //public static List<AlibabaFreightTracking> GetMonthWsEnqDetail(string Task, int lUser)
    //{
    //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString);
    //    List<AlibabaFreightTracking> lstGetEnqDetail = new List<AlibabaFreightTracking>();

    //    try
    //    {
    //        SqlDataReader dr;
    //        SqlCommand cmd = new SqlCommand("FR_GetMonthWsEnqDetail", con);
    //        cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
    //        if (lUser != 0)
    //            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
    //        cmd.CommandType = CommandType.StoredProcedure;

    //        con.Open();
    //        dr = cmd.ExecuteReader();
    //        if (dr.HasRows)
    //        {
    //            while (dr.Read())
    //            {
    //                lstGetEnqDetail.Add(new AlibabaFreightTracking()
    //                {
    //                    StatusId = Convert.ToInt32(dr["StatusId"].ToString()),
    //                    Status = dr["Status"].ToString(),
    //                    January = dr["Jan"].ToString(),
    //                    February = dr["Feb"].ToString(),
    //                    March = dr["Mar"].ToString(),
    //                    April = dr["Apr"].ToString(),
    //                    May = dr["May"].ToString(),
    //                    June = dr["Jun"].ToString(),
    //                    July = dr["Jul"].ToString(),
    //                    August = dr["Aug"].ToString(),
    //                    September = dr["Sep"].ToString(),
    //                    October = dr["Oct"].ToString(),
    //                    November = dr["Nov"].ToString(),
    //                    December = dr["Dec"].ToString(),
    //                });
    //            }
    //        }

    //        dr.Close();
    //        dr.Dispose(); cmd.Dispose();

    //    }
    //    catch (Exception ex)
    //    {
    //        throw (ex);
    //    }
    //    finally
    //    {
    //        if (con.State == ConnectionState.Open)
    //            con.Close();
    //        con.Dispose();
    //    }
    //    return lstGetEnqDetail;
    //}

    public static List<AlibabaFreightTracking> GetFreightUserDetail(string Task, int lUser, int MonthId, string Customer, int Mode)
    {
        SqlConnection con = CDatabase.getConnection();
        List<AlibabaFreightTracking> lstGetEnqDetail = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetUserFreightDetail", con);
            cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
            if (lUser != 0)
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
            cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = MonthId;
            if (Customer != "")
                cmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = Customer;
            if (Mode != 0)
                cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstGetEnqDetail.Add(new AlibabaFreightTracking()
                    {
                        lid = dr["lId"].ToString(),
                        UserName = dr["sName"].ToString(),
                        Enquiry = dr["Enquiry"].ToString(),
                        Quoted = dr["Quoted"].ToString(),
                        Awarded = dr["Awarded"].ToString(),
                        Lost = dr["Lost"].ToString(),
                        Executed = dr["Executed"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose(); cmd.Dispose();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstGetEnqDetail;
    }

    public static List<AlibabaFreightTracking> GetAllEnquiryDetails(string Task, int lUser)
    {
        SqlConnection con = CDatabase.getConnection();
        List<AlibabaFreightTracking> lstFreightTrackingDetails = new List<AlibabaFreightTracking>();

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("FR_GetAllEnquiry", con);
            cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
            if (lUser != 0)
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lstFreightTrackingDetails.Add(new AlibabaFreightTracking()
                    {
                        lid = dr["EnqId"].ToString(),
                        EnqRefNo = dr["EnqRefNo"].ToString(),
                        EnqDate = dr["EnqDate"].ToString(),
                        Mode = dr["Mode"].ToString(),
                        Terms = dr["Term"].ToString(),
                        CustomerName = dr["CustName"].ToString(),
                        Consignee = dr["Consignee"].ToString(),
                        IEC = dr["IEC"].ToString(),
                        Shipper = dr["Shipper"].ToString(),
                        ShipperAddress = dr["ShipperAddress"].ToString(),
                        ShipperAddPinCode = dr["ShipperPincode"].ToString(),
                        PortOfLoading = dr["PortofLoading"].ToString(),
                        PortOfDischarge = dr["PortofDischarged"].ToString(),
                        ShipmentType = dr["ShipmentType"].ToString(),
                        Commodity = dr["Commodity"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        DimensionLength = dr["DimensionLength"].ToString(),
                        DimensionWidth = dr["DimensionWidth"].ToString(),
                        DimensionHeight = dr["DimensionHeight"].ToString(),
                        Dimension = dr["Dimension"].ToString(),
                        Weight = dr["Weight"].ToString(),
                        NoofPkgs = dr["NoofPkg"].ToString(),
                        Cont20 = dr["Cont20"].ToString(),
                        Cont40 = dr["Cont40"].ToString(),
                        IsDgGoods = dr["IsDgGoods"].ToString(),
                        ProductLink = dr["ProductLink"].ToString(),
                        HsCode = dr["HsCode"].ToString(),
                        InvoiceValue = dr["InvoiceValue"].ToString(),
                        DeliveryAddress = dr["DeliveryAddress"].ToString(),
                        DeliveryAddPincode = dr["DeliveryAddPincode"].ToString(),
                        Status = dr["Status"].ToString(),
                        StatusDate = Convert.ToDateTime(dr["StatusDate"].ToString()),
                        lUser = dr["EnquiryUserName"].ToString(),
                        //UserName = dr["UserName"].ToString(),
                    });
                }
            }

            dr.Close();
            dr.Dispose();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Dispose();
        }
        return lstFreightTrackingDetails;
    }

    public static DataSet GetAlibabaFreightSummary(int MonthId, string Task, int lUser)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = MonthId;
        cmd.Parameters.Add("@Task", SqlDbType.NVarChar).Value = Task;
        if (lUser != 0)
            cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = lUser;
        return CDatabase.GetDataSet("FR_GetAlibabaFreightSummary", cmd);
    }

    public static DataSet GetTotalMsgReceived(int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("FR_GetTotalMsgReceived", cmd);
    }

    public static DataSet FillCustForFreightSummary()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataSet("FR_GetCustForFreightSummary", cmd);
    }

    #endregion

    #region Customer Circular Email

    public static void FillServiceMS(ListBox listbox)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(listbox, "BS_GetServicesMS", command, "sName", "ServicesId"); // Company Service List
    }
    public static void FillFAVendorEmail(ListBox listbox, int CategoryID)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(listbox, "BJV_GetVendorEmail", command, "DisplayEMail", "Email"); // customer show text and valu email
    }
    public static void FillCompanyEmailByCategory(ListBox listbox, int CategoryID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID;

        CDatabase.BindControls(listbox, "GetCompanyEmailByCategoryID", command, "DisplayEMail", "Email"); // customer show text and valu email
    }

    public static int AddCircular(int CircularType, string CustomerEmailId, string CustomerSubject, string CustomerBody, string strFilePath, string strFilePath2,
           int CompanyType, bool IsAllContact, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CircularType", SqlDbType.Int).Value = CircularType;
        command.Parameters.Add("@CustomerMailId", SqlDbType.NVarChar).Value = CustomerEmailId;
        command.Parameters.Add("@MailSubject", SqlDbType.NVarChar).Value = CustomerSubject;
        command.Parameters.Add("@MsgBody", SqlDbType.NVarChar).Value = CustomerBody;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = strFilePath;
        command.Parameters.Add("@DocPath2", SqlDbType.NVarChar).Value = strFilePath2;
        command.Parameters.Add("@CompanyType", SqlDbType.Int).Value = CompanyType;
        command.Parameters.Add("@IsAllContact", SqlDbType.Bit).Value = IsAllContact;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCircular", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }



    public static int UpdateCircular(int CircularType, int CircularID, string CustomerEmailId, string CustomerSubject, string CustomerBody, string strFilePath, string strFilePath2,
          int CompanyType, bool IsAllContact, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CircularType", SqlDbType.Int).Value = CircularType;
        command.Parameters.Add("@CircularID", SqlDbType.Int).Value = CircularID;
        command.Parameters.Add("@CustomerMailId", SqlDbType.NVarChar).Value = CustomerEmailId;
        command.Parameters.Add("@MailSubject", SqlDbType.NVarChar).Value = CustomerSubject;
        command.Parameters.Add("@MsgBody", SqlDbType.NVarChar).Value = CustomerBody;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = strFilePath;
        command.Parameters.Add("@DocPath2", SqlDbType.NVarChar).Value = strFilePath2;
        command.Parameters.Add("@CompanyType", SqlDbType.Int).Value = CompanyType;
        command.Parameters.Add("@IsAllContact", SqlDbType.Bit).Value = IsAllContact;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCircular", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddCircularMailStatus(int CircularId, bool IsMailSent, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CircularId", SqlDbType.Int).Value = CircularId;
        command.Parameters.Add("@IsMailSent", SqlDbType.Bit).Value = IsMailSent;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCircularMailStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static DataSet GetCircularDetail(int CircularID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CircularID", SqlDbType.Int).Value = CircularID;

        return CDatabase.GetDataSet("GetCircularById", command);

    }

    public static void GetEmailIdFromBranchIdDetails(ListBox listbox, int BranchId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;

        CDatabase.BindControls(listbox, "GetEmailIdFromBranchId", command, "EmailId", "EmailId");
    }

    public static void GetEmailIdFromAllBranchIdDetails(ListBox listbox)
    {
        SqlCommand command = new SqlCommand();
        CDatabase.BindControls(listbox, "GetEmailIdFromAllBranchId", command, "EmailId", "EmailId");
    }

    public static void CRM_FillCompanyEmailByServiceList(ListBox CheckBoxList, string ServiceListID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@SeriveList", SqlDbType.NVarChar).Value = ServiceListID;

        CDatabase.BindControls(CheckBoxList, "CRM_GetCompanyEmailByCategoryID   ", command, "DisplayEMail", "Email");
    }
    #endregion


    public static int AddCustomerShipper(int CustomerId, int ShipperId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ShipperId", SqlDbType.Int).Value = ShipperId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerShipper", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static void FillAdditionalFields(CheckBoxList CheckBoxList, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        CDatabase.BindControls(CheckBoxList, "GetAdditionalField", command, "FieldName", "FieldId");
    }

    public static void FillReportField(CheckBoxList CheckBoxList, int CustomerId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        CDatabase.BindControls(CheckBoxList, "GetReportField", command, "FieldName", "FieldId");
    }
    // ----------- For New change - Job move delivery detail to PCD
    public static int AddDeliveryToPCD(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPCDToBackOfficeNew", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    //------------------------------------

    #region HSN/SAC DETAILS

    public static DataSet GetHSNSacDetails()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("FR_GetSacWsTaxDetails", "command");
    }

    public static int DeleteSACDetail(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_delSacWsTaxDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddHSNSacDetail(string SACCode, decimal CGST, decimal SGST, decimal IGST, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@SacNo", SqlDbType.NVarChar).Value = SACCode;
        command.Parameters.Add("@CGST", SqlDbType.Decimal).Value = CGST;
        command.Parameters.Add("@SGST", SqlDbType.Decimal).Value = SGST;
        command.Parameters.Add("@IGST", SqlDbType.Decimal).Value = IGST;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insSacWsTaxDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateHSNSacDetail(int lid, decimal CGST, decimal SGST, decimal IGST, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@CGST", SqlDbType.Decimal).Value = CGST;
        command.Parameters.Add("@SGST", SqlDbType.Decimal).Value = SGST;
        command.Parameters.Add("@IGST", SqlDbType.Decimal).Value = IGST;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_updSacWsTaxDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetSacDetailAsPerCharge(int ItemId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@InvoiceItemId", SqlDbType.Int).Value = ItemId;
        return CDatabase.GetDataSet("FR_GetSacDetailAsPerCharge", command);
    }

    public static DataSet GetHSNSacRateDetails(int EnqId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        return CDatabase.GetDataSet("FR_GetHSNSacRateDetails", command);
    }

    public static int AddInvoiceFieldMaster(string FieldName, string strHeader, int FieldUnit, bool isTaxable, string strSACCode,
       decimal TaxRate, string Remark, int UserId, int AirSacId, int SeaSacId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FieldName", SqlDbType.NVarChar).Value = FieldName;
        command.Parameters.Add("@ReportHeader", SqlDbType.NVarChar).Value = strHeader;
        command.Parameters.Add("@UnitMeasurId", SqlDbType.Int).Value = FieldUnit;
        command.Parameters.Add("@SACCode", SqlDbType.NVarChar).Value = strSACCode;
        command.Parameters.Add("@IsTaxable", SqlDbType.Bit).Value = isTaxable;
        command.Parameters.Add("@AirSacId", SqlDbType.Int).Value = AirSacId;
        command.Parameters.Add("@SeaSacId", SqlDbType.Int).Value = SeaSacId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        // if (TaxRate != "")
        // {
        command.Parameters.Add("@TaxRate", SqlDbType.Decimal).Value = TaxRate;
        //}

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insInvoiceFieldMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateInvoiceFieldMaster(int FieldId, string FieldName, string strHeader, int FieldUnit, bool isTaxable,
        string strSACCode, decimal TaxRate, string Remark, int UserId, int AirSacId, int SeaSacId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@FieldName", SqlDbType.NVarChar).Value = FieldName;
        command.Parameters.Add("@ReportHeader", SqlDbType.NVarChar).Value = strHeader;
        command.Parameters.Add("@UnitMeasurId", SqlDbType.Int).Value = FieldUnit;
        command.Parameters.Add("@SACCode", SqlDbType.NVarChar).Value = strSACCode;
        command.Parameters.Add("@IsTaxable", SqlDbType.Bit).Value = isTaxable;
        command.Parameters.Add("@AirSacId", SqlDbType.Int).Value = AirSacId;
        command.Parameters.Add("@SeaSacId", SqlDbType.Int).Value = SeaSacId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        //if (TaxRate != "")
        //{
        command.Parameters.Add("@TaxRate", SqlDbType.Decimal).Value = TaxRate;
        //}
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updInvoiceFieldMS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCANInvoiceDetail(int EnqID, int InvoiceItemId, int UnitOfmeasureId, string InvoiceRate, int CurrencyId, string ExchangeRate,
     string strMinUnit, string strMinAmount, bool IsTaxable, string TaxAmount, string Amount, string TotalAmount, string TaxPercentage, int lUser,
     decimal CGstTax, decimal CGstTaxAmt, decimal SGstTax, decimal SGstTaxAmt, decimal IGstTax, decimal IGstTaxAmt,bool IsCAN)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@InvoiceItemId", SqlDbType.Int).Value = InvoiceItemId;
        command.Parameters.Add("@UOMId", SqlDbType.Int).Value = UnitOfmeasureId;
        command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = InvoiceRate;
        command.Parameters.Add("@CurrencyId", SqlDbType.Int).Value = CurrencyId;
        command.Parameters.Add("@ExchangeRate", SqlDbType.Decimal).Value = ExchangeRate;
        command.Parameters.Add("@MinUnit", SqlDbType.Decimal).Value = strMinUnit;
        command.Parameters.Add("@MinAmount", SqlDbType.Decimal).Value = strMinAmount;
        command.Parameters.Add("@TaxApplicable", SqlDbType.Bit).Value = IsTaxable;
        command.Parameters.Add("@TaxPercentage", SqlDbType.Decimal).Value = TaxPercentage;
        command.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = TaxAmount;
        command.Parameters.Add("@CGstTax", SqlDbType.Decimal).Value = CGstTax;
        command.Parameters.Add("@CGstTaxAmt", SqlDbType.Decimal).Value = CGstTaxAmt;
        command.Parameters.Add("@SGstTax", SqlDbType.Decimal).Value = SGstTax;
        command.Parameters.Add("@SGstTaxAmt", SqlDbType.Decimal).Value = SGstTaxAmt;
        command.Parameters.Add("@IGstTax", SqlDbType.Decimal).Value = IGstTax;
        command.Parameters.Add("@IGstTaxAmt", SqlDbType.Decimal).Value = IGstTaxAmt;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;
        command.Parameters.Add("@IsCAN", SqlDbType.Bit).Value = IsCAN;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insCANInvoiceDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCANInvoiceDetail(int InvoiceId, decimal Rate, string ExchangeRate, string TaxAmount, string Amount, string TotalAmount, int lUser,
         decimal CGstTax, decimal CGstTaxAmt, decimal SGstTax, decimal SGstTaxAmt, decimal IGstTax, decimal IGstTaxAmt)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = InvoiceId;
        command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Rate;
        command.Parameters.Add("@ExchangeRate", SqlDbType.Decimal).Value = ExchangeRate;
        command.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = TaxAmount;
        command.Parameters.Add("@CGstTax", SqlDbType.Decimal).Value = CGstTax;
        command.Parameters.Add("@CGstTaxAmt", SqlDbType.Decimal).Value = CGstTaxAmt;
        command.Parameters.Add("@SGstTax", SqlDbType.Decimal).Value = SGstTax;
        command.Parameters.Add("@SGstTaxAmt", SqlDbType.Decimal).Value = SGstTaxAmt;
        command.Parameters.Add("@IGstTax", SqlDbType.Decimal).Value = IGstTax;
        command.Parameters.Add("@IGstTaxAmt", SqlDbType.Decimal).Value = IGstTaxAmt;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updCANInvoiceDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetStateCodeForLocDetail(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("FR_GetStateCodeForLocDetail", command);
    }

    public static DataSet GetCANInvoiceDetailAsPerLid(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("FOP_GetCANInvoiceDetailAsPerLid", command);
    }

    public static DataSet GetCANPercentDetail(int EnqId, int InvoiceItemId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@InvoiceItemId", SqlDbType.Int).Value = InvoiceItemId;
        return CDatabase.GetDataSet("FOP_GetCANPercentDetail", command);
    }

    public static int UpdateCANPercentDetail(int lid, decimal InvoiceTotal)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@InvoiceTotal", SqlDbType.Decimal).Value = InvoiceTotal;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updCANPercentDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    //public static int AddCANInvoiceDetail(int EnqID, int InvoiceItemId, int UnitOfmeasureId, string InvoiceRate, int CurrencyId, string ExchangeRate,
    //       string strMinUnit, string strMinAmount, bool IsTaxable, string TaxAmount, string Amount, string TotalAmount, string TaxPercentage, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();
    //    string SPresult = "";

    //    command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
    //    command.Parameters.Add("@InvoiceItemId", SqlDbType.Int).Value = InvoiceItemId;
    //    command.Parameters.Add("@UOMId", SqlDbType.Int).Value = UnitOfmeasureId;
    //    command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = InvoiceRate;
    //    command.Parameters.Add("@CurrencyId", SqlDbType.Int).Value = CurrencyId;
    //    command.Parameters.Add("@ExchangeRate", SqlDbType.Decimal).Value = ExchangeRate;
    //    command.Parameters.Add("@MinUnit", SqlDbType.Decimal).Value = strMinUnit;
    //    command.Parameters.Add("@MinAmount", SqlDbType.Decimal).Value = strMinAmount;
    //    command.Parameters.Add("@TaxApplicable", SqlDbType.Bit).Value = IsTaxable;
    //    command.Parameters.Add("@TaxPercentage", SqlDbType.Decimal).Value = TaxPercentage;
    //    command.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = TaxAmount;
    //    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
    //    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;

    //    command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

    //    command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("FOP_insCANInvoiceDetail", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    #region TASK REMINDER

    public static int AddTaskReminder(int CategoryId, int NotificationModeId, DateTime DueDate, int RepeatMonth, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@NotificationModeId", SqlDbType.Int).Value = NotificationModeId;
        command.Parameters.Add("@DueDate", SqlDbType.Date).Value = DueDate;
        command.Parameters.Add("@RepeatMonth", SqlDbType.Int).Value = RepeatMonth;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TS_insTaskReminder", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTaskReminder(int lUser)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("TS_GetTaskReminder", command);
    }

    public static int UpdateReminderStatus(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TS_StopReminder", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteReminder(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TS_delReminder", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetTaskReminderAsPerLid(int lid)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("TS_GetTaskReminderAsPerLid", command);
    }

    public static int UpdateTaskReminder(int lid, int CategoryId, DateTime DueDate, int RepeatMonth, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        command.Parameters.Add("@DueDate", SqlDbType.Date).Value = DueDate;
        command.Parameters.Add("@RepeatMonth", SqlDbType.Int).Value = RepeatMonth;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TS_updTaskReminder", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddReminderUser(int ReminderId, int UserId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ReminderId", SqlDbType.Int).Value = ReminderId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TS_insReminderUser", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetReminderUsers(int ReminderId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ReminderId", SqlDbType.Int).Value = ReminderId;
        return CDatabase.GetDataSet("TS_GetReminderUser", command);
    }

    public static int DeleteReminderUser(int lid)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TS_delReminderUser", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }


    #endregion

    #region MiscellaneousCustomer

    public static int AddMiscellaneousCustomer(int CustomerID, int BranchId, string ActivityDetail, int priorityId, int EmpID, int ActivityTypeId,
             string Subject, string ContactPerson, DateTime StartDate, string strCustFilePath, int lUser, int lUserType)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@PriorityId", SqlDbType.NVarChar).Value = priorityId;
        command.Parameters.Add("@ActivityDetail", SqlDbType.NVarChar).Value = ActivityDetail;
        command.Parameters.Add("@EmpID", SqlDbType.Int).Value = EmpID;
        command.Parameters.Add("@ActivityTypeId", SqlDbType.Int).Value = ActivityTypeId;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
        command.Parameters.Add("@CustFilePath", SqlDbType.NVarChar).Value = strCustFilePath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@lUserType", SqlDbType.Int).Value = lUserType;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BS_insMiscellaneousCustomerTask", command, "@OutPut");
        return Convert.ToInt32(SPresult);


    }

    public static int AddCustomerTaskBabajiToEmployee(int CustomerID, int BranchId, bool IsBillabale, int JobID, string ActivityDetail,
       int priorityId, int OperatioMMSId, int EmpID, int ActivityTypeId,
         string Subject, string ContactPerson, DateTime StartDate, DateTime EstimatDate, string strCustFilePath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerID;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillabale;
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;
        // command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@PriorityId", SqlDbType.NVarChar).Value = priorityId;
        command.Parameters.Add("@OperatioMMSId", SqlDbType.NVarChar).Value = OperatioMMSId;
        command.Parameters.Add("@ActivityDetail", SqlDbType.NVarChar).Value = ActivityDetail;
        command.Parameters.Add("@EmpID", SqlDbType.Int).Value = EmpID;
        command.Parameters.Add("@ActivityTypeId", SqlDbType.Int).Value = ActivityTypeId;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDate;
        command.Parameters.Add("@EstimateDate", SqlDbType.DateTime).Value = EstimatDate;
        command.Parameters.Add("@CustFilePath", SqlDbType.NVarChar).Value = strCustFilePath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BS_insCustomerTaskBabajiToEmployee", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static void FillBranchByjobNo(DropDownList DropDown, int BranchId, int Customerid, int OperationMMSID, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@Customerid", SqlDbType.Int).Value = Customerid;
        command.Parameters.Add("@OperationMMSID", SqlDbType.Int).Value = OperationMMSID;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        CDatabase.BindControls(DropDown, "GetBaBajiBranchByJobID", command, "JobRefNo", "lid");
    }

    public static void FillOperationByjobNo(DropDownList DropDown, int BranchId, int Customerid, int OperationMMSID, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@Customerid", SqlDbType.Int).Value = Customerid;
        command.Parameters.Add("@OperationMMSID", SqlDbType.Int).Value = OperationMMSID;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        CDatabase.BindControls(DropDown, "GetBaBajiBranchByJobID", command, "JobRefNo", "lid");
    }


    public static DataSet EmployeeIdByEmailId(int EmployyeeId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = EmployyeeId;

        return CDatabase.GetDataSet("GetEmployeeByEmailID", command);
    }

    public static int ADDMiscellaneousCustomerTaskSummary(int TaskID, bool IsBillabale, int Jobid, int OperatioMMSId, DateTime EstimateDate, string FollowuUpdate, int Status, DateTime follouptDate,
         string strFilePath, bool isApprove, int LUsere)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TaskID", SqlDbType.Int).Value = TaskID;
        command.Parameters.Add("@IsBillable", SqlDbType.Bit).Value = IsBillabale;
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = Jobid;
        command.Parameters.Add("@OperatioMMSId", SqlDbType.Int).Value = OperatioMMSId;
        command.Parameters.Add("@EstimateDate", SqlDbType.NVarChar).Value = EstimateDate;
        command.Parameters.Add("@FollowuUpdate", SqlDbType.NVarChar).Value = FollowuUpdate;
        command.Parameters.Add("@StatusID", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@follouptDate", SqlDbType.DateTime).Value = follouptDate;
        command.Parameters.Add("@strFilePath", SqlDbType.NVarChar).Value = strFilePath;
        command.Parameters.Add("@IsApprove", SqlDbType.Bit).Value = isApprove;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = LUsere;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insCustomerTaskSummary", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView GETMiscellanceCustomerDetail(int customID)
    {

        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customID;
        return CDatabase.GetDataView("BS_GetMiscellaneousCustomerTaskByid", cmd);
    }

    public static void FillCustTaskStatus(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid,sName FROM BS_CustTaskStatusMS  ORDER by sName", "sName", "lid");
    }

    public static DataSet GETCustomerComplitedTaskById(int customID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customID;

        return CDatabase.GetDataSet("BS_GetCustomerCompletedTaskById", command);

    }

    public static void FillJobByBranchID(DropDownList DropDown, int BranchID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchID;
        CDatabase.BindControls(DropDown, "GetJobNoByBranchID", command, "JobRefNo", "lid");
    }

    #endregion

    public static DataSet GetDocSetByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("BS_GetDocSetByJobId", command);
    }

    public static DataSet GetCustomSetByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("BS_GetCustomSetByJobId", command);
    }

    public static int GetCustomDocSetCount(int JobId, int Type)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Type", SqlDbType.Int).Value = Type;
        return Convert.ToInt32(CDatabase.GetSPCount("BS_GetCustomDocSetCount", command));
    }

    public static int AddHoldBillingAdvice(int JobId, string Remark, string RejectType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@RejectType", SqlDbType.NVarChar).Value = RejectType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insHoldBillingAdvice", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #region  Dispatch Report

    public static DataSet GetDispatchReport(DateTime From, DateTime To)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;

        return CDatabase.GetDataSet("BS_GetDispatchReportByDate", command);
    }

    public static DataSet GetDispatchDetailReport(DateTime From, DateTime To)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;

        return CDatabase.GetDataSet("BS_GetDispatchDetailReport", command);
    }

    public static DataSet GetCountDispatchReport(DateTime From, DateTime To)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;

        return CDatabase.GetDataSet("BS_GetCountDispatchReportByDate", command);
    }

    public static DataSet GetTodaysDispatchRpt(DateTime From, DateTime To)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;

        return CDatabase.GetDataSet("BS_GetTodayDispatchRpt", command);
    }
    public static DataSet GetTodaysDispatchDetailRpt(DateTime From, DateTime To)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;

        return CDatabase.GetDataSet("BS_GetTodayDispatchDetailRpt", command);
    }
    public static DataSet GetTodaysCountDispatchRpt(DateTime From, DateTime To)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@FromDt", SqlDbType.Date).Value = From;
        command.Parameters.Add("@ToDt", SqlDbType.Date).Value = To;

        return CDatabase.GetDataSet("BS_GetTodaysCountDispatchRpt", command);
    }
    #endregion

    #region Vendor Invoice

    public static string AddVendorInvoice(string strCompanyCode, int BabajiBranchID, string strDivisionCode, string strJobRefNO,
        string VendorName, string VendorCode, string GSTNo, int VendorCategoryID, string InvoiceNo, DateTime InvoiceDate,
        Decimal InvoicexAmount, bool IsIGSTApplicable, Decimal GSTRate, Decimal GSTAmount, decimal TotalAmount,
        string strPaymentTerms, bool IsMSME, string strInvoiceFilePath, string strMSMEFilePath, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CompanyCode", SqlDbType.NVarChar).Value = strCompanyCode;
        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = BabajiBranchID;
        command.Parameters.Add("@DivisionCode", SqlDbType.NVarChar).Value = strDivisionCode;
        command.Parameters.Add("@JobRefNO", SqlDbType.NVarChar).Value = strJobRefNO;


        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCode", SqlDbType.NVarChar).Value = VendorCode;
        command.Parameters.Add("@VendorGSTNo", SqlDbType.NVarChar).Value = GSTNo;
        command.Parameters.Add("@VendorCategoryID", SqlDbType.Int).Value = VendorCategoryID;

        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;
        command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        command.Parameters.Add("@InvoiceAmount", SqlDbType.Decimal).Value = InvoicexAmount;

        command.Parameters.Add("@IGSTApplicable", SqlDbType.Bit).Value = IsIGSTApplicable;
        command.Parameters.Add("@GSTRate", SqlDbType.Decimal).Value = GSTRate;
        command.Parameters.Add("@GSTAmount", SqlDbType.Decimal).Value = GSTAmount;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;

        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = strPaymentTerms;
        command.Parameters.Add("@IsMSME", SqlDbType.Bit).Value = IsMSME;
        command.Parameters.Add("@InvoiceFilePath", SqlDbType.NVarChar).Value = strInvoiceFilePath;
        command.Parameters.Add("@MSMEFilePath", SqlDbType.NVarChar).Value = strMSMEFilePath;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceDetail", command, "@OutPut");

        return SPresult;
    }

    public static int UpdateVendorInvoiceCopy(string TokanNo, string FilePath)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@TokanID", SqlDbType.NVarChar).Value = TokanNo;
        command.Parameters.Add("@InvoiceCopyPath", SqlDbType.NVarChar).Value = FilePath;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_updInvoicePath", command, "@OutPut");

        return Convert.ToInt16(SPresult);
    }

    public static int UpdateInvoiceForward(int TokanId, int CurrentStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@TokanId", SqlDbType.Int).Value = TokanId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = CurrentStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceForward", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateInvoiceReceived(int TokanId, int CurrentStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@TokanId", SqlDbType.Int).Value = TokanId;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = CurrentStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insInvoiceReceived", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateJobFundRequestStatus(int JobId, bool IsFundAllowed, int ModuleId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsFunAllowed", SqlDbType.Bit).Value = IsFundAllowed;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_insAllowFundRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    #endregion

    #region Equipment

    public static int AddVehicleTravelLog(int VehicleID, int LogID, DateTime dtLogDate, string strDriver, string strEmployee, string strLocation,
        string strOutTime, string strInTime, string strOpenReading, string strCloseReading, string strFuel, string strAmount, string strFuelType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@VehicleID", SqlDbType.NVarChar).Value = VehicleID;
        command.Parameters.Add("@TravelLogID", SqlDbType.NVarChar).Value = LogID;
        command.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = dtLogDate;
        command.Parameters.Add("@DriverName", SqlDbType.NVarChar).Value = strDriver;
        command.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = strEmployee;
        command.Parameters.Add("@LocationTo", SqlDbType.NVarChar).Value = strLocation;
        command.Parameters.Add("@InTime", SqlDbType.NVarChar).Value = strInTime;
        command.Parameters.Add("@OutTime", SqlDbType.NVarChar).Value = strOutTime;
        command.Parameters.Add("@OpenReading", SqlDbType.NVarChar).Value = strOpenReading;
        command.Parameters.Add("@CloseReading", SqlDbType.NVarChar).Value = strCloseReading;
        command.Parameters.Add("@FuelLiter", SqlDbType.NVarChar).Value = strFuel;
        command.Parameters.Add("@FuelAmount", SqlDbType.NVarChar).Value = strAmount;
        command.Parameters.Add("@FuelType", SqlDbType.NVarChar).Value = strFuelType;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insVehicleDailyLog", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddEquipmentDocument(int VehicleID, string DocTypeName, int DocTypeID, DateTime dtValidFrom,
            DateTime dtValidTill, int RenewalMonth, string DocPath, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@VehicleID", SqlDbType.NVarChar).Value = VehicleID;
        command.Parameters.Add("@DocTypeName", SqlDbType.NVarChar).Value = DocTypeName;
        command.Parameters.Add("@DocTypeID", SqlDbType.Int).Value = DocTypeID;

        if (dtValidFrom != DateTime.MinValue)
            command.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = dtValidFrom;

        if (dtValidTill != DateTime.MinValue)
            command.Parameters.Add("@ValidTill", SqlDbType.DateTime).Value = dtValidTill;

        command.Parameters.Add("@ValidityMonth", SqlDbType.Int).Value = RenewalMonth;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_insEquipmentDoc", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int DeleteEquipmentDocument(int DocID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@DocID", SqlDbType.Int).Value = DocID;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_delEquipmentDoc", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    #endregion

    public static DataSet GetReportForBillDispatch(int UserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataSet("rpt_GetReceivePCDToFinalDispatch", command);
    }

    #region customer side developement

    public static DataSet GetCustomerPCDBillDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobID", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetCustBillDispatchDetail", command);
    }

    public static DataSet GetCustomerUserDetails(int CustomerId, int @CustomerUserId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;

        return CDatabase.GetDataSet("BS_GetCustomerUserDetails", command);
    }

    public static DataSet FillCustPlantDetail(int CustomerId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;

        return CDatabase.GetDataSet("BS_GetCustomerPlant", cmd);
    }

    public static int AddCustomerContactDetails(int CustomerId, int CustPlant, string UserName, string EmailId, int DutyRequest, int ChecklistAppr,
      int DispatchDetail, int PCADispatch, int BillingDispatch, int lUserId)

    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        cmd.Parameters.Add("@CustPlant", SqlDbType.Int).Value = CustPlant;
        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
        cmd.Parameters.Add("@EmailId", SqlDbType.NVarChar).Value = EmailId;
        //cmd.Parameters.Add("@NotificationTypeId", SqlDbType.Int).Value = NotificationTypeId;
        cmd.Parameters.Add("@DutyRequest", SqlDbType.Int).Value = DutyRequest;
        cmd.Parameters.Add("@ChecklistAppr", SqlDbType.Int).Value = ChecklistAppr;
        cmd.Parameters.Add("@DispatchDetail", SqlDbType.Int).Value = DispatchDetail;
        cmd.Parameters.Add("@PCADispatch", SqlDbType.Int).Value = PCADispatch;
        cmd.Parameters.Add("@BillingDispatch", SqlDbType.Int).Value = BillingDispatch;
        cmd.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insCustomerContactDetail", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetJobDetailByCustomerUserId(int CustomerUserId, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataSet("GetJobDetailByCustomerUserId", command);
    }

    public static int UpdateFieldMS(int lid, int lOrder)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lOrder", SqlDbType.Int).Value = lOrder;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updBS_Cust_FieldMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int InsertClearanceDetails(int FieldId, int lOrder, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@lOrder", SqlDbType.Int).Value = lOrder;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insBS_Cust_UCDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetClearanceOrderDetails(int lUser)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("GetBS_CustUCDetails", command);
    }

    public static int DeleteClearanceDetails(int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("delBS_Cust_UCDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetFilterResult(int CustUserId, int FinYearId, int FilterColumnId, int FilterGroupId, string Filter)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@FilterColumnId", SqlDbType.Int).Value = FilterColumnId;
        command.Parameters.Add("@FilterGroupId", SqlDbType.Int).Value = FilterGroupId;
        command.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = Filter;
        return CDatabase.GetDataSet("GetBS_CustFilterResult", command);
    }

    #endregion

    #region Frequently Asked Questions


    public static DataSet ServiceContactUpdateDeatil(int serviceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@serviceid", SqlDbType.Int).Value = serviceId;

        return CDatabase.GetDataSet("FAQ_GetFAQIDContactDetail", command);
    }

    public static DataSet GetServiceContactDeatil(int serviceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@serviceid", SqlDbType.Int).Value = serviceId;

        return CDatabase.GetDataSet("FAQ_GetserviceIDToContactDetail", command);
    }

    public static DataSet GetServiceByAllFaqList(int serviceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@serviceid", SqlDbType.Int).Value = serviceId;


        return CDatabase.GetDataSet("FAQ_GetserviceIDToallList", command);
    }

    public static DataSet GetServiceByAllFaqDetails(int serviceId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@serviceid", SqlDbType.Int).Value = serviceId;


        return CDatabase.GetDataSet("FAQ_GetserviceIDToAllDetail", command);
    }

    public static int UpdateFAQContactDetail(int lid, int faqid, string strName, int Branch, string strPhoneNo, int contacttype, string strEmailId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@FaqId", SqlDbType.Int).Value = faqid;
        command.Parameters.Add("@ContactPerName", SqlDbType.NVarChar).Value = strName;
        command.Parameters.Add("@BrachID", SqlDbType.Int).Value = Branch;
        command.Parameters.Add("@ContactPerPhonNo", SqlDbType.NVarChar).Value = strPhoneNo;
        command.Parameters.Add("@ContactPerEmailid", SqlDbType.NVarChar).Value = strEmailId;
        command.Parameters.Add("@ContactType", SqlDbType.Int).Value = contacttype;
        command.Parameters.Add("@IUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FAQ_updContactDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int UpdateFAQDetail(int Faqid, string Title, string discription, string strfuDocPath,
        string strfuDocForm1Path, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@Faqid", SqlDbType.Int).Value = Faqid;
        command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Title;
        // command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = strSubject;
        command.Parameters.Add("@Discription", SqlDbType.NVarChar).Value = discription;
        if (strfuDocPath != "")
        {
            command.Parameters.Add("@fuDoc1Path", SqlDbType.NVarChar).Value = strfuDocPath;
        }
        //if (strDocPath2 != "")
        //{
        //    command.Parameters.Add("@fuDoc2Path", SqlDbType.NVarChar).Value = strDocPath2;
        //}
        //if (strDoc3Path != "")
        //{
        //    command.Parameters.Add("@fuDoc3Path", SqlDbType.NVarChar).Value = strDoc3Path;
        //}
        if (strfuDocForm1Path != "")
        {
            command.Parameters.Add("@fuDocForm1", SqlDbType.NVarChar).Value = strfuDocForm1Path;
        }
        //if (strfuDocForm2Path != "")
        //{
        //    command.Parameters.Add("@fuDocForm2", SqlDbType.NVarChar).Value = strfuDocForm2Path;
        //}
        //if (strfuDocForm3Path != "")
        //{
        //    command.Parameters.Add("@fuDocForm3", SqlDbType.NVarChar).Value = strfuDocForm3Path;
        //}
        command.Parameters.Add("@IUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FAQ_updfaqDetail", command, "@Output");
        return Convert.ToInt32(SPresult);

    }


    public static int AddFAQDescription(int serviceid, string Tital, string discription, string strfuDoc1Path,
        string strfuDocForm1Path, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = serviceid;
        command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Tital;
        // command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = Subject;
        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = discription;
        command.Parameters.Add("@fuDoc1Path", SqlDbType.NVarChar).Value = strfuDoc1Path;
        //command.Parameters.Add("@fuDoc2Path", SqlDbType.NVarChar).Value = strfuDoc2Path;
        //command.Parameters.Add("@fuDoc3Path", SqlDbType.NVarChar).Value = strfuDoc3Path;
        command.Parameters.Add("@fuDocForm1Path", SqlDbType.NVarChar).Value = strfuDocForm1Path;
        //command.Parameters.Add("@fuDocForm2Path", SqlDbType.NVarChar).Value = strfuDocForm2Path;
        //command.Parameters.Add("@fuDocForm3Path", SqlDbType.NVarChar).Value = strfuDocForm3Path;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FAQ_insFAQDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFAQContactPersonDeatil(int FAQID, string strName, int Branch, string strPhoneNo, string strEmailID, int ContactType, int UserID)
    {

        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@FAQID", SqlDbType.Int).Value = FAQID;
        command.Parameters.Add("@ContactPerName", SqlDbType.NVarChar).Value = strName;
        command.Parameters.Add("@BrachID", SqlDbType.Int).Value = Branch;
        command.Parameters.Add("@ContactPerPhonNo", SqlDbType.NVarChar).Value = strPhoneNo;
        command.Parameters.Add("@ContactPerEmailid", SqlDbType.NVarChar).Value = strEmailID;
        command.Parameters.Add("@ContactType", SqlDbType.Int).Value = ContactType;
        command.Parameters.Add("@IUser", SqlDbType.Int).Value = UserID;
        command.Parameters.Add("@outPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FAQ_insContactPersonDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);

    }

    public static void FillServicess(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId, sName AS Servicess FROM BS_ServicesMS WHERE  bDel=0 Order BY sName", "Servicess", "lId");
    }
    #endregion

    #region ADDITIONAL JOBS

    public static DataSet GetAdditionalJobDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("AD_GetAdditionalJobById", command);
    }

    public static int UpdateJobRemark(int JobID, string strRemarks, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.BigInt).Value = JobID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemarks;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AD_updJobRemark", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet FillExpenseDetails(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("AD_GetBJVDetails", command);
    }

    public static int UpdateBillingStatus(int JobID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.BigInt).Value = JobID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AD_updBillingStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddAddtnlDocs(int JobId, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Docpath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AD_insAddtnlDocument", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int DeleteAddtnlDocs(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AD_delAddtnlDocument", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddDraftInvoiceToTyping(int JobID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.BigInt).Value = JobID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AD_insDraftInvoiceToTyping", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddFinalInvoiceToDispatch(int JobID, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.BigInt).Value = JobID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AD_insFinalInvoiceToDispatch", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    public static int UpdateDOPlanning(int JobId, DateTime ConsolDoProcessDate, DateTime FinalDOProcessDate, int DivisionId,
    int DeliveryTypeId, string DOPlanRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (ConsolDoProcessDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ConsolDoProcessDate", SqlDbType.DateTime).Value = ConsolDoProcessDate;
        }

        if (FinalDOProcessDate != DateTime.MinValue)
        {
            command.Parameters.Add("@FinalDOProcessDate", SqlDbType.DateTime).Value = FinalDOProcessDate;
        }

        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@DeliveryTypeId", SqlDbType.Int).Value = DeliveryTypeId;
        command.Parameters.Add("@DOPlanRemark", SqlDbType.NVarChar).Value = DOPlanRemark;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDoPlanDetailAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #region Out of charge

    //public static int AddOutOfChargeExamineDetail(int JobId, DateTime ExamineDate, DateTime OutOfChargeDate, bool bExamineStatus, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";

    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

    //    if (ExamineDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@ExamineDate", SqlDbType.DateTime).Value = ExamineDate;
    //    }
    //    if (OutOfChargeDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@OutOfChargeDate", SqlDbType.DateTime).Value = OutOfChargeDate;
    //    }

    //    command.Parameters.Add("@ExamineStatus", SqlDbType.Bit).Value = bExamineStatus;

    //    command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
    //    command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("insOutOfChargeExamineDetail", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int AddOutOfChargeExamineDetail(int JobId, DateTime ExamineDate, DateTime OutOfChargeDate, bool bExamineStatus, bool BEPrint, string OOCRemark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        if (ExamineDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ExamineDate", SqlDbType.DateTime).Value = ExamineDate;
        }
        if (OutOfChargeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OutOfChargeDate", SqlDbType.DateTime).Value = OutOfChargeDate;
        }

        command.Parameters.Add("@ExamineStatus", SqlDbType.Bit).Value = bExamineStatus;
        command.Parameters.Add("@BEPrint", SqlDbType.Bit).Value = BEPrint;
        command.Parameters.Add("@OOCRemark", SqlDbType.NVarChar).Value = OOCRemark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insOutOfChargeExamineDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateOutOfChargeStatus(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BS_updOutOfChargeStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView GetOutOfChargeByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("GetOutOfChargeByJobId", cmd);
    }

    public static int UpdateOOCYardExamine(int JobId, bool OOCStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@IsOCCYard", SqlDbType.Bit).Value = OOCStatus;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("updOOCYardExamine", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    public static DataSet GetJobArchiveOrderDetails(int lUser)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("GetBS_CustJADetails", command);
    }

    public static int DeleteJAOrderDetails(int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("delBS_Cust_JADetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int InsertJAOrderDetails(int FieldId, int lOrder, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@FieldId", SqlDbType.Int).Value = FieldId;
        command.Parameters.Add("@lOrder", SqlDbType.Int).Value = lOrder;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("insBS_Cust_JADetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetJArchiveFilterResult(int CustUserId, int FinYearId, int FilterColumnId, int FilterGroupId, string Filter)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@FilterColumnId", SqlDbType.Int).Value = FilterColumnId;
        command.Parameters.Add("@FilterGroupId", SqlDbType.Int).Value = FilterGroupId;
        command.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = Filter;
        return CDatabase.GetDataSet("GetBS_CustJAFilterResult", command);
    }

    public static DataSet StageWiseDetailsCustUser(int CustomerUserId, int FinYearId, int StageId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@StageId", SqlDbType.Int).Value = StageId;

        return CDatabase.GetDataSet("ds_StageWiseDetailsCustUser", command);
    }

    public static DataSet PortwiseJobDetailSummary(int CustomerUserId, int FinYearId, string PortId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@PortId", SqlDbType.NVarChar).Value = PortId;

        return CDatabase.GetDataSet("ds_PortwiseJobDetailSummary", command);
    }

    public static DataSet SummaryTotalShipmentAging(int CustomerUserId, int FinYearId, int AgingNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerUserId", SqlDbType.Int).Value = CustomerUserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@Aging", SqlDbType.NVarChar).Value = AgingNo;

        return CDatabase.GetDataSet("ds_AgingDaysCustUserDetails", command);
    }

    public static DataSet CustGetAdhocReportFieldGroupTest()
    {
        SqlCommand cmd = new SqlCommand();

        return CDatabase.GetDataSet("Cust_GetFieldGroup", cmd);
    }

    public static DataSet GetCustomerReportChildNode(string ParentNode)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;
        return CDatabase.GetDataSet("Cust_GetReportChildNodeByParentNode", cmd);
    }

    public static DataView GetCustAdhocReport(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
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

        return CDatabase.GetDataView("rptAdHocReportCust", cmd);
    }

    public static int DeleteCustAdhocReport(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUpdUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delCustAdhocReport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }



    public static DataSet GetShippingCustMailIDs(string strShippingName)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ShippingName", SqlDbType.NVarChar).Value = strShippingName;

        return CDatabase.GetDataSet("GetShippingCustMailIDs", command);
    }

    public static DataSet GetShippingDetailsByCode(string SCode)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@SCode", SqlDbType.NVarChar).Value = SCode;

        return CDatabase.GetDataSet("GetShippingDetailsByCode", command);
    }
    public static int UpdateShipContactDetails(int lid, string ContactName, string EmailId, string MobileNo, string ContactNo, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ContactName;
        command.Parameters.Add("@EmailId", SqlDbType.NVarChar).Value = EmailId;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("UpdateShipContactDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddShipContactDetails(string SCode, string ContactName, string EmailId, string MobileNo, string ContactNo, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@SCode", SqlDbType.NVarChar).Value = SCode;
        command.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ContactName;
        command.Parameters.Add("@EmailId", SqlDbType.NVarChar).Value = EmailId;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insShipContactDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteShippingDetails(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("delShippingDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateShipMaster(int lid, string ShippingLineName, string ShippingLineCode, string Address, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@ShippingLineName", SqlDbType.NVarChar).Value = ShippingLineName;
        command.Parameters.Add("@ShippingLineCode", SqlDbType.NVarChar).Value = ShippingLineCode;

        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_updShipppingLine", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static string GetNextScodeAutoGenerate() //int FinYearId,
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("GetNextScodeAutoGenerate", command, "@OutPut");

        return Convert.ToString(SPresult);
    }

    public static int AddShippingCompanyMaster(string strSCode, string CompName, string ShippingLIneCode, string CompAddress, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@strSCode", SqlDbType.NVarChar).Value = strSCode;
        command.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = CompName;
        command.Parameters.Add("@ShippingLIneCode", SqlDbType.NVarChar).Value = ShippingLIneCode;

        command.Parameters.Add("@CompAddress", SqlDbType.NVarChar).Value = CompAddress;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insShippingLine", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #region CRM REPORTING

    public static int AddUserReporting(int UserId, int ReportingUserId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@ReportingUserId", SqlDbType.Int).Value = ReportingUserId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("CRM_insUserReporting", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int RemoveUserReporting(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("CRM_updUserReporting", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Manpower Requisition Details

    public static int ManpowerReqDetail(string strDeptName, string strDeptMngrName, string strAdditReq, string strReplaceReq, string strTotalEmpReq,
            DateTime dtManpowerTargetDt, string strPosReqFor, string strPosReportTo, string strLocation, string strSalaryRange, string strMinQualification,
            string strPrevExperiance, string strResponseDuties, string strAddInformation, int FinYearId, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar).Value = strDeptName;
        cmd.Parameters.Add("@DeptMngrName", SqlDbType.NVarChar).Value = strDeptMngrName;
        cmd.Parameters.Add("@AdditReq", SqlDbType.NVarChar).Value = strAdditReq;
        cmd.Parameters.Add("@ReplaceReq", SqlDbType.NVarChar).Value = strReplaceReq;
        cmd.Parameters.Add("@TotalEmpReq", SqlDbType.NVarChar).Value = strTotalEmpReq;
        if (dtManpowerTargetDt != DateTime.MinValue)
        {
            cmd.Parameters.Add("@ManpowerTargetDt", SqlDbType.DateTime).Value = dtManpowerTargetDt;
        }
        cmd.Parameters.Add("@PosReqFor", SqlDbType.NVarChar).Value = strPosReqFor;
        cmd.Parameters.Add("@PosReportTo", SqlDbType.NVarChar).Value = strPosReportTo;
        cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = strLocation;
        cmd.Parameters.Add("@SalaryRange", SqlDbType.NVarChar).Value = strSalaryRange;
        cmd.Parameters.Add("@MinQualification", SqlDbType.NVarChar).Value = strMinQualification;
        cmd.Parameters.Add("@PrevExperiance", SqlDbType.NVarChar).Value = strPrevExperiance;
        cmd.Parameters.Add("@ResponseDuties", SqlDbType.NVarChar).Value = strResponseDuties;
        cmd.Parameters.Add("@AddInformation", SqlDbType.NVarChar).Value = strAddInformation;

        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insManpowerReq", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetManpowerDetailbyId(int lid)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;

        return CDatabase.GetDataSet("GetManpowerDetailbyId", command);
    }

    public static int AddPositionDetails(int ReqId, string PositionReq, string PositionReport, string SalRange, string MinQualif, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@ReqId", SqlDbType.Int).Value = ReqId;
        cmd.Parameters.Add("@PositionReq", SqlDbType.NVarChar).Value = PositionReq;
        cmd.Parameters.Add("@PositionReport", SqlDbType.NVarChar).Value = PositionReport;
        cmd.Parameters.Add("@SalRange", SqlDbType.NVarChar).Value = SalRange;
        cmd.Parameters.Add("@MinQualif", SqlDbType.NVarChar).Value = MinQualif;

        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insPositionDetails", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetManpowerPostDetailbyId(int lid)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;

        return CDatabase.GetDataSet("GetManpowerPostDetailbyId", command);
    }

    public static int UpdApproveStatus(int lid, int Status, string RejectReason, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@RejectReason", SqlDbType.NVarChar).Value = RejectReason;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("UpdApproveStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdRequisitionStatus(int lid, string StatusId, string Source, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@StatusId", SqlDbType.NVarChar).Value = StatusId;
        command.Parameters.Add("@Source", SqlDbType.NVarChar).Value = Source;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("UpdRequisitionStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int ApprovedMailStatusToHR(int lid)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("mailApprovedRequisition", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region LR copy Details
    public static int AddLRDetails(string strCNNo, int compId, int RegnoToPay, string strInvoiceNo, DateTime CNNoDate, DateTime InvoiceDate, string strFrom, string strTo,
            string strconsignor, string strDeliveryAddr, string strState, string strTelNo, string strVehicleNo, string strJobRefNo, string strWayBillNo, string strVehicleType,
            string strBENO, string strBLNo, string strConsigneeAddr, int FinYearId, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@compId", SqlDbType.Int).Value = compId;
        cmd.Parameters.Add("@RegnoToPay", SqlDbType.Int).Value = RegnoToPay;
        cmd.Parameters.Add("@CNNo", SqlDbType.NVarChar).Value = strCNNo;
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = strInvoiceNo;

        if (CNNoDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@CNNoDate", SqlDbType.DateTime).Value = CNNoDate;
        }
        if (InvoiceDate != DateTime.MinValue)
        {
            cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        }

        cmd.Parameters.Add("@From", SqlDbType.NVarChar).Value = strFrom;
        cmd.Parameters.Add("@To", SqlDbType.NVarChar).Value = strTo;
        cmd.Parameters.Add("@consignor", SqlDbType.NVarChar).Value = strconsignor;
        cmd.Parameters.Add("@DeliveryAddr", SqlDbType.NVarChar).Value = strDeliveryAddr;
        cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = strState;
        cmd.Parameters.Add("@TelNo", SqlDbType.NVarChar).Value = strTelNo;
        cmd.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = strVehicleNo;
        cmd.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = strJobRefNo;
        cmd.Parameters.Add("@WayBillNo", SqlDbType.NVarChar).Value = strWayBillNo;
        cmd.Parameters.Add("@VehicleType", SqlDbType.NVarChar).Value = strVehicleType;
        cmd.Parameters.Add("@BENO", SqlDbType.NVarChar).Value = strBENO;
        cmd.Parameters.Add("@BLNo", SqlDbType.NVarChar).Value = strBLNo;
        cmd.Parameters.Add("@ConsigneeAddr", SqlDbType.NVarChar).Value = strConsigneeAddr;

        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insLRCopyDetails", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddLRPackageDetails(int LRId, string Packages, string Description, string ActualWt, string Charged, int lUser)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@LRId", SqlDbType.Int).Value = LRId;
        cmd.Parameters.Add("@Packages", SqlDbType.NVarChar).Value = Packages;
        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        cmd.Parameters.Add("@ActualWt", SqlDbType.NVarChar).Value = ActualWt;
        cmd.Parameters.Add("@Charged", SqlDbType.NVarChar).Value = Charged;

        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insLRPackagesDetails", cmd, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static string GetNextCNNoForLRCopy() //int FinYearId,
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("GetNextCNNoForLRCopy", cmd, "@OutPut");

        return Convert.ToString(SPresult);
    }

    public static DataSet GetLRConsigneeAddr(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("BS_GetLRConsigneeAddr", command);
    }

    public static DataSet GetBillLRStatusCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("GetBillLRStatusCount", command);

        return dsPending;
    }

    ////////////////////////////////////////////  Billing Advice To LR Pending  ///////////////////////////////////////////////////////////

    public static int AddAdviceToLRPending(int JobId, int BillLRStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BillLRStatus", SqlDbType.Int).Value = BillLRStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insAdviceToLRPending", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddLRPendingToScrutiny(int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_inLRPendingToScrutiny", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetUploadedPCDDocument(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetUploadedPCDDocument", command);
    }

    #endregion

    #region  DO Security

    //public static int AddDOSecurityDetail(int JobId, string ChequeNo, DateTime ChequeDate, DateTime FollowupDt, DateTime ChequeCancelDt, string SecRemark, int SecStatus, int lUser)
    //{
    //    SqlCommand command = new SqlCommand();

    //    string SPresult = "";

    //    command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
    //    command.Parameters.Add("@SecChequeNo", SqlDbType.NVarChar).Value = ChequeNo;

    //    if (ChequeDate != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@SecChequeDt", SqlDbType.DateTime).Value = ChequeDate;
    //    }
    //    if (FollowupDt != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@SecFollowupDt", SqlDbType.DateTime).Value = FollowupDt;
    //    }
    //    if (ChequeCancelDt != DateTime.MinValue)
    //    {
    //        command.Parameters.Add("@SecChequeCancelDt", SqlDbType.DateTime).Value = ChequeCancelDt;
    //    }

    //    command.Parameters.Add("@SecRemark", SqlDbType.NVarChar).Value = SecRemark;
    //    command.Parameters.Add("@SecStatus", SqlDbType.Int).Value = SecStatus;

    //    command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
    //    command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

    //    SPresult = CDatabase.GetSPOutPut("insDOSecurityDetail", command, "@OutPut");

    //    return Convert.ToInt32(SPresult);
    //}

    public static int AddDOSecurityDetail(int JobId, int SecType, string ChequeNo, DateTime ChequeDate, DateTime FollowupDt, DateTime ChequeCancelDt, string SecRemark, int SecStatus, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@SecType", SqlDbType.NVarChar).Value = SecType;
        command.Parameters.Add("@SecChequeNo", SqlDbType.NVarChar).Value = ChequeNo;

        if (ChequeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SecChequeDt", SqlDbType.DateTime).Value = ChequeDate;
        }
        if (FollowupDt != DateTime.MinValue)
        {
            command.Parameters.Add("@SecFollowupDt", SqlDbType.DateTime).Value = FollowupDt;
        }
        if (ChequeCancelDt != DateTime.MinValue)
        {
            command.Parameters.Add("@SecChequeCancelDt", SqlDbType.DateTime).Value = ChequeCancelDt;
        }

        command.Parameters.Add("@SecRemark", SqlDbType.NVarChar).Value = SecRemark;
        command.Parameters.Add("@SecStatus", SqlDbType.Int).Value = SecStatus;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDOSecurityDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetContainerDetailByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetContainerDetail", command);
    }

    public static DataSet GetShippingNameByJobId(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("GetShippingNameByJobId", command);
    }

    #endregion

    public static DataSet GetUploadedDocument(int lid)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;

        return CDatabase.GetDataSet("GetUploadedDocumentForMessage", cmd);
    }

    public static int AddJobNotificationWhatsApp(int JobId, int NotificationMode, int NotificationType, string strSentTo,
           string strSubject, string strMessage, string strStatus, string UniqueRef, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModId", SqlDbType.Int).Value = NotificationMode;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = NotificationType;
        command.Parameters.Add("@SentTo", SqlDbType.NVarChar).Value = strSentTo;
        command.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = strSubject;
        command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = strMessage;
        command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = strStatus;
        command.Parameters.Add("@UniqueRef", SqlDbType.NVarChar).Value = UniqueRef;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobNotificationLog", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int CheckJobDocumentById(int JobId, int DocumentId)
    {
        // Return Values - 0 = Document Uploaded
        // Return Values - 1 = Document not Uploaded
        string result = "";
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        result = CDatabase.GetSPOutPut("CheckJobDocumentById", cmd, "@OutPut");
        return Convert.ToInt32(result);
    }

    #region   Claim Job Creation

    public static string GetNextClaimJobNo()
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BS_GetNextClaimJobNo", cmd, "@OutPut");
        return Convert.ToString(SPresult);
    }

    public static int AddClaimJobDetail(String ClaimJobNo, int BranchId, int CustomerId, int DivisionId, int PlantId, int FInYear, string Purpose, int UserID)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@ClaimJobNo", SqlDbType.NVarChar).Value = ClaimJobNo;
        cmd.Parameters.Add("@BabajiBranchId", SqlDbType.Int).Value = BranchId;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerId;
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        cmd.Parameters.Add("@FInYear", SqlDbType.Int).Value = FInYear;
        cmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Purpose;
        cmd.Parameters.Add("lUser", SqlDbType.Int).Value = UserID;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insClaimJobDetail", cmd, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Customer Bill Return
    public static DataSet GetBillReturnDetails(string BillNo, int FinYear)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = BillNo;
        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;

        return CDatabase.GetDataSet("GetBillReturnDetails", command);
    }

    public static DataSet GetBillReturnDetailBS(int BillRetlid, int JobId, int BJVlid, int FinYearId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BillRetlid", SqlDbType.Int).Value = BillRetlid;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BJVlid", SqlDbType.Int).Value = BJVlid;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        return CDatabase.GetDataSet("GetBillReturnDetailBS", command);
    }

    public static int AddBillReturnbyCust(int lid, int JobId, string BJVNo, decimal INVAmount, int Reason, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BJVlid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        //command.Parameters.Add("@BJVNo", SqlDbType.NVarChar).Value = BJVNo;
        command.Parameters.Add("@INVAmount", SqlDbType.Decimal).Value = INVAmount;
        command.Parameters.Add("@Reason", SqlDbType.Int).Value = Reason;
        command.Parameters.Add("@RemarkCust", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBillReturnByCust", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateBillReturn(int JobId, int BJVId, int BillRetLid, DateTime ChangeDate, DateTime NewDispatchDate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BJVlid", SqlDbType.Int).Value = BJVId;
        command.Parameters.Add("@BillRetLid", SqlDbType.Int).Value = BillRetLid;

        if (ChangeDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ChangesDate", SqlDbType.Date).Value = ChangeDate;
        }

        if (NewDispatchDate != DateTime.MinValue)
        {
            command.Parameters.Add("@NewDispatchDate", SqlDbType.Date).Value = NewDispatchDate;
        }
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updBillReturnByBS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int SendMailBillReturn(int lid, int JobId, int BillRetlid, int FinYearId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@BJVlid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@BillRetlid", SqlDbType.Int).Value = BillRetlid;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("mailBillReturnByCustomer", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Export Customer Module

    public static int Ex_UpdateAdhocReport(int ReportId, string ReportName, string ColumnListId, int ReportType, int CustomerId, string ConditionColumnId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@ReportId", SqlDbType.Int).Value = ReportId;
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = ConditionColumnId;
        command.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("EX_updAdhocReport", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int Ex_AddAdhocReport(string ReportName, string ColumnListId, string strConditionColumnId, int ReporType, int CustomerId, int IsCustomer, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ReportName", SqlDbType.NVarChar).Value = ReportName;
        command.Parameters.Add("@ColumnListId", SqlDbType.NVarChar).Value = ColumnListId;
        command.Parameters.Add("@ConditionListId", SqlDbType.NVarChar).Value = strConditionColumnId;
        command.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReporType;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@IsCustomer", SqlDbType.Int).Value = IsCustomer;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insAdhocReport", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet EX_Cust_GetReportChildNodeByParentNode(string ParentNode, int ReportType, int CustomerID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ParentNode", SqlDbType.NVarChar).Value = ParentNode;
        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = ReportType;
        cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
        return CDatabase.GetDataSet("EX_Cust_GetReportChildNodeByParentNode", cmd);
    }

    public static DataView EX_rptCustAdHocReport(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
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

        return CDatabase.GetDataView("EX_rptCustAdHocReport", cmd);
    }

    #endregion

    #region  Freight Export

    public static void FillShipperByCustId(DropDownList DropDown, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        CDatabase.BindControls(DropDown, "FOP_GetShipperByCustomerIdEX", command, "ShipperName", "ShipperId");
    }

    public static int AddContainerDetailExport(int EnqId, string ContainerNo, int ContainerSize, int ContainerType, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@ContainerNo", SqlDbType.NVarChar).Value = ContainerNo;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerType;
        command.Parameters.Add("@ContainerSize", SqlDbType.Int).Value = ContainerSize;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insContainerDetailExport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddFreightBookingExport(string strBookingNo, int EnqID, int FreightMode, string strCustomer, string strConsignee, string strShipper,
       string strConsigneeAddress, int ConsigneeStateID, string strGSTN, string strShipperAddress, int PortOfLoadingId, int PortOfDischargeId, int TermsId, int BranchId,
       string strAgentName, int AgentCompId, int AirLineId, int ContainerTypeId, string ContainerSubType, int Count20, int Count40, string LCLVolume, int NoOfPackages,
       int PackagesType, string strGrossWeight, string strChargeWeight, string InvoiceNo, DateTime InvoiceDate, string PONumber, int Division, int Plant, string OptionId,
       string strDescription, DateTime BookingDate, int CustId, int ShipperId, string BookingDetails, int ExportType, string CartingPoint, int CHABy, string CHAByName, string transportByName, int transportBy, int lType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@BookingNo", SqlDbType.NVarChar).Value = strBookingNo;
        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@FreightMode", SqlDbType.Int).Value = FreightMode;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = strCustomer;

        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = strConsignee;
        command.Parameters.Add("@ConsigneeStateID", SqlDbType.NVarChar).Value = ConsigneeStateID;
        command.Parameters.Add("@ConsigneeAddress", SqlDbType.NVarChar).Value = strConsigneeAddress;
        command.Parameters.Add("@ConsigneeGSTN", SqlDbType.NVarChar).Value = strGSTN;

        command.Parameters.Add("@Shipper", SqlDbType.NVarChar).Value = strShipper;
        command.Parameters.Add("@ShipperAddress", SqlDbType.NVarChar).Value = strShipperAddress;

        command.Parameters.Add("@PortOfLoadingId", SqlDbType.Int).Value = PortOfLoadingId;
        command.Parameters.Add("@PortOfDischargeId", SqlDbType.Int).Value = PortOfDischargeId;
        command.Parameters.Add("@TermsId", SqlDbType.Int).Value = TermsId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AgentName", SqlDbType.NVarChar).Value = strAgentName;
        command.Parameters.Add("@AgentCompId", SqlDbType.Int).Value = AgentCompId;
        command.Parameters.Add("@AirLineId", SqlDbType.Int).Value = AirLineId;

        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerTypeId;
        command.Parameters.Add("@ContainerSubType", SqlDbType.NVarChar).Value = ContainerSubType;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@LCLVolume", SqlDbType.NVarChar).Value = LCLVolume;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@PackageType", SqlDbType.Int).Value = PackagesType;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = strGrossWeight;
        command.Parameters.Add("@ChargeWeight", SqlDbType.NVarChar).Value = strChargeWeight;
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;

        command.Parameters.Add("@PONumber", SqlDbType.NVarChar).Value = PONumber;
        command.Parameters.Add("@CargoDescription", SqlDbType.NVarChar).Value = strDescription;
        command.Parameters.Add("@BookingDetails", SqlDbType.NVarChar).Value = BookingDetails;

        if (InvoiceDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        }

        if (BookingDate != DateTime.MinValue)
            command.Parameters.Add("@BookingDate", SqlDbType.DateTime).Value = BookingDate;

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId; // Export  
        command.Parameters.Add("@ShipId", SqlDbType.Int).Value = ShipperId; // Export  
        command.Parameters.Add("@ExportType", SqlDbType.Int).Value = ExportType; // Export  
        command.Parameters.Add("@CartingPoint", SqlDbType.NVarChar).Value = CartingPoint; // Export  
        command.Parameters.Add("@CHABy", SqlDbType.Int).Value = CHABy;   // Export          
        command.Parameters.Add("@CHAByName", SqlDbType.NVarChar).Value = CHAByName;  // Export
        command.Parameters.Add("@transportByName", SqlDbType.NVarChar).Value = transportByName;  // Export
        command.Parameters.Add("@transportBy", SqlDbType.Int).Value = transportBy;   // Export
        command.Parameters.Add("@lType", SqlDbType.Int).Value = lType;   // Export
        command.Parameters.Add("@Division", SqlDbType.Int).Value = Division;   // Export
        command.Parameters.Add("@Plant", SqlDbType.Int).Value = Plant;   // Export
        command.Parameters.Add("@OptionId", SqlDbType.Int).Value = OptionId;   // Export

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;


        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insBookingExport", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static void FillPreAlertExport(DropDownList DropDown, int lUserId)//, int TransMode)
    {
        //GetPreAlertReceived

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@TransMode", SqlDbType.Int).Value = 0;

        CDatabase.BindControls(DropDown, "GetPreAlertForFrightExport", command, "PreAlertRefNo", "lid");
    }

    //public static void FillPreAlertExport(DropDownList DropDown)//, int TransMode)
    //{
    //    //GetPreAlertReceived

    //    SqlCommand command = new SqlCommand();
    //    command.Parameters.Add("@UserId", SqlDbType.Int).Value = 1;
    //    command.Parameters.Add("@TransMode", SqlDbType.Int).Value = 0;

    //    CDatabase.BindControls(DropDown, "GetPreAlertForFrightExport", command, "PreAlertRefNo", "lid");
    //}

    public static DataView GetFreightExportJobNo(int EnqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        return CDatabase.GetDataView("GetFreightExportJobNo", cmd);
    }

    public static DataSet GetFreightExportPendingCount(int UserId, int FinYear)
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        //command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("FR_GetExportOperationCountForUser", command);

        return dsPending;
    }

    public static string GetNewExportBookingJobNo(string BranchId, string ModeId, string TypeId)
    {
        string strFRJobNo = "";
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@ModeID", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = TypeId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        strFRJobNo = CDatabase.GetSPOutPut("FOP_GetNewExportJobNo", command, "@OutPut");

        return strFRJobNo;
    }

    public static DataSet GetExpBookingDetail(int EnqId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        DataSet dsFreight = CDatabase.GetDataSet("FOP_GetExpBookingDetail", command);

        return dsFreight;
    }
    public static int AddExportOperation(int EnqID, string SBNO, DateTime SBDate, DateTime ContPickupDate, DateTime CustomDate, DateTime StuffingDate, DateTime CLPDate, string ASIBy, DateTime ASIDate, DateTime CartingDate, int ModeId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@SBNO", SqlDbType.NVarChar).Value = SBNO;

        if (SBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SBDate", SqlDbType.Date).Value = SBDate;
        }
        if (ContPickupDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ContPickupDate", SqlDbType.Date).Value = ContPickupDate;
        }
        if (CustomDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CustomDate", SqlDbType.Date).Value = CustomDate;
        }
        if (StuffingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@StuffingDate", SqlDbType.Date).Value = StuffingDate;
        }
        if (CLPDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CLPDate", SqlDbType.Date).Value = CLPDate;
        }

        command.Parameters.Add("@ASIBy", SqlDbType.NVarChar).Value = ASIBy;
        if (ASIDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ASIDate", SqlDbType.Date).Value = ASIDate;
        }
        if (CartingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CartingDate", SqlDbType.Date).Value = CartingDate;
        }

        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insExportOperation", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddExportOperation(int EnqID, string SBNO, DateTime SBDate, DateTime ContPickupDate, DateTime CustomDate, DateTime StuffingDate, DateTime CLPDate, string ASIBy, DateTime ASIDate, DateTime CartingDate, string ShipLineBookingNo, string VesselName, string PlaceOfDelivery, string CHAJobNo, int ModeId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@SBNO", SqlDbType.NVarChar).Value = SBNO;

        if (SBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SBDate", SqlDbType.Date).Value = SBDate;
        }
        if (ContPickupDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ContPickupDate", SqlDbType.Date).Value = ContPickupDate;
        }
        if (CustomDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CustomDate", SqlDbType.Date).Value = CustomDate;
        }
        if (StuffingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@StuffingDate", SqlDbType.Date).Value = StuffingDate;
        }
        if (CLPDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CLPDate", SqlDbType.Date).Value = CLPDate;
        }

        command.Parameters.Add("@ASIBy", SqlDbType.NVarChar).Value = ASIBy;
        if (ASIDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ASIDate", SqlDbType.Date).Value = ASIDate;
        }
        if (CartingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CartingDate", SqlDbType.Date).Value = CartingDate;
        }

        command.Parameters.Add("@ShipLineBookingNo", SqlDbType.NVarChar).Value = ShipLineBookingNo;
        command.Parameters.Add("@VesselName", SqlDbType.NVarChar).Value = VesselName;
        command.Parameters.Add("@PlaceOfDelivery", SqlDbType.NVarChar).Value = PlaceOfDelivery;

        command.Parameters.Add("@CHAJobNo", SqlDbType.NVarChar).Value = CHAJobNo;

        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insExportOperation", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddExportVGMForm13(int EnqID, string SBNo, DateTime SBDate, DateTime VGMDate, DateTime Form13Date, string ASIBy, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@SBNO", SqlDbType.NVarChar).Value = SBNo;

        if (SBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SBDate", SqlDbType.Date).Value = SBDate;
        }

        if (VGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VGMDate", SqlDbType.Date).Value = VGMDate;
        }
        if (Form13Date != DateTime.MinValue)
        {
            command.Parameters.Add("@Form13Date", SqlDbType.Date).Value = Form13Date;
        }
        command.Parameters.Add("@ASIBy", SqlDbType.NVarChar).Value = ASIBy;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insExporVGMForm13", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddExportPreAlert(int EnqID, string MBLNO, string HBLNO, string ContainerNO, string EmailId, DateTime MBLDate, DateTime HBLDate, DateTime LEODate, string FlightScheduleDetail, int ModeId, int CHAById, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@MBLNO", SqlDbType.NVarChar).Value = MBLNO;
        command.Parameters.Add("@HBLNO", SqlDbType.NVarChar).Value = HBLNO;
        command.Parameters.Add("@ContainerNO", SqlDbType.NVarChar).Value = ContainerNO;
        command.Parameters.Add("@EmailId", SqlDbType.NVarChar).Value = EmailId;
        if (MBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MBLDate", SqlDbType.Date).Value = MBLDate;
        }
        if (HBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HBLDate", SqlDbType.Date).Value = HBLDate;
        }
        if (LEODate != DateTime.MinValue)
        {
            command.Parameters.Add("@LEODate", SqlDbType.Date).Value = LEODate;
        }

        command.Parameters.Add("@FlightScheduleDetail", SqlDbType.NVarChar).Value = FlightScheduleDetail;
        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@CHAById", SqlDbType.Int).Value = CHAById;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insExportPreAlert", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddExportPreAlert(int EnqID, string MBLNO, string HBLNO, string ContainerNO, string EmailId, DateTime MBLDate, DateTime HBLDate, DateTime LEODate,
        DateTime ETATranshipment, DateTime ETDTranshipment, DateTime ATADestination, string FlightScheduleDetail, int ModeId, int CHAById, int lUser) 
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        command.Parameters.Add("@MBLNO", SqlDbType.NVarChar).Value = MBLNO;
        command.Parameters.Add("@HBLNO", SqlDbType.NVarChar).Value = HBLNO;
        command.Parameters.Add("@ContainerNO", SqlDbType.NVarChar).Value = ContainerNO;
        command.Parameters.Add("@EmailId", SqlDbType.NVarChar).Value = EmailId;
        if (MBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MBLDate", SqlDbType.Date).Value = MBLDate;
        }
        if (HBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HBLDate", SqlDbType.Date).Value = HBLDate;
        }
        if (LEODate != DateTime.MinValue)
        {
            command.Parameters.Add("@LEODate", SqlDbType.Date).Value = LEODate;
        }
        if (ETATranshipment != DateTime.MinValue)
        {
            command.Parameters.Add("@ETATranshipment", SqlDbType.Date).Value = ETATranshipment;
        }
        if (ETDTranshipment != DateTime.MinValue)
        {
            command.Parameters.Add("@ETDTranshipment", SqlDbType.Date).Value = ETDTranshipment;
        }
        if (ATADestination != DateTime.MinValue)
        {
            command.Parameters.Add("@ATADestination", SqlDbType.Date).Value = ATADestination;
        }
        command.Parameters.Add("@FlightScheduleDetail", SqlDbType.NVarChar).Value = FlightScheduleDetail;
        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;
        command.Parameters.Add("@CHAById", SqlDbType.Int).Value = CHAById;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insExportPreAlert", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }


    public static int AddExportOnBoard(int EnqID, DateTime OnboardDate, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqID", SqlDbType.BigInt).Value = EnqID;
        if (OnboardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OnboardDate", SqlDbType.Date).Value = OnboardDate;
        }
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_insExportShipOnboard", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetDocumentDetailsExport(int EnqId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        return CDatabase.GetDataSet("FOP_GetDocumentDetails", command);
    }

    public static DataSet GetExpPlantDetails(int PlantId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        DataSet dsFRPlant = CDatabase.GetDataSet("GetCustPlantDetailsById", command);

        return dsFRPlant;
    }

    public static DataSet GetCustomerIdByNmae(string CustomerName)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;

        return CDatabase.GetDataSet("FOP_GetCustomerIdByName", command);
    }

    public static int UpdFreightBookingEx(int EnqID, int NoOfPackages, string GrossWeight, string ChargeWeight, string LCLVolume,
        int ContainerTypeId, int Count20, int Count40, string InvoiceNo, string PONumber, string BookingDetails,
        string strDescription, DateTime InvoiceDate, DateTime BookingDate, string strCustomer, int Division, int Plant,
        int TransportionBy, int CHABy, string TransporterName, string CHAName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqID;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@GrossWeight", SqlDbType.NVarChar).Value = GrossWeight;
        command.Parameters.Add("@ChargeWeight", SqlDbType.NVarChar).Value = ChargeWeight;
        command.Parameters.Add("@LCLVolume", SqlDbType.NVarChar).Value = LCLVolume;
        command.Parameters.Add("@ContainerType", SqlDbType.Int).Value = ContainerTypeId;
        command.Parameters.Add("@CountOf20", SqlDbType.Int).Value = Count20;
        command.Parameters.Add("@CountOf40", SqlDbType.Int).Value = Count40;
        command.Parameters.Add("@PONumber", SqlDbType.NVarChar).Value = PONumber;
        //command.Parameters.Add("@BookingDetails", SqlDbType.Int).Value = BookingDetails;
        //command.Parameters.Add("@CargoDescription", SqlDbType.Int).Value = strDescription;   
        command.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar).Value = InvoiceNo;

        command.Parameters.Add("@CargoDescription", SqlDbType.NVarChar).Value = strDescription;
        command.Parameters.Add("@BookingDetails", SqlDbType.NVarChar).Value = BookingDetails;

        if (InvoiceDate != DateTime.MinValue)
        {
            command.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = InvoiceDate;
        }

        if (BookingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@BookingDate", SqlDbType.DateTime).Value = BookingDate;
        }

        command.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = strCustomer;
        command.Parameters.Add("@Division", SqlDbType.NVarChar).Value = Division;
        command.Parameters.Add("@Plant", SqlDbType.NVarChar).Value = Plant;
        command.Parameters.Add("@TransportionBy", SqlDbType.NVarChar).Value = TransportionBy;
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@CHABy", SqlDbType.NVarChar).Value = CHABy;
        command.Parameters.Add("@CHAName", SqlDbType.NVarChar).Value = CHAName;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updBookingDetailsExp", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdFreightOperationEx(int EnqID, int SBNo, DateTime SBDate, DateTime ContainerPickup, DateTime CustomPermission,
         DateTime StuffingDate, DateTime CLPDate, DateTime CartingDate, string ASIBy, DateTime ASIDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqID;
        command.Parameters.Add("@SBNo", SqlDbType.Int).Value = SBNo;

        if (SBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SBDate", SqlDbType.DateTime).Value = SBDate;
        }

        if (ContainerPickup != DateTime.MinValue)
        {
            command.Parameters.Add("@ContainerPickup", SqlDbType.DateTime).Value = ContainerPickup;
        }
        if (CustomPermission != DateTime.MinValue)
        {
            command.Parameters.Add("@CustomPermission", SqlDbType.DateTime).Value = CustomPermission;
        }
        if (StuffingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@StuffingDate", SqlDbType.DateTime).Value = StuffingDate;
        }
        if (CLPDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CLPDate", SqlDbType.DateTime).Value = CLPDate;
        }

        if (CartingDate != DateTime.MinValue)
        {
            command.Parameters.Add("@CartingDate", SqlDbType.DateTime).Value = CartingDate;
        }

        command.Parameters.Add("@ASIBy", SqlDbType.NVarChar).Value = ASIBy;
        if (ASIDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ASIDate", SqlDbType.DateTime).Value = ASIDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updOperationDetailsExp", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static string FOP_GetShipperAddrByCustId(int ShipperId)
    {
        string strAddress = "";

        if (ShipperId != 0)
        {
            string strQuery = "SELECT Address AS Value FROM BS_CustomerMS WHERE lid= @ShipperId";

            SqlCommand command = new SqlCommand();
            command.CommandText = strQuery;
            command.Parameters.Add("@ShipperId", SqlDbType.Int).Value = ShipperId;

            strAddress = CDatabase.ResultInString(command);
        }

        return strAddress;
    }

    public static DataView GetDocumentByEnqId(int PreAlertID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = PreAlertID;

        return CDatabase.GetDataView("FOP_GetDocumentByEnqIdExport", cmd);
    }

    public static void FillFrAirLineMaster(DropDownList DropdownList, int EnqID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnqID", SqlDbType.Int).Value = EnqID;

        CDatabase.BindControls(DropdownList, "FR_GetAirLineMasterdetail", command, "CompName", "lid");
    }

    public static int UpdFreighCustPreAlertEx(int EnqID, string MBLNo, string HBLNo, DateTime MBLDate, DateTime HBLDate,
    string flightSchedule, DateTime LEODate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqID;
        command.Parameters.Add("@MBLNo", SqlDbType.NVarChar).Value = MBLNo;
        command.Parameters.Add("@HBLNo", SqlDbType.NVarChar).Value = HBLNo;

        if (MBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@MBLDate", SqlDbType.DateTime).Value = MBLDate;
        }

        if (HBLDate != DateTime.MinValue)
        {
            command.Parameters.Add("@HBLDate", SqlDbType.DateTime).Value = HBLDate;
        }
        command.Parameters.Add("@flightSchedule", SqlDbType.NVarChar).Value = flightSchedule;

        if (LEODate != DateTime.MinValue)
        {
            command.Parameters.Add("@LEODate", SqlDbType.DateTime).Value = LEODate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updCustPreAlertDetailsExp", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdFreightVGMForm13Ex(int EnqID, int SBNo, DateTime SBDate, DateTime VGMDate, DateTime Form13Date, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqID;
        command.Parameters.Add("@SBNo", SqlDbType.NVarChar).Value = SBNo;

        if (SBDate != DateTime.MinValue)
        {
            command.Parameters.Add("@SBDate", SqlDbType.DateTime).Value = SBDate;
        }
        if (VGMDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VGMDate", SqlDbType.DateTime).Value = VGMDate;
        }

        if (Form13Date != DateTime.MinValue)
        {
            command.Parameters.Add("@Form13Date", SqlDbType.DateTime).Value = Form13Date;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updVFMForm13DetailsExp", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdFreightOnBoardEx(int EnqID, DateTime OnBoardDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqID;

        if (OnBoardDate != DateTime.MinValue)
        {
            command.Parameters.Add("@OnBoardDate", SqlDbType.DateTime).Value = OnBoardDate;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FOP_updOnBoardDetailsExp", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region  Customer Side - Export

    public static DataSet GetPendingStageWsCustJobdetail(int FinYearID, int Status, int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@FinYearID", SqlDbType.Int).Value = FinYearID;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        return CDatabase.GetDataSet("GetPendingStageWsCustJobDetails", command);
    }

    #endregion

    #region ICE Gate

    public static DataSet ICE_GetIGMDetail(int JobID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobID", SqlDbType.Int).Value = JobID;

        return CDatabase.GetDataSet("ICE_GetIGM", cmd);
    }

    #endregion

    #region Job Cancel/ File Sent To Billing

    public static DataSet GetExpenseDetails(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetBJVDetails", command);
    }

    public static int AddJobCancelStatus(int JobId, string DailyProgress, string DocumentPath, int SummaryStatus,
            Boolean VisibleToCustomer, string CancelReason, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@DocumentPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@SummaryStatus", SqlDbType.Int).Value = SummaryStatus;
        command.Parameters.Add("@VisibleToCustomer", SqlDbType.Bit).Value = VisibleToCustomer;
        command.Parameters.Add("@CancelReason", SqlDbType.NVarChar).Value = CancelReason;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobCancelDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetRoleIdByUserId(int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        return CDatabase.GetDataSet("GetRoleByUserId", command);
    }

    public static int AddFileSentToBilling(int JobId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insJobSentToBilling", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region Generate Customer Reference No (SKF)

    public static DataSet GetCustomerRefNo(int CustomerId, int DivisionId, int PlantId, int Mode, int BEType)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@Mode", SqlDbType.Int).Value = Mode;
        command.Parameters.Add("@BEType", SqlDbType.Int).Value = BEType;

        return CDatabase.GetDataSet("GetCustRefNoByCustomer", command);
    }

    #endregion

    #region CRM

    public static DataSet CRM_GetDashboardStagesCount(int MonthId, int CompanyId, int UserId, int FinYearId, int SalesPersonId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@MonthId", SqlDbType.Int).Value = MonthId;
        command.Parameters.Add("@CompanyId", SqlDbType.Int).Value = CompanyId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@SalesPersonId", SqlDbType.Int).Value = SalesPersonId;
        return CDatabase.GetDataSet("CRM_GetDashboardStagesCount", command);
    }

    public static DataSet GetParticularCompany(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetCompanyAsPerLid", command);
    }

    public static DataSet GetBabajiCustomerByLid(int CustomerId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lId", SqlDbType.Int).Value = CustomerId;
        return CDatabase.GetDataSet("GetCustomerBylid", cmd);
    }

    public static int CRM_AddCompanyMS(string sName, string Email, string MobileNo, string ContactNo, string AddressLine1, string AddressLine2,
        string AddressLine3, string Website, string Description, string ContactPerson, string OfficeLocation, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;
        command.Parameters.Add("@AddressLine1", SqlDbType.NVarChar).Value = AddressLine1;
        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = AddressLine2;
        command.Parameters.Add("@AddressLine3", SqlDbType.NVarChar).Value = AddressLine3;
        command.Parameters.Add("@Website", SqlDbType.NVarChar).Value = Website;
        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@OfficeLocation", SqlDbType.NVarChar).Value = OfficeLocation;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insCompanyMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddLead(int CompanyID, int LeadStageID, int LeadSourceID, int SectorID, int RoleId, int CompanyTypeID, int BusinessCategoryID,
        string LeadSourceValue, string Turnover, string EmployeeCount, string ContactName, string Designation, string Email, string MobileNo, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = CompanyID;
        command.Parameters.Add("@LeadStageID", SqlDbType.Int).Value = LeadStageID;
        command.Parameters.Add("@LeadSourceID", SqlDbType.Int).Value = LeadSourceID;
        command.Parameters.Add("@SectorID", SqlDbType.Int).Value = SectorID;
        if (RoleId > 0)
            command.Parameters.Add("@RoleId", SqlDbType.Int).Value = RoleId;
        command.Parameters.Add("@CompanyTypeID", SqlDbType.Int).Value = CompanyTypeID;
        command.Parameters.Add("@BusinessCategoryID", SqlDbType.Int).Value = BusinessCategoryID;
        command.Parameters.Add("@LeadSourceValue", SqlDbType.NVarChar).Value = LeadSourceValue;
        command.Parameters.Add("@Turnover", SqlDbType.NVarChar).Value = Turnover;
        command.Parameters.Add("@EmployeeCount", SqlDbType.NVarChar).Value = EmployeeCount;
        command.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ContactName;
        command.Parameters.Add("@Designation", SqlDbType.NVarChar).Value = Designation;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insLeads", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddEnquiry(int LeadId, string EnquiryNo, string Notes, bool IsOutsideEnq, int ExistingCustomerId, string PaymentTerms,
        string CustReference, string Turnover, string YearsInService, string TotalEmp, int CompanyType, string VolumeExpected, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnquiryNo", SqlDbType.NVarChar).Value = EnquiryNo;
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = Notes;
        //command.Parameters.Add("@ApprovalTo", SqlDbType.Int).Value = ApprovalTo;
        command.Parameters.Add("@IsOutsideEnq", SqlDbType.Bit).Value = IsOutsideEnq;
        if (ExistingCustomerId > 0)
            command.Parameters.Add("@EnqCustomerId", SqlDbType.Int).Value = ExistingCustomerId;
        command.Parameters.Add("@YearsInService", SqlDbType.NVarChar).Value = YearsInService;
        command.Parameters.Add("@TotalEmp", SqlDbType.NVarChar).Value = TotalEmp;
        command.Parameters.Add("@CompanyType", SqlDbType.Int).Value = CompanyType;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = PaymentTerms;
        command.Parameters.Add("@CustRef", SqlDbType.NVarChar).Value = CustReference;
        command.Parameters.Add("@Turnover", SqlDbType.NVarChar).Value = Turnover;
        command.Parameters.Add("@VolumeExpected", SqlDbType.NVarChar).Value = VolumeExpected;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insEnquiry", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddEnquiryHistory(int EnquiryId, int StatusId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = EnquiryId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insEnquiryHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddEnquiry_Service(int EnquiryId, int ServiceId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = EnquiryId;
        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insEnquiry_Services", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddLeadService(int LeadId, int ServiceId, int ServiceLocation, string VolumeExpected, string Requirement, DateTime ExpectedCloseDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@ServiceLocationId", SqlDbType.Int).Value = ServiceLocation;
        command.Parameters.Add("@VolumeExpected", SqlDbType.NVarChar).Value = VolumeExpected;
        command.Parameters.Add("@Requirement", SqlDbType.NVarChar).Value = Requirement;
        if (ExpectedCloseDate != DateTime.MinValue)
            command.Parameters.Add("@ExpectedCloseDate", SqlDbType.DateTime).Value = ExpectedCloseDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insLead_Service", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddLeadStageHistory(int LeadId, int StageId, DateTime TargetDt, string Remark, int lUser)//
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@StageId", SqlDbType.Int).Value = StageId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        if (TargetDt == DateTime.MinValue)
        {
            command.Parameters.Add("@TargetDt", SqlDbType.DateTime).Value = DBNull.Value;
        }
        else
        {
            command.Parameters.Add("@TargetDt", SqlDbType.DateTime).Value = TargetDt;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insLeadStageHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddContactDetail(string ContactName, int CompanyId, string Designation, int RoleId, string MobileNo,
        string Email, string AlternatePhone, string Address, string Description, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ContactName;
        command.Parameters.Add("@CompanyId", SqlDbType.Int).Value = CompanyId;
        command.Parameters.Add("@Designation", SqlDbType.NVarChar).Value = Designation;
        command.Parameters.Add("@RoleId", SqlDbType.Int).Value = RoleId;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@AlternatePhone", SqlDbType.NVarChar).Value = AlternatePhone;
        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insContactDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddFollowupHistory(int LeadId, DateTime Date, bool IsActive, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        if (Date != DateTime.MinValue)
            command.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        //command.Parameters.Add("@IsCompleted", SqlDbType.Int).Value = IsCompleted;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insFollowupHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddVisitReport(int LeadId, DateTime VisitDate, int VisitCategory, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        if (VisitDate != DateTime.MinValue)
            command.Parameters.Add("@VisitDate", SqlDbType.DateTime).Value = VisitDate;
        command.Parameters.Add("@VisitCategory", SqlDbType.Int).Value = VisitCategory; 
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insVisitReport", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddCustomerVisitReport(int CustomerId, DateTime VisitDate, int VisitCategory, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        if (VisitDate != DateTime.MinValue)
            command.Parameters.Add("@VisitDate", SqlDbType.DateTime).Value = VisitDate;
        command.Parameters.Add("@VisitCategory", SqlDbType.Int).Value = VisitCategory;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insCustomerVisitReport", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddOpportunity_MOM(int LeadId, string Title, DateTime MeetingDate, DateTime StartTime, DateTime EndTime, string Observers, string Resources,
                                        string Notes, int lUser, bool IsExistingCust, int CustomerId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@IsExistingCust", SqlDbType.Int).Value = IsExistingCust;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = Title;
        command.Parameters.Add("@Date", SqlDbType.DateTime).Value = MeetingDate;
        if (StartTime != DateTime.MinValue)
            command.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = StartTime;
        if (EndTime != DateTime.MinValue)
            command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = EndTime;
        command.Parameters.Add("@Observers", SqlDbType.NVarChar).Value = Observers;
        command.Parameters.Add("@Resources", SqlDbType.NVarChar).Value = Resources;
        command.Parameters.Add("@SpecialNotes", SqlDbType.NVarChar).Value = Notes;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insOpportunity_MOM", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddMOM_Attendee(int MomId, string UserName, string UserEmail, int UserId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@MomId", SqlDbType.Int).Value = MomId;
        if (UserId == 0)
            command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName;
        command.Parameters.Add("@UserEmail", SqlDbType.NVarChar).Value = UserEmail;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insMOM_Attendee", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddMOM_Agenda(int MomId, string Topic, string Description, string PersonName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@MomId", SqlDbType.Int).Value = MomId;
        command.Parameters.Add("@Topic", SqlDbType.NVarChar).Value = Topic;
        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insMOM_Agenda", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddActivityLog(string Description, int LeadId, int CurrentStatusId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Description;
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@CurrentStatusId", SqlDbType.Int).Value = CurrentStatusId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insActivityLog", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int ApproveEnquiry(int EnquiryId, int IsApproved, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = EnquiryId;
        command.Parameters.Add("@IsApproved", SqlDbType.Int).Value = IsApproved;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_updApproveEnquiry", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_AddEnquiryService(int LeadId, int EnquiryId, int ServiceId, int ServiceLocation, string VolumeExpected, string Requirement, DateTime ExpectedCloseDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = EnquiryId;
        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@ServiceLocationId", SqlDbType.Int).Value = ServiceLocation;
        command.Parameters.Add("@VolumeExpected", SqlDbType.NVarChar).Value = VolumeExpected;
        command.Parameters.Add("@Requirement", SqlDbType.NVarChar).Value = Requirement;
        if (ExpectedCloseDate != DateTime.MinValue)
            command.Parameters.Add("@ExpectedCloseDate", SqlDbType.DateTime).Value = ExpectedCloseDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insEnquiryService", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }


    public static int CRM_UpdLeadService(int lid, int ServiceLocation, string VolumeExpected, string Requirement, DateTime ExpectedCloseDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@ServiceLocationId", SqlDbType.Int).Value = ServiceLocation;
        command.Parameters.Add("@VolumeExpected", SqlDbType.NVarChar).Value = VolumeExpected;
        command.Parameters.Add("@Requirement", SqlDbType.NVarChar).Value = Requirement;
        if (ExpectedCloseDate != DateTime.MinValue)
            command.Parameters.Add("@ExpectedCloseDate", SqlDbType.DateTime).Value = ExpectedCloseDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_updLead_Service", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_UpdFollowupStatusHistory(int lid, int IsCompleted, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@IsCompleted", SqlDbType.Int).Value = IsCompleted;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_updFollowupStatusHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_UpdateLeadStatus(int LeadId, bool Lead, bool Converted, bool Enquiry, bool Quote, bool KYC, bool Contract, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@Lead", SqlDbType.Bit).Value = Lead;
        command.Parameters.Add("@Converted", SqlDbType.Bit).Value = Converted;
        command.Parameters.Add("@Enquiry", SqlDbType.Bit).Value = Enquiry;
        command.Parameters.Add("@Quote", SqlDbType.Bit).Value = Quote;
        command.Parameters.Add("@KYC", SqlDbType.Bit).Value = KYC;
        command.Parameters.Add("@Contract", SqlDbType.Bit).Value = Contract;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_updLeadStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_UpdateLeadRfqReceived(int LeadId, bool RfqReceived, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        command.Parameters.Add("@RfqReceived", SqlDbType.Bit).Value = RfqReceived;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_updLeadRfqReceived", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }



    public static int CRM_DelLeadService(int lid)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_delLead_Service", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_DelVisitReport(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_delVisitReport", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_DelEnquiryService(int lid)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_delEnquiry_Service", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_DelRequirementService(int lid)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_delRequirementService", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static string GetEnquiryRefNo()
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("CRM_GetEnquiryRefNo", command, "@OutPut");
        return SPresult;
    }

    public static DataSet CRM_GetLeadById(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetLeadByLid", command);
    }

    public static DataSet CRM_GetRFQDocuments(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetRFQDocuments", command);
    }

    public static DataSet CRM_GetServices(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetServices", command);
    }

    public static DataSet CRM_GetEnquiryHistoryByLead(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetEnquiryHistoryByLead", command);
    }

    public static DataSet CRM_GetEnquiryByLeadId(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetEnquiryByLeadId", command);
    }

    public static DataSet CRM_GetServicesById(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetServicesByLid", command);
    }

    public static DataSet CRM_GetRequirementById(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetRequirementByLid", command);
    }

    public static DataSet CRM_GetLeadStageHistory(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetLeadStageHistory", command);
    }

    public static DataSet CRM_GetFollowupHistoryByLId(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetFollowupHistoryByLId", command);
    }

    public static DataSet CRM_GetVisitReport(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetVisitReport", command);
    }

    public static DataSet GetUserByID(int UserId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        return CDatabase.GetDataSet("BS_GetUserByID", command);
    }

    public static DataSet CRM_GetOpportunity_MOM(int lUser)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        return CDatabase.GetDataSet("CRM_GetOpportunity_MOM", command);
    }

    public static DataSet CRM_GetOpportunity_MOM_Opportunity(int OpportunityId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@OpportunityId", SqlDbType.Int).Value = OpportunityId;
        return CDatabase.GetDataSet("CRM_GetOpportunityMOM_Opportunity", command);
    }

    public static DataSet CRM_GetOpportunity_MOM_Lid(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetOpportunity_MOM_Lid", command);
    }

    public static DataSet CRM_GetMom_Attendee(int MomId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@MomId", SqlDbType.Int).Value = MomId;
        return CDatabase.GetDataSet("CRM_GetMom_Attendee", command);
    }

    public static DataSet CRM_GetMOM_Agenda(int MomId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@MomId", SqlDbType.Int).Value = MomId;
        return CDatabase.GetDataSet("CRM_GetMOM_Agenda", command);
    }

    public static DataSet CRM_GetQuoteByLead(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetQuoteByLead", command);
    }

    public static DataSet CRM_GetQuoteByLid(int QuotationId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@QuotationId", SqlDbType.Int).Value = QuotationId;
        return CDatabase.GetDataSet("CRM_GetQuoteBylid", command);
    }

    public static DataSet CRM_GetRfqQuoteByLead(int LeadId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        return CDatabase.GetDataSet("CRM_GetRfqQuoteByLead", command);
    }

    public static DataSet GetLeadEnquiryByLid(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetLeadEnquiryByLid", command);
    }

    public static DataSet CRM_GetQuoteByEnquiry(int EnquiryId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = EnquiryId;
        return CDatabase.GetDataSet("CRM_GetQuoteByEnquiry", command);
    }

    public static DataSet CRM_GetRfqEnquiryByLid(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataSet("CRM_GetRfqEnquiryByLid", command);
    }

    public static int CRM_AddUserLocation(int ServiceId, int ServiceLocationId, int UserId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@ServiceId", SqlDbType.Int).Value = ServiceId;
        command.Parameters.Add("@ServiceLocationId", SqlDbType.Int).Value = ServiceLocationId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_insUserLocation", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int CRM_DeleteUserLocation(int lid, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("CRM_delUserLocation", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    #endregion


    #region Contract Billing

    public static DataSet GetContractMasterData(int mastertype)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@mastertype", SqlDbType.Int).Value = mastertype;
        return CDatabase.GetDataSet("Usp_GetContractMasterData", command);
    }
    public static DataTable GetcontractBillingdata(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataTable("Usp_GetContractbillingrData", command);
    }
    public static DataTable GetcontractBillingdata_Pageing(int pageIndex, int PageSize, int lid)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
        command.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@RecordCount", SqlDbType.Int).Value = 4;
        command.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
        return CDatabase.GetDataTable("GetCustomersPageWise", command);
    }
    public static DataTable Usp_GetContractbillingrData_edit(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataTable("Usp_GetContractbillingrData_edit", command);
    }
    public static DataSet ContractBilling_Pint(string Jobno, string Name1)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@Jobno", SqlDbType.VarChar).Value = Jobno;
        command.Parameters.Add("@name1", SqlDbType.VarChar).Value = Name1;
        return CDatabase.GetDataSet("Bill_InvPrint", command);
    }
    public static DataSet GetJobDetail_Users(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobId", command);
        return CDatabase.GetDataSet("GetJobDetail_Users", command);
    }
    public static DataSet GetJobDetailForContractBilling(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobId", command);
        return CDatabase.GetDataSet("Get_JobDetailForContratcBillingByJobId", command);
    }
    public static DataSet GetJobDetailForContractBillingUser(int JobId, string username, int @status)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
        command.Parameters.Add("@status", SqlDbType.Int).Value = status;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobId", command);
        return CDatabase.GetDataSet("Get_JobDetailForContratcBillingByJobIdUser", command);
    }
    public static DataSet CheckCustomerContractActive(int JobId, int ModuleId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ModuleID", SqlDbType.Int).Value = ModuleId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        return CDatabase.GetDataSet("BS_CheckJobContract", command);
    }
    public static DataSet GetJobDetailForContractBilling_insertUserDetails(int lid, string JobNo, string chargecode, string chargename, int qty, int userid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.VarChar).Value = lid;
        command.Parameters.Add("@jobno", SqlDbType.VarChar).Value = JobNo;
        command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = chargecode;
        command.Parameters.Add("@chargename", SqlDbType.VarChar).Value = chargename;
        command.Parameters.Add("@qty", SqlDbType.VarChar).Value = qty;
        command.Parameters.Add("@userid", SqlDbType.VarChar).Value = userid;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        return CDatabase.GetDataSet("usp_Inserttbl_CBBilling_User", command);
    }
    public static int Get_ContractChecking(int JobId)
    {
        SqlCommand command = new SqlCommand();
        int SPresult;
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //CDatabase.GetDataTable("Get_ContractChecking", command);
        SPresult = CDatabase.GetSPCount("Get_ContractChecking", command);
        return Convert.ToInt32(SPresult);
    }
    public static string Get_ContractChecking_withexpired(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult;
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        //CDatabase.GetDataTable("Get_ContractChecking", command);
        //SPresult = CDatabase.GetSPOutPut("Get_ContractChecking_withexpired", command, "@OutPut");
        //SPresult = CDatabase.GetSPOutPut("Get_ContractChecking_withexpired", command, "@OutPut");
        DataTable dtt1 = CDatabase.GetDataTable("Get_ContractChecking_withexpired", command);
        SPresult = dtt1.Rows[0][0].ToString();
        return SPresult;
    }
    public static int Get_JobDetailForContratcBillingByJobIdUser_Checking(int JobId)
    {
        SqlCommand command = new SqlCommand();
        int SPresult;
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobIdUser_Checking", command);
        SPresult = CDatabase.GetSPCount("Get_JobDetailForContratcBillingByJobIdUser_Checking", command);
        return Convert.ToInt32(SPresult);
    }
    
    public static DataTable FillTableData(string sql)
    {
        SqlConnection OpenDB = new SqlConnection();
        OpenDB = CDatabase.getConnection();
        DisconnectSQL(OpenDB);

        OpenDB.Open();
        using (SqlDataAdapter da = new SqlDataAdapter(sql, OpenDB))
        {
            da.SelectCommand.CommandTimeout = 21600;
            DataTable table = new DataTable();
            da.Fill(table);
            DisconnectSQL(OpenDB);
            return table;
        }
    }
    public static DataTable FillGetBillingdata(string RangeCriteria, string Currency, string UOM, string CapCriteria, string UserSystem,
        string Mode, string port, string typeofshippment, string ContainerType, string JobType, string TypeOfBE, string RMSNonRMS,
        string LoadedDeStuff, string Division)
    {
        DataTable dt = new DataTable();
        SqlConnection Conn = CDatabase.getConnection();
        Conn.Open();
        SqlCommand sqlComm = new SqlCommand("Usp_GetBillingdata", Conn);
        sqlComm.Parameters.AddWithValue("@RangeCriteria", RangeCriteria);
        sqlComm.Parameters.AddWithValue("@Currency", Currency);
        sqlComm.Parameters.AddWithValue("@UOM", UOM);
        sqlComm.Parameters.AddWithValue("@CapCriteria", CapCriteria);
        sqlComm.Parameters.AddWithValue("@UserSystem", UserSystem);
        sqlComm.Parameters.AddWithValue("@Mode", Mode);
        sqlComm.Parameters.AddWithValue("@port", port);
        sqlComm.Parameters.AddWithValue("@typeofshippment", typeofshippment);
        sqlComm.Parameters.AddWithValue("@ContainerType", ContainerType);
        sqlComm.Parameters.AddWithValue("@JobType", JobType);
        sqlComm.Parameters.AddWithValue("@TypeOfBE", TypeOfBE);
        sqlComm.Parameters.AddWithValue("@RMSNonRMS", RMSNonRMS);
        sqlComm.Parameters.AddWithValue("@LoadedDeStuff", LoadedDeStuff);
        sqlComm.Parameters.AddWithValue("@Division", Division);

        sqlComm.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = sqlComm;

        da.Fill(dt);
        return dt;
    }
    public static void CreateTable()
    {
        try
        {
            string selSQL = string.Empty;
            string TableName = string.Empty;
            string DisplayName = string.Empty;
            string InsertSQL = string.Empty;
            string CreateSQL = " if not exists( SELECT * FROM INFORMATION_SCHEMA.TABLES ";
            CreateSQL = CreateSQL + " WHERE TABLE_NAME = N'tbl_UserLog" + DateTime.Now.ToString("MMMyyyy") + " ') ";
            CreateSQL = CreateSQL + " Begin";
            CreateSQL = CreateSQL + " Create Table tbl_UserLog" + DateTime.Now.ToString("MMMyyyy") + "";
            CreateSQL = CreateSQL + "([LoginId] [numeric](18, 0) NULL,";
            CreateSQL = CreateSQL + " [FormName] [varchar](100) NULL,";
            CreateSQL = CreateSQL + " [Event] [varchar](100) NULL,";
            CreateSQL = CreateSQL + " [Description] [varchar](1000) NULL,";
            CreateSQL = CreateSQL + " [DateTime] [datetime] NULL) ON [PRIMARY]";
            CreateSQL = CreateSQL + " end";
            SqlConnection Con = CDatabase.getConnection();
            Con.Open();
            SqlCommand cmd = new SqlCommand(CreateSQL, Con);
            cmd.ExecuteNonQuery();

            //Error Log
            CreateSQL = " if not exists( SELECT * FROM INFORMATION_SCHEMA.TABLES ";
            CreateSQL = CreateSQL + " WHERE TABLE_NAME = N'tbl_ErrorLog" + DateTime.Now.ToString("MMMyyyy") + " ') ";
            CreateSQL = CreateSQL + " Begin";
            CreateSQL = CreateSQL + " Create Table tbl_ErrorLog" + DateTime.Now.ToString("MMMyyyy") + "";
            CreateSQL = CreateSQL + " ([LoginId] [numeric](18, 0) NULL,";
            CreateSQL = CreateSQL + " [FormName] [varchar](100) NULL,";
            CreateSQL = CreateSQL + " [Event] [varchar](100) NULL,";
            CreateSQL = CreateSQL + " [ErrorNumber] [varchar](100) NULL,";
            CreateSQL = CreateSQL + " [Description] [varchar](1000) NULL,";
            CreateSQL = CreateSQL + " [DateTime] [datetime] NULL) ON [PRIMARY]";
            CreateSQL = CreateSQL + " end";

            SqlCommand cmdErrorLog = new SqlCommand(CreateSQL, Con);
            cmdErrorLog.ExecuteNonQuery();
            Con.Close();



            TableName = "tbl_ErrorLog" + DateTime.Now.ToString("MMMyyyy");
            DisplayName = "Error Log " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy");
            selSQL = "Select * from tbl_ErrorLogTableName where ActualTableName = '" + TableName + "'";
            DataTable ErrorDT = FillTableData(selSQL);
            if (ErrorDT.Rows.Count > 0)
            {

            }
            else
            {
                InsertSQL = "Insert into tbl_ErrorLogTableName values ('" + TableName + "','" + DisplayName + "','" + UserLogin.ToString() + "',getdate())";
                InsertDeleteCommand(InsertSQL);
            }

            TableName = "tbl_UserLog" + DateTime.Now.ToString("MMMyyyy");
            DisplayName = "User Log " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy");
            selSQL = "Select * from tbl_UserLogTableName where ActualTableName = '" + TableName + "'";
            DataTable UserDT = FillTableData(selSQL);
            if (UserDT.Rows.Count > 0)
            {

            }
            else
            {
                InsertSQL = "Insert into tbl_UserLogTableName values ('" + TableName + "','" + DisplayName + "','" + UserLogin.ToString() + "',getdate())";
                InsertDeleteCommand(InsertSQL);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    public static void UpdateErrorLog(string LoginID, string FormName, string Event, string ErrNumber, string ErrorDescription)
    {
        try
        {
            CreateTable();
            string sql = " Insert into tbl_ErrorLog" + DateTime.Now.ToString("MMMyyyy") + "";
            sql = sql + "(LoginId";
            sql = sql + ",FormName ";
            sql = sql + ",Event ";
            sql = sql + ",ErrorNumber ";
            sql = sql + ",Description ";
            sql = sql + ",[DateTime] ";
            sql = sql + " )";
            sql = sql + " Values";
            sql = sql + " ('" + LoginID + "'";
            sql = sql + ", '" + FormName.Replace("'", "") + "'";
            sql = sql + ", '" + Event + "'";
            sql = sql + ", '" + ErrNumber + "'";
            sql = sql + ", '" + ErrorDescription.Replace("'", "''") + "'";
            sql = sql + ", '" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "'";
            sql = sql + " )";
            SqlConnection Con = CDatabase.getConnection();
            Con.Open();
            SqlCommand cmd = new SqlCommand(sql, Con);
            cmd.ExecuteNonQuery();
            Con.Close();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public static string UserLogin = "1";
    public static void InsertDeleteCommand(string DeleteQuery)
    {
        SqlConnection Con = CDatabase.getConnection();
        Con.Open();
        SqlCommand cmd = new SqlCommand(DeleteQuery, Con);
        cmd.ExecuteNonQuery();
        DisconnectSQL(Con);
    }
    #endregion

    #region Customer License
    public static int AddLicenseRetrun(int LicenseId, DateTime dtReturnDate, string strDispatchAddress, int DispatchMode, string strDispatchName, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@LicenseId", SqlDbType.Int).Value = LicenseId;
        command.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = dtReturnDate;
        command.Parameters.Add("@DispatchAddress", SqlDbType.NVarChar).Value = strDispatchAddress;
        command.Parameters.Add("@DispatchMode", SqlDbType.Int).Value = DispatchMode;
        command.Parameters.Add("@DispatchName", SqlDbType.NVarChar).Value = strDispatchName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updCustomerLicenseReturn", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    #endregion

    #region SKF Customer
    public static DataSet GetSKFIndiaSummary(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetSKFIndiaJob", command);
    }

    public static DataSet GetJobInvoiceMS(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataSet("GetJobInvoiceDetail", command);
    }
    public static DataSet GetSKFPreAlertInvoice(int AlertId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@AlertId", SqlDbType.Int).Value = AlertId;
        return CDatabase.GetDataSet("SF_GetPreAlertDocById", command);
    }
    public static DataSet GetSKFPreAlertItem(int AlertId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@AlertId", SqlDbType.Int).Value = AlertId;
        return CDatabase.GetDataSet("SF_GetPreAlertItemById", command);
    }
    #endregion

    #region CRM_reports

    public static DataTable CRM_GetWeeklySalesWise(string Action_Perform, int Type, DateTime dtStartDate, DateTime dtEndDate, int lUser, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@Action_Perform", SqlDbType.VarChar).Value = Action_Perform;
        command.Parameters.Add("@Type", SqlDbType.Int).Value = Type;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtStartDate;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dtEndDate;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataTable("CRM_rptWeeklySalesWise", command);
    }

    #endregion

    #region crm operation count

    public static DataSet GetCRMOperationCount(int UserId, int FinYear)  //change by sayali on 04-11-2019
    {
        DataSet dsPending;
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYear;
        dsPending = CDatabase.GetDataSet("CRM_GetCountOperation", command);

        return dsPending;
    }

    #endregion

    #region CRM Lead assign to other user //added by sayali 10-12-2019

    public static int CRM_insLeadShare(int LeadId, int AssignUserTo, string Remark, int UserId, int FinYear)
    {
        SqlCommand cmd = new SqlCommand();
        string Result = "";

        cmd.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        cmd.Parameters.Add("@AssignUserId", SqlDbType.Int).Value = AssignUserTo;
        cmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYear;
        cmd.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        Result = CDatabase.GetSPOutPut("CRM_insLeadShare", cmd, "@Output");
        return Convert.ToInt32(Result);
    }

    public static DataTable CRM_GetShareUser(int LeadId, int UserId, int FinYearId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        //cmd.Parameters.Add("", SqlDbType.Int).Value = ShareUserId;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataTable("CRM_GetShareLead", cmd);
    }
    #endregion

    #region CRM find quatation approval lead

    public static DataTable CRM_GetQuoteApprovalLead(int LeadId, int UserId, int FinYearId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataTable("CRM_GetQuoteApprovalLead", cmd);
    }

    #endregion

    #region CRM_DASHBOARD GET ON BOARD VOLUMN SUMMARY
    public static DataTable CRM_GetOnBoardVolumnSummary(int MonthId, int FinYearId, int SalesPersonId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@MonthId", SqlDbType.Int).Value = MonthId;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        cmd.Parameters.Add("@SalesPersonId", SqlDbType.Int).Value = SalesPersonId;
        return CDatabase.GetDataTable("CRM_GetDashboardSummary", cmd);
    }
    #endregion

    #region update rfq status in CRM_LEAD

    public static int CRM_LeadUpdate_RFQStatus(int LeadId, int rfqStatus, int FinYearId)
    {
        SqlCommand cmd = new SqlCommand();
        string Result = "";

        cmd.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        cmd.Parameters.Add("@rfqStatus", SqlDbType.Int).Value = rfqStatus;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        cmd.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        Result = CDatabase.GetSPOutPut("CRM_updLead_RFQStatus", cmd, "@Output");
        return Convert.ToInt32(Result);
    }

    #endregion

    #region update CRM payment terms

    public static int CRM_updPaymentTerms(int LeadId, string PaymentTerms)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LeadId", SqlDbType.Int).Value = LeadId;
        cmd.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = PaymentTerms;
        cmd.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        string Result = CDatabase.GetSPOutPut("CRM_updPaymentTerms", cmd, "@Output");
        return Convert.ToInt32(Result);
    }
    #endregion

    #region CRM_MGMTreports

    public static DataTable CRM_rptMgmtReport(string Action_Perform, int Type, DateTime dtStartDate, DateTime dtEndDate, int lUser, int FinYearId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@Action_Perform", SqlDbType.VarChar).Value = Action_Perform;
        command.Parameters.Add("@Type", SqlDbType.Int).Value = Type;
        command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtStartDate;
        command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dtEndDate;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        return CDatabase.GetDataTable("CRM_rptMgmtReport", command);
    }

    #endregion

    #region Check KYC done
    public static DataTable CRM_CheckKYC(string LeadId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LeadId", LeadId);
        return CDatabase.GetDataTable("CRM_GetAllLeadStatusById", cmd);
    }
    #endregion

    #region Contract Copy
    public static DataTable CRM_GetContractCopy(int LeadId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@LeadId", LeadId);

        return CDatabase.GetDataTable("CRM_GetContractCopyDetail", cmd);
    }

    public static int ADDCustomerEBillConfirm(string strCustomerName, string strContactPerson,
           string strContactNumber, string strContactEmail, Boolean bEBillRequired, string strRemark)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = strCustomerName;
        cmd.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = strContactPerson;
        cmd.Parameters.Add("@ContactNumber", SqlDbType.NVarChar).Value = strContactNumber;
        cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar).Value = strContactEmail;
        cmd.Parameters.Add("@EBillRequired", SqlDbType.Bit).Value = bEBillRequired;
        cmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = strRemark;

        cmd.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        string Result = CDatabase.GetSPOutPut("ins_CustomerEBillConfirm", cmd, "@Output");
        return Convert.ToInt32(Result);
    }
    #endregion

    #region Bank API Status

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

        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;

        if (IsSuccess == 0)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = false;
        }
        else if (IsSuccess == 1)
        {
            command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = true;
        }

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updAPIBankTransaction", command, "@OutPut");

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

    #endregion

    #region My Pacco

    public static DataSet MyPaccoGetAuthTokan()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("MP_GetAuthToken", command);

    }
    public static int MyPaccoUpdateAuthTokan(string TokenNo, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@AuthTokan", SqlDbType.NVarChar).Value = TokenNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MP_updAuthToken", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int MyPaccoAddAWBNo(string strOrderNo, string strAWBNo, DateTime dtAWBDate, string strLSPName, string strJobIdList,
       int CustomerId, int BranchId, int PlantId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
        command.Parameters.Add("@AWBNo", SqlDbType.NVarChar).Value = strAWBNo;
        command.Parameters.Add("@AWBDate", SqlDbType.DateTime).Value = dtAWBDate;
        command.Parameters.Add("@LSPName", SqlDbType.NVarChar).Value = strLSPName;
        command.Parameters.Add("@JobIdList", SqlDbType.NVarChar).Value = strJobIdList;
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MP_insAWBNo", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet MyPaccoCheckAWBForJobList(string strJobIdList)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobIdList", SqlDbType.NVarChar).Value = strJobIdList;

        return CDatabase.GetDataSet("MP_CheckJobAWBExists", command);

    }
    public static DataSet MyPaccoGetJobListByAWB(int AWBId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@AWBId", SqlDbType.NVarChar).Value = AWBId;

        return CDatabase.GetDataSet("MP_GetAWBJobList", command);

    }
    public static int MyPaccoUpdateAWBNo(string strOrderNo, string strAWBNo, DateTime dtAWBDate, string strLSPName, int lStatus,
       DateTime dtStatusDate, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
        command.Parameters.Add("@AWBNo", SqlDbType.NVarChar).Value = strAWBNo;
        command.Parameters.Add("@AWBDate", SqlDbType.DateTime).Value = dtAWBDate;
        command.Parameters.Add("@LSPName", SqlDbType.NVarChar).Value = strLSPName;
        command.Parameters.Add("@lStatus", SqlDbType.Int).Value = lStatus;
        command.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = dtStatusDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MP_UpdAWBStatus", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int MyPaccoUpdateInActive(string strOrderNo, int lStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();
        int SPresult = 0;

        String SqlQury = "Update MP_MyPaccoAWB SET IsActive=0,updUser=1, updDate=GetDate() where OrderNo='" + strOrderNo +"'";
        //command.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = strOrderNo;
        //command.Parameters.Add("@lStatus", SqlDbType.Int).Value = lStatus;
        //command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        //command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.ExecuteSQL(SqlQury);
        return SPresult;
    }
    public static int MyPaccoAddLog(string APIAction, string AppUserName, string ErrCode, bool IsSuccess,
    string OutcomeMsg, string TxnDateTime, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@APIAction", SqlDbType.NVarChar).Value = APIAction;
        command.Parameters.Add("@AppUserName", SqlDbType.NVarChar).Value = AppUserName;
        command.Parameters.Add("@ErrCode", SqlDbType.NVarChar).Value = ErrCode;
        command.Parameters.Add("@IsSuccess", SqlDbType.Bit).Value = IsSuccess;
        command.Parameters.Add("@OutcomeMsg", SqlDbType.NVarChar).Value = OutcomeMsg;
        command.Parameters.Add("@TxnDateTime", SqlDbType.NVarChar).Value = TxnDateTime;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MP_insMyPaccoLog", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }
    #endregion

    public static DataSet GetBillDispatchDetail(int JobId, string JobRefNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        DataSet dsBillDispatch = CDatabase.GetDataSet("BS_BillDispatchMailDetail", command);

        return dsBillDispatch;
    }


    public static DataTable GetBillDoc(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataTable("GetPCDBillDispatchDoc", cmd);
    }

    public static DataTable GetTotalBillDoc()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataTable("GetBillDispatchDoc", cmd);
    }

    public static int AddDocPathForBillDispatch(string filename, string DocPath, int DocumentId, int JobId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDocPathForBillDispatch", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataTable GetAnnexureDetail(string JobRefNo, int FinYr, int BEType)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobRefNo", JobRefNo);
        cmd.Parameters.Add("@FinYr", FinYr);
        cmd.Parameters.Add("@BEType", BEType);
        return CDatabase.GetDataTable("GetAnnexureDetail", cmd);
    }

    // Export Job Cancel
    public static int Ex_InsJobStatus(int JobId, int StatusId, string Reason, string Remark, int FinYr, int ModuleId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = Reason;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYr;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insJobStatus", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    public static DataTable GetEXJobCancelDetail(int JobId, int FinYr)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", JobId);
        cmd.Parameters.Add("@FinYr", FinYr);
        return CDatabase.GetDataTable("Get_ExJobCancel", cmd);
    }

    public static int TR_updTransDetail(int TransReqId, decimal DetentionAmount, decimal VaraiExpense, decimal EmptyContAmt, decimal TollAmt, decimal OtherAmt,
        int DetentionDocId, string DetentionDocPath, string DetentionDocName, int VaraiDocId, string VaraiDocPath, string VaraiDocName,
        int EmptyDocId, string EmptyDocPath, string EmptyDocName, int TollDocId, string TollDocPath, string TollDocName,
        int OtherDocId, string OtherDocPath, string otherDocName, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiAmount", SqlDbType.Decimal).Value = VaraiExpense;
        command.Parameters.Add("@EmptyContAmt", SqlDbType.Decimal).Value = EmptyContAmt;
        command.Parameters.Add("@TollAmt", SqlDbType.Decimal).Value = TollAmt;
        command.Parameters.Add("@OtherAmt", SqlDbType.Decimal).Value = OtherAmt;

        command.Parameters.Add("@DetentionDocId", SqlDbType.Int).Value = DetentionDocId;
        command.Parameters.Add("@DetentionDocPath", SqlDbType.NVarChar).Value = DetentionDocPath;
        command.Parameters.Add("@DetentionDocName", SqlDbType.NVarChar).Value = DetentionDocName;
        command.Parameters.Add("@VaraiDocId", SqlDbType.Int).Value = VaraiDocId;
        command.Parameters.Add("@VaraiDocPath", SqlDbType.NVarChar).Value = VaraiDocPath;
        command.Parameters.Add("@VaraiDocName", SqlDbType.NVarChar).Value = VaraiDocName;

        command.Parameters.Add("@EmtpyContDocId", SqlDbType.Int).Value = EmptyDocId;
        command.Parameters.Add("@EmtpyContDocPath", SqlDbType.NVarChar).Value = EmptyDocPath;
        command.Parameters.Add("@EmtpyContDocName", SqlDbType.NVarChar).Value = EmptyDocName;
        command.Parameters.Add("@TollDocId", SqlDbType.Int).Value = TollDocId;
        command.Parameters.Add("@TollDocPath", SqlDbType.NVarChar).Value = TollDocPath;
        command.Parameters.Add("@TollDocName", SqlDbType.NVarChar).Value = TollDocName;
        command.Parameters.Add("@OtherDocId", SqlDbType.Int).Value = OtherDocId;
        command.Parameters.Add("@OtherDocPath", SqlDbType.NVarChar).Value = OtherDocPath;
        command.Parameters.Add("@OtherDocName", SqlDbType.NVarChar).Value = otherDocName;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updTransDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataTable Get_TransDetail(int TransReqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@TransReqId", TransReqId);
        return CDatabase.GetDataTable("TR_GetTransDetail", cmd);
    }

    //  FREIGHT IMPORT JOB CANCEL DETAIL
    public static DataTable Get_FRPrealertDetail(int EnqId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ENQID", EnqId);
        return CDatabase.GetDataTable("Get_FRCustPreAlertDetail", cmd);
    }

    public static int FR_InsJobStatus(int EnqId, int StatusId, string Reason, string Remark, int FinYr, int ModuleId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = Reason;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@FinYear", SqlDbType.Int).Value = FinYr;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insJobStatus", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    //Get SKF Cust Ref No
    public static DataTable GetInbondCustRefNo(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("JobId", JobId);
        return CDatabase.GetDataTable("GetInbondCustRefNo", cmd);
    }

    public static DataTable FillPCDDocument(int Type)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobType", SqlDbType.Int).Value = Type;
        return CDatabase.GetDataTable("Get_FRDocumentType", command);
    }

    public static DataTable FillAgentDetails(int EnqID)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqID;
        return CDatabase.GetDataTable("FOP_GetAgentInvoiceDetail", cmd);
    }

    public static string GetCustDetail(int CustID)
    {
        string CustDetail = "";
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@BranchID", SqlDbType.Int).Value = CustID;
        //command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

        CustDetail = CDatabase.GetSPOutPut("GetCustDetail", command, "@OutPut");

        return CustDetail;
    }

    #region freight Document
    public static DataTable Get_CompFRDocumentType(string JobType, string JobMode)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobType", SqlDbType.NVarChar).Value = JobType;
        cmd.Parameters.Add("@JobMode", SqlDbType.NVarChar).Value = JobMode;

        return CDatabase.GetDataTable("Get_CompFRDocumentType", cmd);
    }

    public static DataSet GetDocumentMaster()
    {
        SqlCommand command = new SqlCommand();
        return CDatabase.GetDataSet("Get_FRDocument", "command");
    }
    #endregion

    #region Document Master

    public static int AddDocument(string DocumentName, string OprationType, string OperationMode, int Compulsary, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@Documentname", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@OperationType", SqlDbType.NVarChar).Value = OprationType;
        command.Parameters.Add("@OperationMode", SqlDbType.NVarChar).Value = OperationMode;
        command.Parameters.Add("@Compulsary", SqlDbType.NVarChar).Value = Compulsary;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insDocumentMaster", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateDocument(int lid, string DocumentName, string OprationType, string OperationMode, int Compulsary, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@Lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@Documentname", SqlDbType.NVarChar).Value = DocumentName;
        command.Parameters.Add("@OprationType", SqlDbType.NVarChar).Value = OprationType;
        command.Parameters.Add("@OperationMode", SqlDbType.NVarChar).Value = OperationMode;
        command.Parameters.Add("@Compulsary", SqlDbType.NVarChar).Value = Compulsary;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updDocument", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int DeleteDocument(int lid, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string Spresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        Spresult = CDatabase.GetSPOutPut("delDocument", command, "@OutPut");

        return Convert.ToInt32(Spresult);
    }

    #endregion

    #region Billing Instruction

    public static DataTable Get_JobDetailForBillingInstruction(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        return CDatabase.GetDataTable("Get_JobDetailForBillingInstructionByJobId", command);
    }

    //Import Module
    public static int BS_AddBillingInstruction(int JobId, int AlliedAgencyServiceApply, string AlliedAgencyService, string AlliedAgencyRemark, int DeliveyRelatedApply,
        string DeliveyRelated, int VASApply, string VAS, string VASRemark, int CECertificateApply, string CECertificate, string CeCertificateRemark,
        int WithoutLRStatus, string EmailApprovalCopy, string ConsignmentType, string Instruction, string Instruction1, string Instruction2, string Instruction3,
        string InstructioCopy, string InstructioCopy1, string InstructioCopy2, string InstructioCopy3, string OtherService, string OtherServiceRemark, int UserId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@AlliedAgencyServiceApply", SqlDbType.Int).Value = AlliedAgencyServiceApply;
        cmd.Parameters.Add("@AlliedAgencyService", SqlDbType.NVarChar).Value = AlliedAgencyService;
        cmd.Parameters.Add("@AlliedAgencyRemark", SqlDbType.NVarChar).Value = AlliedAgencyRemark;
        cmd.Parameters.Add("@DeliveyRelatedApply", SqlDbType.Int).Value = DeliveyRelatedApply;
        cmd.Parameters.Add("@DeliveyRelated", SqlDbType.NVarChar).Value = DeliveyRelated;
        cmd.Parameters.Add("@VASApply", SqlDbType.Int).Value = VASApply;
        cmd.Parameters.Add("@VAS", SqlDbType.NVarChar).Value = VAS;
        cmd.Parameters.Add("@VASRemark", SqlDbType.NVarChar).Value = VASRemark;
        cmd.Parameters.Add("@CECertificateApply", SqlDbType.Int).Value = CECertificateApply;
        cmd.Parameters.Add("@CECertificate", SqlDbType.NVarChar).Value = CECertificate;
        cmd.Parameters.Add("@CeCertificateRemark", SqlDbType.NVarChar).Value = CeCertificateRemark;
        cmd.Parameters.Add("@WithoutLRStatus", SqlDbType.Int).Value = WithoutLRStatus;
        cmd.Parameters.Add("@EmailApprovalCopy", SqlDbType.NVarChar).Value = EmailApprovalCopy;
        cmd.Parameters.Add("@ConsignmentType", SqlDbType.NVarChar).Value = ConsignmentType;
        cmd.Parameters.Add("@Instruction", SqlDbType.NVarChar).Value = Instruction;
        cmd.Parameters.Add("@Instruction1", SqlDbType.NVarChar).Value = Instruction1;
        cmd.Parameters.Add("@Instruction2", SqlDbType.NVarChar).Value = Instruction2;
        cmd.Parameters.Add("@Instruction3", SqlDbType.NVarChar).Value = Instruction3;
        cmd.Parameters.Add("@InstructionCopy", SqlDbType.NVarChar).Value = InstructioCopy;
        cmd.Parameters.Add("@InstructionCopy1", SqlDbType.NVarChar).Value = InstructioCopy1;
        cmd.Parameters.Add("@InstructionCopy2", SqlDbType.NVarChar).Value = InstructioCopy2;
        cmd.Parameters.Add("@InstructionCopy3", SqlDbType.NVarChar).Value = InstructioCopy3;
        cmd.Parameters.Add("@OtherService", SqlDbType.NVarChar).Value = OtherService;
        cmd.Parameters.Add("@OtherServiceRemark", SqlDbType.NVarChar).Value = OtherServiceRemark;
        cmd.Parameters.Add("@luser", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_insBillingInstructionDetail", cmd, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    //Export Module
    public static int EX_AddBillingInstruction(int JobId, string AlliedAgencyService, string AlliedAgencyRemark, string OtherService, string OtherServiceRemark,
        string Instruction, string Instruction1, string Instruction2, string Instruction3, string InstructioCopy, string InstructioCopy1, string InstructioCopy2, string InstructioCopy3,
        int UserId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
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

        SPresult = CDatabase.GetSPOutPut("EX_insBillingInstructionDetail", cmd, "@OutPut");
        return Convert.ToInt32(SPresult);

    }


    //Freight Module
    public static int FR_AddBillingInstruction(int JobId, int AlliedAgencyServiceApply, string AlliedAgencyService, string AlliedAgencyRemark, int DeliveyRelatedApply,
       string DeliveyRelated, int VASApply, string VAS, string VASRemark, int CECertificateApply, string CECertificate, string CeCertificateRemark,
       int WithoutLRStatus, string EmailApprovalCopy, string ConsignmentType, string Instruction, string InstructioCopy, int UserId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@AlliedAgencyServiceApply", SqlDbType.Int).Value = AlliedAgencyServiceApply;
        cmd.Parameters.Add("@AlliedAgencyService", SqlDbType.NVarChar).Value = AlliedAgencyService;
        cmd.Parameters.Add("@AlliedAgencyRemark", SqlDbType.NVarChar).Value = AlliedAgencyRemark;
        cmd.Parameters.Add("@DeliveyRelatedApply", SqlDbType.Int).Value = DeliveyRelatedApply;
        cmd.Parameters.Add("@DeliveyRelated", SqlDbType.NVarChar).Value = DeliveyRelated;
        cmd.Parameters.Add("@VASApply", SqlDbType.Int).Value = VASApply;
        cmd.Parameters.Add("@VAS", SqlDbType.NVarChar).Value = VAS;
        cmd.Parameters.Add("@VASRemark", SqlDbType.NVarChar).Value = VASRemark;
        cmd.Parameters.Add("@CECertificateApply", SqlDbType.Int).Value = CECertificateApply;
        cmd.Parameters.Add("@CECertificate", SqlDbType.NVarChar).Value = CECertificate;
        cmd.Parameters.Add("@CeCertificateRemark", SqlDbType.NVarChar).Value = CeCertificateRemark;
        cmd.Parameters.Add("@WithoutLRStatus", SqlDbType.Int).Value = WithoutLRStatus;
        cmd.Parameters.Add("@EmailApprovalCopy", SqlDbType.NVarChar).Value = EmailApprovalCopy;
        cmd.Parameters.Add("@ConsignmentType", SqlDbType.NVarChar).Value = ConsignmentType;
        cmd.Parameters.Add("@Instruction", SqlDbType.NVarChar).Value = Instruction;
        cmd.Parameters.Add("@InstructionCopy", SqlDbType.NVarChar).Value = InstructioCopy;
        cmd.Parameters.Add("@luser", SqlDbType.Int).Value = UserId;
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insBillingInstructionDetail", cmd, "@OutPut");
        return Convert.ToInt32(SPresult);

    }


    public static DataTable Get_BillingInstructionDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        return CDatabase.GetDataTable("Get_BillingInstructionDetail", command);
    }

    #endregion

    #region Export Hold option
    public static int EX_AddHoldBillingAdvice(int JobId, string Remark, string RejectType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@RejectType", SqlDbType.NVarChar).Value = RejectType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("EX_insHoldBillingAdvice", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    #endregion

    public static DataSet GetFRJobDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("Get_FRJobDetailById", command);
    }

    public static int FR_AddHoldBillingAdvice(int JobId, string Remark, string RejectType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@RejectType", SqlDbType.NVarChar).Value = RejectType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser; 
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_insHoldBillingAdvice", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetBillDispatchEmail(int JobId, string JobRefNo)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;

        DataSet dsBillDispatch = CDatabase.GetDataSet("DS_GetBillDispatchEmail", command);

        return dsBillDispatch;
    }

    public static DataTable Get_DocumentTypeId()
    {
        SqlCommand cmd = new SqlCommand();
        return CDatabase.GetDataTable("GetPreAlertDocList", cmd);
    }

    public static DataTable GetFinalCheckComplete(int JobId, int ModuleId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add(new SqlParameter("@JobId", JobId));
        cmd.Parameters.Add(new SqlParameter("@ModuleId", ModuleId));

        return CDatabase.GetDataTable("GetFinalCheckComplete", cmd);
    }

    public static DataTable GetKYCVendorId(int leadId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add(new SqlParameter("@leadId", leadId));

        return CDatabase.GetDataTable("GetKYCVendorId", cmd);
    }

    public static DataTable GetAgentDetail(int CheckValue, string Number)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@CheckValue", SqlDbType.Int).Value = CheckValue;
        cmd.Parameters.Add("@NUMBER", SqlDbType.NVarChar).Value = Number;
        return CDatabase.GetDataTable("FOP_GetAgentDetails", cmd);
    }
    #region Transporter Payment
    public static int AddTransPayStatus(int RequestId, int StatusId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insStatusHistory", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddTransPayFromBank(int RequestId, bool IsFundTransFromAPI, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@IsFundTransFromAPI", SqlDbType.Bit).Value = IsFundTransFromAPI;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updTransPayFrom", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataView GetTransportFundRequest(int RequestId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;

        return CDatabase.GetDataView("TRS_GetInvoiceById", cmd);
    }

    public static DataSet GetTransportFundRequest2(int RequestId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;

        return CDatabase.GetDataSet("TRS_GetInvoiceById", cmd);
    }

    public static int AddTransPayTDS(int RequestId, bool tdsApplicable, string strTDSLedgerCode, int TDSRateTpe, decimal decTDSRate, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@TDSApplicable", SqlDbType.Bit).Value = tdsApplicable;
        command.Parameters.Add("@TDSLedgerCode", SqlDbType.NVarChar).Value = strTDSLedgerCode;
        command.Parameters.Add("@TDSRateType", SqlDbType.Int).Value = TDSRateTpe;
        command.Parameters.Add("@TDSRate", SqlDbType.Decimal).Value = decTDSRate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insPaymentTDS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int DeleteTransPayTDS(int ItemId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_delPaymentTDSByID", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetTransPayPayment(int RequestID)
    {
        // Get All Payment
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@RequestID", SqlDbType.Int).Value = RequestID;

        return CDatabase.GetDataSet("TRS_GetPaymentDetail", command);
    }
    public static DataSet GetTransPayPaymentItem(int RequestID, int ItemId)
    {
        // Get All Payment
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@RequestID", SqlDbType.Int).Value = RequestID;
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;

        return CDatabase.GetDataSet("TRS_GetPaymentItemById", command);
    }
    public static int AddTransPayPaymentDeduction(int RequestID, int ItemId, Decimal Deduction, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@RequestID", SqlDbType.Int).Value = RequestID;
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
        command.Parameters.Add("@Deduction", SqlDbType.Decimal).Value = Deduction;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentItemDeduction", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetTransPayPendingPayment(int RequestID)
    {
        // Get All Payment
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@RequestID", SqlDbType.Int).Value = RequestID;

        return CDatabase.GetDataSet("TRS_GetPendingPayment", command);
    }
    public static DataSet GetTransPaymentDetail(int PaymentID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = PaymentID;

        return CDatabase.GetDataSet("TRS_GetInvoicePaymentById", command);
    }
    public static DataSet GetTransPayDocument(int PayRequestId, int DocumentId)
    {
        // Get All Transport Payment Document Set DocumentId = 0
        // Get Specific Document - Provide DocumentID - AC_ExpenseDocDetails Table Promery Key - lid
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PayRequestId", SqlDbType.Int).Value = PayRequestId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;

        return CDatabase.GetDataSet("TRS_GetInvoiceDocument", command);
    }
    
    public static DataSet GetTransPayDocByTypeID(int PayRequestId, int DocumentTypeId)
    {
        // Get All Transport Document By Document Type
        // 1 - Invoice / 2 - Email Approval Copy / 10 - Payment Challan / 11 - Payment Receipt 
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PayRequestId", SqlDbType.Int).Value = PayRequestId;
        command.Parameters.Add("@DocTypeID", SqlDbType.Int).Value = DocumentTypeId;

        return CDatabase.GetDataSet("TRS_GetDocumentByType", command);
    }

    public static int CheckTransActiveTransaction(int RequestId, int PaymentId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_checkAPIActiveTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransFunRequest(int JobId, string JobRefNo, int TransReqId, int TransporterId, int ExpenseTypeId, int PaymentTypeId,
        decimal Amount, int BranchId, string Remark, Boolean AdvanceReceived, decimal AdvanceAmt, string Total_Amnt, int ModuleId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@TransReqId", SqlDbType.NVarChar).Value = TransReqId;
        command.Parameters.Add("@TransporterId", SqlDbType.NVarChar).Value = TransporterId;
        command.Parameters.Add("@RequestTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AdvanceReceived", SqlDbType.Bit).Value = AdvanceReceived;
        command.Parameters.Add("@AdvanceAmt", SqlDbType.Decimal).Value = AdvanceAmt;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@Total_Amnt", SqlDbType.Decimal).Value = Total_Amnt;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insFundRequest", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTransFunRequest(int FundRequestID, int JobId, string JobRefNo, int TransReqId, int TransporterId, int ExpenseTypeId, int PaymentTypeId,
        decimal Amount, int BranchId, string Remark, Boolean AdvanceReceived, decimal AdvanceAmt, string Total_Amnt, int ModuleId, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@RequestID", SqlDbType.Int).Value = FundRequestID;
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@JobRefNo", SqlDbType.NVarChar).Value = JobRefNo;
        command.Parameters.Add("@TransReqId", SqlDbType.NVarChar).Value = TransReqId;
        command.Parameters.Add("@TransporterId", SqlDbType.NVarChar).Value = TransporterId;
        command.Parameters.Add("@RequestTypeId", SqlDbType.Int).Value = ExpenseTypeId;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;
        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@AdvanceReceived", SqlDbType.Bit).Value = AdvanceReceived;
        command.Parameters.Add("@AdvanceAmt", SqlDbType.Decimal).Value = AdvanceAmt;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@Total_Amnt", SqlDbType.Decimal).Value = Total_Amnt;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updFundRequest", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddTransPayPaymentRequest(int RequestId, Boolean IsFullPayment, int PaymentTypeId, int BabajiBankId, int BankAccountId,
       bool IsFundTransFromAPI, int VendorBankAccountId, Decimal PaidAmount, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@IsFullPayment", SqlDbType.Int).Value = IsFullPayment;
        command.Parameters.Add("@PaymentTypeId", SqlDbType.Int).Value = PaymentTypeId;
        command.Parameters.Add("@BabajiBankId", SqlDbType.NVarChar).Value = BabajiBankId;
        command.Parameters.Add("@BankAccountId", SqlDbType.NVarChar).Value = BankAccountId;
        command.Parameters.Add("@IsFundTransFromAPI", SqlDbType.Bit).Value = IsFundTransFromAPI;
        command.Parameters.Add("@VendorBankAccountId", SqlDbType.NVarChar).Value = VendorBankAccountId;
        command.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = PaidAmount;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayPayment(int RequestId, int PaymentId, string InstrumentNo, DateTime InstrumentDate,
        Decimal PaidAmount, string DocPath, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
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

        SPresult = CDatabase.GetSPOutPut("TRS_insTranPayPayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int UpdateFailedTransPayToBankTransfer(int RequestId, int PaymentId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updAPIBankFailedTransaction", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayBacth(int RequestId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insBatchPayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransMempPayBacth(int MemoId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoId", SqlDbType.Int).Value = MemoId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insMemoBatchPayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransBillPaymentRequest(int BillId, int TransReqId, string ApprovalRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = ApprovalRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insFundRequestBill", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int UpdateTransBillPaymentRequest(int PaymentId, int BillId, int TransReqId, int TransporterID, string ApprovalRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@BillId", SqlDbType.Int).Value = BillId;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = ApprovalRemark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updFundRequestBill", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static int AddTransInvoiceDocument(int PaymentId, int DocumentId, string DocPath, string FileName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@DocumentId", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insInvoiceDocument", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    #endregion
    #region Transporter Bill
    public static void FillTransporterBank(DropDownList DropDown, int TransporterId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@TransporterId", SqlDbType.Int).Value = TransporterId;

        CDatabase.BindControls(DropDown, "BS_GetTransporterBankDetails", command, "BankName", "lid");
    }
    public static DataSet GetTransporterBankDetail(int TransporterBankId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@TransporterBankId", SqlDbType.Int).Value = TransporterBankId;

        return CDatabase.GetDataSet("BS_GetTransporterBankDetailById", command);
    }
    public static DataView GetTransportRequest(int TransReqId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;

        return CDatabase.GetDataView("TRS_GetTruckRequestById", cmd);
    }
    public static DataSet GetJobDetailForTransport(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("TRS_GetJobDetailById", command);
    }

    public static int AddTransBillDetail(int TransReqId, int TransporterID, string BillNumber, DateTime BillDate,
        string TotalAmount, string Justification, string DocName, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.NVarChar).Value = TransReqId;
        command.Parameters.Add("@TransporterID", SqlDbType.NVarChar).Value = TransporterID;

        command.Parameters.Add("@BillNumber", SqlDbType.NVarChar).Value = BillNumber;
        command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = BillDate;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;

        command.Parameters.Add("@Justification", SqlDbType.NVarChar).Value = Justification;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insTransBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int UpdateTransBillDetail(int PaymentId, int TransReqId, int TransporterID, string BillNumber, DateTime BillDate,
        string BillAmount, string DetentionAmount, string VaraiAmount, string EmptyContRcptCharges, string TollCharges, string OtherCharges,
        string TotalAmount, string Justification, bool IsConsolidated, int ConsolidateID, string DocName, string DocPath, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@PaymentId", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;

        command.Parameters.Add("@BillNumber", SqlDbType.NVarChar).Value = BillNumber;
        if (BillDate != DateTime.MinValue)
            command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = BillDate;

        command.Parameters.Add("@BillAmount", SqlDbType.Decimal).Value = BillAmount;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiAmount", SqlDbType.Decimal).Value = VaraiAmount;
        command.Parameters.Add("@EmptyContRcptCharges", SqlDbType.Decimal).Value = EmptyContRcptCharges;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = TotalAmount;

        command.Parameters.Add("@Justification", SqlDbType.NVarChar).Value = Justification;
        command.Parameters.Add("@IsConsolidated", SqlDbType.Bit).Value = IsConsolidated;
        command.Parameters.Add("@ConsolidateID", SqlDbType.Int).Value = ConsolidateID;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updTransBillDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int UpdateTransportRate(int TransReqId, int RateId, string VehicleNo,
        decimal Rate, decimal FreightAmount, decimal DetentionAmount, decimal VaraiExpense,
        decimal EmptyContRecptCharges, decimal TollCharges, decimal OtherCharges,
        string LRAttacment, string ChallanAttachment, string EmptyReceiptAttachment, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;

        command.Parameters.Add("@RateId", SqlDbType.Int).Value = RateId;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Rate;
        command.Parameters.Add("@FreightAmount", SqlDbType.Decimal).Value = FreightAmount;
        command.Parameters.Add("@DetentionAmount", SqlDbType.Decimal).Value = DetentionAmount;
        command.Parameters.Add("@VaraiExpense", SqlDbType.Decimal).Value = VaraiExpense;
        command.Parameters.Add("@EmptyCharges", SqlDbType.Decimal).Value = EmptyContRecptCharges;
        command.Parameters.Add("@TollCharges", SqlDbType.Decimal).Value = TollCharges;
        command.Parameters.Add("@OtherCharges", SqlDbType.Decimal).Value = OtherCharges;
        command.Parameters.Add("@LRAttacment", SqlDbType.NVarChar).Value = LRAttacment;
        command.Parameters.Add("@ChallanAttachment", SqlDbType.NVarChar).Value = ChallanAttachment;
        command.Parameters.Add("@EmptyAttachment", SqlDbType.NVarChar).Value = EmptyReceiptAttachment;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updTransRate", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    
    public static int AddBillReceivedDetail(int TransBillId, int StatusId, string Remark, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransBillId", SqlDbType.Int).Value = TransBillId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_updBillReceivedDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    #endregion
    #region Transporter Memo
    public static string GenerateTransMemoRefNo()
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("AC_GetNewMemoRefNo", command, "@OutPut");

        return Convert.ToString(SPresult);
    }
    public static int AddTransMemoStatus(int MemoId, int StatusId, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoId", SqlDbType.Int).Value = MemoId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@sRemark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insMemoStatus", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayMemo(int VendorId, decimal TotalMemoAmount, int TotalRequest, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@TotalRequest", SqlDbType.Int).Value = TotalRequest;
        command.Parameters.Add("@TotalMemoAmount", SqlDbType.Int).Value = TotalMemoAmount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemo", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayMemoDetail(int RequestId, int PaymentId, int MemoID, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@RequestId", SqlDbType.Int).Value = RequestId;
        command.Parameters.Add("@PaymentID", SqlDbType.Int).Value = PaymentId;
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemoDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayMemoAudit(int MemoID, int ApprovalStatus, int InvoiceStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = ApprovalStatus;
        command.Parameters.Add("@InvoiceStatus", SqlDbType.Int).Value = InvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemoAudit", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayMemoApproval(int MemoID, int ApprovalStatus, int InvoiceStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = ApprovalStatus;
        command.Parameters.Add("@InvoiceStatus", SqlDbType.Int).Value = InvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemoAppoval", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayMemoPayment(int MemoID, int MemoPaymentStatus, int InvoiceStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@MemoPaymentStatus", SqlDbType.Int).Value = MemoPaymentStatus;
        command.Parameters.Add("@InvoiceStatus", SqlDbType.Int).Value = InvoiceStatus;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemoPayment", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetTransMemoDetail(int MemoId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@MemoId", SqlDbType.Int).Value = MemoId;

        return CDatabase.GetDataSet("TRS_GetTransMemoById", command);

    }
    public static int AddTransPayMemoCancel(int MemoID, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemoCancel", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    public static int AddTransPayMemoReject(int MemoID, string Remark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@MemoID", SqlDbType.Int).Value = MemoID;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TRS_insPaymentMemoReject", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    #endregion
    #region job ref no
    public static string TR_GetNextOtherJobNo(int BranchId)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = BranchId;
        cmd.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_GetNextOtherJobNo", cmd, "@OutPut");

        return Convert.ToString(SPresult);
    }
    #endregion

    #region HBL
    public static string GetHAWBDetail(int EnqId, string HAWBNo)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;
        cmd.Parameters.Add("@HAWBNo", SqlDbType.NVarChar).Value = HAWBNo;
        cmd.Parameters.Add(new SqlParameter("@OutPut", SqlDbType.NVarChar, 200)).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("GetHAWBDetail", cmd, "@OutPut");

        return SPresult;
    }
    #endregion

    #region Billing Turnaround
    public static DataView GetBillingTurnaroundReport(int ReportId, DateTime DateFrom, DateTime DateTo, string DeliveryStatus, string AdhocFilter, int FinYear, int UserId)
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

        return CDatabase.GetDataView("rptBillingTurnarounReport", cmd);
    }

    #endregion

    #region Noting Detail
    public static DataView GetJobDetailForNoting(int JobId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataView("GetNoingDetails", cmd);
    }
    #endregion

    #region billing instruction
    public static int Get_insBillingInstructionRef(string Value, int JobId, int InstructionId, int ReadBy, int ChargeBy, int ModuleId, int Luser, int updBy)
    {
        SqlCommand cmd = new SqlCommand();
        string SPresult = "";

        cmd.Parameters.Add("@Value", SqlDbType.NVarChar).Value = Value;
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@InstructionId", SqlDbType.Int).Value = InstructionId;
        cmd.Parameters.Add("@ReadBy", SqlDbType.Int).Value = ReadBy;
        // cmd.Parameters.Add("@ReadDate", SqlDbType.NVarChar).Value = Number;
        cmd.Parameters.Add("@ChargeBy", SqlDbType.Int).Value = ChargeBy;
        // cmd.Parameters.Add("@ChargeDate", SqlDbType.NVarChar).Value = Number;
        cmd.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        cmd.Parameters.Add("@lUser", SqlDbType.Int).Value = Luser;
        cmd.Parameters.Add("@updBy", SqlDbType.Int).Value = updBy;
        cmd.Parameters.Add(new SqlParameter("@OutPut", 0)).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("BS_InsBillingInstructionRef", cmd, "@Output");
        return Convert.ToInt32(SPresult);
    }
    #endregion

    public static DataSet GetCustomerIdByName(string CustomerName)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = CustomerName;

        return CDatabase.GetDataSet("FOP_GetCustomerIdByName", command);
    }

    public static DataTable GetExpenseAmount(int ExpenseId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = ExpenseId;
        return CDatabase.GetDataTable("CheckExpenseAmountJobId", cmd);
    }

    public static DataTable GetJobExpenseUserList(string Parameter)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@myParameter", SqlDbType.NVarChar).Value = Parameter;
        return CDatabase.GetDataTable("GetJobExpenseUserList", cmd);
    }

    #region CFS By Branch
    public static int UpdateCFSByBranch(int lid, int CFSId, int CFSUserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;
        command.Parameters.Add("@CFSUserId", SqlDbType.Int).Value = CFSUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("UpdateCFSByBranch", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddBranchCFS(int BranchId, int CFSId, int CFSUserId, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@CFSId", SqlDbType.Int).Value = CFSId;
        command.Parameters.Add("@CFSUserId", SqlDbType.Int).Value = CFSUserId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insBranchCFS", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataTable GetCFSDetailByBranch(int lid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@lid", SqlDbType.NVarChar).Value = lid;
        return CDatabase.GetDataTable("CFSDetailByBranch", cmd);
    }
    #endregion

    public static DataTable BS_GetDocPendingJobs(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        //cmd.Parameters.Add("@BOEType", SqlDbType.Int).Value = BOEType;
        return CDatabase.GetDataTable("BS_GetDocPendingJobs", cmd);
    }

    public static DataTable BS_GetExistPrealertDoc(int Jobid, string Param)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JOBID", SqlDbType.Int).Value = Jobid;
        cmd.Parameters.Add("@Param", SqlDbType.NVarChar).Value = Param;
        return CDatabase.GetDataTable("BS_GetExistPrealertDoc", cmd);
    }

    public static DataTable BS_GetExistDocForDeliveryPlanning(int JobId, int BOEType)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        cmd.Parameters.Add("@BOEType", SqlDbType.Int).Value = BOEType;
        return CDatabase.GetDataTable("BS_GetExistDocForDeliveryPlanning", cmd);
    }

    #region WabTec DSr
    public static DataTable WabTecDSR(int lUser, int FinYearId, int Status, string Consignee, DateTime dtStartDate, DateTime dtEndDate)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = Consignee;
        command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtStartDate;
        command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtEndDate;
        return CDatabase.GetDataTable("WabTec_DSR_Report", command);
    }

    public static DataTable WabTecDSRForBilling(int lUser, int FinYearId, string Consignee, DateTime dtStartDate, DateTime dtEndDate)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = Consignee;
        command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtStartDate;
        command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtEndDate;
        return CDatabase.GetDataTable("WabTec_BillingDSR_Report", command);
    }

    public static DataTable CustWabTecDSR(int lUser, int FinYearId, int Status, string Consignee, DateTime dtStartDate, DateTime dtEndDate)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@Consignee", SqlDbType.NVarChar).Value = Consignee;
        command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtStartDate;
        command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtEndDate;
        return CDatabase.GetDataTable("WabTec_DSR_ReportTest", command);
    }
    #endregion

    public static int FR_UPDDeliveryDetails(int JobId, int DeliveryStatus, int DeliveryType, DateTime TruckRequestDate, DateTime JobDeliveryDate, string DeliveryIns,
             string DeliveryDestination, string DeliveryAddress, bool TransportationByBabaji, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@DeliveryStatus", SqlDbType.Int).Value = DeliveryStatus;
        if (TruckRequestDate != DateTime.MinValue)
        {
            command.Parameters.Add("@TruckRequestDate", SqlDbType.DateTime).Value = TruckRequestDate;
        }
        if (JobDeliveryDate != DateTime.MinValue)
        {
            command.Parameters.Add("@JobDeliveryDate", SqlDbType.DateTime).Value = JobDeliveryDate;
        }

        command.Parameters.Add("@DeliveryIns", SqlDbType.NVarChar).Value = DeliveryIns;
        command.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar).Value = DeliveryAddress;
        command.Parameters.Add("@DeliveryDestination", SqlDbType.NVarChar).Value = DeliveryDestination;
        command.Parameters.Add("@TransportationByBabaji", SqlDbType.Bit).Value = TransportationByBabaji;

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("FR_UPDDeliveryDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #region Thermax Prealert
    public static DataTable Thermaxprealert( int FinYearId, int Status,  DateTime dtStartDate, DateTime dtEndDate)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYearId;
        command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dtStartDate;
        command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dtEndDate;
        command.Parameters.Add("@Status", SqlDbType.Int).Value = Status;
        return CDatabase.GetDataTable("GetThermaxPrealertReport", command);
    }
    #endregion

    #region Weekly Visit report
    public static DataTable get_CRMWeeklyVisit(DateTime StartDt, DateTime Enddate, int luser, int FinYr, int Isvisit)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = Enddate;
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = luser;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        cmd.Parameters.Add("@IsVisitReport", SqlDbType.Int).Value = Isvisit;
        return CDatabase.GetDataTable("CRM_rptWeeklyVisit", cmd);
    }

    public static DataTable get_CRMPendingEnquiry(int luser, DateTime StartDt, DateTime EndDt, int FinYr)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = luser;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        return CDatabase.GetDataTable("CRM_GetPendingEnquiryLast1Month", cmd);
    }

    public static DataTable getCRMOnBoardCustomer(int FinYr, DateTime StartDt, DateTime EndDt, int SalesPersonId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@SalesPersonId", SqlDbType.Int).Value = SalesPersonId;
        return CDatabase.GetDataTable("CRMGetCustomerOnBoard", cmd);

    }
    public static DataTable getCRMVolumeAnalysisImportCHA(int FinYr, DateTime StartDt, DateTime EndDt, int SalesPersonId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@SalesPersonId", SqlDbType.Int).Value = SalesPersonId;
        return CDatabase.GetDataTable("CRMVolumeAnalysisImportCHA", cmd);

    }

    public static DataTable getCRMVolumeAnalysis_Freight(int FinYr, DateTime StartDt, DateTime EndDt, int SalesPersonId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@SalesPersonId", SqlDbType.Int).Value = SalesPersonId;
        return CDatabase.GetDataTable("CRMVolumeAnalysis_Freight", cmd);

    }

    public static DataTable get_CRMPendingEnquiryLast2Month(int luser, DateTime StartDt, DateTime EndDt, int FinYr)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = luser;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        return CDatabase.GetDataTable("CRM_GetPendingEnquiryLast2Month", cmd);
    }

    public static DataTable getCRMVolumeAnalysisExportCHA(int FinYr, DateTime StartDt, DateTime EndDt, int SalesPersonId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        cmd.Parameters.Add("@SalesPersonId", SqlDbType.Int).Value = SalesPersonId;
        return CDatabase.GetDataTable("CRMrptVolumeAnalysisExportCHA", cmd);

    }

    public static DataTable get_PendingFRSummary(int luser, DateTime StartDt, DateTime EndDt, int FinYr)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = luser;
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = StartDt;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = EndDt;
        cmd.Parameters.Add("@FinYearId", SqlDbType.Int).Value = FinYr;
        return CDatabase.GetDataTable("FRSummaryForCRMReport", cmd);
    }
    #endregion

    #region truck request
    public static int UpdateTransRequest(int JobId, string LocTo, string Remark, int DeliveryType, string Dimension, DateTime VehiclePlaceRequireDate, string FileName, string DocPath, int DocType,
                                          int PickupPincode, string PickupState, string PickupCity, int DropPincode, string DropState, string DropCity, string EmptyLetter, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@LocTo", SqlDbType.NVarChar).Value = LocTo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@VehiclePlaceRequireDate", SqlDbType.DateTime).Value = VehiclePlaceRequireDate;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;
        command.Parameters.Add("@PickupPincode", SqlDbType.Int).Value = PickupPincode;                         //added changage for update pincode state city trucrequest details
        command.Parameters.Add("@PickupState", SqlDbType.NVarChar).Value = PickupState;
        command.Parameters.Add("@PickupCity", SqlDbType.NVarChar).Value = PickupCity;
        command.Parameters.Add("@DropPincode", SqlDbType.Int).Value = DropPincode;
        command.Parameters.Add("@DropState", SqlDbType.NVarChar).Value = DropState;
        command.Parameters.Add("@DropCity", SqlDbType.NVarChar).Value = DropCity;
        command.Parameters.Add("@EmptyLetter", SqlDbType.NVarChar).Value = EmptyLetter;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_UpdJobTransportRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int UpdateTransportRequest(int JobId, string LocTo, string Remark, int DeliveryType, string Dimension, DateTime VehiclePlaceRequireDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@LocTo", SqlDbType.NVarChar).Value = LocTo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@VehiclePlaceRequireDate", SqlDbType.DateTime).Value = VehiclePlaceRequireDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_UpdJobTransportRequest", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static int UpdTransportRequestById(int JobId, string LocTo, string Remark, int DeliveryType, string Dimension, DateTime VehiclePlaceRequireDate, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@LocTo", SqlDbType.NVarChar).Value = LocTo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@VehiclePlaceRequireDate", SqlDbType.DateTime).Value = VehiclePlaceRequireDate;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_UpdTransportRequestById", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static DataView GetTransportDetailByJobId(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("TR_GetTruckRequestDetails", cmd);
    }

    public static DataView GetTransportDetailById(int JobId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        return CDatabase.GetDataView("TR_GetTruckRequestDetailsById", cmd);
    }
    #endregion

    #region Freight Delivery
    public static int FF_AddDeliveryDetail(int JobId, int ContainerId, int NoOfPackages, string VehicleNo, int VehicleType,
        DateTime VehicleRcvdDate, string TransporterName, int TransporterID, string LRNo, DateTime LRDate, string DeliveryPoint, DateTime DispatchDate,
        DateTime DeliveryDate, DateTime EmptyContRetrunDate, string CargoReceivedBy, string BabajiChallanNo, DateTime BabajiChallanDate, string ChallanPath,
        string DamageCopyPath, string PODPath, string DriverName, string DriverPhone, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        if (VehicleRcvdDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VehicleRcvdDate", SqlDbType.DateTime).Value = VehicleRcvdDate;
        }
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        if (LRDate != DateTime.MinValue)
        {
            command.Parameters.Add("@LRDate", SqlDbType.DateTime).Value = LRDate;
        }
        command.Parameters.Add("@DeliveryPoint", SqlDbType.NVarChar).Value = DeliveryPoint;
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
        command.Parameters.Add("@CargoReceivedBy", SqlDbType.NVarChar).Value = CargoReceivedBy;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        if (BabajiChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;
        }
        command.Parameters.Add("@ChallanPath", SqlDbType.NVarChar).Value = ChallanPath;
        command.Parameters.Add("@DamageCopyPath", SqlDbType.NVarChar).Value = DamageCopyPath;
        command.Parameters.Add("@PODAttachmentPath", SqlDbType.NVarChar).Value = PODPath;
        command.Parameters.Add("@DriverName", SqlDbType.NVarChar).Value = DriverName;
        command.Parameters.Add("@DriverPhone", SqlDbType.NVarChar).Value = DriverPhone;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FF_insDeliveryDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int TR_AddDeliveryDetailForFreight(int JobId, int ContainerId, int NoOfPackages, string VehicleNo, int VehicleType,
        DateTime VehicleRcvdDate, string TransporterName, int TransporterID, string LRNo, DateTime LRDate, string DeliveryPoint, DateTime DispatchDate,
        DateTime DeliveryDate, DateTime EmptyContRetrunDate, string CargoReceivedBy, string BabajiChallanNo, DateTime BabajiChallanDate, string ChallanPath,
        string DamageCopyPath, string PODPath, string DriverName, string DriverPhone, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@NoOfPackages", SqlDbType.Int).Value = NoOfPackages;
        command.Parameters.Add("@VehicleNo", SqlDbType.NVarChar).Value = VehicleNo;
        command.Parameters.Add("@VehicleType", SqlDbType.Int).Value = VehicleType;
        if (VehicleRcvdDate != DateTime.MinValue)
        {
            command.Parameters.Add("@VehicleRcvdDate", SqlDbType.DateTime).Value = VehicleRcvdDate;
        }
        command.Parameters.Add("@TransporterName", SqlDbType.NVarChar).Value = TransporterName;
        command.Parameters.Add("@TransporterID", SqlDbType.Int).Value = TransporterID;
        command.Parameters.Add("@LRNo", SqlDbType.NVarChar).Value = LRNo;
        if (LRDate != DateTime.MinValue)
        {
            command.Parameters.Add("@LRDate", SqlDbType.DateTime).Value = LRDate;
        }
        command.Parameters.Add("@DeliveryPoint", SqlDbType.NVarChar).Value = DeliveryPoint;
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
        command.Parameters.Add("@CargoReceivedBy", SqlDbType.NVarChar).Value = CargoReceivedBy;
        command.Parameters.Add("@ChallanNo", SqlDbType.NVarChar).Value = BabajiChallanNo;
        if (BabajiChallanDate != DateTime.MinValue)
        {
            command.Parameters.Add("@ChallanDate", SqlDbType.DateTime).Value = BabajiChallanDate;
        }
        command.Parameters.Add("@ChallanPath", SqlDbType.NVarChar).Value = ChallanPath;
        command.Parameters.Add("@DamageCopyPath", SqlDbType.NVarChar).Value = DamageCopyPath;
        command.Parameters.Add("@PODAttachmentPath", SqlDbType.NVarChar).Value = PODPath;
        command.Parameters.Add("@DriverName", SqlDbType.NVarChar).Value = DriverName;
        command.Parameters.Add("@DriverPhone", SqlDbType.NVarChar).Value = DriverPhone;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_insDeliveryDetailforfreight", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    public static void FillUserByKam(DropDownList DropDown)               //added new kam method
    {
        SqlCommand command = new SqlCommand();
        CDatabase.BindControls(DropDown, "BS_UserKam", command, "UserName", "UserId");
    }

    public static int UpdDeliveryStatus(int JobId, int ContainerId, int TransitType, int lUser)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@ContainerId", SqlDbType.Int).Value = ContainerId;
        command.Parameters.Add("@TransitType", SqlDbType.Int).Value = TransitType;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("UpdDeliveryDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #region tRUCK REQUEST
    public static int AddTransAddDetails(int JobId, int TranReqId, string FileName, string DocPath, int DocType, string PickUpAdd, int Pincode1, 
            string State1, string City1, string DropAdd, int Pincode2, string State2, string City2, string EmptyLetter, int DeliveryType, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@TranReqId", SqlDbType.Int).Value = TranReqId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;
        command.Parameters.Add("@PickupAddress", SqlDbType.NVarChar).Value = PickUpAdd; //pincode pickUpAdd dropAdd
        command.Parameters.Add("@PickupPincode", SqlDbType.Int).Value = Pincode1;
        command.Parameters.Add("@PickupState", SqlDbType.NVarChar).Value = State1;
        command.Parameters.Add("@PickupCity", SqlDbType.NVarChar).Value = City1;
        command.Parameters.Add("@DropAddress", SqlDbType.NVarChar).Value = DropAdd;
        command.Parameters.Add("@DropPincode", SqlDbType.Int).Value = Pincode2;
        command.Parameters.Add("@DropState", SqlDbType.NVarChar).Value = State2;
        command.Parameters.Add("@DropCity", SqlDbType.NVarChar).Value = City2;
        command.Parameters.Add("@EmptyLetter", SqlDbType.NVarChar).Value = EmptyLetter;       //FOr Loaded dropdown Empty Letter
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("TR_SpTransportAdd", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int UpTransFreightRequest(int JobId, string LocTo, string Remark, int DeliveryType, string Dimension, DateTime VehiclePlaceRequireDate, string FileName, 
        string DocPath, int DocType, int PickupPincode, string PickupState, string PickupCity, int DropPincode, string DropState, string DropCity, string EmptyLetter, string DocName, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@LocTo", SqlDbType.NVarChar).Value = LocTo;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@DeliveryType", SqlDbType.Int).Value = DeliveryType;
        command.Parameters.Add("@Dimension", SqlDbType.NVarChar).Value = Dimension;
        command.Parameters.Add("@VehiclePlaceRequireDate", SqlDbType.DateTime).Value = VehiclePlaceRequireDate;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = FileName;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocType", SqlDbType.Int).Value = DocType;
        command.Parameters.Add("@PickupPincode", SqlDbType.Int).Value = PickupPincode;         //for freight request update pincode state city truck request details
        command.Parameters.Add("@PickupState", SqlDbType.NVarChar).Value = PickupState;
        command.Parameters.Add("@PickupCity", SqlDbType.NVarChar).Value = PickupCity;
        command.Parameters.Add("@DropPincode", SqlDbType.Int).Value = DropPincode;
        command.Parameters.Add("@DropState", SqlDbType.NVarChar).Value = DropState;
        command.Parameters.Add("@DropCity", SqlDbType.NVarChar).Value = DropCity;
        command.Parameters.Add("@EmptyLetter", SqlDbType.NVarChar).Value = EmptyLetter;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("FR_UpdJobTransReqDetail", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    #endregion

    #region VehiclePlaced
    public static int Tr_InsVehiclePlacedDetail(int TransReqId, int NoOfVehicle, int transporterid, int lUserId)
    {
        SqlCommand command = new SqlCommand();

        string sp_result = string.Empty;
        command.Parameters.Add("@TranReqId", SqlDbType.Int).Value = TransReqId;
        command.Parameters.Add("@NoOfVehicle", SqlDbType.Decimal).Value = NoOfVehicle;
        command.Parameters.Add("@TransporterId", SqlDbType.Decimal).Value = transporterid;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        sp_result = CDatabase.GetSPOutPut("Tr_InsVehiclePlacedDetail", command, "@Output");

        return Convert.ToInt32(sp_result);
    }
    #endregion

    #region MemoUpload
    public static int UpdateMemoDetail(int TransReqId, string MemoAttachment, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@TransReqId", SqlDbType.Int).Value = TransReqId;
        if (MemoAttachment != "")
            command.Parameters.Add("@MemoAttachment", SqlDbType.NVarChar).Value = MemoAttachment;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("TR_updMemoDetail", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    #endregion

    public static DataSet GetJobStatusForDashboard(string JobNo)
    {

        SqlCommand command = new SqlCommand();
        command.CommandText = "GETJobStatusForDashboard";
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@BJVNO", SqlDbType.NVarChar).Value = JobNo;

        return CDatabase.GetDataSet("GETJobStatusForDashboard", command);

    }

    #region Billing Details 
    public static DataSet GetBillingAdvise(int EnqId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@EnqId", SqlDbType.Int).Value = EnqId;

        return CDatabase.GetDataSet("FOP_GetBillingDetail", command);
    }
    #endregion

    #region Misc job Activity Details
    //Mics job daily Activity Insert And Update Records
    public static int AddMiscJobDailyActivity(int JobId, string DailyProgress, string DocumentPath, int SummaryStatus, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@DocumentPath", SqlDbType.NVarChar).Value = DocumentPath;
        command.Parameters.Add("@SummaryStatus", SqlDbType.Int).Value = SummaryStatus;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_insJobDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    //Update daily mics job acitivy
    public static int UpdateMiscJobDailyActivity(int ActivityId, int StatusId, string DailyProgress, int UserId)//Boolean VisibleToCustomer,
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        //  command.Parameters.Add("@VisibleToCustomer", SqlDbType.Bit).Value = VisibleToCustomer;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_updJobDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    //Get Misc job for edit and remove in job activity 
    public static DataSet GetMiscJobActivityDetail(int JobId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("MS_GetJobActivityDetailById", command);
    }

    public static int UpdateJobDailyActivityAdmin(int ActivityId, int StatusId, Boolean VisibleToCustomer, string DailyProgress, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@DailyProgress", SqlDbType.NVarChar).Value = DailyProgress;
        command.Parameters.Add("@VisibleToCustomer", SqlDbType.Bit).Value = VisibleToCustomer;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("updJobDailyActivityAdmin", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    //Delete job daily activity
    public static int DeleteMiscJobDailyActivity(int ActivityId, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_delJobDailyActivity", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    //Cancel/Delete job details
    public static int CancleMiscJobDetails(int Jobid, string txtRemark, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@Jobid", SqlDbType.Int).Value = Jobid;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = txtRemark;    //Added new remark Parameter
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("MS_CancelJobDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }
    #endregion

    public static int UpdStatusActivity(int CompanyId, int StatusActivity, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CompanyId", SqlDbType.Int).Value = CompanyId;
        command.Parameters.Add("@StatusActivity", SqlDbType.Int).Value = StatusActivity;
        command.Parameters.Add("@updUser", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("BS_UpdStatusHistory", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static void FillStatusActivity(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,Status FROM BS_StatusMS Where bDel=0 ORDER by Status", "Status", "lId");
    }

    public static void FillPortByModeId(DropDownList DropDown, int ModeId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@ModeId", SqlDbType.Int).Value = ModeId;

        CDatabase.BindControls(DropDown, "getPortByModeID", command, "PortName", "lId");
    }

    #region Expense master
    public static int AddExpenseType(string ExpenseName, string ChargeCode, string ChargeName, string ChargeHSN, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = ExpenseName;
        command.Parameters.Add("@ChargeCode", SqlDbType.NVarChar).Value = ChargeCode;
        command.Parameters.Add("@ChargeName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@ChargeHSNCode", SqlDbType.NVarChar).Value = ChargeHSN;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_insExpenseType", command, "@Output");
        return Convert.ToInt32(SPresult);
    }
    public static int UpdExpenseType(int ExpId, string ExpenseName, string ChargeCode, string ChargeName, string ChargeHSN, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = ExpId;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = ExpenseName;
        command.Parameters.Add("@ChargeCode", SqlDbType.NVarChar).Value = ChargeCode;
        command.Parameters.Add("@ChargeName", SqlDbType.NVarChar).Value = ChargeName;
        command.Parameters.Add("@ChargeHSNCode", SqlDbType.NVarChar).Value = ChargeHSN;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_updExpenseType", command, "@Output");
        return Convert.ToInt32(SPresult);
    }
    public static int DeleteExpenseType(int ExpId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lid", SqlDbType.Int).Value = ExpId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("AC_DeleteExpenseDetail", command, "@Output");
        return Convert.ToInt32(SPresult);
    }

    #endregion

    public static int UpdInvoiceHoldHistory(int Invoiceid, string Remark, int IsFinalInvoicePending, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = Invoiceid;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@IsFinalInvoicePending", SqlDbType.Int).Value = IsFinalInvoicePending;
        command.Parameters.Add("@Luser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("INV_updInvoiceHoldHistory", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataView GetPincodebysearching(int Pincode)         ///////Get State and City By searching pincode  
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.Add("@Pincode", SqlDbType.Int).Value = Pincode;
        return CDatabase.GetDataView("GetPincodeDetails", cmd);

    }

    #region Export Contract

    public static DataSet GetContractMasterDataForExport(int mastertype)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@mastertype", SqlDbType.Int).Value = mastertype;
        return CDatabase.GetDataSet("Usp_GetContractMasterDataForExport", command);
    }

    public static DataTable EX_GetcontractBillingdata(int lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        return CDatabase.GetDataTable("Usp_ExGetContractbillingrData", command);
    }

    public static DataTable EX_GetcontractBillingdata_Pageing(int pageIndex, int PageSize, int lid)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
        command.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@RecordCount", SqlDbType.Int).Value = 4;
        command.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
        return CDatabase.GetDataTable("EX_GetCustomersPageWise", command);
    }

    public static int CheckContractExist(int CustId, string Startdt, string Enddt)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@custid", SqlDbType.Int).Value = CustId;
        command.Parameters.Add("@Startdt", SqlDbType.NVarChar).Value = Startdt;
        command.Parameters.Add("@enddt", SqlDbType.NVarChar).Value = Enddt;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("ContractCheckExists", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet Ex_GetJobDetailForContractBillingUser(int JobId, string username, int @status)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
        command.Parameters.Add("@status", SqlDbType.Int).Value = status;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobId", command);
        return CDatabase.GetDataSet("Get_EXJobDetailForContratcBillingByJobIdUser", command);
    }

    public static int Get_EXJobDetailForContratcBillingByJobIdUser_Checking(int JobId)
    {
        SqlCommand command = new SqlCommand();
        int SPresult;
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobIdUser_Checking", command);
        SPresult = CDatabase.GetSPCount("Get_EXJobDetailForContratcBillingByJobIdUser_Checking", command);
        return Convert.ToInt32(SPresult);
    }

    public static DataSet EX_GetJobDetailForContractBilling(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobId", command);
        return CDatabase.GetDataSet("Get_EXJobDetailForContratcBillingByJobId", command);
    }

    public static DataSet GetEXJobDetail_Users(int JobId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        //return CDatabase.GetDataTable("Get_JobDetailForContratcBillingByJobId", command);
        return CDatabase.GetDataSet("GetEXJobDetail_Users", command);
    }

    public static DataSet UpdateCBBillingDetail(int lid, int JobId, string ChargeCode, string ChargeName, int Quantity, int lUser, int Status)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
        command.Parameters.Add("@jobid", SqlDbType.Int).Value = JobId;
        //command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = gvrow.Cells[1].Text.ToString();
        command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = ChargeCode;
        command.Parameters.Add("@chargename", SqlDbType.VarChar).Value = ChargeName;
        command.Parameters.Add("@qty", SqlDbType.Int).Value = Quantity;
        command.Parameters.Add("@userid", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@status", SqlDbType.Int).Value = Status;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        //command.Parameters.Add("@todate", SqlDbType.VarChar).Value = todate;
        return CDatabase.GetDataSet("usp_Inserttbl_CBBilling_UserForExport", command);
    }

    public static string Get_EXContractChecking(int JobId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult;
        command.Parameters.Add("@JobId", SqlDbType.VarChar).Value = JobId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("Get_ExContractChecking", command, "@OutPut");
        return SPresult;
    }


    #endregion
}