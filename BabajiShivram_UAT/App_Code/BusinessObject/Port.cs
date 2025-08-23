using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace BSImport.PortManager.BO
{
    /// <summary>
    /// Summary description for Port
    /// </summary>
    public class Port
    {
        #region Private Variable

        private int portId = -1;
        private string name = String.Empty;
        private string code = String.Empty;
        private string city = String.Empty;
        private string state = String.Empty;
        private string country = String.Empty;
        private int mode;
        
        #endregion

        public int PortId
        {
            get
            {
                return portId;
            }
            set 
            {
                portId = value;
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
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
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
