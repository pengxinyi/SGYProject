using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 盘点入库单
    /// </summary>
    public class QueryStockPdInRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public QueryStockPdInData data { get; set; }
    }
    public class QueryStockPdInData
    {
        public List<QueryStockPdInOrder> order { get; set; }

        public int total_count { get; set; }
    }

    public class QueryStockPdInOrder
    {
        public string order_no { get; set; }
        public string warehouse_no { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string check_time { get; set; }
        public string src_order_no { get; set; }
        public string operator_name { get; set; }
        public List<QueryStockPdInDetail> detail_list { get; set; }
    }

    public class QueryStockPdInDetail
    {
        public string goods_no { get; set; }
        public decimal goods_count { get; set; }
        public string remark { get; set; }
        public long rec_id { get; set; }
        public bool defect { get; set; }
    }
}
