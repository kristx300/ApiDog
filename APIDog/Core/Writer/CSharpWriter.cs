using APIDog.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIDog.Core.Writer
{
    /// <summary>
    /// Create C# code
    /// </summary>
    public static class CSharpWriter
    {
        /// <summary>
        /// Create C# property
        /// </summary>
        /// <param name="propName">Property name</param>
        /// <param name="typeName">Type name</param>
        /// <param name="initString">Initialize line</param>
        /// <param name="isNull">Is null</param>
        /// <param name="getSet">Using { get; set; }</param>
        /// <param name="init">Using initialize line</param>
        /// <param name="rewrite">Rewrite property name</param>
        /// <returns>C# property</returns>
        public static string CreateProperty(string propName, string typeName, string initString, bool isNull, bool getSet = true, bool init = true, string rewrite = "")
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(rewrite))
            {
                sb.AppendFormat("[JsonProperty(\"{0}\")]", propName);
                sb.AppendFormat("public {0} {1}", typeName, rewrite);
            }
            else
                sb.AppendFormat("public {0} {1}", typeName, propName);

            if (isNull)
                sb.Append("?");

            if (getSet)
                sb.Append(" { get; set; }");

            if (init && !string.IsNullOrEmpty(initString))
                sb.AppendFormat(" {0}", initString);

            if (!init && (!getSet || string.IsNullOrEmpty(initString)))
                sb.Append(";");

            return sb.ToString();
        }

        /// <summary>
        /// Create C# class
        /// </summary>
        /// <param name="properties">Properties</param>
        /// <param name="name">Class name</param>
        /// <param name="isInternal">Class is internal</param>
        /// <param name="methods">Methods</param>
        /// <returns>C# class</returns>
        public static string CreateClass(IEnumerable<string> properties, string name, bool isInternal = false, List<string> methods = null)
        {
            var sb = new StringBuilder();
            if (isInternal)
                sb.AppendFormat("    {0}", "internal class");
            else
                sb.AppendFormat("    {0}", "public class");
            sb.AppendLine(" " + name.FirstToUpper());
            sb.AppendLine("    {");
            foreach (var item in properties)
            {
                sb.AppendLine("        " + item);
            }
            if (methods != null && methods.Count != 0)
            {
                foreach (var item in methods)
                {
                    sb.AppendLine("        " + item);
                }
            }
            sb.AppendLine("    }");
            return sb.ToString();
        }

        /// <summary>
        /// Create C# namespace
        /// </summary>
        /// <param name="classes">Classes</param>
        /// <param name="usings">Usings</param>
        /// <param name="nameSpace">Namespace name</param>
        /// <returns>C# namespace</returns>
        public static string CreateNameSpace(IEnumerable<string> classes, IEnumerable<string> usings, string nameSpace)
        {
            var sb = new StringBuilder();
            if (usings != null)
                foreach (var item in usings)
                    sb.AppendFormat("using {0};\n", item);
            else
                foreach (var item in GetUsings(classes))
                    sb.AppendFormat("using {0};\n", item);

            sb.AppendLine();
            sb.AppendLine("namespace " + nameSpace);
            sb.AppendLine("{");

            foreach (var item in classes)
                sb.AppendLine(item);

            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Create C# method
        /// </summary>
        /// <param name="body">Body Lines</param>
        /// <param name="param">Parameters</param>
        /// <param name="name">Method name</param>
        /// <param name="isPublic">Is public method</param>
        /// <param name="isInternal">Is internal method</param>
        /// <param name="returnObject">Return object - maybe void</param>
        /// <returns>C# Method</returns>
        public static string CreateMethod(List<string> body, string param, string name, bool isPublic, bool isInternal, string returnObject)
        {
            var sb = new StringBuilder();
            if (!isInternal)
                if (isPublic)
                    sb.Append("public ");
                else
                    sb.Append("private ");
            else
                sb.Append("internal ");
            if (body.Where(x => x.Contains("await")).Count() != 0)
                sb.AppendFormat("async Task<{0}> {1}", returnObject, name);
            else
                sb.AppendFormat("{0} {1}", returnObject, name);
            sb.Append("(");
            sb.Append(param);
            sb.Append(")");
            sb.AppendLine();
            sb.AppendLine("        {");
            foreach (var item in body)
            {
                sb.AppendLine("    " + item);
            }
            sb.AppendLine("        }");
            return sb.ToString();
        }

        /// <summary>
        /// Read class-lines and returns found usings
        /// </summary>
        /// <param name="classes">Classes</param>
        /// <returns>Found usings</returns>
        public static IEnumerable<string> GetUsings(IEnumerable<string> classes, params string[] addUsings)
        {
            foreach (var item in classes)
            {
                if (item.Contains("DateTime"))
                    yield return "System";

                if (item.Contains("List"))
                    yield return "System.Collections.Generic;";

                if (item.Contains("ClientGenerate"))
                    yield return "HttpGenerate";

                if (item.Contains("await"))
                    yield return "System.Threading.Tasks";

                if (item.Contains("JsonConvert"))
                    yield return "Newtonsoft.Json";

                if (item.Contains("ROOTBITCH"))
                    yield return "JsonModels";
                if (item.Contains("Count()"))
                    yield return "System.Linq";
            }
            foreach (var item in addUsings)
            {
                yield return item;
            }
            yield break;
        }

        /// <summary>
        /// Create C# property for method
        /// </summary>
        /// <param name="type">Type name</param>
        /// <param name="name">Property name</param>
        /// <param name="isIEnumerable">Property is IEnumerable</param>
        /// <param name="isNull">Property is can be Null</param>
        /// <returns>C# property for method</returns>
        public static string CreateProperty(string type, string name, bool isIEnumerable, bool isNull)
        {
            var sb = new StringBuilder();

            if (isIEnumerable)
                sb.AppendFormat("IEnumerable<{0}>", type);
            else
                sb.Append(type);

            if (isNull)
                sb.Append("? ");
            else
                sb.Append(" ");

            sb.Append(name);
            return sb.ToString();
        }
    }
}