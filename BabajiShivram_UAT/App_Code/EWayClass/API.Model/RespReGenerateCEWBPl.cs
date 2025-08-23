
using System.Collections.Generic;

namespace TaxProEWB.API
{
  public class RespReGenerateCEWBPl
  {
    public long tripSheetNo { get; set; }

    public List<RespReGenerateCEWBPl.TripSheetEWB> tripSheetEwayBills { get; set; }

    public class TripSheetEWB
    {
      public long ewbNo { get; set; }
    }
  }
}
