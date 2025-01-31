namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UFIDA.U9.InvDoc.MiscRcv;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Util.Context;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Item;
    using System.Web.Script.Serialization;
    using UFIDA.U9.LH.LHPubBP.Option;
    using System.Linq;

    /// <summary>
    /// 定时获取旺店通生产创建杂收杂发单数据
    /// 奇门预入库单【预入库】、其它入库单【盘盈入库、正残转换】、外仓调整入库单【外仓调整入库】杂收
    /// 其它出库单【盘亏出库、正残转换】创建杂发数据、外仓调整出库单【外仓调整出库】杂发
    /// </summary>	
    public partial class AutoCreateMiscShipRcv
    {
        internal BaseStrategy Select()
        {
            return new AutoCreateMiscShipRcvImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoCreateMiscShipRcvImpementStrategy : BaseStrategy
    {
        public AutoCreateMiscShipRcvImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoCreateMiscShipRcv bpObj = (AutoCreateMiscShipRcv)obj;
            string starttime = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            string endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            InventorySV invSV = new InventorySV();
            //杂收
            //获取奇门预入库单【预入库】创建杂收数据
            invSV.GetRcvTransS(1, starttime,endtime);
            //获取旺店通其它入库单【盘盈入库、正残转换】创建杂收数据
            invSV.GetMiscShipRcvs(0, starttime, endtime);
            // 获取旺店通外仓调整入库单【外仓调整入库】创建杂收数据
            invSV.GetMiscRcvOuts(0, starttime, endtime);
            // 获取旺店通入库管理【盘点入库】创建杂收数据 
            invSV.GetMiscRcvBaseStockIn(0, starttime, endtime);
            //获取旺店通盘点入库创建杂收数据
            invSV.GetMiscRcvStockPdIns(0, starttime, endtime);

            //杂发
            //获取旺店通其它出库单【盘亏出库、正残转换】创建杂发数据
            invSV.GetMiscShipss(0, starttime, endtime);
            //获取旺店通外仓调整出库单【外仓调整出库】创建杂发数据
            invSV.GetMiscShipOuts(0, starttime, endtime);
            //获取旺店通出库明细【盘亏出库】创建杂发数据 
            invSV.GetMiscShipBaseStockOut(0, Convert.ToDateTime(endtime).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), endtime);
            //获取旺店通盘点出库创建杂发数据
            invSV.GetMiscShipStockPdOuts(0, starttime, endtime);
            //出库单【正残转换】创建杂发
            invSV.GetMiscShipBaseSearchStockOuts(0, Convert.ToDateTime(endtime).AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), endtime);
            //入库单【正残转换】创建杂收
            invSV.GetMiscShipRcvBaseSearchs(0, starttime, endtime);
            //生产单创建形态转换单
            invSV.GetShiftDocFromProcess(0, starttime, endtime);
            //生产入库创建杂收
            //invSV.GetMiscShipRcvProcess(0, starttime, endtime);
            return "OK";
        }

    }

    #endregion


}