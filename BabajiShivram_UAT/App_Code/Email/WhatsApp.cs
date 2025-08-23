using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
/// <summary>
/// Summary description for WhatsApp
/// </summary>
public class WhatsApp
{
    public WhatsApp()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string SendMessage(string strTO, string strCUSTOM_UID, string strText)
    {
        //strTO = "+919833708840";
        //string strToken = "token=4fa399f1be3487b702701073feccb9455aea7c3a7b550";
        string strToken = "token=82a78de37a3934eec506109fe339ca2a5b59769723e90";
        string strUID = "uid=912266485600";
        strTO = "to=" + strTO;
        strCUSTOM_UID = "custom_uid=" + strCUSTOM_UID;

        strText = "text=" + strText;

        string strMessageBody = strToken + "&" + strUID + "&" + strTO + "&" + strCUSTOM_UID + "&" + strText;

        var client = new RestSharp.RestClient("https://www.waboxapp.com/api/send/chat");

        //var client = new RestClient("https://www.waboxapp.com/api/send/chat");
        var request = new RestRequest(Method.POST);
        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        request.AddParameter("application/x-www-form-urlencoded", strMessageBody, ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);

        string content = response.StatusDescription.ToString(); //response.Content;

        return content.ToString(); 
    }
}