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
/// Summary description for CustomConfigExcludeFileCollection
/// </summary>
public class CustomConfigExcludeFileCollection : ConfigurationElementCollection
{
    public CustomConfigExcludeFile this[int index]
    {
        get
        {
            return base.BaseGet(index) as CustomConfigExcludeFile;
        }
        set
        {
            if (base.BaseGet(index) != null)
            {
                base.BaseRemoveAt(index);
            }
            this.BaseAdd(index, value);
        }
    }

    protected override ConfigurationElement CreateNewElement()
    {
        return new CustomConfigExcludeFile();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((CustomConfigExcludeFile)element).FileName;
    }
}
