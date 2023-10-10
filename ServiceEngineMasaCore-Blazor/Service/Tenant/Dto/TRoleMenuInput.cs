using Furion.DataValidation;

namespace ServiceEngineMasaCore.Blazor.Service.Tenant.Dto
{
    public class TRoleMenuInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 菜单Id集合
        /// </summary>
        public List<long> MenuIdList { get; set; }
    }
}
