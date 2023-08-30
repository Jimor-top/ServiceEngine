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
        ITask<AdminResult<int>> PostSysUserBaseInfoAsync([JsonContent] SysUser body, CancellationToken token = default);


        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/changePwd")]
        ITask<AdminResult<object>> ChangeSysUserPwd([JsonContent] UserPwdDto body, CancellationToken token = default);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysUser/update")]
        ITask<AdminResult<string>> UpdateSysUserBaseInfoAsync([JsonContent] SysUser body,CancellationToken token = default);

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
    }
}
