using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace MyPacco.API
{
    public static class MyPaccoAPITools
    {
        public static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = (IContractResolver)new SubstituteNullWithEmptyStringContractResolver()
        };
        public static MyPaccoLogArgs ScanAPIResponse(string APIAction, IRestResponse response)
        {
            MyPaccoLogArgs apiTxnLogArgs = (MyPaccoLogArgs)null;
            string ErrorMsg = "";
            int statusCode = (int)response.StatusCode;
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                apiTxnLogArgs = MyPaccoAPITools.SetAPITxnLogArgs(APIAction, false, response.StatusDescription, statusCode.ToString());
            if (string.IsNullOrEmpty(response.Content))
                apiTxnLogArgs = MyPaccoAPITools.SetAPITxnLogArgs(APIAction, false, "Failed to get Response. Check connectivity or API settings.", "");

            if (response.Content.Contains("\"status_cd\":\"0\""))
            {
                MyPaccoRespErrPl respErrCodePl = JsonConvert.DeserializeObject<MyPaccoRespErrPl>(response.Content);
                //RespErrCodePl respErrCodePl = JsonConvert.DeserializeObject<RespErrCodePl>(Encoding.UTF8.GetString(Convert.FromBase64String(JsonConvert.DeserializeObject<RespErrPl>(response.Content).error)));
                if (respErrCodePl.error.error_cd.Contains<char>(','))
                {
                    ErrorMsg = respErrCodePl.error.message;

                    //string errorCodes = respErrCodePl.error.error_cd;
                    //char[] chArray = new char[1] { ',' };
                    //foreach (string str1 in errorCodes.Split(chArray))
                    //{
                    //    if (str1 != "")
                    //    {
                    //        string str2;
                    //        ErrCodeDict.EwbErrCodes.TryGetValue(str1.Trim(), out str2);
                    //        ErrorMsg = ErrorMsg + str1 + ": " + str2 + Environment.NewLine;
                    //    }
                    //}
                }
                else
                {
                    //ErrCodeDict.EwbErrCodes.TryGetValue(respErrCodePl.error.error_cd, out ErrorMsg);

                    ErrorMsg = respErrCodePl.error.message;
                }
                apiTxnLogArgs = MyPaccoAPITools.SetAPITxnLogArgs(APIAction, false, ErrorMsg, respErrCodePl.error.error_cd);
            }
            else if (response.Content.Contains("\"status\":\"1\""))
                apiTxnLogArgs = new MyPaccoLogArgs()
                {
                    IsSuccess = true,
                    OutcomeMsg = "Success"
                };
            if (apiTxnLogArgs != null)
                return apiTxnLogArgs;
            return new MyPaccoLogArgs() { IsSuccess = true };
        }

        public static MyPaccoLogArgs SetAPITxnLogArgs(string ApiAction, bool isSuccess, string ErrorMsg, string ErrorCode)
        {
            return new MyPaccoLogArgs()
            {
                TxnDateTime = DateTime.Now,
                IsSuccess = isSuccess,
                ApiAction = ApiAction,
                OutcomeMsg = ErrorMsg,
                ErrCode = ErrorCode

            };
        }
    }
}
