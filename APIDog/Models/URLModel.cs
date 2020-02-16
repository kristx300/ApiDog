using APIDog.Enums;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APIDog.Models
{
    public class UrlModel : INotifyPropertyChanged
    {
        public ObservableCollection<UrlProperty> PropertyList { get; set; } = new ObservableCollection<UrlProperty>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Generate()
        {
            string[] gen = new string[PropertyList.Count];
            for (int i = 0; i < PropertyList.Count; i++)
            {
                gen[i] = PropertyList[i].Generate();
            }
            return string.Join(", ", gen);
        }
    }
}