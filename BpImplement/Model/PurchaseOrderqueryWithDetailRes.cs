using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class PurchaseOrderqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public PurchaseOrderData data { get; set; }
    }
    public class PurchaseOrderData
    {
        public List<PurchaseOrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class PurchaseOrderDto
    {
        public long purchase_id { get; set; }
        public string provider_no { get; set; }
        public string purchase_no { get; set; }
        public int status { get; set; }
        public string prop1 { get; set; }
        public string prop2 { get; set; }

        public List<PurchaseOrderDeatail> detail_list { get; set; }
    }
    public class PurchaseOrderDeatail
    {
        public string goods_no { get; set; }
        public string spec_no { get; set; }


    }
}
