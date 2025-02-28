using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HttpRequestAttribute : Attribute
    {
        public string Method { get; }
        public string Route { get; }

        public HttpRequestAttribute(string method, string route)
        {
            Method = method.ToUpper(); // Ensartede metoder
            Route = route;
        }
    }
}
