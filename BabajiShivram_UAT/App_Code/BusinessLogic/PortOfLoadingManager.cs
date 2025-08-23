using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BSImport.PortOfLoading.BO;
using BSImport.PortOfLoading.DAL;
namespace BSImport.PortOfLoading.BLL
{
    /// <summary>
    /// Summary description for PortOfLoadingManager
    /// </summary>
    public class PortOfLoadingManager
    {
        #region Public Methods

        public static PortOfLoadingList GetList()
        {
            return PortOfLoadingDB.GetList();
        }

        #endregion
    }
}
