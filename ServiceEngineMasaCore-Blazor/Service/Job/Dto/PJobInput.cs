using Furion.DataValidation;

namespace ServiceEngineMasaCore.Blazor.Service.Job.Dto
{
    public class PJobInput
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public virtual int Page { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        //[Range(0, 100, ErrorMessage = "页码容量超过最大限制")]
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
        /// 作业Id
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
    }
}
