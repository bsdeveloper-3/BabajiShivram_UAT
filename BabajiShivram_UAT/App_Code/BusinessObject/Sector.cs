using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.SectorManager.BO
{
    public class Sector
    {
        #region Privare Variables

        private int _variantId = -1;
        private string _variantName = String.Empty;

        #endregion

        public int VariantId
        {
            get { return _variantId; }

            set { _variantId = value; }
        }

        public string VariantName
        {
            get { return _variantName; }

            set { _variantName = value; }
        }
    }
}
