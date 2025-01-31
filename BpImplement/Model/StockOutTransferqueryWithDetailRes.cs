using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockOutTransferqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockOutTransferData data { get; set; }
    }
    public class StockOutTransferData
    {
        public List<StockOutTransferOrderData> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockOutTransferOrderData
    {
        public int stockout_id { get; set; }
        public string order_no { get; set; }
        public string src_order_no { get; set; }
        public string warehouse_no { get; set; }
        public decimal consign_time { get; set; }
        public decimal post_fee { get; set; }
        public int status { get; set; }
        public int goods_count { get; set; }
        public string logistics_no { get; set; }
        public decimal package_fee { get; set; }
        public string receiver_name { get; set; }
        public int receiver_country { get; set; }
        public int receiver_province { get; set; }
        public int receiver_city { get; set; }
        public int receiver_district { get; set; }
        public string receiver_address { get; set; }
        public string receiver_mobile { get; set; }
        public string receiver_telno { get; set; }
        public string receiver_zip { get; set; }
        public string remark { get; set; }
        public string weight { get; set; }
        public string st_created { get; set; }
        public string from_warehouse_no { get; set; }
        public string to_warehouse_no { get; set; }
        public string operator_name { get; set; }
        public decimal goods_total_cost { get; set; }
        public List<StockOutTransferOrderDeatail> detail_list { get; set; }
    }
    public class StockOutTransferOrderDeatail
    {
        public int rec_id { get; set; }
        public int stockout_id { get; set; }
        public decimal goods_count { get; set; }
        public string remark { get; set; }
        public string brand_no { get; set; }
        public string brand_name { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public string spec_no { get; set; }
        public string spec_name { get; set; }
        public string spec_code { get; set; }
        public decimal cost_price { get; set; }
        public decimal total_amount { get; set; }
        public decimal weight { get; set; }
        public int goods_type { get; set; }
        public string std_remark { get; set; }
        public string goods_unit { get; set; }
        public string unit_name { get; set; }
        public string batch_no { get; set; }
        public string expire_date { get; set; }
        public bool defect { get; set; }
        public string position_no { get; set; }
        public List<positiondetailslist> position_details_list { get; set; }


    }
}
