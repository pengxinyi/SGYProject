namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using QimenCloud.Api.scene3ldsmu02o9.Request;
    using QimenCloud.Api.scene3ldsmu02o9.Response;
    using QimenCloud.Api;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Top.Api;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;
    using UFIDA.U9.LH.LHPubBP.Utility;
    using FastJSON;
    using UFIDA.U9.Base;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base.FlexField.ValueSet;
    using UFSoft.UBF.PL;
    using UFIDA.U9.SM.SO;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.PM.PO;
    using UFIDA.U9.PM.Rtn;
    using System.Linq;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFIDA.U9.LH.LHPubBE.FailInfoBE;
    using UFIDA.U9.LH.LHPubBE.SalesShipBE;
    using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockoutSalesQuerywithdetailResponse;
    using System.Data;
    using UFIDA.U9.SM.RMA;
    using UFIDA.U9.SM.Ship;
    using UFSoft.UBF.Util.DataAccess;
    using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtAftersalesRefundRefundSearchResponse;

    /// <summary>
    /// 定时创建销售订单    
    /// </summary>	
    public partial class AutoCreateSO
    {
        internal BaseStrategy Select()
        {
            return new AutoCreateSOImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoCreateSOImpementStrategy : BaseStrategy
    {
        public AutoCreateSOImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoCreateSO bpObj = (AutoCreateSO)obj;
            string starttime = DateTime.Now.AddDays(-1).ToString();
            string endtime = DateTime.Now.ToString();
            while (Convert.ToDateTime(starttime) < Convert.ToDateTime(endtime))
            {
                string end = Convert.ToDateTime(starttime).AddHours(1).ToString();
                GetQMShips(1, starttime, end);
                starttime = end;
            }
            GetQMRMA(1, DateTime.Now.AddDays(-1).ToString(), DateTime.Now.ToString());
            GetWDTStockOuts(0, DateTime.Now.AddDays(-1).ToString(), DateTime.Now.ToString());
            return "";
        }
        private RtnDataJson GetWDTStockOuts(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            while (Convert.ToDateTime(starttime) < Convert.ToDateTime(endtime))
            {
                string end = Convert.ToDateTime(starttime).AddHours(1).ToString();
                rtn = new InventorySV().GetMiscShipBaseSearchStockOuts(page_no, starttime, end);
                starttime = end;
            }

            return rtn;



        }
        /// <summary>
        /// 销售出库单排除平台单，防止重复抓取
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson GetQMShips(int page_no, string starttime, string endtime)
        {

            #region 奇门自定义接口 销售出库查询
            RtnDataJson rtn = new RtnDataJson();

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
            paramsDomain.StartTime = starttime;
            paramsDomain.EndTime = endtime;
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
                    FailInfo fails = null;

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
                                        dataJson = JsonHelper.WdtNoSrcShipJson(details, org2, item.ShopNo, item.StockCheckTime, item.WarehouseNo);
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
                                            else if (customer.CustomerCategory?.Code == "108" && !string.IsNullOrEmpty(item.NickName))
                                            {
                                                int indexOfFirstSpace = item.NickName.IndexOf('-');
                                                if (indexOfFirstSpace > 0)
                                                {
                                                    customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.NickName.Substring(0, indexOfFirstSpace)));

                                                }
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

                                        if (customer != null)
                                        {
                                            SaleShip saleShip = SaleShip.Finder.Find("RecID=@RecID", new UFSoft.UBF.PL.OqlParam("RecID", detailsListDomain.RecId));
                                            if (saleShip == null)
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
                                            else if (customer.CustomerCategory?.Code == "108" && !string.IsNullOrEmpty(item.NickName))
                                            {
                                                int indexOfFirstSpace = item.NickName.IndexOf('-');
                                                if (indexOfFirstSpace > 0)
                                                {
                                                    customer1 = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.NickName.Substring(0, indexOfFirstSpace)));

                                                }
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
                                        if (customer != null && customer.CustomerCategory != null && (customer.CustomerCategory.Code == "102" || customer.CustomerCategory.Code == "106" || customer.CustomerCategory.Code == "107" || customer.CustomerCategory.Code == "109" || customer.CustomerCategory.Code == "110"))
                                        {
                                            SaleShip saleShip = SaleShip.Finder.Find("RecID=@RecID", new UFSoft.UBF.PL.OqlParam("RecID", detailsListDomain.RecId));
                                            if (saleShip == null)
                                            {
                                                CHelper.InsertSaleShip(item, detailsListDomain, org, customer, customer1 != null ? item.ShopNo : ""); //将明细数据写入中间表
                                            }

                                        }


                                    }

                                }
                                ////先建立销售订单
                                if (customer != null && customer.CustomerCategory != null && customer.CustomerCategory.Code != "102" && customer.CustomerCategory.Code != "106" && customer.CustomerCategory.Code != "107" && customer.CustomerCategory.Code != "109" && customer.CustomerCategory.Code != "110")
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
                                        miscSales.share_price = Convert.ToDecimal(listDomain.SellPrice);
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
                        GetQMShips(page_no, starttime, endtime);
                    }
                    rtn.IsSuccess = true;
                }
                else
                {
                    CHelper.InsertU9Log(false, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }
            }
            catch (TopException e)
            {
                rtn.IsSuccess = false;
                rtn.Msg = e.ErrorMsg;
            }
            return rtn;
            #endregion


        }
        /// <summary>
        /// 奇门退回处理创建
        /// </summary>
        /// <param name="page_no"></param>
        private RtnDataJson GetQMRMA(int page_no, string starttime, string endtime)
        {

            RtnDataJson rtn = new RtnDataJson();
            WdtWmsStockinRefundQuerywithdetailRequest request = new WdtWmsStockinRefundQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockinRefundQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockinRefundQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100;
            request.Pager_ = pager;
            WdtWmsStockinRefundQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockinRefundQuerywithdetailRequest.
                ParamsDomain();
            paramsDomain.StartTime = starttime;
            paramsDomain.EndTime = endtime;
            paramsDomain.TimeType = 1L;
            paramsDomain.Status = "80";
            request.Params_ = paramsDomain;

            request.SetTargetAppKey(WDTChelper.target_app_key);// 旺店通appkey
            request.AddOtherParameter("wdt3_customer_id", WDTChelper.qmsid);

            request.WdtAppkey = WDTChelper.key;
            request.WdtSalt = WDTChelper.salt;

            request.WdtSign = WdtUtils.GetQimenCustomWdtSign(request, WDTChelper.secret);
            DefaultQimenCloudClient client = new DefaultQimenCloudClient(WDTChelper.qmapiUrl, WDTChelper.qmappkey, WDTChelper.qmsecret);

            try
            {
                WdtWmsStockinRefundQuerywithdetailResponse response = client.Execute(request);
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    //CHelper.InsertU9Log(true, "奇门退货入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    CommonSV commonSV = new CommonSV();
                    FailInfo fail3;
                    foreach (WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {

                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        ShipLine shipLine = null;
                        List<DetailListDomain> Type = null;
                        List<Organization> orgs = new List<Organization>();
                        Organization org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                        if (!string.IsNullOrEmpty(item.TidList) && org != null && item.TidList.StartsWith("SO9"))
                        {
                            string sql = string.Empty;
                            DataSet ds = new DataSet();
                            sql = string.Format(" select distinct a.docno from sm_ship a left join   sm_shipline b  on a.id = b.ship  where b.srcdocno='{0}' and a.org={1}", item.TidList.Substring(0, 20), org?.ID);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    RMA rma = RMA.Finder.Find("SrcDocNo=@DocNo and Org=@Org", new OqlParam("DocNo", row["docno"].ToString()), new OqlParam("Org", org?.ID));
                                    if (rma != null && rma.Status == RMAStatusEnum.Posting)
                                    {
                                        if (rma.DocType.Code == "H0004")
                                        {
                                            dataJson = JsonHelper.GetStockInWDTUpdateRMA(item, org, rma.ID);
                                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", rma.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                                            if (fail == null)
                                            {
                                                CHelper.InsertFailInfo("退回处理更新", dataJson, org, rma.DocNo, 0);
                                            }

                                        }
                                    }
                                }

                            }
                        }

                        if (!string.IsNullOrEmpty(item.TidList) && org != null)
                        {
                            Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org", new OqlParam("Code", item.ShopNo), new OqlParam("Org", org.ID));
                            if (customer == null)
                            {
                                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                                if (dv == null)
                                {
                                    continue;
                                }
                            }
                            if (!string.IsNullOrEmpty(item.AttachType))
                            {
                                //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                                Ship ship = null;
                                SO so = null;
                                FailInfo fail = null;
                                if (org != null)
                                {
                                    if (item.ProcessStatus == 90)
                                    {
                                        dataJson = JsonHelper.GetQMMiscShip(item, org);
                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂发单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.RefundNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        FailInfo fail2 = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂收单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                        if (fail == null && fail2 != null && Convert.ToDecimal(item.DetailsList[0]?.StockInNum) > 0)
                                        {
                                            CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.RefundNo, 0); //杂发单创建
                                        }
                                    }

                                    if (item.TidList.StartsWith("SO9"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }


                                    if (so != null)
                                    {
                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and DonationType=-1 and ItemInfo.ItemCode = @ItemCode and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new OqlParam("ItemCode", item.DetailsList[0].GoodsNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                Type = GetRefundDetailList(item.RefundNo);
                                                if (Type != null && Type.Count > 0)
                                                {
                                                    // CHelper.InsertU9Log(true, "退换单金额匹配查询", "", JsonConvert.SerializeObject(Type), WDTChelper.qmapiUrl, item.RefundNo);
                                                    dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code, Type);
                                                    if (!string.IsNullOrEmpty(dataJson))
                                                    {
                                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                                        fail3 = FailInfo.Finder.Find("DocNo=@DocNo and DocType='退回处理更新' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                                                        if (fail == null && fail3 == null)
                                                        {
                                                            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                                        }
                                                    }

                                                }



                                            }
                                        }
                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and DonationType!=-1 and ItemInfo.ItemCode = @ItemCode and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new OqlParam("ItemCode", item.DetailsList[0].GoodsNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                Type = GetRefundDetailList(item.RefundNo);
                                                if (Type != null && Type.Count > 0)
                                                {
                                                    // CHelper.InsertU9Log(true, "退换单金额匹配查询", "", JsonConvert.SerializeObject(Type), WDTChelper.qmapiUrl, item.RefundNo);
                                                    dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code, Type);
                                                    if (!string.IsNullOrEmpty(dataJson))
                                                    {
                                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                                        fail3 = FailInfo.Finder.Find("DocNo=@DocNo and DocType='退回处理更新' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                                                        if (fail == null && fail3 == null)
                                                        {
                                                            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                                        }
                                                    }

                                                }



                                            }
                                        }
                                    }
                                    else
                                    {
                                        //判断是否平台单，平台单U9不存在对应订单，走无来源退回处理
                                        //分组织拆单
                                        foreach (WdtWmsStockinRefundQuerywithdetailResponse.DetailsListDomain detail in item.DetailsList)
                                        {
                                            org = CHelper.GetOrg(detail.GoodsNo, "");
                                            //判断有多少个组织
                                            if (org != null && !orgs.Contains(org))
                                            {
                                                orgs.Add(org);
                                            }
                                        }

                                        //循环组织集合往每个组织生单
                                        foreach (Organization OrgDetail in orgs)
                                        {
                                            fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", OrgDetail.ID));
                                            if (fail == null && Convert.ToDecimal(item.DetailsList[0]?.StockInNum) > 0)
                                            {
                                                dataJson = JsonHelper.GetRMAQM(item, OrgDetail, true);
                                                CHelper.InsertFailInfo("退回处理单创建", dataJson, OrgDetail, item.OrderNo, 0);
                                            }
                                        }
                                    }


                                }

                            }
                            else if (string.IsNullOrEmpty(item.AttachType))
                            {
                                //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单--2025-1-31
                                Ship ship = null;
                                SO so = null;
                                if (org != null)
                                {
                                    if (item.TidList.StartsWith("SO9"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }

                                    if (so != null)
                                    {
                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and DonationType=-1 and ItemInfo.ItemCode = @ItemCode and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new OqlParam("ItemCode", item.DetailsList[0].GoodsNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                Type = GetRefundDetailList(item.RefundNo);
                                                if (Type != null)
                                                {
                                                    CHelper.InsertU9Log(true, "退换单金额匹配查询", "", JsonConvert.SerializeObject(Type), WDTChelper.qmapiUrl, item.RefundNo);
                                                    dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code, Type);
                                                }
                                                if (!string.IsNullOrEmpty(dataJson))
                                                {
                                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                                    fail3 = FailInfo.Finder.Find("DocNo=@DocNo and DocType='退回处理更新' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                                                    if (fail == null && fail3 == null)
                                                    {
                                                        CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                                    }

                                                }

                                            }
                                        }

                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and DonationType!=-1 and ItemInfo.ItemCode = @ItemCode and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new OqlParam("ItemCode", item.DetailsList[0].GoodsNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                Type = GetRefundDetailList(item.RefundNo);
                                                if (Type != null)
                                                {
                                                    CHelper.InsertU9Log(true, "退换单金额匹配查询", "", JsonConvert.SerializeObject(Type), WDTChelper.qmapiUrl, item.RefundNo);
                                                    dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code, Type);
                                                }
                                                if (!string.IsNullOrEmpty(dataJson))
                                                {
                                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                                    fail3 = FailInfo.Finder.Find("DocNo=@DocNo and DocType='退回处理更新' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                                                    if (fail == null && fail3 == null)
                                                    {
                                                        CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    else
                                    {
                                        //判断是否平台单，平台单U9不存在对应订单，走无来源退回处理
                                        //分组织拆单
                                        foreach (WdtWmsStockinRefundQuerywithdetailResponse.DetailsListDomain detail in item.DetailsList)
                                        {
                                            org = CHelper.GetOrg(detail.GoodsNo, "");
                                            //判断有多少个组织
                                            if (org != null && !orgs.Contains(org))
                                            {
                                                orgs.Add(org);
                                            }
                                        }

                                        //循环组织集合往每个组织生单
                                        foreach (Organization OrgDetail in orgs)
                                        {
                                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", OrgDetail.ID));
                                            if (fail == null)
                                            {
                                                dataJson = JsonHelper.GetRMAQM(item, OrgDetail);
                                                CHelper.InsertFailInfo("退回处理单创建", dataJson, OrgDetail, item.OrderNo, 0);
                                            }
                                        }

                                    }

                                }
                            }
                        }



                    }
                    if (response.Data?.Order?.Count == 100)
                    {
                        page_no++;
                        GetQMRMA(page_no, starttime, endtime);
                    }
                    rtn.IsSuccess = true;
                }

                if (response.Status != 0)
                {
                    CHelper.InsertU9Log(false, "奇门退货入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }



            }
            catch (TopException e)
            {
                rtn.IsSuccess = false;
                rtn.Msg = e.ErrorMsg;
            }
            return rtn;

        }

        public List<DetailListDomain> GetRefundDetailList(string PDNo)
        {
            // DefectChangeSearchOrder rtn = new DefectChangeSearchOrder();
            List<DetailListDomain> Type = new List<DetailListDomain>();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    WdtAftersalesRefundRefundSearchRequest request = new WdtAftersalesRefundRefundSearchRequest();
                    request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    WdtAftersalesRefundRefundSearchRequest.PagerDomain pager = new WdtAftersalesRefundRefundSearchRequest.PagerDomain();
                    pager.PageNo = 1L;
                    pager.PageSize = 100L;
                    request.Pager_ = pager;
                    WdtAftersalesRefundRefundSearchRequest.ParamsDomain paramsDomain = new WdtAftersalesRefundRefundSearchRequest.
                        ParamsDomain();
                    paramsDomain.RefundNo = PDNo;
                    //paramsDomain.StartTime = "2023-10-16 14:25:00";
                    //paramsDomain.EndTime = "2023-10-16 14:26:00";
                   // paramsDomain.Status = 90L;
                    //paramsDomain.StatusType = 0;
                    request.Params_ = paramsDomain;

                    request.SetTargetAppKey(WDTChelper.target_app_key);// 旺店通appkey
                    request.AddOtherParameter("wdt3_customer_id", WDTChelper.qmsid);

                    request.WdtAppkey = WDTChelper.key;
                    request.WdtSalt = WDTChelper.salt;

                    request.WdtSign = WdtUtils.GetQimenCustomWdtSign(request, WDTChelper.secret);
                    DefaultQimenCloudClient client = new DefaultQimenCloudClient(WDTChelper.qmapiUrl, WDTChelper.qmappkey, WDTChelper.qmsecret);
                    WdtAftersalesRefundRefundSearchResponse response = client.Execute(request);
                    if (response.Status != 0)
                    {
                        CHelper.InsertU9Log(false, "退货入库关联退换单金额查询", response.Body, PDNo, "");
                    }
                    if (response.Data != null && response.Data.Order != null)
                    {
                        Type = response.Data.Order[0].DetailList;
                    }

                    bContinue = false;

                }


            }
            catch (Exception ex)
            {

            }
            return Type;
        }
        private void GetQMSO(int page_no)
        {
            
                #region 奇门自定义接口 订单查询

                WdtSalesTradequeryQuerywithdetailRequest request = new WdtSalesTradequeryQuerywithdetailRequest();
                request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                WdtSalesTradequeryQuerywithdetailRequest.PagerDomain pager = new WdtSalesTradequeryQuerywithdetailRequest.PagerDomain();
                pager.PageNo = page_no;
                pager.PageSize = 100;
                request.Pager_ = pager;
                WdtSalesTradequeryQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtSalesTradequeryQuerywithdetailRequest.
                    ParamsDomain();
                paramsDomain.StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                    WdtSalesTradequeryQuerywithdetailResponse response = client.Execute(request);
                    if (response.Status == 0 && response.Data!=null && response.Data.Order!=null)
                    {
                        string dataJson = string.Empty;
                        // CommonSV commonSV = new CommonSV();
                        // CHelper.InsertU9Log(true, "奇门销售订单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                        foreach (WdtSalesTradequeryQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                        {
                            if (item.TradeFrom == 1)
                            {                             
                                //创建销售订单
                                Organization org = CHelper.GetOrg(item.DetailList[0].GoodsNo, "");
                                if (org != null)
                                {
                                    dataJson = JsonHelper.GetQMOrderQuerySO(item, org.Code, item.TradeFrom);
                                    CHelper.InsertFailInfo("销售订单创建", dataJson,org,item.SrcTids,0);
                                    // commonSV.CreateQMSO(dataJson);
                                }
                            }
                        
                         
                        }
                        if (response.Data.Order.Count==100)
                        {
                            page_no++;
                            GetQMSO(page_no);
                        }
                    }
                    else
                    {
                        CHelper.InsertU9Log(false, "奇门销售订单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    }


                }
                catch (TopException e)
                {
                    //异常如何处理
                    Console.WriteLine(e.ErrorMsg);
                }
                #endregion

           
        }
        private void GetWDTSO(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.TradeQuery.queryWithDetail");
                List<salesTradeQueryWithDetailReq> listreq = new List<salesTradeQueryWithDetailReq>();
                salesTradeQueryWithDetailReq req = new salesTradeQueryWithDetailReq();
                req.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "110";
                //req.trade_from = "1";
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
                string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                salesTradeQueryWithDetailRes res = JsonConvert.DeserializeObject<salesTradeQueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "旺店通销售订单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null)
                {
                    // CommonSV commonSV = new CommonSV();
                    string dataJson = string.Empty;
                    foreach (var item in res.data.order)
                    {
                        if (item.trade_from == 2 || item.trade_from == 3 || item.trade_from == 4 || item.trade_from == 6)
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            dataJson = JsonHelper.GetWDTOrderQuerySO(item, org.Code);
                            CHelper.InsertFailInfo("销售订单创建", dataJson, org, item.src_tids, 0);
                        }
                       
                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetWDTSO(page_no);
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