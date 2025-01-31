namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.InvDoc.TransferIn;
    using UFIDA.U9.ISV.TransferInISV.Proxy;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// 定时获取失败推送列表数据审核调入单
    /// </summary>	
	public partial class AutoUpdateU9TransferInDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateU9TransferInDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
	internal partial class AutoUpdateU9TransferInDocImpementStrategy : BaseStrategy
    {
        public AutoUpdateU9TransferInDocImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateU9TransferInDoc bpObj = (AutoUpdateU9TransferInDoc)obj;
            //定时获取失败推送列表数据审核调入单
            new InventorySV().UpdateTransferIn();
            return "OK";
        }
    }

    #endregion


}