﻿//HintName: .g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.Extensions
{
    public static partial class ParseExtensions
    {
        [global::System.Diagnostics.Contracts.Pure]
        public static Funcky.Monads.Option<global::Funcky.Extensions.Target> ParseTargetOrNone(this string candidate) => global::Funcky.Extensions.Target.TryParse(candidate, out var result) ? result : default(Funcky.Monads.Option<global::Funcky.Extensions.Target>);
        [global::System.Diagnostics.Contracts.Pure]
        public static Funcky.Monads.Option<global::Funcky.Extensions.Target> ParseTargetOrNone(this string candidate, bool caseSensitive) => global::Funcky.Extensions.Target.TryParse(candidate, caseSensitive, out var result) ? result : default(Funcky.Monads.Option<global::Funcky.Extensions.Target>);
    }
}