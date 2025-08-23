using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for FAPayMaster
/// </summary>
public class FAPayMaster
{
    public FAPayMaster()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Private Variable

    private int payMasterId = -1;
    private string par_code = String.Empty;
    private string par_name = String.Empty;
    private string gstin = String.Empty;
    
    #endregion

    public int PayMasterId
    {
        get { return payMasterId; }
        set { payMasterId = value; }
    }

    public string Par_Code
    {
        get
        {
            return par_code;

        }
        set
        {
            par_code = value;
        }
    }
    public string Par_Name
    {
        get
        {
            return par_name;

        }
        set
        {
            par_name = value;
        }
    }
    public string GSTIN
    {
        get { return gstin; }
        set { gstin = value; }
    }
}