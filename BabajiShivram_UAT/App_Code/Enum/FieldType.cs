
/// <summary>
/// Possible types of field objects. This enumeration can be used when constructing
/// field objects from external data.
/// </summary>

    public enum FieldDataType
    {
        Alphanumeric    = 1,
        Numeric         = 2,
        Date            = 3,
        Percent         = 4,
        Currency        = 5,
        CheckBox        = 6
    }

    public enum EnumBOEType
    {
        Home    = 1,
        Inbond  = 2,
        Exbond  = 3
    }
    public enum EnumWarehouseType
    {
        General = 1,
        Bonded  = 2,
        SEZ     = 3
    }
    public enum EnumContainerType
    {
        FCL = 1,
        LCL = 2
    }
    public enum EnumContainerSize
    {
        Twenty      = 1,
        Forty       = 2,
        FortyFive   = 3
    }
    public enum EnumUserType
    {
        AdminUser       = -1,
        BabajiUser      = 1,
        CustomerUser    = 2,
        FreightAgent    = 3
    }

    public enum EnumJobType
    {
        Courier     = 1,
        OldUsed     = 2,
        ReImport    = 3,
        RandR       = 4,
        TemporaryImport = 5,
        HighSeaSale  = 6,
        New         = 9,
        ADC         = 10,
        PHO         = 11,
        ADCPHO      = 12,
        DPD         = 13,
    }

    public enum EnumPCDDocType
    {
        BackOffice      = 1,
        PCACustomer     = 2,
        BillingAdvice   = 3,
        BillingDispatch = 4
    }

    public enum EnumUnit
    { 
        perKG   =   1,
        perMT   =   2,
        perFRT  =   3,
        perTEU  =   4,
        perFEU  =   5,
        perBL   =   6,
        PercentOf = 7,
        perCBM  =   8,
	    PerAWB  =   9,
	    PerCont =  10
    }

   public enum EnumBranch
    {
        Mumbai      = 3,
        Delhi       = 5,
        Chennai     = 6,
        Kolkata     = 7, 
        Gandhidham  = 8,
        Kakinada    = 12,
        Ahmedabad   = 13, 
        Vizag       = 14,
        Jaipur      = 15,
        Ankleshwar  = 16,
        Goa         = 18,
        Bangalore   = 20,
        NAGPUR      = 21,
        Hyderabad   = 23
    }

public enum EnumCompanyType
{
    Customer    = 1,
    Consignee   = 2,
    FreightForwarder = 3,
    Shipper     = 4,
    Agent       = 5,
    Transporter = 6,
    Vendor = 7
}

//---------Start Biiling---------------
    public enum EnumBilltype
    {
        BillingScrutiny = 1,
        DraftInvoice = 2,
        DraftCheck = 3,
        FinalInvoiceTyping = 4,
        FinalInvoiceCheck = 5,
        BillDispatch = 6
    }
//---------End Biiling---------------

public enum EnumCompanyCategory
{ 
	Customer = 1,
	Consignee = 2,
	Freight = 3,
	Shipper = 4,
	Agent = 5,
	Transport = 6
}

// --------END Company Category ---------

// ----- FA - BJV - Payment Type -----
public enum EnumFAPayMode
{
    Cash = 1,
    Cheque = 2,
    DD = 3,
    RTGS = 4,
    Online = 5,
    Credit = 6
}
public enum EnumFARIMorAG
{
    RIM = 1,
    AG = 2
}



