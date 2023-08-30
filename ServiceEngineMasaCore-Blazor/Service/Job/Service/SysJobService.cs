using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Job.Callers;
using ServiceEngineMasaCore.Blazor.Service.Job.Dto;
using ServiceEngineMasaCore.Blazor.Service.Job.Interface;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Callers;
using WebApiClientCore.Attributes;

namespace ServiceEngineMasaCore.Blazor.Service.Job.Service
{
    public class SysJobService : BaseService, ISysJobService
    {
        private readonly IJobClient _client;
        public SysJobService(IJobClient client, IPopupService popup) : base(popup)
          => _client = client;

        public Task<AdminResult<SqlSugarPagedList<JobOutput>>> GetSysJobPageAsync(PJobInput input)
         => HandleErrorAsync(_client.GetSysJobPageAsync(input));

    }
}
