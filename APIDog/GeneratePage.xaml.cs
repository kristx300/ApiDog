using APIDog.Models;
using APIDog.ViewModels;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для GeneratePage.xaml
    /// </summary>
    public partial class GeneratePage : Window
    {
        public GeneratePage()
        {
            InitializeComponent();
        }

        public GeneratePage(Project model)
        {
            InitializeComponent();
            DataContext = new GenerateViewModel(model);
        }
    }
}