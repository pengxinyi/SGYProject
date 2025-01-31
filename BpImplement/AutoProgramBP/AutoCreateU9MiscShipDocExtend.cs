namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.PM.Rtn;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// 定时获取失败推送列表数据生成杂发单
    /// </summary>	
    public partial class AutoCreateU9MiscShipDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoCreateU9MiscShipDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoCreateU9MiscShipDocImpementStrategy : BaseStrategy
    {
        public AutoCreateU9MiscShipDocImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoCreateU9MiscShipDoc bpObj = (AutoCreateU9MiscShipDoc)obj;
            InventorySV invSV = new InventorySV();
             invSV.CreateMiscShip();
             invSV.CreateShiftDoc();
            //if (!rtn.IsSuccess)
            //    //    CHelper.UpdateRequsetError("AutoCreateU9MiscShipDoc", rtn.Msg);
            //    throw new Exception(rtn.Msg);
            return "";
        }
    }

    #endregion


}