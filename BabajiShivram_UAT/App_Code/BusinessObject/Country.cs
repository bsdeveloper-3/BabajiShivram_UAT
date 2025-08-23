using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.CountryManager.BO
{
    /// <summary>
    /// Summary description for Country
    /// </summary>
    public class Country
    {
        #region Privare Variables

        private int _countryId = -1;
        private string _countryName = String.Empty;

        #endregion

        public int CountryId
        {
            get { return _countryId; }

            set { _countryId = value; }
        }

        public string CountryName
        {
            get { return _countryName; }

            set { _countryName = value; }
        }
    }
}