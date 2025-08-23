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
/// Summary description for CompanyAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class CompanyAutoComplete : System.Web.Services.WebService
{
    public CompanyAutoComplete()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "srchCompanyByType";
        //cmd.CommandType   =   CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        //cmd.CommandText = "SELECT lId AS CustomerId,CustName FROM BS_CustomerMS WHERE CustName like @myParameter AND bDel=0 ORDER BY CustName";

        cmd.Parameters.AddWithValue("@SearchText", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@CategoryID", contextKey);

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

            ClientList.ClientId = Convert.ToInt32(row["CustomerId"]);
            ClientList.ClientName = row["CustName"].ToString();
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
