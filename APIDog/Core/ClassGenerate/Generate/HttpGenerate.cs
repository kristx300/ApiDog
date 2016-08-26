using APIDog.Enums;

namespace APIDog.Core.ClassGenerate.Generate
{
    public class HttpGenerate : IHttpClassGenerate
    {
        /// <summary>
        /// Using extension patch request
        /// </summary>
        private bool Patch;
        public HttpGenerate(bool withPatch)
        {
            Patch = withPatch;
        }
        public string GetName()
        {
            return "HttpClientGenerate";
        }

        public string Generate()
        {
            if (Patch)
                return Properties.Resources.http;
            else
                return Properties.Resources.withoutpatch;
        }

        public string GetFormat(string url)
        {
            return string.Format("await client.Get(\"{0}\");", url);
        }

        public string PostFormat(string url, string data, TypeHttpMethod method)
        {
            return string.Format("await client.NotGet(\"{0}\",{1},\"{2}\");", url, data, method);
        }
    }
}