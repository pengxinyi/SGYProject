using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 盘点出库单
    /// </summary>
    public class QueryStockPdOutRes
    {

        public int status { get; set; }
        public string message { get; set; }

        public QueryStockPdOutData data { get; set; }
    }
    public class QueryStockPdOutData
    {
        public List<QueryStockPdOutOrder> order { get; set; }

        public int total_count { get; set; }
    }

    public class QueryStockPdOutOrder
    {
        public string order_no { get; set; }
        public string warehouse_no { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string consign_time { get; set; }
        public string src_order_no { get; set; }
        public string operator_name { get; set; }
        public List<QueryStockPdOutDetail> detail_list { get; set; }
    }

    public class QueryStockPdOutDetail
    {
        public string goods_no { get; set; }
        public decimal goods_count { get; set; }
        public string remark { get; set; }

        public bool defect { get; set; }
    }
}
