using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class ResponseData
    {
        public ObservableCollection<WebResponseHeader> Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }

        public static ResponseData Create(HttpWebResponse v)
        {
            var rd = new ResponseData
            {
                StatusCode = v.StatusCode,
                Headers = new ObservableCollection<WebResponseHeader>()
            };

            for (int i = 0; i < v.Headers.Count; i++)
            {
                HttpResponseHeader hrh;
                if (Enum.TryParse(v.Headers.GetKey(i), out hrh))
                    if (rd.Headers.Where(x => x.ResponseHeader == hrh).Count() == 0)
                        rd.Headers.Add(new WebResponseHeader { ResponseHeader = hrh, Value = v.Headers[i] });
            }

            var encoding = !string.IsNullOrEmpty(v.CharacterSet) ? Encoding.GetEncoding(v.CharacterSet) : Encoding.UTF8;
            using (StreamReader reader = new StreamReader(v.GetResponseStream(), encoding))
                rd.Response = reader.ReadToEnd();
            v.Close();
            return rd;
        }
    }
}