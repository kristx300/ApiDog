using APIDog.Models.Pre;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APIDog.Models
{
    public class ProjectItem : INotifyPropertyChanged
    {
        public RequestData Request { get; set; }
        public ResponseData Response { get; set; }
        public UrlModel UrlModel { get; set; }
        public ObservableCollection<PreClass> ListModel { get; set; }
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}