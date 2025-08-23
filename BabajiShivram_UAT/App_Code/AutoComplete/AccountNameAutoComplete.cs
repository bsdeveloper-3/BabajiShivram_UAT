using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using BSImport.CountryManager.BO;
using AjaxControlToolkit;

/// <summary>
/// Summary description for AccountNameAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class AccountNameAutoComplete : System.Web.Services.WebService
{

    public AccountNameAutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.Text;
        //Compare String From Textbox(prefixText) AND String From Column in DataBase(CompanyName)
        //If String from DataBase is equal to String from TextBox(prefixText) then add it to return ItemList
        //-----I Defined a parameter instead of passing value directly to prevent sql injection--------//
        cmd.CommandText = "SELECT lId,sName FROM AC_AccountCodeMS WHERE sName like @myParameter AND bDel=0 ORDER BY sName";
        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch
        {
        }
        finally
        {
            cn.Close();
        }
        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<AccountCode> txtItems = new List<AccountCode>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            AccountCode ACCodeList = new AccountCode();

            ACCodeList.AcCodeId = Convert.ToInt32(row["lid"]);
            ACCodeList.AccountName = row["sName"].ToString();
            txtItems.Add(ACCodeList);
        }

        List<string> items = new List<string>();
        foreach (AccountCode c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.AccountName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}
