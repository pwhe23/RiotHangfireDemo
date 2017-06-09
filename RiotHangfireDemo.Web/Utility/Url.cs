using System;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace RiotHangfireDemo
{
    public class Url
    {
        private readonly UriBuilder _uriBuilder;
        private readonly NameValueCollection _queryString;
        private HttpWebRequest _request;

        public Url()
        {
            _uriBuilder = new UriBuilder();
            _queryString = HttpUtility.ParseQueryString("");
        }

        public Url(Uri uri)
        {
            _uriBuilder = new UriBuilder(uri.AbsoluteUri);
            _queryString = HttpUtility.ParseQueryString(_uriBuilder.Query);
        }

        public Url(string url)
        {
            _uriBuilder = GetUri(url);
            _queryString = HttpUtility.ParseQueryString(_uriBuilder.Query);
        }

        public Url(string url, object prms)
        {
            _uriBuilder = GetUri(url);
            _queryString = HttpUtility.ParseQueryString(_uriBuilder.Query);

            foreach (var prop in prms.GetType().GetSimpleProperties())
            {
                Add(prop.Name, prop.GetValue(prms));
            }
        }

        public Url(object prms)
        {
            _uriBuilder = new UriBuilder();
            _queryString = HttpUtility.ParseQueryString("");

            foreach (var prop in prms.GetType().GetSimpleProperties())
            {
                var value = prop.GetValue(prms);
                if (value.Is(prop.PropertyType.GetDefault()))
                    continue;

                Add(prop.Name, value);
            }
        }

        public Url Add(string name, object value)
        {
            if (value == null)
                return this;

            var str = value.ToString();
            if (value is DateTime)
            {
                if ((DateTime)value == DateTime.MinValue)
                    return this;
                str = str.Replace(" 12:00:00 AM", "");
            }

            _queryString[name] = str;
            return this;
        }

        public Url Add<T>(Expression<Func<T>> member)
        {
            var argumentName = ((MemberExpression)member.Body).Member.Name;
            var func = member.Compile();
            var argumentValue = func();
            return Add(argumentName, argumentValue);
        }

        public Url Add<T>(T obj) where T : class
        {
            var dict = new RouteValueDictionary(obj);
            foreach (var item in dict)
            {
                Add(item.Key, item.Value);
            }
            return this;
        }

        public Url Remove(params string[] names)
        {
            foreach (var name in names)
            {
                _queryString.Remove(name);
            }

            return this;
        }

        public Url AddHeader(string name, string value)
        {
            if (_request == null)
                _request = WebRequest.CreateHttp(ToString());

            _request.Headers.Add(name, value);
            return this;
        }

        public Url Path(string path)
        {
            _uriBuilder.Path = path;
            return this;
        }

        public override string ToString()
        {
            _uriBuilder.Query = _queryString.ToString();
            return _uriBuilder.Uri.ToString();
        }

        public string ToString(string url)
        {
            var uriBuilder = GetUri(url);
            uriBuilder.Query = _queryString.ToString();
            return uriBuilder.Uri.ToString();
        }

        private static UriBuilder GetUri(string url)
        {
            if (url == null)
            {
                return new UriBuilder();
            }
            else if (url.StartsWith("/"))
            {
                var uri = HttpContext.Current.Request.Url;
                url = uri.Scheme + "://" + uri.Host + (!uri.IsDefaultPort ? ":" + uri.Port : "") + url;
            }
            return new UriBuilder(url);
        }

        public static implicit operator string(Url url)
        {
            return url?.ToString();
        }
    };

    public class Url<T> : Url
    {
        public Url(string url, Action<T> populate) : base(url, Get(populate))
        {
        }

        public Url(Action<T> populate) : base(Get(populate))
        {
        }

        private static T Get(Action<T> populate)
        {
            var type = typeof(T);
            var constr = type.GetConstructors().FirstOrDefault();
            if (constr == null)
                return Activator.CreateInstance<T>();

            var obj = (T)constr.Invoke(constr.GetParameters().Select(x => (object)null).ToArray());
            populate?.Invoke(obj);
            return obj;
        }
    };

    public static class UrlExtensions
    {
        public static Url Add(this Uri uri, string name, object value)
        {
            return new Url(uri).Add(name, value);
        }
    };
}
