using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFIDA.U9.Base.Organization;
using UFIDA.U9.CBO.SCM.Warehouse;
using UFIDA.U9.InvDoc.TransferIn;
using UFIDA.U9.LH.LHPubBP.Model;
using UFSoft.UBF.Business;
using UFIDA.U9.InvDoc.MiscRcv;
using QimenCloud.Api.scene3ldsmu02o9.Request;
using QimenCloud.Api.scene3ldsmu02o9.Response;
using QimenCloud.Api;
using Top.Api;
using UFIDA.U9.LH.LHPubBP.Utility;
using System.Data;
using UFSoft.UBF.Util.DataAccess;
using UFIDA.U9.CBO.Pub.Controller;
using System.Web.Script.Serialization;
using UFSoft.UBF.Transactions;
using UFIDA.U9.InvDoc.MiscShip;
using UFSoft.UBF.PL;
using UFIDA.U9.ISV.TransferInISV.Proxy;
using UFIDA.U9.Base;
using UFIDA.U9.SM.SO;
using UFIDA.U9.SM.RMA;
using UFIDA.U9.LH.LHPubBE.FailInfoBE;
using Top.Api.Domain;
using FastJSON;
using UFIDA.U9.PM.Rtn;
using UFIDA.U9.PM.PO;
using System.IO;

