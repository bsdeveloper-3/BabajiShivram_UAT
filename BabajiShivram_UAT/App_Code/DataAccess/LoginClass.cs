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
using System.Collections;
using BSImport;

using BSImport.UserManager.BO;


/// <summary>
/// Summary description for LoginClass
/// </summary>

public class LoginClass : System.Web.UI.Page
{

    public LoginClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
        
    #region Properties

    public int glType // User Type: babaji,Customer user
    {
        get
        {
            int temp = 0;

            if (HttpContext.Current.Session["lType"] != null)
            {
                temp = Convert.ToInt32(HttpContext.Current.Session["lType"].ToString());
            }

            return temp;
        }
        set
        {
            HttpContext.Current.Session["lType"] = value;
        }
    }

    public bool ValidUser
    {
        get
        {
            if (Session["VU"] == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(Session["VU"]);
            }

        }
        set
        {
            Session["VU"] = value;

        }
    }

    public int glFinYearId
    {
        get
        {
            int temp = 0;

            if (HttpContext.Current.Session["FinYearId"] != null)
            {
                temp = Convert.ToInt32(HttpContext.Current.Session["FinYearId"].ToString());
            }

            return temp;
        }
        set
        {
            HttpContext.Current.Session["FinYearId"] = value;
        }
    }

    public string glFinYearName
    {
        get
        {
            string temp = "";

            if (HttpContext.Current.Session["FinYearName"] != null)
            {
                temp = HttpContext.Current.Session["FinYearName"].ToString();
            }

            return temp;
        }
        set
        {
            HttpContext.Current.Session["FinYearName"] = value;
        }
    }

    public int glCompanyId
    {
        get
        {
            int temp = 0;

            if (Session["CID"] != null)
            {
                temp = Convert.ToInt16(Session["CID"].ToString());
            }

            return temp;
        }
        set
        {
            Session["CID"] = value;
        }
    }
        
    public int glRoleId
    {
        get
        {
            int temp = 0;

            if (Session["RID"] != null)
            {
                temp = Convert.ToInt16(Session["RID"].ToString());
            }

            return temp;
        }
        set
        {
            Session["RID"] = value;
        }
    }
    
    public int glModuleId
    {
        get
        {
            int temp = 0;

            if (Session["MID"] != null)
            {
                temp = Convert.ToInt16(Session["MID"].ToString());
            }

            return temp;
        }
        set
        {
            Session["MID"] = value;
        }
    }

    public int glSystemUser
    {
        get
        {
            int temp = -123;
            /*
             * -1 for admin
             *  0 for Normal User 
             */
            if (Session["UTYPE"] != null)
            {
                temp = Convert.ToInt16(Session["UTYPE"].ToString());
            }
            return temp;
        }
        set
        {
            Session["UTYPE"] = value;
        }
    }

    public int glUserId
    {
        get
        {
            int temp = 0;

            if (Session["UserId"] != null)
            {
                temp = (Int32) (Session["UserId"]);
            }

            return temp;
        }
        set
        {
            Session["UserId"] = value;
        }
    }
    
    public string rollname
    {
        get
        {
            if (Session["RollName"] != null)
            {
                return Session["RollName"].ToString();
            }
            else
            {
                return null;
            }
        }
        set
        {
            Session["RollName"] = value;
        }

    }
    
    public string glUserName
    {
        get
        {
            string temp = "";

            if (Session["UserId"] != null || Session["CustUserId"] != null)
            {
                if (Session["UserName"] != null)
                {
                    temp = Session["UserName"].ToString().Trim();
                }

            }
            return temp;
        }
        set
        {
            Session["UserName"] = value;
        }
    }
        
    public string GetDepartmentName
    {
        get
        {
            if (Session["DeptName"] != null)
                return Session["DeptName"].ToString();
            else
                return "";
        }
        set
        {
            Session["DeptName"] = value;
        }
    }
    #endregion
    //Newly added
    #region Properties of CustomerUser

