using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class PurchaseOrderqueryWithDetailReq
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
        /// 采购单状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 采购单号
        /// </summary>
        public string purchase_no { get; set; }
    }
}
