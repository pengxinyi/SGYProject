using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class UpdateDocDto
    {
        public string DocNo { get; set; }
        public long ID { get; set; }
        public string OrgCode { get; set; }
        public string Remark { get; set; }
        public string RefundNo { get; set; }
        public string RKDocNo { get; set; }
        public List<UpdateDocLineDto> DocLines { get; set; }
    }
    public class UpdateDocLineDto
    { 
        public int DocLineNo { get; set; }
        public string ItemCode { get; set; }
        public decimal Amount { get; set; }
        public string WHCode { get; set; }
        public string specno { get; set; }
        public bool defect { get; set; }
        public string remark { get; set; }
    }
}
