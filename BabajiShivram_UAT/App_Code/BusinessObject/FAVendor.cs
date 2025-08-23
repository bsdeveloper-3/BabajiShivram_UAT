using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FAVendor
/// </summary>
namespace BSImport.VendorManager.BO
{
    public class FAVendor
    {
        #region Private Varibales

        private int vendorId = -1;
        private string name = String.Empty;
        private string code = String.Empty;
        private string state = String.Empty;
        private string gstin = String.Empty;
        private string pan = String.Empty;
        private string creditDays = String.Empty;

        #endregion
        public int VendorId
        {
            get { return vendorId; }
            set { vendorId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        public string GSTIN
        {
            get { return gstin; }
            set { gstin = value; }
        }
        public string PANNo
        {
            get { return pan; }
            set { pan = value; }
        }
        public string CreditDays
        {
            get { return creditDays; }
            set { creditDays = value; }
        }
    }
}