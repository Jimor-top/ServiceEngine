using Furion.DataValidation;
using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Service.Notice.Dto
{
    public class PNoticeInput
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
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 类型（1通知 2公告）
        /// </summary>
        public virtual NoticeTypeEnum? Type { get; set; }
    }
}
