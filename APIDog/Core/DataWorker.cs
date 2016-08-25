using Newtonsoft.Json;
using System;
using System.IO;

namespace APIDog.Core
{
    /// <summary>
    /// Class for Json Serialize/Deserialize from file
    /// </summary>
    public static class DataWorker
    {
        /// <summary>
        /// Open file, if file not exists then create it, read file to string and deserialize it
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="type">Type of object</param>
        /// <returns>Parsed object</returns>
        public static object Deserialize(string path, Type type)
        {
            if (File.Exists(path))
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize(file, type);
                }
            else
                Serialize(path, null);
            return null;
        }

        /// <summary>
        /// Open file, if file not exists then create it, serialize it and write string to file
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="ject">Object</param>
        public static void Serialize(string path, object ject)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, ject);
            }
        }
    }
}