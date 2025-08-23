using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
/// <summary>
/// Class to generate tree menu
/// </summary>
public class TreeMenuClass
{

    public TreeMenuClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //Modified by amit

    public static string GetCurrentPageName()
    {
        string[] urlSegments = System.Web.HttpContext.Current.Request.Url.Segments;
        string strPath = "";
        /*	string strPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
		System.IO.FileInfo oInfo = new System.IO.FileInfo(strPath);
		string strRet = oInfo.Name;
		return strRet;
        */
        int TotalSeagments = (urlSegments.Length - 1);
        if(TotalSeagments == 3)
            strPath = urlSegments[TotalSeagments -1 ].ToString() + urlSegments[TotalSeagments].ToString();
        else
            strPath = urlSegments[TotalSeagments].ToString();

        return strPath;
    }

    /// <summary>
    /// Get Parent Menu Nodes
    /// </summary>
    /// <param name="MenuID"></param>
    /// <returns>Menu List Item</returns>
    public static MnuItemList GetParentItem(string MenuID)
    {
        MnuItemList mlist = new MnuItemList();
        StringBuilder strSQL = new StringBuilder("");
        strSQL.Append("SELECT PageId,PageName,PageLink,MenuNode FROM WAM1601 where ParentPage=0 and ChildNode=0 and ModuleID=" + MenuID.ToString() + " ORDER BY NodeSequence");
        SqlDataReader dr = CDatabase.GetDataReader(strSQL.ToString());

        if (dr.HasRows == true)
        {
            while (dr.Read() == true)
                mlist.Add(new MnuItem(dr["PageId"].ToString(), dr["PageName"].ToString(), dr["PageLink"].ToString()));
        }

        return mlist;
    }

    /// <summary>
    /// Get Parent Menu Nodes
    /// </summary>
    /// <returns>Menu List Item</returns>
    public static MnuItemList GetParentItem()
    {
        MnuItemList mlist = new MnuItemList();
        StringBuilder strSQL = new StringBuilder("");
        strSQL.Append("SELECT PageId,PageName,PageLink,MenuNode FROM WAM1601 where ParentPage=0 and ChildNode=0 ORDER BY NodeSequence");
        SqlDataReader dr = CDatabase.GetDataReader(strSQL.ToString());

        if (dr.HasRows == true)
        {
            while (dr.Read() == true)
                mlist.Add(new MnuItem(dr["PageId"].ToString(), dr["PageName"].ToString(), dr["PageLink"].ToString()));
        }
        dr.Close();

        return mlist;
    }

