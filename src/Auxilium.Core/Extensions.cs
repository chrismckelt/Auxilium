using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Auxilium.Core.Utilities;
using Newtonsoft.Json;

namespace Auxilium.Core
{
    public static  class Extensions
    {
        
        public static string Dump<T>(this T obj, string description = null, bool indent = false)
        {
            var content = JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

            Consoler.Information(content);
            return content;
        }

        public static string GetDescription(this Enum enumerationValue)
        {
            if (enumerationValue == null)
                return null;
            var attributes =
                (DescriptionAttribute[])
                enumerationValue.GetType()
                    .GetField(enumerationValue.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : enumerationValue.ToString();
        }


        public static void SaveToFile<T>(this T source, string filename)
        {
            string content = JsonConvert.SerializeObject(source);

            File.WriteAllText(filename,content);
        }

        public static T ReadFromFile<T>(this string filename)
        {
            string content = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string ToCsv<T>(this IEnumerable<T> items, char separator)
        {   
            string output = "";
                var delimiter = ',';
                var properties = typeof(T).GetProperties()
                    .Where(n =>
                        n.PropertyType == typeof(string)
                        || n.PropertyType == typeof(bool)
                        || n.PropertyType == typeof(char)
                        || n.PropertyType == typeof(byte)
                        || n.PropertyType == typeof(decimal)
                        || n.PropertyType == typeof(int)
                        || n.PropertyType == typeof(DateTime)
                        || n.PropertyType == typeof(DateTime?));
                using (var sw = new StringWriter())
                {
                    var header = properties
                        .Select(n => n.Name)
                        .Aggregate((a, b) => a + delimiter + b);
                    sw.WriteLine(header);
                    foreach (var item in items)
                    {
                        var row = properties
                            .Select(n => n.GetValue(item, null))
                            .Select(n => n?.ToString())
                            .Aggregate((a, b) => a + delimiter + b);
                        sw.WriteLine(row);
                    }
                    output = sw.ToString();
                }
                return output;
            }
        }
    }

