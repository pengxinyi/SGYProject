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
    public class QueryStockPdOutReq
    {
        public string start_time { get; set; }

        public string end_time { get; set; }

        public string status { get; set; } = "110";//已完成

        public int time_type { get; set; } = 1;
    }
}
