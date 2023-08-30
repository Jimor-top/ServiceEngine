using ServiceEngine.Core;
using ServiceEngine.Core.Service;

namespace ServiceEngineMasaCore.Blazor.Service.Login.Interface
{
    public interface ISysAuthService
    {
        Task<AdminResult<LoginOutput>> LoginAsync(LoginInput input);

        Task LogoutAsync();
    }
}
