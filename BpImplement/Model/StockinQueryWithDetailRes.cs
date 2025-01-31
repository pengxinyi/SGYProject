using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinQueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockinData data { get; set; }
    }

    public class StockinData
    { 
     public List<OrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class OrderDto
    { 
        //public int stockin_id { get; set; }
        public string order_no { get; set; }
        public int status { get; set; }
        public string warehouse_no { get; set; }
        public string warehouse_name { get; set; }
        public string stockin_time { get; set; }
        public string created_time { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public decimal goods_count { get; set; }
       // public int logistics_type { get; set; }
        public string check_time { get; set; }
        public string src_order_no { get; set; }
        public string operator_name { get; set; }
        public decimal total_price { get; set; }
        public decimal total_cost { get; set; }
        public string logistics_no { get; set; }
        public List<OrderDeatail> detail_list { get; set; }
    }
    public class OrderDeatail
    {
        //public long stockin_id { get; set; }
        public decimal goods_count { get; set; }
        public decimal total_cost { get; set; }
        public string remark { get; set; }
        public decimal right_num { get; set; }
        public string goods_unit { get; set; }
        public string batch_no { get; set; }
       // public int rec_id { get; set; }
        public string production_date { get; set; }
        public string expire_date { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public string spec_no { get; set; }
        public string prop2 { get; set; }
        public string spec_name { get; set; }
        public string spec_code { get; set; }
        public string brand_no { get; set; }
        public string brand_name { get; set; }
        public bool defect { get; set; }
        public decimal checked_cost_price { get; set; }
    }
}
