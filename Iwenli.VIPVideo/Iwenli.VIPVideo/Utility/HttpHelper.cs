using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Iwenli.VIPVideo.Utility
{
    public class HttpHelper
    {
        private string _cookieHeader = string.Empty;
        /// <summary> 
        /// 网站Cookies 
        /// </summary> 
        public string CookieHeader
        {
            get
            {
                return _cookieHeader;
            }
            set
            {
                _cookieHeader = value;
            }
        }

        /// <summary>
        /// 功能描述：模拟登录页面，提交登录数据进行登录，并记录Header中的cookie 
        /// </summary>
        /// <param name="strURL">登录数据提交的页面地址</param>
        /// <param name="strArgs">请求参数</param>
        /// <param name="strReferer">引用地址</param>
        /// <param name="code">网站编码</param>
        /// <param name="method">POST 或 GET</param>
        /// <param name="contentType">设置 Content-typeHTTP 标头的值</param>
        /// <returns></returns>
        public string SendData(string strURL, string strArgs,
            string strReferer, string code = "utf-8", string method = "POST",
            string contentType = "application/x-www-form-urlencoded; charset=UTF-8")
        {
            try
            {
                string strResult = "";
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);
                myHttpWebRequest.AllowAutoRedirect = true;
                myHttpWebRequest.KeepAlive = true;
                myHttpWebRequest.Accept = "application/json, text/javascript, */*; q=0.01";
                myHttpWebRequest.Referer = strReferer;
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
                myHttpWebRequest.Method = method;
                myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                myHttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;

                myHttpWebRequest.ContentType = contentType;

                if (myHttpWebRequest.CookieContainer == null)
                {
                    myHttpWebRequest.CookieContainer = new CookieContainer();
                }

                if (this.CookieHeader.Length > 0)
                {
                    myHttpWebRequest.Headers.Add("cookie:" + this.CookieHeader);
                    myHttpWebRequest.CookieContainer.SetCookies(new Uri(strURL), this.CookieHeader);
                }

                byte[] postData = Encoding.GetEncoding(code).GetBytes(strArgs);
                myHttpWebRequest.ContentLength = postData.Length;

                System.IO.Stream PostStream = myHttpWebRequest.GetRequestStream();
                PostStream.Write(postData, 0, postData.Length);
                PostStream.Close();

                HttpWebResponse response = null;
                System.IO.StreamReader sr = null;
                response = (HttpWebResponse)myHttpWebRequest.GetResponse();



                if (myHttpWebRequest.CookieContainer != null)
                {
                    this.CookieHeader = myHttpWebRequest.CookieContainer.GetCookieHeader(new Uri(strURL));
                }

                sr = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding(code));
                strResult = sr.ReadToEnd();
                sr.Close();
                response.Close();
                return strResult;
            }
            catch (Exception ex)
            {

            }
            return strArgs;
        }


        /// <summary>
        /// 功能描述：在PostLogin成功登录后记录下Headers中的cookie，然后获取此网站上其他页面的内容 
        /// </summary>
        /// <param name="strURL">获取网站的某页面的地址</param>
        /// <param name="strReferer">引用的地址</param>
        /// <param name="code">网站编码</param>
        /// <param name="contentType"></param>
        /// <returns>返回页面内容</returns>
        public string GetPage(string strURL, string strReferer, string code = "utf-8",
            string contentType = "application/x-www-form-urlencoded; charset=UTF-8")
        {
            string strResult = "";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(strURL);
            myHttpWebRequest.AllowAutoRedirect = true;
            myHttpWebRequest.KeepAlive = true;
            myHttpWebRequest.Referer = strReferer;
            myHttpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            myHttpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";

            if (string.IsNullOrEmpty(contentType))
            {
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                myHttpWebRequest.ContentType = contentType;
            }
            myHttpWebRequest.Method = "GET";

            if (myHttpWebRequest.CookieContainer == null)
            {
                myHttpWebRequest.CookieContainer = new CookieContainer();
            }

            if (this.CookieHeader.Length > 0)
            {
                myHttpWebRequest.Headers.Add("cookie:" + this.CookieHeader);
                myHttpWebRequest.CookieContainer.SetCookies(new Uri(strURL), this.CookieHeader);
            }


            HttpWebResponse response = null;
            System.IO.StreamReader sr = null;
            response = (HttpWebResponse)myHttpWebRequest.GetResponse();


            Stream streamReceive;
            string gzip = response.ContentEncoding;

            if (string.IsNullOrEmpty(gzip) || gzip.ToLower() != "gzip")
            {
                streamReceive = response.GetResponseStream();
            }
            else
            {
                streamReceive = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
            }

            sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding(code));

            if (response.ContentLength > 1)
            {
                strResult = sr.ReadToEnd();
            }
            else
            {
                char[] buffer = new char[256];
                int count = 0;
                StringBuilder sb = new StringBuilder();
                while ((count = sr.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sb.Append(new string(buffer));
                }
                strResult = sb.ToString();
            }
            sr.Close();
            response.Close();
            return strResult;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="strURL">上传文件路径</param>
        /// <param name="filePath">文件路径(相对路径)</param>
        /// <param name="formData">表单键值对</param>
        /// <returns></returns>
        public static string UploadFile(string strURL, string filePath, NameValueCollection formData)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader r = new BinaryReader(stream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                byte[] dataBytes = r.ReadBytes((int)r.BaseStream.Length);

                return UploadFile(strURL, Path.GetFileName(filePath), dataBytes, formData);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="strURL">上传文件路径</param>
        /// <param name="fileName">文件名(带后缀)</param>
        /// <param name="fileBytes">文件字节流</param>
        /// <param name="formData">表单键值对</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public static string UploadFile(string strURL, string fileName, byte[] fileBytes, NameValueCollection formData, int timeOut = 36000)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(strURL);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            //var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");


            // 写入文件  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, "Filedata", fileName);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            memStream.Write(fileBytes, 0, fileBytes.Length);

            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in formData.Keys
                                             select string.Format(stringKeyHeader, key, formData[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);
            webRequest.ContentLength = memStream.Length;
            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            httpWebResponse.Close();
            webRequest.Abort();
            return responseContent;
        }
    }
}
