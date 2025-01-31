using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class PurchaseReturnqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }
        public PurchaseReturnqueryData data { get; set; }
    }
    public class PurchaseReturnqueryData
    {
        public List<PurchaseReturnOrderData> order { get; set; }
        public int total_count { get; set; }
    }

    public class PurchaseReturnOrderData
    {
        //public long stockout_id { get; set; }
        public string order_no { get; set; }
        public string src_order_no { get; set; }
        public string warehouse_no { get; set; }
        public string consign_time { get; set; }
        public Byte status { get; set; }
        //public decimal goods_count { get; set; }
        //public string logistics_no { get; set; }
        //public decimal post_fee { get; set; }
        //public string receiver_name { get; set; }
        //public string receiver_province { get; set; }
        //public string receiver_city { get; set; }
      // public string receiver_district { get; set; }
       // public string receiver_address { get; set; }
       // public string receiver_telno { get; set; }
        public string remark { get; set; }
       // public decimal weight { get; set; }
       // public string provider_no { get; set; }
       // public string provider_name { get; set; }
      //  public string last_load_purchase_no { get; set; }
       // public int goods_type_count { get; set; }
        //public string create_time { get; set; }
       // public string operator_name { get; set; }
       // public decimal goods_total_cost { get; set; }
        //public decimal goods_total_amount { get; set; }
        //public decimal checked_goods_total_cost { get; set; }
       // public string modified { get; set; }
        public List<PurchaseReturnOrderDeatail> details_list { get; set; }
    }
    public class PurchaseReturnOrderDeatail
    {
       // public long rec_id { get; set; }
       // public long stockout_id { get; set; }
        public string spec_no { get; set; }
        public decimal goods_count { get; set; }
       // public decimal total_amount { get; set; }
      //  public decimal sell_price { get; set; }
        public string remark { get; set; }
       // public string brand_no { get; set; }
      //  public string brand_name { get; set; }
       // public string goods_name { get; set; }
        public string goods_no { get; set; }
        public bool defect { get; set; }
       // public string spec_name { get; set; }
       // public string spec_code { get; set; }
       /// public decimal cost_price { get; set; }
        //public decimal weight { get; set; }
        //public int goods_type { get; set; }
       // public string goods_unit { get; set; }
       // public string batch_no { get; set; }
       // public string expire_date { get; set; }
      //  public bool defect { get; set; }
       // public string position_no { get; set; }
        //public decimal total_checked_cost_price { get; set; }
       // public string prop1 { get; set; }
       // public string prop2 { get; set; }
       // public List<positiondetailslist> position_details_list { get; set; }

    }
}
