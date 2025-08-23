using System;
using System.ComponentModel;
using System.Transactions;

using BSImport.BranchManager.BO;
using BSImport.BranchManager.DAL;
using System.Data;

namespace BSImport.BranchManager.BLL
{
    /// <summary>
    /// Summary description for BranchManager
    /// </summary>
    public class BranchManager
    {
        public static DataTable dt;

        public static DataTable dtselectedbranch;
        public static DataTable getselectedbranchtable
        {
            get
            {
                return dtselectedbranch;
            }
            set
            {
                dtselectedbranch = value;
            }


        }
        public static DataTable getcloneselectedbranchtable
        {
            get
            {
                return dtselectedbranch;
            }
            set
            {
                dtselectedbranch = value;
            }


        }
        public static DataTable getselectedpopuptable
        {
            get
            {
                return dtselectedbranch;
            }
            set
            {
                dtselectedbranch = value;
            }


        }

        public static DataTable getbranchtable
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
            }


        }
        #region Public Methods
        public static BranchList GetList()
        {
            return BranchDB.GetList();
        }

        public string getmaxbranchlid()
        {
            BranchDB objdb = new BranchDB();
            return objdb.getmaxbranchlid();
        }
        public DataTable getallbranch()
        {
            BranchDB objdb = new BranchDB();
            return objdb.getallbranch();
        }

        public string InsertBranch(string branchname, string branchcode, string city, string address, string contactno)
        {
            BranchDB objdb = new BranchDB();
            return objdb.inserbranch(branchname, branchcode, city, address, contactno);

        }

        //public string insertbranchport(string branchid,string portid)
        //{

        //    BranchDB objdb = new BranchDB();
        //    return objdb.insertbranchport(branchid, portid);
           

        //}
        //public string updatebranchport(string nflid, string lid, string branchid, string portid)
        //{

        //    BranchDB objdb = new BranchDB();
        //   return  objdb.updatebranchport(nflid, lid, branchid, portid);

        //}
        public DataTable getbranchport(string branchid)
        {
            BranchDB objdb = new BranchDB();
           return objdb.getbranchport(branchid);
        }


        public string InsertUserBranch(string uid, string branchid, string remark)
        {
            BranchDB objdb = new BranchDB();
            return objdb.insertuserbranch(uid, branchid, remark);
        }

        public DataTable getbranchbyuserid(string userid)
        {
            BranchDB objdb = new BranchDB();
            return objdb.getbranchbyuserid(userid);
        }
        public string updateuserbranch(string branchid, string uid,string lid,string nf)
        {
            BranchDB objdb = new BranchDB();
            return objdb.updateuserbranch(uid, branchid,lid,nf);


        }
        public DataTable getbranch(string mode,string uid)
        {
            BranchDB objdb = new BranchDB();
            return objdb.getbranch(mode,uid);

        }

        public DataTable getbranchbybranchid(string branchid)
        {
            BranchDB objdb = new BranchDB();
            return objdb.getbranchbybranchid(branchid);
        }

        public string updatebranch(string lid, string branchname, string branchcode, string city, string address, string contactno)
        {
             BranchDB objdb = new BranchDB();
             return objdb.updatebranch(lid, branchname, branchcode, city, address, contactno);
        }

        public string deletebranch(string lid)
        {
            BranchDB objdb = new BranchDB();
            return objdb.deletebranch(lid);
        }

       
        #endregion
    }
}
