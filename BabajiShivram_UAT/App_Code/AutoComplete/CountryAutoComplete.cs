using System;
using System.Collections;
//using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using AjaxControlToolkit;
using BSImport.CountryManager.BO;

/// <summary>
/// Summary description for CountryAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]

[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CountryAutoComplete : System.Web.Services.WebService {

    public CountryAutoComplete () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCountryCompletionList(string prefixText, int count)
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
        cmd.CommandText = "SELECT lId as CountryId, sName AS CountryName FROM LocDetail WHERE sName like @myParameter and lTypId=2 and bDel=0 ORDER BY sName";
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
        List<Country> txtItems = new List<Country>();
        
        foreach (DataRow row in dt.Rows)
        {
            Country CountryList = new Country();

            CountryList.CountryId = Convert.ToInt32(row["CountryId"]);
            CountryList.CountryName = row["CountryName"].ToString();
            txtItems.Add(CountryList);

        }

        List<string> items = new List<string>();
        foreach (Country c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.CountryName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }
    
}
