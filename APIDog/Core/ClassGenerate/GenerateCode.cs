using APIDog.Core.ClassGenerate.Generate;
using APIDog.Core.Writer;
using APIDog.Enums;
using APIDog.Extensions;
using APIDog.Models;
using APIDog.Models.Pre;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace APIDog.Core.ClassGenerate
{
    /// <summary>
    /// Generate C# code from Project
    /// </summary>
    public class GenerateCode
    {
        /// <summary>
        /// Different implementation of network connection
        /// </summary>
        private IHttpClassGenerate HttpClassGenerate { get; set; }
        /// <summary>
        /// Information about Project
        /// </summary>
        private Project ProjectInfo { get; set; }

        /// <summary>
        /// Create class generator
        /// </summary>
        /// <param name="proj">The Project which will generate</param>
        /// <param name="http">Сoncrete implementation of network connection</param>
        /// <param name="cookie">Do cookies need?</param>
        /// <param name="headers">Do headers need?</param>
        public GenerateCode(Project proj, HttpGenerateType http, bool cookie = false, bool headers = false)
        {
            ProjectInfo = proj;
            switch (http)
            {
                case HttpGenerateType.WebRequest:
                    HttpClassGenerate = new WebRequestGenerate(cookie, headers);
                    break;

                case HttpGenerateType.WebClient:
                    HttpClassGenerate = new WebGenerate();
                    break;

                case HttpGenerateType.HttpClient:
                    HttpClassGenerate = new HttpGenerate(ProjectInfo.Items.Select(x => x.Request.TypeMethod).Contains(TypeHttpMethod.Patch));
                    break;

                default:
                    throw new InvalidOperationException("Http generate type is not exist");
            }
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <returns>True if path refers to an existing directory
        /// false if the directory does not exist or an error occurs when trying to determine if the specified file exists.</returns>
        public bool DirectoryNotFoundException()
        {
            return !Directory.Exists(ProjectInfo.ProjectPath);
        }

        /// <summary>
        /// Generating code and saving to files
        /// </summary>
        public void Generate()
        {
            if (DirectoryNotFoundException())
                return;
            PreClass factory = new PreClass
            {
                IsInternal = false,
                Name = "Factory",
                Properties = new ObservableCollection<PreProperty>(),
                Methods = new List<PreMethod>()
            };
            List<string> json = new List<string>();

            #region Generate

            foreach (var Item in ProjectInfo.Items)
            {
                string jsonName = Item.ListModel.SingleOrDefault(x => x.Name == "ROOTBITCH").GetName();
                foreach (var model in Item.ListModel)
                    json.Add(model.Generate());

                PreMethod preM = new PreMethod
                {
                    IsPublic = true,
                    Name = Item.Name,
                    ReturnObject = jsonName,
                    IsInternal = false,
                    Params = Item.UrlModel?.Generate(),
                    BodyLines = new List<string>()
                };

                preM.Name = Regex.Replace(preM.Name, @"[\d-: \/]", string.Empty);

                string[] data = new string[0];
                if (Item.UrlModel != null)
                {
                    data = new string[Item.UrlModel.PropertyList.Count];

                    for (int i = 0; i < Item.UrlModel.PropertyList.Count; i++)
                    {
                        string ggn = Item.UrlModel.PropertyList[i].GetGenerationName();

                        if (Item.UrlModel.PropertyList[i].IsIEnumerable)
                            data[i] = "\"" + Item.UrlModel.PropertyList[i].PropertyName + "=\" + " + "string.Joun(',',ggn)";
                        else
                            data[i] = "\"" + Item.UrlModel.PropertyList[i].PropertyName + "=\" + " + ggn;
                    }
                }
                
                if (HttpClassGenerate.GetName() == "WebRequestGenerate")
                {
                    preM.BodyLines.Add("        var whc = new WebHeaderCollection();");
                    foreach (var item in Item.Request.Headers)
                        preM.BodyLines.Add("        whc.Add(" + item.RequestHeader + ",\"" + item.Value + "\");");
                }

                if (Item.Request.TypeMethod != TypeHttpMethod.Get)
                    preM.BodyLines.Add("        string result = " + HttpClassGenerate.PostFormat(Item.Request.Url.WithoutParams(), string.Join(" + \"&\" +", data), Item.Request.TypeMethod));
                else
                    preM.BodyLines.Add("        string result = " + HttpClassGenerate.GetFormat(Item.Request.Url.WithoutParams() + string.Join(" + \"&\" +", data)));

                preM.BodyLines.Add("        return JsonConvert.DeserializeObject<" + jsonName + ">(result);");

                factory.Methods.Add(preM);
            }

            #endregion Generate

            PreProperty client = new PreProperty
            {
                GetSet = true,
                Init = true,
                InitString = " = new " + HttpClassGenerate.GetName() + "();",
                IsNull = false,
                PropName = "client",
                TypeName = HttpClassGenerate.GetName()
            };
            factory.Properties.Add(client);

            var toName = new List<string>();
            toName.Add(factory.Generate());
            string crud = CSharpWriter.CreateNameSpace(toName, CSharpWriter.GetUsings(toName, ProjectInfo.ProjectNamespace + "." + ProjectInfo.ModelNamespace), ProjectInfo.ProjectNamespace + "." + ProjectInfo.CRUDNamespace);
            string models = CSharpWriter.CreateNameSpace(json, CSharpWriter.GetUsings(json), ProjectInfo.ProjectNamespace + "." + ProjectInfo.ModelNamespace);
            File.WriteAllText(ProjectInfo.ProjectPath + @"/" + HttpClassGenerate.GetName() + "Crud.cs", crud);
            File.WriteAllText(ProjectInfo.ProjectPath + @"/" + HttpClassGenerate.GetName() + "Models.cs", models);
            File.WriteAllText(ProjectInfo.ProjectPath + @"/" + HttpClassGenerate.GetName() + "Client.cs", HttpClassGenerate.Generate());
        }
    }
}