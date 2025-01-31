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
    using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// 定时创建委外收货废弃，合并到标准收货
	/// </summary>	
	public partial class AutoCreateOCRcv 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateOCRcvImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateOCRcvImpementStrategy : BaseStrategy
	{
		public AutoCreateOCRcvImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateOCRcv bpObj = (AutoCreateOCRcv)obj;
            GetOCRcv(0);
            return "OK";
        }
        private void GetOCRcv(int page_no)
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
                    PurchaseOrder miscRcvTrans = null;
                    string dataJson = string.Empty;
                    foreach (StockinPurchaseOrderDto item in res.data.order)
                    {
                       
                         miscRcvTrans = PurchaseOrder.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.purchase_no));
                        if (miscRcvTrans != null)
                        {
                            Organization org = CHelper.GetOrg(miscRcvTrans.POLines[0].ItemInfo.ItemCode, "");
                            if (org != null)
                            {
                                if (miscRcvTrans != null && miscRcvTrans.DocType.Code == "PO22")
                                {
                                    dataJson = JsonHelper.GetPurReturn(miscRcvTrans, item, org.Code);
                                    FailInfo fail = FailInfo.Finder.Find("DocNo=@DocNo", new UFSoft.UBF.PL.OqlParam("DocNo", item.order_no));
                                    if (fail == null)
                                    {
                                        CHelper.InsertFailInfo("委外收货创建", dataJson, org, item.order_no, 0, "");
                                    }
                                 
                                    //commonSV.CreateMiscRcvTrans(dataJson);
                                }
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
         
    }

	#endregion
	
	
}