using APIDog.Core.Writer;
using PropertyChanged;
using System.Collections.Generic;

namespace APIDog.Models.Pre
{
    [ImplementPropertyChanged]
    public class PreMethod
    {
        public List<string> BodyLines { get; set; }
        public string Params { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsInternal { get; set; }
        public string ReturnObject { get; set; }

        public string Generate()
        {
            return CSharpWriter.CreateMethod(BodyLines, Params, Name, IsPublic, IsInternal, ReturnObject);
        }
    }
}