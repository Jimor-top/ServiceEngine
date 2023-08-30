using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Menu.Callers
{
    [JwtAuthentication]
    public interface IMenuClient : IHttpApi
    {
        /// <summary>
        /// 获取登录账号
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysMenu/loginMenuTree")]
        ITask<AdminResult<List<MenuOutput>>> GetSysMenuTreeAsync(CancellationToken token = default);

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysMenu/list")]
        ITask<AdminResult<List<SysMenu>>> GetSysMenuListAsync(string title, MenuTypeEnum? type, CancellationToken token = default);
    }
}
