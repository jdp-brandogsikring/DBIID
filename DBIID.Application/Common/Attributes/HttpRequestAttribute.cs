using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HttpRequestAttribute : Attribute
    {
        public HttpMethodType Method { get; }
        public string Route { get; }

        public HttpRequestAttribute(HttpMethodType method, string route)
        {
            Method = method;
            Route = route;
        }
    }
}
