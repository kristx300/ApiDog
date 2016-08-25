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
        private CookieContainer CookieBox { get; set; } = new CookieContainer();
        public string NotGet(string url, string data, TypeHttpMethod method, WebHeaderCollection headers)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
            Request.CookieContainer = CookieBox;
            byte[] buf = Encoding.UTF8.GetBytes(data);

            Request.Method = method.ToString();
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = buf.Length;
            Request.Accept = headers[HttpRequestHeader.Accept];
            Request.Connection = headers[HttpRequestHeader.Connection];
            Request.KeepAlive = Convert.ToBoolean(headers[HttpRequestHeader.KeepAlive]);
            Request.Referer = headers[HttpRequestHeader.Referer];
            Request.UserAgent = headers[HttpRequestHeader.UserAgent];

            var heads = new HttpRequestHeader[] { HttpRequestHeader.Accept, HttpRequestHeader.Connection,
                                                    HttpRequestHeader.KeepAlive, HttpRequestHeader.Referer,
                                                    HttpRequestHeader.UserAgent, HttpRequestHeader.ContentLength,
                                                    HttpRequestHeader.ContentType};
            var items = Enumerable
                .Range(0, headers.Count)
                .SelectMany(i => headers.GetValues(i)
                .Select(v => Tuple.Create(headers.GetKey(i), v)))
                .Where
                    (x => !heads
                        .Select(y => y.ToString())
                        .Contains(x.Item1)
                    )
                .ToList();
            foreach (var item in items)
            {
                Request.Headers.Add(item.Item1, item.Item2);
            }
            using (var stream = Request.GetRequestStream()) 
                stream.Write(buf, 0, data.Length);

            var response = (HttpWebResponse)Request.GetResponse();

            if (response.Cookies != null && response.Cookies.Count != 0)
                foreach (Cookie item in response.Cookies)
                    CookieBox.Add(item);

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        public string Get(string url, WebHeaderCollection headers)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);

            Request.CookieContainer = CookieBox;
            Request.Accept = headers[HttpRequestHeader.Accept];
            Request.Connection = headers[HttpRequestHeader.Connection];
            Request.KeepAlive = Convert.ToBoolean(headers[HttpRequestHeader.KeepAlive]);
            Request.Referer = headers[HttpRequestHeader.Referer];
            Request.UserAgent = headers[HttpRequestHeader.UserAgent];

            var heads = new HttpRequestHeader[] { HttpRequestHeader.Accept, HttpRequestHeader.Connection,
                                                    HttpRequestHeader.KeepAlive, HttpRequestHeader.Referer,
                                                    HttpRequestHeader.UserAgent, HttpRequestHeader.ContentLength,
                                                    HttpRequestHeader.ContentType};
            var items = Enumerable
                .Range(0, headers.Count)
                .SelectMany(i => headers.GetValues(i)
                .Select(v => Tuple.Create(headers.GetKey(i), v)))
                .Where
                    (x => !heads
                        .Select(y => y.ToString())
                        .Contains(x.Item1)
                    )
                .ToList();
            foreach (var item in items)
            {
                Request.Headers.Add(item.Item1, item.Item2);
            }

            var response = (HttpWebResponse)Request.GetResponse();

            if (response.Cookies != null && response.Cookies.Count != 0)
                foreach (Cookie item in response.Cookies)
                    CookieBox.Add(item);

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }

    public enum TypeHttpMethod
    {
        Post,
        Put,
        Update,
        Delete
    }
    public class WebRequestHeader
    {
        public HttpRequestHeader RequestHeader { get; set; }
        public string Value { get; set; }
    }
}