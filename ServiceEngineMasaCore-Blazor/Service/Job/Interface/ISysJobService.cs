using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Job.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Job.Interface
{
    public interface ISysJobService
    {
        Task<AdminResult<SqlSugarPagedList<JobOutput>>> GetSysJobPageAsync(PJobInput input);
    }
}
