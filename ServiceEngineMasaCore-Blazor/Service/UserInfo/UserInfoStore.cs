using Newtonsoft.Json;
using ServiceEngine.Core.Service;
using ServiceEngine.Core.Util;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo
{
    public class UserInfoStore : IUserInfoStore
    {
        [Inject]
        [NotNull]
        ISysConstService? _sysConstService { get; set; }

        [Inject]
        [NotNull]
        ISysUserService? _SysUserService { get; set; }

        [Inject]
        [NotNull]
        public ISessionStorageUtil SessionStorageUtil { get; set; }

        public UserInfoStore(ISysConstService sysConstService, ISysUserService sysUserService, ISessionStorageUtil sessionStorageUtil)
            => (_sysConstService, _SysUserService, SessionStorageUtil) = (sysConstService, sysUserService, sessionStorageUtil);


        public List<ConstOutput> ConstList = new List<ConstOutput>();
        public UserInfoDto UserInfos;

        public async Task<List<ConstOutput>> GetSysConstList()
        {
            var result = await _sysConstService.GetSysConstListAsync();
            ConstList = result.Result ?? new List<ConstOutput>();
            return ConstList;
        }

        public async Task<bool> SetUserInfos()
        {
            // 存储用户信息到浏览器缓存
            var userInfos = await SessionStorageUtil.Get<UserInfoDto>("userInfo");
            if (userInfos == null)
            {
                userInfos = await GetSysAuthUserInfoAsync();
            }
            if (userInfos != null)
            {
                UserInfos = userInfos;
                return true;
            }
            return false;
        }

        public UserInfoDto GetUserInfos() {
            return UserInfos;
        }

        private async Task<UserInfoDto> GetSysAuthUserInfoAsync()
        {
            var result = await _SysUserService.GetSysAuthUserInfoAsync();
            if (result.Result == null) return new UserInfoDto();
            var data = result.Result;
            var userInfos = new UserInfoDto
            {
                Account = data.Account,
                RealName = data.RealName,
                Avatar = string.IsNullOrWhiteSpace(data.Avatar) ? data.Avatar : "/favicon.ico",
                Address = data.Address,
                Signature = data.Signature,
                OrgId = data.OrgId,
                OrgName = data.OrgName,
                PosName = data.PosName,
                Roles = new List<string>(),
                AuthBtnList = data.Buttons,
                LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            return userInfos;
        }
    }

}
