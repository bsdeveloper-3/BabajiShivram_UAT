using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Security.Principal;
using System.Runtime.InteropServices;
/// <summary>
/// Summary description for FileDownload
/// </summary>
public class FileDownload
{
    /*
    public const int LOGON32_LOGON_INTERACTIVE = 2;
    public const int LOGON32_PROVIDER_DEFAULT = 0;

    public static WindowsImpersonationContext impersonationContext ;

    [DllImport("advapi32.dll")]
    public static extern int LogonUserA(String lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int DuplicateToken(IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool RevertToSelf();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);
    */
    public FileDownload()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void Download(HttpResponse Response, string filepath, string FileName)
    {        
        if (filepath != null)
        {
          // string strUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
          //  Response.Write(strUserName);

            /*            
            bool isValiduser = FileDownload.impersonateValidUser("BSImport", "Weboarise2", "bsc#123");
            if (isValiduser)
            {*/
                if (File.Exists(filepath))
                {
                    string filename = Path.GetFileName(filepath);
                    string fileExt = Path.GetExtension(filepath);
                    // string str2UserName = " SecondName"+System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    // Response.Write(str2UserName);

                    Response.Clear();

                    if (fileExt.ToLower() == ".pdf")
                    {
                        Response.ContentType = "application/pdf";
                    }
                    else
                    {
                        Response.ContentType = "application/octet-stream";
                    }

                    //Response.ContentType = "application/octet-stream";
                    
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    //   Response.Flush();
                    Response.WriteFile(filepath);
                    Response.End();

                    //Response.Flush();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
             
           /* } *///END_IF
            
            /*
            if (File.Exists(filepath))
            {
                IntPtr token;

                    ///// Network Download Solution From
                    // http://stackoverflow.com/questions/2563724/accessing-password-protected-network-drives-in-windows-in-c/2563809#2563809 
                    ////
        if (!NativeMethods.LogonUser(
            "BSImport",
            "Weboarise2",
            "bsc#123",
            NativeMethods.LogonType.NewCredentials,
            NativeMethods.LogonProvider.Default,
            out token))
        {
            throw new System.ComponentModel.Win32Exception();
        }

        try
        {
            IntPtr tokenDuplicate;

            if (!NativeMethods.DuplicateToken(
                token,
                NativeMethods.SecurityImpersonationLevel.Impersonation,
                out tokenDuplicate))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                using (WindowsImpersonationContext impersonationContext =
                    new WindowsIdentity(tokenDuplicate).Impersonate())
                {
                    /////////////////Do stuff with your share here////////////////////////
                
                    string filename = Path.GetFileName(filepath);
                   // string str2UserName = " SecondName"+System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                   // Response.Write(str2UserName);
                   
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    //   Response.Flush();
                    Response.WriteFile(filepath);
                    Response.End();
                   
                    //////////////////////////////////////////////////////
                  impersonationContext.Undo();
                    return;
                }
            }
            finally
            {
                if (tokenDuplicate != IntPtr.Zero)
                {
                    if (!NativeMethods.CloseHandle(tokenDuplicate))
                    {
                        // Uncomment if you need to know this case.
                        ////throw new Win32Exception();

                        throw new System.ComponentModel.Win32Exception();
                    }
                }
            }
        }
        finally
        {
            if (token != IntPtr.Zero)
            {
                if (!NativeMethods.CloseHandle(token))
                {
                    // Uncomment if you need to know this case.
                    ////throw new Win32Exception();

                    throw new System.ComponentModel.Win32Exception();
                }
            }
        }       
                }//END_IF
                else
                {
                    Response.Write("File Not Found!");
                }*/

        }//END_IF
        
    }

    /*
    private static bool impersonateValidUser(String userName, String domain, String password)
    {
        WindowsIdentity tempWindowsIdentity;
        IntPtr token = IntPtr.Zero;
        IntPtr tokenDuplicate = IntPtr.Zero;

        if (RevertToSelf())
        {
            if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                   impersonationContext = tempWindowsIdentity.Impersonate();
                    if (impersonationContext != null)
                    {
                        CloseHandle(token);
                        CloseHandle(tokenDuplicate);
                        return true;
                    }
                }
            }
        }
        if (token != IntPtr.Zero)
            CloseHandle(token);
        if (tokenDuplicate != IntPtr.Zero)
            CloseHandle(tokenDuplicate);
        return false;
    }

    private void undoImpersonation()
    {
        impersonationContext.Undo();
    }
    */
}
