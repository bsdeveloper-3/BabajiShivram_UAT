using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyPacco.API
{
    /// <summary>
    /// Summary description for MYPACCOAPI
    /// </summary>
    public static class MYPACCOAPI
    {
        public static MyPaccoRespAccessToken GetAuthTokenAsync(MyPaccoSession EwbSession, string Payload)
        {

            /***************************************
            var client1 = new RestClient("http://api.mypacco.com/api/getAccessToken");
            client1.Timeout = -1;
            var request1 = new RestRequest(Method.POST);
            request1.AddHeader("Content-Type", "application/json");
            // request1.AddHeader("Content-Type", "application/json");
            // request1.AddHeader("Cookie", "PHPSESSID=50640999e24682615583d0c952244b40; ci_session=a%3A5%3A%7Bs%3A10%3A%22session_id%22%3Bs%3A32%3A%2296b78430caed1bd912f17b1d13762ca3%22%3Bs%3A10%3A%22ip_address%22%3Bs%3A14%3A%22124.30.114.114%22%3Bs%3A10%3A%22user_agent%22%3Bs%3A21%3A%22PostmanRuntime%2F7.26.5%22%3Bs%3A13%3A%22last_activity%22%3Bi%3A1603678944%3Bs%3A9%3A%22user_data%22%3Bs%3A0%3A%22%22%3B%7D5ed2dd3f5a20ff8b8d056fe07d2c809dc6ec9a1d");
            request1.AddParameter("application/json", "{\"access_token\":\"\",\"data\":[{\"client_id\":\"9702420066\",\"client_secret\":\"Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA\"}]}", ParameterType.RequestBody);
           // request1.AddParameter("undefined", Payload, ParameterType.RequestBody);
            IRestResponse response1 = client1.Execute(request1);
            string strtest= response1.Content;

            **************************************/

            bool isSuccess = false;
            MyPaccoRespAccessToken objMyPaccoRespAccessToken = new MyPaccoRespAccessToken();
            objMyPaccoRespAccessToken.IsSuccess = false;
                        
            RestClient client = new RestClient(EwbSession.MyPaccoApiSetting.MyPaccoBaseUrl + "getAccessToken");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            //request.AddParameter("application/json", Payload, ParameterType.RequestBody);

            //request.AddParameter("undefined", @"{""access_token"":"""",""data"": [{""client_id"":""9702420066"",""client_secret"": ""Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA""}]}", ParameterType.RequestBody);

            request.AddParameter("application/json", Payload, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute((IRestRequest)request);
            IRestResponse response = restResponse;

              //  restResponse = (IRestResponse)null;
                if (!response.Content.Contains("Error") && response.Content != "")
                {
                    try
                    {
                        objMyPaccoRespAccessToken = JsonConvert.DeserializeObject<MyPaccoRespAccessToken>(response.Content);
                        EwbSession.MyPaccoApiLoginDetails.MyPaccoTokenExp = DateTime.Now.AddSeconds((double)MyPaccoConstants.AuthTokenValidityMin);
                        EwbSession.MyPaccoApiLoginDetails.MyPaccoAuthToken = objMyPaccoRespAccessToken.Data.access_token;
                    }
                    catch
                    {
                        objMyPaccoRespAccessToken.IsSuccess = false;
                        isSuccess = false;
                    }
                }
                
                client = (RestClient)null;
                request = (RestRequest)null;
                response = (IRestResponse)null;

            return objMyPaccoRespAccessToken;
        }

        public static MyPaccoRespAddOrder GenAWBAsync(MyPaccoSession EwbSession, string JsonPayload)
        {
            bool isSuccess = false;

            MyPaccoRespAddOrder objMyPaccoRespAddOrder = new MyPaccoRespAddOrder();
            objMyPaccoRespAddOrder.IsSuccess = false;

            RestClient client = new RestClient(EwbSession.MyPaccoApiSetting.MyPaccoBaseUrl + "addOrder");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonPayload, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute((IRestRequest)request);
            IRestResponse response = restResponse;

            if (!response.Content.Contains("Error") && response.Content != "")
            {
                try
                {
                    objMyPaccoRespAddOrder = JsonConvert.DeserializeObject<MyPaccoRespAddOrder>(response.Content);
                }
                catch
                {
                    objMyPaccoRespAddOrder.IsSuccess = false;
                    isSuccess = false;
                }
            }
            else if (response.Content != "")
            {
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                string errorMsg = "";

                foreach (var item in json)
                {
                    if (item.Key.ToLower() == "message")
                    {
                        errorMsg = errorMsg + item.Value.ToString() + "<BR>";
                    }
                    if (item.Key.ToLower() == "error")
                    {
                        errorMsg = errorMsg + item.Value.ToString() + "<BR>";
                    }
                }

                objMyPaccoRespAddOrder.Message = errorMsg;
            }

            client = (RestClient)null;
            request = (RestRequest)null;
            response = (IRestResponse)null;

            return objMyPaccoRespAddOrder;
        }

        public static void PrintAWBAsync(MyPaccoSession EwbSession, string JsonPayload)
        {
            bool isSuccess = false;
            JsonPayload = "{\"access_token\": \"QugkATszZkRA516oCSsUhqQa2YsXnJO24TcrCsRr\",\"data\": [{\"orders\": [\"MP0271520\"]}]}";

            MyPaccoRespAddOrder objMyPaccoRespAddOrder = new MyPaccoRespAddOrder();
            objMyPaccoRespAddOrder.IsSuccess = false;

            RestClient client = new RestClient(EwbSession.MyPaccoApiSetting.MyPaccoBaseUrl + "getDocs");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonPayload, ParameterType.RequestBody);

            client.DownloadData(request);

          //  IRestResponse restResponse = client.Execute((IRestRequest)request);
          //  IRestResponse response = restResponse;

            //if (!response.Content.Contains("Error") && response.Content != "")
            //{
            //    string strCon = response.Content;
            //}

            //client = (RestClient)null;
            //request = (RestRequest)null;
            //response = (IRestResponse)null;

        }

        public static MyPaccoRespTrackOrder TrackAWBAsync(MyPaccoSession EwbSession, string JsonPayload)
        {
            MyPaccoRespTrackOrder objMyPaccoRespTrackOrder = new MyPaccoRespTrackOrder();
            objMyPaccoRespTrackOrder.IsSuccess = false;

            RestClient client = new RestClient(EwbSession.MyPaccoApiSetting.MyPaccoBaseUrl + "/trackOrder");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonPayload, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute((IRestRequest)request);
            IRestResponse response = restResponse;

            if (response.Content.Contains("AWBNumber") && response.Content != "")
            {
                try
                {
                    objMyPaccoRespTrackOrder = JsonConvert.DeserializeObject<MyPaccoRespTrackOrder>(response.Content);
                }
                catch
                {
                    objMyPaccoRespTrackOrder.IsSuccess = false;
                }
            }

            client = (RestClient)null;
            request = (RestRequest)null;
            response = (IRestResponse)null;

            return objMyPaccoRespTrackOrder;
        }

    }
}