using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockoutProcessReq
    {
        public string start_time { get; set; }

        public string end_time { get; set; }

        public int  time_type { get; set; } = 3;//最后修改时间

        public string status { get; set; } = "110";//已完成
    }
}
