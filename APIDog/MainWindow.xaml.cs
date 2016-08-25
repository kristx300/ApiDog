using APIDog.Core;
using APIDog.ViewModels;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as MainViewModel)?.Serialize();
        }
    }
}