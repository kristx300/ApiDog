using APIDog.Enums;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class UrlModel
    {
        public ObservableCollection<UrlProperty> PropertyList { get; set; } = new ObservableCollection<UrlProperty>();

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