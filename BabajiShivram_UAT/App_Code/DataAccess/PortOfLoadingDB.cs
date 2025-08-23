using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BSImport.PortOfLoading.BO;
namespace BSImport.PortOfLoading.DAL
{
    /// <summary>
    /// Summary description for PortOfLoadingDB
    /// </summary>
    public class PortOfLoadingDB
    {
        #region Public Methods

        public static PortOfLoadingBO GetItem(int LoadingPortId)
        {
            PortOfLoadingBO CurrentPort = null;

            SqlConnection myConnection = CDatabase.getConnection();

            SqlCommand myCommand = new SqlCommand("GetLoadingPortById", myConnection);
            myCommand.Parameters.Add("lId", SqlDbType.Int).Value = LoadingPortId;
            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();

            using (SqlDataReader myReader = myCommand.ExecuteReader())
            {
                if (myReader.Read())
                {
                    CurrentPort = FillDataRecord(myReader);
                }

                myReader.Close();
            }

            return CurrentPort;
        }

        public static PortOfLoadingList GetList()
        {
            PortOfLoadingList tempList = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand = new SqlCommand("GetPortList", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new PortOfLoadingList();
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
        
        private static PortOfLoadingBO FillDataRecord(IDataRecord myDataRecord)
        {
            PortOfLoadingBO CurrPort = new PortOfLoadingBO();

            CurrPort.PortOfLoadingId = myDataRecord.GetInt32(myDataRecord.GetOrdinal("lId"));
            CurrPort.Name = myDataRecord.GetString(myDataRecord.GetOrdinal("PortOfLodingName"));
            CurrPort.Code = myDataRecord.GetString(myDataRecord.GetOrdinal("PortCode"));

            return CurrPort;
        }
        #endregion

    }
}
