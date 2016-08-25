using APIDog.Core;
using APIDog.Core.Parser;
using APIDog.Core.Writer;
using System;
using System.Windows;

namespace APIDog
{
    /// <summary>
    /// Логика взаимодействия для JsonGenerate.xaml
    /// </summary>
    public partial class JsonGenerate : Window
    {
        public JsonGenerate()
        {
            InitializeComponent();
        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            try
            {
                var parse = new Parser().Parse(Json.Text).Types;
                var model = ModelCreator.CreateClasses(parse,true);
                string data = "";
                foreach (var item in model)
                {
                    data += item.Generate();
                }
                Class.Text = data;
            }
            catch (Exception ex)
            {
                WindowsManager.Message("ApiDog", ex.Message);
            }
        }
    }
}