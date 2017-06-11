using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RiotHangfireDemo
{
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

        public static bool HasInterface<T>(this Type cls)
        {
            return typeof(T).IsAssignableFrom(cls);
        }

        public static Dictionary<Type, Type> GetInterfacesWithSingleImplementation(this Assembly assembly)
        {
            return assembly
                .GetExportedTypes()
                .Where(x => x.IsClass
                            && !x.IsAbstract)
                .SelectMany(x => x
                    .GetInterfaces()
                    .Where(i => i.Assembly == assembly)
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
    };
}