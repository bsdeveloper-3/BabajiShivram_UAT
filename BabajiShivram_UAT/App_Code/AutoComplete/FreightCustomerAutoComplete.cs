using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
/// <summary>
/// Summary description for FreightCustomerAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class FreightCustomerAutoComplete : System.Web.Services.WebService {

    public FreightCustomerAutoComplete () {

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
        cmd.CommandText = "select Distinct(Customer) AS CustomerName from FR_Enquiry WHERE Customer like @myParameter ORDER BY Customer";
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
        List<String> txtItems = new List<String>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {

            dbValues = row["CustomerName"].ToString();
            txtItems.Add(dbValues);

        }

        return txtItems.ToArray();

    }
    
}
