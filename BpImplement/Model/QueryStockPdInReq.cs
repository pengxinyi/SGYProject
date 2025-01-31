using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 盘点入库单查询
    /// </summary>
    public class QueryStockPdInReq
    {
        public string start_time { get; set; }

        public string end_time { get; set; }

        public string status { get; set; } = "80";//已完成

        public int time_type { get; set; } = 1;
    }
}
