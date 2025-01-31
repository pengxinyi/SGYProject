namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Microsoft.SqlServer.Server;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.Pub.Controller;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.InvDoc.MiscShip;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// 定时获取失败推送列表数据更新审核杂发单
    /// </summary>	
    public partial class AutoUpdateU9MiscShipDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateU9MiscShipDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoUpdateU9MiscShipDocImpementStrategy : BaseStrategy
    {
        public AutoUpdateU9MiscShipDocImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateU9MiscShipDoc bpObj = (AutoUpdateU9MiscShipDoc)obj;

            //定时获取失败推送列表数据更新审核杂发单
            new InventorySV().UpdateMiscShip();
            return "OK";
        }
        
    }

    #endregion


}