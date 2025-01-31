







namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
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

    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.UFIDA.org", Name="UFIDA.U9.LH.LHPubBP.AutoProgramBP.IAutoCreateU9RMADoc")]
    public interface IAutoCreateU9RMADoc
    {
	[OperationContract()]
        System.String Do(UFSoft.UBF.Service.ISVContext context );
    }

    [UFSoft.UBF.Service.ServiceImplement]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class AutoCreateU9RMADocStub : ISVStubBase, IAutoCreateU9RMADoc
    {
        #region IAutoCreateU9RMADoc Members

        //[OperationBehavior]
        public System.String Do(UFSoft.UBF.Service.ISVContext context )
        {
			
			ICommonDataContract commonData = CommonDataContractFactory.GetCommonData(context);
			return DoEx(commonData);
        }
        
        //[OperationBehavior]
        public System.String DoEx(ICommonDataContract commonData)
        {
			this.CommonData = commonData ;
            try
            {
                BeforeInvoke("UFIDA.U9.LH.LHPubBP.AutoProgramBP.AutoCreateU9RMADoc");                
                AutoCreateU9RMADoc objectRef = new AutoCreateU9RMADoc();


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
				FinallyInvoke("UFIDA.U9.LH.LHPubBP.AutoProgramBP.AutoCreateU9RMADoc");
            }
        }
	#endregion
    }
}
