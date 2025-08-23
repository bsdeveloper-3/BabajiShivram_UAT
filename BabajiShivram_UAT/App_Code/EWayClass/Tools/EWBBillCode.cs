
using System.Collections.Generic;

namespace TaxProEWB.API
{
    public static class EWBBillCode
    {
    public static Dictionary<string, string> EwbBillCodes = new Dictionary<string, string>()
    {
      {
        "I",
        "Inward"
      },
      {
        "O",
        "Outward"
      },
      {
        "1",
        "Supply"
      },
      {
        "2",
        "Import"
      },
      {
        "3",
        "Export"
      },
      {
        "4",
        "Job Work"
      },
      {
        "5",
        "For Own Use"
      },
      {
        "6",
        "Job work Returns"
      },
      {
        "7",
        "Sales Return"
      },

      {
        "8",
        "Others"
      },
      {
        "9",
        "SKD/CKD"
      },
      {
        "10",
        "Line Sales"
      },
      {
        "11",
        "Recipient Not Known"
      },
      {
        "12",
        "Exhibition or Fairs"
      },
    };
    }
}
