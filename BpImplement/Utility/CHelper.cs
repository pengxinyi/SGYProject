using Newtonsoft.Json.Serialization;
using QimenCloud.Api.scene3ldsmu02o9.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Top.Api.Domain;
using UFIDA.U9.Base;
using UFIDA.U9.Base.FlexField.ValueSet;
using UFIDA.U9.Base.Organization;
using UFIDA.U9.CBO.Pub.Controller;
using UFIDA.U9.CBO.SCM.Customer;
using UFIDA.U9.CBO.SCM.Customs;
using UFIDA.U9.CBO.SCM.Item;
using UFIDA.U9.CBO.SCM.Warehouse;
using UFIDA.U9.LH.LHPubBE.SalesShipBE;
using UFIDA.U9.PM.Rcv;
using UFSoft.UBF.Business;
using UFSoft.UBF.PL;
using UFSoft.UBF.Transactions;
using UFSoft.UBF.UI;
using UFSoft.UBF.UI.IView;
using UFSoft.UBF.UI.JMF.Forms.FormProcess;
using UFSoft.UBF.Util.Context;
using UFSoft.UBF.Util.DataAccess;
using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockoutSalesQuerywithdetailResponse;

namespace UFIDA.U9.LH.LHPubBP
{
    public class CHelper
    {
        /// <summary>
        /// 企业编码
        /// </summary>
        public static string EntCode = "03";
        /// <summary>
        /// 委外收货操作类型
        /// </summary>
        public static string PMRcvOptype = "PMRcvCreate";
        /// <summary>
        /// 来源出货计划出货操作类型
        /// </summary>
        public static string ShipSrcShipOptype = "ShipSrcShipLineCreate";
        /// <summary>
        /// 杂收单操作类型
        /// </summary>
        public static string MiscRcvTransOptype = "MiscRcvTransCreate";
        /// <summary>
        /// 来源出货退回处理操作类型
        /// </summary>
        public static string RMAOptype = "RMACreateShip";
        /// <summary>
        /// 来源销售出货操作类型
        /// </summary>
        public static string ShipSrcSOOptype = "ShipSrcSOLineCreate";
        /// <summary>
        /// 销售订单操作类型
        /// </summary>
        public static string SOOptype = "SOCreate";
        /// <summary>
        /// 杂发单操作类型
        /// </summary>
        public static string MiscShipOptype = "TransInToMiscShipCreate";
        /// <summary>
        /// 形态转换单操作类型
        /// </summary>
        public static string  ShiftDocOptype = "TransferFormCreate";
        /// <summary>
        /// 采购收货操作类型
        /// </summary>
        public static string ReceivementSrcPOOptype = "ReceivementSrcPOCreate";
        /// <summary>
        /// 无来源采购收货操作类型
        /// </summary>
        public static string  ReceivementNoSrcPOOptype = "ReceivementCreate";
        /// <summary>
        /// 无来源出货
        /// </summary>
        public static string ShipOptype = "ShipCreate";
        /// <summary>
        /// 无来源出货审核
        /// </summary>
        public static string ShipApproveOptype = "ShipApprove";
        /// <summary>
        /// 料品转换单审核
        /// </summary>
        public static string  TransferFormApproveOptype = "TransferFormApprove";
        /// <summary>
        /// 无来源退回处理
        /// </summary>
        public static string NoSrcRMAOptype = "RMACreate";
        /// <summary>
        /// 应收单操作类型
        /// </summary>
        public static string  AROptype = "ARBillCreate";
        /// <summary>
        /// 委外退料单操作类型
        /// </summary>
        public static string  RtnPMOptype = "RtnPMIssueCreate";
        /// <summary>
        /// 委外退料单审核操作类型
        /// </summary>
        public static string  ApproveRtnPMOptype = "RtnPMIssueApprove";
        /// <summary>
        /// 委外收货单审核操作类型
        /// </summary>
        public static string ApprovePMRcvOptype = "PMRcvApprove";
        /// <summary>
        /// 调入单操作类型
        /// </summary>
        public static string  TransferOptype = "TransferInCreate";
        /// <summary>
        /// 界面弹出窗方法
        /// </summary>
        /// <param name="part"></param>
        /// <param name="theMessage"></param>
        public static void ShowAlertMessage(IPart part, string theMessage)
        {
            BaseWebForm webPart = part as BaseWebForm;
            if (webPart == null) return;

            string alertScript = GetAlertScript(theMessage);
            AtlasHelper.RegisterAtlasStartupScript(webPart.Page, webPart.Page.GetType(), Guid.NewGuid().ToString(), alertScript, false);
        }
        private static string GetAlertScript(string theMessage)
        {
            theMessage = ReplaceLawlessCharForShowAlertScript(theMessage);
            return ("<script language=\"javascript\">\n alert('" + theMessage + "'); </script>\n");
        }
        private static string ReplaceLawlessCharForShowAlertScript(string message)
        {
            message = message.Replace(@"\r", "\r");
            message = message.Replace(@"\n", "\n");
            message = message.Replace("\r", @"\r");
            message = message.Replace("\n", @"\n");
            return message;
        }
        /// <summary>
        /// 切换登入组织
        /// </summary>
        /// <param name="OrgCode">目标组织编码</param>
        public static void SetLoginOrg(string OrgCode)
        {
            Organization orgInfo = Organization.FindByCode(OrgCode);
            PlatformContext context = PlatformContext.Current;
            new CBO.Pub.Controller.ContextDTO
            {
                UserCode = context.UserCode,
                EntCode = context.EnterpriseID,
                CultureName = context.Culture,
                OrgCode = orgInfo.Code,
                OrgID = orgInfo.ID
            }.WriteToContext();
        }
        /// <summary>
        /// DB获取
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetStrInfo(string sql)
        {
            string ret = string.Empty;
            DataSet dsT = new DataSet();
            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out dsT);
            if (dsT != null && dsT.Tables.Count > 0 && dsT.Tables[0].Rows.Count > 0)
            {
                ret = dsT.Tables[0].Rows[0][0].ToString();

            }

