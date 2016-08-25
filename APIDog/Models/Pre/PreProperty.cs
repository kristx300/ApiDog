using APIDog.Core.Writer;
using PropertyChanged;

namespace APIDog.Models.Pre
{
    [ImplementPropertyChanged]
    public class PreProperty
    {
        public string TypeName { get; set; }
        public string PropName { get; set; }
        public string ReWrite { get; set; }
        public string InitString { get; set; }
        public bool IsNull { get; set; }
        public bool GetSet { get; set; }
        public bool Init { get; set; }

        public string Generate()
        {
            return CSharpWriter.CreateProperty(PropName, TypeName, InitString, IsNull, GetSet, Init, ReWrite);
        }
    }
}