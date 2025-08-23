using System.Collections.Generic;
namespace BankAPI.YesBank
{
    public class BNRespTransferStatus
    {
        public string beneficiaryAccountNo { get; set; }
        public string beneficiaryBank { get; set; }
        public string beneficiaryName { get; set; }
        public string reqTransferType { get; set; }
        public string transactionDate { get; set; }
        public string transactionStatus { get; set; }
        public string transferAmount { get; set; }
        public string transferCurrencyCode { get; set; }
        public string transferType { get; set; }
        public string Version { get; set; }

    }
}

/**********Sample JSON************************
{
     "beneficiaryAccountNo": "000000000000002",
     "beneficiaryBank": "YesBank Ltd",
     "beneficiaryName": "MOM",
     "reqTransferType": "UPI",
     "transactionDate": "01/01/2019",
     "transactionStatus": "success",
     "transferAmount": "500",
     "transferCurrencyCode": "INR",
     "transferType": "UPI",
     "Version": "1"
}

*******************************************/
