namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using QimenCloud.Api.scene3ldsmu02o9.Request;
    using QimenCloud.Api;
    using System;
	using System.Collections.Generic;
    using System.Text;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.LH.LHPubBE.FailInfoBE;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Utility;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Util.Context;
    using QimenCloud.Api.scene3ldsmu02o9.Response;
    using Top.Api;
    using UFIDA.U9.Base.FlexField.ValueSet;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFIDA.U9.SM.SO;
    using UFIDA.U9.CBO.SCM.Warehouse;

    /// <summary>
    /// AutoCreateARDoc partial 
    /// </summary>	
    public partial class AutoCreateARDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoCreateARDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
	internal partial class AutoCreateARDocImpementStrategy : BaseStrategy
	{
		public AutoCreateARDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateARDoc bpObj = (AutoCreateARDoc)obj;
            GetARDoc(1L);
            GetARDocs(1L);
            return "OK";

        }
        private void  GetARDoc(long page_no)
        {
            WdtAftersalesRefundRefundSearchRequest request = new WdtAftersalesRefundRefundSearchRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtAftersalesRefundRefundSearchRequest.PagerDomain pager = new WdtAftersalesRefundRefundSearchRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100L;
            request.Pager_ = pager;
            WdtAftersalesRefundRefundSearchRequest.ParamsDomain paramsDomain = new WdtAftersalesRefundRefundSearchRequest.
                ParamsDomain();
            paramsDomain.ModifiedFrom = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.ModifiedTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.StartTime = "2023-10-16 14:25:00";
            //paramsDomain.EndTime = "2023-10-16 14:26:00";
            paramsDomain.Status = 90L ;
            paramsDomain.Type = 4L;
            //paramsDomain.StatusType = 0;
            request.Params_ = paramsDomain;

            request.SetTargetAppKey(WDTChelper.target_app_key);// 旺店通appkey
            request.AddOtherParameter("wdt3_customer_id", WDTChelper.qmsid);

            request.WdtAppkey = WDTChelper.key;
            request.WdtSalt = WDTChelper.salt;

            request.WdtSign = WdtUtils.GetQimenCustomWdtSign(request, WDTChelper.secret);
            DefaultQimenCloudClient client = new DefaultQimenCloudClient(WDTChelper.qmapiUrl, WDTChelper.qmappkey, WDTChelper.qmsecret);
            try
            {
                WdtAftersalesRefundRefundSearchResponse response = client.Execute(request);
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    string dataJson = string.Empty;
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtAftersalesRefundRefundSearchResponse.OrderDomain item in response.Data.Order)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Customer customer = Customer.Finder.Find("Code=@Code", new OqlParam("Code", item.ShopNo));
                        if (customer == null)
                        {
                            continue;
                        }
                        Organization  organization = Organization.FindByCode("192");
                        if (organization != null)
                        {
                              WH = Warehouse.FindByCode(organization, item.ReturnWarehouseNo);
                        }
                      
                        if (WH!=null)
                        {
                            org = organization;
                        }
                        else
                        {
                            org = CHelper.GetOrg(item.DetailList[0].GoodsNo, "");
                        }
                        
                        if (org != null)
                        {
                            //如果已存在，不插入日志
                            //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.StockinNo));
                            FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and DocType='退回处理单创建'", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.RefundNo));
                            if (failInfo == null)
                            {
                                dataJson = JsonHelper.GetWdtRefundRMA(item, org);
                                CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.RefundNo, 0, "", "");
                            }

                        }
                    }

                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetARDoc(page_no);
                    }
                }
                if (response.Status!=0)
                {
                    CHelper.InsertU9Log(false, "奇门退换单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }
               
            }
            catch (TopException e)
            {
                Console.WriteLine(e.ErrorMsg);
            }
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("aftersales.refund.Refund.search");
            //    List<AfterSalesOrderqueryWithDetailReq> listreq = new List<AfterSalesOrderqueryWithDetailReq>();
            //    AfterSalesOrderqueryWithDetailReq req = new AfterSalesOrderqueryWithDetailReq();
            //    req.modified_from = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //    req.modified_to = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = 90;
            //   // req.type = 4;
            //    listreq.Add(req);
            //    string json = JsonConvert.SerializeObject(listreq);
            //    list.Add("body", json);
            //    //分页数据
            //    list.Add("page_size", "100");
            //    list.Add("page_no", page_no.ToString());
            //    list.Add("calc_total", "1");
            //    string sign = WDTChelper.GetWDTSign(list);
            //    list.Add("sign", sign);
            //    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //    string apireuslt = Tools.HttpPost(url, json);
            //    AfterSalesOrderqueryWithDetailRes res = JsonConvert.DeserializeObject<AfterSalesOrderqueryWithDetailRes>(apireuslt);
            //    if (res.status != 0)
            //    {
            //        CHelper.InsertU9Log(false, "退换单查询", apireuslt, json, url);
            //    }
            //    if (res.data != null && res.data.order != null)
            //    {
            //        string dataJson = string.Empty;
            //        string PODocNo = string.Empty;
            //        FailInfo fail = null;
            //        foreach (var item in res.data.order)
            //        {
            //            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //            if (org != null)
            //            {
            //                //如果已存在，不插入日志
            //                //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.StockinNo));
            //                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0 and DocType='退回处理单创建'", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.refund_no));
            //                if (failInfo == null)
            //                {
            //                    dataJson = JsonHelper.GetWdtRefundRMA(item, org);
            //                    CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.refund_no, 0, "", "");
            //                }

            //            }
            //        }
            //    }

            //    if (res.data.order.Count == 100)
            //    {
            //        page_no++;
            //        GetARDoc(page_no);
            //    }
            //}

            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
        private void GetARDocs(long page_no)
        {
            WdtAftersalesRefundRefundSearchRequest request = new WdtAftersalesRefundRefundSearchRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtAftersalesRefundRefundSearchRequest.PagerDomain pager = new WdtAftersalesRefundRefundSearchRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100L;
            request.Pager_ = pager;
            WdtAftersalesRefundRefundSearchRequest.ParamsDomain paramsDomain = new WdtAftersalesRefundRefundSearchRequest.
                ParamsDomain();
            paramsDomain.ModifiedFrom = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.ModifiedTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.StartTime = "2023-10-16 14:25:00";
            //paramsDomain.EndTime = "2023-10-16 14:26:00";
            paramsDomain.Status = 90L;
            paramsDomain.Type = 6L;
            //paramsDomain.StatusType = 0;
            request.Params_ = paramsDomain;

            request.SetTargetAppKey(WDTChelper.target_app_key);// 旺店通appkey
            request.AddOtherParameter("wdt3_customer_id", WDTChelper.qmsid);

            request.WdtAppkey = WDTChelper.key;
            request.WdtSalt = WDTChelper.salt;

            request.WdtSign = WdtUtils.GetQimenCustomWdtSign(request, WDTChelper.secret);
            DefaultQimenCloudClient client = new DefaultQimenCloudClient(WDTChelper.qmapiUrl, WDTChelper.qmappkey, WDTChelper.qmsecret);
            try
            {
                WdtAftersalesRefundRefundSearchResponse response = client.Execute(request);
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    string dataJson = string.Empty;
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtAftersalesRefundRefundSearchResponse.OrderDomain item in response.Data.Order)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Customer customer = Customer.Finder.Find("Code=@Code", new OqlParam("Code", item.ShopNo));
                        if (customer == null)
                        {
                            continue;
                        }
                        Organization organization = Organization.FindByCode("192");
                        if (organization != null)
                        {
                            WH = Warehouse.FindByCode(organization, item.ReturnWarehouseNo);
                        }

                        if (WH != null)
                        {
                            org = organization;
                        }
                        else
                        {
                            org = CHelper.GetOrg(item.DetailList[0].GoodsNo, "");
                        }

                        if (org != null)
                        {
                            //如果已存在，不插入日志
                            //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.StockinNo));
                            FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and DocType='退回处理单创建'", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.RefundNo));
                            if (failInfo == null)
                            {
                                dataJson = JsonHelper.GetWdtRefundRMAs(item, org);
                                if (!string.IsNullOrEmpty(dataJson))
                                {
                                    CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.RefundNo, 0, "", "");
                                }
                               
                            }

                        }
                    }

                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetARDocs(page_no);
                    }
                }
                if (response.Status != 0)
                {
                    CHelper.InsertU9Log(false, "奇门退换单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }

            }
            catch (TopException e)
            {
                Console.WriteLine(e.ErrorMsg);
            }
           
        }
    }

	#endregion
	
	
}