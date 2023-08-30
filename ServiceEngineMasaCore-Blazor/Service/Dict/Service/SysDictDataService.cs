using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Dict.Callers;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using ServiceEngineMasaCore.Blazor.Service.Dict.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Callers;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Service
{
    public class SysDictDataService : BaseService, ISysDictDataService
    {
        private readonly IDictClient _client;
        public SysDictDataService(IDictClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysDictData>>> GetSysDictDataPageAsync([JsonContent] PDictDataInput input)
          => HandleErrorAsync(_client.GetSysDictDataPageAsync(input));
    }
}
