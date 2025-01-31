using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class DefectChangeSearchRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public DefectChangeSearchData data { get; set; }
    }
    public class  DefectChangeSearchData
    {
        public List<DefectChangeSearchOrder>  order { get; set; }

        public int total_count { get; set; }
    }

    public class DefectChangeSearchOrder
    {
        public string  change_no { get; set; }
        public string warehouse_no { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public bool type { get; set; }

        public List<DefectChangeSearchDetail>  details_list { get; set; }
    }

    public class DefectChangeSearchDetail
    {
        public string  spec_no { get; set; }
        public decimal num { get; set; }
        public string remark { get; set; }
        public bool defect { get; set; }
    }
}
