using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.ServicesManager.BO
{
    /// <summary>
    /// Summary description for FAQServices
    /// </summary>

    public class FAQServices
    {
        #region Privare Variables

        private int _ServiceId = -1;
        private string _ServiceName = String.Empty;

        #endregion

        public int ServiceId
        {
            get { return _ServiceId; }

            set { _ServiceId = value; }
        }

        public string ServiceName
        {
            get { return _ServiceName; }

            set { _ServiceName = value; }
        }
    }
}
    