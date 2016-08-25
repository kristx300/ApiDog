using APIDog.Models;
using APIDog.Models.Pre;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APIDog.Core
{
    /// <summary>
    /// Custom factory to open windows dialog
    /// </summary>
    public static class WindowsManager
    {
        /// <summary>
        /// Show dialog and return data
        /// </summary>
        /// <typeparam name="T">Type of returned object</typeparam>
        /// <param name="window">Main Window</param>
        /// <returns>Returned data</returns>
        private static T Make<T>(IWindow<T> window) where T : class
        {
            var res = window.ShowDialog();

            if (res.HasValue && window.CorrectClosing)
                return window.Data;
            else
                return null;
        }

        public static ObservableCollection<PreClass> Create(IEnumerable<PreClass> items)
        {
            var win = new JsonModelPage(items, true);
            return Make(win);
        }

        public static AnalogTuple<RequestData, ResponseData> Create(AnalogTuple<RequestData, ResponseData> data)
        {
            var win = new RequestPage(data, true);
            return Make(win);
        }

        public static AnalogTuple<RequestData, ResponseData> Create()
        {
            var win = new RequestPage();
            return Make(win);
        }

        public static UrlModel Create(UrlModel model)
        {
            var win = new PropertyPage(model);
            return Make(win);
        }

        public static UrlModel Create(IEnumerable<string> items)
        {
            var win = new PropertyPage(items);
            return Make(win);
        }

        public static ObservableCollection<ProjectItem> Create(Project model)
        {
            var win = new ProjectPage(model);
            return Make(win);
        }

        public static void Generate(Project model)
        {
            var win = new GeneratePage(model);
            win.ShowDialog();
        }

        public static void OpenJSGen()
        {
            var win = new JsonGenerate();
            win.ShowDialog();
        }

        public static void Message(string title, string msg)
        {
            var win = new MessagePage(title, msg);
            win.ShowDialog();
        }
    }
}