using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoRespAddOrderPl
/// </summary>
namespace MyPacco.API
{
    public class MyPaccoRespAddOrder
    {
        public bool IsSuccess { get; set; }
        public List<MyPaccoRespOrderDeail> Data { get; set; }
        public string Message { get; set; }
    }
    public class MyPaccoRespOrderDeail
    {
        public string lsp_name { get; set; }
        public string awb_number { get; set; }
        public string mypacco_order_id { get; set; }
        public string seller_order_number { get; set; }
        public bool error { get; set; }
        public string error_message { get; set; }
    }
        
}