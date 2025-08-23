
namespace TaxProEWB.API
{
  public class ReqIniMultiVehicleMov
  {
    public long ewbNo { get; set; }

    public string reasonCode { get; set; }

    public string reasonRem { get; set; }

    public string fromPlace { get; set; }

    public int fromState { get; set; }

    public string toPlace { get; set; }

    public int toState { get; set; }

    public string transMode { get; set; }

    public int totalQuantity { get; set; }

    public string unitCode { get; set; }
  }
}
