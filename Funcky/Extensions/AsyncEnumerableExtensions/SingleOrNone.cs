namespace Funcky.Extensions;

public static partial class AsyncEnumerableExtensions
{
    /// <summary>
    /// Returns the only element of a sequence as an <see cref="Option{T}" />, or a <see cref="Option{T}.None" /> value if the sequence is empty.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if there is more than one element in the sequence.</exception>
    [Pure]
    public static ValueTask<Option<TSource>> SingleOrNoneAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        where TSource : notnull
        => source.SingleOrNoneAwaitWithCancellationAsync(ToAsyncPredicateWithCancellationToken<TSource>(True), cancellationToken);

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition as an <see cref="Option{T}" /> or a <see cref="Option{T}.None" /> value if no such element exists.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if more than one element satisfies the condition.</exception>
    [Pure]
    public static ValueTask<Option<TSource>> SingleOrNoneAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        where TSource : notnull
        => source.SingleOrNoneAwaitWithCancellationAsync(ToAsyncPredicateWithCancellationToken(predicate), cancellationToken);

    /// <inheritdoc cref="SingleOrNoneAsync{TSource}(System.Collections.Generic.IAsyncEnumerable{TSource},System.Threading.CancellationToken)"/>
    [Pure]
    public static ValueTask<Option<TSource>> SingleOrNoneAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        where TSource : notnull
        => source.SingleOrNoneAwaitWithCancellationAsync(ToAsyncPredicateWithCancellationToken(predicate), cancellationToken);

    /// <inheritdoc cref="SingleOrNoneAsync{TSource}(System.Collections.Generic.IAsyncEnumerable{TSource},System.Threading.CancellationToken)"/>
    [Pure]
    public static async ValueTask<Option<TSource>> SingleOrNoneAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        where TSource : notnull
    {
        var enumerator = source.GetAsyncEnumerator(cancellationToken);
        await using var enumeratorGuard = enumerator.ConfigureAwait(false);

        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
        {
            var currentItem = enumerator.Current;
            if (await predicate(currentItem, cancellationToken).ConfigureAwait(false))
            {
                await ThrowIfEnumeratorContainsMoreMatchingElements(enumerator, predicate, cancellationToken).ConfigureAwait(false);
                return currentItem;
            }
        }

        return Option<TSource>.None();
    }

    private static async ValueTask ThrowIfEnumeratorContainsMoreMatchingElements<TSource>(IAsyncEnumerator<TSource> enumerator, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken)
    {
        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
        {
            if (await predicate(enumerator.Current, cancellationToken).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }
        }
    }
}
