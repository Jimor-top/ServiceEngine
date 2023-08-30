using ServiceEngine.Core.Service;
using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface
{
    public interface ISysConstService
    {
        Task<AdminResult<List<ConstOutput>>> GetSysConstListAsync();
        Task<AdminResult<List<ConstOutput>>> GetSysConstDataAsync(string typeName);
    }
}
