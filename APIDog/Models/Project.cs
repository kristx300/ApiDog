using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APIDog.Models
{
    public class Project : INotifyPropertyChanged
    {
        public string ProjectNamespace { get; set; }
        public string ModelNamespace { get; set; }
        public string CRUDNamespace { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string Description { get; set; }
        public ObservableCollection<ProjectItem> Items { get; set; } = new ObservableCollection<ProjectItem>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}