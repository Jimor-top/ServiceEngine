using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Role.Callers
{
    [JwtAuthentication]
    public interface IRoleClient : IHttpApi
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/page")]
        ITask<AdminResult<SqlSugarPagedList<SysRole>>> GetSysRolePageAsync([JsonContent] PRoleInput input, CancellationToken token = default);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysRole/list")]
        ITask<AdminResult<List<RoleOutput>>> GetSysRoleListAsync(CancellationToken token = default);

        /// <summary>
        /// 授权数据范围
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/grantDataScope")]
        ITask<AdminResult<object>> SysRoleGrantDataScopeAsync([JsonContent] DataScopeRoleInput input,CancellationToken token = default);

        /// <summary>
        /// 设置角色状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/setStatus")]
        ITask<AdminResult<int>> SetSysRoleStatusAsync([JsonContent] StatusRoleInput input,CancellationToken token = default);

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/add")]
        ITask<AdminResult<object>> AddSysRoleAsync([JsonContent] AddRoleInput input, CancellationToken token = default);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/update")]
        ITask<AdminResult<object>> UpdateSysRoleAsync([JsonContent] UpdateRoleInput input, CancellationToken token = default);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysRole/delete")]
        ITask<AdminResult<object>> DeleteSysRoleAsync([JsonContent] DltRoleInput input, CancellationToken token = default);

        /// <summary>
        /// 获取角色个人机构列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysRole/ownOrgList")]
        ITask<AdminResult<List<long>>> GetOwnOrgListAsync(long Id, CancellationToken token = default);

        /// <summary>
        /// 获取角色个人菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysRole/ownMenuList")]
        ITask<AdminResult<List<long>>> GetOwnMenuListAsync(long Id, CancellationToken token = default);
    }
}
