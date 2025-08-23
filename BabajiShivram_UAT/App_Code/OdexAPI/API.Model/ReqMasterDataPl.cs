using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdexConnect
{
    /// <summary>
    /// ODeX Master Data API Request
    /// </summary>
    public class ReqMasterDataPl
    {
        /// <summary>
        /// Optional - BENEFICIARY, LOCATION(blank for all master data / Max 20 Char
        /// </summary>
        public string masterTp { get; set; }

        /// <summary>
        /// Mandatatory - Customer code provided by ODeX / Max 50 Char
        /// </summary>
        public string pyrCode { get; set; }
    }
    
}
