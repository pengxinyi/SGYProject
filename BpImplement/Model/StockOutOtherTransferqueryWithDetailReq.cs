using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockOutOtherTransferqueryWithDetailReq
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// 入库单状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 出库单号
        /// </summary>
        public string other_out_no { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public int time_type { get; set; }
    }
}
