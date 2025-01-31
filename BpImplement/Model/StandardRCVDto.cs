using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    public class StandardRCVDto
    {
        public string purchase_no { get; set; }
        public string warehouse_no { get; set; }
        public string logistics_code { get; set; }
        public string logistics_no { get; set; }
        public string remark { get; set; }
        public int create_mode { get; set; }
    }
    public class StandardRcvDetailsDto
    { 
      public string spec_no { get; set; }
        public decimal num { get; set; }
        public bool defect { get; set; }
        public string unit_name { get; set; }
        public string batch_no { get; set; }
        public string expire_date { get; set; }
        public string position_no { get; set; }
        public string sn_strings { get; set; }
        public string production_date { get; set; }
        public string remark { get; set; }
    }
}
