using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdexConnect.API.Model
{
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AttDtl
    {
        public string attTitile { get; set; }
        public string attNm { get; set; }
        public string attData { get; set; }
        public string remarks { get; set; }
    }

    public class BlDtls
    {
        //   public string odexRefNo { get; set; }
        public string pyrCode { get; set; }
      //  public string tpOfService { get; set; }
        public string bnfCode { get; set; }
        public string bookingLineCode { get; set; }
        public string blNo { get; set; }
        public string isMblReq { get; set; }
        public string mblNo { get; set; }
        public string locationCode { get; set; }
        public string invCategory { get; set; }
        public string typOfCargo { get; set; }
        public string deliveryTp { get; set; }
        public string freeDays { get; set; }
        public string validTill { get; set; }
        public string doExtFlg { get; set; }
        public string highSeaSales { get; set; }
        public string isSeawayBL { get; set; }
        public string isMblHbl { get; set; }
        public string isAddChrgReq { get; set; }
        public int hblCount { get; set; }
        public string storageChargeDays { get; set; }
        public string latePymtChrg { get; set; }
        public string jobNo { get; set; }
        public string chaNm { get; set; }
        public string remarks { get; set; }
        public string haz { get; set; }
        public string odc { get; set; }
        public string dischargeDt { get; set; }
        public int noOfFreeDays { get; set; }
        public string consigneeNm { get; set; }
        public string gstNo { get; set; }
        public string contactNo { get; set; }
        public string emailId { get; set; }
        public string address { get; set; }
        public string stateCd { get; set; }
    }

    public class CntnrDtl
    {
        public string cntNo { get; set; }
        public string cntSize { get; set; }
        public string validTill { get; set; }
    }

    public class ReqInvoicePl
    {
        public BlDtls blDtls { get; set; }
        public List<AttDtl> attDtls { get; set; }
        public List<CntnrDtl> cntnrDtls { get; set; }
    }


}
