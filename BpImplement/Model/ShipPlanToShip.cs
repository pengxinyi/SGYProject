using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class ShipPlanToShip
    {
        public string EntCode { get; set; } 
        public string OrgCode { get; set; } 
        public string UserCode { get; set; } 
        public string OptType { get; set; } 
        public ShipPlanToShipDTO ShipDTO { get; set; }
    }
    public class ShipPlanToShipDTO
    {
        public int Status { get; set; }
        public string BusinessDate { get; set; }
        public string DocType { get; set; }
        public string OrgCode { get; set; }
        public string CreatedBy { get; set; }
        public string DocNo { get; set; }
        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 出货计划行
        /// </summary>
        public List<ShipPlanToShipLineDTO> ShipLineDt { get; set; }

    }

    /// <summary>
    /// 出货计划行信息
    /// </summary>
    public class ShipPlanToShipLineDTO
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal ShipQty { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public string ShipPlanDocNo { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public int ShipPlanLineNum { get; set; }
        /// <summary>
        /// 匹配行号
        /// </summary>
        public int LineNo { get; set; }
        /// <summary>
        /// 存储地点
        /// </summary>
        public string ShipWH { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string WHMan { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }
        /// <summary>
        /// 库位
        /// </summary>
        public string BinCode { get; set; }
        /// <summary>
        /// 确认日期
        /// </summary>
        public string ConfirmDate { get; set; }
        /// <summary>
        /// 番号
        /// </summary>
        public string SeiBanCode { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalMoneyTC { get; set; }
        /// <summary>
        /// 料品编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

    }
}
