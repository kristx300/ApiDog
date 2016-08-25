using PropertyChanged;
using System.Net;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class WebRequestHeader
    {
        public HttpRequestHeader RequestHeader { get; set; }
        public string Value { get; set; }
    }

    [ImplementPropertyChanged]
    public class WebResponseHeader
    {
        public HttpResponseHeader ResponseHeader { get; set; }
        public string Value { get; set; }
    }
}