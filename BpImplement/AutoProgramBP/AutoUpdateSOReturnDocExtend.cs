namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// AutoUpdateSOReturnDoc partial 
	/// </summary>	
	public partial class AutoUpdateSOReturnDoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoUpdateSOReturnDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoUpdateSOReturnDocImpementStrategy : BaseStrategy
	{
		public AutoUpdateSOReturnDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoUpdateSOReturnDoc bpObj = (AutoUpdateSOReturnDoc)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}