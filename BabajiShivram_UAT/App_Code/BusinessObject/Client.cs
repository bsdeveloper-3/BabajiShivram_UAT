using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;


namespace BSImport.ClientManager.BO
{
    /// <summary>
    /// Summary description for Client
    /// </summary>
    public class Client
    {
        #region Private Variable

        private int clientId = -1;
        private string clientName = String.Empty;
        private string address = String.Empty;
        private string contactPerson = String.Empty;
        private string mobileNo = String.Empty;
        
        #endregion

        public int ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }

        public string ClientName
        {
            get 
            {
                return clientName;
                 
            }
            set 
            {
                clientName = value;
            }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string ContactPerson
        {
            get { return contactPerson; }
            set { contactPerson = value; }
        }
        public string MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
    }
}
