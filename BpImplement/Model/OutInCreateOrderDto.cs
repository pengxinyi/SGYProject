using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class OutInCreateOrderDto
    {
        public string warehouse_no { get; set; }
        public string  remark { get; set; }
        public string  order_no { get; set; }
        public int  src_order_type { get; set; }
        public string src_order_no { get; set; }
        public string  reason { get; set; }
    }
    internal class OutInCreateOrderDetailDto
    {
        public string  spec_no { get; set; }
        public int  num { get; set; }
        public string  aux_unit_name { get; set; }
        public string  remark { get; set; }
        public bool  defect { get; set; }
    }
}
