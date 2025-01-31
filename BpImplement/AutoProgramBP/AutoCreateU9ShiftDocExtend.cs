namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// AutoCreateU9ShiftDoc partial 
	/// </summary>	
	public partial class AutoCreateU9ShiftDoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateU9ShiftDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateU9ShiftDocImpementStrategy : BaseStrategy
	{
		public AutoCreateU9ShiftDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateU9ShiftDoc bpObj = (AutoCreateU9ShiftDoc)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}