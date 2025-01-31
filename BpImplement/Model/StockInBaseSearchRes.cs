using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{

    /// <summary>
    ///  汇总入库单【入库单查询】
    /// </summary>
    public class StockInBaseSearchRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockInBaseSearchData data { get; set; }
    }

    public class StockInBaseSearchData
    {
        public List<StockInBaseSearchOrder> order_list { get; set; }

        public int total_count { get; set; }
    }

    public class StockInBaseSearchOrder
    {
        public string stockin_no { get; set; }
        public string warehouse_no { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string check_time { get; set; }
        public string src_order_type { get; set; }
        public string src_order_no { get; set; }
        public string operator_name { get; set; }
        public List<StockInBaseSearchDetail> detail_list { get; set; }
    }

    public class StockInBaseSearchDetail
    {
        public string goods_no { get; set; }
        public decimal num { get; set; }
        public string remark { get; set; }
        public string warehouse_no { get; set; }
        public bool defect { get; set; }
        public string spec_no { get; set; }
    }

}
