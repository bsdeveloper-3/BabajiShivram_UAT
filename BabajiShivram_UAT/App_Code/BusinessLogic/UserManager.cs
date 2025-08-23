using System;
using System.ComponentModel;
using System.Transactions;
using BSImport.UserManager.BO;
using BSImport.UserManager.DAL;
using System.Data.SqlClient;
using System.Data;
namespace BSImport.UserManager.BLL
{
    /// <summary>
    /// Summary description for UserManager
    /// </summary>
    public class UserManager
    {
        public static DataTable dt;
        #region Public Methods
        /// <summary>
        /// Gets a list with all Users from the database when all the database contains any User, or null otherwise
        /// </summary>
        /// <returns>A User object when the User Id Exists in the database, or <see langword="null" /> otherwise.</returns>
        public static UserList GetList()
        {
            return UserDB.GetList();
        }
      
        

        /// <summary>
        /// Gets a single User from the Database
        /// </summary>
        /// <param name="UserId">The User ID of the User in the database</param>
        /// <returns>A User object when the ID of the user exists in the database, or <see langword="null"/> otherwise.</returns>
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public static User GetItem(int UserId)
        {
            return GetItem(UserId, false);
        }


        public static User GetItem(int UserId, bool getDetail)
        {
            User myUser = UserManager.GetItem(UserId);
            if (myUser != null && getDetail)
            { 
            
            }
            return myUser;
        }
        
        public int insertuser(string UserName, string Email, int DivisionId, string EmpCode, string Mobile, string Address, int DeptId, int BSUserType,int UserRole,int lUser)
        {
            UserDB objuserdb = new UserDB();
            return objuserdb.insertuser(UserName, Email, DivisionId, EmpCode, Mobile, Address, DeptId, BSUserType, UserRole,lUser);
         

        }
        public int updateuser(int lid, string username, string Email, int DivisionId, string EmpCode, string Mobile,string Address, int DeptId, int BSUserType, int UserRole,int lUser)
        {
            UserDB objuserdb = new UserDB();
            return objuserdb.updateuser(lid, username, Email, DivisionId, EmpCode, Mobile, Address, DeptId, BSUserType, UserRole,lUser);



        }

        public int UpdatePersonalDetail(int lid,  string Email, string Mobile, string Address)
        {
            UserDB objuserPDdb = new UserDB();
            return objuserPDdb.UpdatePersonalDetail(lid,  Email, Mobile, Address);

        }

        public string deleteuser(string lid)
        {
            UserDB objuserdal = new UserDB();
            return objuserdal.deleteuser(lid);
        }
        #endregion
    }
}
