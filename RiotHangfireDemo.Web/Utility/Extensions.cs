using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using MediatR;

namespace RiotHangfireDemo
{
    public static class Ext
    {
        public static object Execute(this IMediator mediator, object request)
        {
            if (request == null)
                return null;

            try
            {
                var requestInterface = request.GetType().GetInterface("IRequest`1");
                var send = mediator.GetType().GetMethod("Send").MakeGenericMethod(requestInterface.GetGenericArguments());
                return send.Invoke(mediator, new[] { request });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ERROR on request type {request.GetType()}", ex);
            }

            return null;
        }

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