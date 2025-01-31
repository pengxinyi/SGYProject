using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockoutTransferqueryWithRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockoutTransferOrderData data { get; set; }
    }

    public class StockoutTransferOrderData
    {
        public List<StockoutTransferOrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockoutTransferOrderDto
    {
        public string order_no { get; set; }
        public string src_order_no { get; set; }
        public int status { get; set; }
        public string remark { get; set; }
        public string from_warehouse_no { get; set; }
        public string modified { get; set; }
        public string to_warehouse_no { get; set; }
        public string operator_name { get; set; }
        public string consign_time { get; set; }
        public List<StockoutTransferOrderDeatail> detail_list { get; set; }
    }
    public class StockoutTransferOrderDeatail
    {
        public decimal  goods_count { get; set; }
        public string goods_no { get; set; }
        List<positiondetailslist> position_details_list { get; set; }
    }
    
}
