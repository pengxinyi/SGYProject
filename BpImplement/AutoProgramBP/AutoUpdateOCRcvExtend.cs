namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.FlexField.ValueSet;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.PM.PO;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// 外仓调整出入库查询创建杂收杂发单
    /// </summary>	
    public partial class AutoUpdateOCRcv
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateOCRcvImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoUpdateOCRcvImpementStrategy : BaseStrategy
    {
        public AutoUpdateOCRcvImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateOCRcv bpObj = (AutoUpdateOCRcv)obj;
            GetMiscRcv(0);
            GetMiscShip(0);
            return "OK";
        }
        private void GetMiscRcv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
                List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
                OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "50";
                //req.src_order_type = 0;
                listreq.Add(req);
                string json = JsonConvert.SerializeObject(listreq);
                list.Add("body", json);
                //分页数据
                list.Add("page_size", "100");
                list.Add("page_no", page_no.ToString());
                list.Add("calc_total", "1");
                string sign = WDTChelper.GetWDTSign(list);
                list.Add("sign", sign);
                string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                string apireuslt = Tools.HttpPost(url, json);
                OuterInquerywithdetailRes res = JsonConvert.DeserializeObject<OuterInquerywithdetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "外仓调整入库单查询", apireuslt, json, url);
                }
                string dataJson = string.Empty;
                string posturl = string.Empty;
                string apirEEEeuslt = string.Empty;
                CommonSV commonSV = new CommonSV();
                if (res.data != null && res.data.order != null)
                {
                    foreach (var item in res.data.order)
                    {
                        UFIDA.U9.CBO.SCM.Warehouse.Warehouse wh = CHelper.GetWH(item.warehouse_no);
                        if (wh == null) continue;

                        //盘盈入库生成杂收
                        Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                        if (org != null)
                        {
                            dataJson = JsonHelper.GetWdtOuterInProfitMiscRcvTrans(item, org);
                            //commonSV.CreateMiscRcvTrans(dataJson);
                            CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.outer_in_no, 0);
                        }

                    }
                }

                if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                {
                    page_no++;
                    GetMiscRcv(page_no);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GetMiscShip(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
                List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
                OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "50";
                //req.order_type = 0;
                listreq.Add(req);
                string json = JsonConvert.SerializeObject(listreq);
                list.Add("body", json);
                //分页数据
                list.Add("page_size", "100");
                list.Add("page_no", page_no.ToString());
                list.Add("calc_total", "1");
                string sign = WDTChelper.GetWDTSign(list);
                list.Add("sign", sign);
                string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                string apireuslt = Tools.HttpPost(url, json);
                OuterOutquerywithdetailRes res = JsonConvert.DeserializeObject<OuterOutquerywithdetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "外仓调整出库单查询", apireuslt, json, url);
                }
                string dataJson = string.Empty;
                string posturl = string.Empty;
                string apirEEEeuslt = string.Empty;
                CommonSV commonSV = new CommonSV();
                if (res.data != null && res.data.order != null)
                {
                    foreach (var item in res.data.order)
                    {
                        UFIDA.U9.CBO.SCM.Warehouse.Warehouse wh = CHelper.GetWH(item.warehouse_no);
                        if (wh == null) continue;

                        //盘亏出库生成杂发
                        Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                        if (org != null)
                        {
                            dataJson = JsonHelper.GetWdtOuterOutLossMiscShip(item, org);
                            // commonSV.CreateMiscShip(dataJson);
                            CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.outer_out_no, 0);
                        }
                    }
                }

                if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                {
                    page_no++;
                    GetMiscShip(page_no);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    #endregion


}