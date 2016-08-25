using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIDog.Core.Parser
{
    public class Parser
    {
        /// <summary>
        /// Main data of parse
        /// </summary>
        private ParserData _localData = new ParserData();

        /// <summary>
        /// Parse from json
        /// </summary>
        /// <param name="json">Json string</param>
        /// <param name="rootName">RootName of object</param>
        /// <returns>Main data of parse</returns>
        public ParserData Parse(string json, string rootName = "ROOTBITCH")
        {
            Init();
            JObject[] examples = new JObject[0];
            JToken j = JToken.Parse(json);
            if (j is JArray)
            {
                examples = ((JArray)j).Cast<JObject>().ToArray();
            }
            else if (j is JObject)
            {
                examples = new[] { j as JObject };
            }
            else
            {
                throw new Exception("Sample JSON must be either a JSON array, or a JSON object.");
            }
            var mainType = JType.Create(examples[0]);
            mainType.Name = rootName;
            MakeData(examples, mainType);
            return GetData();
        }

        /// <summary>
        /// Initialization of class
        /// </summary>
        private void Init()
        {
            if (_localData == null)
                _localData = new ParserData();
        }

        /// <summary>
        /// Get data of parse
        /// </summary>
        /// <returns>Data after parse</returns>
        private ParserData GetData()
        {
            var ret = _localData;
            Distinct(ret.Types);
            return ret;
        }

        /// <summary>
        /// Remove all same items
        /// </summary>
        /// <param name="types">Part of MainData</param>
        private static void Distinct(List<JType> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                types[i].SubTypes = types[i].SubTypes.GroupBy(x => x.Name).Select(x => x.First()).ToList();
                Distinct(types[i].SubTypes);
            }
        }

        /// <summary>
        /// Create root
        /// </summary>
        /// <param name="examples">Items</param>
        /// <param name="main">Main Jtype</param>
        private void MakeData(JObject[] examples, JType main)
        {
            foreach (var item in examples)
            {
                foreach (var property in item.Properties())
                {
                    var rt = JType.Create(property.Value);
                    rt.Name = property.Name;
                    if (!main.SubTypes.Contains(rt))
                        main.SubTypes.Add(rt);
                }
            }
            foreach (var item in main.SubTypes)
            {
                if (item.Type == JTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var objc in examples)
                    {
                        JToken value;
                        if (objc.TryGetValue(item.Name, out value))
                        {
                            if (value.Type == JTokenType.Object)
                            {
                                subexamples.Add((JObject)value);
                            }
                        }
                    }

                    if (!_localData.Names.Contains(item.Name))
                    {
                        _localData.Names.Add(item.Name);
                        MakeData(subexamples.ToArray(), item);
                    }
                }
                if (item.SubType?.Type == JTypeEnum.Object)
                {
                    var subexamples = new List<JObject>(examples.Length);
                    foreach (var obj in examples)
                    {
                        JToken value;
                        if (obj.TryGetValue(item.Name, out value))
                        {
                            if (value.Type == JTokenType.Array)
                            {
                                foreach (var itemValue in (JArray)value)
                                {
                                    if (!(itemValue is JObject)) throw new NotSupportedException("Arrays of non-objects are not supported yet.");
                                    subexamples.Add((JObject)itemValue);
                                }
                            }
                            else if (value.Type == JTokenType.Object)
                            {
                                foreach (var itemValue in (JObject)value)
                                {
                                    if (!(itemValue.Value is JObject)) throw new NotSupportedException("Arrays of non-objects are not supported yet.");

                                    subexamples.Add((JObject)itemValue.Value);
                                }
                            }
                        }
                    }
                    item.SubType.Name = item.Name;
                    MakeData(subexamples.ToArray(), item.SubType);
                }
            }
            _localData.Types.Add(main);
        }
    }

    /// <summary>
    /// Data after parse
    /// </summary>
    public class ParserData
    {
        /// <summary>
        /// List of class names
        /// </summary>
        public List<string> Names { get; set; } = new List<string>();
        /// <summary>
        /// List of class types
        /// </summary>
        public List<JType> Types { get; set; } = new List<JType>();
    }
}