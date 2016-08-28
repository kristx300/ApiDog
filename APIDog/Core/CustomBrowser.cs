using APIDog.Extensions;
using APIDog.Models;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIDog.Core
{
    /// <summary>
    /// Main browser in project
    /// </summary>
    public class CustomBrowser
    {
        private CustomBrowser()
        {
        }

        private static volatile CustomBrowser CBrows;
        private static object syncRoot = new object();

        /// <summary>
        /// Creation of a single browser
        /// </summary>
        /// <returns>Main Browser</returns>
        public static CustomBrowser Create()
        {
            if (CBrows == null)
                lock (syncRoot)
                    if (CBrows == null)
                        CBrows = new CustomBrowser();
            return CBrows;
        }

        /// <summary>
        /// Current request, for abort, if request is not null
        /// </summary>
        private HttpWebRequest Request { get; set; }
        public Task<ResponseData> RequestAsync(RequestData rd)
        {
            return Task.Run(() => MakeRequest(rd));
        }

        /// <summary>
        /// Create request
        /// </summary>
        /// <param name="rd">Data of request</param>
        /// <returns>Response</returns>
        public ResponseData MakeRequest(RequestData rd)
        {
            if (rd.TypeMethod == Enums.TypeHttpMethod.Get)
                Request = (HttpWebRequest)WebRequest.Create(rd.Url);
            else
                Request = (HttpWebRequest)WebRequest.Create(rd.Url.WithoutParams());

            if (Request != null)
            {
                Request.Method = rd.TypeMethod.ToString();
                Request.Accept = rd.Headers.Get(HttpRequestHeader.Accept);
                Request.Connection = rd.Headers.Get(HttpRequestHeader.Connection);
                Request.KeepAlive = Convert.ToBoolean(rd.Headers.Get(HttpRequestHeader.KeepAlive));
                Request.Referer = rd.Headers.Get(HttpRequestHeader.Referer);
                Request.UserAgent = rd.Headers.Get(HttpRequestHeader.UserAgent);
                Request.ContentType = rd.Headers.Get(HttpRequestHeader.ContentType);

                foreach (var item in rd.Headers.Get(HttpRequestHeader.Accept, HttpRequestHeader.Connection,
                                                    HttpRequestHeader.KeepAlive, HttpRequestHeader.Referer,
                                                    HttpRequestHeader.UserAgent, HttpRequestHeader.ContentLength,
                                                    HttpRequestHeader.ContentType))
                {
                    Request.Headers.Add(item.RequestHeader, item.Value);
                }

                if (rd.TypeMethod != Enums.TypeHttpMethod.Get)
                {
                    if (rd.Url.WithoutUrlCount() == 1)
                    {
                        Request.ContentType = "application/x-www-form-urlencoded";
                        Request.ContentLength = 0;
                    }
                    else
                    {
                        byte[] buf = Encoding.UTF8.GetBytes(rd.Url.WithoutUrl());
                        Request.ContentType = "application/x-www-form-urlencoded";
                        Request.ContentLength = buf.Length;
                        Request.GetRequestStream().Write(buf, 0, buf.Length);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                return ResponseData.Create(response);
            }
            return null;
        }

        /// <summary>
        /// Abort of request, if request is not null
        /// </summary>
        public void Abort()
        {
            Request?.Abort();
        }
    }
}