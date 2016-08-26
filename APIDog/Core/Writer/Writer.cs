using APIDog.Core.Parser;
using APIDog.Models.Pre;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APIDog.Core.Writer
{
    /// <summary>
    /// Create models from JType list
    /// </summary>
    public static class ModelCreator
    {
        /// <summary>
        /// Create classes from JType List
        /// </summary>
        /// <param name="types">JType List</param>
        /// <param name="GetSet">Use GetSet in properties</param>
        /// <returns>Model</returns>
        public static IEnumerable<PreClass> CreateClasses(List<JType> types, bool GetSet = true)
        {
            foreach (var item in types)
            {
                yield return new PreClass
                {
                    Properties = new ObservableCollection<PreProperty>(CreateProperty(item.SubTypes, GetSet)),
                    Name = item.Name,
                    IsInternal = false
                };
            }
            yield break;
        }

        /// <summary>
        /// Create properties from JType list
        /// </summary>
        /// <param name="types">JType List</param>
        /// <param name="GetSet">Use GetSet</param>
        /// <returns>Properties</returns>
        public static IEnumerable<PreProperty> CreateProperty(List<JType> types, bool GetSet = false)
        {
            foreach (var item in types)
            {
                if (item.Name != "ROOTBITCH")
                    yield return new PreProperty
                    {
                        GetSet = GetSet,
                        Init = true,
                        InitString = JTypeWriter.GetEnumInit(item),
                        IsNull = item.IsNull,
                        PropName = item.Name,
                        TypeName = JTypeWriter.GetEnumName(item)
                    };
            }
            yield break;
        }
    }
}