            return ret;
        }
        /// <summary>
        ///插入日志表
        /// </summary>

        public static void InsertU9Log(bool isSuccess, string sDocType, string rtnMsg, string json, string url, string docNo)
        {
            using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.RequiresNew))
            {
                try
                {
                    //将信息写入日志中
                    using (ISession session = Session.Open())
                    {
                        LHPubBE.LogBE.AutoLog log = LHPubBE.LogBE.AutoLog.Create();
                        DateTime dt = DateTime.Now;
                        log.Org = Context.LoginOrg;
                        log.DocType = sDocType;
                        log.CreatedOn = dt;
                        log.IsSuccess = isSuccess;
                        log.DocNo = docNo;
                        log.ModifiedOn = dt;
                        if (!string.IsNullOrEmpty(rtnMsg) && (rtnMsg.Contains("&") || rtnMsg.Contains("<") || rtnMsg.Contains(">")))
                        {
                            rtnMsg = rtnMsg.Replace("&", "&amp;");
                            rtnMsg = rtnMsg.Replace("<", "&lt;");
                            rtnMsg = rtnMsg.Replace(">", "&gt;");
                        }

                        if (!string.IsNullOrEmpty(json) && (json.Contains("&") || json.Contains("<") || json.Contains(">")))
                        {
                            json = json.Replace("&", "&amp;");
                            json = json.Replace("<", "&lt;");
                            json = json.Replace(">", "&gt;");
                        }
                        log.Msg = rtnMsg?.ToString();
                        log.Json = json?.ToString();
                        log.Url = url;
                        session.InList(log);
                        session.Commit();
                    }
                    scope.Commit();

                    //清空日志 只保留7天的日志
                    //string sql = @"DELETE Cust_AutoLogBE WHERE CreatedOn<DATEADD(DAY, -7, GETDATE());";
                    //DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                }
            }
        }


        public static void InsertU9Log(bool isSuccess, string sDocType, string rtnMsg, string json)
        {
            InsertU9Log(isSuccess, sDocType, rtnMsg, json, "");
        }
        public static void InsertU9Log(bool isSuccess, string sDocType, string rtnMsg, string json, string docNo)
        {
            InsertU9Log(isSuccess, sDocType, rtnMsg, json, "", docNo);
        }
        
        /// <summary>
        /// 接口地址
        /// </summary>
        /// <returns></returns>
        public static string GetDefineValueUrl(string sZJCode, string sCode)
        {
            DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code",
                                                new OqlParam("ZJCode", sZJCode),
                                                new OqlParam("Code", sCode)
                                                );
            if (dv == null)
                throw new Exception("没有配置值集:" + sZJCode + ",请先维护！");

            if (dv != null && string.IsNullOrEmpty(dv.Name))
            {
                throw new Exception("没有配置值集:" + sZJCode + "编码：" + sCode + "的名称！请维护。");
            }
            return dv.Name;
        }

        /// <summary>
        /// 插入推送记录表
        /// </summary>
        /// <param name="sDocType"></param>
        /// <param name="json"></param>
        /// <param name="docNo"></param>
        public static void InsertFailInfo(string sDocType, string json, Organization org, string docNo, int IsSuccess, string reason = "", string srcdocno = "")
        {
            using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.RequiresNew))
            {
                try
                {
                    //将信息写入日志中
                    using (ISession session = Session.Open())
                    {
                        LHPubBE.FailInfoBE.FailInfo failInfo = LHPubBE.FailInfoBE.FailInfo.Create();
                        // LHPubBE.SalesShipBE.SaleShip saleShip = LHPubBE.SalesShipBE.SaleShip.Create();
                        DateTime dt = DateTime.Now;
                        failInfo.Org = org;
                        failInfo.DocType = sDocType;
                        failInfo.CreatedOn = dt;
                        failInfo.DocNo = docNo;
                        failInfo.SrcDocNo = srcdocno;
                        failInfo.ISSuccess = CreateDocStateEnum.GetFromValue(IsSuccess);
                        failInfo.Reason = reason;
                        if (!string.IsNullOrEmpty(json) && (json.Contains("&") || json.Contains("<") || json.Contains(">")))
                        {
                            json = json.Replace("&", "&amp;");
                            json = json.Replace("<", "&lt;");
                            json = json.Replace(">", "&gt;");
                        }
                        failInfo.Json = json.ToString();
                        session.InList(failInfo);
                        session.Commit();
                    }
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                }
            }
        }

        /// <summary>
        /// 插入销售出库订单明细
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Detail"></param>
        /// <param name="org"></param>
        ///  /// <param name="CusCode"></param>
        public static void InsertSaleShip(WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item, DetailsListDomain Detail, Organization org,Customer cus,string CusCode="")
        {
            using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.RequiresNew))
            {
                try
                {

                    using (ISession session = Session.Open())
                    {
                        LHPubBE.SalesShipBE.SaleShip saleShip = LHPubBE.SalesShipBE.SaleShip.Create();
                        DateTime dt = DateTime.Now;
                        saleShip.Org = org;
                        saleShip.CreatedOn = dt;
                        saleShip.DocNo = item.OrderNo;
                        saleShip.SrcDocNo = item.SrcTradeNo;
                        saleShip.ShipDate = Convert.ToDateTime(item.ConsignTime); // Convert.ToDateTime(CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.ConsignTime)));
                        saleShip.TradeFrom = Convert.ToInt32(item.TradeFrom);
                        saleShip.IsPriceIncludeTax = Convert.ToDecimal(item.TaxRate) > 0 ? true : false;
                        saleShip.Status = CreateDocStateEnum.Undo;
                        saleShip.CusCode = cus?.Code;
                        saleShip.CusName = cus?.Name;

                        ItemMaster itemMaster = ItemMaster.Finder.Find("Code=@Code and Org = @Org", new OqlParam("Code", Detail.GoodsNo), new OqlParam("Org", org?.ID));
                        saleShip.ItemCode = Detail.GoodsNo;
                        saleShip.ItemName = itemMaster?.Name;
                        Warehouse wh = Warehouse.Finder.Find("Code=@Code and Org = @Org", new OqlParam("Code", item.WarehouseNo), new OqlParam("Org", org?.ID));
                        saleShip.WHCode = item.WarehouseNo;
                        saleShip.WHName = wh.Name;
                        saleShip.Amount = Convert.ToDecimal(Detail.GoodsCount);
                        saleShip.FreeType = Detail.GiftType==0?-1:0;
                        saleShip.SellPrice = Convert.ToDecimal(Detail.SellPrice);
                        saleShip.TotalMny = Convert.ToDecimal(Detail.ShareAmount);
                        saleShip.RecID = Detail.RecId;
                        saleShip.SO = CusCode;
                        session.InList(saleShip);
                        session.Commit();
                    }
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                }
            }
        }
        /// <summary>
        /// 地址解析
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        //public static (string province, string city, string county, string town, string village) Analysis(string address)
        //{
        //    string regex = "(?<province>[^省]+自治区|.*?省|.*?行政区|.*?市)?(?<city>[^市]+自治州|.*?地区|.*?行政单位|.+盟|市辖区|.*?市|.*?县)?(?<county>[^县]+县|.+区|.+市|.+旗|.+海域|.+岛)?(?<town>[^区]+区|.+镇)?(?<village>.*)";

        //    var m = Regex.Match(address, regex, RegexOptions.IgnoreCase);

        //    var province = m.Groups["province"].Value;
        //    var city = m.Groups["city"].Value;
        //    var county = m.Groups["county"].Value;
        //    var town = m.Groups["town"].Value;
        //    var village = m.Groups["village"].Value;

        //    return (province, city, county, town, village);
        //}
        /// <summary>
        /// 获取组织编码
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public static Organization GetOrg(string itemcode, string itemcode1)
        {
            Organization org = null;
            ItemMaster item = null;
            if (!string.IsNullOrEmpty(itemcode1))
            {
                item = ItemMaster.Finder.Find("Code1=@Code", new UFSoft.UBF.PL.OqlParam("Code", itemcode1));
            }
            else
            {
                item = ItemMaster.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", itemcode));
            }

            if (item != null)
            {
                // 8.24 从料品主分类取值集改为私有字段15
                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1011"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg15));
                if (dv != null)
                {
                    org = Organization.FindByCode(dv.Description);
                }

            }

            return org;

        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetStr(object obj)
        {
            string str = string.Empty;
            if (obj != null && obj != DBNull.Value)
                str = obj.ToString();

            return str;
        }
        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetNumeric(string code)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in code)
            {
                if (!char.IsPunctuation(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();

        }

        /// <summary>
        /// 13位时间戳转 日期格式   1652338858000 -> 2022-05-12 03:00:58
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string GetDateTimeMilliseconds(long timestamp)
        {
            long begtime = timestamp * 10000;
            DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
            long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
            long time_tricks = tricks_1970 + begtime;//日志日期刻度
            DateTime dt = new DateTime(time_tricks);//转化为DateTime
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 存储地点
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static UFIDA.U9.CBO.SCM.Warehouse.Warehouse GetWH(string Code)
        {
            string opath = string.Format(@" Effective.IsEffective=1 and Effective.EffectiveDate <= getdate() and Effective.DisableDate > getdate() and Org.ID={0} and Code='{1}'", Context.LoginOrg.ID, Code);
            return UFIDA.U9.CBO.SCM.Warehouse.Warehouse.Finder.Find(opath, new OqlParam[0]);
        }

        /// <summary>
        /// 更新请求执行结果错误信息
        /// </summary>
        /// <param name="sRequestCode"></param>
        /// <param name="sErrorMsg"></param>
        public static void UpdateRequsetError(string sRequestCode, string sErrorMsg)
        {
            object obj =new object();
            lock (obj)
            {
                string strSql = string.Format(@"
DECLARE  @MaxJobID bigint
SELECT @MaxJobID= MAX(b.ID) FROM dbo.UBF_Job_Request AS a
INNER JOIN dbo.UBF_Job_DailyPattern AS b ON a.ID = b.Request
WHERE a.Code = '{0}'
UPDATE UBF_Job_DailyPattern SET ExceptionMsg= '{1}' WHERE ID =@MaxJobID", sRequestCode, sErrorMsg);
                DataAccessor.RunSQL(DataAccessor.GetConn(), strSql, null);
            }

        }
        /// <summary>
        /// /判断是否退补
        /// </summary>
        /// <param name="rcv"></param>
        /// <returns></returns>
        public static bool IsRtnFill(Receivement rcv)
        {
            bool b = false;
            foreach (RcvLine item in rcv.RcvLines)
            {
                if (item.RtnFillQtyPU>0)
                {
                    b = true;
                }
            }
            return b;
        }
    }
}
