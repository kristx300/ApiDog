using APIDog.Core;
using APIDog.Models;
using APIDog.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для RequestPage.xaml
    /// </summary>
    public partial class RequestPage : Window, IWindow<AnalogTuple<RequestData, ResponseData>>
    {
        internal static readonly ObservableCollection<string> ArrayOfHeaders = new ObservableCollection<string>(Enum.GetNames(typeof(HttpRequestHeader)));

        public RequestPage()
        {
            InitializeComponent();
            foreach (var item in ArrayOfHeaders)
                Headers.Items.Add(item);
        }

        public RequestPage(AnalogTuple<RequestData, ResponseData> data, bool IsLast = false)
        {
            InitializeComponent();
            DataContext = new RequestViewModel(data);
            if (IsLast)
                LastButton.Content = "Ok";
            foreach (var item in ArrayOfHeaders)
                Headers.Items.Add(item);
        }

        private void ClickClose(object sender, RoutedEventArgs e)
        {
            CorrectClosing = true;
            Close();
        }

        public AnalogTuple<RequestData, ResponseData> Data
        {
            get
            {
                var data = (DataContext as RequestViewModel);
                return new AnalogTuple<RequestData, ResponseData> { First = data.Request, Second = data.Response };
            }
        }
        public bool CorrectClosing { get; set; } = false;
    }
}