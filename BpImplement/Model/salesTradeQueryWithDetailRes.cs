using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class salesTradeQueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }
        public SalesOrdersData data { get; set; }
    }
    public class SalesOrdersData
    {
        public List<SalesOrderData> order { get; set; }
        public int total_count { get; set; }
    }
    public class SalesOrderData
    { 
      public string src_tids { get; set; }
        public string trade_time { get; set; }
        public int trade_type { get; set; }
        public decimal tax_rate { get; set; }
        public string shop_no { get; set; }
        public string status { get; set; }
        public string cs_remark { get; set; }
        public int trade_from { get; set; }
        public int gift_mask { get; set; }
        public string warehouse_no { get; set; }
        public decimal post_amount { get; set; }
        public string trade_no { get; set; }
        public int refund_status { get; set; }
        public string fenxiao_nick { get; set; }
        public string logistics_name { get; set; }
        public string logistics_no { get; set; }
        public string stockout_no { get; set; }
        public string receiver_area { get; set; }
        public string salesman_name { get; set; }
        public List<SalesOrderLineData> detail_list { get; set; }
    }

    public class SalesOrderLineData
    { 
     public string goods_no { get; set; }
        public string spec_no { get; set; }
        public decimal num { get; set; }
        public decimal share_price { get; set; }
        public decimal tax_rate { get; set; }
        public string src_oid { get; set; }
        public string suite_no { get; set; }
        public decimal suite_num { get; set; }
        public decimal commission { get; set; }
        public decimal share_amount { get; set; }
        public string remark { get; set; }
        public decimal share_post_price { get; set; }
    }
}
