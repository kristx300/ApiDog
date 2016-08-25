using PropertyChanged;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class CancelCollection
    {
        public string Key { get; set; }
        public bool[] Value { get; set; }
    }
}