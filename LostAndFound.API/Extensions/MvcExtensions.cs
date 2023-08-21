using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace LostAndFound.API.Extensions
{
    public static class MvcExtensions
    { 
        public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder builder)
        {
            return builder.AddNewtonsoftJson(opt => opt.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }

        public static void ConfigureCacheProfiles(this MvcOptions options)
        {
            options.CacheProfiles.Add(
                "default",
                new CacheProfile
                {
                    Duration = 60,
                    Location = ResponseCacheLocation.Any
                }
            );
        }
    }
}
