using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.ConsigneeManager.BO
{
    /// <summary>
    /// Summary description for Consignee
    /// </summary>
    public class Consignee
    {
        #region Privare Variables

        private int _consigneeId  = -1;
        private string _consigneeName = String.Empty;
        private string _consigneeEmail = String.Empty;
        private string _consigneeAddress = String.Empty;
        private string _consigneeContactNo = String.Empty;

        #endregion

        public int ConsigneeId
        {
            get { return _consigneeId;}

            set {_consigneeId = value;}
        }

        public string ConsigneeName
        {
            get { return _consigneeName; }

            set { _consigneeName = value; }
        }

        public string ConsigneeEmail
        {
            get { return _consigneeEmail; }

            set { _consigneeEmail = value; }
        }

        public string ConsigneeAddress
        {
            get { return _consigneeAddress; }

            set { _consigneeAddress = value; }
        }

        public string ConsigneeContactNo
        {
            get { return _consigneeContactNo; }

            set { _consigneeContactNo = value; }
        }

    }
}
