using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Job.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Job.Callers
{
    [JwtAuthentication]
    public interface IJobClient : IHttpApi
    {
        /// <summary>
        /// 获取访问日志
        /// </summary>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("/api/sysJob/pageJobDetail")]
        ITask<AdminResult<SqlSugarPagedList<JobOutput>>> GetSysJobPageAsync([JsonContent] PJobInput input, CancellationToken token = default);
    }
}
