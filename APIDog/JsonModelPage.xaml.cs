using APIDog.Core;
using APIDog.Models.Pre;
using APIDog.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для ModelPage.xaml
    /// </summary>
    public partial class JsonModelPage : Window, IWindow<ObservableCollection<PreClass>>
    {
        public JsonModelPage()
        {
            InitializeComponent();
        }

        public JsonModelPage(IEnumerable<PreClass> items, bool IsLast = false)
        {
            InitializeComponent();
            DataContext = new JsonModelViewModel(items);
            if (IsLast)
            {
                LastButton.Content = "Ok";
            }
        }

        private void ClickClose(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Data.SingleOrDefault(x => x.Name == "ROOTBITCH").ReWrite))
            {
                WindowsManager.Message("ApiDog", "Please rename ROOTBITCH object");
            }
            else
            {
                CorrectClosing = true;
                Close();
            }
        }

        public ObservableCollection<PreClass> Data
        {
            get
            {
                return (DataContext as JsonModelViewModel).Collection;
            }
        }

        public bool CorrectClosing { get; set; } = false;
    }
}