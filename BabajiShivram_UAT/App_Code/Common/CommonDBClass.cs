using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BSImport;

/// <summary>
/// Summary description for CommonDBClass 
/// </summary>

namespace BSImport
{
    public class CommonDBClass : CDatabase
    {
        SqlConnection con = null;
        public CommonDBClass()
        {
            // TODO: Add constructor logic here
        }
        public int ExecuteStoredProc(String storedproc, SqlParameter[] para)
        {
            SqlCommand cmd = null;
            try
            {
                con = getConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedproc;
                cmd.Connection = con;
                foreach (SqlParameter parameter in para)
                {

                    cmd.Parameters.Add(parameter);
                }
                con.Open();
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return i;

            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return 0;
            }

        }

        public string ExecuteStoredProcRetId(String storedproc, SqlCommand cmd)
        {
            string str = null;
            try
            {
                con = getConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedproc;
                cmd.Connection = con;
                con.Open();
                str = Convert.ToString(cmd.ExecuteScalar());
                cmd.Dispose();
                con.Close();
                con.Dispose();
                return str;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                str = null;
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return "0";
            }
        }
        public string ExecuteStoredProcRetId(String storedproc)
        {
            SqlCommand cmd = new SqlCommand();
            string str = null;
            try
            {
                con = getConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedproc;
                cmd.Connection = con;
                con.Open();
                str = Convert.ToString(cmd.ExecuteScalar());
                cmd.Dispose();
                con.Close();
                con.Dispose();
                return str;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                str = null;
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return "0";
            }
        }
        public int ExecuteStoredProc(String storedproc, SqlCommand cmd)
        {
            int i = 0;
            try
            {
                con = getConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedproc;
                cmd.Connection = con;
                con.Open();
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con.Dispose();
                return i;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                i = 0;
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return 0;
            }
        }
        public int ExecuteStoredProc(String storedproc)
        {
            int i = 0;
            SqlCommand cmd = null;
            try
            {
                con = getConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedproc;
                cmd.Connection = con;
                con.Open();
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con.Dispose();
                return i;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                i = 0;
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return 0;
            }
        }
        public int ExecuteQry(String sqlstat, SqlParameter[] para)
        {
            SqlCommand cmd = null;
            int i = 0;
            try
            {
                con = getConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlstat;
                cmd.Connection = con;
                foreach (SqlParameter parameter in para)
                {
                    cmd.Parameters.Add(parameter);
                }
                con.Open();
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con.Dispose();
                return i;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                i = 0;
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return 0;
            }

        }
        public int ExecuteQry(String sqlstat)
        {
            SqlCommand cmd = null;
            int i = 0;
            try
            {
                con = getConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlstat;
                cmd.Connection = con;
                con.Open();
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con.Dispose();
                return i;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                i = 0;
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return 0;
            }
        }
        public SqlDataReader GetDataReader(String sqlstat)
        {
            SqlDataReader sdr = null;
            SqlCommand scmd = null;
            try
            {
                con = getConnection();
                scmd = new SqlCommand();
                scmd.CommandType = CommandType.Text;
                scmd.CommandText = sqlstat;
                scmd.Connection = con;
                scmd.Connection.Open();
                sdr = scmd.ExecuteReader(CommandBehavior.CloseConnection);
                scmd.Dispose();
                return sdr;
            }
            catch (Exception Ex)
            {
                scmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return sdr = null;
            }
        }

        public SqlDataReader GetDataReader(SqlCommand scmd)
        {
            SqlDataReader sdr = null;
            //SqlCommand scmd = null;
            try
            {
                con = getConnection();
                //scmd = new SqlCommand();
                //scmd.CommandType = CommandType.Text;
                //scmd.CommandText = sqlstat;
                scmd.Connection = con;
                scmd.Connection.Open();
                sdr = scmd.ExecuteReader(CommandBehavior.CloseConnection);
                scmd.Dispose();
                return sdr;
            }
            catch (Exception Ex)
            {
                scmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return sdr = null;
            }
        }

