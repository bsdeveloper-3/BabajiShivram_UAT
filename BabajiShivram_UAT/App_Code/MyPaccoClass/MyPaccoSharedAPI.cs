using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyPacco.API
{
    public static class MyPaccoSharedAPI
    {
        public static MyPaccoRespWithObj<string> PostAPIAsync<T>(MyPaccoSession EwbSession, string Action, T PayloadObj, bool LogJsonFile = false)
        {
            string Payload = JsonConvert.SerializeObject((object)(T)PayloadObj, Formatting.Indented);
            if (LogJsonFile)
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "EWB_Json"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "EWB_Json");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "EWB_Json\\" +  Action + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".json", Payload);
            }
            MyPaccoRespWithObj<string> txnRespWithObj = MyPaccoSharedAPI.PostAPIAsync(EwbSession, Action, Payload);
            return txnRespWithObj;
        }

        public static MyPaccoRespWithObj<string> PostAPIAsync(MyPaccoSession EwbSession, string ApiAction, string JsonPayload)
        {
            bool isSuccess = false;
            string strOutcome = "Errors in MyPacco API Call.";
            MyPaccoReqPl reqPL = new MyPaccoReqPl();
            MyPaccoReqPl respPL = (MyPaccoReqPl)null;
            string decData = "";
           // MyPaccoResp txnResp = EwbSession.StartAPITxn(false);
           // MyPaccoResp startTxnResp = txnResp;
          //  txnResp = (MyPaccoResp)null;
            MyPaccoLogArgs txnLogArgs;

            //if (!(isSuccess = startTxnResp.IsSuccess))
            //{
            //    txnLogArgs = MyPaccoAPITools.SetAPITxnLogArgs(ApiAction, isSuccess, startTxnResp.TxnOutcome, "");
            //}
            //else
            //{
            //    //string strURL = @"https://api.taxprogsp.co.in/v1.03/dec/ewayapi?action=GENEWAYBILL&aspid=1602821918&password=babaji@123&gstin=27AAACN1163G1ZR&authtoken=4gNNltgEMvhx9pUBrXPCX9hYF&username=NAVBHARAT_APINAV";

            //    RestClient client = new RestClient(EwbSession.MyPaccoApiSetting.MyPaccoBaseUrl + "/ewayapi");

            //    //RestClient client = new RestClient(strURL);

            //    RestRequest request = new RestRequest(Method.POST);
            //    EwbSession.AddApiHeaders(request);
            //    //reqPL.action = ApiAction;
            //    request.AddParameter("undefined", JsonPayload, ParameterType.RequestBody);
            //    request.AddParameter("action", ApiAction, ParameterType.QueryString);
            //    //JsonPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonPayload));
            //    //reqPL.data = TPCrypto.AesEncryptBase64(JsonPayload, EwbSession.EwbApiLoginDetails.EwbSEK);
            //    // reqPL.data = JsonPayload;
            //    // request.AddBody((object)JsonPayload);
            //    IRestResponse restResponse = client.Execute(request);
            //    IRestResponse response = restResponse;
            //    restResponse = (IRestResponse)null;
            //    txnLogArgs = MyPaccoAPITools.ScanAPIResponse(ApiAction, response);

            //    if (isSuccess = txnLogArgs.IsSuccess)
            //    {
            //        try
            //        {
            //            decData = response.Content;

            //            strOutcome = "Success.";
            //        }
            //        catch
            //        {
            //            strOutcome = MyPaccoConstants.DecryptionError;
            //        }

            //        txnLogArgs = MyPaccoAPITools.SetAPITxnLogArgs(ApiAction, isSuccess, strOutcome, "");
            //    }

            //    client = (RestClient)null;
            //    request = (RestRequest)null;
            //    response = (IRestResponse)null;
            //}

           // EwbSession.MyPaccoLogAPITxn(txnLogArgs);

            return new MyPaccoRespWithObj<string>()
            {
                IsSuccess = isSuccess,
                //TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
                RespObj = decData

            };
        }

        public static MyPaccoRespWithObj<string> DownloadAPIAsync(MyPaccoSession EwbSession, string JsonPayload)
        {
            bool isSuccess = false;
            string strOutcome = "Errors in PRINT EWB API Call.";
            MyPaccoReqPl reqPL = new MyPaccoReqPl();
            MyPaccoReqPl respPL = (MyPaccoReqPl)null;
            string decData = "";
            MyPaccoLogArgs txnLogArgs;

            string strURL = @"https://api.taxprogsp.co.in/aspapi/v1.0/printewb";

            RestClient client = new RestClient(strURL);

            RestRequest request = new RestRequest(Method.POST);
            //request.AddParameter("aspid", EwbSession.EwbApiSetting.ID.ToString(), ParameterType.QueryString);
            //request.AddParameter("Gstin", EwbSession.EwbApiLoginDetails.EwbGstin, ParameterType.QueryString);
            //request.AddParameter("password", EwbSession.EwbApiSetting.AspPassword, ParameterType.QueryString);
            //request.AddParameter("undefined", JsonPayload, ParameterType.RequestBody); // EWay Detail Response JSON

            IRestResponse restResponse = client.Execute(request);
            IRestResponse response = restResponse;
            restResponse = (IRestResponse)null;
            decData = response.Content;
            if (isSuccess == true)
            {
                try
                {
                    decData = response.Content;

                    strOutcome = "Success.";
                }
                catch
                {
                    strOutcome = MyPaccoConstants.DecryptionError;
                }

                txnLogArgs = MyPaccoAPITools.SetAPITxnLogArgs("PrintEWB", isSuccess, strOutcome, "");
            }

            client = (RestClient)null;
            request = (RestRequest)null;
            response = (IRestResponse)null;

            return new MyPaccoRespWithObj<string>()
            {
                IsSuccess = isSuccess,
                RespObj = decData
            };
        }
    }
}