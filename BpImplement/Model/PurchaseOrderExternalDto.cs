using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 委外采购单
    /// </summary>
   public class PurchaseOrderExternalDto
    {
        public string purchase_no { get; set; }
        public string provider_no { get; set; }
        public string receive_warehouse_nos { get; set; }
        public string expect_warehouse_no { get; set; }
        public string purchaser_name { get; set; }
        public bool is_check { get; set; }
       // public string apply_nos { get; set; }
        //public string receive_address { get; set; }
        //public string contact { get; set; }
        //public string telno { get; set; }
       // public string flag_name { get; set; }
        public string expect_time { get; set; }
       // public string created { get; set; }
       // public int pay_type { get; set; }
        //public int postfee_pay_type { get; set; }
        public string remark { get; set; }
       // public decimal post_fee { get; set; }
        //public decimal other_fee { get; set; }
       // public string logistics_no { get; set; }
        public string prop1 { get; set; }
       // public string prop2 { get; set; }
        public string prop3 { get; set; }
       // public string prop4 { get; set; }
        public List<PurchaseOrderExternalDetail> purchase_details { get; set; }
    }

    public class PurchaseOrderExternalDetail
    {
        public string spec_no { get; set; }
        public int num { get; set; }
        public string purchase_unit_name { get; set; }
        public string remark { get; set; }
        public string prop1 { get; set; }
       // public string prop2 { get; set; }
       // public string prop3 { get; set; }
       // public string prop4 { get; set; }
        public decimal price { get; set; }
        public decimal tax_price { get; set; }
        public decimal discount { get; set; }
        public decimal tax_rate { get; set; }
        public bool defect { get; set; }
       // public string batch_no { get; set; }
       // public string expire_date { get; set; }      
    }
}
