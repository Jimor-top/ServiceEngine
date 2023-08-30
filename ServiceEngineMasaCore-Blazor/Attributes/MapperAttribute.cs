namespace ServiceEngineMasaCore.Blazor.Attributes
{
    /// <summary>
    /// 映射字典
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = true)]
    public class MapperAttribute : Attribute
    {
        public MapperAttribute(string key, string? value) => (Key, Value) = (key, value);

        /// <summary>
        /// 键，Pascal case（帕斯卡命名）
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 值
        /// </summary>
        public string? Value { get; }
    }
}
