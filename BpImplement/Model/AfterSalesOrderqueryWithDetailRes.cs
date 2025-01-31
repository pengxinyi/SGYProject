using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class AfterSalesOrderqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public  AfterSalesOrderData data { get; set; }
    }
    public class AfterSalesOrderData
    {
        public List<AfterSalesOrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class  AfterSalesOrderDto
    {
        public string src_tids { get; set; }
        public string refund_no { get; set; }
        public int type { get; set; }
        public string remark { get; set; }
        public int stockin_status { get; set; }
        public string flag_name { get; set; }
        public decimal return_goods_count { get; set; }
        public string shop_no { get; set; }
        public int from_type { get; set; }
        public string created { get; set; }
        public string settle_time { get; set; }
        public string check_time { get; set; }
        public string return_logistics_no { get; set; }
        public decimal guarantee_refund_amount { get; set; }
        public decimal return_goods_amount { get; set; }
        public string refund_reason { get; set; }
        public decimal actual_refund_amount { get; set; }
        public string return_warehouse_no { get; set; }
        public int status { get; set; }
        public List<AfterSalesOrderDeatail> detail_list { get; set; }
    }
    public class  AfterSalesOrderDeatail
    {
        public decimal num { get; set; }
        public decimal refund_num { get; set; }
        public decimal total_amount { get; set; }       
        public string remark { get; set; }
        public string goods_no { get; set; }
        

    }
}
