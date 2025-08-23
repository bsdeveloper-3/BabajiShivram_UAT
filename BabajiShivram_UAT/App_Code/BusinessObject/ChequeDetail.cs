using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChequeDetail
/// </summary>
namespace BSImport.ChequeManager.BO
{
    public class ChequeDetail
    {
        #region Privare Variables

        private int _ChequeId = -1;
        private string _ChequeNo = String.Empty;
        private string _BankName = String.Empty;
        private string _AccountName = String.Empty;

        #endregion

        public int ChequeId
        {
            get { return _ChequeId; }

            set { _ChequeId = value; }
        }

        public string ChequeNo
        {
            get { return _ChequeNo; }

            set { _ChequeNo = value; }
        }

        public string BankName
        {
            get { return _BankName; }

            set { _BankName = value; }
        }
        public string AccountName
        {
            get { return _AccountName; }

            set { _AccountName = value; }
        }
    }
}