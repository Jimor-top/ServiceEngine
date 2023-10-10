using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Dict.Callers;
using ServiceEngineMasaCore.Blazor.Service.Dict.Dto;
using ServiceEngineMasaCore.Blazor.Service.Dict.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Callers;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using WebApiClientCore.Attributes;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBBatchCreateContainerServiceVersionRequest.Types;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Service
{
    public class SysDictDataService : BaseService, ISysDictDataService
    {
        private readonly IDictClient _client;
        public SysDictDataService(IDictClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<SysDictData>> GetSysDictDataDetailAsync(PageDictDataInput input)
          => HandleErrorAsync(_client.GetSysDictDataDetailAsync(input));

        public Task<AdminResult<List<SysDictData>>> GetSysDictDataListAsync(long DictTypeId)
            => HandleErrorAsync(_client.GetSysDictDataListAsync(DictTypeId));

        public Task<AdminResult<List<SysDictData>>> GetSysDictDataListAsync(QueryDictDataInput input)
            => HandleErrorAsync(_client.GetSysDictDataListAsync(input));

        public Task<AdminResult<List<SysDictData>>> GetSysDictDataListByCodeAsync(string code)
            => HandleErrorAsync(_client.GetSysDictDataListByCodeAsync(code));

        public Task<AdminResult<SqlSugarPagedList<SysDictData>>> GetSysDictDataPageAsync(PDictDataInput input)
          => HandleErrorAsync(_client.GetSysDictDataPageAsync(input));

        public Task<AdminResult<object>> SetSysDictDataStatusAsync(DictInput input)
            => HandleErrorAsync(_client.SetSysDictDataStatusAsync(input));
    }
}
