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
using BSImport.UserManager.BO;


/// <summary>
/// Summary description for UserAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class UserAutoComplete : System.Web.Services.WebService {

    public UserAutoComplete () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetUserCompletionList(string prefixText, int count)
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
        cmd.CommandText = "SELECT lId AS UserId,sName AS UserName FROM BS_UserMS WHERE sName like @myParameter AND bDel=0 AND lType IN(1,3) ORDER BY sName";
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
        List<User> txtItems = new List<User>();
        
        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            User UserList = new User();

            UserList.Userid = row["UserId"].ToString();
            UserList.Username = row["UserName"].ToString();
            txtItems.Add(UserList);

        }

        List<string> items = new List<string>();
        foreach (User c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Username, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetJobExpenseUserList(string prefixText, int count)
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
        cmd.CommandText = "SELECT A.lId AS UserId,A.sName AS UserName FROM BS_UserMS A INNER JOIN BS_UserDetail B on B.UserId=A.lid INNER JOIN BS_DivisionMS AS C ON C.lId = B.DivisionId AND B.DivisionId IN (2044,2045) WHERE A.sName like @myParameter AND A.bDel = 0 AND lType IN(1, 3) ORDER BY sName";
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
        List<User> txtItems = new List<User>();

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            User UserList = new User();

            UserList.Userid = row["UserId"].ToString();
            UserList.Username = row["UserName"].ToString();
            txtItems.Add(UserList);

        }

        List<string> items = new List<string>();
        foreach (User c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Username, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}
