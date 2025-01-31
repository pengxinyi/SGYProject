﻿








namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP.Proxy
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

    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.UFIDA.org", Name="UFIDA.U9.LH.LHPubBP.AutoProgramBP.IAutoCreateU9ShiftDoc")]
    public interface IAutoCreateU9ShiftDoc
    {
		[OperationContract()]
		System.String Do(UFSoft.UBF.Service.ISVContext context );
    }
	[Serializable]    
    public class AutoCreateU9ShiftDocProxy : OperationProxyBase//, UFIDA.U9.LH.LHPubBP.AutoProgramBP.Proxy.IAutoCreateU9ShiftDoc
    {
	#region Fields	
	
	#endregion	
		
	#region Properties
	
	
	#endregion	


	#region Constructors
        public AutoCreateU9ShiftDocProxy()
        {
        }
        #endregion
        

		#region Public Method
		
        public System.String Do()
        {
  			InitKeyList() ;
 			System.String result = (System.String)InvokeAgent<UFIDA.U9.LH.LHPubBP.AutoProgramBP.Proxy.IAutoCreateU9ShiftDoc>();
			return GetRealResult(result);
        }
        
		protected override object InvokeImplement<T>(T oChannel)
        {
			IContext context = ContextManager.Context;			

            IAutoCreateU9ShiftDoc channel = oChannel as IAutoCreateU9ShiftDoc;
            if (channel != null)
            {
				UFSoft.UBF.Service.ISVContext isvContext =  GetISVContext(context);
				return channel.Do(isvContext );
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



