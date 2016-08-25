using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace HttpGenerate
{
    public class WebClientGenerate : IDisposable
    {
        private WebClient WebClient { get; set; } = new WebClient();

        public string NotGet(string url, string data, TypeHttpMethod method)
        {
            var values = new NameValueCollection();
            foreach (var item in data.Split('&'))
            {
                string[] val = item.Split('=');
                values[val[0]] = val[1];
            }
            var response = WebClient.UploadValues(url, method.ToString(), values);

            return Encoding.UTF8.GetString(response);
        }

        public string Get(string url)
        {
            return WebClient.DownloadString(url);
        }

        public void Dispose()
        {
            ((IDisposable)WebClient).Dispose();
        }
    }
    public enum TypeHttpMethod
    {
        Post,
        Put,
        Update,
        Delete
    }
}