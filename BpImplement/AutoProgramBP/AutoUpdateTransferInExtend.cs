namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
	using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Top.Api.Domain;
    using UFIDA.U9.Base;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.InvDoc.MiscRcv;
    using UFIDA.U9.InvDoc.TransferIn;
    using UFIDA.U9.ISV.TransferInISV.Proxy;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.PM.PO;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// 定时获取更新调入单数据
    /// </summary>	
    public partial class AutoUpdateTransferIn 
	{	
		internal BaseStrategy Select()
		{
			return new AutoUpdateTransferInImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoUpdateTransferInImpementStrategy : BaseStrategy
	{
		public AutoUpdateTransferInImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoUpdateTransferIn bpObj = (AutoUpdateTransferIn)obj;
            //获取旺店通调拨出入库单调入单更新数据
            new InventorySV().GetUpdateTransferIn();
            //获取旺店通调拨入库单创建U9调入单--一步式
            new InventorySV().CreateU9TransferInss(0);
            //获取旺店通调拨入库单调入单更新数据
           // new InventorySV().UpdateU9TransferIn(0);
            return "OK";
        }		
	}

	#endregion
	
	
}