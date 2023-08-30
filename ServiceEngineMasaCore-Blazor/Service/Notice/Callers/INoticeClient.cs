using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Notice.Callers
{
    [JwtAuthentication]
    public interface INoticeClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/page")]
        ITask<AdminResult<SqlSugarPagedList<SysNotice>>> GetSysNoticePageAsynct([JsonContent] PNoticeInput input, CancellationToken token = default);
    }
}
