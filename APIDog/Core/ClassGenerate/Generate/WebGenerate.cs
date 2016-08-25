using APIDog.Enums;

namespace APIDog.Core.ClassGenerate.Generate
{
    public class WebGenerate : IHttpClassGenerate
    {
        public WebGenerate()
        {
        }

        public string GetName()
        {
            return "WebClientGenerate";
        }

        public string Generate()
        {
            return Properties.Resources.web;
        }

        public string GetFormat(string url)
        {
            return string.Format("client.Get({0});", url);
        }

        public string PostFormat(string url, string data, TypeHttpMethod method)
        {
            return string.Format("client.NotGet(\"{0}\",{1},\"{2}\");", url, data, method);
        }
    }
}