using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
namespace BSImport.UserManager.BO
{
    public class User
    {
        #region Private Variables



        #endregion


        private string userid;
        private string username;
        private int deptid;
        private int designid;
        private string divid;
        private int branchid;
        private string hod;
        private string empcode;
        private string email;
        private string contactno;
        private bool bdel;

        public string Userid
        {
            get
            {

                return userid;
            }
            set
            {
                userid = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        public int Deptid
        {
            get
            {
                return deptid;
            }
            set
            {
                deptid = value;
            }
        }

        public int Designid
        {
            get
            {
                return designid;
            }
            set
            {

                designid = value;
            }
        }

        public string Divid
        {
            get
            {
                return divid;
            }
            set
            {
                divid = value;
            }
        }

        public int Branchid
        {
            get
            {
                return branchid;
            }
            set
            {
                branchid = value;
            }
        }

        public string Hod
        {
            get
            {
                return hod;
            }
            set
            {
                hod = value;
            }
        }

        public string Empcode
        {
            get
            {
               return empcode;
            }
            set
            {
                empcode = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        public string Contactno
        {
            get
            {
                return contactno;
            }
            set
            {
                contactno = value;
            }
        }

        public bool Bdel
        {
            get
            {
                return bdel;
            }

            set
            {
                bdel = value;
            }

            
        }
    }
}
