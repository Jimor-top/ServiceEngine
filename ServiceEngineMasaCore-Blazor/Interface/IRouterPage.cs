using ServiceEngineMasaCore.Blazor.Common;

namespace ServiceEngineMasaCore.Blazor.Interface
{
    public interface IRouterPage
    {
        static string? Description { get; }

        static IEnumerable<Permission> Authorities { get; }
    }
}
