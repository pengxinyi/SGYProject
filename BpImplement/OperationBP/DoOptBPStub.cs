







namespace UFIDA.U9.LH.LHPubBP.OperationBP
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.Runtime.Serialization;
	using System.IO;
	using UFSoft.UBF.Util.Context;
	using UFSoft.UBF;
	using UFSoft.UBF.Exceptions;
	using UFSoft.UBF.Service.Base ;

    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.UFIDA.org", Name="UFIDA.U9.LH.LHPubBP.OperationBP.IDoOptBP")]
    public interface IDoOptBP
    {
	[OperationContract()]
        System.String Do(UFSoft.UBF.Service.ISVContext context ,System.String optType, System.String jsonData);
    }

    [UFSoft.UBF.Service.ServiceImplement]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class DoOptBPStub : ISVStubBase, IDoOptBP
    {
        #region IDoOptBP Members

        //[OperationBehavior]
        public System.String Do(UFSoft.UBF.Service.ISVContext context , System.String optType, System.String jsonData)
        {
			
			ICommonDataContract commonData = CommonDataContractFactory.GetCommonData(context);
			return DoEx(commonData, optType, jsonData);
        }
        
        //[OperationBehavior]
        public System.String DoEx(ICommonDataContract commonData, System.String optType, System.String jsonData)
        {
			this.CommonData = commonData ;
            try
            {
                BeforeInvoke("UFIDA.U9.LH.LHPubBP.OperationBP.DoOptBP");                
                DoOptBP objectRef = new DoOptBP();
		
				objectRef.OptType = optType;
				objectRef.JsonData = jsonData;

				//处理返回类型.
				System.String result = objectRef.Do();
				return result ;
						return result;

	        }
			catch (System.Exception e)
            {
				DealException(e);
				throw;
            }
            finally
            {
				FinallyInvoke("UFIDA.U9.LH.LHPubBP.OperationBP.DoOptBP");
            }
        }
	#endregion
    }
}
