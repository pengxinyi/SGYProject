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
    public class StockOutBaseSearchReq
    {
        public string start_time { get; set; }

        public string end_time { get; set; }

        public int order_type { get; set; } = 4;//盘点出库

        public string status { get; set; } = "110";//已完成
    }
}
