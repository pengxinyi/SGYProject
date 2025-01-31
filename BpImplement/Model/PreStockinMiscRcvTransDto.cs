using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class PreStockinMiscRcvTransDto
    {
        public string EntCode { get; set; } 
        public string OrgCode { get; set; }
        public string UserCode { get; set; }
        public string OptType { get; set; }
        public MiscRcvSVData MiscRcvDTO { get; set; }
    }
    /// <summary>
    /// 杂收单信息
    /// </summary>
    public class MiscRcvSVData
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// MES单号
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 组织
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string WHMan { get; set; }
        /// <summary>
        /// 单据日期
        /// </summary>
        public string BussinessDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }



        /// <summary>
        /// 杂发单行集合
        /// </summary>
        public List<MiscRcvLineData> MiscRcvLineDt { get; set; }
    }
    /// <summary>
    /// 杂发单行信息
    /// </summary>
    public class MiscRcvLineData
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNum { get; set; }

        /// <summary>
        /// MO单号
        /// </summary>
        public string MODocNo { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 杂收数量
        /// </summary>
        public decimal RcvQty { get; set; }
        /// <summary>
        /// 杂收单价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 批号生效时间
        /// </summary>
        public string LotCodeEffectiveDate { get; set; }

        /// <summary>
        /// 批号失效时间
        /// </summary>
        public string LotCodExpirationDate { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 入库仓库
        /// </summary>
        public string RcvWH { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }


        /// <summary>
        /// 库位
        /// </summary>
        public string TransBin { get; set; }
        /// <summary>
        /// 番号
        /// </summary>
        public string SeiBanCode { get; set; }
        /// <summary>
        /// 行操作类型
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 存储类型
        /// </summary>
        public int StoreType { get; set; }

    }
}
