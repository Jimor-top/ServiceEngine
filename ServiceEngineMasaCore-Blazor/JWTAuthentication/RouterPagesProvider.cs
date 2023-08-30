using ServiceEngineMasaCore.Blazor.Common;
using ServiceEngineMasaCore.Blazor.Interface;

namespace ServiceEngineMasaCore.Blazor.JWTAuthentication
{
    public sealed class RouterPagesProvider
    {
        private IEnumerable<RouterPageMetadata> _executingPages;

        public IEnumerable<RouterPageMetadata> ExecutingPages => _executingPages;

        public RouterPagesProvider()
        {
            _executingPages = Enumerable.Empty<RouterPageMetadata>();
            LoadExecutingRouterPages();
        }

        private void LoadExecutingRouterPages()
            => _executingPages = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType && x.IsAssignableTo(typeof(IRouterPage)))
                    .Select(x => new RouterPageMetadata(x.Name, GetDescription(x), null, GetAuthorities(x),
                        x.GetCustomAttributes<RouteAttribute>().Select(y => y.Template)));

        private static string? GetDescription(Type type)
            => type.GetProperty(nameof(IRouterPage.Description), BindingFlags.Public | BindingFlags.Static)?.GetValue(null)?.ToString();

        private static IEnumerable<Permission>? GetAuthorities(Type type)
            => type.GetProperty(nameof(IRouterPage.Authorities), BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as IEnumerable<Permission>;
    }

    public record RouterPageMetadata(string Name,
        string? Description,
        string? GroupName,
        IEnumerable<Permission>? Authorities,
        IEnumerable<string>? Routes);
}
