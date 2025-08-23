using System;
using System.ComponentModel;
using System.Transactions;
using BSImport.ClientManager.BO;
using BSImport.ClientManager.DAL;
namespace BSImport.ClientManager.BLL
{
    /// <summary>
    /// Summary description for ClientManager
    /// </summary>
    public class ClientManager
    {
        #region Public Methods

        public static ClientList GetList()
        {
            return ClientDB.GetList();

        }

        #endregion
    }
}