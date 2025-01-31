using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class StockinPurchaseOrderqueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockinPurchaseOrderData data { get; set; }
    }
    public class StockinPurchaseOrderData
    {
        public List<StockinPurchaseOrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class StockinPurchaseOrderDto
    {
        public int stockin_id { get; set; }
        public string order_no { get; set; }
        public int status { get; set; }
        public string warehouse_no { get; set; }
        public string warehouse_name { get; set; }
        public string modified { get; set; }
        public string logistics_type_name { get; set; }
        public string created_time { get; set; }
        public string remark { get; set; }
        public long purchase_id { get; set; }
        public string purchase_no { get; set; }
        public decimal goods_count { get; set; }
        public string check_time { get; set; }
         public string provider_no { get; set; }
        public string provider_name { get; set; }
        public string logistics_no { get; set; }
        public string logistics_name { get; set; }
        public decimal goods_amount { get; set; }
        public decimal total_price { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_stockin_price { get; set; }
        public string flag_name { get; set; }
        public string operator_name { get; set; }
        public List<StockinPurchaseOrderDeatail> details_list { get; set; }
    }
    public class StockinPurchaseOrderDeatail
    {
         public int rec_id { get; set; }
        public decimal num { get; set; }
        public decimal discount { get; set; }
        public decimal cost_price { get; set; }
        public decimal src_price { get; set; }
        public decimal tax_price { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax { get; set; }
        public decimal total_cost { get; set; }
        public string remark { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public string spec_no { get; set; }
        public string spec_code { get; set; }
        public string prop1 { get; set; }
        public string prop2 { get; set; }
        public string prop3 { get; set; }
        public string prop4 { get; set; }
        public string spec_name { get; set; }
        public string brand_name { get; set; }
        public string unit_name { get; set; }
        public string batch_no { get; set; }
        public string expire_date { get; set; }
        public string production_date { get; set; }
        public string position_no { get; set; }
        public bool defect { get; set; }
        public decimal unit_ratio { get; set; }
        public string purchase_unit_name { get; set; }
        public decimal stockin_price { get; set; }

    }
}
