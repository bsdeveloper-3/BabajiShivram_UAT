using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using AjaxControlToolkit;

/// <summary>
/// Summary description for LeadAutoCompany
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class LeadAutoCompany : System.Web.Services.WebService
{

    public LeadAutoCompany()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count, int contextKey)
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
        cmd.CommandText = "SELECT A.lid AS CompanyId, A.CustName AS CompanyName FROM BS_CustomerMS A INNER JOIN BS_CompanyCategory B " +
                            " ON B.CompanyID=A.lid AND B.bDel=0 AND B.CategoryID=1 WHERE A.bDel = 0 AND A.CustName LIKE @myParameter";
        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception en)
        {
        }
        finally
        {
            cn.Close();
        }
        dt = ds.Tables[0];

        if ((dt == null) || (dt.Rows.Count == 0))
        {
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT lid AS CompanyId, sName AS CompanyName FROM CRM_CompanyMS WHERE bDel=0 AND sName LIKE @myParameter";
            cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception en)
            {
            }
            finally
            {
                cn.Close();
            }
            dt = ds.Tables[0];
        }

        //Then return List of string(txtItems) as result
        List<Company> txtItems = new List<Company>();

        foreach (DataRow row in dt.Rows)
        {
            Company CompanyList = new Company();

            CompanyList.CompanyId = Convert.ToInt32(row["CompanyId"]);
            CompanyList.CompanyName = row["CompanyName"].ToString();
            txtItems.Add(CompanyList);
        }

        List<string> items = new List<string>();
        foreach (Company c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.CompanyName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetCompanyList(string prefixText, int count, int contextKey)
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
        cmd.CommandText = "SELECT lid AS CompanyId, sName AS CompanyName FROM CRM_CompanyMS WHERE bDel=0 AND sName LIKE @myParameter";
        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception en)
        {
        }
        finally
        {
            cn.Close();
        }
        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<Company> txtItems = new List<Company>();

        foreach (DataRow row in dt.Rows)
        {
            Company CompanyList = new Company();

            CompanyList.CompanyId = Convert.ToInt32(row["CompanyId"]);
            CompanyList.CompanyName = row["CompanyName"].ToString();
            txtItems.Add(CompanyList);
        }

        List<string> items = new List<string>();
        foreach (Company c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.CompanyName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}
