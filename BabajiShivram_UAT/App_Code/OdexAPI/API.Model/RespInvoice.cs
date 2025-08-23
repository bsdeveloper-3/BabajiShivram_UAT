using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdexConnect.API.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class InvAttList
    {
        public string attTitile { get; set; }
        public byte[] attData { get; set; }
        public string attNm { get; set; }
    }

    public class InvoiceResponse
    {
        public string bnfCode { get; set; }
        public string locationCode { get; set; }
        public string bookingLineCode { get; set; }
        public string typOfCargo { get; set; }
        public string idTp { get; set; }
        public string blNo { get; set; }
        public string invNo { get; set; }
        public string totalPymtAmt { get; set; }
        public string invCategory { get; set; }
        public string invTp { get; set; }
        public string invDt { get; set; }
        public string billToParty { get; set; }
        public string gstNo { get; set; }
        public List<InvAttList> invAttList { get; set; }
        public string partialPymt { get; set; }
        public string isDORevalidation { get; set; }
        public string doRevalidationCharges { get; set; }
        public string isTdsDeduction { get; set; }
        public List<TdsList> tdsList { get; set; }
        public string jobNo { get; set; }
    }

    public class TdsList
    {
        public string TDS { get; set; }
    }
}