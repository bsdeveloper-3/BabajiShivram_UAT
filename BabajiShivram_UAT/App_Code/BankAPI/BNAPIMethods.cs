using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace BankAPI.YesBank
{
    public class BNAPIMethods
    {
        public BNAPIMethods()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static BNRespGenAccessToken BankGetAuthToken(string ConsumerKey, string ConsumerSecret)
        {
            string ApiUrl = "https://api.yesdeveloper.in/oauth/client_credential/accesstoken?grant_type=client_credentials";

            BNRespGenAccessToken GeneratedToken = null;
            /**************PostMAN*********
             var client = new RestClient("https://api.yesdeveloper.in/oauth/client_credential/accesstoken?grant_type=client_credentials");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aGxlTWhzY3hQeUVPR2hCQzJEbGM2Q2FCam9sRGdJNFo6NHBVS3k1aVlJQWx1YVpXZQ==");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            **************************/
            RestClient client = new RestClient(ApiUrl);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aGxlTWhzY3hQeUVPR2hCQzJEbGM2Q2FCam9sRGdJNFo6NHBVS3k1aVlJQWx1YVpXZQ==");
            request.AlwaysMultipartFormData = true;

            IRestResponse response = client.Execute((IRestRequest)request);

            GeneratedToken = JsonConvert.DeserializeObject<BNRespGenAccessToken>(response.Content);

            client = (RestClient)null;
            request = (RestRequest)null;

            return GeneratedToken;

        }

        public static BNRespGetBalance BankGetBalance(string CustomerID, string AccoutnNo, string strBearerToken)
        {
            strBearerToken = "Bearer"+ " " + strBearerToken;
            string ApiUrl = "https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/balance?AccountNumber=" + AccoutnNo + "";

            //string ApiUrl = "https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/balance?customerID="+ CustomerID + "&AccountNumber="+ AccoutnNo + "";

            BNRespGetBalance respBalance = null;

            /**************PostMAN*********
             var client = new RestClient("https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/balance?customerID=1&AccountNumber=000000000000002");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer KFN0f0HDV9ZLXFjOonoIeskalIDe");
            request.AddParameter("text/plain", "",  ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            **************************/
            RestClient client = new RestClient(ApiUrl);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", strBearerToken);
            
            IRestResponse response = client.Execute((IRestRequest)request);

           // string strTest = response.Content;

            respBalance = JsonConvert.DeserializeObject<BNRespGetBalance>(response.Content);

            client = (RestClient)null;
            request = (RestRequest)null;

            return respBalance;

        }

        public static BNRespStartTransfer BankFundStartTransfer(string JsonPayload, string strBearerToken)
        {
            strBearerToken = "Bearer" + " " + strBearerToken;
            string ApiUrl = "https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/starttransfer";

            BNRespStartTransfer respStartTransfer = null;

            /**************PostMAN*********
            var client = new RestClient("https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/starttransfer");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer aiwL4YDdTG5iWLIGsihGvvbZ6afv");
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("text/plain", "{\"transfer\":{\"version\":\"1\",\"uniqueRequestNo\":\"001\"",  ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            **************************/
            RestClient client = new RestClient(ApiUrl);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", strBearerToken);
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("undefined", JsonPayload, ParameterType.RequestBody);

            IRestResponse response = client.Execute((IRestRequest)request);

            // string strTest = response.Content;

            respStartTransfer = JsonConvert.DeserializeObject<BNRespStartTransfer>(response.Content);

            client = (RestClient)null;
            request = (RestRequest)null;

            return respStartTransfer;

        }

        public static BNRespTransfer BankFundTransfer(string JsonPayload, string strBearerToken)
        {
            strBearerToken = "Bearer" + " " + strBearerToken;
            string ApiUrl = "https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/transfer";

            BNRespTransfer respTransfer = null;

            /**************PostMAN*********
            var client = new RestClient("https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/transfer");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer aiwL4YDdTG5iWLIGsihGvvbZ6afv");
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("text/plain", "{\"transfer\":{\"version\":\"1\",\"uniqueRequestNo\":\"001\"",  ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            **************************/
            RestClient client = new RestClient(ApiUrl);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", strBearerToken);
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("undefined", JsonPayload, ParameterType.RequestBody);

            IRestResponse response = client.Execute((IRestRequest)request);

           // string strTest = response.Content;

            respTransfer = JsonConvert.DeserializeObject<BNRespTransfer>(response.Content);

            client = (RestClient)null;
            request = (RestRequest)null;

            return respTransfer;

        }

        public static BNRespTransferStatus BankGetTransferStatus(string strRefNo, string strCustomerId, string strBearerToken)
        {
            strBearerToken = "Bearer" + " " + strBearerToken;
            string ApiUrl = "https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/status?requestReferenceNo=" + strRefNo + "&customerID=" + strCustomerId;

            BNRespTransferStatus respStstus = null;

            /**************PostMAN*********
            var client = new RestClient("https://api.yesdeveloper.in/fundstransferbycustomerservice2httpservice/status?requestReferenceNo=01&customerID=1");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer yinRihB678mjzOyxGOBo8dDwohEI");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            **************************/
            RestClient client = new RestClient(ApiUrl);
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", strBearerToken);

            IRestResponse response = client.Execute((IRestRequest)request);

            // string strTest = response.Content;

            respStstus = JsonConvert.DeserializeObject<BNRespTransferStatus>(response.Content);

            client = (RestClient)null;
            request = (RestRequest)null;

            return respStstus;

        }

    }
}