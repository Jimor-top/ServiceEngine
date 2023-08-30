using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Wechat.Callers
{
    [JwtAuthentication]
    public interface IWeChatClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysWechatUser/page")]
        ITask<AdminResult<SqlSugarPagedList<SysWechatUser>>> GetSysRolePageAsync([JsonContent] PWechatUserInput input, CancellationToken token = default);
    }
}
