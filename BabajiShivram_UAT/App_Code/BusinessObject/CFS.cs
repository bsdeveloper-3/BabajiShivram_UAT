using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.CFSManager.BO
{
    /// <summary>
    /// Summary description for CFS
    /// </summary>
    public class CFS
    {
        #region Privare Variables

        private int _cfsId = -1;
        private string _cfsName = String.Empty;

        #endregion

        public int CFSId
        {
            get { return _cfsId; }

            set { _cfsId = value; }
        }

        public string CFSName
        {
            get { return _cfsName; }

            set { _cfsName = value; }
        }
    }
}