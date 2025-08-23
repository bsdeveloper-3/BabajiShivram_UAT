using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using BSImport;

/// <summary>
/// Summary description for Common Functions
/// </summary>
public class Commonfunctions
{

    public static string GetJobRefNoValadationRexExp()
    {
        // RegEx Format 1. Start First Two Char, 2. 5 Digit Number 3. "/" 4. Four Char (City Code)  "MBOI" 5. "15-16" Short Fin Year

        string strQuery = "SELECT RIGHT(CONVERT(VARCHAR(8), StartDate, 1),2) + '-' + RIGHT(CONVERT(VARCHAR(8), EndDate, 1),2)as ShortFinYEAR FROM BS_FinYearMS WHERE bDel=0";
        string strShorFinYear = "14-15"; //14-15
        string strRegEx = @"[A-Z a-z]{2}\d{5}/[A-Z a-z]{4}/";

        SqlConnection con = CDatabase.getConnection();
        SqlCommand cmd = new SqlCommand(strQuery,con);
        cmd.Connection.Open();

        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (dr.HasRows == true)
        {
            while (dr.Read() == true)
            {
                strShorFinYear = dr["ShortFinYEAR"].ToString();
            }
        }
        cmd.Dispose();

        return strRegEx + strShorFinYear;
    }

    public static DateTime GetSQLDate(string sqlstat)
    {
        DateTime dt = System.DateTime.Today;
        SqlConnection con = CDatabase.getConnection();
        SqlCommand cmd = new SqlCommand(sqlstat);
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (dr.HasRows == true)
        {
            while (dr.Read() == true)
            {
                dt = Convert.ToDateTime(dr[0].ToString());
            }
        }
        cmd.Dispose();
        return dt;
    }

    public static DateTime ConvertToDateTime(string StrDate_yyyyMMdd)
    {
        string d, m, y;

        y = StrDate_yyyyMMdd.ToString().Substring(0, 4);
        m = StrDate_yyyyMMdd.ToString().Substring(4, 2);
        d = StrDate_yyyyMMdd.ToString().Substring(6, 2);

        return Convert.ToDateTime((m + "/" + d + "/" + y).ToString());
    }
    
    public static DateTime CDateTime(string StrDate_ddMMyyyy)
    {
        //string d, m, y;

        //d = StrDate_ddMMyyyy.ToString().Substring(0, 2);
        //m = StrDate_ddMMyyyy.ToString().Substring(3, 2);
        //y = StrDate_ddMMyyyy.ToString().Substring(6, 4);
        //return (Convert.ToDateTime((m + "/" + d + "/" + y).ToString()));

        StrDate_ddMMyyyy = StrDate_ddMMyyyy.Replace('-', '/');
        StrDate_ddMMyyyy = StrDate_ddMMyyyy.Replace('.', '/');
       // return DateTime.ParseExact(StrDate_ddMMyyyy, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        return DateTime.ParseExact(StrDate_ddMMyyyy, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);

    }
    
    public static string ConvertToDate(string StrDate)
    {
        string d, m, y;

        y = StrDate.ToString().Substring(0, 4);
        m = StrDate.ToString().Substring(4, 2);
        d = StrDate.ToString().Substring(6, 2);

        return Convert.ToDateTime((m + "/" + d + "/" + y).ToString()).ToString("dd/MM/yyyy");
    }
    
    public static string ConvertToDate(int lDate)
    {
        return DateTime.ParseExact(lDate.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
    }

    public static string date_Format()
    {

        DateTime datetime1 = System.DateTime.Now;
        string dateformat = datetime1.ToString("yyyyMMdd");
        return dateformat;

    }
}
