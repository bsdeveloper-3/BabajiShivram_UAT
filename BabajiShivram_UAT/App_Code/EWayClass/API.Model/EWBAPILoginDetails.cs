using System;

namespace TaxProEWB.API
{
  public class EWBAPILoginDetails
  {
    public string EwbGstin { get; set; }

    public string EwbUserID { get; set; }

    public string EwbPassword { get; set; }

    public string EwbAppKey { get; set; }

    public string EwbAuthToken { get; set; }

    public DateTime EwbTokenExp { get; set; }

    public string EwbSEK { get; set; }
  }
}
