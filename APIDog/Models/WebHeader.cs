using PropertyChanged;
using System.ComponentModel;
using System.Net;

namespace APIDog.Models
{
    public class WebRequestHeader : INotifyPropertyChanged
    {
        public HttpRequestHeader RequestHeader { get; set; }
        public string Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class WebResponseHeader : INotifyPropertyChanged
    {
        public HttpResponseHeader ResponseHeader { get; set; }
        public string Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}