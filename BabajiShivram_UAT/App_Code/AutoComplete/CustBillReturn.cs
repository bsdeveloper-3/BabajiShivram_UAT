using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using AjaxControlToolkit;
using BSImport.ClientManager.BO;

/// <summary>
/// Summary description for CustBillReturn
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class CustBillReturn : System.Web.Services.WebService
{
    LoginClass LoggedInUser = new LoginClass();
    public CustBillReturn()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetBillReturnList(string prefixText, int count, string contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        string[] strContext = contextKey.Split(',');
        int finyear = Convert.ToInt32(strContext[0].ToString().Trim());
        int userId = Convert.ToInt32(strContext[1].ToString().Trim());


        // int aa = Convert.ToInt32(Session["FinYearId"]);

        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SearchBillReturn";
        //cmd.CommandType   =   CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        //cmd.CommandText = "SELECT lId AS CustomerId,CustName FROM BS_CustomerMS WHERE CustName like @myParameter AND bDel=0 ORDER BY CustName";

        cmd.Parameters.AddWithValue("@SearchText", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@FinYear", finyear);  
        cmd.Parameters.AddWithValue("@userId", userId);

        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            cn.Close();
        }

        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<Client> txtItems = new List<Client>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            Client ClientList = new Client();

            //  ClientList.ClientId = row["SCode"].ToString();
            ClientList.ClientName = row["INVNO"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (Client c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.ClientName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}
