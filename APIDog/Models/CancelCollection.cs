using PropertyChanged;
using System.ComponentModel;

namespace APIDog.Models
{
    public class CancelCollection : INotifyPropertyChanged
    {
        public string Key { get; set; }
        public bool[] Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}