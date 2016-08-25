using APIDog.Core;
using APIDog.Models;
using APIDog.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Window, IWindow<ObservableCollection<ProjectItem>>
    {
        public ProjectPage()
        {
            InitializeComponent();
        }

        public ProjectPage(Project Model)
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(Model);
        }

        public ObservableCollection<ProjectItem> Data
        {
            get
            {
                return (DataContext as ProjectViewModel).Model.Items;
            }
        }
        public bool CorrectClosing { get; set; } = true;
    }
}