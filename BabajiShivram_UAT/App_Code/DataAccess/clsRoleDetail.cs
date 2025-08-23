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
/// Summary description for RoleDetail
/// </summary>
public class clsRoleDetail : CDatabase
{
    //public class clsRoleDetail : CommonDBClass
    //{
        //Varialbe Declareation 
        private int iRoleId; //Role ID
        private int ilCompId; //Company ID
        private int ilModuleId; //Module ID
        private int ilSubModuleId; //SubModuleId not in use

        private string ccTyp; // Task Type
        private int ilTaskId; //Task Id
        private string ssTaskId; //Task id in String Format
        private int ilTypId; //not know
        private int ilMode; //Right's mode
        private int ibDel;
        private int ilUserId;
        private int ilDate;
        private DateTime dtdtDate;
        private int ilVersion;

        //Class Properties
        public int lRoleId
        {
            get { return iRoleId; }
            set { iRoleId = value; }
        }

        public int lCompId
        {
            get { return ilCompId; }
            set { ilCompId = value; }
        }

        public int lModuleId
        {
            get { return ilModuleId; }
            set { ilModuleId = value; }
        }

        public int lSubModuleId //not in use
        {
            get { return ilSubModuleId; }
            set { ilSubModuleId = value; }
        }

        public string cTyp
        {
            get { return ccTyp; }
            set { ccTyp = value; }
        }
        public int lTaskId
        {
            get { return ilTaskId; }
            set { ilTaskId = value; }
        }
        public string sTaskId
        {
            get { return ssTaskId; }
            set { ssTaskId = value; }
        }
        public int lTypId
        {
            get { return ilTypId; }
            set { ilTypId = value; }
        }
        public int lMode
        {
            get { return ilMode; }
            set { ilMode = value; }
        }
        public int bDel
        {
            get { return ibDel; }
            set { ibDel = value; }
        }
        public int lUserId
        {
            get { return ilUserId; }
            set { ilUserId = value; }
        }
        

        //Class Table
        DataTable dtgrdUserRights = new DataTable();


        //Class Constructor
        public clsRoleDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /*
        *  0 - All
        *  1 - Add
        *  2 - Edit
        *  3 - del
        *  4 - Prev
        */
        
