using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinPreStockinqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }
        public StockinPreStockData data { get; set; }
    }
    public class StockinPreStockData
    {
        public List<StockinPreStockOrderData> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockinPreStockOrderData
    {
        public long stockin_id { get; set; }
        public string stockin_no { get; set; }
        public string warehouse_no { get; set; }
        public string src_order_no { get; set; }
        public int src_order_type { get; set; }
        public string logistics_no { get; set; }
        public string logistics_name { get; set; }
        public int status { get; set; }
        public decimal goods_count { get; set; }
        public int goods_type_count { get; set; }
        public int note_count { get; set; }
        public string flag_name { get; set; }
        public string operator_name { get; set; }
        public decimal checker_name { get; set; }
        public string remark { get; set; }
        public DateTime check_time { get; set; }
        public TimeSpan created { get; set; }
        public string prop1 { get; set; }
        public string prop2 { get; set; }
        public int logistics_id { get; set; }
        public int src_order_id { get; set; }
        public string modified_date { get; set; }
        public string created_date { get; set; }
        public int warehouse_id { get; set; }
        public List<StockinPreStockOrderDeatail> detail_list { get; set; }
    }
    public class StockinPreStockOrderDeatail
    {
        public long stockin_id { get; set; }
        public decimal num { get; set; }
        public decimal num2 { get; set; }
        public decimal expect_num { get; set; }
        public string remark { get; set; }
        public long rec_id { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public string spec_no { get; set; }
        public string spec_name { get; set; }
        public bool defect { get; set; }
        public string batch_no { get; set; }
        public string expire_date { get; set; }
        public string production_date { get; set; }
        public decimal weight { get; set; }
        public decimal goods_weight { get; set; }
        public decimal unit_ratio { get; set; }
        public int validity_days { get; set; }
        public decimal need_inspect_num { get; set; }
        public string unit_name { get; set; }
        public string aux_unit_name { get; set; }
        public string position_no { get; set; }
        public int goods_id { get; set; }
        public int spec_id { get; set; }
        public string modified_date { get; set; }
        public string created_date { get; set; }
    }
}
