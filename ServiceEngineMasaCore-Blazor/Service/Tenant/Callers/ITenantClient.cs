using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Tenant.Callers
{
    [JwtAuthentication]
    public interface ITenantClient : IHttpApi
    {
        /// <summary>
        /// 获取租户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/page")]
        ITask<AdminResult<SqlSugarPagedList<TenantOutput>>> GetSysTenantPageAsync([JsonContent] PTenantInput input, CancellationToken token = default);

        /// <summary>
        /// 设置租户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/setStatus")]
        ITask<AdminResult<int>> SetSysTenantStatusAsync([JsonContent] TTenantInput input, CancellationToken token = default);

        /// <summary>
        /// 获取租户管理员角色拥有菜单Id集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysTenant/ownMenuList")]
        ITask<AdminResult<List<long>>> GetSysTenantMenuListAsync(TenantUserInput input, CancellationToken token = default);

        /// <summary>
        /// 重置租户管理员密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/resetPwd")]
        ITask<AdminResult<object>> ResetPwdSysTenantAsync(TenantUserInput input, CancellationToken token = default);

        /// <summary>
        /// 增加租户
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/add")]
        ITask<AdminResult<object>> AddSysTenantAsync(AddTenantInput input, CancellationToken token = default);

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/delete")]
        ITask<AdminResult<object>> DeleteSysTenantAsync(DeleteTenantInput input, CancellationToken token = default);

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/update")]
        ITask<AdminResult<object>> UpdateSysTenantAsync(UpdateTenantInput input, CancellationToken token = default);

        /// <summary>
        /// 创建租户数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysTenant/createDb")]
        ITask<AdminResult<object>> CreateSysTenantDbAsync(TTenantInput input, CancellationToken token = default);
    }
}
