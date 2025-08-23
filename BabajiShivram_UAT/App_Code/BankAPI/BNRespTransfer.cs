using System.Collections.Generic;
namespace BankAPI.YesBank
{
    public class BNRespStartTransfer
    {
        public string attemptNo { get; set; }
        public string reqTransferType { get; set; }
        public string requestReferenceNo { get; set; }
        public string statusCode { get; set; }
        public string subStatusCode { get; set; }
        public string subStatusText { get; set; }
        public string uniqueResponseNo { get; set; }
        public string version { get; set; }
    }
    public class BNRespTransfer
    {
        public string attemptNo { get; set; }
        public string beneficiaryBank { get; set; }
        public string lowBalanceAlert { get; set; }
        public string nameWithBeneficiaryBank { get; set; }
        public string requestReferenceNo { get; set; }
        public string transactionStatus { get; set; }
        public string transferType { get; set; }
        public string uniqueResponseNo { get; set; }
        public string version { get; set; }
    }
}

/********** Sample Response JSON Start Transfer ****************************
 
    {
     "attemptNo": "1",
     "reqTransferType": "NEFT",
     "requestReferenceNo": "dbb9ab39abaeaa59",
     "statusCode": "ok",
     "subStatusCode": "",
     "subStatusText": "",
     "uniqueResponseNo": "2b2b689ab98ebafa",
     "version": "1.0"
}

 ********************************************************************/
/********** Sample Response JSON Transfer ****************************
{
     "attemptNo": "1",
     "beneficiaryBank": "ICICI",
     "lowBalanceAlert": "N",
     "nameWithBeneficiaryBank": "XYZ",
     "requestReferenceNo": "187a8a89bd8f889b",
     "transactionStatus": "success",
     "transferType": "NEFT",
     "uniqueResponseNo": "891818ab86868a68",
     "version": "1"
}

*************************************************************/
