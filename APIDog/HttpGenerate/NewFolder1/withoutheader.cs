using System.IO;
using System.Net;
using System.Text;

namespace HttpGenerate
{
    public class WebRequestGenerate
    {
        private HttpWebRequest Request { get; set; }
        private CookieContainer CookieBox { get; set; } = new CookieContainer();

        public string NotGet(string url, string data, TypeHttpMethod method)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
            Request.CookieContainer = CookieBox;
            byte[] buf = Encoding.UTF8.GetBytes(data);

            Request.Method = method.ToString();
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = buf.Length;
            Request.Accept = "application/json";
            Request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

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

            Request.Accept = "application/json";
            Request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

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
        Patch,
        Delete
    }
}