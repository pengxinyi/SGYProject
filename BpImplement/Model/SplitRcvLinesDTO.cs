using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFIDA.U9.CBO.DTOs;
using UFIDA.U9.CBO.SCM.PropertyTypes;
using UFIDA.U9.PM.Rcv;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    internal class SplitRcvLinesDTO
    {
        public RcvLine RcvLine { get; set; }
        public string InvLotCode { get; set; }
        public DoubleQuantityData NewQtyTU { get; set; }
        public SrcDocInfo RcvInfo { get; set; }
        public bool Defect { get; set; }
    }
}
