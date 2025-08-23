// Decompiled with JetBrains decompiler
// Type: TaxProEWB.API.SharedEwbAPI
// Assembly: TaxProEWB.API, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 651C2957-9A00-43E1-9864-7C8CEF88DD73
// Assembly location: C:\inetpub\wwwroot\TaxProEWBApiIntigrationDemo.NET\bin\x86\Debug\TaxProEWB.API.dll

using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TaxProEWB.API
{
  public static class SharedEwbAPI
  {
    public static TxnRespWithObj<string> PostAPIAsync<T>(EWBSession EwbSession, string Action, T PayloadObj, bool LogJsonFile = false)
    {
      string Payload = JsonConvert.SerializeObject((object) (T) PayloadObj, Formatting.Indented);
      if (LogJsonFile)
      {
        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "EWB_Json"))
          Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "EWB_Json");
        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "EWB_Json\\" + EwbSession.EwbApiLoginDetails.EwbUserID + "_" + Action + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".json", Payload);
      }
      TxnRespWithObj<string> txnRespWithObj =  SharedEwbAPI.PostAPIAsync(EwbSession, Action, Payload);
      return txnRespWithObj;
    }

    public static  TxnRespWithObj<string> PostAPIAsync(EWBSession EwbSession, string ApiAction, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in EWB API Call.";
      ReqPl reqPL = new ReqPl();
      RespPl respPL = (RespPl) null;
      string decData = "";
      TxnResp txnResp =  EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        txnLogArgs = APITools.SetAPITxnLogArgs(ApiAction, isSuccess, startTxnResp.TxnOutcome, "");
      }
      else
      {
        //string strURL = @"https://api.taxprogsp.co.in/v1.02/dec/ewayapi?action=GENEWAYBILL&aspid=1602821918&password=babaji@123&gstin=27AAACN1163G1ZR&authtoken=4gNNltgEMvhx9pUBrXPCX9hYF&username=NAVBHARAT_APINAV";
                                
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/ewayapi");

        //RestClient client = new RestClient(strURL);

        RestRequest request = new RestRequest(Method.POST);
        EwbSession.AddApiHeaders(request);
        //reqPL.action = ApiAction;
        request.AddParameter("undefined",JsonPayload, ParameterType.RequestBody);
        request.AddParameter("action", ApiAction, ParameterType.QueryString);
        //JsonPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonPayload));
        //reqPL.data = TPCrypto.AesEncryptBase64(JsonPayload, EwbSession.EwbApiLoginDetails.EwbSEK);
        // reqPL.data = JsonPayload;
        // request.AddBody((object)JsonPayload);
        IRestResponse restResponse =  client.Execute(request);
        IRestResponse response = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(ApiAction, response);
            
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            decData = response.Content;
           
            strOutcome = "Success.";
          }
          catch
          {            
            strOutcome = Constants.DecryptionError;
          }

          txnLogArgs = APITools.SetAPITxnLogArgs(ApiAction, isSuccess, strOutcome, "");
        }
        
        client = (RestClient) null;
        request = (RestRequest) null;
        response = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
        return new TxnRespWithObj<string>()
        {
            IsSuccess = isSuccess,
            TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
            RespObj = decData

        };
    }
  }
}
