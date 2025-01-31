using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    public class WDTRes
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResData data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string message { get; set; }
    }
    public class ResData
    { 
        public int code { get; set; }
    public string message { get; set; }
        public string status { get; set; }

        public int error_count { get; set; }
    }
}
