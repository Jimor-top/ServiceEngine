namespace ServiceEngineMasaCore.Blazor.Service.Role.Dto
{
    public class DataScopeRoleInput
    {
        public virtual long Id { get; set; }
        /// <summary>
        /// 数据范围
        /// </summary>
        public int DataScope { get; set; }

        /// <summary>
        /// 机构Id集合
        /// </summary>
        public List<long> OrgIdList { get; set; }
    }
}
