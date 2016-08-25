using APIDog.Core;
using APIDog.Core.ClassGenerate;
using APIDog.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace APIDog.ViewModels
{
    public class GenerateViewModel
    {
        public OptionsGenerate Options { get; set; } = new OptionsGenerate();
        public ICommand ClickGenerate { get; set; }
        private Project TargetGenerate { get; set; }
        private GenerateCode GCode { get; set; }

        public GenerateViewModel()
        {
            ClickGenerate = new RelayCommand(x => Generate());
        }

        public GenerateViewModel(Project proj)
        {
            TargetGenerate = proj;
            ClickGenerate = new RelayCommand(x => Generate());
        }

        private void Generate()
        {
            GCode = new GenerateCode(TargetGenerate, Options.Type, Options.WithCoockie, Options.WithHeaders);
            if (GCode.DirectoryNotFoundException())
            {
                WindowsManager.Message("Project path error", "Directory path of project not found");
                return;
            }
            GCode.Generate();
            if (Options.OpenFolder)
            {
                Process.Start(TargetGenerate.ProjectPath);
            }
        }
    }
}