        public SqlDataReader GetDataReaderStoredProc(String StoredProc)
        {
            SqlDataReader sdr = null;
            SqlCommand scmd = null;
            try
            {
                con = getConnection();
                scmd = new SqlCommand(StoredProc, con);
                con.Open();
                sdr = scmd.ExecuteReader(CommandBehavior.CloseConnection);
                scmd.Dispose();
                return sdr;
            }
            catch (Exception Ex)
            {
                scmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return sdr = null;
            }
        }
        public SqlDataReader GetDataReader(String sqlstat, SqlParameter[] para)
        {
            SqlDataReader sdr = null;
            SqlCommand scmd = null;
            try
            {
                con = getConnection();
                scmd = new SqlCommand();
                scmd.CommandType = CommandType.Text;
                scmd.CommandText = sqlstat;
                scmd.Connection = con;
                foreach (SqlParameter paramater in para)
                {
                    scmd.Parameters.Add(paramater);
                }
                con.Open();
                sdr = scmd.ExecuteReader(CommandBehavior.CloseConnection);
                scmd.Dispose();
                return sdr;
            }
            catch (Exception Ex)
            {
                scmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return sdr = null;
            }
        }
        public SqlDataReader GetDataReaderStoredProc(String StoredProc, SqlParameter[] para)
        {
            SqlDataReader sdr = null;
            SqlCommand scmd = null;
            try
            {
                con = getConnection();
                scmd = new SqlCommand();
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = StoredProc;
                scmd.Connection = con;
                foreach (SqlParameter paramater in para)
                {
                    scmd.Parameters.Add(paramater);
                }
                con.Open();
                sdr = scmd.ExecuteReader(CommandBehavior.CloseConnection);
                scmd.Dispose();
                return sdr;
            }
            catch (Exception Ex)
            {
                scmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("Sql Server,CommonDBClass Has Exception " + Ex.Message);
                return sdr = null;
            }

        }
        public SqlDataReader GetDataReaderStoredProc(String StoredProc, SqlCommand scmd)
        {
            SqlDataReader sdr;
            try
            {
                con = getConnection();
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.CommandText = StoredProc;
                scmd.Connection = con;
                con.Open();
                sdr = scmd.ExecuteReader(CommandBehavior.CloseConnection);
                scmd.Dispose();
                return sdr;
            }
            catch (SqlException Ex)
            {
                con.Close();
                scmd.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return sdr = null;
            }
        }
        public DataSet GetDataSet(String sqlstat, String tblname)
        {
            DataSet ds;
            try
            {
                con = getConnection();
                SqlDataAdapter SqlDaApter = new SqlDataAdapter(sqlstat, con);
                ds = new DataSet(tblname);
                SqlDaApter.Fill(ds, tblname);
                con.Close();
                con.Dispose();
            }
            catch (SqlException Ex)
            {
                ds = null;
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
            }
            return ds;
        }
        public DataSet GetDataSet(String sqlstatOrStordProcPName)
        {
            DataSet ds;
            try
            {
                con = getConnection();
                SqlDataAdapter SqlDaApter = new SqlDataAdapter(sqlstatOrStordProcPName, con);
                ds = new DataSet();
                SqlDaApter.Fill(ds);
                con.Close();
                con.Dispose();
            }
            catch (SqlException Ex)
            {
                ds = null;
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
            }
            return ds;
        }

        public DataSet GetDataSet(SqlCommand scmd)
        {
            DataSet ds;
            try
            {
                con = getConnection();
                scmd.Connection = con;
                SqlDataAdapter SqlDaApter = new SqlDataAdapter(scmd);
                ds = new DataSet();
                SqlDaApter.Fill(ds);
                con.Close();
                con.Dispose();
            }
            catch (SqlException Ex)
            {
                ds = null;
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
            }
            return ds;
        }

        public DataSet GetDataSet(String sqlstatOrStordProcPName, DataSet ds)
        {

            try
            {
                con = getConnection();
                SqlDataAdapter SqlDaApter = new SqlDataAdapter(sqlstatOrStordProcPName, con);
                SqlDaApter.Fill(ds);
                con.Close();
                con.Dispose();
            }
            catch (SqlException Ex)
            {
                ds = null;
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
            }
            return ds;
        }
        public DataTable GetDataTable(String sqlstat, string tblnm)
        {
            DataTable daT;
            try
            {
                con = getConnection();
                daT = new DataTable(tblnm);
                SqlDataAdapter da = new SqlDataAdapter(sqlstat, con);
                da.Fill(daT);
                con.Close();
                con.Dispose();
            }
            catch (SqlException Ex)
            {
                daT = null;
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
            }
            return daT;
        }

