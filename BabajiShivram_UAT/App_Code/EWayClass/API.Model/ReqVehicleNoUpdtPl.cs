
namespace TaxProEWB.API
{
  public class ReqVehicleNoUpdtPl
  {
    public long EwbNo { get; set; }

    public string VehicleNo { get; set; }

    public string FromPlace { get; set; }

    public int FromState { get; set; }

    public string ReasonCode { get; set; }

    public string ReasonRem { get; set; }

    public string TransDocNo { get; set; }

    public string TransDocDate { get; set; }

    public string TransMode { get; set; }

    public string vehicleType { get; set; }
  }
}
