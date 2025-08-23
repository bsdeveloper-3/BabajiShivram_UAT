
using System;

namespace TaxProEWB.API
{
  public class TxnResp : EventArgs
  {
    public bool IsSuccess { get; set; }

    public string TxnOutcome { get; set; }
  }
}
