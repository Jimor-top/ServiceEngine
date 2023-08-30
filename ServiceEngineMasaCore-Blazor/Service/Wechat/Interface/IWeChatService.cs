
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Wechat.Interface
{
    public interface IWeChatService
    {
        Task<AdminResult<SqlSugarPagedList<SysWechatUser>>> GetWeChatPageAsync([JsonContent] PWechatUserInput input);
    }
}
