using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.File.Callers;
using ServiceEngineMasaCore.Blazor.Service.File.Dto;
using ServiceEngineMasaCore.Blazor.Service.File.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.File.Service
{
    public class SysFileService : BaseService, ISysFileService
    {
        private readonly IFileClient _client;
        public SysFileService(IFileClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysFile>>> GetSysFilePageAsync(PFileInput input)
            => HandleErrorAsync(_client.GetSysFilePageAsync(input));
    }
}
