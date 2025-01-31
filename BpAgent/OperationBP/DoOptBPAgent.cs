








namespace UFIDA.U9.LH.LHPubBP.OperationBP.Proxy
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	using System.ServiceModel;
	using System.Runtime.Serialization;
	using UFSoft.UBF;
	using UFSoft.UBF.Exceptions;
	using UFSoft.UBF.Util.Context;
	using UFSoft.UBF.Service;
	using UFSoft.UBF.Service.Base ;

    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.UFIDA.org", Name="UFIDA.U9.LH.LHPubBP.OperationBP.IDoOptBP")]
    public interface IDoOptBP
    {
		[OperationContract()]
		System.String Do(UFSoft.UBF.Service.ISVContext context ,System.String optType, System.String jsonData);
    }
	[Serializable]    
    public class DoOptBPProxy : OperationProxyBase//, UFIDA.U9.LH.LHPubBP.OperationBP.Proxy.IDoOptBP
    {
	#region Fields	
				private System.String optType ;
						private System.String jsonData ;
			
	#endregion	
		
	#region Properties
	
				

		/// <summary>
		/// 操作类型 (该属性可为空,且无默认值)
		/// 执行操作.Misc.操作类型
		/// </summary>
		/// <value>System.String</value>
		public System.String OptType
		{
			get	
			{	
				return this.optType;
			}

			set	
			{	
				this.optType = value;	
			}
		}		
						

		/// <summary>
		/// JsonData (该属性可为空,且无默认值)
		/// 执行操作.Misc.JsonData
		/// </summary>
		/// <value>System.XML</value>
		public System.String JsonData
		{
			get	
			{	
				return this.jsonData;
			}

			set	
			{	
				this.jsonData = value;	
			}
		}		
			
	#endregion	


	#region Constructors
        public DoOptBPProxy()
        {
        }
        #endregion
        

		#region Public Method
		
        public System.String Do()
        {
  			InitKeyList() ;
 			System.String result = (System.String)InvokeAgent<UFIDA.U9.LH.LHPubBP.OperationBP.Proxy.IDoOptBP>();
			return GetRealResult(result);
        }
        
		protected override object InvokeImplement<T>(T oChannel)
        {
			IContext context = ContextManager.Context;			

            IDoOptBP channel = oChannel as IDoOptBP;
            if (channel != null)
            {
				UFSoft.UBF.Service.ISVContext isvContext =  GetISVContext(context);
				return channel.Do(isvContext , optType, jsonData);
	    }
            return  null;
        }
		#endregion
		
		//处理由于序列化导致的返回值接口变化，而进行返回值的实际类型转换处理．
		private System.String GetRealResult(System.String result)
		{

				return result ;
		}
		#region  Init KeyList 
		//初始化SKey集合--由于接口不一样.BP.SV都要处理
		private void InitKeyList()
		{
			System.Collections.Hashtable dict = new System.Collections.Hashtable() ;
										
		}
		#endregion 

    }
}