    public int glCustUserId
    {
        get
        {
            int temp = 0;

            if (HttpContext.Current.Session["CustUserId"] != null)
            {
                temp = Convert.ToInt32(HttpContext.Current.Session["CustUserId"]);
            }

            return temp;
        }
        set
        {
            HttpContext.Current.Session["CustUserId"] = value;
        }
    }



    public int glCustId
    {
        get
        {
            int temp = 0;

            if (HttpContext.Current.Session["CustId"] != null)
            {
                temp = Convert.ToInt32(HttpContext.Current.Session["CustId"]);
            }

            return temp;
        }
        set
        {
            HttpContext.Current.Session["CustId"] = value;
        }
    }

    public string glCustName
    {
        get
        {
            string temp = "";

            if (HttpContext.Current.Session["CustId"] != null)
            {
                if (HttpContext.Current.Session["CustName"] != null)
                {
                    temp = HttpContext.Current.Session["CustName"].ToString().Trim();
                }

            }
            return temp;
        }
        set
        {
            HttpContext.Current.Session["CustName"] = value;
        }
    }

    public string glEmpName
    {
        get
        {
            string temp = "";

          //  if (HttpContext.Current.Session["CustUserId"] != null)
          //  {
                if (HttpContext.Current.Session["EmpName"] != null)
                {
                    temp = HttpContext.Current.Session["EmpName"].ToString().Trim();
                }

          //  }
            return temp;
        }
        set
        {
            HttpContext.Current.Session["EmpName"] = value;
        }
    }

    public string isLogin
    {
        get
        {
            if (HttpContext.Current.Session["Islogin"] != null)
            {
                return HttpContext.Current.Session["Islogin"].ToString();


            }
            else
            {
                return "login";

            }

        }
        set
        {

            HttpContext.Current.Session["Islogin"] = value;
        }

    }

    public bool ValidCust
    {
        get
        {
            if (HttpContext.Current.Session["CU"] == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(HttpContext.Current.Session["CU"]);
            }

        }
        set
        {
            HttpContext.Current.Session["CU"] = value;

        }
    }
    #endregion

    #region Functions
    
    public void ClearSession()
    {
        Session["lType"] = null;
        Session["VU"] = null;
        Session["FinYearId"] = null;
        Session["FinYearName"] = null;
        Session["CID"] = null;
        Session["RID"] = null;
        Session["MID"] = null;
        Session["UTYPE"] = null;
        Session["UserId"] = null;
        Session["RollName"] = null;
        Session["UserName"] = null;
        Session["DeptName"] = null;
        Session["CustUserId"] = null;
        Session["CustId"] = null;
        Session["CustName"] = null;
        Session["EmpName"] = null;
        Session["Islogin"] = null;
        Session["CU"] = null;
        Session.Clear();
        Session.RemoveAll();
    }
    
