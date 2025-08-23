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
/// Summary description for CustomConfigutation
/// </summary>
public class CustomConfigutation : ConfigurationSection
{
    public static CustomConfigutation GetExcludedFilesConfig()
    {
        return ConfigurationManager.GetSection("BabajiCustomConfigutation/ExcludedFilesFromTreeBind") as CustomConfigutation;
    }


    [ConfigurationProperty("Description", IsRequired = true)]
    public string Description
    {
        get
        {
            return this["Description"] as string;
        }
    }

    [ConfigurationProperty("ExcludeFiles")]
    public CustomConfigExcludeFileCollection ExcludeFiles
    {
        get
        {
            return this["ExcludeFiles"] as CustomConfigExcludeFileCollection;
        }
    }
}
