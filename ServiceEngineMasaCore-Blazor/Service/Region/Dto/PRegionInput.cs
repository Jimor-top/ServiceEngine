using Furion.DataValidation;

namespace ServiceEngineMasaCore.Blazor.Service.Region.Dto
{
    public class PRegionInput
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public virtual int Page { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        public virtual int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序字段
        /// </summary>
        public virtual string Field { get; set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public virtual string Order { get; set; }

        /// <summary>
        /// 降序排序
        /// </summary>
        public virtual string DescStr { get; set; } = "descending";
        /// <summary>
        /// 父节点Id
        /// </summary>
        public long Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
