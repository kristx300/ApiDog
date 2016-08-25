using APIDog.Core.Writer;
using PropertyChanged;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class UrlProperty
    {
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public bool IsRewriteName { get; set; }
        public string RewriteName { get; set; }
        public bool IsIEnumerable { get; set; }
        public bool IsNullable { get; set; }
        public string CodeComment { get; set; }

        public string Generate()
        {
            return CSharpWriter.CreateProperty(Type, GetGenerationName(), IsIEnumerable, IsNullable);
        }

        public string GetGenerationName()
        {
            if (IsRewriteName && !string.IsNullOrEmpty(RewriteName))
                return RewriteName;
            else
                return PropertyName;
        }
    }
}