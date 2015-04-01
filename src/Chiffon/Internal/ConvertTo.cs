﻿namespace Chiffon.Internal
{
    using System.Diagnostics.CodeAnalysis;

    internal static class ConvertTo
    {
        /// <remarks>
        /// WARNING: Does not work consistently for Flags enums:
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static TEnum? Enum<TEnum>(object value) where TEnum : struct
        {
            var type = typeof(TEnum);

            if (System.Enum.IsDefined(type, value))
            {
                return (TEnum)System.Enum.ToObject(type, value);
            }
            else
            {
                return null;
            }
        }
    }
}
