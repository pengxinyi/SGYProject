





namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;
	using UFSoft.UBF.AopFrame; 	

	/// <summary>
	/// 定时创建退回处理单(无来源) business operation
	/// 
	/// </summary>
	[Serializable]	
	public partial class AutoCreateRtnRcv
	{
	    #region Fields
		
	    #endregion
		
	    #region constructor
		public AutoCreateRtnRcv()
		{}
		
	    #endregion

	    #region member		
	    #endregion
		
	    #region do method 
		[Transaction(UFSoft.UBF.Transactions.TransactionOption.Required)]
		[Logger]
		[Authorize]
		public System.String Do()
		{	
		    BaseStrategy selector = Select();	
				System.String result =  (System.String)selector.Execute(this);	
		    
			return result ; 
		}			
	    #endregion 					
	} 		
}
