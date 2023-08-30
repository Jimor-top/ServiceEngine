using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Cache.Callers
{
    [JwtAuthentication]
    public interface ICacheClient : IHttpApi
    {
        /// 获取缓存页面数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysCache/keyList")]
        ITask<AdminResult<List<string>>> GetSysCachePageAsync(CancellationToken token = default);

        /// 获取缓存数据值
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysCache/value/{key}")]
        ITask<AdminResult<object>> GetSysCacheValueAsync(string key,CancellationToken token = default);
    }
}
