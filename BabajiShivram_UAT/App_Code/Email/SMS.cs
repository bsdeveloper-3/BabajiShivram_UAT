using System;
using System.Net;
/// <summary>
/// Summary description for SMS
/// </summary>
public class SMS
{
	public SMS()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static bool SendSMS(string strMessage,string strMobileNo)
    {
        LoginClass loggedInUser = new LoginClass();
        bool bSuccess = false; String strURI = "";
        string strLogMessage = "";
                        
        try
        {
        
            string strSMSKey    =   EmailConfig.GetSMSApiKey();
            

            //Transactional Route API - Babaji
            strURI = "http://login.smsgatewayhub.com/smsapi/pushsms.aspx?apikey=" + strSMSKey + "&fl=0&gwid=2&sid=BSLive&to=" + strMobileNo + "&msg=" + strMessage + "";

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(strURI);

            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Timeout = System.Threading.Timeout.Infinite;
            webRequest.KeepAlive = true;

            HttpWebResponse webresponse = (HttpWebResponse)webRequest.GetResponse();

            // Status Code 200 - OK
            if (webresponse.StatusCode == HttpStatusCode.OK)
            {
                bSuccess = true;
                webresponse.Close();
                
                return bSuccess;
            }
            else
            {
                bSuccess = false;
                webresponse.Close();
                return bSuccess;
            }
            
            //END_IF

            // User Name and Password Not Required, Key Used for Authenticate
            // string strUserName  =   EmailConfig.GetSMSUserName();
            // string strUserPassCode = EmailConfig.GetSMSPassCode();
            
            //Promotional Route API
            //string strURI = "http://login.smsgatewayhub.com/smsapi/pushsms.aspx?user=" + strUserName + "&pwd=" + strUserPassCode + "&to=" + strMobileNo + "&sid=BSLive&msg=" + strMessage + "&fl=0";
        
        }
        catch (Exception Ex)
        {

            try
            {
                HttpWebRequest webRequest2 = (HttpWebRequest)HttpWebRequest.Create(strURI);

                webRequest2.Method = "GET";
                webRequest2.ContentType = "application/x-www-form-urlencoded";
                webRequest2.Timeout = 2500;
                HttpWebResponse webresponse2 = (HttpWebResponse)webRequest2.GetResponse();

                // Status Code 200 - OK
                if (webresponse2.StatusCode == HttpStatusCode.OK)
                {
                    bSuccess = true;
                    webresponse2.Close();
                    return bSuccess;
                }
                else
                {
                    bSuccess = false;
                    webresponse2.Close();
                    return bSuccess;
                }
            }
            catch (Exception Ex2)
            {
                strLogMessage = "SMS Second Time Sending Failed." + "To:" + strMobileNo + " Message:" + strMessage + " URL:" + strURI;
                ErrorLog.LogToDatabase(0, "SMS.cs", "", strLogMessage, Ex2, "", loggedInUser.glUserId);
                ErrorLog.LogToTextFile("SMS.cs" + strLogMessage, Ex);

                return bSuccess;
            }
            
            return bSuccess;

            
        }
    }
}
