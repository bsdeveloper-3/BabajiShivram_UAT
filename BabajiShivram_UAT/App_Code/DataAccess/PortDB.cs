using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BSImport.PortManager.BO;

namespace BSImport.PortManager.DAL
{
    /// <summary>
    /// Summary description for PortDB
    /// </summary>
    public class PortDB
    {
        #region Public Methods
        
        public static Port GetItem( int PortId)
        {
            Port CurrentPort = null;

            SqlConnection myConnection = CDatabase.getConnection();
            
            SqlCommand myCommand    =   new SqlCommand("GetPortById",myConnection);
            myCommand.CommandType   =   CommandType.StoredProcedure;
            myCommand.Parameters.Add("lId", SqlDbType.Int).Value = PortId;
            myConnection.Open();

            using(SqlDataReader myReader = myCommand.ExecuteReader())
            {
                if(myReader.Read())
                {
                    CurrentPort = FillDataRecord(myReader);
                }

                myReader.Close();
            }
            
            return CurrentPort;
        }

        public static PortList GetList()
        {
            PortList tempList = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("GetPortList", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;

                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if(myReader.HasRows)
                    {
                        tempList = new PortList();
                        while (myReader.Read())
                        {
                            tempList.Add(FillDataRecord(myReader));
                        }
                    }
                    myReader.Close();
                }

                myConnection.Close();
            }

            return tempList;
        }

        public static int Save(Port myPort)
        {
            int result = 0;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("AddPort", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;

                if (myPort.PortId == -1)
                {
                    myCommand.Parameters.AddWithValue("@PortId", DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@PortId", myPort.PortId);
                }

                myCommand.Parameters.AddWithValue("@Name",myPort.Name);
                myCommand.Parameters.AddWithValue("@Code", myPort.Code);

                DbParameter retrunValue;
                retrunValue = myCommand.CreateParameter();
                retrunValue.Direction = ParameterDirection.ReturnValue;
                myCommand.Parameters.Add(retrunValue);

                myConnection.Open();

                myCommand.ExecuteNonQuery();

                result = Convert.ToInt32(retrunValue.Value);
                myConnection.Close();
            }

            return result;
        
        }

        public static bool Delete(int PortId)
        {
            int result = 0;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("DeletePort", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@PortId", PortId);
                myConnection.Open();
                result = myCommand.ExecuteNonQuery();
                myConnection.Close();
            }

            return result > 0;
        }
        #endregion

        private static Port FillDataRecord(IDataRecord myDataRecord)
        {
            Port CurrPort = new Port();

            CurrPort.PortId =   myDataRecord.GetInt32(myDataRecord.GetOrdinal("lId"));
            CurrPort.Name   =   myDataRecord.GetString(myDataRecord.GetOrdinal("PortName"));
            CurrPort.Code   =   myDataRecord.GetString(myDataRecord.GetOrdinal("PortCode"));

            return CurrPort;
        }

        public DataTable getallPort()
        {
            SqlConnection con = CDatabase.getConnection();
            DataTable dt = new DataTable(); ;
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("getallPort", con);
            da.SelectCommand = cmd;
            con.Open();
            try
            {
                da.Fill(dt);

            }
            catch
            {
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}

