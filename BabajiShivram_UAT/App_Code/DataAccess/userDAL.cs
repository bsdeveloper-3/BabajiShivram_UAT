using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Security;
using System.Web.Security;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

public class USERDAL
{
    MembershipUser objmembeshipuser;
    LoginClass LoggedInuser = new LoginClass();
    public List<USERSBO> getusers()
    {
        Membership.ApplicationName = "BSIMPORT";
        List<USERSBO> userslist = new List<USERSBO>();
        MembershipUserCollection users = Membership.GetAllUsers();

        foreach (MembershipUser user in users)
        {
            USERSBO objuserbo = new USERSBO();
            objuserbo.USERID = user.ProviderUserKey.ToString();

            objuserbo.USERNAME = user.UserName;
            objuserbo.USERQUEST = user.PasswordQuestion;
            objuserbo.ISAPPROVED = user.IsApproved;
            objuserbo.USEREMAIL = user.Email;

            userslist.Add(objuserbo);





        }


        return userslist;




    }
    public int insertuser(USERSBO user)
    {
        Membership.ApplicationName = "BSIMPORT";
        int i = 0;

        MembershipCreateStatus objsttus;


        Membership.CreateUser(user.USERNAME, user.Pssword, user.USEREMAIL, user.USERQUEST, user.USERANSWER, user.ISAPPROVED, out objsttus);

        return i;



    }
    public int updteuser(USERSBO user)
    {
        int i = 0;

        Membership.ApplicationName = "BSIMPORT";

        objmembeshipuser = Membership.GetUser(user.USERNAME);





        objmembeshipuser.Email = user.USEREMAIL;
        objmembeshipuser.IsApproved = user.ISAPPROVED;




        Membership.UpdateUser(objmembeshipuser);
        return i;



    }
    public List<USERSBO> getuserbyid(string usernme)
    {
        Membership.ApplicationName = "BSIMPORT";

        USERSBO user = new USERSBO();
        List<USERSBO> userlist = new List<USERSBO>();
        try
        {
            MembershipUser objmembeshipuser = Membership.GetUser(usernme);

            user.USERNAME = objmembeshipuser.UserName;
            user.USERID = objmembeshipuser.ProviderUserKey.ToString();
            user.USERQUEST = objmembeshipuser.PasswordQuestion;

            user.USEREMAIL = objmembeshipuser.Email;
            user.ISAPPROVED = objmembeshipuser.IsApproved;
            userlist.Add(user);
            return userlist;
        }
        catch (NullReferenceException ex)
        {

            return userlist;

        }
        finally
        {

        }



    }
    public int ValidateUser(string userName)
    {
        SqlConnection con = CDatabase.getConnection();
        int intStatus = -123; // Customer Login Status Check For Paswsrd Reset
        string lookupPassword = string.Empty;

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("TSValidateUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", userName);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                LoggedInuser.glType = Convert.ToInt32(dr["lType"].ToString());

                if (LoggedInuser.glType == 1)
                {
                    LoggedInuser.glUserId = (Int32)dr["lId"];
                    LoggedInuser.glEmpName = dr["sName"].ToString();
                    LoggedInuser.glUserName = userName;
                    lookupPassword = dr["sCode"].ToString();
                    LoggedInuser.ValidUser = true;
                    LoggedInuser.glRoleId = Convert.ToInt32(dr["lRoleId"].ToString());
                    intStatus = Convert.ToInt32(dr["Status"]);

                    /*	-1 for admin(System)	 0 for Normal User */

                }

                else if (LoggedInuser.glType == -1)    // Babaji Administrator
                {
                    LoggedInuser.glUserId = (Int32)dr["lId"];
                    LoggedInuser.glEmpName = dr["sName"].ToString();
                    LoggedInuser.glUserName = userName;
                    lookupPassword = dr["sCode"].ToString();
                    intStatus = Convert.ToInt32(dr["Status"]);
                }
            }

            dr.Close();
            dr.Dispose();

        }// END_TRY
        catch (SqlException ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            if (ex.Number == -2) // Timeout Exception
            {
                intStatus = -124;
            }

            string ParamDescription1 = "User ID: " + userName;
            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For User ID: " + userName;

        }

        return intStatus;
    }
}
