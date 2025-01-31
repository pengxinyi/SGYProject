using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using QimenCloud.Api;
using QimenCloud.Api.scene3ldsmu02o9.Request;
using QimenCloud.Api.scene3ldsmu02o9.Response;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Top.Api;
using Top.Api.Domain;
using UFIDA.U9.Base;
using UFIDA.U9.Base.FlexField.ValueSet;
using UFIDA.U9.Base.Organization;
using UFIDA.U9.CBO.Pub.Controller;
using UFIDA.U9.CBO.SCM.Customer;
using UFIDA.U9.CBO.SCM.Item;
using UFIDA.U9.CBO.SCM.Supplier;
using UFIDA.U9.CBO.SCM.Warehouse;
using UFIDA.U9.InvDoc.MiscRcv;
using UFIDA.U9.InvDoc.MiscShip;
using UFIDA.U9.InvDoc.TransferIn;
using UFIDA.U9.ISV.TransferInISV.Proxy;
using UFIDA.U9.LH.LHPubBE.FailInfoBE;
using UFIDA.U9.LH.LHPubBP.Model;
using UFIDA.U9.LH.LHPubBP.Utility;
using UFIDA.U9.PM.PO;
using UFIDA.U9.PM.Rcv;
using UFIDA.U9.PM.Rcv.Proxy;
using UFIDA.U9.PM.Rtn;
using UFIDA.U9.SM.Ship;
using UFIDA.U9.SM.ShipPlan;
using UFIDA.U9.SM.SO;
using UFSoft.UBF.Business;
using UFSoft.UBF.PL;
using UFSoft.UBF.Transactions;
using UFSoft.UBF.UI.Portal.Util;
using UFSoft.UBF.Util.DataAccess;
using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockoutSalesQuerywithdetailResponse;

namespace UFIDA.U9.LH.LHPubBP.Option
{
    /// <summary>
    /// 推送旺店通
    /// </summary>
    public class WDTSV
    {
        /// <summary>
        /// 货品推送
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTGoods(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            //try
            //{

            //    List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
            //    string sErrorItem = "";
            //    foreach (long ID in IDs)
            //    {
            //        ItemMaster item = ItemMaster.Finder.FindByID(ID);
            //        //if (dv != null && dv.Description == "推")
            //        //{
            //        string dataJson = JsonHelper.GetSendDataJson(item);
            //        Dictionary<string, string> list = WDTChelper.GetWDTInfo("goods.Goods.push");
            //        list.Add("body", dataJson);
            //        string sign = WDTChelper.GetWDTSign(list);
            //        list.Add("sign", sign);
            //        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //        string apireuslt = Tools.HttpPost(url, dataJson);
            //        WDTItemRes res = JsonConvert.DeserializeObject<WDTItemRes>(apireuslt);
            //        if (res.status != 0)
            //        {
            //            CHelper.InsertU9Log(false, "货品推送", res.message, dataJson, url, item.Code);
            //            sErrorItem += item.Code + res.message + ",";
            //        }
            //        else
            //        {
            //            CHelper.InsertU9Log(true, "货品推送", res.message, dataJson, url, item.Code);
            //        }
            //        if (!string.IsNullOrEmpty(sErrorItem))
            //        {
            //            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
            //            rtn.IsSuccess = false;
            //            rtn.Msg = string.Format("料号【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
            //        }
            //        else
            //            rtn.IsSuccess = true;



            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //    rtn.Msg = ex.Message;
            //    rtn.IsSuccess = false;
            //}
            //GetMiscShipRcv(0);
            //GetMiscShips(0);
            //GetQMSO(1);
             //GetQMShip(1);
            // InsertQMShip(1);
             //GetPORtnRcv(0);
            // GetRcvTrans(1);
            //GetRcv(0);
            //GetPODocNo("PO201012310010003");
            //GetOCRcv(0);
             int page_no = 0;

            #region 合并生单SO
            //try
            //{
            //    DataSet ds = new DataSet();
            //    DataSet dsq = new DataSet();
            //    List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
            //    string dataJson = string.Empty;
            //    string sql = string.Empty;
            //    string errormsg = string.Empty;
            //    //select distinct CusCode from sgy_saleship Where convert(varchar(10),ShipDate,120) = Cast(getdate() as Date)
            //    sql = string.Format("select distinct CusCode,Org from sgy_saleship Where Org=1002307200000031");
            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
            //    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
            //    {
            //        string posturl = string.Empty;
            //        string apirEEEeuslt = string.Empty;
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            sql = string.Format(" select CusCode,WHCode,ItemCode,sum(Amount)数量,SellPrice,TradeFrom,Org from sgy_saleship where CusCode ='{0}' and Org= {1}  group by Org, CusCode,WHCode,ItemCode,SellPrice ,TradeFrom", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]));
            //            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out dsq);
            //            if ((dsq != null) && (dsq.Tables.Count > 0) && (dsq.Tables.Count >= 1 && dsq.Tables[0].Rows.Count > 0))
            //            {

            //                Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
            //                dataJson = JsonHelper.MergeSOJson(dsq, org, row["CusCode"].ToString());                         
            //                posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            //                apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
            //                JavaScriptSerializer serializer = new JavaScriptSerializer();
            //                RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            //                if (rtns.IsSuccess)
            //                {
            //                    sql = string.Format(@"update sgy_saleship set  Status=1  where CusCode='{0}' and Org={1}", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]));
            //                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                }
            //                else
            //                {
            //                    CHelper.InsertU9Log(false, "合并销售明细生成销售订单", rtns.Msg, dataJson, posturl, "");
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    rtn.Msg = ex.Message;
            //    rtn.IsSuccess = false;
            //}
            #endregion

            #region 采购退货更新
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.PurchaseReturn.queryWithDetail");
            //    List<PurchaseReturnqueryWithDetailReq> listreq = new List<PurchaseReturnqueryWithDetailReq>();
            //    PurchaseReturnqueryWithDetailReq req = new PurchaseReturnqueryWithDetailReq();
            //    req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //    req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = "110";
            //    req.time_type = 2;
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
            //    PurchaseReturnqueryWithDetailRes res = JsonConvert.DeserializeObject<PurchaseReturnqueryWithDetailRes>(apireuslt);
            //    if (res.status != 0)
            //    {
            //        CHelper.InsertU9Log(false, "采购退货出库单查询", apireuslt, json, url);
            //    }
            //    else if (res.data != null && res.data.order != null)
            //    {
            //        Receivement miscRcvTrans = null;
            //        string dataJson = string.Empty;
            //        foreach (var item in res.data.order)
            //        {
            //            miscRcvTrans = Receivement.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_order_no));
            //            if (miscRcvTrans != null)
            //            {
            //                Organization org = CHelper.GetOrg(miscRcvTrans.RcvLines[0].ItemInfo.ItemCode, "");
            //                if (org != null)
            //                {
            //                    if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "RCV95" && miscRcvTrans.Status == RcvStatusEnum.Approving)
            //                    {
            //                        dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
            //                        CHelper.InsertFailInfo("采购退货更新", dataJson, org, miscRcvTrans.DocNo, 0);
            //                    }
            //                }
            //            }

            //            //using (ISession session = Session.Open())
            //            //{
            //            //    foreach (RcvLine tranLine in miscRcvTrans.RcvLines)
            //            //    {
            //            //        foreach (PurchaseReturnOrderDeatail Deatail in item.details_list)
            //            //        {
            //            //            if (tranLine.DocLineNo == Convert.ToInt32(Deatail.remark))
            //            //            {
            //            //                tranLine.RejectQtyTU = Deatail.goods_count;
            //            //                tranLine.RtnFillQtyTU = Deatail.goods_count;
            //            //                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", item.warehouse_no), new UFSoft.UBF.PL.OqlParam("org", org.ID));
            //            //                if (wh != null)
            //            //                {
            //            //                    tranLine.Wh = wh;
            //            //                    tranLine.Wh.ID = wh.ID;
            //            //                    tranLine.Wh.Code = wh.Code;
            //            //                }

            //            //            }
            //            //        }
            //            //    }
            //            //    session.Commit();
            //            //}
            //            //if (miscRcvTrans.Status == RcvStatusEnum.Approving)
            //            //{
            //            //    using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
            //            //    {
            //            //        try
            //            //        {
            //            //            List<long> lstID = new List<long>();
            //            //            lstID.Add(miscRcvTrans.ID);
            //            //            RcvBatchApprovedBPProxy appProxy = new RcvBatchApprovedBPProxy();
            //            //            appProxy.ActType = 8;
            //            //            appProxy.DocHeadIDs = lstID;

            //            //            List<UFIDA.U9.PM.Rcv.PMDocErrListDTOData> appErrors = appProxy.Do();
            //            //            if (appErrors.Count > 0 && !string.IsNullOrEmpty(appErrors[0].ErrMsg))
            //            //                // throw new Exception(appErrors[0].ErrMsg);
            //            //                CHelper.InsertU9Log(false, "委外退货审核", appErrors[0].ErrMsg, miscRcvTrans.DocNo, "");

            //            //            scope.Commit();
            //            //        }
            //            //        catch (Exception ex)
            //            //        {
            //            //            scope.Rollback();
            //            //            // throw new Exception(ex.Message);
            //            //        }
            //            //    }
            //            //}


            //        }
            //    }


            //    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
            //    {
            //        page_no++;
            //        GetOCRtnRcv(page_no);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            //GetOCRtnRcv(0);
            #endregion
            #region 来源销售订单标准出货创建
            //string dataJson = string.Empty;
            //string posturl = string.Empty;
            //try
            //{
            //    string sql = string.Empty;
            //    DataSet ds = new DataSet();
            //    sql = string.Format("Select top 100  ID,DocType,Json,SrcDocNo,DocNo,Org  from Cust_FailInfo where issuccess=0 and  DocType='标准出货创建'");
            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
            //    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
            //    {
            //        string apirEEEeuslt = string.Empty;
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            if (!string.IsNullOrEmpty(Convert.ToString(row["SrcDocNo"])))
            //            {
            //                SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", row["SrcDocNo"].ToString()), new UFSoft.UBF.PL.OqlParam("Org", Convert.ToInt64(row["Org"])));
            //                if (so != null)
            //                {
            //                    Ship ship = Ship.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", row["DocNo"].ToString()), new UFSoft.UBF.PL.OqlParam("Org", Convert.ToInt64(row["Org"])));
            //                    if (ship != null)
            //                    {
            //                        continue;
            //                    }
            //                    Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
            //                    MiscSalesOrderDto orderdto = JsonConvert.DeserializeObject<MiscSalesOrderDto>(row["Json"].ToString());
            //                    dataJson = JsonHelper.GetWDTShipFromSO(so, row["DocNo"].ToString(), org?.Code, orderdto);
            //                    posturl = CHelper.GetDefineValueUrl("CustParam", "01"); //CustParam
            //                    apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
            //                    JavaScriptSerializer serializer = new JavaScriptSerializer();
            //                    RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            //                    if (rtns.IsSuccess)
            //                    {
            //                        sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
            //                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                    }
            //                    else
            //                    {
            //                        sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), rtns.Msg.Substring(0, 100));
            //                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            //                dataJson = row["Json"].ToString();
            //                apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
            //                JavaScriptSerializer serializer = new JavaScriptSerializer();
            //                RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            //                if (rtns.IsSuccess)
            //                {
            //                    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
            //                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                }
            //                else
            //                {
            //                    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), rtns.Msg.Substring(0, 100));
            //                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                }
            //            }
            //        }

            //    }


            //}
            //catch (Exception ex)
            //{
            //    CHelper.InsertU9Log(false, "标准出货创建", ex.Message, dataJson, posturl);
            //    throw new Exception(ex.Message);
            //}
            #endregion

            #region 盘盈入库 正残转换 生成杂收
            //正残转换 QR202309070001
            //盘盈入库 QR202309070002
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
            //    List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
            //    StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
            //    req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //    req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = "70";
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
            //    StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
            //    if (res.status != 0)
            //    {
            //        CHelper.InsertU9Log(false, "其它入库单查询", apireuslt, json, url);
            //    }
            //    string dataJson = string.Empty;
            //    string posturl = string.Empty;
            //    string apirEEEeuslt = string.Empty;
            //    if (res.data != null && res.data.order != null)
            //    {
            //        foreach (var item in res.data.order)
            //        {
            //            //盘盈入库生成杂收
            //            if (item.reason == "盘盈入库")
            //            {
            //                Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //                if (org != null)
            //                {
            //                    dataJson = JsonHelper.GetWdtStockInProfitMiscRcvTrans(item, org);
            //                    //commonSV.CreateMiscRcvTrans(dataJson);
            //                    CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0);
            //                }
            //            }
            //            //正残转换
            //            if (item.remark.Contains("正残转换"))
            //            {
            //                Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //                if (org != null)
            //                {
            //                    dataJson = JsonHelper.GetWdtShiftMiscRcvTrans(item, org);
            //                    //commonSV.CreateMiscRcvTrans(dataJson);
            //                    CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0);
            //                }
            //            }
            //        }
            //    }

            //    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
            //    {
            //        page_no++;
            //        GetMiscRcv(page_no);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            #endregion

            #region 盘盈出库 正残转换 生成杂发
            //盘盈出库 QC202309070002
            //正残转换 QC202309070001
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
            //    List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
            //    StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
            //    req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //    req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = "70";
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
            //    StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
            //    if (res.status != 0)
            //    {
            //        CHelper.InsertU9Log(false, "其他出库单查询", apireuslt, json, url);
            //    }
            //    string dataJson = string.Empty;
            //    string posturl = string.Empty;
            //    string apirEEEeuslt = string.Empty;
            //    CommonSV commonSV = new CommonSV();
            //    if (res.data != null && res.data.order != null)
            //    {
            //        foreach (var item in res.data.order)
            //        {
            //            //盘亏出库生成杂发
            //            if (item.reason == "盘亏出库")
            //            {
            //                Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //                if (org != null)
            //                {
            //                    dataJson = JsonHelper.GetWdtStockOutLossMiscShip(item, org);
            //                    // commonSV.CreateMiscShip(dataJson);
            //                    CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0);
            //                }
            //            }
            //            //正残转换
            //            if (item.remark.Contains("正残转换"))
            //            {
            //                Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //                if (org != null)
            //                {
            //                    dataJson = JsonHelper.GetWdtShiftMiscShip(item, org);
            //                    // commonSV.CreateMiscShip(dataJson);
            //                    CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0);
            //                }
            //            }
            //        }
            //    }

            //    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
            //    {
            //        page_no++;
            //        GetMiscShip(page_no);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //} 
            #endregion

            #region 外仓调整入库
            //外仓调整入库
            //WI202309070001
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
            //    List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
            //    OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
            //    req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //    req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = "50";
            //    req.src_order_type = 0;
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
            //    OuterInquerywithdetailRes res = JsonConvert.DeserializeObject<OuterInquerywithdetailRes>(apireuslt);
            //    if (res.status != 0)
            //    {
            //        CHelper.InsertU9Log(false, "外仓调整入库单查询", apireuslt, json, url);
            //    }
            //    string dataJson = string.Empty;
            //    string posturl = string.Empty;
            //    string apirEEEeuslt = string.Empty;
            //    CommonSV commonSV = new CommonSV();
            //    if (res.data != null && res.data.order != null)
            //    {
            //        foreach (var item in res.data.order)
            //        {
            //            //盘盈入库生成杂收
            //            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //            if (org != null)
            //            {
            //                dataJson = JsonHelper.GetWdtOuterInProfitMiscRcvTrans(item, org);
            //                //commonSV.CreateMiscRcvTrans(dataJson);
            //                CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.outer_in_no, 0);
            //            }

