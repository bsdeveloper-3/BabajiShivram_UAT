using System.Collections.Generic;
namespace BankAPI.YesBank
{
    public class BNRespGetBalance
    {
        public string Version { get; set; }

        public string accountBalanceAmount { get; set; }

        public string accountCurrencyCode { get; set; }

        public string lowBalanceAlert { get; set; }

    }
}