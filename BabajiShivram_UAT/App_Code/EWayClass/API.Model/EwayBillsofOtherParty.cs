
namespace TaxProEWB.API
{
  public class EwayBillsofOtherParty
  {
    public long ewbNo { get; set; }

    public string ewayBillDate { get; set; }

    public string genMode { get; set; }

    public string genGstin { get; set; }

    public string docNo { get; set; }

    public string docDate { get; set; }

    public string fromgstin { get; set; }

    public string fromTradename { get; set; }

    public string togstin { get; set; }

    public string toTradename { get; set; }

    public double totInvValue { get; set; }

    public int hsnCode { get; set; }

    public string hsnDesc { get; set; }

    public string status { get; set; }

    public string rejectStatus { get; set; }
  }
}
