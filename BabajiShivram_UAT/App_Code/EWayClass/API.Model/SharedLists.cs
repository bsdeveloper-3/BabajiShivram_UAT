// Decompiled with JetBrains decompiler
// Type: TaxProEWB.API.SharedLists
// Assembly: TaxProEWB.API, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 651C2957-9A00-43E1-9864-7C8CEF88DD73
// Assembly location: C:\inetpub\wwwroot\TaxProEWBApiIntigrationDemo.NET\bin\x86\Debug\TaxProEWB.API.dll

using System.Collections.Generic;

namespace TaxProEWB.API
{
  public static class SharedLists
  {
    public static IList<ListCodeItem> SupplyTypes = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "Inward", Code = "I" },
      new ListCodeItem() { Description = "Outward", Code = "O" }
    };
    public static IList<ListCodeItem> SubSupplyTypes = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "Supply", Code = "1" },
      new ListCodeItem() { Description = "Import", Code = "2" },
      new ListCodeItem() { Description = "Export", Code = "3" },
      new ListCodeItem() { Description = "Job Work", Code = "4" },
      new ListCodeItem()
      {
        Description = "For Own Use",
        Code = "5"
      },
      new ListCodeItem()
      {
        Description = "Job work Returns",
        Code = "6"
      },
      new ListCodeItem()
      {
        Description = "Sales Return",
        Code = "7"
      },
      new ListCodeItem() { Description = "Others", Code = "8" },
      new ListCodeItem() { Description = "SKD/CKD", Code = "9" },
      new ListCodeItem()
      {
        Description = "Line Sales",
        Code = "10"
      },
      new ListCodeItem()
      {
        Description = "Recipient  Not Known",
        Code = "11"
      },
      new ListCodeItem()
      {
        Description = "Exhibition or Fairs",
        Code = "12"
      }
    };
    public static IList<ListCodeItem> DocumentTypes = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem()
      {
        Description = "Tax Invoice",
        Code = "INV"
      },
      new ListCodeItem()
      {
        Description = "Bill of Supply",
        Code = "BIL"
      },
      new ListCodeItem()
      {
        Description = "Bill of Entry",
        Code = "BOE"
      },
      new ListCodeItem()
      {
        Description = "Delivery Challan",
        Code = "CHL"
      },
      new ListCodeItem()
      {
        Description = "Credit Note",
        Code = "CNT"
      },
      new ListCodeItem() { Description = "Others", Code = "OTH" }
    };
    public static IList<ListCodeItem> TransportationModes = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "Road", Code = "1" },
      new ListCodeItem() { Description = "Rail", Code = "2" },
      new ListCodeItem() { Description = "Air", Code = "3" },
      new ListCodeItem() { Description = "Ship", Code = "4" }
    };
    public static IList<ListCodeItem> UnitList = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "BAGS", Code = "BAG" },
      new ListCodeItem() { Description = "BALE", Code = "BAL" },
      new ListCodeItem() { Description = "BUNDLES", Code = "BDL" },
      new ListCodeItem() { Description = "BUCKLES", Code = "BKL" },
      new ListCodeItem()
      {
        Description = "BILLION OF UNITS",
        Code = "BOU"
      },
      new ListCodeItem() { Description = "BOX", Code = "BOX" },
      new ListCodeItem() { Description = "BOTTLES", Code = "BTL" },
      new ListCodeItem() { Description = "BUNCHES", Code = "BUN" },
      new ListCodeItem() { Description = "CANS", Code = "CAN" },
      new ListCodeItem()
      {
        Description = "CUBIC METERS",
        Code = "CBN"
      },
      new ListCodeItem()
      {
        Description = "CUBIC CENTIMETERS",
        Code = "CCN"
      },
      new ListCodeItem()
      {
        Description = "CENTI METERS",
        Code = "CMS"
      },
      new ListCodeItem() { Description = "METERS", Code = "MTR" },
      new ListCodeItem() { Description = "CARTONS", Code = "CTN" },
      new ListCodeItem() { Description = "DOZENS", Code = "DOZ" },
      new ListCodeItem() { Description = "DRUMS", Code = "DRM" },
      new ListCodeItem()
      {
        Description = "GREAT GROSS",
        Code = "GGK"
      },
      new ListCodeItem() { Description = "GRAMMES", Code = "GMS" },
      new ListCodeItem() { Description = "GROSS", Code = "GRS" },
      new ListCodeItem()
      {
        Description = "GROSS YARDS",
        Code = "GYD"
      },
      new ListCodeItem()
      {
        Description = "KILOGRAMS",
        Code = "KGS"
      },
      new ListCodeItem()
      {
        Description = "KILOLITRE",
        Code = "KLR"
      },
      new ListCodeItem()
      {
        Description = "KILOMETRE",
        Code = "KME"
      },
      new ListCodeItem()
      {
        Description = " MILILITRE",
        Code = "MLT"
      },
      new ListCodeItem()
      {
        Description = "METRIC TON",
        Code = "MTS"
      },
      new ListCodeItem() { Description = "NUMBERS", Code = "NOS" },
      new ListCodeItem() { Description = "OTHERS", Code = "OTH" },
      new ListCodeItem() { Description = "PACKS", Code = "PAC" },
      new ListCodeItem() { Description = "PIECES", Code = "PCS" },
      new ListCodeItem() { Description = "PAIRS", Code = "PRS" },
      new ListCodeItem() { Description = "QUINTAL", Code = "QTL" },
      new ListCodeItem() { Description = "ROLLS", Code = "ROL" },
      new ListCodeItem() { Description = "SETS", Code = "SET" },
      new ListCodeItem()
      {
        Description = "SQUARE FEET",
        Code = "SQF"
      },
      new ListCodeItem()
      {
        Description = "SQUARE METERS",
        Code = "SQM"
      },
      new ListCodeItem()
      {
        Description = "SQUARE YARDS",
        Code = "SQY"
      },
      new ListCodeItem() { Description = "TABLETS", Code = "TBS" },
      new ListCodeItem()
      {
        Description = "TEN GROSS",
        Code = "TGM"
      },
      new ListCodeItem()
      {
        Description = "THOUSANDS",
        Code = "THD"
      },
      new ListCodeItem() { Description = "TONNES", Code = "TON" },
      new ListCodeItem() { Description = "TUBES", Code = "TUB" },
      new ListCodeItem()
      {
        Description = "US GALLONS",
        Code = "UGS"
      },
      new ListCodeItem() { Description = "UNITS", Code = "UNT" },
      new ListCodeItem() { Description = "YARDS", Code = "YDS" }
    };
    public static IList<ListCodeItem> StateCodeList = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem()
      {
        Description = "Andaman and Nicobar Islands",
        Code = "35"
      },
      new ListCodeItem()
      {
        Description = "Andhra Pradesh (New)",
        Code = "37"
      },
      new ListCodeItem()
      {
        Description = "Arunachal Pradesh",
        Code = "12"
      },
      new ListCodeItem() { Description = "Assam", Code = "18" },
      new ListCodeItem() { Description = "Bihar", Code = "10" },
      new ListCodeItem()
      {
        Description = "Chandigarh",
        Code = "4"
      },
      new ListCodeItem()
      {
        Description = "Chattisgarh",
        Code = "22"
      },
      new ListCodeItem()
      {
        Description = "Dadra and Nagar Haveli",
        Code = "26"
      },
      new ListCodeItem()
      {
        Description = "Daman and Diu",
        Code = "25"
      },
      new ListCodeItem() { Description = "Delhi", Code = "7" },
      new ListCodeItem() { Description = "Goa", Code = "30" },
      new ListCodeItem() { Description = "Gujarat", Code = "24" },
      new ListCodeItem() { Description = "Haryana", Code = "6" },
      new ListCodeItem()
      {
        Description = "Himachal Pradesh",
        Code = "2"
      },
      new ListCodeItem()
      {
        Description = "Jammu and Kashmir",
        Code = "1"
      },
      new ListCodeItem()
      {
        Description = "Jharkhand",
        Code = "20"
      },
      new ListCodeItem()
      {
        Description = "Karnataka",
        Code = "29"
      },
      new ListCodeItem() { Description = "Kerala", Code = "32" },
      new ListCodeItem()
      {
        Description = "Lakshadweep Islands",
        Code = "31"
      },
      new ListCodeItem()
      {
        Description = "Madhya Pradesh",
        Code = "23"
      },
      new ListCodeItem()
      {
        Description = "Maharashtra",
        Code = "27"
      },
      new ListCodeItem() { Description = "Manipur", Code = "14" },
      new ListCodeItem()
      {
        Description = "Meghalaya",
        Code = "17"
      },
      new ListCodeItem() { Description = "Mizoram", Code = "15" },
      new ListCodeItem() { Description = "Nagaland", Code = "13" },
      new ListCodeItem() { Description = "Odisha", Code = "21" },
      new ListCodeItem()
      {
        Description = "Other Territory",
        Code = "97"
      },
      new ListCodeItem()
      {
        Description = "Pondicherry",
        Code = "34"
      },
      new ListCodeItem() { Description = "Punjab", Code = "3" },
      new ListCodeItem() { Description = "Rajasthan", Code = "8" },
      new ListCodeItem() { Description = "Sikkim", Code = "11" },
      new ListCodeItem()
      {
        Description = "Tamil Nadu",
        Code = "33"
      },
      new ListCodeItem()
      {
        Description = "Telangana",
        Code = "36"
      },
      new ListCodeItem() { Description = "Tripura", Code = "16" },
      new ListCodeItem()
      {
        Description = "Uttar Pradesh",
        Code = "9"
      },
      new ListCodeItem()
      {
        Description = "Uttarakhand",
        Code = "5"
      },
      new ListCodeItem()
      {
        Description = "West Bengal",
        Code = "19"
      },
      new ListCodeItem()
      {
        Description = "OTHER COUNTRY",
        Code = "99"
      }
    };
    public static IList<ListCodeItem> VehicleUpdateReasonCodes = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem()
      {
        Description = "Due to Break Down",
        Code = "1"
      },
      new ListCodeItem()
      {
        Description = "Due to Transhipment",
        Code = "2"
      },
      new ListCodeItem()
      {
        Description = "Others (Pls. Specify)",
        Code = "3"
      },
      new ListCodeItem()
      {
        Description = "First Time",
        Code = "4"
      }
    };
    public static IList<ListCodeItem> ModeOfGenerationCode = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem()
      {
        Description = "Application Programming Interface",
        Code = "API"
      },
      new ListCodeItem()
      {
        Description = "Bulk Upload",
        Code = "Exc"
      },
      new ListCodeItem()
      {
        Description = "SMS Facility",
        Code = "SMS"
      },
      new ListCodeItem()
      {
        Description = "Mobile APP",
        Code = "APP"
      },
      new ListCodeItem()
      {
        Description = "Web based system",
        Code = "WEB"
      }
    };
    public static IList<ListCodeItem> EwayBillStatus = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "Active", Code = "ACT" },
      new ListCodeItem()
      {
        Description = "Cancelled",
        Code = "CNL"
      }
    };
    public static IList<ListCodeItem> VehicleType = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "Regular", Code = "R" },
      new ListCodeItem()
      {
        Description = "Over Dimensional Cargo",
        Code = "O"
      }
    };
    public static IList<ListCodeItem> CancelationReasonCode = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem() { Description = "Duplicate", Code = "1" },
      new ListCodeItem()
      {
        Description = "Order Cancelled",
        Code = "2"
      },
      new ListCodeItem()
      {
        Description = "Data Entry mistake",
        Code = "3"
      },
      new ListCodeItem() { Description = "Others", Code = "4" }
    };
    public static IList<ListCodeItem> ReasonForExtentionOfValidity = (IList<ListCodeItem>) new List<ListCodeItem>()
    {
      new ListCodeItem()
      {
        Description = "Natural Calamity",
        Code = "1"
      },
      new ListCodeItem()
      {
        Description = "Law and Order Situation",
        Code = "2"
      },
      new ListCodeItem()
      {
        Description = "Transshipment",
        Code = "4"
      },
      new ListCodeItem() { Description = "Accident", Code = "5" },
      new ListCodeItem() { Description = "Others", Code = "99" }
    };
  }
}
