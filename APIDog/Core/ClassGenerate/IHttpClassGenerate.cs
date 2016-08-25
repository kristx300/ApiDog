using APIDog.Enums;

namespace APIDog.Core.ClassGenerate
{
    internal interface IHttpClassGenerate
    {
        /// <summary>
        /// Generation implementation code
        /// </summary>
        /// <returns></returns>
        string Generate();

        /// <summary>
        /// Get generation class name
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Get format for get request
        /// </summary>
        /// <param name="url">Url link</param>
        /// <returns>String line</returns>
        string GetFormat(string url);

        /// <summary>
        /// Get format for not get request
        /// </summary>
        /// <param name="url">Url link</param>
        /// <param name="data">Parameters data</param>
        /// <param name="method">Type of http method</param>
        /// <returns>String line</returns>
        string PostFormat(string url, string data, TypeHttpMethod method);
    }
}