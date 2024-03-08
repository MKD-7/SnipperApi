using Microsoft.AspNetCore.Identity;
using SnipperAPI.interfaces;

namespace SnipperAPI
{
    public static class Extentions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        { 
            services.AddSingleton<IEncrypt, EncryptionService>();
            return services;
        }

    }
}
