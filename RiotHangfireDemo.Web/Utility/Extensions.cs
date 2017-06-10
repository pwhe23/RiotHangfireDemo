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
        public static bool IsSimpleType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsGenericType)
            {
                type = Nullable.GetUnderlyingType(type);
                if (type == null) return false;
            }

            if (type.IsPrimitive)
                return true;

            if (type == typeof(string))
                return true;

            if (type == typeof(DateTime))
                return true;

            if (type == typeof(decimal))
                return true;

            return false;
        }

        public static List<PropertyInfo> GetSimpleProperties(this Type type)
        {
            return type
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.PropertyType.IsSimpleType())
                .ToList();
        }

        //REF: http://stackoverflow.com/questions/325426/programmatic-equivalent-of-defaulttype
        public static object GetDefault(this Type type)
        {
            if (type != null && type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static bool Is<T>(this T obj1, T obj2)
        {
            if (obj1 == null && obj2 == null)
                return true;

            if (obj1 == null || obj2 == null)
                return false;

            return obj1.Equals(obj2);
        }

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
    };
}