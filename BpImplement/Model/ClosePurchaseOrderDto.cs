using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class ClosePurchaseOrderDto
    {
        public int operate_type { get; set; }
        public List<string> purchase_no_list { get; set; }
        public int allow_cancel_checked_order { get; set; }
    }
}
