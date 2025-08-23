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
using BSImport.PortOfLoading.BO;

/// <summary>
/// Summary description for PortOfLoadingAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PortOfLoadingAutoComplete : System.Web.Services.WebService {

    public PortOfLoadingAutoComplete () {

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
        cmd.CommandText = "SELECT lId AS PortOfLoadingId,LoadingPortName FROM BS_PortOfLoadingMS WHERE (LoadingPortName like @myParameter Or PortCode like @myParameter) AND bDel=0 ORDER BY LoadingPortName";
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
        List<PortOfLoadingBO> txtItems = new List<PortOfLoadingBO>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            PortOfLoadingBO PortList = new PortOfLoadingBO();

            PortList.PortOfLoadingId = Convert.ToInt32(row["PortOfLoadingId"]);
            PortList.Name = row["LoadingPortName"].ToString();
            txtItems.Add(PortList);

        }

        List<string> items = new List<string>();
        foreach (PortOfLoadingBO c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }
    
}

