namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// 定时获取失败推送列表数据生成杂收单
    /// </summary>	
    public partial class AutoCreateU9MiscRcvTransDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoCreateU9MiscRcvTransDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoCreateU9MiscRcvTransDocImpementStrategy : BaseStrategy
    {
        public AutoCreateU9MiscRcvTransDocImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoCreateU9MiscRcvTransDoc bpObj = (AutoCreateU9MiscRcvTransDoc)obj;
            RtnDataJson rtn = new InventorySV().CreateMiscRcv();
            //if (!rtn.IsSuccess)
            //    //   CHelper.UpdateRequsetError("AutoCreateU9MiscRcvTransDoc", rtn.Msg);
            //    throw new Exception(rtn.Msg);

            return "";
        }
    }

    #endregion


}
