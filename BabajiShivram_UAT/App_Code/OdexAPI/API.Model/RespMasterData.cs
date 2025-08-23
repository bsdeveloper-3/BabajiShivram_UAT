using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdexConnect
{    
    public class Beneficiary
    {
        public string bnfCode { get; set; }
        public string bnfName { get; set; }
    }

    public class Location
    {
        public string locationNm { get; set; }
        public string locationCode { get; set; }
    }

    public class RespMasterData
    {
        public List<Beneficiary> Beneficiaries { get; set; }
        public List<Location> Location { get; set; }
    }
}
