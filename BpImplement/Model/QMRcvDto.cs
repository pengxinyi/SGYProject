using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class QMRcvDto
    {
        public string EntCode { get; set; }
        public string OrgCode { get; set; }
        public string UserCode { get; set; }
        public string OptType { get; set; }
        public RCVDTO RCVDTO { get; set; }
    }
    /// <summary>
    /// 收货单信息
    /// </summary>
    public class RCVDTO
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string CurrencyCode { get; set; }


        /// <summary>
        /// 单号
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 单据日期
        /// </summary>
        public string BusinessDate { get; set; }

        /// <summary>
        /// 仓库员
        /// </summary>
        public string WhManCode { get; set; }

        /// <summary>
        ///供应商
        /// </summary>
        public string SupplyCode { get; set; }

        /// <summary>
        /// 收货单类型
        /// </summary>
       // public string ReceivementType { get; set; }

        /// <summary>
        /// 组织
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 是否默认单据类型
        /// </summary>
       // public string IsDefault { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

       // public long ID { get; set; }

        /// <summary>
        /// 出货行
        /// </summary>
        public List<RCVLineSVDto> RCVLineDto { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

    }

    /// <summary>
    /// 收货单行信息
    /// </summary>
    public class RCVLineSVDto
    {

        public string LineNo { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int SrcPOLineNo { get; set; }

        /// <summary>
        /// 最终价
        /// </summary>
        public string FinallyPriceTC { get; set; }


        /// <summary>
        /// 来源采购订单号
        /// </summary>
        public string SrcPODocNo { get; set; }

        /// <summary>
        /// 入库确认日
        /// </summary>
        public string ConfirmDate { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal PurQtyTU { get; set; }
        /// <summary>
        /// 拒收数量
        /// </summary>
        public decimal RejectQtyCU { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 存储地点
        /// </summary>
        public string WHCode { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string RcvDeptCode { get; set; }

        /// <summary>
        /// 料品编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 库位
        /// </summary>

        public string BinCode { get; set; }
        /// <summary>
        /// 残次品
        /// </summary>

        public bool Defect { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 库管员编码
        /// </summary>
        public string WhMan { get; set; }

        /// <summary>
        /// 批号编码
        /// </summary>
        public string LotCode { get; set; }
        /// <summary>
        /// 番号
        /// </summary>
        public string SeiBanCode { get; set; }
        /// <summary>
        /// 批号生效日期
        /// </summary>
        public string LotCodeEffectiveDate { get; set; }

        /// <summary>
        /// 批号失效日期
        /// </summary>
        public string LotCodExpirationDate { get; set; }
    }
}
