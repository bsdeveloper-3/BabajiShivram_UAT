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
using BSImport.ConsigneeManager.BO;
/// <summary>
/// Summary description for ConsigneeAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ConsigneeAutoComplete : System.Web.Services.WebService
{
	public ConsigneeAutoComplete()
	{
		//
		// TODO: Add constructor logic here
		//
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
        cmd.CommandText = "SELECT A.lId as ConsigneeId,A.CustName AS  ConsigneeName FROM BS_CustomerMS AS A " +
            " INNER JOIN BS_CompanyCategory AS C ON C.CompanyID = A.lid" +
            " WHERE C.CategoryID=2 AND A.CustName like @myParameter AND A.bDel=0 AND C.bDel=0 ORDER BY ConsigneeName";
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
        List<Consignee> txtItems = new List<Consignee>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
          //String From DataBase(dbValues)
          //  dbValues = row["ConsigneeName"].ToString();
          //  dbValues = dbValues.ToLower();

            Consignee ConsigneeList = new Consignee();

            ConsigneeList.ConsigneeId =Convert.ToInt32(row["ConsigneeId"]);
            ConsigneeList.ConsigneeName = row["ConsigneeName"].ToString();
            txtItems.Add(ConsigneeList);

        }

        List<string> items = new List<string>();
        foreach (Consignee c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.ConsigneeName,serializer.Serialize(c)));
        }

        return items.ToArray();

    }
   
}
