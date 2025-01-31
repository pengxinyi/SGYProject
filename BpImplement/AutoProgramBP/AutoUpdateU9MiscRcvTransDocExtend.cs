namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using Top.Api;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.Pub.Controller;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.InvDoc.MiscRcv;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    ///  定时获取失败推送列表数据更新审核杂收单
    /// </summary>	
    public partial class AutoUpdateU9MiscRcvTransDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateU9MiscRcvTransDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoUpdateU9MiscRcvTransDocImpementStrategy : BaseStrategy
    {
        public AutoUpdateU9MiscRcvTransDocImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateU9MiscRcvTransDoc bpObj = (AutoUpdateU9MiscRcvTransDoc)obj;
            
            //定时获取失败推送列表数据更新审核杂收单
            new InventorySV().UpdateMiscRcv();
            return "OK";
        }

    }

    #endregion


}