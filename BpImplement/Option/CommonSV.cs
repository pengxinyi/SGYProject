using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using UFIDA.U9.LH.LHPubBP.Model;

namespace UFIDA.U9.LH.LHPubBP.Option
{
    /// <summary>
    /// 生成U9单据方法类
    /// </summary>
   public  class CommonSV
    {
        /// <summary>
        /// 创建U9杂发单
        /// </summary>
        public void CreateMiscShip(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "奇门杂发单创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "奇门杂发单创建", apirEEEeuslt, dataJson, "");
            }
        }

        /// <summary>
        /// 创建U9杂收单
        /// </summary>
        public void CreateMiscRcvTrans(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            //posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "奇门杂收单创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "奇门杂收单创建", apirEEEeuslt, dataJson, "");
            }
        }
        /// <summary>
        /// 创建U9委外收货单
        /// </summary>
        public void CreatePMRcv(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            //posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "委外收货创建", apirEEEeuslt, dataJson, null);
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "委外收货创建", apirEEEeuslt, dataJson, null);
            }
        }
        /// <summary>
        /// 创建U9来源出货退回处理
        /// </summary>
        public void CreateSrcShipRMA(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "来源出货退回处理单创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "来源出货退回处理单创建", apirEEEeuslt, dataJson, "");
            }
        }
        /// <summary>
        /// 创建U9来源出货计划出货单
        /// </summary>
        public void CreateSrcShipFromShipPlan(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "来源出货计划出货单创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "来源出货计划出货单创建", apirEEEeuslt, dataJson, "");
            }
        }
        /// <summary>
        /// 创建U9销售订单
        /// </summary>
        public RtnDataJson CreateSO(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            return rtns;
        }
        /// <summary>
        /// 创建U9来源销售订单出货单
        /// </summary>
        public void CreateSrcShipFromSO(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "来源销售订单出货单创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "来源销售订单出货单创建", apirEEEeuslt, dataJson, "");
            }
        }
        /// <summary>
        /// 创建U9收货单
        /// </summary>
        public void CreateReceivement(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "收货单创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "收货单创建", apirEEEeuslt, dataJson, "");
            }
        }
        /// <summary>
        /// 创建U9标准销售
        /// </summary>
        public void CreateQMSO(string dataJson)
        {
            string posturl = string.Empty;
            string apirEEEeuslt = string.Empty;
            // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
            apirEEEeuslt = Tools.HttpPost(posturl, dataJson);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
            if (!rtns.IsSuccess)
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(false, "奇门标准销售创建", apirEEEeuslt, dataJson, "");
            }
            else
            {
                //写入日志
                if (!string.IsNullOrEmpty(dataJson))
                    CHelper.InsertU9Log(true, "奇门标准销售创建", apirEEEeuslt, dataJson, "");
            }
        }
    }
}
