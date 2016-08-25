using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpGenerate
{
    public class WebRequestGenerate
    {
        private HttpWebRequest Request { get; set; }
        public string NotGet(string url, string data, string method)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
            byte[] buf = Encoding.UTF8.GetBytes(data);

            Request.Method = method;
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = buf.Length;
            Request.Accept = "application/json";
            Request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

            using (var stream = Request.GetRequestStream()) 
                stream.Write(buf, 0, data.Length);

            var response = (HttpWebResponse)Request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public string Get(string url, WebHeaderCollection headers)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);

            Request.Accept = "application/json";
            Request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

            var response = (HttpWebResponse)Request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}