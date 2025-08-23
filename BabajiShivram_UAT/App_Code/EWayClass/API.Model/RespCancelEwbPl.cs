using System.Collections.Generic;
namespace TaxProEWB.API
{
  public class RespCancelEwbPl
  {
    public string ewayBillNo { get; set; }

    public string cancelDate { get; set; }

    List<RespCancelEwbPl> EWayCancleList { get; set; }
    
    }
}
