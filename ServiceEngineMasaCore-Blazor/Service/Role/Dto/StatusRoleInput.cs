using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.Role.Dto
{
    public class StatusRoleInput
    {
        public virtual long Id { get; set; }
        public virtual StatusEnum Status { get; set; }
    }
}
