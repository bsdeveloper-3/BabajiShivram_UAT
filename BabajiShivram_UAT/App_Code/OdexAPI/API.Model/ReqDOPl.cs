using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
namespace OdexConnect.API.Model
{

    public class ReqDOPl
    {
        public DoReqDtls doReqDtls { get; set; }
        public List<AttDOReqDtl> attDOReqDtls { get; set; }
    }
    
    public class DoReqDtls
    {
        public string pyrCode { get; set; }
        public string locationCode { get; set; }
        public string bnfCode { get; set; }
        public string blNo { get; set; }
        public string cargoTp { get; set; }
        public string stuffTp { get; set; }
        public string isSeawayBL { get; set; }
        public string freeDays { get; set; }
        public string validTill { get; set; }
        public string doExtFlg { get; set; }
        public string odexPymt { get; set; }
        public string advncBlSubmit { get; set; }
        public string runnerBoy { get; set; }
        public string remarks { get; set; }
        public string mobNo { get; set; }
        public string hssCustomerNm { get; set; }
        public string factoryAddr { get; set; }
        public string factoryPin { get; set; }
    }

    public class AttDOReqDtl
    {
        public string attTitile { get; set; }
        public string attNm { get; set; }
        public string attData { get; set; }
    }



}
