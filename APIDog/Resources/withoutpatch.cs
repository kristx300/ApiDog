using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpGenerate
{
    public class HttpClientGenerate : IDisposable
    {
        private HttpClient HttpClient { get; set; }

        public async Task<string> NotGet(string url, string data, string method)
        {
            var values = new Dictionary<string, string>();
            foreach (var item in data.Split('&'))
            {
                string[] val = item.Split('=');
                values[val[0]] = val[1];
            }
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage hrm;
            switch (method)
            {
                case TypeHttpMethod.Post:
                    hrm = await HttpClient.PostAsync(url, content);
                    break;

                case TypeHttpMethod.Put:
                    hrm = await HttpClient.PutAsync(url, content);
                    break;

                case TypeHttpMethod.Delete:
                    hrm = await HttpClient.DeleteAsync(url);
                    break;

                default:
                    throw new InvalidOperationException("Not exist Http method");
            }

            return await hrm.Content.ReadAsStringAsync();
        }

        public async Task<string> Get(string url)
        {
            return await HttpClient.GetStringAsync(url);
        }

        public void Dispose()
        {
            ((IDisposable)HttpClient).Dispose();
        }
    }
}