using APIDog.Enums;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace APIDog.Models
{
    public class RequestData : INotifyPropertyChanged
    {
        public TypeHttpMethod TypeMethod { get; set; } = TypeHttpMethod.Get;
        public ObservableCollection<WebRequestHeader> Headers { get; set; } = new ObservableCollection<WebRequestHeader>();
        public string Url { get; set; }
        public HttpRequestHeader CurrentHeader { get; set; } = HttpRequestHeader.Accept;
        public string CurrentHeaderValue { get; set; } = @"application/json";
        public bool UriValidate
        {
            get
            {
                Uri uriResult;
                return Uri.TryCreate(Url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            }
        }
        public bool IsEntered { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}