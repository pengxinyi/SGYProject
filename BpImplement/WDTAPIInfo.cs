using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP
{
    public static class WDTAPIInfo
    {
        public static string sid = "wdtapi3";
        public static string key = "sgy3-test";
        public static string appsecret = "6835ba1dc:5468021089055275111f740307b7fed9";
        public static string secret = "6835ba1dc";
        public static string salt = "5468021089055275111f740307b7fed9";
        public static string apiUri = "http://47.92.239.46/openapi";

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetWDTSign(string method)
        {
            string sign = "";
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("body", "");
            result.Add("key","");
            result.Add("method", "");
            result.Add("salt", "");
            result.Add("sid", "");
            result.Add("timestamp", "");
            result.Add("v", "");


            return sign;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.Default.GetBytes(str));
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
            return result.ToString();
        }

    }

}
