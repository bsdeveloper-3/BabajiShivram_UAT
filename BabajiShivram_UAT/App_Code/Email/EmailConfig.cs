using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Text;
using System.IO;
/// <summary>
/// Summary description for EmailConfig
/// </summary>
public class EmailConfig
{
    public EmailConfig()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetSMTPIP()
    {
        string SmtpIP;
        try
        {
            SmtpIP = WebConfigurationManager.AppSettings["EmailIP"];
        }
        catch
        {
            throw new Exception("Smtp IP not defined!");
        }
        return SmtpIP;
    }
    
    public static int GetSMTPPort()
    {
        int SmtpPort;
        try
        {
            SmtpPort =Convert.ToInt32(WebConfigurationManager.AppSettings["SMTPPort"]);
        }
        catch
        {
            throw new Exception("SMTPPort not defined!");
        }
        return SmtpPort;
    }

    public static string GetEmailFrom()
    {
        string strEmailFrom;
        try
        {
            strEmailFrom = WebConfigurationManager.AppSettings["EmailFrom"];
        }
        catch
        {
            throw new Exception("EmailFrom not defined!");
        }
        return strEmailFrom;
    }
    public static string GetEmailFromEBill()
    {
        string strEmailFrom;
        try
        {
            strEmailFrom = WebConfigurationManager.AppSettings["EmailFromEBill"];
        }
        catch
        {
            throw new Exception("EmailFromEBill not defined!");
        }
        return strEmailFrom;
    }
    public static string GetEmailPassCode()
    {
        string strPassCode;
        try
        {
            strPassCode = WebConfigurationManager.AppSettings["EmailPassCode"];
        }
        catch
        {
            throw new Exception("EmailPassCode not defined!");
        }
        return strPassCode;
    }

    public static string GetEmailCCTo()
    {
        string strEmailCCTo="";
        try
        {
            strEmailCCTo = WebConfigurationManager.AppSettings["EmailCCTo"];
        }
        catch
        {
            throw new Exception("EmailCCTo not defined!");
        }
        return strEmailCCTo;
    }

    public static string GetEmailFreightCCTo()
    {
        string strEmailFreightCCTo = "";
        try
        {
            strEmailFreightCCTo = WebConfigurationManager.AppSettings["EmailFreightCCTo"];
        }
        catch
        {
            throw new Exception("EmailFreightCCTo not defined!");
        }
        return strEmailFreightCCTo;
    }
    public static string GetEmailAccountCCTo()
    {
        string strEmailFreightCCTo = "";
        try
        {
            strEmailFreightCCTo = WebConfigurationManager.AppSettings["EmailAccountCCTo"];
        }
        catch
        {
            throw new Exception("EmailAccountCCTo not defined!");
        }
        return strEmailFreightCCTo;
    }
    public static string GetEmailBCCTo()
    {
        string strEmailBCCTo;
        try
        {
            strEmailBCCTo = WebConfigurationManager.AppSettings["EmailBCCTo"];
        }
        catch
        {
            throw new Exception("EmailBCCTo not defined!");
        }
        return strEmailBCCTo;
    }

    public static string GetErrorEmailTo()
    {
        string strErrorEmailTo;
        try
        {
            strErrorEmailTo = WebConfigurationManager.AppSettings["ErrorEmailTo"];
        }
        catch
        {
            throw new Exception("ErrorEmailTo not defined!");
        }
        return strErrorEmailTo;
    }

    public static bool GetEmailSSL()
    {
        bool bSSL;
        try
        {
            bSSL = Convert.ToBoolean(WebConfigurationManager.AppSettings["EmailSSL"]);
        }
        catch
        {
            throw new Exception("EmailSSL not defined");
        }
        return bSSL;
    }

    public static string GetSMSApiKey()
    {
        string strSMSAPIKey;
        try
        {
            strSMSAPIKey = WebConfigurationManager.AppSettings["SMSApiKey"];
        }
        catch
        {
            throw new Exception("SMS API Key Not Defined!");
        }
        return strSMSAPIKey;
    }

    public static string GetSMSUserName()
    {
        string strSMSUserName;
        try
        {
            strSMSUserName = WebConfigurationManager.AppSettings["SMSUserName"];
        }
        catch
        {
            throw new Exception("strSMSUserName not defined!");
        }
        return strSMSUserName;
    }
    
    public static string GetSMSPassCode()
    {
        string strSMSPassCode;
        try
        {
            strSMSPassCode = WebConfigurationManager.AppSettings["SMSPassCode"];
        }
        catch
        {
            throw new Exception("SMSPassCode not defined!");
        }
        return strSMSPassCode;
    }
    /* 
     * This Function can be used to parse the text to html TEXT TO HTML 
     * and  html to text. 
     * CAN BE USED AS postmess.Text = parsetext(txtTempBody.Text, true);
     * TRUE VALUE IS TO PARSE IT HTML
     */ 
    public string parsetext(string text, bool allow)
    {
        //Create a StringBuilder object from the string intput
        //parameter
        StringBuilder sb = new StringBuilder(text);
        //Replace all double white spaces with a single white space
        //and &nbsp;
        sb.Replace(" ", " &nbsp;");
        //Check if HTML tags are not allowed
        if (!allow)
        {
            //Convert the brackets into HTML equivalents
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            //Convert the double quote
            sb.Replace("\"", "&quot;");
        }
        //Create a StringReader from the processed string of 
        //the StringBuilder object
        StringReader sr = new StringReader(sb.ToString());
        StringWriter sw = new StringWriter();
        //Loop while next character exists
        while (sr.Peek() > -1)
        {
            //Read a line from the string and store it to a temp
            //variable
            string temp = sr.ReadLine();
            //write the string with the HTML break tag
            //Note here write method writes to a Internal StringBuilder
            //object created automatically
            sw.Write(temp + "<br>");
        }
        //Return the final processed text
       
        return sw.GetStringBuilder().ToString();
    }
     
}
