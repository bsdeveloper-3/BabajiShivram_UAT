using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPacco.API
{
    public class ReqOrderNo
    {
        public List<string> tracking_number { get; set; }
    }

    public class MyPaccoResqTrackOrder
    {
        public string access_token { get; set; }
        public List<ReqOrderNo> data { get; set; }
    }
}
