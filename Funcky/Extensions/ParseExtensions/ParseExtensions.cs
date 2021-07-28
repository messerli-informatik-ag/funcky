using System;
using System.Diagnostics.Contracts;
using Funcky.Monads;

namespace Funcky.Extensions
{
    public static partial class ParseExtensions
    {
        [Pure]
        public static Option<bool> ParseBooleanOrNone(this string candidate)
            => bool.TryParse(candidate, out var boolResult)
                ? boolResult
                : Option<bool>.None();

        [Pure]
        public static Option<TEnum> ParseEnumOrNone<TEnum>(this string candidate)
            where TEnum : struct
            => Enum.TryParse(candidate, out TEnum enumValue)
                ? enumValue
                : Option<TEnum>.None();

        [Pure]
        public static Option<TEnum> ParseEnumOrNone<TEnum>(this string candidate, bool ignoreCase)
            where TEnum : struct
            => Enum.TryParse(candidate, ignoreCase, out TEnum enumValue)
                ? enumValue
                : Option<TEnum>.None();
    }
}
