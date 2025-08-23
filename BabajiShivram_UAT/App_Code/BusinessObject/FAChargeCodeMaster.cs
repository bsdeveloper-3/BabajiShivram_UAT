using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FAChargeCodeMaster
/// </summary>
public class FAChargeCodeMaster
{
    public FAChargeCodeMaster()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Private Variable

    private int chargeCodeId = -1;
    private string chargeCode = String.Empty;
    private string chargeName = String.Empty;
    private string hsnCode = String.Empty;
    private string chargeCategory = String.Empty;

    #endregion
    public int ChargeCodeId
    {
        get { return chargeCodeId; }
        set { chargeCodeId = value; }
    }
    public string Charge_Code
    {
        get
        {
            return chargeCode;

        }
        set
        {
            chargeCode = value;
        }
    }
    public string Charge_Name
    {
        get
        {
            return chargeName;

        }
        set
        {
            chargeName = value;
        }
    }
    public string HSN_Code
    {
        get { return hsnCode; }
        set { hsnCode = value; }
    }
    public string Charge_Category
    {
        get { return chargeCategory; }
        set { chargeCategory = value; }
    }
}