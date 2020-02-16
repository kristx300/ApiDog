using APIDog.Enums;
using PropertyChanged;
using System.ComponentModel;

namespace APIDog.Models
{
    public class OptionsGenerate : INotifyPropertyChanged
    {
        public HttpGenerateType Type { get; set; }
        public bool WithCoockie { get; set; }
        public bool WithHeaders { get; set; }
        public bool OpenFolder { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}