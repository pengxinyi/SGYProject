namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using QimenCloud.Api;
    using QimenCloud.Api.scene3ldsmu02o9.Request;
    using QimenCloud.Api.scene3ldsmu02o9.Response;
    using System;
	using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using Top.Api;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.FlexField.ValueSet;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.LH.LHPubBE.FailInfoBE;
    using UFIDA.U9.LH.LHPubBE.SalesShipBE;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.LH.LHPubBP.Utility;
    using UFIDA.U9.SM.SO;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Util.Context;
    using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockoutSalesQuerywithdetailResponse;

    /// <summary>
    /// 定时创建出货单
    /// </summary>	
    public partial class AutoCreateShip 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateShipImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateShipImpementStrategy : BaseStrategy
	{
		public AutoCreateShipImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateShip bpObj = (AutoCreateShip)obj;
			string result = "";
			QimenSearch();
			//WdtSearch();
			return result;

		}

        private void QimenSearch()
        {
            GetQMShip(1L);
        }

        private void WdtSearch()
        {
            ////旺店通   销售出库查询Demo

            GetWDTShip(0);
        }
        /// <summary>
        /// 原逻辑不合并
        /// </summary>
        /// <param name="page_no"></param>
        private void GetQMSrcShips(long page_no)
        {
            #region 奇门自定义接口 销售出库查询

            WdtWmsStockoutSalesQuerywithdetailRequest request = new WdtWmsStockoutSalesQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100L;
            request.Pager_ = pager;
            WdtWmsStockoutSalesQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockoutSalesQuerywithdetailRequest.
                ParamsDomain();
            paramsDomain.StartTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.StartTime = "2023-09-01 13:04:41";
            //paramsDomain.EndTime = "2023-09-01 14:01:41";
            paramsDomain.Status = "110";
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
                WdtWmsStockoutSalesQuerywithdetailResponse response = client.Execute(request);
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    //CommonSV commonSV = new CommonSV();
                    UFIDA.U9.SM.ShipPlan.ShipPlan shipPlan = null;
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                        //if (item.TradeFrom == 1)
                        //{
                        string dataJson = string.Empty;
                        Organization org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                        if (org != null)
                        {
                            Customer customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", item.ShopNo), new OqlParam("Org", org.ID));
                            if (customer==null)
                            {
                                continue;
                            }

                            if (!string.IsNullOrEmpty(item.SrcTradeNo))
                            {
                                //先建立销售订单
                                SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                if (so == null)
                                {
                                    dataJson = JsonHelper.GetWDTOrderStockOutQuerySO(item, org.Code);
                                    CHelper.InsertFailInfo("销售订单创建", dataJson, org, item.SrcTradeNo, 0, "");
                                }
                               

                                //dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                                //后建立出货单
                                MiscSalesOrderDto orderDto = new MiscSalesOrderDto();
                                List<MiscSalesOrderDeatail> details_list = new List<MiscSalesOrderDeatail>();
                                orderDto.trade_from = Convert.ToInt32(item.TradeFrom);
                                orderDto.created = item.Created;
                                orderDto.logistics_name = item.LogisticsName;
                                orderDto.logistics_no = item.LogisticsNo;
                                orderDto.warehouse_no = item.WarehouseNo;
                                foreach (DetailsListDomain listDomain in item.DetailsList)
                                {
                                    MiscSalesOrderDeatail miscSales = new MiscSalesOrderDeatail();
                                    miscSales.goods_no = listDomain.GoodsNo;
                                    miscSales.goods_count = Convert.ToDecimal(listDomain.GoodsCount);
                                    miscSales.share_price = Convert.ToDecimal(listDomain.SellPrice);
                                    details_list.Add(miscSales);
                                }
                                orderDto.details_list = details_list;
                                dataJson = JsonConvert.SerializeObject(orderDto);
                                CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.OrderNo, 0, "", item.SrcTradeNo); //来源销售订单出货创建
                                                                                                                       //commonSV.CreateSrcShipFromSO(dataJson);
                            }
                            //shipPlan = UFIDA.U9.SM.ShipPlan.ShipPlan.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            //if (shipPlan != null)
                            //{
                            //    dataJson = JsonHelper.GetShipPlanQM(shipPlan, item, org.Code, item.TradeFrom);
                            //    CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.OrderNo, 0);//来源出货计划出货创建
                            //    // commonSV.CreateSrcShipFromShipPlan(dataJson);
                            //}
                            //else
                            //{
                            //    //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                            //    SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            //    if (so != null)
                            //    {
                            //        // dataJson = JsonHelper.GetShipFromSO(so, item.SrcOrderNo,item.ShopNo, org.Code,item.TradeFrom,item.LogisticsName,item.LogisticsNo,);
                            //        dataJson = JsonHelper.GetQMShipFromSO(so, item.SrcOrderNo, org.Code, item);
                            //        CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.SrcOrderNo, 0);//来源销售订单出货创建
                            //        //commonSV.CreateSrcShipFromSO(dataJson);
                            //    }


                            //}
                        }
                        //}


                    }
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetQMSrcShips(page_no);
                    }
                }
                else
                {
                    CHelper.InsertU9Log(false, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }


            }
            catch (TopException e)
            {
                Console.WriteLine(e.ErrorMsg);
            }
            #endregion
        }
        /// <summary>
        /// 现逻辑合并销售出库单生成无来源出货单
        /// </summary>
        /// <param name="page_no"></param>
        private void GetQMShip(long page_no)
        {
            #region 奇门自定义接口 销售出库查询

            WdtWmsStockoutSalesQuerywithdetailRequest request = new WdtWmsStockoutSalesQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 500L;
            request.Pager_ = pager;
            WdtWmsStockoutSalesQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockoutSalesQuerywithdetailRequest.
                ParamsDomain();
            //paramsDomain.StartTime = "2023-12-01 13:00:00";
            //paramsDomain.EndTime = "2023-12-01 13:30:00";
            paramsDomain.StartTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.StartTime = "2023-10-16 14:25:00";
            //paramsDomain.EndTime = "2023-10-16 14:26:00";
            paramsDomain.Status = "110";
            request.Params_ = paramsDomain;

            request.SetTargetAppKey(WDTChelper.target_app_key);// 旺店通appkey
            request.AddOtherParameter("wdt3_customer_id", WDTChelper.qmsid);

            request.WdtAppkey = WDTChelper.key;
            request.WdtSalt = WDTChelper.salt;

            request.WdtSign = WdtUtils.GetQimenCustomWdtSign(request, WDTChelper.secret);
            DefaultQimenCloudClient client = new DefaultQimenCloudClient(WDTChelper.qmapiUrl, WDTChelper.qmappkey, WDTChelper.qmsecret);

            try
            {
                WdtWmsStockoutSalesQuerywithdetailResponse response = client.Execute(request);
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    //CommonSV commonSV = new CommonSV();
                    UFIDA.U9.SM.ShipPlan.ShipPlan shipPlan = null;
                    string dataJson = string.Empty;
                    List<string> SrcTradeNo = new List<string>();   
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                        if (item.Status != "110")
                        {
                            continue;
                        }
                        Organization org = null;
                        Customer customer1 = null;
                        Customer customer = Customer.Finder.Find("Code=@Code", new OqlParam("Code", item.ShopNo));
                        //if (customer == null)
                        //{
                        //    continue;
                        //}
                        //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                        FailInfo fails = null;
                        if (item.TradeFrom == 1L)
                        {
                            if (!string.IsNullOrEmpty(item.SrcTradeNo) && item.SrcTradeNo.StartsWith("SO9"))
                            {
                                SO so = SO.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo.Substring(0, 20)));
                                if (so != null)
                                {
                                    if (item.DetailsList.Where(t => t.SrcTid == null).Count() > 0)
                                    {
                                        Organization org2 = Organization.FindByCode("101");
                                        List<DetailsListDomain> details = item.DetailsList.Where(t => t.SrcTid == null).ToList();
                                        dataJson = JsonHelper.WdtNoSrcShipJson(details, org2, item.ShopNo, item.StockCheckTime, item.WarehouseNo, item.OrderNo + "-1");
                                        fails = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='标准出货创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo + "-1"), new OqlParam("Org", so.Org.ID));
                                        if (fails == null)
                                        {
                                            CHelper.InsertFailInfo("标准出货创建", dataJson, so.Org, item.OrderNo + "-1", 0, "", "");//无来源出货创建
                                        }
                                    }
                                    dataJson = JsonHelper.GetQMShipFromSO(so, item.OrderNo, so.Org.Code, item);
                                     fails = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='标准出货创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", so.Org.ID));
                                    if (fails == null)
                                    {
                                        CHelper.InsertFailInfo("标准出货创建", dataJson, so.Org, item.OrderNo, 0, "", so.DocNo);//来源销售订单出货创建
                                    }

                                }
                                //if (item.SrcTradeNo.Contains(","))
                                //{
                                //    SrcTradeNo = item.SrcTradeNo.Split(',').ToList();
                                //    foreach (string TradeNo in SrcTradeNo)
                                //    {
                                //        SO so = SO.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", TradeNo.Substring(0, 20)));
                                //        if (so != null)
                                //        {
                                //            dataJson = JsonHelper.GetQMShipFromSO(so, item.OrderNo, so.Org.Code, item);
                                //            FailInfo fails = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='标准出货创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", so.Org.ID));
                                //            if (fails == null)
                                //            {
                                //                CHelper.InsertFailInfo("标准出货创建", dataJson, so.Org, item.OrderNo, 0, "", so.DocNo);//来源销售订单出货创建
                                //            }

                                //        }
                                //    }
                                //}
                                //else
                                //{
                                //    SO so = SO.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo.Substring(0, 20)));
                                //    if (so != null)
                                //    {
                                //        dataJson = JsonHelper.GetQMShipFromSO(so, item.OrderNo, so.Org.Code, item);
                                //        FailInfo fails = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='标准出货创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", so.Org.ID));
                                //        if (fails == null)
                                //        {
                                //            CHelper.InsertFailInfo("标准出货创建", dataJson, so.Org, item.OrderNo, 0, "", so.DocNo);//来源销售订单出货创建
                                //        }

                                //    }
                                //}                 
                            }
                            else if (!string.IsNullOrEmpty(item.SrcTradeNo) && !item.SrcTradeNo.StartsWith("SO9"))
                            {
                                foreach (DetailsListDomain detailsListDomain in item.DetailsList)
                                {
                                    org = CHelper.GetOrg(detailsListDomain.GoodsNo, "");
                                    if (org != null)
                                    {
                                        if (customer != null)
                                        {
                                            if (customer.CustomerCategory?.Code == "109" && !string.IsNullOrEmpty(item.FenXiaoNick))
                                            {
                                                customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenXiaoNick));
                                            }
                                            else if (customer.CustomerCategory?.Code == "110" && !string.IsNullOrEmpty(item.CsRemark))
                                            {
                                                customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.CsRemark));

                                            }                                        
                                            else if (customer.CustomerCategory?.Code == "107" && !string.IsNullOrEmpty(item.DetailsList[0].Remark))
                                            {
                                                int indexOfFirstSpace = item.DetailsList[0].Remark.IndexOf(' ');
                                                if (indexOfFirstSpace > 0)
                                                {
                                                    customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.DetailsList[0].Remark.Substring(0, indexOfFirstSpace)));

                                                }
                                            }
                                        }
                                        if (customer1 != null)
                                        {
                                            customer = customer1;
                                        }
                                        //if (!string.IsNullOrEmpty(item.FenXiaoNick))
                                        //{
                                        //    DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                                        //    if (dv != null)
                                        //    {
                                        //        customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenXiaoNick));

                                        //    }
                                        //}
                                        if (customer!=null)
                                        {
                                            SaleShip saleShip = SaleShip.Finder.Find("RecID=@RecID", new UFSoft.UBF.PL.OqlParam("RecID", detailsListDomain.RecId));
                                            if (saleShip==null)
                                            {
                                                CHelper.InsertSaleShip(item, detailsListDomain, org, customer, customer1 != null ? item.ShopNo : ""); //将明细数据写入中间表
                                            }
                                          
                                        }

                                       
                                    }

                                }
                            }

                        }
                        else if (item.TradeFrom != 1L)
                        {
                            FailInfo fail = null;
                            org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                            //if (org != null)
                            //{                           
                                if (!string.IsNullOrEmpty(item.SrcTradeNo) && !item.SrcTradeNo.StartsWith("SO9"))
                                {
                                    foreach (DetailsListDomain detailsListDomain in item.DetailsList)
                                    {
                                        org = CHelper.GetOrg(detailsListDomain.GoodsNo, "");
                                        if (org != null)
                                        {
                                        if (customer != null)
                                        {
                                            if (customer.CustomerCategory?.Code == "109" && !string.IsNullOrEmpty(item.FenXiaoNick))
                                            {
                                                customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenXiaoNick));
                                            }
                                            else if (customer.CustomerCategory?.Code == "110" && !string.IsNullOrEmpty(item.CsRemark))
                                            {
                                                customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.CsRemark));

                                            }                                       
                                            else if (customer.CustomerCategory?.Code == "107" && !string.IsNullOrEmpty(item.DetailsList[0].Remark))
                                            {
                                                int indexOfFirstSpace = item.DetailsList[0].Remark.IndexOf(' ');
                                                if (indexOfFirstSpace > 0)
                                                {
                                                    customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.DetailsList[0].Remark.Substring(0, indexOfFirstSpace)));

                                                }
                                            }
                                        }
                                        if (customer1 != null)
                                        {
                                            customer = customer1;
                                        }
                                        //if (!string.IsNullOrEmpty(item.FenXiaoNick))
                                        //{
                                        //    DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                                        //    if (dv != null)
                                        //    {
                                        //        customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenXiaoNick));

                                        //    }
                                        //}
                                        if (customer != null && customer.CustomerCategory != null && (customer.CustomerCategory.Code == "102" || customer.CustomerCategory.Code == "106" || customer.CustomerCategory.Code == "107" || customer.CustomerCategory.Code == "109" || customer.CustomerCategory.Code == "110"))
                                            {
                                                SaleShip saleShip = SaleShip.Finder.Find("RecID=@RecID", new UFSoft.UBF.PL.OqlParam("RecID", detailsListDomain.RecId));
                                                if (saleShip == null)
                                                {
                                                    CHelper.InsertSaleShip(item, detailsListDomain, org, customer, customer1!=null?item.ShopNo:""); //将明细数据写入中间表
                                                }

                                            }


                                        }

                                    }
                                ////先建立销售订单
                                if (customer!=null && customer.CustomerCategory!=null && customer.CustomerCategory.Code!="102" && customer.CustomerCategory.Code!="106" && customer.CustomerCategory.Code != "107" && customer.CustomerCategory.Code != "109" && customer.CustomerCategory.Code != "110")
                                {
                                    SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (so == null)
                                    {
                                        dataJson = JsonHelper.GetWDTOrderStockOutQuerySO(item, org.Code);
                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='销售订单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTradeNo), new OqlParam("Org", org.ID));
                                        if (fail == null && customer != null)
                                        {
                                            CHelper.InsertFailInfo("销售订单创建", dataJson, org, item.SrcTradeNo, 0, "");
                                        }
                                    }

                                    //dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                                    //后建立出货单
                                    MiscSalesOrderDto orderDto = new MiscSalesOrderDto();
                                    List<MiscSalesOrderDeatail> details_list = new List<MiscSalesOrderDeatail>();
                                    orderDto.trade_from = Convert.ToInt32(item.TradeFrom);
                                    orderDto.created = item.Created;
                                    orderDto.logistics_name = item.LogisticsName;
                                    orderDto.logistics_no = item.LogisticsNo;
                                    orderDto.warehouse_no = item.WarehouseNo;
                                    orderDto.consign_time = item.ConsignTime;
                                    foreach (DetailsListDomain listDomain in item.DetailsList)
                                    {
                                        MiscSalesOrderDeatail miscSales = new MiscSalesOrderDeatail();
                                        miscSales.goods_no = listDomain.GoodsNo;
                                        miscSales.src_oid = listDomain.SrcOid;
                                        miscSales.goods_count = Convert.ToDecimal(listDomain.GoodsCount);
                                        miscSales.share_price = Convert.ToDecimal(listDomain.SharePrice);
                                        details_list.Add(miscSales);
                                    }
                                    orderDto.details_list = details_list;
                                    dataJson = JsonConvert.SerializeObject(orderDto);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='标准出货创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                    if (fail == null && customer != null)
                                    {
                                        CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.OrderNo, 0, "", item.SrcTradeNo); //来源销售订单出货创建
                                    }
                                }
                               

                            }
                           // }
                        }
                    }

                    if (response.Data.Order.Count == 500)
                    {
                        page_no++;
                        GetQMShip(page_no);
                    }
                }
                else
                {
                    CHelper.InsertU9Log(false, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }
            }
            catch (TopException e)
            {
                Console.WriteLine(e.ErrorMsg);
            }
            #endregion
        }
        private void GetQMShip(int page_no)
        {
            #region 奇门自定义接口 销售出库查询

            WdtWmsStockoutSalesQuerywithdetailRequest request = new WdtWmsStockoutSalesQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100L;
            request.Pager_ = pager;
            WdtWmsStockoutSalesQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockoutSalesQuerywithdetailRequest.
                ParamsDomain();
            //paramsDomain.StartTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.StartTime = "2023-09-09 14:08:00";
            paramsDomain.EndTime = "2023-09-09 14:18:00";
            paramsDomain.Status = "110";
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
                WdtWmsStockoutSalesQuerywithdetailResponse response = client.Execute(request);
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    //CommonSV commonSV = new CommonSV();
                    UFIDA.U9.SM.ShipPlan.ShipPlan shipPlan = null;
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                        Organization org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                        if (org != null)
                        {
                            foreach (DetailsListDomain detailsListDomain in item.DetailsList)
                            {
                                //CHelper.InsertSaleShip(item,detailsListDomain,org,Customer); //将明细数据写入中间表
                            }
                        }                       
                    }
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetQMShip(page_no);
                    }
                }
                else
                {
                    CHelper.InsertU9Log(false, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }


            }
            catch (TopException e)
            {
                Console.WriteLine(e.ErrorMsg);
            }
            #endregion
        }
        private void GetWDTShip(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Sales.queryWithDetail");
                List<StockOutQueryWithDetailReq> listreq = new List<StockOutQueryWithDetailReq>();
                StockOutQueryWithDetailReq req = new StockOutQueryWithDetailReq();
                req.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                //req.status_type = 0;
                req.status = "110";
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
                StockOutQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutQueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "旺店通销售出库单查询", apireuslt, json, url);
                }
                //  throw new Exception(res.message);

                if (res.data != null && res.data.order != null)
                {
                    UFIDA.U9.SM.ShipPlan.ShipPlan shipPlan = null;
                    foreach (var item in res.data.order)
                    {
                        if (item.trade_from == 2 || item.trade_from == 3 || item.trade_from == 4 || item.trade_from == 6)
                        {
                            string dataJson = string.Empty;
                            //string posturl = string.Empty;
                            //string apirEEEeuslt = string.Empty;
                            Organization org = CHelper.GetOrg(item.details_list[0].goods_no, "");
                            if (org != null)
                            {
                                if (!string.IsNullOrEmpty(item.src_trade_no))
                                {
                                    //dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                                    dataJson = JsonConvert.SerializeObject(item);
                                    CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.outer_no, 0, "", item.src_order_no); //来源销售订单出货创建
                                                                                                                                  //commonSV.CreateSrcShipFromSO(dataJson);
                                }
                                //shipPlan = UFIDA.U9.SM.ShipPlan.ShipPlan.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                //if (shipPlan != null)
                                //{
                                //    dataJson = JsonHelper.GetShipPlanWDT(shipPlan, item, org.Code,"SM4"); //待确认
                                //    CHelper.InsertFailInfo("标准出货创建", dataJson,org,item.order_no, 0); //来源出货计划出货单创建
                                //    //commonSV.CreateSrcShipFromShipPlan(dataJson);
                                //}
                                //else
                                //{
                                //    //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                                //    //SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                //    if (!string.IsNullOrEmpty(item.src_trade_no))
                                //    {
                                //        //dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                                //        dataJson = JsonConvert.SerializeObject(item);
                                //        CHelper.InsertFailInfo("标准出货创建", dataJson,org,item.src_order_no, 0,"",item.src_trade_no); //来源销售订单出货创建
                                //        //commonSV.CreateSrcShipFromSO(dataJson);
                                //    }

                                //}

                            }
                        }
                        //if (item.trade_from == 1)
                        //{
                        //    string dataJson = string.Empty;
                        //    //string posturl = string.Empty;
                        //    //string apirEEEeuslt = string.Empty;
                        //    Organization org = CHelper.GetOrg(item.details_list[0].goods_no, "");
                        //    if (org != null)
                        //    {
                        //        shipPlan = UFIDA.U9.SM.ShipPlan.ShipPlan.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                        //        if (shipPlan != null)
                        //        {
                        //            dataJson = JsonHelper.GetShipPlanWDT(shipPlan, item, org.Code,"SM4");
                        //            CHelper.InsertFailInfo("标准出货创建", dataJson,org,item.order_no, 0); //来源出货计划出货单创建
                        //            //commonSV.CreateSrcShipFromShipPlan(dataJson);
                        //        }
                        //        else
                        //        {
                        //            //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                        //            SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                        //            if (so != null)
                        //            {
                        //                dataJson = JsonHelper.GetShipFromSO(so, item.order_no,item.shop_no, org.Code, item.trade_from);
                        //                CHelper.InsertFailInfo("标准出货创建", dataJson,org,item.src_order_no, 0);//来源销售订单出货创建
                        //                //commonSV.CreateSrcShipFromSO(dataJson);
                        //            }

                        //        }

                        //    }
                        //}
                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetWDTShip(page_no);
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