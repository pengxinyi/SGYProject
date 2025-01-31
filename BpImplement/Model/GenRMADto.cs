using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFIDA.U9.SM.RMA;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 退回处理单信息
    /// </summary>
    internal class GenRMADto
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
            public string Customer { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Memo { get; set; }

            /// <summary>
            /// 币种
            /// </summary>
            public string Currency { get; set; }


            /// <summary>
            /// 业务员
            /// </summary>
            public string Seller { get; set; }

            /// <summary>
            /// 部门
            /// </summary>
            public string Department { get; set; }

            /// <summary>
            /// 单据状态
            /// </summary>
            public string Status { get; set; }


            /// <summary>
            /// 共有段私有段
            /// </summary>
            public PubPriSVData PubPriDt { get; set; }

            /// <summary>
            /// 单行集合
            /// </summary>
            public List<RMALineSVDTO> DocLine { get; set; }
    }
}