    public int ValidateUser(string userName, string passWord)
    {
        SqlConnection con = CDatabase.getConnection();
        int intStatus = -123; // Customer Login Status Check For Paswsrd Reset
        string lookupPassword = string.Empty;
        
        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", userName);
         
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                glType = Convert.ToInt32(dr["lType"].ToString());
                
		        if (glType == 1 || glType == 3) // Babaji User Or Agent Login
                {
                    glUserId            = (Int32) dr["lId"];
                    glEmpName           = dr["sName"].ToString();
                    glUserName          = userName;
                    lookupPassword      = dr["sCode"].ToString();
                    GetDepartmentName   = dr["DeptName"].ToString();
                    glRoleId            = Convert.ToInt32(dr["lRoleId"].ToString());
                    intStatus           = Convert.ToInt32(dr["Status"]);
                    
                    /*	-1 for admin(System)	 0 for Normal User */
                   
                }
                else if(glType == 2)     //CustomerUser Information
                {
                    glEmpName       = dr["sName"].ToString();
                    glCustUserId    = (Int32)dr["lId"];
                    glCustId        = (Int32)dr["CustomerId"];
                    glCustName      = dr["CustName"].ToString();
                    glUserName      = userName;
                    lookupPassword  = dr["sCode"].ToString();
                    intStatus       = Convert.ToInt32(dr["Status"]);
                }
                else if (glType == -1)    // Babaji Administrator
                {
                    glUserId        = (Int32)dr["lId"];
                    glEmpName       = dr["sName"].ToString();
                    glUserName      = userName;
                    lookupPassword  = dr["sCode"].ToString();
                    intStatus       = Convert.ToInt32(dr["Status"]);
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
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass ValidateUser Function", "ValidateUser", ex.Message, ex, ParamDescription1, 0);

            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For User ID: " + userName;
            ErrorLog.SendMail(strErrorMessage, ex);
        }
        catch (Exception ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            string ParamDescription = "User ID: " + userName ;
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass ValidateUser Function", "ValidateUser", ex.Message, ex, ParamDescription, 0);
            
            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For User ID: "+ userName ;
            ErrorLog.SendMail(strErrorMessage,ex);
        }

        if (intStatus == -123)
        {
            return intStatus; //Error in stored procedure execution;
        }
        else if (intStatus == -124)
        {
            return intStatus; //Error in stored procedure execution;
        }
        else if (intStatus == 20) //Customer user reset password success;
            return intStatus;
        else if (intStatus == 21) // Customer user reset email Failed
            return intStatus;

        // If no password found, return false.

        else if (null == lookupPassword || lookupPassword == "")
        {
            ValidUser = false;
            
            return 2; //Wrong Email Id

        }//END_IF
        else
        {
            // Compare lookupPassword and input passWord, using a case-sensitive comparison.
            if (0 == string.Compare(lookupPassword, passWord, false))
            {
                ValidUser = true;
            
                return 0; // Valid User
            }
            else
            {
                ValidUser = false;
                return 1; // Wrong Password
            }
        }//END_ELSE
    }

    public int SetUserLoginDetail(string userID)
    {
        SqlConnection con = CDatabase.getConnection();
        int intStatus = -123; // Customer Login Status Check For Paswsrd Reset

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("BS_GetUserById", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userID", userID);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                glType      =   Convert.ToInt32(dr["lType"].ToString());
                ValidUser   =   true;
                intStatus   =   0; // Success

                if (glType == 1 || glType == 3) // Babaji User Or Freight Agent User
                {
                    glUserId    = (Int32)dr["lId"];
                    glEmpName   = dr["sName"].ToString();
                    glUserName  = dr["sEmail"].ToString();
                    GetDepartmentName = dr["DeptName"].ToString();
                    glRoleId    = Convert.ToInt32(dr["lRoleId"].ToString());
                }
                else if (glType == 2)     //Customer User Information
                {
                    glEmpName   = dr["sName"].ToString();
                    glCustUserId = (Int32)dr["lId"];
                    glCustId    = (Int32)dr["CustomerId"];
                    glCustName  = dr["CustName"].ToString();
                    glUserName =  dr["sEmail"].ToString(); ;
                }
                else if (glType == -1)    // Babaji Administrator
                {
                    glUserId = (Int32)dr["lId"];
                    glEmpName = dr["sName"].ToString();
                    glUserName = dr["sEmail"].ToString(); 
                }
            }

            dr.Close();
            dr.Dispose();

        }// END_TRY
        catch (Exception ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            string ParamDescription = "User ID: " + userID;
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass SetUserLoginDetail Function", "BS_GetUserById", ex.Message, ex, ParamDescription, 0);

            // Send Error Email To Admin

            string strErrorMessage = "System Set Login Detail Issue For User ID: " + userID;
            ErrorLog.SendMail(strErrorMessage, ex);
        }

