using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFSoft.UBF.Execution.Configuration;

namespace UFIDA.U9.LH.LHPubBP
{
    /// <summary>
    /// 旺店通帮助类
    /// </summary>
    public class WDTChelper
    {
        #region 测试
        //public static string sid = "wdtapi3";//测试卖家账号
        //public static string key = "sgy3-test";//测试接口账号
        //public static string appsecret = "6835ba1dc:5468021089055275111f740307b7fed9";//测试
        #endregion
        public static string sid = "sgy3";//正式卖家账号
        public static string key = "34132811";//正式接口账号
       // public static string appsecret = "6835ba1dc:5468021089055275111f740307b7fed9";//测试
        public static string appsecret = "b9a5daa2bac58b29d6e6e22ecc083c24:57641573811230246500fba4f5cc5b5e";//正式
        public static string secret = "b9a5daa2bac58b29d6e6e22ecc083c24";
        public static string salt = "57641573811230246500fba4f5cc5b5e";
        public static string apiUrl = "http://wdt.wangdian.cn/openapi?";//正式地址
 

        //用于奇门信息
        public static string target_app_key = "21363512";//旺店通appkey
        public static string qmsid = "sgy3";//蔬果园卖家账号
        public static string qmappkey = "34132811";//自己的奇门appkey
        public static string qmsecret = "cabc2beaae29d370e60cd2908e5d22fd";//自己的奇门secret
        public static string qmapiUrl = "http://3ldsmu02o9.api.taobao.com/router/qm";//奇门正式环境地址

        /// <summary>
        /// 获取旺店通的接口配置
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetWDTInfo(string method)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("key", WDTChelper.key);
            list.Add("method", method);
            list.Add("salt", WDTChelper.salt);
            list.Add("sid", WDTChelper.sid);
            list.Add("timestamp", (DateTimeOffset.Now.ToUnixTimeSeconds() - 1325347200).ToString());
            list.Add("v", "1.0");
            return list;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <returns></returns>
        public static string GetWDTSign(Dictionary<string, string> result)
        {
            string sign = "";
            result = result.OrderBy(o => o.Key).ToDictionary(o => o.Key, o => o.Value);
            foreach (var item in result.Keys)
            {
                sign += item + result[item];
            }

            sign = secret + sign + secret;
            string mdg5sign = GetMD5(sign);

           // CHelper.InsertU9Log(true, "签名", mdg5sign, sign);
            return mdg5sign;
        }

        /// <summary>
        /// 字段拼接URL
        /// </summary>
        /// <returns></returns>
        public static string GetUrlApend(Dictionary<string, string> result)
        {
            string surl = "";

            foreach (var item in result.Keys)
            {
                if (item == "body") continue;
                surl += item + "=" + result[item] + "&";
            }
            surl = surl.TrimEnd(new char[] { '&' });
            return surl;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                string hex = s[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }
            return result.ToString().ToLower();
        }
    }
}
