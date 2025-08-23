using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace OdexConnect
{
    /// <summary>
    /// Get New Invoice Request Response
    /// {For Success : "odexRefNo":"BL2023062100010","invoiceStatus":"Requested","remarks":"","status":"SUCCESS"}
    /// {For Failed: "remarks": "BL No. does not exist"}
    /// </summary>
    public class ResInvoiceSuccess
    {
        public string odexRefNo { get; set; }
        public string invoiceStatus { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }
    }
}