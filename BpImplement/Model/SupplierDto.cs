using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 供应商信息
    /// </summary>
   public class SupplierDto
    {
        public string provider_no { get; set; }
        public string provider_name { get; set; }
        //public string contact { get; set; }
        //public string telno { get; set; }
        //public string mobile { get; set; }
        //public string fax { get; set; }
        //public string qq { get; set; }
        //public string zip { get; set; }
        //public string wangwang { get; set; }
        //public string email { get; set; }
        //public string website { get; set; }
        //public string address { get; set; }
        //public int arrive_cycle_days { get; set; }
        public string remark { get; set; }
        public int is_disabled { get; set; }
        //public string account_bank_no { get; set; }
        //public string account_bank { get; set; }
        //public string collect_name { get; set; }
        //public int province { get; set; }
        //public int city { get; set; }
        //public int district { get; set; }
    }
}
