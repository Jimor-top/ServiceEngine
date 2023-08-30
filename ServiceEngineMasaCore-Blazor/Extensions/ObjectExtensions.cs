namespace ServiceEngineMasaCore.Blazor.Extensions
{

    /// <summary>
    /// Object 扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        public static object? ChangeType(object? value, Type? type, IFormatProvider? provider = null)
        {
            if (value is null || type is null)
            {
                return null;
            }

            var valueType = value.GetType();
            if (type == valueType)
            {
                return value;
            }

            //如果是枚举类型
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return ChangeType(value, Nullable.GetUnderlyingType(type), provider);
                }

                if (!type.IsInterface)
                {
                    var innerType = type.GetGenericArguments()[0];
                    var innerValue = ChangeType(value, innerType, provider);
                    return Activator.CreateInstance(type, new object?[] { innerValue });
                }

                return null;
            }
            else if (type == typeof(DateTimeOffset))
            {
                if (value is DateTime date)
                {
                    return new DateTimeOffset(date);
                }
                else if (DateTimeOffset.TryParse(value.ToString(), provider, DateTimeStyles.None, out var offset))
                {
                    return offset;
                }

                return null;
            }
            else if (type == typeof(bool))
            {
                var str = value.ToString();
                if (string.IsNullOrWhiteSpace(str))
                {
                    return null;
                }
                else if (str == "1" || str.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (str == "0" || str.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
            else if (type == typeof(Guid))
            {
                return Guid.TryParseExact(value.ToString(), null, out var result) ? result : null;
            }
            else if (type.IsEnum)
            {
                return Enum.TryParse(type, value.ToString(), out var result) ? result : null;
            }

            if (value is IConvertible)
            {
                return Convert.ChangeType(value, type, provider);
            }
            else if (value is DateTimeOffset offsetValue)
            {
                return ChangeType(offsetValue.Date, type, provider);
            }
            else
            {
                return null;
            }
        }
    }

}
