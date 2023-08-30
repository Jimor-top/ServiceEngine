using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Notice.Interface
{
    public interface ISysNoticeService
    {
        Task<AdminResult<SqlSugarPagedList<SysNotice>>> GetSysNoticePageAsynct([JsonContent] PNoticeInput input);
    }
}
