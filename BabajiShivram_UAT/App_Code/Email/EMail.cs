using System;
using System.Web;
using System.Net.Mail;
using System.Collections.Generic;

/// <summary>
/// Summary description for SendMail
/// </summary>
public class EMail
{
    public EMail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool SendMail(string ReplyToEmail, string ToEmail, string Subject, string MailBody, string strAttachFilePath)
    {  
        // string SenderEmail = "Admin@BabajiShivram.com";

        bool bSuccess = false;

        LoginClass loggedInUser = new LoginClass();
        try
        {
            string SenderEmail  = EmailConfig.GetEmailFrom();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();

            string strEmailCC   = EmailConfig.GetEmailCCTo();
            string strEmailBCC  = EmailConfig.GetEmailBCCTo();

            string strSMTP      = EmailConfig.GetSMTPIP();
            int intSMTPort      = EmailConfig.GetSMTPPort();
            bool SSLRequired    = EmailConfig.GetEmailSSL();

            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }
            
            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                msg.IsBodyHtml = true;

                if (strAttachFilePath.Trim() != String.Empty)
                {
                    /* Create the Email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + strAttachFilePath);
                    }
                    else
                    {
                        ServerPath = ServerPath + strAttachFilePath;
                    }

                    if (System.IO.File.Exists(ServerPath))
                    {
                        //Attachment attach = new Attachment(ServerPath);
                                                
                        /* Attach the newly created email attachment */
                        //msg.Attachments.Add(attach);
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmail, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                //MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                    bSuccess = true;
                }
                catch (System.Exception Ex)
                {
                    string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send:" + strLogMessage, Ex);
                }

            }//END_USING


        }//END_Try
        catch (System.Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage:" + strLogMessage, Ex);
        }

        return bSuccess;
    }

    public static bool SendMail2(string ReplyToEmail, string ToEmail, string Subject, string MailBody, string strAttachFilePath)
    {
        // string SenderEmail = "Admin@BabajiShivram.com";

        bool bSuccess = false;

        LoginClass loggedInUser = new LoginClass();
        try
        {
            string SenderEmail = EmailConfig.GetEmailFrom();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();

            string strEmailCC = EmailConfig.GetEmailCCTo();
        
            string strSMTP = EmailConfig.GetSMTPIP();
            int intSMTPort = EmailConfig.GetSMTPPort();
            bool SSLRequired = EmailConfig.GetEmailSSL();

            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }

            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
              
                msg.IsBodyHtml = true;

                if (strAttachFilePath.Trim() != String.Empty)
                {
                    /* Create the Email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + strAttachFilePath);
                    }
                    else
                    {
                        ServerPath = ServerPath + strAttachFilePath;
                    }

                    if (System.IO.File.Exists(ServerPath))
                    {
                        //Attachment attach = new Attachment(ServerPath);

                        /* Attach the newly created email attachment */
                        //msg.Attachments.Add(attach);
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmail, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                //MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                    bSuccess = true;
                }
                catch (System.Exception Ex)
                {
                    string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send:" + strLogMessage, Ex);
                }

            }//END_USING


        }//END_Try
        catch (System.Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage:" + strLogMessage, Ex);
        }

        return bSuccess;
    }

    public static bool SendMailCC(string ReplyToEmail, string ToEmail, string ToEmailCC, string Subject, string MailBody, string strAttachFilePath)
    {
        string strEmailCC = ToEmailCC; string strEmailBCC = "";

        bool bSuccess = false;

        LoginClass loggedInUser = new LoginClass();
        try
        {
            /*** Alert@Babaji Email Setting *****
            string SenderEmail = EmailCircularConfig.GetEmailFrom();
            string SenderEmailUser = EmailCircularConfig.GetEmailUser();
            string SenderEmailPass = EmailCircularConfig.GetEmailPassCode();
            strEmailBCC  = EmailConfig.GetEmailBCCTo();
             * 
            string strSMTP = EmailCircularConfig.GetSMTPIP();
            int intSMTPort = EmailCircularConfig.GetSMTPPort();
            bool SSLRequired = EmailCircularConfig.GetEmailSSL();

           *** END **************************************/

            /***** Admin@Babaji Email Setting ******/
            string SenderEmail = EmailConfig.GetEmailFrom();
            string SenderEmailUser = EmailConfig.GetEmailFrom();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();
            strEmailBCC = EmailConfig.GetEmailBCCTo();

            string strSMTP = EmailConfig.GetSMTPIP();
            int intSMTPort = EmailConfig.GetSMTPPort();
            bool SSLRequired = EmailConfig.GetEmailSSL();

            /*********END***********************/

            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }

            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                msg.IsBodyHtml = true;

                if (strAttachFilePath.Trim() != String.Empty)
                {
                    /* Create the Email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + strAttachFilePath);
                    }
                    else
                    {
                        ServerPath = ServerPath + strAttachFilePath;
                    }

                    if (System.IO.File.Exists(ServerPath))
                    {
                        //Attachment attach = new Attachment(ServerPath);

                        /* Attach the newly created email attachment */
                        //msg.Attachments.Add(attach);
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmailUser, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                //MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                    bSuccess = true;
                }
                catch (System.Exception Ex)
                {
                    string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send:" + strLogMessage, Ex);
                }

            }//END_USING


        }//END_Try
        catch (System.Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage:" + strLogMessage, Ex);
        }

        return bSuccess;
    }

    public static bool SendMailAlert(string ReplyToEmail, string ToEmail, string Subject, string MailBody, string strAttachFilePath)
    {
        // string SenderEmail = "Admin@BabajiShivram.com";

        bool bSuccess = false;

        LoginClass loggedInUser = new LoginClass();
        try
        {
            string SenderEmail = EmailCircularConfig.GetEmailFrom();
            string SenderEmailUser = EmailCircularConfig.GetEmailUser();
            string SenderEmailPass = EmailCircularConfig.GetEmailPassCode();

            string strEmailCC = EmailCircularConfig.GetEmailCCTo();
            string strEmailBCC = EmailCircularConfig.GetEmailBCCTo();

            string strSMTP = EmailCircularConfig.GetSMTPIP();
            int intSMTPort = EmailCircularConfig.GetSMTPPort();
            bool SSLRequired = EmailCircularConfig.GetEmailSSL();


            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }

            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                msg.IsBodyHtml = true;

                if (strAttachFilePath.Trim() != String.Empty)
                {
                    /* Create the Email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + strAttachFilePath);
                    }
                    else
                    {
                        ServerPath = ServerPath + strAttachFilePath;
                    }

                    if (System.IO.File.Exists(ServerPath))
                    {
                        //Attachment attach = new Attachment(ServerPath);

                        /* Attach the newly created email attachment */
                        //msg.Attachments.Add(attach);
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmail, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                //MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                    bSuccess = true;
                }
                catch (System.Exception Ex)
                {
                    string strLogMessage = "Alert Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send:" + strLogMessage, Ex);
                }

            }//END_USING


        }//END_Try
        catch (System.Exception Ex)
        {
            string strLogMessage = "Alert Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage:" + strLogMessage, Ex);
        }

        return bSuccess;
    }

    public static bool SendMailBCC(string ReplyToEmail, string ToEmail, string CCEmail, string BCCEmail, string Subject, string MailBody, string strAttachFilePath)
    {
        // string SenderEmail = "Admin@BabajiShivram.com";

        bool bSuccess = false;

        LoginClass loggedInUser = new LoginClass();
        try
        {
            string SenderEmail = EmailConfig.GetEmailFrom();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();

            string strEmailCC = EmailConfig.GetEmailCCTo();
            string strEmailBCC = EmailConfig.GetEmailBCCTo();

            string strSMTP = EmailConfig.GetSMTPIP();
            int intSMTPort = EmailConfig.GetSMTPPort();
            bool SSLRequired = EmailConfig.GetEmailSSL();

            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }

            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }

                if (CCEmail != "")
                {
                    msg.CC.Add(CCEmail);
                }

                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                if (BCCEmail != "")
                {
                    msg.Bcc.Add(BCCEmail);
                }


                msg.IsBodyHtml = true;

                if (strAttachFilePath.Trim() != String.Empty)
                {
                    /* Create the Email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + strAttachFilePath);
                    }
                    else
                    {
                        ServerPath = ServerPath + strAttachFilePath;
                    }

                    if (System.IO.File.Exists(ServerPath))
                    {
                        Attachment attach = new Attachment(ServerPath);

                        /* Attach the newly created email attachment */
                        msg.Attachments.Add(attach);
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmail, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                //MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                    bSuccess = true;
                }
                catch (System.Exception Ex)
                {
                    string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send:" + strLogMessage, Ex);
                }

            }//END_USING


        }//END_Try
        catch (System.Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage:" + strLogMessage, Ex);
        }

        return bSuccess;
    }

    public static bool SendMailMultiAttach(string ReplyToEmail, string ToEmail, string CCEmail, string Subject, string MailBody, List <string> ListFilePath)
    {
        LoginClass loggedInUser = new LoginClass();

        bool bSuccess = true;

        try
        {
            string strEmailCC = ""; string strEmailBCC = "";

            string SenderEmail = EmailConfig.GetEmailFrom();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();

            if (loggedInUser.glModuleId == 2)// Freight CC
            {
                strEmailCC = EmailConfig.GetEmailFreightCCTo();
            }
            else
            {
                strEmailCC = EmailConfig.GetEmailCCTo();
            }
            
            strEmailBCC = EmailConfig.GetEmailBCCTo();

            string strSMTP = EmailConfig.GetSMTPIP();
            int intSMTPort = EmailConfig.GetSMTPPort();
            bool SSLRequired = EmailConfig.GetEmailSSL();

            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }
            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

               // msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                
              //  msg.From = new MailAddress(loggedInUser.glUserName, "Freight Care Babaji Shivram");

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
                if (CCEmail != "")
                {
                    msg.CC.Add(CCEmail);
                }
                
                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                msg.IsBodyHtml = true;

                if (ListFilePath.Count > 0)
                {
                    /* Create the email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\");
                    }

                    foreach (string strFilePath in ListFilePath)
                    {
                        string strAttchPath = ServerPath + strFilePath;

                        if (System.IO.File.Exists(strAttchPath))
                        {
                            Attachment attach = new Attachment(strAttchPath);

                            /* Attach the newly created email attachment */
                            msg.Attachments.Add(attach);
                        }
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmail, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                }
                catch (Exception Ex)
                {
                    string strLogMessage = "Email MultiAttach Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send - MultiAttach", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send: - MultiAttach" + strLogMessage, Ex);

                    bSuccess = false;
                }

            }//END_USING


        }//END_Try
        catch (Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage - MultiAttach", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage: - MultiAttach" + strLogMessage, Ex);

            bSuccess = false;
        }

        return bSuccess; 

    }

    public static bool SendMailMultiAttach2(string ReplyToEmail, string ToEmail, string CCEmail, string Subject, string MailBody, List<string> ListFilePath)
    {
        LoginClass loggedInUser = new LoginClass();

        bool bSuccess = true;

        try
        {
            string strEmailCC = ""; string strEmailBCC = "";

            /*** Alert@Babaji Email Setting **************
            string SenderEmail = EmailCircularConfig.GetEmailFrom();
            string SenderEmailUser = EmailCircularConfig.GetEmailUser();
            string SenderEmailPass = EmailCircularConfig.GetEmailPassCode();

            string strSMTP = EmailCircularConfig.GetSMTPIP();
            int intSMTPort = EmailCircularConfig.GetSMTPPort();
            bool SSLRequired = EmailCircularConfig.GetEmailSSL();

            strEmailBCC = EmailConfig.GetEmailBCCTo();
            *** END **************************************/

            /***** Admin@Babaji Email Setting ***********/
            
            string SenderEmail = EmailConfig.GetEmailFrom();
            string SenderEmailUser = EmailConfig.GetEmailFrom();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();
            
            string strSMTP = EmailConfig.GetSMTPIP();
            int intSMTPort = EmailConfig.GetSMTPPort();
            bool SSLRequired = EmailConfig.GetEmailSSL();

            /*****END**************************************/

            if (loggedInUser.glModuleId == 2)// Freight CC
            {
                strEmailCC = EmailConfig.GetEmailFreightCCTo();
            }
            else
            {
                strEmailCC = EmailConfig.GetEmailCCTo();
            }
                        
            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }
            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                // msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;

                //  msg.From = new MailAddress(loggedInUser.glUserName, "Freight Care Babaji Shivram");

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
                if (CCEmail != "")
                {
                    msg.CC.Add(CCEmail);
                }
                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                msg.IsBodyHtml = true;

                if (ListFilePath.Count > 0)
                {
                    /* Create the email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\");
                    }

                    foreach (string strFilePath in ListFilePath)
                    {
                        string strAttchPath = ServerPath + strFilePath;

                        if (System.IO.File.Exists(strAttchPath))
                        {
                            Attachment attach = new Attachment(strAttchPath);

                            /* Attach the newly created email attachment */
                            msg.Attachments.Add(attach);
                        }
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmailUser, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                }
                catch (Exception Ex)
                {
                    string strLogMessage = "Email MultiAttach BCC Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send - MultiAttachBCC", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send: - MultiAttachBCC" + strLogMessage, Ex);

                    bSuccess = false;
                }

            }//END_USING


        }//END_Try
        catch (Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage - MultiAttach", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage: - MultiAttach" + strLogMessage, Ex);

            bSuccess = false;
        }

        return bSuccess;

    }
    
    public static bool SendCircularMail(string ReplyToEmail, string ToEmail, string CCEmail, string BCCEmail, string Subject, string MailBody, List<string> ListFilePath)
    {
        // string SenderEmail = "Admin@BabajiShivram.com";

        bool bSuccess = false;

        LoginClass loggedInUser = new LoginClass();
        try
        {
            string SenderEmail = EmailCircularConfig.GetEmailFrom();
            string SenderEmailUser = EmailCircularConfig.GetEmailUser();
            string SenderEmailPass = EmailCircularConfig.GetEmailPassCode();

            string strEmailCC = EmailCircularConfig.GetEmailCCTo();
            string strEmailBCC = EmailCircularConfig.GetEmailBCCTo();

            string strSMTP = EmailCircularConfig.GetSMTPIP();
            int intSMTPort = EmailCircularConfig.GetSMTPPort();
            bool SSLRequired = EmailCircularConfig.GetEmailSSL();

            if (ToEmail == "")
            {
                ToEmail = "javed.shaikh@BabajiShivram.com";
            }

            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (CCEmail != "")
                {
                    msg.CC.Add(CCEmail);
                }
                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }

                if (BCCEmail != "")
                {
                    msg.Bcc.Add(BCCEmail);
                }

                if (strEmailBCC != "" && strEmailBCC != null)
                {
                    msg.Bcc.Add(strEmailBCC);
                }

                msg.IsBodyHtml = true;

                if (ListFilePath.Count > 0)
                {
                    /* Create the email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\");
                    }

                    foreach (string strFilePath in ListFilePath)
                    {
                        string strAttchPath = ServerPath + strFilePath;

                        if (System.IO.File.Exists(strAttchPath))
                        {
                            Attachment attach = new Attachment(strAttchPath);

                            /* Attach the newly created email attachment */
                            msg.Attachments.Add(attach);
                        }
                    }
                } //END_IF_Attachment

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmailUser, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                    bSuccess = true;
                }
                catch (System.Exception Ex)
                {
                    string strLogMessage = "Circular Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "CircularEmail.cs.MailClient.Send", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("CircularEmail.cs.MailClient.Send:" + strLogMessage, Ex);
                }

            }//END_USING


        }//END_Try
        catch (System.Exception Ex)
        {
            string strLogMessage = "Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage:" + strLogMessage, Ex);
        }

        return bSuccess;
    }

    public static bool SendMailEbill(string ReplyToEmail, string ToEmail, string CCEmail, string Subject, string MailBody, List<string> ListFilePath)
    {
        LoginClass loggedInUser = new LoginClass();

        bool bSuccess = true;

        try
        {
            string strEmailCC = ""; string strEmailBCC = "";

            string SenderEmail = EmailConfig.GetEmailFromEBill();
            string SenderEmailUser = EmailConfig.GetEmailFromEBill();
            string SenderEmailPass = EmailConfig.GetEmailPassCode();

            string strSMTP = EmailConfig.GetSMTPIP();
            int intSMTPort = EmailConfig.GetSMTPPort();
            bool SSLRequired = EmailConfig.GetEmailSSL();

            strEmailCC = EmailConfig.GetEmailCCTo();

            if (ToEmail == "")
            {
                ToEmail = "amit.bakshi@BabajiShivram.com";
            }

            using (MailMessage msg = new MailMessage(SenderEmail, ToEmail))
            {
                msg.Subject = Subject;
                msg.Body = MailBody;

                // msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;

                //  msg.From = new MailAddress(loggedInUser.glUserName, "Freight Care Babaji Shivram");

                if (ReplyToEmail != "")
                    msg.ReplyToList.Add(ReplyToEmail);

                if (strEmailCC != "" && strEmailCC != null)
                {
                    msg.CC.Add(strEmailCC);
                }
                if (CCEmail != "")
                {
                    msg.CC.Add(CCEmail);
                }

                msg.IsBodyHtml = true;

                if (ListFilePath.Count > 0)
                {
                    /* Create the email attachment with the uploaded file */
                    String ServerPath = FileServer.GetFileServerDir();

                    if (ServerPath == "")
                    {
                        ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\");
                    }

                    foreach (string strFilePath in ListFilePath)
                    {
                        string strAttchPath = ServerPath + strFilePath;

                        if (System.IO.File.Exists(strAttchPath))
                        {
                            Attachment attach = new Attachment(strAttchPath);

                            /* Attach the newly created email attachment */
                            msg.Attachments.Add(attach);
                        }
                    }
                }
                //END_IF

                // For Web Config Email Setting

                SmtpClient MailClient = new SmtpClient(strSMTP, intSMTPort);
                MailClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential mailAuthentication = new
                System.Net.NetworkCredential(SenderEmailUser, SenderEmailPass);
                MailClient.Credentials = mailAuthentication;
                MailClient.EnableSsl = SSLRequired;

                // Time specefied in MilliSeconds
                //MailClient.Timeout = 500000; // 500 seconds

                try
                {
                    MailClient.Send(msg);
                }
                catch (Exception Ex)
                {
                    string strLogMessage = "EBill Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
                    ErrorLog.LogToDatabase(0, "Email.cs.MailClient.Send - SendMailEbill", "", strLogMessage, Ex, "", loggedInUser.glUserId);
                    ErrorLog.LogToTextFile("Email.cs.MailClient.Send: - SendMailEbill" + strLogMessage, Ex);

                    bSuccess = false;
                }

            }//END_USING


        }//END_Try
        catch (Exception Ex)
        {
            string strLogMessage = "EBill Email Sending Failed." + "To:" + ToEmail + " Sub:" + Subject;
            ErrorLog.LogToDatabase(0, "Email.cs.MailMessage - SendMailEbill", "", strLogMessage, Ex, "", loggedInUser.glUserId);
            ErrorLog.LogToTextFile("Email.cs.MailMessage: - SendMailEbill" + strLogMessage, Ex);

            bSuccess = false;
        }

        return bSuccess;

    }
}
