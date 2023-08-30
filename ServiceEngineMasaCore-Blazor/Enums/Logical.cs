using ServiceEngineMasaCore.Blazor.Attributes;
using ServiceEngine.Core.Const;

namespace ServiceEngineMasaCore.Blazor.Enums
{
    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum Logical
    {
        /// <summary>
        /// 并且
        /// </summary>
        [Description("并且")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.logical.and")]
        And = 0,

        /// <summary>
        /// 或者
        /// </summary>
        [Description("或者")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.logical.or")]
        Or = 1,
    }
}
