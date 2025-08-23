using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using BSImport.BranchManager.BO;

namespace BSImport.BranchManager.DAL
{
    /// <summary>
    /// Summary description for BranchDB
    /// </summary>
    public class BranchDB
    {
        #region Public Methods
        public DataTable getallbranch()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds=new DataSet();
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("getallbranch", con);
            try
            {
                con.Open();
                da.SelectCommand = cmd;
                da.Fill(ds);

            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }

        public static Branch GetItem(int BranchId)
        {
            Branch CurrentBranch = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    = new SqlCommand("GetBranchById", myConnection);
                myCommand.CommandType   = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("BranchId", BranchId);

                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.Read())
                    {
                        CurrentBranch = FillDataRecord(myReader);
                    }

                    myReader.Close();
                }

                myConnection.Close();
            }

            return CurrentBranch; 
        
        }
        
        public static BranchList GetList()
        {
            BranchList tempBranch = null;

            using (SqlConnection myConnection = CDatabase.getConnection())
            {
                SqlCommand myCommand    =   new SqlCommand("GetBranchList", myConnection);
                myCommand.CommandType   =   CommandType.StoredProcedure;
                myConnection.Open();

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            tempBranch.Add(FillDataRecord(myReader));
                        }
                    }

                    myReader.Close();
                }
                myConnection.Close();
            }

            return tempBranch;
        }
        #endregion

        private static Branch FillDataRecord(IDataRecord myDataRecord)
        {
            Branch CurrBranch = new Branch();

            CurrBranch.BranchId     =   myDataRecord.GetInt32(myDataRecord.GetOrdinal("lId"));
            CurrBranch.BranchName   =   myDataRecord.GetString(myDataRecord.GetOrdinal("BranchName"));
            CurrBranch.ContactNo    =   myDataRecord.GetString(myDataRecord.GetOrdinal("ContactNo"));

            return CurrBranch;
        }

        public  string insertuserbranch(string uid,string branchid,string remarks)
        {
            string i="";
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("sp_insertbs_user_branchms", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@branchid",branchid);
                        
                        cmd.CommandType = CommandType.StoredProcedure;
            
                        con.Open();
                        try
                        {

                            Convert.ToInt32(cmd.ExecuteScalar());
                            i="0";
                        }
                        catch(Exception ex)
                        {
                            i = ex.Message.ToString();
                        }
                        finally
                        {
                            con.Close();
                            
                        }

                        return i;
        }

        public DataTable getbranchbyuserid(string userid)
        {
            SqlConnection con = CDatabase.getConnection();

            SqlDataAdapter da = new SqlDataAdapter("select BS_BranchMS.lid,BS_BranchMS.*,BS_User_branch.lid as userbranchlid from BS_User_branch Left join  BS_BranchMS on BS_BranchMS.lid=BS_User_branch.branchid where BS_User_branch.bdel=0 and BS_User_branch.userid=" + userid.Trim(), con);
            DataTable dt = new DataTable();
            
            try
            {
                con.Open();
                da.Fill(dt);


            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public string updateuserbranch(string uid, string branchid,string lid,string nf)
        {
            string i="0";
            if (lid == "&nbsp;")
            {
                lid = "";
            }
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("sp_updateuser_branch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nflid", nf);
            cmd.Parameters.AddWithValue("@lid", lid);
            cmd.Parameters.AddWithValue("@uid",uid);
            cmd.Parameters.AddWithValue("@branchid",branchid);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                i = "0";
            }
            catch(Exception ex)
            {
                i = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return i.ToString();

        }

        public string inserbranch(string branchname,string branchcode,string city,string address,string contactno)
        {
            string i = "";
            SqlConnection con = CDatabase.getConnection();
            
            SqlCommand cmd = new SqlCommand("sp_inserbranch",con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@branchname",branchname);
            //cmd.Parameters.AddWithValue("@branchcode", branchcode);
            //cmd.Parameters.AddWithValue("@cityId", city);
            //cmd.Parameters.AddWithValue("@address", address);
            //cmd.Parameters.AddWithValue("@contactno", contactno);
            cmd.Parameters.Add("@branchname",SqlDbType.NVarChar,200).Value=branchname;
            cmd.Parameters.Add("@branchcode", SqlDbType.NVarChar, 200).Value=branchcode;
            cmd.Parameters.Add("@cityId", SqlDbType.Int, 8).Value=city;
            cmd.Parameters.Add("@address", SqlDbType.NVarChar, 200,"Address").Value=address;
            cmd.Parameters.Add("@contactno", SqlDbType.NVarChar, 200).Value=contactno;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                i = "0";
            }
            catch (Exception ex)
            {
                i = ex.Message.ToString();
            }
            finally
            {
            }
            return i;


        }

        public string getmaxbranchlid()
        {
            string i = "";
            DataSet ds=new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("select max(lid) from bs_branchms", con);
            da.SelectCommand = cmd;

            con.Open();
            try
            {
                da.Fill(ds);
              
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                con.Close();

            }

            return ds.Tables[0].Rows[0][0].ToString();

        }

        //public string insertbranchport(string branchid,string portid)
        //{
        //     string i = "";
        //     SqlConnection con = CDatabase.getConnection();
        //    SqlCommand cmd = new SqlCommand("INS_Insert_Branch_Port", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@branchid", branchid);
        //    cmd.Parameters.AddWithValue("@portid", portid);
        //    try
        //    {
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        i = "0";

        //    }
        //    catch (Exception ex)
        //    {
        //        i = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return i;

        //}

        //public string updatebranchport(string @nflid, string @lid, string @branchid, string @portid)
        //{
        //    string i = "";
        //    SqlConnection con = CDatabase.getConnection();
        //    SqlCommand cmd = new SqlCommand("upd_branch_port", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@nflid", @nflid);
        //    cmd.Parameters.AddWithValue("@lid", @lid);
        //    cmd.Parameters.AddWithValue("@branchid", @branchid);
        //    cmd.Parameters.AddWithValue("@portid", @portid);
            
        //    try
        //    {
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        i = "0";

        //    }
        //    catch (Exception ex)
        //    {
        //        i = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        con.Close();

        //    }

        //    return i.ToString();

        //}
        public DataTable getbranch(string mode,string uid)
        {
            SqlConnection con = CDatabase.getConnection();
            if (mode == "New")
            {
                SqlDataAdapter da = new SqlDataAdapter(
    "SELECT distinct BS_BranchMS.lid,BS_BranchMS.branchname" +
    " FROM   BS_BranchMS left JOIN " +
                          "BS_User_Branch ON BS_BranchMS.lid = BS_User_Branch.BranchId and BS_User_Branch.bdel=0 where BS_BranchMS.bdel=0 ", con);

                //            SqlDataAdapter da = new SqlDataAdapter(
                //"SELECT * from BS_BranchMS", con);
                DataTable dt = new DataTable();
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
            else
            {

                SqlDataAdapter da = new SqlDataAdapter("select * from BS_BranchMS", con);
   

                //            SqlDataAdapter da = new SqlDataAdapter(
                //"SELECT * from BS_BranchMS", con);
                DataTable dt = new DataTable();
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

        public DataTable getbranchbybranchid(string branchid)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("sp_getbranchbylid",con);
            cmd.Parameters.AddWithValue("@lid", branchid);
            cmd.CommandType = CommandType.StoredProcedure;
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

        public string updatebranch(string lid,string branchname, string branchcode, string city, string address, string contactno)
        {
            string i = "";
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("sp_updatebranch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@lid", lid);
            cmd.Parameters.AddWithValue("@branchname", branchname);
            cmd.Parameters.AddWithValue("@branchcode", branchcode);
            cmd.Parameters.AddWithValue("@cityId", city);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@contactno", contactno);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                i = "0";

            }
            catch(Exception ex)
            {
                i = ex.Message.ToString();
            }
            finally
            {
                con.Close();

            }

            return i.ToString();
        }

        public string deletebranch(string lid)
        {
            string i = "";
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("sp_deletebranch", con);
            cmd.Parameters.AddWithValue("@lId", lid);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                i = "0";
            }
            catch(Exception ex)
            {
                i = ex.Message.ToString();

            }
            finally
            {
                con.Close();
            }
            return i.ToString();
        }
        public DataTable getbranchport(string branchid)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = CDatabase.getConnection();
            SqlCommand cmd = new SqlCommand("getPortByBranchId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@branchid", branchid);

            try
            {
                con.Open();
                da.SelectCommand = cmd;
                da.Fill(ds);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];

        }
    }
}
