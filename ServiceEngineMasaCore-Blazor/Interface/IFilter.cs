using ServiceEngineMasaCore.Blazor.Enums;

namespace ServiceEngineMasaCore.Blazor.Interface
{

    public interface IFilter
    {
        /// <summary>
        /// 与其它条件之间的逻辑关系 
        /// </summary>
        Logical Logicaler { get; set; }

        /// <summary>
        /// 条件比较符
        /// </summary>
        Comparer Comparator { get; set; }

        string? Key { get; }

        Type? Type { get; }

        bool? CaseInsensitive { get; }

        object? Value { get; }

        bool IsNoNeedToSetValue();

        IFilter FormatBy(IFormatProvider? provider);

        void SetValue<TValue>(TValue value);

        TValue? GetValue<TValue>();

        bool IsActive();

        void Reset();

        IFilter Clone();
    }
}
