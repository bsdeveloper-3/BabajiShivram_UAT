using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using BSImport.ClientManager.BO;


namespace BSImport.ClientManager.DAL
{
    /// <summary>
    /// Summary description for ClientDB
    /// </summary>
    public class ClientDB
    {
        #region Public Methods

        public static Client GetItem(int ClientId)
        {
            Client CurrentClient = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    = new SqlCommand("GetClientById", myConnection);
                myCommand.CommandType   = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@ClientId", ClientId);

                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        CurrentClient = FillDataRecord(myReader);
                    }

                    myReader.Close();
                }

                myConnection.Close();
            }

            return CurrentClient;
        }

        public static ClientList GetList()
        {
            ClientList tempList = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("GetClientList", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;

                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
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

        public static int Save(Client myClient)
        {
            int result = 0;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("AddClient", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;

                if (myClient.ClientId == -1)
                {
                    myCommand.Parameters.AddWithValue("@ClientId", DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@ClientId", myClient.ClientId);
                }

                myCommand.Parameters.AddWithValue("@Name", myClient.ClientName);
                myCommand.Parameters.AddWithValue("@ContactPerson", myClient.ContactPerson);
                myCommand.Parameters.AddWithValue("@MobileNo", myClient.MobileNo);
                myCommand.Parameters.AddWithValue("@Address", myClient.Address);

                DbParameter returnValue;

                returnValue = myCommand.CreateParameter();
                returnValue.Direction = ParameterDirection.Output;
                myCommand.Parameters.Add(returnValue);

                myConnection.Open();

                myCommand.ExecuteNonQuery();

                result = Convert.ToInt32(returnValue.Value);

                myConnection.Close();
            }

            return  result;
        }

        public static bool Delete(int ClientId)
        {
            int result = 0;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    = new SqlCommand("DeleteClient", myConnection);
                myCommand.CommandType   = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@ClientId", ClientId);

                myConnection.Open();
                result = myCommand.ExecuteNonQuery();
                myConnection.Close();
            }

            return result > 0;
        }
        
        #endregion

        private static Client FillDataRecord(IDataRecord myDataRecord)
        {
            Client CurrClient = new Client();

            CurrClient.ClientId     =   myDataRecord.GetInt32(myDataRecord.GetOrdinal("lId"));
            CurrClient.ClientName   =   myDataRecord.GetString(myDataRecord.GetOrdinal("Name"));
            CurrClient.ContactPerson =  myDataRecord.GetString(myDataRecord.GetOrdinal("ContactPerson"));
            CurrClient.MobileNo     =   myDataRecord.GetString(myDataRecord.GetOrdinal("ModileNo"));

            return CurrClient;
        }
    }
}
