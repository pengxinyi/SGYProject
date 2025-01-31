﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class OuterOutquerywithdetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public OuterOutData data { get; set; }
    }
    public class OuterOutData
    {
        public List<OuterOutOrderDto> order { get; set; }
        public int total_count { get; set; }
    }
    public class OuterOutOrderDto
    {
        //public int stockin_id { get; set; }
        public string outer_out_no { get; set; }
        public string warehouse_no { get; set; }
        public string warehouse_name { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public string src_order_no { get; set; }
        public string creator_name { get; set; }
        public string modified { get; set; }
        public List<OuterOutOrderDeatail> detail_list { get; set; }
    }
    public class OuterOutOrderDeatail
    {
        public string goods_no { get; set; }
        public decimal num { get; set; }
        public string batch_no { get; set; }
        public bool defect { get; set; }
    }
}
