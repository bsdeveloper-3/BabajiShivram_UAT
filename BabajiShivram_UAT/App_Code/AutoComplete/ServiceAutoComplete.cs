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
using BSImport.ServicesManager.BO;

/// <summary>
/// Summary description for VariantAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ServiceAutoComplete : System.Web.Services.WebService
{
    public ServiceAutoComplete()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    [WebMethod]
    public string[] GetServiceList(string prefixText, int count)
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
        cmd.CommandText = "Select lid As ServiceId,sName AS ServiceName from BS_ServicesMS  where bDel=0 AND sName like @myParameter order by sName ";
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
        List<FAQServices> txtItems = new List<FAQServices>();
        String dbValues;
        foreach (DataRow row in dt.Rows)
        {
            FAQServices ServiceList = new FAQServices();

            ServiceList.ServiceId = Convert.ToInt32(row["ServiceId"]);
            ServiceList.ServiceName = row["ServiceName"].ToString();
            txtItems.Add(ServiceList);

        }

        List<string> items = new List<string>();
        foreach (FAQServices c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.ServiceName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}

