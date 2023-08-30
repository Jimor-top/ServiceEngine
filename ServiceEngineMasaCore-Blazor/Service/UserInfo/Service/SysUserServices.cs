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

        public Task<AdminResult<int>> PostSysUserBaseInfoAsync(SysUser userinfo)
            => HandleErrorAsync(_client.PostSysUserBaseInfoAsync(userinfo));

        public Task<AdminResult<object>> ChangeSysUserPwd(UserPwdDto userPwdDto)
          => HandleErrorAsync(_client.ChangeSysUserPwd(userPwdDto));

        public Task<AdminResult<string>> UpdateSysUserBaseInfoAsync(SysUser userinfo)
            => HandleErrorAsync(_client.UpdateSysUserBaseInfoAsync(userinfo));

        public Task<AdminResult<object>> SysAuthUserlogoutAsync()
          => HandleErrorAsync(_client.SysAuthUserlogoutAsync());

   
    }
}
