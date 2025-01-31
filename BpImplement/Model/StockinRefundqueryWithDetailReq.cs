using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinRefundqueryWithDetailReq
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
        /// 退货入库单状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public int time_type { get; set; }
    }
}
