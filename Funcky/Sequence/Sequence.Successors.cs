namespace Funcky;

public static partial class Sequence
{
    /// <summary>
    /// Generates a sequence based on a <paramref name="successor"/> function stopping at the first <see cref="Option{TItem}.None"/> value.
    /// This is essentially the inverse operation of an <see cref="Enumerable.Aggregate{T}"/>.
    /// </summary>
    /// <param name="first">The first element of the sequence.</param>
    /// <param name="successor">Generates the next element of the sequence or <see cref="Option{TItem}.None"/> based on the previous item.</param>
    /// <remarks>Use <see cref="Enumerable.Skip{TSource}(IEnumerable{TSource}, int)"/> on the result if you don't want the first item to be included.</remarks>
    [Pure]
    public static IEnumerable<TResult> Successors<TResult>(Option<TResult> first, Func<TResult, Option<TResult>> successor)
        where TResult : notnull
    {
        var item = first;
        while (item.TryGetValue(out var itemValue))
        {
            yield return itemValue;
            item = successor(itemValue);
        }
    }

    /// <inheritdoc cref="Successors{TResult}(Option{TResult}, Func{TResult, Option{TResult}})" />
    [Pure]
    public static IEnumerable<TResult> Successors<TResult>(TResult first, Func<TResult, Option<TResult>> successor)
        where TResult : notnull
        => Successors(Option.Some(first), successor);

    /// <inheritdoc cref="Successors{TResult}(Option{TResult}, Func{TResult, Option{TResult}})" />
    [Pure]
    public static IEnumerable<TResult> Successors<TResult>(Option<TResult> first, Func<TResult, TResult> successor)
        where TResult : notnull
        => Successors(first, previous => Option.Some(successor(previous)));

    /// <inheritdoc cref="Successors{TResult}(Option{TResult}, Func{TResult, Option{TResult}})" />
    [Pure]
    public static IEnumerable<TResult> Successors<TResult>(TResult first, Func<TResult, TResult> successor)
    {
        var item = first;
        while (true)
        {
            yield return item;
            item = successor(item);
        }
    }
}
