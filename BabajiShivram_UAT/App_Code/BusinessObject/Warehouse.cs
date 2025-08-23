using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.WarehouseManager.BO
{
/// <summary>
/// Summary description for Warehouse
/// </summary>
    public class Warehouse
    {
	
	    #region Privare Variables

        private int _warehouseId = -1;
        private string _warehouseName = String.Empty;

        #endregion

        public int WarehouseId
        {
            get { return _warehouseId; }

            set { _warehouseId = value; }
        }

        public string WarehouseName
        {
            get { return _warehouseName; }

            set { _warehouseName = value; }
        }
	
    }
}