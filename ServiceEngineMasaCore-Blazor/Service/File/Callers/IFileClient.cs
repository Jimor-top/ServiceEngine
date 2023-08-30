using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using ServiceEngineMasaCore.Blazor.Service.File.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.File.Callers
{
    [JwtAuthentication]
    public interface IFileClient : IHttpApi
    {
        /// 获取字典页面数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysFile/page")]
        ITask<AdminResult<SqlSugarPagedList<SysFile>>> GetSysFilePageAsync([JsonContent] PFileInput input, CancellationToken token = default);
    }
}
