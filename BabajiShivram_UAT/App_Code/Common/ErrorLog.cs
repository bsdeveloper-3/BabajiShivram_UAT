using System;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using System.IO;
/// <summary>
/// Summary description for ErrorLog
/// </summary>
public class ErrorLog
{
    public static void LogToTextFile(string Message)
    {
        ErrorLog.LogToTextFile(Message, null);
    }

    public static void LogToTextFile(string Message, Exception Ex)
    {
        try
        {
            string fileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "BabajiError.log");

            FileInfo objFi = new FileInfo(fileName);

            // Rename File if file size is log greater than 2.5 MB
            if (objFi.Exists)
            {
                if (objFi.Length > 2500000) // Length in Bytes. 100,00,00 Bytes = 1.5 MB
                {
                    string NewFileName = "BabajiError_Old_" + DateTime.Now.ToFileTimeUtc().ToString() + ".log";
                    objFi.MoveTo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, NewFileName));
                }
            }
            using (StreamWriter logFile = new StreamWriter(fileName, true))
            {
                logFile.WriteLine("********************************************");
                logFile.WriteLine("{0}: {1}", DateTime.Now, Message);

                if (Ex != null)
                {
                    logFile.WriteLine("Exception Detail:");
                    logFile.WriteLine(Ex.ToString());
                }
                //END_IF

                logFile.Close();
                logFile.Dispose();
                GC.Collect();
            }
        }
        catch (System.IO.IOException)
        {

        }
        catch (Exception)
        {

        }
    }

    public static string DisplayExcetions(Exception ex, bool ShowMessage, UpdatePanel up)
    {
        string strMessage = null;
        bool LogMsg = true;
        switch (ex.GetType().Name)
        {
            case "ThreadAbortException":
                LogMsg = false;
                break;
            case "ApplicationException":
                strMessage = "Application Exception is caught";
                break;
            case "ArgumentNullException":
                strMessage = "Argument Null Exception is caught";
                break;
            case "ArgumentOutOfRangeException":
                strMessage = "Argument Out Of Range Exception is caught";
                break;
            case "DllNotFoundException":
                strMessage = "Dll Not Found Exception is caught";
                break;
            case "IndexOutOfRangeException":
                strMessage = "Index Out Of Range Exception is caught";
                break;
            case "InsufficientMemoryException":
                strMessage = "Insufficient Memory Exception is caught";
                break;
            case "InvalidCastException":
                strMessage = "Invalid Cast Exception is caught";
                break;
            case "InvalidOperationException":
                strMessage = "Invalid Operation Exception is caught";
                break;
            case "NullReferenceException":
                strMessage = "Null Reference Exception is caught";
                break;
            case "OutOfMemoryException":
                strMessage = "Out Of Memory Exception is caught";
                break;
            case "OverflowException":
                strMessage = "Overflow Exception is caught";
                break;
            case "TimeoutException":
                strMessage = "Timeout Exception is caught";
                break;
            case "UnauthorizedAccessException":
                strMessage = "Unauthorized Access Exception  is caught";
                break;
            case "SqlException":
                strMessage = "Sql Exception is caught";
                break;
            default:
                strMessage = "Exception is caught";
                break;
        }
        if (LogMsg)
        {
            if (ShowMessage == true)
            {
                ErrorLog el = new ErrorLog();
                if (up == null)
                    el.ShowMessageBox(strMessage);
                else
                    el.ShowMessageBox(strMessage, up);

            }
            try
            {
                string strExceptionDetails = Environment.NewLine + "Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString() + Environment.NewLine;
                if (ex.Data["Query"] != null)
                {
                    strExceptionDetails +=
                        "###-----------------------------------------------------------------------###" + Environment.NewLine +
                        ex.Data["Query"] + Environment.NewLine +
                         "***-----------------------------------------------------------------------***";
                }
                else
                {
                    strExceptionDetails += "***-----------------------------------------------------------------------***";
                }
                //     string strExceptionDetails = Environment.NewLine + "Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                //"" + Environment.NewLine + "Date :" + DateTime.Now.ToString() + Environment.NewLine +
                //"-----------------------------------------------------------------------------" + Environment.NewLine +
                //ex.Data["Query"] + Environment.NewLine +
                //"-----------------------------------------------------------------------------";
                string strMonthOftheYear = DateTime.Now.ToString("MMM-yyyy");
                string strFileName = @"C:\Exception Files\" + strMonthOftheYear;
                DirectoryInfo dir = new DirectoryInfo(strFileName);
                if (!dir.Exists)
                    dir.Create();

                string folderName = @"C:\Exception Files\" + strMonthOftheYear;

                strFileName = strFileName + "\\Exceptions On " + DateTime.Now.Day + ".txt";



                if (!File.Exists(strFileName))
                {
                    File.WriteAllText(strFileName, strExceptionDetails);
                }
                else
                {
                    Int64 fileSizeInBytes = new FileInfo(strFileName).Length;




                    if (fileSizeInBytes < 3145728)
                    {
                        File.AppendAllText(strFileName, strExceptionDetails);
                    }
                    else
                    {
                        FileInfo[] files = dir.GetFiles("*Exceptions On " + DateTime.Now.Day + "*", SearchOption.TopDirectoryOnly);
                        string MoveFilename = folderName + "\\Exceptions On " + DateTime.Now.Day + "_" + files.Length + ".txt";
                        bool filemoved = false;
                        try
                        {
                            File.Move(strFileName, MoveFilename);
                            filemoved = true;
                        }
                        catch
                        {

                        }
                        if (filemoved)
                            File.WriteAllText(strFileName, strExceptionDetails);
                        else
                        {
                            File.AppendAllText(strFileName, strExceptionDetails);
                        }

                    }

                }
            }
            catch
            {
            }
        }
        return strMessage;
    }

    public void ShowMessageBox(string message)
    {
        var page = HttpContext.Current.CurrentHandler as Page;
        if (page != null)
        {
            page.ClientScript.RegisterStartupScript(this.GetType(), "newWindow", "alert('" + message + "');", true);
        }
    }
    public void ShowMessageBox(string message, UpdatePanel up)
    {

        ScriptManager.RegisterStartupScript(up, this.GetType(), "newWindow", "alert('" + message + "');", true);

    }
    public static void SendMail(string Message)
    {
        ErrorLog.SendMail(Message, null);
    }

    public static void SendMail(string Message, Exception Ex)
    {
        string strEmailTo = EmailConfig.GetErrorEmailTo();

        string strSubject = "LiveTracking Error Message-" + DateTime.Now.ToString();

        string strMessageBody = "";

        if (Ex == null)
        {
            strMessageBody = "Error Reported In LiveTracking./n" + Message;
        }
        else
        {
            strMessageBody = "Error Reported In LiveTracking- " + DateTime.Now.ToString() + " <BR> <b>Error Message:</b> <BR>" + Message + "<BR> Exception Details: " + Ex.Message.ToString();
        }

        EMail.SendMail("", strEmailTo, strSubject, strMessageBody, "");

    }

    public static void SendMail(string Subject, string Message, Exception Ex)
    {
        string strEmailTo = EmailConfig.GetErrorEmailTo();
        string strSubject = "LiveTracking Error Message-" + DateTime.Now.ToString();
        string strMessageBody = "";

        if (Subject != "")
        {
            strSubject = Subject + " - " + DateTime.Now.ToString();
        }

        if (Ex == null)
        {
            strMessageBody = "Error Reported In LiveTracking./n" + Message;
        }
        else
        {
            strMessageBody = "Error Reported In LiveTracking- " + DateTime.Now.ToString() + " <BR> <b>Error Message:</b> <BR>" + Message + "<BR> Exception Details: " + Ex.Message.ToString();
        }

        EMail.SendMail("", strEmailTo, strSubject, strMessageBody, "");

    }

    public static void LogToDatabase(int JobId, string TypeName, string ProcedureName, string Message, Exception Ex, string Description, int lUser)
    {
        if (Ex != null)
        {
            Description = Description + " Exception Details:" + Ex.Message;
        }

        DBOperations.AddErrorLog(JobId, TypeName, ProcedureName, Message, Description, lUser);

    }
}
