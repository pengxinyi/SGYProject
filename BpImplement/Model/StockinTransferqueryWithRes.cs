using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockinTransferqueryWithRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public  StockinTransferOrderData data { get; set; }
    }
    public class  StockinTransferOrderData
    {
        public List<StockinTransferOrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockinTransferOrderDto
    {
        public string order_no { get; set; }
        public int status { get; set; }
        public string remark { get; set; }
        public string from_warehouse_no { get; set; }
        public string modified { get; set; }
        public string to_warehouse_no { get; set; }
        public string operator_name { get; set; }
        public string src_order_no { get; set; }
        public List<StockinTransferOrderDeatail> detail_list { get; set; }
    }
    public class StockinTransferOrderDeatail
    {
        public decimal num { get; set; }
        public string goods_no { get; set; }
        public string remark { get; set; }

    }
}
