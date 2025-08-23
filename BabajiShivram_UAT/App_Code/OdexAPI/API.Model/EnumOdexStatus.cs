using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EnumOdexStatus
/// </summary>
namespace OdexConnect.API.Model
{
    public enum OdexStatus
    {
        InvoiceError        = 99,
        InvoiceRequested    = 100,
        InvoiceRejected     = 102,
        AwaitingIGM         = 104,
        InvoiceReleased     = 106,
        InvoiceCancelled    = 108,
        DOError             = 199,
        DORequested         = 200,
        DORejected          = 202,
        DOConfirmed         = 206,
        DOCancelled         = 208,
    }
}