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
/// Summary description for JobNumberAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class JobNumberAutoComplete : System.Web.Services.WebService
{

    public JobNumberAutoComplete()
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
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "AC_GetJobDetailByUserId";
        //cmd.CommandType   =   CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        //cmd.CommandText = "SELECT lId AS CustomerId,CustName FROM BS_CustomerMS WHERE CustName like @myParameter AND bDel=0 ORDER BY CustName";

        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@UserId", contextKey);
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
        List<JobDetail> txtItems = new List<JobDetail>();

        foreach (DataRow row in dt.Rows)
        {
            JobDetail JobList = new JobDetail();

            JobList.ModuleId = Convert.ToInt32(row["ModuleId"]);
            JobList.JobId = Convert.ToInt32(row["JobId"]);
            JobList.JobName = row["JobRefNo"].ToString();            
            txtItems.Add(JobList);
        }

        List<string> items = new List<string>();
        foreach (JobDetail c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.JobName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetJobListForDelivery(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "srcPendingDeliveryForUser";

        cmd.Parameters.AddWithValue("@SearchText", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@UserId", 1);

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
        List<JobDetail> txtItems = new List<JobDetail>();

        foreach (DataRow row in dt.Rows)
        {
            JobDetail JobList = new JobDetail();

            JobList.JobId = Convert.ToInt32(row["JobId"]);
            JobList.JobName = row["JobRefNo"].ToString();
            txtItems.Add(JobList);
        }

        List<string> items = new List<string>();
        foreach (JobDetail c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.JobName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetJobListForEWayBill(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "TR_EWaySearchJob";

        cmd.Parameters.AddWithValue("@SearchText", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@UserId", contextKey);

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
        List<JobDetail> txtItems = new List<JobDetail>();

        foreach (DataRow row in dt.Rows)
        {
            JobDetail JobList = new JobDetail();

            JobList.JobId = Convert.ToInt32(row["JobId"]);
            JobList.JobName = row["JobRefNo"].ToString();
            txtItems.Add(JobList);
        }

        List<string> items = new List<string>();
        foreach (JobDetail c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.JobName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetCompletionListForbillinginstruction(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "GetJobDetailByUserId";
        //cmd.CommandType   =   CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        //cmd.CommandText = "SELECT lId AS CustomerId,CustName FROM BS_CustomerMS WHERE CustName like @myParameter AND bDel=0 ORDER BY CustName";

        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@UserId", contextKey);
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
        List<JobDetail> txtItems = new List<JobDetail>();

        foreach (DataRow row in dt.Rows)
        {
            JobDetail JobList = new JobDetail();

            JobList.ModuleId = Convert.ToInt32(row["ModuleId"]);
            JobList.JobId = Convert.ToInt32(row["JobId"]);
            JobList.JobName = row["JobRefNo"].ToString();
            txtItems.Add(JobList);
        }

        List<string> items = new List<string>();
        foreach (JobDetail c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.JobName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }
}
