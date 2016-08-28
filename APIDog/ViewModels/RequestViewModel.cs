using APIDog.Core;
using APIDog.Enums;
using APIDog.Extensions;
using APIDog.Models;
using System;
using System.Linq;
using System.Net;
using System.Windows.Input;

namespace APIDog.ViewModels
{
    public class RequestViewModel
    {
        public RequestData Request { get; set; } = new RequestData();
        public ResponseData Response { get; set; } = new ResponseData();
        public AnalogTuple<bool> Parameters { get; set; } = new AnalogTuple<bool>();
        public ICommand ClickAddHeader { get; set; }
        public ICommand ClickUpdateHeader { get; set; } 
        public ICommand ClickRemoveHeader { get; set; }
        public ICommand ClickEnter { get; set; }
        public ICommand ClickDefaultHeaders { get; set; }
        public ICommand ClickCancelHeaders { get; set; }
        private CustomBrowser Browser { get; set; }

        public RequestViewModel()
        {
            Init();
            Request.TypeMethod = TypeHttpMethod.Get;
            Request.Url = @"https://api.vk.com/method/users.get?user_ids=291529102&fields=photo_50,city,verified";
            Request.Headers.AddRange(new WebRequestHeader[]
            {
                new WebRequestHeader { RequestHeader = HttpRequestHeader.Accept, Value = "application/json" },
                new WebRequestHeader { RequestHeader = HttpRequestHeader.UserAgent, Value = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36" },
                new WebRequestHeader { RequestHeader = HttpRequestHeader.AcceptCharset, Value = "utf-8" },
            });
        }

        public RequestViewModel(AnalogTuple<RequestData, ResponseData> data)
        {
            Init();
            Request = data.First;
            Response = data.Second;
        }

        private void Init()
        {
            ClickEnter = new RelayCommand(x => Enter());
            ClickAddHeader = new RelayCommand(x => AddHeader());
            ClickUpdateHeader = new RelayCommand(x => UpdateHeader());
            ClickRemoveHeader = new RelayCommand(x => RemoveHeader());
            ClickDefaultHeaders = new RelayCommand(x => DefaultHeaders());
            ClickCancelHeaders = new RelayCommand(x => CalcelHeaders());
        }

        private void CalcelHeaders()
        {
            if (!Request.Headers.Any(x => x.RequestHeader == HttpRequestHeader.Accept))
                Request.Headers.Remove(Request.Headers.SingleOrDefault(x => x.RequestHeader == HttpRequestHeader.Accept));
            if (!Request.Headers.Any(x => x.RequestHeader == HttpRequestHeader.UserAgent))
                Request.Headers.Remove(Request.Headers.SingleOrDefault(x => x.RequestHeader == HttpRequestHeader.UserAgent));
            if (!Request.Headers.Any(x => x.RequestHeader == HttpRequestHeader.AcceptCharset))
                Request.Headers.Remove(Request.Headers.SingleOrDefault(x => x.RequestHeader == HttpRequestHeader.AcceptCharset));
            Parameters.First = false;
        }

        private void DefaultHeaders()
        {
            if (!Request.Headers.Any(x => x.RequestHeader == HttpRequestHeader.Accept))
                Request.Headers.Add(new WebRequestHeader { RequestHeader = HttpRequestHeader.Accept, Value = "application/json" });
            if (!Request.Headers.Any(x => x.RequestHeader == HttpRequestHeader.UserAgent))
                Request.Headers.Add(new WebRequestHeader { RequestHeader = HttpRequestHeader.UserAgent, Value = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36" });
            if (!Request.Headers.Any(x => x.RequestHeader == HttpRequestHeader.AcceptCharset))
                Request.Headers.Add(new WebRequestHeader { RequestHeader = HttpRequestHeader.AcceptCharset, Value = "utf-8" });
            Parameters.First = true;
        }

        private async void Enter()
        {
            Request.IsEntered = false;
            Browser = CustomBrowser.Create();
            try
            {
                var locale = await Browser.RequestAsync(Request);
                Response.Headers = locale.Headers;
                Response.Response = locale.Response;
                Response.StatusCode = locale.StatusCode;
                Request.IsEntered = true;
            }
            catch (ArgumentNullException ex)
            {
                WindowsManager.Message("ApiDog", ex.Message);
            }
            catch (Exception ex)
            {
                WindowsManager.Message("ApiDog", ex.Message);
            }
            finally
            {
                Browser.Abort();
            }
        }

        private void AddHeader()
        {
            if (Request.Headers.Where(x => x.RequestHeader == Request.CurrentHeader).Count() == 0)
                Request.Headers.Add(new WebRequestHeader { RequestHeader = Request.CurrentHeader, Value = Request.CurrentHeaderValue });
        }

        private void UpdateHeader()
        {
            if (Request.Headers.Where(x => x.RequestHeader == Request.CurrentHeader).Count() == 1)
                Request.Headers.SingleOrDefault(x => x.RequestHeader == Request.CurrentHeader).Value = Request.CurrentHeaderValue;
        }

        private void RemoveHeader()
        {
            if (Request.Headers.Where(x => x.RequestHeader == Request.CurrentHeader).Count() == 1)
                Request.Headers.Remove(Request.Headers.SingleOrDefault(x => x.RequestHeader == Request.CurrentHeader));
        }
    }
}