using APIDog.Core;
using APIDog.Extensions;
using APIDog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace APIDog.ViewModels
{
    public class PropertyViewModel
    {
        public UrlModel Model { get; set; } = new UrlModel();
        public UrlProperty Property { get; set; } = new UrlProperty();
        public AnalogTuple<bool> Parameters { get; set; } = new AnalogTuple<bool>();
        public ICommand ClickForEach { get; set; }
        public ICommand ClickCancelEach { get; set; }
        private List<CancelCollection> CancelData { get; set; } = new List<CancelCollection>();

        public PropertyViewModel()
        {
            Init();
            Model.PropertyList.AddRange(new UrlProperty[]
            {
                new UrlProperty {PropertyName="name" },
                new UrlProperty {PropertyName="password" },
                new UrlProperty {PropertyName="email" },
                new UrlProperty {PropertyName="types" },
                new UrlProperty {PropertyName="rules" },
            });
        }

        public PropertyViewModel(UrlModel uRLModel)
        {
            Init();
            Model = uRLModel;
        }

        public PropertyViewModel(IEnumerable<string> props)
        {
            Init();
            foreach (var item in props)
            {
                Model.PropertyList.Add(new UrlProperty { PropertyName = item });
            }
        }

        private void Init()
        {
            ClickForEach = new RelayCommand(x => ForEach(x));
            ClickCancelEach = new RelayCommand(x => CancelEach(x));
        }

        private void ForEach(object parameter)
        {
            if (Property == null)
                return;

            var str = parameter as string;

            if (str == "IsNullable")
            {
                CancelData.Add(new CancelCollection { Key = str, Value = Model.PropertyList.Select(x => x.IsNullable).ToArray() });
                for (int i = 0; i < Model.PropertyList.Count; i++)
                    Model.PropertyList[i].IsNullable = Property.IsNullable;
            }
            else if (str == "IsIEnumerable")
            {
                CancelData.Add(new CancelCollection { Key = str, Value = Model.PropertyList.Select(x => x.IsIEnumerable).ToArray() });
                for (int i = 0; i < Model.PropertyList.Count; i++)
                    Model.PropertyList[i].IsIEnumerable = Property.IsIEnumerable;
            }
            Parameters.First = true;
        }

        private void CancelEach(object parameter)
        {
            var str = parameter as string;
            var local = CancelData.SingleOrDefault(x => x.Key == str);

            if (str == "IsNullable")
                for (int i = 0; i < Model.PropertyList.Count; i++)
                    Model.PropertyList[i].IsNullable = local.Value[i];
            else if (str == "IsIEnumerable")
                for (int i = 0; i < Model.PropertyList.Count; i++)
                    Model.PropertyList[i].IsIEnumerable = local.Value[i];
            Parameters.First = false;
            CancelData.Remove(local);
        }
    }
}