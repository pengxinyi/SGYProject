namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
	using System.Collections.Generic;
    using System.Text;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.LH.LHPubBE.FailInfoBE;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.PM.PO;
    using UFIDA.U9.PM.Rcv;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Util.Context;

    /// <summary>
    /// AutoUpdateSaleReturnDoc partial 
    /// </summary>	
    public partial class AutoUpdateSaleReturnDoc
    {
        internal BaseStrategy Select()
        {
			return new AutoUpdateSaleReturnDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoUpdateSaleReturnDocImpementStrategy : BaseStrategy
	{
		public AutoUpdateSaleReturnDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoUpdateSaleReturnDoc bpObj = (AutoUpdateSaleReturnDoc)obj;
            GetOCRtnRcv(0);
            return "OK";

        }
        private void GetOCRtnRcv(int page_no)
        {
            try
            {
                Dictionary<string, string> list = WDTChelper.GetWDTInfo("wms.stockout.PurchaseReturn.queryWithDetail");
                List<PurchaseReturnqueryWithDetailReq> listreq = new List<PurchaseReturnqueryWithDetailReq>();
                PurchaseReturnqueryWithDetailReq req = new PurchaseReturnqueryWithDetailReq();
                req.start_time = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                req.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", miscRcvTrans.DocNo), new OqlParam("Org", org.ID));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("采购退货更新", dataJson, org, miscRcvTrans.DocNo, 0);
                                    }
                                   
                                }
                                if (miscRcvTrans.DocType.Code == "RCV93" && miscRcvTrans.Status == RcvStatusEnum.Approving)
                                {
                                    dataJson = JsonHelper.GetWDTUpdatePurRtnRcv(item, org, miscRcvTrans.ID);
                                    fail = FailInfo.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", miscRcvTrans.DocNo), new OqlParam("Org", org.ID));
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
                    GetOCRtnRcv(page_no);
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