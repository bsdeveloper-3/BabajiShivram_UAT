using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoResp
/// </summary>
namespace MyPacco.API
{
    public class MyPaccoResp : EventArgs
    {
        public bool IsSuccess { get; set; }

        public string TxnOutcome { get; set; }
    }
}
