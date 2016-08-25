using APIDog.Core;
using APIDog.Models;
using APIDog.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для PropertyPage.xaml
    /// </summary>
    public partial class PropertyPage : Window, IWindow<UrlModel>
    {
        internal static readonly ObservableCollection<string> ArrayOfTypes = new ObservableCollection<string>(new string[]
                {
                    "bool","byte","char","decimal","double","float","int","long","sbyte","string","short","uint","ulong","ushort"
                });

        public PropertyPage(IEnumerable<string> properties)
        {
            InitializeComponent();
            DataContext = new PropertyViewModel(properties);
            foreach (var item in ArrayOfTypes)
                Types.Items.Add(item);
        }

        public PropertyPage(UrlModel uRLModel)
        {
            InitializeComponent();
            DataContext = new PropertyViewModel(uRLModel);
            LastButton.Content = "Ok";
            foreach (var item in ArrayOfTypes)
                Types.Items.Add(item);
        }

        private void ClickClose(object sender, RoutedEventArgs e)
        {
            CorrectClosing = true;
            Close();
        }

        public UrlModel Data
        {
            get
            {
                return (DataContext as PropertyViewModel).Model;
            }
        }
        public bool CorrectClosing { get; set; } = false;
    }
}