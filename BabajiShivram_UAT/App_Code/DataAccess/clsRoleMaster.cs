using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using BSImport;
/// <summary>
/// Summary description for clsRoleMaster
/// </summary>
public class clsRoleMaster : CommonDBClass
{
    //public class clsclsRoleMaster : CommonDBClass
    //{
        private int iRoleId;
        private string ssName;
        private string ssRemarks;

        private int lCompId;
        private int lUserId;
        private int lDate;
        private int dtWefDate;

        SqlConnection conn = new SqlConnection();

        public clsRoleMaster()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int RoleId
        {
            get { return iRoleId; }
            set { iRoleId = value; }
        }

        public string sName
        {
            get { return ssName; }
            set { ssName = value; }
        }

        public string sRemarks
        {
            get { return ssRemarks; }
            set { ssRemarks = value; }
        }

        public int CompId
        {
            get { return lCompId; }
            set { lCompId = value; }
        }
        public int UserId
        {
            get { return lUserId; }
            set { lUserId = value; }
        }
        public int IDate
        {
            get { return lDate; }
            set { lDate = value; }
        }
        public int wefDate
        {
            get { return dtWefDate; }
            set { dtWefDate = value; }
        }

        public int save()
        {
            /*
             *   return 0 for Successfull Insert
             *   return 1 for Successfull Update
             *   return 2 for already Exist
             */
            SqlCommand cmdWAM1602 = new SqlCommand();
            cmdWAM1602.Parameters.Add("@lRoleId", SqlDbType.Int).Value = RoleId;
            cmdWAM1602.Parameters.Add("@sName", SqlDbType.NVarChar).Value = sName;
            cmdWAM1602.Parameters.Add("@sRemarks", SqlDbType.NVarChar).Value = sRemarks;

            cmdWAM1602.Parameters.Add("@lwefDate", SqlDbType.Int).Value = wefDate;
            cmdWAM1602.Parameters.Add("@lUserId", SqlDbType.Int).Value = lUserId;
            cmdWAM1602.Parameters.Add("@lDate", SqlDbType.Int).Value = lDate;
            cmdWAM1602.Parameters.Add("@bDel", SqlDbType.Int).Value = 0;

            return ExecuteStoredProc("INS_UPD_RoleMaster", cmdWAM1602);
        }

        public string checkRoleByName()
        {
            string sroleid = "";
            SqlCommand sqlcmd;

            conn = CDatabase.getConnection();

          //  sqlcmd = new SqlCommand("SELECT lRoleId from WAM1602 WHERE UPPER(sName) = UPPER(@sName)", conn);
            sqlcmd = new SqlCommand("SELECT lRoleId from BS_RoleMS WHERE UPPER(sName) = UPPER(@sName)", conn);
            sqlcmd.Parameters.Add("@sName", SqlDbType.NVarChar).Value = this.sName;

            try
            {
                conn.Open();
                sroleid = sqlcmd.ExecuteScalar().ToString();
            }
            catch (Exception e) { e.Message.ToString(); }
            finally
            {
                conn.Close();
            }
            return sroleid;
        }

        public string checkRoleById()
        {
            string sroleid = "";
            SqlCommand sqlcmd;

            conn = CDatabase.getConnection();

            sqlcmd = new SqlCommand("SELECT lRoleId FROM BS_RoleMS WHERE lRoleId = @lRoleId", conn);
            sqlcmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = this.RoleId;

            try
            {
                conn.Open();
                sroleid = sqlcmd.ExecuteScalar().ToString();
            }
            catch (Exception e) { e.Message.ToString(); }
            finally
            {
                conn.Close();
            }
            return sroleid;
        }
}
