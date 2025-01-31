using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 调入单信息
    /// </summary>
   public class TransferlnDto
    {
        public string src_order_no { get; set; }
        public string warehouse_no { get; set; }
        public string logistics_code { get; set; }
        public string remark { get; set; }
    }

    public class TransferlnLineDto
    {
        public string spec_no { get; set; }
        public decimal num { get; set; }
        public string unit_name { get; set; }
        public string position_no { get; set; }
        public string batch_no { get; set; }
        public string remark { get; set; }

    }
}
