using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Callers
{
    [JwtAuthentication]
    public interface IDictClient : IHttpApi
    {  
        /// 获取字典页面数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysDictData/page")]
        ITask<AdminResult<SqlSugarPagedList<SysDictData>>> GetSysDictDataPageAsync([JsonContent] PDictDataInput input,CancellationToken token = default);
    }
}
