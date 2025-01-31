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
    using UFIDA.U9.InvDoc.MiscRcv;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// 定时获取旺店通普通其他入库更新杂收杂发单数据
    /// </summary>	
    public partial class AutoUpdateMiscRcv
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateMiscRcvImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoUpdateMiscRcvImpementStrategy : BaseStrategy
    {
        public AutoUpdateMiscRcvImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateMiscRcv bpObj = (AutoUpdateMiscRcv)obj;
            string starttime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InventorySV invSV = new InventorySV();
            //定时获取旺店通普通其他入库杂收单更新数据
            invSV.GetUpdateMiscRcv(0, starttime, endtime);
            //定时获取旺店通普通其他入库杂发单更新数据
            invSV.GetUpdateMiscShip(0, starttime, endtime);
            return "OK";
        }
    }

    #endregion


}