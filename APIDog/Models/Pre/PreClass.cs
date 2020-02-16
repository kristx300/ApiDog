using APIDog.Core.Writer;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APIDog.Models.Pre
{
    public class PreClass : INotifyPropertyChanged
    {
        public ObservableCollection<PreProperty> Properties { get; set; }
        internal List<PreMethod> Methods { get; set; }
        public string Name { get; set; }
        public bool IsInternal { get; set; }
        public string ReWrite { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Generate()
        {
            List<string> props = new List<string>();
            foreach (var item in Properties)
                props.Add(item.Generate());

            List<string> meth = new List<string>();
            if (Methods != null && Methods.Count != 0)
                foreach (var item in Methods)
                    meth.Add(item.Generate());

            return CSharpWriter.CreateClass(props, GetName(), IsInternal, meth);
        }

        public string GetName()
        {
            if (!string.IsNullOrEmpty(ReWrite))
                return ReWrite;
            else
                return Name;
        }
    }
}