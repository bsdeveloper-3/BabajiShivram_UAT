using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.IO;

    /// <summary>
    /// This base class provide implementation of request
    /// and response method during HTTPS calls.
    /// </summary>
    public class HTTPSBaseClass
    {
        private string Request;

        public HTTPSBaseClass()
        { 
        }
        public HTTPSBaseClass(string HttpsRequest)
        {
            Request = HttpsRequest;
        }

        public HttpWebRequest CreateWebRequest(string uri, CookieContainer CC, string RequestMethod, 
                                    NameValueCollection RequestHeader,bool IsNew, string Referral)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);

            webRequest.KeepAlive = true;
            webRequest.AllowAutoRedirect = false;
            webRequest.Method = RequestMethod;
            webRequest.Headers.Add(RequestHeader);
            webRequest.CookieContainer = CC;
          //  webRequest.Referer = Referral;
            webRequest.ContentType = "application/json";
          //  webRequest.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
          //  webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
          //  webRequest.UserAgent ="Mozilla/5.0 (Windows NT 6.1; WOW64; rv:24.0) Gecko/20170104 Firefox/24.0";;

            //if (webRequest.Method == "POST")
            //{
            //    webRequest.ContentType = "application/x-www-form-urlencoded";
            //}
            
            return webRequest;
        }


        /// <summary>

        /// This method retreives redirected URL from

        /// response header and also passes back

        /// any cookie (if there is any)

        /// </summary>

        /// <param name="webresponse"></param>

        /// <param name="Cookie"></param>

        /// <returns></returns>

        public virtual string GetRedirectURL(HttpWebResponse
             webresponse, ref CookieContainer CookieCont)
        {
            string uri = "";

            CookieCont.Add(webresponse.Cookies);

            WebHeaderCollection headers = webresponse.Headers;

            if ((webresponse.StatusCode == HttpStatusCode.Found) ||
              (webresponse.StatusCode == HttpStatusCode.Redirect) ||
              (webresponse.StatusCode == HttpStatusCode.Moved) ||
              (webresponse.StatusCode == HttpStatusCode.MovedPermanently))
            {
                // Get redirected uri

                uri = headers["Location"];
                uri = uri.Trim();
            }

            string StartURI = "";

            if (uri.Length > 0 && uri.StartsWith(StartURI) == false)
            {
                uri = StartURI + uri;
            }

            return uri;
        }//End of GetRedirectURL method

        public virtual string GetFinalResponse(string ReUri,
         CookieContainer CC, string RequestMethod, bool NwCred,string Referral)
        {
            NameValueCollection RequestHeader2 = new NameValueCollection();
            HttpWebRequest webrequest =
              CreateWebRequest(ReUri, CC, RequestMethod,RequestHeader2, NwCred, Referral);

            //  BuildReqStream(ref webrequest);

            HttpWebResponse webresponse;

            webresponse = (HttpWebResponse)webrequest.GetResponse();

            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new
              StreamReader(webresponse.GetResponseStream(), enc);

            string Response = loResponseStream.ReadToEnd();

            loResponseStream.Close();
            webresponse.Close();

            return Response;
        }

        // This method build the request stream for WebRequest
        private void BuildRequestStream(ref HttpWebRequest webRequest)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Request);
            webRequest.ContentLength = bytes.Length;
            Stream oStreamOut = webRequest.GetRequestStream();
            oStreamOut.Write(bytes,0,bytes.Length);
            oStreamOut.Close();
        }
    }

