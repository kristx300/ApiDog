using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для MessagePage.xaml
    /// </summary>
    public partial class MessagePage : Window
    {
        private MessagePage()
        {
            InitializeComponent();
        }

        public MessagePage(string title, string body)
        {
            InitializeComponent();
            TitleM.Content = title;
            Text.Text = body;
        }

        private void Okay_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}