using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Service
{
    public class SysUserServices : BaseService, ISysUserService
    {
        private readonly IUserClient _client;

        public SysUserServices(IUserClient client, IPopupService popup) : base(popup)
            => _client = client;

        public Task<AdminResult<SqlSugarPagedList<SysUser>>> GetSysUserPageAsync(PageInputDto input)
            => HandleErrorAsync(_client.GetSysUserPageAsync(input));

        public Task<AdminResult<LoginUserOutput>> GetSysAuthUserInfoAsync()
            => HandleErrorAsync(_client.GetSysAuthUserInfoAsync());

        public Task<AdminResult<LoginUserOutput>> GetSysAuthUserConfigAsync()
            => HandleErrorAsync(_client.GetSysAuthUserConfigAsync());

        public Task<AdminResult<SysUser>> GetSysUserBaseInfoAsync()
           => HandleErrorAsync(_client.GetSysUserBaseInfoAsync());

        public Task<AdminResult<int>> PostSysUserBaseInfoAsync(SysUser input)
            => HandleErrorAsync(_client.PostSysUserBaseInfoAsync(input));

        public Task<AdminResult<object>> ChangeSysUserPwd(UserPwdDto input)
          => HandleErrorAsync(_client.ChangeSysUserPwd(input));

        public Task<AdminResult<string>> UpdateSysUserBaseInfoAsync(SysUser input)
            => HandleErrorAsync(_client.UpdateSysUserBaseInfoAsync(input));

        public Task<AdminResult<object>> SysAuthUserlogoutAsync()
          => HandleErrorAsync(_client.SysAuthUserlogoutAsync());

        public Task<AdminResult<int>> SetSysUserStatusAsync(UInput input)
         => HandleErrorAsync(_client.SetSysUserStatusAsync(input));

        public Task<AdminResult<int>> ResetPwdAsync(ResetPwdInput input)
            => HandleErrorAsync(_client.ResetPwdAsync(input));

        public Task<AdminResult<object>> AddSysUserAsync(AddUserInput input)
            => HandleErrorAsync(_client.AddSysUserAsync(input));

        public Task<AdminResult<object>> UpdateSysUserAsync(UpdateUserInput input)
            => HandleErrorAsync(_client.UpdateSysUserAsync(input));

        public Task<AdminResult<object>> DeleteSysUserAsync(DeleteInput input)
            => HandleErrorAsync(_client.DeleteSysUserAsync(input));

        public Task<AdminResult<List<long>>> GetSysUserRoleListAsync(long userId)
            => HandleErrorAsync(_client.GetSysUserRoleListAsync(userId));

        public Task<AdminResult<List<SysUserExtOrg>>> GetSysUserExtRoleListAsync(long userId)
           => HandleErrorAsync(_client.GetSysUserExtRoleListAsync(userId));
    }
}
