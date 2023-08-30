using Furion.DataValidation;

namespace ServiceEngineMasaCore.Blazor.Service.Dict.Dto
{
    public class PDictDataInput
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
        /// 字典类型Id
        /// </summary>
        public long DictTypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
