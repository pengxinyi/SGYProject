using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 出库明细【出库单查询】
    /// </summary>
    public class StockOutBaseSearchRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockOutBaseSearchData data { get; set; }
    }
    public class StockOutBaseSearchData
    {
        public List<StockOutBaseSearchOrder> order_list { get; set; }

        public int total_count { get; set; }
    }

    public class StockOutBaseSearchOrder
    {
        public string stockout_no { get; set; }
        public string warehouse_no { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string check_time { get; set; }
        public string src_order_type { get; set; }
        public string src_order_no { get; set; }
        public string operator_name { get; set; }
        public string consign_time { get; set; }
        public List<StockOutBaseSearchDetail> detail_list { get; set; }
    }

    public class StockOutBaseSearchDetail
    {
        public string goods_no { get; set; }
        public decimal num { get; set; }
        public string remark { get; set; }
        public string warehouse_no { get; set; }
        public string spec_no { get; set; }
        public bool defect { get; set; }
    }
}
