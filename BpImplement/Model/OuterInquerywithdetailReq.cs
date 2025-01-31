using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class OuterInquerywithdetailReq
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
        /// 出库类型 0 调整入库
        /// </summary>
        //public int src_order_type { get; set; }
        ///外仓入库单号
        public string outer_in_no { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public int time_type { get; set; }
    }
}
