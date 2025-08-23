using System.Collections.Generic;
namespace MyPacco.API
{
    public class MyPaccoResqAddOrderPl
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
        public string delivery_state { get; set; }
        public string delivery_country { get; set; }
        public int transport_mode { get; set; }
        public int payment_type { get; set; }
        public string currency_unit { get; set; }
        public string pickup_date { get; set; }
        //public bool combined_order { get; set; }

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
        public List<order_items> itemList { get; set; }
        public class order_items
        {
            public string item_title { get; set; }
            public string item_desc { get; set; }
            public int item_quantity { get; set; }
            public int length { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public double weight { get; set; }
            public double base_price { get; set; }
            public double shipp_handling_charges { get; set; }
            public double other_charges { get; set; }
            public double total_amount { get; set; }
        }

        public class combined_order_values
        {
            public int combined_length { get; set; }
            public int combined_height { get; set; }
            public int combined_width { get; set; }
            public double combined_weight { get; set; }
            public double combined_base_price { get; set; }
            public double combined_shipp_handling_charges { get; set; }
            public double combined_other_charges { get; set; }
            public double combined_total_amount { get; set; }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Order
    {
        public string seller_order_number { get; set; }
        public string seller_warehouse_code { get; set; }
        public string pickup_fullname { get; set; }
        public string pickup_mobile { get; set; }
        public string pickup_email { get; set; }
        public string pickup_address { get; set; }
        public string pickup_pincode { get; set; }
        public string pickup_landmark { get; set; }
        public string pickup_city { get; set; }
        public string pickup_state { get; set; }
        public string pickup_country { get; set; }
        public string delivery_fullname { get; set; }
        public string delivery_mobile { get; set; }
        public string delivery_email { get; set; }
        public string delivery_address { get; set; }
        public string delivery_pincode { get; set; }
        public string delivery_landmark { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_country { get; set; }
        public string transport_mode { get; set; }
        public string payment_type { get; set; }
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
        public string currency_unit { get; set; }
        public string pickup_date { get; set; }
    }

    public class Datum
    {
        public bool rts_order { get; set; }
        public List<Order> orders { get; set; }
    }

    public class Root
    {
        public string access_token { get; set; }
        public List<Datum> data { get; set; }
    }

}