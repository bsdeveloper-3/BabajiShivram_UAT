
namespace TaxProEWB.API
{
  public class AssignedEWBItem
  {
    public long ewbNo { get; set; }

    public string ewbDate { get; set; }

    public string genGstin { get; set; }

    public string docNo { get; set; }

    public string docDate { get; set; }

    public string delPlace { get; set; }

    public string delPinCode { get; set; }

    public int delStateCode { get; set; }

    public string validUpto { get; set; }

    public string extendedTimes { get; set; }

    public string status { get; set; }

    public string rejectStatus { get; set; }
  }
}
