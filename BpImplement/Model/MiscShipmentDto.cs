using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 杂发单
    /// </summary>
   public class MiscShipmentDto
    {
        public string outer_no { get; set; }
        public string warehouse_no { get; set; }
        public bool is_check { get; set; }
        public string remark { get; set; }
        public string reason { get; set; }
        public List<MiscShipLine> goods_list { get; set; }
    }
    public class MiscShipLine
    { 
      public string spec_no { get; set; }
        public decimal num { get; set; }
        public string remark { get; set; }
        public string batch_no { get; set; }
        public string production_date { get; set; }
        public string expire_date { get; set; }
    }
}
