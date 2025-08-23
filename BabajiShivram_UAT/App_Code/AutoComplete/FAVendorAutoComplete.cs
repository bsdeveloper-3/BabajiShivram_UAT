using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using AjaxControlToolkit;
using BSImport.VendorManager.BO;
/// <summary>
/// Summary description for FAVendorAutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class FAVendorAutoComplete : System.Web.Services.WebService
{
    public FAVendorAutoComplete()
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
        cmd.CommandText = "BJV_srcVendorByName";
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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetCompletionListByGSTIN(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@GSTIN", prefixText);

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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.GSTIN, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetCompletionListByPANNo(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@PanNo", prefixText);

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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            string strDisplayName = c.PANNo + '-' +c.Name ;
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(strDisplayName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }


    // Get Vendor Detail - -- LEDGCODE='21' Vendor

    #region FA_Vendor
    [WebMethod]
    public string[] GetVendorListByName(string prefixText, int count, string contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@SearchName", prefixText);
        if (contextKey == "10.21")
        {
            cmd.Parameters.AddWithValue("@LEDGCODE", "10.21");
        }
        else
        {
            cmd.Parameters.AddWithValue("@LEDGCODE", "21");
        }
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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetVendorByGSTIN(string prefixText, int count, string contextKey)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@GSTIN", prefixText);
        //cmd.Parameters.AddWithValue("@LEDGCODE", "21");
        if (contextKey == "10.21")
        {
            cmd.Parameters.AddWithValue("@LEDGCODE", "10.21");
        }
        else
        {
            cmd.Parameters.AddWithValue("@LEDGCODE", "21");
        }
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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.GSTIN, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetVendorListByPANNo(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@PanNo", prefixText);
        cmd.Parameters.AddWithValue("@LEDGCODE", "21");
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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            string strDisplayName = c.PANNo + '-' + c.Name;
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(strDisplayName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    #endregion

    // Get FA Customer Detail - -- LEDGCODE='22' Customer

    #region FA_Customer
    [WebMethod]
    public string[] GetCustomerListByName(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@SearchName", prefixText);
        cmd.Parameters.AddWithValue("@LEDGCODE", "22");

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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    [WebMethod]
    public string[] GetCustomerByGSTIN(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@GSTIN", prefixText);
        cmd.Parameters.AddWithValue("@LEDGCODE", "22");
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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.GSTIN, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    
    [WebMethod]
    public string[] GetCustomerListByPANNo(string prefixText, int count)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //ADO.Net
        SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;
        cmd.CommandText = "BJV_srcVendorByName";
        cmd.CommandType = CommandType.StoredProcedure;
        //-----Define a parameter instead of passing value directly to prevent sql injection--------//

        cmd.Parameters.AddWithValue("@PanNo", prefixText);
        cmd.Parameters.AddWithValue("@LEDGCODE", "22");
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
        List<FAVendor> txtItems = new List<FAVendor>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            //String From DataBase(dbValues)
            //  dbValues = row["ConsigneeName"].ToString();
            //  dbValues = dbValues.ToLower();

            FAVendor VendorList = new FAVendor();

            VendorList.Name = row["par_name"].ToString();
            VendorList.Code = row["par_code"].ToString();
            VendorList.GSTIN = row["gstin"].ToString();
            VendorList.PANNo = row["girno"].ToString();
            VendorList.CreditDays = row["crdays"].ToString();
            VendorList.State = row["state"].ToString();

            txtItems.Add(VendorList);

        }

        List<string> items = new List<string>();
        foreach (FAVendor c in txtItems)
        {
            string strDisplayName = c.PANNo + '-' + c.Name;
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(strDisplayName, serializer.Serialize(c)));
        }

        return items.ToArray();

    }

    #endregion
}
