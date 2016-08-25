using APIDog.Core;
using APIDog.Extensions;
using APIDog.Models;
using APIDog.Models.Pre;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace APIDog.ViewModels
{
    public class JsonModelViewModel
    {
        public ObservableCollection<PreClass> Collection { get; set; } = new ObservableCollection<PreClass>();
        public PreClass SelectedClass { get; set; }
        public AnalogTuple<bool> Parameters { get; set; } = new AnalogTuple<bool>();
        public PreProperty SelectedProperty { get; set; }
        public ObservableCollection<string> Types
        {
            get
            {
                var types = new ObservableCollection<string>(new string[]
                {
                    "bool","byte","char","decimal","double","float","int","long","sbyte","short","string","uint","ulong","ushort"
                });
                types.AddRange(Collection.Select(x => x.Name).Where(x => x != "ROOTBITCH").ToArray());
                return types;
            }
        }
        public ICommand ClickForProp { get; set; }
        public ICommand ClickForModel { get; set; }
        public ICommand ClickCancelProp { get; set; }
        public ICommand ClickCancelModel { get; set; }
        public ICommand TextChanged { get; set; }
        private List<CancelCollection> CancelData { get; set; } = new List<CancelCollection>();

        private void Init()
        {
            ClickForProp = new RelayCommand(x => ForProp(x));
            ClickCancelProp = new RelayCommand(x => CancelProp(x));
            ClickForModel = new RelayCommand(x => ForModel());
            ClickCancelModel = new RelayCommand(x => CancelModel());
        }

        public JsonModelViewModel()
        {
            Init();
        }

        public JsonModelViewModel(IEnumerable<PreClass> items)
        {
            Init();
            Collection = new ObservableCollection<PreClass>(items);
            if (Collection.Count > 0)
            {
                SelectedClass = Collection[0];
                if (SelectedClass.Properties.Count > 0)
                    SelectedProperty = SelectedClass.Properties[0];
            }
        }

        private void CancelModel()
        {
            if (SelectedClass == null)
                return;
            var locale = CancelData.SingleOrDefault(x => x.Key == "IsInternal");
            for (int i = 0; i < locale.Value.Count(); i++)
            {
                Collection[i].IsInternal = locale.Value[i];
            }
            CancelData.Remove(locale);
            Parameters.First = false;
        }

        private void ForModel()
        {
            if (SelectedClass == null)
                return;
            CancelData.RemoveAll(x => x.Key == "IsInternal");
            CancelData.Add(new CancelCollection { Key = "IsInternal", Value = Collection.Select(x => x.IsInternal).ToArray() });
            for (int i = 0; i < Collection.Count; i++)
                Collection[i].IsInternal = SelectedClass.IsInternal;
            Parameters.First = true;
        }

        private void CancelProp(object parameter)
        {
            if (SelectedProperty == null)
                return;
            string param = parameter as string;

            var local = CancelData.Where(x => x.Key == param).ToList();
            if (param == "Add get/set")
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    for (int j = 0; j < Collection[i].Properties.Count; j++)
                        Collection[i].Properties[j].GetSet = local[i].Value[j];
                }
            }
            else if (param == "IsNull")
            {
                for (int i = 0; i < Collection.Count; i++)
                    for (int j = 0; j < Collection[i].Properties.Count; j++)
                        Collection[i].Properties[j].IsNull = local[i].Value[j];
            }
            else if (param == "Initialize")
            {
                for (int i = 0; i < Collection.Count; i++)
                    for (int j = 0; j < Collection[i].Properties.Count; j++)
                        Collection[i].Properties[j].Init = local[i].Value[j];
            }
            foreach (var item in local)
            {
                CancelData.Remove(item);
            }
            Parameters.Second = false;
        }

        private void ForProp(object parameter)
        {
            if (SelectedProperty == null)
                return;

            string param = parameter as string;
            CancelData.RemoveAll(x => x.Key == param);
            if (param == "Add get/set")
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    CancelData.Add(new CancelCollection { Key = param, Value = Collection[i].Properties.Select(x => x.GetSet).ToArray() });
                    for (int j = 0; j < Collection[i].Properties.Count; j++)
                        Collection[i].Properties[j].GetSet = SelectedProperty.GetSet;
                }
            }
            else if (param == "IsNull")
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    CancelData.Add(new CancelCollection { Key = param, Value = Collection[i].Properties.Select(x => x.IsNull).ToArray() });
                    for (int j = 0; j < Collection[i].Properties.Count; j++)
                        Collection[i].Properties[j].IsNull = SelectedProperty.IsNull;
                }
            }
            else if (param == "Initialize")
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    CancelData.Add(new CancelCollection { Key = param, Value = Collection[i].Properties.Select(x => x.Init).ToArray() });
                    for (int j = 0; j < Collection[i].Properties.Count; j++)
                        Collection[i].Properties[j].Init = SelectedProperty.Init;
                }
            }
            Parameters.Second = true;
        }
    }
}