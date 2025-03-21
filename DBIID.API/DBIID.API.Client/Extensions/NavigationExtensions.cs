using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace DBIID.API.Client.Extensions
{
    public static class NavigationExtensions
    {
        public static string? GetQueryParameter(this NavigationManager nav, string key)
        {
            var uri = nav.ToAbsoluteUri(nav.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);
            return query.TryGetValue(key, out var value) ? value.ToString() : null;
        }
    }
}
