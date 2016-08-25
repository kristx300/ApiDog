using APIDog.Models.Pre;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace APIDog.Models
{
    [ImplementPropertyChanged]
    public class ProjectItem
    {
        public RequestData Request { get; set; }
        public ResponseData Response { get; set; }
        public UrlModel UrlModel { get; set; }
        public ObservableCollection<PreClass> ListModel { get; set; }
        public string Name { get; set; }
    }
}