using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceEngine.Core.Util;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using ServiceEngineMasaCore.Blazor.Service.Cache.Callers;
using ServiceEngineMasaCore.Blazor.Service.Cache.Interface;
using ServiceEngineMasaCore.Blazor.Service.Cache.Service;
using ServiceEngineMasaCore.Blazor.Service.Config.Callers;
using ServiceEngineMasaCore.Blazor.Service.Config.Interface;
using ServiceEngineMasaCore.Blazor.Service.Config.Service;
using ServiceEngineMasaCore.Blazor.Service.Dict.Callers;
using ServiceEngineMasaCore.Blazor.Service.Dict.Interface;
using ServiceEngineMasaCore.Blazor.Service.Dict.Service;
using ServiceEngineMasaCore.Blazor.Service.File.Callers;
using ServiceEngineMasaCore.Blazor.Service.File.Interface;
using ServiceEngineMasaCore.Blazor.Service.File.Service;
using ServiceEngineMasaCore.Blazor.Service.Job.Callers;
using ServiceEngineMasaCore.Blazor.Service.Job.Interface;
using ServiceEngineMasaCore.Blazor.Service.Job.Service;
using ServiceEngineMasaCore.Blazor.Service.Log.Callers;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using ServiceEngineMasaCore.Blazor.Service.Log.Service;
using ServiceEngineMasaCore.Blazor.Service.Login.Callers;
using ServiceEngineMasaCore.Blazor.Service.Login.Interface;
using ServiceEngineMasaCore.Blazor.Service.Login.Service;
using ServiceEngineMasaCore.Blazor.Service.Menu;
using ServiceEngineMasaCore.Blazor.Service.Menu.Callers;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Service;
using ServiceEngineMasaCore.Blazor.Service.Notice.Callers;
using ServiceEngineMasaCore.Blazor.Service.Notice.Interface;
using ServiceEngineMasaCore.Blazor.Service.Notice.Service;
using ServiceEngineMasaCore.Blazor.Service.Org.Callers;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.Org.Service;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Caller;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Interface;
using ServiceEngineMasaCore.Blazor.Service.Plugin.Service;
using ServiceEngineMasaCore.Blazor.Service.Pos.Callers;
using ServiceEngineMasaCore.Blazor.Service.Pos.Interface;
using ServiceEngineMasaCore.Blazor.Service.Pos.Service;
using ServiceEngineMasaCore.Blazor.Service.Print.Callers;
using ServiceEngineMasaCore.Blazor.Service.Print.Interface;
using ServiceEngineMasaCore.Blazor.Service.Print.Service;
using ServiceEngineMasaCore.Blazor.Service.Region.Callers;
using ServiceEngineMasaCore.Blazor.Service.Region.Interface;
using ServiceEngineMasaCore.Blazor.Service.Region.Service;
using ServiceEngineMasaCore.Blazor.Service.Role.Callers;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Service;
using ServiceEngineMasaCore.Blazor.Service.Server.Callers;
using ServiceEngineMasaCore.Blazor.Service.Server.Interface;
using ServiceEngineMasaCore.Blazor.Service.Server.Service;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Callers;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Interface;
using ServiceEngineMasaCore.Blazor.Service.Tenant.Service;
using ServiceEngineMasaCore.Blazor.Service.UserInfo;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Callers;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Service;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Callers;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Interface;
using ServiceEngineMasaCore.Blazor.Service.Wechat.Service;

namespace ServiceEngineMasaCore.Blazor.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterAppServices(this IServiceCollection services, string baseAddress)
        {
            services.AddSingleton<RouterPagesProvider>();
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();

            services.AddSingleton<IJSInvoker, JSInvoker>();

            services
           .AddScoped<JwtAuthenticationStateProvider>()
           .AddScoped<IJwtStorageService>(x => x.GetRequiredService<JwtAuthenticationStateProvider>())
           .AddScoped<IJSInvokerService>(x => x.GetRequiredService<JwtAuthenticationStateProvider>())
           .AddScoped<AuthenticationStateProvider>(x => x.GetRequiredService<JwtAuthenticationStateProvider>())
           .AddScoped<GlobalConfig>();

            #region 注册webapi请求代理
            services.AddHttpApi<IAuthClient>();
            services.AddHttpApi<IConstClient>();
            services.AddHttpApi<IUserClient>();
            services.AddHttpApi<IMenuClient>();
            services.AddHttpApi<ISysOrgClient>();
            services.AddHttpApi<ILogClient>();
            services.AddHttpApi<IRoleClient>();
            services.AddHttpApi<IPosClient>();
            services.AddHttpApi<INoticeClient>();
            services.AddHttpApi<IWeChatClient>();
            services.AddHttpApi<ITenantClient>();
            services.AddHttpApi<IConfigClient>();
            services.AddHttpApi<IDictClient>();
            services.AddHttpApi<IJobClient>();
            services.AddHttpApi<IServerClient>();
            services.AddHttpApi<IFileClient>();
            services.AddHttpApi<IPrintClient>();
            services.AddHttpApi<IPluginClient>();
            services.AddHttpApi<IRegionClient>();
            services.AddHttpApi<ICacheClient>();
            services.AddWebApiClient().ConfigureHttpApi(x =>
            {
                //x.GlobalFilters.Add(new LogFilter());
                x.HttpHost = new Uri(baseAddress);
                x.Properties.Add("Cache-Control", "no-cache");
                x.Properties.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
            });


            #endregion
            #region 注册业务服务
            services.AddScoped<ILocalStorageUtil, LocalStorageUtil>();
            services.AddScoped<ISessionStorageUtil, SessionStorageUtil>();
            services.AddScoped<ISysAuthService, SysAuthService>();
            services.AddScoped<IMenuStore, MenuStore>();
            services.AddScoped<ISysConstService, SysConstServices>();
            services.AddScoped<ISysUserService, SysUserServices>();
            services.AddScoped<IUserInfoStore, UserInfoStore>();
            services.AddScoped<ISysMenuService, SysMenuService>();
            services.AddScoped<ISysOrgService, SysOrgService>();
            services.AddScoped<ISysLogService, SysLogService>();
            services.AddScoped<ISysRoleService, SysRoleService>();
            services.AddScoped<ISysPosService, SysPosService>();
            services.AddScoped<ISysNoticeService, SysNoticeService>();
            services.AddScoped<IWeChatService, WeChatService>();
            services.AddScoped<ISysTenantService, SysTenantService>();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<ISysDictDataService, SysDictDataService>();
            services.AddScoped<ISysJobService, SysJobService>();
            services.AddScoped<ISysServerService, SysServerService>();
            services.AddScoped<ISysFileService, SysFileService>();
            services.AddScoped<ISysPrintService, SysPrintService>();
            services.AddScoped<ISysPluginService, SysPluginService>();
            services.AddScoped<ISysRegionService, SysRegionService>();
            services.AddScoped<ISysCacheService, SysCacheService>();

            #endregion
        }
    }
}
