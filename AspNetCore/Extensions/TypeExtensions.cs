namespace System
{
    using Collections.Generic;
    using JetBrains.Annotations;
    using Linq;

    /// <summary>A class with methods that extend <see cref="Type"/>.</summary>
    [PublicAPI]
    public static class TypeExtensions
    {
        private static readonly IEnumerable<Type> NumericTypes = new[]
        {
            typeof(short),
            typeof(short?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?),
            typeof(ushort),
            typeof(ushort?),
            typeof(uint),
            typeof(uint?),
            typeof(ulong),
            typeof(ulong?)
        };

        /// <summary>Determines whether this instance is numeric.</summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <see langword="true"/> if the specified type is numeric; otherwise, <see langword="false"/>.</returns>
        public static bool IsNumeric(this Type type) => NumericTypes.Contains(type);
    }
}