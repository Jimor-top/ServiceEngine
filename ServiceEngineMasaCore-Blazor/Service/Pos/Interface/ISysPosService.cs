using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Pos.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Pos.Interface
{
    public interface ISysPosService
    {
        Task<AdminResult<List<SysPos>>> GetSysPosListAsync(PosInput input);
        Task<AdminResult<object>> AddSysPosAsync(AddPosInput input);
        Task<AdminResult<object>> UpdateSysPosAsync(UpdatePosInput input);
        Task<AdminResult<object>> DeleteSysPosAsync(DltPosInput input);

    }
}
