using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Notice.Callers;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;
using ServiceEngineMasaCore.Blazor.Service.Notice.Interface;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Notice.Service
{
    public class SysNoticeService : BaseService, ISysNoticeService
    {
        public INoticeClient _client;
        public SysNoticeService(INoticeClient clinet, IPopupService popup) : base(popup)
            => _client = clinet;

        public Task<AdminResult<object>> AddSysNoticeAsync(AddNoticeInput input)
            => HandleErrorAsync(_client.AddSysNoticeAsync(input));

        public Task<AdminResult<object>> DeleteSysNoticeAsync(NotiInput input)
            => HandleErrorAsync(_client.DeleteSysNoticeAsync(input));

        public Task<AdminResult<SqlSugarPagedList<SysNotice>>> GetSysNoticePageAsync(PNoticeInput input)
            => HandleErrorAsync(_client.GetSysNoticePageAsync(input));

        public Task<AdminResult<object>> PublicSysNoticeAsync(NotiInput input)
            => HandleErrorAsync(_client.PublicSysNoticeAsync(input));

        public Task<AdminResult<SqlSugarPagedList<SysNoticeUser>>> ReceivedSysNoticeAsync(PNoticeInput input)
            => HandleErrorAsync(_client.ReceivedSysNoticeAsync(input));

        public Task<AdminResult<object>> SetReadSysNoticeAsync(NotiInput input)
            => HandleErrorAsync(_client.SetReadSysNoticeAsync(input));

        public Task<AdminResult<List<SysNotice>>> UnReadSysNoticeAsync()
            => HandleErrorAsync(_client.UnReadSysNoticeAsync());

        public Task<AdminResult<object>> UpdateSysNoticeAsync(UpdateNoticeInput input)
            => HandleErrorAsync(_client.UpdateSysNoticeAsync(input));
    }
}
