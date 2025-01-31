using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockProcessReq
    {
        public string start_time { get; set; }

        public string end_time { get; set; }

        public int status { get; set; } = 70;//已完成
    }
}
