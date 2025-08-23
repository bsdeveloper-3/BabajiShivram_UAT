
using System.Collections.Generic;

namespace TaxProEWB.API
{
  public class ReqGenCEwbPl
  {
    public string fromPlace { get; set; }

    public string fromState { get; set; }

    public string vehicleNo { get; set; }

    public string transMode { get; set; }

    public string TransDocNo { get; set; }

    public string TransDocDate { get; set; }

    public List<ReqGenCEwbPl.TripSheetGenCEWB> tripSheetEwbBills { get; set; }

    public class TripSheetGenCEWB
    {
      public long ewbNo { get; set; }
    }
  }
}
