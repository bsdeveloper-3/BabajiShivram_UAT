using System;
using System.Web.Configuration;
using System.Text;
using System.IO;
/// <summary>
/// Summary description for EmailConfig
/// </summary>
public class EmailCircularConfig
{
    public EmailCircularConfig()
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
            SmtpIP = WebConfigurationManager.AppSettings["CircularEmailIP"];
        }
        catch
        {
            throw new Exception("Circular Smtp IP not defined!");
        }
        return SmtpIP;
    }
    
    public static int GetSMTPPort()
    {
        int SmtpPort;
        try
        {
            SmtpPort =Convert.ToInt32(WebConfigurationManager.AppSettings["CircularSMTPPort"]);
        }
        catch
        {
            throw new Exception("Circular SMTPPort not defined!");
        }
        return SmtpPort;
    }

    public static string GetEmailFrom()
    {
        string strEmailFrom;
        try
        {
            strEmailFrom = WebConfigurationManager.AppSettings["CircularEmailFrom"];
        }
        catch
        {
            throw new Exception("Circular EmailFrom not defined!");
        }
        return strEmailFrom;
    }

    public static string GetEmailUser()
    {
        string strEmailFrom;
        try
        {
            strEmailFrom = WebConfigurationManager.AppSettings["CircularEmailUser"];
        }
        catch
        {
            throw new Exception("Circular EmailFrom not defined!");
        }
        return strEmailFrom;
    }

    public static string GetEmailPassCode()
    {
        string strPassCode;
        try
        {
            strPassCode = WebConfigurationManager.AppSettings["CircularEmailPassCode"];
        }
        catch
        {
            throw new Exception("Circular EmailPassCode not defined!");
        }
        return strPassCode;
    }

    public static string GetEmailCCTo()
    {
        string strEmailCCTo="";
        try
        {
            strEmailCCTo = WebConfigurationManager.AppSettings["CircularEmailCCTo"];
        }
        catch
        {
            throw new Exception("Circular EmailCCTo not defined!");
        }
        return strEmailCCTo;
    }
        
    public static string GetEmailBCCTo()
    {
        string strEmailBCCTo;
        try
        {
            strEmailBCCTo = WebConfigurationManager.AppSettings["CircularEmailBCCTo"];
        }
        catch
        {
            throw new Exception("Circular EmailBCCTo not defined!");
        }
        return strEmailBCCTo;
    }

    public static string GetErrorEmailTo()
    {
        string strErrorEmailTo;
        try
        {
            strErrorEmailTo = WebConfigurationManager.AppSettings["CircularErrorEmailTo"];
        }
        catch
        {
            throw new Exception("Circular ErrorEmailTo not defined!");
        }
        return strErrorEmailTo;
    }

    public static bool GetEmailSSL()
    {
        bool bSSL;
        try
        {
            bSSL = Convert.ToBoolean(WebConfigurationManager.AppSettings["CircularEmailSSL"]);
        }
        catch
        {
            throw new Exception("CircularEmailSSL not defined");
        }
        return bSSL;
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
