namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using QimenCloud.Api;
    using QimenCloud.Api.scene3ldsmu02o9.Request;
    using QimenCloud.Api.scene3ldsmu02o9.Response;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using Top.Api;
    using Top.Api.Domain;
    using UFIDA.U9.Base;
    using UFIDA.U9.Base.FlexField.ValueSet;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.InvDoc.MiscRcv;
    using UFIDA.U9.LH.LHPubBE.FailInfoBE;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.LH.LHPubBP.Option;
    using UFIDA.U9.LH.LHPubBP.Utility;
    using UFIDA.U9.SM.RMA;
    using UFIDA.U9.SM.Ship;
    using UFIDA.U9.SM.SO;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;
    using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtAftersalesRefundRefundSearchResponse;
    using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockinRefundQuerywithdetailResponse;

    /// <summary>
    /// 定时创建退回处理	--有来源
    /// </summary>	
    public partial class AutoCreateRMA
    {
        internal BaseStrategy Select()
        {
            return new AutoCreateRMAImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class AutoCreateRMAImpementStrategy : BaseStrategy
    {
        public AutoCreateRMAImpementStrategy() { }

        public override object Do(object obj)
        {
            AutoCreateRMA bpObj = (AutoCreateRMA)obj;
            QimenSearch();
            //WdtSearch();
            return "OK";
        }
        /// <summary>
        /// 奇门接口
        /// </summary>
        public void QimenSearch()
        {
            GetQMRMAS(1);
        }
        /// <summary>
        /// 旺店通接口
        /// </summary>
        public void WdtSearch()
        {
            GetWDTRMA(0);
        }
        private void  GetQMRMAS(int page_no)
        {

            WdtWmsStockinRefundQuerywithdetailRequest request = new WdtWmsStockinRefundQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockinRefundQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockinRefundQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100;
            request.Pager_ = pager;
            WdtWmsStockinRefundQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockinRefundQuerywithdetailRequest.
                ParamsDomain();
            paramsDomain.StartTime = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.TimeType = 0L;
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
                    foreach (WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {

                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        ShipLine shipLine = null;
                        FailInfo fail3;
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
                                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and DocType='退回处理更新' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                                            if (fail == null)
                                            {
                                                CHelper.InsertFailInfo("退回处理更新", dataJson, org,item.OrderNo, 0);
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
                                if (org != null)
                                {
                                    FailInfo fail = null;
                                    if (item.ProcessStatus==90)
                                    {
                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂收单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                        if (fail!=null)
                                        {
                                            dataJson = JsonHelper.GetQMMiscShip(item, org);
                                            fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂发单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.RefundNo), new OqlParam("Org", org.ID));
                                            if (fail == null && Convert.ToDecimal(item.DetailsList[0]?.StockInNum) > 0)
                                            {
                                                CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.RefundNo, 0); //杂发单创建
                                            }
                                        }
                                       
                                    }
                                  
                                    if (item.TidList.StartsWith("SO9"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }
                                    else
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
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
                                                if (Type != null && Type.Count>0)
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
                                        foreach (DetailsListDomain detail in item.DetailsList)
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
                                            if (fail == null && Convert.ToDecimal(item.DetailsList[0]?.StockInNum)>0)
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
                                //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                                Ship ship = null;
                                SO so = null;
                                if (org != null)
                                {
                                    if (item.TidList.StartsWith("SO9"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }
                                    else
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
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
                                        foreach (DetailsListDomain detail in item.DetailsList)
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
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetQMRMAS(page_no);
                    }
                }

                if (response.Status != 0)
                {
                   // CHelper.InsertU9Log(false, "奇门退货入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }



            }
            catch (TopException e)
            {
                //  throw new Exception(e.ErrorMsg);
                CHelper.InsertU9Log(false, "来源出货单创建", e.ErrorMsg, "", "", "");
            }


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
        private void GetQMRMA(int page_no)
        {

            WdtWmsStockinRefundQuerywithdetailRequest request = new WdtWmsStockinRefundQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockinRefundQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockinRefundQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100;
            request.Pager_ = pager;
            WdtWmsStockinRefundQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockinRefundQuerywithdetailRequest.
                ParamsDomain();
            paramsDomain.StartTime = DateTime.Now.AddHours(-3).ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                    foreach (WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                       
                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        ShipLine shipLine = null;
                        List<DetailListDomain> Type;
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
                       
                        if(!string.IsNullOrEmpty(item.TidList) && org != null)
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
                                if (org != null)
                                {                                
                                   // dataJson = JsonHelper.GetQMShip(item, org.Code);
                                   // CHelper.InsertFailInfo("无来源标准出货创建", dataJson, org, "", 0); //无来源出货单创建
                                   
                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂发单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.RefundNo),new OqlParam("Org",org.ID));
                                    FailInfo fail2 = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂收单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                    if (fail==null && fail2!=null)
                                    {
                                        dataJson = JsonHelper.GetQMMiscShip(item, org);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.RefundNo, 0); //杂发单创建
                                    }
                                    if (item.TidList.StartsWith("SO9"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }
                                    else
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }
                                   
                                    if (so != null)
                                    {
                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                Type = GetRefundDetailList(item.RefundNo);
                                                dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code, Type);
                                                  fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                                if (fail == null)
                                                {
                                                    CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                                }


                                            }
                                        }
                                    }
                                    else
                                    {
                                        //判断是否平台单，平台单U9不存在对应订单，走无来源退回处理
                                        //分组织拆单
                                        foreach (DetailsListDomain detail in item.DetailsList)
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
                                            if (fail == null)
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
                                //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                                Ship ship = null;
                                SO so = null;
                                if (org != null)
                                {
                                    if (item.TidList.StartsWith("SO9"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }
                                    else
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }
                                    
                                    if (so != null)
                                    {
                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                Type = GetRefundDetailList(item.RefundNo);
                                                dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code, Type);
                                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                                if (fail == null)
                                                {
                                                    CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        //判断是否平台单，平台单U9不存在对应订单，走无来源退回处理
                                        //分组织拆单
                                        foreach (DetailsListDomain detail in item.DetailsList)
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
                                           FailInfo  fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", OrgDetail.ID));
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
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetQMRMA(page_no);
                    }
                }

                if (response.Status!=0)
                {
                    CHelper.InsertU9Log(false, "奇门退货入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }
                 


            }
            catch (TopException e)
            {
                //  throw new Exception(e.ErrorMsg);
                CHelper.InsertU9Log(false, "来源出货单创建", e.ErrorMsg, "", "", "");
            }


        }
        private void GetWDTRMA(int page_no)
        {
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Refund.queryWithDetail");
            //    List<StockinRefundqueryWithDetailReq> listreq = new List<StockinRefundqueryWithDetailReq>();
            //    StockinRefundqueryWithDetailReq req = new StockinRefundqueryWithDetailReq();
            //    req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            //    req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = "80";
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
            //  //  string apireuslt = Tools.HttpPost(url, json);
            //    StockinRefundqueryWithDetailRes res = JsonConvert.DeserializeObject<StockinRefundqueryWithDetailRes>(apireuslt);
            //    if (res.status!=0)
            //    {
            //        CHelper.InsertU9Log(false, "旺店通退货入库单查询", apireuslt, json, url);
            //    }
            //    if (res.data != null && res.data.order != null)
            //    {
            //        CommonSV commonSV = new CommonSV();
            //        foreach (var item in res.data.order)
            //        {

            //            string dataJson = string.Empty;
            //            string posturl = string.Empty;
            //            string apirEEEeuslt = string.Empty;
            //            //生成无来源出货 单据类型 SM5
                        
            //            //生成来源出货退回处理，原始订单匹配出货单号
                        
            //                Organization org =CHelper.GetOrg(item.details_list[0].goods_no,"");
            //                Ship ship = null;
            //                if (org != null)
            //                {
            //                if (item.attach_type>=0)
            //                {
            //                    dataJson = JsonHelper.GetWDTShip(item, org.Code);
            //                    CHelper.InsertFailInfo("无来源标准出货创建", dataJson, org, item.order_no, 0);//无来源出货单创建
            //                }
                               
            //                    SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.tid_list), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
            //                    ShipLine shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
            //                    if (shipLine != null)
            //                    {
            //                        ship = Ship.Finder.FindByID(shipLine.Ship.ID);
            //                    }
            //                }

            //                if (ship != null)
            //                {
            //                    dataJson = JsonHelper.GetSrcShipRMAWdt(item, ship, org.Code);
            //                    CHelper.InsertFailInfo("退回处理单创建", dataJson,org, item.order_no, 0);//退回处理单创建
            //                    // commonSV.CreateSrcShipRMA(dataJson);
            //                }
                     


            //        }
            //        if (res.data.order.Count==100)
            //        {
            //            page_no++;
            //            GetWDTRMA(page_no);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
    }

    #endregion


}