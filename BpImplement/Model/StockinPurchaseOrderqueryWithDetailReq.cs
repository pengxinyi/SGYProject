using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinPurchaseOrderqueryWithDetailReq
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
        public int status { get; set; }

        /// <summary>
        /// 出库单状态详细
        /// </summary>
        [JsonIgnore]
        public string warehouse_no { get; set; }
    }
}
