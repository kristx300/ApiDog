using APIDog.Core;
using APIDog.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace APIDog.ViewModels
{
    public class MainViewModel
    {
        public Project SelectedItem { get; set; }
        public ObservableCollection<Project> Collection { get; set; } = new ObservableCollection<Project>();
        public ICommand ClickCreate { get; set; }
        public ICommand ClickSave { get; set; }
        public ICommand ClickDelete { get; set; }
        public ICommand ClickJson { get; set; }
        public ICommand ClickAbout { get; set; }
        public ICommand ClickOpen { get; set; }
        public ICommand ClickPath { get; set; }

        public MainViewModel()
        {
            ClickCreate = new RelayCommand(x => Create());
            ClickSave = new RelayCommand(x => Save());
            ClickDelete = new RelayCommand(x => Delete());
            ClickJson = new RelayCommand(x => Json());
            ClickAbout = new RelayCommand(x => About());
            ClickOpen = new RelayCommand(x => Open());
            ClickPath = new RelayCommand(x => Path());
            Deserialize();
        }

        private void Path()
        {
            var dialog = new FolderBrowserDialog { SelectedPath = AppDomain.CurrentDomain.BaseDirectory, ShowNewFolderButton = true };
            if (dialog.ShowDialog() == DialogResult.OK)
                if (SelectedItem != null)
                    SelectedItem.ProjectPath = dialog.SelectedPath;
        }

        public void Open()
        {
            if (SelectedItem == null)
                return;
            var result = WindowsManager.Create(SelectedItem);
            if (result != null)
                SelectedItem.Items = result;
        }

        private void Json()
        {
            WindowsManager.OpenJSGen();
        }

        private void About()
        {
        }

        private void Create()
        {
            Project news;
            if (SelectedItem != null)
                news = new Project { Description = SelectedItem.Description, ProjectName = SelectedItem.ProjectName, ProjectPath = SelectedItem.ProjectPath };
            else
                news = new Project { Description = "Default", ProjectName = "New Project" };
            Collection.Add(news);
        }

        private void Save()
        {
            Serialize();
        }

        private void Delete()
        {
            if (SelectedItem == null)
                return;
            Collection?.Remove(SelectedItem);
        }

        public void Serialize()
        {
            DataWorker.Serialize(AppDomain.CurrentDomain.BaseDirectory + @"\ProjectList.json", new ObservableCollection<Project>(Collection));
        }

        private void Deserialize()
        {
            var locale = DataWorker.Deserialize(AppDomain.CurrentDomain.BaseDirectory + @"\ProjectList.json", typeof(ObservableCollection<Project>)) as ObservableCollection<Project>;

            if (locale != null)
                Collection = locale;
            if (Collection.Count != 0)
                SelectedItem = Collection[0];
        }
    }
}