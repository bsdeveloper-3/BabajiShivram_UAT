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
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using BSImport;

/// <summary>
/// Summary description for InventoryOperation
/// </summary>
public class InventoryOperation
{
	public InventoryOperation()
	{
		//
		// TODO: Add constructor logic here
		//
	}


     #region Inventory

    public static int AddImsCategory(string imsCategoryName, int userID)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = imsCategoryName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = userID;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("IMS_insCategoryItem", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddImsDispached(string EmployeeName, int Quantity, int BranchID, int DepartmentId,
            int Item, DateTime DispatchDate, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = EmployeeName;
        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = Item;
        command.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
        command.Parameters.Add("@DeptId", SqlDbType.Int).Value = DepartmentId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchID;
        command.Parameters.Add("@DispatchDate", SqlDbType.DateTime).Value = DispatchDate;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("IMS_insDispatchItem", command, "@OutPut");
        return Convert.ToInt32(SPresult);

    }

    public static int AddItemDetail(int Category, string itemtype, int userid)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@TypeId", SqlDbType.Int).Value = Category;
        command.Parameters.Add("@sName", SqlDbType.NVarChar).Value = itemtype;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = userid;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("IMS_insItemDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static void FillIMSItemTypeMS(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "select lid,sName from IMS_ItemTypeMS where bdel=0 order by sName ", "sName", "lid");

    }
    
    public static void FillBranchIMS(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,BranchName FROM BS_BranchMS order by BranchName", "BranchName", "lId");
    }

    public static void FillDeptNameIms(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,DeptName FROM BS_DeptMS WHERE bDel = 0 ORDER BY DeptName", "DeptName", "lId");
    }


    public static int UpdateIMSQuantity(int itemid, int NewQuantity, string VendorName,string BillNo, DateTime  billdate,string 
        billamount, int UserId) 
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@ItemID", SqlDbType.Int).Value = itemid;
       // command.Parameters.Add("@Categoryid", SqlDbType.Int).Value = categoryId;
        command.Parameters.Add("@newQuantity", SqlDbType.BigInt).Value = NewQuantity;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@BillNO", SqlDbType.NVarChar).Value = BillNo;
        command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billdate;
        command.Parameters.Add("@BillAmount", SqlDbType.NVarChar).Value = billamount;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("IMS_updItemDetails", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static void FillItemsIms(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lid,sName FROM IMS_ItemDetails WHERE bDel = 0 ORDER BY sName", "sName", "lId");
    }

    public static void FillItemByCategoryId(DropDownList DropDown, int CategoryId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;
        CDatabase.BindControls(DropDown, "IMS_GetItemByCategoryID", command, "ItemName","ItemID");
    }

    public static DataSet GetItemByCategoryId(int CategoryId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = CategoryId;

        return CDatabase.GetDataSet("IMS_GetItemByCategoryID", command);

    }

    public static DataSet GetItemById(int ItemID)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemID;

        return CDatabase.GetDataSet("IMS_GetItemById", command);
    }
  
    #endregion
}


