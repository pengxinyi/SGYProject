using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP
{
    public class Tools
    {
        #region 数据转换
        public static DateTime ConvertToDateTime(object value)
        {
            return ((value == null) || (value.ToString().Length <= 0)) ? DateTime.MinValue : Convert.ToDateTime(value);
        }
        public static long ConvertToInt64(object value, bool isReference)
        {
            if ((value != null) && (value.ToString().Length > 0))
            {
                return Convert.ToInt64(value);
            }
            return (isReference ? -1L : 0L);
        }

        public static long ConvertToInt64(object value)
        {
            if ((value != null) && (value.ToString().Length > 0))
            {
                return Convert.ToInt64(value);
            }
            return 0L;
        }
        public static int ConvertToInt32(object value)
        {
            if ((value != null) && (value.ToString().Length > 0))
            {
                return Convert.ToInt32(value);
            }
            return 0;
        }
        public static int ConvertToInt32(object value, bool isEnum)
        {
            return ((value == null) || (value.ToString().Length <= 0)) ? 0 : Convert.ToInt32(value);
        }
        public static bool ConvertToBoolean(object value)
        {
            if (value.ToString().Length == 1)
            {
                return ((value != null) && (value.ToString().Length > 0)) && (Convert.ToBoolean(value.ToString().Equals("1") ? true : false));
            }
            else
            {
                return ((value != null) && (value.ToString().Length > 0)) && Convert.ToBoolean(value.ToString().ToLower().Equals("true") ? true : false);
            }
        }
        public static Decimal ConvertToDecimal(object value)
        {
            if ((value != null) && (value.ToString().Length > 0))
            {
                return Convert.ToDecimal(value);
            }
            return 0M;
        }

        #endregion

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookie, string jsonData, Dictionary<string, string> heads)
        {
            try
            {
                //解决未能创建 SSL/TLS 安全通道
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;



                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.Timeout = 100000;
                webRequest.ContentType = "application/json;charset=utf-8";
                webRequest.CookieContainer = cookie;

                foreach (string item in heads.Keys)
                {
                    webRequest.Headers.Add(item, heads[item]);
                }
                byte[] dataEncode = Encoding.UTF8.GetBytes(jsonData);
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(dataEncode, 0, dataEncode.Length);
                requestStream.Close();
                HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                stream = rsp.GetResponseStream();
                StreamReader streamReader = new StreamReader(rsp.GetResponseStream(), encoding);
                string responseContent = streamReader.ReadToEnd();
                if (streamReader != null)
                    streamReader.Close();
                if (stream != null)
                    stream.Close();
                if (rsp != null)
                    rsp.Close();
                return responseContent;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("请求抛错【{0}】", ex.Message));
            }
        }


        /// <summary>
        /// 发送请求
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, string json)
        {
            try
            {
                //解决未能创建 SSL/TLS 安全通道
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;



                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.Timeout = 300000;
                webRequest.ContentType = "application/json;charset=utf-8";


                byte[] dataEncode = Encoding.UTF8.GetBytes(json);
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(dataEncode, 0, dataEncode.Length);
                requestStream.Close();

                HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                stream = rsp.GetResponseStream();
                StreamReader streamReader = new StreamReader(rsp.GetResponseStream(), encoding);
                string responseContent = streamReader.ReadToEnd();
                if (streamReader != null)
                    streamReader.Close();
                if (stream != null)
                    stream.Close();
                if (rsp != null)
                    rsp.Close();
                return responseContent;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("请求抛错【{0}】", ex.Message));
            }
        }
        /// <summary>
        /// token 发送请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string HttpPost(string posturl, string postData, string token)
        {
            string url = posturl;
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            //Encoding encoding = Encoding.UTF8;
            //string postD=postData.Replace("\\","");
            byte[] data = Encoding.UTF8.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.Accept = "application/json,*/*;";
                //request.UserAgent = @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.KeepAlive = false;
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Add("token", token);
                }
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码

                string content = sr.ReadToEnd();
                string err = string.Empty;
                outstream.Close();
                return content;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return err;
            }
        }
        /// <summary>
        /// 获取唯一ID
        /// </summary>
        /// <returns></returns>
        public static long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        public static string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }
}
