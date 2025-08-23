
namespace TaxProEWB.API
{
  public class ReqExtendValidityEWBPl
  {
    public long ewbNo { get; set; }

    public string vehicleNo { get; set; }
    public string fromPincode { get; set; }
    public string fromPlace { get; set; }

    public int fromState { get; set; }
    public string consignmentStatus { get; set; }
    public string addressLine1 { get; set; }
    public string addressLine2 { get; set; }
    public string addressLine3 { get; set; }

    public int remainingDistance { get; set; }

    public string transDocNo { get; set; }

    public string transDocDate { get; set; }

    public string transMode { get; set; }

    public int extnRsnCode { get; set; }

    public string extnRemarks { get; set; }
    public string transitType { get; set; }
    }
}
