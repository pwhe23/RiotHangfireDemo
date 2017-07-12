using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Ext
    {
        public static string ReadToEnd(this Stream stream)
        {
            if (stream == null)
                return null;

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static int? ToInt(this string value)
        {
            return int.TryParse(value, out var result) ? result : (int?) null;
        }

        public static bool HasInterface<T>(this Type cls)
        {
            return typeof(T).IsAssignableFrom(cls);
        }

        public static bool HasOpenInterface(this Type cls, Type iface)
        {
            return cls
                .GetInterfaces()
                .Any(x => x.IsGenericType
                          && iface.IsAssignableFrom(x.GetGenericTypeDefinition()));
        }

        public static Dictionary<Type, Type> GetInterfacesWithSingleImplementation(this Assembly[] assemblies)
        {
            return assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.IsClass
                            && !x.IsAbstract)
                .SelectMany(x => x
                    .GetInterfaces()
                    .Where(i => assemblies.Contains(i.Assembly))
                    .Select(i => new
                    {
                        Implementation = x,
                        Interface = i,
                    })
                )
                .GroupBy(x => x.Interface, (k, g) => new
                {
                    Interface = k,
                    Implemenations = g.Select(y => y.Implementation).ToArray(),
                })
                .Where(x => x.Implemenations.Length == 1)
                .ToDictionary(x => x.Interface, x => x.Implemenations[0]);
        }

        /// <summary>
        /// Translate Web.config AppSettings to a strongly-typed config class.
        /// </summary>
        public static T MapAppSettingsToClass<T>() where T : new()
        {
            var config = new T();
            var prefix = config.GetType().Name + ".";
            var properties = config.GetType().GetProperties().ToDictionary(x => x.Name);

            foreach (string appSettingKey in ConfigurationManager.AppSettings)
            {
                if (!appSettingKey.StartsWith(prefix))
                    continue;

                var propertyName = appSettingKey.Substring(prefix.Length);
                if (!properties.ContainsKey(propertyName))
                    continue;

                var property = properties[propertyName];
                var appSettingValue = ConfigurationManager.AppSettings[appSettingKey];
                var propertyValue = Convert.ChangeType(appSettingValue, property.PropertyType);

                property.SetValue(config, propertyValue);
            }

            return config;
        }
    };
}