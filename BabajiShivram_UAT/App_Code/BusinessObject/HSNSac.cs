using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.ConsigneeManager.BO
{
    /// <summary>
    /// Summary description for HSNSac
    /// </summary>

    public class HSNSac
    {
        #region Privare Variables

        private int _SacId = -1;
        private string _SacNo = String.Empty;

        #endregion

        public int SacId
        {
            get { return _SacId; }

            set { _SacId = value; }
        }

        public string SacNo
        {
            get { return _SacNo; }

            set { _SacNo = value; }
        }

        //public HSNSac()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}
    }
}