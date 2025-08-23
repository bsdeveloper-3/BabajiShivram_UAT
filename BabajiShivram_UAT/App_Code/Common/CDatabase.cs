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
using System.Web.Configuration;
//using AjaxControlToolkit;

    public class CDatabase
    {
        public CDatabase()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Member variables

        static string databaseOwner = "dbo";	// overwrite in web.config
        string _connectionString = null;

        #endregion

        #region SQL Connection

        /// <summary>
        /// Will get the connection string from the web.config file
        /// </summary>
        /// <returns>Sql Connection</returns>
        public static SqlConnection getConnection()
        {
            SqlConnection conn;
            try
            {
                //conn = new SqlConnection(this.ConnectionString);            
                //  conn = new SqlConnection(WebConfigurationManager.AppSettings["constr"]);
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString);

            }
            catch (Exception ex)
            {
                ErrorLog.LogToTextFile("DB Connection Exception", ex);

                ErrorLog.SendMail("DB Connection Exception", ex);

                //ErrorLog.LogToDatabase(0,"DBConnectionString Issue","getConnection Function",ex.Message, ex,"",0);
                
                throw new Exception("SQL Connection String is Invalid.");
            }
            return conn;
        }

        public static SqlConnection getFAConnection()
        {
            SqlConnection conn;
            try
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionFA"].ConnectionString);
            }
            catch (Exception ex)
            {
                ErrorLog.LogToTextFile("FA DB Connection Exception", ex);

                throw new Exception("FA SQL Connection String is Invalid.");
            }
            return conn;
        }
        /// <summary>
        ///  Property for the connection string
        /// </summary>
        public string connectionString
        {
            get
            {
                _connectionString = WebConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
                return _connectionString;
            }

        }

        #endregion

        #region Function
        public static int ReturnIntResultSQL(string strSql)
        {
            int tempID = 0; // temp.  ID Value
            SqlConnection connection = getConnection();
            SqlDataReader dr = null;

            try
            {
                // Execute Sql command & return DataReader object
                SqlCommand command = new SqlCommand(strSql, connection);
                command.Connection.Open();
                dr = command.ExecuteReader(System.Data.CommandBehavior.Default);
                if (dr.HasRows == true)
                {
                    while (dr.Read() == true)
                    {
                        if (!dr.IsDBNull(0))
                        {
                            tempID = Convert.ToInt32(dr["ID"]);
                        }
                        else
                        {
                            tempID = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
                if (connection.State == ConnectionState.Open) connection.Close();
                //command.Dispose();
                connection.Dispose();
            }
            return tempID;
        }

        #endregion

        #region Functions for SQL Queries

        /// <summary>
        /// Executes Query and returns an open SqlDataReader
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <returns>Open SqlDataReader</returns>
        public static SqlDataReader GetDataReader(string SqlQuery)
        {
            SqlDataReader dr = null;
            SqlCommand command = new SqlCommand();
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getConnection();
                command = new SqlCommand(SqlQuery, connection);

                command.Connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dr;
        }
        /// <summary>
        /// Executes Query And returns SqlDataAdapter
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <returns> SqlDataAdapter</returns>        
        public static SqlDataAdapter GetDataAdapter(string SqlQuery)
        {
            SqlCommand command = new SqlCommand();
            SqlConnection connection = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection = getConnection();

                ////Mark As Sql Text i.e. Query
                command.CommandType = CommandType.Text;
                command.CommandText = SqlQuery;
                command.Connection = connection;

                da.SelectCommand = command;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return da;
        }
        /// <summary>
        /// Executes Query and returns DataSet
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>	
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string SqlQuery)
        {
            SqlConnection connection = new SqlConnection(); // = getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand command = new SqlCommand();
            try
            {
                connection = getConnection();

                //Mark As Sql Text i.e. Query
                command.CommandType = CommandType.Text;
                command.CommandText = SqlQuery;
                command.Connection = connection;

                da.SelectCommand = command;
                da.Fill(ds);

                /*
                 * Another way to get SqlDataAdapter
			  
                 da = new SqlDataAdapter(SqlQuery, connection);
                  da.Fill(ds); 
			  
                */
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return ds;
        }
        /// <summary>
        /// Executes Query and returns DataSet of a given table in query
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <param name="TableName"> Table name whose Dataset is to be returned</param>
        /// <returns>DataSet</returns>		
        public static DataSet GetDataSet(string SqlQuery, string TableName)
        {
            SqlConnection connection = new SqlConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand command = new SqlCommand();
            try
            {
                connection = getConnection();

                //Mark As Sql Text i.e. Query
                command.CommandType = CommandType.Text;
                command.CommandText = SqlQuery;
                command.Connection = connection;

                da.SelectCommand = command;
                da.Fill(ds, TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return ds;
        }
        /// <summary>
        /// Executes Query and returns DataTable
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>	
        /// <returns>DataTable</returns>		
        public static DataTable GetDataTable(string SqlQuery)
        {
            DataTable datatable = new DataTable();
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getConnection();
                SqlDataAdapter da = new SqlDataAdapter(SqlQuery, connection);
                da.Fill(datatable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
            }
            return datatable;
        }
        /// <summary>
        /// Executes Query and returns DataView
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>		
        /// <returns>DataView</returns>
        public static DataView GetDataView(string SqlQuery)
        {
            SqlConnection connection = new SqlConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            DataView dv = new DataView();

            try
            {
                connection = getConnection();
                da = new SqlDataAdapter(SqlQuery, connection);
                da.Fill(ds);
                dv.Table = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
            }
            return dv;
        }
        /// <summary>
        /// Checks if record for the given Query exists or not
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <returns>true if record exist otherwise false</returns>
        public static bool IsRecordExist(string SqlQuery)
        {
            bool result = false;
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            try
            {
                connection = getConnection();
                command = new SqlCommand(SqlQuery, connection);
                command.Connection.Open();
                int numRec = (command.ExecuteScalar() == null ? 0 : (int)command.ExecuteScalar());

                //If record exists
                if (numRec > 0) result = true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return result;
        }
        /// <summary>
        /// Executes Query and returns number of rows affected by Query
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>		
        /// <returns>Number of rows affected</returns>
        public static int ExecuteSQL(string SqlQuery)
        {
            int i = 0;
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();

            try
            {
                connection = getConnection();
                connection.Open();

                //Mark As Sql Text i.e. Query
                command.CommandType = CommandType.Text;
                command.CommandText = SqlQuery;
                command.Connection = connection;

                i = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        public static int ExecuteCommand(SqlCommand command)
        {
            int i = 0;
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getConnection();
                connection.Open();

                command.CommandType = CommandType.Text;
                command.Connection = connection;

                i = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }

        /// <summary>
        /// Executes Query and returns the result in string format 
        /// Please give alias to the column as 'Value' for which you want result
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <returns>Result of Query in string format</returns>
        public static string ResultInString(string SqlQuery)
        {
            string strValue = ""; // temp. hold ID Value
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            try
            {
                connection = getConnection();
                command = new SqlCommand(SqlQuery, connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dr.HasRows == true)
                {
                    while (dr.Read() == true)
                        strValue = dr["Value"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return strValue;

        }

	public static string ResultInString(SqlCommand command)
        {
            string strValue = ""; // temp. hold ID Value
            SqlConnection connection = new SqlConnection();
            
            try
            {
                connection = getConnection();
                command.Connection = connection;
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dr.HasRows == true)
                {
                    while (dr.Read() == true)
                        strValue = dr["Value"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return strValue;

        }
        /// <summary>
        /// Executes Query and returns the two result in string format 
        /// Please give alias to the First column as 'Value'and Second Column as 'SecondValue' for which you want result
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <returns>Result of Query in string format</returns>
        /// <returns>Second Result of Query in string format as Reference Value</returns>
        public static string ResultInString(string SqlQuery, ref string SecondResult)
        {
            string strValue = ""; // temp. hold ID Value
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            try
            {
                connection = getConnection();
                command = new SqlCommand(SqlQuery, connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dr.HasRows == true)
                {
                    while (dr.Read() == true)
                    {
                        strValue = dr["Value"].ToString();

                        SecondResult = dr["SecondValue"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return strValue;

        }
        /// <summary>
        /// Executes Query and returns the result in integer format 
        /// Please give alias to the column as 'ID' for which you want result
        /// </summary>
        /// <param name="SqlQuery">SQL Query statement</param>
        /// <returns>Result of Query in Integer format</returns>
        public static int ResultInInteger(string SqlQuery)
        {
            int tempID = 0; // temp.  ID Value
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            SqlDataReader dr = null;

            try
            {
                connection = getConnection();
                command = new SqlCommand(SqlQuery, connection);

                command.Connection.Open();
                dr = command.ExecuteReader(System.Data.CommandBehavior.Default);
                if (dr.HasRows == true)
                {
                    while (dr.Read() == true)
                    {
                        if (!dr.IsDBNull(0))
                            tempID = Convert.ToInt32(dr["ID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return tempID;
        }

        public static int ResultInInteger(SqlCommand command)
        {
            int tempID = 0; // temp.  ID Value
            SqlConnection connection = new SqlConnection();
            SqlDataReader dr = null;

            try
            {
                connection = getConnection();
                command.Connection = connection;
                command.Connection.Open();

                dr = command.ExecuteReader(System.Data.CommandBehavior.Default);
                if (dr.HasRows == true)
                {
                    while (dr.Read() == true)
                    {
                        if (!dr.IsDBNull(0))
                            tempID = Convert.ToInt32(dr["ID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return tempID;
        }
        #endregion

        #region  Functions for Stored Procedures

        /// <summary>
        /// Executes Stored procedure and returns SqlDataReader
        /// </summary>
        /// <param name="ProcedureName">Stored Procedure name</param>
        /// <param name="command">SqlCommand object</param>
        /// <returns>SqlDataReader</returns>	
        public static SqlDataReader GetDataReader(string ProcedureName, SqlCommand command)
        {
            SqlDataReader dr = null;
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                command.Connection.Open();
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                int ParamCount = command.Parameters.Count;
                int JobId = 0, lUser = 0;
                string ParamName = "", ParamValue = "", ParamDescription = "";

                bool IsJobId = command.Parameters.Contains("@JobId");
                bool IsUser = command.Parameters.Contains("@lUser");

                if (IsJobId == true)
                    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                if (IsUser == true)
                    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                else if (command.Parameters.Contains("@UserId"))
                    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                if (ParamCount > 0)
                {
                    for (int i = 0; i < ParamCount; i++)
                    {
                        ParamName = command.Parameters[i].ParameterName;
                        if (command.Parameters[i].Value != null)
                            ParamValue = command.Parameters[i].Value.ToString();

                        ParamDescription += ParamName + ": " + ParamValue + " *** ";
                    }
                }

                ErrorLog.LogToDatabase(JobId, "GetDataReader Function", ProcedureName, ex.Message, ex, ParamDescription, lUser);

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();

                throw ex;
            }
            
            return dr;
        }
        /// <summary>
        /// Execute Stored Procedure And Returns data Adapter as a result set
        /// </summary>
        /// <param name="ProcedureName">Stored Procedure name</param>
        /// <param name="command">SqlCommand object</param>
        /// <returns> SqlDataAdapter</returns>        
        public static SqlDataAdapter GetDataAdapter(string ProcedureName, SqlCommand command)
        {
            SqlConnection connection = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                da.SelectCommand = command;
            }
            catch (Exception ex)
            {
                int ParamCount = command.Parameters.Count;
                int JobId = 0, lUser = 0;
                string ParamName = "", ParamValue = "", ParamDescription = "";

                bool IsJobId = command.Parameters.Contains("@JobId");
                bool IsUser = command.Parameters.Contains("@lUser");

                if (IsJobId == true)
                    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                if (IsUser == true)
                    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                else if (command.Parameters.Contains("@UserId"))
                    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                if (ParamCount > 0)
                {
                    for (int i = 0; i < ParamCount; i++)
                    {
                        ParamName = command.Parameters[i].ParameterName;
                        if (command.Parameters[i].Value != null)
                            ParamValue = command.Parameters[i].Value.ToString();

                        ParamDescription += ParamName + ": " + ParamValue + " *** ";
                    }
                }

                ErrorLog.LogToDatabase(JobId, "GetDataAdapter Function", ProcedureName, ex.Message, ex, ParamDescription, lUser);

                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return da;
        }
        /// <summary>
        /// Returns dataset from stored procedure
        /// </summary>
        /// <param name="ProcedureName">Stored procedure name</param>
        /// <param name="command">SqlCommand object</param>
        /// <returns>Data set</returns>
        public static DataSet GetDataSet(string ProcedureName, SqlCommand command)
        {
            SqlConnection connection = new SqlConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                int ParamCount = command.Parameters.Count;
                int JobId = 0, lUser = 0;
                string ParamName = "", ParamValue = "", ParamDescription = "";

                bool IsJobId = command.Parameters.Contains("@JobId");
                bool IsUser = command.Parameters.Contains("@lUser");

                if (IsJobId == true)
                    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                if (IsUser == true)
                    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                else if (command.Parameters.Contains("@UserId"))
                    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                if (ParamCount > 0)
                {
                    for (int i = 0; i < ParamCount; i++)
                    {
                        ParamName = command.Parameters[i].ParameterName;
                        if (command.Parameters[i].Value != null)
                            ParamValue = command.Parameters[i].Value.ToString();

                        ParamDescription += ParamName + ": " + ParamValue + " *** ";
                    }
                }

                ErrorLog.LogToDatabase(JobId, "GetDataSet Function", ProcedureName, ex.Message, ex, ParamDescription, lUser);

                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return ds;
        }
        /// <summary>
        /// Will return a Datatable from a stored procedure
        /// </summary>
        /// <param name="ProcedureName">Name of stored procedure</param>
        /// <param name="command">Command - Mark As Stored Procedure</param>
        /// <returns>Data table</returns>
        public static DataTable GetDataTable(string ProcedureName, SqlCommand command)
        {
            DataTable datatable = new DataTable();
            SqlConnection connection = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                da.SelectCommand = command;
                da.Fill(datatable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return datatable;
        }
        /// <summary>
        /// Will return datatable from stored procedure and table name
        /// </summary>
        /// <param name="ProcedureName">Name Of procedure</param>
        /// <param name="command">SqlCommand object</param>
        /// <param name="tableName">Name of mapping table</param>
        /// <returns>Data table</returns>
        public static DataTable GetDataTable(string ProcedureName, SqlCommand command, string tableName)
        {
            DataTable datatable = new DataTable();
            SqlConnection connection = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                da.SelectCommand = command;
                da.Fill(datatable);
                datatable.TableName = tableName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return datatable;
        }
        /// <summary>
        /// Returns DataView from stored procedure
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="command">SqlCommand object</param>
        /// <returns>Data view</returns>
        public static DataView GetDataView(string ProcedureName, SqlCommand command)
        {
            SqlConnection connection = new SqlConnection();
            DataSet ds = new DataSet();
            DataView dataView = new DataView();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;
                // For Adhoc Report Set Max Possible Command Timeout
                // Time In Seconds - 300000  = 5 Minuts
                //  0 is unlimited
                command.CommandTimeout = 300000; 

                da.SelectCommand = command;
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    dataView = ds.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                int ParamCount = command.Parameters.Count;
                int JobId = 0, lUser = 0;
                string ParamName = "", ParamValue = "", ParamDescription = "";

                bool IsJobId = command.Parameters.Contains("@JobId");
                bool IsUser = command.Parameters.Contains("@lUser");

                if (IsJobId == true)
                    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                if (IsUser == true)
                    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                else if (command.Parameters.Contains("@UserId"))
                    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                if (ParamCount > 0)
                {
                    for (int i = 0; i < ParamCount; i++)
                    {
                        ParamName = command.Parameters[i].ParameterName;
                        if (command.Parameters[i].Value != null)
                            ParamValue = command.Parameters[i].Value.ToString();

                        ParamDescription += ParamName + ": " + ParamValue + " *** ";
                    }
                }

                //ErrorLog.LogToDatabase(JobId, "GetDataView Function", ProcedureName, ex.Message, ex, ParamDescription, lUser);
                                
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }

            return dataView;
        }
        /// <summary>
        /// Will Execute SP and return the number of rows effected
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="command">command - Stored procedure</param>
        /// <returns>integer</returns>
        public static int ExecuteSP(string ProcedureName, SqlCommand command)
        {
            int i = 0;
            SqlConnection connection = new SqlConnection();
                        
            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                connection.Open();
                i = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //int ParamCount = command.Parameters.Count;
                //int JobId = 0, lUser = 0;
                //string ParamName = "", ParamValue = "", ParamDescription = "";

                //bool IsJobId = command.Parameters.Contains("@JobId");
                //bool IsUser = command.Parameters.Contains("@lUser");

                //if (IsJobId == true)
                //    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                //if (IsUser == true)
                //    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                //else if (command.Parameters.Contains("@UserId"))
                //    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                //if (ParamCount > 0)
                //{
                //    for (int j = 0; j < ParamCount; j++)
                //    {
                //        ParamName = command.Parameters[j].ParameterName;
                //        if (command.Parameters[j].Value != null)
                //            ParamValue = command.Parameters[j].Value.ToString();

                //        ParamDescription += ParamName + ": " + ParamValue + " *** ";
                //    }
                //}

                ErrorLog.LogToTextFile("procedure Name: " + ProcedureName, ex);
               // ErrorLog.LogToDatabase(JobId, "ExecuteSP Function", ProcedureName, ex.Message, ex, ParamDescription, lUser);
                
                throw ex;
                //sqlTrans.Rollback();

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return i;
        }
        /// <summary>
        /// it will return count return by a stored procedure
        /// </summary>
        /// <param name="ProcedureName">Name of procedure</param>
        /// <param name="command">command - Stored procedure</param>
        /// <returns>integer</returns>        
        public static int GetSPCount(string ProcedureName, SqlCommand command)
        {
            int result = 0;
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getConnection();
                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;

                connection.Open();
                result = Convert.ToInt32(command.ExecuteScalar());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return result;
        }
        /// <summary>
        /// return one Argument which is returned by a stored procedure 
        /// </summary>
        /// <param name="ProcedureName">Stored Procedure name</param>
        /// <param name="command">command</param>
        /// <param name="OutputParameterIndex">index of the SP output parameter</param>
        /// <returns>string</returns>
        public static string GetSPOutPut(string ProcedureName, SqlCommand command, int OutputParameterIndex)
        {
            SqlDataReader dr = null;
            SqlConnection connection = new SqlConnection();
            string Result = "";

            try
            {
                connection = getConnection();

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;
                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                Result = command.Parameters[OutputParameterIndex].Value.ToString();

            }
            catch (Exception ex)
            {
                int ParamCount  = command.Parameters.Count;
                if (ParamCount > 0)
                { 
                    foreach(string strName in command.Parameters)
                    {
                        string strValue = command.Parameters[strName].Value.ToString();
                    }
                }
              //  ErrorLog.LogToDatabase(0, "DBConnectionString Issue", "getConnection Function", "DB Connection Exception", ex);
                throw ex;
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return Result;
        }
        /// <summary>
        /// return one Argument which is returned by a stored procedure 
        /// </summary>
        /// <param name="ProcedureName">name of stored procedure </param>
        /// <param name="command">sql command</param>
        /// <param name="OutputParameter">name of stored procedure output parameter</param>
        /// <returns>string</returns>
        public static string GetSPOutPut(string ProcedureName, SqlCommand command, string OutputParameter)
        {
            string result = "";
            SqlDataReader dr = null;
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getConnection();

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;
                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                result = command.Parameters[OutputParameter].Value.ToString();
            }
            catch (Exception ex)
            {
                int ParamCount = command.Parameters.Count;
                int JobId = 0, lUser = 0;
                string ParamName = "", ParamValue = "", ParamDescription = "";

                bool IsJobId = command.Parameters.Contains("@JobId");
                bool IsUser = command.Parameters.Contains("@lUser");

                if (IsJobId == true)
                    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                if (IsUser == true)
                    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                else if (command.Parameters.Contains("@UserId"))
                    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                if (ParamCount > 0)
                {
                    for (int i = 0; i < ParamCount; i++ )
                    {
                        ParamName = command.Parameters[i].ParameterName;
                        if(command.Parameters[i].Value != null)
                            ParamValue = command.Parameters[i].Value.ToString();

                        ParamDescription +=  ParamName + ": " + ParamValue + " *** ";
                    }
                }

                ErrorLog.LogToDatabase(JobId, "GetSPOutPut Function", ProcedureName, ex.Message, ex, ParamDescription,lUser);

                //MyTrans.Rollback("Before");                 
                throw ex;
                //sqlTrans.Rollback();
            }

            finally
            {
                if (dr != null)
                {
                    dr.Close();

                    dr.Dispose();
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                command.Dispose();

                connection.Dispose();

            }

            return result;
        }

        #endregion

        #region Functions to bind controls

        /// <summary>
        ///  Will bind dropdown controls from database                
        /// </summary>
        /// <param name="ctlName">Name of dropdown control</param>
        /// <param name="SqlQuery">SQL query</param>
        /// <param name="DisplayValue">Values to show in control</param>
        /// <param name="ValueCntrl">Value of the control item</param>        
        public static void BindControls(DropDownList ctlName, string SqlQuery, string DisplayValue, string ValueCntrl)
        {
            if (ctlName != null && ctlName.Items.Count > 0)
                ctlName.Items.Clear();
            try
            {
                ctlName.DataSource = GetDataView(SqlQuery);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
                // Following code adds 'Select' to the DropDown         
                ListItem First = new ListItem(" - Select - ", "0", true);
                ctlName.Items.Insert(0, First);

                for (int i = 0; i < ctlName.Items.Count; i++)
                {
                    ctlName.Items[i].Attributes.Add("title", ctlName.Items[i].Text);
                }

            }
            catch (Exception ex)
            {
                int JobId = 0, lUser = 0;
                
                string ParamDescription = SqlQuery + "**** Control Name:" + ctlName.ID + " *** Display Valye:" + DisplayValue + " ***ValueCntrl=" + ValueCntrl;
                
                ErrorLog.LogToDatabase(JobId, "BindControls Function", "Sql Query", ex.Message, ex, ParamDescription, lUser);

                throw ex;
            }
        }
        /*
        public static void BindControls(ComboBox ctlName, string SqlQuery, string DisplayValue, string ValueCntrl)
        {
            if (ctlName != null && ctlName.Items.Count > 0)
                ctlName.Items.Clear();
            try
            {
                ctlName.DataSource = GetDataView(SqlQuery);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
                // Following code adds 'Select' to the DropDown                 //SUDHIR 
                ListItem First = new ListItem(" - Select - ", "0", true);
                ctlName.Items.Insert(0, First);

                for (int i = 0; i < ctlName.Items.Count; i++)
                {
                    ctlName.Items[i].Attributes.Add("title", ctlName.Items[i].Text);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        public static void BindControls(DropDownList ctlName, string SqlProcedure, SqlCommand command, string DisplayValue, string ValueCntrl)
        {

            if (ctlName != null && ctlName.Items.Count > 0)
                ctlName.Items.Clear();
            try
            {
                ctlName.DataSource = GetDataView(SqlProcedure, command);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
                // Following code adds 'Select' to the DropDown
                ListItem First = new ListItem(" - Select - ", "0", true);
                ctlName.Items.Insert(0, First);

                for (int i = 0; i < ctlName.Items.Count; i++)
                {
                    ctlName.Items[i].Attributes.Add("title", ctlName.Items[i].Text);
                }

            }
            catch (Exception ex)
            {
                int JobId = 0, lUser = 0;

                string ParamDescription = "**** Control Name:" + ctlName.ID + " *** Display Valye:" + DisplayValue + " ***ValueCntrl=" + ValueCntrl;
                
                ErrorLog.LogToDatabase(JobId, "BindControls Function", SqlProcedure, ex.Message, ex, ParamDescription, lUser);

                throw ex;
            }
        }
                
        /// <summary>
        ///  Will bind radio button controls from database                
        /// </summary>
        /// <param name="ctlName">Name of radio control</param>
        /// <param name="SqlQuery">SQL query</param>
        /// <param name="DisplayValue">Values to show in control</param>
        /// <param name="ValueCntrl">Value of the control item</param>      
        public static void BindControls(RadioButtonList ctlName, string SqlQuery, string DisplayValue, string ValueCntrl)
        {
            try
            {
                ctlName.DataSource = GetDataView(SqlQuery);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  Will bind Check Box list controls from database                
        /// </summary>
        /// <param name="ctlName">Name of check box list</param>
        /// <param name="SqlQuery">SQL query</param>
        /// <param name="DisplayValue">Values to show in control</param>
        /// <param name="ValueCntrl">Value of the control item</param>      
        public static void BindControls(CheckBoxList ctlName, string SqlQuery, string DisplayValue, string ValueCntrl)
        {
            try
            {
                ctlName.DataSource = GetDataView(SqlQuery);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void BindControls(CheckBoxList ctlName, string SqlProcedure, SqlCommand command, string DisplayValue, string ValueCntrl)
        {

            try
            {
                ctlName.DataSource = GetDataView(SqlProcedure, command);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  Will bind ListBox controls from database                
        /// </summary>
        /// <param name="ctlName">Name of check box list</param>
        /// <param name="SqlQuery">SQL query</param>
        /// <param name="DisplayValue">Values to show in control</param>
        /// <param name="ValueCntrl">Value of the control item</param>      
        public static void BindControls(ListBox ctlName, string SqlQuery, string DisplayValue, string ValueCntrl)
        {
            try
            {
                ctlName.DataSource = GetDataView(SqlQuery);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 
        public static void BindControls(ListBox ctlName, string SqlProcedure, SqlCommand command, string DisplayValue, string ValueCntrl)
        {

            if (ctlName != null && ctlName.Items.Count > 0)
                ctlName.Items.Clear();
            try
            {
                ctlName.DataSource = GetDataView(SqlProcedure, command);
                ctlName.DataTextField = DisplayValue;
                ctlName.DataValueField = ValueCntrl;
                ctlName.DataBind();
          

                for (int i = 0; i < ctlName.Items.Count; i++)
                {
                    ctlName.Items[i].Attributes.Add("title", ctlName.Items[i].Text);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Function to Bind DropDown using Enumaration



        #endregion

        #region FA Command Connection
        public static DataSet GetFADataSet(string SqlQuery)
        {
            SqlConnection connection = new SqlConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand command = new SqlCommand();
            try
            {
                connection = getFAConnection();

                //Mark As Sql Text i.e. Query
                command.CommandType = CommandType.Text;
                command.CommandText = SqlQuery;
                command.Connection = connection;

                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            return ds;
        }

        public static string GetFASPOutPut(string ProcedureName, SqlCommand command, string OutputParameter)
        {
            string result = "-123";
            SqlDataReader dr = null;
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = getFAConnection();

                //Mark As Stored Procedure
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = databaseOwner + "." + ProcedureName;
                command.Connection = connection;
                connection.Open();

                dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                result = command.Parameters[OutputParameter].Value.ToString();
            }

            catch (Exception ex)
            {
                int ParamCount = command.Parameters.Count;
                int JobId = 0, lUser = 0;
                string ParamName = "", ParamValue = "", ParamDescription = "";

                bool IsJobId = command.Parameters.Contains("@JobId");
                bool IsUser = command.Parameters.Contains("@lUser");

                if (IsJobId == true)
                    JobId = Convert.ToInt32(command.Parameters["@JobId"].Value);

                if (IsUser == true)
                    lUser = Convert.ToInt32(command.Parameters["@lUser"].Value);
                else if (command.Parameters.Contains("@UserId"))
                    lUser = Convert.ToInt32(command.Parameters["@UserId"].Value);

                if (ParamCount > 0)
                {
                    for (int i = 0; i < ParamCount; i++)
                    {
                        ParamName = command.Parameters[i].ParameterName;
                        if (command.Parameters[i].Value != null)
                            ParamValue = command.Parameters[i].Value.ToString();

                        ParamDescription += ParamName + ": " + ParamValue + " *** ";
                    }
                }

                ErrorLog.LogToDatabase(JobId, "GetFASPOutPut Function", ProcedureName, ex.Message, ex, ParamDescription, lUser);

                //MyTrans.Rollback("Before");                 
                throw ex;
                //sqlTrans.Rollback();
            }

            finally
            {
                if (dr != null)
                {
                    dr.Close();

                    dr.Dispose();
                }

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                command.Dispose();

                connection.Dispose();
            }

            return result;
        }
        #endregion
    }
