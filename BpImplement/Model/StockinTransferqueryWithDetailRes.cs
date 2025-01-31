using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinTransferqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockInTransferData data { get; set; }
    }
    public class StockInTransferData
    {
        public List<StockInTransferOrderData> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockInTransferOrderData
    {
        public long stockin_id { get; set; }
        public string order_no { get; set; }
        public string created_time { get; set; }
        public string check_time { get; set; }
        public string modified { get; set; }
        public string src_order_no { get; set; }
        public string st_created { get; set; }
        public decimal goods_count { get; set; }
        public string from_warehouse_no { get; set; }
        public string from_warehouse_name { get; set; }
        public string to_warehouse_no { get; set; }
        public string to_warehouse_name { get; set; }
        public string operator_name { get; set; }
        public decimal total_price { get; set; }
        public int   logistics_type { get; set; }
        public string logistics_no { get; set; }
        public string logistics_name { get; set; }
        public string logistics_code { get; set; }
        public int  status { get; set; }  
        public List<StockInTransferOrderDeatail> detail_list { get; set; }
    }
    public class StockInTransferOrderDeatail
    {
        public long stockin_id { get; set; }
        public decimal num { get; set; }
        public decimal total_cost { get; set; }
        public string remark { get; set; }
        public decimal right_num { get; set; }
        public int rec_id { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public string spec_no { get; set; }
        public string prop2 { get; set; }
        public string spec_name { get; set; }
        public string spec_code { get; set; }
        public string brand_no { get; set; }
        public string brand_name { get; set; }
        public string goods_unit { get; set; }
        public bool defect { get; set; }
        public string batch_no { get; set; }
        public string expire_date { get; set; }
        public string production_date { get; set; }


    }
}
