namespace ServiceEngineMasaCore.Blazor.Common
{

    /// <summary>
    /// RESTful风格---XIAONUO返回格式
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class XnRestfulResult<TItem>
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public TItem? Data { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string? Extras { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
    }
}
