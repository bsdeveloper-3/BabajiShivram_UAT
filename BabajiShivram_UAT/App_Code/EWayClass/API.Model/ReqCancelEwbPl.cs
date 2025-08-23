
namespace TaxProEWB.API
{
  public class ReqCancelEwbPl
  {
    public long ewbNo { get; set; }

    public int cancelRsnCode { get; set; }

    public string cancelRmrk { get; set; }
  }
}
