using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockinProcessRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockinProcessData data { get; set; }
    }
    public class  StockinProcessData
    {
        public List<StockinProcessOrder> order { get; set; }

        public int total_count { get; set; }
    }

    public class StockinProcessOrder
    {
        public string  stockin_no { get; set; }
        public string warehouse_no { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string consign_time { get; set; }
        public string src_order_type { get; set; }
        public string process_no { get; set; }
        public string operator_name { get; set; }
        public List<StockinProcessDetail> detail_list { get; set; }
    }

    public class  StockinProcessDetail
    {
        public string goods_no { get; set; }
        public decimal num { get; set; }
        public string remark { get; set; }
        public string spec_no { get; set; }
        public bool defect { get; set; }
        public decimal checked_cost_price { get; set; }
    }
}
