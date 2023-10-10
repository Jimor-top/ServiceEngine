using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Pos.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Pos.Callers
{
    [JwtAuthentication]
    public interface IPosClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysPos/list")]
        ITask<AdminResult<List<SysPos>>> GetSysPosListAsync(PosInput input, CancellationToken token = default);

        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysPos/add")]
        ITask<AdminResult<object>> AddSysPosAsync([JsonContent] AddPosInput input, CancellationToken token = default);


        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysPos/update")]
        ITask<AdminResult<object>> UpdateSysPosAsync([JsonContent] UpdatePosInput input, CancellationToken token = default);

        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysPos/delete")]
        ITask<AdminResult<object>> DeleteSysPosAsync([JsonContent] DltPosInput input, CancellationToken token = default);



    }
}
