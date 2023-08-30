using ServiceEngine.Core.Service;
using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.Pos.Interface
{
    public interface ISysPosService
    {
        Task<AdminResult<List<SysPos>>> GetSysPosListAsync(PosInput input);
    }
}
