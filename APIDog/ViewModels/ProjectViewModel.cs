using APIDog.Core;
using APIDog.Core.Parser;
using APIDog.Core.Writer;
using APIDog.Extensions;
using APIDog.Models;
using System.Linq;
using System.Windows.Input;

namespace APIDog.ViewModels
{
    public class ProjectViewModel
    {
        public Project Model { get; set; }
        public ProjectItem SelectedItem { get; set; }
        private ProjectItem toCreate { get; set; }
        public ICommand ClickCreate { get; set; }
        public ICommand ClickRemove { get; set; }
        public ICommand ClickGenerate { get; set; }
        public ICommand ClickRequest { get; set; }
        public ICommand ClickUrlModel { get; set; }
        public ICommand ClickClassModel { get; set; }

        public ProjectViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            ClickCreate = new RelayCommand(x => Create());
            ClickRemove = new RelayCommand(x => Remove());
            ClickGenerate = new RelayCommand(x => Generate());
            ClickRequest = new RelayCommand(x => Request());
            ClickUrlModel = new RelayCommand(x => URLModel());
            ClickClassModel = new RelayCommand(x => ClassModel());
        }

        public ProjectViewModel(Project model)
        {
            Initialize();
            Model = model;
            if (Model.Items.Count > 0)
                SelectedItem = Model.Items[0];
        }

        private void ClassModel()
        {
            if (SelectedItem == null)
                return;
            if (SelectedItem.ListModel == null)
            {
                var parse = new Parser().Parse(SelectedItem.Response.Response).Types;
                var props = WindowsManager.Create(ModelCreator.CreateClasses(parse));
                if (props != null)
                    SelectedItem.ListModel = props;
            }
            else
            {
                var result = WindowsManager.Create(SelectedItem.ListModel);
                if (result != null)
                    SelectedItem.ListModel = result;
            }
        }

        private void Request()
        {
            if (SelectedItem == null)
                return;
            if (SelectedItem.Request == null || SelectedItem.Response == null)
                WindowsManager.Message("Request error", "Request or Response is Null");
            var result = WindowsManager.Create(new AnalogTuple<RequestData, ResponseData> { First = SelectedItem.Request, Second = SelectedItem.Response });
            if (result != null)
            {
                SelectedItem.Request = result.First;
                SelectedItem.Response = result.Second;
            }
        }

        private void URLModel()
        {
            if (SelectedItem == null)
                return;
            UrlModel model;

            if (SelectedItem.UrlModel == null)
                model = WindowsManager.Create(SelectedItem.Request.Url.QueryParams());
            else
                model = WindowsManager.Create(SelectedItem.UrlModel);

            if (model != null)
                SelectedItem.UrlModel = model;
        }

        private void Generate()
        {
            WindowsManager.Generate(Model);
        }

        private void Remove()
        {
            if (SelectedItem != null)
                Model.Items.Remove(SelectedItem);
        }

        private void Create()
        {
            toCreate = new ProjectItem();

            var request = WindowsManager.Create();

            if (request != null)
            {
                toCreate.Request = request.First;
                toCreate.Response = request.Second;
                toCreate.Name = request.First.Url.QueryName() + " : " + toCreate.Request.TypeMethod;
            }
            else return;

            var pars = toCreate.Request.Url.QueryParams();

            if (pars.Count() != 0)
            {
                var model = WindowsManager.Create(pars);

                if (model != null)
                    toCreate.UrlModel = model;
            }

            var parse = new Parser().Parse(toCreate.Response.Response).Types;

            var props = WindowsManager.Create(ModelCreator.CreateClasses(parse));

            if (props != null)
                toCreate.ListModel = props;

            Model.Items.Add(toCreate);

            WindowsManager.Message("All right", "Object creating is success");
        }
    }
}