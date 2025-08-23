using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using OdexConnect.API.Methods;
using OdexConnect.API.Model;

namespace OdexConnect.API.Methods
{
    public class OdexAPI
    {
        //public static TxnRespWithObj<OdexSession> GetMasterDate(OdexSession odexSession)
        //{
        //    bool isSucess = false;
        //    string ApiAction = "getMasterDtls";

        //    if (odexSession == null)
        //    {
        //        return new TxnRespWithObj<OdexSession>()
        //        {
        //            isSuccess = false,
        //            TxnOutcome = "Login Session Expired!"
        //        };
        //    }
        //    else
        //        return new TxnRespWithObj<OdexSession>()
        //        {
        //            isSuccess = true,
        //            TxnOutcome = "Login Sucess!"
        //        };
        //}

        public static string ODexInvoiceRequest(string jsonPayLoad, ref ResInvoiceSuccess objResponse)
        {
            try
            {
                //string ApiUrl = "https://staging.odexglobal.com:8443/RS/iEDOService/json/saveInvReqDtls";

                //string ApiUrl = "https://api.odexglobal.com/RS/iEDOService/json/saveInvReqDtls";
                string ApiUrl = "https://api.odexglobal.com/RS/iEDOService/json/saveInvReqDtls";

                RestClient client = new RestClient(ApiUrl);
                client.Timeout = -1;
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", jsonPayLoad, ParameterType.RequestBody);
                IRestResponse response = client.Execute((IRestRequest)request);
                // string strTest = response.Content;

                client = (RestClient)null;
                request = (RestRequest)null;

                if (response.Content.ToString().Trim() == "")
                {
                    return response.ErrorException.Message;
                }
                else
                {
                    objResponse = JsonConvert.DeserializeObject<ResInvoiceSuccess>(response.Content);
                    return response.Content;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        public static string ODexDORequest(string jsonPayLoad, ref ResDOSuccess objResponse)
        {
            try
            {
                //string ApiUrl = "https://staging.odexglobal.com:8443/RS/iEDOService/json/saveDOReqDtls";

                //string ApiUrl = "https://api.odexglobal.com/RS/iEDOService/json/saveDOReqDtls";
                string ApiUrl = "https://api.odexglobal.com/RS/iEDOService/json/saveDOReqDtls";

                RestClient client = new RestClient(ApiUrl);
                client.Timeout = -1;
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", jsonPayLoad, ParameterType.RequestBody);
                IRestResponse response = client.Execute((IRestRequest)request);
                // string strTest = response.Content;

                client = (RestClient)null;
                request = (RestRequest)null;

                if (response.Content.ToString().Trim() == "")
                {
                    return response.ErrorException.Message;
                }
                else
                {
                    objResponse = JsonConvert.DeserializeObject<ResDOSuccess>(response.Content);
                    return response.Content;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
    }
}
