using System;
using System.Collections;
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
using BSImport.ShipperManager.BO;

/// <summary>
/// Summary description for CustomerShipperAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class CustomerShipperAutoComplete : System.Web.Services.WebService
{

    public CustomerShipperAutoComplete()
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
        cmd.CommandText = "select A.lid AS ShipperId,A.CustName As ShipperName from BS_CustomerMS A inner join BS_CompanyCategory B ON A.lid = B.CompanyID AND B.bDel = 0 WHERE B.CategoryID = 4 AND A.CustName LIKE @myParameter ORDER BY A.CustName";
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
        List<Shipper> txtItems = new List<Shipper>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            Shipper ShipperList = new Shipper();

            ShipperList.ShipperId = Convert.ToInt32(row["ShipperId"]);
            ShipperList.ShipperName = row["ShipperName"].ToString();
            txtItems.Add(ShipperList);

        }

        List<string> items = new List<string>();
        foreach (Shipper s in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(s.ShipperName, serializer.Serialize(s)));
        }

        return items.ToArray();

    }

}
