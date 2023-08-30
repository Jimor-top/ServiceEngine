using static SKIT.FlurlHttpClient.Wechat.Api.Models.ProductSPUUpdateRequest.Types;

namespace ServiceEngineMasaCore.Blazor.Common
{
    /// <summary>
    /// 权限点
    /// </summary>
    public class Permission
    {
        public Permission(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public Permission(string code, string name, string? description) : this(code, name)
        {
            Description = description;
        }

        /// <summary>
        /// 静态编码
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; }

        public override string ToString() => Code;
    }

}
