using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for AccountCode
/// </summary>

namespace BSImport.CountryManager.BO
{
    public class AccountCode
    {
        #region Privare Variables

        private int _AcCodeId = -1;
        private string _AcCodeName = String.Empty;
        private string _AccountName = String.Empty;

        #endregion

        public int AcCodeId
        {
            get { return _AcCodeId; }

            set { _AcCodeId = value; }
        }

        public string AcCodeName
        {
            get { return _AcCodeName; }

            set { _AcCodeName = value; }
        }

        public string AccountName
        {
            get { return _AccountName; }

            set { _AccountName = value; }
        }
    }
}