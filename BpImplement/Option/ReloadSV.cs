using Newtonsoft.Json;
using QimenCloud.Api.scene3ldsmu02o9.Request;
using QimenCloud.Api.scene3ldsmu02o9.Response;
using QimenCloud.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using UFIDA.U9.Base.Organization;
using UFIDA.U9.LH.LHPubBE.FailInfoBE;
using UFIDA.U9.LH.LHPubBP.Model;
using UFIDA.U9.LH.LHPubBP.Utility;
using UFIDA.U9.PM.PO;
using UFIDA.U9.SM.Ship;
using UFIDA.U9.SM.SO;
using UFIDA.U9.Base.FlexField.ValueSet;
using UFIDA.U9.CBO.SCM.Customer;
using UFSoft.UBF.PL;
using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockoutSalesQuerywithdetailResponse;
using UFIDA.U9.SM.RMA;
using UFIDA.U9.PM.Rcv;
using System.Data;
using UFSoft.UBF.Util.DataAccess;
using UFIDA.U9.CBO.SCM.Warehouse;
using UFIDA.U9.InvDoc.TransferIn;
using UFSoft.UBF.Business;
using UFIDA.U9.LH.LHPubBE.SalesShipBE;
using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtAftersalesRefundRefundSearchResponse;
using UFIDA.U9.InvDoc.MiscShip;
using UFIDA.U9.Base;

