using Furion.DataValidation;

namespace ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto
{
    public class PageInputDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        //[Range(0, 100, ErrorMessage = "页码容量超过最大限制")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 降序排序
        /// </summary>
        public string DescStr { get; set; } = "descending";
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 查询时所选机构Id
        /// </summary>
        public long OrgId { get; set; }
    }
}
