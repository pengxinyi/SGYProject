namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.LH.LHPubBE.FailInfoBE;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.PM.PO;
    using UFIDA.U9.PM.Rcv;
    using UFIDA.U9.PM.Rcv.Proxy;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// 定时创建标准收货
    /// </summary>	
    public partial class AutoUpdateRcv
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateRcvImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoUpdateRcvImpementStrategy : BaseStrategy
    {
        public AutoUpdateRcvImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoUpdateRcv bpObj = (AutoUpdateRcv)obj;
            GetRcv(0);
            return "OK";
        }

        private void GetRcv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Purchase.queryWithDetail");
                List<StockinPurchaseOrderqueryWithDetailReq> listreq = new List<StockinPurchaseOrderqueryWithDetailReq>();
                StockinPurchaseOrderqueryWithDetailReq req = new StockinPurchaseOrderqueryWithDetailReq();
                req.start_time = DateTime.Now.AddHours(-4).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = 80;
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
                StockinPurchaseOrderqueryWithDetailRes res = JsonConvert.DeserializeObject<StockinPurchaseOrderqueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "采购入库单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null)
                {
                    string dataJson = string.Empty;
                    string PODocNo =  string.Empty;
                    FailInfo fail = null;
                    foreach (var item in res.data.order)
                    {
                        //先调用采购单查询接口返回U9采购单号
                        PODocNo = GetPODocNo(item.purchase_no);
                        PurchaseOrder miscRcvTrans = PurchaseOrder.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo",PODocNo));
                        if (miscRcvTrans != null)
                        {
                            Organization org = CHelper.GetOrg(miscRcvTrans.POLines[0].ItemInfo.ItemCode, "");
                            if (org != null)
                            {
                                //标准收货
                                if (miscRcvTrans.DocType.Code == "PO20")
                                {
                                    dataJson = JsonHelper.GetWDTReceivement(miscRcvTrans, item, org);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("标准收货创建", dataJson, org, item.order_no, 0);
                                    }
                                   
                                    //commonSV.CreateMiscRcvTrans(dataJson);
                                }
                                if (miscRcvTrans.DocType.Code == "PO22")
                                {
                                    dataJson = JsonHelper.GetPurReturn(miscRcvTrans, item, org.Code);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("委外收货创建", dataJson, org, item.order_no, 0, "");
                                    }

                                   

                                }
                            }
                        }
                        else
                        {
                            Organization org = CHelper.GetOrg(miscRcvTrans.POLines[0].ItemInfo.ItemCode, "");
                            if (org != null)
                            {
                                //无来源标准收货
                                dataJson = JsonHelper.GetWDTNoSrcReceivement(item, org);
                                fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new OqlParam("Org", org.ID));
                                if (fail == null)
                                {
                                    CHelper.InsertFailInfo("标准收货创建", dataJson, org, item.order_no, 0);
                                }


                            }
                        }
                    }
                }

                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetRcv(page_no);
                    }
                }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 调用旺店通采购订单查询接口抓取U9采购订单号
        /// </summary>
        /// <param name="PurchaseOrder"></param>
        /// <returns></returns>
        public string GetPODocNo(string PurchaseOrder)
        {
            string DocNo = string.Empty;
            Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseOrder.queryWithDetail");
            List<PurchaseOrderqueryWithDetailReq> listreq = new List<PurchaseOrderqueryWithDetailReq>();
            PurchaseOrderqueryWithDetailReq req = new PurchaseOrderqueryWithDetailReq();
            //req.start_time = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            req.purchase_no = PurchaseOrder;
            //req.status = "90";
            listreq.Add(req);
            string json = JsonConvert.SerializeObject(listreq);
            list.Add("body", json);
            //分页数据
            list.Add("calc_total", "1");
            list.Add("page_no", "0");
            list.Add("page_size", "100");
            string sign = WDTChelper.GetWDTSign(list);
            list.Add("sign", sign);
            string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            string apireuslt = Tools.HttpPost(url, json);
            PurchaseOrderqueryWithDetailRes res = JsonConvert.DeserializeObject<PurchaseOrderqueryWithDetailRes>(apireuslt);
            if (res.status != 0)
            {
                CHelper.InsertU9Log(false, "采购单查询", apireuslt, json, url);
            }
            else if (res.data != null && res.data.order != null)
            {
                DocNo = res.data.order[0].prop1;
            }
            return DocNo;
        }
    }

    #endregion


}