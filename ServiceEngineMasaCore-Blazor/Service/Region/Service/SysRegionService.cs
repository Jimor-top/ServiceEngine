using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Region.Callers;
using ServiceEngineMasaCore.Blazor.Service.Region.Dto;
using ServiceEngineMasaCore.Blazor.Service.Region.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Region.Service
{
    public class SysRegionService : BaseService, ISysRegionService
    {
        private readonly IRegionClient _client;

        public SysRegionService(IRegionClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<List<SysRegion>>> GetSysRegionListAsync(long Id)
        => HandleErrorAsync(_client.GetSysRegionListAsync(Id));

        public Task<AdminResult<SqlSugarPagedList<SysRegion>>> GetSysRegionPageAsync(PRegionInput input)
         => HandleErrorAsync(_client.GetSysRegionPageAsync(input));
    }
}
