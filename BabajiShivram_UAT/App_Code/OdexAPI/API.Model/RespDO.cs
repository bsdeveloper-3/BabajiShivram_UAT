using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
public class RespDO
{
 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DoAttList
    {
        public string attTitile { get; set; }
        public byte[] attData { get; set; }
        public string attNm { get; set; }
    }

    public class DoDtl
    {
        public string doReqStatus { get; set; }
        public List<DoAttList> doAttList { get; set; }
    }

    public class DOResponse
    {
        public string bnfCode { get; set; }
        public List<DoDtl> doDtls { get; set; }
        public string blNo { get; set; }
        public string locationCode { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }
    }


}