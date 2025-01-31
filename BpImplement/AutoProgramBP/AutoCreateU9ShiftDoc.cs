





namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;
	using UFSoft.UBF.AopFrame; 	

	/// <summary>
	/// 创建U9形态转换单 business operation
	/// 
	/// </summary>
	[Serializable]	
	public partial class AutoCreateU9ShiftDoc
	{
	    #region Fields
		
	    #endregion
		
	    #region constructor
		public AutoCreateU9ShiftDoc()
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
