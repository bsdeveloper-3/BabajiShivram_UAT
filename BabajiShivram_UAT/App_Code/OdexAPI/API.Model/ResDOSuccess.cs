using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace OdexConnect
{
    /// <summary>
    /// Summary description for ResDOSuccess
    /// </summary>
    public class ResDOSuccess
    {
        /// <summary>
        /// Success, Fail
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// BL Number
        /// </summary>
        public string blNo { get; set; }
        /// <summary>
        /// Doc Confirmation Status -- Requested. Rejected, Confirmed
        /// </summary>
        public string doReqStatus { get; set; }
        /// <summary>
        /// In case of fail
        /// </summary>
        public string remarks { get; set; }


    }
}