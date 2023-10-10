using ServiceEngine.Core;
using ServiceEngine.Core.Service;
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

        /// 获取字典列表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysDictData/list")]
        ITask<AdminResult<List<SysDictData>>> GetSysDictDataListAsync(long DictTypeId, CancellationToken token = default);

        /// 获取字典详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysDictData/detail")]
        ITask<AdminResult<SysDictData>> GetSysDictDataDetailAsync(PageDictDataInput input, CancellationToken token = default);
        
        /// </summary>
        /// 设置字典状态
        /// <returns></returns>
        [HttpPost("api/sysDictData/setStatus")]
        ITask<AdminResult<object>> SetSysDictDataStatusAsync([JsonContent] DictInput input, CancellationToken token = default);
        
        /// </summary>
        /// 根据编码获取数据列表
        /// <returns></returns>
        [HttpGet("api/sysDictData/dataList/{code}")]
        ITask<AdminResult<List<SysDictData>>> GetSysDictDataListByCodeAsync(string code, CancellationToken token = default);
        
        /// </summary>
        /// 根据编码获取数据列表
        /// <returns></returns>
        [HttpGet("api/sysDictData/dataList")]
        ITask<AdminResult<List<SysDictData>>> GetSysDictDataListAsync(QueryDictDataInput input, CancellationToken token = default);

    }
}
