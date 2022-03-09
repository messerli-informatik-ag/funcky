namespace Funcky.Extensions;

public static partial class EnumerableExtensions
{
    /// <summary>
    /// Returns every nth (interval) element from the source sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence.</param>
    /// <param name="interval">the interval between elements in the source sequences.</param>
    /// <returns>Returns a sequence where only every nth element (interval) of source sequence is used. </returns>
    [Pure]
    public static IEnumerable<TSource> TakeEvery<TSource>(this IEnumerable<TSource> source, int interval)
    {
        ValidateInterval(interval);

        return source.Where((_, index) => index % interval == 0);
    }

    private static void ValidateInterval(int interval)
    {
        if (interval <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(interval), interval, "Interval must be larger than 0");
        }
    }
}
