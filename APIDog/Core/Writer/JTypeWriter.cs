using APIDog.Core.Parser;
using APIDog.Extensions;
using System;

namespace APIDog.Core.Writer
{
    /// <summary>
    /// Convert from JType to string
    /// </summary>
    public static class JTypeWriter
    {
        /// <summary>
        /// Convert from JType to name
        /// </summary>
        /// <param name="type">JType</param>
        /// <param name="arrayIsList">Is Array or List</param>
        /// <returns>Enum Name</returns>
        public static string GetEnumName(JType type, bool arrayIsList = true)
        {
            var name = Enum.GetName(typeof(JTypeEnum), type.Type);
            name = name.ToLower();

            if (name == "array")
            {
                var locale = GetEnumName(type.SubType);
                if (arrayIsList)
                    name = "List<" + locale + ">";
                else
                    name = locale + "[]";
            }

            if (name == "object")
                name = type.Name.FirstToUpper();

            if (name == "randomtype")
                name = "object";

            return name;
        }

        /// <summary>
        /// Convert from JType to init
        /// </summary>
        /// <param name="type">JType</param>
        /// <param name="arrayIsList">Is Array or List</param>
        /// <returns>Enum Init</returns>
        public static string GetEnumInit(JType type, bool arrayIsList = true)
        {
            string initDefault = "new object();";
            switch (type.Type)
            {
                case JTypeEnum.String:
                case JTypeEnum.Bool:
                case JTypeEnum.Int:
                case JTypeEnum.Long:
                case JTypeEnum.Float:
                case JTypeEnum.Double:
                case JTypeEnum.DateTime:
                    initDefault = "";
                    break;

                case JTypeEnum.Array:
                    if (arrayIsList)
                        initDefault = "= new " + GetEnumName(type, arrayIsList: arrayIsList) + "();";
                    else
                    {
                        var name = GetEnumName(type, arrayIsList: arrayIsList).FirstToUpper();
                        initDefault = "= new " + name.Insert(name.IndexOf('['), "0") + ";";
                    }
                    break;

                case JTypeEnum.Object:
                    initDefault = "= new " + type.Name.FirstToUpper() + "();";
                    break;
                case JTypeEnum.RandomType:
                    break;
            }
            return initDefault;
        }
    }
}