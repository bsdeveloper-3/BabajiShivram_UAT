
using System.Collections.Generic;
namespace BankAPI.YesBank
{
    public class BNRespGenAccessToken
    {
        public string access_token { get; set; }

        public string expires_in { get; set; }

        public string token_type { get; set; }

        public string status { get; set; }

        public string GENERATE_STATUS { get; set; }
    }
}