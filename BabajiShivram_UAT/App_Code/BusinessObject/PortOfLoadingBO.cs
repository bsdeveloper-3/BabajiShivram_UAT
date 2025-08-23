using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace BSImport.PortOfLoading.BO
{
    /// <summary>
    /// Summary description for PortOfLoadingBO
    /// </summary>
    public class PortOfLoadingBO
    {
        #region Private Variable

        private int portOfLoadingId = -1;
        private string name = String.Empty;
        private string code = String.Empty;
        private string city = String.Empty;
        private int mode;

        #endregion

        public int PortOfLoadingId
        {
            get
            {
                return portOfLoadingId;
            }
            set
            {
                portOfLoadingId = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        
        public int Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

    }
}
