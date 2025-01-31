namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using QimenCloud.Api;
    using QimenCloud.Api.scene3ldsmu02o9.Request;
    using QimenCloud.Api.scene3ldsmu02o9.Response;
    using System;
	using System.Collections.Generic;
	using System.Text;
    using System.Web.Script.Serialization;
    using Top.Api;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.LH.LHPubBP.Utility;
    using UFIDA.U9.PM.PO;
    using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// 定时预入库创建杂收
	/// </summary>	
	public partial class AutoCreateRtnRcv 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateRtnRcvImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateRtnRcvImpementStrategy : BaseStrategy
	{
		public AutoCreateRtnRcvImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateRtnRcv bpObj = (AutoCreateRtnRcv)obj;
            QimenSearch();
           // WdtSearch();
          
            return "OK";
        }
        private void QimenSearch()
        {
            GetRcvTrans(1);
        }
        private void WdtSearch()
        {
            GetWDTRcvTrans(0);
        }
        private void GetRcvTrans(int page_no)
        {

                WdtWmsStockinPrestockinSearchRequest request = new WdtWmsStockinPrestockinSearchRequest();
                request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                WdtWmsStockinPrestockinSearchRequest.PagerDomain pager = new WdtWmsStockinPrestockinSearchRequest.PagerDomain();
                pager.PageNo = page_no;
                pager.PageSize = 100;
                request.Pager_ = pager;
                WdtWmsStockinPrestockinSearchRequest.ParamsDomain paramsDomain = new WdtWmsStockinPrestockinSearchRequest.
                    ParamsDomain();
                paramsDomain.CtFrom = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                paramsDomain.CtTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                paramsDomain.Status = 80;
                request.Params_ = paramsDomain;

                request.SetTargetAppKey(WDTChelper.target_app_key);// 旺店通appkey
                request.AddOtherParameter("wdt3_customer_id", WDTChelper.qmsid);

                request.WdtAppkey = WDTChelper.key;
                request.WdtSalt = WDTChelper.salt;

                request.WdtSign = WdtUtils.GetQimenCustomWdtSign(request, WDTChelper.secret);
                DefaultQimenCloudClient client = new DefaultQimenCloudClient(WDTChelper.qmapiUrl, WDTChelper.qmappkey, WDTChelper.qmsecret);

                try
                {
                    WdtWmsStockinPrestockinSearchResponse response = client.Execute(request);
                    if (response.Status == 0 && response.Data!=null && response.Data.Order!=null)
                    {
                        // CHelper.InsertU9Log(true, "奇门预入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                        string dataJson = string.Empty;
                        if (response.Data!=null && response.Data.Order!=null && response.Data.Order.Count>0)
                        {
                            foreach (WdtWmsStockinPrestockinSearchResponse.OrderDomain item in response.Data.Order)
                            {
                            //if (item.FlagName == "预入库杂收单")
                            //{
                            UFIDA.U9.CBO.SCM.Warehouse.Warehouse wh = CHelper.GetWH(item.WarehouseNo);
                            if (wh == null) continue;
                            Organization org = CHelper.GetOrg(item.DetailList[0].GoodsNo, "");
                                    if (org != null)
                                    {
                                        dataJson = JsonHelper.GetQMMiscRcvTrans(item, org);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson,org, item.StockinNo, 0);
                                        // commonSV.CreateMiscRcvTrans(dataJson);
                                    }
                                //}

                            }
                        }
                       
                        if (response.Data.Order.Count==100)
                        {
                            page_no++;
                            GetRcvTrans(page_no);
                        }
                    }
                    else
                    {
                        CHelper.InsertU9Log(false, "奇门预入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    }


                }
                catch (TopException e)
                {
                    throw new Exception(e.ErrorMsg);
                }
        }
        private void GetWDTRcvTrans(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.PreStockin.search");
                List<StockinPreStockinqueryWithDetailReq> listreq = new List<StockinPreStockinqueryWithDetailReq>();
                StockinPreStockinqueryWithDetailReq req = new StockinPreStockinqueryWithDetailReq();
                req.start_time = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
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
                StockinPreStockinqueryWithDetailRes res = JsonConvert.DeserializeObject<StockinPreStockinqueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "旺店通预入库单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null)
                {
                    CommonSV commonSV = new CommonSV();
                    foreach (var item in res.data.order)
                    {
                        if (item.flag_name == "预入库杂收单")
                        {
                            string dataJson = string.Empty;
                            string posturl = string.Empty;
                            string apirEEEeuslt = string.Empty;
                            Organization org =CHelper.GetOrg(item.detail_list[0].goods_no,"");
                            if (org != null)
                            {
                                dataJson = JsonHelper.GetWdtMiscRcvTrans(item, org);
                                CHelper.InsertFailInfo("杂收单创建", dataJson,org,item.src_order_no, 0);
                                //commonSV.CreateMiscRcvTrans(dataJson);
                            }

                        }


                    }
                    if (res.data.order.Count==100)
                    {
                        page_no++;
                        GetWDTRcvTrans(page_no);
                    }
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