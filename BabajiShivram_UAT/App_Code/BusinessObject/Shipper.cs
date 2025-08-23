using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.ShipperManager.BO
{
    /// <summary>
    /// Summary description for Shipper
    /// </summary>
    public class Shipper
    {
        #region Privare Variables

        private int _shipperId = -1;
        private string _shipperName = String.Empty;

        #endregion

        public int ShipperId
        {
            get { return _shipperId; }

            set { _shipperId = value; }
        }

        public string ShipperName
        {
            get { return _shipperName; }

            set { _shipperName = value; }
        }
    }
}