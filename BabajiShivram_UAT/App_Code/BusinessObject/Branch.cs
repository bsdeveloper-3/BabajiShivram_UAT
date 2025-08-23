using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.BranchManager.BO
{

    /// <summary>
    /// Summary description for Branch
    /// </summary>
    public class Branch
    {
        #region Private Varibales

        private int branchId = -1;
        private string branchName = String.Empty;
        private string branchHead = String.Empty;
        private string contactNo = String.Empty;

        #endregion
        public int BranchId
        {
            get { return branchId; }
            set { branchId = value; }
        }

        public string BranchName
        {
            get { return branchName ; }
            set { branchName = value; }
        }
        
        public string BranchHead
        {
            get { return branchHead; }
            set { branchHead = value; }
        }
        public string ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }
        
    }
}
