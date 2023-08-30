using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface
{
    public interface IUserInfoStore
    {
        public Task<List<ConstOutput>> GetSysConstList();
        public Task<bool> SetUserInfos();
        public UserInfoDto GetUserInfos();
    }
}
