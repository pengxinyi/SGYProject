﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class StockTransferEditDto
    {
        public string outer_no { get; set; }
        public string from_warehouse_no { get; set; }
        public string  to_warehouse_no { get; set; }
        public int mode { get; set; }
        public string remark { get; set; }
    }
    internal class StockTransferEditDetailDto
    { 
     public string spec_no { get; set; }
        public decimal num { get; set; }

    }
}
