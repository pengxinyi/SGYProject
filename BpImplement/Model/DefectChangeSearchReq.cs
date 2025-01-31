using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class DefectChangeSearchReq
    {
        /// <summary>
        /// 转换单号
        /// </summary>
        public string change_no { get; set; }

        public string status { get; set; } = "50";//已完成
    }
}
