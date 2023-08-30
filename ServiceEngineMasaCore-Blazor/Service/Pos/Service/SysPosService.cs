using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Pos.Callers;
using ServiceEngineMasaCore.Blazor.Service.Pos.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Callers;

namespace ServiceEngineMasaCore.Blazor.Service.Pos.Service
{
    public class SysPosService : BaseService, ISysPosService
    {
        private readonly IPosClient _client;

        public SysPosService(IPosClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<List<SysPos>>> GetSysPosListAsync(PosInput input)
            => HandleErrorAsync(_client.GetSysPosListAsync(input));
    }
}
