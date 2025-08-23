using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using BSImport.UserManager.BO;


namespace BSImport.UserManager.DAL
{
    /// <summary>
    /// Summary description for UserDB
    /// </summary>
    public class UserDB
    {
        #region Public Mathods

        public static User GetItem(int UserId)
        {
            User CurrentUser = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("GetUserById", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@UserId", UserId);

                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        //CurrentUser = FillDataRecord(myReader);
                    }

                    myReader.Close();
                }

                myConnection.Close();
            }

            return CurrentUser;
        }

        public static UserList GetList()
        {
            UserList tempList = null;
            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("GetUserList", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;

                myConnection.Open();
                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        tempList = new UserList();
                        while (myReader.Read())
                        {
                            //tempList.Add(FillDataRecord(myReader));
                        }
                    }
                    myReader.Close();
                }

                myConnection.Close();
            }
            return tempList;
        }
           public string deleteuser(string lid)
    {
        string i = "";
        int result = -1;
        SqlConnection con = CDatabase.getConnection();
       // SqlCommand cmd = new SqlCommand("Update Bs_UserMs set bdel=1 where lid=@lid",con);
        SqlCommand cmd = new SqlCommand("delUserDetail", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@lid", lid);
        try
        {
            con.Open();
           result= cmd.ExecuteNonQuery();
            
                i = "0";
            

        }
        catch (Exception ex)
        {
            i = "1";
        }

        finally
        {
            con.Close();
        }

        return i.ToString();
    }
        public static int Save(User myUser)
        {
            int result = 0;
            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("AddUser", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;

                
                    myCommand.Parameters.AddWithValue("@UserId", myUser.Userid);
            
                myCommand.Parameters.AddWithValue("@Name", myUser.Username);
                myCommand.Parameters.AddWithValue("@UserName", myUser.Username);
                myCommand.Parameters.AddWithValue("@Deptid", myUser.Deptid);
                myCommand.Parameters.AddWithValue("@Designid", myUser.Designid);
                myCommand.Parameters.AddWithValue("@Divisionid", myUser.Divid);
                myCommand.Parameters.AddWithValue("@Branchid", myUser.Branchid);
                myCommand.Parameters.AddWithValue("@HOD", myUser.Hod);
                myCommand.Parameters.AddWithValue("@EmpCode", myUser.Empcode);
                myCommand.Parameters.AddWithValue("@Email", myUser.Email);
                myCommand.Parameters.AddWithValue("@Contactno", myUser.Contactno);
               
                myCommand.Parameters.AddWithValue("@bdel", myUser.Bdel);
              
                DbParameter returnValue;
                returnValue = myCommand.CreateParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                myCommand.Parameters.Add(returnValue);

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                result = Convert.ToInt32(returnValue.Value);
                myConnection.Close();
            }
            return result;
        }

       //public static int updateuser(string userid)
       // {
       //     int result = 0;
       //     using (SqlConnection myConnection = new SqlConnection(AppConfiguration.ConnectionString))
       //     {
       //         SqlCommand myCommand = new SqlCommand("aspnet_membership_updateuser", myConnection);
       //         myCommand.CommandType = CommandType.StoredProcedure;

               
       //             myCommand.Parameters.AddWithValue("@UserId", DBNull.Value);
                
                
       //             myCommand.Parameters.AddWithValue("@UserId", myUser.UserId);
                

       //         myCommand.Parameters.AddWithValue("@Name", myUser.Name);
       //         myCommand.Parameters.AddWithValue("@UserName", myUser.UserName);
       //         myCommand.Parameters.AddWithValue("@EmpCode", myUser.EmpCode);

       //         DbParameter returnValue;
       //         returnValue = myCommand.CreateParameter();
       //         returnValue.Direction = ParameterDirection.ReturnValue;
       //         myCommand.Parameters.Add(returnValue);

       //         myConnection.Open();
       //         myCommand.ExecuteNonQuery();
       //         result = Convert.ToInt32(returnValue.Value);
       //         myConnection.Close();
       //     }
       //     return result;



       // }

        public static bool Delete(int UserId)
        {
            int result = 0;
            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("DeleteUser", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@UserId", UserId);
                myConnection.Open();
                result = myCommand.ExecuteNonQuery();
                myConnection.Close();
            }

            return result > 0;
        }

        #endregion

        //private static User FillDataRecord(IDataRecord myDataRecord)
        //{
        //    //User CurrUser = new User();

        //    //CurrUser.Userid     =   myDataRecord.GetInt32(myDataRecord.GetOrdinal("UserId"));
        //    //CurrUser.Username   =   myDataRecord.GetString(myDataRecord.GetOrdinal("UserName"));
        //    //CurrUser.Empcode    =   myDataRecord.GetString(myDataRecord.GetOrdinal("EmpCode"));

        //    //return CurrUser;
        //}
                
        public int insertuser(string username, string email, int DivisionId, string empcode, 
            string mobile, string address, int Deptid, int BSUserType, int UserRole,int luser)
        { 
            int Result = 0;
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("insUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Empname",username);
            cmd.Parameters.AddWithValue("@Email",email);
            cmd.Parameters.AddWithValue("@Deptid", Deptid);
            cmd.Parameters.AddWithValue("@Empcode", empcode);
            cmd.Parameters.AddWithValue("@DivisionId", DivisionId);
            cmd.Parameters.AddWithValue("@Mobile", mobile);
            cmd.Parameters.AddWithValue("@Address", address);
           // cmd.Parameters.AddWithValue("@BSUserType", BSUserType);
            cmd.Parameters.AddWithValue("@UserRole", UserRole);
            cmd.Parameters.AddWithValue("@lUser", luser);
            con.Open();
            try
            {
               Result = cmd.ExecuteNonQuery();
               // i = "0";
            }
            catch (Exception ex)
            {
                Result = 2; // SP Execution Error
                //  i = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }

            return Result;

        }


        public int updateuser(int lid,string username, string email, int DivisionId, string empcode, 
            string mobile, string address, int deptid,int BSUserType,int UserRole,int lUser)
        {
            int Result  = 0;
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("updUserDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@lid", lid);
            cmd.Parameters.AddWithValue("@Empname", username);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@DeptId", deptid);
            cmd.Parameters.AddWithValue("@DivisionId", DivisionId);
            cmd.Parameters.AddWithValue("@EmpCode", empcode);
            cmd.Parameters.AddWithValue("@Mobile", mobile);
            cmd.Parameters.AddWithValue("@Address", address);
          //  cmd.Parameters.AddWithValue("@BSUserType", BSUserType);
            cmd.Parameters.AddWithValue("@UserRole", UserRole);
            cmd.Parameters.AddWithValue("@lUser", lUser);
            con.Open();
            try
            {
                Result  =   cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Result = 2; // SP Execution Error
                //i = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }

            return Result;

        }

        public int UpdatePersonalDetail(int lid, string email,string mobile, string address)
        {
            int Result1= -1;
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("updUserPersonalDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@lid", lid);
            
            //cmd.Parameters.AddWithValue("@Email", email);
            //cmd.Parameters.AddWithValue("@Mobile", mobile);
            //cmd.Parameters.AddWithValue("@Address", address);

            cmd.Parameters.Add("@lid", SqlDbType.Int).Value = lid;
            cmd.Parameters.Add("@sEmail", SqlDbType.NVarChar).Value = email;
            cmd.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = mobile;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = address;
            cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

            con.Open();
            try
            {
                Result1 = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Result1 = 2; // SP Execution Error
                //i = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }

            return Result1;

        }
    }
}