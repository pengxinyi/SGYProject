using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockOutPushDto
    {
        public string outer_no { get; set; }
        public string warehouse_no { get; set; }
        public string receiver_name { get; set; }
        public string receiver_mobile { get; set; }
        public string receiver_province { get; set; }
        public string receiver_city { get; set; }
        public string receiver_district { get; set; }
        public string receiver_address { get; set; }
        public string reason { get; set; }
        public string flag_name { get; set; }
        public string remark { get; set; }
        public string prop1 { get; set; }
        public string prop2 { get; set; }
        public string prop3 { get; set; }
        public string prop4 { get; set; }
        public string prop5 { get; set; }
        public bool is_check { get; set; }
    }
    internal class StockOutPushDetailDto
    {
        public string spec_no { get; set; }
        public int num { get; set; }
        public string batch_no { get; set; }
        public string remark { get; set; }
        public string aux_unit_name { get; set; }

        public bool defect { get; set; }
    }
}