    /// <summary>
    /// Get childmenu items
    /// </summary>
    /// <param name="menuid">id of menu for which child menu require</param>
    /// <returns>sub menu item list</returns>
    public static SubMnuItemList GetsubMenuItem(string menuid, int AccId, int MnuId)
    {

        SubMnuItemList submnulist = new SubMnuItemList();
                
        SqlCommand command = new SqlCommand();
        SqlConnection connection = CDatabase.getConnection();
        SqlDataReader dr = null;
        command.Parameters.Add("@ParentPage", SqlDbType.VarChar, 100).Value = menuid;
        command.Parameters.Add("@AccessId", SqlDbType.Int).Value = AccId;
        command.Parameters.Add("@MdlId", SqlDbType.Int).Value = MnuId;
        try
        {

            command.CommandText = "pr_GetChildNodes";
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();
            dr = command.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read() == true)
                {
                    submnulist.Add(new SubMnuItem(dr["PageId"].ToString(), dr["PageName"].ToString(), dr["PageLink"].ToString()));
                    //GetsubMenuItem(dr["PageId"].ToString(), AccId, MnuId);
                }
            }
            dr.Close();
            connection.Close();
            command.Dispose();
            connection.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (!dr.IsClosed) dr.Close();
            if (connection.State == ConnectionState.Open) connection.Close();
        }
        return submnulist;
    }
        
    public static MnuItemList GetParentItem(int ModuleId, int RoleId, int UserType)
    {
        MnuItemList mlist = new MnuItemList();
        StringBuilder strSQL = new StringBuilder("");

        if (UserType == -1) 
        {
            strSQL.Append("SELECT PageId,PageName,PageLink FROM BS_PageMS WHERE ParentPage=0 AND ChildNode=0 ");
            strSQL.Append(" AND ModuleId = " + ModuleId + " AND bDel=0 ORDER BY NodeSequence ");
        }
        else if(UserType == 1 || UserType == 3) 
        {
            strSQL.Append("SELECT PageId,PageName,PageLink FROM BS_PageMS WHERE ParentPage=0 AND ChildNode=0 ");
            strSQL.Append(" AND ModuleId = " + ModuleId + " AND bDel=0 ORDER BY NodeSequence ");

            //strSQL.Append(" SELECT DISTINCT B.PageId, B.PageName, B.PageLink, B.NodeSequence");
            //strSQL.Append(" FROM BS_RoleDetail AS A INNER JOIN BS_PageMS AS B ON  A.lTaskId = B.PageId");
            //strSQL.Append(" WHERE B.ParentPage=0 AND B.ChildNode=0  AND B.bDel =0 AND A.bDel=0 AND A.lRoleId=" + RoleId);
            //strSQL.Append(" AND A.lModuleId=" + ModuleId + " ORDER BY NodeSequence");
        }

        SqlDataReader dr = CDatabase.GetDataReader(strSQL.ToString());
        
        if (dr.HasRows == true)
        {
            while (dr.Read() == true)
                mlist.Add(new MnuItem(dr["PageId"].ToString(), dr["PageName"].ToString(), dr["PageLink"].ToString()));
        }
        
        dr.Close();

        return mlist;
    }

    
    public static SubMnuItemList GetsubMenuItem(string ParentPageId, int ModuleId, int RoleId, int UserType)
    {

        SubMnuItemList submnulist = new SubMnuItemList();
        
        StringBuilder strSQL = new StringBuilder("");

        //If System user
        if (UserType == -1)
        {
            strSQL.Append(" SELECT B.PageId, B.PageName, B.PageLink, B.ChildNode");
            strSQL.Append(" FROM BS_PageMS AS B WHERE B.bdel=0 AND B.ParentPage=" + ParentPageId + " ORDER BY B.ChildNode");
        }
        else if(UserType == 1 || UserType == 3)
        {
            strSQL.Append(" SELECT B.PageId, B.PageName, B.PageLink, B.ChildNode");
            strSQL.Append(" FROM BS_RoleDetail AS A INNER JOIN BS_PageMS AS B ON  A.lTaskID=B.PageID");
            strSQL.Append(" WHERE A.bDel=0 AND B.bDel=0 AND A.lRoleId=" + RoleId + " AND B.ParentPage=" + ParentPageId);
            strSQL.Append(" AND A.lModuleId="+ModuleId +" ORDER BY B.ChildNode");

	  //  strSQL.Append(" SELECT DISTINCT A.lMode,B.PageId, B.PageName, B.PageLink, B.ChildNode");
          //  strSQL.Append(" FROM BS_RoleDetail AS A INNER JOIN BS_PageMS AS B ON  A.lModuleId = B.ModuleID AND A.lTaskID=B.PageID");
          //  strSQL.Append(" WHERE A.bDel=0 AND B.bDel=0 AND A.lRoleId=" + RoleId + " AND B.ParentPage=" + ParentPageId);
          //  strSQL.Append(" AND A.lModuleId="+ModuleId +" ORDER BY B.ChildNode");
        }

        SqlCommand command = new SqlCommand();
        SqlDataReader dr = CDatabase.GetDataReader(strSQL.ToString());
        
        if (dr.HasRows == true)
        {
            // for System User Bind All Pages in TreeView    
            if (UserType == -1)
            {
                while (dr.Read() == true)
                {
                    submnulist.Add(new SubMnuItem(dr["PageId"].ToString(), dr["PageName"].ToString(), dr["PageLink"].ToString()));
                }
            }

              // for Other user Bind Pages according to Role
            else
            {
                while (dr.Read() == true)
                {
                    submnulist.Add(new SubMnuItem(dr["PageId"].ToString(), dr["PageName"].ToString(), dr["PageLink"].ToString())); //+ "?ID=" + dr["lMode"].ToString()));
                }
            }
        }
        dr.Close();
        return submnulist;
    }
}

public class MnuItem
{

    public String Id;
    public String Name;
    public String PLink;

    public MnuItem(String id, String name, String plink)
    {
        this.Id = id;
        this.Name = name;
        this.PLink = plink;
    }
}

public class MnuItemList : ArrayList
{

    public new MnuItem this[int i]
    {
        get
        {
            return (MnuItem)base[i];
        }
        set
        {
            base[i] = value;
        }
    }
}

public class SubMnuItem
{

    public String Id;
    public String Name;
    public String PLink;

    public SubMnuItem(String id, String name, String plink)
    {
        this.Id = id;
        this.Name = name;
        this.PLink = plink;
    }
}

public class SubMnuItemList : ArrayList
{

    public new SubMnuItem this[int i]
    {
        get
        {
            return (SubMnuItem)base[i];
        }
        set
        {
            base[i] = value;
        }
    }
}
