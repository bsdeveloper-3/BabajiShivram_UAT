using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.Web.Script.Serialization;
/// <summary>
/// Summary description for FAPayToAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class FAPayToAutoComplete : System.Web.Services.WebService
{
    public FAPayToAutoComplete()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionFAMaster"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType   =   CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        cmd.CommandText = "SELECT par_code,par_name,GSTIN FROM sal_part WHERE par_name like @myParameter and LEDGCODE=22 ORDER BY par_name";

        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@lUser", contextKey);
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            cn.Close();
        }

        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<FAPayMaster> txtItems = new List<FAPayMaster>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            FAPayMaster ClientList = new FAPayMaster();

            ClientList.PayMasterId = 0;
            ClientList.Par_Code = row["par_code"].ToString();
            ClientList.Par_Name = row["par_name"].ToString();
            ClientList.GSTIN = row["gstin"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAPayMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Par_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetFAChargeCode(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionFAMaster"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        cmd.CommandText = "select charge,name,HSN_SC from charges WHERE name like @myParameter AND locked=0 ORDER BY name";

        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@lUser", contextKey);
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            cn.Close();
        }

        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<FAChargeCodeMaster> txtItems = new List<FAChargeCodeMaster>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            FAChargeCodeMaster ClientList = new FAChargeCodeMaster();

            ClientList.ChargeCodeId = 0;
            ClientList.Charge_Code = row["charge"].ToString();
            ClientList.Charge_Name = row["name"].ToString();
            ClientList.HSN_Code = row["HSN_SC"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAChargeCodeMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Charge_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetFAVendorCode(string prefixText, int count, int contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionFAMaster"].ConnectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandType = CommandType.Text;

        //-----Define parameter instead of passing value directly to prevent sql injection--------//
        cmd.CommandText = "SELECT par_code,par_name,GSTIN FROM sal_part WHERE par_name like @myParameter and LEDGCODE=21 ORDER BY par_name";

        cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        cmd.Parameters.AddWithValue("@lUser", contextKey);
        try
        {
            cn.Open();
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            cn.Close();
        }

        dt = ds.Tables[0];

        //Then return List of string(txtItems) as result
        List<FAVendorCodeMaster> txtItems = new List<FAVendorCodeMaster>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            FAVendorCodeMaster ClientList = new FAVendorCodeMaster();

            ClientList.VendorCodeId = 0;
            ClientList.Vendor_Code = row["par_code"].ToString();
            ClientList.Vendor_Name = row["par_name"].ToString();
            //ClientList.GSTIN = row["gstin"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAVendorCodeMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Vendor_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetFAChargeCodeByName(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcChargeByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@SearchName", prefixText);

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
       
        List<FAChargeCodeMaster> txtItems = new List<FAChargeCodeMaster>();
       
        foreach (DataRow row in dt.Rows)
        {
            FAChargeCodeMaster ClientList = new FAChargeCodeMaster();

            ClientList.ChargeCodeId = 0;
            ClientList.Charge_Code = row["charge"].ToString();
            ClientList.Charge_Name = row["name"].ToString();
            ClientList.HSN_Code = row["HSN_SC"].ToString();
            ClientList.Charge_Category = row["Category"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAChargeCodeMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Charge_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetFAChargeCodeByCode(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcChargeByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@ChargeCode", prefixText);

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

        List<FAChargeCodeMaster> txtItems = new List<FAChargeCodeMaster>();

        foreach (DataRow row in dt.Rows)
        {
            FAChargeCodeMaster ClientList = new FAChargeCodeMaster();

            ClientList.ChargeCodeId = 0;
            ClientList.Charge_Code = row["charge"].ToString();
            ClientList.Charge_Name = row["name"].ToString();
            ClientList.HSN_Code = row["HSN_SC"].ToString();
            ClientList.Charge_Category = row["Category"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAChargeCodeMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Charge_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetFAChargeCodeByHSN(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcChargeByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@ChargeHSN", prefixText);

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

        List<FAChargeCodeMaster> txtItems = new List<FAChargeCodeMaster>();

        foreach (DataRow row in dt.Rows)
        {
            FAChargeCodeMaster ClientList = new FAChargeCodeMaster();

            ClientList.ChargeCodeId = 0;
            ClientList.Charge_Code = row["charge"].ToString();
            ClientList.Charge_Name = row["name"].ToString();
            ClientList.HSN_Code = row["HSN_SC"].ToString();
            ClientList.Charge_Category = row["Category"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAChargeCodeMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Charge_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetFAChargeCodeByCategory(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcChargeByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@ChargeCategory", prefixText);

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

        List<FAChargeCodeMaster> txtItems = new List<FAChargeCodeMaster>();

        foreach (DataRow row in dt.Rows)
        {
            FAChargeCodeMaster ClientList = new FAChargeCodeMaster();

            ClientList.ChargeCodeId = 0;
            ClientList.Charge_Code = row["charge"].ToString();
            ClientList.Charge_Name = row["name"].ToString();
            ClientList.HSN_Code = row["HSN_SC"].ToString();
            ClientList.Charge_Category = row["Category"].ToString();
            txtItems.Add(ClientList);

        }

        List<string> items = new List<string>();
        foreach (FAChargeCodeMaster c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Charge_Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

}
