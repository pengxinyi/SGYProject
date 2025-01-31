using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class SrcShipRMA
    {
        public string EntCode { get; set; } 
        public string OrgCode { get; set; } 
        public string UserCode { get; set; } 
        public string OptType { get; set; } 
        public List<SrcShipRMALineDTO> ShipData { get; set; }
    }

    /// <summary>
    /// 退回处理行信息
    /// </summary>
    public class SrcShipRMALineDTO
    {
        /// <summary>
        /// 来源出货单号
        /// </summary>
        public string SrcDocNo { get; set; }

        /// <summary>
        /// 来源行号
        /// </summary>
        public string DocLine { get; set; }

        /// <summary>
        /// 退货数量
        /// </summary>
        public decimal Num { get; set; }
        /// <summary>
        /// 退回处理单号
        /// </summary>

        public string DocNo { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WHCode { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public string DocType { get; set; }
        public string CreateBy { get; set; }
        public bool Defect { get; set; }
        public string LogisticsName { get; set; }
        public string LogisticsNo { get; set; }
        public string BusinessDate { get; set; }
        public string OrderNo { get; set; }
    }
}
