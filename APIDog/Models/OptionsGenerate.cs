using APIDog.Enums;
using PropertyChanged;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class OptionsGenerate
    {
        public HttpGenerateType Type { get; set; }
        public bool WithCoockie { get; set; }
        public bool WithHeaders { get; set; }
        public bool OpenFolder { get; set; }
    }
}