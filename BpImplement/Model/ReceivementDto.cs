using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 委外退货
    /// </summary>
   public class ReceivementDto
    {
        public string outer_no { get; set; }
        public string warehouse_no { get; set; }
        public string provider_no { get; set; }
        //public string contact { get; set; }
        //public string telno { get; set; }
        //public string receive_address { get; set; }
        //public string receive_province { get; set; }
        //public string receive_city { get; set; }
        //public string receive_district { get; set; }
         public string remark { get; set; }
    }
    public class RcvReturnLine
    {
        public string spec_no { get; set; }
        public decimal num { get; set; }
        public string remark { get; set; }
        public string batch_no { get; set; }
        public decimal tax_rate { get; set; }
        public decimal price { get; set; }
        public string unit_name { get; set; }
        public decimal discount { get; set; }
        public bool defect { get; set; }

    }
}
