using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Server.Callers
{
    [JwtAuthentication]
    public interface IServerClient : IHttpApi
    {
        /// <summary>
        /// 获取服务器配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysServer/serverBase")]
        ITask<AdminResult<dynamic>> GetSysServerBaseAsync(CancellationToken token = default);

        /// <summary>
        /// 获取服务器使用信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysServer/serverUsed")]
        ITask<AdminResult<dynamic>> GetSysServerUsedAsync(CancellationToken token = default);

        /// <summary>
        /// 获取服务器磁盘信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysServer/serverDisk")]
        ITask<AdminResult<dynamic>> GetSysServerDiskAsync(CancellationToken token = default);

        /// <summary>
        /// 获取框架主要程序集
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysServer/assemblyList")]
        ITask<AdminResult<dynamic>> GetSysServerAssemblyListAsync(CancellationToken token = default);
    }
}
