
namespace TaxProEWB.API
{
  public class ReqMultiVehUpdt
  {
    public long ewbNo { get; set; }

    public string groupNo { get; set; }

    public string oldvehicleNo { get; set; }

    public string newVehicleNo { get; set; }

    public string oldTranNo { get; set; }

    public string newTranNo { get; set; }

    public string fromPlace { get; set; }

    public int fromState { get; set; }

    public string reasonRem { get; set; }

    public string reasonCode { get; set; }
  }
}
