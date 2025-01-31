using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class rawTradeOrderReq
    {
        public string tid { get; set; }
        public int process_status { get; set; }
        public int trade_status { get; set; }
        public int refund_status { get; set; }
        public int pay_status { get; set; }
        public int order_count { get; set; }
        public decimal goods_count { get; set; }
        public int pay_method { get; set; }
        public string trade_time { get; set; }
        public string pay_time { get; set; }
        public string end_time { get; set; }
        public string buyer_nick { get; set; }
        public string buyer_message { get; set; }
        public string buyer_email { get; set; }
        public string buyer_area { get; set; }
        public string receiver_name { get; set; }
        public string receiver_area { get; set; }
        public string receiver_address { get; set; }
        public string receiver_zip { get; set; }
        public string receiver_mobile { get; set; }
        public string receiver_telno { get; set; }
        public decimal post_amount { get; set; }
        public decimal other_amount { get; set; }
        public decimal discount { get; set; }
        public decimal receivable { get; set; }
        public decimal platform_cost { get; set; }
        public int invoice_type { get; set; }
        public string invoice_title { get; set; }
        public string invoice_content { get; set; }
        public int logistics_type { get; set; }
        public string cust_data { get; set; }
        public int delivery_term { get; set; }
        public string pay_id { get; set; }
        public string remark { get; set; }
        public int remark_flag { get; set; }
        public decimal cod_amount { get; set; }
        public bool is_auto_wms { get; set; }
        public string warehouse_no { get; set; }
        public string pay_account { get; set; }
        public string to_deliver_time { get; set; }
        public decimal received { get; set; }
        public int consign_interval { get; set; }
        public string paid { get; set; }
      
    }
    class rawTradeOrderListReq
    { 
     public string tid { get; set; }
        public string oid { get; set; }
        public int status { get; set; }
        public int refund_status { get; set; }
        public string goods_id { get; set; }
        public string spec_id { get; set; }
        public string goods_no { get; set; }
        public string spec_no { get; set; }
        public string goods_name { get; set; }
        public string spec_name { get; set; }
        public int order_type { get; set; }
        public string cid { get; set; }
        public decimal num { get; set; }
        public decimal price { get; set; }
        public decimal discount { get; set; }
        public decimal share_discount { get; set; }
        public decimal total_amount { get; set; }
        public decimal adjust_amount { get; set; }
        public decimal refund_amount { get; set; }
        public string remark { get; set; }
        public string json { get; set; }
      
    }
}
