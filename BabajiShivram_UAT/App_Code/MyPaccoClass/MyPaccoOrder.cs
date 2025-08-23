using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPacco.API
{
    public class MyPaccoOrder
    {
        public string seller_email { get; set; }
        public string seller_order_number { get; set; }
        public string seller_warehouse_code { get; set; }
        public string delivery_fullname { get; set; }
        public string delivery_mobile { get; set; }
        public string delivery_email { get; set; }
        public string delivery_address { get; set; }
        public string delivery_pincode { get; set; }
        public string delivery_landmark { get; set; }
        public string delivery_city { get; set; }
        public string cessRate { get; set; }
        public string delivery_state { get; set; }
        public string delivery_country { get; set; }
        public string transport_mode { get; set; }
        public string payment_type { get; set; }
        public string currency_unit { get; set; }
        public string pickup_date { get; set; }
        public bool combined_order { get; set; }
    }

    public class MyPaccoOrderItem
    {
        public string item_title { get; set; }
        public string item_desc { get; set; }
        public string item_quantity { get; set; }
        public string length { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string weight { get; set; }
        public string base_price { get; set; }
        public string shipp_handling_charges { get; set; }
        public string other_charges { get; set; }
        public string total_amount { get; set; }

    }
}