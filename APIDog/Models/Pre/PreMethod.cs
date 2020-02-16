using APIDog.Core.Writer;
using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel;

namespace APIDog.Models.Pre
{
    public class PreMethod : INotifyPropertyChanged
    {
        public List<string> BodyLines { get; set; }
        public string Params { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsInternal { get; set; }
        public string ReturnObject { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Generate()
        {
            return CSharpWriter.CreateMethod(BodyLines, Params, Name, IsPublic, IsInternal, ReturnObject);
        }
    }
}