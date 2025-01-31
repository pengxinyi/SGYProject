namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// 定时推送基础档案信息
    /// </summary>	
    public partial class AutoSendBase
    {
        internal BaseStrategy Select()
        {
            return new AutoSendBaseImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoSendBaseImpementStrategy : BaseStrategy
    {
        public AutoSendBaseImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoSendBase bpObj = (AutoSendBase)obj;

            //get business operation context is as follows
            //IContext context = ContextManager.Context	

            //auto generating code end,underside is user custom code
            //and if you Implement replace this Exception Code...
            throw new NotImplementedException();
        }
    }

    #endregion


}