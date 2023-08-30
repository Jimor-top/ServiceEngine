using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Interface
{
    public interface ISysDictDataService
    {
        Task<AdminResult<SqlSugarPagedList<SysDictData>>> GetSysDictDataPageAsync([JsonContent] PDictDataInput input);
    }
}
