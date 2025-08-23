using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BSImport.ChequeManager.BO;
using AjaxControlToolkit;
/// <summary>
/// Summary description for AccountChequeNoAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AccountChequeNoAutoComplete : System.Web.Services.WebService
{

    public AccountChequeNoAutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "AC_SrchBankChequeNo";
        //cmd.CommandType   =   CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        //cmd.CommandText = "SELECT lId AS CustomerId,CustName FROM BS_CustomerMS WHERE CustName like @myParameter AND bDel=0 ORDER BY CustName";

        cmd.Parameters.AddWithValue("@myParameter", prefixText + "%");
        cmd.Parameters.AddWithValue("@UserId", contextKey);
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception en)
        {
        }
        finally
        {
            cn.Close();
        }
        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<ChequeDetail> txtItems = new List<ChequeDetail>();

        foreach (DataRow row in dt.Rows)
        {
            ChequeDetail ChequeList = new ChequeDetail();

            ChequeList.ChequeId = Convert.ToInt32(row["lid"]);
            ChequeList.ChequeNo =   row["ChequeNo"].ToString();
            ChequeList.BankName = row["BankName"].ToString();
            ChequeList.AccountName = row["BankAccountName"].ToString();
            txtItems.Add(ChequeList);
        }

        List<string> items = new List<string>();
        foreach (ChequeDetail c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.ChequeNo, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}
