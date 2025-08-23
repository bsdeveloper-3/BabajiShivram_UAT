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
/// Summary description for ReportOperations
/// </summary>
public class ReportOperations
{
    public ReportOperations()
    {
        //
        // TODO: Add constructor logic here

        //
    }

    #region Get Report Data
    public static DataSet GetJobDetailForCustomer(int CustId)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustId;

        return CDatabase.GetDataSet("rptGetJobDetailForCustomer", command);

    }
    public static DataSet GetJobDetailForCustomerDatewise(int Date)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@Date", SqlDbType.Int).Value = Date;

        return CDatabase.GetDataSet("rptGetJobDetailDatewise", command);

    }
    public static DataSet GetJobDetailForCustomerBetweenDate(int Date1, int Date2)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@Date1", SqlDbType.Int).Value = Date1;
        command.Parameters.Add("@Date2", SqlDbType.Int).Value = Date2;
        return CDatabase.GetDataSet("rptGetJobDetailBetweenDate", command);

    }

    public static DataSet GetJobTrackingDetail(int JobId)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("rptJobTracking", command);
    }

    public static DataSet GetShipmentAgeing(int JobId)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@JobId", SqlDbType.Int).Value = JobId;

        return CDatabase.GetDataSet("rptShipmentAgeing", command);

    }
    
    public static DataSet GetPortDetail(int PortId)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;

        return CDatabase.GetDataSet("rptPortwiseCustomerWise", command);

    }
    public static DataSet GetPortDetail()
    {

        SqlCommand command = new SqlCommand();
        //command.Parameters.Add("@PId", SqlDbType.Int).Value = PId;

        return CDatabase.GetDataSet("rptPortwiseCustomerWise", command);

    }
    public static DataSet GetPriorityShipment(int PriorityId)
    {

        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@PriorityId", SqlDbType.Int).Value = PriorityId;

        return CDatabase.GetDataSet("rptPriorityShipment", command);

    }

    public static DataSet GetJobCreate()
    {
        SqlCommand command = new SqlCommand();
      
        return CDatabase.GetDataSet("rptJobCreate", command);
    }
    public static DataSet GetShippmentArrival()
    {
        SqlCommand command = new SqlCommand();

        return CDatabase.GetDataSet("rptShippmentArrival", command);
    }
        
    public static DataSet GetPrealertsPortWise(int PortId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@PortId", SqlDbType.Int).Value = PortId;
        return CDatabase.GetDataSet("rptPreAlertPortWise", cmd);
    }
    public static DataSet GetPrealertsDivisionPlantWise(int DivisionId,int PlantId)
    {
        SqlCommand cmd = new SqlCommand();
       
        cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisionId;
        
        if (PlantId != 0)
        {
            cmd.Parameters.Add("@PlantId", SqlDbType.Int).Value = PlantId;
        }
        return CDatabase.GetDataSet("rptPreAlertDivisionPlantWise", cmd);

    }

    public static DataSet GetDailyJobOpeningPortBranchWise(int PortId, int Branch)
    {
        SqlCommand cmd = new SqlCommand();
        
         cmd.Parameters.Add("@Port", SqlDbType.Int).Value = PortId;

         if (Branch != 0)
        {
            cmd.Parameters.Add("@Branch", SqlDbType.Int).Value = Branch;
        }

        return CDatabase.GetDataSet("rptDailyJobOpeningPortBranchWise", cmd);

    }
    //Daily Job Opening for Customer
    public static DataSet GetDailyJobOpeningPortWiseToCust(int PortId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@Port", SqlDbType.Int).Value = PortId;
        return CDatabase.GetDataSet("rptDailyJobOpeningPortWiseToCust", cmd);
    }

    public static DataSet GetDailyJobOpeningConsigneeWiseToCust(int consignee)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@Consignee", SqlDbType.Int).Value = consignee;
        return CDatabase.GetDataSet("rptDailyJobOpeningConsigneeWiseToCust", cmd);
    }

    public static DataSet GetDailyJobOpeningDivisionPlantWiseToCust(int DivisionId, int PlantId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@Division", SqlDbType.Int).Value = DivisionId;

        if (PlantId != 0)
        {
            cmd.Parameters.Add("@Plant", SqlDbType.Int).Value = PlantId;
        }
        return CDatabase.GetDataSet("rptDailyJobOpeningDivisionPlantWiseToCust", cmd);

    }
    public static DataSet GetCustomerAdditionalJobField(int CustomerId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        
        return CDatabase.GetDataSet("rptCustomerAdditionalJobField", cmd);

    }

    public static DataSet GetCustomerQueryField(string strColumnList)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@ColumnList", SqlDbType.NVarChar).Value = strColumnList;

        return CDatabase.GetDataSet("rptCustomerQueryField", cmd);

    }
    #endregion
}
