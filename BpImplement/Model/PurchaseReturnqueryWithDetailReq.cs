using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class PurchaseReturnqueryWithDetailReq
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
        /// 采购退货单状态
        /// </summary>
        public string status { get; set; }   
        /// <summary>
        /// 时间类型：1 出库时间 2 创建时间
        /// </summary>
        public int time_type { get; set; }
    }
}