            //        }
            //    }

            //    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
            //    {
            //        page_no++;
            //        GetMiscRcv(page_no);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            #endregion

            #region 外仓调整出库
            //外仓调整出库
            //WO202309070001
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
            //    List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
            //    OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
            //    req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //    req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    req.status = "50";
            //    req.order_type = 0;
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
            //    OuterOutquerywithdetailRes res = JsonConvert.DeserializeObject<OuterOutquerywithdetailRes>(apireuslt);
            //    if (res.status != 0)
            //    {
            //        CHelper.InsertU9Log(false, "外仓调整出库单查询", apireuslt, json, url);
            //    }
            //    string dataJson = string.Empty;
            //    string posturl = string.Empty;
            //    string apirEEEeuslt = string.Empty;
            //    CommonSV commonSV = new CommonSV();
            //    if (res.data != null && res.data.order != null)
            //    {
            //        foreach (var item in res.data.order)
            //        {
            //            //盘亏出库生成杂发
            //            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //            if (org != null)
            //            {
            //                dataJson = JsonHelper.GetWdtOuterOutLossMiscShip(item, org);
            //                // commonSV.CreateMiscShip(dataJson);
            //                CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.outer_out_no, 0);
            //            }
            //        }
            //    }

            //    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
            //    {
            //        page_no++;
            //        GetMiscShip(page_no);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //} 
            #endregion
            return rtn;
        }
        private void InsertQMShip(int page_no)
        {
            #region 写入中间表
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
            paramsDomain.StartTime = "2023-09-09 14:38:00";
            paramsDomain.EndTime = "2023-09-09 14:44:00";
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
                if (response.Status == 0 && response.Data != null && response.Data.Order != null)
                {
                    //CommonSV commonSV = new CommonSV();
                    UFIDA.U9.SM.ShipPlan.ShipPlan shipPlan = null;
                    Organization org = null;
                    //CHelper.InsertU9Log(true, "奇门销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                    foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                    {
                     
                        if (org != null)
                        {
                            foreach (DetailsListDomain detailsListDomain in item.DetailsList)
                            {
                                  org = CHelper.GetOrg(detailsListDomain.GoodsNo, "");
                                if (org!=null)
                                {
                                    CHelper.InsertSaleShip(item, detailsListDomain, org); //将明细数据写入中间表
                                }
                               
                            }
                        }
                    }
                    if (response.Data.Order.Count == 100)
                    {
                        page_no++;
                        InsertQMShip(page_no);
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
        /// 审核料品推送
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public RtnDataJson ApproveSendWDTGoods(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    ItemMaster item = ItemMaster.Finder.FindByID(ID);
                    DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1010"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg1));
                    if (dv != null && dv.Description == "推")
                    {
                        string dataJson = JsonHelper.GetSendDataJson(item);
                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("goods.Goods.push");
                        list.Add("body", dataJson);
                        string sign = WDTChelper.GetWDTSign(list);
                        list.Add("sign", sign);
                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                        string apireuslt = Tools.HttpPost(url, dataJson);
                        WDTItemRes res = JsonConvert.DeserializeObject<WDTItemRes>(apireuslt);
                        if (res.status != 0)
                        {
                            CHelper.InsertU9Log(false, "货品推送", res.message, dataJson, url, item.Code);
                            sErrorItem += item.Code + res.message + ",";
                        }
                        else
                        {
                            CHelper.InsertU9Log(true, "货品推送", res.message, dataJson, url, item.Code);
                        }
                        if (!string.IsNullOrEmpty(sErrorItem))
                        {
                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                            rtn.IsSuccess = false;
                            rtn.Msg = string.Format("料号【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                        }
                        else
                            rtn.IsSuccess = true;
                    }


                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 奇门销售订单出库查询
        /// </summary>
        /// <param name="page_no"></param>
        private void GetQMShip(int page_no)
        {
            #region 奇门自定义接口 销售出库查询

            WdtWmsStockoutSalesQuerywithdetailRequest request = new WdtWmsStockoutSalesQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain();
            pager.PageNo = 1L;
            pager.PageSize = 100L;
            request.Pager_ = pager;
            WdtWmsStockoutSalesQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockoutSalesQuerywithdetailRequest.
                ParamsDomain();
            //paramsDomain.StartTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.StartTime = "2023-09-09 14:42:41";
            //paramsDomain.EndTime = "2023-09-09 14:43:41";
            paramsDomain.Status = "110";
            paramsDomain.StockoutNo = "CK2023091428311";
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
       
        private void GetMiscRcvv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
                List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
                OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
                req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "50";
                req.src_order_type = 0;
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
                //CommonSV commonSV = new CommonSV();
                if (res.data != null && res.data.order != null)
                {
                    foreach (var item in res.data.order)
                    {
                        //调整入库生成杂收     
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
                    GetMiscRcvv(page_no);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GetMiscShipp(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
                List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
                OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
                req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "50";
                req.order_type = 0;
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
               // CommonSV commonSV = new CommonSV();
                if (res.data != null && res.data.order != null)
                {
                    foreach (var item in res.data.order)
                    {
                        //调整出库生成杂发
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
                    GetMiscShipp(page_no);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 销售退货入库单  退回处理
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetWDTRMA(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Refund.queryWithDetail");
                List<StockinRefundqueryWithDetailReq> listreq = new List<StockinRefundqueryWithDetailReq>();
                StockinRefundqueryWithDetailReq req = new StockinRefundqueryWithDetailReq();
                req.start_time = "2023-08-22 13:20:05";
                req.end_time = "2023-08-22 15:20:05";
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
                                CHelper.InsertFailInfo("无来源标准出货创建", dataJson, org, item.order_no, 0);//无来源出货单创建
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
                            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, ship.DocNo, 0);//退回处理单创建
                                                                                            // commonSV.CreateSrcShipRMA(dataJson);
                        }



                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetWDTRMA(page_no);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 奇门退货入库单 退回处理
        /// </summary>
        /// <param name="page_no"></param>
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
            paramsDomain.StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                        if (!string.IsNullOrEmpty(item.AttachType))
                        {
                            //生成无来源的出货单SM5

                            //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                            if (!string.IsNullOrEmpty(item.TidList))
                            {
                                Organization org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                                Ship ship = null;
                                if (org != null)
                                {
                                    Customer customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", item.ShopNo), new OqlParam("Org", org.ID));
                                    if (customer == null)
                                    {
                                        DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                                        if (dv == null)
                                        {
                                            continue;
                                        }
                                    }
                                    dataJson = JsonHelper.GetQMShip(item, org.Code);
                                    CHelper.InsertFailInfo("无来源标准出货创建", dataJson, org, "", 0); //无来源出货单创建
                                    SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    ShipLine shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (shipLine != null)
                                    {
                                        ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                        if (ship != null)
                                        {
                                            dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code);
                                            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建

                                        }
                                    }

                                }

                               

                            }
                        }
                        else if (string.IsNullOrEmpty(item.AttachType))
                        {
                            //通过原始订单号匹配U9出货单，生成来源于出货单的退回处理单
                            if (!string.IsNullOrEmpty(item.TidList))
                            {
                                Organization org = CHelper.GetOrg(item.DetailsList[0].GoodsNo, "");
                                Ship ship = null;
                                if (org != null)
                                {
                                    SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.TidList), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    ShipLine shipLine = ShipLine.Finder.Find("SrcDocNo=@SrcDocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("SrcDocNo", so?.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (shipLine != null)
                                    {
                                        ship = Ship.Finder.FindByID(shipLine.Ship.ID);
                                        if (ship != null)
                                        {
                                            dataJson = JsonHelper.GetSrcShipRMAQM(item, ship, org.Code);
                                            CHelper.InsertFailInfo("退回处理单创建", dataJson, org, item.OrderNo, 0);//来源出货单退回处理创建

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
                else
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
        /// <summary>
        /// 销售出库创建销售订单
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetWDTSO(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.TradeQuery.queryWithDetail");
                List<salesTradeQueryWithDetailReq> listreq = new List<salesTradeQueryWithDetailReq>();
                salesTradeQueryWithDetailReq req = new salesTradeQueryWithDetailReq();
                req.start_time = "2023-08-25 16:21:05";
                req.end_time = "2023-08-25 20:21:05";
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
                        if (item.trade_from == 2 || item.trade_from ==3 || item.trade_from == 4 || item.trade_from == 6)
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
        /// <summary>
        /// 创建销售订单
        /// </summary>
        /// <param name="page_no"></param>
        private void GetQMSO(int page_no)
        {

            #region 奇门自定义接口 订单查询

            WdtSalesTradequeryQuerywithdetailRequest request = new WdtSalesTradequeryQuerywithdetailRequest();
            request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            WdtSalesTradequeryQuerywithdetailRequest.PagerDomain pager = new WdtSalesTradequeryQuerywithdetailRequest.PagerDomain();
            pager.PageNo = 1L;
            pager.PageSize = 100L;
            request.Pager_ = pager;
            WdtSalesTradequeryQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtSalesTradequeryQuerywithdetailRequest. ParamsDomain();
            //paramsDomain.StartTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //paramsDomain.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            paramsDomain.TradeNo = "JY202308310033";
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
               // WdtSalesTradequeryQuerywithdetailResponse response = client.Execute(request);
                var  response = client.Execute(request);
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
                                CHelper.InsertFailInfo("销售订单创建", dataJson, org, item.SrcTids, 0);
                                // commonSV.CreateQMSO(dataJson);
                            }
                        }


                    }
                    if (response.Data.Order.Count == 100)
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
                CHelper.InsertU9Log(false, "奇门销售订单查询", e.ErrorMsg, JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
            }
            #endregion


        }
        /// <summary>
        /// 奇门创建杂收
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetRcvTrans(int page_no)
        {
            try
            {
                WdtWmsStockinPrestockinSearchRequest request = new WdtWmsStockinPrestockinSearchRequest();
                request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                WdtWmsStockinPrestockinSearchRequest.PagerDomain pager = new WdtWmsStockinPrestockinSearchRequest.PagerDomain();
                pager.PageNo = page_no;
                pager.PageSize = 10;
                request.Pager_ = pager;
                WdtWmsStockinPrestockinSearchRequest.ParamsDomain paramsDomain = new WdtWmsStockinPrestockinSearchRequest.
                    ParamsDomain();
                paramsDomain.CtFrom = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
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
                    if (response.Status == 0)
                    {
                        // CHelper.InsertU9Log(true, "奇门预入库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                        string dataJson = string.Empty;
                        if (response.Data != null && response.Data.Order != null && response.Data.Order.Count > 0)
                        {
                            foreach (WdtWmsStockinPrestockinSearchResponse.OrderDomain item in response.Data.Order)
                            {
                                //if (item.FlagName == "预入库杂收单")
                                //{
                                    Organization org = CHelper.GetOrg(item.DetailList[0].GoodsNo, "");
                                    if (org != null)
                                    {
                                        dataJson = JsonHelper.GetQMMiscRcvTrans(item, org);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson,org, item.SrcOrderNo, 0);
                                        // commonSV.CreateMiscRcvTrans(dataJson);
                                    }
                                //}

                            }
                        }
                    
                        if (response.Data.Order.Count == 100)
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 预入库创建杂收
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetWDTRcvTrans(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.PreStockin.search");
                List<StockinPreStockinqueryWithDetailReq> listreq = new List<StockinPreStockinqueryWithDetailReq>();
                StockinPreStockinqueryWithDetailReq req = new StockinPreStockinqueryWithDetailReq();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
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
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                dataJson = JsonHelper.GetWdtMiscRcvTrans(item, org);
                                CHelper.InsertFailInfo("杂收单创建", dataJson,org, item.src_order_no, 0);
                                //commonSV.CreateMiscRcvTrans(dataJson);
                            }

                        }


                    }
                    if (res.data.order.Count == 100)
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
        /// <summary>
        /// 盘亏 杂发创建
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetMiscShips(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                req.start_time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "70";
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
                StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "其他出库单查询", apireuslt, json, url);
                }
                string dataJson = string.Empty;
                string posturl = string.Empty;
                string apirEEEeuslt = string.Empty;
                CommonSV commonSV = new CommonSV();
                if (res.data != null && res.data.order != null)
                {
                    foreach (var item in res.data.order)
                    {
                        //盘亏出库生成杂发
                        if (item.reason == "盘亏出库")
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                dataJson = JsonHelper.GetWdtStockOutLossMiscShip(item, org);
                                // commonSV.CreateMiscShip(dataJson);
                                CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0);
                            }
                        }
                        //正残转换
                        if (item.remark.Contains("正残转换"))
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                dataJson = JsonHelper.GetWdtShiftMiscShip(item, org);
                                // commonSV.CreateMiscShip(dataJson);
                                CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0);
                            }
                        }
                    }
                }

                if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                {
                    page_no++;
                    GetMiscShips(page_no);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 盘盈杂收创建
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetMiscShipRcv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
                StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
                req.start_time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "70";
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
                StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "其它入库单查询", apireuslt, json, url);
                }
                string dataJson = string.Empty;
                string posturl = string.Empty;
                string apirEEEeuslt = string.Empty;
                CommonSV commonSV = new CommonSV();
                if (res.data != null && res.data.order != null)
                {
                    foreach (var item in res.data.order)
                    {
                        //盘盈入库生成杂收
                        if (item.reason == "盘盈入库")
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                dataJson = JsonHelper.GetWdtStockInProfitMiscRcvTrans(item, org);
                                //commonSV.CreateMiscRcvTrans(dataJson);
                                CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0);
                            }
                        }
                        //正残转换
                        if (item.remark.Contains("正残转换"))
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                dataJson = JsonHelper.GetWdtShiftMiscRcvTrans(item, org);
                                //commonSV.CreateMiscRcvTrans(dataJson);
                                CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0);
                            }
                        }
                    }
                }

                if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                {
                    page_no++;
                    GetMiscShipRcv(page_no);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 旺店通创建来源出货计划出货
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetWDTShip(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Sales.queryWithDetail");
                List<StockOutQueryWithDetailReq> listreq = new List<StockOutQueryWithDetailReq>();
                StockOutQueryWithDetailReq req = new StockOutQueryWithDetailReq();
                //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.start_time = "2023-08-29 14:00:05";
                req.end_time = "2023-08-29 14:20:05";
                req.status_type = 0;
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
                        if (item.trade_from == 2 || item.trade_from == 3 || item.trade_from ==4 || item.trade_from ==6)
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
                                    dataJson = JsonHelper.GetShipPlanWDT(shipPlan, item, org.Code, "SM4"); //待确认
                                    CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.order_no, 0); //来源出货计划出货单创建
                                    //commonSV.CreateSrcShipFromShipPlan(dataJson);
                                }
                                else
                                {
                                    //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                                    //SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                    if (!string.IsNullOrEmpty(item.src_trade_no))
                                    {
                                        //dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                                        dataJson = JsonConvert.SerializeObject(item);
                                        CHelper.InsertFailInfo("标准出货创建", dataJson, org, item.src_order_no, 0, "", item.src_trade_no); //来源销售订单出货创建
                                        //commonSV.CreateSrcShipFromSO(dataJson);
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
                        //            CHelper.InsertFailInfo("来源出货计划出货单创建", dataJson, org,item.order_no, 0);
                        //            //commonSV.CreateSrcShipFromShipPlan(dataJson);
                        //        }
                        //        else
                        //        {
                        //            //未匹配到出货计划在匹配销售订单，匹配到在创建来源so出货
                        //            SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.src_trade_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                        //            if (so != null)
                        //            {
                        //                dataJson = JsonHelper.GetWDTShipFromSO(so, item.order_no, org.Code, item);
                        //                CHelper.InsertFailInfo("来源销售订单出货创建", dataJson, org,item.src_order_no, 0);
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
        /// <summary>
        /// 调入单更新
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GetTransferIn()
        {
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format(" Select top 50 DocNo,ID from InvDoc_TransferIn where Status=1");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TransferIn transfer = TransferIn.Finder.FindByID(Convert.ToInt64(row["ID"]));
                        if (transfer != null && transfer.TransInLines[0].TransInOrg != transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
                        {
                            if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                                    List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
                                    StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.other_in_no = transfer.DocNo;
                                    req.status = "70";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "1000");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                string dataJson = string.Empty;
                                                foreach (Model.StockInOrderDto order in res.data.order)
                                                {
                                                    if (order.other_in_no == transfer.DocNo)
                                                    {
                                                        // dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (StockInOrderDeatail detail in order.detail_list)
                                                            {
                                                                if (tranLine.ItemInfo.ItemCode == detail.goods_no)
                                                                {
                                                                    tranLine.StoreUOMQty = detail.in_num;
                                                                    tranLine.DescFlexSegments.PrivateDescSeg10 = "已更新";
                                                                    //  tranLine.TransInWh.Code = order.wa
                                                                }
                                                            }

                                                        }
                                                        //foreach (var item in order.detail_list)
                                                        //{
                                                        //foreach (TransInLine tranLine in transfer.TransInLines)
                                                        //{
                                                        //    if (tranLine.DocLineNo == Convert.ToInt32(item.remark) && tranLine.ItemInfo.ItemCode == item.goods_no)
                                                        //    {
                                                        //        tranLine.CostUOMQty = item.goo;
                                                        //    }
                                                        //}
                                                        //}
                                                    }
                                                    //if (tranLine.ItemInfo.ItemCode == Deatail.goods_no)
                                                    //{
                                                    //    tranLine.CostUOMQty = Deatail.goods_count;
                                                    //}
                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "调拨入库单查询", apireuslt, json, url);
                                    }
                                }

                            }
                            if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                                    List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                                    StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.other_out_no = transfer.DocNo;
                                    req.status = "70";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "1000");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                foreach (Model.StockOutOrderDto order in res.data.order)
                                                {
                                                    if (order.other_out_no == transfer.DocNo)
                                                    {
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
                                                            {
                                                                if (tranLine.ID == lnSubLine.TransInLine.ID)
                                                                {
                                                                    foreach (StockOutOrderDeatail detail in order.detail_list)
                                                                    {
                                                                        if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                                        {
                                                                            lnSubLine.StoreUOMQty = detail.out_num;
                                                                            lnSubLine.DescFlexSegments.PrivateDescSeg10 = "已更新";
                                                                            //  tranLine.TransInWh.Code = order.wa
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                            // tranLine.StoreUOMQty = order.goods_count;
                                                        }
                                                    }
                                                }

                                                session.Commit();
                                            }
                                        }


                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "调拨出库单查询", apireuslt, json, url);
                                    }
                                }

                            }
                        }
                        else if (transfer != null && transfer.TransInLines[0].TransInOrg == transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
                        {
                            if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                                    List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
                                    StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.other_in_no = transfer.DocNo;
                                    req.status = "70";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "1000");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                foreach (Model.StockInOrderDto order in res.data.order)
                                                {
                                                    if (order.other_in_no == transfer.DocNo)
                                                    {
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (StockInOrderDeatail detail in order.detail_list)
                                                            {
                                                                if (tranLine.ItemInfo.ItemCode == detail.goods_no)
                                                                {
                                                                    tranLine.StoreUOMQty = detail.in_num;
                                                                    tranLine.DescFlexSegments.PrivateDescSeg10 = "已更新";
                                                                    //  tranLine.TransInWh.Code = order.wa
                                                                }
                                                            }

                                                        }
                                                        //foreach (var item in order.detail_list)
                                                        //{
                                                        //foreach (TransInLine tranLine in transfer.TransInLines)
                                                        //{
                                                        //    if (tranLine.DocLineNo == Convert.ToInt32(item.remark) && tranLine.ItemInfo.ItemCode == item.goods_no)
                                                        //    {
                                                        //        tranLine.CostUOMQty = item.goo;
                                                        //    }
                                                        //}
                                                        //}
                                                    }
                                                    //if (tranLine.ItemInfo.ItemCode == Deatail.goods_no)
                                                    //{
                                                    //    tranLine.CostUOMQty = Deatail.goods_count;
                                                    //}
                                                }

                                                session.Commit();
                                            }
                                        }


                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "调拨入库单查询", apireuslt, json, url);
                                    }
                                }

                            }
                            if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                                    List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                                    StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.other_out_no = transfer.DocNo;
                                    req.status = "70";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "1000");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                foreach (StockOutOrderDto order in res.data.order)
                                                {
                                                    if (order.other_out_no == transfer.DocNo)
                                                    {
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
                                                            {
                                                                if (tranLine.ID == lnSubLine.TransInLine.ID)
                                                                {
                                                                    foreach (StockOutOrderDeatail detail in order.detail_list)
                                                                    {
                                                                        if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                                        {
                                                                            lnSubLine.StoreUOMQty = detail.out_num;
                                                                            lnSubLine.DescFlexSegments.PrivateDescSeg10 = "已更新";
                                                                            //  tranLine.TransInWh.Code = order.wa
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                            // tranLine.StoreUOMQty = order.goods_count;
                                                        }
                                                    }
                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "调拨出库单查询", apireuslt, json, url);
                                    }
                                }

                            }
                        }
                        string DataJson = JsonHelper.GetWDTUpdateTransferIn(transfer);
                        if (!string.IsNullOrEmpty(DataJson))
                        {
                            CHelper.InsertFailInfo("调入单更新", DataJson, transfer.Org, transfer.DocNo, 0);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //try
            //{
            //    string sql = string.Empty;
            //    DataSet ds = new DataSet();
            //    sql = string.Format(" Select top 50 DocNo,ID from InvDoc_TransferIn where Status=1");
            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
            //    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
            //    {
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            TransferIn transfer = TransferIn.Finder.FindByID(Convert.ToInt64(row["ID"]));
            //            if (transfer != null && transfer.TransInLines[0].TransInOrg != transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
            //            {
            //                if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
            //                {
            //                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
            //                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                    {
            //                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
            //                        List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
            //                        StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
            //                        //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //                        //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                        req.other_in_no = transfer.DocNo;
            //                        req.status = "70";
            //                        listreq.Add(req);
            //                        string json = JsonConvert.SerializeObject(listreq);
            //                        list.Add("body", json);
            //                        //分页数据
            //                        list.Add("page_size", "100");
            //                        list.Add("page_no", "0");
            //                        list.Add("calc_total", "1");
            //                        string sign = WDTChelper.GetWDTSign(list);
            //                        list.Add("sign", sign);
            //                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                        string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
            //                        StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
            //                        if (res.status == 0)
            //                        {
            //                            if (res.data != null && res.data.order.Count > 0)
            //                            {
            //                                using (ISession session = Session.Open())
            //                                {
            //                                    //foreach (TransInLine tranLine in transfer.TransInLines)
            //                                    //{
            //                                    string dataJson = string.Empty;
            //                                    foreach (Model.StockInOrderDto order in res.data.order)
            //                                    {
            //                                        if (order.other_in_no == transfer.DocNo)
            //                                        {
            //                                            // dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
            //                                            foreach (TransInLine tranLine in transfer.TransInLines)
            //                                            {
            //                                                foreach (StockInOrderDeatail detail in order.detail_list)
            //                                                {
            //                                                    if (tranLine.ItemInfo.ItemCode == detail.goods_no)
            //                                                    {
            //                                                        tranLine.StoreUOMQty = detail.in_num;
            //                                                        //  tranLine.TransInWh.Code = order.wa
            //                                                    }
            //                                                }

            //                                            }
            //                                            //foreach (var item in order.detail_list)
            //                                            //{
            //                                            //foreach (TransInLine tranLine in transfer.TransInLines)
            //                                            //{
            //                                            //    if (tranLine.DocLineNo == Convert.ToInt32(item.remark) && tranLine.ItemInfo.ItemCode == item.goods_no)
            //                                            //    {
            //                                            //        tranLine.CostUOMQty = item.goo;
            //                                            //    }
            //                                            //}
            //                                            //}
            //                                        }
            //                                        //if (tranLine.ItemInfo.ItemCode == Deatail.goods_no)
            //                                        //{
            //                                        //    tranLine.CostUOMQty = Deatail.goods_count;
            //                                        //}
            //                                    }

            //                                    session.Commit();
            //                                }
            //                            }

            //                        }
            //                        else
            //                        {
            //                            CHelper.InsertU9Log(false, "调拨入库单查询", apireuslt, json, url);
            //                        }
            //                    }

            //                }
            //                if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null)
            //                {
            //                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
            //                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                    {
            //                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
            //                        List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
            //                        StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
            //                        //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //                        //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                        req.other_out_no = transfer.DocNo;
            //                        req.status = "70";
            //                        listreq.Add(req);
            //                        string json = JsonConvert.SerializeObject(listreq);
            //                        list.Add("body", json);
            //                        //分页数据
            //                        list.Add("page_size", "100");
            //                        list.Add("page_no", "0");
            //                        list.Add("calc_total", "1");
            //                        string sign = WDTChelper.GetWDTSign(list);
            //                        list.Add("sign", sign);
            //                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                        string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
            //                        StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
            //                        if (res.status == 0)
            //                        {
            //                            if (res.data != null && res.data.order.Count > 0)
            //                            {
            //                                using (ISession session = Session.Open())
            //                                {
            //                                    //foreach (TransInLine tranLine in transfer.TransInLines)
            //                                    //{
            //                                    foreach (Model.StockOutOrderDto order in res.data.order)
            //                                    {
            //                                        if (order.other_out_no == transfer.DocNo)
            //                                        {
            //                                            foreach (TransInLine tranLine in transfer.TransInLines)
            //                                            {
            //                                                foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
            //                                                {
            //                                                    if (tranLine.ID == lnSubLine.TransInLine.ID)
            //                                                    {
            //                                                        foreach (StockOutOrderDeatail detail in order.detail_list)
            //                                                        {
            //                                                            if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
            //                                                            {
            //                                                                lnSubLine.StoreUOMQty = detail.out_num;
            //                                                                //  tranLine.TransInWh.Code = order.wa
            //                                                            }
            //                                                        }
            //                                                    }

            //                                                }
            //                                                // tranLine.StoreUOMQty = order.goods_count;
            //                                            }
            //                                        }
            //                                    }

            //                                    session.Commit();
            //                                }
            //                            }

            //                        }
            //                        else
            //                        {
            //                            CHelper.InsertU9Log(false, "调拨出库单查询", apireuslt, json, url);
            //                        }
            //                    }

            //                }
            //            }
            //            else if (transfer != null && transfer.TransInLines[0].TransInOrg == transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
            //            {
            //                if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
            //                {
            //                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
            //                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                    {
            //                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
            //                        List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
            //                        StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
            //                        //req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            //                        //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                        req.other_in_no = transfer.DocNo;
            //                        req.status = "70";
            //                        listreq.Add(req);
            //                        string json = JsonConvert.SerializeObject(listreq);
            //                        list.Add("body", json);
            //                        //分页数据
            //                        list.Add("page_size", "100");
            //                        list.Add("page_no", "0");
            //                        list.Add("calc_total", "1");
            //                        string sign = WDTChelper.GetWDTSign(list);
            //                        list.Add("sign", sign);
            //                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                        string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
            //                        StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
            //                        if (res.status == 0)
            //                        {
            //                            if (res.data != null && res.data.order.Count > 0)
            //                            {
            //                                using (ISession session = Session.Open())
            //                                {
            //                                    //foreach (TransInLine tranLine in transfer.TransInLines)
            //                                    //{
            //                                    foreach (Model.StockInOrderDto order in res.data.order)
            //                                    {
            //                                        if (order.other_in_no == transfer.DocNo)
            //                                        {
            //                                            foreach (TransInLine tranLine in transfer.TransInLines)
            //                                            {
            //                                                foreach (StockInOrderDeatail detail in order.detail_list)
            //                                                {
            //                                                    if (tranLine.ItemInfo.ItemCode == detail.goods_no)
            //                                                    {
            //                                                        tranLine.StoreUOMQty = detail.in_num;
            //                                                        //  tranLine.TransInWh.Code = order.wa
            //                                                    }
            //                                                }

            //                                            }
            //                                            //foreach (var item in order.detail_list)
            //                                            //{
            //                                            //foreach (TransInLine tranLine in transfer.TransInLines)
            //                                            //{
            //                                            //    if (tranLine.DocLineNo == Convert.ToInt32(item.remark) && tranLine.ItemInfo.ItemCode == item.goods_no)
            //                                            //    {
            //                                            //        tranLine.CostUOMQty = item.goo;
            //                                            //    }
            //                                            //}
            //                                            //}
            //                                        }
            //                                        //if (tranLine.ItemInfo.ItemCode == Deatail.goods_no)
            //                                        //{
            //                                        //    tranLine.CostUOMQty = Deatail.goods_count;
            //                                        //}
            //                                    }

            //                                    session.Commit();
            //                                }
            //                            }

            //                        }
            //                        else
            //                        {
            //                            CHelper.InsertU9Log(false, "调拨入库单查询", apireuslt, json, url);
            //                        }
            //                    }

            //                }
            //                if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null)
            //                {
            //                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
            //                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                    {
            //                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
            //                        List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
            //                        StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
            //                        //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            //                        //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                        req.other_out_no = transfer.DocNo;
            //                        req.status = "70";
            //                        listreq.Add(req);
            //                        string json = JsonConvert.SerializeObject(listreq);
            //                        list.Add("body", json);
            //                        //分页数据
            //                        list.Add("page_size", "100");
            //                        list.Add("page_no", "0");
            //                        list.Add("calc_total", "1");
            //                        string sign = WDTChelper.GetWDTSign(list);
            //                        list.Add("sign", sign);
            //                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                        string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
            //                        StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
            //                        if (res.status == 0)
            //                        {
            //                            if (res.data != null && res.data.order.Count > 0)
            //                            {
            //                                using (ISession session = Session.Open())
            //                                {
            //                                    //foreach (TransInLine tranLine in transfer.TransInLines)
            //                                    //{
            //                                    foreach (StockOutOrderDto order in res.data.order)
            //                                    {
            //                                        if (order.other_out_no == transfer.DocNo)
            //                                        {
            //                                            foreach (TransInLine tranLine in transfer.TransInLines)
            //                                            {
            //                                                foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
            //                                                {
            //                                                    if (tranLine.ID == lnSubLine.TransInLine.ID)
            //                                                    {
            //                                                        foreach (StockOutOrderDeatail detail in order.detail_list)
            //                                                        {
            //                                                            if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
            //                                                            {
            //                                                                lnSubLine.StoreUOMQty = detail.out_num;
            //                                                                //  tranLine.TransInWh.Code = order.wa
            //                                                            }
            //                                                        }
            //                                                    }

            //                                                }
            //                                                // tranLine.StoreUOMQty = order.goods_count;
            //                                            }
            //                                        }
            //                                    }

            //                                    session.Commit();
            //                                }
            //                            }

            //                        }
            //                        else
            //                        {
            //                            CHelper.InsertU9Log(false, "调拨出库单查询", apireuslt, json, url);
            //                        }
            //                    }

            //                }
            //            }

            //            try
            //            {
            //                List<CBO.Pub.Controller.CommonArchiveDataDTOData> result = new List<CBO.Pub.Controller.CommonArchiveDataDTOData>();
            //                CBO.Pub.Controller.CommonArchiveDataDTOData commonArchiveDataDTOData = new CBO.Pub.Controller.CommonArchiveDataDTOData();
            //                commonArchiveDataDTOData.ID = transfer.ID;
            //                result.Add(commonArchiveDataDTOData);
            //                TransferInBatchApproveSRVProxy approveProxy = new TransferInBatchApproveSRVProxy()
            //                {
            //                    ApprovedBy = Context.LoginUser,
            //                    ApprovedOn = DateTime.Now,
            //                    TargetOrgCode = Context.LoginOrg.Code,
            //                    TargetOrgName = Context.LoginOrg.Name,
            //                    DocList = result
            //                };
            //                approveProxy.Do();
            //            }
            //            catch (Exception ex)
            //            {
            //                CHelper.InsertU9Log(false, "调入单审核", ex.Message, null, null);
            //            }
            //        }

            //        // throw new Exception(res.message);
            //    }
            //    //foreach (var item in res.data.order)
            //    //{
            //    //    MiscRcvTrans miscRcvTrans = MiscRcvTrans.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", Context.LoginOrg.ID));
            //    //    if (miscRcvTrans != null)
            //    //    {
            //    //        using (ISession session = Session.Open())
            //    //        {
            //    //            foreach (MiscRcvTransL tranLine in miscRcvTrans.MiscRcvTransLs)
            //    //            {
            //    //                foreach (OrderDeatail Deatail in item.detail_list)
            //    //                {
            //    //                    if (tranLine.ItemInfo.ItemCode == Deatail.goods_no)
            //    //                    {
            //    //                        tranLine.CostUOMQty = Deatail.goods_count;
            //    //                    }
            //    //                }
            //    //            }
            //    //            session.Commit();
            //    //        }
            //    //        if (miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
            //    //        {
            //    //            using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
            //    //            {
            //    //                try
            //    //                {
            //    //                    List<long> lstID = new List<long>();
            //    //                    lstID.Add(miscRcvTrans.ID);
            //    //                    SubmitMiscRcv(lstID);
            //    //                    ApproveMiscRcv(lstID);


            //    //                    scope.Commit();
            //    //                }
            //    //                catch (Exception ex)
            //    //                {
            //    //                    scope.Rollback();
            //    //                    // throw new Exception(ex.Message);
            //    //                }
            //    //            }
            //    //        }
            //    //    }

            //    //}
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
        /// <summary>
        /// 标准收货创建
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetRcv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Purchase.queryWithDetail");
                List<StockinPurchaseOrderqueryWithDetailReq> listreq = new List<StockinPurchaseOrderqueryWithDetailReq>();
                StockinPurchaseOrderqueryWithDetailReq req = new StockinPurchaseOrderqueryWithDetailReq();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
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
                    string PODocNo = string.Empty;
                    //CommonSV commonSV = new CommonSV();
                    foreach (var item in res.data.order)
                    {
                        //先调用采购单查询接口找到U9采购单号
                        PODocNo = GetPODocNo(item.purchase_no);
                        PurchaseOrder miscRcvTrans = PurchaseOrder.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", PODocNo));
                        if (miscRcvTrans!=null)
                        {
                            Organization org = CHelper.GetOrg(miscRcvTrans.POLines[0].ItemInfo.ItemCode, "");
                            if (org != null)
                            {                              
                                if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "PO20")
                                {
                                    dataJson = JsonHelper.GetWDTReceivement(miscRcvTrans, item, org);
                                    CHelper.InsertFailInfo("标准收货创建", dataJson, org, item.order_no, 0);
                                    //commonSV.CreateMiscRcvTrans(dataJson);
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
            return  DocNo;
        }
        /// <summary>
        /// 委外收货创建
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetOCRcv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Purchase.queryWithDetail");
                List<StockinPurchaseOrderqueryWithDetailReq> listreq = new List<StockinPurchaseOrderqueryWithDetailReq>();
                StockinPurchaseOrderqueryWithDetailReq req = new StockinPurchaseOrderqueryWithDetailReq();
                req.start_time = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
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
                                CHelper.InsertFailInfo("委外收货创建", dataJson, org,item.order_no, 0, "");
                                // commonSV.CreatePMRcv(dataJson);
                            }
                        }





                    }
                    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                    {
                        page_no++;
                        GetOCRcv(page_no);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 委外退货更新
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetOCRtnRcv(int page_no)
        {
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("Select top 100  ID,DocType,Json,DocNo  from Cust_FailInfo where issuccess=0 and  DocType='采购退货更新' order by CreatedOn desc");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    Receivement Rcv = null;
                    string ErrMsg = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataJson = row["Json"].ToString();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(dataJson);
                        if (docDto != null)
                        {
                            Rcv = Receivement.Finder.FindByID(docDto.ID);
                            Organization org = Organization.FindByCode(docDto.OrgCode);

                            if (Rcv != null && Rcv.Status == RcvStatusEnum.Approving)
                            {
                                using (ISession session = Session.Open())
                                {
                                    foreach (RcvLine rcvline in Rcv.RcvLines)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                            {
                                                rcvline.RejectQtyTU = Deatail.Amount;
                                                rcvline.RtnFillQtyTU = Deatail.Amount;
                                                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Wh = wh;
                                                    rcvline.Wh.ID = wh.ID;
                                                    rcvline.Wh.Code = wh.Code;
                                                }
                                                rcvline.ConfirmDate = Rcv.BusinessDate;
                                            }
                                            else if (rcvline.DescFlexSegments.PrivateDescSeg1 == Deatail.specno)
                                            {
                                                rcvline.RejectQtyTU = Deatail.Amount;
                                                rcvline.RtnFillQtyTU = Deatail.Amount;
                                                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Wh = wh;
                                                    rcvline.Wh.ID = wh.ID;
                                                    rcvline.Wh.Code = wh.Code;
                                                }
                                                rcvline.ConfirmDate = Rcv.BusinessDate;
                                            }
                                        }
                                    }
                                    session.Commit();
                                }
                                using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                                {
                                    try
                                    {
                                        List<long> lstID = new List<long>();
                                        lstID.Add(Rcv.ID);
                                        RcvBatchApprovedBPProxy appProxy = new RcvBatchApprovedBPProxy();
                                        appProxy.ActType = 8;
                                        appProxy.DocHeadIDs = lstID;

                                        List<UFIDA.U9.PM.Rcv.PMDocErrListDTOData> appErrors = appProxy.Do();
                                        if (appErrors.Count > 0 && !string.IsNullOrEmpty(appErrors[0].ErrMsg))
                                        {
                                            ErrMsg = appErrors[0].ErrMsg;
                                            sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
                                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                            CHelper.InsertU9Log(false, "采购退货审核", appErrors[0].ErrMsg, Rcv.DocNo, "");
                                        }
                                        else
                                        {
                                            sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                        }
                                        scope.Commit();

                                    }
                                    catch (Exception ex)
                                    {
                                        scope.Rollback();
                                        // throw new Exception(ex.Message);
                                    }
                                }
                            }
                            //Rcv = Receivement.Finder.FindByID(docDto.ID);
                            //if (Rcv!=null && Rcv.Status == RcvStatusEnum.Closed)
                            //{
                            //	sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                            //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                            //else
                            //{
                            //	sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
                            //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "采购退货更新", ex.Message, dataJson, "");
                throw new Exception(ex.Message);
            }
            //string dataJson = string.Empty;
            //try
            //{
            //    string sql = string.Empty;
            //    DataSet ds = new DataSet();
            //    sql = string.Format("Select top 100  ID,DocType,Json,DocNo  from Cust_FailInfo where issuccess=0 and  DocType='委外退货更新'");
            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
            //    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
            //    {
            //        string apirEEEeuslt = string.Empty;
            //        Receivement Rcv = null;
            //        string ErrMsg = string.Empty;
            //        foreach (DataRow row in ds.Tables[0].Rows)
            //        {
            //            dataJson = row["Json"].ToString();
            //            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //            UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(dataJson);
            //            if (docDto != null)
            //            {
            //                Rcv = Receivement.Finder.FindByID(docDto.ID);
            //                Organization org = Organization.FindByCode(docDto.OrgCode);

            //                if (Rcv != null && Rcv.Status == RcvStatusEnum.Approving)
            //                {
            //                    using (ISession session = Session.Open())
            //                    {

            //                        foreach (RcvLine rcvline in Rcv.RcvLines)
            //                        {
            //                            foreach (UpdateDocLineDto Deatail in docDto.DocLines)
            //                            {
            //                                if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
            //                                {
            //                                    rcvline.RejectQtyTU = Deatail.Amount;
            //                                    rcvline.RtnFillQtyTU = Deatail.Amount;
            //                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
            //                                    if (wh != null)
            //                                    {
            //                                        rcvline.Wh = wh;
            //                                        rcvline.Wh.ID = wh.ID;
            //                                        rcvline.Wh.Code = wh.Code;
            //                                    }
            //                                    rcvline.ConfirmDate = Rcv.BusinessDate;

            //                                }
            //                            }
            //                        }
            //                        session.Commit();
            //                    }
            //                    using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
            //                    {
            //                        try
            //                        {
            //                            List<long> lstID = new List<long>();
            //                            lstID.Add(Rcv.ID);
            //                            RcvBatchApprovedBPProxy appProxy = new RcvBatchApprovedBPProxy();
            //                            appProxy.ActType = 8;
            //                            appProxy.DocHeadIDs = lstID;

            //                            List<UFIDA.U9.PM.Rcv.PMDocErrListDTOData> appErrors = appProxy.Do();
            //                            if (appErrors.Count > 0 && !string.IsNullOrEmpty(appErrors[0].ErrMsg))
            //                            {
            //                                ErrMsg = appErrors[0].ErrMsg;
            //                                sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
            //                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                                CHelper.InsertU9Log(false, "委外退货审核", appErrors[0].ErrMsg, Rcv.DocNo, "");
            //                            }
            //                            else
            //                            {
            //                                sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
            //                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                            }
            //                            scope.Commit();

            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            scope.Rollback();
            //                            // throw new Exception(ex.Message);
            //                        }
            //                    }
            //                }
            //                //Rcv = Receivement.Finder.FindByID(docDto.ID);
            //                //if (Rcv!=null && Rcv.Status == RcvStatusEnum.Closed)
            //                //{
            //                //	sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
            //                //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                //}
            //                //else
            //                //{
            //                //	sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
            //                //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
            //                //}
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    CHelper.InsertU9Log(false, "委外退货更新", ex.Message, dataJson, "");
            //    throw new Exception(ex.Message);
            //}


        }
        /// <summary>
        /// 杂收单更新
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetMiscRcv(int page_no)
        {
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                string Msg = string.Empty;
                sql = string.Format("Select top 50  ID,DocType,Json,DocNo  from Cust_FailInfo where issuccess=0 and  DocType='杂收单更新'");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    MiscRcvTrans Rcv = null;
                    string ErrMsg = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataJson = row["Json"].ToString();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(dataJson);
                        if (docDto != null)
                        {
                            Rcv = MiscRcvTrans.Finder.FindByID(docDto.ID);
                            Organization org = Organization.FindByCode(docDto.OrgCode);

                            if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approving)
                            {
                                using (ISession session = Session.Open())
                                {
                                    foreach (MiscRcvTransL rcvline in Rcv.MiscRcvTransLs)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                            {
                                                rcvline.CostUOMQty = Deatail.Amount;
                                                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Wh = wh;
                                                    rcvline.Wh.ID = wh.ID;
                                                    rcvline.Wh.Code = wh.Code;
                                                }

                                            }
                                        }
                                    }
                                    session.Commit();
                                }
                                using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                                {
                                    try
                                    {
                                        List<long> lstID = new List<long>();
                                        lstID.Add(Rcv.ID);
                                        Msg = ApproveMiscRcv(lstID);


                                        scope.Commit();
                                        sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    }
                                    catch (Exception ex)
                                    {
                                        Msg = ex.Message;                                      
                                        scope.Rollback();
                                        sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), Msg);
                                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                        // throw new Exception(ex.Message);
                                    }
                                }
                            }
                            //Rcv = MiscRcvTrans.Finder.FindByID(docDto.ID);
                            //if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approved)
                            //{
                            //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                            //else
                            //{
                            //    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), Msg);
                            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "杂收单更新", ex.Message, dataJson, "");
                //throw new Exception(ex.Message);
            }
          
        }
        private void GetMiscRcvs(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                List<stockinOtherqueryWithDetailReq> listreq = new List<stockinOtherqueryWithDetailReq>();
                stockinOtherqueryWithDetailReq req = new stockinOtherqueryWithDetailReq();
                req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                req.end_time = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                req.status = "70";
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
                StockinOtherQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinOtherQueryWithDetailRes>(apireuslt);
                if (res.status == 0)
                {
                    MiscRcvTrans miscRcvTrans = null;
                    string dataJson = string.Empty;
                    foreach (var item in res.data.order)
                    {
                        if (item.reason == "普通其他入库")
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                miscRcvTrans = MiscRcvTrans.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.other_in_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            }
                            if (miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
                            {
                                if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "MiscRcv002")
                                {
                                    dataJson = JsonHelper.GetWDTStockInUpdateMiscRcvTrans(item, org, miscRcvTrans.ID);
                                    CHelper.InsertFailInfo("杂收单更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                    //using (ISession session = Session.Open())
                                    //{
                                    //    foreach (MiscRcvTransL tranLine in miscRcvTrans.MiscRcvTransLs)
                                    //    {
                                    //        foreach (OrderDeatail Deatail in item.detail_list)
                                    //        {
                                    //            if (tranLine.ItemInfo.ItemCode == Deatail.goods_no && tranLine.DocLineNo == Convert.ToInt32(Deatail.remark))
                                    //            {
                                    //                tranLine.CostUOMQty = Deatail.goods_count;
                                    //            }
                                    //        }
                                    //    }
                                    //    session.Commit();
                                    //}


                                    //using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                                    //{
                                    //    try
                                    //    {
                                    //        List<long> lstID = new List<long>();
                                    //        lstID.Add(miscRcvTrans.ID);
                                    //        ApproveMiscRcv(lstID);


                                    //        scope.Commit();
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        scope.Rollback();
                                    //        // throw new Exception(ex.Message);
                                    //    }
                                    //}

                                }
                            }

                        }
                        if (res.data.order.Count == 100)
                        {
                            page_no++;
                            GetMiscRcvs(page_no);
                        }
                    }
                }
                else
                {
                    CHelper.InsertU9Log(false, "其它入库单查询", apireuslt, json, url);
                    // throw new Exception(res.message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Other.queryWithDetail");
            //    List<stockinOtherqueryWithDetailReq> listreq = new List<stockinOtherqueryWithDetailReq>();
            //    stockinOtherqueryWithDetailReq req = new stockinOtherqueryWithDetailReq();
            //    req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            //    req.end_time = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
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
            //    string apireuslt = Tools.HttpPost(url, json);
            //    StockinQueryWithDetailRes res = JsonConvert.DeserializeObject<StockinQueryWithDetailRes>(apireuslt);
            //    if (res.status == 0)
            //    {
            //        MiscRcvTrans miscRcvTrans = null;
            //        string dataJson = string.Empty;
            //        foreach (var item in res.data.order)
            //        {
            //            if (item.reason == "普通其他入库")
            //            {
            //                Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //                if (org != null)
            //                {
            //                    miscRcvTrans = MiscRcvTrans.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
            //                }
            //                if (miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
            //                {
            //                    if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "MiscRcv002")
            //                    {
            //                        dataJson = JsonHelper.GetWDTUpdateMiscRcvTrans(item, org,miscRcvTrans.ID);
            //                        CHelper.InsertFailInfo("杂收单更新", dataJson,org, miscRcvTrans.DocNo,0);
            //                        //using (ISession session = Session.Open())
            //                        //{
            //                        //    foreach (MiscRcvTransL tranLine in miscRcvTrans.MiscRcvTransLs)
            //                        //    {
            //                        //        foreach (OrderDeatail Deatail in item.detail_list)
            //                        //        {
            //                        //            if (tranLine.ItemInfo.ItemCode == Deatail.goods_no && tranLine.DocLineNo == Convert.ToInt32(Deatail.remark))
            //                        //            {
            //                        //                tranLine.CostUOMQty = Deatail.goods_count;
            //                        //            }
            //                        //        }
            //                        //    }
            //                        //    session.Commit();
            //                        //}


            //                        //using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
            //                        //{
            //                        //    try
            //                        //    {
            //                        //        List<long> lstID = new List<long>();
            //                        //        lstID.Add(miscRcvTrans.ID);
            //                        //        ApproveMiscRcv(lstID);


            //                        //        scope.Commit();
            //                        //    }
            //                        //    catch (Exception ex)
            //                        //    {
            //                        //        scope.Rollback();
            //                        //        // throw new Exception(ex.Message);
            //                        //    }
            //                        //}

            //                    }
            //                }

            //            }
            //            if (res.data.order.Count == 100)
            //            {
            //                page_no++;
            //                GetMiscRcv(page_no);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        CHelper.InsertU9Log(false, "其它入库单查询", apireuslt, json, url);
            //        // throw new Exception(res.message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
        /// <summary>
        /// 杂发单更新
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetMiscShip(int page_no)
        {
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                string Msg = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("Select top 100  ID,DocType,Json,DocNo  from Cust_FailInfo where issuccess=0 and  DocType='杂发单更新'");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    MiscShipment Rcv = null;
                    string ErrMsg = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataJson = row["Json"].ToString();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(dataJson);
                        if (docDto != null)
                        {
                            Rcv = MiscShipment.Finder.FindByID(docDto.ID);
                            Organization org = Organization.FindByCode(docDto.OrgCode);

                            if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approving)
                            {
                                using (ISession session = Session.Open())
                                {
                                    foreach (MiscShipmentL rcvline in Rcv.MiscShipLs)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                            {
                                                rcvline.CostUOMQty = Deatail.Amount;
                                                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Wh = wh;
                                                    rcvline.Wh.ID = wh.ID;
                                                    rcvline.Wh.Code = wh.Code;
                                                }

                                            }
                                        }
                                    }
                                    session.Commit();
                                }
                                using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                                {
                                    try
                                    {
                                        List<long> lstID = new List<long>();
                                        lstID.Add(Rcv.ID);
                                        Msg = ApproveMiscShip(lstID);


                                        scope.Commit();
                                        sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    }
                                    catch (Exception ex)
                                    {
                                        Msg = ex.Message;
                                       
                                        scope.Rollback();
                                        sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), Msg);
                                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                        // throw new Exception(ex.Message);
                                    }
                                }
                            }
                            //Rcv = MiscShipment.Finder.FindByID(docDto.ID);
                            //if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approved)
                            //{
                            //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                            //else
                            //{
                            //    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), Msg);
                            //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "杂发单更新", ex.Message, dataJson, "");
                throw new Exception(ex.Message);
            }
            //try
            //{
            //    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.OtherQuery.queryWithDetail");
            //    List<StockOutOtherqueryWithDetailReq> listreq = new List<StockOutOtherqueryWithDetailReq>();
            //    StockOutOtherqueryWithDetailReq req = new StockOutOtherqueryWithDetailReq();
            //    req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            //    req.end_time = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            //    req.status = 70;
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
            //    StockOutMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutMiscShipQueryWithDetailRes>(apireuslt);
            //    if (res.status == 0)
            //    {
            //        MiscShipment miscRcvTrans = null;
            //        foreach (var item in res.data.order)
            //        {
            //            if (item.reason == "普通其他出库")
            //            {
            //                Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
            //                if (org != null)
            //                {
            //                    miscRcvTrans = MiscShipment.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
            //                }
            //                if (miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
            //                {
            //                    if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "MiscShip003")
            //                    {
            //                        using (ISession session = Session.Open())
            //                        {
            //                            foreach (MiscShipmentL tranLine in miscRcvTrans.MiscShipLs)
            //                            {
            //                                foreach (PurchaseReturnOrderDetail Deatail in item.detail_list)
            //                                {
            //                                    if (tranLine.ItemInfo.ItemCode == Deatail.goods_no && tranLine.DocLineNo == Convert.ToInt32(Deatail.remark))
            //                                    {
            //                                        tranLine.CostUOMQty = Deatail.goods_count;
            //                                    }
            //                                }
            //                            }
            //                            session.Commit();
            //                        }

            //                        using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
            //                        {
            //                            try
            //                            {
            //                                List<long> lstID = new List<long>();
            //                                lstID.Add(miscRcvTrans.ID);
            //                                SubmitMiscShip(lstID);
            //                                ApproveMiscShip(lstID);


            //                                scope.Commit();
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                scope.Rollback();
            //                                // throw new Exception(ex.Message);
            //                            }
            //                        }
            //                    }
            //                }

            //            }


            //        }
            //        if (res.data.order.Count == 100)
            //        {
            //            page_no++;
            //            GetMiscShip(page_no);
            //        }
            //    }
            //    else
            //    {
            //        CHelper.InsertU9Log(false, "其它出库单查询", apireuslt, json, url);
            //        // throw new Exception(res.message);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
        /// <summary>
        /// 杂发单更新写入
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        private void GetMiscShipss(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                req.start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                req.status = "70";
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
                StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);
                if (res.status == 0 && res.data != null && res.data.order != null)
                {
                    MiscShipment miscRcvTrans = null;
                    string dataJson = string.Empty;
                    foreach (var item in res.data.order)
                    {
                        if (item.reason == "普通其他出库")
                        {
                            Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            if (org != null)
                            {
                                miscRcvTrans = MiscShipment.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.other_out_no), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                            }
                            if (miscRcvTrans !=null && miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
                            {
                                if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "MiscShip003")
                                {
                                    dataJson = JsonHelper.GetWDTStockOutUpdateMiscShip(item, org, miscRcvTrans.ID);
                                    CHelper.InsertFailInfo("杂发单更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                    //using (ISession session = Session.Open())
                                    //{
                                    //    foreach (MiscShipmentL tranLine in miscRcvTrans.MiscShipLs)
                                    //    {
                                    //        foreach (PurchaseReturnOrderDetail Deatail in item.detail_list)
                                    //        {
                                    //            if (tranLine.ItemInfo.ItemCode == Deatail.goods_no && tranLine.DocLineNo == Convert.ToInt32(Deatail.remark))
                                    //            {
                                    //                tranLine.CostUOMQty = Deatail.goods_count;
                                    //            }
                                    //        }
                                    //    }
                                    //    session.Commit();
                                    //}

                                    //using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                                    //{
                                    //    try
                                    //    {
                                    //        List<long> lstID = new List<long>();
                                    //        lstID.Add(miscRcvTrans.ID);
                                    //        ApproveMiscShip(lstID);


                                    //        scope.Commit();
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        scope.Rollback();
                                    //        // throw new Exception(ex.Message);
                                    //    }
                                    //}
                                }
                            }

                        }


                    }
                    if (res.data.order.Count == 100)
                    {
                        page_no++;
                        GetMiscShipss(page_no);
                    }
                }
                else
                {
                    CHelper.InsertU9Log(false, "其它出库单查询", apireuslt, json, url);
                    // throw new Exception(res.message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            } 
        }
        /// <summary>
        /// 提交杂发单
        /// </summary>
        /// <param name="lstID"></param>
        private void SubmitMiscShip(List<long> lstID)
        {
            try
            {
                if (lstID == null || (lstID != null && lstID.Count <= 0))
                    throw new Exception("杂发单ID不能为空！");
                foreach (long ID in lstID)
                {
                    List<CommonArchiveDataDTOData> lst = new List<CommonArchiveDataDTOData>();
                    CommonArchiveDataDTOData misdto = new CommonArchiveDataDTOData();
                    misdto.ID = ID;
                    lst.Add(misdto);
                    //提交
                    UFIDA.U9.ISV.MiscShipISV.Proxy.CommonCommitMiscShipSVProxy submitpx = new ISV.MiscShipISV.Proxy.CommonCommitMiscShipSVProxy();
                    submitpx.MiscShipmentKeyList = lst;
                    submitpx.Do();
                }
            }
            catch (Exception EX)
            {
                throw new Exception(EX.Message);
            }
        }

         
      
        /// <summary>
        /// 审核杂收单
        /// </summary>
        /// <param name="lstID"></param>
        private string ApproveMiscRcv(List<long> lstID)
        {
            string Msg = string.Empty;
            try
            {
                if (lstID == null || (lstID != null && lstID.Count <= 0))
                    throw new Exception("杂收单ID不能为空！");
                foreach (long ID in lstID)
                {
                    List<CommonArchiveDataDTOData> lst = new List<CommonArchiveDataDTOData>();
                    CommonArchiveDataDTOData misdto = new CommonArchiveDataDTOData();
                    misdto.ID = ID;
                    lst.Add(misdto);

                    UFIDA.U9.ISV.MiscRcvISV.Proxy.CommonApproveMiscRcvProxy appx = new ISV.MiscRcvISV.Proxy.CommonApproveMiscRcvProxy();
                    appx.MiscRcvKeys = lst;
                    appx.Do();
                }
            }
            catch (Exception EX)
            {
                Msg = EX.Message;
            }
            return Msg;
        }
        /// <summary>
        /// 供应商推送
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTProvider(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    Supplier supplier = Supplier.Finder.FindByID(ID);
                    string dataJson = JsonHelper.GetSupplierJson(supplier);
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("setting.PurchaseProvider.push");
                    list.Add("body", dataJson);
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, dataJson,"");
                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                    if (res.status == 0)
                        CHelper.InsertU9Log(true, "供应商推送", apireuslt, dataJson, url, supplier.Code);
                    else
                    {
                         CHelper.InsertU9Log(false, "供应商推送", res.message, dataJson, url, supplier.Code);
                       // CHelper.InsertFailInfo("供应商推送", dataJson, supplier.Code, res.message);
                        sErrorItem += supplier.Code +res.message+ ",";
                    }
                    if (!string.IsNullOrEmpty(sErrorItem))
                    {
                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                        rtn.IsSuccess = false;
                        rtn.Msg = string.Format("供应商【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                    }
                    else
                        rtn.IsSuccess = true;

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 审核供应商推送
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public RtnDataJson ApproveSendWDTProvider(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    Supplier supplier = Supplier.Finder.FindByID(ID);
                    
                    //DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1016"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg1));
                    if (supplier.Category!=null && supplier.Category.DescFlexField.PrivateDescSeg1=="10")
                    {
                        string dataJson = JsonHelper.GetSupplierJson(supplier);
                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("setting.PurchaseProvider.push");
                        list.Add("body", dataJson);
                        string sign = WDTChelper.GetWDTSign(list);
                        list.Add("sign", sign);
                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                        string apireuslt = Tools.HttpPost(url, dataJson, "");
                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                        if (res.status == 0)
                            CHelper.InsertU9Log(true, "供应商推送", apireuslt, dataJson, url, supplier.Code);
                        else
                        {
                            CHelper.InsertU9Log(false, "供应商推送", res.message, dataJson, url, supplier.Code);
                            // CHelper.InsertFailInfo("供应商推送", dataJson, supplier.Code, res.message);
                            sErrorItem += supplier.Code + res.message + ",";
                        }
                        if (!string.IsNullOrEmpty(sErrorItem))
                        {
                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                            rtn.IsSuccess = false;
                            rtn.Msg = string.Format("供应商【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                        }
                        else
                            rtn.IsSuccess = true;
                    }
                   

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 调拨入库
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTTransfer(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    TransferIn transfer = TransferIn.Finder.FindByID(ID);
                    if (transfer != null && transfer.TransInLines[0]!=null)
                    {
                        Organization org = CHelper.GetOrg(transfer.TransInLines[0].ItemInfo.ItemCode, "");
                        if (org == transfer.Org) 
                        {
                            if (transfer != null && transfer.TransInLines[0].TransInOrg != transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
                            {
                                if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOtherInTransferJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.In.push");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)//成功了是否需要记录日志
                                            CHelper.InsertU9Log(true, "调入单推送调拨入库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送调拨入库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("调入单推送调拨入库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                        }
                                        else
                                        {
                                            rtn.IsSuccess = true;
                                            rtn.DocNo = res.data.message;
                                        }
                                    }


                                }
                                if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null && transfer.TransInLines[0].TransInSubLines[0].TransOutWh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOutTransferOutJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.Out.push");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)
                                            CHelper.InsertU9Log(true, "调入单推送调拨出库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送调拨出库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg += string.Format("调入单推送调拨出库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                        }
                                        else
                                        {
                                            rtn.IsSuccess = true;
                                            rtn.DocNo = res.data.message;
                                        }
                                    }


                                }
                            }
                            else if (transfer != null && transfer.TransInLines[0].TransInOrg == transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
                            {
                                if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOtherInTransferJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.In.push");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)//成功了是否需要记录日志
                                            CHelper.InsertU9Log(true, "调入单推送调拨入库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送调拨入库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("调入单推送调拨入库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                        }
                                        else
                                        {
                                            rtn.IsSuccess = true;
                                            rtn.DocNo = res.data.message;
                                        }
                                    }


                                }
                                if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null && transfer.TransInLines[0].TransInSubLines[0].TransOutWh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOutTransferOutJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.Out.push");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)
                                            CHelper.InsertU9Log(true, "调入单推送调拨出库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送调拨出库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg += string.Format("调入单推送调拨出库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                        }
                                        else
                                        {
                                            rtn.IsSuccess = true;
                                            rtn.DocNo = res.data.message;
                                        }
                                    }


                                }
                            }
                        }
                        else
                        {
                            rtn.IsSuccess = false;
                            rtn.Msg = "当前组织和料品品牌维护的组织不一致";
                        }
                    }
                    

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            //try
            //{
            //    List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
            //    string sErrorItem = "";
            //    foreach (long ID in IDs)
            //    {
            //        TransferIn transfer = TransferIn.Finder.FindByID(ID);
            //        if (transfer!=null && transfer.TransInLines[0].TransInOrg!= transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
            //        {
            //            if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
            //            {
            //                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
            //                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                {
            //                    string dataJson = JsonHelper.GetTransferJson(transfer);
            //                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Transfer.createOrder");
            //                    list.Add("body", dataJson);
            //                    string sign = WDTChelper.GetWDTSign(list);
            //                    list.Add("sign", sign);
            //                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                    string apireuslt = Tools.HttpPost(url, dataJson);
            //                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
            //                    if (res.status == 0)//成功了是否需要记录日志
            //                        CHelper.InsertU9Log(true, "调入单推送调拨入库", apireuslt, dataJson, url, transfer.DocNo);
            //                    else
            //                    {
            //                        CHelper.InsertU9Log(false, "调入单推送调拨入库", res.message, dataJson, url, transfer.DocNo);
            //                        sErrorItem += transfer.DocNo + res.message + ",";
            //                    }
            //                    if (!string.IsNullOrEmpty(sErrorItem))
            //                    {
            //                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
            //                        rtn.IsSuccess = false;
            //                        rtn.Msg = string.Format("调入单推送调拨入库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
            //                    }
            //                    else
            //                    {
            //                        rtn.IsSuccess = true;
            //                        rtn.DocNo = res.data.message;
            //                    }
            //                }


            //            }
            //            if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null && transfer.TransInLines[0].TransInSubLines[0].TransOutWh != null)
            //            {
            //                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
            //                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                {
            //                    string dataJson = JsonHelper.GetTransferOutJson(transfer);
            //                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Transfer.createOrder");
            //                    list.Add("body", dataJson);
            //                    string sign = WDTChelper.GetWDTSign(list);
            //                    list.Add("sign", sign);
            //                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                    string apireuslt = Tools.HttpPost(url, dataJson);
            //                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
            //                    if (res.status == 0)
            //                        CHelper.InsertU9Log(true, "调入单推送调拨出库", apireuslt, dataJson, url, transfer.DocNo);
            //                    else
            //                    {
            //                        CHelper.InsertU9Log(false, "调入单推送调拨出库", res.message, dataJson, url, transfer.DocNo);
            //                        sErrorItem += transfer.DocNo + res.message + ",";
            //                    }
            //                    if (!string.IsNullOrEmpty(sErrorItem))
            //                    {
            //                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
            //                        rtn.IsSuccess = false;
            //                        rtn.Msg += string.Format("调入单推送调拨出库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
            //                    }
            //                    else
            //                    {
            //                        rtn.IsSuccess = true;
            //                        rtn.DocNo = res.data.message;
            //                    }
            //                }


            //            }
            //        }
            //        else if (transfer != null && transfer.TransInLines[0].TransInOrg == transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
            //        {
            //            if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
            //            {
            //                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
            //                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                {
            //                    string dataJson = JsonHelper.GetTransferJson(transfer);
            //                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Transfer.createOrder");
            //                    list.Add("body", dataJson);
            //                    string sign = WDTChelper.GetWDTSign(list);
            //                    list.Add("sign", sign);
            //                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                    string apireuslt = Tools.HttpPost(url, dataJson);
            //                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
            //                    if (res.status == 0)//成功了是否需要记录日志
            //                        CHelper.InsertU9Log(true, "调入单推送调拨入库", apireuslt, dataJson, url, transfer.DocNo);
            //                    else
            //                    {
            //                        CHelper.InsertU9Log(false, "调入单推送调拨入库", res.message, dataJson, url, transfer.DocNo);
            //                        sErrorItem += transfer.DocNo + res.message + ",";
            //                    }
            //                    if (!string.IsNullOrEmpty(sErrorItem))
            //                    {
            //                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
            //                        rtn.IsSuccess = false;
            //                        rtn.Msg = string.Format("调入单推送调拨入库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
            //                    }
            //                    else
            //                    {
            //                        rtn.IsSuccess = true;
            //                        rtn.DocNo = res.data.message;
            //                    }
            //                }


            //            }
            //            if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null && transfer.TransInLines[0].TransInSubLines[0].TransOutWh != null)
            //            {
            //                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
            //                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
            //                {
            //                    string dataJson = JsonHelper.GetTransferOutJson(transfer);
            //                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Transfer.createOrder");
            //                    list.Add("body", dataJson);
            //                    string sign = WDTChelper.GetWDTSign(list);
            //                    list.Add("sign", sign);
            //                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
            //                    string apireuslt = Tools.HttpPost(url, dataJson);
            //                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
            //                    if (res.status == 0)
            //                        CHelper.InsertU9Log(true, "调入单推送调拨出库", apireuslt, dataJson, url, transfer.DocNo);
            //                    else
            //                    {
            //                        CHelper.InsertU9Log(false, "调入单推送调拨出库", res.message, dataJson, url, transfer.DocNo);
            //                        sErrorItem += transfer.DocNo + res.message + ",";
            //                    }
            //                    if (!string.IsNullOrEmpty(sErrorItem))
            //                    {
            //                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
            //                        rtn.IsSuccess = false;
            //                        rtn.Msg += string.Format("调入单推送调拨出库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
            //                    }
            //                    else
            //                    {
            //                        rtn.IsSuccess = true;
            //                        rtn.DocNo = res.data.message;
            //                    }
            //                }


            //            }
            //        }

            //    }

            //}
            //catch (Exception ex)
            //{
            //    rtn.Msg = ex.Message;
            //    rtn.IsSuccess = false;
            //}
            return rtn;
        }
        /// <summary>
        /// 调拨出库
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTTransferOut(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    TransferIn transfer = TransferIn.Finder.FindByID(ID);
                    string dataJson = JsonHelper.GetTransferOutJson(transfer);
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Transfer.createOrder");
                    list.Add("body", dataJson);
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, dataJson);
                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                    if (res.status == 0)
                        CHelper.InsertU9Log(true, "调入单推送", apireuslt, dataJson, url, transfer.DocNo);
                    else
                    {
                        CHelper.InsertU9Log(false, "调入单推送", res.message, dataJson, url, transfer.DocNo);
                        sErrorItem += transfer.DocNo + ",";
                    }
                    if (!string.IsNullOrEmpty(sErrorItem))
                    {
                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                        rtn.IsSuccess = false;
                        rtn.Msg = string.Format("调入单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                    }
                    else
                        rtn.IsSuccess = true;
                    rtn.DocNo = res.data.message;
                    using (ISession session = Session.Open())
                    {
                        transfer.DescFlexField.PrivateDescSeg4 = rtn.DocNo;
                        session.Commit();
                    }
                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 杂发单
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTMiscShip(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    UFIDA.U9.InvDoc.MiscShip.MiscShipment miscShipment = UFIDA.U9.InvDoc.MiscShip.MiscShipment.Finder.FindByID(ID);
                    if (miscShipment != null)
                    {
                        Organization org = CHelper.GetOrg(miscShipment.MiscShipLs[0].ItemInfo.ItemCode, "");
                        if (org == miscShipment.Org)
                        {
                            if (miscShipment.DocType.Code == "MiscShip003" && miscShipment.MiscShipLs[0].Wh != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(miscShipment.MiscShipLs[0].Wh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                {
                                    string dataJson = JsonHelper.GetStockoutMiscShipJson(miscShipment);
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.Out.push");
                                    list.Add("body", dataJson);
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = Tools.HttpPost(url, dataJson);
                                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                    if (res.status == 0)
                                        CHelper.InsertU9Log(true, "杂发单推送", apireuslt, dataJson, url, miscShipment.DocNo);
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "杂发单推送", res.message, dataJson, url, miscShipment.DocNo);
                                        sErrorItem += miscShipment.DocNo + res.message + ",";
                                    }
                                    if (!string.IsNullOrEmpty(sErrorItem))
                                    {
                                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                        rtn.IsSuccess = false;
                                        rtn.Msg = string.Format("杂发单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                    }
                                    else
                                        rtn.IsSuccess = true;
                                }
                            }
                        }
                        else
                        {
                            rtn.IsSuccess = false;
                            rtn.Msg = "当前组织和料品品牌维护的组织不一致";
                        }
                    }
                  
                  
                     

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }

        /// <summary>
        /// 杂收单
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTMiscRcvTrans(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    MiscRcvTrans MiscRcvTrans = MiscRcvTrans.Finder.FindByID(ID);
                    if (MiscRcvTrans != null)
                    {
                        Organization org = CHelper.GetOrg(MiscRcvTrans.MiscRcvTransLs[0].ItemInfo.ItemCode, "");
                        if (org == MiscRcvTrans.Org)
                        {
                            if (MiscRcvTrans.DocType.Code == "MiscRcv002" && MiscRcvTrans.MiscRcvTransLs[0].Wh != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(MiscRcvTrans.MiscRcvTransLs[0].Wh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                {
                                    string dataJson = JsonHelper.GetStockinMiscRcvTransJson(MiscRcvTrans);
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.In.push");
                                    list.Add("body", dataJson);
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = Tools.HttpPost(url, dataJson);
                                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                    if (res.status == 0)
                                        CHelper.InsertU9Log(true, "杂收单推送", apireuslt, dataJson, url, MiscRcvTrans.DocNo);
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "杂收单推送", res.message, dataJson, url, MiscRcvTrans.DocNo);
                                        sErrorItem += MiscRcvTrans.DocNo + res.message + ",";
                                    }
                                    if (!string.IsNullOrEmpty(sErrorItem))
                                    {
                                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                        rtn.IsSuccess = false;
                                        rtn.Msg = string.Format("杂收单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                    }
                                    else
                                        rtn.IsSuccess = true;
                                }
                            }
                        }
                        else
                        {
                            rtn.IsSuccess = false;
                            rtn.Msg = "当前组织和料品品牌维护的组织不一致";
                        }
                    }
                   
                
                  

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 委外退货
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTPurchaseReturn(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    Receivement Rcv = Receivement.Finder.FindByID(ID);
                    if (Rcv != null)
                    {
                        Organization org = CHelper.GetOrg(Rcv.RcvLines[0].ItemInfo.ItemCode, "");
                        if (org==Rcv.Org)
                        {
                            if (Rcv.DocType.Code == "RCV93")
                            {
                                if (Rcv.RcvLines[0].Wh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(Rcv.RcvLines[0].Wh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                    {
                                        string dataJson = JsonHelper.GetMiscRcvPurReturn(Rcv);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseReturn.createOrder");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)
                                            CHelper.InsertU9Log(true, "委外退货单推送", apireuslt, dataJson, url, Rcv.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "委外退货单推送", res.message, dataJson, url, Rcv.DocNo);
                                            sErrorItem += Rcv.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("委外退货单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                        }
                                        else
                                            rtn.IsSuccess = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            rtn.IsSuccess = false;
                            rtn.Msg = "当前组织和料品品牌维护的组织不一致";
                        }
                    }
                    
                  
                    

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 采购退货
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTSalesPOReturn(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    Receivement Rcv = Receivement.Finder.FindByID(ID);
                    if (Rcv != null)
                    {
                        Organization org = CHelper.GetOrg(Rcv.RcvLines[0].ItemInfo.ItemCode, "");
                        if (org == Rcv.Org)
                        {
                            if (Rcv.DocType.Code == "RCV95")
                            {
                                if (Rcv.RcvLines[0].Wh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(Rcv.RcvLines[0].Wh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                    {
                                        string dataJson = JsonHelper.GetMiscRcvPurReturn(Rcv);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseReturn.createOrder");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)
                                            CHelper.InsertU9Log(true, "采购退货单推送", apireuslt, dataJson, url, Rcv.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "采购退货单推送", res.message, dataJson, url, Rcv.DocNo);
                                            sErrorItem += Rcv.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("采购退货单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                        }
                                        else
                                            rtn.IsSuccess = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            rtn.IsSuccess = false;
                            rtn.Msg = "当前组织和料品品牌维护的组织不一致";
                        }
                    }
                    



                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 委外采购
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTPurchaseExternal(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    PurchaseOrder PO = PurchaseOrder.Finder.FindByID(ID);
                    if(PO != null && PO.POLines[0] != null && PO.POLines[0].POShiplines[0] != null && PO.POLines[0].POShiplines[0].Wh != null)
                    {
                        Warehouse wh = Warehouse.Finder.FindByID(PO.POLines[0].POShiplines[0].Wh.ID);
                        if(wh != null && wh.DescFlexField.PrivateDescSeg1=="10")
                        {
                            Organization org = CHelper.GetOrg(PO.POLines[0].ItemInfo.ItemCode, "");
                            if (org == PO.Org)
                            {
                                if (PO.DocType.Code == "PO22")//具体的编码
                                {
                                    string dataJson = JsonHelper.GetPurchaseOrderExternal(PO);
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseOrder.createOrder");
                                    list.Add("body", dataJson);
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = Tools.HttpPost(url, dataJson);
                                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                    if (res.status != 0)
                                    {
                                        CHelper.InsertU9Log(false, "外协采购单推送", res.message, dataJson, url, PO.DocNo);
                                        sErrorItem += PO.DocNo + res.message + ",";
                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(true, "外协采购单推送", res.message, dataJson, url, PO.DocNo);
                                    }
                                    if (!string.IsNullOrEmpty(sErrorItem))
                                    {
                                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                        rtn.IsSuccess = false;
                                        rtn.Msg = string.Format("外协采购单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                    }
                                    else
                                        rtn.IsSuccess = true;
                                }
                            }
                            else
                            {
                                rtn.IsSuccess = false;
                                rtn.Msg = "当前组织和料品维护的品牌组织不一致";
                            }
                        }
                        
                    }
                   
                    
                   

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 委外采购关闭
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson CloseWDTPurchaseExternal(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                List<string> DocLists = new List<string>();
                foreach (long ID in IDs)
                {
                    PurchaseOrder PO = PurchaseOrder.Finder.FindByID(ID);
                    if (PO != null && PO.POLines[0] != null && PO.POLines[0].POShiplines[0] != null && PO.POLines[0].POShiplines[0].Wh != null)
                    {
                        Warehouse wh = Warehouse.Finder.FindByID(PO.POLines[0].POShiplines[0].Wh.ID);
                        if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                        {
                            Organization org = CHelper.GetOrg(PO.POLines[0].ItemInfo.ItemCode, "");
                            if (org == PO.Org)
                            {

                                if (PO.DocType.Code == "PO22" && !DocLists.Contains(PO.DocNo))
                                {
                                    DocLists.Add(PO.DocNo);
                                }

                            }
                        }

                    }
                }
                string dataJson = JsonHelper.GetPurchaseOrderByClose(DocLists);
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseOrder.cancelByType");
                list.Add("body", dataJson);
                string sign = WDTChelper.GetWDTSign(list);
                list.Add("sign", sign);
                string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                string apireuslt = Tools.HttpPost(url, dataJson);
                WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "委外采购单关闭", apireuslt, dataJson, url, string.Join(",", DocLists.ToArray()));
                    sErrorItem += string.Join(",", DocLists.ToArray()) + res.message + ",";
                }
                else
                {
                    CHelper.InsertU9Log(true, "委外采购单关闭", apireuslt, dataJson, url, string.Join(",", DocLists.ToArray()));


                }

                if (!string.IsNullOrEmpty(sErrorItem))
                {
                    sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                    rtn.IsSuccess = false;
                    rtn.Msg = string.Format("委外采购单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                }
                else
                    rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 标准收货
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTReceivement(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    Receivement Rcv = Receivement.Finder.FindByID(ID);
                    if (Rcv!=null && Rcv.RcvLines[0].Wh!=null)
                    {
                        Warehouse wh = Warehouse.Finder.FindByID(Rcv.RcvLines[0].Wh.ID);
                        if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                        {
                            string dataJson = JsonHelper.GetReceivement(Rcv);
                            Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Purchase.upload");
                            list.Add("body", dataJson);
                            string sign = WDTChelper.GetWDTSign(list);
                            list.Add("sign", sign);
                            string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                            string apireuslt = Tools.HttpPost(url, dataJson);
                            WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                            if (res.status == 0)
                                CHelper.InsertU9Log(true, "标准收货单推送", apireuslt, dataJson, url, Rcv.DocNo);
                            else
                            {
                                CHelper.InsertU9Log(false, "标准收货单推送", res.message, dataJson, url, Rcv.DocNo);
                                sErrorItem += Rcv.DocNo + ",";
                            }
                            if (!string.IsNullOrEmpty(sErrorItem))
                            {
                                sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                rtn.IsSuccess = false;
                                rtn.Msg = string.Format("标准收货单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                            }
                            else
                                rtn.IsSuccess = true;
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 标准采购
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTPurchaseOrder(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                foreach (long ID in IDs)
                {
                    PurchaseOrder PO = PurchaseOrder.Finder.FindByID(ID);
                    if (PO != null && PO.POLines[0]!=null && PO.POLines[0].POShiplines[0]!=null && PO.POLines[0].POShiplines[0].Wh!=null)
                    {
                        Warehouse wh = Warehouse.Finder.FindByID(PO.POLines[0].POShiplines[0].Wh.ID);
                        if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                        {
                            Organization org = CHelper.GetOrg(PO.POLines[0].ItemInfo.ItemCode, "");
                            if (org == PO.Org)
                            {
                                if (PO.DocType.Code == "PO20")
                                {
                                    string dataJson = JsonHelper.GetPurchaseOrder(PO);
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseOrder.createOrder");
                                    list.Add("body", dataJson);
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = Tools.HttpPost(url, dataJson);
                                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                    if (res.status != 0)
                                    {
                                        CHelper.InsertU9Log(false, "标准采购单推送", apireuslt, dataJson, url, PO.DocNo);
                                        sErrorItem += PO.DocNo + res.message + ",";
                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(true, "标准采购单推送", apireuslt, dataJson, url, PO.DocNo);


                                    }

                                    if (!string.IsNullOrEmpty(sErrorItem))
                                    {
                                        sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                        rtn.IsSuccess = false;
                                        rtn.Msg = string.Format("标准采购单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                    }
                                    else
                                        rtn.IsSuccess = true;
                                }

                            }
                            else
                            {
                                rtn.IsSuccess = false;
                                rtn.Msg = "当前组织和料品维护的品牌组织不一致";
                            }
                        }
                        
                    }
                   
                   

                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 标准采购关闭
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson CloseWDTPurchaseOrder(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                List<string> DocLists = new List<string>();
                foreach (long ID in IDs)
                {
                    PurchaseOrder PO = PurchaseOrder.Finder.FindByID(ID);
                    if (PO != null && PO.POLines[0] != null && PO.POLines[0].POShiplines[0] != null && PO.POLines[0].POShiplines[0].Wh != null)
                    {
                        Warehouse wh = Warehouse.Finder.FindByID(PO.POLines[0].POShiplines[0].Wh.ID);
                        if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                        {
                            Organization org = CHelper.GetOrg(PO.POLines[0].ItemInfo.ItemCode, "");
                            if (org == PO.Org)
                            {

                                if (PO.DocType.Code == "PO20" && !DocLists.Contains(PO.DocNo))
                                {
                                    DocLists.Add(PO.DocNo);
                                }

                            }                           
                        }

                    }
                }
                string dataJson = JsonHelper.GetPurchaseOrderByClose(DocLists);
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("purchase.PurchaseOrder.cancelByType");
                list.Add("body", dataJson);
                string sign = WDTChelper.GetWDTSign(list);
                list.Add("sign", sign);
                string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                string apireuslt = Tools.HttpPost(url, dataJson);
                WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "标准采购单关闭", apireuslt, dataJson, url, string.Join(",", DocLists.ToArray()));
                    sErrorItem += string.Join(",", DocLists.ToArray()) + res.message + ",";
                }
                else
                {
                    CHelper.InsertU9Log(true, "标准采购单关闭", apireuslt, dataJson, url, string.Join(",", DocLists.ToArray()));


                }

                if (!string.IsNullOrEmpty(sErrorItem))
                {
                    sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                    rtn.IsSuccess = false;
                    rtn.Msg = string.Format("标准采购单【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                }
                else
                    rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }

        /// <summary>
        /// 更新出货单物流信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson UpdateShipInfo(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                foreach (long ID in IDs)
                {
                    Ship Rcv = Ship.Finder.FindByID(ID);
                    WdtWmsStockoutSalesQuerywithdetailRequest request = new WdtWmsStockoutSalesQuerywithdetailRequest();
                    request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain pager = new WdtWmsStockoutSalesQuerywithdetailRequest.PagerDomain();
                    pager.PageNo = 1;
                    pager.PageSize = 100;
                    request.Pager_ = pager;
                    WdtWmsStockoutSalesQuerywithdetailRequest.ParamsDomain paramsDomain = new WdtWmsStockoutSalesQuerywithdetailRequest.
                        ParamsDomain();
                    //paramsDomain.StartTime = "2022-04-01 01:00:00";
                    //paramsDomain.EndTime = "2022-04-01 02:00:00";
                    //paramsDomain.Status = "110";
                    paramsDomain.StockoutNo = Rcv.DocNo;
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
                        if (response.Status == 0)
                        {
                            CHelper.InsertU9Log(true, "销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                            foreach (WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item in response.Data.Order)
                            {
                                if (Rcv != null)
                                {
                                    using (ISession session = Session.Open())
                                    {
                                        Rcv.DescFlexField.PrivateDescSeg3 = item.LogisticsNo;
                                        Rcv.DescFlexField.PrivateDescSeg4 = item.LogisticsName;
                                        session.Commit();
                                    }
                                }
                            }
                            rtn.IsSuccess = true;
                        }
                        else
                        {
                            CHelper.InsertU9Log(false, "销售出库单查询", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(request), WDTChelper.qmapiUrl, "");
                        }

                    }
                    catch (TopException e)
                    {
                        rtn.IsSuccess = false;
                        rtn.Msg = e.ErrorMsg;
                        Console.WriteLine(e.ErrorMsg);
                    }




                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 出货计划-原始订单
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDTOrIginalOrder(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";
                List<ShipPlanLine> listShipLine=new List<ShipPlanLine>();
                List<ShipPlanLine> listRepeatShipLine = new List<ShipPlanLine>();
                foreach (long ID in IDs)
                {
                    ShipPlan shipPlan = ShipPlan.Finder.FindByID(ID);
                    //int num = 1;
                    //string trate = string.Empty;
                    //foreach (ShipPlanLine shipplanline in shipPlan.ShipPlanLines)
                    //{
                    //    //具体的判断字段待补充
                    //    listShipLine = shipPlan.ShipPlanLines.Cast<ShipPlanLine>().Where(w => w.FillQtyTU == shipplanline.FillQtyTU && w.DescFlexField.PrivateDescSeg1 == shipplanline.DescFlexField.PrivateDescSeg1 && w.DescFlexField.PrivateDescSeg4 == shipplanline.DescFlexField.PrivateDescSeg4).OrderBy(p => p.DocLineNo).ToList();
                    //    if (listRepeatShipLine!= listShipLine)
                    //    {
                    //        //调用接口
                    //        trate = "00" + num;
                    //        string dataJson = JsonHelper.GetShipPlan(shipPlan, listShipLine,trate);
                    //        Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.RawTrade.pushSelf");
                    //        list.Add("body", dataJson);
                    //        string sign = WDTChelper.GetWDTSign(list);
                    //        list.Add("sign", sign);
                    //        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    //        string apireuslt = Tools.HttpPost(url, dataJson);
                    //        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                    //        if (res.status == 0)
                    //            CHelper.InsertU9Log(true, "原始订单推送", apireuslt, dataJson, url, shipPlan.DocNo);
                    //        else
                    //        {
                    //            CHelper.InsertU9Log(false, "原始订单推送", res.message, dataJson, url, shipPlan.DocNo);
                    //            sErrorItem += shipPlan.DocNo +res.message+ ",";
                    //        }
                    //        if (!string.IsNullOrEmpty(sErrorItem))
                    //        {
                    //            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                    //            rtn.IsSuccess = false;
                    //            rtn.Msg = string.Format("原始订单推送【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                    //        }
                    //        else
                    //            rtn.IsSuccess = true;
                    //        num++;
                    //    }
                    //    listRepeatShipLine = listShipLine;
                    //}
                    if (shipPlan.DocType.Code =="SH2")
                    {
                        if (shipPlan != null && shipPlan.ShipPlanLines[0].WH != null)
                        {
                            Warehouse wh = Warehouse.Finder.FindByID(shipPlan.ShipPlanLines[0].WH.ID);
                            if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                            {
                                string trate = string.Empty;
                                string dataJson = JsonHelper.GetShipPlan(shipPlan);
                                Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.RawTrade.pushSelf");
                                list.Add("body", dataJson);
                                string sign = WDTChelper.GetWDTSign(list);
                                list.Add("sign", sign);
                                string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                string apireuslt = Tools.HttpPost(url, dataJson);
                                WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                if (res.status == 0)
                                    CHelper.InsertU9Log(true, "原始订单推送", apireuslt, dataJson, url, shipPlan.DocNo);
                                else
                                {
                                    CHelper.InsertU9Log(false, "原始订单推送", res.message, dataJson, url, shipPlan.DocNo);
                                    sErrorItem += shipPlan.DocNo + res.message + ",";
                                }
                                if (!string.IsNullOrEmpty(sErrorItem))
                                {
                                    sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                    rtn.IsSuccess = false;
                                    rtn.Msg = string.Format("原始订单推送【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                }
                                else
                                    rtn.IsSuccess = true;
                            }
                        }
                    }
                 
                    



                }
                 
                

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 销售订单-原始订单
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendWDSOTOrIginalOrder(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {

                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string sErrorItem = "";       
                foreach (long ID in IDs)
                {
                    SO so = SO.Finder.FindByID(ID);        
                    if (so != null)
                    {
                        Organization org = CHelper.GetOrg(so.SOLines[0].ItemInfo.ItemCode, "");
                        if (org == so.Org) 
                        {
                            if (so.DocType.Code == "SO9")
                            {
                               // 1.先找出订单所有的线上仓，一个线上仓对应一张原始订单
                               List<string> whs=new List<string>();
                                foreach (SOLine soline in so.SOLines)
                                {
                                    if (soline.SOShiplines[0]!=null && soline.SOShiplines[0].WH!=null)
                                    {
                                        Warehouse wh = Warehouse.Finder.FindByID(soline.SOShiplines[0].WH.ID);
                                        if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                        {
                                            if (!whs.Contains(wh.Code))
                                            {
                                                whs.Add(wh.Code);
                                            }
                                        }
                                    }
                                }
                                // 2.循环行数据生单
                                foreach (string whcode in whs)
                                {
                                    string dataJson = JsonHelper.GetOrginalSO(so,whcode);
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.RawTrade.pushSelf2");
                                    list.Add("body", dataJson);
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = Tools.HttpPost(url, dataJson);
                                    WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                    if (res.status == 0)
                                        CHelper.InsertU9Log(true, "原始订单推送", apireuslt, dataJson, url, so.DocNo);
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "原始订单推送", res.message, dataJson, url, so.DocNo);
                                        sErrorItem += so.DocNo + res.message + ",";
                                    }
                                  
                                }
                                if (!string.IsNullOrEmpty(sErrorItem))
                                {
                                    sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                    rtn.IsSuccess = false;
                                    rtn.Msg = string.Format("原始订单推送【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                }
                                else
                                    rtn.IsSuccess = true;
                                //if (so != null && so.SOLines[0].SOShiplines[0] != null && so.SOLines[0].SOShiplines[0].WH != null)
                                //{
                                //    Warehouse wh = Warehouse.Finder.FindByID(so.SOLines[0].SOShiplines[0].WH.ID);
                                //    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                                //    {
                                //        string dataJson = JsonHelper.GetOrginalSO(so);
                                //        Dictionary<string, string> list = WDTChelper.GetWDTInfo("sales.RawTrade.pushSelf2");
                                //        list.Add("body", dataJson);
                                //        string sign = WDTChelper.GetWDTSign(list);
                                //        list.Add("sign", sign);
                                //        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                //        string apireuslt = Tools.HttpPost(url, dataJson);
                                //        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                //        if (res.status == 0)
                                //            CHelper.InsertU9Log(true, "原始订单推送", apireuslt, dataJson, url, so.DocNo);
                                //        else
                                //        {
                                //            CHelper.InsertU9Log(false, "原始订单推送", res.message, dataJson, url, so.DocNo);
                                //            sErrorItem += so.DocNo + res.message + ",";
                                //        }
                                //        if (!string.IsNullOrEmpty(sErrorItem))
                                //        {
                                //            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                //            rtn.IsSuccess = false;
                                //            rtn.Msg = string.Format("原始订单推送【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
                                //        }
                                //        else
                                //            rtn.IsSuccess = true;
                                //    }
                                //}
                            }
                        }
                        else
                        {
                            rtn.IsSuccess = false;
                            rtn.Msg = "当前组织和料品品牌维护的组织不一致";
                        }
                    }
                    





                }



            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 失败记录重新推送方法
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson SendFaidRecord(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                CommonSV commonSV = new CommonSV();
                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string dataJson = string.Empty;
                string sql = string.Empty;
                string errormsg = string.Empty;
                foreach (long ID in IDs)
                {
                    FailInfo fail = FailInfo.Finder.FindByID(ID);
                    if (fail != null && fail.ISSuccess == 2)
                    {
                        //if (fail.DocType== "委外收货创建")
                        //{
                        //    commonSV.CreatePMRcv(fail.Json);
                        //}
                        //switch (fail.DocType)
                        //{
                        //    case "委外收货创建":
                        //        commonSV.CreatePMRcv(fail.Json);
                        //        break;
                        //    case "杂收单创建":
                        //        commonSV.CreateMiscRcvTrans(fail.Json);
                        //        break;
                        //}
                        if (fail.DocType.Contains("创建"))
                        {
                            string posturl = string.Empty;
                            string apirEEEeuslt = string.Empty;
                            posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                            dataJson = fail.Json;
                            apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                            if (rtns.IsSuccess)
                            {
                                sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", fail.ID);
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            }
                            else
                            {
                                sql = string.Format(@"update Cust_FailInfo set Reason='{1}'  where ID={0}", fail.ID, rtns.Msg.Substring(0,100));
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            }
                        }
                        else if (fail.DocType.Contains("更新"))
                        {
                            switch (fail.DocType)
                            {
                                case "杂收单更新":
                                    UpdateRcvTrans(fail.Json,fail.ID);
                                    break;
                                case "杂发单更新":
                                    UpdateMiscShip(fail.Json, fail.ID);
                                    break;
                                case "委外退货更新":
                                    UpdatePurRtnRtn(fail.Json, fail.ID);
                                    break;
                                case "调入单更新":
                                    UpdateTransferIn(fail.Json, fail.ID);
                                    break;
                                default:
                                    break;
                            }
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        /// <summary>
        /// 合并生单SO
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public RtnDataJson MergerSalesOrderCreate4SOShip(string jsondata)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                DataSet ds = new DataSet();
                DataSet dsq = new DataSet();
                List<string> SODocList = new List<string>();
                List<string> ShipDocList = new List<string>();
                List<long> IDs = JsonConvert.DeserializeObject<List<long>>(jsondata);
                string dataJson = string.Empty;
                string sql = string.Empty;
                string errormsg = string.Empty;
                string sodoc = string.Empty;
                string IDList = string.Join(",", IDs?.ToArray());
                //select distinct CusCode from sgy_saleship Where convert(varchar(10),ShipDate,120) = Cast(getdate() as Date)
                sql = string.Format("select distinct CusCode,Org from sgy_saleship Where ID in({0}) and Status=0", IDList);
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    RtnDataJson rtns = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sql = string.Format(" select CusCode,WHCode,ItemCode,sum(Amount)数量,SellPrice,TradeFrom,Org from sgy_saleship where ID in({0}) and CusCode ='{1}' and Org= {2}  group by Org, CusCode,WHCode,ItemCode,SellPrice ,TradeFrom", IDList, row["CusCode"].ToString(), Convert.ToInt64(row["Org"]));
                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out dsq);
                        if ((dsq != null) && (dsq.Tables.Count > 0) && (dsq.Tables.Count >= 1 && dsq.Tables[0].Rows.Count > 0))
                        {
                            sql = string.Format(@"Select so from sgy_saleship where ID in({0}) and so<>''", string.Join(",", IDs.ToArray()));
                            sodoc = CHelper.GetStrInfo(sql);
                            if (!string.IsNullOrEmpty(sodoc))
                            {
                                SO SO = SO.Finder.Find("DocNo=@DocNo and Org=@Org" ,new OqlParam("DocNo",sodoc),new OqlParam("Org", Convert.ToInt64(row["Org"])));
                                if (SO != null)
                                {
                                    dataJson = JsonHelper.MergeShipJson(SO);
                                    posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                                    apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                                    rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                                    if (rtns.IsSuccess)
                                    {
                                        ShipDocList.Add(rtns.DocNo);
                                        CHelper.InsertU9Log(true, "合并销售明细生成出货单", rtns.Msg, dataJson, "", rtns.DocNo);
                                        sql = string.Format(@"update sgy_saleship set Ship='{2}'  where ID in({3}) and CusCode='{0}' and Org={1} and SO<>''", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]), rtns.DocNo, IDList);
                                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "合并销售明细生成出货单", rtns.Msg, dataJson, posturl, "");
                                        rtn.IsSuccess = false;
                                        errormsg += rtns.Msg;
                                    }
                                }
                            }
                            else
                            {
                                Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
                                dataJson = JsonHelper.MergeSOJson(dsq, org, row["CusCode"].ToString());
                                posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                                apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);                             
                                 rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                                if (rtns.IsSuccess)
                                {
                                    SODocList.Add(rtns.DocNo);
                                    CHelper.InsertU9Log(true, "合并销售明细生成销售订单", rtns.Msg, dataJson, "", rtns.DocNo);
                                    sql = string.Format(@"update sgy_saleship set  Status=1,SO='{2}'  where ID in ({3}) and  CusCode='{0}' and Org={1}", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]), rtns.DocNo, IDList);
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    //生成销售订单成功后生成对应出货单
                                    SO SO = SO.Finder.FindByID(rtns.ID);
                                    if (SO != null)
                                    {
                                        dataJson = JsonHelper.MergeShipJson(SO);
                                        posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                                        apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                                        rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                                        if (rtns.IsSuccess)
                                        {
                                            ShipDocList.Add(rtns.DocNo);
                                            CHelper.InsertU9Log(true, "合并销售明细生成出货单", rtns.Msg, dataJson, "", rtns.DocNo);
                                            sql = string.Format(@"update sgy_saleship set Ship='{2}'  where ID in ({3}) and  CusCode='{0}' and Org={1}", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]), rtns.DocNo, IDList);
                                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                        }
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "合并销售明细生成出货单", rtns.Msg, dataJson, posturl, "");
                                            rtn.IsSuccess = false;
                                            errormsg += rtns.Msg;
                                        }
                                    }


                                }
                                else
                                {
                                    CHelper.InsertU9Log(false, "合并销售明细生成销售订单", rtns.Msg, dataJson, posturl, "");
                                    rtn.IsSuccess = false;
                                    errormsg += rtns.Msg;
                                }
                            }
                          
                        }
                    }
                    if (!string.IsNullOrEmpty(errormsg))
                    {
                        rtn.Msg = errormsg;
                    }
                    if (SODocList!=null)
                    {
                        rtn.IsSuccess = true;
                        rtn.DocNo ="生单成功，销售订单号： "+ string.Join(",", SODocList.ToArray());
                    }
                    if (ShipDocList!=null)
                    {
                        rtn.IsSuccess = true;
                        rtn.DocNo += "," + "出货订单号：" + string.Join(",", ShipDocList.ToArray());
                    }
                } 
            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
            }
            return rtn;
        }
        
        /// <summary>
        /// 更新杂收单方法
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string UpdateRcvTrans(string json,long ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(json);
            if (docDto != null)
            {
                string sql = string.Empty;
                MiscRcvTrans Rcv = null;
                string Msg = string.Empty;
                Rcv = MiscRcvTrans.Finder.FindByID(docDto.ID);
                Organization org = Organization.FindByCode(docDto.OrgCode);

                if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approving)
                {
                    using (ISession session = Session.Open())
                    {
                        foreach (MiscRcvTransL rcvline in Rcv.MiscRcvTransLs)
                        {
                            foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                            {
                                if (rcvline.DocLineNo == Deatail.DocLineNo)
                                {
                                    rcvline.CostUOMQty = Deatail.Amount;
                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                    if (wh != null)
                                    {
                                        rcvline.Wh = wh;
                                        rcvline.Wh.ID = wh.ID;
                                        rcvline.Wh.Code = wh.Code;
                                    }

                                }
                            }
                        }
                        session.Commit();
                    }
                    using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                    {
                        try
                        {
                            List<long> lstID = new List<long>();
                            lstID.Add(Rcv.ID);
                            Msg = ApproveMiscRcv(lstID);
                            scope.Commit();
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                        catch (Exception ex)
                        {
                            Msg = ex.Message;
                            scope.Rollback();
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, Msg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            // throw new Exception(ex.Message);
                        }
                    }
                }
                //Rcv = MiscRcvTrans.Finder.FindByID(docDto.ID);
                //if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approved)
                //{
                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                //}
                //else
                //{
                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, Msg);
                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                //}
            }

            return "";
        }

        /// <summary>
        /// 更新杂发单方法
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string UpdateMiscShip(string json,long ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(json);
            if (docDto != null)
            {
                MiscShipment Rcv = null;
                string sql = string.Empty;
                string Msg = string.Empty;
                Rcv = MiscShipment.Finder.FindByID(docDto.ID);
                Organization org = Organization.FindByCode(docDto.OrgCode);

                if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approving)
                {
                    using (ISession session = Session.Open())
                    {
                        foreach (MiscShipmentL rcvline in Rcv.MiscShipLs)
                        {
                            foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                            {
                                if (rcvline.DocLineNo == Deatail.DocLineNo)
                                {
                                    rcvline.CostUOMQty = Deatail.Amount;
                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                    if (wh != null)
                                    {
                                        rcvline.Wh = wh;
                                        rcvline.Wh.ID = wh.ID;
                                        rcvline.Wh.Code = wh.Code;
                                    }

                                }
                            }
                        }
                        session.Commit();
                    }
                    using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                    {
                        try
                        {
                            List<long> lstID = new List<long>();
                            lstID.Add(Rcv.ID);
                            Msg = ApproveMiscShip(lstID);
                            scope.Commit();
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                        catch (Exception ex)
                        {
                            Msg = ex.Message;
                            scope.Rollback();
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, Msg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            // throw new Exception(ex.Message);
                        }
                    }
                }
                //Rcv = MiscShipment.Finder.FindByID(docDto.ID);
                //if (Rcv != null && Rcv.Status == InvDoc.Enums.INVDocStatus.Approved)
                //{
                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                //}
                //else
                //{
                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, Msg);
                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                //}
            }
            return "";
        }

        /// <summary>
        /// 更新委外退货方法
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string UpdatePurRtnRtn(string json,long ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(json);
            if (docDto != null)
            {
                Receivement Rcv = null;
                string ErrMsg = string.Empty;
                string sql = string.Empty;
                Rcv = Receivement.Finder.FindByID(docDto.ID);
                Organization org = Organization.FindByCode(docDto.OrgCode);

                if (Rcv != null && Rcv.Status == RcvStatusEnum.Approving)
                {
                    using (ISession session = Session.Open())
                    {
                        foreach (RcvLine rcvline in Rcv.RcvLines)
                        {
                            foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                            {
                                if (rcvline.DocLineNo == Deatail.DocLineNo)
                                {
                                    rcvline.RejectQtyTU = Deatail.Amount;
                                    rcvline.RtnFillQtyTU = Deatail.Amount;
                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                    if (wh != null)
                                    {
                                        rcvline.Wh = wh;
                                        rcvline.Wh.ID = wh.ID;
                                        rcvline.Wh.Code = wh.Code;
                                    }

                                }
                            }
                        }
                        session.Commit();
                    }
                    using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                    {
                        try
                        {
                            List<long> lstID = new List<long>();
                            lstID.Add(Rcv.ID);
                            RcvBatchApprovedBPProxy appProxy = new RcvBatchApprovedBPProxy();
                            appProxy.ActType = 8;
                            appProxy.DocHeadIDs = lstID;

                            List<UFIDA.U9.PM.Rcv.PMDocErrListDTOData> appErrors = appProxy.Do();
                            if (appErrors.Count > 0 && !string.IsNullOrEmpty(appErrors[0].ErrMsg))
                            {
                                ErrMsg = appErrors[0].ErrMsg;
                                sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, ErrMsg);
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                // CHelper.InsertU9Log(false, "委外退货审核", appErrors[0].ErrMsg, Rcv.DocNo, "");
                            }
                            else
                            {
                                sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            }
                            scope.Commit();
                           
                        }
                        catch (Exception ex)
                        {
                            scope.Rollback();
                            // throw new Exception(ex.Message);
                        }
                    }
                }
                //Rcv = Receivement.Finder.FindByID(docDto.ID);
                //if (Rcv != null && Rcv.Status == RcvStatusEnum.Closed)
                //{
                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                //}
                //else
                //{
                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, ErrMsg);
                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                //}
            }
            return "";
        } /// <summary>
          /// 更新调入单方法
          /// </summary>
          /// <param name="json"></param>
          /// <returns></returns>
        public string UpdateTransferIn(string json,long ID)
        {
            try
            {
                string sql = string.Empty;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                UpdateTransferDocDto docDto = serializer.Deserialize<UpdateTransferDocDto>(json);
                if (docDto != null)
                {
                    TransferIn Rcv = TransferIn.Finder.FindByID(docDto.ID);
                    Organization org = Organization.FindByCode(docDto.OrgCode);

                    if (Rcv != null && Rcv.Status == TransInStatus.Approving)
                    {
                        using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
                        {
                            try
                            {

                                List<CBO.Pub.Controller.CommonArchiveDataDTOData> result = new List<CBO.Pub.Controller.CommonArchiveDataDTOData>();
                                CBO.Pub.Controller.CommonArchiveDataDTOData commonArchiveDataDTOData = new CBO.Pub.Controller.CommonArchiveDataDTOData();
                                commonArchiveDataDTOData.ID = Rcv.ID;
                                result.Add(commonArchiveDataDTOData);
                                TransferInBatchApproveSRVProxy approveProxy = new TransferInBatchApproveSRVProxy()
                                {
                                    ApprovedBy = Context.LoginUser,
                                    ApprovedOn = DateTime.Now,
                                    TargetOrgCode = Context.LoginOrg.Code,
                                    TargetOrgName = Context.LoginOrg.Name,
                                    DocList = result
                                };
                                approveProxy.Do();
                                scope.Commit();
                                sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", ID);
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            }
                            catch (Exception ex)
                            {
                                scope.Rollback();
                                sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", ID, ex.Message);
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                CHelper.InsertU9Log(false, "调入单审核", ex.Message, Rcv.DocNo, "");
                            }
                        }
                    }
                    //Rcv = Receivement.Finder.FindByID(docDto.ID);
                    //if (Rcv!=null && Rcv.Status == RcvStatusEnum.Closed)
                    //{
                    //	sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                    //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                    //}
                    //else
                    //{
                    //	sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
                    //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                    //}
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "调入单更新", ex.Message, json, "");
                throw new Exception(ex.Message);
            }
            return "";
        }
        /// <summary>
        /// 审核杂发单
        /// </summary>
        /// <param name="lstID"></param>
        private string ApproveMiscShip(List<long> lstID)
        {
            string Msg = string.Empty;
            try
            {

                if (lstID == null || (lstID != null && lstID.Count <= 0))
                    throw new Exception("杂发单ID不能为空！");
                foreach (long ID in lstID)
                {
                    List<CommonArchiveDataDTOData> lst = new List<CommonArchiveDataDTOData>();
                    CommonArchiveDataDTOData misdto = new CommonArchiveDataDTOData();
                    misdto.ID = ID;
                    lst.Add(misdto);

                    UFIDA.U9.ISV.MiscShipISV.Proxy.CommonApproveMiscShipSVProxy appx = new ISV.MiscShipISV.Proxy.CommonApproveMiscShipSVProxy();
                    appx.MiscShipmentKeyList = lst;
                    List<ISV.MiscShipISV.IC_MiscShipmentDTOData> SS = appx.Do();

                }
            }
            catch (Exception EX)
            {
                Msg = EX.Message;
            }
            return Msg;
        }
    }
}