namespace UFIDA.U9.LH.LHPubBP.Option
{
    /// <summary>
    /// 库存模块
    /// </summary>
    public class InventorySV
    {
        #region 调入
        /// <summary>
        /// U9调入单生成旺店通其他入库、其他出库
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
                    if (transfer != null && transfer.TransInLines[0] != null)
                    {
                        Organization org = CHelper.GetOrg(transfer.TransInLines[0].ItemInfo.ItemCode, "");
                        if (org == transfer.Org)
                        {
                            if (transfer != null && transfer.TransInLines[0].TransInOrg != transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
                            {
                                if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
                                {
                                    Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                    else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOutInTransferJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.createOrder");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)//成功了是否需要记录日志
                                            CHelper.InsertU9Log(true, "调入单推送外仓调整入库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送外仓调整入库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("调入单推送外仓调整入库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
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
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                    else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOutoutTransferJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.createOrder");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)//成功了是否需要记录日志
                                            CHelper.InsertU9Log(true, "调入单推送外仓调整出库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送外仓调整出库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("调入单推送外仓调整出库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
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
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                    else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOutInTransferJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.createOrder");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)//成功了是否需要记录日志
                                            CHelper.InsertU9Log(true, "调入单推送外仓调整入库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送外仓调整入库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("调入单推送外仓调整入库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
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
                                    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                    else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                    {
                                        string dataJson = JsonHelper.GetStockOutoutTransferJson(transfer);
                                        Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.createOrder");
                                        list.Add("body", dataJson);
                                        string sign = WDTChelper.GetWDTSign(list);
                                        list.Add("sign", sign);
                                        string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                        string apireuslt = Tools.HttpPost(url, dataJson);
                                        WDTRes res = JsonConvert.DeserializeObject<WDTRes>(apireuslt);
                                        if (res.status == 0)//成功了是否需要记录日志
                                            CHelper.InsertU9Log(true, "调入单推送外仓调整出库", apireuslt, dataJson, url, transfer.DocNo);
                                        else
                                        {
                                            CHelper.InsertU9Log(false, "调入单推送外仓调整出库", res.message, dataJson, url, transfer.DocNo);
                                            sErrorItem += transfer.DocNo + res.message + ",";
                                        }
                                        if (!string.IsNullOrEmpty(sErrorItem))
                                        {
                                            sErrorItem = sErrorItem.TrimEnd(new char[] { ',' });
                                            rtn.IsSuccess = false;
                                            rtn.Msg = string.Format("调入单推送外仓调整出库【{0}】,推送失败,具体原因，请查看程序日志！", sErrorItem);
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
            return rtn;
        }

        /// <summary>
        /// 获取旺店通调拨出入库单调入单更新数据
        /// </summary>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetUpdateTransferIn()
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                string docTypes = CHelper.GetDefineValueUrl("CustParam", "007");
                docTypes = docTypes.Replace(",", "','");
                sql = string.Format(@" SELECT distinct  a.ID,a.DocNo  FROM dbo.InvDoc_TransferIn AS a
INNER JOIN dbo.InvDoc_TransInDocType AS b ON a.TransInDocType = b.ID
INNER JOIN dbo.InvDoc_TransInLine AS c ON a.ID = c.TransferIn
INNER JOIN dbo.InvDoc_TransInSubLine AS d ON c.ID = d.TransInLine
WHERE a.Status = 1 AND b.Code IN ('{0}') and (c.DescFlexSegments_PrivateDescSeg2 <> 'True' and d.DescFlexSegments_PrivateDescSeg1 <> 'True')", docTypes);
                //                sql = string.Format(@" SELECT TOP 100 a.ID,a.DocNo FROM dbo.InvDoc_TransferIn AS a
                //INNER JOIN dbo.InvDoc_TransInDocType AS b ON a.TransInDocType = b.ID
                //WHERE a.Status = 1 AND b.Code IN ('TransIn009','TransIn010')");
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
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                    list.Add("page_size", "100");
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
                                                                if (detail.in_num>0 && tranLine.DescFlexSegments.PrivateDescSeg2 != "True")
                                                                {
                                                                    if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                    {
                                                                        if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.in_num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                            //  tranLine.TransInWh.Code = order.wa
                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tranLine.ItemInfo.ItemCode == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.in_num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                            //  tranLine.TransInWh.Code = order.wa
                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                }
                                                               
                                                              

                                                            }

                                                        }

                                                    }

                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "其它入库业务单查询", apireuslt, json, url);
                                    }

                                }
                                else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
                                    List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
                                    OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.outer_in_no = transfer.DocNo;
                                    req.status = "50";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "100");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    OuterInquerywithdetailRes res = JsonConvert.DeserializeObject<OuterInquerywithdetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                string dataJson = string.Empty;
                                                foreach (Model.OuterInOrderDto order in res.data.order)
                                                {
                                                    if (order.outer_in_no == transfer.DocNo)
                                                    {
                                                        // dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (OuterInOrderDeatail detail in order.detail_list)
                                                            {
                                                                if (detail.num>0 && tranLine.DescFlexSegments.PrivateDescSeg2 != "True")
                                                                {
                                                                    if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                    {
                                                                        if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tranLine.ItemInfo.ItemCode == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                }
                                                                
                                                              
                                                            }

                                                        }

                                                    }

                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "外仓调整入库单查询", apireuslt, json, url);
                                    }
                                }

                            }
                            if (transfer.DocType.Code == "TransIn009" && transfer.TransInLines != null && transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                                                        if (detail.out_num>0 && lnSubLine.DescFlexSegments.PrivateDescSeg1 != "True")
                                                                        {
                                                                            if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                            {
                                                                                if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.out_num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;
                                                                                    //  tranLine.TransInWh.Code = order.wa

                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";

                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.out_num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;
                                                                                    //  tranLine.TransInWh.Code = order.wa

                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";

                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
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
                                        CHelper.InsertU9Log(false, "其它出库业务单查询", apireuslt, json, url);
                                    }
                                }
                                else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
                                    List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
                                    OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.outer_out_no = transfer.DocNo;
                                    req.status = "50";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "100");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    OuterInquerywithdetailRes res = JsonConvert.DeserializeObject<OuterInquerywithdetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                string dataJson = string.Empty;
                                                foreach (Model.OuterInOrderDto order in res.data.order)
                                                {
                                                    if (order.outer_out_no == transfer.DocNo)
                                                    {
                                                        // dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
                                                            {
                                                                if (tranLine.ID == lnSubLine.TransInLine.ID)
                                                                {
                                                                    foreach (OuterInOrderDeatail detail in order.detail_list)
                                                                    {
                                                                        if (detail.num>0 && lnSubLine.DescFlexSegments.PrivateDescSeg1 != "True")
                                                                        {
                                                                            if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                            {
                                                                                if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";

                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";

                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                        }
                                                                       
                                                                    }
                                                                }

                                                            }

                                                        }

                                                    }

                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "外仓调整出库单查询", apireuslt, json, url);
                                    }
                                }
                            }
                        }
                        else if (transfer != null && transfer.TransInLines[0].TransInOrg == transfer.TransInLines[0].TransInSubLines[0].TransOutOrg)
                        {
                            if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                                                if (detail.in_num>0 && tranLine.DescFlexSegments.PrivateDescSeg2 != "True")
                                                                {
                                                                    if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                    {
                                                                        if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.in_num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";

                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tranLine.ItemInfo.ItemCode == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.in_num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";

                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                }
                                                               
                                                            }

                                                        }

                                                    }

                                                }

                                                session.Commit();
                                            }
                                        }


                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "其他业务入库单查询", apireuslt, json, url);
                                    }
                                }
                                else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
                                    List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
                                    OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.outer_in_no = transfer.DocNo;
                                    req.status = "50";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "100");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    OuterInquerywithdetailRes res = JsonConvert.DeserializeObject<OuterInquerywithdetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                string dataJson = string.Empty;
                                                foreach (Model.OuterInOrderDto order in res.data.order)
                                                {
                                                    if (order.outer_in_no == transfer.DocNo)
                                                    {
                                                        // dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (OuterInOrderDeatail detail in order.detail_list)
                                                            {
                                                                if (detail.num>0 && tranLine.DescFlexSegments.PrivateDescSeg2 != "True")
                                                                {
                                                                    if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                    {
                                                                        if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                            //  tranLine.TransInWh.Code = order.wa

                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tranLine.ItemInfo.ItemCode == detail.goods_no)
                                                                        {
                                                                            tranLine.StoreUOMQty = detail.num;
                                                                            tranLine.CostUOMQty = tranLine.StoreUOMQty;
                                                                            tranLine.DescFlexSegments.PrivateDescSeg2 = "True";
                                                                            //  tranLine.TransInWh.Code = order.wa

                                                                            if (tranLine.TransInBins != null && tranLine.TransInBins.Count > 0)
                                                                                tranLine.TransInBins[0].StoreUOMQty = tranLine.StoreUOMQty;
                                                                        }
                                                                    }
                                                                }
                                                               
                                                            }

                                                        }

                                                    }

                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "外仓调整入库单查询", apireuslt, json, url);
                                    }
                                }

                            }
                            if (transfer.DocType.Code == "TransIn010" && transfer.TransInLines != null && transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null)
                            {
                                Warehouse wh = Warehouse.Finder.FindByID(transfer.TransInLines[0].TransInSubLines[0].TransOutWh.ID);
                                if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 != "10")
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
                                                                        if (detail.out_num>0 && lnSubLine.DescFlexSegments.PrivateDescSeg1 != "True")
                                                                        {
                                                                            if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                            {
                                                                                if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.out_num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;

                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";

                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.out_num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;

                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";

                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                        }
                                                                       
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "其他业务出库单查询", apireuslt, json, url);
                                    }
                                }
                                else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10" && wh.DescFlexField.PrivateDescSeg3 == "10")
                                {
                                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
                                    List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
                                    OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
                                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                                    //req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    req.outer_out_no = transfer.DocNo;
                                    req.status = "50";
                                    listreq.Add(req);
                                    string json = JsonConvert.SerializeObject(listreq);
                                    list.Add("body", json);
                                    //分页数据
                                    list.Add("page_size", "100");
                                    list.Add("page_no", "0");
                                    list.Add("calc_total", "1");
                                    string sign = WDTChelper.GetWDTSign(list);
                                    list.Add("sign", sign);
                                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                                    string apireuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(url, json);
                                    OuterInquerywithdetailRes res = JsonConvert.DeserializeObject<OuterInquerywithdetailRes>(apireuslt);
                                    if (res.status == 0)
                                    {
                                        if (res.data != null && res.data.order.Count > 0)
                                        {
                                            using (ISession session = Session.Open())
                                            {
                                                //foreach (TransInLine tranLine in transfer.TransInLines)
                                                //{
                                                string dataJson = string.Empty;
                                                foreach (Model.OuterInOrderDto order in res.data.order)
                                                {
                                                    if (order.outer_out_no == transfer.DocNo)
                                                    {
                                                        // dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                                        foreach (TransInLine tranLine in transfer.TransInLines)
                                                        {
                                                            foreach (TransInSubLine lnSubLine in tranLine.TransInSubLines)
                                                            {
                                                                if (tranLine.ID == lnSubLine.TransInLine.ID)
                                                                {
                                                                    foreach (OuterInOrderDeatail detail in order.detail_list)
                                                                    {
                                                                        if (detail.num>0 && lnSubLine.DescFlexSegments.PrivateDescSeg1 != "True")
                                                                        {
                                                                            if (!string.IsNullOrEmpty(tranLine.DescFlexSegments.PrivateDescSeg4))
                                                                            {
                                                                                if (tranLine.DescFlexSegments.PrivateDescSeg4 == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;

                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";
                                                                                    //  tranLine.TransInWh.Code = order.wa
                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lnSubLine.ItemInfo.ItemCode == detail.goods_no)
                                                                                {
                                                                                    lnSubLine.StoreUOMQty = detail.num;
                                                                                    lnSubLine.CostUOMQty = lnSubLine.StoreUOMQty;

                                                                                    lnSubLine.PriceUomQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.TransInSUQty = lnSubLine.StoreUOMQty;
                                                                                    lnSubLine.DescFlexSegments.PrivateDescSeg1 = "True";
                                                                                    //  tranLine.TransInWh.Code = order.wa
                                                                                    if (lnSubLine.TransInBins != null && lnSubLine.TransInBins.Count > 0)
                                                                                        lnSubLine.TransInBins[0].StoreUOMQty = lnSubLine.StoreUOMQty;
                                                                                }
                                                                            }
                                                                        }
                                                                      
                                                                    }
                                                                }

                                                            }


                                                        }

                                                    }

                                                }

                                                session.Commit();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        CHelper.InsertU9Log(false, "外仓调整出库单查询", apireuslt, json, url);
                                    }
                                }
                            }
                        }
                       // string DataJson = JsonHelper.GetWDTUpdateTransferIn(transfer);
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
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
                CHelper.InsertU9Log(false, "调入单更新", ex.Message, "", "");
            }
            return rtn;
        }
        /// <summary>
        /// 旺店通调拨入库单查询 审核U9调入单
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void  UpdateU9TransferIn(int page_no)
        {
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
                                if (item.remark.Contains("停止等待"))
                                {
                                    continue;
                                }
                                using (ISession session = Session.Open())
                                {
                                    //foreach (TransInLine tranLine in transfer.TransInLines)
                                    //{
                                    foreach (StockinTransferOrderDto order in res.data.order)
                                    {
                                        if (order.src_order_no == transfer?.DescFlexField.PrivateDescSeg1)
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
                                                                if (lnSubLine.CostPrice>0)
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

                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 旺店通调拨出库单查询 创建U9调入单
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void CreateU9TransferIn(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Transfer.queryWithDetail");
                List<StockoutTransferqueryWithDetail> listreq = new List<StockoutTransferqueryWithDetail>();
                StockoutTransferqueryWithDetail req = new StockoutTransferqueryWithDetail();
                req.start_time = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                StockoutTransferqueryWithRes res = JsonConvert.DeserializeObject<StockoutTransferqueryWithRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "调拨出库单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null && res.data.order.Count > 0)
                {
                    string dataJson = string.Empty;
                    foreach (StockoutTransferOrderDto item in res.data.order)
                    {

                        Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                        if (org != null)
                        {

                            dataJson = JsonHelper.GetTransferInDoc(item, org);
                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                            if (fail == null)
                            {
                                CHelper.InsertFailInfo("调入单创建", dataJson, org, item.order_no, 0,"", item.src_order_no);
                            }

                        }

                        //foreach (QueryStockPdInDetail detail in item.detail_list)
                        //{
                        //    org = CHelper.GetOrg(detail.goods_no, "");
                        //    //判断有多少个组织
                        //    if (org != null && !orgs.Contains(org))
                        //    {
                        //        orgs.Add(org);
                        //    }

                        //    if (organization != null)
                        //    {
                        //        WH = Warehouse.FindByCode(organization, item.warehouse_no);
                        //    }
                        //    if (WH != null && !orgs.Contains(organization))
                        //    {
                        //        orgs.Add(organization);
                        //    }
                        //}

                        ////循环组织集合往每个组织生单
                        //foreach (Organization OrgDetail in orgs)
                        //{
                        //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.order_no + "_" + OrgDetail.Code));
                        //    if (failInfo == null)
                        //    {
                        //        dataJson = JsonHelper.GetWdtStockPdInMiscRcvTrans(item, OrgDetail);
                        //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.order_no + "_" + OrgDetail.Code, 0, "", item.order_no);
                        //    }
                        //}


                    }
                    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                    {
                        page_no++;
                        CreateU9TransferIn(page_no);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 旺店通调拨出库单查询 创建U9调入单 多品牌 多组织 拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void CreateU9TransferIns(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Transfer.queryWithDetail");
                List<StockoutTransferqueryWithDetail> listreq = new List<StockoutTransferqueryWithDetail>();
                StockoutTransferqueryWithDetail req = new StockoutTransferqueryWithDetail();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                StockoutTransferqueryWithRes res = JsonConvert.DeserializeObject<StockoutTransferqueryWithRes>(apireuslt);
                if (res.status != 0)
                {
                    CHelper.InsertU9Log(false, "调拨出库单查询", apireuslt, json, url);
                }
                if (res.data != null && res.data.order != null && res.data.order.Count > 0)
                {
                    string dataJson = string.Empty;
                    Organization org = null;
                    Warehouse WH = null;
                    Organization organization = Organization.FindByCode("192");
                   
                    foreach (StockoutTransferOrderDto item in res.data.order)
                    {

                        List<Organization> orgs = new List<Organization>();
                        if (organization != null)
                        {
                            WH = Warehouse.FindByCode(organization, item.from_warehouse_no);
                            if (WH != null)
                            {
                               
                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no),new OqlParam("Org",organization.ID));
                                if (fail == null)
                                {
                                     dataJson = JsonHelper.GetTransferInDoc(item, organization);
                                    CHelper.InsertFailInfo("调入单创建", dataJson, organization, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                        }
                        else
                        {
                            foreach (StockoutTransferOrderDeatail detail in item.detail_list)
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
                                    dataJson = JsonHelper.GetTransferInDocs(item, OrgDetail);
                                    CHelper.InsertFailInfo("调入单创建", dataJson, OrgDetail, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                            }
                        }
                     


                    }
                    if (res.data != null && res.data.order != null && res.data.order.Count == 100)
                    {
                        page_no++;
                        CreateU9TransferIns(page_no);
                    }
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 调拨入库 创建调入单 拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void  CreateU9TransferInss(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Transfer.queryWithDetail");
                List<StockinTransferqueryWithDetail> listreq = new List<StockinTransferqueryWithDetail>();
                StockinTransferqueryWithDetail req = new StockinTransferqueryWithDetail();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                        if (item.remark.Contains("调拨单停止等待并回补库"))
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
                    CreateU9TransferInss(page_no);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 定时获取失败推送列表数据审核调入单
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void UpdateTransferIn()
        {
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("SELECT TOP 10  ID,DocType,Json,DocNo FROM dbo.Cust_FailInfo WHERE ISSuccess=0 AND  DocType='调入单更新' order by CreatedOn");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    TransferIn Rcv = null;
                    string ErrMsg = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataJson = row["Json"].ToString();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        UpdateTransferDocDto docDto = serializer.Deserialize<UpdateTransferDocDto>(dataJson);
                        if (docDto != null)
                        {
                            Rcv = TransferIn.Finder.FindByID(docDto.ID);
                            Organization org = Organization.FindByCode(docDto.OrgCode);

                            if (Rcv != null && Rcv.Status == TransInStatus.Opening)
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

                                    sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=1,FailTime = GETDATE() WHERE ID = {0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                }
                                catch (Exception ex)
                                {
                                    sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=2,Reason='{1}',FailTime = GETDATE() WHERE ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    CHelper.InsertU9Log(false, "调入单审核", ex.Message, Rcv.DocNo, "");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "调入单更新", ex.Message, dataJson, "");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新调入单方法
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string UpdateTransferIn(string json, long ID)
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
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "调入单更新", ex.Message, json, "");
                throw new Exception(ex.Message);
            }
            return "";
        }
        #endregion

        #region 杂收
        /// <summary>
        /// U9杂收单推送旺店通生成其它入库单
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
        /// 获取奇门预入库单【预入库】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetRcvTrans(int page_no)
        {
            //paramsDomain.CtFrom = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string CtFrom = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            string CtTo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetRcvTrans(page_no, CtFrom, CtTo);
        }

        /// <summary>
        /// 获取奇门预入库单【预入库】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetRcvTrans(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;
                while (bContinue)
                {
                    WdtWmsStockinPrestockinSearchRequest request = new WdtWmsStockinPrestockinSearchRequest();
                    request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    WdtWmsStockinPrestockinSearchRequest.PagerDomain pager = new WdtWmsStockinPrestockinSearchRequest.PagerDomain();
                    pager.PageNo = page_no;
                    pager.PageSize = pageSize;
                    request.Pager_ = pager;
                    WdtWmsStockinPrestockinSearchRequest.ParamsDomain paramsDomain = new WdtWmsStockinPrestockinSearchRequest.
                        ParamsDomain();
                    paramsDomain.CtFrom = starttime;
                    paramsDomain.CtTo = endtime;
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
                                    Organization org = null;
                                    Warehouse WH = null;
                                    Organization organization = Organization.FindByCode("192");
                                    if (organization != null)
                                    {
                                        WH = Warehouse.FindByCode(organization, item.WarehouseNo);
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
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and DocType='杂收单创建'", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.StockinNo));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetQMMiscRcvTrans(item, org);
                                            CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.StockinNo, 0, "", item.SrcOrderNo);
                                        }

                                    }
                                }
                            }

                            if (response.Data != null && response.Data.Order != null && response.Data.Order.Count >= pageSize)
                                page_no++;
                            else
                                bContinue = false;
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
        /// 获取奇门预入库单【预入库】创建杂收数据 拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetRcvTransS(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;
                while (bContinue)
                {
                    WdtWmsStockinPrestockinSearchRequest request = new WdtWmsStockinPrestockinSearchRequest();
                    request.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    WdtWmsStockinPrestockinSearchRequest.PagerDomain pager = new WdtWmsStockinPrestockinSearchRequest.PagerDomain();
                    pager.PageNo = page_no;
                    pager.PageSize = pageSize;
                    request.Pager_ = pager;
                    WdtWmsStockinPrestockinSearchRequest.ParamsDomain paramsDomain = new WdtWmsStockinPrestockinSearchRequest.
                        ParamsDomain();
                    paramsDomain.CtFrom = starttime;
                    paramsDomain.CtTo = endtime;
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
                                Organization org = null;
                                Warehouse WH = null;
                                Organization organization = Organization.FindByCode("192");
                                
                                foreach (WdtWmsStockinPrestockinSearchResponse.OrderDomain item in response.Data.Order)
                                {
                                    List<Organization> orgs = new List<Organization>();
                                    //if (organization != null)
                                    //{
                                    //    WH = Warehouse.FindByCode(organization, item.WarehouseNo);
                                    //}

                                    //if (WH != null)
                                    //{
                                    //    org = organization;
                                    //}
                                    //else
                                    //{
                                    //    org = CHelper.GetOrg(item.DetailList[0].GoodsNo, "");
                                    //}

                                    //if (org != null)
                                    //{
                                    //    //如果已存在，不插入日志
                                    //    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.StockinNo));
                                    //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and DocType='杂收单创建'", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.StockinNo));
                                    //    if (failInfo == null)
                                    //    {
                                    //        dataJson = JsonHelper.GetQMMiscRcvTrans(item, org);
                                    //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.StockinNo, 0, "", item.SrcOrderNo);
                                    //    }

                                    //}


                                    if (organization != null)
                                    {
                                        WH = Warehouse.FindByCode(organization, item.WarehouseNo);
                                       
                                    }

                                    if (WH != null)
                                    {

                                        FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and DocType='杂收单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.StockinNo), new OqlParam("Org", organization.ID));
                                        if (fail == null)
                                        {
                                            dataJson = JsonHelper.GetQMMiscRcvTrans(item, organization);
                                            CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.StockinNo, 0, "", item.SrcOrderNo);
                                        }
                                    }
                                    else
                                    {
                                        foreach (WdtWmsStockinPrestockinSearchResponse.DetailListDomain detail in item.DetailList)
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
                                            FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocType='杂收单创建'  and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.StockinNo));
                                            if (failInfo == null)
                                            {
                                                dataJson = JsonHelper.GetQMMiscRcvTranss(item, OrgDetail);
                                                CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.StockinNo, 0, "", item.SrcOrderNo);
                                            }
                                        }
                                    }
                                }
                            }

                            if (response.Data != null && response.Data.Order != null && response.Data.Order.Count >= pageSize)
                                page_no++;
                            else
                                bContinue = false;
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
        /// 获取旺店通其它入库单【盘盈入库、正残转换】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetMiscShipRcv(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscShipRcv(page_no, start_time, end_time);
        }
       
        /// <summary>
        /// 获取旺店通其它入库单【盘盈入库、正残转换】创建杂收数据
        /// 盘盈入库取消 20230922 变更
        /// 增加库存异动入库
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShipRcv(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                    List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
                    StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "70";
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            //盘盈入库生成杂收取消 20230922
                            //if (item.reason == "盘盈入库")
                            //{
                            //    Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //    if (org != null)
                            //    {
                            //        //如果已存在，不插入日志
                            //        MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                            //        if (rcvTrans == null)
                            //        {
                            //            dataJson = JsonHelper.GetWdtStockInProfitMiscRcvTrans(item, org);
                            //            CHelper.InsertFailInfo("杂收单创建", dataJson, org, "", 0, "", item.other_in_no);
                            //        }
                            //    }
                            //}
                            //正残转换
                            if (item.remark.Contains("正残转换"))
                            {


                                if (WH != null)
                                {
                                    org = organization;
                                }
                                else
                                {
                                    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                }

                                if (org != null)
                                {
                                    //如果已存在，不插入日志
                                    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtShiftMiscRcvTrans(item, org);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0, "", "");
                                    }
                                }
                            }
                            //库存异动入库
                            if (item.reason.Contains("外仓库存异动") && !item.remark.Contains("正残转换"))
                            {
                                if (WH != null)
                                {
                                    org = organization;
                                }
                                else
                                {
                                    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                }

                                if (org != null)
                                {
                                    //如果已存在，不插入日志
                                    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtAbnormalMiscRcvTrans(item, org);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0, "", "");
                                    }
                                }
                            }
                            //销售退回处理更新
                            if (item.reason == "U9退货入库")
                            {
                                if (WH != null)
                                {
                                    org = organization;
                                }
                                else
                                {
                                    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                }
                                if (org != null)
                                {
                                    RMA rma = RMA.Finder.Find("DocNo=@DocNo and Org=@Org", new OqlParam("DocNo", item.other_in_no), new OqlParam("Org", org.ID));
                                    if (rma != null && rma.Status == RMAStatusEnum.Posting)
                                    {
                                        if (rma.DocType.Code == "H0004")
                                        {
                                            dataJson = JsonHelper.GetWDTStockOtherUpdateRMA(item, org, rma.ID);
                                            FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", rma.DocNo), new UFSoft.UBF.PL.OqlParam("Org", org.ID));
                                            if (fail == null)
                                            {
                                                CHelper.InsertFailInfo("退回处理更新", dataJson, org, rma.DocNo, 0);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通其它入库单【盘盈入库、正残转换】创建杂收数据 拆单逻辑
        /// 盘盈入库取消 20230922 变更
        /// 增加库存异动入库
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipRcvs(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                    List<StockinOtherTransferqueryWithDetailReq> listreq = new List<StockinOtherTransferqueryWithDetailReq>();
                    StockinOtherTransferqueryWithDetailReq req = new StockinOtherTransferqueryWithDetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "70";
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                      
                        foreach (var item in res.data.order)
                        {
                            List<Organization> orgs = new List<Organization>();

                            //正残转换
                            if (item.remark.Contains("正残转换"))
                            {


                                //if (WH != null)
                                //{
                                //    org = organization;
                                //}
                                //else
                                //{
                                //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                //}

                                //if (org != null)
                                //{
                                //    //如果已存在，不插入日志
                                //    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                //    if (failInfo == null)
                                //    {
                                //        dataJson = JsonHelper.GetWdtShiftMiscRcvTrans(item, org);
                                //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0, "", "");
                                //    }
                                //}
                                if (organization != null)
                                {
                                    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                   
                                }

                                if (WH != null)
                                {

                                    FailInfo fail = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", organization.ID), new OqlParam("DocNo", item.other_in_no));
                                    if (fail == null)
                                    {
                                        dataJson = JsonHelper.GetWdtShiftMiscRcvTrans(item, organization);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.other_in_no, 0, "", "");
                                    }
                                }
                                else
                                {
                                    foreach (StockInOrderDeatail detail in item.detail_list)
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
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.other_in_no));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetWdtShiftMiscRcvTranss(item, OrgDetail);
                                            CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.other_in_no, 0, "", "");
                                        }
                                    }
                                }
                            }
                            //库存异动入库
                            if (item.reason.Contains("外仓库存异动") && !item.remark.Contains("正残转换"))
                            {
                                //if (WH != null)
                                //{
                                //    org = organization;
                                //}
                                //else
                                //{
                                //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                //}

                                //if (org != null)
                                //{
                                //    //如果已存在，不插入日志
                                //    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_in_no));
                                //    if (failInfo == null)
                                //    {
                                //        dataJson = JsonHelper.GetWdtAbnormalMiscRcvTrans(item, org);
                                //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.other_in_no, 0, "", "");
                                //    }
                                //}
                                if (organization != null)
                                {
                                    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                  
                                }

                                if (WH != null)
                                {

                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.other_in_no), new OqlParam("Org", organization.ID));
                                    if (fail == null)
                                    {
                                        dataJson = JsonHelper.GetWdtAbnormalMiscRcvTrans(item, organization);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.other_in_no, 0, "", "");
                                    }
                                }
                                else
                                {
                                    foreach (StockInOrderDeatail detail in item.detail_list)
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
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.other_in_no));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetWdtAbnormalMiscRcvTranss(item, OrgDetail);
                                            CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.other_in_no, 0, "", "");
                                        }
                                    }
                                }
                            }
                            //销售退回处理更新
                            if (item.reason == "U9退货入库")
                            {
                                if (WH != null)
                                {
                                    org = organization;
                                }
                                else
                                {
                                    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                }
                                if (org != null)
                                {
                                    RMA rma = RMA.Finder.Find("DocNo=@DocNo and Org=@Org", new OqlParam("DocNo", item.other_in_no), new OqlParam("Org", org?.ID));
                                    if (rma != null && rma.Status == RMAStatusEnum.Posting)
                                    {
                                        if (rma.DocType.Code == "H0004")
                                        {
                                            dataJson = JsonHelper.GetWDTStockOtherUpdateRMA(item, org, rma.ID);
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
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通外仓调整入库单【外仓调整入库】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetMiscRcvOut(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscRcvOut(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通外仓调整入库单【外仓调整入库】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscRcvOut(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
                    List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
                    OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "50";
                    //req.src_order_type = 0;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192"); 
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }
                           
                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                Warehouse WH2 = Warehouse.FindByCode(org, item.warehouse_no);
                                if (WH2 != null && WH2.DescFlexField.PrivateDescSeg3 == "10")
                                {
                                    if (item.src_order_type != 0)
                                    {
                                        continue;
                                    }
                                }
                                //如果已存在，不插入日志
                                //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_in_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_in_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtOuterInProfitMiscRcvTrans(item, org);
                                    CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.outer_in_no, 0, "", "");
                                }
                            }

                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 获取旺店通外仓调整入库单【外仓调整入库】创建杂收数据 包含拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscRcvOuts(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterIn.queryWithDetail");
                    List<OuterInquerywithdetailReq> listreq = new List<OuterInquerywithdetailReq>();
                    OuterInquerywithdetailReq req = new OuterInquerywithdetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "50";
                    //req.src_order_type = 0;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                       
                        foreach (var item in res.data.order)
                        {
                            if (item.reason == "POP入库")
                            {
                                continue;
                            }
                            List<Organization> orgs = new List<Organization>();
                            //调整入库生成杂收                              
                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    Warehouse WH2 = Warehouse.FindByCode(org, item.warehouse_no);
                            //    if (WH2 != null && WH2.DescFlexField.PrivateDescSeg3 == "10")
                            //    {
                            //        if (item.src_order_type != 0)
                            //        {
                            //            continue;
                            //        }
                            //    }
                            //    //如果已存在，不插入日志
                            //    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_in_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_in_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtOuterInProfitMiscRcvTrans(item, org);
                            //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.outer_in_no, 0, "", "");
                            //    }
                            //}

                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                               
                            }

                            if (WH != null)
                            {

                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.outer_in_no), new OqlParam("Org", organization.ID));
                                if (fail == null)
                                {
                                    dataJson = JsonHelper.GetWdtOuterInProfitMiscRcvTrans(item, organization);
                                    CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.outer_in_no, 0, "", "");
                                }
                            }
                            else
                            {
                                foreach (OuterInOrderDeatail detail in item.detail_list)
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
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.outer_in_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtOuterInProfitMiscRcvTranss(item, OrgDetail);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.outer_in_no, 0, "", "");
                                    }
                                }
                            }
                        }
                    }
                    

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 获取旺店通汇总入库管理【盘点入库】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetMiscRcvBaseStockIn(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscRcvBaseStockIn(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通汇总入库管理【盘点入库】创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscRcvBaseStockIn(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Base.search");
                    List<StockInBaseSearchReq> listreq = new List<StockInBaseSearchReq>();
                    StockInBaseSearchReq req = new StockInBaseSearchReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.order_type = 4;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockInBaseSearchRes res = JsonConvert.DeserializeObject<StockInBaseSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "入库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // DefectChangeSearchOrder defectChange = null;
                    List<DefectPDdetailSearchDetail> Type;
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order_list != null)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");

                        foreach (var item in res.data.order_list)
                        {
                            List<Organization> orgs = new List<Organization>();
                            //源单号为空不抓
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //转换单号关联查询正残转换单获取残次品信息
                            Type = GetDefectPDdetailSearch(0, item.src_order_no);
                            //入库单查询盘点入库生成杂收     

                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    //如果已存在，不插入日志
                            //    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTrans(item, org, defectChange);
                            //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.stockin_no, 0, "", "");
                            //    }
                            //}
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                if (WH != null)
                                {

                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and  DocType='杂收单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.stockin_no), new OqlParam("Org", organization.ID));
                                    if (fail == null)
                                    {
                                        //dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTransByPD(item, organization, Type);
                                        //CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.stockin_no, 0, "", "");
                                    }
                                }
                            }
                            else
                            {
                                foreach (StockInBaseSearchDetail detail in item.detail_list)
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
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and  DocType='杂收单创建' and  DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.stockin_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTranssByPD(item, OrgDetail, Type);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.stockin_no, 0, "", "");
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order_list != null && res.data.order_list.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 获取旺店通盘点入库创建杂收单数据
        /// </summary>
        /// <param name="page_no"></param>
        public void GetMiscRcvStockPdIn(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscRcvStockPdIn(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通盘点入库创建杂收单数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public RtnDataJson GetMiscRcvStockPdIn(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.StockPd.queryStockPdInDetail");
                    List<QueryStockPdInReq> listreq = new List<QueryStockPdInReq>();
                    QueryStockPdInReq req = new QueryStockPdInReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    QueryStockPdInRes res = JsonConvert.DeserializeObject<QueryStockPdInRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "盘点入库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order != null)
                    {
                        foreach (var item in res.data.order)
                        {
                            //源单号为空不抓
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //入库单查询盘点入库生成杂收     
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }
                          
                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                //如果已存在，不插入日志
                                //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.order_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtStockPdInMiscRcvTrans(item, org);
                                    CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.order_no, 0, "", item.src_order_no);
                                }
                            }

                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 包含拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public RtnDataJson  GetMiscRcvStockPdIns(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.StockPd.queryStockPdInDetail");
                    List<QueryStockPdInReq> listreq = new List<QueryStockPdInReq>();
                    QueryStockPdInReq req = new QueryStockPdInReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    QueryStockPdInRes res = JsonConvert.DeserializeObject<QueryStockPdInRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "盘点入库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order != null)
                    {
                        Organization org = null;
                        Organization organization = Organization.FindByCode("192");
                       
                        foreach (var item in res.data.order)
                        {
                            List<Organization> orgs = new List<Organization>();
                            Warehouse WH = null;
                            //源单号为空不抓
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //入库单查询盘点入库生成杂收     
                            if (organization!=null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }
                            if (WH!=null)
                            {
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", organization.ID), new OqlParam("DocNo", item.order_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtStockPdInMiscRcvTrans(item, organization);
                                    CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                            else
                            {
                                foreach (QueryStockPdInDetail detail in item.detail_list)
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
                                    WH = Warehouse.FindByCode(OrgDetail, item.warehouse_no);
                                    if (WH != null && !string.IsNullOrEmpty(WH.DescFlexField.PrivateDescSeg12))
                                    {
                                        if (Convert.ToBoolean(WH.DescFlexField.PrivateDescSeg12))
                                        {
                                            continue;
                                        }
                                    }
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.order_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtStockPdInMiscRcvTranss(item, OrgDetail);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.order_no, 0, "", item.src_order_no);
                                    }
                                }
                            }
                          


                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 获取旺店通普通其他入库杂收单更新数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetUpdateMiscRcv(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetUpdateMiscRcv(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通普通其他入库杂收单更新数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetUpdateMiscRcv(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.InQuery.queryWithDetail");
                    List<stockinOtherqueryWithDetailReq> listreq = new List<stockinOtherqueryWithDetailReq>();
                    stockinOtherqueryWithDetailReq req = new stockinOtherqueryWithDetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "70";
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                    if (res.data != null && res.data.order != null)
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
                                if (miscRcvTrans != null && miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
                                {
                                    if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "MiscRcv002")
                                    {
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and DocType='杂收单更新'", new OqlParam("Org", org.ID), new OqlParam("DocNo", miscRcvTrans.DocNo));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetWDTStockInUpdateMiscRcvTrans(item, org, miscRcvTrans.ID);
                                            CHelper.InsertFailInfo("杂收单更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                        }
                                    }
                                }

                            }

                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通入库单查询【正残转换】创建杂收数据
        /// 盘盈入库取消 20231102 变更
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShipRcvBaseSearch(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Base.search");
                    List<StockInBaseSearchReq> listreq = new List<StockInBaseSearchReq>();
                    StockInBaseSearchReq req = new StockInBaseSearchReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.order_type = 8;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockInBaseSearchRes res = JsonConvert.DeserializeObject<StockInBaseSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "入库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    //DefectChangeSearchOrder defectChange = null;
                    bool Type;
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order_list != null)
                    {
                        foreach (var item in res.data.order_list)
                        {
                            //源单号为空不抓
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //转换单号关联查询正残转换单获取残次品信息
                            Type = GetDefectChangeSearch(0, item.src_order_no);
                            //入库单查询盘点入库生成杂收     
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                //如果已存在，不插入日志
                                //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTrans(item, org, Type);
                                    CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.stockin_no, 0, "", "");
                                }
                            }

                        }
                    }

                    if (res.data != null && res.data.order_list != null && res.data.order_list.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 生产入库单创建杂收数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipRcvProcess(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Process.queryWithDetail");
                    List<StockinProcessReq> listreq = new List<StockinProcessReq>();
                    StockinProcessReq req = new StockinProcessReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockinProcessRes res = JsonConvert.DeserializeObject<StockinProcessRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "生产入库单查询", apireuslt, json, url);
                    }
              
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order != null)
                    {
                        string dataJson = string.Empty;
                        string posturl = string.Empty;
                        string apirEEEeuslt = string.Empty;
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                        foreach (var item in res.data.order)
                        {
                            //入库单查询盘点入库生成杂收     
                           
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                //如果已存在，不插入日志
                                //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtProcessStockInMiscRcvTrans(item, org);
                                    CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.stockin_no, 0, "", "");
                                }
                            }

                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 获取旺店通入库单查询【正残转换】创建杂收数据 拆弹逻辑
        /// 盘盈入库取消 20231102 变更
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipRcvBaseSearchs(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockin.Base.search");
                    List<StockInBaseSearchReq> listreq = new List<StockInBaseSearchReq>();
                    StockInBaseSearchReq req = new StockInBaseSearchReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.order_type = 8;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockInBaseSearchRes res = JsonConvert.DeserializeObject<StockInBaseSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "入库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // DefectChangeSearchOrder defectChange = null;
                    bool Type;
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order_list != null)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                        
                        foreach (var item in res.data.order_list)
                        {
                            List<Organization> orgs = new List<Organization>();
                            //源单号为空不抓
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //转换单号关联查询正残转换单获取残次品信息
                            Type = GetDefectChangeSearch(0, item.src_order_no);
                            //入库单查询盘点入库生成杂收     

                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    //如果已存在，不插入日志
                            //    //MiscRcvTrans rcvTrans = MiscRcvTrans.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and ISSuccess=0", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockin_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTrans(item, org, defectChange);
                            //        CHelper.InsertFailInfo("杂收单创建", dataJson, org, item.stockin_no, 0, "", "");
                            //    }
                            //}
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                if (WH != null)
                                {

                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and  DocType='杂收单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.stockin_no), new OqlParam("Org", organization.ID));
                                    if (fail == null)
                                    {
                                        dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTrans(item, organization, Type);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, organization, item.stockin_no, 0, "", "");
                                    }
                                }
                            }
                            else
                            {
                                foreach (StockInBaseSearchDetail detail in item.detail_list)
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
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and  DocType='杂收单创建' and  DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.stockin_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtBaseSearchStockInMiscRcvTranss(item, OrgDetail, Type);
                                        CHelper.InsertFailInfo("杂收单创建", dataJson, OrgDetail, item.stockin_no, 0, "", "");
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order_list != null && res.data.order_list.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

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
        /// 正残转换单查询
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="ChangeNo"></param>
        /// <returns></returns>
        public bool GetDefectChangeSearch(int page_no, string ChangeNo)
        {
            // DefectChangeSearchOrder rtn = new DefectChangeSearchOrder();
            bool Type=false ;
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockdefect.DefectChange.search");
                    List<DefectChangeSearchReq> listreq = new List<DefectChangeSearchReq>();
                    DefectChangeSearchReq req = new DefectChangeSearchReq();
                    req.change_no = ChangeNo;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    DefectChangeSearchRes res = JsonConvert.DeserializeObject<DefectChangeSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "正残转换单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    //CommonSV commonSV = new CommonSV();
                    if (res.data != null && res.data.order != null && res.data.order.Count > 0)
                    {
                        Type = (bool)res.data.order[0]?.type;
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;

                }


            }
            catch (Exception ex)
            {

            }
            return Type;
        }

        /// <summary>
        /// 盘点单明细查询
        /// </summary>
        /// <param name="page_no"></param>
        /// <param name="ChangeNo"></param>
        /// <returns></returns>
        public List<DefectPDdetailSearchDetail> GetDefectPDdetailSearch(int page_no, string PDNo)
        {
            // DefectChangeSearchOrder rtn = new DefectChangeSearchOrder();
            List<DefectPDdetailSearchDetail> Type = new List<DefectPDdetailSearchDetail>();
            try
            {
                int pageSize = 300;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.StockPd.queryStockPdDetail");
                    List<DefectPDdetailSearchReq> listreq = new List<DefectPDdetailSearchReq>();
                    DefectPDdetailSearchReq req = new DefectPDdetailSearchReq();
                    req.pd_no = PDNo;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    DefectPDdetailSearchRes res = JsonConvert.DeserializeObject<DefectPDdetailSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "盘点单明细查询", apireuslt, json, url);
                    }
                    Type = res.data;
                    bContinue = false;

                }


            }
            catch (Exception ex)
            {
                
            }
            return Type;
        }
        /// <summary>
        /// 定时获取失败推送列表数据生成杂收单
        /// </summary>
        /// <exception cref="Exception"></exception>
        public RtnDataJson CreateMiscRcv()
        {
            RtnDataJson rtn = new RtnDataJson();
            string dataJson = string.Empty;
            string posturl = string.Empty;
            string sErrorDocNos = "";
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("SELECT TOP 100 ID, DocType, Json,SrcDocNo FROM dbo.Cust_FailInfo WHERE ISSuccess=0 AND DocType='杂收单创建' ORDER BY CreatedOn;");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                        posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                        dataJson = row["Json"].ToString();
                        string sDocNo = row["SrcDocNo"].ToString();
                        apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                        if (rtns.IsSuccess)
                        {
                            sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=1,Reason='',DocNo ='{1}',FailTime = GETDATE() WHERE ID = {0}", Convert.ToInt64(row["ID"]), rtns.DocNo);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                        else
                        {
                            rtns.Msg = rtns.Msg.Replace("'", "''");
                            string sErrorMsg = rtns.Msg.Length > 200 ? rtns.Msg.Substring(0, 200) : rtns.Msg;
                            sql = string.Format(@"UPDATE Cust_FailInfo SET Reason='{1}',FailTime = GETDATE() WHERE ID={0}", Convert.ToInt64(row["ID"]), sErrorMsg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //sErrorDocNos += sDocNo + ",";
                            sErrorDocNos += "[" + sDocNo + ":" + sErrorMsg + "],";
                        }
                    }
                }

                if (string.IsNullOrEmpty(sErrorDocNos))
                    rtn.IsSuccess = true;
                else
                {
                    rtn.IsSuccess = false;
                    rtn.Msg = "【" + sErrorDocNos.TrimEnd(new char[] { ',' }) + "】生单失败，请联系IT处理！";
                }

            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
                CHelper.InsertU9Log(false, "杂收单创建", ex.Message, dataJson, posturl);
            }

            return rtn;
        }

        /// <summary>
        /// 定时获取失败推送列表数据更新审核杂收单
        /// </summary>
        public RtnDataJson UpdateMiscRcv()
        {
            RtnDataJson rtn = new RtnDataJson();
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                string Msg = string.Empty;
                sql = string.Format("SELECT ID,DocType,Json,DocNo FROM dbo.Cust_FailInfo WHERE ISSuccess=0 AND DocType='杂收单更新' order by CreatedOn");
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
                                    Rcv.DescFlexField.PrivateDescSeg2 = "True";
                                    Rcv.Memo = docDto.Remark;
                                    foreach (MiscRcvTransL rcvline in Rcv.MiscRcvTransLs)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (!string.IsNullOrEmpty(rcvline.DescFlexSegments.PrivateDescSeg3))
                                            {
                                                if (rcvline.DescFlexSegments.PrivateDescSeg3==Deatail.ItemCode)
                                                {
                                                    rcvline.Memo = Deatail.remark;
                                                    rcvline.StoreUOMQty = Deatail.Amount;
                                                    //rcvline.TradeUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostMny = rcvline.CostUOMQty * rcvline.CostPrice;
                                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                    if (wh != null)
                                                    {
                                                        rcvline.Wh = wh;
                                                        rcvline.Wh.ID = wh.ID;
                                                        rcvline.Wh.Code = wh.Code;
                                                    }
                                                    if (rcvline.MiscRcvTransBins != null && rcvline.MiscRcvTransBins.Count > 0)
                                                        rcvline.MiscRcvTransBins[0].StoreUOMQty = rcvline.StoreUOMQty;
                                                }
                                            }
                                            else
                                            {
                                                if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                                {
                                                    rcvline.Memo = Deatail.remark;
                                                    rcvline.StoreUOMQty = Deatail.Amount;
                                                    //rcvline.TradeUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostMny = rcvline.CostUOMQty * rcvline.CostPrice;
                                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                    if (wh != null)
                                                    {
                                                        rcvline.Wh = wh;
                                                        rcvline.Wh.ID = wh.ID;
                                                        rcvline.Wh.Code = wh.Code;
                                                    }
                                                    if (rcvline.MiscRcvTransBins != null && rcvline.MiscRcvTransBins.Count > 0)
                                                        rcvline.MiscRcvTransBins[0].StoreUOMQty = rcvline.StoreUOMQty;

                                                }
                                            }
                                           
                                        }
                                    }
                                    session.Commit();
                                    sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=1,FailTime = GETDATE() WHERE ID = {0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                }

                                //try
                                //{
                                //    List<long> lstID = new List<long>();
                                //    lstID.Add(Rcv.ID);
                                //    Msg = ApproveMiscRcv(lstID);


                                //    sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=1,FailTime = GETDATE() WHERE ID = {0}", Convert.ToInt64(row["ID"]));
                                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                //}
                                //catch (Exception ex)
                                //{
                                //    Msg = ex.Message;
                                //    sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=2,Reason='{1}',FailTime = GETDATE() WHERE ID={0}", Convert.ToInt64(row["ID"]), Msg);
                                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                //}
                            }
                        }
                    }
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "杂收单更新", ex.Message, dataJson, "");
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
            }
            return rtn;
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
        /// 更新杂收单方法
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string UpdateRcvTrans(string json, long ID)
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
            }

            return "";
        }
        #endregion

        #region 杂发
        /// <summary>
        /// U9杂发单推送旺店通生成其它出库单
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
        /// 获取旺店通其它出库单【盘亏出库、正残转换】创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetMiscShips(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscShips(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通其它出库单【盘亏出库、正残转换】创建杂发数据
        /// 盘盈出库取消 20230922 变更
        /// 增加库存异动出库
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShips(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                    List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                    StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "70";
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            //盘亏出库生成杂发
                            //if (item.reason == "盘亏出库")
                            //{
                            //    Organization org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //    if (org != null)
                            //    {
                            //        MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                            //        if (shipment == null)
                            //        {
                            //            dataJson = JsonHelper.GetWdtStockOutLossMiscShip(item, org);
                            //            CHelper.InsertFailInfo("杂发单创建", dataJson, org, "", 0, "", item.other_out_no);
                            //        }
                            //    }
                            //}
                            //正残转换
                            if (item.remark.Contains("正残转换"))
                            {

                                if (WH != null)
                                {
                                    org = organization;
                                }
                                else
                                {
                                    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                }
                                if (org != null)
                                {
                                    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtShiftMiscShip(item, org);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0, "", "");
                                    }
                                }
                            }
                            if (item.reason.Contains("外仓库存异动") && !item.remark.Contains("正残转换"))
                            {
                                if (WH != null)
                                {
                                    org = organization;
                                }
                                else
                                {
                                    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                }
                                if (org != null)
                                {
                                    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtAbnormalLossMiscShip(item, org);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0, "", "");
                                    }
                                }

                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通其它出库单【盘亏出库、正残转换】创建杂发数据 拆单逻辑
        /// 盘盈出库取消 20230922 变更
        /// 增加库存异动出库
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipss(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {

                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                    List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                    StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "70";
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                       
                        foreach (var item in res.data.order)
                        {

                            List<Organization> orgs = new List<Organization>();
                            //正残转换
                            if (item.remark.Contains("正残转换"))
                            {

                                //if (WH != null)
                                //{
                                //    org = organization;
                                //}
                                //else
                                //{
                                //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                //}
                                //if (org != null)
                                //{
                                //    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                //    if (failInfo == null)
                                //    {
                                //        dataJson = JsonHelper.GetWdtShiftMiscShip(item, org);
                                //        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0, "", "");
                                //    }
                                //}
                                if (organization != null)
                                {
                                    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                   
                                }

                                if (WH != null)
                                {

                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and DocType='杂发单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.other_out_no), new OqlParam("Org", organization.ID));
                                    if (fail == null)
                                    {
                                        dataJson = JsonHelper.GetWdtShiftMiscShip(item, organization);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, organization, item.other_out_no, 0, "", "");
                                    }
                                }
                                else
                                {
                                    foreach (StockOutOrderDeatail detail in item.detail_list)
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
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocType='杂发单创建' and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.other_out_no));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetWdtShiftMiscShipsZC(item, OrgDetail);
                                            CHelper.InsertFailInfo("杂发单创建", dataJson, OrgDetail, item.other_out_no, 0, "", "");
                                        }
                                    }
                                }
                            }
                            if (item.reason.Contains("外仓库存异动") && !item.remark.Contains("正残转换"))
                            {
                                //if (WH != null)
                                //{
                                //    org = organization;
                                //}
                                //else
                                //{
                                //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                                //}
                                //if (org != null)
                                //{
                                //    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.other_out_no));
                                //    if (failInfo == null)
                                //    {
                                //        dataJson = JsonHelper.GetWdtAbnormalLossMiscShip(item, org);
                                //        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.other_out_no, 0, "", "");
                                //    }
                                //}
                                if (organization != null)
                                {
                                    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                   
                                }

                                if (WH != null)
                                {

                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and DocType='杂发单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.other_out_no), new OqlParam("Org", organization.ID));
                                    if (fail == null)
                                    {
                                        dataJson = JsonHelper.GetWdtShiftMiscShip(item, organization);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, organization, item.other_out_no, 0, "", "");
                                    }
                                }
                                else
                                {
                                    foreach (StockOutOrderDeatail detail in item.detail_list)
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
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocType='杂发单创建' and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.other_out_no));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetWdtShiftMiscShips(item, OrgDetail);
                                            CHelper.InsertFailInfo("杂发单创建", dataJson, OrgDetail, item.other_out_no, 0, "", "");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通外仓调整出库单【外仓调整出库】创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetMiscShipOut(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscShipOut(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通外仓调整出库单【外仓调整出库】创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShipOut(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
                    List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
                    OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "50";
                    //req.order_type = 0;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                    if (res.status == 0 && res.data != null && res.data.order != null)
                    {
                        foreach (var item in res.data.order)
                        {
                            //调整出库生成杂发
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                Warehouse WH2 = Warehouse.FindByCode(org, item.warehouse_no);
                                if (WH2 != null && WH2.DescFlexField.PrivateDescSeg3 == "10")
                                {
                                    continue;
                                }
                                //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_out_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_out_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtOuterOutLossMiscShip(item, org);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.outer_out_no, 0, "", "");
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通外仓调整出库单【外仓调整出库】创建杂发数据 拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipOuts(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.outer.OuterOut.queryWithDetail");
                    List<OuterOutquerywithdetailReq> listreq = new List<OuterOutquerywithdetailReq>();
                    OuterOutquerywithdetailReq req = new OuterOutquerywithdetailReq();
                    //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "50";
                    //req.order_type = 0;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
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
                    if (res.status == 0 && res.data != null && res.data.order != null)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                       
                        foreach (var item in res.data.order)
                        {
                            if (item.reason == "POP出库")
                            {
                                continue;
                            }
                            List<Organization> orgs = new List<Organization>();
                            //调整出库生成杂发
                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    Warehouse WH2 = Warehouse.FindByCode(org, item.warehouse_no);
                            //    if (WH2 != null && WH2.DescFlexField.PrivateDescSeg3 == "10")
                            //    {
                            //        continue;
                            //    }
                            //    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_out_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.outer_out_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtOuterOutLossMiscShip(item, org);
                            //        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.outer_out_no, 0, "", "");
                            //    }
                            //}
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                               
                            }

                            if (WH != null)
                            {
                              
                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.outer_out_no), new OqlParam("Org", organization.ID));
                                if (fail == null)
                                {
                                    dataJson = JsonHelper.GetWdtOuterOutLossMiscShip(item, organization);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, organization, item.outer_out_no, 0, "", "");
                                }
                            }
                            else
                            {
                                foreach (OuterOutOrderDeatail detail in item.detail_list)
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
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.outer_out_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtOuterOutLossMiscShips(item, OrgDetail);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, OrgDetail, item.outer_out_no, 0, "", "");
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通出库明细【盘亏出库】创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetMiscShipBaseStockOut(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscShipBaseStockOut(page_no, start_time, end_time);
        }
        /// <summary>
        /// 获取旺店通出库明细【盘亏出库】创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShipBaseStockOut(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Base.search");
                    List<StockOutBaseSearchReq> listreq = new List<StockOutBaseSearchReq>();
                    StockOutBaseSearchReq req = new StockOutBaseSearchReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.order_type = 4;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockOutBaseSearchRes res = JsonConvert.DeserializeObject<StockOutBaseSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "出库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order_list != null)
                    {
                        //DefectChangeSearchOrder defectChange = null;
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                        List<DefectPDdetailSearchDetail> Type;
                        foreach (var item in res.data.order_list)
                        {
                            List<Organization> orgs = new List<Organization>();
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //转换单号关联查询正残转换单获取残次品信息
                            Type = GetDefectPDdetailSearch(0, item.src_order_no);

                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShip(item, org, defectChange);
                            //        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.stockout_no, 0, "", "");
                            //    }
                            //}
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);

                            }

                            if (WH != null)
                            {

                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and DocType='杂发单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.stockout_no), new OqlParam("Org", organization.ID));
                                if (fail == null)
                                {
                                    //dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShipByPD(item, organization, Type);
                                    //CHelper.InsertFailInfo("杂发单创建", dataJson, organization, item.stockout_no, 0, "", "");
                                }
                            }
                            else
                            {
                                foreach (StockOutBaseSearchDetail detail in item.detail_list)
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
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocType='杂发单创建' and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.stockout_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShipsByPD(item, OrgDetail, Type);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, OrgDetail, item.stockout_no, 0, "", "");
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order_list != null && res.data.order_list.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通出库明细【正残转换出库】创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShipBaseSearchStockOut(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Base.search");
                    List<StockOutBaseSearchReq> listreq = new List<StockOutBaseSearchReq>();
                    StockOutBaseSearchReq req = new StockOutBaseSearchReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.order_type = 8;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockOutBaseSearchRes res = JsonConvert.DeserializeObject<StockOutBaseSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "出库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order_list != null)
                    {
                        //DefectChangeSearchOrder defectChange = null;
                        bool Type;
                        foreach (var item in res.data.order_list)
                        {
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //转换单号关联查询正残转换单获取残次品信息
                            Type = GetDefectChangeSearch(0, item.src_order_no);
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShip(item, org, Type);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.stockout_no, 0, "", "");
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order_list != null && res.data.order_list.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 生产出库创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipstockOutProcess(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Process.queryWithDetail");
                    List<StockoutProcessReq> listreq = new List<StockoutProcessReq>();
                    StockoutProcessReq req = new StockoutProcessReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockoutProcessRes res = JsonConvert.DeserializeObject<StockoutProcessRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "生产出库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order != null)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                        foreach (var item in res.data.order)
                        {
                          
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtProcessStockOutLossMiscShip(item, org);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.stockout_no, 0, "", "");
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 生产单查询创建形态转换
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetShiftDocFromProcess(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("process.Process.search");
                    List<StockProcessReq> listreq = new List<StockProcessReq>();
                    StockProcessReq req = new StockProcessReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockProcessRes res = JsonConvert.DeserializeObject<StockProcessRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "生产单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order != null)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                        foreach (var item in res.data.order)
                        {

                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.in_warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg("", item.detail_list[0].spec_no);
                            }
                            if (org != null)
                            {
                                //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.process_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtProcessStockShiftDoc(item, org);
                                    CHelper.InsertFailInfo("形态转换单创建", dataJson, org, item.process_no, 0, "", "");
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通出库明细【正残转换出库】创建杂发数据 拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipBaseSearchStockOuts(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.Base.search");
                    List<StockOutBaseSearchReq> listreq = new List<StockOutBaseSearchReq>();
                    StockOutBaseSearchReq req = new StockOutBaseSearchReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.order_type = 8;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockOutBaseSearchRes res = JsonConvert.DeserializeObject<StockOutBaseSearchRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "出库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order_list != null)
                    {
                        //DefectChangeSearchOrder defectChange = null;
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                        bool Type;
                        foreach (var item in res.data.order_list)
                        {
                            List<Organization> orgs = new List<Organization>();
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            //转换单号关联查询正残转换单获取残次品信息
                            Type = GetDefectChangeSearch(0, item.src_order_no);

                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShip(item, org, defectChange);
                            //        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.stockout_no, 0, "", "");
                            //    }
                            //}
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                                                             
                            }

                            if (WH != null)
                            {

                                FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo and DocType='杂发单创建' and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", item.stockout_no), new OqlParam("Org", organization.ID));
                                if (fail == null)
                                {
                                    dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShip(item, organization, Type);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, organization, item.stockout_no, 0, "", "");
                                }
                            }
                            else
                            {
                                foreach (StockOutBaseSearchDetail detail in item.detail_list)
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
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocType='杂发单创建' and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.stockout_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtBaseSearchStockOutLossMiscShips(item, OrgDetail, Type);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, OrgDetail, item.stockout_no, 0, "", "");
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order_list != null && res.data.order_list.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取盘点出库创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        public void GetMiscShipStockPdOut(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetMiscShipStockPdOut(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取盘点出库创建杂发数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetMiscShipStockPdOut(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.StockPd.queryStockPdOutDetail");
                    List<QueryStockPdOutReq> listreq = new List<QueryStockPdOutReq>();
                    QueryStockPdOutReq req = new QueryStockPdOutReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    QueryStockPdOutRes res = JsonConvert.DeserializeObject<QueryStockPdOutRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "盘点出库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order != null)
                    {
                        foreach (var item in res.data.order)
                        {
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;
                            Organization org = null;
                            Warehouse WH = null;
                            Organization organization = Organization.FindByCode("192");
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }

                            if (WH != null)
                            {
                                org = organization;
                            }
                            else
                            {
                                org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            }
                            if (org != null)
                            {
                                //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.order_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtStockPdOutMiscShip(item, org);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取盘点出库创建杂发数据 拆单逻辑
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson  GetMiscShipStockPdOuts(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.StockPd.queryStockPdOutDetail");
                    List<QueryStockPdOutReq> listreq = new List<QueryStockPdOutReq>();
                    QueryStockPdOutReq req = new QueryStockPdOutReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    QueryStockPdOutRes res = JsonConvert.DeserializeObject<QueryStockPdOutRes>(apireuslt);
                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "盘点出库单查询", apireuslt, json, url);
                    }
                    string dataJson = string.Empty;
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    // CommonSV commonSV = new CommonSV();
                    if (res.status == 0 && res.data != null && res.data.order != null)
                    {
                        Organization org = null;
                        Warehouse WH = null;
                        Organization organization = Organization.FindByCode("192");
                       
                        foreach (var item in res.data.order)
                        {
                            List<Organization> orgs = new List<Organization>();
                            if (string.IsNullOrEmpty(item.src_order_no)) continue;

                            //if (organization != null)
                            //{
                            //    WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            //}

                            //if (WH != null)
                            //{
                            //    org = organization;
                            //}
                            //else
                            //{
                            //    org = CHelper.GetOrg(item.detail_list[0].goods_no, "");
                            //}
                            //if (org != null)
                            //{
                            //    //MiscShipment shipment = MiscShipment.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.stockout_no));
                            //    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", org.ID), new OqlParam("DocNo", item.order_no));
                            //    if (failInfo == null)
                            //    {
                            //        dataJson = JsonHelper.GetWdtStockPdOutMiscShip(item, org);
                            //        CHelper.InsertFailInfo("杂发单创建", dataJson, org, item.order_no, 0, "", item.src_order_no);
                            //    }
                            //}
                            if (organization != null)
                            {
                                WH = Warehouse.FindByCode(organization, item.warehouse_no);
                            }
                            if (WH != null)
                            {
                                FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", organization.ID), new OqlParam("DocNo", item.order_no));
                                if (failInfo == null)
                                {
                                    dataJson = JsonHelper.GetWdtStockPdOutMiscShip(item, organization);
                                    CHelper.InsertFailInfo("杂发单创建", dataJson, organization, item.order_no, 0, "", item.src_order_no);
                                }
                            }
                            else
                            {
                                foreach (QueryStockPdOutDetail detail in item.detail_list)
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
                                    WH = Warehouse.FindByCode(OrgDetail, item.warehouse_no);
                                    if (WH != null && !string.IsNullOrEmpty(WH.DescFlexField.PrivateDescSeg12))
                                    {
                                        if (Convert.ToBoolean(WH.DescFlexField.PrivateDescSeg12))
                                        {
                                            continue;
                                        }
                                    }
                                    FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo", new OqlParam("Org", OrgDetail.ID), new OqlParam("DocNo", item.order_no));
                                    if (failInfo == null)
                                    {
                                        dataJson = JsonHelper.GetWdtStockPdOutMiscShips(item, OrgDetail);
                                        CHelper.InsertFailInfo("杂发单创建", dataJson, OrgDetail, item.order_no, 0, "", item.src_order_no);
                                    }
                                }
                            }
                        }
                    }

                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 获取旺店通普通其他入库杂发单更新数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public void GetUpdateMiscShip(int page_no)
        {
            //req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string start_time = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            GetUpdateMiscShip(page_no, start_time, end_time);
        }

        /// <summary>
        /// 获取旺店通普通其他入库杂发单更新数据
        /// </summary>
        /// <param name="page_no"></param>
        /// <exception cref="Exception"></exception>
        public RtnDataJson GetUpdateMiscShip(int page_no, string starttime, string endtime)
        {
            RtnDataJson rtn = new RtnDataJson();
            try
            {
                int pageSize = 100;
                bool bContinue = true;

                while (bContinue)
                {
                    Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockother.OutQuery.queryWithDetail");
                    List<StockOutOtherTransferqueryWithDetailReq> listreq = new List<StockOutOtherTransferqueryWithDetailReq>();
                    StockOutOtherTransferqueryWithDetailReq req = new StockOutOtherTransferqueryWithDetailReq();
                    req.start_time = starttime;
                    req.end_time = endtime;
                    req.time_type = 2;
                    req.status = "70";
                    listreq.Add(req);
                    string json = JsonConvert.SerializeObject(listreq);
                    list.Add("body", json);
                    //分页数据
                    list.Add("page_size", pageSize.ToString());
                    list.Add("page_no", page_no.ToString());
                    list.Add("calc_total", "1");
                    string sign = WDTChelper.GetWDTSign(list);
                    list.Add("sign", sign);
                    string url = WDTChelper.apiUrl + WDTChelper.GetUrlApend(list);
                    string apireuslt = Tools.HttpPost(url, json);
                    StockOutOtherMiscShipQueryWithDetailRes res = JsonConvert.DeserializeObject<StockOutOtherMiscShipQueryWithDetailRes>(apireuslt);

                    if (res.status != 0)
                    {
                        CHelper.InsertU9Log(false, "其它出库单查询", apireuslt, json, url);
                    }
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
                                if (miscRcvTrans != null && miscRcvTrans.Status == InvDoc.Enums.INVDocStatus.Approving)
                                {
                                    if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "MiscShip003")
                                    {
                                        FailInfo failInfo = FailInfo.Finder.Find("Org=@Org and DocNo=@DocNo and DocType='杂发单更新'", new OqlParam("Org", org.ID), new OqlParam("DocNo", miscRcvTrans.DocNo));
                                        if (failInfo == null)
                                        {
                                            dataJson = JsonHelper.GetWDTStockOutUpdateMiscShip(item, org, miscRcvTrans.ID);
                                            CHelper.InsertFailInfo("杂发单更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (res.data != null && res.data.order != null && res.data.order.Count >= pageSize)
                        page_no++;
                    else
                        bContinue = false;
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
        /// 定时获取失败推送列表数据生成杂发单
        /// </summary>
        /// <exception cref="Exception"></exception>
        public RtnDataJson CreateMiscShip()
        {
            RtnDataJson rtn = new RtnDataJson();
            string dataJson = string.Empty;
            string posturl = string.Empty;
            string sErrorDocNos = "";
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("SELECT TOP 100 ID, DocType, Json, SrcDocNo FROM dbo.Cust_FailInfo WHERE ISSuccess=0 AND DocType='杂发单创建' ORDER BY CreatedOn;");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                        posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                        dataJson = row["Json"].ToString();
                        string sDocNo = row["SrcDocNo"].ToString();
                        apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                        if (rtns.IsSuccess)
                        {
                            sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=1,DocNo ='{1}',FailTime = GETDATE() WHERE ID = {0}", Convert.ToInt64(row["ID"]), rtns.DocNo);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                        else
                        {
                            rtns.Msg = rtns.Msg.Replace("'", "''");
                            string sErrorMsg = rtns.Msg.Length > 200 ? rtns.Msg.Substring(0, 200) : rtns.Msg;
                            sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=2,Reason='{1}',FailTime = GETDATE() WHERE ID={0}", Convert.ToInt64(row["ID"]), sErrorMsg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //sErrorDocNos += sDocNo + ",";
                            sErrorDocNos += "[" + sDocNo + ":" + sErrorMsg + "],";
                        }
                    }

                }

                if (string.IsNullOrEmpty(sErrorDocNos))
                    rtn.IsSuccess = true;
                else
                {
                    rtn.IsSuccess = false;
                    rtn.Msg = "【" + sErrorDocNos.TrimEnd(new char[] { ',' }) + "】生单失败，请联系IT处理！";
                }
            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
                CHelper.InsertU9Log(false, "杂发单创建", ex.Message, dataJson, posturl);
            }
            return rtn;
        }
        /// <summary>
        /// 生产单查询创建形态转换单
        /// </summary>
        /// <returns></returns>
        public RtnDataJson CreateShiftDoc()
        {
            RtnDataJson rtn = new RtnDataJson();
            string dataJson = string.Empty;
            string posturl = string.Empty;
            try
            {
                string sql = string.Empty;
                string dataJson1 = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("Select top 100  ID,DocType,Json,Org  from Cust_FailInfo where issuccess=0 and  DocType='形态转换单创建' order by CreatedOn");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                        posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                        dataJson = row["Json"].ToString();
                        apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                        if (rtns.IsSuccess)
                        {
                            rtn.Msg = rtns.DocNo;
                            rtn.IsSuccess = true;
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
                            dataJson1 = JsonHelper.ApproveTransferFormJson(org, rtns.DocNo);
                            apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson1);
                           
                        }
                        else
                        {
                            rtns.Msg = rtns.Msg.Length > 100 ? rtns.Msg.Substring(0, 100) : rtns.Msg;
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), rtns.Msg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            rtn.Msg = rtns.Msg;
                            rtn.IsSuccess = false;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                rtn.Msg = ex.Message;
                rtn.IsSuccess = false;
                CHelper.InsertU9Log(false, "形态转换单创建", ex.Message, dataJson, posturl);               
            }
            return rtn;
        }
        /// <summary>
        /// 定时获取失败推送列表数据更新审核杂发单
        /// </summary>
        /// <exception cref="Exception"></exception>
        public RtnDataJson UpdateMiscShip()
        {
            RtnDataJson rtn = new RtnDataJson();
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                string Msg = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("SELECT ID,DocType,Json,DocNo FROM dbo.Cust_FailInfo WHERE ISSuccess=0 AND  DocType='杂发单更新' order by CreatedOn");
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
                                    Rcv.DescFlexField.PrivateDescSeg1 = "True";
                                    Rcv.Memo = docDto.Remark;
                                    foreach (MiscShipmentL rcvline in Rcv.MiscShipLs)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (!string.IsNullOrEmpty(rcvline.DescFlexSegments.PrivateDescSeg3))
                                            {
                                                if (rcvline.DescFlexSegments.PrivateDescSeg3==Deatail.ItemCode)
                                                {
                                                    rcvline.Memo = Deatail.remark;
                                                    rcvline.StoreUOMQty = Deatail.Amount;
                                                    //rcvline.TradeUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostMny = rcvline.CostUOMQty * rcvline.CostPrice;

                                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                    if (wh != null)
                                                    {
                                                        rcvline.Wh = wh;
                                                        rcvline.Wh.ID = wh.ID;
                                                        rcvline.Wh.Code = wh.Code;
                                                    }
                                                    if (rcvline.MiscShipBins != null && rcvline.MiscShipBins.Count > 0)
                                                        rcvline.MiscShipBins[0].StoreUOMQty = rcvline.StoreUOMQty;
                                                }
                                            }
                                            else
                                            {
                                                if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                                {
                                                    rcvline.Memo = Deatail.remark;
                                                    rcvline.StoreUOMQty = Deatail.Amount;
                                                    //rcvline.TradeUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostUOMQty = rcvline.StoreUOMQty;
                                                    rcvline.CostMny = rcvline.CostUOMQty * rcvline.CostPrice;

                                                    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                    if (wh != null)
                                                    {
                                                        rcvline.Wh = wh;
                                                        rcvline.Wh.ID = wh.ID;
                                                        rcvline.Wh.Code = wh.Code;
                                                    }
                                                    if (rcvline.MiscShipBins != null && rcvline.MiscShipBins.Count > 0)
                                                        rcvline.MiscShipBins[0].StoreUOMQty = rcvline.StoreUOMQty;
                                                }
                                            }
                                          
                                        }
                                    }
                                    session.Commit();

                                    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                }

                                //try
                                //{
                                //    List<long> lstID = new List<long>();
                                //    lstID.Add(Rcv.ID);
                                //    Msg = ApproveMiscShip(lstID);

                                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                //}
                                //catch (Exception ex)
                                //{
                                //    Msg = ex.Message;

                                //    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), Msg);
                                //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                //}
                            }
                        }
                    }
                }
                rtn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rtn.IsSuccess = false;
                rtn.Msg = ex.Message;
                CHelper.InsertU9Log(false, "杂发单更新", ex.Message, dataJson, "");
            }
            return rtn;
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

        /// <summary>
        /// 更新杂发单方法
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string UpdateMiscShip(string json, long ID)
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
                            sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=1,FailTime = GETDATE() WHERE ID = {0}", ID);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                        catch (Exception ex)
                        {
                            Msg = ex.Message;
                            scope.Rollback();
                            sql = string.Format(@"UPDATE Cust_FailInfo SET ISSuccess=2,Reason='{1}',FailTime = GETDATE() WHERE ID={0}", ID, Msg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                    }
                }
            }
            return "";
        }
        #endregion
    }
}
