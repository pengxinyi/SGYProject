





namespace UFIDA.U9.LH.LHPubBP.OperationBP
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;
	using UFSoft.UBF.AopFrame; 	

	/// <summary>
	/// 执行操作 business operation
	/// 
	/// </summary>
	[Serializable]	
	public partial class DoOptBP
	{
	    #region Fields
		private System.String optType;
		private System.String jsonData;
		
	    #endregion
		
	    #region constructor
		public DoOptBP()
		{}
		
	    #endregion

	    #region member		
		/// <summary>
		/// 操作类型	
		/// 执行操作.Misc.操作类型
		/// </summary>
		/// <value></value>
		public System.String OptType
		{
			get
			{
				return this.optType;
			}
			set
			{
				optType = value;
			}
		}
		/// <summary>
		/// JsonData	
		/// 执行操作.Misc.JsonData
		/// </summary>
		/// <value></value>
		public System.String JsonData
		{
			get
			{
				return this.jsonData;
			}
			set
			{
				jsonData = value;
			}
		}
	    #endregion
		
	    #region do method 
		[Transaction(UFSoft.UBF.Transactions.TransactionOption.Supported)]
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
