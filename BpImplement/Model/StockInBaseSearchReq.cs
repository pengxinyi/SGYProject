using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 汇总入库单【入库单查询】
    /// </summary>
    public class StockInBaseSearchReq
    {
        public string start_time { get; set; }

        public string end_time { get; set; }

        public int order_type { get; set; } = 4;//盘点入库

        public string status { get; set; } = "80";//已完成
    }
}
