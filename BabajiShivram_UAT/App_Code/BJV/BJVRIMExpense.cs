using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BJVRIMExpense
/// </summary>
public class BJVRIMExpense
{
    #region Private Variables
    // BJV TABLE ERC
    private string      ay           =   "";
    private string      type         =   "";
    private int         billno       =   0;
    private string      suffix       =   "";
    private int         ser_no       =   1;
    private DateTime    billdate;
    private string      par_code     =   "";
    private string      refNO        =   "";
    private string      narration    =   "";
    private decimal     amount       =   0m;
    private string      jvpcode      =   "";
    private string      chargable    =   "";
    private string      chequeno     =   "";
    private DateTime    chequedt;
    private string      bankname     =   "";
    private string      createdby    =   "";
    private string      createdtime  =   "";
    private string      modifiedby   =   "";
    private string      modifiedtime =   "";
    private bool        approve;
    private string      approveby;
    private DateTime    approvedate;
    private string      remarks      =   "";
    private string      CSTCODE1     =   "";
    private string      CSTCODE2     =   "";
    private string      CSTCODE3     =   "";
    private string      CSTCODE4     =   "";
    private string      vcode        =   "";

    // BJV TABLE RECEIPT
    private string      charge       =  "";
    private string      paid_to      =  "";
    private string      rno          =  "1";
    private DateTime    paid_date;

    private string      seq          =  "";
    private string      oref         =  "";
    private string      hsn_sc       =  "";
    private string      txval        =  "";
    private string      cgstrt       =  "";
    private string      cgstamt      =  "";
    private string      sgstrt       =  "";
    private string      sgstamt      =  "";
    private string      igstrt       =  "";
    private string      igstamt      =  "";
    //@vcode varchar(11),
    private string      brdate       =  "";

    /// LEDGBDBF -------
    private bool        mark;
    private string      bc           = "";
    private DateTime    cdate;
    private decimal     balamt;
	private decimal     bjvadv;
    #endregion

    public string AY {
        get { return ay; }
        set { ay = value; }
    }
    public string Type
    {
        get { return type; }
        set { type = value; }
    }
    public int BillNo
    {
        get { return billno; }
        set { billno = value; }
    }
    public string Suffix
    {
        get { return suffix; }
        set { suffix = value; }
    }
    public int Ser_no
    {
        get { return ser_no; }
        set { ser_no = value; }
    }
    public DateTime BillDate
    {
        get { return billdate; }
        set { billdate = value; }
    }
    public string Par_Code
    {
        get { return par_code; }
        set { par_code = value; }
    }
    public string RefNO
    {
        get { return refNO; }
        set { refNO = value; }
    }
    public string Narration
    {
        get { return narration; }
        set { narration = value; }
    }
    public decimal Amount
    {
        get { return amount; }
        set { amount = value; }
    }
    public string JvpCode
    {
        get { return jvpcode; }
        set { jvpcode = value; }
    }
    public string Chargable
    {
        get { return chargable; }
        set { chargable = value; }
    }
    public string ChequeNo
    {
        get { return chequeno; }
        set { chequeno = value; }
    }
    public DateTime ChequeDate
    {
        get { return chequedt; }
        set { chequedt = value; }
    }
    public string BankName
    {
        get { return bankname; }
        set { bankname = value; }
    }
    public string CreatedBy
    {
        get { return createdby; }
        set { createdby = value; }
    }
    public string CreatedTime
    {
        get { return createdtime; }
        set { createdtime = value; }
    }
    public string ModifiedBy
    {
        get { return modifiedby; }
        set { modifiedby = value; }
    }
    public string ModifiedTime
    {
        get { return modifiedtime; }
        set { modifiedtime = value; }
    }
    public bool Approve
    {
        get { return approve; }
        set { approve = value; }
    }
    public string Approveby
    {
        get { return approveby; }
        set { approveby = value; }
    }
    public DateTime ApproveDate
    {
        get { return approvedate; }
        set { approvedate = value; }
    }
    public string Remarks
    {
        get { return remarks; }
        set { remarks = value; }
    }
    public string CSTCODEOne
    {
        get { return CSTCODE1; }
        set { CSTCODE1 = value; }
    }
    public string CSTCODETwo
    {
        get { return CSTCODE2; }
        set { CSTCODE2 = value; }
    }
    public string CSTCODEThree
    {
        get { return CSTCODE3; }
        set { CSTCODE3 = value; }
    }
    public string CSTCODEFour
    {
        get { return CSTCODE4; }
        set { CSTCODE4 = value; }
    }
    public string VCode
    {
        get { return vcode; }
        set { vcode = value; }
    }

    // BJV TABLE RECEIPT
    public string ChargeCode
    {
        get { return charge; }
        set { charge = value; }
    }
    public string Paid_To
    {
        get { return paid_to; }
        set { paid_to = value; }
    }
    public string RNO
    {
        get { return rno; }
        set { rno = value; }
    }
    public DateTime Paid_Date
    {
        get { return paid_date; }
        set { paid_date = value; }
    }
    public string Seq
    {
        get { return seq; }
        set { seq = value; }
    }
    public string OREF
    {
        get { return oref; }
        set { oref = value; }
    }
    public string HSN_SC
    {
        get { return hsn_sc; }
        set { hsn_sc = value; }
    }
    public string Txval
    {
        get { return txval; }
        set { txval = value; }
    }
    public string CGSTRT
    {
        get { return cgstrt; }
        set { cgstrt = value; }
    }
    public string CGSTAMT
    {
        get { return cgstamt; }
        set { cgstamt = value; }
    }
    public string SGSTRT
    {
        get { return sgstrt; }
        set { sgstrt = value; }
    }
    public string SGSTAMT
    {
        get { return sgstamt; }
        set { sgstamt = value; }
    }
    public string IGSTRT
    {
        get { return igstrt; }
        set { igstrt= value; }
    }
    public string IGSTAMT
    {
        get { return igstamt; }
        set { igstamt = value; }
    }
    public string BRDate
    {
        get { return brdate; }
        set { brdate = value; }
    }

    /// LEDGBDBF -------
    public bool Mark
    {
        get { return mark; }
        set { mark = value; }
    }
    public string BC
    {
        get { return bc; }
        set { bc = value; }
    }
    public DateTime CDate
    {
        get { return cdate; }
        set { cdate = value; }
    }
    public decimal BalAmt
    {
        get { return balamt; }
        set { balamt = value; }
    }
    public decimal BJVADV
    {
        get { return bjvadv; }
        set { bjvadv = value; }
    }
}