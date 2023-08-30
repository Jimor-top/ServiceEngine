using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Service
{
    public class SysConstServices : BaseService, ISysConstService
    {
        private readonly IConstClient _client;

        public SysConstServices(IConstClient client, IPopupService popup) : base(popup)
         => _client = client;

        public Task<AdminResult<List<ConstOutput>>> GetSysConstDataAsync(string typeName)
          => HandleErrorAsync(_client.GetSysConstDataAsync(typeName));

        public Task<AdminResult<List<ConstOutput>>> GetSysConstListAsync()
          => HandleErrorAsync(_client.GetSysConstListAsync());
    }
}
