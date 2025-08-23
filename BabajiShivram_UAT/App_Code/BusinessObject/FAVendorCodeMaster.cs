using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FAVendorCodeMaster
/// </summary>
public class FAVendorCodeMaster
{
    public FAVendorCodeMaster()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Private Variable

    private int vendorCodeId = -1;
    private string vendorCode = String.Empty;
    private string vendorName = String.Empty;
    
    #endregion
    public int VendorCodeId
    {
        get { return vendorCodeId; }
        set { vendorCodeId = value; }
    }
    public string Vendor_Code
    {
        get
        {
            return vendorCode;

        }
        set
        {
            vendorCode = value;
        }
    }
    public string Vendor_Name
    {
        get
        {
            return vendorName;

        }
        set
        {
            vendorName = value;
        }
    }
    
}