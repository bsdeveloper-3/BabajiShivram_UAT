using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CustomConfigExcludeFile
/// </summary>
public class CustomConfigExcludeFile : ConfigurationElement
{
    [ConfigurationProperty("ExcludeFileName", IsRequired = true)]
    public string FileName
    {
        get
        {
            return this["ExcludeFileName"] as string;
        }
    }
}
