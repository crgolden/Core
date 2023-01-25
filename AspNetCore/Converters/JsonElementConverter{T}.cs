namespace Core.Converters
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using JetBrains.Annotations;
    using static System.Reflection.BindingFlags;
    using static System.StringComparison;
    using static System.Text.Json.JsonTokenType;

    /// <inheritdoc />
    [PublicAPI]
    public class JsonElementConverter<T> : JsonConverter<T>
        where T : class, new()
    {
        /// <inheritdoc />
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var type = typeof(T);
            var properties = type.GetProperties(Instance | Public);
            var value = new T();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case EndObject:
                        return value;
                    case PropertyName:
                    {
                        var propertyName = reader.GetString();
                        var property = properties.SingleOrDefault(x => string.Equals(x.Name, propertyName, InvariantCultureIgnoreCase) && x.CanWrite);
                        if (property == default || !reader.Read())
                        {
                            throw new JsonException();
                        }

                        switch (reader.TokenType)
                        {
                            case Number when property.PropertyType.IsNumeric():
                                if ((property.PropertyType == typeof(short) || property.PropertyType == typeof(short?)) && reader.TryGetInt16(out var shortValue))
                                {
                                    property.SetValue(value, shortValue);
                                }
                                else if ((property.PropertyType == typeof(int) || property.PropertyType == typeof(int?)) && reader.TryGetInt32(out var intValue))
                                {
                                    property.SetValue(value, intValue);
                                }
                                else if ((property.PropertyType == typeof(long) || property.PropertyType == typeof(long?)) && reader.TryGetInt64(out var longValue))
                                {
                                    property.SetValue(value, longValue);
                                }
                                else if ((property.PropertyType == typeof(double) || property.PropertyType == typeof(double?)) && reader.TryGetDouble(out var doubleValue))
                                {
                                    property.SetValue(value, doubleValue);
                                }
                                else if ((property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?)) && reader.TryGetDecimal(out var decimalValue))
                                {
                                    property.SetValue(value, decimalValue);
                                }
                                else if ((property.PropertyType == typeof(float) || property.PropertyType == typeof(float?)) && reader.TryGetSingle(out var floatValue))
                                {
                                    property.SetValue(value, floatValue);
                                }
                                else if ((property.PropertyType == typeof(ushort) || property.PropertyType == typeof(ushort?)) && reader.TryGetUInt16(out var ushortValue))
                                {
                                    property.SetValue(value, ushortValue);
                                }
                                else if ((property.PropertyType == typeof(uint) || property.PropertyType == typeof(uint?)) && reader.TryGetInt32(out var uintValue))
                                {
                                    property.SetValue(value, uintValue);
                                }
                                else if ((property.PropertyType == typeof(ulong) || property.PropertyType == typeof(ulong?)) && reader.TryGetInt64(out var ulongValue))
                                {
                                    property.SetValue(value, ulongValue);
                                }

                                break;
                            case True:
                            case False:
                                if ((property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)) && bool.TryParse(reader.GetString(), out var boolValue))
                                {
                                    property.SetValue(value, boolValue);
                                }

                                break;
                            case JsonTokenType.String:
                                if (property.PropertyType == typeof(string))
                                {
                                    property.SetValue(value, reader.GetString());
                                }
                                else if ((property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)) && reader.TryGetDateTime(out var dateTimeValue))
                                {
                                    property.SetValue(value, dateTimeValue);
                                }
                                else if ((property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?)) && reader.TryGetDateTimeOffset(out var dateTimeOffsetValue))
                                {
                                    property.SetValue(value, dateTimeOffsetValue);
                                }
                                else if ((property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?)) && reader.TryGetGuid(out var guidValue))
                                {
                                    property.SetValue(value, guidValue);
                                }

                                break;
                            case None:
                                break;
                            case StartObject:
                                break;
                            case EndObject:
                                break;
                            case StartArray:
                                break;
                            case EndArray:
                                break;
                            case PropertyName:
                                break;
                            case Comment:
                                break;
                            case Null:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(reader));
                        }

                        break;
                    }

                    case None:
                        break;
                    case StartObject:
                        break;
                    case StartArray:
                        break;
                    case EndArray:
                        break;
                    case Comment:
                        break;
                    case JsonTokenType.String:
                        break;
                    case Number:
                        break;
                    case True:
                        break;
                    case False:
                        break;
                    case Null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(reader));
                }
            }

            throw new JsonException();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (writer == default)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (value == default)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var type = typeof(T);
            var properties = type.GetProperties(Instance | Public);
            writer.WriteStartObject();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsNumeric())
                {
                    if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                    {
                        var doubleValue = (double?)property.GetValue(value);
                        if (doubleValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, doubleValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        var decimalValue = (decimal?)property.GetValue(value);
                        if (decimalValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, decimalValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
                    {
                        var floatValue = (float?)property.GetValue(value);
                        if (floatValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, floatValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?))
                    {
                        var shortValue = (short?)property.GetValue(value);
                        if (shortValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, shortValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    {
                        var intValue = (int?)property.GetValue(value);
                        if (intValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, intValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                    {
                        var longValue = (long?)property.GetValue(value);
                        if (longValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, longValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(ushort) || property.PropertyType == typeof(ushort?))
                    {
                        var ushortValue = (ushort?)property.GetValue(value);
                        if (ushortValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, ushortValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(uint) || property.PropertyType == typeof(uint?))
                    {
                        var uintValue = (uint?)property.GetValue(value);
                        if (uintValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, uintValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(ulong) || property.PropertyType == typeof(ulong?))
                    {
                        var ulongValue = (ulong?)property.GetValue(value);
                        if (ulongValue.HasValue)
                        {
                            writer.WriteNumber(property.Name, ulongValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    var boolValue = (bool?)property.GetValue(value);
                    if (boolValue.HasValue)
                    {
                        writer.WriteBoolean(property.Name, boolValue.Value);
                    }
                    else
                    {
                        writer.WriteNull(property.Name);
                    }
                }
                else
                {
                    if (property.PropertyType == typeof(string))
                    {
                        writer.WriteString(property.Name, property.GetValue(value).ToString());
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        var dateTimeValue = (DateTime?)property.GetValue(value);
                        if (dateTimeValue.HasValue)
                        {
                            writer.WriteString(property.Name, dateTimeValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?))
                    {
                        var dateTimeOffsetValue = (DateTimeOffset?)property.GetValue(value);
                        if (dateTimeOffsetValue.HasValue)
                        {
                            writer.WriteString(property.Name, dateTimeOffsetValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
                    {
                        var guidValue = (Guid?)property.GetValue(value);
                        if (guidValue.HasValue)
                        {
                            writer.WriteString(property.Name, guidValue.Value);
                        }
                        else
                        {
                            writer.WriteNull(property.Name);
                        }
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}
