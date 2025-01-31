using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class AfterSalesOrderqueryWithDetailReq
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string modified_from { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string  modified_to { get; set; }

        /// <summary>
        /// 退换单状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 退换单类型 4
        /// </summary>
      //  public int  type { get; set; }
        /// <summary>
        /// 退换单号
        /// </summary>
        public string refund_no { get; set; }
    }
}