        //this function is use to save Pagewise access for single checkbox gridview (for All lMode)
        public int saveSingleMode(GridView gvUserRights)
        {
            int retval = 1;
            int iTypId = 0;
           // string ConnStr = System.Configuration.ConfigurationManager.AppSettings["constr"].ToString();
            SqlConnection con = CDatabase.getConnection();

            SqlTransaction tran = null;
            SqlCommand cmdWAM1604;
            SqlCommand cmdDelWAM1604;

            try
            {
                con.Open();
                tran = con.BeginTransaction();
                if (this.lModuleId == 2) //MMM
                    cmdDelWAM1604 = new SqlCommand("UPDATE BS_RoleDetail SET bDel = (bDel + 1)  where lRoleId = " + this.lRoleId + " and lModuleId = " + this.lModuleId + " and cTyp = '" + ((Label)gvUserRights.Rows[1].FindControl("lblcTyp")).Text.Trim() + "'", con);
                else
                    cmdDelWAM1604 = new SqlCommand("UPDATE BS_RoleDetail SET bDel = (bDel + 1)  where lRoleId = " + this.lRoleId + " and lModuleId = " + this.lModuleId, con);
                cmdDelWAM1604.Transaction = tran;

                cmdDelWAM1604.ExecuteNonQuery();

                foreach (GridViewRow row in gvUserRights.Rows)
                {
                    if (((CheckBox)row.FindControl("chkAll")).Checked == true)
                    {
                        cmdWAM1604 = new SqlCommand("INS_UPD_RoleDetail", con);

                        cmdWAM1604.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdWAM1604.Transaction = tran;

                        cmdWAM1604.Parameters.Add(new SqlParameter("@lRoleId", System.Data.SqlDbType.Int));
                        cmdWAM1604.Parameters["@lRoleId"].Value = this.lRoleId;

                        //cmdWAM1604.Parameters.Add(new SqlParameter("@lCompId", System.Data.SqlDbType.Int));
                        //cmdWAM1604.Parameters["@lCompId"].Value = this.lCompId;

                        cmdWAM1604.Parameters.Add(new SqlParameter("@lModuleId", System.Data.SqlDbType.Int));
                        cmdWAM1604.Parameters["@lModuleId"].Value = this.lModuleId;

                        cmdWAM1604.Parameters.Add(new SqlParameter("@cTyp", System.Data.SqlDbType.Char, 4));
                        cmdWAM1604.Parameters["@cTyp"].Value = ((Label)row.FindControl("lblcTyp")).Text;//row.Cells[10].Text.Trim(); //from Grid

                        if (((Label)row.FindControl("lblcTyp")).Text == "R")
                        {
                            cmdWAM1604.Parameters.Add(new SqlParameter("@lTaskId", System.Data.SqlDbType.Int));
                            cmdWAM1604.Parameters["@lTaskId"].Value = Int32.Parse(((Label)row.FindControl("lbllPrevId")).Text); //Int32.Parse(row.Cells[0].Text);//from Grid
                        }
                        else
                        {
                            cmdWAM1604.Parameters.Add(new SqlParameter("@lTaskId", System.Data.SqlDbType.Int));
                            cmdWAM1604.Parameters["@lTaskId"].Value = Int32.Parse(((Label)row.FindControl("lbllId")).Text); //Int32.Parse(row.Cells[0].Text);//from Grid 
                        }

                        //cmdWAM1604.Parameters.Add(new SqlParameter("@lTaskId", System.Data.SqlDbType.Int));
                        //cmdWAM1604.Parameters["@lTaskId"].Value = Int32.Parse(((Label)row.FindControl("lbllId")).Text); //Int32.Parse(row.Cells[0].Text);//from Grid

                        cmdWAM1604.Parameters.Add(new SqlParameter("@sTaskId", System.Data.SqlDbType.NVarChar, 255));
                        cmdWAM1604.Parameters["@sTaskId"].Value = ((Label)row.FindControl("lblsName")).Text.Trim(".-+ ".ToCharArray());//row.Cells[2].Text.Trim(".-+ ".ToCharArray());//from Grid

                        if (((Label)row.FindControl("lblcTyp")).Text == "R")
                        {
                            iTypId = Int32.Parse(((Label)row.FindControl("lbllId")).Text); //this.lTaskId; //not in use 
                        }
                        else
                        {
                            iTypId = 0;
                        }
                        cmdWAM1604.Parameters.Add(new SqlParameter("@lTypId", System.Data.SqlDbType.Int));
                        cmdWAM1604.Parameters["@lTypId"].Value = iTypId;//from Grid

                        this.lMode = 0;

                        cmdWAM1604.Parameters.Add(new SqlParameter("@lMode", System.Data.SqlDbType.Int));
                        cmdWAM1604.Parameters["@lMode"].Value = this.lMode; //Int32.Parse(iflag[i].ToString());//from Grid
                                                
                        cmdWAM1604.Parameters.Add(new SqlParameter("@lUserId", System.Data.SqlDbType.Int));
                        cmdWAM1604.Parameters["@lUserId"].Value = this.lUserId;

                        cmdWAM1604.Parameters.Add(new SqlParameter("@bDel", System.Data.SqlDbType.Int));
                        cmdWAM1604.Parameters["@bDel"].Value = this.bDel;
                                                
                        cmdWAM1604.Transaction = tran;

                        cmdWAM1604.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tran.Rollback();
                retval = 0;
            }
            finally
            {
                con.Close();
            }
            return retval;
        }//end of function

            //Return DataTable for QC
        public DataTable getDataTableForRole(int roleId, int compId, int moduleId)
        {
            CommonDBClass dbobj = new CommonDBClass();
            string sQuery = "";

            createDataTable();

            sQuery = "SELECT lId, PageLink, sName, lPrevId, lIndexId, cTyp, cGsGpFlag, " +
                    " CASE WHEN [0] IS NULL THEN 'false' WHEN [0] = 0 THEN 'true' END as [0], " +
                    " CASE WHEN [1] IS NULL THEN 'false' WHEN [1] = 1 THEN 'true' END as [1], " +
                    " CASE WHEN [2] IS NULL THEN 'false' WHEN [2] = 2 THEN 'true' END as [2], " +
                    " CASE WHEN [3] IS NULL THEN 'false' WHEN [3] = 3 THEN 'true' END as [3], " +
                    " CASE WHEN [4] IS NULL THEN 'false' WHEN [4] = 4 THEN 'true' END as [4] " +
                " FROM " +
                " ( " +
                    " SELECT DISTINCT BS_PageMS.PageId as lId, BS_PageMS.PageLink, BS_PageMS.PageName as sName, " +
                    " BS_PageMS.ParentPage as lPrevId, BS_PageMS.ChildNode as lIndexId, BS_PageMS.cTyp, " +
                    " BS_PageMS.cGsGpFlag, BS_RoleDetail.lMode, BS_RoleDetail.lRoleId, BS_RoleDetail.lTaskId " +
                    " FROM BS_PageMS left join BS_RoleDetail on BS_PageMS.PageId = BS_RoleDetail.lTaskId " +
                    " and BS_RoleDetail.lRoleId = "+roleId +" " +
                    " WHERE BS_PageMS.ModuleId = "+moduleId +" " +
                    " and BS_PageMS.ParentPage = 0 AND BS_PageMS.bDel=0" +
                 " ) p " +
                     " PIVOT " +
                     " ( SUM(lMode) For lMode in ([0],[1],[2],[3],[4])) " +
                     " as pvt ORDER BY lIndexId ;";


            if (sQuery != "")
            {
            
                buildDataTableRole(dbobj.GetDataTable(sQuery, "ForMenu"), 0, roleId, 0, moduleId);
            }
            else
            {
                dtgrdUserRights = null;
            }

            return dtgrdUserRights;
        }

        //Build Data table
        public void buildDataTableRole(DataTable tdt, int ilevel, int roleId, int compId, int moduleId)
        {
            string sQuery;
            string sEmpty = " ";
            //DataTable retdtTable;
            CommonDBClass dbobj;

            for (int i = 0; i < ilevel; i++)
            {
                sEmpty = "....";
            }

            foreach (DataRow row in tdt.Rows)
            {
                if (row["cGsGpFlag"].ToString() == "G")
                {
                    DataRow temprow = dtgrdUserRights.NewRow();
                    temprow["lId"] = Int32.Parse(row["lId"].ToString());

                    //temprow["sName"] = "".PadLeft(ilevel * 3) + " + " + row["sName"].ToString();
                    temprow["sName"] = sEmpty + " + " + row["sName"].ToString();
                    temprow["cGsGpFlag"] = row["cGsGpFlag"].ToString();

                    temprow["0"] = bool.Parse(row["0"].ToString());
                    temprow["1"] = bool.Parse(row["1"].ToString());
                    temprow["2"] = bool.Parse(row["2"].ToString());
                    temprow["3"] = bool.Parse(row["3"].ToString());
                    temprow["4"] = bool.Parse(row["4"].ToString());
                    temprow["lPrevId"] = Int32.Parse(row["lPrevId"].ToString());
                    temprow["lIndexId"] = Int32.Parse(row["lIndexId"].ToString());
                    temprow["cTyp"] = row["cTyp"].ToString();

                    dtgrdUserRights.Rows.Add(temprow);

                    sQuery = "select lId, PageLink, sName, lPrevId, lIndexId, cTyp, cGsGpFlag, " +
                    " CASE WHEN [0] IS NULL THEN 'false' WHEN [0] = 0 THEN 'true' END as [0], " +
                    " CASE WHEN [1] IS NULL THEN 'false' WHEN [1] = 1 THEN 'true' END as [1], " +
                    " CASE WHEN [2] IS NULL THEN 'false' WHEN [2] = 2 THEN 'true' END as [2], " +
                    " CASE WHEN [3] IS NULL THEN 'false' WHEN [3] = 3 THEN 'true' END as [3], " +
                    " CASE WHEN [4] IS NULL THEN 'false' WHEN [4] = 4 THEN 'true' END as [4] from " +
                    " ( select distinct BS_PageMS.PageId as lId, BS_PageMS.PageLink , BS_PageMS.PageName as sName, BS_PageMS.ParentPage as lPrevId, " +
                    " BS_PageMS.ChildNode as lIndexId, BS_PageMS.cTyp, BS_PageMS.cGsGpFlag, BS_RoleDetail.lMode, BS_RoleDetail.lRoleId, BS_RoleDetail.lTaskId " +
                    " from BS_PageMS left join BS_RoleDetail on BS_PageMS.PageId = BS_RoleDetail.lTaskId and BS_RoleDetail.lRoleId = " + roleId + " " +
                    " AND BS_RoleDetail.bDel = 0 where BS_PageMS.ModuleId = " + moduleId + " and BS_PageMS.ParentPage = " + row["lId"].ToString() + " AND BS_PageMS.bDel=0" +
                    " ) p " +
                    " PIVOT " +
                    " ( SUM(lMode) For lMode in ([0],[1],[2],[3],[4])) " +
                    " as pvt ORDER BY lIndexId ";


                    dbobj = new CommonDBClass();
                    buildDataTableRole(dbobj.GetDataTable(sQuery, "ForMenu"), (ilevel + 1), 1, 0,moduleId);
                }
                else
                {
                    DataRow temprow = dtgrdUserRights.NewRow();
                    temprow["lId"] = Int32.Parse(row["lId"].ToString());

                    //temprow["sName"] = "".PadLeft(ilevel * 3) + " + " + row["sName"].ToString();
                    temprow["sName"] = sEmpty + " -- " + row["sName"].ToString();
                    temprow["cGsGpFlag"] = row["cGsGpFlag"].ToString();

                    temprow["0"] = bool.Parse(row["0"].ToString());
                    temprow["1"] = bool.Parse(row["1"].ToString());
                    temprow["2"] = bool.Parse(row["2"].ToString());
                    temprow["3"] = bool.Parse(row["3"].ToString());
                    temprow["4"] = bool.Parse(row["4"].ToString());
                    temprow["lPrevId"] = Int32.Parse(row["lPrevId"].ToString());
                    temprow["lIndexId"] = Int32.Parse(row["lIndexId"].ToString());
                    temprow["cTyp"] = row["cTyp"].ToString();

                    dtgrdUserRights.Rows.Add(temprow);
                }
            }
        }//end of function


        public void createDataTable()
        {
            dtgrdUserRights.Columns.Add(new DataColumn("lId", System.Type.GetType("System.Int32")));
            dtgrdUserRights.Columns.Add(new DataColumn("sName", System.Type.GetType("System.String")));
            dtgrdUserRights.Columns.Add(new DataColumn("0", System.Type.GetType("System.Boolean")));
            dtgrdUserRights.Columns.Add(new DataColumn("1", System.Type.GetType("System.Boolean")));
            dtgrdUserRights.Columns.Add(new DataColumn("2", System.Type.GetType("System.Boolean")));
            dtgrdUserRights.Columns.Add(new DataColumn("3", System.Type.GetType("System.Boolean")));
            dtgrdUserRights.Columns.Add(new DataColumn("4", System.Type.GetType("System.Boolean")));
            dtgrdUserRights.Columns.Add(new DataColumn("lPrevId", System.Type.GetType("System.Int32")));
            dtgrdUserRights.Columns.Add(new DataColumn("lIndexId", System.Type.GetType("System.Int32")));
            dtgrdUserRights.Columns.Add(new DataColumn("cTyp", System.Type.GetType("System.String")));
            dtgrdUserRights.Columns.Add(new DataColumn("cGsGpFlag", System.Type.GetType("System.String")));
        }//end of function
            
}
