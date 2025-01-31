using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
  
    public class TransferToIn
    {
        public string EntCode { get; set; }
        public string OrgCode { get; set; }
        public string UserCode { get; set; }
        public string OptType { get; set; }
        public TransferOneInDTO TransInDTO { get; set; }
    }
    public class TransferOneInDTO
    {

        public string DocType { get; set; }

        public string CreatedBy { get; set; }
        public int Status { get; set; }
        public string  BussinessDate { get; set; }
        public int TransferInType { get; set; }
        public int TransferDirection { get; set; }
        public string DocNo { get; set; }
        public string Memo { get; set; }
        public PubPriSVData PubPriDt { get; set; }
        /// <summary>
        /// 退货行
        /// </summary>
        public List<TransInOneLineDTO> TransInLineData { get; set; }

    }

    /// <summary>
    /// 采购退货行信息
    /// </summary>
    public class  TransInOneLineDTO
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNum { get; set; }

        /// <summary>
        /// 调入组织
        /// </summary>
        public string RcvOrg { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 番号
        /// </summary>
        public string SeibanCode { get; set; }


        /// <summary>
        /// 调入仓库
        /// </summary>
        public string RcvWH { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string BinCode { get; set; }


        /// <summary>
        /// 调入数量
        /// </summary>
        public decimal RcvQty { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }


        /// <summary>
        /// 调入单子行
        /// </summary>
        public List<TransInChildLineSVData> TransInSubLines { get; set; }
    }

    /// <summary>
    /// 子行
    /// </summary>
    public class TransInChildLineSVData
    {
        /// <summary>
        /// 番号
        /// </summary>
        public string SeibanCode { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNum { get; set; }

        /// <summary>
        /// 调出组织
        /// </summary>
        public string OutOrg { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 调出仓库
        /// </summary>
        public string ShipWH { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string BinCode { get; set; }

        /// <summary>
        /// 调出数量
        /// </summary>
        public decimal ShipQty { get; set; }



        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }
    }
}
