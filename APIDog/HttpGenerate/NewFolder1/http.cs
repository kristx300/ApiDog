using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpGenerate
{
    public class HttpClientGenerate : IDisposable
    {
        private HttpClient HttpClient { get; set; }

        public async Task<string> NotGet(string url, string data, TypeHttpMethod method)
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

                case TypeHttpMethod.Patch:
                    hrm = await HttpClient.PatchAsync(url, content);
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

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("Patch");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            HttpResponseMessage response = new HttpResponseMessage();

            response = await client.SendAsync(request);

            return response;
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