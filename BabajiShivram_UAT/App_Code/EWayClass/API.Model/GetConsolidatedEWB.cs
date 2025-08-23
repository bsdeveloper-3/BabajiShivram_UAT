
using System.Collections.Generic;

namespace TaxProEWB.API
{
  public class GetConsolidatedEWB
  {
    public string tripSheetNo { get; set; }

    public string fromPlace { get; set; }

    public string fromState { get; set; }

    public string vehicleNo { get; set; }

    public int transMode { get; set; }

    public string userGstin { get; set; }

    public string enteredDate { get; set; }

    public string transDocNo { get; set; }

    public string transDocDate { get; set; }

    public List<GetConsolidatedEWB.TripSheetBills> tripSheetEwbBills { get; set; }

    public class TripSheetBills
    {
      public long ewbNo { get; set; }

      public string ewbDate { get; set; }

      public string userGstin { get; set; }

      public string docNo { get; set; }

      public string docDate { get; set; }

      public string fromGstin { get; set; }

      public string fromTradeName { get; set; }

      public string toGstin { get; set; }

      public string toTradeName { get; set; }

      public double totInvValue { get; set; }

      public string validUpto { get; set; }
    }
  }
}
