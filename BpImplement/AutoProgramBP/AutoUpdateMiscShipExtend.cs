namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.Pub.Controller;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.InvDoc.MiscShip;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// 不启用 获取杂发单的方式在AutoUpdateMiscRcv调度
    /// </summary>	
    public partial class AutoUpdateMiscShip
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateMiscShipImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoUpdateMiscShipImpementStrategy : BaseStrategy
    {
        public AutoUpdateMiscShipImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateMiscShip bpObj = (AutoUpdateMiscShip)obj;
            return "OK";
        }
    }

    #endregion


}