        return intStatus;

    }
    
    public int ValidateModuleRole(int ModuleId, int RoleId)
    {
        int intStatus = -123; // Module -Role Access Status
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
        cmd.Parameters.AddWithValue("@RoleId", RoleId);
        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        try
        {
            intStatus = Convert.ToInt32(CDatabase.GetSPOutPut("ValidateModuleRole", cmd, "@Output"));

        }// END_TRY
        catch (Exception ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            string ParamDescription = "Module ID: " + ModuleId.ToString() + " RoleId=" + RoleId.ToString();
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass ValidateModuleRole Function", "ValidateModuleRole", ex.Message, ex, ParamDescription, 0);

            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For Module ID: " + ModuleId.ToString() + " RoleId=" + RoleId.ToString();
            ErrorLog.SendMail(strErrorMessage, ex);
        }

        return intStatus;
    }

    public int ValidateModulePage(int UserId, int ModuleId, int RoleId)
    {
        string strPageName = GetCurrentPageName();

        int intStatus = -123; // Module -Role Access Status
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.AddWithValue("@lUser", UserId);
        cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
        cmd.Parameters.AddWithValue("@RoleId", RoleId);
        cmd.Parameters.AddWithValue("@PageName", strPageName);

        cmd.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        try
        {
            intStatus = Convert.ToInt32(CDatabase.GetSPOutPut("ValidateModulePage", cmd, "@Output"));

        }// END_TRY
        catch (Exception ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            string ParamDescription = "Module ID: " + ModuleId.ToString() + " RoleId=" + RoleId.ToString();
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass ValidateModuleRole Function", "ValidateModuleRole", ex.Message, ex, ParamDescription, 0);

            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For Module ID: " + ModuleId.ToString() + " RoleId=" + RoleId.ToString();
            ErrorLog.SendMail(strErrorMessage, ex);
        }

        return intStatus;
    }

    public int ValidateCustomer(string userName, string passWord)
    {
        SqlConnection con = CDatabase.getConnection();
        int intStatus = -123; // Customer Login Status Check For Paswsrd Reset
        string lookupPassword = string.Empty;

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateCustomer", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", userName);
            //   cmd.Parameters.AddWithValue("@lType", lType);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                glType = Convert.ToInt32(dr["lType"].ToString());
                
                if (glType == 2)     //CustomerUser Information
                {
                    glEmpName = dr["sName"].ToString();
                    glCustUserId = (Int32)dr["lId"];
                    glCustId = (Int32)dr["CustomerId"];
                    glCustName = dr["CustName"].ToString();
                    glUserName = userName;
                    lookupPassword = dr["sCode"].ToString();
                    intStatus = Convert.ToInt32(dr["Status"]);
                }
            }

        }// END_TRY
        catch (Exception ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            string ParamDescription = "User ID: " + userName;
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass ValidateUser Function", "ValidateUser", ex.Message, ex, ParamDescription, 0);

            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For User ID: " + userName;
            ErrorLog.SendMail(strErrorMessage, ex);
        }

        if (intStatus == -123)
        {
            return intStatus; //Error in stored procedure execution;
        }
        else if (intStatus == 20) //Customer user reset password success;
            return intStatus;
        else if (intStatus == 21) // Customer user reset email Failed
            return intStatus;

        // If no password found, return false.

        else if (null == lookupPassword || lookupPassword == "")
        {
            ValidUser = false;

            return 2; //Wrong Email Id

        }//END_IF
        else
        {
            // Compare lookupPassword and input passWord, using a case-sensitive comparison.
            if (0 == string.Compare(lookupPassword, passWord, false))
            {
                ValidUser = true;

                return 0; // Valid User
            }
            else
            {
                ValidUser = false;
                return 1; // Wrong Password
            }
        }//END_ELSE
    }

    public int ValidateFirstTimeUser(string userName)
    {
        SqlConnection con = CDatabase.getConnection();
        int intStatus = -123; // Customer Login Status Check For Paswsrd Reset
        string lookupPassword = string.Empty;

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateUserResetCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", userName);
            //   cmd.Parameters.AddWithValue("@lType", lType);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                intStatus = Convert.ToInt32(dr["Status"].ToString());
            }

        }// END_TRY
        catch (Exception ex)
        {
            intStatus = -123; // Error In Stored Procedure Execution

            string ParamDescription = "User ID: " + userName;
            // Log To Database
            ErrorLog.LogToDatabase(0, "LoginClass ValidateFirstTimeUser Function", "ValidateFirstTimeUser", ex.Message, ex, ParamDescription, 0);

            // Send Error Email To Admin

            string strErrorMessage = "System Login Issue For User ID: " + userName;
            ErrorLog.SendMail(strErrorMessage, ex);
        }

        if (intStatus == -123)
        {
            return intStatus; //Error in stored procedure execution;
        }
        else
            return intStatus;
        // If no password found, return false.
    }

    public static int UpdateUserPassword(int UserId, string password)
    {
        string Result = "-123";
        
        SqlCommand command = new SqlCommand();
        
        command.Parameters.Add("@lid", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        Result = CDatabase.GetSPOutPut("updUserPassword", command, "@OutPut");

        return Convert.ToInt32(Result);

    }

    public static int UpdateUserLastLoginDate(int UserId, string IPAddress)
    {
        string Result = "-123";
                
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = IPAddress;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        Result = CDatabase.GetSPOutPut("updUserLastLogin", command, "@OutPut");

        return Convert.ToInt32(Result);

    }

    public static int UpdateUserLoginDate(int UserId, int ModuleId, string IPAddress)
    {
        string Result = "-123";

        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@ModuleId", SqlDbType.Int).Value = ModuleId;
        command.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = IPAddress;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        Result = CDatabase.GetSPOutPut("updUserLastLogin", command, "@OutPut");

        return Convert.ToInt32(Result);

    }

    public int ValidateVendorLogin(string userName, string passWord)
    {
        SqlConnection con = CDatabase.getConnection();
        string lookupPassword = string.Empty;

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateVendorLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", userName);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lookupPassword = dr["Passcode"].ToString();
                Session["VendorId"] = dr["lid"].ToString();
            }

            dr.Close();
            dr.Dispose();

        }// END_TRY
        catch (Exception ex)
        {
            //intStatus = -123; // Error In Stored Procedure Execution

            //string ParamDescription = "User ID: " + userName;
            //// Log To Database
            //ErrorLog.LogToDatabase(0, "LoginClass ValidateUser Function", "ValidateUser", ex.Message, ex, ParamDescription, 0);

            //// Send Error Email To Admin

            //string strErrorMessage = "System Login Issue For User ID: " + userName;
            //ErrorLog.SendMail(strErrorMessage, ex);
        }

        // If no password found, return false.

        if (null == lookupPassword || lookupPassword == "")
        {
            ValidUser = false;
            return 2; //Wrong Email Id

        }//END_IF
        else
        {
            // Compare lookupPassword and input passWord, using a case-sensitive comparison.
            if (0 == string.Compare(lookupPassword, passWord, false))
            {
                ValidUser = true;
                return 0; // Valid User
            }
            else
            {
                ValidUser = false;
                return 1; // Wrong Password
            }
        }//END_ELSE
    }

    public int ValidateAlibabaLogin(string userName, string passWord)
    {
        SqlConnection con = CDatabase.getConnection();
        string lookupPassword = string.Empty;

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateAlibabaLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", userName);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lookupPassword = dr["Passcode"].ToString();
                Session["VendorId"] = dr["lid"].ToString();
                glEmpName = dr["sName"].ToString();
                glRoleId = Convert.ToInt32(dr["RoleId"].ToString()); // Specifies role of user 
            }

            dr.Close();
            dr.Dispose();

        }// END_TRY
        catch (Exception ex)
        {
            //intStatus = -123; // Error In Stored Procedure Execution

            //string ParamDescription = "User ID: " + userName;
            //// Log To Database
            //ErrorLog.LogToDatabase(0, "LoginClass ValidateUser Function", "ValidateUser", ex.Message, ex, ParamDescription, 0);

            //// Send Error Email To Admin

            //string strErrorMessage = "System Login Issue For User ID: " + userName;
            //ErrorLog.SendMail(strErrorMessage, ex);
        }

        // If no password found, return false.

        if (null == lookupPassword || lookupPassword == "")
        {
            ValidUser = false;
            return 2; //Wrong Email Id

        }//END_IF
        else
        {
            // Compare lookupPassword and input passWord, using a case-sensitive comparison.
            if (0 == string.Compare(lookupPassword, passWord, false))
            {
                ValidUser = true;
                return 0; // Valid User
            }
            else
            {
                ValidUser = false;
                return 1; // Wrong Password
            }
        }//END_ELSE
    }
    
    public int ValidateTransportLogin(string userName, string passWord)
    {
        SqlConnection con = CDatabase.getConnection();
        string lookupPassword = string.Empty;

        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateTransportLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", userName);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lookupPassword = dr["Passcode"].ToString();
                Session["VendorId"] = dr["lid"].ToString();
                glUserId = (Int32)dr["lId"];
                Session["CID"] = dr["CompanyId"].ToString();
                glEmpName = dr["sName"].ToString();
            }

            dr.Close();
            dr.Dispose();

        }// END_TRY
        catch (Exception ex)
        {
            //intStatus = -123; // Error In Stored Procedure Execution

            //string ParamDescription = "User ID: " + userName;
            //// Log To Database
            //ErrorLog.LogToDatabase(0, "LoginClass ValidateUser Function", "ValidateUser", ex.Message, ex, ParamDescription, 0);

            //// Send Error Email To Admin

            //string strErrorMessage = "System Login Issue For User ID: " + userName;
            //ErrorLog.SendMail(strErrorMessage, ex);
        }

        // If no password found, return false.

        if (null == lookupPassword || lookupPassword == "")
        {
            ValidUser = false;
            return 2; //Wrong Email Id

        }//END_IF
        else
        {
            // Compare lookupPassword and input passWord, using a case-sensitive comparison.
            if (0 == string.Compare(lookupPassword, passWord, false))
            {
                ValidUser = true;
                return 0; // Valid User
            }
            else
            {
                ValidUser = false;
                return 1; // Wrong Password
            }
        }//END_ELSE
    }

    public int ValidateCustomerEmail(string strEmail)
    {
        SqlConnection con = CDatabase.getConnection();
        string lookupPassword = string.Empty;
        int Result = 0;
        try
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand("ValidateCustomerEmail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", strEmail);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();

                if (dr["lid"].ToString() == "0")
                {
                    Result = 0;
                }
                else
                {
                    Result = Convert.ToInt32(dr["lid"]);
                }
            }
            else
            {
                Result = 0;
            }
            dr.Close();
            dr.Dispose();

        }// END_TRY
        catch (Exception ex)
        {
            Result = -1;
        }

        return Result;

    }

    public string GetCurrentPageName()
    {
        string[] urlSegments = System.Web.HttpContext.Current.Request.Url.Segments;
        string strPath = "";
        /*	string strPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
		System.IO.FileInfo oInfo = new System.IO.FileInfo(strPath);
		string strRet = oInfo.Name;
		return strRet;
        */
        int TotalSeagments = (urlSegments.Length - 1);
        if (TotalSeagments == 3)
            strPath = urlSegments[TotalSeagments - 1].ToString() + urlSegments[TotalSeagments].ToString();
        else
            strPath = urlSegments[TotalSeagments].ToString();

        return strPath;
    }
    #endregion

    //#endregion
}
