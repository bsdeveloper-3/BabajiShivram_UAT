using System.Collections.Generic;
namespace BankAPI.YesBank
{
    public class BNReqGetBalance
    {
        public string version { get; set; }

        public string appID { get; set; }

        public string customerID { get; set; }

        public string AccountNumber { get; set; }

    }

    // Sample JSON
    /************************************************
     {
  "getBalance": {
    "version": "1.0",
    "appID": "96422",
    "customerID": "96422",
    "AccountNumber": "001090600001595"
  }
}

    ************************************************/

}