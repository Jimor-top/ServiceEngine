using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Print.Callers;
using ServiceEngineMasaCore.Blazor.Service.Print.Dto;
using ServiceEngineMasaCore.Blazor.Service.Print.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.Print.Service
{
    public class SysPrintService : BaseService, ISysPrintService
    {
        private readonly IPrintClient _client;

        public SysPrintService(IPrintClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysPrint>>> GetSysPrintPageAsync(PPrintInput input)
          => HandleErrorAsync(_client.GetSysPrintPageAsync(input));
    }
}
