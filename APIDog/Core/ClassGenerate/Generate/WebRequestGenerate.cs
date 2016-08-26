using APIDog.Enums;

namespace APIDog.Core.ClassGenerate.Generate
{
    public class WebRequestGenerate : IHttpClassGenerate
    {
        /// <summary>
        /// Do headers need?
        /// </summary>
        private bool Headers;

        /// <summary>
        /// Do cookies need?
        /// </summary>
        private bool Cookie;

        public WebRequestGenerate(bool cookie, bool headers)
        {
            Headers = headers;
            Cookie = cookie;
        }

        public string GetName()
        {
            return "WebRequestGenerate";
        }

        public string Generate()
        {
            if (Headers)
                if (Cookie)
                    return Properties.Resources.full;
                else
                    return Properties.Resources.withoutcookie;
            else
                if (Cookie)
                return Properties.Resources.withoutheader;
            else
                return Properties.Resources.clear;
        }

        public string GetFormat(string url)
        {
            if (Headers)
                return string.Format("client.Get(\"{0}\",whc);", url);
            else
                return string.Format("client.Get(\"{0}\");", url);
        }

        public string PostFormat(string url, string data, TypeHttpMethod method)
        {
            if (Headers)
                return string.Format("client.NotGet(\"{0}\",{1},\"{2}\",whc);", url, data, method.ToString());
            else
                return string.Format("client.NotGet(\"{0}\",{1},\"{2}\");", url, data, method.ToString());
        }
    }
}