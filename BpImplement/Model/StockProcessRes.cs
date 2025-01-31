using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockProcessRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockProcessData data { get; set; }
    }

    public class StockProcessData
    {
        public List<StockProcessOrder> order { get; set; }

        public int total_count { get; set; }
    }

    public class  StockProcessOrder
    {
        public string process_no { get; set; }
        public int status { get; set; }
        public string remark { get; set; }
        public string modified { get; set; }
        public string operator_name { get; set; }
        public string out_warehouse_no { get; set; }
        public string in_warehouse_no { get; set; }
        public List<StockProcessDetail> detail_list { get; set; }
    }
    public class StockProcessDetail
    {
        public decimal  in_num { get; set; }
        public decimal  out_num { get; set; }
        public string remark { get; set; }
        public string spec_no { get; set; }
        public bool is_product { get; set; }
        public bool Defect { get; set; }
    }
}
