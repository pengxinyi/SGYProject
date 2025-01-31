using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 退料信息
    /// </summary>
    public class PMIssueDTO
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
        /// 领料日期
        /// </summary>
        public string IssueDate { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }


        /// <summary>
        /// 部门
        /// </summary>
        public string HandleDeptCode { get; set; }
        public string HandleDeptName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 领料行集合
        /// </summary>
        public List<PMIssueLineData> PMIssueLineDTO { get; set; }
    }
    /// <summary>
    /// 领料行集合
    /// </summary>
    public class PMIssueLineData
    {


        /// <summary>
        /// 行号
        /// </summary>
        public string LineNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// 来源单号
        /// </summary>
        public string SrcDocNo { get; set; }

        /// <summary>
        /// 来源行号
        /// </summary>
        public string SrcLineNo { get; set; }


        /// <summary>
        /// 备料行号
        /// </summary>
        public string PickLineNo { get; set; }

        /// <summary>
        /// 领料数量
        /// </summary>
        public decimal IssueQty { get; set; }



        /// <summary>
        /// 领料仓库
        /// </summary>
        public string IssueWH { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 来源收货行ID
        /// </summary>
        public long SrcLineID { get; set; }


        /// <summary>
        /// 共有段私有段
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

    }
}
