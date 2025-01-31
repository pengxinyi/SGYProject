using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 返回信息
    /// </summary>
    public class RtnDataJson
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; } = "";

        /// <summary>
        /// 单号ID集合
        /// </summary>
        public List<long> IDs { get; set; }

        /// <summary>
        /// 单号集合
        /// </summary>
        public List<string> DocNos { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
    }
}
