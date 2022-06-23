namespace Funcky.Extensions;

public static partial class EnumerableExtensions
{
    /// <summary>
    /// Filters out all the empty values from an <c>IEnumerable&lt;Option&lt;T&gt;&gt;</c> and therefore returns an <see cref="IEnumerable{TItem}"/>.
    /// </summary>
    [Pure]
    public static IEnumerable<TSource> WhereSelect<TSource>(this IEnumerable<Option<TSource>> source)
        where TSource : notnull
        => source.WhereSelect(Identity);

    /// <summary>
    /// Projects and filters an <see cref="IEnumerable{T}"/> at the same time.
    /// This is done by filtering out any empty <see cref="Option{T}"/> values returned by the <paramref name="selector"/>.
    /// </summary>
    [Pure]
    public static IEnumerable<TResult> WhereSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Option<TResult>> selector)
        where TResult : notnull
        => source.SelectMany(input => selector(input).ToEnumerable());
}
