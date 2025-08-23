using System.Collections.Generic;
/// <summary>
/// Summary description for clsEWayJson
/// </summary>
public class EwayItemList
{
    public int itemNo { get; set; }
    public string productName { get; set; }
    public string productDesc { get; set; }
    public int hsnCode { get; set; }
    public double quantity { get; set; }
    public string qtyUnit { get; set; }
    public double taxableAmount { get; set; }
    public double sgstRate { get; set; }
    public double cgstRate { get; set; }
    public double igstRate { get; set; }
    public double cessRate { get; set; }
}

public class EWayBillList
{
    public string genMode { get; set; }
    public string userGstin { get; set; }
    public string supplyType { get; set; }
    public string subSupplyType { get; set; }
    public string docType { get; set; }
    public string docNo { get; set; }
    public string docDate { get; set; }
    public string fromGstin { get; set; }
    public string fromTrdName { get; set; }
    public string fromAddr1 { get; set; }
    public string fromAddr2 { get; set; }
    public string fromPlace { get; set; }
    public int fromPincode { get; set; }
    public int fromStateCode { get; set; }
    public int actualFromStateCode { get; set; }
    public string toGstin { get; set; }
    public string toTrdName { get; set; }
    public string toAddr1 { get; set; }
    public string toAddr2 { get; set; }
    public string toPlace { get; set; }
    public int toPincode { get; set; }
    public int toStateCode { get; set; }
    public int actualToStateCode { get; set; }
    public double totalValue { get; set; }
    public double cgstValue { get; set; }
    public double sgstValue { get; set; }
    public double igstValue { get; set; }
    public double cessValue { get; set; }
    public double totInvValue { get; set; }
    public string transMode { get; set; }
    public string transDistance { get; set; }
    public string transporterName { get; set; }
    public string transporterId { get; set; }
    public string transDocNo { get; set; }
    public string transDocDate { get; set; }
    public string vehicleNo { get; set; }
    public string vehicleType { get; set; }
    public int mainHsnCode { get; set; }
    public List<EwayItemList> itemList { get; set; }
}

public class EWayRootObject
{
    public string version { get; set; }
    public List<EWayBillList> billLists { get; set; }
}