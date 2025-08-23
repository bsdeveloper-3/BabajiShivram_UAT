
using System.Collections.Generic;
namespace TaxProEWB.API
{
  public class RespGenEwbPl
  {
    public string ewayBillNo { get; set; }

    public string ewayBillDate { get; set; }

    public string validUpto { get; set; }

    public string alert { get; set; }
    List<RespGenEwbPl> EWayList { get; set; }
  }
}
