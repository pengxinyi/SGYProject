using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class RawRefundDto
    {
        public string refund_no { get; set; }
        public decimal num { get; set; }
        public string tid { get; set; }
        public string oid { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public string refund_version { get; set; }
        public decimal refund_amount { get; set; }
        public decimal actual_refund_amount { get; set; }
        public string logistics_name { get; set; }
        public string logistics_no { get; set; }
        public string buyer_nick { get; set; }
        public string reason { get; set; }
        public string refund_time { get; set; }
        public bool is_aftersale { get; set; }
        public string remark { get; set; }
        public string title { get; set; }
        public string spec_no { get; set; }
        public string spec_id { get; set; }
        public string goods_no { get; set; }
        public string goods_id { get;set; }
        public decimal price { get; set; }
    }
    
}
