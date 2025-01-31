using FastJSON;
using QimenCloud.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Utility
{
    public class WdtUtils
    {
        private static string[] QIMEN_CRM_SIGNED_FIELDS =
        {
            "pageNo", "pageSize", "fields", "extendProps", "customerid", "method", "sd_code", "startModified",
            "endModified"
        };
        //private static string[] QIMEN_JSON_FIELDS = { "params", "pager", "extendProps" };
        private static string[] QIMEN_EXCLUDE_SIGN_FIELDS = { "wdt3_customer_id", "wdt_sign" };

        public static string GetQimenOfficialWdtSign<T>(BaseQimenCloudRequest<T> request, string wdtSecret) where T : QimenCloudResponse
        {
            StringBuilder stringBuilder = new StringBuilder();
            var paramsDict = new Dictionary<string, object>();
            paramsDict.Add("method", StripPrefix(request.GetApiName(), "qimen."));
            foreach (KeyValuePair<string, string> pair in request.GetParameters())
            {
                if (Array.IndexOf(QIMEN_CRM_SIGNED_FIELDS, pair.Key) >= 0)
                    paramsDict.Add(pair.Key, pair.Value);
            }

            GetToBeSignedString(stringBuilder, paramsDict);
            string toBeSigned = stringBuilder.Insert(0, wdtSecret).Append(wdtSecret).ToString();

            return GetMd5(toBeSigned);
        }

        public static string GetQimenCustomWdtSign<T>(BaseQimenCloudRequest<T> request, string wdtSecret) where T : QimenCloudResponse
        {
            StringBuilder stringBuilder = new StringBuilder();
            var paramsDict = request.GetParameters();
            paramsDict.Add("method", request.GetApiName());
            GetToBeSignedString(stringBuilder, paramsDict);
            string toBeSigned = stringBuilder.Insert(0, wdtSecret).Append(wdtSecret).ToString();

            return GetMd5(toBeSigned);
        }

        private static bool IsValidJson(string content)
        {
            try
            {
                var trimmedContent = content.Trim();
                if (!(trimmedContent.StartsWith("{") && trimmedContent.EndsWith("}"))
                    && !(trimmedContent.StartsWith("[") && trimmedContent.EndsWith("]")))
                    return false;

                JSON.Parse(content);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static void GetToBeSignedString(StringBuilder stringBuilder, object obj)
        {
            if (obj is IDictionary)
            {
                var json = JSON.ToJSON(obj);
                var dict = new SortedDictionary<string, object>((IDictionary<string, object>)JSON.Parse(json));

                foreach (string dictKey in dict.Keys)
                {
                    var value = dict[dictKey];
                    if (Array.IndexOf(QIMEN_EXCLUDE_SIGN_FIELDS, dictKey) >= 0 || null == value)
                        continue;
                    stringBuilder.Append(dictKey);

                    if (value is string)
                    {
                        //if (Array.IndexOf(QIMEN_JSON_FIELDS, dictKey) >= 0)
                        if (IsValidJson((string)value))
                        {
                            GetToBeSignedString(stringBuilder, JSON.Parse((string)value));
                        }
                        else
                        {
                            stringBuilder.Append(value);
                        }
                    }
                    else if (value is bool)
                    {
                        stringBuilder.Append(value.ToString().ToLower());
                    }
                    else
                    {
                        GetToBeSignedString(stringBuilder, JSON.Parse(JSON.ToJSON(value)));
                    }
                }
            }
            else if (obj is IList<object> || obj is Array)
            {
                foreach (object o in (List<object>)obj)
                {
                    GetToBeSignedString(stringBuilder, o);
                }
            }
            else
            {
                stringBuilder.Append(obj);
            }
        }

        private static string StripPrefix(string text, string prefix)
        {
            return text.StartsWith(prefix) ? text.Substring(prefix.Length) : text;
        }

        private static string GetMd5(string context)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(context));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

    }
}
