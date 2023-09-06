using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using Microsoft.AspNetCore.Authorization;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers
{
    [JwtAuthentication]
    public interface IUserClient : IHttpApi
    {
        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/page")]
        ITask<AdminResult<SqlSugarPagedList<SysUser>>> GetSysUserPageAsync([JsonContent] PageInputDto input, CancellationToken token = default);
        /// <summary>
        /// 获取登录账号
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysAuth/userInfo")]
        ITask<AdminResult<LoginUserOutput>> GetSysAuthUserInfoAsync(CancellationToken token = default);

        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysUser/baseInfo")]
        ITask<AdminResult<SysUser>> GetSysUserBaseInfoAsync(CancellationToken token = default);

        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/baseInfo")]
        ITask<AdminResult<int>> PostSysUserBaseInfoAsync([JsonContent] SysUser input, CancellationToken token = default);

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/setStatus")]
        ITask<AdminResult<int>> SetSysUserStatusAsync([JsonContent] UInput input, CancellationToken token = default);

        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/changePwd")]
        ITask<AdminResult<object>> ChangeSysUserPwd([JsonContent] UserPwdDto input, CancellationToken token = default);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/update")]
        ITask<AdminResult<string>> UpdateSysUserBaseInfoAsync([JsonContent] SysUser input, CancellationToken token = default);

        /// <summary>
        /// 获取用户配置
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysAuth/userConfig")]
        ITask<AdminResult<LoginUserOutput>> GetSysAuthUserConfigAsync(CancellationToken token = default);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysAuth/logout")]
        ITask<AdminResult<object>> SysAuthUserlogoutAsync(CancellationToken token = default);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/resetPwd")]
        ITask<AdminResult<int>> ResetPwdAsync([JsonContent] ResetPwdInput input,CancellationToken token = default);

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/add")]
        ITask<AdminResult<object>> AddSysUserAsync([JsonContent] AddUserInput input, CancellationToken token = default);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/update")]
        ITask<AdminResult<object>> UpdateSysUserAsync([JsonContent] UpdateUserInput input, CancellationToken token = default);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/delete")]
        ITask<AdminResult<object>> DeleteSysUserAsync([JsonContent] DeleteInput input, CancellationToken token = default);

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysUser/ownRoleList/{userId}")]
        ITask<AdminResult<List<long>>> GetSysUserRoleListAsync(long userId, CancellationToken token = default);

        /// <summary>
        /// 获取用户附属角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysUser/ownExtOrgList/{userId}")]
        ITask<AdminResult<List<SysUserExtOrg>>> GetSysUserExtRoleListAsync(long userId, CancellationToken token = default);
    }
}
