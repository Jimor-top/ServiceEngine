using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface
{
    public interface ISysUserService
    {
        Task<AdminResult<SqlSugarPagedList<SysUser>>> GetSysUserPageAsync(PageInputDto input);
        Task<AdminResult<LoginUserOutput>> GetSysAuthUserInfoAsync();
        Task<AdminResult<LoginUserOutput>> GetSysAuthUserConfigAsync();
        Task<AdminResult<SysUser>> GetSysUserBaseInfoAsync();
        Task<AdminResult<int>> PostSysUserBaseInfoAsync(SysUser userinfo);
        Task<AdminResult<object>> ChangeSysUserPwd(UserPwdDto body);
        Task<AdminResult<string>> UpdateSysUserBaseInfoAsync(SysUser userinfo);
        Task<AdminResult<object>> SysAuthUserlogoutAsync();
    }
}
