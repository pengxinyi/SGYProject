using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockinOtherTransferqueryWithDetailReq
    {
        /// <summary>
        /// 1：创建时间；2：最后修改时间
        /// </summary>
        public int time_type { get; set; } = 2;

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
        /// 调拨单号
        /// </summary>
        public string other_in_no { get; set; }

        /// <summary>
        /// 出库单状态详细
        /// </summary>
        [JsonIgnore]
        public string warehouse_no { get; set; }
    }
}
