using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for Company
/// </summary>
public class Company
{
    private int _CompanyId = -1;
    private string _CompanyName = String.Empty;

    public int CompanyId
    {
        get { return _CompanyId; }

        set { _CompanyId = value; }
    }

    public string CompanyName
    {
        get { return _CompanyName; }

        set { _CompanyName = value; }
    }
}