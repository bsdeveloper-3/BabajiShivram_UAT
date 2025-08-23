using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPacco.API
{
    public class MyPaccoRespTrackOrder
    {
        public bool IsSuccess { get; set; }
        public List<TrackOrderResult> Data { get; set; }
        public string Message { get; set; }

    }

    public class TrackOrderResult
    {
        public string AWBNumber { get; set; }
        public string MyPaccoId { get; set; }
        public List<TrackingStatus> TrackingStatus { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class TrackingStatus
    {
        public string status { get; set; }
        public string status_code { get; set; }
        public string date { get; set; }
        public string location { get; set; }
        public string supplier_name { get; set; }
        public string weight_unit { get; set; }
        public string weight { get; set; }
        public string final_weight { get; set; }
    }
}
