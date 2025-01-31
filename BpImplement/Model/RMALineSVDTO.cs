using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 退回处理行信息
    /// </summary>
    internal class RMALineSVDTO
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string DocLineNo { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string ItemInfo { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UOM { get; set; }

        /// <summary>
        /// 存储地点
        /// </summary>
        public string WareHouse { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 定价
        /// </summary>
        public string OrderPrice { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }
    }
}
