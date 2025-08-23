using System;
using System.ComponentModel;
using System.Transactions;
using BSImport.PortManager.BO;
using BSImport.PortManager.DAL;
using System.Data;
namespace BSImport.PortManager.BLL
{
    /// <summary>
    /// Summary description for PortManager
    /// </summary>
    /// 
   
    public class PortManager
    {
       static DataTable dt;
        public static DataTable getselectedport
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
            }
        }
        #region Public Methods
        
        public static PortList GetList()
        {
            return PortDB.GetList();
        }


        public DataTable getallPort()
        {
            PortDB objportdb = new PortDB();
           return  objportdb.getallPort();
        }

       
        #endregion
    }
}
