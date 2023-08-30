using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Role.Callers;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Callers;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Dto;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Interface;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Wechat.Service
{
    public class WeChatService : BaseService, IWeChatService
    {
        private readonly IWeChatClient _client;

        public WeChatService(IWeChatClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysWechatUser>>> GetWeChatPageAsync([JsonContent] PWechatUserInput input)
          => HandleErrorAsync(_client.GetSysRolePageAsync(input));
    }
}
