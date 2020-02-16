using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace APIDog.Core.Parser
{
    /// <summary>
    /// Main class that contains information about objects
    /// </summary>
    public class JType
    {
        /// <summary>
        /// Type of object
        /// </summary>
        public JTypeEnum Type { get; set; }
        /// <summary>
        /// Object can be null
        /// </summary>
        public bool IsNull { get; set; } = false;
        /// <summary>
        /// SubType if main type is an array
        /// </summary>
        public JType SubType { get; private set; }
        /// <summary>
        /// Name object
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Subtypes that contain information about the properties, if the type is an object
        /// </summary>
        public List<JType> SubTypes { get; set; } = new List<JType>();

        private JType()
        {
        }

        /// <summary>
        /// Get type of two
        /// </summary>
        /// <param name="second">Second type which compares</param>
        /// <returns>Main type</returns>
        private JType GetEitherType(JType second)
        {
            var sub = Either(this, second);
            if (Type == JTypeEnum.Array)
            {
                if (second.IsNull)
                    return this;

                if (IsNull)
                    return second;

                var locale = SubType.GetEitherType(second.SubType).GetEitherType(CreateNull());

                if (locale != SubType)
                    return new JType { Type = JTypeEnum.Array, SubType = locale };
            }
            if (Type == sub.Type)
                return this;

            return sub.GetEitherType(CreateNull());
        }

        /// <summary>
        /// Create JType from JToken
        /// </summary>
        /// <param name="token">Parsed from json</param>
        /// <returns>Main JType</returns>
        public static JType Create(JToken token)
        {
            JType j = new JType();
            switch (token.Type)
            {
                case JTokenType.Integer:
                    if ((long)((JValue)token).Value < int.MaxValue)
                        j.Type = JTypeEnum.Int;
                    else
                        j.Type = JTypeEnum.Long;
                    break;

                case JTokenType.Float:
                    if ((double)((JValue)token).Value < float.MaxValue)
                        j.Type = JTypeEnum.Float;
                    else
                        j.Type = JTypeEnum.Double;
                    break;

                case JTokenType.Array:
                    j.Type = JTypeEnum.Array;
                    break;

                case JTokenType.Object:
                    j.Type = JTypeEnum.Object;
                    break;

                case JTokenType.Comment:
                case JTokenType.String:
                    j.Type = JTypeEnum.String;
                    break;

                case JTokenType.Boolean:
                    j.Type = JTypeEnum.Bool;
                    break;

                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.None:
                    j.IsNull = true;
                    break;

                case JTokenType.Date:
                    j.Type = JTypeEnum.DateTime;
                    break;

                default:
                    j.Type = JTypeEnum.RandomType;
                    break;
            }
            if (j.Type == JTypeEnum.Array)
            {
                var array = (JArray)token;
                j.SubType = GetSubType(array.ToArray());
            }
            return j;
        }

        /// <summary>
        /// Create JType which equals null
        /// </summary>
        /// <returns>Main JType</returns>
        private static JType CreateNull()
        {
            return new JType { IsNull = true, Type = JTypeEnum.RandomType };
        }

        /// <summary>
        /// Get SubType of array
        /// </summary>
        /// <param name="array"></param>
        /// <returns>Main JType</returns>
        private static JType GetSubType(JToken[] array)
        {
            if (array.Length == 0)
                return new JType { IsNull = true };

            var locale = Create(array[0]).GetEitherType(CreateNull());
            for (int i = 1; i < array.Length; i++)
            {
                locale = locale.GetEitherType(Create(array[i]));
            }
            return locale;
        }

        /// <summary>
        /// Get type of two
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>Main JType</returns>
        private static JType Either(JType first, JType second)
        {
            if (first.IsNull)
                return second;

            if (second.IsNull)
                return first;

            switch (first.Type)
            {
                case JTypeEnum.RandomType:
                    if (IsRandom(second))
                        first.Type = second.Type;
                    return first;

                case JTypeEnum.String:
                case JTypeEnum.Bool:
                case JTypeEnum.DateTime:
                case JTypeEnum.Object:
                case JTypeEnum.Array:
                case JTypeEnum.Int:
                case JTypeEnum.Long:
                case JTypeEnum.Float:
                case JTypeEnum.Double:
                    if (IsRelatedValues(first, second))
                        first.Type = second.Type;
                    return first;

                default:
                    return new JType { IsNull = true, Type = JTypeEnum.RandomType };
            }
        }

        /// <summary>
        /// Get similar types
        /// </summary>
        /// <param name="first">First type</param>
        /// <param name="second">Second type</param>
        /// <returns>True, if first type is similar to second, otherwise no</returns>
        private static bool IsRelatedValues(JType first, JType second)
        {
            var f = first.Type == JTypeEnum.Int || first.Type == JTypeEnum.Long || first.Type == JTypeEnum.Float || first.Type == JTypeEnum.Double;
            var s = second.Type == JTypeEnum.Int || second.Type == JTypeEnum.Long || second.Type == JTypeEnum.Float || second.Type == JTypeEnum.Double;
            return f == s;
        }

        /// <summary>
        /// Is random type OR is unknown type
        /// </summary>
        /// <param name="val">Main Type</param>
        /// <returns>True, if type is unknown type, otherwise no</returns>
        private static bool IsRandom(JType val)
        {
            return val.Type == JTypeEnum.String || val.Type == JTypeEnum.Int || val.Type == JTypeEnum.Float ||
                    val.Type == JTypeEnum.Double || val.Type == JTypeEnum.Long || val.Type == JTypeEnum.Bool ||
                    val.Type == JTypeEnum.DateTime || val.Type == JTypeEnum.Array || val.Type == JTypeEnum.Object;
        }
    }
}