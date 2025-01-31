using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class SOReq
    {
        public string EntCode { get; set; } 
        public string OrgCode { get; set; } 
        public string UserCode { get; set; } 
        public string OptType { get; set; } 
        public SOHeadDTO SODt { get; set; }
    }
    public class SOHeadDTO
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 单据日期
        /// </summary>
        public string BusinessDate { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 是否含税
        /// </summary>
        public bool IsPriceIncludeTax { get; set; }
        /// <summary>
        /// 销售订单号
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string Seller { get; set; }

        /// <summary>
        /// 出货规则
        /// </summary>
        public string ShipRule { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string SaleDepartment { get; set; }
        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }
        /// <summary>
        /// 销售订单行
        /// </summary>
        public List<DocLineDTO> DocLine { get; set; }

    }

    /// <summary>
    /// 退回处理行信息
    /// </summary>
    public class DocLineDTO
    {
        /// <summary>
        /// 免费品类型
        /// </summary>
        public string FreeType { get; set; }

        /// <summary>
        /// 番号
        /// </summary>
        public string SeibanCode { get; set; }

        /// <summary>
        /// 行动作
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UOM { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public string DocLineNo { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string ItemInfo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }


        /// <summary>
        /// 最终价
        /// </summary>
        public string FinallyPrice { get; set; }


        /// <summary>
        /// 交期
        /// </summary>
        public string RequireDate { get; set; }


        /// <summary>
        /// 收款条件
        /// </summary>
        public string RecTerm { get; set; }

        /// <summary>
        /// 贸易路径
        /// </summary>
        public string TradePath { get; set; }
        /// <summary>
        /// 存储地点
        /// </summary>

        public string WHCode { get; set; }
        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

        /// <summary>
        /// 销售子行
        /// </summary>
        public List<EditSODocSubLineDTO> SubLine { get; set; }

    }
    /// <summary>
    /// 销售子行信息
    /// </summary>
    public class EditSODocSubLineDTO
    {

        /// <summary>
        /// 子行号
        /// </summary>
        public string DocSubLineNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 交期
        /// </summary>
        public string RequireDate { get; set; }

        /// <summary>
        /// 承诺日期
        /// </summary>
        public string DeliveryDate { get; set; }

        /// <summary>
        /// 计划出货日
        /// </summary>
        public string PlanDate { get; set; }


        /// <summary>
        /// 贸易路径
        /// </summary>
        public string TradePath { get; set; }
    }
    /// <summary>
    /// 共有段，私有段
    /// </summary>
    public class PubPriSVData
    {
      
        #region  私有段
        public string PrivateDescSeg1 { get; set; }
        public string PrivateDescSeg2 { get; set; }
        public string PrivateDescSeg3 { get; set; }
        public string PrivateDescSeg4 { get; set; }
        public string PrivateDescSeg5 { get; set; }
        public string PrivateDescSeg6 { get; set; }
        public string PrivateDescSeg7 { get; set; }
        public string PrivateDescSeg8 { get; set; }
        public string PrivateDescSeg9 { get; set; }
        public string PrivateDescSeg10 { get; set; }
        public string PrivateDescSeg11 { get; set; }
        public string PrivateDescSeg12 { get; set; }

        #endregion
    }
}
