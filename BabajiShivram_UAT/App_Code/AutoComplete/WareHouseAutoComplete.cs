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
using BSImport.WarehouseManager.BO;

/// <summary>
/// Summary description for WareHouseAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]

[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WareHouseAutoComplete : System.Web.Services.WebService {

    public WareHouseAutoComplete () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]

    public string[] GetWarehouseCompletionList(string prefixText, int count)
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
        cmd.CommandText = "SELECT lId as WarehouseId,sName AS WarehouseName FROM BS_WarehouseMS WHERE sName like @myParameter AND bDel=0 ORDER BY sName";
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
        List<Warehouse> txtItems = new List<Warehouse>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            Warehouse WarehouseList = new Warehouse();

            WarehouseList.WarehouseId = Convert.ToInt32(row["WarehouseId"]);
            WarehouseList.WarehouseName = row["WarehouseName"].ToString();
            txtItems.Add(WarehouseList);

        }

        List<string> items = new List<string>();
        foreach (Warehouse c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.WarehouseName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }
    
}

