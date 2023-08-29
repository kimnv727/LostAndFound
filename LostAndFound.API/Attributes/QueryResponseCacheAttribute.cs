using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace LostAndFound.API.Attributes
{
    public class QueryResponseCacheAttribute : ResponseCacheAttribute
    {
        public QueryResponseCacheAttribute(Type queryType, string profileName = "default")
        {
            CacheProfileName = profileName;
            VaryByQueryKeys = queryType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop => prop.Name).ToArray();
        }
    }
}
