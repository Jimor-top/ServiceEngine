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
        Task<AdminResult<int>> SetSysUserStatusAsync(UInput input);
        Task<AdminResult<int>> PostSysUserBaseInfoAsync(SysUser input);
        Task<AdminResult<object>> ChangeSysUserPwd(UserPwdDto input);
        Task<AdminResult<string>> UpdateSysUserBaseInfoAsync(SysUser input);
        Task<AdminResult<object>> SysAuthUserlogoutAsync();
        Task<AdminResult<int>> ResetPwdAsync(ResetPwdInput input);
        Task<AdminResult<object>> AddSysUserAsync(AddUserInput input);
        Task<AdminResult<object>> UpdateSysUserAsync(UpdateUserInput input);
        Task<AdminResult<object>> DeleteSysUserAsync(DeleteInput input);
        Task<AdminResult<List<long>>> GetSysUserRoleListAsync(long userId);
        Task<AdminResult<List<SysUserExtOrg>>> GetSysUserExtRoleListAsync(long userId);
    }
}
