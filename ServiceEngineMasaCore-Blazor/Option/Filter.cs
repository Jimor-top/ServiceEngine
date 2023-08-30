using ServiceEngineMasaCore.Blazor.Enums;
using ServiceEngineMasaCore.Blazor.Extensions;
using ServiceEngineMasaCore.Blazor.Interface;
using System.Text.Json.Serialization;

namespace ServiceEngineMasaCore.Blazor.Option
{

    /// <summary>
    /// 数据筛选器
    /// </summary>
    [Serializable]
    public class Filter : IFilter
    {
        private Comparer _comparator = Comparer.Empty;
        private IFormatProvider? _formatProvider;
        private List<Filter>? _children;

        internal Guid Id { get; }

        public Filter() => Id = Guid.NewGuid();

        public Filter(Logical logic) : this() => Logicaler = logic;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Logical Logicaler { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual string? Key { get; set; }

        [JsonIgnore]
        public virtual Type? Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual bool? CaseInsensitive { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Comparer Comparator
        {
            get => _comparator;
            set
            {
                if (value == Comparer.IsEmpty || value == Comparer.IsNotEmpty)
                {
                    Value = string.Empty;
                }
                else if (value == Comparer.IsNull || value == Comparer.IsNotNull)
                {
                    Value = null;
                }

                _comparator = value;
            }
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Value { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IReadOnlyCollection<Filter>? Children => _children;

        public bool IsNoNeedToSetValue()
        {
            return Comparator == Comparer.IsNull || Comparator == Comparer.IsNotNull ||
                Comparator == Comparer.IsEmpty || Comparator == Comparer.IsNotEmpty;
        }

        public IFilter FormatBy(IFormatProvider? provider)
        {
            _formatProvider = provider;
            return this;
        }

        public void SetValue<TValue>(TValue value)
        {
            if (IsNoNeedToSetValue())
            {
                Value = null;
                return;
            }

            Value = value;
        }

        public TValue? GetValue<TValue>()
        {
            if (Value is null)
            {
                return default;
            }

            var value = ObjectExtensions.ChangeType(Value, typeof(TValue), _formatProvider);
            return value is null ? default : (TValue)value;
        }

        public bool IsActive() => _comparator != Comparer.Empty && (IsNoNeedToSetValue() || Value is not null);

        public void Reset()
        {
            Value = null;
            Comparator = Comparer.Empty;
        }

        public virtual IFilter Clone() => new Filter()
        {
            Logicaler = Logicaler,
            Key = Key,
            Type = Type ?? Value?.GetType(),
            CaseInsensitive = CaseInsensitive,
            Comparator = Comparator,
            Value = Value
        };

        public void AddChild(IFilter? filter)
        {
            if (filter is null)
            {
                return;
            }

            if (filter is Filter value)
            {
                _children ??= new List<Filter>();
                _children.Add(value);
            }
        }

        public void RemoveChild(IFilter? filter)
        {
            if (_children is null || filter is null)
            {
                return;
            }

            if (filter is Filter value)
            {
                _children.Remove(value);
            }
        }

        public void ClearChildren()
        {
            if (_children is not null)
            {
                _children.Clear();
                _children = null;
            }
        }
    }

}
