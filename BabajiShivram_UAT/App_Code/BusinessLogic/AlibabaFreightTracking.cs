using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[Serializable]
public class AlibabaFreightTracking
{
    public string lid { get; set; }
    public string EnqRefNo { get; set; }
    public string EnqDate { get; set; }
    public string FinYear { get; set; }
    public int ModeId { get; set; }
    public int TermsId { get; set; }
    public string Mode { get; set; }
    public string Terms { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Consignee { get; set; }
    public string IEC { get; set; }
    public string ShipperAddress { get; set; }
    public string Shipper { get; set; }
    public string ShipperAddPinCode { get; set; }
    public int PortOfLoadingId { get; set; }
    public string PortOfLoading { get; set; }
    public int PortOfDischargeId { get; set; }
    public string PortOfDischarge { get; set; }
    public int ShipmentTypeId { get; set; }
    public string ShipmentType { get; set; }
    public string Commodity { get; set; }
    public string Quantity { get; set; }
    public string DimensionLength { get; set; }
    public string DimensionWidth { get; set; }
    public string DimensionHeight { get; set; }
    public string Dimension { get; set; }
    public string Weight { get; set; }
    public string NoofPkgs { get; set; }
    public string Cont20 { get; set; }
    public string Cont40 { get; set; }
    public string IsDgGoods { get; set; }
    public string ProductLink { get; set; }
    public string HsCode { get; set; }
    public string InvoiceValue { get; set; }
    public string DeliveryAddress { get; set; }
    public string DeliveryAddPincode { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public DateTime StatusDate { get; set; }
    public string DocDir { get; set; }
    public int DocId { get; set; }
    public string DocName { get; set; }
    public string DocPath { get; set; }
    public DateTime UploadedDate { get; set; }
    public string Remarks { get; set; }
    public string lUser { get; set; }
    public string UserName { get; set; }

    public string January { get; set; }
    public string February { get; set; }
    public string March { get; set; }
    public string April { get; set; }
    public string May { get; set; }
    public string June { get; set; }
    public string July { get; set; }
    public string August { get; set; }
    public string September { get; set; }
    public string October { get; set; }
    public string November { get; set; }
    public string December { get; set; }

    public string Enquiry { get; set; }
    public string Quoted { get; set; }
    public string Awarded { get; set; }
    public string Lost { get; set; }
    public string Executed { get; set; }
}