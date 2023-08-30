using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NavServiceCollectionExtensions
    {

        public static IServiceCollection AddNav(this IServiceCollection services, List<NavModel> navList)
        {
            services.AddSingleton(navList);
            services.AddScoped<NavHelper>();

            return services;
        }

        public static IServiceCollection AddNav(this IServiceCollection services, string navSettingsFile)
        {
            string text = File.ReadAllText(navSettingsFile);

            if (!string.IsNullOrWhiteSpace(text))
            {
                var navList = JsonSerializer.Deserialize<List<NavModel>>(text);

                if (navList is null) throw new Exception("Please configure the navigation first!");

                services.AddNav(navList);
            }
            else {
                services.AddNav(new List<NavModel>());
            }
            return services;
        }
    }
}