namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// AutoCreateU9SODoc partial 
	/// </summary>	
	public partial class AutoCreateU9SODoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateU9SODocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateU9SODocImpementStrategy : BaseStrategy
	{
		public AutoCreateU9SODocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateU9SODoc bpObj = (AutoCreateU9SODoc)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}