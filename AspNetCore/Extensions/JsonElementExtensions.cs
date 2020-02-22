namespace System.Text.Json
{
    using JetBrains.Annotations;
    using Linq;
    using static JsonValueKind;
    using static StringComparison;

    /// <summary>A class with methods that extend <see cref="JsonElement"/>.</summary>
    [PublicAPI]
    public static class JsonElementExtensions
    {
        /// <summary>Maps a <see cref="JsonElement"/> to a <typeparamref name="T"/>.</summary>
        /// <param name="element">The element.</param>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <returns>A mapped <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="element"/> must be an object.</exception>
        public static T ToModel<T>(this JsonElement element)
            where T : class, new()
        {
            if (element.ValueKind != Object)
            {
                throw new ArgumentException("Element must be an object", nameof(element));
            }

            var value = new T();
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var jsonProperty in element.EnumerateObject())
            {
                var property = properties.SingleOrDefault(x => string.Equals(x.Name, jsonProperty.Name, InvariantCultureIgnoreCase) && x.CanWrite);
                if (property == default)
                {
                    continue;
                }

                switch (jsonProperty.Value.ValueKind)
                {
                    case Number when property.PropertyType.IsNumeric():
                        if ((property.PropertyType == typeof(short) || property.PropertyType == typeof(short?)) && jsonProperty.Value.TryGetInt16(out var shortValue))
                        {
                            property.SetValue(value, shortValue);
                        }
                        else if ((property.PropertyType == typeof(int) || property.PropertyType == typeof(int?)) && jsonProperty.Value.TryGetInt32(out var intValue))
                        {
                            property.SetValue(value, intValue);
                        }
                        else if ((property.PropertyType == typeof(long) || property.PropertyType == typeof(long?)) && jsonProperty.Value.TryGetInt64(out var longValue))
                        {
                            property.SetValue(value, longValue);
                        }
                        else if ((property.PropertyType == typeof(double) || property.PropertyType == typeof(double?)) && jsonProperty.Value.TryGetDouble(out var doubleValue))
                        {
                            property.SetValue(value, doubleValue);
                        }
                        else if ((property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?)) && jsonProperty.Value.TryGetDecimal(out var decimalValue))
                        {
                            property.SetValue(value, decimalValue);
                        }
                        else if ((property.PropertyType == typeof(float) || property.PropertyType == typeof(float?)) && jsonProperty.Value.TryGetSingle(out var floatValue))
                        {
                            property.SetValue(value, floatValue);
                        }
                        else if ((property.PropertyType == typeof(ushort) || property.PropertyType == typeof(ushort?)) && jsonProperty.Value.TryGetUInt16(out var ushortValue))
                        {
                            property.SetValue(value, ushortValue);
                        }
                        else if ((property.PropertyType == typeof(uint) || property.PropertyType == typeof(uint?)) && jsonProperty.Value.TryGetInt32(out var uintValue))
                        {
                            property.SetValue(value, uintValue);
                        }
                        else if ((property.PropertyType == typeof(ulong) || property.PropertyType == typeof(ulong?)) && jsonProperty.Value.TryGetInt64(out var ulongValue))
                        {
                            property.SetValue(value, ulongValue);
                        }

                        break;
                    case False:
                    case True:
                        if ((property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)) &&
                            bool.TryParse(jsonProperty.Value.GetString(), out var boolValue))
                        {
                            property.SetValue(value, boolValue);
                        }

                        break;
                    case String:
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(value, jsonProperty.Value.GetString());
                        }
                        else if ((property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)) &&
                                 jsonProperty.Value.TryGetDateTime(out var dateTimeValue))
                        {
                            property.SetValue(value, dateTimeValue);
                        }
                        else if ((property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?)) &&
                                 jsonProperty.Value.TryGetDateTimeOffset(out var dateTimeOffsetValue))
                        {
                            property.SetValue(value, dateTimeOffsetValue);
                        }
                        else if ((property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?)) &&
                                 jsonProperty.Value.TryGetGuid(out var guidValue))
                        {
                            property.SetValue(value, guidValue);
                        }

                        break;
                }
            }

            return value;
        }
    }
}
