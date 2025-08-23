
namespace TaxProEWB.API
{
  public class ReqMultiVehAdd
  {
    public long ewbNo { get; set; }

    public string groupNo { get; set; }

    public string vehicleNo { get; set; }

    public string transDocNo { get; set; }

    public string transDocDate { get; set; }

    public int quantity { get; set; }
  }
}