        public DataTable GetDataTable(String StoredProc)
        {
            DataTable dt = null;
            try
            {
                con = getConnection();
                SqlDataAdapter da = new SqlDataAdapter(StoredProc, con);
                dt = new DataTable();
                da.Fill(dt);
                con.Close();
                con.Dispose();
                return dt;
            }
            catch (SqlException Ex)
            {
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                con.Close();
                con.Dispose();
                return dt = null;
            }

        }
        public DataView GetDataView(String sqlstat)
        {
            DataView dv = new DataView();
            try
            {
                con = getConnection();
                SqlDataAdapter SqlDtAptr = new SqlDataAdapter(sqlstat, con);
                DataSet ds = new DataSet();
                SqlDtAptr.Fill(ds);
                dv.Table = ds.Tables[0];
                con.Dispose();
            }
            catch (SqlException Ex)
            {
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                con.Close();
                con.Dispose();
                dv = null;
            }
            return dv;
        }
        public string GetReturnValue(String sqlstat)
        {
            string strval = null;
            SqlDataReader objdr = null;
            SqlCommand cmd = null;
            try
            {
                con = getConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlstat;
                cmd.Connection = con;
                cmd.Connection.Open();
                objdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (objdr.HasRows == true)
                {
                    while (objdr.Read() == true)
                    {
                        strval = objdr[0].ToString();
                    }
                }
                cmd.Connection.Close();
                objdr.Dispose();
                return strval;
            }
            catch (SqlException Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                objdr.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return strval = null;
            }
        }

        public string GetReturnString(String sqlstat, SqlCommand cmd)
        {
            string strval = null;
            try
            {
                con = getConnection();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlstat;
                cmd.Connection = con;
                con.Open();
                strval = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                con.Dispose();
                cmd.Dispose();
                return strval;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return strval = Ex.Message;
                //HttpContext.Current.Server.Transfer("login.aspx?Ex=" + Ex.Message);
                //HttpContext.Current.Response.Write("alert('" + Ex.Message + "')");
            }

        }

        public string GetReturnString(String SqlStoredPro)
        {
            SqlCommand cmd = null;
            string str = null;
            try
            {
                con = getConnection();
                cmd = new SqlCommand(SqlStoredPro, con);
                con.Open();
                str = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                con.Dispose();
                cmd.Dispose();
                return str;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return str = null;
            }
        }
        public string GetReturnValue(String StoredProc, SqlCommand cmd)
        {
            string strval = null;
            SqlDataReader objdr = null;
            try
            {
                con = getConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = StoredProc;
                cmd.Connection = con;
                cmd.Connection.Open();
                objdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (objdr.HasRows == true)
                {
                    while (objdr.Read() == true)
                    {
                        strval = objdr[0].ToString();
                    }
                }
                cmd.Connection.Close();
                objdr.Dispose();
                cmd.Dispose();
                return strval;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                objdr.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return strval = null;
            }
        }
        public SqlDataSource GetSqlDataSource(string sSQl)
        {
            SqlDataSource sqlDS = new SqlDataSource(connectionString, sSQl);
            sqlDS.ID = "ds";
            return sqlDS;
        }
        public bool GetRecExists(string sqlstat)
        {
            SqlCommand cmd = null;
            try
            {
                con = getConnection();
                cmd = new SqlCommand(sqlstat, con);
                con.Open();
                int recStatus = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                con.Dispose();
                cmd.Dispose();
                if (recStatus == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return false;
            }

        }
        public int GetRecCount(String sqlstat)
        {
            SqlCommand cmd = null;
            int getVal = 0;
            try
            {
                string str = null;
                con = getConnection();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlstat;
                cmd.Connection = con;
                cmd.Connection.Open();
                str = Convert.ToString(cmd.ExecuteScalar());
                if (str == "")
                    getVal = 0;
                else
                    getVal = Convert.ToInt32(str);
                con.Close();
                con.Dispose();
                cmd.Dispose();
                return getVal;
            }
            catch (Exception Ex)
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
                System.Diagnostics.Trace.WriteLine("CommonDBClass Has Exception " + Ex.Message);
                return getVal = 0;
            }
        }
        
    }
}