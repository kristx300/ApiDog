using APIDog.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace APIDog.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, T[] array)
        {
            foreach (var item in array)
            {
                collection.Add(item);
            }
        }

        public static string Get(this ObservableCollection<WebRequestHeader> model, HttpRequestHeader type)
        {
            return model.SingleOrDefault(x => x.RequestHeader == type) == null ?
                "false" : model.SingleOrDefault(x => x.RequestHeader == type).Value;
        }

        public static string Get(this ObservableCollection<WebResponseHeader> model, HttpResponseHeader type)
        {
            return model.SingleOrDefault(x => x.ResponseHeader == type) == null ?
                "false" : model.SingleOrDefault(x => x.ResponseHeader == type).Value;
        }

        public static List<WebRequestHeader> Get(this ObservableCollection<WebRequestHeader> model, params HttpRequestHeader[] type)
        {
            return model.Where(x => !type.Contains(x.RequestHeader)).ToList();
        }

        public static List<WebResponseHeader> Get(this ObservableCollection<WebResponseHeader> model, params HttpResponseHeader[] type)
        {
            return model.Where(x => !type.Contains(x.ResponseHeader)).ToList();
        }
    }
}