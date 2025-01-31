using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFIDA.U9.CBO.DTOs;
using UFIDA.U9.CBO.SCM.PropertyTypes;
using UFIDA.U9.PM.Rcv;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    public class PMRcvToRtn
    {
        public string EntCode { get; set; } 
        public string OrgCode { get; set; } 
        public string UserCode { get; set; } 
        public string OptType { get; set; }
        public RcvToRtnRcvDTO RtnRCVDTO { get; set; }
    }
    public class RcvToRtnRcvDTO
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplyCode { get; set; }

        public string ConfirmDate { get; set; }
        public int Status { get; set; }
        public string BusinessDate { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string OrgCode { get; set; }
        public string CreatedBy { get; set; }
        public string Memo { get; set; }
        public PubPriSVData PubPriDt { get; set; }
        /// <summary>
        /// 退货行
        /// </summary>
        public List<RcvToRtnLineDTO> RcvToRtnLine { get; set; }

    }

    /// <summary>
    /// 采购退货行信息
    /// </summary>
    public class RcvToRtnLineDTO 
    {
        /// <summary>
        /// 退货数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public string FromDocNo { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public string FromDocLineNo { get; set; }
        /// <summary>
        /// 存储地点
        /// </summary>
        public string WHCode { get; set; }
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
        /// 残次品
        /// </summary>
        public bool Defect { get; set; }

        public PubPriSVData PubPriDt { get; set; }
    }

     

  
}
