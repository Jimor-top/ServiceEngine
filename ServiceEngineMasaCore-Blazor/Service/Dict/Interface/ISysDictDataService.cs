using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Interface
{
    public interface ISysDictDataService
    {
        Task<AdminResult<SqlSugarPagedList<SysDictData>>> GetSysDictDataPageAsync(PDictDataInput input);
        Task<AdminResult<List<SysDictData>>> GetSysDictDataListAsync(long DictTypeId);
        Task<AdminResult<SysDictData>> GetSysDictDataDetailAsync(PageDictDataInput input);
        Task<AdminResult<object>> SetSysDictDataStatusAsync(DictInput input);
        Task<AdminResult<List<SysDictData>>> GetSysDictDataListByCodeAsync(string code);
        Task<AdminResult<List<SysDictData>>> GetSysDictDataListAsync(QueryDictDataInput input);
    }
}
