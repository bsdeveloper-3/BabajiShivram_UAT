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
using TransportTrack;

/// <summary>
/// Summary description for ContractOperation
/// </summary>
public class ContractOperation
{
    public ContractOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable GetCustDetail(int CustId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@CustId", SqlDbType.Int).Value = CustId;

        return CDatabase.GetDataTable("Get_CustomerDetail", command);
    }

    public static int AddContractdocPath(string filename, string DocPath, int CbId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@CBId", SqlDbType.Int).Value = CbId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insContractDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateCBIdForContractDoc(int DocId,int ContractId)
    {
        string SPresult = "";
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@DocId", SqlDbType.Int).Value = DocId;
        command.Parameters.Add("@CBId", SqlDbType.Int).Value = ContractId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("UpdCBIdForContractDoc", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
}