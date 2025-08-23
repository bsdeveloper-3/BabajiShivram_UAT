
namespace TaxProEWB.API
{
  public class ReqReGenerateCEWBPl
  {
    public long tripSheetNo { get; set; }

    public string vehicleNo { get; set; }

    public string fromPlace { get; set; }

    public int fromState { get; set; }

    public string transDocNo { get; set; }

    public string transDocDate { get; set; }

    public string transMode { get; set; }

    public int reasonCode { get; set; }

    public string reasonRem { get; set; }
  }
}
