using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Notice.Interface
{
    public interface ISysNoticeService
    {
        Task<AdminResult<SqlSugarPagedList<SysNotice>>> GetSysNoticePageAsync(PNoticeInput input);
        Task<AdminResult<object>> PublicSysNoticeAsync(NotiInput input);
        Task<AdminResult<object>> SetReadSysNoticeAsync(NotiInput input);
        Task<AdminResult<SqlSugarPagedList<SysNoticeUser>>> ReceivedSysNoticeAsync(PNoticeInput input);
        Task<AdminResult<List<SysNotice>>> UnReadSysNoticeAsync();
        Task<AdminResult<object>> AddSysNoticeAsync(AddNoticeInput input);
        Task<AdminResult<object>> UpdateSysNoticeAsync(UpdateNoticeInput input);
        Task<AdminResult<object>> DeleteSysNoticeAsync(NotiInput input);
    }
}
