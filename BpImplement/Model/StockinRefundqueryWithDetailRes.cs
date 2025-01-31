using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinRefundqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }
        public StockinRefundData data { get; set; }
    }
    public class StockinRefundData
    {
        public List<StockinRefundOrderData> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockinRefundOrderData
    {
        public string order_no { get; set; }
        public int status { get; set; }
        public int attach_type { get; set; } = -1;
        public string warehouse_no { get; set; }
        public string warehouse_name { get; set; }
        public string created_time { get; set; }
        public string remark { get; set; }
        public string fenxiao_nick { get; set; }
        public string operator_name { get; set; }
        public int operator_id { get; set; }
        public int logistics_type { get; set; }
        public string logistics_name { get; set; }
        public string logistics_no { get; set; }
        public int logistics_id { get; set; }
        public string check_time { get; set; }
        public string refund_no { get; set; }
        public decimal goods_count { get; set; }
        public decimal actual_refund_amount { get; set; }
        public string customer_no { get; set; }
        public string customer_name { get; set; }
        public string nick_name { get; set; }
        public string shop_name { get; set; }
        public string shop_no { get; set; }
        public string shop_remark { get; set; }
        public string flag_name { get; set; }
        public string trade_no_list { get; set; }
        public string tid_list { get; set; }
        public int src_order_id { get; set; }
        public int stockin_id { get; set; }
        public int shop_platform_id { get; set; }
        public int sub_platform_id { get; set; }
        public int shop_id { get; set; }
        public int warehouse_id { get; set; }
        public decimal total_price { get; set; }
        public decimal total_goods_stockin_num { get; set; }
        public int process_status { get; set; }
        public string modified { get; set; }
        public string check_operator_name { get; set; }
        public int check_operator_id { get; set; }
        public string reason { get; set; }
        public int reason_id { get; set; }
        public decimal refund_amount { get; set; }
        public decimal adjust_num { get; set; }
        public string created { get; set; }
        public int flag_id { get; set; }
        public int goods_type_count { get; set; }
        public string src_order_no { get; set; }
        public int note_count { get; set; }
        public int src_order_type { get; set; }
        public List<StockinRefundOrderDeatail> details_list { get; set; }
    }
    public class StockinRefundOrderDeatail
    {
        public int rec_id { get; set; }
        public int stockin_id { get; set; }
        public string refund_detail_id { get; set; }
        public decimal total_cost { get; set; }
        public decimal num { get; set; }
        public decimal right_num { get; set; }
        public string remark { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public int goods_id { get; set; }
        public int spec_id { get; set; }
        public bool defect { get; set; }
        public string prop2 { get; set; }
        public string spec_name { get; set; }
        public string spec_code { get; set; }
        public string brand_no { get; set; }
        public string brand_name { get; set; }
        public string goods_unit { get; set; }
        public string logistics_name { get; set; }
        public string logistics_no { get; set; }
        public int warehouse_id { get; set; }
        public int src_order_id { get; set; }
        public int logistics_id { get; set; }
        public string base_unit_name { get; set; }
        public string batch_no { get; set; }
        public string expire_date { get; set; }
        public string production_date { get; set; }
        public string position_no { get; set; }
        public decimal expect_num { get; set; }
        public decimal stockin_num { get; set; }
        public decimal checked_cost_price { get; set; }
        public decimal refund_amount { get; set; }
        public string sn_list { get; set; }
        public int unit_id { get; set; }
        public int base_unit_id { get; set; }
        public int org_stockin_detail_id { get; set; }
        public int batch_id { get; set; }
        public int position_id { get; set; }
        public int validity_days { get; set; }
        public decimal num2 { get; set; }
        public decimal adjust_num { get; set; }
        public decimal unit_ratio { get; set; }
        public string modified { get; set; }
        public string created { get; set; }
        public int src_order_type { get; set; }
        public List<refund_order_detail_list> refund_order_detail_list { get; set; }



    }
    public class refund_order_detail_list
    { 
      public int refund_order_id { get; set; }
        public int stockin_order_detail_id { get; set; }
        public decimal price { get; set; }
        public string spec_no { get; set; }
        public string oid { get; set; }
    }
}
