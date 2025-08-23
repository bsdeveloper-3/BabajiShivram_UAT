
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace TaxProEWB.API
{
  public static class APITools
  {
    public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
    {
      ContractResolver = (IContractResolver) new SubstituteNullWithEmptyStringContractResolver()
    };

        public static APITxnLogArgs ScanAPIResponse(string APIAction, IRestResponse response)
        {
            APITxnLogArgs apiTxnLogArgs = (APITxnLogArgs)null;
            string ErrorMsg = "";
            int statusCode = (int)response.StatusCode;
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                apiTxnLogArgs = APITools.SetAPITxnLogArgs(APIAction, false, response.StatusDescription, statusCode.ToString());
            if (string.IsNullOrEmpty(response.Content))
                apiTxnLogArgs = APITools.SetAPITxnLogArgs(APIAction, false, "Failed to get Response. Check connectivity or API settings.", "");
                        
            if (response.Content.Contains("\"status_cd\":\"0\""))
            {
                RespErrPl respErrCodePl = JsonConvert.DeserializeObject<RespErrPl>(response.Content);
                //RespErrCodePl respErrCodePl = JsonConvert.DeserializeObject<RespErrCodePl>(Encoding.UTF8.GetString(Convert.FromBase64String(JsonConvert.DeserializeObject<RespErrPl>(response.Content).error)));
                if (respErrCodePl.error.error_cd.Contains<char>(','))
                {
                    string errorCodes = respErrCodePl.error.error_cd;
                    char[] chArray = new char[1] { ',' };
                    foreach (string str1 in errorCodes.Split(chArray))
                    {
                        if (str1 != "")
                        {
                            string str2;
                            ErrCodeDict.EwbErrCodes.TryGetValue(str1.Trim(), out str2);
                            ErrorMsg = ErrorMsg + str1 + ": " + str2 + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    //ErrCodeDict.EwbErrCodes.TryGetValue(respErrCodePl.error.error_cd, out ErrorMsg);

                    ErrorMsg = respErrCodePl.error.message;
                }
                apiTxnLogArgs = APITools.SetAPITxnLogArgs(APIAction, false, ErrorMsg, respErrCodePl.error.error_cd);
            }
            else if (response.Content.Contains("\"status\":\"1\""))
                apiTxnLogArgs = new APITxnLogArgs()
                {
                    IsSuccess = true,
                    OutcomeMsg = "Success"
                };
            if (apiTxnLogArgs != null)
                return apiTxnLogArgs;
            return new APITxnLogArgs() { IsSuccess = true };
        }

    public static APITxnLogArgs SetAPITxnLogArgs(string ApiAction, bool isSuccess, string ErrorMsg, string ErrorCode)
    {
      return new APITxnLogArgs()
      {
        TxnDateTime = DateTime.Now,
        IsSuccess = isSuccess,
        ApiAction = ApiAction,
        OutcomeMsg = ErrorMsg,
        ErrCode = ErrorCode
      };
    }

    public static APITxnLogArgs SetAPITxnLogArgs(string ApiAction, bool isSuccess, string ErrorMsg, string ErrorCode, string ErrInfo)
    {
        APITxnLogArgs e = new APITxnLogArgs()
        {
            TxnDateTime = DateTime.Now,
            IsSuccess = isSuccess,
            ApiAction = ApiAction,
            OutcomeMsg = ErrorMsg,
            ErrCode = ErrorCode,
            ErrInfo = ErrInfo
        };
        return e;
    }
  }
}
