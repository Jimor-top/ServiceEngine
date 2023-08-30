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

        public Task<AdminResult<SqlSugarPagedList<SysNotice>>> GetSysNoticePageAsynct([JsonContent] PNoticeInput input)
            => HandleErrorAsync(_client.GetSysNoticePageAsynct(input));
    }
}
