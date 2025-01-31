using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockinOtherQueryWithDetailRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockinOtherData data { get; set; }
    }
    public class StockinOtherData
    {
        public List<StockInOrderDto> order { get; set; }
        public int total_count { get; set; }
    }
    public class StockInOrderDto
    {
        //public int stockin_id { get; set; }
        public string other_in_no { get; set; }
        public string warehouse_no { get; set; }
        public string warehouse_name { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public string prop2 { get; set; }
        public string employee_name { get; set; }
        public string modified { get; set; }
        public List<StockInOrderDeatail> detail_list { get; set; }
    }
    public class StockInOrderDeatail
    { 
    public string goods_no { get; set; }
    public decimal in_num { get; set; }
    public decimal num { get; set; }
    public string batch_no { get; set;}
        public bool defect { get; set; }
        public string remark { get; set; }
    }
}
