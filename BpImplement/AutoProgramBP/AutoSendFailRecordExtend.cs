namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// 定时推送失败记录数据
	/// </summary>	
	public partial class AutoSendFailRecord 
	{	
		internal BaseStrategy Select()
		{
			return new AutoSendFailRecordImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoSendFailRecordImpementStrategy : BaseStrategy
	{
		public AutoSendFailRecordImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoSendFailRecord bpObj = (AutoSendFailRecord)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}