using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for JobDetail
/// </summary>
namespace BSImport.CountryManager.BO
{

    public class JobDetail
    {
        #region Privare Variables

        private int _JobId = -1;
        private string _JobName = String.Empty;
        private int _ModuleId = 0;

        #endregion

        public int JobId
        {
            get { return _JobId; }

            set { _JobId = value; }
        }

        public string JobName
        {
            get { return _JobName; }

            set { _JobName = value; }
        }

        public int ModuleId
        {
            get { return _ModuleId; }

            set { _ModuleId = value; }
        }
    }
}