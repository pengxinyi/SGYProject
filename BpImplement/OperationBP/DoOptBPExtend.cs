namespace UFIDA.U9.LH.LHPubBP.OperationBP
{
    using Newtonsoft.Json;
    using QimenCloud.Api.scene3ldsmu02o9.Request;
    using QimenCloud.Api.scene3ldsmu02o9.Response;
    using QimenCloud.Api;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Top.Api;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.LH.LHPubBP.Utility;
    using UFIDA.U9.SM.Ship;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// DoOptBP partial 
    /// </summary>	
    public partial class DoOptBP
    {
        internal BaseStrategy Select()
        {
            return new DoOptBPImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class DoOptBPImpementStrategy : BaseStrategy
    {
        public DoOptBPImpementStrategy() { }

        public override object Do(object obj)
        {
            DoOptBP bpObj = (DoOptBP)obj;

            string result = "";
            if (string.IsNullOrEmpty(bpObj.OptType))
                throw new Exception("操作类型不能为空！");

            RtnDataJson Rtn = new RtnDataJson();
            switch (bpObj.OptType)
            {
                //料品按钮推送
                case "SendItem":
                    {
                        Rtn = new WDTSV().SendWDTGoods(bpObj.JsonData);
                        break;
                    }
                case "ApproveSendItemMaster":
                    {
                        Rtn = new WDTSV().ApproveSendWDTGoods(bpObj.JsonData);
                        break;
                    }
                //供应商
                case "SendSupplier":
                    {
                        Rtn = new WDTSV().SendWDTProvider(bpObj.JsonData);
                        break;
                    }
                case "ApproveSendSupplier":
                    {
                        Rtn = new WDTSV().ApproveSendWDTProvider(bpObj.JsonData);
                        break;
                    }

                //委外退货
                case "SendRcvPurReturn":
                    {
                        Rtn = new WDTSV().SendWDTPurchaseReturn(bpObj.JsonData);
                        break;
                    }
                //委外退货审核生成委外退料
                case "CreatePMRtnIssue":
                    {
                        Rtn = new WDTSV().CreatePmRtnIssue(bpObj.JsonData);
                        break;
                    }
                //委外采购
                case "SendExternalPO":
                    {
                        Rtn = new WDTSV().SendWDTPurchaseExternal(bpObj.JsonData);
                        break;
                    }
                //委外采购关闭
                case "CloseExternalPO":
                    {
                        Rtn = new WDTSV().CloseWDTPurchaseExternal(bpObj.JsonData);
                        break;
                    }
                //标准收货
                case "SendReceivement":
                    {
                        Rtn = new WDTSV().SendWDTReceivement(bpObj.JsonData);
                        break;
                    }
                //采购退货
                case "SendRcvSaleReturn":
                    {
                        Rtn = new WDTSV().SendWDTSalesPOReturn(bpObj.JsonData);
                        break;
                    }
                //退货审核创建退补采购单
                case "CreatePO4PMRtn":
                    {
                        Rtn = new WDTSV().SendWDTPOReturn4PM(bpObj.JsonData);
                        break;
                    }
                //标准采购
                case "SendPO":
                    {
                        Rtn = new WDTSV().SendWDTPurchaseOrder(bpObj.JsonData);
                        break;
                    }
                //标准采购关闭
                case "ClosePO":
                    {
                        Rtn = new WDTSV().CloseWDTPurchaseOrder(bpObj.JsonData);
                        break;
                    }
                //更新出货单物流信息
                case "UpdateShipInfo":
                    {
                        //   Rtn = new WDTSV().UpdateShipInfo(bpObj.JsonData);
                        break;
                    }
                //出货计划 -原始订单
                case "SendShipPlan":
                    {
                        Rtn = new WDTSV().SendWDTOrIginalOrder(bpObj.JsonData);
                        break;
                    }
                //销售订单-原始订单
                case "SendSO":
                    {
                        Rtn = new WDTSV().SendWDSOTOrIginalOrder(bpObj.JsonData);
                        break;
                    }
                //退回处理
                case "SendRMA":
                    {
                        Rtn = new WDTSV().SendWDTRMA(bpObj.JsonData);
                        break;
                    }
                //重新生单
                case "RepeatSendFailRecord":
                    {
                        Rtn = new WDTSV().SendFaidRecord(bpObj.JsonData);
                        break;
                    }
                //重新拉取
                case "ReloadWDTDoc":
                    {
                        Rtn = new ReloadSV().ReloadRecord(bpObj.JsonData);
                        break;
                    }
                //合并生单
                case "MergeOrder":
                    {
                        Rtn = new WDTSV().MergerSalesOrderCreate4SOShip(bpObj.JsonData);
                        break;
                    }

                #region 库存模块
                //调入
                case "SendTransferIn":
                    {
                        Rtn = new InventorySV().SendWDTTransfer(bpObj.JsonData);
                        break;
                    }
                //杂发
                case "SendMiscShipment":
                    {
                        Rtn = new InventorySV().SendWDTMiscShip(bpObj.JsonData);
                        break;
                    }
                //杂收
                case "SendMiscRcvTrans":
                    {
                        Rtn = new InventorySV().SendWDTMiscRcvTrans(bpObj.JsonData);
                        break;
                    }             
                    #endregion
            }
            return JsonConvert.SerializeObject(Rtn);
        }
    }

    #endregion


}