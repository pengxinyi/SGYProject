using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class UpdateTransferDocDto
    {
        public string DocNo { get; set; }
        public long ID { get; set; }
        public string OrgCode { get; set; }
        public List<UpdateTransferDocLineDto> DocLines { get; set; }
    }
    public class UpdateTransferDocLineDto
    {
        public string RItemCode { get; set; }
        public string CItemCode { get; set; }
        public decimal RAmount { get; set; }
        public decimal CAmount { get; set; }
        public string RWHCode { get; set; }
        public string CWHCode { get; set; }
    }
}
