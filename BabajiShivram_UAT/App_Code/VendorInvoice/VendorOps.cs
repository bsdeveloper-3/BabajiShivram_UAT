using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for VendorOps
/// </summary>
namespace VendorInvoiceOps
{
    public static class VendorOps
    {        
        public static bool IsFinalInvoicePending(int JobId, ref DataSet dsPending)
        {
            bool isPending = false;

            // Check if any Vendor Final Invoice Pending for Submission against Proforma Invoice 

            dsPending = AccountExpense.CheckFinalInvoicePending("", JobId, 1);

            if (dsPending.Tables.Count > 0)
            {
                if (dsPending.Tables[0].Rows.Count > 0)
                {
                    isPending = true;
                }
            }

            return isPending;

        }

        public static bool IsVendorInvoicePending(int JobId, ref DataSet dsPending)
        {
            bool isPending = false;

            // Check if any Vendor Invoice Pending for Submission 

            dsPending = AccountExpense.CheckVendorInvoicePending(JobId, 1);

            if (dsPending.Tables.Count > 0)
            {
                if (dsPending.Tables[0].Rows.Count > 0)
                {
                    isPending = true;
                }
            }

            return isPending;

        }

        public static bool IsPaymentReceiptPending(int JobId, ref DataSet dsPending)
        {
            bool isPending = false;

            // Check If Vendor Payment Receipt Pending

            int IsVendorReceiptPending = DBOperations.CheckPaymentReceiptPending(JobId, 1);

            if (IsVendorReceiptPending == 1)
            {
                isPending = true;
            }

            return isPending;

        }
    }
}