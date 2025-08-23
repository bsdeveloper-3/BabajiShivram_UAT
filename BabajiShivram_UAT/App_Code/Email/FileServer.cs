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
/// Summary description for FileServer
/// </summary>
public class FileServer
{
    public FileServer()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetFileServerDir()
    {
        string RootDir;
        try
        {
            RootDir = WebConfigurationManager.AppSettings["FileServerPath"];
        }
        catch
        {
            throw new Exception("FileServerPath not defined");
        }
        return RootDir;
    }

    public static string GetBillingServerDir()
    {
        string RootDir;
        try
        {
            RootDir = WebConfigurationManager.AppSettings["BilingServerPath"];
        }
        catch
        {
            throw new Exception("BilingServerPath not defined");
        }
        return RootDir;
    }

    public static string ValidateFileName(string strFileName)
    {
        strFileName     =   strFileName.Replace(",","-");
        strFileName     =   strFileName.Replace(" ","");
        strFileName     =   strFileName.Replace("/", "");
        strFileName     =   strFileName.Replace("%", "");
        strFileName     =   strFileName.Replace("*", "");
        strFileName     =   strFileName.Replace("\"", "");
        strFileName     =   strFileName.Replace("#", "");
        strFileName     =   strFileName.Replace("@", "-");
        strFileName     =   strFileName.Replace("(", "");
        strFileName     =   strFileName.Replace(")", "");
        strFileName     =   strFileName.Trim();

        if (strFileName.Length > 80)
        {
            int RemoveIndex = 40;
            int ValidLength = strFileName.Length - RemoveIndex;

            if (ValidLength > 80)
            {
                ValidLength = ValidLength + 20;

                RemoveIndex = RemoveIndex + 20;
            }

            strFileName = strFileName.Remove(5, RemoveIndex);
        }

        return strFileName;
    }
}
