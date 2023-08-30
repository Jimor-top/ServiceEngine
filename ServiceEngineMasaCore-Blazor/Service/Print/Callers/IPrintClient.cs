using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Print.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Print.Callers
{
    [JwtAuthentication]
    public interface IPrintClient : IHttpApi
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysPrint/page")]
        ITask<AdminResult<SqlSugarPagedList<SysPrint>>> GetSysPrintPageAsync([JsonContent] PPrintInput input, CancellationToken token = default);
    }
}
