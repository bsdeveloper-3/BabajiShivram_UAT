using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxProEWB.API
{
  public static class EWBAPI
  {
    public static  TxnRespWithObj<EWBSession> GetAuthTokenAsync(EWBSession EwbSession)
    {
      bool isSuccess = false;
      string ApiAction = "ACCESSTOKEN";
      AuthReqPl reqToken = new AuthReqPl();
      if (EwbSession == null)
        return new TxnRespWithObj<EWBSession>()
        {
          IsSuccess = false,
          TxnOutcome = "Error: EwbSession Not Valid. Please create Credentials for EWB User."
        };
     // TxnResp txnResp = EwbSession.StartAPITxn(true);
     // TxnResp startTxnResp = txnResp;
     // txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      //if (!(isSuccess = startTxnResp.IsSuccess))
      //{
      //  txnLogArgs = APITools.SetAPITxnLogArgs(ApiAction, isSuccess, startTxnResp.TxnOutcome, "");
      //}
      //else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/auth");
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        reqToken.action = ApiAction;
                
       // byte[] appKey = TPCrypto.GenAesKey();
        //EwbSession.EwbApiLoginDetails.EwbAppKey = Convert.ToBase64String(appKey);
        reqToken.username = EwbSession.EwbApiLoginDetails.EwbUserID;
        //reqToken.app_key = TPCrypto.EncryptUsingPubKey(appKey);
                
        reqToken.password = EwbSession.EwbApiSetting.AspPassword; //"babaji@123";
        request.AddBody((object) reqToken);

        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse response = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(reqToken.action, response);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            AuthRespPl resp = JsonConvert.DeserializeObject<AuthRespPl>(response.Content);
            EwbSession.EwbApiLoginDetails.EwbTokenExp = DateTime.Now.AddMinutes((double) Constants.AuthTokenValidityMin);
            EwbSession.EwbApiLoginDetails.EwbAuthToken = resp.authtoken;
                        
            txnLogArgs = APITools.SetAPITxnLogArgs(ApiAction, isSuccess, txnLogArgs.OutcomeMsg, "");
          }
          catch
          {
            isSuccess = false;
            txnLogArgs.OutcomeMsg = Constants.DecryptionError;
          }
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        //appKey = (byte[]) null;
        response = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<EWBSession>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = EwbSession
      };
    }

    public static  TxnRespWithObj<RespGenEwbPl> GenEWBAsync(EWBSession EwbSession, ReqGenEwbPl ewbGen, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in EWB Generation.";
      RespGenEwbPl GeneratedEwb = (RespGenEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqGenEwbPl>(EwbSession, "GENEWAYBILL", ewbGen, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          GeneratedEwb = JsonConvert.DeserializeObject<RespGenEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      else
        strOutcome = TxnResp.TxnOutcome;
      return new TxnRespWithObj<RespGenEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = GeneratedEwb
      };
    }

    public static  TxnRespWithObj<RespGenEwbPl> GenEWBAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in EWB Generation.";
      RespGenEwbPl GeneratedEwb = (RespGenEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "GENEWAYBILL", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          GeneratedEwb = JsonConvert.DeserializeObject<RespGenEwbPl>(TxnResp.RespObj);
            //GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(response.Content);
            strOutcome = "Request for EWB Assigned for transporter Successfull";
            //string DecRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            //decData = TPCrypto.AesDecryptData(respPL.data, DecRek);
            //getEwbForTrans = JsonConvert.DeserializeObject<List<AssignedEWBItem>>(decData);
            //getEwbForTrans = JsonConvert.DeserializeObject<List<AssignedEWBItem>>(response.Content, APITools.jsonSettings);
            //respPL = (GetRespPl) null;
            //DecRek = (string) null;
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
        else if (TxnResp.TxnOutcome.StartsWith("GSP102"))
        {
            // Auth Token Expired Re-Generate Totaken

            TxnResp txnResp = EwbSession.StartAPITxn(true);
        }
      return new TxnRespWithObj<RespGenEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = GeneratedEwb
      };
    }

    public static  TxnRespWithObj<RespVehicleNoUpdtPl> UpdateVehicleNosync(EWBSession EwbSession, ReqVehicleNoUpdtPl reqVehicleNoUpdt, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Updation of Vehicle No.";
      RespVehicleNoUpdtPl updtVehicleNo = (RespVehicleNoUpdtPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqVehicleNoUpdtPl>(EwbSession, "VEHEWB", reqVehicleNoUpdt, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          updtVehicleNo = JsonConvert.DeserializeObject<RespVehicleNoUpdtPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespVehicleNoUpdtPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = updtVehicleNo
      };
    }

    public static  TxnRespWithObj<RespVehicleNoUpdtPl> UpdateVehicleNosync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Updation of Vehicle No.";
      RespVehicleNoUpdtPl updtVehicleNo = (RespVehicleNoUpdtPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "VEHEWB", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          updtVehicleNo = JsonConvert.DeserializeObject<RespVehicleNoUpdtPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
        else if (TxnResp.TxnOutcome.StartsWith("GSP102"))
        {
        // Auth Token Expired Re-Generate Totaken

            TxnResp txnResp = EwbSession.StartAPITxn(true);
        }
      return new TxnRespWithObj<RespVehicleNoUpdtPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = updtVehicleNo
      };
    }
    public static TxnRespWithObj<RespIniMultiVehicleMov> InitiateMultiVehMovntAsync(EWBSession EwbSession, string JsonPayload)
        {
            bool isSuccess = false;
            string strOutcome = "Errors in Updation of Vehicle No.";
            RespIniMultiVehicleMov responceIniMulVeh = (RespIniMultiVehicleMov)null;
            TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "MULTIVEHMOVINT", JsonPayload);
            TxnRespWithObj<string> TxnResp = txnRespWithObj;
            txnRespWithObj = (TxnRespWithObj<string>)null;
            strOutcome = TxnResp.TxnOutcome;
            if (isSuccess = TxnResp.IsSuccess)
            {
                try
                {
                    responceIniMulVeh = JsonConvert.DeserializeObject<RespIniMultiVehicleMov>(TxnResp.RespObj);
                }
                catch
                {
                    strOutcome = Constants.DecryptionError;
                    isSuccess = false;
                }
            }
            else if (TxnResp.TxnOutcome.StartsWith("GSP102"))
            {
                // Auth Token Expired Re-Generate Totaken

                TxnResp txnResp = EwbSession.StartAPITxn(true);
            }
            return new TxnRespWithObj<RespIniMultiVehicleMov>()
            {
                IsSuccess = isSuccess,
                TxnOutcome = strOutcome,
                RespObj = responceIniMulVeh
            };
        }
    public static TxnRespWithObj<RespMultiVehAdd> AddMultiVehicleNosync(EWBSession EwbSession, string JsonPayload)
        {
            bool isSuccess = false;
            string strOutcome = "Errors in Updation of Vehicle No.";
            RespMultiVehAdd updtVehicleNo = (RespMultiVehAdd)null;
            TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "MULTIVEHADD", JsonPayload);
            TxnRespWithObj<string> TxnResp = txnRespWithObj;
            txnRespWithObj = (TxnRespWithObj<string>)null;
            strOutcome = TxnResp.TxnOutcome;
            if (isSuccess = TxnResp.IsSuccess)
            {
                try
                {
                    updtVehicleNo = JsonConvert.DeserializeObject<RespMultiVehAdd>(TxnResp.RespObj);
                }
                catch
                {
                    strOutcome = Constants.DecryptionError;
                    isSuccess = false;
                }
            }
            else if (TxnResp.TxnOutcome.StartsWith("GSP102"))
            {
                // Auth Token Expired Re-Generate Totaken

                TxnResp txnResp = EwbSession.StartAPITxn(true);
            }
            return new TxnRespWithObj<RespMultiVehAdd>()
            {
                IsSuccess = isSuccess,
                TxnOutcome = strOutcome,
                RespObj = updtVehicleNo
            };
        }
    public static  TxnRespWithObj<RespGenCEwbPl> GenCEWBAsync(EWBSession EwbSession, ReqGenCEwbPl reqCEWB, bool LogJsONfILE = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Generation of Consolidated EWB";
      RespGenCEwbPl GenCEWB = (RespGenCEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj =  SharedEwbAPI.PostAPIAsync<ReqGenCEwbPl>(EwbSession, "GENCEWB", reqCEWB, LogJsONfILE);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          GenCEWB = JsonConvert.DeserializeObject<RespGenCEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespGenCEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = GenCEWB
      };
    }

    public static  TxnRespWithObj<RespGenCEwbPl> GenCEWBAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Generation of Consolidated EWB";
      RespGenCEwbPl GenCEWB = (RespGenCEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "GENCEWB", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          GenCEWB = JsonConvert.DeserializeObject<RespGenCEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
    else if (TxnResp.TxnOutcome.StartsWith("GSP102"))
    {
        // Auth Token Expired Re-Generate Totaken

        TxnResp txnResp = EwbSession.StartAPITxn(true);
    }
      return new TxnRespWithObj<RespGenCEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = GenCEWB
      };
    }

    public static  TxnRespWithObj<RespCancelEwbPl> CancelEWBAsync(EWBSession EwbSession, ReqCancelEwbPl reqCancelEwb, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Cancellation of E-way bill";
      RespCancelEwbPl CancelEWB = (RespCancelEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqCancelEwbPl>(EwbSession, "CANEWB", reqCancelEwb, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          CancelEWB = JsonConvert.DeserializeObject<RespCancelEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      else if(TxnResp.TxnOutcome == "GSP102")
        {
            // Auth Token Expired Re-Generate Totaken

            TxnResp txnResp = EwbSession.StartAPITxn(true);
        }
      return new TxnRespWithObj<RespCancelEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = CancelEWB
      };
    }

    public static  TxnRespWithObj<RespCancelEwbPl> CancelEWBAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Cancellation of E-way bill";
      RespCancelEwbPl CancelEWB = (RespCancelEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "CANEWB", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          CancelEWB = JsonConvert.DeserializeObject<RespCancelEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
        else if (TxnResp.TxnOutcome == "GSP102")
        {
            // Auth Token Expired Re-Generate Totaken

            TxnResp txnResp = EwbSession.StartAPITxn(true);
        }
        return new TxnRespWithObj<RespCancelEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = CancelEWB
      };
    }

    public static  TxnRespWithObj<RespRejectEwbPl> RejectEWBAsync(EWBSession EwbSession, ReqRejectEwbPl reqRejectEwb, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Rejection of E-way bill";
      RespRejectEwbPl RejectEWB = (RespRejectEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqRejectEwbPl>(EwbSession, "REJEWB", reqRejectEwb, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          RejectEWB = JsonConvert.DeserializeObject<RespRejectEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespRejectEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = RejectEWB
      };
    }

    public static  TxnRespWithObj<RespRejectEwbPl> RejectEWBAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Rejection of E-way bill";
      RespRejectEwbPl RejectEWB = (RespRejectEwbPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "REJEWB", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          RejectEWB = JsonConvert.DeserializeObject<RespRejectEwbPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
    else if (TxnResp.TxnOutcome.StartsWith("GSP102"))
    {
        // Auth Token Expired Re-Generate Totaken

        TxnResp txnResp = EwbSession.StartAPITxn(true);
    }
      return new TxnRespWithObj<RespRejectEwbPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = RejectEWB
      };
    }

    public static  TxnRespWithObj<RespUpdtTransporterPl> UpdtTransporterAsync(EWBSession EwbSession, ReqUpdtTransporterPl reqUpdtTrans, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Updation of Transporter";
      RespUpdtTransporterPl respUpdtTrans = (RespUpdtTransporterPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqUpdtTransporterPl>(EwbSession, "UPDATETRANSPORTER", reqUpdtTrans, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          respUpdtTrans = JsonConvert.DeserializeObject<RespUpdtTransporterPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespUpdtTransporterPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = respUpdtTrans
      };
    }

    public static  TxnRespWithObj<RespUpdtTransporterPl> UpdtTransporterAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Updation of Transporter";
      RespUpdtTransporterPl respUpdtTrans = (RespUpdtTransporterPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "UPDATETRANSPORTER", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          respUpdtTrans = JsonConvert.DeserializeObject<RespUpdtTransporterPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespUpdtTransporterPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = respUpdtTrans
      };
    }

    public static  TxnRespWithObj<RespExtendValidityEWBPl> ExtendValidityOfEWBAsync(EWBSession EwbSession, ReqExtendValidityEWBPl reqObject, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      RespExtendValidityEWBPl RespObjExtValidity = (RespExtendValidityEWBPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqExtendValidityEWBPl>(EwbSession, "EXTENDVALIDITY", reqObject, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      string strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          RespObjExtValidity = JsonConvert.DeserializeObject<RespExtendValidityEWBPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespExtendValidityEWBPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = RespObjExtValidity
      };
    }

    public static  TxnRespWithObj<RespExtendValidityEWBPl> ExtendValidityOfEWBAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in Extend Validity of EWB";
      RespExtendValidityEWBPl RespObjExtValidity = (RespExtendValidityEWBPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "EXTENDVALIDITY", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          RespObjExtValidity = JsonConvert.DeserializeObject<RespExtendValidityEWBPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespExtendValidityEWBPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = RespObjExtValidity
      };
    }

    public static  TxnRespWithObj<RespReGenerateCEWBPl> ReGenCEWBAsync(EWBSession EwbSession, ReqReGenerateCEWBPl reqObject, bool LogJsonFile = false)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in ReGen Consolidated e-Way Bill";
      RespReGenerateCEWBPl RespObjReCEWB = (RespReGenerateCEWBPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync<ReqReGenerateCEWBPl>(EwbSession, "REGENTRIPSHEET", reqObject, LogJsonFile);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          RespObjReCEWB = JsonConvert.DeserializeObject<RespReGenerateCEWBPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespReGenerateCEWBPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = RespObjReCEWB
      };
    }

    public static  TxnRespWithObj<RespReGenerateCEWBPl> ReGenCEWBAsync(EWBSession EwbSession, string JsonPayload)
    {
      bool isSuccess = false;
      string strOutcome = "Errors in ReGen Consolidated e-Way Bill";
      RespReGenerateCEWBPl RespObjReCEWB = (RespReGenerateCEWBPl) null;
      TxnRespWithObj<string> txnRespWithObj = SharedEwbAPI.PostAPIAsync(EwbSession, "REGENTRIPSHEET", JsonPayload);
      TxnRespWithObj<string> TxnResp = txnRespWithObj;
      txnRespWithObj = (TxnRespWithObj<string>) null;
      strOutcome = TxnResp.TxnOutcome;
      if (isSuccess = TxnResp.IsSuccess)
      {
        try
        {
          RespObjReCEWB = JsonConvert.DeserializeObject<RespReGenerateCEWBPl>(TxnResp.RespObj);
        }
        catch
        {
          strOutcome = Constants.DecryptionError;
          isSuccess = false;
        }
      }
      return new TxnRespWithObj<RespReGenerateCEWBPl>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = strOutcome,
        RespObj = RespObjReCEWB
      };
    }    
    public static TxnRespWithObj<RespGetEWBDetail> GetEWBDetailAsync(EWBSession EwbSession, long EwbNo)
    {
      bool isSuccess = false;
      string strOutcome = ""; string strRawData = "";
      string APIAction = "GetEwayBill";
      RespGetEWBDetail getEwbDetail = (RespGetEWBDetail) null;
      TxnResp txnResp =  EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;

      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/ewayapi");
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("action", (object)APIAction, ParameterType.QueryString);
        request.AddParameter("EwbNo", (object) EwbNo, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse EWBDetailResp = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, EWBDetailResp);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            //GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(EWBDetailResp.Content);
            strOutcome = "Request for E-Wey-Bill Detail Successful";
            strRawData = EWBDetailResp.Content;
           // string DecRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
           // string decData = TPCrypto.AesDecryptData(respPL.data, DecRek);
                        getEwbDetail = JsonConvert.DeserializeObject<RespGetEWBDetail>(EWBDetailResp.Content, APITools.jsonSettings);
           // respPL = (GetRespPl) null;
          //  DecRek = (string) null;
          //  decData = (string) null;
          }
          catch (Exception ex1)
          {
            Exception ex = ex1;
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        else if(txnLogArgs.ErrCode == "GSP102")
        {
            TxnResp txnRespNew = EwbSession.StartAPITxn(true);
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        EWBDetailResp = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<RespGetEWBDetail>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = getEwbDetail,
        RawData = strRawData
      };
    }

    public static  TxnRespWithObj<List<AssignedEWBItem>> GetEWBAssignedForTransAsync(EWBSession EwbSession, string date)
    {
      string decData = "";
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetEwayBillsForTransporter";
      List<AssignedEWBItem> getEwbForTrans = (List<AssignedEWBItem>) null;
      TxnResp txnResp =  EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/ewayapi");
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("action", (object)APIAction, ParameterType.QueryString);
        request.AddParameter("date", (object) date, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute(request);
        IRestResponse response = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, response);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            //GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(response.Content);
            strOutcome = "Request for EWB Assigned for transporter Successfull";
            //string DecRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            //decData = TPCrypto.AesDecryptData(respPL.data, DecRek);
            //getEwbForTrans = JsonConvert.DeserializeObject<List<AssignedEWBItem>>(decData);
            getEwbForTrans = JsonConvert.DeserializeObject<List<AssignedEWBItem>>(response.Content, APITools.jsonSettings);
            //respPL = (GetRespPl) null;
            //DecRek = (string) null;
          }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        response = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<List<AssignedEWBItem>>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = getEwbForTrans
      };
    }

    public static TxnRespWithObj<List<AssignedEWBItem>> GetEWBAssignedForTransByGstinAsync(EWBSession EwbSession, string date, string Gen_gstin)
    {
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetEwayBillsForTransporterByGstin";
      List<AssignedEWBItem> getEwbForTransByGstin = (List<AssignedEWBItem>) null;
      TxnResp txnResp = EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/ewayapi/");
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("action", (object)APIAction, ParameterType.QueryString);
        request.AddParameter("Gen_gstin", (object) Gen_gstin, ParameterType.QueryString);
        request.AddParameter("date", (object) date, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse EWBAssignedForTransByGstinResp = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, EWBAssignedForTransByGstinResp);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            //GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(EWBAssignedForTransByGstinResp.Content);
            strOutcome = "Request for EWB Assigned for transporter By GSTIN Successfull";
                        //string DecRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
                        //string decData = TPCrypto.AesDecryptData(respPL.data, DecRek);
                        //getEwbForTransByGstin = JsonConvert.DeserializeObject<List<AssignedEWBItem>>(decData, APITools.jsonSettings);
                        getEwbForTransByGstin = JsonConvert.DeserializeObject<List<AssignedEWBItem>>(EWBAssignedForTransByGstinResp.Content, APITools.jsonSettings);
                        //respPL = (GetRespPl) null;
                        //DecRek = (string) null;
                        //decData = (string) null;
                    }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        EWBAssignedForTransByGstinResp = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<List<AssignedEWBItem>>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = getEwbForTransByGstin
      };
    }

    public static  TxnRespWithObj<List<EwayBillsofOtherParty>> GetEWBOfOtherPartyAsync(EWBSession EwbSession, string date)
    {
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetEwayBillsofOtherParty";
      List<EwayBillsofOtherParty> getEwbOnOtherPartyReq = (List<EwayBillsofOtherParty>) null;
      TxnResp txnResp =  EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/ewayapi");
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("action", (object)APIAction, ParameterType.QueryString);
        request.AddParameter("date", (object)date, ParameterType.QueryString);
        IRestResponse restResponse =  client.Execute((IRestRequest) request);
        IRestResponse EWBOnOtherPartyReqResp = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, EWBOnOtherPartyReqResp);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            //GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(EWBOnOtherPartyReqResp.Content);
            strOutcome = "Request for EWB Generated on request of other party Successfull";
            //string decRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            //string decData = TPCrypto.AesDecryptData(respPL.data, decRek);
            //getEwbOnOtherPartyReq = JsonConvert.DeserializeObject<List<EwayBillsofOtherParty>>(decData, APITools.jsonSettings);
            getEwbOnOtherPartyReq = JsonConvert.DeserializeObject<List<EwayBillsofOtherParty>>(EWBOnOtherPartyReqResp.Content, APITools.jsonSettings);

            //respPL = (GetRespPl) null;
            //decRek = (string) null;
            //decData = (string) null;
          }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        EWBOnOtherPartyReqResp = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<List<EwayBillsofOtherParty>>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = getEwbOnOtherPartyReq
      };
    }

    public static TxnRespWithObj<GetConsolidatedEWB> GetConsolidatedEWBAsync(EWBSession EwbSession, string tripSheetNo)
    {
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetTripSheet";
      GetConsolidatedEWB conEwbGen = (GetConsolidatedEWB) null;
      TxnResp txnResp = EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/ewayapi/" + APIAction);
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("tripSheetNo", (object) tripSheetNo, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse ConsolidatedEWBResp = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, ConsolidatedEWBResp);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(ConsolidatedEWBResp.Content);
            strOutcome = "Request for Consolidated EWB Generation is Successfull";
            string decRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            string decData = TPCrypto.AesDecryptData(respPL.data, decRek);
            conEwbGen = JsonConvert.DeserializeObject<GetConsolidatedEWB>(decData, APITools.jsonSettings);
            respPL = (GetRespPl) null;
            decRek = (string) null;
            decData = (string) null;
          }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        ConsolidatedEWBResp = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<GetConsolidatedEWB>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = conEwbGen
      };
    }

    public static TxnRespWithObj<GSTINDetail> GetGSTNDetailAsync(EWBSession EwbSession, string GSTIN)
    {
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetGSTINDetails";
      GSTINDetail gstnDetail = (GSTINDetail) null;
      TxnResp txnResp = EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/Master/" + APIAction);
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("GSTIN", (object) GSTIN, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse respObj = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, respObj);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(respObj.Content);
            strOutcome = "Request for Get GSTIN Detail is Successfull";
            string decRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            string decData = TPCrypto.AesDecryptData(respPL.data, decRek);
            gstnDetail = JsonConvert.DeserializeObject<GSTINDetail>(decData, APITools.jsonSettings);
            respPL = (GetRespPl) null;
            decRek = (string) null;
            decData = (string) null;
          }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        respObj = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<GSTINDetail>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = gstnDetail
      };
    }

    public static TxnRespWithObj<TransinDetail> GetTransinDetailAsync(EWBSession EwbSession, string trn_no)
    {
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetTransporterDetails";
      TransinDetail transinDetail = (TransinDetail) null;
      TxnResp txnResp = EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/Master/" + APIAction);
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("trn_no", (object) trn_no, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse respObj = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, respObj);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(respObj.Content);
            strOutcome = "Request for Get GSTIN Detail is Successfull";
            string decRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            string decData = TPCrypto.AesDecryptData(respPL.data, decRek);
            transinDetail = JsonConvert.DeserializeObject<TransinDetail>(decData, APITools.jsonSettings);
            respPL = (GetRespPl) null;
            decRek = (string) null;
            decData = (string) null;
          }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        respObj = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<TransinDetail>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = transinDetail
      };
    }

    public static TxnRespWithObj<HSNDetail> GetHSNDetailAsync(EWBSession EwbSession, string Hsncode)
    {
      bool isSuccess = false;
      string strOutcome = "";
      string APIAction = "GetHsnDetailsByHsnCode";
      HSNDetail hsnDetail = (HSNDetail) null;
      TxnResp txnResp = EwbSession.StartAPITxn(false);
      TxnResp startTxnResp = txnResp;
      txnResp = (TxnResp) null;
      APITxnLogArgs txnLogArgs;
      if (!(isSuccess = startTxnResp.IsSuccess))
      {
        strOutcome = startTxnResp.TxnOutcome;
        txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
      }
      else
      {
        RestClient client = new RestClient(EwbSession.EwbApiSetting.BaseUrl + "/Master/" + APIAction);
        RestRequest request = new RestRequest(Method.GET);
        EwbSession.AddApiHeaders(request);
        request.AddParameter("Hsncode", (object) Hsncode, ParameterType.QueryString);
        IRestResponse restResponse = client.Execute((IRestRequest) request);
        IRestResponse respObj = restResponse;
        restResponse = (IRestResponse) null;
        txnLogArgs = APITools.ScanAPIResponse(APIAction, respObj);
        if (isSuccess = txnLogArgs.IsSuccess)
        {
          try
          {
            GetRespPl respPL = JsonConvert.DeserializeObject<GetRespPl>(respObj.Content);
            strOutcome = "Request for Get HSN Detail is Successfull";
            string decRek = TPCrypto.AesDecryptBase64(respPL.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
            string decData = TPCrypto.AesDecryptData(respPL.data, decRek);
            hsnDetail = JsonConvert.DeserializeObject<HSNDetail>(decData, APITools.jsonSettings);
            respPL = (GetRespPl) null;
            decRek = (string) null;
            decData = (string) null;
          }
          catch
          {
            isSuccess = false;
            strOutcome = Constants.DecryptionError;
          }
          txnLogArgs = APITools.SetAPITxnLogArgs(APIAction, isSuccess, strOutcome, "");
        }
        client = (RestClient) null;
        request = (RestRequest) null;
        respObj = (IRestResponse) null;
      }
      EwbSession.LogAPITxn(txnLogArgs);
      return new TxnRespWithObj<HSNDetail>()
      {
        IsSuccess = isSuccess,
        TxnOutcome = txnLogArgs.ErrCode + " " + txnLogArgs.OutcomeMsg,
        RespObj = hsnDetail
      };
    }

    /*****************  Update Nov.2019 ****************************************/

    public static TxnRespWithObj<List<RespError>> GetErrorListAsync(EWBSession EwbSession)
    {
        APITxnLogArgs aPITxnLogArg;
        bool flag = false;
        string decryptionError = "";
        string str = "GetErrorList";
        List<RespError> respErrors = null;
        TxnResp txnResp = EwbSession.StartAPITxn(false);
        TxnResp txnResp1 = txnResp;
        txnResp = null;
        bool isSuccess = txnResp1.IsSuccess;
        bool flag1 = isSuccess;
        flag = isSuccess;
        if (flag1)
        {
            RestClient restClient = new RestClient(string.Concat(EwbSession.EwbApiSetting.BaseUrl, "/Master/", str));
            RestRequest restRequest = new RestRequest(Method.GET);
            EwbSession.AddApiHeaders(restRequest);
            IRestResponse restResponse = restClient.Execute(restRequest);
            IRestResponse restResponse1 = restResponse;
            restResponse = null;
            aPITxnLogArg = APITools.ScanAPIResponse(str, restResponse1);
            bool isSuccess1 = aPITxnLogArg.IsSuccess;
            flag1 = isSuccess1;
            flag = isSuccess1;
            if (flag1)
            {
                try
                {
                    GetRespPl getRespPl = JsonConvert.DeserializeObject<GetRespPl>(restResponse1.Content);
                    decryptionError = "Request for Get HSN Detail is Successfull";
                    string str1 = TPCrypto.AesDecryptBase64(getRespPl.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
                    string str2 = TPCrypto.AesDecryptData(getRespPl.data, str1);
                    respErrors = JsonConvert.DeserializeObject<List<RespError>>(str2, APITools.jsonSettings);
                    getRespPl = null;
                    str1 = null;
                    str2 = null;
                }
                catch
                {
                    flag = false;
                    decryptionError = Constants.DecryptionError;
                }
                aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
            }
            restClient = null;
            restRequest = null;
            restResponse1 = null;
        }
        else
        {
            decryptionError = txnResp1.TxnOutcome;
            aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
        }
        EwbSession.LogAPITxn(aPITxnLogArg);
        TxnRespWithObj<List<RespError>> txnRespWithObj = new TxnRespWithObj<List<RespError>>()
        {
            IsSuccess = flag,
            TxnOutcome = string.Concat(aPITxnLogArg.ErrCode, " ", aPITxnLogArg.OutcomeMsg),
            RespObj = respErrors
        };
        return txnRespWithObj;
    }

    public static TxnRespWithObj<RespEwbDetGenByConsigner> GetEwayBillGeneratedByConsignor(EWBSession EwbSession, string DoccType, string DocNo)
    {
        APITxnLogArgs aPITxnLogArg;
        bool flag = false;
        string decryptionError = "";
        string str = "GetEwayBillGeneratedByConsigner";
        RespEwbDetGenByConsigner respEwbDetGenByConsigner = null;
        TxnResp txnResp = EwbSession.StartAPITxn(false);
        TxnResp txnResp1 = txnResp;
        txnResp = null;
        bool isSuccess = txnResp1.IsSuccess;
        bool flag1 = isSuccess;
        flag = isSuccess;
        if (flag1)
        {
            RestClient restClient = new RestClient(string.Concat(EwbSession.EwbApiSetting.BaseUrl, "/ewayapi/", str));
            RestRequest restRequest = new RestRequest(Method.GET);
            EwbSession.AddApiHeaders(restRequest);
            restRequest.AddParameter("docType", DoccType, ParameterType.QueryString);
            restRequest.AddParameter("docNo", DocNo, ParameterType.QueryString);
            IRestResponse restResponse = restClient.Execute(restRequest);
            IRestResponse restResponse1 = restResponse;
            restResponse = null;
            aPITxnLogArg = APITools.ScanAPIResponse(str, restResponse1);
            bool isSuccess1 = aPITxnLogArg.IsSuccess;
            flag1 = isSuccess1;
            flag = isSuccess1;
            if (flag1)
            {
                try
                {
                    GetRespPl getRespPl = JsonConvert.DeserializeObject<GetRespPl>(restResponse1.Content);
                    decryptionError = "Request for Get Eway Bill Generated By Consignor is successfull!!!";
                    string str1 = TPCrypto.AesDecryptBase64(getRespPl.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
                    string str2 = TPCrypto.AesDecryptData(getRespPl.data, str1);
                    respEwbDetGenByConsigner = JsonConvert.DeserializeObject<RespEwbDetGenByConsigner>(str2, APITools.jsonSettings);
                    getRespPl = null;
                    str1 = null;
                    str2 = null;
                }
                catch
                {
                    flag = false;
                    decryptionError = Constants.DecryptionError;
                }
                aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
            }
            restClient = null;
            restRequest = null;
            restResponse1 = null;
        }
        else
        {
            decryptionError = txnResp1.TxnOutcome;
            aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
        }
        EwbSession.LogAPITxn(aPITxnLogArg);
        TxnRespWithObj<RespEwbDetGenByConsigner> txnRespWithObj = new TxnRespWithObj<RespEwbDetGenByConsigner>()
        {
            IsSuccess = flag,
            TxnOutcome = string.Concat(aPITxnLogArg.ErrCode, " ", aPITxnLogArg.OutcomeMsg),
            RespObj = respEwbDetGenByConsigner
        };
        return txnRespWithObj;
    }

    public static TxnRespWithObj<List<RespGetEwayBillsByDate>> GetEwayBillsByDateAsync(EWBSession EwbSession, string Date)
    {
        APITxnLogArgs aPITxnLogArg;
        bool flag = false;
        string decryptionError = "";
        string str = "GetEwayBillsByDate";
        List<RespGetEwayBillsByDate> respGetEwayBillsByDates = null;
        TxnResp txnResp = EwbSession.StartAPITxn(false);
        TxnResp txnResp1 = txnResp;
        txnResp = null;
        bool isSuccess = txnResp1.IsSuccess;
        bool flag1 = isSuccess;
        flag = isSuccess;
        if (flag1)
        {
            RestClient restClient = new RestClient(string.Concat(EwbSession.EwbApiSetting.BaseUrl, "/ewayapi/"));
            RestRequest restRequest = new RestRequest(Method.GET);
            EwbSession.AddApiHeaders(restRequest);
            restRequest.AddParameter("date", Date, ParameterType.QueryString);
            restRequest.AddParameter("action", (object)str, ParameterType.QueryString);
            IRestResponse restResponse = restClient.Execute(restRequest);
            IRestResponse EwayBillsByDateResponse = restResponse;
            restResponse = null;
            aPITxnLogArg = APITools.ScanAPIResponse(str, EwayBillsByDateResponse);
            bool isSuccess1 = aPITxnLogArg.IsSuccess;
            flag1 = isSuccess1;
            flag = isSuccess1;
            if (flag1)
            {
                try
                {
                    //GetRespPl getRespPl = JsonConvert.DeserializeObject<GetRespPl>(restResponse1.Content);
                    decryptionError = "Request for Get Eway Bill Generated By Date is successfull!!!";
                    //string str1 = TPCrypto.AesDecryptBase64(getRespPl.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
                    //string str2 = TPCrypto.AesDecryptData(getRespPl.data, str1);
                    respGetEwayBillsByDates = JsonConvert.DeserializeObject<List<RespGetEwayBillsByDate>>(EwayBillsByDateResponse.Content, APITools.jsonSettings);

                }
                catch
                {
                    flag = false;
                    decryptionError = Constants.DecryptionError;
                }
                aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
            }
            restClient = null;
            restRequest = null;
            EwayBillsByDateResponse = null;
        }
        else
        {
            decryptionError = txnResp1.TxnOutcome;
            aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
        }
        EwbSession.LogAPITxn(aPITxnLogArg);
        TxnRespWithObj<List<RespGetEwayBillsByDate>> txnRespWithObj = new TxnRespWithObj<List<RespGetEwayBillsByDate>>()
        {
            IsSuccess = flag,
            TxnOutcome = string.Concat(aPITxnLogArg.ErrCode, " ", aPITxnLogArg.OutcomeMsg),
            RespObj = respGetEwayBillsByDates
        };
        return txnRespWithObj;
    }

    public static TxnRespWithObj<List<RespGetEwayBillsForTranByState>> GetEwayBillsForTransporterByState(EWBSession EwbSession, string Date, int StateCode)
    {
        APITxnLogArgs aPITxnLogArg;
        bool flag = false;
        string decryptionError = "";
        string str = "GetEwayBillsForTransporterByState";
        List<RespGetEwayBillsForTranByState> respGetEwayBillsForTranByStates = null;
        TxnResp txnResp = EwbSession.StartAPITxn(false);
        TxnResp txnResp1 = txnResp;
        txnResp = null;
        bool isSuccess = txnResp1.IsSuccess;
        bool flag1 = isSuccess;
        flag = isSuccess;
        if (flag1)
        {
            RestClient restClient = new RestClient(string.Concat(EwbSession.EwbApiSetting.BaseUrl, "/ewayapi/"));
            RestRequest restRequest = new RestRequest(Method.GET);
            EwbSession.AddApiHeaders(restRequest);
            restRequest.AddParameter("date", Date, ParameterType.QueryString);
            restRequest.AddParameter("stateCode", StateCode, ParameterType.QueryString);
            restRequest.AddParameter("action", (object)str, ParameterType.QueryString);
            IRestResponse restResponse = restClient.Execute(restRequest);
            IRestResponse restResponseByState = restResponse;
            restResponse = null;
            aPITxnLogArg = APITools.ScanAPIResponse(str, restResponseByState);
            bool isSuccess1 = aPITxnLogArg.IsSuccess;
            flag1 = isSuccess1;
            flag = isSuccess1;
            if (flag1)
            {
                try
                {
                    //GetRespPl getRespPl = JsonConvert.DeserializeObject<GetRespPl>(restResponse1.Content);
                    decryptionError = "Request for Get Eway Bill Assigned to transporter By state is successfull!";
                    //string str1 = TPCrypto.AesDecryptBase64(getRespPl.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
                    //string str2 = TPCrypto.AesDecryptData(getRespPl.data, str1);
                    respGetEwayBillsForTranByStates = JsonConvert.DeserializeObject<List<RespGetEwayBillsForTranByState>>(restResponseByState.Content, APITools.jsonSettings);

                    //getRespPl = null;
                    //str1 = null;
                    //str2 = null;
                }
                catch
                {
                    flag = false;
                    decryptionError = Constants.DecryptionError;
                }
                aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
            }
            restClient = null;
            restRequest = null;
            restResponseByState = null;
        }
        else
        {
            decryptionError = txnResp1.TxnOutcome;
            aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
        }
        EwbSession.LogAPITxn(aPITxnLogArg);
        TxnRespWithObj<List<RespGetEwayBillsForTranByState>> txnRespWithObj = new TxnRespWithObj<List<RespGetEwayBillsForTranByState>>()
        {
            IsSuccess = flag,
            TxnOutcome = string.Concat(aPITxnLogArg.ErrCode, " ", aPITxnLogArg.OutcomeMsg),
            RespObj = respGetEwayBillsForTranByStates
        };
        return txnRespWithObj;
    }

    public static TxnRespWithObj<List<RespGetEwayBillsRejectedByOthers>> GetEwayBillsRejectedByOthersAsync(EWBSession EwbSession, string Date)
    {
        APITxnLogArgs aPITxnLogArg;
        bool flag = false;
        string decryptionError = "";
        string str = "GetEwayBillsRejectedByOthers";
        List<RespGetEwayBillsRejectedByOthers> respGetEwayBillsRejectedByOthers = null;
        TxnResp txnResp = EwbSession.StartAPITxn(false);
        TxnResp txnResp1 = txnResp;
        txnResp = null;
        bool isSuccess = txnResp1.IsSuccess;
        bool flag1 = isSuccess;
        flag = isSuccess;
        if (flag1)
        {
            RestClient restClient = new RestClient(string.Concat(EwbSession.EwbApiSetting.BaseUrl, "/ewayapi/", str));
            RestRequest restRequest = new RestRequest(Method.GET);
            EwbSession.AddApiHeaders(restRequest);
            restRequest.AddParameter("date", Date, ParameterType.QueryString);
            IRestResponse restResponse = restClient.Execute(restRequest);
            IRestResponse restResponse1 = restResponse;
            restResponse = null;
            aPITxnLogArg = APITools.ScanAPIResponse(str, restResponse1);
            bool isSuccess1 = aPITxnLogArg.IsSuccess;
            flag1 = isSuccess1;
            flag = isSuccess1;
            if (flag1)
            {
                try
                {
                    GetRespPl getRespPl = JsonConvert.DeserializeObject<GetRespPl>(restResponse1.Content);
                    decryptionError = "Request for Get Eway Bill Generated By Date is successfull!!!";
                    string str1 = TPCrypto.AesDecryptBase64(getRespPl.rek, EwbSession.EwbApiLoginDetails.EwbSEK);
                    string str2 = TPCrypto.AesDecryptData(getRespPl.data, str1);
                    respGetEwayBillsRejectedByOthers = JsonConvert.DeserializeObject<List<RespGetEwayBillsRejectedByOthers>>(str2, APITools.jsonSettings);
                    getRespPl = null;
                    str1 = null;
                    str2 = null;
                }
                catch
                {
                    flag = false;
                    decryptionError = Constants.DecryptionError;
                }
                aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
            }
            restClient = null;
            restRequest = null;
            restResponse1 = null;
        }
        else
        {
            decryptionError = txnResp1.TxnOutcome;
            aPITxnLogArg = APITools.SetAPITxnLogArgs(str, flag, decryptionError, "", "");
        }
        EwbSession.LogAPITxn(aPITxnLogArg);
        TxnRespWithObj<List<RespGetEwayBillsRejectedByOthers>> txnRespWithObj = new TxnRespWithObj<List<RespGetEwayBillsRejectedByOthers>>()
        {
            IsSuccess = flag,
            TxnOutcome = string.Concat(aPITxnLogArg.ErrCode, " ", aPITxnLogArg.OutcomeMsg),
            RespObj = respGetEwayBillsRejectedByOthers
        };
        return txnRespWithObj;
    }
  }
}
