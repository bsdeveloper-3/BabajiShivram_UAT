using System;

public enum EnumInvoiceStatus
{
    TransporterBillUploaded = 50,
    BillReceived = 55,
    BillRejected = 56,
    InvoiceUploaded = 100,
    InvoiceCancelled = 105,
    HODOnHold   = 108,
    HODReject   = 109,
    HODApproved = 110,
    MgmtOnHold  = 111,
    MgmtReject  = 112,
    MgmtApproved = 115,
    AccountSubmit = 120,
    L1OnHold    = 121,
    L1Reject    = 122,
    MemoAuditCompleted = 130,
    MemoPrepared = 131,
    MemoApproved = 132,
    MemoAuditReject = 133,
    MemoMgmtReject = 134,
    MemoPaymentInitiated = 135,
    MemoCancelled = 139,
    L1Approved  = 140,
    L2OnHold    = 141,
    L2Reject    = 142,
    L2Approved  = 145,
    PaymentOnHold = 149,
    PaymentApproved = 150,
    PartialPayment = 151,
    PaymentComplete = 152
}