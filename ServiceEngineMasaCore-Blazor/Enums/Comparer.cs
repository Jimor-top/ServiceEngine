using ServiceEngine.Core.Const;
using ServiceEngineMasaCore.Blazor.Attributes;

namespace ServiceEngineMasaCore.Blazor.Enums
{

    /// <summary>
    /// 指定筛选器的比较运算符。
    /// </summary>
    public enum Comparer : int
    {
        /// <summary>
        /// 如果当前值等于指定值，则满足。
        /// </summary>
        [Description("未设置")]
        [Mapper(Constants.Symbol, "?")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.empty")]
        Empty = 0,

        /// <summary>
        /// 如果当前值等于指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "=")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.equals")]
        [Description("等于")]
        Equals = 1,

        /// <summary>
        /// 如果当前值不等于指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "≠")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.notEquals")]
        [Description("不等于")]
        NotEquals = 2,

        /// <summary>
        /// 如果当前值小于指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "<")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.lessThan")]
        [Description("小于")]
        LessThan = 3,

        /// <summary>
        /// 如果当前值小于或等于指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "≤")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.lessThanOrEquals")]
        [Description("小于等于")]
        LessThanOrEquals = 4,

        /// <summary>
        /// 如果当前值大于指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, ">")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.greaterThan")]
        [Description("大于")]
        GreaterThan = 5,

        /// <summary>
        /// 如果当前值大于或等于指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "≥")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.greaterThanOrEquals")]
        [Description("大于等于")]
        GreaterThanOrEquals = 6,

        /// <summary>
        /// 如果当前值包含指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "*A*")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.contains")]
        [Description("包含")]
        Contains = 7,

        /// <summary>
        /// 如果当前值以指定值开始，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "A**")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.startsWith")]
        [Description("头匹配")]
        StartsWith = 8,

        /// <summary>
        /// 如果当前值以指定值结束，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "**A")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.endsWith")]
        [Description("尾匹配")]
        EndsWith = 9,

        /// <summary>
        /// 如果当前值不包含指定值，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "≠*A*")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.doesNotContain")]
        [Description("不包含")]
        DoesNotContain = 10,

        /// <summary>
        /// 如果当前值为null，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "∅")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.isNull")]
        [Description("是空值")]
        IsNull = 11,

        /// <summary>
        /// 如果当前值为 <see cref="string.Empty"/>，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "=\"_\"")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.isEmpty")]
        [Description("是空字符串")]
        IsEmpty = 12,

        /// <summary>
        /// 如果当前值不为null，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "≠∅")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.isNotNull")]
        [Description("不是空值")]
        IsNotNull = 13,

        /// <summary>
        /// 如果当前值不为 <see cref="string.Empty"/>，则满足。
        /// </summary>
        [Mapper(Constants.Symbol, "≠\"_\"")]
        [Mapper(Constants.I18nKey, "$enjoyBlazor.comparer.isNotEmpty")]
        [Description("不是空字符串")]
        IsNotEmpty = 14
    }
}
