using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    public class TransferFormSVData
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// MES单号
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// 业务日期
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
        /// 形态转换行集合
        /// </summary>
        public List<TransferFormLSVData> TransferFormLineData { get; set; }
    }
    /// <summary>
    /// 行信息
    /// </summary>
    public class TransferFormLSVData
    {
        /// <summary>
        /// 转换类别
        /// </summary>
        public int TransferType { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public string DocLineNo { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 存储地点
        /// </summary>
        public string WhCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal StoreUOMQty { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 合格/不合格
        /// </summary>
        public int StoreType { get; set; }
        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// 经手人
        /// </summary>
        public string HandlePsn { get; set; }

        /// <summary>
        /// 经手部门
        /// </summary>
        public string HandleDept { get; set; }



        /// <summary>
        /// 形态转换子行
        /// </summary>
        public List<TransferFormSLSVData> TransferFormChildDt
        { get; set; }
    }

    /// <summary>
    /// 子行
    /// </summary>
    public class TransferFormSLSVData
    {
        /// <summary>
        /// 转换类别
        /// </summary>
        public int TransferType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// 经手人
        /// </summary>
        public string HandlePsn { get; set; }

        /// <summary>
        /// 经手部门
        /// </summary>
        public string HandleDept { get; set; }

        public string DocSubLineNo { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }


        /// <summary>
        /// 存储地点
        /// </summary>
        public string WhCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal StoreUOMQty { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 合格/不合格
        /// </summary>
        public int StoreType { get; set; }
        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

    }
}