namespace UFIDA.U9.LH.LHPubBP.Option
{
    internal class ReloadSV
    {
        /// <summary>
        /// 重新拉取旺店通数据方法
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson ReloadRecord(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                ReloadQueryInfo Query = JsonConvert.DeserializeObject<ReloadQueryInfo>(jsondata);
                switch (Query.DocQuery)
                {

                    case "奇门退货入库单":
                        rtn = GetQMRMA(1, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "奇门历史退货入库单":
                        rtn = GetQMRMAHIS(0, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "奇门线上退货入库单":
                        rtn = GetQMRMAS(1, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "旺店通退货出库单":
                        rtn = GetRcvDoc(0, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "奇门销售出库单":
                        rtn = GetQMShip(1, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "旺店通销售出库单":
                        rtn = GetQMShips(1, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "旺店通线上销售出库单":
                        rtn = GetQMQMQM(1, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "旺店通采购入库单":
                        rtn = GetRcv(0, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "退换单":
                        rtn = GetARDoc(1, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    //case "调拨入库单":
                    //    rtn = GetTransferDoc(0, Query.starttime.ToString(), Query.endtime.ToString());
                    //    break;
                    case "调拨入库单":
                          rtn = CreateU9TransferIn(0, Query.starttime.ToString(), Query.endtime.ToString());
                        break;
                    case "奇门预入库单":
                        rtn = new InventorySV().GetRcvTransS(1, Query.starttime, Query.endtime);
                        break;
                    case "旺店通其他入库单":
                        rtn = new InventorySV().GetMiscShipRcvs(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通外仓调整入库单":
                        rtn = new InventorySV().GetMiscRcvOuts(0, Query.starttime, Query.endtime);
                        break;

                    case "旺店通其他出库单":
                        rtn = new InventorySV().GetMiscShipss(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通外仓调整出库单":
                        rtn = new InventorySV().GetMiscShipOuts(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通汇总入库管理":
                        rtn = new InventorySV().GetMiscShipRcvBaseSearchs(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通出库管理":
                        rtn = new InventorySV().GetMiscShipBaseStockOut(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通出库明细盘点出库":
                        rtn = GetWDTStockOut(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通入库明细盘点入库":
                        rtn = GetWDTStockIn(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通生产单管理":
                        rtn = new InventorySV().GetShiftDocFromProcess(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通盘点入库":
                        rtn = new InventorySV().GetMiscRcvStockPdIns(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通盘点出库":
                        rtn = new InventorySV().GetMiscShipStockPdOuts(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通调入单更新":
                        rtn = new InventorySV().GetUpdateTransferIn();
                        break;
                    case "旺店通杂收单更新":
                        rtn = new InventorySV().GetUpdateMiscRcv(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通杂发单更新":
                        rtn = new InventorySV().GetUpdateMiscShip(0, Query.starttime, Query.endtime);
                        break;
                    case "U9杂收单更新":
                        rtn = new InventorySV().UpdateMiscRcv();
                        break;
                    case "U9杂发单更新":
                        rtn = new InventorySV().UpdateMiscShip();
                        break;
                    case "旺店通出库明细":
                        rtn = GetWDTStockOuts(0, Query.starttime, Query.endtime);
                        break;
                    case "旺店通出库明细60":                       
                        rtn = new InventorySV().GetMiscShipBaseSearchStockOuts(0, Query.starttime, Query.endtime);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        private RtnDataJson GetARDoc(long page_no, string startime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
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
                        GetARDoc(page_no,startime,endtime);
                    }
                    rtn.IsSuccess = true;
                }
                if (response.Status != 0)
                {
                    CHelper.InsertU9Log(false, "奇门退换单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }

            }
            catch (TopException e)
            {
                rtn.IsSuccess = false;
                rtn.Msg = e.ErrorMsg;
            }
            return rtn;
        }
        /// <summary>
        /// 采购入库创建收货单
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson GetRcv(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Purchase.queryWithDetail");
                List<StockinPurchaseOrderqueryWithDetailReq> listreq = new List<StockinPurchaseOrderqueryWithDetailReq>();
                StockinPurchaseOrderqueryWithDetailReq req = new StockinPurchaseOrderqueryWithDetailReq();
                req.start_time = starttime;
                req.end_time = endtime;
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
                    string PODocNo = string.Empty;
                    FailInfo fail = null;
                    foreach (var item in res.data.order)
                    {
                        //先调用采购单查询接口返回U9采购单号
                        PODocNo = GetPODocNo(item.purchase_no);
                        PurchaseOrder miscRcvTrans = PurchaseOrder.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", PODocNo));
                        if (miscRcvTrans != null)
                        {
                            Organization org = CHelper.GetOrg(miscRcvTrans.POLines[0].ItemInfo.ItemCode, "");
                            if (org != null)
                            {
                                //标准收货
                                if (miscRcvTrans.DocType.Code == "PO20")
                                {
                                    dataJson = JsonHelper.GetWDTReceivement(miscRcvTrans, item, org);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("标准收货创建", dataJson, org, item.order_no, 0);
                                    }

                                    //commonSV.CreateMiscRcvTrans(dataJson);
                                }
                                if (miscRcvTrans.DocType.Code == "PO22")
                                {
                                    dataJson = JsonHelper.GetPurReturn(miscRcvTrans, item, org.Code);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("委外收货创建", dataJson, org, item.order_no, 0, "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Organization org = CHelper.GetOrg(item.details_list[0].goods_no, "");
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
                    GetRcv(page_no, starttime, endtime);
                }
                rtn.IsSuccess = true;
            }

            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }
            return rtn;
        }

        public RtnDataJson GetTransferDoc(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                //string docTypes = CHelper.GetDefineValueUrl("CustParam", "007");
                //docTypes = docTypes.Replace(",", "','");
                sql = string.Format(@"SELECT DISTINCT a.ID,a.DocNo  FROM dbo.InvDoc_TransferIn AS a
INNER JOIN dbo.InvDoc_TransInDocType AS b ON a.TransInDocType = b.ID
INNER JOIN dbo.InvDoc_TransInLine AS c ON a.ID = c.TransferIn
INNER JOIN dbo.InvDoc_TransInSubLine AS d ON c.ID = d.TransInLine
WHERE a.Status = 0 AND b.code='TransIn011' AND (c.DescFlexSegments_PrivateDescSeg2 <> 'True' OR d.DescFlexSegments_PrivateDescSeg1 <> 'True')
--AND DocNo IN ( 'TransIn0101012023110033','TransIn0101012023110032')");
                //                sql = string.Format(@" SELECT TOP 100 a.ID,a.DocNo FROM dbo.InvDoc_TransferIn AS a
                //INNER JOIN dbo.InvDoc_TransInDocType AS b ON a.TransInDocType = b.ID
                //WHERE a.Status = 1 AND b.Code IN ('TransIn009','TransIn010')");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TransferIn transfer = TransferIn.Finder.FindByID(Convert.ToInt64(row["ID"]));
                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Transfer.queryWithDetail");
                        List<StockinTransferqueryWithDetail> listreq = new List<StockinTransferqueryWithDetail>();
                        StockinTransferqueryWithDetail req = new StockinTransferqueryWithDetail();
                      //  req.transfer_no = transfer?.DescFlexField.PrivateDescSeg1;
                        req.status = "80";
                        listreq.Add(req);
                        string json = JsonConvert.SerializeObject(listreq);
                        list.Add("body", json);
                        //分页数据
                        list.Add("page_size", "10");
                        list.Add("page_no", page_no.ToString());
                        list.Add("calc_total", "1");
                        string sign = WDTChelper.GetWDTSign(list);
                        list.Add("sign", sign);
                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                        string apireuslt = Tools.HttpPost(url, json);
                        StockinTransferqueryWithRes res = JsonConvert.DeserializeObject<StockinTransferqueryWithRes>(apireuslt);
                        if (res.status != 0)
                        {
                            CHelper.InsertU9Log(false, "调拨入库单查询", apireuslt, json, url);
                        }
                        if (res.data != null && res.data.order != null && res.data.order.Count > 0)
                        {
                            string dataJson = string.Empty;
                            foreach (StockinTransferOrderDto item in res.data.order)
                            {

                                using (ISession session = Session.Open())
                                {
                                    //foreach (TransInLine tranLine in transfer.TransInLines)
                                    //{
                                    foreach (StockinTransferOrderDto order in res.data.order)
                                    {
                                        if (order.src_order_no == transfer.DocNo)
                                        {
                                            foreach (TransInLine tranLine in transfer.TransInLines)
                                            {
                                                foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
                                                {
                                                    if (tranLine.ID == lnSubLine.TransInLine.ID)
                                                    {
                                                        foreach (StockinTransferOrderDeatail detail in order.detail_list)
                                                        {
                                                            if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                            {
                                                                tranLine.StoreUOMQty = detail.num;
                                                                tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                //  tranLine.TransInWh.Code = order.wa

                                                                if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                {
                                                                    tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                }

                                                                lnSubLine.StoreUOMQty = detail.num;
                                                                lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;

                                                                lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";
                                                                if (lnSubLine.CostPrice > 0)
                                                                {
                                                                    lnSubLine.CostMoney = lnSubLine.StoreUOMQty * lnSubLine.CostPrice;
                                                                }
                                                                if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                    lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    session.Commit();
                                }


                                //string DataJson = JsonHelper.GetWDTUpdateTransferIn(transfer);
                                //if (!string.IsNullOrEmpty(DataJson))
                                //{
                                //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", transfer.Org.ID), new OqlParam("DocNo", transfer.DocNo));
                                //    if (failInfo == null)
                                //        CHelper.InsertFailInfo("调入单更新", DataJson, transfer.Org, transfer.DocNo, 0);
                                //}

                            }
                            rtn.IsSuccess = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                rtn.IsSuccess =false;
                rtn.Msg = ex.Message;
            }
            return rtn;
        }
        public RtnDataJson CreateU9TransferIn(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Transfer.queryWithDetail");
                List<StockinTransferqueryWithDetail> listreq = new List<StockinTransferqueryWithDetail>();
                StockinTransferqueryWithDetail req = new StockinTransferqueryWithDetail();
                req.start_time = starttime;
                req.end_time = endtime;
                req.status = "80";
                listreq.Add(req);
                string json = JsonConvert.SerializeObject(listreq);
                list.Add("body", json);
                //分页数据
                list.Add("page_size", "10");
                list.Add("page_no", page_no.ToString());
                list.Add("calc_total", "1");
                string sign = WDTChelper.GetWDTSign(list);
                list.Add("sign", sign);
                string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                string apireuslt = Tools.HttpPost(url, json);
                StockinTransferqueryWithRes res = JsonConvert.DeserializeObject<StockinTransferqueryWithRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "调拨入库单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null && res.data.order.Count > 0)
                {
                    string dataJson = string.Empty;
                    Organization org = null;
                    Warehouse WH = null;
                    Organization organization = Organization.FindByCode("192");

                    foreach (StockinTransferOrderDto item in res.data.order)
                    {
                        if (item.remark.Contains("停止等待"))
                        {
                            continue;
                        }
                        List<Organization> orgs = new List<Organization>();
                        if (organization != null)
                        {
                            WH = Warehouse.FindByCode(organization, item.from_warehouse_no);
                            if (WH != null)
                            {

                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new OqlParam("Org", organization.ID));
                                if (fail == null)
                                {
                                    dataJson = JsonHelper.GetTransferInDocFromIn(item, organization);
                                    CHelper.InsertFailInfo("调入单创建", dataJson, organization, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                        }
                        else
                        {
                            foreach (StockinTransferOrderDeatail detail in item.detail_list)
                            {
                                org = CHelper.GetOrg(detail.goods_no, "");
                                //判断有多少个组织
                                if (org != null && !orgs.Contains(org))
                                {
                                    orgs.Add(org);
                                }
                            }

                            //循环组织集合往每个组织生单
                            foreach (Organization OrgDetail in orgs)
                            {
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.order_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetTransferInDocsFromIn(item, OrgDetail);
                                    CHelper.InsertFailInfo("调入单创建", dataJson, OrgDetail, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                        }
                    }



                }
                if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                {
                    page_no++;
                    CreateU9TransferIn(page_no,starttime,endtime);
                }
                rtn.IsSuccess = true;

            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg= ex.Message;
            }
            return rtn;
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
        /// <summary>
        /// 奇门订单查询
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson GetQMSO(int page_no, string starttime, string endtime)
        {
            #region 奇门自定义接口 订单查询
            RtnDataJson rtn = new RtnDataJson();
            WdtSalesTradequeryQuerywithdetailRequest request = new WdtSalesTradequeryQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtSalesTradequeryQuerywithdetailRequest.PagerDomain pager = new WdtSalesTradequeryQuerywithdetailRequest.PagerDomain();
            pager.PageNo = page_no;
            pager.PageSize = 100;
            request.Pager_ = pager;
            WdtSalesTradequeryQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtSalesTradequeryQuerywithdetailRequest.
                ParamsDomain();
            paramsDomain.StartTime = starttime;
            paramsDomain.EndTime = endtime;
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
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
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
                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.SrcTids));
                                if (fail == null)
                                {
                                    CHelper.InsertFailInfo("销售订单创建", dataJson, org, item.SrcTids, 0);
                                }
                                // commonSV.CreateQMSO(dataJson);
                            }
                        }

                    }
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetQMSO(page_no, starttime, endtime);
                    }
                }
                else
                {
                    CHelper.InsertU9Log(false, "奇门销售订单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                }
                rtn.IsSuccess = true;

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
        /// wdt销售订单查询
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson GetWDTSO(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.TradeQuery.queryWithDetail");
                List<salesTradeQueryWithDetailReq> listreq = new List<salesTradeQueryWithDetailReq>();
                salesTradeQueryWithDetailReq req = new salesTradeQueryWithDetailReq();
                req.start_time = starttime;
                req.end_time = endtime;
                req.status = "110";
                // req.trade_from = "1";
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
                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_tids));
                            if (fail == null)
                            {
                                CHelper.InsertFailInfo("销售订单创建", dataJson, org, item.src_tids, 0);
                            }
                        }



                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetWDTSO(page_no, starttime, endtime);
                    }
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }
            return rtn;
        }
        /// <summary>
        ///奇门销售出库创建出货单
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson GetQMShip(int page_no, string starttime, string endtime)
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
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                        Organization org = null;
                        Customer customer = Customer.Finder.Find("Code=@Code", new OqlParam("Code", item.ShopNo));
                        //if (customer == null)
                        //{
                        //    continue;
                        //}
                        //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");

                        if (item.TradeFrom == 1L)
                        {
                            if (item.Status=="110")
                            {
                                if (!string.IsNullOrEmpty(item.SrcTradeNo) && !item.SrcTradeNo.Contains("SO"))
                                {
                                    foreach (DetailsListDomain detailsListDomain in item.DetailsList)
                                    {
                                        org = CHelper.GetOrg(detailsListDomain.GoodsNo, "");
                                        if (org != null)
                                        {
                                            if (!string.IsNullOrEmpty(item.FenXiaoNick))
                                            {
                                                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                                                if (dv != null)
                                                {
                                                    customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenXiaoNick));

                                                }
                                            }
                                            if (customer!=null)
                                            {
                                                SaleShip saleShip = SaleShip.Finder.Find("RecID=@RecID", new UFSoft.UBF.PL.OqlParam("RecID", detailsListDomain.RecId));
                                                if (saleShip == null)
                                                {
                                                    CHelper.InsertSaleShip(item, detailsListDomain, org, customer); //将明细数据写入中间表
                                                }
                                            }
                                         
                                        }

                                    }
                                }
                            }
                          

                        }
                     
                    }

                    if (response.Data.Order.Count == 500)
                    {
                        page_no++;
                        GetQMShip(page_no, starttime, endtime);
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
                                                CHelper.InsertSaleShip(item, detailsListDomain, org, customer, customer1 != null?item.ShopNo:""); //将明细数据写入中间表
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
                        GetQMShips(page_no,starttime,endtime);
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
        private RtnDataJson GetQMQMQM(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            while (Convert.ToDateTime(starttime) <Convert.ToDateTime(endtime))
            {
                string end = Convert.ToDateTime(starttime).AddHours(1).ToString();
                rtn = GetQMShips(page_no, starttime, end);
                starttime = end;
            }

            return rtn;
             


        }
        private RtnDataJson  GetWDTStockIn(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            while (Convert.ToDateTime(starttime) < Convert.ToDateTime(endtime))
            {
                string end = Convert.ToDateTime(starttime).AddHours(1).ToString();
                rtn = new InventorySV().GetMiscRcvBaseStockIn(page_no, starttime, end);
                starttime = end;
            }

            return rtn;



        }
        private RtnDataJson GetWDTStockOut(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            while (Convert.ToDateTime(starttime) < Convert.ToDateTime(endtime))
            {
                string end = Convert.ToDateTime(starttime).AddHours(1).ToString();
                rtn = new InventorySV().GetMiscShipBaseStockOut(page_no, starttime, end);
                starttime = end;
            }

            return rtn;



        }
        private RtnDataJson  GetWDTStockOuts(int page_no, string starttime, string endtime)
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
        private RtnDataJson  GetRMARMARMA(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            while (Convert.ToDateTime(starttime) < Convert.ToDateTime(endtime))
            {
                string end = Convert.ToDateTime(starttime).AddHours(1).ToString();
                rtn = GetQMRMAS(page_no, starttime, end);
                starttime = end;
            }

            return rtn;



        }
        /// <summary>
        /// 抓线上手工订单做合并
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson  GetQMOnLineShips(int page_no, string starttime, string endtime)
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
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                        if (item.Status != "110")
                        {
                            continue;
                        }
                        Organization org = null;
                        Customer customer = Customer.Finder.Find("Code=@Code", new OqlParam("Code", item.ShopNo));
                        //if (customer == null)
                        //{
                        //    continue;
                        //}
                        //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");

                      
                        if (item.TradeFrom != 1L)
                        {
                            FailInfo fail = null;

                            if (!string.IsNullOrEmpty(item.SrcTradeNo) && !item.SrcTradeNo.Contains("SO"))
                            {
                                foreach (DetailsListDomain detailsListDomain in item.DetailsList)
                                {
                                    org = CHelper.GetOrg(detailsListDomain.GoodsNo, "");
                                    if (org != null)
                                    {
                                        if (!string.IsNullOrEmpty(item.FenXiaoNick))
                                        {
                                            DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                                            if (dv != null)
                                            {
                                                customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenXiaoNick));

                                            }
                                        }
                                        if (customer != null && customer.CustomerCategory != null && (customer.CustomerCategory.Code=="102" || customer.CustomerCategory.Code == "106"))
                                        {
                                            SaleShip saleShip = SaleShip.Finder.Find("RecID=@RecID", new UFSoft.UBF.PL.OqlParam("RecID", detailsListDomain.RecId));
                                            if (saleShip == null)
                                            {
                                                CHelper.InsertSaleShip(item, detailsListDomain, org, customer); //将明细数据写入中间表
                                            }
                                        }

                                    }

                                }
                            }

                        }
                    }

                    if (response.Data.Order.Count == 500)
                    {
                        page_no++;
                        GetQMOnLineShips(page_no, starttime, endtime);
                    }
                    rtn.IsSuccess = true;
                }
                else
                {
                   // CHelper.InsertU9Log(false, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
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
        /// 旺店通销售出库创建出货单
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private RtnDataJson GetWDTShip(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Sales.queryWithDetail");
                List<StockOutQueryWithDetailReq> listreq = new List<StockOutQueryWithDetailReq>();
                StockOutQueryWithDetailReq req = new StockOutQueryWithDetailReq();
                req.start_time = starttime;
                req.end_time = endtime;
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
                                shipPlan = UFIDA.U9.SM.ShipPlan.ShipPlan.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                if (shipPlan != null)
                                {
                                    dataJson = JsonHelper.GetShipPlanWDT(shipPlan, item, org.Code, "SM4");
                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.order_no, 0);//来源出货计划出货单创建
                                    }

                                    //commonSV.CreateSrcShipFromShipPlan(dataJson);
                                }
                                else
                                {
                                    //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                                    SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (so != null)
                                    {
                                        dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                                        FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_order_no));
                                        if (fail == null)
                                        {
                                            CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.src_order_no, 0);//来源销售订单出货创建
                                        }


                                    }

                                }

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
                        //            dataJson = JsonHelper.GetShipPlanWDT(shipPlan, item, org.Code, "SM4");
                        //            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                        //            if (fail == null)
                        //            {
                        //                CHelper.InsertFailInfo("来源出货计划出货单创建", dataJson, org,item.order_no, 0);
                        //            }

                        //            //commonSV.CreateSrcShipFromShipPlan(dataJson);
                        //        }
                        //        else
                        //        {
                        //            //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                        //            SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                        //            if (so != null)
                        //            {
                        //                dataJson = JsonHelper.GetShipFromSO(so, item.order_no, item.shop_no, org.Code, item.trade_from, item.logistics_name, item.logistics_no);
                        //                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_order_no));
                        //                if (fail == null)
                        //                {
                        //                    CHelper.InsertFailInfo("来源销售订单出货创建", dataJson,org, item.src_order_no, 0);
                        //                }                                                                      
                        //            }

                        //        }

                        //    }
                        //}
                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetWDTShip(page_no, starttime, endtime);
                    }
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }
            return rtn;
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
                                                CHelper.InsertFailInfo("退回处理更新", dataJson, org, item.OrderNo, 0);
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
                                SO  so = null;
                                FailInfo fail = null;
                                if (org != null)
                                {
                                    if (item.ProcessStatus==90)
                                    {
                                        dataJson = JsonHelper.GetQMMiscShip(item, org);
                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂发单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.RefundNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        FailInfo fail2 = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂收单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new OqlParam("Org", org.ID));
                                        if (fail == null && fail2!=null && Convert.ToDecimal(item.DetailsList[0]?.StockInNum) > 0)
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
                                //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
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

                //if (response.Status != 0)
                //{
                //    CHelper.InsertU9Log(false, "奇门退货入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                //}



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
                    //paramsDomain.Status = 90L;
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
        /// <summary>
        /// 奇门历史退货入库单-退回处理创建
        /// </summary>
        /// <param name="page_no"></param>
        private RtnDataJson GetQMRMAHIS(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Refund.queryHisWithDetail");
            List<StockinRefundqueryWithDetailReq> listreq = new List<StockinRefundqueryWithDetailReq>();
            StockinRefundqueryWithDetailReq req = new StockinRefundqueryWithDetailReq();
            req.start_time = starttime;
            req.end_time = endtime;         
            req.status = "80";
            req.time_type = 1;
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
            StockinRefundqueryWithDetailRes res = JsonConvert.DeserializeObject<StockinRefundqueryWithDetailRes>(apireuslt);
            if (res.status != 0)
            {
                CHelper.InsertU9Log(false, "旺店通历史退货入库单查询", apireuslt, json, url);
            }

            try
            {
                //Console.WriteLine("code:" + response.Code);
                //Console.WriteLine("message:" + response.Message);
                //Console.WriteLine("body:" + response.Body);
                if (res.status == 0 && res.data != null && res.data.order != null)
                {
                    //CHelper.InsertU9Log(true, "奇门退货入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    CommonSV commonSV = new CommonSV();
                    foreach (var item in res.data.order)
                    {

                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        ShipLine shipLine = null;
                        Organization org = CHelper.GetOrg(item.details_list[0].goods_no, "");
                        //if (!string.IsNullOrEmpty(item.tid_list) && org != null && item.tid_list.Contains("SO"))
                        //{
                        //    string sql = string.Empty;
                        //    DataSet ds = new DataSet();
                        //    sql = string.Format(" select distinct a.docno from sm_ship a left join   sm_shipline b  on a.id = b.ship  where b.srcdocno='{0}' and a.org={1}", item.tid_list.Substring(0, 20), org?.ID);
                        //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                        //    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                        //    {
                        //        foreach (DataRow row in ds.Tables[0].Rows)
                        //        {
                        //            RMA rma = RMA.Finder.Find("SrcDocNo=@DocNo and Org=@Org", new OqlParam("DocNo", row["docno"].ToString()), new OqlParam("Org", org?.ID));
                        //            if (rma != null && rma.Status == RMAStatusEnum.Posting)
                        //            {
                        //                if (rma.DocType.Code == "H0004")
                        //                {
                        //                    dataJson = JsonHelper.GetStockInWDTUpdateRMA(item, org, rma.ID);
                        //                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", rma.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org?.ID));
                        //                    if (fail == null)
                        //                    {
                        //                        CHelper.InsertFailInfo("退回处理更新", dataJson, org, rma.DocNo, 0);
                        //                    }

                        //                }
                        //            }
                        //        }

                        //    }
                        //}

                        if (!string.IsNullOrEmpty(item.tid_list) && org != null)
                        {
                            Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org", new OqlParam("Code", item.shop_no), new OqlParam("Org", org.ID));
                            if (customer == null)
                            {
                                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.shop_no));
                                if (dv == null)
                                {
                                    continue;
                                }
                            }
                       
                                //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                                Ship ship = null;
                                SO so = null;
                                if (org != null)
                                {
                                    // dataJson = JsonHelper.GetQMShip(item, org.Code);
                                    // CHelper.InsertFailInfo("无来源标准出货创建", dataJson, org, "", 0); //无来源出货单创建
                                    dataJson = JsonHelper.GetQMMiscShipFromHis(item, org);
                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='杂发单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.refund_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.refund_no, 0); //杂发单创建
                                    }
                                    if (item.tid_list.Contains("SO"))
                                    {
                                        so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.tid_list.Length >= 20 ? item.tid_list.Substring(0, 20) : item.tid_list), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    }


                                    if (so != null)
                                    {
                                        shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and ItemInfo.ItemCode = @ItemCode and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new OqlParam("ItemCode", item.details_list[0].goods_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (shipLine != null)
                                        {
                                            ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                            if (ship != null)
                                            {
                                                dataJson = JsonHelper.GetSrcShipRMAQMFromHis(item, ship, org.Code);
                                                fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                                if (fail == null)
                                                {
                                                    CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.order_no, 0);//来源出货单退回处理创建
                                                }


                                            }
                                        }
                                    }
                                    else
                                    {
                                        //判断是否平台单，平台单U9不存在对应订单，走无来源退回处理
                                        dataJson = JsonHelper.GetRMAQMFromHis(item, org);
                                        fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                        if (fail == null)
                                        {
                                            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.order_no, 0);//来源出货单退回处理创建
                                        }
                                    }


                                }

                            
                      
                        }



                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetQMRMAHIS(page_no, starttime, endtime);
                    }
                    rtn.IsSuccess = true;
                }

            



            }
            catch (TopException e)
            {
                rtn.IsSuccess = false;
                rtn.Msg = e.ErrorMsg;
            }
            return rtn;

        }
        private RtnDataJson  GetQMRMAS(int page_no, string starttime, string endtime)
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
                    foreach (WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {

                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        ShipLine shipLine = null;
                        Organization org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                       

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
                            //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                            Ship ship = null;
                            SO so = null;
                            
                                if (item.TidList.Contains("SO"))
                                {
                                    so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList.Length >= 20 ? item.TidList.Substring(0, 20) : item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                }

                                if (so != null)
                                {
                                    //shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    //if (shipLine != null)
                                    //{
                                    //    ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                    //    if (ship != null)
                                    //    {
                                    //        dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code);
                                    //        FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    //        if (fail == null)
                                    //        {
                                    //            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                    //        }

                                    //    }
                                    //}
                                }
                                else
                                {
                                    //判断是否平台单，平台单U9不存在对应订单，走无来源退回处理
                                    dataJson = JsonHelper.GetRMAQM(item, org);
                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org and DocType='退回处理单创建'", new UFSoft.UBF.PL.OqlParam("DocNo", item.OrderNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建
                                    }

                                }
 
                        }



                    }
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        GetQMRMAS(page_no, starttime, endtime);
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
        private RtnDataJson GetWDTRMA(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Refund.queryWithDetail");
                List<StockinRefundqueryWithDetailReq> listreq = new List<StockinRefundqueryWithDetailReq>();
                StockinRefundqueryWithDetailReq req = new StockinRefundqueryWithDetailReq();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "80";
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
                StockinRefundqueryWithDetailRes res = JsonConvert.DeserializeObject<StockinRefundqueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "旺店通退货入库单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null)
                {
                    CommonSV commonSV = new CommonSV();
                    foreach (var item in res.data.order)
                    {

                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        //生成无来源出货 单据类型 SM5

                        //生成来源出货退回处理，原始订单匹配出货单号

                        Organization org = CHelper.GetOrg(item.details_list[0].goods_no, "");
                        Ship ship = null;
                        if (org != null)
                        {
                            if (item.attach_type >= 0)
                            {
                                dataJson = JsonHelper.GetWDTShip(item, org.Code);
                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                                if (fail == null)
                                {
                                    CHelper.InsertFailInfo("无来源标准出货创建", dataJson, org, item.order_no, 0);//无来源出货单创建
                                }

                            }

                            SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.tid_list), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            ShipLine shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            if (shipLine != null)
                            {
                                ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                            }
                        }

                        if (ship != null)
                        {
                            dataJson = JsonHelper.GetSrcShipRMAWdt(item, ship, org.Code);
                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                            if (fail == null)
                            {
                                CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.order_no, 0);//退回处理单创建
                            }


                        }



                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetWDTRMA(page_no, starttime, endtime);
                    }
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }
            return rtn;
        }
        /// <summary>
        /// 采购退货
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private RtnDataJson GetRcvDoc(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.PurchaseReturn.queryWithDetail");
                List<PurchaseReturnqueryWithDetailReq> listreq = new List<PurchaseReturnqueryWithDetailReq>();
                PurchaseReturnqueryWithDetailReq req = new PurchaseReturnqueryWithDetailReq();
                req.start_time = starttime;
                req.end_time = endtime;
                req.status = "110";
                req.time_type = 2;
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
                PurchaseReturnqueryWithDetailRes res = JsonConvert.DeserializeObject<PurchaseReturnqueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "采购退货出库单查询", apireuslt, json, url);
                }
                else if (res.data != null && res.data.order != null)
                {
                    Receivement miscRcvTrans = null;
                    string dataJson = string.Empty;
                    FailInfo fail = null;
                    foreach (var item in res.data.order)
                    {
                        miscRcvTrans = Receivement.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_order_no));
                        if (miscRcvTrans != null)
                        {
                            Organization org = CHelper.GetOrg(miscRcvTrans.RcvLines[0].ItemInfo.ItemCode, "");
                            if (org != null)
                            {
                                if (miscRcvTrans.DocType.Code == "RCV95" && miscRcvTrans.Status == RcvStatusEnum.Approving)
                                {
                                    dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", miscRcvTrans.DocNo));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("采购退货更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                    }

                                }
                                if (miscRcvTrans.DocType.Code == "RCV93" && miscRcvTrans.Status == RcvStatusEnum.Approving)
                                {
                                    dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", miscRcvTrans.DocNo));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("委外退货更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                    }

                                }
                            }
                        }

                        //using (ISession session = Session.Open())
                        //{
                        //    foreach (RcvLine tranLine in miscRcvTrans.RcvLines)
                        //    {
                        //        foreach (PurchaseReturnOrderDeatail Deatail in item.details_list)
                        //        {
                        //            if (tranLine.DocLineNo == Convert.ToInt32(Deatail.remark))
                        //            {
                        //                tranLine.RejectQtyTU = Deatail.goods_count;
                        //                tranLine.RtnFillQtyTU = Deatail.goods_count;
                        //                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", item.warehouse_no), new UFSoft.UBF.PL.OqlParam("org", org.ID));
                        //                if (wh != null)
                        //                {
                        //                    tranLine.Wh = wh;
                        //                    tranLine.Wh.ID = wh.ID;
                        //                    tranLine.Wh.Code = wh.Code;
                        //                }

                        //            }
                        //        }
                        //    }
                        //    session.Commit();
                        //}
                        //if (miscRcvTrans.Status == RcvStatusEnum.Approving)
                        //{
                        //    using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                        //    {
                        //        try
                        //        {
                        //            List<long> lstID = new List<long>();
                        //            lstID.Add(miscRcvTrans.ID);
                        //            RcvBatchApprovedBPProxy appProxy = new RcvBatchApprovedBPProxy();
                        //            appProxy.ActType = 8;
                        //            appProxy.DocHeadIDs = lstID;

                        //            List<UFIDA.U9.PM.Rcv.PMDocErrListDTOData> appErrors = appProxy.Do();
                        //            if (appErrors.Count > 0 && !string.IsNullOrEmpty(appErrors[0].ErrMsg))
                        //                // throw new Exception(appErrors[0].ErrMsg);
                        //                CHelper.InsertU9Log(false, "委外退货审核", appErrors[0].ErrMsg, miscRcvTrans.DocNo, "");

                        //            scope.Commit();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            scope.Rollback();
                        //            // throw new Exception(ex.Message);
                        //        }
                        //    }
                        //}


                    }
                }


                if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                {
                    page_no++;
                    GetRcvDoc(page_no, starttime, endtime);
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }
            return rtn;
        }
        /// <summary>
        /// 采购入库单 委外收货
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private RtnDataJson GetPurRcv(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Purchase.queryWithDetail");
                List<StockinPurchaseOrderqueryWithDetailReq> listreq = new List<StockinPurchaseOrderqueryWithDetailReq>();
                StockinPurchaseOrderqueryWithDetailReq req = new StockinPurchaseOrderqueryWithDetailReq();
                req.start_time = starttime;
                req.end_time = endtime;
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
                    CommonSV commonSV = new CommonSV();
                    PurchaseOrder miscRcvTrans = null;
                    foreach (StockinPurchaseOrderDto item in res.data.order)
                    {
                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        Organization org = CHelper.GetOrg(item.details_list[0].goods_no, "");
                        if (org != null)
                        {
                            miscRcvTrans = PurchaseOrder.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.purchase_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            if (miscRcvTrans != null && miscRcvTrans.DocType != null && miscRcvTrans.DocType.Code == "PO22")
                            {
                                dataJson = JsonHelper.GetPurReturn(miscRcvTrans, item, org.Code);
                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                                if (fail == null)
                                {
                                    CHelper.InsertFailInfo("委外收货创建", dataJson, org, item.order_no, 0, "");
                                }

                                // commonSV.CreatePMRcv(dataJson);
                            }
                        }
                    }
                    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                    {
                        page_no++;
                        GetPurRcv(page_no, starttime, endtime);
                    }
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }

            return rtn;
        }
    }
}
