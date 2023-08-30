using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace ServiceEngine.Application;

[AppStartup(100)]
public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}