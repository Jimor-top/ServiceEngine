using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers
{
    [JwtAuthentication]
    public interface IConstClient : IHttpApi
    {
        /// <summary>
        /// 获取所有常量列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysConst/list")]
        ITask<AdminResult<List<ConstOutput>>> GetSysConstListAsync(CancellationToken token = default);

        /// <summary>
        /// 根据类名获取常量数据
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        [HttpGet("api/sysConst/data")]
        ITask<AdminResult<List<ConstOutput>>> GetSysConstDataAsync(string typeName, CancellationToken token = default);
    }
}
