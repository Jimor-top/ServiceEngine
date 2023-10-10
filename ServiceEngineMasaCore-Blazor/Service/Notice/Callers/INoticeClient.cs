using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Notice.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Notice.Callers
{
    [JwtAuthentication]
    public interface INoticeClient : IHttpApi
    {
        /// <summary>
        /// 获取通知公告列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/page")]
        ITask<AdminResult<SqlSugarPagedList<SysNotice>>> GetSysNoticePageAsync([JsonContent] PNoticeInput input, CancellationToken token = default);

        /// <summary>
        /// 发布通知公告
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/public")]
        ITask<AdminResult<object>> PublicSysNoticeAsync([JsonContent] NotiInput input, CancellationToken token = default);

        /// <summary>
        /// 设置通知公告已读状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/setRead")]
        ITask<AdminResult<object>> SetReadSysNoticeAsync([JsonContent] NotiInput input, CancellationToken token = default);


        /// <summary>
        /// 获取接收的通知公告
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysNotice/pageReceived")]
        ITask<AdminResult<SqlSugarPagedList<SysNoticeUser>>> ReceivedSysNoticeAsync(PNoticeInput input, CancellationToken token = default);

        /// <summary>
        /// 获取未读的通知公告
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysNotice/unReadList")]
        ITask<AdminResult<List<SysNotice>>> UnReadSysNoticeAsync(CancellationToken token = default);

        /// <summary>
        /// 增加通知公告
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/add")]
        ITask<AdminResult<object>> AddSysNoticeAsync([JsonContent] AddNoticeInput input, CancellationToken token = default);

        /// <summary>
        /// 更新通知公告
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/update")]
        ITask<AdminResult<object>> UpdateSysNoticeAsync([JsonContent] UpdateNoticeInput input, CancellationToken token = default);

        /// <summary>
        /// 删除通知公告
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysNotice/delete")]
        ITask<AdminResult<object>> DeleteSysNoticeAsync([JsonContent] NotiInput input, CancellationToken token = default);
    }
}
