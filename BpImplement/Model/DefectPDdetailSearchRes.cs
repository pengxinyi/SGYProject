using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    class DefectPDdetailSearchRes
    {
        public int status { get; set; }
        public int total { get; set; }
        public string message { get; set; }

        public List<DefectPDdetailSearchDetail> data { get; set; }
    }
    //public class DefectPDdetailSearchData
    //{
    //    public List<DefectPDdetailSearchDetail> pd_detail_list { get; set; }
    //}
    public class DefectPDdetailSearchDetail
    {
        public string spec_no { get; set; }
        public string goods_no { get; set; }
        public int defect { get; set; }
    }
}
